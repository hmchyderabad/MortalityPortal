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
    public partial class ReviewForm : System.Web.UI.Page
    {
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;
        string hospitalConnection = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

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
                LoadWards();
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

        private void LoadWards()
        {
            using (SqlConnection con = new SqlConnection(hospitalConnection))
            {
                string query = @"SELECT WardCode, WardName FROM Gen_WardMaster ORDER BY WardName";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlShiftedIntoCriticalWard.DataSource = dr;
                ddlShiftedIntoCriticalWard.DataTextField = "WardName";
                ddlShiftedIntoCriticalWard.DataValueField = "WardCode";
                ddlShiftedIntoCriticalWard.DataBind();

                dr.Close();
            }
        }

        protected void btnSaveReview_Click(object sender, EventArgs e)
        {
            // Save review form data
            string certificateID = Request.QueryString["CertificateID"];
            
            // Reload certificate data to ensure CertificateNo is available
            LoadCertificateData();

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                // Check if review already exists
                string checkQuery = @"SELECT COUNT(*) FROM DeathCertificateReview WHERE CertificateID = @CertificateID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@CertificateID", certificateID);
                con.Open();
                int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                con.Close();

                string query = @"INSERT INTO DeathCertificateReview
                            (CertificateID, DateTimeShiftedToCriticalWard, ShiftedIntoCriticalWard, CoMorbidities, BriefClinicalSummary,
                            ExpectedDeath, ExpectedDeathRemark, CodeDecision, CodeDecisionRemark,
                            DelayDiagnosis, DelayDiagnosisRemark, DelayTreatment, DelayTreatmentRemark,
                            GuidelinesFollowed, GuidelinesFollowedRemark, Communication, CommunicationRemark,
                            Documentation, DocumentationRemark, IcuReturn, IcuReturnRemark,
                            IncidentReported, IncidentReportedRemark, SentinelEvent, SentinelEventRemark,
                            HAIReported, HAIReportedRemark, CreatedDate, CreatedBy, ReviewFormID)
                            VALUES
                            (@CertificateID, @DateTimeShifted, @ShiftedIntoCriticalWard, @CoMorbidities, @ClinicalSummary,
                            @ExpectedDeath, @ExpectedDeathRemark, @CodeDecision, @CodeDecisionRemark,
                            @DelayDiagnosis, @DelayDiagnosisRemark, @DelayTreatment, @DelayTreatmentRemark,
                            @GuidelinesFollowed, @GuidelinesFollowedRemark, @Communication, @CommunicationRemark,
                            @Documentation, @DocumentationRemark, @IcuReturn, @IcuReturnRemark,
                            @IncidentReported, @IncidentReportedRemark, @SentinelEvent, @SentinelEventRemark,
                            @HAIReported, @HAIReportedRemark, GETDATE(), @CreatedBy, @ReviewFormID)";


                string reviewFormNo = GenerateReviewFormNumber();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ReviewFormID", reviewFormNo);
                cmd.Parameters.AddWithValue("@DateTimeShifted", string.IsNullOrEmpty(txtDateShifted.Text) ? (object)DBNull.Value : DateTime.Parse(txtDateShifted.Text));
                cmd.Parameters.AddWithValue("@ShiftedIntoCriticalWard", ddlShiftedIntoCriticalWard.SelectedValue);
                cmd.Parameters.AddWithValue("@CoMorbidities", txtCoMorbidities.Text);
                cmd.Parameters.AddWithValue("@ClinicalSummary", txtBriefClinicalSummary.Text);

                // Review Criteria - Question 1: Expected Death
                cmd.Parameters.AddWithValue("@ExpectedDeath", GetRadioButtonValue(rbExpY, rbExpN, rbExpNA));
                cmd.Parameters.AddWithValue("@ExpectedDeathRemark", txtExpRemark.Text);

                // Review Criteria - Question 2: Code Decision
                cmd.Parameters.AddWithValue("@CodeDecision", GetRadioButtonValue(rbCodeY, rbCodeN, rbCodeNA));
                cmd.Parameters.AddWithValue("@CodeDecisionRemark", txtCodeRemark.Text);

                // Review Criteria - Question 3: Delay Diagnosis
                cmd.Parameters.AddWithValue("@DelayDiagnosis", GetRadioButtonValue(rbDiagY, rbDiagN, rbDiagNA));
                cmd.Parameters.AddWithValue("@DelayDiagnosisRemark", txtDiagRemark.Text);

                // Review Criteria - Question 4: Delay Treatment
                cmd.Parameters.AddWithValue("@DelayTreatment", GetRadioButtonValue(rbTreatY, rbTreatN, rbTreatNA));
                cmd.Parameters.AddWithValue("@DelayTreatmentRemark", txtTreatRemark.Text);

                // Review Criteria - Question 5: Guidelines Followed
                cmd.Parameters.AddWithValue("@GuidelinesFollowed", GetRadioButtonValue(rbProtoY, rbProtoN, rbProtoNA));
                cmd.Parameters.AddWithValue("@GuidelinesFollowedRemark", txtProtoRemark.Text);

                // Review Criteria - Question 6: Communication
                cmd.Parameters.AddWithValue("@Communication", GetRadioButtonValue(rbCommY, rbCommN, rbCommNA));
                cmd.Parameters.AddWithValue("@CommunicationRemark", txtCommRemark.Text);

                // Review Criteria - Question 7: Documentation
                cmd.Parameters.AddWithValue("@Documentation", GetRadioButtonValue(rbDocY, rbDocN, rbDocNA));
                cmd.Parameters.AddWithValue("@DocumentationRemark", txtDocRemark.Text);

                // Review Criteria - Question 8: ICU Return
                cmd.Parameters.AddWithValue("@IcuReturn", GetRadioButtonValue(rbIcuY, rbIcuN, rbIcuNA));
                cmd.Parameters.AddWithValue("@IcuReturnRemark", txtIcuRemark.Text);

                // Review Criteria - Question 9: Incident Reported
                cmd.Parameters.AddWithValue("@IncidentReported", GetRadioButtonValue(rbIncY, rbIncN, rbIncNA));
                cmd.Parameters.AddWithValue("@IncidentReportedRemark", txtIncRemark.Text);

                // Review Criteria - Question 10: Sentinel Event
                cmd.Parameters.AddWithValue("@SentinelEvent", GetRadioButtonValue(rbSentY, rbSentN, rbSentNA));
                cmd.Parameters.AddWithValue("@SentinelEventRemark", txtSentRemark.Text);

                // Review Criteria - Question 11: HAI Reported
                cmd.Parameters.AddWithValue("@HAIReported", GetRadioButtonValue(rbHaiY, rbHaiN, rbHaiNA));
                cmd.Parameters.AddWithValue("@HAIReportedRemark", txtHaiRemark.Text);

                cmd.Parameters.AddWithValue("@CertificateID", certificateID);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserID"] != null ? Session["UserID"].ToString() : "");

                con.Open();
                cmd.ExecuteNonQuery();
                // Insert into MortalityTracking table
                string trackingQuery = @"Update MortalityTracking 
                                        Set ReviewFormNo = @ReviewFormNo, IsReviewFormCreated = 1
                                        Where DeathCertificateNo = @DeathCertNo AND IsReviewFormCreated = 0 AND IsDeathCertificateCreated = 1;";

                SqlCommand trackingCmd = new SqlCommand(trackingQuery, con);
                trackingCmd.Parameters.AddWithValue("@DeathCertNo", CertificateNo);
                trackingCmd.Parameters.AddWithValue("@ReviewFormNo", reviewFormNo);

                trackingCmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, GetType(), "success", "Swal.fire('Success','Review form saved successfully!','success');", true);
                Response.Redirect("ReviewFormList.aspx");
            }
        }

        private string GetRadioButtonValue(RadioButton rbY, RadioButton rbN, RadioButton rbNA)
        {
            if (rbY.Checked) return "Y";
            if (rbN.Checked) return "N";
            if (rbNA.Checked) return "NA";
            return "";
        }

         private string GenerateReviewFormNumber()
        {
            string year = DateTime.Now.Year.ToString();
            string prefix = "RF/" + year + "/";

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = "SELECT COUNT(*) FROM DeathCertificateStore WHERE CertificateNo LIKE @prefix + '%'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                con.Close();
                return prefix + count.ToString("D6");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("View.aspx?CertificateID=" + Request.QueryString["CertificateID"]);
        }
    }
}