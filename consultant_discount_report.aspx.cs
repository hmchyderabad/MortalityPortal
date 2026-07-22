using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class consultant_discount_report : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default dates (current month)
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                LoadConsultants();
                LoadReport();
            }
        }

        private void LoadConsultants()
        {
            try
            {
                // Get distinct consultants from DrTransactions table
                string query = @"SELECT DISTINCT ConsultantCode, ConsultantName 
                                FROM [dbo].[DrTransactions] 
                                WHERE ConsultantName IS NOT NULL AND ConsultantName != ''
                                ORDER BY ConsultantName";

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlConsultant.DataSource = reader;
                        ddlConsultant.DataTextField = "ConsultantName";
                        ddlConsultant.DataValueField = "ConsultantCode";
                        ddlConsultant.DataBind();
                        conn.Close();
                    }
                }

                ddlConsultant.Items.Insert(0, new ListItem("-- All Consultants --", ""));
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading consultants: " + ex.Message);
            }
        }

        private void LoadReport()
        {
            try
            {
                DataTable dt = GetReportData();
                gvDiscountReport.DataSource = dt;
                gvDiscountReport.DataBind();

                // Calculate summary
                if (dt.Rows.Count > 0)
                {
                    decimal totalDiscount = 0;
                    decimal totalFinalAmount = 0;
                    decimal totalDiscountPercent = 0;
                    int rowsWithDiscount = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        decimal discountAmount = Convert.ToDecimal(row["DiscountAmount"]);
                        decimal finalAmount = Convert.ToDecimal(row["FinalAmount"]);
                        decimal discountPercent = Convert.ToDecimal(row["DiscountPercent"]);

                        totalDiscount += discountAmount;
                        totalFinalAmount += finalAmount;

                        if (discountPercent > 0)
                        {
                            totalDiscountPercent += discountPercent;
                            rowsWithDiscount++;
                        }
                    }

                    lblTotalDiscount.Text = totalDiscount.ToString("N2");
                    lblTotalFinalAmount.Text = totalFinalAmount.ToString("N2");
                    lblTotalPatients.Text = dt.Rows.Count.ToString();
                    lblAvgDiscount.Text = rowsWithDiscount > 0 ? (totalDiscountPercent / rowsWithDiscount).ToString("N2") + "%" : "0%";
                }
                else
                {
                    ClearSummary();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading report: " + ex.Message);
                ClearSummary();
            }
        }

        private DataTable GetReportData()
        {
            DataTable dt = new DataTable();

            string fromDate = txtFromDate.Text.Trim();
            string toDate = txtToDate.Text.Trim();
            string consultantCode = ddlConsultant.SelectedValue;

            // Direct query using DrTransactions table
            string query = @"
                SELECT 
                    TransactionID,
                    AdmNo,
                    MRNo,
                    ISNULL(ConsultantCode, '') AS ConsultantCode,
                    ISNULL(ConsultantName, '') AS ConsultantName,
                    ISNULL(VisitFee, 0) AS VisitFee,
                    ISNULL(ProcedureFee, 0) AS ProcedureFee,
                    ISNULL(TotalFee, 0) AS TotalFee,
                    ISNULL(DiscountPercent, 0) AS DiscountPercent,
                    ISNULL(FinalAmount, 0) AS FinalAmount,
                    CreatedDate,
                    ISNULL(TotalFee, 0) - ISNULL(FinalAmount, 0) AS DiscountAmount
                FROM [dbo].[DrTransactions]
                WHERE 1=1";

            // Add date filter
            if (!string.IsNullOrEmpty(fromDate))
            {
                query += " AND CAST(CreatedDate AS DATE) >= @FromDate";
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                query += " AND CAST(CreatedDate AS DATE) <= @ToDate";
            }

            // Add consultant filter
            if (!string.IsNullOrEmpty(consultantCode))
            {
                query += " AND ConsultantCode = @ConsultantCode";
            }

            query += " ORDER BY CreatedDate DESC, ConsultantName";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(fromDate))
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    if (!string.IsNullOrEmpty(toDate))
                        cmd.Parameters.AddWithValue("@ToDate", toDate);
                    if (!string.IsNullOrEmpty(consultantCode))
                        cmd.Parameters.AddWithValue("@ConsultantCode", consultantCode);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                }
            }

            return dt;
        }

        private void ClearSummary()
        {
            lblTotalDiscount.Text = "0";
            lblTotalFinalAmount.Text = "0";
            lblTotalPatients.Text = "0";
            lblAvgDiscount.Text = "0%";
        }

        private void ShowAlert(string message)
        {
            string cleanMessage = message.Replace("'", "\\'").Replace(Environment.NewLine, " ");
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                $"<script>alert('{cleanMessage}');</script>");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = GetReportData();

                if (dt.Rows.Count > 0)
                {
                    // Prepare response for Excel export
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                        $"attachment;filename=ConsultantDiscountReport_{DateTime.Now:yyyyMMdd_HHmmss}.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Charset = "";

                    // Disable paging for export
                    gvDiscountReport.AllowPaging = false;
                    gvDiscountReport.DataSource = dt;
                    gvDiscountReport.DataBind();

                    // Render GridView to HTML
                    using (StringWriter sw = new StringWriter())
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        gvDiscountReport.RenderControl(hw);
                        Response.Write(sw.ToString());
                    }

                    Response.Flush();
                    Response.End();
                }
                else
                {
                    ShowAlert("No data to export!");
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error exporting to Excel: " + ex.Message);
            }
        }

        protected void gvDiscountReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDiscountReport.PageIndex = e.NewPageIndex;
            LoadReport();
        }

        // Required for GridView export
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Verifies that the control is rendered
        }
    }
}