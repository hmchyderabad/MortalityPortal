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
    public partial class PrintCertificate : System.Web.UI.Page
    {
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;
        string hospitalConnection = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string certID = Request.QueryString["CertificateID"];
                if (!string.IsNullOrEmpty(certID))
                {
                    LoadCertificateForPrint(certID);
                }
                else
                {
                    Response.Redirect("DeathCertificate.aspx");
                }
            }
        }

        private void LoadCertificateForPrint(string certID)
        {
            // First, get the certificate data from MortalityDB to get MR# or ADM#
            string mrNo = "";
            string admNo = "";

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = "SELECT MRNo, AdmNo, CertificateNo, CauseOfDeath, Diagnosis, DateOfDeath, TimeOfDeath, HandOverName, HandOverRelation FROM DeathCertificateStore WHERE CertificateID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", certID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    mrNo = dr["MRNo"] != DBNull.Value ? dr["MRNo"].ToString() : "";
                    admNo = dr["AdmNo"] != DBNull.Value ? dr["AdmNo"].ToString() : "";
                }
                else
                {
                    Response.Write("<script>alert('Certificate not found'); window.close();</script>");
                    return;
                }
                dr.Close();
            }

            // Now fetch patient data from HospitalDB using MR# or ADM#
            if (!string.IsNullOrEmpty(mrNo) || !string.IsNullOrEmpty(admNo))
            {
                GenerateCertificateHTML(mrNo, admNo, certID);
            }
            else
            {
                Response.Write("<script>alert('No MR# or ADM# found for this certificate'); window.close();</script>");
            }
        }

        private void GenerateCertificateHTML(string mrNo, string admNo, string certID)
        {
            // Fetch patient data from HospitalDB using MR# or ADM#
            string patientName = "";
            string fatherName = "";
            string gender = "";
            string age = "";
            string cnic = "";
            string contactNo = "";
            string address = "";
            string consultantName = "";

            using (SqlConnection con = new SqlConnection(hospitalConnection))
            {
                string query = @"
                    SELECT
    a.MRNo,
    b.PatientTitle,
    b.PatientName,
    b.FatherTitle,
    b.FatherName,

    AGE =
        CAST(AgeCalc.Years AS VARCHAR(3)) + ' Years ' +
        CAST(AgeCalc.Months AS VARCHAR(2)) + ' Months ' +
        CAST(AgeCalc.Days AS VARCHAR(2)) + ' Days',

    b.Gender,
    b.Address,
    b.CNIC,
    b.CellNo
FROM IPD_Admission a
INNER JOIN EMR_Patients b
    ON a.MRNo = b.MRNo

CROSS APPLY
(
    SELECT
        Years = DATEDIFF(YEAR, b.DoB, GETDATE())
                - CASE
                    WHEN DATEADD(YEAR, DATEDIFF(YEAR, b.DoB, GETDATE()), b.DoB) > GETDATE()
                    THEN 1 ELSE 0 END
) Y

CROSS APPLY
(
    SELECT
        YearDate = DATEADD(YEAR, Y.Years, b.DoB)
) D1

CROSS APPLY
(
    SELECT
        Months = DATEDIFF(MONTH, D1.YearDate, GETDATE())
                 - CASE
                     WHEN DATEADD(MONTH, DATEDIFF(MONTH, D1.YearDate, GETDATE()), D1.YearDate) > GETDATE()
                     THEN 1 ELSE 0 END
) M

CROSS APPLY
(
    SELECT
        MonthDate = DATEADD(MONTH, M.Months, D1.YearDate)
) D2

CROSS APPLY
(
    SELECT
        Years = Y.Years,
        Months = M.Months,
        Days = DATEDIFF(DAY, D2.MonthDate, GETDATE())
) AgeCalc
                    WHERE (a.MRNo = @MRNo OR a.AdmNo = @AdmNo)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MRNo", mrNo);
                cmd.Parameters.AddWithValue("@AdmNo", admNo);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    patientName = (dr["PatientTitle"] != DBNull.Value ? dr["PatientTitle"].ToString() + " " : "") +
                                   (dr["PatientName"] != DBNull.Value ? dr["PatientName"].ToString() : "");
                    fatherName = (dr["FatherTitle"] != DBNull.Value ? dr["FatherTitle"].ToString() + " " : "") +
                                  (dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : "");
                    age = dr["AGE"] != DBNull.Value ? dr["AGE"].ToString() : "";
                    gender = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : "";
                    cnic = dr["CNIC"] != DBNull.Value ? dr["CNIC"].ToString() : "";
                    contactNo = dr["CellNo"] != DBNull.Value ? dr["CellNo"].ToString() : "";
                    address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : "";
                    consultantName = ""; // Consultant name not in this query, will fetch separately
                }
                dr.Close();
            }

            // Fetch consultant name separately
            if (string.IsNullOrEmpty(consultantName))
            {
                using (SqlConnection con = new SqlConnection(hospitalConnection))
                {
                    string query = @"
                        SELECT ISNULL(e.ConsultantName, 'N/A') AS ConsultantName
                        FROM IPD_Admission a
                        LEFT JOIN Gen_Consultants e ON a.ConsultantCode = e.ConsultantCode
                        WHERE (a.MRNo = @MRNo OR a.AdmNo = @AdmNo)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MRNo", mrNo);
                    cmd.Parameters.AddWithValue("@AdmNo", admNo);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        consultantName = result.ToString();
                    }
                }
            }

            // Fetch certificate data from MortalityDB using CertificateID
            string certificateNo = "";
            string causeOfDeath = "";
            string diagnosis = "";
            string dateOfDeath = "";
            string timeOfDeath = "";
            string handOverName = "";
            string handOverRelation = "";

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"
                    SELECT 
                        CertificateNo, CauseOfDeath, Diagnosis, DateOfDeath, TimeOfDeath,
                        HandOverName, HandOverRelation
                    FROM DeathCertificateStore 
                    WHERE CertificateID = @CertificateID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CertificateID", certID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    certificateNo = dr["CertificateNo"] != DBNull.Value ? dr["CertificateNo"].ToString() : "";
                    causeOfDeath = dr["CauseOfDeath"] != DBNull.Value ? dr["CauseOfDeath"].ToString() : "";
                    diagnosis = dr["Diagnosis"] != DBNull.Value ? dr["Diagnosis"].ToString() : "";
                    timeOfDeath = dr["TimeOfDeath"] != DBNull.Value ? dr["TimeOfDeath"].ToString() : "";
                    handOverName = dr["HandOverName"] != DBNull.Value ? dr["HandOverName"].ToString() : "";
                    handOverRelation = dr["HandOverRelation"] != DBNull.Value ? dr["HandOverRelation"].ToString() : "son of";

                    if (dr["DateOfDeath"] != DBNull.Value)
                    {
                        dateOfDeath = Convert.ToDateTime(dr["DateOfDeath"]).ToString("dd-MMM-yyyy");
                    }
                }
                dr.Close();
            }

            // Certificate No
            litCertNo.Text = certificateNo;

            // Date - use print date (current date)
            litDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            // Full Name
            litFullName.Text = patientName;

            // Guardian Name
            litGuardianName.Text = fatherName;

            // Age
            litAge.Text = age;

            // Gender
            litGender.Text = gender;

            // Address
            litAddress.Text = address;

            // CNIC
            litCNIC.Text = cnic;

            // Time of Death
            litTime.Text = timeOfDeath;

            // Date of Death
            litDeathDate.Text = dateOfDeath;

            // Place of Death
            litPlaceOfDeath.Text = "Hashim Medical City";


            // Doctor Details
            litDoctorName.Text = consultantName;

            // Diagnosis
            litDiagnosis.Text = diagnosis;

            // Cause of Death (Immediate cause)
            litImmediateCause.Text = causeOfDeath;
        }
    }
}