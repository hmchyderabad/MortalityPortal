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
    public partial class Patient_sent_from_billing : System.Web.UI.Page
    {
        private string cs => ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;
        private string css => ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatientGrid("");   // first time page open pe grid load
            }

            string eventTarget = Request["__EVENTTARGET"];
            string eventArg = Request["__EVENTARGUMENT"];

            if (eventTarget == "LoadPatient" && !string.IsNullOrEmpty(eventArg))
            {
                txtSearch.Text = eventArg;

                LoadPatientData(eventArg);
                LoadPatientGrid(eventArg);
            }
        }
        protected void btnLoadPatient_Click(object sender, EventArgs e)
        {
            string value = txtSearch.Text.Trim();

            LoadPatientData(value);
            LoadPatientGrid("");

            pnlData.Visible = true;
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == "")
            {
                pnlData.Visible = false;
                gvPatients.DataSource = null;
                gvPatients.DataBind();
                return;
            }

            LoadPatientData(txtSearch.Text.Trim());
            LoadPatientGrid(txtSearch.Text.Trim());
        }

        //[System.Web.Services.WebMethod]
        //public static List<string> GetPatients(string prefix)
        //{
        //    List<string> patients = new List<string>();

        //    string cs = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        string query = @"
        //SELECT TOP 10
        //    CONCAT(
        //        ISNULL(p.MRNo, ''), 
        //        ' - ', 
        //        ISNULL(p.PatientName, '')
        //    ) AS Patient
        //FROM EMR_Patients p
        //LEFT JOIN IPD_Admission a
        //    ON p.MRNo = a.MRNo
        //    AND a.BillNo IS NULL
        //WHERE 
        //    p.MRNo LIKE @p 
        //    OR p.PatientName LIKE @p
        //    OR p.CNIC LIKE @p
        //ORDER BY p.PatientName";

        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@p", "%" + prefix + "%");

        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            patients.Add(dr["Patient"].ToString());
        //        }
        //    }

        //    return patients;
        //}

        //void LoadPatientData(string search)
        //{
        //    // 🔥 STEP 0: Clean input (VERY IMPORTANT)
        //    string searchValue = search.Trim();

        //    // If autocomplete sends full string like:
        //    // ADM/2025/017553 - NAME
        //    if (searchValue.Contains("-"))
        //    {
        //        searchValue = searchValue.Split('-')[0].Trim();
        //    }

        //    // Check if searchValue is empty
        //    if (string.IsNullOrWhiteSpace(searchValue))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "msg", "alert('Please enter a valid Admission Number, MR Number or Patient Name.');", true);
        //        return;
        //    }

        //    // ============================================================
        //    // STEP 1: Check if this is an OPD Patient (No Admission)
        //    // ============================================================
        //    bool isOPDPatient = false;
        //    string mrNo = "";
        //    string patientName = "";

        //    using (SqlConnection conCheck = new SqlConnection(cs))
        //    {
        //        conCheck.Open();

        //        // Check if patient exists in EMR_Patients but not in IPD_Admission
        //        string checkOPDQuery = @"
        //SELECT TOP 1 
        //    p.MRNo,
        //    p.PatientName,
        //    CASE 
        //        WHEN EXISTS (
        //            SELECT 1 FROM IPD_Admission 
        //            WHERE MRNo = p.MRNo 
        //            AND BillNo IS NULL
        //        ) THEN 0 
        //        ELSE 1 
        //    END AS IsOPD
        //FROM EMR_Patients p
        //WHERE p.MRNo = @search 
        //   OR p.PatientName LIKE @searchLike
        //   OR p.CNIC = @search";

        //        SqlCommand cmdCheck = new SqlCommand(checkOPDQuery, conCheck);
        //        cmdCheck.Parameters.AddWithValue("@search", searchValue);
        //        cmdCheck.Parameters.AddWithValue("@searchLike", "%" + searchValue + "%");

        //        SqlDataReader drCheck = cmdCheck.ExecuteReader();

        //        if (drCheck.Read())
        //        {
        //            mrNo = drCheck["MRNo"].ToString();
        //            patientName = drCheck["PatientName"].ToString();
        //            isOPDPatient = Convert.ToInt32(drCheck["IsOPD"]) == 1;
        //        }
        //        drCheck.Close();
        //        conCheck.Close();
        //    }

        //    // ============================================================
        //    // STEP 2: Check Status from Welfare DB (for IPD patients only)
        //    // ============================================================
        //    if (!isOPDPatient)
        //    {
        //        using (SqlConnection con1 = new SqlConnection(css))
        //        {
        //            con1.Open();

        //            string checkQuery = @"
        //    SELECT 
        //    CASE
        //        WHEN EXISTS (
        //            SELECT 1 FROM Billing_Send_Patient_For_Welfare
        //            WHERE AdmNo=@search AND Status='Approved'
        //        ) THEN 'Approved'

        //        WHEN EXISTS (
        //            SELECT 1 FROM Billing_Send_Patient_For_Welfare
        //            WHERE AdmNo=@search AND Status='In Process'
        //        ) THEN 'In Process'

        //        WHEN EXISTS (
        //            SELECT 1 FROM Billing_Send_Patient_For_Welfare
        //            WHERE AdmNo=@search AND Status='Rejected'
        //        ) THEN 'Rejected'

        //        ELSE ''
        //    END AS Status";

        //            SqlCommand checkCmd = new SqlCommand(checkQuery, con1);
        //            checkCmd.Parameters.AddWithValue("@search", searchValue);

        //            object result = checkCmd.ExecuteScalar();

        //            if (result != null && result != DBNull.Value)
        //            {
        //                string status = result.ToString().Trim();

        //                if (!string.IsNullOrEmpty(status))
        //                {
        //                    if (status == "Approved")
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(),
        //                            "msg", "alert('This patient is already Approved by Welfare.');", true);
        //                        return;
        //                    }
        //                    else if (status == "In Process")
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(),
        //                            "msg", "alert('This patient request is already In Process.');", true);
        //                        return;
        //                    }
        //                    else if (status == "Rejected")
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(),
        //                            "msg", "alert('This patient was Rejected by Welfare.');", true);
        //                        return;
        //                    }
        //                }
        //            }

        //            con1.Close();
        //        }
        //    }

        //    // ============================================================
        //    // STEP 3: Fetch from Hospital DB (IPD or OPD)
        //    // ============================================================
        //    using (SqlConnection con2 = new SqlConnection(cs))
        //    {
        //        con2.Open();

        //        string query = "";

        //        if (isOPDPatient)
        //        {
        //            // ============================
        //            // OPD Patient Query
        //            // ============================
        //            query = @"
        //    SELECT TOP 1
        //        p.MRNo,
        //        '' AS AdmNo,
        //        GETDATE() AS AdmDate,
        //        '' AS PartyCode,
        //        '' AS PartyDescription,
        //        '' AS WardCode,
        //        '' AS WardName,
        //        '' AS ConsultantCode,
        //        '' AS ConsultantName,

        //        PatientFullName = 
        //            ISNULL(p.PatientTitle, '') + ' ' + 
        //            ISNULL(p.PatientName, '') + ' ' + 
        //            ISNULL(p.FatherTitle, '') + ' ' + 
        //            ISNULL(p.FatherName, ''),

        //        Age = DATEDIFF(YEAR, ISNULL(p.DoB, GETDATE()), GETDATE()),
        //        ISNULL(p.Gender, '') AS Gender,
        //        ISNULL(p.CellNo, '') AS CellNo,
        //        ISNULL(p.CNIC, '') AS CNIC,
        //        ISNULL(c.CityName, '') AS CityName,

        //        0 AS TotalBillAmount,
        //        0 AS Advance,
        //        0 AS ReceivableAmount

        //    FROM EMR_Patients p
        //    LEFT JOIN Global_Cities c ON p.City = c.CityCode
        //    WHERE p.MRNo = @search 
        //       OR p.PatientName LIKE @searchLike
        //       OR p.CNIC = @search";
        //        }
        //        else
        //        {
        //            // ============================
        //            // IPD Patient Query (Existing)
        //            // ============================
        //            query = @"
        //    SELECT TOP 1
        //        a.MRNo,
        //        a.AdmNo,
        //        a.AdmDate,
        //        a.PartyCode,
        //        ISNULL(e.PartyDescription, '') AS PartyDescription,
        //        a.WardCode,
        //        ISNULL(f.WardName, '') AS WardName,
        //        b.ConsultantCode,
        //        ISNULL(b.ConsultantName, '') AS ConsultantName,

        //        PatientFullName = 
        //            ISNULL(c.PatientTitle, '') + ' ' + 
        //            ISNULL(c.PatientName, '') + ' ' + 
        //            ISNULL(c.FatherTitle, '') + ' ' + 
        //            ISNULL(c.FatherName, ''),

        //        Age = DATEDIFF(YEAR, ISNULL(c.DoB, GETDATE()), GETDATE()),
        //        ISNULL(c.Gender, '') AS Gender,
        //        ISNULL(c.CellNo, '') AS CellNo,
        //        ISNULL(c.CNIC, '') AS CNIC,
        //        ISNULL(d.CityName, '') AS CityName,

        //        ISNULL(a.TotalBillAmount, 0) AS TotalBillAmount,

        //        ISNULL(dbo.fnGetDecryptDataDec(
        //            RTRIM(a.CompCode)+'001'+RTRIM(a.AdmNo),
        //            a.DepositAmount
        //        ), 0) AS Advance,

        //        (ISNULL(a.TotalBillAmount, 0) -
        //        ISNULL(dbo.fnGetDecryptDataDec(
        //            RTRIM(a.CompCode)+'001'+RTRIM(a.AdmNo),
        //            a.DepositAmount
        //        ), 0)) AS ReceivableAmount

        //    FROM IPD_Admission a
        //    INNER JOIN EMR_Patients c ON a.MRNo = c.MRNo
        //    LEFT JOIN Global_Cities d ON c.City = d.CityCode
        //    INNER JOIN Gen_Consultants b ON a.ConsultantCode = b.ConsultantCode
        //    INNER JOIN Gen_Party e ON a.PartyCode = e.PartyCode
        //    INNER JOIN Gen_WardMaster f ON a.WardCode = f.WardCode

        //    WHERE a.BillNo IS NULL
        //    AND a.AdmNo = @search";
        //        }

        //        SqlCommand cmd = new SqlCommand(query, con2);
        //        cmd.Parameters.AddWithValue("@search", searchValue);
        //        cmd.Parameters.AddWithValue("@searchLike", "%" + searchValue + "%");

        //        SqlDataReader dr = cmd.ExecuteReader();

        //        if (dr.Read())
        //        {
        //            // Safe assignment with null checks
        //            lblMR.Text = dr["MRNo"] != DBNull.Value ? dr["MRNo"].ToString() : "";
        //            lblAdmNo.Text = dr["AdmNo"] != DBNull.Value ? dr["AdmNo"].ToString() : "";

        //            // If OPD patient, show message
        //            if (isOPDPatient)
        //            {
        //                lblAdmNo.Text = "OPD Patient (No Admission)";
        //                lblAdmNo.ForeColor = System.Drawing.Color.Blue;
        //            }
        //            else
        //            {
        //                lblAdmNo.ForeColor = System.Drawing.Color.Black;
        //            }

        //            // Format date if exists
        //            if (dr["AdmDate"] != DBNull.Value)
        //            {
        //                DateTime admDate;
        //                if (DateTime.TryParse(dr["AdmDate"].ToString(), out admDate))
        //                {
        //                    lblAdmissionDate.Text = admDate.ToString("dd-MMM-yyyy");
        //                }
        //                else
        //                {
        //                    lblAdmissionDate.Text = dr["AdmDate"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                lblAdmissionDate.Text = isOPDPatient ? "OPD Visit" : "";
        //            }

        //            lblName.Text = dr["PatientFullName"] != DBNull.Value ? dr["PatientFullName"].ToString() : "";

        //            // Clean up extra spaces in name
        //            lblName.Text = System.Text.RegularExpressions.Regex.Replace(lblName.Text, @"\s+", " ").Trim();

        //            lblPartyName.Text = dr["PartyDescription"] != DBNull.Value ? dr["PartyDescription"].ToString() : (isOPDPatient ? "OPD Patient" : "");
        //            lblAge.Text = dr["Age"] != DBNull.Value ? dr["Age"].ToString() : "0";
        //            lblGender.Text = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : "";
        //            lblCNIC.Text = dr["CNIC"] != DBNull.Value ? dr["CNIC"].ToString() : "";
        //            lblMobile.Text = dr["CellNo"] != DBNull.Value ? dr["CellNo"].ToString() : "";
        //            lblCity.Text = dr["CityName"] != DBNull.Value ? dr["CityName"].ToString() : "";

        //            // Format bill amount with commas
        //            decimal billAmount = 0;
        //            if (dr["TotalBillAmount"] != DBNull.Value)
        //                decimal.TryParse(dr["TotalBillAmount"].ToString(), out billAmount);
        //            lblBill.Text = billAmount.ToString("N0");

        //            decimal advance = 0;
        //            if (dr["Advance"] != DBNull.Value)
        //                decimal.TryParse(dr["Advance"].ToString(), out advance);
        //            lblAdvance.Text = advance.ToString("N0");

        //            decimal receivable = 0;
        //            if (dr["ReceivableAmount"] != DBNull.Value)
        //                decimal.TryParse(dr["ReceivableAmount"].ToString(), out receivable);
        //            lblReceivable.Text = receivable.ToString("N0");

        //            lblWardName.Text = dr["WardName"] != DBNull.Value ? dr["WardName"].ToString() : (isOPDPatient ? "OPD" : "");
        //            lblConsultant.Text = dr["ConsultantName"] != DBNull.Value ? dr["ConsultantName"].ToString() : (isOPDPatient ? "Not Assigned" : "");

        //            // If OPD patient, hide some fields or show appropriate message
        //            if (isOPDPatient)
        //            {
        //                // You can hide/show panels based on OPD/IPD
        //                // For example:
        //                // pnlIPDDetails.Visible = false;
        //                // lblOPDMessage.Visible = true;
        //                // lblOPDMessage.Text = "This is an OPD patient. No admission details available.";
        //            }

        //            // Show panel
        //            pnlData.Visible = true;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "msg", "alert('No Patient Found in Hospital System. Please check MR Number, CNIC or Patient Name.');", true);
        //            pnlData.Visible = false;
        //        }

        //        con2.Close();
        //    }
        //}

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

        void LoadPatientData(string search)
        {
            // 🔥 STEP 0: Clean input (VERY IMPORTANT)
            string searchValue = search.Trim();

            // If autocomplete sends full string like:
            // ADM/2025/017553 - NAME
            if (searchValue.Contains("-"))
            {
                searchValue = searchValue.Split('-')[0].Trim();
            }

            // -------------------------------------------------
            // STEP 1: Check Status from Welfare DB
            // -------------------------------------------------
            using (SqlConnection con1 = new SqlConnection(css))
            {
                con1.Open();

                string checkQuery = @"
        SELECT 
        CASE
            WHEN EXISTS (
                SELECT 1 FROM Billing_Send_Patient_For_Welfare
                WHERE AdmNo=@search AND Status='Approved'
            ) THEN 'Approved'

            WHEN EXISTS (
                SELECT 1 FROM Billing_Send_Patient_For_Welfare
                WHERE AdmNo=@search AND Status='In Process'
            ) THEN 'In Process'

            WHEN EXISTS (
                SELECT 1 FROM Billing_Send_Patient_For_Welfare
                WHERE AdmNo=@search AND Status='Rejected'
            ) THEN 'Rejected'

            ELSE ''
        END";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con1);
                checkCmd.Parameters.AddWithValue("@search", searchValue);

                object result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    string status = result.ToString().Trim();

                    if (status == "Approved")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('This patient is already Approved by Welfare.');", true);
                        return;
                    }
                    else if (status == "In Process")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('This patient request is already In Process.');", true);
                        return;
                    }
                    else if (status == "Rejected")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('This patient was Rejected by Welfare.');", true);
                        return;
                    }
                }

                con1.Close();
            }

            // -------------------------------------------------
            // STEP 2: Fetch from Hospital DB
            // -------------------------------------------------
            using (SqlConnection con2 = new SqlConnection(cs))
            {
                con2.Open();

                string query = @"
        SELECT TOP 1
            a.MRNo,
            a.AdmNo,
            a.AdmDate,
            a.PartyCode,
            e.PartyDescription,
            a.WardCode,
            f.WardName,
            b.ConsultantCode,
            b.ConsultantName,

            PatientFullName =
                c.PatientTitle + ' ' + c.PatientName + ' ' +
                c.FatherTitle + ' ' + c.FatherName,

            Age = DATEDIFF(YEAR, c.DoB, GETDATE()),
            c.Gender,
            c.CellNo,
            c.CNIC,
            ISNULL(d.CityName,'') AS CityName,

            a.TotalBillAmount,

            dbo.fnGetDecryptDataDec(
                RTRIM(a.CompCode)+'001'+RTRIM(a.AdmNo),
                a.DepositAmount
            ) AS Advance,

            (a.TotalBillAmount -
            dbo.fnGetDecryptDataDec(
                RTRIM(a.CompCode)+'001'+RTRIM(a.AdmNo),
                a.DepositAmount
            )) AS ReceivableAmount

        FROM IPD_Admission a
        INNER JOIN EMR_Patients c ON a.MRNo = c.MRNo
        LEFT JOIN Global_Cities d ON c.City = d.CityCode
        INNER JOIN Gen_Consultants b ON a.ConsultantCode = b.ConsultantCode
        inner join Gen_Party e on a.PartyCode=e.PartyCode
        inner join Gen_WardMaster f on a.WardCode=f.WardCode

        WHERE a.BillNo IS NULL
        AND a.AdmNo = @search";

                SqlCommand cmd = new SqlCommand(query, con2);
                cmd.Parameters.AddWithValue("@search", searchValue);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblMR.Text = dr["MRNo"].ToString();
                    lblAdmNo.Text = dr["AdmNo"].ToString();
                    lblAdmissionDate.Text = dr["AdmDate"].ToString();
                    lblName.Text = dr["PatientFullName"].ToString();
                    lblPartyName.Text = dr["PartyDescription"].ToString();
                    lblAge.Text = dr["Age"].ToString();
                    lblGender.Text = dr["Gender"].ToString();
                    lblCNIC.Text = dr["CNIC"].ToString();
                    lblMobile.Text = dr["CellNo"].ToString();
                    lblCity.Text = dr["CityName"].ToString();
                    lblBill.Text = dr["TotalBillAmount"].ToString();
                    lblAdvance.Text = dr["Advance"].ToString();
                    lblReceivable.Text = dr["ReceivableAmount"].ToString();
                    lblWardName.Text = dr["WardName"].ToString();
                    lblConsultant.Text = dr["ConsultantName"].ToString();


                    pnlData.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "msg", "alert('No Patient Found in Hospital System');", true);
                }

                con2.Close();
            }
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            //foreach (System.Web.UI.WebControls.GridViewRow row in gvConsultants.Rows)
            //{
            //    string consultantName = row.Cells[0].Text;
            //    decimal procedureFee = Convert.ToDecimal(row.Cells[1].Text);
            //    decimal visitFee = Convert.ToDecimal(row.Cells[2].Text);
            //    decimal totalFee = Convert.ToDecimal(row.Cells[3].Text);

            using (SqlConnection con = new SqlConnection(css))
            {
                string query = @"

                            INSERT INTO Billing_Send_Patient_For_Welfare
                            (
                            MRNo,
                            AdmNo,
                            PatientName,
                            ConsultantName,
                            Age,
                            Gender,
                            CNIC,
                            Mobile,
                            City,
                            BillAmount,
                            AdvanceAmount,
                            ReceivableAmount,
                            Status,
                            AdmDate,
                            PartyDescription,
                            WardName,
                            CreatedDate
                            )

                            VALUES
                            (
                            @MRNo,
                            @AdmNo,
                            @PatientName,
                            @ConsultantName,
                            @Age,
                            @Gender,
                            @CNIC,
                            @Mobile,
                            @City,
                            @Bill,
                            @Advance,
                            @Receivable,
                            'In Process',
                            @AdmDate,
                            @PartyDescription,
                            @WardName,
                            GETDATE()
                            )
                            ";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@MRNo", lblMR.Text);
                cmd.Parameters.AddWithValue("@AdmNo", lblAdmNo.Text);
                cmd.Parameters.AddWithValue("@PatientName", lblName.Text);
                cmd.Parameters.AddWithValue("@Age", lblAge.Text);
                cmd.Parameters.AddWithValue("@Gender", lblGender.Text);
                cmd.Parameters.AddWithValue("@CNIC", lblCNIC.Text);
                cmd.Parameters.AddWithValue("@Mobile", lblMobile.Text);
                cmd.Parameters.AddWithValue("@City", lblCity.Text);
                cmd.Parameters.AddWithValue("@Bill", lblBill.Text);
                cmd.Parameters.AddWithValue("@Advance", lblAdvance.Text);
                cmd.Parameters.AddWithValue("@Receivable", lblReceivable.Text);

                cmd.Parameters.AddWithValue("@ConsultantName", lblConsultant.Text);
                cmd.Parameters.AddWithValue("@AdmDate", lblAdmissionDate.Text);
                cmd.Parameters.AddWithValue("@PartyDescription", lblPartyName.Text);
                cmd.Parameters.AddWithValue("@WardName", lblWardName.Text);
                //cmd.Parameters.AddWithValue("@ProcedureFee", procedureFee);
                //cmd.Parameters.AddWithValue("@VisitFee", visitFee);
                //cmd.Parameters.AddWithValue("@TotalFee", totalFee);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            //}

            ScriptManager.RegisterStartupScript(this, GetType(),
            "msg",
            "alert('Patient Sent To Welfare Successfully'); window.location='Patient_sent_from_billing.aspx';",
            true);
        }




        //protected void btnProceed_Click(object sender, EventArgs e)
        //{
        //    if (!chkPreAdmission.Checked && string.IsNullOrEmpty(lblAdmNo.Text))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "error",
        //            "alert('Please enter Admission Number or check Pre-Admission option.');",
        //            true);
        //        return;
        //    }

        //    using (SqlConnection con = new SqlConnection(css))
        //    {
        //        string query = @"
        //    INSERT INTO Billing_Send_Patient_For_Welfare
        //    (
        //        MRNo,
        //        AdmNo,
        //        PatientName,
        //        ConsultantName,
        //        Age,
        //        Gender,
        //        CNIC,
        //        Mobile,
        //        City,
        //        BillAmount,
        //        AdvanceAmount,
        //        ReceivableAmount,
        //        Status,
        //        AdmDate,
        //        PartyDescription,
        //        WardName,
        //        CreatedDate,
        //        IsPreAdmission
        //    )
        //    VALUES
        //    (
        //        @MRNo,
        //        @AdmNo,
        //        @PatientName,
        //        @ConsultantName,
        //        @Age,
        //        @Gender,
        //        @CNIC,
        //        @Mobile,
        //        @City,
        //        @Bill,
        //        @Advance,
        //        @Receivable,
        //        'In Process',
        //        @AdmDate,
        //        @PartyDescription,
        //        @WardName,
        //        GETDATE(),
        //        @IsPreAdmission  -- New parameter
        //    );
            
        //    SELECT SCOPE_IDENTITY();
        //";

        //        SqlCommand cmd = new SqlCommand(query, con);

        //        // Check if Pre-Admission is checked
        //        bool isPreAdmission = chkPreAdmission.Checked;
        //        cmd.Parameters.AddWithValue("@MRNo", lblMR.Text);
        //        cmd.Parameters.AddWithValue("@AdmNo", lblAdmNo.Text);
        //        cmd.Parameters.AddWithValue("@PatientName", lblName.Text);
        //        cmd.Parameters.AddWithValue("@Age", lblAge.Text);
        //        cmd.Parameters.AddWithValue("@Gender", lblGender.Text);
        //        cmd.Parameters.AddWithValue("@CNIC", lblCNIC.Text);
        //        cmd.Parameters.AddWithValue("@Mobile", lblMobile.Text);
        //        cmd.Parameters.AddWithValue("@City", lblCity.Text);
        //        cmd.Parameters.AddWithValue("@Bill", lblBill.Text);
        //        cmd.Parameters.AddWithValue("@Advance", lblAdvance.Text);
        //        cmd.Parameters.AddWithValue("@Receivable", lblReceivable.Text);
        //        cmd.Parameters.AddWithValue("@ConsultantName", lblConsultant.Text);
        //        cmd.Parameters.AddWithValue("@AdmDate", lblAdmissionDate.Text);
        //        cmd.Parameters.AddWithValue("@PartyDescription", lblPartyName.Text);
        //        cmd.Parameters.AddWithValue("@WardName", lblWardName.Text);
        //        cmd.Parameters.AddWithValue("@IsPreAdmission", isPreAdmission);  // New parameter

        //        con.Open();
        //        int newRequestID = Convert.ToInt32(cmd.ExecuteScalar());
        //        con.Close();

        //        // Formatted RequestID generate karein
        //        string formattedRequestID = $"WLF/{DateTime.Now.Year}/{newRequestID.ToString("D6")}";

        //        // Success message with Pre-Admission status
        //        string preAdmissionStatus = isPreAdmission ? " (Pre-Admission)" : "";
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "msg",
        //            $"alert('Patient Sent To Welfare Successfully! Request ID: {formattedRequestID}{preAdmissionStatus}'); window.location='Patient_sent_from_billing.aspx';",
        //            true);
        //    }
        //}




        void LoadPatientGrid(string search)
        {
            using (SqlConnection con = new SqlConnection(css))
            {
                string query = @"
            WITH cte AS (
                SELECT 
                    MRNo,
                    AdmNo,
                    PatientName,
                    Age,
                    BillAmount,
                    Status,
                    Comments,
                    IsPreAdmission,
                    CreatedDate,
                    ROW_NUMBER() OVER (PARTITION BY MRNo, AdmNo ORDER BY CreatedDate DESC) AS rn
                FROM Billing_Send_Patient_For_Welfare
                WHERE 
                    MRNo LIKE @search
                    OR AdmNo LIKE @search
                    OR PatientName LIKE @search
            )
            SELECT *
            FROM cte
            WHERE rn = 1
            ORDER BY CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPatients.DataSource = dt;
                gvPatients.DataBind();
            }
        }

        protected void gvPatients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Resend")
            {
                string admNo = e.CommandArgument.ToString();

                using (SqlConnection con = new SqlConnection(css))
                {
                    string query = @"
            UPDATE Billing_Send_Patient_For_Welfare
            SET Status='In Process',
                Comments='Resent by Billing',
                CreatedDate=GETDATE()
            WHERE AdmNo=@AdmNo";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AdmNo", admNo);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                LoadPatientGrid("");
            }
        }

        [WebMethod]
        public static object GetPatientDetail(string value)
        {
            string cs = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"select top 1
        a.MRNo, a.AdmNo,
        c.PatientTitle+' '+c.PatientName+' '+c.FatherTitle+' '+c.FatherName as PatientFullName,
        DATEDIFF(YEAR,c.DoB,GETDATE()) as Age,
        c.Gender, c.CellNo, c.CNIC, d.CityName,
        a.TotalBillAmount,
        dbo.fnGetDecryptDataDec(RTRIM(a.CompCode)+'001'+RTRIM(a.AdmNo),a.DepositAmount) as Advance,
        (a.TotalBillAmount -
        dbo.fnGetDecryptDataDec(RTRIM(a.CompCode)+'001'+RTRIM(a.AdmNo),a.DepositAmount)) as ReceivableAmount
        from IPD_Admission a
        inner join EMR_Patients c on a.MRNo=c.MRNo
        inner join Global_Cities d on c.City=d.CityCode
        where (a.MRNo + ' - ' + c.PatientName) = @val";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@val", value);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new
                    {
                        MRNo = dr["MRNo"].ToString(),
                        AdmNo = dr["AdmNo"].ToString(),
                        Name = dr["PatientFullName"].ToString(),
                        Age = dr["Age"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        CNIC = dr["CNIC"].ToString(),
                        Mobile = dr["CellNo"].ToString(),
                        City = dr["CityName"].ToString(),
                        Bill = dr["TotalBillAmount"].ToString(),
                        Advance = dr["Advance"].ToString(),
                        Receivable = dr["ReceivableAmount"].ToString()
                    };
                }

                return null;
            }
        }
    }
}