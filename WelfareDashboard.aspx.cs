using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class WelfareDashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadDashboardData();
                LoadChartsData();
                LoadRecentRequests();
                lblLastUpdated.Text = "Last updated: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
            LoadChartsData();
            LoadRecentRequests();
            lblLastUpdated.Text = "Last updated: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
        }

        // =============================================
        // LOAD DASHBOARD STATISTICS
        // =============================================
        private void LoadDashboardData()
        {
            try
            {
                // 1. Total Patients
                string totalPatientsQuery = "SELECT COUNT(*) FROM [dbo].[Billing_Send_Patient_For_Welfare]";
                int totalPatients = ExecuteScalarQuery(totalPatientsQuery);
                lblTotalPatients.Text = totalPatients.ToString("N0");

                // 2. Total Donations
                string totalDonationsQuery = "SELECT ISNULL(SUM(Amount), 0) FROM [dbo].[Donations]";
                decimal totalDonations = ExecuteScalarQueryDecimal(totalDonationsQuery);
                lblTotalDonations.Text = totalDonations.ToString("N0");

                // 3. Pending Requests
                string pendingRequestsQuery = @"
                    SELECT COUNT(*) 
                    FROM [dbo].[Billing_Send_Patient_For_Welfare] 
                    WHERE ISNULL(Status, 'Pending') IN ('Pending', 'Submitted', 'New', '')
                ";
                int pendingRequests = ExecuteScalarQuery(pendingRequestsQuery);
                lblPendingRequests.Text = pendingRequests.ToString("N0");

                // 4. Approved Cases
                string approvedCasesQuery = "SELECT COUNT(*) FROM [dbo].[WelfareFinalApproval]";
                int approvedCases = ExecuteScalarQuery(approvedCasesQuery);
                lblApprovedCases.Text = approvedCases.ToString("N0");

                // 5. Total Requests count for badge
                string totalRequestsQuery = "SELECT COUNT(*) FROM [dbo].[Billing_Send_Patient_For_Welfare]";
                int totalRequests = ExecuteScalarQuery(totalRequestsQuery);
                lblTotalRequests.Text = totalRequests.ToString("N0");
            }
            catch (Exception ex)
            {
                // Log error if needed
                lblTotalPatients.Text = "0";
                lblTotalDonations.Text = "0";
                lblPendingRequests.Text = "0";
                lblApprovedCases.Text = "0";
            }
        }

        // =============================================
        // LOAD CHARTS DATA
        // =============================================
        private void LoadChartsData()
        {
            try
            {
                // ---------- Monthly Cases Chart ----------
                var monthlyData = GetMonthlyCaseData();
                hfMonthlyLabels.Value = JsonConvert.SerializeObject(monthlyData.Keys.ToArray());
                hfMonthlyData.Value = JsonConvert.SerializeObject(monthlyData.Values.ToArray());

                // ---------- Status Pie Chart ----------
                var statusData = GetStatusDistribution();
                hfStatusLabels.Value = JsonConvert.SerializeObject(statusData.Keys.ToArray());
                hfStatusData.Value = JsonConvert.SerializeObject(statusData.Values.ToArray());

                // Status colors
                string[] statusColors = {
                    "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF", "#FF9F40"
                };
                hfStatusColors.Value = JsonConvert.SerializeObject(statusColors);

                // ---------- Donation Trend Chart ----------
                var donationData = GetDonationTrend();
                hfDonationLabels.Value = JsonConvert.SerializeObject(donationData.Keys.ToArray());
                hfDonationData.Value = JsonConvert.SerializeObject(donationData.Values.ToArray());

                // ---------- Top Donors Chart ----------
                var donorData = GetTopDonors();
                hfDonorLabels.Value = JsonConvert.SerializeObject(donorData.Keys.ToArray());
                hfDonorData.Value = JsonConvert.SerializeObject(donorData.Values.ToArray());
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        // =============================================
        // GET MONTHLY CASE DATA - FIXED: Using CreatedDate
        // =============================================
        private Dictionary<string, int> GetMonthlyCaseData()
        {
            var result = new Dictionary<string, int>();

            // Get last 6 months
            for (int i = 5; i >= 0; i--)
            {
                string monthName = DateTime.Now.AddMonths(-i).ToString("MMM");
                result[monthName] = 0;
            }

            string query = @"
                SELECT 
                    FORMAT(CreatedDate, 'MMM') as MonthName,
                    COUNT(*) as CaseCount
                FROM [dbo].[Billing_Send_Patient_For_Welfare]
                WHERE CreatedDate >= DATEADD(MONTH, -6, GETDATE())
                GROUP BY FORMAT(CreatedDate, 'MMM')
                ORDER BY MIN(CreatedDate)
            ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string month = reader["MonthName"].ToString();
                            int count = Convert.ToInt32(reader["CaseCount"]);
                            if (result.ContainsKey(month))
                            {
                                result[month] = count;
                            }
                        }
                    }
                }
            }

            return result;
        }

        // =============================================
        // GET STATUS DISTRIBUTION
        // =============================================
        private Dictionary<string, int> GetStatusDistribution()
        {
            var result = new Dictionary<string, int>();

            string query = @"
                SELECT 
                    CASE 
                        WHEN Status IS NULL OR Status = '' THEN 'Pending'
                        ELSE Status 
                    END as Status,
                    COUNT(*) as Count
                FROM [dbo].[Billing_Send_Patient_For_Welfare]
                GROUP BY CASE 
                    WHEN Status IS NULL OR Status = '' THEN 'Pending'
                    ELSE Status 
                END
            ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string status = reader["Status"].ToString();
                            int count = Convert.ToInt32(reader["Count"]);
                            result[status] = count;
                        }
                    }
                }
            }

            return result;
        }

        // =============================================
        // GET DONATION TREND (Last 6 Months)
        // =============================================
        private Dictionary<string, decimal> GetDonationTrend()
        {
            var result = new Dictionary<string, decimal>();

            for (int i = 5; i >= 0; i--)
            {
                string monthName = DateTime.Now.AddMonths(-i).ToString("MMM");
                result[monthName] = 0;
            }

            string query = @"
                SELECT 
                    FORMAT(DonationDate, 'MMM') as MonthName,
                    ISNULL(SUM(Amount), 0) as TotalAmount
                FROM [dbo].[Donations]
                WHERE DonationDate >= DATEADD(MONTH, -6, GETDATE())
                GROUP BY FORMAT(DonationDate, 'MMM')
                ORDER BY MIN(DonationDate)
            ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string month = reader["MonthName"].ToString();
                            decimal amount = Convert.ToDecimal(reader["TotalAmount"]);
                            if (result.ContainsKey(month))
                            {
                                result[month] = amount;
                            }
                        }
                    }
                }
            }

            return result;
        }

        // =============================================
        // GET TOP DONORS
        // =============================================
        private Dictionary<string, decimal> GetTopDonors()
        {
            var result = new Dictionary<string, decimal>();

            string query = @"
                SELECT TOP 5
                    DonorName,
                    ISNULL(SUM(Amount), 0) as TotalDonation
                FROM [dbo].[Donations]
                GROUP BY DonorName
                ORDER BY TotalDonation DESC
            ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["DonorName"].ToString();
                            decimal amount = Convert.ToDecimal(reader["TotalDonation"]);
                            result[name] = amount;
                        }
                    }
                }
            }

            return result;
        }

        // =============================================
        // LOAD RECENT REQUESTS - FIXED: Using actual column names
        // =============================================
        private void LoadRecentRequests()
        {
            string query = @"
                SELECT TOP 10
                    RequestID as RequestId,
                    PatientName,
                    CNIC,
                    CreatedDate as RequestDate,
                    WardName as Department,
                    BillAmount as Amount,
                    ISNULL(Status, 'Pending') as Status
                FROM [dbo].[Billing_Send_Patient_For_Welfare]
                ORDER BY CreatedDate DESC
            ";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }

            gvRecentRequests.DataSource = dt;
            gvRecentRequests.DataBind();
        }

        // =============================================
        // GRIDVIEW ROW DATA BOUND - Set Status Badge
        // =============================================
        protected void gvRecentRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Label lblStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblStatus");
                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    lblStatus.CssClass = "status-badge " + GetStatusClass(status);
                }
            }
        }

        // =============================================
        // HELPER: Get Status CSS Class
        // =============================================
        protected string GetStatusClass(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                case "submitted":
                case "new":
                    return "pending";
                case "approved":
                case "accepted":
                    return "approved";
                case "rejected":
                case "declined":
                    return "rejected";
                case "in progress":
                case "processing":
                    return "in-progress";
                default:
                    return "pending";
            }
        }

        // =============================================
        // HELPER: Execute Scalar (Integer)
        // =============================================
        private int ExecuteScalarQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();
                    return result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }
            }
        }

        // =============================================
        // HELPER: Execute Scalar (Decimal)
        // =============================================
        private decimal ExecuteScalarQueryDecimal(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();
                    return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }
            }
        }
    }
}