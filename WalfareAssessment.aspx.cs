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

namespace welfareSystem
{
    public partial class WalfareAssessment : System.Web.UI.Page
    {
        private int GetRequestId()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT TOP 1 RequestID FROM Billing_Send_Patient_For_Welfare WHERE MRNo = @MRNo";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MRNo", lblMRNo.Text);

                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
        private string cs => ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();

                string req = Request.QueryString["RequestID"];

                if (!int.TryParse(req, out int requestId))
                {
                    Response.Write("RequestID missing or invalid in URL");
                    return;
                }

                hdnRequestID.Value = requestId.ToString(); // ✅ IMPORTANT LINE

                LoadPatientData(requestId);
            }
        }

        private void LoadPatientData(int requestId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT 
                                AdmNo,
                                MRNo,
                                ConsultantName,
                                PatientName,
                                Gender,
                                AdmDate,
                                PartyDescription,
                                WardName,
                                City,
                                BillAmount,
                                AdvanceAmount,
                                ReceivableAmount
                            FROM Billing_Send_Patient_For_Welfare
                            WHERE RequestID=@RequestID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RequestID", requestId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblAdmNo.Text = dr["AdmNo"].ToString();
                    lblMRNo.Text = dr["MRNo"].ToString();
                    lblPatientName.Text = dr["PatientName"].ToString();
                    lblCity.Text = dr["City"].ToString();
                    lblBillAmount.Text = dr["BillAmount"].ToString();
                    lblAdvanceAmount.Text = dr["AdvanceAmount"].ToString();
                    lblReceivableAmount.Text = dr["ReceivableAmount"].ToString();
                    lblWardName.Text = dr["WardName"].ToString();
                    lblConsultant.Text = dr["ConsultantName"].ToString();
                    lblAdmissionDate.Text = dr["AdmDate"].ToString();
                    lblGender.Text = dr["Gender"].ToString();
                    lblPartyName.Text = dr["PartyDescription"].ToString();
                }

                con.Close();
            }
        }

        private void LoadPatient(string search)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT TOP 1
                        RequestID,
                        AdmNo,
                        MRNo,
                        ConsultantName,
                        PatientName,
                        Gender,
                        AdmDate,
                        PartyDescription,
                        WardName,
                        City,
                        BillAmount,
                        AdvanceAmount,
                        ReceivableAmount
                    FROM Billing_Send_Patient_For_Welfare
                    WHERE Status = 'Approved'
                    AND (
                        AdmNo LIKE '%' + @Search + '%' 
                        OR MRNo LIKE '%' + @Search + '%' 
                        OR PatientName LIKE '%' + @Search + '%'
                    )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", search);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        hdnRequestID.Value = dr["RequestID"].ToString();
                        lblAdmNo.Text = dr["AdmNo"].ToString();
                        lblMRNo.Text = dr["MRNo"].ToString();
                        lblPatientName.Text = dr["PatientName"].ToString();
                        lblCity.Text = dr["City"].ToString();
                        lblBillAmount.Text = dr["BillAmount"].ToString();
                        lblAdvanceAmount.Text = dr["AdvanceAmount"].ToString();
                        lblReceivableAmount.Text = dr["ReceivableAmount"].ToString();
                        lblWardName.Text = dr["WardName"].ToString();
                        lblConsultant.Text = dr["ConsultantName"].ToString();
                        lblAdmissionDate.Text = dr["AdmDate"].ToString();
                        lblGender.Text = dr["Gender"].ToString();
                        lblPartyName.Text = dr["PartyDescription"].ToString();
                    }
                    else
                    {
                        ClearLabels();
                    }
                }
            }
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadPatient(txtSearch.Text.Trim());
        }

        protected void btnSaveAssessment_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                int requestId;

                if (!int.TryParse(hdnRequestID.Value, out requestId))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('RequestId Missing');", true);
                }

                string query = @"INSERT INTO WelfareAssessment
        (
            RequestID,  
            DateReceived,
            ChildrenCount,Under18Count,TotalDependents,
            MonthlyIncome,IncomeSource,PayerRelation,
            PropertyStatus,RentAmount,UtilityExpenses,
            GoldAvailable,GoldDetails,
            SilverAvailable,SilverDetails,
            StockAvailable,StockDetails,
            CashAvailable,CashDetails,
            Remarks,
            CNICCopy,UtilityBills,IncomeProof,HardshipProof,
            FullSupport,FullDiscount,
            PartialSupport,PartialDiscount,
            ProfessionalDiscount,BabaFoundation,
            NotRecommended,NotRecommendedReason,LowIncome,ChronicIllness,BreadWinner,HighCost,EmpID
        )
        VALUES
        (
            @RequestID,
            @DateReceived,
            @ChildrenCount,@Under18Count,@TotalDependents,
            @MonthlyIncome,@IncomeSource,@PayerRelation,
            @PropertyStatus,@RentAmount,@UtilityExpenses,
            @GoldAvailable,@GoldDetails,
            @SilverAvailable,@SilverDetails,
            @StockAvailable,@StockDetails,
            @CashAvailable,@CashDetails,
            @Remarks,
            @CNICCopy,@UtilityBills,@IncomeProof,@HardshipProof,
            @FullSupport,@FullDiscount,
            @PartialSupport,@PartialDiscount,
            @ProfessionalDiscount,@BabaFoundation,
            @NotRecommended,@NotRecommendedReason,@LowIncome,@Chronic,@BreadWinner,@HighCost,@EmpID
        )";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@RequestID", requestId);

                cmd.Parameters.AddWithValue("@DateReceived", Convert.ToDateTime(txtDateReceived.Text));
                //cmd.Parameters.AddWithValue("@ReceivedBy", ddlReceivedBy.SelectedValue);

                cmd.Parameters.AddWithValue("@ChildrenCount", txtChildren.Text);
                cmd.Parameters.AddWithValue("@Under18Count", txtUnder18.Text);
                cmd.Parameters.AddWithValue("@TotalDependents", txtDependents.Text);

                cmd.Parameters.AddWithValue("@MonthlyIncome", txtIncome.Text);
                cmd.Parameters.AddWithValue("@IncomeSource", txtIncomeSource.Text);
                cmd.Parameters.AddWithValue("@PayerRelation", txtPayerRelation.Text);

                cmd.Parameters.AddWithValue("@PropertyStatus", ddlProperty.SelectedValue);
                cmd.Parameters.AddWithValue("@RentAmount", txtRent.Text);
                cmd.Parameters.AddWithValue("@UtilityExpenses", txtUtility.Text);

                cmd.Parameters.AddWithValue("@GoldAvailable", chkGold.Checked);
                cmd.Parameters.AddWithValue("@GoldDetails", txtGoldAmount.Text);

                cmd.Parameters.AddWithValue("@SilverAvailable", chkSilver.Checked);
                cmd.Parameters.AddWithValue("@SilverDetails", txtSilverAmount.Text);

                cmd.Parameters.AddWithValue("@StockAvailable", chkStock.Checked);
                cmd.Parameters.AddWithValue("@StockDetails", txtStockAmount.Text);

                cmd.Parameters.AddWithValue("@CashAvailable", chkCash.Checked);
                cmd.Parameters.AddWithValue("@CashDetails", txtCashAmount.Text);

                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);

                cmd.Parameters.AddWithValue("@CNICCopy", chkCNIC.Checked);
                cmd.Parameters.AddWithValue("@UtilityBills", chkUtilityBills.Checked);
                cmd.Parameters.AddWithValue("@IncomeProof", chkIncomeProof.Checked);
                cmd.Parameters.AddWithValue("@HardshipProof", chkHardship.Checked);

                cmd.Parameters.AddWithValue("@FullSupport", chkFullSupport.Checked);
                cmd.Parameters.AddWithValue("@FullDiscount", string.IsNullOrEmpty(txtFullDiscount.Text) ? 0 : Convert.ToDecimal(txtFullDiscount.Text));

                cmd.Parameters.AddWithValue("@PartialSupport", chkPartialSupport.Checked);
                cmd.Parameters.AddWithValue("@PartialDiscount", string.IsNullOrEmpty(txtPartialDiscount.Text) ? 0 : Convert.ToDecimal(txtPartialDiscount.Text));

                cmd.Parameters.AddWithValue("@ProfessionalDiscount", chkProfessional.Checked);
                cmd.Parameters.AddWithValue("@BabaFoundation", chkBabaFoundation.Checked);

                cmd.Parameters.AddWithValue("@NotRecommended", chkNotRecommended.Checked);
                cmd.Parameters.AddWithValue("@NotRecommendedReason", txtReason.Text);

                cmd.Parameters.AddWithValue("@LowIncome", chkLowIncome.Checked);
                cmd.Parameters.AddWithValue("@Chronic", chkChronic.Checked);
                cmd.Parameters.AddWithValue("@BreadWinner", chkBreadwinner.Checked);   
                cmd.Parameters.AddWithValue("@HighCost", chkHighCost.Checked);
                cmd.Parameters.AddWithValue("@EmpID", txtEmpId.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ClearForm();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Saved Successfully');", true);
                Response.Write("Hidden RequestID = " + hdnRequestID.Value);
                //Response.Write("RequestID = " + hdnRequestID.Value);
            }
        }
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM WelfareAssessment ORDER BY AssessmentID DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvAssessment.DataSource = dt;
                gvAssessment.DataBind();
            }
        }
        void ClearForm()
        {
            txtChildren.Text = "";
            txtUnder18.Text = "";
            txtDependents.Text = "";
            txtIncome.Text = "";
            txtIncomeSource.Text = "";
            txtPayerRelation.Text = "";
            txtRent.Text = "";
            txtUtility.Text = "";
            txtGoldAmount.Text = "";
            txtSilverAmount.Text = "";
            txtStockAmount.Text = "";
            txtCashAmount.Text = "";
            txtRemarks.Text = "";
            txtFullDiscount.Text = "";
            txtPartialDiscount.Text = "";
            txtReason.Text = "";

            chkGold.Checked = false;
            chkSilver.Checked = false;
            chkStock.Checked = false;
            chkCash.Checked = false;

            chkCNIC.Checked = false;
            chkUtilityBills.Checked = false;
            chkIncomeProof.Checked = false;
            chkHardship.Checked = false;

            chkLowIncome.Checked = false;
            chkChronic.Checked = false;
            chkBreadwinner.Checked = false;
            chkHighCost.Checked = false;
        }
        protected void gvAssessment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAssessment.EditIndex = e.NewEditIndex;
            LoadData();
        }
        protected void gvAssessment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAssessment.EditIndex = -1;
            LoadData();
        }
        protected void gvAssessment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvAssessment.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvAssessment.Rows[e.RowIndex];

            //string receivedBy = ((TextBox)row.Cells[1].Controls[0]).Text;
            string children = ((TextBox)row.Cells[2].Controls[0]).Text;
            string dependents = ((TextBox)row.Cells[3].Controls[0]).Text;
            string income = ((TextBox)row.Cells[4].Controls[0]).Text;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE WelfareAssessment
                        SET
                        ChildrenCount=@Children,
                        TotalDependents=@Dependents,
                        MonthlyIncome=@Income
                        WHERE AssessmentID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                //cmd.Parameters.AddWithValue("@ReceivedBy", receivedBy);
                cmd.Parameters.AddWithValue("@Children", children);
                cmd.Parameters.AddWithValue("@Dependents", dependents);
                cmd.Parameters.AddWithValue("@Income", income);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            gvAssessment.EditIndex = -1;
            LoadData();
        }
        protected void gvAssessment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvAssessment.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM WelfareAssessment WHERE AssessmentID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            LoadData();
        }

       
        private void ClearLabels()
        {
            lblAdmNo.Text = "";
            lblMRNo.Text = "";
            lblPatientName.Text = "";
            lblCity.Text = "";
            lblBillAmount.Text = "";
            lblAdvanceAmount.Text = "";
            lblReceivableAmount.Text = "";
        }

        [WebMethod]
        public static List<object> GetPatients(string prefix)
        {
            List<object> patients = new List<object>();

            string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
        SELECT TOP 10 AdmNo, MRNo, PatientName
        FROM Billing_Send_Patient_For_Welfare
        WHERE Status = 'Approved'
        AND (
            AdmNo LIKE '%' + @Search + '%' OR
            MRNo LIKE '%' + @Search + '%' OR
            PatientName LIKE '%' + @Search + '%'
        )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Search", prefix);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    patients.Add(new
                    {
                        AdmNo = dr["AdmNo"].ToString(),
                        DisplayText = dr["AdmNo"] + " - " + dr["MRNo"] + " - " + dr["PatientName"]
                    });
                }
            }

            return patients;
        }
        [WebMethod]
        public static object GetPatientDetails(string admNo)
        {
            string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
        SELECT 
            RequestID,   -- 🔥 ADD THIS
            AdmNo,
            MRNo,
            ConsultantName,
            PatientName,
            Gender,
            AdmDate,
            PartyDescription,
            WardName,
            City,
            BillAmount,
            AdvanceAmount,
            ReceivableAmount
        FROM Billing_Send_Patient_For_Welfare
        WHERE AdmNo = @AdmNo AND Status='Approved'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AdmNo", admNo);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new
                    {
                        Test = "OK",
                        RequestID = dr["RequestID"].ToString(),
                        AdmNo = dr["AdmNo"].ToString(),
                        MRNo = dr["MRNo"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        City = dr["City"].ToString(),
                        BillAmount = dr["BillAmount"].ToString(),
                        AdvanceAmount = dr["AdvanceAmount"].ToString(),
                        ReceivableAmount = dr["ReceivableAmount"].ToString(),
                        wardName = dr["WardName"].ToString(),
                        Consultant = dr["ConsultantName"].ToString(),
                        AdmissionDate = dr["AdmDate"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        PartyName = dr["PartyDescription"].ToString()
                    };
                }
            }

            return null;
        }
    }
}