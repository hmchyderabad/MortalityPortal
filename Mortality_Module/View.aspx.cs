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
    public partial class View : System.Web.UI.Page
    {
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;

        public string CertificateNo { get; set; }
        public string MRNo { get; set; }
        public string AdmNo { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public string ContactNo { get; set; }
        public string Diagnosis { get; set; }
        public string DateOfDeath { get; set; }
        public string TimeOfDeath { get; set; }
        public string CauseOfDeath { get; set; }
        public string HandOverName { get; set; }
        public string HandOverRelation { get; set; }
        public string HandOverCNIC { get; set; }
        public string HandOverCellNo { get; set; }
        public string ConsultantName { get; set; }
        public string MedicalOfficerId { get; set; }
        public string CreatedDate { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCertificateData();
            }
        }

        private void LoadCertificateData()
        {
            string certificateID = Request.QueryString["CertificateID"];

            if (string.IsNullOrEmpty(certificateID))
            {
                Response.Redirect("DeathCertificate.aspx");
                return;
            }

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"SELECT * FROM DeathCertificateStore WHERE CertificateID = @CertificateID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CertificateID", certificateID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    CertificateNo = dr["CertificateNo"] != DBNull.Value ? dr["CertificateNo"].ToString() : "";
                    MRNo = dr["MRNo"] != DBNull.Value ? dr["MRNo"].ToString() : "";
                    AdmNo = dr["AdmNo"] != DBNull.Value ? dr["AdmNo"].ToString() : "";
                    PatientName = dr["PatientName"] != DBNull.Value ? dr["PatientName"].ToString() : "";
                    Age = dr["Age"] != DBNull.Value ? dr["Age"].ToString() : "";
                    Gender = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : "";
                    CNIC = dr["CNIC"] != DBNull.Value ? dr["CNIC"].ToString() : "";
                    ContactNo = dr["ContactNo"] != DBNull.Value ? dr["ContactNo"].ToString() : "";
                    Diagnosis = dr["Diagnosis"] != DBNull.Value ? dr["Diagnosis"].ToString() : "";
                    DateOfDeath = dr["DateOfDeath"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfDeath"]).ToString("dd-MMM-yyyy") : "";
                    TimeOfDeath = dr["TimeOfDeath"] != DBNull.Value ? dr["TimeOfDeath"].ToString() : "";
                    CauseOfDeath = dr["CauseOfDeath"] != DBNull.Value ? dr["CauseOfDeath"].ToString() : "";
                    HandOverName = dr["HandOverName"] != DBNull.Value ? dr["HandOverName"].ToString() : "";
                    HandOverRelation = dr["HandOverRelation"] != DBNull.Value ? dr["HandOverRelation"].ToString() : "";
                    HandOverCNIC = dr["HandOverCNIC"] != DBNull.Value ? dr["HandOverCNIC"].ToString() : "";
                    HandOverCellNo = dr["HandOverCellNo"] != DBNull.Value ? dr["HandOverCellNo"].ToString() : "";
                    ConsultantName = dr["ConsultantName"] != DBNull.Value ? dr["ConsultantName"].ToString() : "";
                    MedicalOfficerId = dr["MedicalOfficerId"] != DBNull.Value ? dr["MedicalOfficerId"].ToString() : "";
                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy hh:mm tt") : "";
                }
                else
                {
                    Response.Redirect("DeathCertificate.aspx");
                }
                dr.Close();
            }
        }

        protected void btnReviewForm_Click(object sender, EventArgs e)
        {
            // Redirect to review form page with CertificateID
            string certificateID = Request.QueryString["CertificateID"];
            Response.Redirect("ReviewForm.aspx?CertificateID=" + certificateID);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("DeathCertificate.aspx");
        }
    }
}