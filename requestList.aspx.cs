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
    public partial class requestList : System.Web.UI.Page
    {
        string cs =
            ConfigurationManager
            .ConnectionStrings["HospitalDBConnection"]
            .ConnectionString;

        string css =
            ConfigurationManager
            .ConnectionStrings["welfare"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRequests();
            }
        }

        void LoadRequests()
        {
            using (SqlConnection con =
                new SqlConnection(css))
            {
                string query = @"

                SELECT 
    B.RequestID,
    B.AdmNo,
    B.MRNo,
    B.PatientName,
    B.Age,
    B.Gender,
    B.CNIC,
    B.Mobile,
    B.City,
    B.BillAmount,
    B.AdvanceAmount,
    B.ReceivableAmount,
    B.ConsultantName,
    B.VisitFee,
    B.ProcedureFee,
    B.TotalFee,
    B.DiscountPercent,
    B.FinalAmount,
    B.Status,
    B.WardName,
    B.PartyDescription,

    W.ChildrenCount,
    W.Under18Count,
    W.TotalDependents,
    W.MonthlyIncome,
    W.IncomeSource,
    W.PayerRelation,
    W.PropertyStatus,
    W.RentAmount,
    W.UtilityExpenses,
    W.GoldDetails,
    W.SilverDetails,
    W.StockDetails,
    W.CashDetails,
    W.LowIncome,
    W.ChronicIllness,
    W.BreadWinner,
    W.HighCost

FROM Billing_Send_Patient_For_Welfare B

INNER JOIN WelfareAssessment W
    ON B.RequestID = W.RequestID

WHERE B.Status = 'Approved'
ORDER BY RequestID DESC";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvRequests.DataSource = dt;

                gvRequests.DataBind();
            }
        }

        protected void gvRequests_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewRequest")
            {
                string requestId =
                    e.CommandArgument.ToString();

                Response.Redirect(
                    "finalApproval.aspx?RequestID="
                    + requestId
                );
            }
        }
    }
}