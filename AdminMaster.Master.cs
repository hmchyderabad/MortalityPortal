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
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        private string cs = ConfigurationManager
            .ConnectionStrings["welfare"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // LOGIN CHECK
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadMenus();
                lblUsername.InnerText = Session["Username"].ToString();
            }
        }

        // =====================================
        // LOAD ALL MENUS
        // =====================================
        private void LoadMenus()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT
                        F.FormName,
                        F.FormUrl,
                        F.MenuCategory
                    FROM UserFormPermissions UFP
                    INNER JOIN Forms F ON UFP.FormId = F.FormId
                    WHERE UFP.UserId = @UserId AND UFP.CanIndex = 1
                    ORDER BY 
                        CASE F.MenuCategory
                            WHEN 'Master' THEN 1
                            WHEN 'Transactions' THEN 2
                            WHEN 'Reports' THEN 3
                            WHEN 'Security' THEN 4
                            WHEN 'Dashboards' THEN 5
                            ELSE 6
                        END,
                        F.FormName
                ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", Session["UserId"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // ---- MASTER ----
                DataView dvMaster = new DataView(dt);
                dvMaster.RowFilter = "MenuCategory='Master'";
                rptMaster.DataSource = dvMaster;
                rptMaster.DataBind();

                // ---- TRANSACTIONS ----
                DataView dvTran = new DataView(dt);
                dvTran.RowFilter = "MenuCategory='Transactions'";
                rptTransactions.DataSource = dvTran;
                rptTransactions.DataBind();

                // ---- REPORTS ----
                DataView dvRpt = new DataView(dt);
                dvRpt.RowFilter = "MenuCategory='Reports'";
                rptReports.DataSource = dvRpt;
                rptReports.DataBind();

                // ---- SECURITY (NEW) ----
                DataView dvSecurity = new DataView(dt);
                dvSecurity.RowFilter = "MenuCategory='Security'";
                rptSecurity.DataSource = dvSecurity;
                rptSecurity.DataBind();

                // ---- DASHBOARDS (NEW) ----
                DataView dvDashboards = new DataView(dt);
                dvDashboards.RowFilter = "MenuCategory='Dashboards'";
                rptDashboards.DataSource = dvDashboards;
                rptDashboards.DataBind();

                // ---- Mortality (NEW) ----
                DataView dvMortality = new DataView(dt);
                dvMortality.RowFilter = "MenuCategory='Mortality'";
                rptMortality.DataSource = dvMortality;
                rptMortality.DataBind();

                // Hide empty modules
                HideEmptyModules();
            }
        }

        // =====================================
        // HIDE EMPTY MODULES
        // =====================================
        private void HideEmptyModules()
        {
            // Check if Security has any items
            bool hasSecurity = false;
            foreach (RepeaterItem item in rptSecurity.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    hasSecurity = true;
                    break;
                }
            }

            // Check if Dashboards has any items
            bool hasDashboards = false;
            foreach (RepeaterItem item in rptDashboards.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    hasDashboards = true;
                    break;
                }
            }

            // Note: To hide the entire dropdown, we'd need to access the parent nav-item
            // This can be done with JavaScript or by using runat="server" on the container
            // For now, empty repeaters will just show no items
        }

        // =====================================
        // LOGOUT
        // =====================================
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = "";
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            Response.Redirect(ResolveUrl("~/Login.aspx"));
        }
    }
}