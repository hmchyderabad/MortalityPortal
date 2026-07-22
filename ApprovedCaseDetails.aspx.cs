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
    public partial class ApprovedCaseDetails : System.Web.UI.Page
    {
        string cs = ConfigurationManager
             .ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int finalApprovalId =
                        Convert.ToInt32(Request.QueryString["id"]);

                    LoadMainDetails(finalApprovalId);

                    LoadConsultants(finalApprovalId);

                    LoadServices(finalApprovalId);
                    LoadFinalAmountMainDetails(finalApprovalId);
                }
            }
        }


        void LoadMainDetails(int finalApprovalId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT 
                wf.AdmNo, 
                wf.MRNo,
                bp.PatientName,
                bp.RequestID
            FROM WelfareFinalApproval wf
            INNER JOIN Billing_Send_Patient_For_Welfare bp
                ON wf.MRNo = bp.MRNo
            WHERE FinalApprovalId = @FinalApprovalId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FinalApprovalId", finalApprovalId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblAdmNo.Text = dr["AdmNo"].ToString();
                    lblMRNo.Text = dr["MRNo"].ToString();
                    lblPatientName.Text = dr["PatientName"].ToString();

                    // RequestID ko formatted string mein convert karein
                    if (dr["RequestID"] != DBNull.Value)
                    {
                        int requestID = Convert.ToInt32(dr["RequestID"]);
                        int currentYear = DateTime.Now.Year;

                        // Format: WLF/2026/000016
                        string formattedRequestID = $"WLF/{currentYear}/{requestID.ToString("D6")}";
                        lblRequestID.Text = formattedRequestID;
                    }
                    else
                    {
                        lblRequestID.Text = "N/A";
                    }
                }

                con.Close();
            }
        }

        //    void LoadMainDetails(int finalApprovalId)
        //    {
        //        using (SqlConnection con = new SqlConnection(cs))
        //        {
        //            string query = @"
        //            SELECT 
        //wf.AdmNo, 
        //wf.MRNo,
        //bp.PatientName
        //            FROM WelfareFinalApproval wf
        //INNER JOIN Billing_Send_Patient_For_Welfare bp
        //ON wf.MRNo = bp.MRNo
        //            WHERE FinalApprovalId = @FinalApprovalId";

        //            SqlCommand cmd = new SqlCommand(query, con);

        //            cmd.Parameters.AddWithValue(
        //                "@FinalApprovalId",
        //                finalApprovalId
        //            );

        //            con.Open();

        //            SqlDataReader dr = cmd.ExecuteReader();

        //            if (dr.Read())
        //            {
        //                lblAdmNo.Text = dr["AdmNo"].ToString();

        //                lblMRNo.Text = dr["MRNo"].ToString();

        //                lblPatientName.Text = dr["PatientName"].ToString();
        //            }

        //            con.Close();
        //        }
        //    }

        void LoadConsultants(int finalApprovalId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    ConsultantName,
                    VisitFee,
                    ProcedureFee,
                    DiscountPercent,
                    DiscountAmount,
                    FinalAmount
                FROM WelfareFinalApprovalConsultants
                WHERE FinalApprovalId = @FinalApprovalId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FinalApprovalId",
                    finalApprovalId
                );

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvConsultants.DataSource = dt;

                gvConsultants.DataBind();
            }
        }

        void LoadServices(int finalApprovalId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    ServiceName,
                    Amount,
                    DiscountPercent,
                    DiscountAmount,
                    FinalAmount
                FROM WelfareFinalApprovalServices
                WHERE FinalApprovalId = @FinalApprovalId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FinalApprovalId",
                    finalApprovalId
                );

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvServices.DataSource = dt;

                gvServices.DataBind();
            }
        }

        void LoadFinalAmountMainDetails(int finalApprovalId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
        SELECT 
            AdmNo,
            MRNo,
            ReceivableAmount,
            TotalDiscountAmount,
            GrandTotal
        FROM WelfareFinalApproval
        WHERE FinalApprovalId = @FinalApprovalId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FinalApprovalId",
                    finalApprovalId
                );

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblAdmNo.Text = dr["AdmNo"].ToString();

                    lblMRNo.Text = dr["MRNo"].ToString();

                    lblReceivable.Text =
                        Convert.ToDecimal(
                            dr["ReceivableAmount"]
                        ).ToString("N0");

                    lblTotalDiscount.Text =
                        Convert.ToDecimal(
                            dr["TotalDiscountAmount"]
                        ).ToString("N0");

                    lblGrandTotal.Text =
                        Convert.ToDecimal(
                            dr["GrandTotal"]
                        ).ToString("N0");
                }

                con.Close();
            }
        }
    }
}