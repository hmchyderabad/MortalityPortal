using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem.Mortality_Module
{
    public partial class DeathCertificateList : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT Id, MRNo, PatientName, Age, Gender, CNICNo, ContactNo, Address, " +
                                   "Diagnosis, DateOfDeath, TimeOfDeath, CauseOfDeath, DeathConfirmedBy, " +
                                   "ReceiverName, ReceiverRelation, ReceiverCNIC, ReceiverCellNo, CreatedDate " +
                                   "FROM DeathCertificates WHERE IsDeleted = 0 ORDER BY Id DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvDeathCertificates.DataSource = dt;
                    gvDeathCertificates.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading data: " + ex.Message, "danger");
            }
        }

        private void SearchData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT Id, MRNo, PatientName, Age, Gender, CNICNo, ContactNo, Address, ");
                    query.Append("Diagnosis, DateOfDeath, TimeOfDeath, CauseOfDeath, DeathConfirmedBy, ");
                    query.Append("ReceiverName, ReceiverRelation, ReceiverCNIC, ReceiverCellNo, CreatedDate ");
                    query.Append("FROM DeathCertificates WHERE IsDeleted = 0");

                    if (!string.IsNullOrEmpty(txtSearchName.Text))
                    {
                        query.Append(" AND PatientName LIKE @PatientName");
                    }
                    if (!string.IsNullOrEmpty(txtSearchMRNo.Text))
                    {
                        query.Append(" AND MRNo LIKE @MRNo");
                    }
                    if (!string.IsNullOrEmpty(txtFromDate.Text))
                    {
                        query.Append(" AND DateOfDeath >= @FromDate");
                    }
                    if (!string.IsNullOrEmpty(txtToDate.Text))
                    {
                        query.Append(" AND DateOfDeath <= @ToDate");
                    }

                    query.Append(" ORDER BY Id DESC");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);

                    if (!string.IsNullOrEmpty(txtSearchName.Text))
                    {
                        cmd.Parameters.AddWithValue("@PatientName", "%" + txtSearchName.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrEmpty(txtSearchMRNo.Text))
                    {
                        cmd.Parameters.AddWithValue("@MRNo", "%" + txtSearchMRNo.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrEmpty(txtFromDate.Text))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(txtFromDate.Text));
                    }
                    if (!string.IsNullOrEmpty(txtToDate.Text))
                    {
                        cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(txtToDate.Text));
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvDeathCertificates.DataSource = dt;
                    gvDeathCertificates.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                        ShowMessage("No records found matching your search criteria.", "danger");
                    }
                    else
                    {
                        ShowMessage($"Found {dt.Rows.Count} record(s).", "success");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error searching: " + ex.Message, "danger");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = "";
            txtSearchMRNo.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            LoadData();
            ShowMessage("Filters reset. Showing all records.", "success");
        }

        protected void gvDeathCertificates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRecord")
            {
                Response.Redirect($"DeathCertificateEdit.aspx?id={id}");
            }
            else if (e.CommandName == "DeleteRecord")
            {
                DeleteRecord(id);
            }
            else if (e.CommandName == "ViewRecord")
            {
                ViewRecord(id);
            }
        }

        private void DeleteRecord(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    // Soft delete (update IsDeleted flag)
                    string query = "UPDATE DeathCertificates SET IsDeleted = 1 WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        ShowMessage("Record deleted successfully!", "success");
                        LoadData();
                    }
                    else
                    {
                        ShowMessage("Error deleting record.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        private void ViewRecord(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT * FROM DeathCertificates WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        StringBuilder html = new StringBuilder();
                        html.Append("<div style='font-family: Arial;'>");

                        html.Append("<div class='detail-row'><span class='detail-label'>M.R. No:</span> " + dr["MRNo"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Patient Name:</span> " + dr["PatientName"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Age:</span> " + dr["Age"] + " Years</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Gender:</span> " + dr["Gender"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>CNIC No:</span> " + (dr["CNICNo"] == DBNull.Value ? "N/A" : dr["CNICNo"]) + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Contact No:</span> " + (dr["ContactNo"] == DBNull.Value ? "N/A" : dr["ContactNo"]) + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Address:</span> " + (dr["Address"] == DBNull.Value ? "N/A" : dr["Address"]) + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Diagnosis:</span> " + dr["Diagnosis"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Date of Death:</span> " + Convert.ToDateTime(dr["DateOfDeath"]).ToString("dd/MM/yyyy") + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Time of Death:</span> " + dr["TimeOfDeath"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Cause of Death:</span> " + dr["CauseOfDeath"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Death Confirmed By:</span> " + dr["DeathConfirmedBy"] + "</div>");
                        html.Append("<hr style='margin: 20px 0;' />");
                        html.Append("<h3>Dead Body Received By:</h3>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Name:</span> " + dr["ReceiverName"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Relation:</span> " + dr["ReceiverRelation"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>CNIC:</span> " + (dr["ReceiverCNIC"] == DBNull.Value ? "N/A" : dr["ReceiverCNIC"]) + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Cell No:</span> " + dr["ReceiverCellNo"] + "</div>");
                        html.Append("<div class='detail-row'><span class='detail-label'>Created Date:</span> " + Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</div>");

                        html.Append("</div>");

                        litDetails.Text = html.ToString();

                        // Register script to open modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openModal();", true);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error viewing details: " + ex.Message, "danger");
            }
        }

        protected void gvDeathCertificates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDeathCertificates.PageIndex = e.NewPageIndex;
            LoadData();
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;

            if (type == "success")
            {
                pnlMessage.CssClass = "alert alert-success";
            }
            else if (type == "danger")
            {
                pnlMessage.CssClass = "alert alert-danger";
            }

            pnlMessage.Visible = true;

            string hideScript = @"setTimeout(function() {
                var pnl = document.getElementById('" + pnlMessage.ClientID + @"');
                if(pnl) {
                    pnl.style.display = 'none';
                }
            }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "hideMessage", hideScript, true);
        }
    }
}