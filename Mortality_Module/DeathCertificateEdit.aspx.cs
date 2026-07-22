using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem.Mortality_Module
{
    public partial class DeathCertificateEdit : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        int recordId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    recordId = Convert.ToInt32(Request.QueryString["id"]);
                    LoadRecord(recordId);
                }
                else
                {
                    Response.Redirect("DeathCertificateList.aspx");
                }
            }
        }

        private void LoadRecord(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT * FROM DeathCertificates WHERE Id = @Id AND IsDeleted = 0";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtMRNo.Text = dr["MRNo"].ToString();
                        txtPatientName.Text = dr["PatientName"].ToString();
                        txtAge.Text = dr["Age"].ToString();
                        ddlGender.SelectedValue = dr["Gender"].ToString();
                        txtCNIC.Text = dr["CNICNo"] == DBNull.Value ? "" : dr["CNICNo"].ToString();
                        txtContactNo.Text = dr["ContactNo"] == DBNull.Value ? "" : dr["ContactNo"].ToString();
                        txtAddress.Text = dr["Address"] == DBNull.Value ? "" : dr["Address"].ToString();
                        txtDiagnosis.Text = dr["Diagnosis"].ToString();
                        txtDateOfDeath.Text = Convert.ToDateTime(dr["DateOfDeath"]).ToString("yyyy-MM-dd");
                        txtTimeOfDeath.Text = dr["TimeOfDeath"].ToString();
                        txtCauseOfDeath.Text = dr["CauseOfDeath"].ToString();
                        txtConfirmedBy.Text = dr["DeathConfirmedBy"].ToString();
                        txtReceiverName.Text = dr["ReceiverName"].ToString();
                        txtRelation.Text = dr["ReceiverRelation"].ToString();
                        txtReceiverCNIC.Text = dr["ReceiverCNIC"] == DBNull.Value ? "" : dr["ReceiverCNIC"].ToString();
                        txtReceiverCell.Text = dr["ReceiverCellNo"].ToString();
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading record: " + ex.Message, "danger");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(txtMRNo.Text) || string.IsNullOrEmpty(txtPatientName.Text) ||
                    string.IsNullOrEmpty(txtAge.Text) || string.IsNullOrEmpty(ddlGender.SelectedValue) ||
                    string.IsNullOrEmpty(txtDiagnosis.Text) || string.IsNullOrEmpty(txtDateOfDeath.Text) ||
                    string.IsNullOrEmpty(txtTimeOfDeath.Text) || string.IsNullOrEmpty(txtCauseOfDeath.Text) ||
                    string.IsNullOrEmpty(txtConfirmedBy.Text) || string.IsNullOrEmpty(txtReceiverName.Text) ||
                    string.IsNullOrEmpty(txtRelation.Text) || string.IsNullOrEmpty(txtReceiverCell.Text))
                {
                    ShowMessage("Please fill all required fields (*)", "danger");
                    return;
                }

                recordId = Convert.ToInt32(Request.QueryString["id"]);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"UPDATE DeathCertificates SET 
                                    MRNo = @MRNo,
                                    PatientName = @PatientName,
                                    Age = @Age,
                                    Gender = @Gender,
                                    CNICNo = @CNICNo,
                                    ContactNo = @ContactNo,
                                    Address = @Address,
                                    Diagnosis = @Diagnosis,
                                    DateOfDeath = @DateOfDeath,
                                    TimeOfDeath = @TimeOfDeath,
                                    CauseOfDeath = @CauseOfDeath,
                                    DeathConfirmedBy = @DeathConfirmedBy,
                                    ReceiverName = @ReceiverName,
                                    ReceiverRelation = @ReceiverRelation,
                                    ReceiverCNIC = @ReceiverCNIC,
                                    ReceiverCellNo = @ReceiverCellNo
                                    WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", recordId);
                    cmd.Parameters.AddWithValue("@MRNo", txtMRNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(txtAge.Text));
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@CNICNo", string.IsNullOrEmpty(txtCNIC.Text) ? (object)DBNull.Value : txtCNIC.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactNo", string.IsNullOrEmpty(txtContactNo.Text) ? (object)DBNull.Value : txtContactNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Diagnosis", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfDeath", DateTime.Parse(txtDateOfDeath.Text));
                    cmd.Parameters.AddWithValue("@TimeOfDeath", TimeSpan.Parse(txtTimeOfDeath.Text));
                    cmd.Parameters.AddWithValue("@CauseOfDeath", txtCauseOfDeath.Text.Trim());
                    cmd.Parameters.AddWithValue("@DeathConfirmedBy", txtConfirmedBy.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReceiverName", txtReceiverName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReceiverRelation", txtRelation.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReceiverCNIC", string.IsNullOrEmpty(txtReceiverCNIC.Text) ? (object)DBNull.Value : txtReceiverCNIC.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReceiverCellNo", txtReceiverCell.Text.Trim());

                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        ShowMessage("Certificate updated successfully!", "success");
                        Response.Redirect("DeathCertificateList.aspx?updated=1");
                    }
                    else
                    {
                        ShowMessage("Error updating certificate.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("DeathCertificateList.aspx");
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            pnlMessage.CssClass = type == "success" ? "alert-success" : "alert-danger";
            pnlMessage.Visible = true;
        }
    }
}