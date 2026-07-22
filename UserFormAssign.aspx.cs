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
    public partial class UserFormAssign : System.Web.UI.Page
    {
        private string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
                LoadFormsHierarchy();
            }
        }

        private void LoadUsers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT UserId, Username
                    FROM Users
                    WHERE IsActive = 1
                    ORDER BY Username
                ";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = "Username";
                ddlUsers.DataValueField = "UserId";
                ddlUsers.DataBind();

                ddlUsers.Items.Insert(0, new ListItem("-- Select User --", "0"));
            }
        }

        private void LoadFormsHierarchy()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        FormId,
                        FormName,
                        FormUrl,
                        MenuCategory
                    FROM Forms
                    WHERE MenuCategory IS NOT NULL
                    ORDER BY 
                        CASE MenuCategory
                            WHEN 'Master' THEN 1
                            WHEN 'Transactions' THEN 2
                            WHEN 'Security' THEN 3
                            WHEN 'Reports' THEN 4
                            WHEN 'Dashboards' THEN 5
                            ELSE 6
                        END,
                        FormName
                ";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Group by Category
                    var categories = dt.AsEnumerable()
                        .GroupBy(r => r.Field<string>("MenuCategory"))
                        .Select(g => new
                        {
                            CategoryName = g.Key,
                            FormCount = g.Count(),
                            Forms = g.Select(f => new
                            {
                                FormId = f.Field<int>("FormId"),
                                FormName = f.Field<string>("FormName"),
                                FormUrl = f.Field<string>("FormUrl"),
                                MenuCategory = f.Field<string>("MenuCategory")
                            }).ToList()
                        }).ToList();

                    // Build HTML
                    System.Text.StringBuilder html = new System.Text.StringBuilder();

                    int categoryIndex = 0;
                    foreach (var category in categories)
                    {
                        string categoryId = "cat" + categoryIndex;
                        string collapseId = "collapse" + categoryIndex;

                        html.AppendLine($@"
                        <div class='category-accordion'>
                            <div class='accordion'>
                                <div class='accordion-item'>
                                    <h2 class='accordion-header'>
                                        <button class='accordion-button collapsed' 
                                            type='button' 
                                            data-bs-toggle='collapse' 
                                            data-bs-target='#{collapseId}'
                                            aria-expanded='false'>
                                            <i class='{GetCategoryIcon(category.CategoryName)}'></i>
                                            <span>{category.CategoryName}</span>
                                            <span class='badge-count'>{category.FormCount}</span>
                                        </button>
                                    </h2>
                                    <div id='{collapseId}' class='accordion-collapse collapse'>
                                        <div class='accordion-body'>");

                        // Sub-Category: Get unique sub-category name
                        string subCategoryName = GetSubCategory(category.CategoryName);

                        html.AppendLine($@"
                                            <div class='sub-category'>
                                                <div class='sub-category-title'>
                                                    <i class='fa fa-folder-open'></i>
                                                    {subCategoryName}
                                                </div>
                                                <div class='form-check-group'>");

                        // Forms
                        foreach (var form in category.Forms)
                        {
                            html.AppendLine($@"
                                                    <div class='form-check'>
                                                        <input type='checkbox' 
                                                            class='form-check-input form-check-option' 
                                                            id='chk_{form.FormId}'
                                                            value='{form.FormId}' />
                                                        <label class='form-check-label' for='chk_{form.FormId}'>
                                                            <i class='{GetFormIcon(form.FormName)} me-1'></i>
                                                            {form.FormName}
                                                        </label>
                                                    </div>");
                        }

                        html.AppendLine($@"
                                                </div>
                                            </div>

                                            <!-- Select All -->
                                            <div class='select-all-container'>
                                                <div class='form-check'>
                                                    <input type='checkbox' 
                                                        class='form-check-input select-all-category' 
                                                        id='selectAll_{categoryIndex}' />
                                                    <label class='form-check-label' for='selectAll_{categoryIndex}'>
                                                        <i class='fa fa-check-double me-1'></i>Select All in this Category
                                                    </label>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>");

                        categoryIndex++;
                    }

                    litForms.Text = html.ToString();
                    formsArea.Visible = true;
                    noFormsMessage.Visible = false;
                }
                else
                {
                    formsArea.Visible = false;
                    noFormsMessage.Visible = true;
                }
            }
        }

        private string GetSubCategory(string menuCategory)
        {
            switch (menuCategory)
            {
                case "Master":
                    return "Master Data";
                case "Transactions":
                    return "Transaction Management";
                case "Security":
                    return "Security Management";
                case "Reports":
                    return "Report Management";
                case "Dashboards":
                    return "Analytics Dashboard";
                default:
                    return "General";
            }
        }

        public string GetCategoryIcon(string categoryName)
        {
            switch (categoryName)
            {
                case "Master":
                    return "fa fa-database module-icon-welfare";
                case "Transactions":
                    return "fa fa-exchange-alt module-icon-transactions";
                case "Security":
                    return "fa fa-shield-alt module-icon-security";
                case "Reports":
                    return "fa fa-chart-bar module-icon-reports";
                case "Dashboards":
                    return "fa fa-chart-pie module-icon-dashboards";
                default:
                    return "fa fa-folder";
            }
        }

        public string GetFormIcon(string formName)
        {
            string name = formName.ToLower();
            if (name.Contains("dashboard")) return "fa fa-chart-line";
            if (name.Contains("report")) return "fa fa-file-alt";
            if (name.Contains("donation")) return "fa fa-hand-holding-usd";
            if (name.Contains("patient")) return "fa fa-user-injured";
            if (name.Contains("user") || name.Contains("login") || name.Contains("sign")) return "fa fa-user";
            if (name.Contains("security") || name.Contains("audit")) return "fa fa-lock";
            if (name.Contains("approval") || name.Contains("limit")) return "fa fa-check-circle";
            if (name.Contains("discount")) return "fa fa-percent";
            if (name.Contains("income") || name.Contains("financial")) return "fa fa-money-bill-wave";
            if (name.Contains("service")) return "fa fa-concierge-bell";
            if (name.Contains("donor")) return "fa fa-user-friends";
            return "fa fa-file";
        }

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAssignedForms(ddlUsers.SelectedValue);
        }

        private void LoadAssignedForms(string userId)
        {
            if (userId == "0")
            {
                string script = @"
                    <script type='text/javascript'>
                        document.querySelectorAll('.form-check-option').forEach(function(chk) {
                            chk.checked = false;
                        });
                        document.querySelectorAll('.select-all-category').forEach(function(chk) {
                            chk.checked = false;
                        });
                        updateCount();
                    </script>
                ";
                ClientScript.RegisterStartupScript(this.GetType(), "clearAll", script, false);
                lblAssignedCount.Text = "0 forms assigned";
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT FormId FROM UserForms WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                List<int> assignedFormIds = new List<int>();
                while (dr.Read())
                {
                    assignedFormIds.Add(Convert.ToInt32(dr["FormId"]));
                }
                dr.Close();

                if (assignedFormIds.Count > 0)
                {
                    string ids = string.Join(",", assignedFormIds);
                    string script = $@"
                        <script type='text/javascript'>
                            var assignedIds = [{ids}];
                            document.querySelectorAll('.form-check-option').forEach(function(chk) {{
                                var id = parseInt(chk.value);
                                if (assignedIds.indexOf(id) !== -1) {{
                                    chk.checked = true;
                                }}
                            }});
                            updateCount();
                            
                            document.querySelectorAll('.select-all-category').forEach(function(parent) {{
                                var parentBody = parent.closest('.accordion-body');
                                if (parentBody) {{
                                    var allChecked = parentBody.querySelectorAll('.form-check-option:checked').length;
                                    var total = parentBody.querySelectorAll('.form-check-option').length;
                                    parent.checked = (allChecked === total && total > 0);
                                }}
                            }});
                        </script>
                    ";
                    ClientScript.RegisterStartupScript(this.GetType(), "loadAssigned", script, false);
                    lblAssignedCount.Text = assignedFormIds.Count + " forms assigned";
                }
                else
                {
                    string script = @"
                        <script type='text/javascript'>
                            document.querySelectorAll('.form-check-option').forEach(function(chk) {
                                chk.checked = false;
                            });
                            document.querySelectorAll('.select-all-category').forEach(function(chk) {
                                chk.checked = false;
                            });
                            updateCount();
                        </script>
                    ";
                    ClientScript.RegisterStartupScript(this.GetType(), "clearAll", script, false);
                    lblAssignedCount.Text = "0 forms assigned";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlUsers.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "alert('Please select a user first!');",
                    true
                );
                return;
            }

            try
            {
                List<int> selectedFormIds = GetSelectedFormIds();
                int userId = Convert.ToInt32(ddlUsers.SelectedValue);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string delQuery = "DELETE FROM UserForms WHERE UserId = @UserId";
                    SqlCommand cmdDel = new SqlCommand(delQuery, con);
                    cmdDel.Parameters.AddWithValue("@UserId", userId);
                    cmdDel.ExecuteNonQuery();

                    if (selectedFormIds.Count > 0)
                    {
                        string insertQuery = "INSERT INTO UserForms (UserId, FormId) VALUES (@UserId, @FormId)";

                        foreach (int formId in selectedFormIds)
                        {
                            SqlCommand cmd = new SqlCommand(insertQuery, con);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@FormId", formId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    con.Close();
                }

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "alert('Forms assigned successfully to " + ddlUsers.SelectedItem.Text + "!');",
                    true
                );

                lblAssignedCount.Text = selectedFormIds.Count + " forms assigned";
                lblStatus.Text = "✅ Saved at " + DateTime.Now.ToString("hh:mm tt");

                LoadAssignedForms(ddlUsers.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "alert('Error: " + ex.Message.Replace("'", "") + "');",
                    true
                );
            }
        }

        private List<int> GetSelectedFormIds()
        {
            List<int> selectedIds = new List<int>();

            // Get values from Request.Form
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.StartsWith("chk_"))
                {
                    string value = Request.Form[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        int formId;
                        if (int.TryParse(value, out formId))
                        {
                            selectedIds.Add(formId);
                        }
                    }
                }
            }

            return selectedIds;
        }
    }
}