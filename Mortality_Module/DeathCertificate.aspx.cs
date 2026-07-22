using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem.Mortality_Module
{
    public partial class DeathCertificate : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;
        string mortalityConnection = ConfigurationManager.ConnectionStrings["MortalityDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCertificatesGrid();
            }
        }

        private void LoadCertificatesGrid(string search = "", string status = "")
        {
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"SELECT CertificateID, CertificateNo, MRNo, PatientName, DateOfDeath, 
                                TimeOfDeath, CauseOfDeath, Diagnosis, HandOverName, HandOverRelation, 
                                HandOverCNIC, HandOverCellNo, ConsultantName, CreatedDate
                                FROM DeathCertificateStore 
                                WHERE (1=1)";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND (MRNo LIKE @search OR PatientName LIKE @search OR CertificateNo LIKE @search)";
                }
                if (status == "Issued")
                {
                    query += " AND CertificateNo IS NOT NULL";
                }

                query += " ORDER BY CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                if (!string.IsNullOrEmpty(search))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCertificates.DataSource = dt;
                gvCertificates.DataBind();
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length >= 2)
            {
                SearchPatientAndLoadDetails(txtSearch.Text);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCertificatesGrid(txtSearch.Text, ddlStatus.SelectedValue);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
            pnlPatientDetails.Visible = false;
            pnlCertificateForm.Visible = false;
            LoadCertificatesGrid();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCertificatesGrid(txtSearch.Text, ddlStatus.SelectedValue);
        }

        private void SearchPatientAndLoadDetails(string searchTerm)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 10
            a.AdmNo + ' - ' + a.MRNo + ' - ' + c.PatientName AS Patient
        FROM IPD_Admission a
        INNER JOIN EMR_Patients c ON a.MRNo = c.MRNo
        WHERE a.BillNo IS NULL
        AND (
            a.AdmNo LIKE @p 
            OR a.MRNo LIKE @p
            OR c.PatientName LIKE @p
        )
        ORDER BY c.PatientName";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@p", "%" + searchTerm + "%");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblMRNo.Text = dr["MRNo"].ToString();
                    lblAdmNo.Text = dr["AdmNo"].ToString();
                    lblPatientName.Text = dr["PatientFullName"].ToString();
                    lblAgeGender.Text = dr["Age"].ToString() + " yrs / " + dr["Gender"].ToString();
                    lblCNIC.Text = dr["CNIC"].ToString();
                    lblContact.Text = dr["CellNo"].ToString();
                    lblConsultant.Text = dr["ConsultantName"].ToString();
                    lblDiagnosis.Text = dr["Diagnosis"].ToString();

                    pnlPatientDetails.Visible = true;
                    pnlCertificateForm.Visible = true;

                    // Set default values
                    txtFinalDiagnosis.Text = dr["Diagnosis"].ToString();
                    hdnCertificateID.Value = "";
                }
                else
                {
                    pnlPatientDetails.Visible = false;
                    pnlCertificateForm.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Not Found','No patient found with this search criteria','warning');", true);
                }
                dr.Close();
            }
        }

        protected void btnNewCertificate_Click(object sender, EventArgs e)
        {
            pnlPatientDetails.Visible = false;
            pnlCertificateForm.Visible = true;
            ClearForm();
        }

        private void ClearForm()
        {
            txtDateOfDeath.Text = "";
            txtTimeOfDeath.Text = "";
            txtCauseOfDeath.Text = "";
            txtFinalDiagnosis.Text = "";
            txtReceiverName.Text = "";
            txtRelation.Text = "";
            txtReceiverCNIC.Text = "";
            txtReceiverContact.Text = "";
            hdnCertificateID.Value = "";
        }

        protected void btnSaveCertificate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDateOfDeath.Text) || string.IsNullOrEmpty(txtCauseOfDeath.Text) || string.IsNullOrEmpty(txtReceiverName.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Required','Please fill all required fields','error');", true);
                return;
            }

            string certificateNo = GenerateCertificateNumber();

            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"INSERT INTO DeathCertificateStore 
                                (CertificateNo, MRNo, AdmNo, PatientName, Age, Gender, CNIC, ContactNo,
                                Diagnosis, DateOfDeath, TimeOfDeath, CauseOfDeath, HandOverName, 
                                HandOverRelation, HandOverCNIC, HandOverCellNo, ConsultantName, 
                                CreatedDate, CompCode)
                                VALUES 
                                (@CertNo, @MRNo, @AdmNo, @PatientName, @Age, @Gender, @CNIC, @Contact,
                                @Diagnosis, @DOD, @TOD, @Cause, @HandOver, @Relation, @HandCNIC, @HandCell,
                                @Consultant, GETDATE(), @CompCode)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CertNo", certificateNo);
                cmd.Parameters.AddWithValue("@MRNo", lblMRNo.Text);
                cmd.Parameters.AddWithValue("@AdmNo", lblAdmNo.Text);
                cmd.Parameters.AddWithValue("@PatientName", lblPatientName.Text);
                cmd.Parameters.AddWithValue("@Age", lblAgeGender.Text.Split('/')[0].Replace("yrs", "").Trim());
                cmd.Parameters.AddWithValue("@Gender", lblAgeGender.Text.Split('/')[1].Trim());
                cmd.Parameters.AddWithValue("@CNIC", lblCNIC.Text);
                cmd.Parameters.AddWithValue("@Contact", lblContact.Text);
                cmd.Parameters.AddWithValue("@Diagnosis", txtFinalDiagnosis.Text);
                cmd.Parameters.AddWithValue("@DOD", DateTime.Parse(txtDateOfDeath.Text));
                cmd.Parameters.AddWithValue("@TOD", txtTimeOfDeath.Text);
                cmd.Parameters.AddWithValue("@Cause", txtCauseOfDeath.Text);
                cmd.Parameters.AddWithValue("@HandOver", txtReceiverName.Text);
                cmd.Parameters.AddWithValue("@Relation", txtRelation.Text);
                cmd.Parameters.AddWithValue("@HandCNIC", txtReceiverCNIC.Text);
                cmd.Parameters.AddWithValue("@HandCell", txtReceiverContact.Text);
                cmd.Parameters.AddWithValue("@Consultant", lblConsultant.Text);
                cmd.Parameters.AddWithValue("@CompCode", "001");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                ScriptManager.RegisterStartupScript(this, GetType(), "success", "Swal.fire('Success','Death Certificate saved successfully!','success');", true);
                LoadCertificatesGrid();
                ClearForm();
                pnlCertificateForm.Visible = false;
            }
        }

        private string GenerateCertificateNumber()
        {
            string year = DateTime.Now.Year.ToString();
            string prefix = "DC/" + year + "/";

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

        protected void btnCancelForm_Click(object sender, EventArgs e)
        {
            pnlCertificateForm.Visible = false;
            pnlPatientDetails.Visible = false;
            ClearForm();
        }

        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            GenerateCertificateHTML();
        }

        protected void gvCertificates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewCert" || e.CommandName == "PrintCert")
            {
                string certID = e.CommandArgument.ToString();
                Response.Redirect("PrintCertificate.aspx?CertificateID=" + certID);
            }
            else if (e.CommandName == "DeleteCert")
            {
                string certID = e.CommandArgument.ToString();
                DeleteCertificate(certID);
            }
        }

        private void DeleteCertificate(string certID)
        {
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = "DELETE FROM DeathCertificateStore WHERE CertificateID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", certID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                LoadCertificatesGrid();
                ScriptManager.RegisterStartupScript(this, GetType(), "deleted", "Swal.fire('Deleted','Certificate deleted successfully','success');", true);
            }
        }

        private void LoadCertificateForPrint(string certID)
        {
            hdnCertificateID.Value = certID;
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = "SELECT * FROM DeathCertificateStore WHERE CertificateID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", certID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    GenerateCertificateHTML(dr);
                }
                dr.Close();
            }
        }

        private void GenerateCertificateHTML(SqlDataReader dr = null)
        {
            string html = @"
            
                <div style='text-align: center; border-bottom: 2px solid #1a5a4c; padding-bottom: 15px; margin-bottom: 20px;'>
                    <h1 style='color: #1a5a4c; margin: 0;'>HASHIM MEDICAL CITY</h1>
                    <p style='font-size: 14px; margin: 5px 0;'>The Hashim Medical City Hospital, Beside Palm Enclave Society, By-Pass Hyderabad</p>
                    <h3 style='color: #dc3545; margin: 10px 0;'>DEATH CERTIFICATE</h3>
                </div>";

            if (dr != null && dr.HasRows)
            {
                html += @"
                <table style='width: 100%; border-collapse: collapse;'>
                    <tr><td style='padding: 8px; width: 35%; font-weight: bold;'>Certificate No:</td><td style='padding: 8px;'>" + dr["CertificateNo"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 8px; font-weight: bold;'>M.R. No:</td><td style='padding: 8px;'>" + dr["MRNo"] + @"</td></tr>
                    <tr><td style='padding: 8px; font-weight: bold;'>Patient Name:</td><td style='padding: 8px;'>" + dr["PatientName"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 8px; font-weight: bold;'>Age / Gender:</td><td style='padding: 8px;'>" + dr["Age"] + " yrs / " + dr["Gender"] + @"</td></tr>
                    <tr><td style='padding: 8px; font-weight: bold;'>CNIC No:</td><td style='padding: 8px;'>" + dr["CNIC"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 8px; font-weight: bold;'>Date of Death:</td><td style='padding: 8px;'>" + Convert.ToDateTime(dr["DateOfDeath"]).ToString("dd-MMM-yyyy") + " at " + dr["TimeOfDeath"] + @"</td></tr>
                    <tr><td style='padding: 8px; font-weight: bold;'>Cause of Death:</td><td style='padding: 8px;'>" + dr["CauseOfDeath"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 8px; font-weight: bold;'>Diagnosis:</td><td style='padding: 8px;'>" + dr["Diagnosis"] + @"</td></tr>
                    <tr><td style='padding: 8px; font-weight: bold;'>Attending Consultant:</td><td style='padding: 8px;'>" + dr["ConsultantName"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 8px; font-weight: bold;'>Body Received By:</td><td style='padding: 8px;'>" + dr["HandOverName"] + " (" + dr["HandOverRelation"] + @")</td></tr>
                    <tr><td style='padding: 8px; font-weight: bold;'>Receiver CNIC / Contact:</td><td style='padding: 8px;'>" + dr["HandOverCNIC"] + " / " + dr["HandOverCellNo"] + @"</td></tr>
                </table>

                <div style='margin-top: 40px; padding-top: 20px; border-top: 1px dashed #ccc; display: flex; justify-content: space-between;'>
                    <div style='text-align: center;'>
                        <p>_________________________</p>
                        <p><strong>Medical Officer</strong></p>
                        <p>Hashim Medical City</p>
                    </div>
                    <div style='text-align: center;'>
                        <p>_________________________</p>
                        <p><strong>Medical Superintendent</strong></p>
                        <p>Hashim Medical City</p>
                    </div>
                </div>

                <div style='text-align: center; margin-top: 25px; font-size: 11px; color: #666;'>
                    <p>Issued on: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") + @" | Computer Generated Certificate</p>
                    <p>This is a system-generated certificate and requires no signature if digitally verified.</p>
                </div>";
            }

            html += "</div>";

            // Inject into modal
            string script = "document.getElementById('certificatePrintArea').innerHTML = `" + html.Replace("`", "\\`") + "`;";
            ScriptManager.RegisterStartupScript(this, GetType(), "loadCert", script, true);
        }

        private void GenerateCertificateHTML()
        {
            using (SqlConnection con = new SqlConnection(mortalityConnection))
            {
                string query = @"SELECT * FROM DeathCertificateStore WHERE MRNo = @MRNo ORDER BY CertificateID DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MRNo", lblMRNo.Text);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    GenerateCertificateHTML(dr);
                }
                else
                {
                    // Generate from form data
                    string html = CreateHTMLFromForm();
                    string script = "document.getElementById('certificatePrintArea').innerHTML = `" + html.Replace("`", "\\`") + "`;";
                    ScriptManager.RegisterStartupScript(this, GetType(), "loadCert", script, true);
                }
                dr.Close();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "showCertificateModal();", true);
        }

        private string CreateHTMLFromForm()
        {
            return @"
            <div style='font-family: 'Times New Roman', serif; padding: 30px; max-width: 800px; margin: 0 auto;'>
                <div style='text-align: center; border-bottom: 2px solid #1a5a4c; padding-bottom: 15px;'>
                    <h1 style='color: #1a5a4c;'>HASHIM MEDICAL CITY</h1>
                    <p>The Hashim Medical City Hospital, Beside Palm Enclave Society, By-Pass Hyderabad</p>
                    <h3 style='color: #dc3545;'>DEATH CERTIFICATE (DRAFT)</h3>
                </div>
                <table style='width: 100%; margin-top: 20px;'>
                    <tr><td><strong>Patient Name:</strong></td><td>" + lblPatientName.Text + @"</td></tr>
                    <tr><td><strong>MR No:</strong></td><td>" + lblMRNo.Text + @"</td></tr>
                    <tr><td><strong>Date of Death:</strong></td><td>" + txtDateOfDeath.Text + " at " + txtTimeOfDeath.Text + @"</td></tr>
                    <tr><td><strong>Cause of Death:</strong></td><td>" + txtCauseOfDeath.Text + @"</td></tr>
                    <tr><td><strong>Body Received By:</strong></td><td>" + txtReceiverName.Text + " (" + txtRelation.Text + @")</td></tr>
                </table>
                <div style='margin-top: 50px; text-align: center;'>
                    <p><em>This is a draft certificate. Please save to generate official certificate number.</em></p>
                </div>
            </div>";
        }

        protected void gvCertificates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='#f0f8ff';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='';";
            }
        }

        // WEB METHOD FOR AUTOCOMPLETE
        [System.Web.Services.WebMethod]
        public static List<string> GetPatients(string prefix)
        {
            List<string> patients = new List<string>();
            string cs = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT TOP 10
            a.AdmNo + ' - ' + a.MRNo + ' - ' + c.PatientName AS Patient
        FROM IPD_Admission a
        INNER JOIN EMR_Patients c ON a.MRNo = c.MRNo
        WHERE a.BillNo IS NULL
        AND (
            a.AdmNo LIKE @p 
            OR a.MRNo LIKE @p
            OR c.PatientName LIKE @p
        )
        ORDER BY c.PatientName";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@p", "%" + prefix + "%");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    patients.Add(dr["Patient"].ToString());
                }
            }

            return patients;
        }

        // LOAD PATIENT DATA WHEN SELECTED FROM AUTOCOMPLETE
        protected void btnLoadPatient_Click(object sender, EventArgs e)
        {
            try
            {
                string mrNo = hfMRNo.Value.Trim();
                System.Diagnostics.Debug.WriteLine("btnLoadPatient_Click called. MRNo: " + mrNo);

                if (string.IsNullOrEmpty(mrNo))
                {
                    pnlPatientDetails.Visible = false;
                    pnlCertificateForm.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Warning','Please select a valid patient','warning');", true);
                    return;
                }

                string query = @"
                    SELECT 
                        a.MRNo,
                        a.AdmNo,
                        c.PatientTitle + ' ' + c.PatientName + ' ' + ISNULL(c.FatherTitle, '') + ' ' + ISNULL(c.FatherName, '') AS PatientFullName,
                        DATEDIFF(YEAR, c.DoB, GETDATE()) AS Age,
                        c.Gender, 
                        c.CellNo, 
                        c.CNIC, 
                        e.ConsultantName,
                        '' AS Diagnosis
                    FROM IPD_Admission a
                    INNER JOIN EMR_Patients c ON a.MRNo = c.MRNo
                    LEFT JOIN Gen_Consultants e ON a.ConsultantCode = e.ConsultantCode
                    WHERE a.MRNo = @MRNo
                    AND a.BillNo IS NULL";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MRNo", mrNo);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        lblMRNo.Text = dr["MRNo"].ToString();
                        lblAdmNo.Text = dr["AdmNo"].ToString();
                        lblPatientName.Text = dr["PatientFullName"].ToString();
                        lblAgeGender.Text = dr["Age"].ToString() + " yrs / " + dr["Gender"].ToString();
                        lblCNIC.Text = dr["CNIC"].ToString();
                        lblContact.Text = dr["CellNo"].ToString();
                        lblConsultant.Text = dr["ConsultantName"] != DBNull.Value ? dr["ConsultantName"].ToString() : "N/A";
                        lblDiagnosis.Text = dr["Diagnosis"] != DBNull.Value ? dr["Diagnosis"].ToString() : "N/A";

                        pnlPatientDetails.Visible = true;
                        pnlCertificateForm.Visible = true;

                        // Set default values
                        txtFinalDiagnosis.Text = dr["Diagnosis"] != DBNull.Value ? dr["Diagnosis"].ToString() : "";
                        hdnCertificateID.Value = "";
                    }
                    else
                    {
                        pnlPatientDetails.Visible = false;
                        pnlCertificateForm.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Not Found','Patient not found or bill has been generated','warning');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                pnlPatientDetails.Visible = false;
                pnlCertificateForm.Visible = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error','" + ex.Message.Replace("'", "\\'") + "','error');", true);
            }
        }
    }
}