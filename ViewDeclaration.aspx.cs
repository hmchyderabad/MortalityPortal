using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace welfareSystem
{
    public partial class ViewDeclaration : System.Web.UI.Page
    {
        private string cs =>
            ConfigurationManager
            .ConnectionStrings["welfare"]
            .ConnectionString;

        protected void Page_Load(
            object sender,
            EventArgs e
        )
        {
            if (!IsPostBack)
            {
                LoadDeclaration();
                LoadDocuments();
            }
        }

        // =========================
        // LOAD LATEST DECLARATION
        // =========================

        private void LoadDeclaration()
        {
            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

SELECT TOP 1
    ID,
    RequestID,
    AdmNo,
    MRNo,
    Patient_Name,
    AttendantName,
    Relationship,
    SubmissionDate

FROM PatientDeclarations

ORDER BY ID DESC
";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, con);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr =
                        dt.Rows[0];

                    // LABELS

                    lblRequestID.Text =
                        dr["RequestID"].ToString();

                    lblAdmNo.Text =
                        dr["AdmNo"].ToString();

                    lblMRNo.Text =
                        dr["MRNo"].ToString();

                    lblPatientName.Text =
                        dr["Patient_Name"].ToString();

                    lblAttendantName.Text =
                        dr["AttendantName"].ToString();

                    lblRelationship.Text =
                        dr["Relationship"].ToString();

                    lblDate.Text =
                        Convert.ToDateTime(
                            dr["SubmissionDate"]
                        ).ToString("dd-MM-yyyy");

                    // SIGNATURE IMAGE

                    string declarationId =
                        dr["ID"].ToString();

                    imgSignature.Attributes["src"] =
                    "Handler.ashx?id="
                    + declarationId;
                                }
            }
        }

        // =========================
        // LOAD DOCUMENTS
        // =========================

        private void LoadDocuments()
        {
            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

SELECT TOP 50
    DocumentId,
    RequestID,
    AdmNo,
    MRNo,
    DocumentName,
    FileName,
    FilePath,
    UploadedDate

FROM PatientDeclarationDocuments

ORDER BY DocumentId DESC
";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, con);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                gvDocuments.DataSource =
                    dt;

                gvDocuments.DataBind();
            }
        }
    }
}