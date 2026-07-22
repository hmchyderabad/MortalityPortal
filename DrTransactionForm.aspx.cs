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
    public partial class DrTransactionForm : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;
        string css = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadAllConsultants();
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                pnlData.Visible = false;
                return;
            }

            LoadPatientData(txtSearch.Text.Trim());
        }

        void LoadPatientData(string search)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                select
                a.MRNo,
                a.AdmNo,
                b.ConsultantCode,
                b.ConsultantName,
                PatientFullName = c.PatientTitle+' '+c.PatientName+' '+c.FatherTitle+' '+c.FatherName,
                Age = DATEDIFF(YEAR, c.DoB, GETDATE()),
                c.Gender,
                c.CellNo,
                c.CNIC,
                ISNULL(d.CityName,'') AS CityName,
                a.TotalBillAmount,
                dbo.fnGetDecryptDataDec(rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),a.DepositAmount) as Advance,
                (a.TotalBillAmount - dbo.fnGetDecryptDataDec(rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),a.DepositAmount)) as ReceivableAmount
                from IPD_Admission a
                inner join EMR_Patients c on a.MRNo=c.MRNo
                left join Global_Cities d on c.City=d.CityCode
                inner join Gen_Consultants b on a.ConsultantCode=b.ConsultantCode
                where a.BillNo IS NULL
                AND (
                    a.MRNo LIKE @search
                    OR a.AdmNo LIKE @search
                    OR c.PatientName LIKE @search
                )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    pnlData.Visible = true;  // 🔥 PANEL SHOW

                    // Patient Info
                    lblMR.Text = dr["MRNo"].ToString();
                    lblAdmNo.Text = dr["AdmNo"].ToString();
                    lblName.Text = dr["PatientFullName"].ToString();
                    lblAge.Text = dr["Age"].ToString();
                    lblGender.Text = dr["Gender"].ToString();
                    lblCNIC.Text = dr["CNIC"].ToString();
                    lblMobile.Text = dr["CellNo"].ToString();
                    lblCity.Text = dr["CityName"].ToString();
                    lblBill.Text = dr["TotalBillAmount"].ToString();
                    lblAdvance.Text = dr["Advance"].ToString();
                    lblReceivable.Text = dr["ReceivableAmount"].ToString();

                    // 🔥 Consultant Auto Select (SAFE)
                    string consultantCode = dr["ConsultantCode"].ToString();
                    string consultantName = dr["ConsultantName"].ToString();

                    // Hidden field me save
                    hdnConsultantName.Value = consultantName;

                    // ListBox select
                    if (ddlConsultant.Items.FindByValue(consultantCode) != null)
                    {
                        ddlConsultant.ClearSelection();
                        ddlConsultant.Items.FindByValue(consultantCode).Selected = true;
                    }

                    // 🔥 Textbox me name bhejo
                    ScriptManager.RegisterStartupScript(this, GetType(), "setConsultant",
                        "setConsultantTextbox('" + consultantName.Replace("'", "\\'") + "');", true);
                    string admNo = dr["AdmNo"].ToString();

                    // 🔥 Load Consultants for this patient
                    LoadConsultantsByAdmission(admNo);

                    // 🔥 Show all consultant details
                    LoadConsultantDetails(admNo);
                }
                else
                {
                    pnlData.Visible = false;
                    string script = @"
    var modal = document.createElement('div');
    modal.innerHTML = `
        <div id='customPopup' style='
            position:fixed;
            top:0;
            left:0;
            width:100%;
            height:100%;
            background:rgba(0,0,0,0.55);
            display:flex;
            justify-content:center;
            align-items:center;
            z-index:99999;
            font-family:Arial,sans-serif;
        '>
            <div style='
                background:#ffffff;
                width:420px;
                max-width:90%;
                border-radius:18px;
                padding:30px 25px;
                text-align:center;
                box-shadow:0 15px 35px rgba(0,0,0,0.25);
                animation:popupFade 0.3s ease;
            '>
                <div style='font-size:55px; margin-bottom:10px;'>⚠️</div>

                <h2 style='margin:0; color:#dc3545; font-size:24px;'>
                    Final Bill Generated
                </h2>

                <p style='margin-top:15px; color:#555; font-size:16px; line-height:1.6;'>
                    This patient's final bill has already been generated.
                    <br><br>
                    If you want to reopen it, please contact
                    <b>Billing Department</b>.
                </p>

                <button onclick='document.getElementById(""customPopup"").remove();'
                    style='
                        margin-top:18px;
                        background:#007bff;
                        color:white;
                        border:none;
                        padding:12px 30px;
                        border-radius:10px;
                        font-size:16px;
                        cursor:pointer;
                        font-weight:bold;
                    '>
                    OK
                </button>
            </div>
        </div>

        <style>
            @keyframes popupFade{
                from{transform:scale(0.7);opacity:0;}
                to{transform:scale(1);opacity:1;}
            }
        </style>
    `;
    document.body.appendChild(modal);
    ";

                    ScriptManager.RegisterStartupScript(this, GetType(), "popupmsg", script, true);
                }

                con.Close();
            }
        }

        protected void ddlConsultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            string admNo = lblAdmNo.Text;
            string consultantCode = ddlConsultant.SelectedValue;

            LoadConsultantFee(admNo, consultantCode);
        }

        void LoadConsultantDetails(string admNo)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"WITH A AS (
    SELECT *
    FROM IPD_Admission
    WHERE AdmNo = @AdmNo
)

