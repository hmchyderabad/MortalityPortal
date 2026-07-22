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
    public partial class DeclarationCasesList : System.Web.UI.Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e) 
        {
            if (!IsPostBack)
            {
                LoadApprovedCases();
            }
        }

        void LoadApprovedCases()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    wf.FinalApprovalId,
                    wf.RequestID,
                    wf.AdmNo,
                    wf.MRNo,
					bp.PatientName,
                    wf.DonorId,
                    wf.TotalDiscountAmount,
                    wf.ReceivableAmount,
                    wf.GrandTotal,
                    wf.ApprovedBy,
                    wf.ApprovalDate
                FROM WelfareFinalApproval wf
				INNER JOIN Billing_Send_Patient_For_Welfare bp 
				ON wf.MRNo = bp.MRNo
                ORDER BY FinalApprovalId DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvApprovedCases.DataSource = dt;
                gvApprovedCases.DataBind();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string requestId = btn.CommandArgument;

            Response.Redirect(
                "DeclarationForm.aspx?RequestID=" + requestId
            );
        }

        protected void gvApprovedCases_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApprovedCases.PageIndex = e.NewPageIndex;

            LoadApprovedCases();
        }
    }
}