using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class AdminIndex : System.Web.UI.Page
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    LoadDashboardCounts();
            //}
        }

        //private void LoadDashboardCounts()
        //{
        //    try
        //    {
        //        // 1. Total Patients - Count all requests in Billing_Send_Patient_For_Welfare
        //        string totalPatientsQuery = "SELECT COUNT(*) FROM [dbo].[Billing_Send_Patient_For_Welfare]";
        //        int totalPatients = ExecuteScalarQuery(totalPatientsQuery);
        //        lblTotalPatients.Text = totalPatients.ToString();

        //        // 2. Total Donations Amount - Sum of all donation amounts
        //        string totalDonationsQuery = "SELECT ISNULL(SUM(Amount), 0) FROM [dbo].[Donations]";
        //        decimal totalDonations = ExecuteScalarQueryDecimal(totalDonationsQuery);
        //        lblTotalDonations.Text = totalDonations.ToString("N0"); // Formats with commas

        //        // 3. Pending Requests - Count where Status is 'Pending' (adjust status value as per your database)
        //        // Common status values: 'Pending', 'Submitted', 'New'
        //        string pendingRequestsQuery = "SELECT COUNT(*) FROM [dbo].[Billing_Send_Patient_For_Welfare] WHERE Status = 'Pending' OR Status IS NULL OR Status = ''";
        //        int pendingRequests = ExecuteScalarQuery(pendingRequestsQuery);
        //        lblPendingRequests.Text = pendingRequests.ToString();

        //        // 4. Approved Cases - Count from WelfareFinalApproval table (each entry is an approved case)
        //        string approvedCasesQuery = "SELECT COUNT(*) FROM [dbo].[WelfareFinalApproval]";
        //        int approvedCases = ExecuteScalarQuery(approvedCasesQuery);
        //        lblApprovedCases.Text = approvedCases.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle error - you might want to log this
        //        lblTotalPatients.Text = "Error";
        //        lblTotalDonations.Text = "Error";
        //        lblPendingRequests.Text = "Error";
        //        lblApprovedCases.Text = "Error";

        //        // Optional: Show error message
        //        // Response.Write("<script>alert('Error loading dashboard: " + ex.Message + "');</script>");
        //    }
        //}

        //private int ExecuteScalarQuery(string query)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            conn.Open();
        //            int result = Convert.ToInt32(cmd.ExecuteScalar());
        //            conn.Close();
        //            return result;
        //        }
        //    }
        //}

        //private decimal ExecuteScalarQueryDecimal(string query)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            conn.Open();
        //            decimal result = Convert.ToDecimal(cmd.ExecuteScalar());
        //            conn.Close();
        //            return result;
        //        }
        //    }
        //}
    }
}