-- Visit Doctors
SELECT 
    v.ConsultantCode,
    b.ConsultantName,

    ISNULL(
        v.Charges,
        ISNULL(
            dbo.fnGetDecryptDataDec(
                RTRIM(a.CompCode) + '001' + RTRIM(a.AdmNo),
                a.ConsultantVisitCharges
            ),
            0
        )
    ) AS VisitFee,

    0 AS ProcedureFee

FROM A a
JOIN IPD_ConsultantVisit v
    ON v.AdmNo = a.AdmNo

LEFT JOIN Gen_Consultants b
    ON b.ConsultantCode = v.ConsultantCode


UNION ALL


-- Procedure Doctors
SELECT 
    p.ConsultantCode,
    b.ConsultantName,
    0 AS VisitFee,
    ISNULL(p.ConsultantShareAmount,0) AS ProcedureFee

FROM A a
JOIN IPD_AdmissionProcedure p
    ON p.AdmNo = a.AdmNo

LEFT JOIN Gen_Consultants b
    ON b.ConsultantCode = p.ConsultantCode";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AdmNo", admNo);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                string result = "";
                decimal totalVisit = 0;
                decimal totalProcedure = 0;
                while (dr.Read())
                {
                    string name = dr["ConsultantName"].ToString();
                    decimal visit = Convert.ToDecimal(dr["VisitFee"]);
                    decimal proc = Convert.ToDecimal(dr["ProcedureFee"]);

                    totalVisit += visit;
                    totalProcedure += proc;

                    result += "👨‍⚕️ " + name +
                              " | Visit: " + visit +
                              " | Procedure: " + proc + "<br/>";
                }

                // 🔥 Show in label
                //lblConsultantDetails.Text = result;

                // 🔥 totals bhi set karo
                txtVisitFee.Text = totalVisit.ToString();
                txtProcedureFee.Text = totalProcedure.ToString();

                decimal total = totalVisit + totalProcedure;
                txtTotalFee.Text = total.ToString();
                txtFinalAmount.Text = total.ToString();

                con.Close();
            }
        }
        void LoadConsultantsByAdmission(string admNo)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
WITH A AS (
    SELECT * FROM IPD_Admission WHERE AdmNo=@AdmNo
)

