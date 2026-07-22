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
    public partial class HRDashboard : System.Web.UI.Page
    {
        public int TotalEmployees { get; set; }
        public int ConfirmedEmployees { get; set; }
        public int ContractEmployees { get; set; }
        public int ProbationEmployees { get; set; }
        public int DailyWageEmployees { get; set; }
        public int InternshipEmployees { get; set; }
        public int LeavingCount { get; set; }

        public DataTable EmpTypeTable { get; set; }
        public DataTable DeptTable { get; set; }
        public DataTable ShiftTable { get; set; }
        public DataTable MachineTable { get; set; }
        public DataTable SalaryTable { get; set; }
        public DataTable LeaveTable { get; set; }
        private string connectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["HRDB"].ConnectionString;

            if (!IsPostBack)
            {
                LoadAllData();
            }
        }

        private void LoadAllData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Total Employees
                    TotalEmployees = GetTotalEmployees(conn);

                    // 2. Employee Type by Location
                    EmpTypeTable = GetEmployeeTypeData(conn);
                    CalculateEmployeeTypeCounts(EmpTypeTable);

                    // 3. Department Wise Employees
                    DeptTable = GetDepartmentData(conn);

                    // 4. Shift Wise Data (Today)
                    ShiftTable = GetShiftData(conn);

                    // 5. Attendance by Machine (Today)
                    MachineTable = GetMachineData(conn);

                    // 6. Department Wise Salaries
                    SalaryTable = GetSalaryData(conn);

                    // 7. Employees on Leave (Today)
                    LeaveTable = GetLeaveData(conn);

                    // 8. Leaving Employees (June 2026)
                    LeavingCount = GetLeavingCount(conn);
                }
            }
            catch (Exception ex)
            {
                // Log error and set empty tables
                System.Diagnostics.Debug.WriteLine("Error loading dashboard data: " + ex.Message);
                SetEmptyTables();
            }
        }

        private int GetTotalEmployees(SqlConnection conn)
        {
            string query = "SELECT COUNT(*) AS TotalEmployees FROM qryCurrentEmpInfo";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        private DataTable GetEmployeeTypeData(SqlConnection conn)
        {
            string query = @"
                SELECT 
                    EmpType,
                    COUNT(*) AS TotalEmployees,
                    Location
                FROM qryCurrentEmpInfo
                GROUP BY EmpType, Location
                ORDER BY EmpType";

            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void CalculateEmployeeTypeCounts(DataTable dt)
        {
            ConfirmedEmployees = 0;
            ContractEmployees = 0;
            ProbationEmployees = 0;
            DailyWageEmployees = 0;
            InternshipEmployees = 0;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string empType = row["EmpType"].ToString().Trim();
                    int count = Convert.ToInt32(row["TotalEmployees"]);

                    switch (empType.ToLower())
                    {
                        case "confirmed":
                            ConfirmedEmployees += count;
                            break;
                        case "contract":
                            ContractEmployees += count;
                            break;
                        case "on probation":
                            ProbationEmployees += count;
                            break;
                        case "daily wages":
                            DailyWageEmployees += count;
                            break;
                        case "internship":
                            InternshipEmployees += count;
                            break;
                    }
                }
            }
        }

        private DataTable GetDepartmentData(SqlConnection conn)
        {
            string query = @"
                SELECT 
                    MainDepartment,
                    COUNT(*) AS TotalEmployees,
                    Location
                FROM qryCurrentEmpInfo
                GROUP BY MainDepartment, Location
                ORDER BY MainDepartment";

            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetShiftData(SqlConnection conn)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string query = @"
                SELECT 
                    a.ShiftCode, 
                    b.Description,
                    COUNT(*) AS TotalEmployee
                FROM tblAttendance AS a
                INNER JOIN tblRosterShift AS b ON a.ShiftCode = b.Code
                WHERE CAST(a.AttDate AS DATE) = @Today
                GROUP BY a.ShiftCode, b.Description
                ORDER BY a.ShiftCode";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Today", today);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        private DataTable GetMachineData(SqlConnection conn)
        {
            string query = @"
                SELECT 
                    MachineId,
                    COUNT(*) AS TotalEmployee
                FROM tblMachineTagging
                WHERE CAST(AttDate AS DATE) = CAST(GETDATE() AS DATE)
                GROUP BY MachineId
                ORDER BY MachineId";

            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetSalaryData(SqlConnection conn)
        {
            string query = @"
                SELECT
                    MainDepartment,
                    Location,
                    SUM(TRY_CAST(Gross AS DECIMAL(18,2))) AS TotalGross
                FROM qryCurrentEmpInfo
                GROUP BY MainDepartment, Location
                ORDER BY MainDepartment";

            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetLeaveData(SqlConnection conn)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string query = @"
                SELECT 
                    b.EmpName,
                    a.DateFrom,
                    a.DateTo
                FROM tblEmpLeave AS a
                INNER JOIN qryCurrentEmpInfo AS b ON a.EmpId = b.EmpId
                WHERE CAST(a.DateFrom AS DATE) = @Today 
                   OR CAST(a.DateTo AS DATE) = @Today";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Today", today);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        private int GetLeavingCount(SqlConnection conn)
        {
            string query = @"
                SELECT COUNT(EmpCode) 
                FROM EmployeeExitClearanceExit 
                WHERE LeavingDate BETWEEN '2026-06-01 00:00:00.000' AND '2026-06-30 23:59:59.999'";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        private void SetEmptyTables()
        {
            EmpTypeTable = new DataTable();
            DeptTable = new DataTable();
            ShiftTable = new DataTable();
            MachineTable = new DataTable();
            SalaryTable = new DataTable();
            LeaveTable = new DataTable();
            TotalEmployees = 0;
            ConfirmedEmployees = 0;
            ContractEmployees = 0;
            ProbationEmployees = 0;
            DailyWageEmployees = 0;
            InternshipEmployees = 0;
            LeavingCount = 0;
        }
    }
}