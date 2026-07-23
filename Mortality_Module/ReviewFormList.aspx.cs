using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem.Mortality_Module
{
    public partial class ReviewFormList : System.Web.UI.Page
    {
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;

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
                using (SqlConnection con = new SqlConnection(mortalityConnection))
                {
                    string query = @"SELECT r.ReviewID, r.CertificateID, r.CreatedDate, r.CreatedBy,
                                     dc.CertificateNo, dc.MRNo, dc.PatientName, dc.DateOfDeath
                                     FROM DeathCertificateReview r
                                     INNER JOIN DeathCertificateStore dc ON r.CertificateID = dc.CertificateID
                                     ORDER BY r.CreatedDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvReviewForms.DataSource = dt;
                    gvReviewForms.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading data: " + ex.Message, "danger");
            }
        }

        protected void gvReviewForms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int reviewID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "PrintRecord")
            {
                Response.Redirect($"View.aspx?CertificateID={GetCertificateID(reviewID)}&print=1");
            }
            else if (e.CommandName == "EditRecord")
            {
                Response.Redirect($"EditReviewForm.aspx?ReviewID={reviewID}");
            }
            else if (e.CommandName == "DeleteRecord")
            {
                DeleteRecord(reviewID);
            }
        }

        private string GetCertificateID(int reviewId)
        {
            string certificateID = "";
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = "SELECT CertificateID FROM DeathCertificateReview WHERE ReviewID = @ReviewID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ReviewID", reviewId);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    certificateID = result.ToString();
                }
            }
            return certificateID;
        }

        private void DeleteRecord(int reviewID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(mortalityConnection))
                {
                    string query = "DELETE FROM DeathCertificateReview WHERE ReviewID = @ReviewID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ReviewID", reviewID);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        ShowMessage("Review form deleted successfully!", "success");
                        LoadData();
                    }
                    else
                    {
                        ShowMessage("Error deleting review form.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void gvReviewForms_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReviewForms.PageIndex = e.NewPageIndex;
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