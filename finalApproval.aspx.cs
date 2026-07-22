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
    public partial class finalApproval : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString;

        string css = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        //void LoadData()
        //{
        //    // =========================
        //    // Check RequestID
        //    // =========================

        //    if (string.IsNullOrEmpty(Request.QueryString["RequestID"]))
        //    {
        //        Response.Write("RequestID missing!");
        //        return;
        //    }

        //    int requestId;

        //    if (!int.TryParse(Request.QueryString["RequestID"], out requestId))
        //    {
        //        Response.Write("Invalid RequestID!");
        //        return;
        //    }

        //    using (SqlConnection con = new SqlConnection(css))
        //    {
        //        string query = @"
        //    SELECT 
        //        B.RequestID,
        //        B.AdmNo,
        //        B.MRNo,
        //        B.PatientName,
        //        B.Age,
        //        B.Gender,
        //        B.City,
        //        B.BillAmount,
        //        B.AdvanceAmount,
        //        B.ReceivableAmount,
        //        B.WardName,

        //        W.TotalDependents,
        //        W.MonthlyIncome,

        //        D.ConsultantName,
        //        D.VisitFee,
        //        D.ProcedureFee,
        //        D.DiscountPercent,
        //        D.FinalAmount,

        //        H.Code AS CategoryCode,
        //        H.Category AS IncomeCategory,

        //        S.ServiceName,

        //        -- Directly get percentage from CategoryServices
        //        CS.MinPercentage,
        //        CS.MaxPercentage,

        //        -- Create aliases for PercentageFrom and PercentageTo
        //        CS.MinPercentage AS PercentageFrom,
        //        CS.MaxPercentage AS PercentageTo,

        //        -- Calculate average percentage
        //        CAST((CS.MinPercentage + CS.MaxPercentage)/2.0 
        //        AS DECIMAL(10,2)) AS FinalPercentage,

        //        DN.FirstName AS DonorName,
        //        DN.DonorId

        //    FROM Billing_Send_Patient_For_Welfare B

        //    INNER JOIN WelfareAssessment W
        //        ON B.RequestID = W.RequestID

        //    LEFT JOIN DrTransactions D
        //        ON B.AdmNo = D.AdmNo

        //    LEFT JOIN HouseholdIncomeCategory H
        //        ON W.MonthlyIncome BETWEEN H.IncomeMin AND H.IncomeMax

        //    -- Directly join with CategoryServices using Category Code
        //    LEFT JOIN CategoryServices CS
        //        ON H.Code = CS.CategoryId

        //    LEFT JOIN Services S
        //        ON CS.ServiceId = S.ServiceId
        //        AND ISNULL(S.ServiceName,'') <> ''
        //        AND S.ServiceName NOT IN
        //        (
        //            'Consultant Visit',
        //            'Procedure Other Amount',
        //            'Surgeon Fee',
        //            'Visit Fee'
        //        )

        //    -- Direct join with Donors through ApprovalLimit
        //    LEFT JOIN ApprovalLimit AL
        //        ON CAST((CS.MinPercentage + CS.MaxPercentage)/2.0 AS DECIMAL(10,2))
        //        BETWEEN AL.AmountFrom AND AL.AmountTo

        //    LEFT JOIN Donors DN
        //        ON AL.DonorId = DN.DonorId

        //    WHERE B.RequestID = @RequestID
        //";

        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@RequestID", requestId);

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            DataRow dr = dt.Rows[0];

        //            // =========================
        //            // Patient Info
        //            // =========================

        //            lblRequestID.Text = dr["RequestID"].ToString();
        //            lblAdmNo.Text = dr["AdmNo"].ToString();
        //            lblMRNo.Text = dr["MRNo"].ToString();
        //            lblPatientName.Text = dr["PatientName"].ToString();
        //            lblAge.Text = dr["Age"].ToString();
        //            lblGender.Text = dr["Gender"].ToString();
        //            lblCity.Text = dr["City"].ToString();
        //            lblWard.Text = dr["WardName"].ToString();

        //            // =========================
        //            // Financial
        //            // =========================

        //            lblBillAmount.Text = dr["BillAmount"].ToString();
        //            lblAdvanceAmount.Text = dr["AdvanceAmount"].ToString();
        //            lblReceivableAmount.Text = dr["ReceivableAmount"].ToString();

        //            lblMonthlyIncome.Text = dr["MonthlyIncome"].ToString();
        //            lblDependents.Text = dr["TotalDependents"].ToString();

        //            lblIncomeCategory.Text = dr["IncomeCategory"] != DBNull.Value ? dr["IncomeCategory"].ToString() : "N/A";

        //            // Show percentage range
        //            string percentageDisplay = "";
        //            string minPerc = dr["MinPercentage"] != DBNull.Value ? dr["MinPercentage"].ToString() : "";
        //            string maxPerc = dr["MaxPercentage"] != DBNull.Value ? dr["MaxPercentage"].ToString() : "";

        //            if (!string.IsNullOrEmpty(minPerc) && !string.IsNullOrEmpty(maxPerc))
        //            {
        //                percentageDisplay = minPerc + "% - " + maxPerc + "%";
        //            }
        //            else
        //            {
        //                percentageDisplay = "N/A";
        //            }
        //            lblPercentage.Text = percentageDisplay;

        //            lblDonor.Text = dr["DonorName"] != DBNull.Value ? dr["DonorName"].ToString() : "N/A";
        //            hfDonorId.Value = dr["DonorId"] != DBNull.Value ? dr["DonorId"].ToString() : "";

        //            // =========================
        //            // Consultants Repeater
        //            // =========================

        //            DataView dvConsultants = new DataView(dt);
        //            DataTable dtConsultants = dvConsultants.ToTable(
        //                true,
        //                "ConsultantName",
        //                "VisitFee",
        //                "ProcedureFee",
        //                "DiscountPercent",
        //                "FinalAmount"
        //            );

        //            // Remove empty consultant rows
        //            DataRow[] consultantRows = dtConsultants.Select("ConsultantName IS NOT NULL AND ConsultantName <> ''");
        //            if (consultantRows.Length > 0)
        //            {
        //                dtConsultants = consultantRows.CopyToDataTable();
        //            }
        //            else
        //            {
        //                // If no consultants, create empty table with required columns
        //                dtConsultants.Clear();
        //            }

        //            // Add DiscountAmount column if not exists
        //            if (!dtConsultants.Columns.Contains("DiscountAmount"))
        //            {
        //                dtConsultants.Columns.Add("DiscountAmount", typeof(decimal));
        //            }

        //            // Calculate discount for each consultant
        //            foreach (DataRow row in dtConsultants.Rows)
        //            {
        //                decimal visitFee = 0;
        //                decimal procedureFee = 0;
        //                decimal discountPercent = 0;

        //                // Safe parsing with null check
        //                if (row["VisitFee"] != DBNull.Value)
        //                    decimal.TryParse(row["VisitFee"].ToString(), out visitFee);

        //                if (row["ProcedureFee"] != DBNull.Value)
        //                    decimal.TryParse(row["ProcedureFee"].ToString(), out procedureFee);

        //                if (row["DiscountPercent"] != DBNull.Value)
        //                    decimal.TryParse(row["DiscountPercent"].ToString(), out discountPercent);

        //                decimal total = visitFee + procedureFee;
        //                decimal discountAmount = (total * discountPercent) / 100;
        //                row["DiscountAmount"] = discountAmount;
        //            }

        //            rptConsultants.DataSource = dtConsultants;
        //            rptConsultants.DataBind();

        //            // =========================
        //            // Services Repeater (with PercentageFrom & PercentageTo)
        //            // =========================

        //            DataView dvServices = new DataView(dt);

        //            // Now include PercentageFrom and PercentageTo columns
        //            DataTable dtServices = dvServices.ToTable(
        //                true,
        //                "ServiceName",
        //                "FinalPercentage",
        //                "PercentageFrom",
        //                "PercentageTo",
        //                "MinPercentage",
        //                "MaxPercentage"
        //            );

        //            // Remove Empty Service Names
        //            DataRow[] serviceRows = dtServices.Select("ServiceName <> '' AND ServiceName IS NOT NULL");
        //            if (serviceRows.Length > 0)
        //            {
        //                dtServices = serviceRows.CopyToDataTable();
        //            }
        //            else
        //            {
        //                // If no services, still bind with empty table
        //                rptServices.DataSource = null;
        //                rptServices.DataBind();
        //                // Continue with remaining code, don't return
        //            }

        //            // Only process services if there are any
        //            if (dtServices.Rows.Count > 0)
        //            {
        //                if (!dtServices.Columns.Contains("ServiceAmount"))
        //                {
        //                    dtServices.Columns.Add("ServiceAmount", typeof(decimal));
        //                }

        //                using (SqlConnection conHospital = new SqlConnection(cs))
        //                {
        //                    conHospital.Open();

        //                    foreach (DataRow row in dtServices.Rows)
        //                    {
        //                        string service = row["ServiceName"].ToString();
        //                        string admNo = dr["AdmNo"].ToString();
        //                        decimal amount = 0;

        //                        string serviceQuery = @"
        //                SELECT
        //                CASE
        //                    WHEN @Service = 'Lab'
        //                    THEN dbo.fnGetDecryptDataDec(
        //                        rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //                        a.LaboratoryCharges
        //                    )
        //                    WHEN @Service = 'Radiology'
        //                    THEN dbo.fnGetDecryptDataDec(
        //                        rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //                        a.RadiologyCharges
        //                    )
        //                    WHEN @Service = 'Meal'
        //                    THEN dbo.fnGetDecryptDataDec(
        //                        rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //                        a.MealCharges
        //                    )
        //                    WHEN @Service = 'Pharmacy'
        //                    THEN dbo.fnGetDecryptDataDec(
        //                        rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //                        a.PharmacyCharges
        //                    )
        //                    WHEN @Service = 'Rooms'
        //                    THEN dbo.fnGetDecryptDataDec(
        //                        rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //                        a.RoomCharges
        //                    )
        //                    WHEN @Service = 'Internal Service'
        //                    THEN dbo.fnGetDecryptDataDec(
        //                        rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //                        a.InternalServiceCharges
        //                    )
        //                    WHEN @Service = 'Anaesthetics'
        //                    THEN ISNULL(g.AnesthesiaShareAmount,0)
        //                    WHEN @Service = 'Recovery room'
        //                    THEN ISNULL(g.RecoveryRoomCharges,0)
        //                    ELSE 0
        //                END AS ServiceAmount
        //                FROM IPD_Admission a
        //                LEFT JOIN IPD_AdmissionProcedure g
        //                ON a.AdmNo = g.AdmNo
        //                WHERE a.AdmNo = @AdmNo
        //            ";

        //                        SqlCommand cmdService = new SqlCommand(serviceQuery, conHospital);
        //                        cmdService.Parameters.AddWithValue("@AdmNo", admNo);
        //                        cmdService.Parameters.AddWithValue("@Service", service);

        //                        object obj = cmdService.ExecuteScalar();
        //                        if (obj != null && obj != DBNull.Value)
        //                        {
        //                            decimal.TryParse(obj.ToString(), out amount);
        //                        }
        //                        row["ServiceAmount"] = amount;
        //                    }
        //                }

        //                rptServices.DataSource = dtServices;
        //                rptServices.DataBind();
        //            }

        //            // =========================
        //            // Total Discount Calculation
        //            // =========================

        //            decimal totalServiceDiscount = 0;
        //            if (dtServices.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in dtServices.Rows)
        //                {
        //                    decimal amount = 0;
        //                    decimal percentage = 0;

        //                    if (row["ServiceAmount"] != DBNull.Value)
        //                        decimal.TryParse(row["ServiceAmount"].ToString(), out amount);

        //                    if (row["FinalPercentage"] != DBNull.Value)
        //                        decimal.TryParse(row["FinalPercentage"].ToString(), out percentage);

        //                    decimal discount = (amount * percentage) / 100;
        //                    totalServiceDiscount += discount;
        //                }
        //            }

        //            decimal consultantDiscountTotal = 0;
        //            if (dtConsultants.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in dtConsultants.Rows)
        //                {
        //                    decimal consultantDiscount = 0;
        //                    if (row["DiscountAmount"] != DBNull.Value)
        //                        decimal.TryParse(row["DiscountAmount"].ToString(), out consultantDiscount);
        //                    consultantDiscountTotal += consultantDiscount;
        //                }
        //            }

        //            decimal finalTotalDiscount = totalServiceDiscount + consultantDiscountTotal;
        //            decimal receivable = 0;
        //            if (dr["ReceivableAmount"] != DBNull.Value)
        //                decimal.TryParse(dr["ReceivableAmount"].ToString(), out receivable);

        //            decimal grandTotal = receivable - finalTotalDiscount;

        //            lblTotalDiscount.Text = finalTotalDiscount.ToString("N0");
        //            lblReceivable.Text = receivable.ToString("N0");
        //            lblGrandTotal.Text = grandTotal.ToString("N0");
        //        }
        //        else
        //        {
        //            Response.Write("No Record Found!");
        //        }
        //    }
        //}


        //void LoadData()
        //{
        //    // =========================
        //    // Check RequestID
        //    // =========================

        //    if (string.IsNullOrEmpty(Request.QueryString["RequestID"]))
        //    {
        //        Response.Write("RequestID missing!");
        //        return;
        //    }

        //    int requestId;

        //    if (!int.TryParse(Request.QueryString["RequestID"], out requestId))
        //    {
        //        Response.Write("Invalid RequestID!");
        //        return;
        //    }

        //    using (SqlConnection con = new SqlConnection(css))
        //    {
        //        string query = @"

        //SELECT 
        //    B.RequestID,
        //    B.AdmNo,
        //    B.MRNo,
        //    B.PatientName,
        //    B.Age,
        //    B.Gender,
        //    B.City,
        //    B.BillAmount,
        //    B.AdvanceAmount,
        //    B.ReceivableAmount,
        //    B.WardName,

        //    W.TotalDependents,
        //    W.MonthlyIncome,

        //    D.ConsultantName,
        //    D.VisitFee,
        //    D.ProcedureFee,
        //    D.DiscountPercent,
        //    D.FinalAmount,

        //    H.Category AS IncomeCategory,

        //    S.ServiceName,

        //    C.PercentageFrom,
        //    C.PercentageTo,

        //    CAST((C.PercentageFrom + C.PercentageTo)/2.0 
        //    AS DECIMAL(10,2)) AS FinalPercentage,

        //    DN.FirstName AS DonorName,
        //    DN.DonorId

        //FROM Billing_Send_Patient_For_Welfare B

        //INNER JOIN WelfareAssessment W
        //ON B.RequestID = W.RequestID

        //INNER JOIN DrTransactions D
        //ON B.AdmNo = D.AdmNo

        //LEFT JOIN HouseholdIncomeCategory H
        //ON W.MonthlyIncome BETWEEN H.IncomeMin AND H.IncomeMax

        //LEFT JOIN CategoryServices CS
        //ON H.Code = CS.CategoryId

        //LEFT JOIN Services S
        //ON CS.ServiceId = S.ServiceId
        //AND ISNULL(S.ServiceName,'') <> ''
        //AND S.ServiceName NOT IN
        //(
        //    'Consultant Visit',
        //    'Procedure Other Amount',
        //    'Surgeon Fee',
        //    'Visit Fee'
        //)

        //LEFT JOIN ChartOfRemission C
        //ON H.Category = C.Qualifier
        //AND B.ReceivableAmount BETWEEN C.BillFrom AND C.BillTo
        //AND W.TotalDependents BETWEEN C.DependantFrom 
        //AND C.DependantTo

        //LEFT JOIN ApprovalLimit AL
        //ON CAST(
        //    (C.PercentageFrom + C.PercentageTo)/2.0
        //    AS DECIMAL(10,2)
        //)
        //BETWEEN AL.AmountFrom AND AL.AmountTo

        //LEFT JOIN Donors DN
        //ON AL.DonorId = DN.DonorId

        //WHERE B.RequestID = @RequestID
        //";

        //        SqlCommand cmd = new SqlCommand(query, con);

        //        // SAFE PARAMETER
        //        cmd.Parameters.AddWithValue("@RequestID", requestId);

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        DataTable dt = new DataTable();

        //        da.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            DataRow dr = dt.Rows[0];

        //            // =========================
        //            // Patient Info
        //            // =========================

        //            lblRequestID.Text = dr["RequestID"].ToString();
        //            lblAdmNo.Text = dr["AdmNo"].ToString();
        //            lblMRNo.Text = dr["MRNo"].ToString();
        //            lblPatientName.Text = dr["PatientName"].ToString();
        //            lblAge.Text = dr["Age"].ToString();
        //            lblGender.Text = dr["Gender"].ToString();
        //            lblCity.Text = dr["City"].ToString();
        //            lblWard.Text = dr["WardName"].ToString();

        //            // =========================
        //            // Financial
        //            // =========================

        //            lblBillAmount.Text = dr["BillAmount"].ToString();
        //            lblAdvanceAmount.Text = dr["AdvanceAmount"].ToString();
        //            lblReceivableAmount.Text = dr["ReceivableAmount"].ToString();

        //            lblMonthlyIncome.Text = dr["MonthlyIncome"].ToString();
        //            lblDependents.Text = dr["TotalDependents"].ToString();

        //            lblIncomeCategory.Text = dr["IncomeCategory"].ToString();
        //            lblPercentage.Text = dr["FinalPercentage"].ToString() + "%";

        //            lblDonor.Text = dr["DonorName"].ToString();

        //            hfDonorId.Value = dr["DonorId"].ToString();

        //            // =========================
        //            // Consultants Repeater
        //            // =========================

        //            DataView dvConsultants = new DataView(dt);

        //            DataTable dtConsultants =
        //                dvConsultants.ToTable(
        //                    true,
        //                    "ConsultantName",
        //                    "VisitFee",
        //                    "ProcedureFee",
        //                    "DiscountPercent",
        //                    "FinalAmount"
        //                );

        //            // Add Discount Column
        //            if (!dtConsultants.Columns.Contains("DiscountAmount"))
        //            {
        //                dtConsultants.Columns.Add(
        //                    "DiscountAmount",
        //                    typeof(decimal)
        //                );
        //            }

        //            foreach (DataRow row in dtConsultants.Rows)
        //            {
        //                decimal visitFee = 0;
        //                decimal procedureFee = 0;
        //                decimal discountPercent = 0;

        //                decimal.TryParse(
        //                    row["VisitFee"].ToString(),
        //                    out visitFee
        //                );

        //                decimal.TryParse(
        //                    row["ProcedureFee"].ToString(),
        //                    out procedureFee
        //                );

        //                decimal.TryParse(
        //                    row["DiscountPercent"].ToString(),
        //                    out discountPercent
        //                );

        //                decimal total = visitFee + procedureFee;

        //                decimal discountAmount =
        //                    (total * discountPercent) / 100;

        //                row["DiscountAmount"] = discountAmount;
        //            }

        //            rptConsultants.DataSource = dtConsultants;
        //            rptConsultants.DataBind();

        //            // =========================
        //            // Services Repeater
        //            // =========================

        //            DataView dvServices = new DataView(dt);

        //            DataTable dtServices =
        //                dvServices.ToTable(
        //                    true,
        //                    "ServiceName",
        //                    "FinalPercentage",
        //                    "PercentageFrom",
        //                    "PercentageTo"
        //                );

        //            // Remove Empty Service Names
        //            DataRow[] rows =
        //                dtServices.Select("ServiceName <> ''");

        //            if (rows.Length > 0)
        //            {
        //                dtServices = rows.CopyToDataTable();
        //            }

        //            if (!dtServices.Columns.Contains("ServiceAmount"))
        //            {
        //                dtServices.Columns.Add(
        //                    "ServiceAmount",
        //                    typeof(decimal)
        //                );
        //            }

        //            using (SqlConnection conHospital =
        //                new SqlConnection(cs))
        //            {
        //                conHospital.Open();

        //                foreach (DataRow row in dtServices.Rows)
        //                {
        //                    string service =
        //                        row["ServiceName"].ToString();

        //                    string admNo =
        //                        dr["AdmNo"].ToString();

        //                    decimal amount = 0;

        //                    string serviceQuery = @"

        //SELECT
        //CASE

        //WHEN @Service = 'Lab'
        //THEN dbo.fnGetDecryptDataDec(
        //rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //a.LaboratoryCharges
        //)

        //WHEN @Service = 'Radiology'
        //THEN dbo.fnGetDecryptDataDec(
        //rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //a.RadiologyCharges
        //)

        //WHEN @Service = 'Meal'
        //THEN dbo.fnGetDecryptDataDec(
        //rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //a.MealCharges
        //)

        //WHEN @Service = 'Pharmacy'
        //THEN dbo.fnGetDecryptDataDec(
        //rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //a.PharmacyCharges
        //)

        //WHEN @Service = 'Rooms'
        //THEN dbo.fnGetDecryptDataDec(
        //rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //a.RoomCharges
        //)

        //WHEN @Service = 'Internal Service'
        //THEN dbo.fnGetDecryptDataDec(
        //rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
        //a.InternalServiceCharges
        //)

        //WHEN @Service = 'Anaesthetics'
        //THEN ISNULL(g.AnesthesiaShareAmount,0)

        //WHEN @Service = 'Recovery room'
        //THEN ISNULL(g.RecoveryRoomCharges,0)

        //ELSE 0

        //END AS ServiceAmount

        //FROM IPD_Admission a

        //LEFT JOIN IPD_AdmissionProcedure g
        //ON a.AdmNo = g.AdmNo

        //WHERE a.AdmNo = @AdmNo
        //";

        //                    SqlCommand cmdService =
        //                        new SqlCommand(serviceQuery, conHospital);

        //                    cmdService.Parameters.AddWithValue(
        //                        "@AdmNo",
        //                        admNo
        //                    );

        //                    cmdService.Parameters.AddWithValue(
        //                        "@Service",
        //                        service
        //                    );

        //                    object obj =
        //                        cmdService.ExecuteScalar();

        //                    if (obj != null &&
        //                        obj != DBNull.Value)
        //                    {
        //                        decimal.TryParse(
        //                            obj.ToString(),
        //                            out amount
        //                        );
        //                    }

        //                    row["ServiceAmount"] = amount;
        //                }
        //            }

        //            rptServices.DataSource = dtServices;
        //            rptServices.DataBind();

        //            // =========================
        //            // Total Discount
        //            // =========================

        //            decimal totalServiceDiscount = 0;

        //            foreach (DataRow row in dtServices.Rows)
        //            {
        //                decimal amount = 0;
        //                decimal percentage = 0;

        //                decimal.TryParse(
        //                    row["ServiceAmount"].ToString(),
        //                    out amount
        //                );

        //                decimal.TryParse(
        //                    row["FinalPercentage"].ToString(),
        //                    out percentage
        //                );

        //                decimal discount =
        //                    (amount * percentage) / 100;

        //                totalServiceDiscount += discount;
        //            }

        //            decimal consultantDiscountTotal = 0;

        //            foreach (DataRow row in dtConsultants.Rows)
        //            {
        //                decimal consultantDiscount = 0;

        //                decimal.TryParse(
        //                    row["DiscountAmount"].ToString(),
        //                    out consultantDiscount
        //                );

        //                consultantDiscountTotal += consultantDiscount;
        //            }

        //            decimal finalTotalDiscount =
        //                totalServiceDiscount +
        //                consultantDiscountTotal;

        //            decimal receivable = 0;

        //            decimal.TryParse(
        //                dr["ReceivableAmount"].ToString(),
        //                out receivable
        //            );

        //            decimal grandTotal =
        //                receivable - finalTotalDiscount;

        //            lblTotalDiscount.Text =
        //                finalTotalDiscount.ToString("N0");

        //            lblReceivable.Text =
        //                receivable.ToString("N0");

        //            lblGrandTotal.Text =
        //                grandTotal.ToString("N0");
        //        }
        //        else
        //        {
        //            Response.Write("No Record Found!");
        //        }
        //    }
        //}




        void LoadData()
        {
            // =========================
            // Check RequestID
            // =========================

            if (string.IsNullOrEmpty(Request.QueryString["RequestID"]))
            {
                Response.Write("RequestID missing!");
                return;
            }

            int requestId;

            if (!int.TryParse(Request.QueryString["RequestID"], out requestId))
            {
                Response.Write("Invalid RequestID!");
                return;
            }

            using (SqlConnection con = new SqlConnection(css))
            {
                string query = @"
            SELECT 
                B.RequestID,
                B.AdmNo,
                B.MRNo,
                B.PatientName,
                B.Age,
                B.Gender,
                B.City,
                B.BillAmount,
                B.AdvanceAmount,
                B.ReceivableAmount,
                B.WardName,

                W.TotalDependents,
                W.MonthlyIncome,

                D.ConsultantName,
                D.VisitFee,
                D.ProcedureFee,
                D.DiscountPercent,
                D.FinalAmount,

                H.Code AS CategoryCode,
                H.Category AS IncomeCategory,

                S.ServiceName,

                -- Directly get percentage from CategoryServices
                CS.MinPercentage,
                CS.MaxPercentage,

                -- Create aliases for PercentageFrom and PercentageTo
                CS.MinPercentage AS PercentageFrom,
                CS.MaxPercentage AS PercentageTo,

                -- Calculate average percentage
                CAST((CS.MinPercentage + CS.MaxPercentage)/2.0 
                AS DECIMAL(10,2)) AS FinalPercentage,

                DN.FirstName AS DonorName,
                DN.DonorId

            FROM Billing_Send_Patient_For_Welfare B

            INNER JOIN WelfareAssessment W
                ON B.RequestID = W.RequestID

            INNER JOIN DrTransactions D
                ON B.AdmNo = D.AdmNo

            LEFT JOIN HouseholdIncomeCategory H
                ON W.MonthlyIncome BETWEEN H.IncomeMin AND H.IncomeMax

            -- Directly join with CategoryServices using Category Code
            LEFT JOIN CategoryServices CS
                ON H.Code = CS.CategoryId

            LEFT JOIN Services S
                ON CS.ServiceId = S.ServiceId
                AND ISNULL(S.ServiceName,'') <> ''
                AND S.ServiceName NOT IN
                (
                    'Consultant Visit',
                    'Procedure Other Amount',
                    'Surgeon Fee',
                    'Visit Fee'
                )

            -- Direct join with Donors through ApprovalLimit
            LEFT JOIN ApprovalLimit AL
                ON CAST((CS.MinPercentage + CS.MaxPercentage)/2.0 AS DECIMAL(10,2))
                BETWEEN AL.AmountFrom AND AL.AmountTo

            LEFT JOIN Donors DN
                ON AL.DonorId = DN.DonorId

            WHERE B.RequestID = @RequestID
        ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RequestID", requestId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    // =========================
                    // Patient Info
                    // =========================

                    lblRequestID.Text = dr["RequestID"].ToString();
                    lblAdmNo.Text = dr["AdmNo"].ToString();
                    lblMRNo.Text = dr["MRNo"].ToString();
                    lblPatientName.Text = dr["PatientName"].ToString();
                    lblAge.Text = dr["Age"].ToString();
                    lblGender.Text = dr["Gender"].ToString();
                    lblCity.Text = dr["City"].ToString();
                    lblWard.Text = dr["WardName"].ToString();

                    // =========================
                    // Financial
                    // =========================

                    lblBillAmount.Text = dr["BillAmount"].ToString();
                    lblAdvanceAmount.Text = dr["AdvanceAmount"].ToString();
                    lblReceivableAmount.Text = dr["ReceivableAmount"].ToString();

                    lblMonthlyIncome.Text = dr["MonthlyIncome"].ToString();
                    lblDependents.Text = dr["TotalDependents"].ToString();

                    lblIncomeCategory.Text = dr["IncomeCategory"].ToString();

                    // Show percentage range
                    string percentageDisplay = "";
                    if (!string.IsNullOrEmpty(dr["MinPercentage"].ToString()) &&
                        !string.IsNullOrEmpty(dr["MaxPercentage"].ToString()))
                    {
                        percentageDisplay = dr["MinPercentage"].ToString() + "% - " +
                                           dr["MaxPercentage"].ToString() + "%";
                    }
                    else
                    {
                        percentageDisplay = "N/A";
                    }
                    lblPercentage.Text = percentageDisplay;

                    lblDonor.Text = dr["DonorName"].ToString();
                    hfDonorId.Value = dr["DonorId"].ToString();

                    // =========================
                    // Consultants Repeater
                    // =========================

                    DataView dvConsultants = new DataView(dt);
                    DataTable dtConsultants = dvConsultants.ToTable(
                        true,
                        "ConsultantName",
                        "VisitFee",
                        "ProcedureFee",
                        "DiscountPercent",
                        "FinalAmount"
                    );

                    if (!dtConsultants.Columns.Contains("DiscountAmount"))
                    {
                        dtConsultants.Columns.Add("DiscountAmount", typeof(decimal));
                    }

                    foreach (DataRow row in dtConsultants.Rows)
                    {
                        decimal visitFee = 0;
                        decimal procedureFee = 0;
                        decimal discountPercent = 0;

                        decimal.TryParse(row["VisitFee"].ToString(), out visitFee);
                        decimal.TryParse(row["ProcedureFee"].ToString(), out procedureFee);
                        decimal.TryParse(row["DiscountPercent"].ToString(), out discountPercent);

                        decimal total = visitFee + procedureFee;
                        decimal discountAmount = (total * discountPercent) / 100;
                        row["DiscountAmount"] = discountAmount;
                    }

                    rptConsultants.DataSource = dtConsultants;
                    rptConsultants.DataBind();

                    // =========================
                    // Services Repeater (with PercentageFrom & PercentageTo)
                    // =========================

                    DataView dvServices = new DataView(dt);

                    // Now include PercentageFrom and PercentageTo columns
                    DataTable dtServices = dvServices.ToTable(
                        true,
                        "ServiceName",
                        "FinalPercentage",
                        "PercentageFrom",  // Added this
                        "PercentageTo",    // Added this
                        "MinPercentage",
                        "MaxPercentage"
                    );

                    // Remove Empty Service Names
                    DataRow[] rows = dtServices.Select("ServiceName <> '' AND ServiceName IS NOT NULL");
                    if (rows.Length > 0)
                    {
                        dtServices = rows.CopyToDataTable();
                    }
                    else
                    {
                        rptServices.DataSource = null;
                        rptServices.DataBind();
                        return;
                    }

                    if (!dtServices.Columns.Contains("ServiceAmount"))
                    {
                        dtServices.Columns.Add("ServiceAmount", typeof(decimal));
                    }

                    using (SqlConnection conHospital = new SqlConnection(cs))
                    {
                        conHospital.Open();

                        foreach (DataRow row in dtServices.Rows)
                        {
                            string service = row["ServiceName"].ToString();
                            string admNo = dr["AdmNo"].ToString();
                            decimal amount = 0;

                            string serviceQuery = @"
                        SELECT
                        CASE
                            WHEN @Service = 'Lab'
                            THEN dbo.fnGetDecryptDataDec(
                                rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
                                a.LaboratoryCharges
                            )
                            WHEN @Service = 'Radiology'
                            THEN dbo.fnGetDecryptDataDec(
                                rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
                                a.RadiologyCharges
                            )
                            WHEN @Service = 'Meal'
                            THEN dbo.fnGetDecryptDataDec(
                                rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
                                a.MealCharges
                            )
                            WHEN @Service = 'Pharmacy'
                            THEN dbo.fnGetDecryptDataDec(
                                rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
                                a.PharmacyCharges
                            )
                            WHEN @Service = 'Rooms'
                            THEN dbo.fnGetDecryptDataDec(
                                rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
                                a.RoomCharges
                            )
                            WHEN @Service = 'Internal Service'
                            THEN dbo.fnGetDecryptDataDec(
                                rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
                                a.InternalServiceCharges
                            )
                            WHEN @Service = 'Anaesthetics'
                            THEN ISNULL(g.AnesthesiaShareAmount,0)
                            WHEN @Service = 'Recovery room'
                            THEN ISNULL(g.RecoveryRoomCharges,0)
                            ELSE 0
                        END AS ServiceAmount
                        FROM IPD_Admission a
                        LEFT JOIN IPD_AdmissionProcedure g
                        ON a.AdmNo = g.AdmNo
                        WHERE a.AdmNo = @AdmNo
                    ";

                            SqlCommand cmdService = new SqlCommand(serviceQuery, conHospital);
                            cmdService.Parameters.AddWithValue("@AdmNo", admNo);
                            cmdService.Parameters.AddWithValue("@Service", service);

                            object obj = cmdService.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value)
                            {
                                decimal.TryParse(obj.ToString(), out amount);
                            }
                            row["ServiceAmount"] = amount;
                        }
                    }

                    rptServices.DataSource = dtServices;
                    rptServices.DataBind();

                    // =========================
                    // Total Discount Calculation
                    // =========================

                    decimal totalServiceDiscount = 0;
                    foreach (DataRow row in dtServices.Rows)
                    {
                        decimal amount = 0;
                        decimal percentage = 0;

                        decimal.TryParse(row["ServiceAmount"].ToString(), out amount);
                        decimal.TryParse(row["FinalPercentage"].ToString(), out percentage);

                        decimal discount = (amount * percentage) / 100;
                        totalServiceDiscount += discount;
                    }

                    decimal consultantDiscountTotal = 0;
                    foreach (DataRow row in dtConsultants.Rows)
                    {
                        decimal consultantDiscount = 0;
                        decimal.TryParse(row["DiscountAmount"].ToString(), out consultantDiscount);
                        consultantDiscountTotal += consultantDiscount;
                    }

                    decimal finalTotalDiscount = totalServiceDiscount + consultantDiscountTotal;
                    decimal receivable = 0;
                    decimal.TryParse(dr["ReceivableAmount"].ToString(), out receivable);
                    decimal grandTotal = receivable - finalTotalDiscount;

                    lblTotalDiscount.Text = finalTotalDiscount.ToString("N0");
                    lblReceivable.Text = receivable.ToString("N0");
                    lblGrandTotal.Text = grandTotal.ToString("N0");
                }
                else
                {
                    Response.Write("No Record Found!");
                }
            }
        }
        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlService =
                    (DropDownList)e.Item.FindControl("ddlService");

                DropDownList ddlPercentage =
                    (DropDownList)e.Item.FindControl("ddlPercentage");

                TextBox txtCustomPercentage =
                    (TextBox)e.Item.FindControl("txtCustomPercentage");

                TextBox txtAmount =
                    (TextBox)e.Item.FindControl("txtAmount");

                TextBox txtDiscount =
                    (TextBox)e.Item.FindControl("txtDiscount");

                TextBox txtFinalAmount =
                    (TextBox)e.Item.FindControl("txtFinalAmount");

                DataRowView drv = (DataRowView)e.Item.DataItem;

                // Service
                ddlService.Items.Clear();

                ddlService.Items.Add(
                    new ListItem(
                        drv["ServiceName"].ToString(),
                        drv["ServiceName"].ToString()
                    )
                );

                // Percentage
                ddlPercentage.Items.Clear();

                int from = 0;
                int to = 0;

                if (drv["PercentageFrom"] != DBNull.Value)
                {
                    from = Convert.ToInt32(drv["PercentageFrom"]);
                }

                if (drv["PercentageTo"] != DBNull.Value)
                {
                    to = Convert.ToInt32(drv["PercentageTo"]);
                }

                for (int i = from; i <= to; i++)
                {
                    ddlPercentage.Items.Add(
                        new ListItem(i + "%", i.ToString())
                    );
                }

                string finalPer = "0";

                if (drv["FinalPercentage"] != DBNull.Value)
                {
                    finalPer = Convert.ToInt32(
                        drv["FinalPercentage"]
                    ).ToString();
                }

                ddlPercentage.SelectedValue = finalPer;

                txtCustomPercentage.Text = finalPer;

                // Amount Calculation
                decimal amount =
                    Convert.ToDecimal(drv["ServiceAmount"]);

                decimal percentage =
                    Convert.ToDecimal(finalPer);

                decimal discount =
                    (amount * percentage) / 100;

                decimal finalAmount =
                    amount - discount;

                txtAmount.Text = amount.ToString("N0");

                txtDiscount.Text = discount.ToString("N0");

                txtFinalAmount.Text = finalAmount.ToString("N0");
            }
        }



        //protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item ||
        //        e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        DropDownList ddlService =
        //            (DropDownList)e.Item.FindControl("ddlService");

        //        DropDownList ddlPercentage =
        //            (DropDownList)e.Item.FindControl("ddlPercentage");

        //        TextBox txtCustomPercentage =
        //            (TextBox)e.Item.FindControl("txtCustomPercentage");

        //        TextBox txtAmount =
        //            (TextBox)e.Item.FindControl("txtAmount");

        //        TextBox txtDiscount =
        //            (TextBox)e.Item.FindControl("txtDiscount");

        //        TextBox txtFinalAmount =
        //            (TextBox)e.Item.FindControl("txtFinalAmount");

        //        DataRowView drv = (DataRowView)e.Item.DataItem;

        //        // Service
        //        ddlService.Items.Clear();

        //        ddlService.Items.Add(
        //            new ListItem(
        //                drv["ServiceName"].ToString(),
        //                drv["ServiceName"].ToString()
        //            )
        //        );

        //        // Percentage
        //        ddlPercentage.Items.Clear();

        //        int from = 0;
        //        int to = 0;

        //        if (drv["PercentageFrom"] != DBNull.Value)
        //        {
        //            from = Convert.ToInt32(drv["PercentageFrom"]);
        //        }

        //        if (drv["PercentageTo"] != DBNull.Value)
        //        {
        //            to = Convert.ToInt32(drv["PercentageTo"]);
        //        }

        //        for (int i = from; i <= to; i++)
        //        {
        //            ddlPercentage.Items.Add(
        //                new ListItem(i + "%", i.ToString())
        //            );
        //        }

        //        string finalPer = "0";

        //        if (drv["FinalPercentage"] != DBNull.Value)
        //        {
        //            finalPer = Convert.ToInt32(
        //                drv["FinalPercentage"]
        //            ).ToString();
        //        }

        //        ddlPercentage.SelectedValue = finalPer;

        //        txtCustomPercentage.Text = finalPer;

        //        // Amount Calculation
        //        decimal amount =
        //            Convert.ToDecimal(drv["ServiceAmount"]);

        //        decimal percentage =
        //            Convert.ToDecimal(finalPer);

        //        decimal discount =
        //            (amount * percentage) / 100;

        //        decimal finalAmount =
        //            amount - discount;

        //        txtAmount.Text = amount.ToString("N0");

        //        txtDiscount.Text = discount.ToString("N0");

        //        txtFinalAmount.Text = finalAmount.ToString("N0");
        //    }
        //}


        //protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item ||
        //        e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        DropDownList ddlService =
        //            (DropDownList)e.Item.FindControl("ddlService");

        //        DropDownList ddlPercentage =
        //            (DropDownList)e.Item.FindControl("ddlPercentage");

        //        TextBox txtCustomPercentage =
        //            (TextBox)e.Item.FindControl("txtCustomPercentage");

        //        TextBox txtAmount =
        //            (TextBox)e.Item.FindControl("txtAmount");

        //        TextBox txtDiscount =
        //            (TextBox)e.Item.FindControl("txtDiscount");

        //        TextBox txtFinalAmount =
        //            (TextBox)e.Item.FindControl("txtFinalAmount");

        //        DataRowView drv = (DataRowView)e.Item.DataItem;

        //        // Service
        //        ddlService.Items.Clear();
        //        ddlService.Items.Add(
        //            new ListItem(
        //                drv["ServiceName"].ToString(),
        //                drv["ServiceName"].ToString()
        //            )
        //        );

        //        // Percentage - Now with proper null checking
        //        ddlPercentage.Items.Clear();

        //        int from = 0;
        //        int to = 0;

        //        // Safe null checking for PercentageFrom
        //        if (drv.DataView.Table.Columns.Contains("PercentageFrom") &&
        //            drv["PercentageFrom"] != DBNull.Value &&
        //            drv["PercentageFrom"] != null)
        //        {
        //            int.TryParse(drv["PercentageFrom"].ToString(), out from);
        //        }

        //        // Safe null checking for PercentageTo
        //        if (drv.DataView.Table.Columns.Contains("PercentageTo") &&
        //            drv["PercentageTo"] != DBNull.Value &&
        //            drv["PercentageTo"] != null)
        //        {
        //            int.TryParse(drv["PercentageTo"].ToString(), out to);
        //        }

        //        // Add percentage range to dropdown
        //        if (from > 0 && to > 0 && from <= to)
        //        {
        //            for (int i = from; i <= to; i++)
        //            {
        //                ddlPercentage.Items.Add(
        //                    new ListItem(i + "%", i.ToString())
        //                );
        //            }
        //        }
        //        else
        //        {
        //            // Default range if no percentage found
        //            for (int i = 0; i <= 100; i += 5)
        //            {
        //                ddlPercentage.Items.Add(
        //                    new ListItem(i + "%", i.ToString())
        //                );
        //            }
        //        }

        //        // Set selected value from FinalPercentage
        //        string finalPer = "0";
        //        if (drv.DataView.Table.Columns.Contains("FinalPercentage") &&
        //            drv["FinalPercentage"] != DBNull.Value &&
        //            drv["FinalPercentage"] != null)
        //        {
        //            decimal tempPer = 0;
        //            decimal.TryParse(drv["FinalPercentage"].ToString(), out tempPer);
        //            finalPer = Convert.ToInt32(tempPer).ToString();
        //        }

        //        // Try to select the value in dropdown
        //        if (ddlPercentage.Items.FindByValue(finalPer) != null)
        //        {
        //            ddlPercentage.SelectedValue = finalPer;
        //        }
        //        else
        //        {
        //            // If exact value not found, select closest
        //            int selectedIndex = 0;
        //            for (int i = 0; i < ddlPercentage.Items.Count; i++)
        //            {
        //                int itemValue = Convert.ToInt32(ddlPercentage.Items[i].Value);
        //                int targetValue = Convert.ToInt32(finalPer);
        //                if (itemValue >= targetValue)
        //                {
        //                    selectedIndex = i;
        //                    break;
        //                }
        //            }
        //            ddlPercentage.SelectedIndex = selectedIndex;
        //        }

        //        txtCustomPercentage.Text = finalPer;

        //        // Amount Calculation
        //        decimal amount = 0;
        //        decimal percentage = Convert.ToDecimal(finalPer);

        //        if (drv.DataView.Table.Columns.Contains("ServiceAmount") &&
        //            drv["ServiceAmount"] != DBNull.Value &&
        //            drv["ServiceAmount"] != null)
        //        {
        //            decimal.TryParse(drv["ServiceAmount"].ToString(), out amount);
        //        }

        //        decimal discount = (amount * percentage) / 100;
        //        decimal finalAmount = amount - discount;

        //        txtAmount.Text = amount.ToString("N0");
        //        txtDiscount.Text = discount.ToString("N0");
        //        txtFinalAmount.Text = finalAmount.ToString("N0");
        //    }
        //}
        protected void ddlPercentage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl =
                (DropDownList)sender;

            RepeaterItem item =
                (RepeaterItem)ddl.NamingContainer;

            TextBox txtPer =
                (TextBox)item.FindControl("txtCustomPercentage");

            txtPer.Text = ddl.SelectedValue;

            CalculateAmounts(item);
        }

        protected void txtCustomPercentage_TextChanged(object sender, EventArgs e)
        {
            TextBox txt =
                (TextBox)sender;

            RepeaterItem item =
                (RepeaterItem)txt.NamingContainer;

            CalculateAmounts(item);
        }

        private void CalculateAmounts(RepeaterItem item)
        {
            TextBox txtAmount =
                (TextBox)item.FindControl("txtAmount");

            TextBox txtPer =
                (TextBox)item.FindControl("txtCustomPercentage");

            TextBox txtDiscount =
                (TextBox)item.FindControl("txtDiscount");

            TextBox txtFinal =
                (TextBox)item.FindControl("txtFinalAmount");

            decimal amount = 0;

            decimal percentage = 0;

            decimal.TryParse(txtAmount.Text.Replace(",", ""), out amount);

            decimal.TryParse(txtPer.Text, out percentage);

            decimal discount =
                (amount * percentage) / 100;

            decimal final =
                amount - discount;

            txtDiscount.Text = discount.ToString("N0");

            txtFinal.Text = final.ToString("N0");
        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    using (SqlConnection con = new SqlConnection(css))
        //    {
        //        con.Open();

        //        SqlTransaction tran = con.BeginTransaction();

        //        try
        //        {
        //            // =========================
        //            // CALCULATE FROM REPEATER (NOT LABEL)
        //            // =========================
        //            decimal totalServiceDiscount = 0;

        //            foreach (RepeaterItem item in rptServices.Items)
        //            {
        //                TextBox txtDiscount = (TextBox)item.FindControl("txtDiscount");

        //                decimal discount = 0;

        //                if (txtDiscount != null && !string.IsNullOrWhiteSpace(txtDiscount.Text))
        //                {
        //                    decimal.TryParse(txtDiscount.Text.Replace(",", ""), out discount);
        //                }

        //                totalServiceDiscount += discount;
        //            }

        //            decimal totalConsultantDiscount = 0;

        //            foreach (RepeaterItem item in rptConsultants.Items)
        //            {
        //                Literal ltDiscountAmount = (Literal)item.FindControl("ltDiscountAmount");

        //                decimal discount = 0;

        //                if (ltDiscountAmount != null && !string.IsNullOrWhiteSpace(ltDiscountAmount.Text))
        //                {
        //                    decimal.TryParse(ltDiscountAmount.Text.Replace(",", ""), out discount);
        //                }

        //                totalConsultantDiscount += discount;
        //            }

        //            decimal totalDiscount = totalServiceDiscount + totalConsultantDiscount;

        //            decimal receivable = 0;

        //            if (!string.IsNullOrWhiteSpace(lblReceivable.Text))
        //            {
        //                decimal.TryParse(lblReceivable.Text.Replace(",", ""), out receivable);
        //            }

        //            decimal grandTotal = receivable - totalDiscount;

        //            int donorId = 0;
        //            if (!string.IsNullOrWhiteSpace(hfDonorId.Value))
        //            {
        //                int.TryParse(hfDonorId.Value, out donorId);
        //            }

        //            // =========================
        //            // SAVE MASTER
        //            // =========================

        //            string masterQuery = @"
        //    INSERT INTO WelfareFinalApproval
        //    (
        //        RequestID,
        //        AdmNo,
        //        MRNo,
        //        DonorId,
        //        TotalDiscountAmount,
        //        ReceivableAmount,
        //        GrandTotal,
        //        ApprovedBy
        //    )
        //    VALUES
        //    (
        //        @RequestID,
        //        @AdmNo,
        //        @MRNo,
        //        @DonorId,
        //        @TotalDiscount,
        //        @Receivable,
        //        @GrandTotal,
        //        @ApprovedBy
        //    )

        //    SELECT SCOPE_IDENTITY()
        //    ";

        //            SqlCommand cmdMaster = new SqlCommand(masterQuery, con, tran);

        //            cmdMaster.Parameters.AddWithValue("@RequestID", lblRequestID.Text ?? "");
        //            cmdMaster.Parameters.AddWithValue("@AdmNo", lblAdmNo.Text ?? "");
        //            cmdMaster.Parameters.AddWithValue("@MRNo", lblMRNo.Text ?? "");
        //            cmdMaster.Parameters.AddWithValue("@DonorId", donorId);
        //            cmdMaster.Parameters.AddWithValue("@TotalDiscount", totalDiscount);
        //            cmdMaster.Parameters.AddWithValue("@Receivable", receivable);
        //            cmdMaster.Parameters.AddWithValue("@GrandTotal", grandTotal);
        //            cmdMaster.Parameters.AddWithValue("@ApprovedBy", "Mehran");

        //            int finalApprovalId = Convert.ToInt32(cmdMaster.ExecuteScalar());

        //            // =========================
        //            // SAVE SERVICES
        //            // =========================

        //            foreach (RepeaterItem item in rptServices.Items)
        //            {
        //                DropDownList ddlService = (DropDownList)item.FindControl("ddlService");
        //                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
        //                TextBox txtPer = (TextBox)item.FindControl("txtCustomPercentage");
        //                TextBox txtDiscount = (TextBox)item.FindControl("txtDiscount");
        //                TextBox txtFinal = (TextBox)item.FindControl("txtFinalAmount");

        //                // Safe conversion for all values
        //                decimal amount = 0;
        //                if (txtAmount != null && !string.IsNullOrWhiteSpace(txtAmount.Text))
        //                {
        //                    decimal.TryParse(txtAmount.Text.Replace(",", ""), out amount);
        //                }

        //                decimal discountPercent = 0;
        //                if (txtPer != null && !string.IsNullOrWhiteSpace(txtPer.Text))
        //                {
        //                    decimal.TryParse(txtPer.Text.Replace("%", ""), out discountPercent);
        //                }

        //                decimal discountAmount = 0;
        //                if (txtDiscount != null && !string.IsNullOrWhiteSpace(txtDiscount.Text))
        //                {
        //                    decimal.TryParse(txtDiscount.Text.Replace(",", ""), out discountAmount);
        //                }

        //                decimal finalAmount = 0;
        //                if (txtFinal != null && !string.IsNullOrWhiteSpace(txtFinal.Text))
        //                {
        //                    decimal.TryParse(txtFinal.Text.Replace(",", ""), out finalAmount);
        //                }

        //                string detailQuery = @"
        //        INSERT INTO WelfareFinalApprovalServices
        //        (
        //            FinalApprovalId,
        //            ServiceName,
        //            Amount,
        //            DiscountPercent,
        //            DiscountAmount,
        //            FinalAmount
        //        )
        //        VALUES
        //        (
        //            @FinalApprovalId,
        //            @ServiceName,
        //            @Amount,
        //            @DiscountPercent,
        //            @DiscountAmount,
        //            @FinalAmount
        //        )";

        //                SqlCommand cmdDetail = new SqlCommand(detailQuery, con, tran);

        //                cmdDetail.Parameters.AddWithValue("@FinalApprovalId", finalApprovalId);
        //                cmdDetail.Parameters.AddWithValue("@ServiceName", ddlService != null ? ddlService.SelectedValue : "");
        //                cmdDetail.Parameters.AddWithValue("@Amount", amount);
        //                cmdDetail.Parameters.AddWithValue("@DiscountPercent", discountPercent);
        //                cmdDetail.Parameters.AddWithValue("@DiscountAmount", discountAmount);
        //                cmdDetail.Parameters.AddWithValue("@FinalAmount", finalAmount);

        //                cmdDetail.ExecuteNonQuery();
        //            }

        //            // =========================
        //            // SAVE CONSULTANTS
        //            // =========================

        //            foreach (RepeaterItem item in rptConsultants.Items)
        //            {
        //                Literal ltConsultantName = (Literal)item.FindControl("ltConsultantName");
        //                Literal ltVisitFee = (Literal)item.FindControl("ltVisitFee");
        //                Literal ltProcedureFee = (Literal)item.FindControl("ltProcedureFee");
        //                Literal ltDiscountPercent = (Literal)item.FindControl("ltDiscountPercent");
        //                Literal ltDiscountAmount = (Literal)item.FindControl("ltDiscountAmount");
        //                Literal ltFinalAmount = (Literal)item.FindControl("ltFinalAmount");

        //                // Safe conversion for all values
        //                decimal visitFee = 0;
        //                if (ltVisitFee != null && !string.IsNullOrWhiteSpace(ltVisitFee.Text))
        //                {
        //                    decimal.TryParse(ltVisitFee.Text.Replace(",", ""), out visitFee);
        //                }

        //                decimal procedureFee = 0;
        //                if (ltProcedureFee != null && !string.IsNullOrWhiteSpace(ltProcedureFee.Text))
        //                {
        //                    decimal.TryParse(ltProcedureFee.Text.Replace(",", ""), out procedureFee);
        //                }

        //                decimal discountPercent = 0;
        //                if (ltDiscountPercent != null && !string.IsNullOrWhiteSpace(ltDiscountPercent.Text))
        //                {
        //                    decimal.TryParse(ltDiscountPercent.Text.Replace("%", ""), out discountPercent);
        //                }

        //                decimal discountAmount = 0;
        //                if (ltDiscountAmount != null && !string.IsNullOrWhiteSpace(ltDiscountAmount.Text))
        //                {
        //                    decimal.TryParse(ltDiscountAmount.Text.Replace(",", ""), out discountAmount);
        //                }

        //                decimal finalAmount = 0;
        //                if (ltFinalAmount != null && !string.IsNullOrWhiteSpace(ltFinalAmount.Text))
        //                {
        //                    decimal.TryParse(ltFinalAmount.Text.Replace(",", ""), out finalAmount);
        //                }

        //                string consultantQuery = @"
        //        INSERT INTO WelfareFinalApprovalConsultants
        //        (
        //            FinalApprovalId,
        //            ConsultantName,
        //            VisitFee,
        //            ProcedureFee,
        //            DiscountPercent,
        //            DiscountAmount,
        //            FinalAmount
        //        )
        //        VALUES
        //        (
        //            @FinalApprovalId,
        //            @ConsultantName,
        //            @VisitFee,
        //            @ProcedureFee,
        //            @DiscountPercent,
        //            @DiscountAmount,
        //            @FinalAmount
        //        )";

        //                SqlCommand cmdConsultant = new SqlCommand(consultantQuery, con, tran);

        //                cmdConsultant.Parameters.AddWithValue("@FinalApprovalId", finalApprovalId);
        //                cmdConsultant.Parameters.AddWithValue("@ConsultantName", ltConsultantName != null ? ltConsultantName.Text : "");
        //                cmdConsultant.Parameters.AddWithValue("@VisitFee", visitFee);
        //                cmdConsultant.Parameters.AddWithValue("@ProcedureFee", procedureFee);
        //                cmdConsultant.Parameters.AddWithValue("@DiscountPercent", discountPercent);
        //                cmdConsultant.Parameters.AddWithValue("@DiscountAmount", discountAmount);
        //                cmdConsultant.Parameters.AddWithValue("@FinalAmount", finalAmount);

        //                cmdConsultant.ExecuteNonQuery();
        //            }

        //            // =========================
        //            // DONOR BALANCE CHECK (Commented - Uncomment if needed)
        //            // =========================

        //            // string donorBalanceQuery = @"
        //            // SELECT ISNULL(SUM(Amount),0)
        //            // FROM Donations
        //            // WHERE DonorId=@DonorId";

        //            // SqlCommand cmdBalance = new SqlCommand(donorBalanceQuery, con, tran);
        //            // cmdBalance.Parameters.AddWithValue("@DonorId", donorId);

        //            // decimal donorBalance = Convert.ToDecimal(cmdBalance.ExecuteScalar());

        //            // if (donorBalance < totalDiscount)
        //            // {
        //            //     tran.Rollback();
        //            //     ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Insufficient donor balance');", true);
        //            //     return;
        //            // }

        //            // // =========================
        //            // // UPDATE DONOR
        //            // // =========================

        //            // string donorQuery = @"
        //            // UPDATE Donations
        //            // SET Amount = Amount - @TotalDiscount
        //            // WHERE DonorId=@DonorId";

        //            // SqlCommand cmdDonor = new SqlCommand(donorQuery, con, tran);
        //            // cmdDonor.Parameters.AddWithValue("@TotalDiscount", totalDiscount);
        //            // cmdDonor.Parameters.AddWithValue("@DonorId", donorId);
        //            // cmdDonor.ExecuteNonQuery();

        //            tran.Commit();

        //            ScriptManager.RegisterStartupScript(
        //                this,
        //                GetType(),
        //                "msg",
        //                "alert('Final Approval Saved Successfully');" +
        //                "window.location='DeclarationForm.aspx?RequestID="
        //                + lblRequestID.Text + "';",
        //                true
        //            );
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();

        //            ScriptManager.RegisterStartupScript(
        //                this,
        //                GetType(),
        //                "msg",
        //                "alert('" +
        //                ex.Message.Replace("'", "") +
        //                "');",
        //                true
        //            );
        //        }
        //    }
        //}

        //Save Data


//        void LoadData()
//        {
//            // =========================
//            // Check RequestID
//            // =========================

//            if (string.IsNullOrEmpty(Request.QueryString["RequestID"]))
//            {
//                Response.Write("RequestID missing!");
//                return;
//            }

//            int requestId;

//            if (!int.TryParse(Request.QueryString["RequestID"], out requestId))
//            {
//                Response.Write("Invalid RequestID!");
//                return;
//            }

//            using (SqlConnection con = new SqlConnection(css))
//            {
//                string query = @"

//SELECT 
//    B.RequestID,
//    B.AdmNo,
//    B.MRNo,
//    B.PatientName,
//    B.Age,
//    B.Gender,
//    B.City,
//    B.BillAmount,
//    B.AdvanceAmount,
//    B.ReceivableAmount,
//    B.WardName,

//    W.TotalDependents,
//    W.MonthlyIncome,

//    D.ConsultantName,
//    D.VisitFee,
//    D.ProcedureFee,
//    D.DiscountPercent,
//    D.FinalAmount,

//    H.Category AS IncomeCategory,

//    S.ServiceName,

//    C.PercentageFrom,
//    C.PercentageTo,

//    CAST((C.PercentageFrom + C.PercentageTo)/2.0 
//    AS DECIMAL(10,2)) AS FinalPercentage,

//    DN.FirstName AS DonorName,
//    DN.DonorId

//FROM Billing_Send_Patient_For_Welfare B

//INNER JOIN WelfareAssessment W
//ON B.RequestID = W.RequestID

//INNER JOIN DrTransactions D
//ON B.AdmNo = D.AdmNo

//LEFT JOIN HouseholdIncomeCategory H
//ON W.MonthlyIncome BETWEEN H.IncomeMin AND H.IncomeMax

//LEFT JOIN CategoryServices CS
//ON H.Code = CS.CategoryId

//LEFT JOIN Services S
//ON CS.ServiceId = S.ServiceId
//AND ISNULL(S.ServiceName,'') <> ''
//AND S.ServiceName NOT IN
//(
//    'Consultant Visit',
//    'Procedure Other Amount',
//    'Surgeon Fee',
//    'Visit Fee'
//)

//LEFT JOIN ChartOfRemission C
//ON H.Category = C.Qualifier
//AND B.ReceivableAmount BETWEEN C.BillFrom AND C.BillTo
//AND W.TotalDependents BETWEEN C.DependantFrom 
//AND C.DependantTo

//LEFT JOIN ApprovalLimit AL
//ON CAST(
//    (C.PercentageFrom + C.PercentageTo)/2.0
//    AS DECIMAL(10,2)
//)
//BETWEEN AL.AmountFrom AND AL.AmountTo

//LEFT JOIN Donors DN
//ON AL.DonorId = DN.DonorId

//WHERE B.RequestID = @RequestID
//";

//                SqlCommand cmd = new SqlCommand(query, con);

//                // SAFE PARAMETER
//                cmd.Parameters.AddWithValue("@RequestID", requestId);

//                SqlDataAdapter da = new SqlDataAdapter(cmd);

//                DataTable dt = new DataTable();

//                da.Fill(dt);

//                if (dt.Rows.Count > 0)
//                {
//                    DataRow dr = dt.Rows[0];

//                    // =========================
//                    // Patient Info
//                    // =========================

//                    lblRequestID.Text = dr["RequestID"].ToString();
//                    lblAdmNo.Text = dr["AdmNo"].ToString();
//                    lblMRNo.Text = dr["MRNo"].ToString();
//                    lblPatientName.Text = dr["PatientName"].ToString();
//                    lblAge.Text = dr["Age"].ToString();
//                    lblGender.Text = dr["Gender"].ToString();
//                    lblCity.Text = dr["City"].ToString();
//                    lblWard.Text = dr["WardName"].ToString();

//                    // =========================
//                    // Financial
//                    // =========================

//                    lblBillAmount.Text = dr["BillAmount"].ToString();
//                    lblAdvanceAmount.Text = dr["AdvanceAmount"].ToString();
//                    lblReceivableAmount.Text = dr["ReceivableAmount"].ToString();

//                    lblMonthlyIncome.Text = dr["MonthlyIncome"].ToString();
//                    lblDependents.Text = dr["TotalDependents"].ToString();

//                    lblIncomeCategory.Text = dr["IncomeCategory"].ToString();
//                    lblPercentage.Text = dr["FinalPercentage"].ToString() + "%";

//                    lblDonor.Text = dr["DonorName"].ToString();

//                    hfDonorId.Value = dr["DonorId"].ToString();

//                    // =========================
//                    // Consultants Repeater
//                    // =========================

//                    DataView dvConsultants = new DataView(dt);

//                    DataTable dtConsultants =
//                        dvConsultants.ToTable(
//                            true,
//                            "ConsultantName",
//                            "VisitFee",
//                            "ProcedureFee",
//                            "DiscountPercent",
//                            "FinalAmount"
//                        );

//                    // Add Discount Column
//                    if (!dtConsultants.Columns.Contains("DiscountAmount"))
//                    {
//                        dtConsultants.Columns.Add(
//                            "DiscountAmount",
//                            typeof(decimal)
//                        );
//                    }

//                    foreach (DataRow row in dtConsultants.Rows)
//                    {
//                        decimal visitFee = 0;
//                        decimal procedureFee = 0;
//                        decimal discountPercent = 0;

//                        decimal.TryParse(
//                            row["VisitFee"].ToString(),
//                            out visitFee
//                        );

//                        decimal.TryParse(
//                            row["ProcedureFee"].ToString(),
//                            out procedureFee
//                        );

//                        decimal.TryParse(
//                            row["DiscountPercent"].ToString(),
//                            out discountPercent
//                        );

//                        decimal total = visitFee + procedureFee;

//                        decimal discountAmount =
//                            (total * discountPercent) / 100;

//                        row["DiscountAmount"] = discountAmount;
//                    }

//                    rptConsultants.DataSource = dtConsultants;
//                    rptConsultants.DataBind();

//                    // =========================
//                    // Services Repeater
//                    // =========================

//                    DataView dvServices = new DataView(dt);

//                    DataTable dtServices =
//                        dvServices.ToTable(
//                            true,
//                            "ServiceName",
//                            "FinalPercentage",
//                            "PercentageFrom",
//                            "PercentageTo"
//                        );

//                    // Remove Empty Service Names
//                    DataRow[] rows =
//                        dtServices.Select("ServiceName <> ''");

//                    if (rows.Length > 0)
//                    {
//                        dtServices = rows.CopyToDataTable();
//                    }

//                    if (!dtServices.Columns.Contains("ServiceAmount"))
//                    {
//                        dtServices.Columns.Add(
//                            "ServiceAmount",
//                            typeof(decimal)
//                        );
//                    }

//                    using (SqlConnection conHospital =
//                        new SqlConnection(cs))
//                    {
//                        conHospital.Open();

//                        foreach (DataRow row in dtServices.Rows)
//                        {
//                            string service =
//                                row["ServiceName"].ToString();

//                            string admNo =
//                                dr["AdmNo"].ToString();

//                            decimal amount = 0;

//                            string serviceQuery = @"

//SELECT
//CASE

//WHEN @Service = 'Lab'
//THEN dbo.fnGetDecryptDataDec(
//rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
//a.LaboratoryCharges
//)

//WHEN @Service = 'Radiology'
//THEN dbo.fnGetDecryptDataDec(
//rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
//a.RadiologyCharges
//)

//WHEN @Service = 'Meal'
//THEN dbo.fnGetDecryptDataDec(
//rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
//a.MealCharges
//)

//WHEN @Service = 'Pharmacy'
//THEN dbo.fnGetDecryptDataDec(
//rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
//a.PharmacyCharges
//)

//WHEN @Service = 'Rooms'
//THEN dbo.fnGetDecryptDataDec(
//rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
//a.RoomCharges
//)

//WHEN @Service = 'Internal Service'
//THEN dbo.fnGetDecryptDataDec(
//rtrim(a.CompCode)+'001'+rtrim(a.AdmNo),
//a.InternalServiceCharges
//)

//WHEN @Service = 'Anaesthetics'
//THEN ISNULL(g.AnesthesiaShareAmount,0)

//WHEN @Service = 'Recovery room'
//THEN ISNULL(g.RecoveryRoomCharges,0)

//ELSE 0

//END AS ServiceAmount

//FROM IPD_Admission a

//LEFT JOIN IPD_AdmissionProcedure g
//ON a.AdmNo = g.AdmNo

//WHERE a.AdmNo = @AdmNo
//";

//                            SqlCommand cmdService =
//                                new SqlCommand(serviceQuery, conHospital);

//                            cmdService.Parameters.AddWithValue(
//                                "@AdmNo",
//                                admNo
//                            );

//                            cmdService.Parameters.AddWithValue(
//                                "@Service",
//                                service
//                            );

//                            object obj =
//                                cmdService.ExecuteScalar();

//                            if (obj != null &&
//                                obj != DBNull.Value)
//                            {
//                                decimal.TryParse(
//                                    obj.ToString(),
//                                    out amount
//                                );
//                            }

//                            row["ServiceAmount"] = amount;
//                        }
//                    }

//                    rptServices.DataSource = dtServices;
//                    rptServices.DataBind();

//                    // =========================
//                    // Total Discount
//                    // =========================

//                    decimal totalServiceDiscount = 0;

//                    foreach (DataRow row in dtServices.Rows)
//                    {
//                        decimal amount = 0;
//                        decimal percentage = 0;

//                        decimal.TryParse(
//                            row["ServiceAmount"].ToString(),
//                            out amount
//                        );

//                        decimal.TryParse(
//                            row["FinalPercentage"].ToString(),
//                            out percentage
//                        );

//                        decimal discount =
//                            (amount * percentage) / 100;

//                        totalServiceDiscount += discount;
//                    }

//                    decimal consultantDiscountTotal = 0;

//                    foreach (DataRow row in dtConsultants.Rows)
//                    {
//                        decimal consultantDiscount = 0;

//                        decimal.TryParse(
//                            row["DiscountAmount"].ToString(),
//                            out consultantDiscount
//                        );

//                        consultantDiscountTotal += consultantDiscount;
//                    }

//                    decimal finalTotalDiscount =
//                        totalServiceDiscount +
//                        consultantDiscountTotal;

//                    decimal receivable = 0;

//                    decimal.TryParse(
//                        dr["ReceivableAmount"].ToString(),
//                        out receivable
//                    );

//                    decimal grandTotal =
//                        receivable - finalTotalDiscount;

//                    lblTotalDiscount.Text =
//                        finalTotalDiscount.ToString("N0");

//                    lblReceivable.Text =
//                        receivable.ToString("N0");

//                    lblGrandTotal.Text =
//                        grandTotal.ToString("N0");
//                }
//                else
//                {
//                    Response.Write("No Record Found!");
//                }
//            }
//        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(css))
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    // =========================
                    // CALCULATE FROM REPEATER (NOT LABEL)
                    // =========================
                    decimal totalServiceDiscount = 0;

                    foreach (RepeaterItem item in rptServices.Items)
                    {
                        TextBox txtDiscount =
                            (TextBox)item.FindControl("txtDiscount");

                        decimal discount = 0;

                        decimal.TryParse(
                            txtDiscount.Text.Replace(",", ""),
                            out discount
                        );

                        totalServiceDiscount += discount;
                    }

                    decimal totalConsultantDiscount = 0;

                    foreach (RepeaterItem item in rptConsultants.Items)
                    {
                        Literal ltDiscountAmount =
                            (Literal)item.FindControl("ltDiscountAmount");

                        decimal discount = 0;

                        decimal.TryParse(
                            ltDiscountAmount.Text.Replace(",", ""),
                            out discount
                        );

                        totalConsultantDiscount += discount;
                    }

                    decimal totalDiscount =
                        totalServiceDiscount + totalConsultantDiscount;

                    decimal receivable = 0;

                    decimal.TryParse(
                        lblReceivable.Text.Replace(",", ""),
                        out receivable
                    );

                    decimal grandTotal =
                        receivable - totalDiscount;

                    int donorId =
                        Convert.ToInt32(hfDonorId.Value);

                    // =========================
                    // SAVE MASTER
                    // =========================

                    string masterQuery = @"
            INSERT INTO WelfareFinalApproval
            (
                RequestID,
                AdmNo,
                MRNo,
                DonorId,
                TotalDiscountAmount,
                ReceivableAmount,
                GrandTotal,
                ApprovedBy
            )
            VALUES
            (
                @RequestID,
                @AdmNo,
                @MRNo,
                @DonorId,
                @TotalDiscount,
                @Receivable,
                @GrandTotal,
                @ApprovedBy
            )

            SELECT SCOPE_IDENTITY()
            ";

                    SqlCommand cmdMaster =
                        new SqlCommand(masterQuery, con, tran);

                    cmdMaster.Parameters.AddWithValue(
                        "@RequestID",
                        lblRequestID.Text
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@AdmNo",
                        lblAdmNo.Text
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@MRNo",
                        lblMRNo.Text
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@DonorId",
                        donorId
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@TotalDiscount",
                        totalDiscount
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@Receivable",
                        receivable
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@GrandTotal",
                        grandTotal
                    );

                    cmdMaster.Parameters.AddWithValue(
                        "@ApprovedBy",
                        "Mehran"
                    );

                    int finalApprovalId =
                        Convert.ToInt32(
                            cmdMaster.ExecuteScalar()
                        );

                    // =========================
                    // SAVE SERVICES
                    // =========================

                    foreach (RepeaterItem item in rptServices.Items)
                    {
                        DropDownList ddlService =
                            (DropDownList)item.FindControl("ddlService");

                        TextBox txtAmount =
                            (TextBox)item.FindControl("txtAmount");

                        TextBox txtPer =
                            (TextBox)item.FindControl("txtCustomPercentage");

                        TextBox txtDiscount =
                            (TextBox)item.FindControl("txtDiscount");

                        TextBox txtFinal =
                            (TextBox)item.FindControl("txtFinalAmount");

                        string detailQuery = @"
                INSERT INTO WelfareFinalApprovalServices
                (
                    FinalApprovalId,
                    ServiceName,
                    Amount,
                    DiscountPercent,
                    DiscountAmount,
                    FinalAmount
                )
                VALUES
                (
                    @FinalApprovalId,
                    @ServiceName,
                    @Amount,
                    @DiscountPercent,
                    @DiscountAmount,
                    @FinalAmount
                )";

                        SqlCommand cmdDetail =
                            new SqlCommand(detailQuery, con, tran);

                        cmdDetail.Parameters.AddWithValue(
                            "@FinalApprovalId",
                            finalApprovalId
                        );

                        cmdDetail.Parameters.AddWithValue(
                            "@ServiceName",
                            ddlService.SelectedValue
                        );

                        cmdDetail.Parameters.AddWithValue(
                            "@Amount",
                            Convert.ToDecimal(
                                txtAmount.Text.Replace(",", "")
                            )
                        );

                        cmdDetail.Parameters.AddWithValue(
                            "@DiscountPercent",
                            Convert.ToDecimal(txtPer.Text)
                        );

                        cmdDetail.Parameters.AddWithValue(
                            "@DiscountAmount",
                            Convert.ToDecimal(
                                txtDiscount.Text.Replace(",", "")
                            )
                        );

                        cmdDetail.Parameters.AddWithValue(
                            "@FinalAmount",
                            Convert.ToDecimal(
                                txtFinal.Text.Replace(",", "")
                            )
                        );

                        cmdDetail.ExecuteNonQuery();
                    }

                    // =========================
                    // SAVE CONSULTANTS
                    // =========================

                    foreach (RepeaterItem item in rptConsultants.Items)
                    {
                        Literal ltConsultantName =
                            (Literal)item.FindControl("ltConsultantName");

                        Literal ltVisitFee =
                            (Literal)item.FindControl("ltVisitFee");

                        Literal ltProcedureFee =
                            (Literal)item.FindControl("ltProcedureFee");

                        Literal ltDiscountPercent =
                            (Literal)item.FindControl("ltDiscountPercent");

                        Literal ltDiscountAmount =
                            (Literal)item.FindControl("ltDiscountAmount");

                        Literal ltFinalAmount =
                            (Literal)item.FindControl("ltFinalAmount");

                        string consultantQuery = @"
                INSERT INTO WelfareFinalApprovalConsultants
                (
                    FinalApprovalId,
                    ConsultantName,
                    VisitFee,
                    ProcedureFee,
                    DiscountPercent,
                    DiscountAmount,
                    FinalAmount
                )
                VALUES
                (
                    @FinalApprovalId,
                    @ConsultantName,
                    @VisitFee,
                    @ProcedureFee,
                    @DiscountPercent,
                    @DiscountAmount,
                    @FinalAmount
                )";

                        SqlCommand cmdConsultant =
                            new SqlCommand(
                                consultantQuery,
                                con,
                                tran
                            );

                        cmdConsultant.Parameters.AddWithValue(
                            "@FinalApprovalId",
                            finalApprovalId
                        );

                        cmdConsultant.Parameters.AddWithValue(
                            "@ConsultantName",
                            ltConsultantName.Text
                        );

                        cmdConsultant.Parameters.AddWithValue(
                            "@VisitFee",
                            Convert.ToDecimal(
                                ltVisitFee.Text.Replace(",", "")
                            )
                        );

                        cmdConsultant.Parameters.AddWithValue(
                            "@ProcedureFee",
                            Convert.ToDecimal(
                                ltProcedureFee.Text.Replace(",", "")
                            )
                        );

                        cmdConsultant.Parameters.AddWithValue(
                            "@DiscountPercent",
                            Convert.ToDecimal(
                                ltDiscountPercent.Text.Replace("%", "")
                            )
                        );

                        cmdConsultant.Parameters.AddWithValue(
                            "@DiscountAmount",
                            Convert.ToDecimal(
                                ltDiscountAmount.Text.Replace(",", "")
                            )
                        );

                        cmdConsultant.Parameters.AddWithValue(
                            "@FinalAmount",
                            Convert.ToDecimal(
                                ltFinalAmount.Text.Replace(",", "")
                            )
                        );

                        cmdConsultant.ExecuteNonQuery();
                    }

                    // =========================
                    // DONOR BALANCE CHECK
                    // =========================

                    //        string donorBalanceQuery = @"
                    //SELECT ISNULL(SUM(Amount),0)
                    //FROM Donations
                    //WHERE DonorId=@DonorId";

                    //        SqlCommand cmdBalance =
                    //            new SqlCommand(
                    //                donorBalanceQuery,
                    //                con,
                    //                tran
                    //            );

                    //        cmdBalance.Parameters.AddWithValue(
                    //            "@DonorId",
                    //            donorId
                    //        );

                    //        decimal donorBalance =
                    //            Convert.ToDecimal(
                    //                cmdBalance.ExecuteScalar()
                    //            );

                    //        if (donorBalance < totalDiscount)
                    //        {
                    //            tran.Rollback();

                    //            ScriptManager.RegisterStartupScript(
                    //                this,
                    //                GetType(),
                    //                "msg",
                    //                "alert('Insufficient donor balance');",
                    //                true
                    //            );
                    //            return;
                    //        }

                    //        // =========================
                    //        // UPDATE DONOR
                    //        // =========================

                    //        string donorQuery = @"
                    //UPDATE Donations
                    //SET Amount = Amount - @TotalDiscount
                    //WHERE DonorId=@DonorId";

                    //        SqlCommand cmdDonor =
                    //            new SqlCommand(
                    //                donorQuery,
                    //                con,
                    //                tran
                    //            );

                    //        cmdDonor.Parameters.AddWithValue(
                    //            "@TotalDiscount",
                    //            totalDiscount
                    //        );

                    //        cmdDonor.Parameters.AddWithValue(
                    //            "@DonorId",
                    //            donorId
                    //        );

                    //        cmdDonor.ExecuteNonQuery();

                    tran.Commit();

                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "msg",
                        "alert('Final Approval Saved Successfully');" +
                        "window.location='DeclarationForm.aspx?RequestID="
                        + lblRequestID.Text + "';",
                        true
                    );
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "msg",
                        "alert('" +
                        ex.Message.Replace("'", "") +
                        "');",
                        true
                    );
                }
            }
        }
    }
}