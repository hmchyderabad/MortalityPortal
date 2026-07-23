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
    public partial class EditReviewForm : System.Web.UI.Page
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
                LoadReviewData();
            }
        }

        private string GetCertificateIDFromReviewID(int reviewId)
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

        private void LoadCertificateData()
        {
            string reviewID = Request.QueryString["ReviewID"];

            if (string.IsNullOrEmpty(reviewID))
            {
                Response.Redirect("DeathCertificate.aspx");
                return;
            }

            string certificateID = GetCertificateIDFromReviewID(Convert.ToInt32(reviewID));

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

        private void LoadReviewData()
        {
            string reviewID = Request.QueryString["ReviewID"];

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"SELECT * FROM DeathCertificateReview WHERE ReviewID = @ReviewID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ReviewID", reviewID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtDateShifted.Text = dr["DateShiftedToCriticalWard"] != DBNull.Value ? Convert.ToDateTime(dr["DateShiftedToCriticalWard"]).ToString("yyyy-MM-dd") : "";
                    txtCoMorbidities.Text = dr["CoMorbidities"] != DBNull.Value ? dr["CoMorbidities"].ToString() : "";
                    txtBriefClinicalSummary.Text = dr["BriefClinicalSummary"] != DBNull.Value ? dr["BriefClinicalSummary"].ToString() : "";

                    SetRadioButtonValue(rbExpY, rbExpN, rbExpNA, dr["ExpectedDeath"] != DBNull.Value ? dr["ExpectedDeath"].ToString() : "");
                    txtExpRemark.Text = dr["ExpectedDeathRemark"] != DBNull.Value ? dr["ExpectedDeathRemark"].ToString() : "";

                    SetRadioButtonValue(rbCodeY, rbCodeN, rbCodeNA, dr["CodeDecision"] != DBNull.Value ? dr["CodeDecision"].ToString() : "");
                    txtCodeRemark.Text = dr["CodeDecisionRemark"] != DBNull.Value ? dr["CodeDecisionRemark"].ToString() : "";

                    SetRadioButtonValue(rbDiagY, rbDiagN, rbDiagNA, dr["DelayDiagnosis"] != DBNull.Value ? dr["DelayDiagnosis"].ToString() : "");
                    txtDiagRemark.Text = dr["DelayDiagnosisRemark"] != DBNull.Value ? dr["DelayDiagnosisRemark"].ToString() : "";

                    SetRadioButtonValue(rbTreatY, rbTreatN, rbTreatNA, dr["DelayTreatment"] != DBNull.Value ? dr["DelayTreatment"].ToString() : "");
                    txtTreatRemark.Text = dr["DelayTreatmentRemark"] != DBNull.Value ? dr["DelayTreatmentRemark"].ToString() : "";

                    SetRadioButtonValue(rbProtoY, rbProtoN, rbProtoNA, dr["GuidelinesFollowed"] != DBNull.Value ? dr["GuidelinesFollowed"].ToString() : "");
                    txtProtoRemark.Text = dr["GuidelinesFollowedRemark"] != DBNull.Value ? dr["GuidelinesFollowedRemark"].ToString() : "";

                    SetRadioButtonValue(rbCommY, rbCommN, rbCommNA, dr["Communication"] != DBNull.Value ? dr["Communication"].ToString() : "");
                    txtCommRemark.Text = dr["CommunicationRemark"] != DBNull.Value ? dr["CommunicationRemark"].ToString() : "";

                    SetRadioButtonValue(rbDocY, rbDocN, rbDocNA, dr["Documentation"] != DBNull.Value ? dr["Documentation"].ToString() : "");
                    txtDocRemark.Text = dr["DocumentationRemark"] != DBNull.Value ? dr["DocumentationRemark"].ToString() : "";

                    SetRadioButtonValue(rbIcuY, rbIcuN, rbIcuNA, dr["IcuReturn"] != DBNull.Value ? dr["IcuReturn"].ToString() : "");
                    txtIcuRemark.Text = dr["IcuReturnRemark"] != DBNull.Value ? dr["IcuReturnRemark"].ToString() : "";

                    SetRadioButtonValue(rbIncY, rbIncN, rbIncNA, dr["IncidentReported"] != DBNull.Value ? dr["IncidentReported"].ToString() : "");
                    txtIncRemark.Text = dr["IncidentReportedRemark"] != DBNull.Value ? dr["IncidentReportedRemark"].ToString() : "";

                    SetRadioButtonValue(rbSentY, rbSentN, rbSentNA, dr["SentinelEvent"] != DBNull.Value ? dr["SentinelEvent"].ToString() : "");
                    txtSentRemark.Text = dr["SentinelEventRemark"] != DBNull.Value ? dr["SentinelEventRemark"].ToString() : "";

                    SetRadioButtonValue(rbHaiY, rbHaiN, rbHaiNA, dr["HAIReported"] != DBNull.Value ? dr["HAIReported"].ToString() : "");
                    txtHaiRemark.Text = dr["HAIReportedRemark"] != DBNull.Value ? dr["HAIReportedRemark"].ToString() : "";
                }
                dr.Close();
            }
        }

        private void SetRadioButtonValue(RadioButton rbY, RadioButton rbN, RadioButton rbNA, string value)
        {
            rbY.Checked = (value == "Y");
            rbN.Checked = (value == "N");
            rbNA.Checked = (value == "NA");
        }

        protected void btnUpdateReview_Click(object sender, EventArgs e)
        {
            string reviewID = Request.QueryString["ReviewID"];

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"UPDATE DeathCertificateReview
                        SET DateShiftedToCriticalWard = @DateShifted,
                            CoMorbidities = @CoMorbidities,
                            BriefClinicalSummary = @ClinicalSummary,
                            ExpectedDeath = @ExpectedDeath,
                            ExpectedDeathRemark = @ExpectedDeathRemark,
                            CodeDecision = @CodeDecision,
                            CodeDecisionRemark = @CodeDecisionRemark,
                            DelayDiagnosis = @DelayDiagnosis,
                            DelayDiagnosisRemark = @DelayDiagnosisRemark,
                            DelayTreatment = @DelayTreatment,
                            DelayTreatmentRemark = @DelayTreatmentRemark,
                            GuidelinesFollowed = @GuidelinesFollowed,
                            GuidelinesFollowedRemark = @GuidelinesFollowedRemark,
                            Communication = @Communication,
                            CommunicationRemark = @CommunicationRemark,
                            Documentation = @Documentation,
                            DocumentationRemark = @DocumentationRemark,
                            IcuReturn = @IcuReturn,
                            IcuReturnRemark = @IcuReturnRemark,
                            IncidentReported = @IncidentReported,
                            IncidentReportedRemark = @IncidentReportedRemark,
                            SentinelEvent = @SentinelEvent,
                            SentinelEventRemark = @SentinelEventRemark,
                            HAIReported = @HAIReported,
                            HAIReportedRemark = @HAIReportedRemark,
                            UpdatedDate = GETDATE()
                        WHERE ReviewID = @ReviewID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DateShifted", txtDateShifted.Text);
                cmd.Parameters.AddWithValue("@CoMorbidities", txtCoMorbidities.Text);
                cmd.Parameters.AddWithValue("@ClinicalSummary", txtBriefClinicalSummary.Text);

                cmd.Parameters.AddWithValue("@ExpectedDeath", GetRadioButtonValue(rbExpY, rbExpN, rbExpNA));
                cmd.Parameters.AddWithValue("@ExpectedDeathRemark", txtExpRemark.Text);

                cmd.Parameters.AddWithValue("@CodeDecision", GetRadioButtonValue(rbCodeY, rbCodeN, rbCodeNA));
                cmd.Parameters.AddWithValue("@CodeDecisionRemark", txtCodeRemark.Text);

                cmd.Parameters.AddWithValue("@DelayDiagnosis", GetRadioButtonValue(rbDiagY, rbDiagN, rbDiagNA));
                cmd.Parameters.AddWithValue("@DelayDiagnosisRemark", txtDiagRemark.Text);

                cmd.Parameters.AddWithValue("@DelayTreatment", GetRadioButtonValue(rbTreatY, rbTreatN, rbTreatNA));
                cmd.Parameters.AddWithValue("@DelayTreatmentRemark", txtTreatRemark.Text);

                cmd.Parameters.AddWithValue("@GuidelinesFollowed", GetRadioButtonValue(rbProtoY, rbProtoN, rbProtoNA));
                cmd.Parameters.AddWithValue("@GuidelinesFollowedRemark", txtProtoRemark.Text);

                cmd.Parameters.AddWithValue("@Communication", GetRadioButtonValue(rbCommY, rbCommN, rbCommNA));
                cmd.Parameters.AddWithValue("@CommunicationRemark", txtCommRemark.Text);

                cmd.Parameters.AddWithValue("@Documentation", GetRadioButtonValue(rbDocY, rbDocN, rbDocNA));
                cmd.Parameters.AddWithValue("@DocumentationRemark", txtDocRemark.Text);

                cmd.Parameters.AddWithValue("@IcuReturn", GetRadioButtonValue(rbIcuY, rbIcuN, rbIcuNA));
                cmd.Parameters.AddWithValue("@IcuReturnRemark", txtIcuRemark.Text);

                cmd.Parameters.AddWithValue("@IncidentReported", GetRadioButtonValue(rbIncY, rbIncN, rbIncNA));
                cmd.Parameters.AddWithValue("@IncidentReportedRemark", txtIncRemark.Text);

                cmd.Parameters.AddWithValue("@SentinelEvent", GetRadioButtonValue(rbSentY, rbSentN, rbSentNA));
                cmd.Parameters.AddWithValue("@SentinelEventRemark", txtSentRemark.Text);

                cmd.Parameters.AddWithValue("@HAIReported", GetRadioButtonValue(rbHaiY, rbHaiN, rbHaiNA));
                cmd.Parameters.AddWithValue("@HAIReportedRemark", txtHaiRemark.Text);

                cmd.Parameters.AddWithValue("@ReviewID", reviewID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, GetType(), "success", "Swal.fire('Success','Review form updated successfully!','success');", true);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string reviewID = Request.QueryString["ReviewID"];
            string certificateID = GetCertificateIDFromReviewID(Convert.ToInt32(reviewID));
            Response.Redirect("View.aspx?CertificateID=" + certificateID);
        }
    }
}