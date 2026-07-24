using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem.Mortality_Module
{
    public partial class ReviewList : System.Web.UI.Page
    {
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPendingReviews();
                LoadCompletedReviews();
            }
        }

        private void LoadPendingReviews()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(mortalityConnection))
                {
                    string query = @"SELECT Id, DeathCertificateNo, MRNo, AdmNo, PatientName, CreatedAt 
                                    FROM MortalityTracking 
                                    WHERE IsDeathCertificateCreated = 1 AND IsReviewFormCreated = 0 
                                    ORDER BY CreatedAt DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvPendingReviews.DataSource = dt;
                    gvPendingReviews.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading pending reviews: " + ex.Message, "danger");
            }
        }

        private void LoadCompletedReviews()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(mortalityConnection))
                {
                    string query = @"SELECT Id, DeathCertificateNo, ReviewFormNo, MRNo, AdmNo, PatientName, CreatedAt 
                                    FROM MortalityTracking 
                                    WHERE IsReviewFormCreated = 1 
                                    ORDER BY CreatedAt DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvCompletedReviews.DataSource = dt;
                    gvCompletedReviews.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading completed reviews: " + ex.Message, "danger");
            }
        }

        protected void gvPendingReviews_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CreateReview")
            {
                string certificateNo = e.CommandArgument.ToString();
                
                // Get CertificateID from DeathCertificateStore
                using (SqlConnection con = new SqlConnection(mortalityConnection))
                {
                    string query = "SELECT CertificateID FROM DeathCertificateStore WHERE CertificateNo = @CertificateNo";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CertificateNo", certificateNo);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();

                    if (result != null)
                    {
                        Response.Redirect("ReviewForm.aspx?CertificateID=" + result.ToString());
                    }
                    else
                    {
                        ShowMessage("Certificate not found.", "danger");
                    }
                }
            }
        }

        protected void gvCompletedReviews_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string reviewFormNo = e.CommandArgument.ToString();

            if (e.CommandName == "ViewReview")
            {
                Response.Redirect("PrintReviewForm.aspx?ReviewFormNo=" + reviewFormNo);
            }
            else if (e.CommandName == "EditReview")
            {
                Response.Redirect("EditReviewForm.aspx?ReviewFormNo=" + reviewFormNo);
            }
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