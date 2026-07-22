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
    public partial class Services_discount_report : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default dates (current month)
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                LoadServices();
                LoadReport();
            }
        }

        private void LoadServices()
        {
            try
            {
                // Get services from Services table
                string query = @"SELECT ServiceId, ServiceName 
                                FROM [dbo].[Services] 
                                WHERE ServiceName IS NOT NULL AND ServiceName != ''
                                ORDER BY ServiceName";

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlService.DataSource = reader;
                        ddlService.DataTextField = "ServiceName";
                        ddlService.DataValueField = "ServiceId";
                        ddlService.DataBind();
                        conn.Close();
                    }
                }

                ddlService.Items.Insert(0, new ListItem("-- All Services --", ""));
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading services: " + ex.Message);

                // Fallback: Get distinct services from WelfareFinalApprovalServices table
                string fallbackQuery = @"SELECT DISTINCT ServiceName 
                                        FROM [dbo].[WelfareFinalApprovalServices] 
                                        WHERE ServiceName IS NOT NULL AND ServiceName != ''
                                        ORDER BY ServiceName";

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(fallbackQuery, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlService.DataSource = reader;
                        ddlService.DataTextField = "ServiceName";
                        ddlService.DataValueField = "ServiceName";
                        ddlService.DataBind();
                        conn.Close();
                    }
                }
                ddlService.Items.Insert(0, new ListItem("-- All Services --", ""));
            }
        }

        private void LoadReport()
        {
            try
            {
                DataTable dt = GetReportData();
                gvServicesReport.DataSource = dt;
                gvServicesReport.DataBind();

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
                    lblTotalServices.Text = dt.Rows.Count.ToString();
                    lblAvgDiscount.Text = rowsWithDiscount > 0 ? (totalDiscountPercent / rowsWithDiscount).ToString("N2") + "%" : "0%";

                    // Load service-wise summary when "All Services" is selected
                    if (string.IsNullOrEmpty(ddlService.SelectedValue))
                    {
                        LoadServiceSummary();
                        divServiceSummary.Visible = true;
                    }
                    else
                    {
                        divServiceSummary.Visible = false;
                    }
                }
                else
                {
                    ClearSummary();
                    divServiceSummary.Visible = false;
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
            string serviceName = ddlService.SelectedValue;

            // Query with date filter using WelfareFinalApproval table for dates
            string query = @"
                SELECT 
                    wfas.Id,
                    wfas.FinalApprovalId,
                    wfas.ServiceName,
                    ISNULL(wfas.Amount, 0) AS Amount,
                    ISNULL(wfas.DiscountPercent, 0) AS DiscountPercent,
                    ISNULL(wfas.DiscountAmount, 0) AS DiscountAmount,
                    ISNULL(wfas.FinalAmount, 0) AS FinalAmount,
                    wfa.ApprovalDate
                FROM [dbo].[WelfareFinalApprovalServices] wfas
                INNER JOIN [dbo].[WelfareFinalApproval] wfa ON wfas.FinalApprovalId = wfa.FinalApprovalId
                WHERE 1=1";

            // Add date filter
            if (!string.IsNullOrEmpty(fromDate))
            {
                query += " AND CAST(wfa.ApprovalDate AS DATE) >= @FromDate";
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                query += " AND CAST(wfa.ApprovalDate AS DATE) <= @ToDate";
            }

            // Add service filter
            if (!string.IsNullOrEmpty(serviceName))
            {
                query += " AND wfas.ServiceName = @ServiceName";
            }

            query += " ORDER BY wfa.ApprovalDate DESC, wfas.ServiceName";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(fromDate))
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    if (!string.IsNullOrEmpty(toDate))
                        cmd.Parameters.AddWithValue("@ToDate", toDate);
                    if (!string.IsNullOrEmpty(serviceName))
                        cmd.Parameters.AddWithValue("@ServiceName", serviceName);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                }
            }

            return dt;
        }

        private void LoadServiceSummary()
        {
            try
            {
                string fromDate = txtFromDate.Text.Trim();
                string toDate = txtToDate.Text.Trim();

                string query = @"
                    SELECT 
                        wfas.ServiceName,
                        ISNULL(SUM(wfas.Amount), 0) AS TotalAmount,
                        ISNULL(SUM(wfas.DiscountAmount), 0) AS TotalDiscount,
                        ISNULL(SUM(wfas.FinalAmount), 0) AS TotalFinalAmount,
                        COUNT(*) AS ServiceCount,
                        ISNULL(AVG(wfas.DiscountPercent), 0) AS AvgDiscountPercent
                    FROM [dbo].[WelfareFinalApprovalServices] wfas
                    INNER JOIN [dbo].[WelfareFinalApproval] wfa ON wfas.FinalApprovalId = wfa.FinalApprovalId
                    WHERE 1=1";

                if (!string.IsNullOrEmpty(fromDate))
                {
                    query += " AND CAST(wfa.ApprovalDate AS DATE) >= @FromDate";
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    query += " AND CAST(wfa.ApprovalDate AS DATE) <= @ToDate";
                }

                query += " GROUP BY wfas.ServiceName ORDER BY wfas.ServiceName";

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(fromDate))
                            cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        if (!string.IsNullOrEmpty(toDate))
                            cmd.Parameters.AddWithValue("@ToDate", toDate);

                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        conn.Close();
                    }
                }

                gvServiceSummary.DataSource = dt;
                gvServiceSummary.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading service summary: " + ex.Message);
            }
        }

        private void ClearSummary()
        {
            lblTotalDiscount.Text = "0";
            lblTotalFinalAmount.Text = "0";
            lblTotalServices.Text = "0";
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
                        $"attachment;filename=ServicesDiscountReport_{DateTime.Now:yyyyMMdd_HHmmss}.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Charset = "";

                    // Disable paging for export
                    gvServicesReport.AllowPaging = false;
                    gvServicesReport.DataSource = dt;
                    gvServicesReport.DataBind();

                    // Render GridView to HTML
                    using (StringWriter sw = new StringWriter())
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        gvServicesReport.RenderControl(hw);
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

        protected void gvServicesReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvServicesReport.PageIndex = e.NewPageIndex;
            LoadReport();
        }

        protected void gvServicesReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Add color coding based on discount percentage
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                decimal discountPercent = Convert.ToDecimal(drv["DiscountPercent"]);

                if (discountPercent >= 50)
                {
                    e.Row.BackColor = System.Drawing.Color.LightCoral;
                }
                else if (discountPercent >= 25)
                {
                    e.Row.BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }

        // Required for GridView export
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Verifies that the control is rendered
        }
    }
}