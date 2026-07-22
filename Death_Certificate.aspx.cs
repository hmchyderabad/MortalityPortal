using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class Death_Certificate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlData.Visible = false;
                lblMessage.Visible = false;
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
            select top 1
	  b.AdmNo+ ' - ' + b.MRNo + ' - ' + a.PatientName AS Patient
	  from EMR_Patients  a
	  inner join IPD_Admission b ON a.MRNo = b.MRNo
	  where b.BillNo IS NULL 
	  and 
	  (
	  a.MRNo like @p
	  OR a.PatientName LIKE @p
                    )
                    ORDER BY a.PatientName";

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

        // LOAD PATIENT DATA WHEN SELECTED
        protected void btnLoadPatient_Click(object sender, EventArgs e)
        {
            try
            {
                string mrNo = hfMRNo.Value.Trim();

                if (string.IsNullOrEmpty(mrNo))
                {
                    pnlData.Visible = false;
                    ShowMessage("Please select a valid patient.", "warning");
                    return;
                }

                string cs = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

                string query = @"
                    SELECT 
                        a.MRNo,
                        a.AdmNo,
                        c.PatientTitle + ' ' + c.PatientName + ' ' + ISNULL(c.FatherTitle, '') + ' ' + ISNULL(c.FatherName, '') AS PatientFullName,
                        DATEDIFF(YEAR, c.DoB, GETDATE()) AS Age,
                        c.Gender, 
                        c.CellNo, 
                        c.CNIC, 
                        d.CityName,
                        e.ConsultantName
                    FROM IPD_Admission a
                    INNER JOIN EMR_Patients c ON a.MRNo = c.MRNo
                    INNER JOIN Global_Cities d ON c.City = d.CityCode
                    LEFT JOIN Gen_Consultants e ON a.ConsultantCode = e.ConsultantCode
                    WHERE a.MRNo = @MRNo
                    AND a.BillNo IS NULL";

                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MRNo", mrNo);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        // Set all labels
                        lblMR.Text = dr["MRNo"].ToString();
                        lblAdmNo.Text = dr["AdmNo"].ToString();
                        lblName.Text = dr["PatientFullName"].ToString();
                        lblAge.Text = dr["Age"].ToString();
                        lblGender.Text = dr["Gender"].ToString();
                        lblCNIC.Text = dr["CNIC"].ToString();
                        lblMobile.Text = dr["CellNo"].ToString();
                        lblCity.Text = dr["CityName"].ToString();
                        lblConsultant.Text = dr["ConsultantName"] != DBNull.Value ? dr["ConsultantName"].ToString() : "N/A";

                        pnlData.Visible = true;
                        lblMessage.Visible = false;
                    }
                    else
                    {
                        pnlData.Visible = false;
                        ShowMessage("Patient not found or bill has been generated.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                pnlData.Visible = false;
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        // PRINT BUTTON CLICK
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            // Your print logic here
            // Example: Response.Redirect("Print_Death_Certificate.aspx?MRNo=" + hfMRNo.Value);
            ShowMessage("Print functionality coming soon!", "info");
        }

        // CLEAR BUTTON CLICK
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            hfMRNo.Value = string.Empty;
            hfAdmNo.Value = string.Empty;
            pnlData.Visible = false;
            lblMessage.Visible = false;

            // Clear all labels
            lblMR.Text = string.Empty;
            lblAdmNo.Text = string.Empty;
            lblName.Text = string.Empty;
            lblAge.Text = string.Empty;
            lblGender.Text = string.Empty;
            lblCNIC.Text = string.Empty;
            lblMobile.Text = string.Empty;
            lblCity.Text = string.Empty;
            lblConsultant.Text = string.Empty;
        }

        // HELPER METHOD TO SHOW MESSAGES
        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;

            // Set CSS class based on message type
            if (type == "danger")
                lblMessage.CssClass = "text-danger";
            else if (type == "warning")
                lblMessage.CssClass = "text-warning";
            else if (type == "info")
                lblMessage.CssClass = "text-info";
            else
                lblMessage.CssClass = "text-success";
        }
    }
}