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

                // Debug: Show form count
                System.Diagnostics.Debug.WriteLine("Forms loaded: " + dt.Rows.Count);
                lblStatus.Text = "Loaded " + dt.Rows.Count + " forms from database";
                
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
                                        <button class='accordion-button' 
                                            type='button' 
                                            data-bs-toggle='collapse' 
                                            data-bs-target='#{collapseId}'
                                            aria-expanded='true'>
                                            <i class='{GetCategoryIcon(category.CategoryName)} me-2'></i>
                                            <span class='me-2'>{category.CategoryName}</span>
                                            <span class='badge-count me-2'>{category.FormCount}</span>
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

                        // Forms with permissions
                        foreach (var form in category.Forms)
                        {
                            html.AppendLine($@"
                                                    <div class='form-permissions-row' style='display: flex; justify-content: space-between; align-items: center; width: 100%;'>
                                                        <div class='form-name-cell' style='flex: 0 0 auto;'>
                                                            <i class='{GetFormIcon(form.FormName)}'></i>
                                                            {form.FormName}
                                                        </div>
                                                        <div class='permissions-cell' style='flex: 1; display: flex; justify-content: flex-end;' >
                                                            <div class='permission-row'>
                                                                <input type='checkbox' 
                                                                    name='perm_{form.FormId}_index'
                                                                    id='perm_{form.FormId}_index'
                                                                    data-formid='{form.FormId}' 
                                                                    data-permission='CanIndex' />
                                                                <label for='perm_{form.FormId}_index'>Index</label>
                                                            </div>
                                                            <div class='permission-row'>
                                                                <input type='checkbox' 
                                                                    name='perm_{form.FormId}_create'
                                                                    id='perm_{form.FormId}_create'
                                                                    data-formid='{form.FormId}' 
                                                                    data-permission='CanCreate' />
                                                                <label for='perm_{form.FormId}_create'>Create</label>
                                                            </div>
                                                            <div class='permission-row'>
                                                                <input type='checkbox' 
                                                                    name='perm_{form.FormId}_edit'
                                                                    id='perm_{form.FormId}_edit'
                                                                    data-formid='{form.FormId}' 
                                                                    data-permission='CanEdit' />
                                                                <label for='perm_{form.FormId}_edit'>Edit</label>
                                                            </div>
                                                            <div class='permission-row'>
                                                                <input type='checkbox' 
                                                                    name='perm_{form.FormId}_delete'
                                                                    id='perm_{form.FormId}_delete'
                                                                    data-formid='{form.FormId}' 
                                                                    data-permission='CanDelete' />
                                                                <label for='perm_{form.FormId}_delete'>Delete</label>
                                                            </div>
                                                            <div class='permission-row'>
                                                                <input type='checkbox' 
                                                                    name='perm_{form.FormId}_print'
                                                                    id='perm_{form.FormId}_print'
                                                                    data-formid='{form.FormId}' 
                                                                    data-permission='CanPrint' />
                                                                <label for='perm_{form.FormId}_print'>Print</label>
                                                            </div>
                                                            <div class='permission-row'>
                                                                <input type='checkbox' 
                                                                    name='perm_{form.FormId}_view'
                                                                    id='perm_{form.FormId}_view'
                                                                    data-formid='{form.FormId}' 
                                                                    data-permission='CanView' />
                                                                <label for='perm_{form.FormId}_view'>View</label>
                                                            </div>
                                                        </div>
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
                                                        id='selectAll_{categoryIndex}' 
                                                        onclick='toggleCategorySelectAll(this)' />
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
                    lblStatus.Text = "Rendered " + categories.Count + " categories with " + dt.Rows.Count + " forms";
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
            System.Diagnostics.Debug.WriteLine("Loading assigned forms for userId: " + userId);
            lblStatus.Text = "Loading permissions for user...";
            
            if (userId == "0")
            {
                string script = @"<script type='text/javascript'>
                        document.querySelectorAll('.permission-switch .form-check-input').forEach(function(chk) {
                            chk.checked = false;
                        });
                        updateCount();
                    </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "clearAll", script, false);
                lblAssignedCount.Text = "0 forms assigned";
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT FormId, CanIndex, CanCreate, CanEdit, CanDelete, CanPrint, CanView 
                    FROM UserFormPermissions
                    WHERE UserId = @UserId
                ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var permissions = new Dictionary<int, Dictionary<string, bool>>();
                while (dr.Read())
                {
                    int formId = Convert.ToInt32(dr["FormId"]);
                    permissions[formId] = new Dictionary<string, bool>
                    {
                        { "CanIndex", dr["CanIndex"] != DBNull.Value && Convert.ToBoolean(dr["CanIndex"]) },
                        { "CanCreate", dr["CanCreate"] != DBNull.Value && Convert.ToBoolean(dr["CanCreate"]) },
                        { "CanEdit", dr["CanEdit"] != DBNull.Value && Convert.ToBoolean(dr["CanEdit"]) },
                        { "CanDelete", dr["CanDelete"] != DBNull.Value && Convert.ToBoolean(dr["CanDelete"]) },
                        { "CanPrint", dr["CanPrint"] != DBNull.Value && Convert.ToBoolean(dr["CanPrint"]) },
                        { "CanView", dr["CanView"] != DBNull.Value && Convert.ToBoolean(dr["CanView"]) }
                    };
                    System.Diagnostics.Debug.WriteLine("Loaded permissions for FormId " + formId + ": Index=" + permissions[formId]["CanIndex"] + ", Create=" + permissions[formId]["CanCreate"]);
                }
                dr.Close();
                
                System.Diagnostics.Debug.WriteLine("Total permissions loaded: " + permissions.Count);
                lblStatus.Text = "Loaded " + permissions.Count + " form permissions";

                if (permissions.Count > 0)
                {
                    System.Text.StringBuilder scriptBuilder = new System.Text.StringBuilder();
                    scriptBuilder.AppendLine("<script type='text/javascript'>");
                    scriptBuilder.AppendLine("    console.log('Starting to check permission boxes...');");
                    
                    foreach (var perm in permissions)
                    {
                        int formId = perm.Key;
                        var perms = perm.Value;
                        
                        scriptBuilder.AppendLine($"    console.log('Processing FormId: {formId}');");
                        scriptBuilder.AppendLine($"    var formId = {formId};");
                        
                        if (perms["CanIndex"])
                        {
                            scriptBuilder.AppendLine($"    var elemIndex = document.getElementById('perm_{formId}_index');");
                            scriptBuilder.AppendLine($"    if(elemIndex) {{ elemIndex.checked = true; console.log('Checked Index for form {formId}'); }}");
                        }
                        if (perms["CanCreate"])
                        {
                            scriptBuilder.AppendLine($"    var elemCreate = document.getElementById('perm_{formId}_create');");
                            scriptBuilder.AppendLine($"    if(elemCreate) {{ elemCreate.checked = true; console.log('Checked Create for form {formId}'); }}");
                        }
                        if (perms["CanEdit"])
                        {
                            scriptBuilder.AppendLine($"    var elemEdit = document.getElementById('perm_{formId}_edit');");
                            scriptBuilder.AppendLine($"    if(elemEdit) {{ elemEdit.checked = true; console.log('Checked Edit for form {formId}'); }}");
                        }
                        if (perms["CanDelete"])
                        {
                            scriptBuilder.AppendLine($"    var elemDelete = document.getElementById('perm_{formId}_delete');");
                            scriptBuilder.AppendLine($"    if(elemDelete) {{ elemDelete.checked = true; console.log('Checked Delete for form {formId}'); }}");
                        }
                        if (perms["CanPrint"])
                        {
                            scriptBuilder.AppendLine($"    var elemPrint = document.getElementById('perm_{formId}_print');");
                            scriptBuilder.AppendLine($"    if(elemPrint) {{ elemPrint.checked = true; console.log('Checked Print for form {formId}'); }}");
                        }
                        if (perms["CanView"])
                        {
                            scriptBuilder.AppendLine($"    var elemView = document.getElementById('perm_{formId}_view');");
                            scriptBuilder.AppendLine($"    if(elemView) {{ elemView.checked = true; console.log('Checked View for form {formId}'); }}");
                        }
                    }
                    
                    scriptBuilder.AppendLine("    updateCount();");
                    scriptBuilder.AppendLine("    console.log('Finished checking permission boxes');");
                    scriptBuilder.AppendLine("</script>");
                    
                    ClientScript.RegisterStartupScript(this.GetType(), "loadAssigned", scriptBuilder.ToString(), false);
                    lblAssignedCount.Text = permissions.Count + " forms assigned";
                }
                else
                {
                    string script = @"
                        <script type='text/javascript'>
                            document.querySelectorAll('.permission-switch .form-check-input').forEach(function(chk) {
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
            // Debug: Log dropdown state
            System.Diagnostics.Debug.WriteLine("Save button clicked");
            System.Diagnostics.Debug.WriteLine("ddlUsers.SelectedIndex: " + ddlUsers.SelectedIndex);
            System.Diagnostics.Debug.WriteLine("ddlUsers.SelectedValue: " + ddlUsers.SelectedValue);
            System.Diagnostics.Debug.WriteLine("ddlUsers.SelectedItem.Text: " + (ddlUsers.SelectedItem != null ? ddlUsers.SelectedItem.Text : "null"));
            System.Diagnostics.Debug.WriteLine("Request.Form[ddlUsers]: " + Request.Form["ddlUsers"]);

            // Get userId from Request.Form first (most reliable on postback), then fallback to dropdown
            string selectedUserId = Request.Form["ddlUsers"];
            if (string.IsNullOrEmpty(selectedUserId) || selectedUserId == "0")
            {
                selectedUserId = ddlUsers.SelectedValue;
                System.Diagnostics.Debug.WriteLine("Using ddlUsers.SelectedValue: " + selectedUserId);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Using Request.Form value: " + selectedUserId);
            }

            if (string.IsNullOrEmpty(selectedUserId) || selectedUserId == "0")
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
                var selectedPermissions = GetSelectedPermissions();
                int userId = Convert.ToInt32(selectedUserId);
                System.Diagnostics.Debug.WriteLine("Saving permissions for userId: " + userId);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Delete existing UserFormPermissions for this user
                    string delPermissionsQuery = "DELETE FROM UserFormPermissions WHERE UserId = @UserId";
                    SqlCommand cmdDelPermissions = new SqlCommand(delPermissionsQuery, con);
                    cmdDelPermissions.Parameters.AddWithValue("@UserId", userId);
                    cmdDelPermissions.ExecuteNonQuery();

                    if (selectedPermissions.Count > 0)
                    {
                        // Insert directly into UserFormPermissions table using FormId
                        string insertPermissionsQuery = @"
                            INSERT INTO UserFormPermissions (UserId, FormId, CanIndex, CanCreate, CanEdit, CanDelete, CanPrint, CanView) 
                            VALUES (@UserId, @FormId, @CanIndex, @CanCreate, @CanEdit, @CanDelete, @CanPrint, @CanView)
                        ";

                        foreach (var perm in selectedPermissions)
                        {
                            SqlCommand cmdPermissions = new SqlCommand(insertPermissionsQuery, con);
                            cmdPermissions.Parameters.AddWithValue("@UserId", userId);
                            cmdPermissions.Parameters.AddWithValue("@FormId", perm.Key);
                            cmdPermissions.Parameters.AddWithValue("@CanIndex", perm.Value["CanIndex"]);
                            cmdPermissions.Parameters.AddWithValue("@CanCreate", perm.Value["CanCreate"]);
                            cmdPermissions.Parameters.AddWithValue("@CanEdit", perm.Value["CanEdit"]);
                            cmdPermissions.Parameters.AddWithValue("@CanDelete", perm.Value["CanDelete"]);
                            cmdPermissions.Parameters.AddWithValue("@CanPrint", perm.Value["CanPrint"]);
                            cmdPermissions.Parameters.AddWithValue("@CanView", perm.Value["CanView"]);
                            cmdPermissions.ExecuteNonQuery();
                        }
                    }

                    con.Close();
                }

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "alert('Forms and permissions assigned successfully to " + ddlUsers.SelectedItem.Text + "!');",
                    true
                );

                lblAssignedCount.Text = selectedPermissions.Count + " forms assigned";
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

        private Dictionary<int, Dictionary<string, bool>> GetSelectedPermissions()
        {
            var permissions = new Dictionary<int, Dictionary<string, bool>>();

            // Get all form IDs that have at least one permission checked
            var formIds = new HashSet<int>();

            foreach (string key in Request.Form.AllKeys)
            {
                if (key != null && key.StartsWith("perm_") && Request.Form[key] == "on")
                {
                    // Parse key format: perm_{formId}_{permission}
                    string[] parts = key.Split('_');
                    if (parts.Length >= 3)
                    {
                        int formId;
                        if (int.TryParse(parts[1], out formId))
                        {
                            formIds.Add(formId);
                        }
                    }
                }
            }

            // For each form ID, collect all permissions
            foreach (int formId in formIds)
            {
                var formPermissions = new Dictionary<string, bool>
                {
                    { "CanIndex", Request.Form["perm_" + formId + "_index"] == "on" },
                    { "CanCreate", Request.Form["perm_" + formId + "_create"] == "on" },
                    { "CanEdit", Request.Form["perm_" + formId + "_edit"] == "on" },
                    { "CanDelete", Request.Form["perm_" + formId + "_delete"] == "on" },
                    { "CanPrint", Request.Form["perm_" + formId + "_print"] == "on" },
                    { "CanView", Request.Form["perm_" + formId + "_view"] == "on" }
                };

                // Only add if at least one permission is true
                if (formPermissions.Values.Any(v => v))
                {
                    permissions[formId] = formPermissions;
                }
            }

            return permissions;
        }
    }
}