SELECT DISTINCT ConsultantCode, ConsultantName
FROM
(
    SELECT v.ConsultantCode, b.ConsultantName
    FROM A a
    JOIN IPD_ConsultantVisit v ON v.AdmNo=a.AdmNo
    LEFT JOIN Gen_Consultants b ON b.ConsultantCode=v.ConsultantCode

    UNION

    SELECT p.ConsultantCode, b.ConsultantName
    FROM A a
    JOIN IPD_AdmissionProcedure p ON p.AdmNo=a.AdmNo
    LEFT JOIN Gen_Consultants b ON b.ConsultantCode=p.ConsultantCode
) X
ORDER BY ConsultantName";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AdmNo", admNo);

                con.Open();

                ddlConsultant.DataSource = cmd.ExecuteReader();
                ddlConsultant.DataTextField = "ConsultantName";
                ddlConsultant.DataValueField = "ConsultantCode";
                ddlConsultant.DataBind();

                con.Close();
            }
        }

        void LoadConsultantFee(string admNo, string consultantCode)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"

WITH A AS (
    SELECT *
    FROM IPD_Admission
    WHERE AdmNo = @AdmNo
)

SELECT 
    X.ConsultantCode,
    X.ConsultantName,
    SUM(X.VisitFee) AS VisitFee,
    SUM(X.ProcedureFee) AS ProcedureFee
FROM
(
    -- Visit Doctors
    SELECT 
        v.ConsultantCode,
        b.ConsultantName,

        ISNULL(
            v.Charges,
            ISNULL(
                dbo.fnGetDecryptDataDec(
                    RTRIM(a.CompCode) + '001' + RTRIM(a.AdmNo),
                    a.ConsultantVisitCharges
                ),
                0
            )
        ) AS VisitFee,

        0 AS ProcedureFee

    FROM A a
    JOIN IPD_ConsultantVisit v
        ON v.AdmNo = a.AdmNo

    LEFT JOIN Gen_Consultants b
        ON b.ConsultantCode = v.ConsultantCode


    UNION ALL


    -- Procedure Doctors
    SELECT 
        p.ConsultantCode,
        b.ConsultantName,
        0 AS VisitFee,
        ISNULL(p.ConsultantShareAmount,0) AS ProcedureFee

    FROM A a
    JOIN IPD_AdmissionProcedure p
        ON p.AdmNo = a.AdmNo

    LEFT JOIN Gen_Consultants b
        ON b.ConsultantCode = p.ConsultantCode

) X

WHERE X.ConsultantCode = @ConsultantCode
GROUP BY X.ConsultantCode, X.ConsultantName

