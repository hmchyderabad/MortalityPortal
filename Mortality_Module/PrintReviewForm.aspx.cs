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
    public partial class PrintReviewForm : System.Web.UI.Page
    {
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;
        string hospitalConnection = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReviewFormData();
            }
        }

        private void LoadReviewFormData()
        {
            string reviewID = Request.QueryString["ReviewID"];

            if (string.IsNullOrEmpty(reviewID))
            {
                Response.Redirect("ReviewFormList.aspx");
                return;
            }

            // Get CertificateID from ReviewID
            string certificateID = GetCertificateIDFromReviewID(Convert.ToInt32(reviewID));

            if (string.IsNullOrEmpty(certificateID))
            {
                Response.Redirect("ReviewFormList.aspx");
                return;
            }

            // Load data from both tables
            LoadDeathCertificateData(certificateID, reviewID);
            LoadIPDAdmissionData();
            LoadReviewData(reviewID);
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

        private void LoadDeathCertificateData(string certificateID, string ReviewFormID)
        {
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"SELECT * FROM DeathCertificateStore WHERE CertificateID = @CertificateID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CertificateID", certificateID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Patient Identification
                    lblPatientName.Text = dr["PatientName"] != DBNull.Value ? dr["PatientName"].ToString() : "";
                    lblMRNo.Text = dr["MRNo"] != DBNull.Value ? dr["MRNo"].ToString() : "";
                    lblSerialNo.Text = ReviewFormID;

                    // Death Certificate Serial #
                    lblDeathCertSerial.Text = dr["CertificateNo"] != DBNull.Value ? dr["CertificateNo"].ToString() : "";

                    // Date & Time of Death
                    if (dr["DateOfDeath"] != DBNull.Value && dr["TimeOfDeath"] != DBNull.Value)
                    {
                        DateTime dod = Convert.ToDateTime(dr["DateOfDeath"]);
                        string tod = dr["TimeOfDeath"].ToString();
                        lblDateTimeOfDeath.Text = dod.ToString("dd-MMM-yyyy") + " at " + tod;
                    }

                    // Primary Diagnosis
                    lblPrimaryDiagnosis.Text = dr["Diagnosis"] != DBNull.Value ? dr["Diagnosis"].ToString() : "";

                    // Attending Physician
                    lblAttendingPhysician.Text = dr["ConsultantName"] != DBNull.Value ? dr["ConsultantName"].ToString() : "";
                }
                dr.Close();
            }
        }

        private void LoadIPDAdmissionData()
        {
            string mrNo = lblMRNo.Text;
            if (string.IsNullOrEmpty(mrNo))
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(hospitalConnection))
            {
                string query = @"SELECT
                    a.AdmDate,
                    b.WardName,
                    c.ConsultantName,
                    a.MRNo,
                    LengthOfStay = DATEDIFF(DAY, a.AdmDate, ISNULL(a.DischargeDateTime, GETDATE())) + 1
                FROM IPD_Admission AS a
                INNER JOIN Gen_WardMaster AS b
                    ON a.WardCode = b.WardCode
                LEFT JOIN Gen_Consultants AS c
                    ON a.ConsultantCode = c.ConsultantCode
                WHERE
                    (
                        a.BillNo IS NULL
                        OR a.BillNo = ''
                        OR a.DischargeFlag = 1
                    )
                    AND a.MRNo = @MRNo";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MRNo", mrNo);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Date of Admission
                    if (dr["AdmDate"] != DBNull.Value)
                    {
                        lblDateOfAdmission.Text = Convert.ToDateTime(dr["AdmDate"]).ToString("dd-MMM-yyyy");
                    }

                    // Admitting Ward
                    lblAdmittingWard.Text = dr["WardName"] != DBNull.Value ? dr["WardName"].ToString() : "";

                    // Length of Stay
                    lblLengthOfStay.Text = dr["LengthOfStay"] != DBNull.Value ? dr["LengthOfStay"].ToString() : "";
                }
                dr.Close();
            }
        }

        private void LoadReviewData(string reviewID)
        {
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"SELECT * FROM DeathCertificateReview WHERE ReviewID = @ReviewID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ReviewID", reviewID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Date of Review
                    lblDateOfReview.Text = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy") : "";

                    // Date Shifted into Critical Ward
                    if (dr["DateShiftedToCriticalWard"] != DBNull.Value)
                    {
                        lblDateShiftedCritical.Text = Convert.ToDateTime(dr["DateShiftedToCriticalWard"]).ToString("dd-MMM-yyyy");
                    }

                    // Co-morbidities
                    lblCoMorbidities.Text = dr["CoMorbidities"] != DBNull.Value ? dr["CoMorbidities"].ToString() : "";

                    // Brief Clinical Summary
                    lblClinicalSummary.Text = dr["BriefClinicalSummary"] != DBNull.Value ? dr["BriefClinicalSummary"].ToString() : "";

                    // Review Criteria - Labels
                    SetReviewCriteriaLabel(lblExpY, lblExpN, lblExpNA, dr["ExpectedDeath"] != DBNull.Value ? dr["ExpectedDeath"].ToString() : "");
                    SetReviewCriteriaLabel(lblCodeY, lblCodeN, lblCodeNA, dr["CodeDecision"] != DBNull.Value ? dr["CodeDecision"].ToString() : "");
                    SetReviewCriteriaLabel(lblDiagY, lblDiagN, lblDiagNA, dr["DelayDiagnosis"] != DBNull.Value ? dr["DelayDiagnosis"].ToString() : "");
                    SetReviewCriteriaLabel(lblTreatY, lblTreatN, lblTreatNA, dr["DelayTreatment"] != DBNull.Value ? dr["DelayTreatment"].ToString() : "");
                    SetReviewCriteriaLabel(lblProtoY, lblProtoN, lblProtoNA, dr["GuidelinesFollowed"] != DBNull.Value ? dr["GuidelinesFollowed"].ToString() : "");
                    SetReviewCriteriaLabel(lblCommY, lblCommN, lblCommNA, dr["Communication"] != DBNull.Value ? dr["Communication"].ToString() : "");
                    SetReviewCriteriaLabel(lblDocY, lblDocN, lblDocNA, dr["Documentation"] != DBNull.Value ? dr["Documentation"].ToString() : "");
                    SetReviewCriteriaLabel(lblIcuY, lblIcuN, lblIcuNA, dr["IcuReturn"] != DBNull.Value ? dr["IcuReturn"].ToString() : "");
                    SetReviewCriteriaLabel(lblIncY, lblIncN, lblIncNA, dr["IncidentReported"] != DBNull.Value ? dr["IncidentReported"].ToString() : "");
                    SetReviewCriteriaLabel(lblSentY, lblSentN, lblSentNA, dr["SentinelEvent"] != DBNull.Value ? dr["SentinelEvent"].ToString() : "");
                    SetReviewCriteriaLabel(lblHaiY, lblHaiN, lblHaiNA, dr["HAIReported"] != DBNull.Value ? dr["HAIReported"].ToString() : "");

                    // Review Criteria - Remarks
                    lblExpRemark.Text = dr["ExpectedDeathRemark"] != DBNull.Value ? dr["ExpectedDeathRemark"].ToString() : "";
                    lblCodeRemark.Text = dr["CodeDecisionRemark"] != DBNull.Value ? dr["CodeDecisionRemark"].ToString() : "";
                    lblDiagRemark.Text = dr["DelayDiagnosisRemark"] != DBNull.Value ? dr["DelayDiagnosisRemark"].ToString() : "";
                    lblTreatRemark.Text = dr["DelayTreatmentRemark"] != DBNull.Value ? dr["DelayTreatmentRemark"].ToString() : "";
                    lblProtoRemark.Text = dr["GuidelinesFollowedRemark"] != DBNull.Value ? dr["GuidelinesFollowedRemark"].ToString() : "";
                    lblCommRemark.Text = dr["CommunicationRemark"] != DBNull.Value ? dr["CommunicationRemark"].ToString() : "";
                    lblDocRemark.Text = dr["DocumentationRemark"] != DBNull.Value ? dr["DocumentationRemark"].ToString() : "";
                    lblIcuRemark.Text = dr["IcuReturnRemark"] != DBNull.Value ? dr["IcuReturnRemark"].ToString() : "";
                    lblIncRemark.Text = dr["IncidentReportedRemark"] != DBNull.Value ? dr["IncidentReportedRemark"].ToString() : "";
                    lblSentRemark.Text = dr["SentinelEventRemark"] != DBNull.Value ? dr["SentinelEventRemark"].ToString() : "";
                    lblHaiRemark.Text = dr["HAIReportedRemark"] != DBNull.Value ? dr["HAIReportedRemark"].ToString() : "";
                }
                dr.Close();
            }
        }

        private void SetReviewCriteriaLabel(Label lblY, Label lblN, Label lblNA, string value)
        {
            lblY.Text = (value == "Y") ? "✓" : "";
            lblN.Text = (value == "N") ? "✓" : "";
            lblNA.Text = (value == "NA") ? "✓" : "";
        }
    }
}