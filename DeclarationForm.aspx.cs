using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class DeclarationForm : System.Web.UI.Page
    {
        private string cs =>
           ConfigurationManager
           .ConnectionStrings["welfare"]
           .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text =
                    DateTime.Now.ToString("yyyy-MM-dd");

                LoadPatientData();
            }
        }

        // =========================
        // LOAD PATIENT DATA
        // =========================

        private void LoadPatientData()
        {
            if (
                string.IsNullOrEmpty(
                    Request.QueryString["RequestID"]
                )
            )
            {
                ShowAlert(
                    "RequestID Missing!",
                    "error"
                );

                return;
            }

            int requestId =
                Convert.ToInt32(
                    Request.QueryString["RequestID"]
                );

            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

                SELECT
                    WFA.FinalApprovalId,
                    WFA.RequestID,
                    WFA.AdmNo,
                    WFA.MRNo,
                    B.PatientName

                FROM WelfareFinalApproval WFA

                INNER JOIN Billing_Send_Patient_For_Welfare B
                ON WFA.RequestID = B.RequestID

                WHERE WFA.RequestID = @RequestID
                ";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@RequestID",
                    requestId
                );

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr =
                        dt.Rows[0];

                    // SHOW DATA
                    txtPatientName.Text =
                        dr["PatientName"]
                        .ToString();

                    txtRequestID.Text =
                        dr["RequestID"]
                        .ToString();

                    txtAdmNo.Text =
                        dr["AdmNo"]
                        .ToString();

                    txtMRNo.Text =
                        dr["MRNo"]
                        .ToString();

                    // HIDDEN FIELDS
                    hfRequestID.Value =
                        dr["RequestID"]
                        .ToString();

                    hfAdmNo.Value =
                        dr["AdmNo"]
                        .ToString();

                    hfMRNo.Value =
                        dr["MRNo"]
                        .ToString();

                    hfPatientName.Value =
                        dr["PatientName"]
                        .ToString();
                }
                else
                {
                    ShowAlert(
                        "Final Approval Not Found!",
                        "warning"
                    );
                }
            }
        }

        // =========================
        // SAVE DECLARATION
        // =========================

        protected void btnSubmit_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                string rawData =
                    hfSignatureData.Value;

                if (
                    string.IsNullOrEmpty(rawData)
                    || !rawData.Contains(",")
                )
                {
                    ShowAlert(
                        "Please provide a signature!",
                        "warning"
                    );

                    return;
                }

                // BASE64 TO BYTE[]
                string base64Data =
                    rawData.Substring(
                        rawData.IndexOf(",") + 1
                    );

                byte[] binarySignature =
                    Convert.FromBase64String(
                        base64Data
                    );

                using (
                    SqlConnection con =
                    new SqlConnection(cs)
                )
                {
                    string sql = @"

                    INSERT INTO PatientDeclarations
                    (
                        AttendantName,
                        Relationship,
                        SignatureData,
                        SubmissionDate,
                        RequestID,
                        AdmNo,
                        MRNo,
                        Patient_Name
                    )

                    VALUES
                    (
                        @AttendantName,
                        @Relationship,
                        @SignatureData,
                        @SubmissionDate,
                        @RequestID,
                        @AdmNo,
                        @MRNo,
                        @Patient_Name
                    )
                    ";

                    SqlCommand cmd =
                        new SqlCommand(sql, con);

                    cmd.Parameters.AddWithValue(
                        "@AttendantName",
                        txtName.Text.Trim()
                    );

                    cmd.Parameters.AddWithValue(
                        "@Relationship",
                        txtRelationship.Text.Trim()
                    );

                    cmd.Parameters.AddWithValue(
                        "@SignatureData",
                        binarySignature
                    );

                    cmd.Parameters.AddWithValue(
                        "@SubmissionDate",
                        DateTime.Now
                    );

                    cmd.Parameters.AddWithValue(
                        "@RequestID",
                        hfRequestID.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@AdmNo",
                        hfAdmNo.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@MRNo",
                        hfMRNo.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@Patient_Name",
                        txtPatientName.Text.Trim()
                    );

                    con.Open();

                    int rows =
                        cmd.ExecuteNonQuery();

                    con.Close();

                    if (rows > 0)
                    {
                        // =========================
                        // SAVE DOCUMENTS
                        // =========================

                        SaveDocument(
                            fileCNIC,
                            "CNIC Copy",
                            hfRequestID.Value,
                            hfAdmNo.Value,
                            hfMRNo.Value
                        );

                        SaveDocument(
                            fileUtility,
                            "Utility Bill",
                            hfRequestID.Value,
                            hfAdmNo.Value,
                            hfMRNo.Value
                        );

                        SaveDocument(
                            fileIncome,
                            "Income Proof",
                            hfRequestID.Value,
                            hfAdmNo.Value,
                            hfMRNo.Value
                        );

                        SaveDocument(
                            fileHardship,
                            "Financial Hardship",
                            hfRequestID.Value,
                            hfAdmNo.Value,
                            hfMRNo.Value
                        );

                        string script = @"
                            Swal.fire({
                                icon: 'success',
                                title: 'Declaration Submitted Successfully!',
                                showConfirmButton: false,
                                timer: 2000
                            }).then(function () {
                                window.location.href = 'ViewDeclaration.aspx';
                            });
                            ";

                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "successRedirect",
                            script,
                            true
                        );

                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert(
                    "Error: " +
                    ex.Message.Replace("'", ""),
                    "error"
                );
            }
        }

        // =========================
        // ALERT
        // =========================

        void ShowAlert(
            string message,
            string type
        )
        {
            string script =
                $"Swal.fire({{ icon: '{type}', title: '{message}', showConfirmButton: false, timer: 2000 }});";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "alert",
                script,
                true
            );
        }

        // =========================
        // CLEAR
        // =========================

        private void ClearFields()
        {
            txtName.Text = "";
            txtRelationship.Text = "";
            hfSignatureData.Value = "";
        }

        // =========================
        // SAVE DOCUMENT FUNCTION
        // =========================

        private void SaveDocument(
            FileUpload fileUpload,
            string documentName,
            string requestId,
            string admNo,
            string mrNo
        )
        {
            try
            {
                if (fileUpload.HasFile)
                {
                    string folderPath =
                        Server.MapPath("~/Uploads/");

                    string extension =
                        System.IO.Path.GetExtension(
                            fileUpload.FileName
                        );

                    string uniqueFileName =
                        Guid.NewGuid().ToString()
                        + extension;

                    string fullPath =
                        folderPath + uniqueFileName;

                    // SAVE FILE
                    fileUpload.SaveAs(fullPath);

                    // DATABASE PATH
                    string dbPath =
                        "~/Uploads/" + uniqueFileName;

                    using (
                        SqlConnection con =
                        new SqlConnection(cs)
                    )
                    {
                        string query = @"

                        INSERT INTO PatientDeclarationDocuments
                        (
                            RequestID,
                            AdmNo,
                            MRNo,
                            DocumentName,
                            FileName,
                            FilePath
                        )

                        VALUES
                        (
                            @RequestID,
                            @AdmNo,
                            @MRNo,
                            @DocumentName,
                            @FileName,
                            @FilePath
                        )
                        ";

                        SqlCommand cmd =
                            new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue(
                            "@RequestID",
                            requestId
                        );

                        cmd.Parameters.AddWithValue(
                            "@AdmNo",
                            admNo
                        );

                        cmd.Parameters.AddWithValue(
                            "@MRNo",
                            mrNo
                        );

                        cmd.Parameters.AddWithValue(
                            "@DocumentName",
                            documentName
                        );

                        cmd.Parameters.AddWithValue(
                            "@FileName",
                            uniqueFileName
                        );

                        cmd.Parameters.AddWithValue(
                            "@FilePath",
                            dbPath
                        );

                        con.Open();

                        cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Document Upload Error: "
                    + ex.Message
                );
            }
        }
    }
}