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
                else
                {
                    Response.Write("<script>alert('Certificate not found'); window.close();</script>");
                }
                dr.Close();
            }
        }

        private void GenerateCertificateHTML(SqlDataReader dr)
        {
            string html = @"
            <div style='font-family: Times New Roman, serif; padding: 40px; max-width: 800px; margin: 0 auto; border: 2px solid #1a5a4c;'>
                <div style='text-align: center; border-bottom: 3px solid #1a5a4c; padding-bottom: 20px; margin-bottom: 30px;'>
                    <h1 style='color: #1a5a4c; margin: 0; font-size: 28px;'>HASHIM MEDICAL CITY</h1>
                    <p style='font-size: 14px; margin: 8px 0; color: #555;'>The Hashim Medical City Hospital, Beside Palm Enclave Society, By-Pass Hyderabad</p>
                    <h3 style='color: #dc3545; margin: 15px 0; font-size: 24px;'>DEATH CERTIFICATE</h3>
                </div>

                <table style='width: 100%; border-collapse: collapse; font-size: 14px;'>
                    <tr><td style='padding: 12px 8px; width: 40%; font-weight: bold; border-bottom: 1px solid #ddd;'>Certificate No:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["CertificateNo"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>M.R. No:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["MRNo"] + @"</td></tr>
                    <tr><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Patient Name:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["PatientName"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Age / Gender:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["Age"] + " yrs / " + dr["Gender"] + @"</td></tr>
                    <tr><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>CNIC No:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["CNIC"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Date of Death:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + Convert.ToDateTime(dr["DateOfDeath"]).ToString("dd-MMM-yyyy") + " at " + dr["TimeOfDeath"] + @"</td></tr>
                    <tr><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Cause of Death:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["CauseOfDeath"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Diagnosis:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["Diagnosis"] + @"</td></tr>
                    <tr><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Attending Consultant:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["ConsultantName"] + @"</td></tr>
                    <tr style='background: #f8f9fa;'><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Body Received By:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["HandOverName"] + " (" + dr["HandOverRelation"] + @")</td></tr>
                    <tr><td style='padding: 12px 8px; font-weight: bold; border-bottom: 1px solid #ddd;'>Receiver CNIC / Contact:</td><td style='padding: 12px 8px; border-bottom: 1px solid #ddd;'>" + dr["HandOverCNIC"] + " / " + dr["HandOverCellNo"] + @"</td></tr>
                </table>

                <div style='margin-top: 50px; padding-top: 25px; border-top: 2px solid #1a5a4c; display: flex; justify-content: space-between;'>
                    <div style='text-align: center; width: 45%;'>
                        <p style='margin: 0; height: 60px;'></p>
                        <p style='font-weight: bold; margin: 5px 0;'>Medical Officer</p>
                        <p style='font-size: 12px; margin: 0;'>Hashim Medical City</p>
                    </div>
                    <div style='text-align: center; width: 45%;'>
                        <p style='margin: 0; height: 60px;'></p>
                        <p style='font-weight: bold; margin: 5px 0;'>Medical Superintendent</p>
                        <p style='font-size: 12px; margin: 0;'>Hashim Medical City</p>
                    </div>
                </div>

                <div style='text-align: center; margin-top: 30px; font-size: 11px; color: #666; padding-top: 15px; border-top: 1px dashed #ccc;'>
                    <p style='margin: 5px 0;'>Issued on: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") + @" | Computer Generated Certificate</p>
                    <p style='margin: 5px 0;'>This is a system-generated certificate and requires no signature if digitally verified.</p>
                </div>
            </div>";

            litCertificate.Text = html;
        }
    }
}