";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@AdmNo", admNo);
                cmd.Parameters.AddWithValue("@ConsultantCode", consultantCode);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    decimal visitFee = Convert.ToDecimal(dr["VisitFee"]);
                    decimal procedureFee = Convert.ToDecimal(dr["ProcedureFee"]);

                    txtVisitFee.Text = visitFee.ToString("0.00");
                    txtProcedureFee.Text = procedureFee.ToString("0.00");

                    decimal total = visitFee + procedureFee;

                    txtTotalFee.Text = total.ToString("0.00");
                    txtFinalAmount.Text = total.ToString("0.00");
                }

                con.Close();
            }
        }

        decimal CalculateFinalAmount()
        {
            decimal total = 0;
            decimal discount = 0;

            decimal.TryParse(txtTotalFee.Text, out total);
            decimal.TryParse(txtDiscount.Text, out discount);

            decimal final = total - ((total * discount) / 100);

            txtFinalAmount.Text = final.ToString();

            return final;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal finalAmount = CalculateFinalAmount();
            using (SqlConnection con = new SqlConnection(css))
            {
                string query = @"
                    IF NOT EXISTS (
                        SELECT 1 FROM DrTransactions 
                        WHERE AdmNo = @AdmNo AND ConsultantCode = @ConsultantCode
                    )
                    BEGIN
                        INSERT INTO DrTransactions
                        (AdmNo,MRNo,ConsultantCode,ConsultantName,
                         VisitFee,ProcedureFee,TotalFee,
                         DiscountPercent,FinalAmount)

                        VALUES
                        (@AdmNo,@MRNo,@ConsultantCode,@ConsultantName,
                         @VisitFee,@ProcedureFee,@TotalFee,
                         @Discount,@FinalAmount)
                    END
                    ELSE
                    BEGIN
                        SELECT 'EXISTS'
                    END";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@AdmNo", lblAdmNo.Text);
                cmd.Parameters.AddWithValue("@MRNo", lblMR.Text);

                cmd.Parameters.AddWithValue("@ConsultantCode", ddlConsultant.SelectedValue);
                cmd.Parameters.AddWithValue("@ConsultantName", ddlConsultant.SelectedItem.Text);

                //cmd.Parameters.AddWithValue("@VisitFee", txtVisitFee.Text);
                cmd.Parameters.Add("@VisitFee", SqlDbType.Decimal).Value = Convert.ToDecimal(txtVisitFee.Text);
                cmd.Parameters.AddWithValue("@ProcedureFee", txtProcedureFee.Text);
                cmd.Parameters.AddWithValue("@TotalFee", txtTotalFee.Text);

                cmd.Parameters.AddWithValue("@Discount", txtDiscount.Text);
                cmd.Parameters.AddWithValue("@FinalAmount", finalAmount);



                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result != null && result.ToString() == "EXISTS")
                {
                    Response.Write("<script>alert('Consultant already added for this Admission')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Transaction Saved')</script>");
                }
            }
            LoadTransactions();   // Grid refresh
            ClearForm();          // Form clear
        }
        void ClearForm()
        {
            txtConsultantSearch.Value = "";
            ddlConsultant.SelectedIndex = 0;

            txtVisitFee.Text = "";
            txtProcedureFee.Text = "";
            txtTotalFee.Text = "";
            txtDiscount.Text = "";
            txtFinalAmount.Text = "";

            
        }
        void LoadTransactions()
        {
            using (SqlConnection con = new SqlConnection(css))
            {
                string query = "SELECT * FROM DrTransactions ORDER BY TransactionID DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvTransactions.DataSource = dt;
                gvTransactions.DataBind();
            }
        }
        protected void gvTransactions_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTransactions.EditIndex = e.NewEditIndex;
            LoadTransactions();
        }
        protected void gvTransactions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTransactions.EditIndex = -1;
            LoadTransactions();
        }
        protected void gvTransactions_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvTransactions.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvTransactions.Rows[e.RowIndex];

            decimal visitFee = Convert.ToDecimal(((TextBox)row.Cells[3].Controls[0]).Text);
            decimal procedureFee = Convert.ToDecimal(((TextBox)row.Cells[4].Controls[0]).Text);
            decimal discount = Convert.ToDecimal(((TextBox)row.Cells[6].Controls[0]).Text);

            decimal totalFee = visitFee + procedureFee;

            decimal finalAmount = totalFee - ((totalFee * discount) / 100);

            using (SqlConnection con = new SqlConnection(css))
            {
                string query = @"UPDATE dbo.DrTransactions SET
                        VisitFee=@VisitFee,
                        ProcedureFee=@ProcedureFee,
                        TotalFee=@TotalFee,
                        DiscountPercent=@Discount,
                        FinalAmount=@FinalAmount
                        WHERE TransactionID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@VisitFee", visitFee);
                cmd.Parameters.AddWithValue("@ProcedureFee", procedureFee);
                cmd.Parameters.AddWithValue("@TotalFee", totalFee);
                cmd.Parameters.AddWithValue("@Discount", discount);
                cmd.Parameters.AddWithValue("@FinalAmount", finalAmount);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            gvTransactions.EditIndex = -1;
            LoadTransactions();
        }
        protected void gvTransactions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvTransactions.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(css))
            {
                string query = "DELETE FROM dbo.DrTransactions WHERE TransactionID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            LoadTransactions();
        }

    }
}