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
    public partial class CategoryServices : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategory();
                LoadServicesWithPercentage();
                LoadGrid();

                if (Request.QueryString["edit"] != null)
                {
                    LoadEdit(Request.QueryString["edit"]);
                }

                if (Request.QueryString["delete"] != null)
                {
                    DeleteRecord(Request.QueryString["delete"]);
                }
            }
        }

        void LoadCategory()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("select CategoryId,CategoryName from Category", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", "0"));
            }
        }

        void LoadServicesWithPercentage()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("select ServiceId, ServiceName from Services", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptServices.DataSource = dt;
                rptServices.DataBind();
            }
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string serviceId = drv["ServiceId"].ToString();

                // Check if this service is already assigned to the selected category
                if (ddlCategory.SelectedValue != "0" && !string.IsNullOrEmpty(ddlCategory.SelectedValue))
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlCommand cmd = new SqlCommand(
                            "SELECT MinPercentage, MaxPercentage FROM CategoryServices WHERE CategoryId=@CategoryId AND ServiceId=@ServiceId",
                            con);
                        cmd.Parameters.AddWithValue("@CategoryId", ddlCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ServiceId", serviceId);

                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            TextBox txtMin = (TextBox)e.Item.FindControl("txtMinPercentage");
                            TextBox txtMax = (TextBox)e.Item.FindControl("txtMaxPercentage");
                            CheckBox chkSelected = (CheckBox)e.Item.FindControl("chkSelected");

                            txtMin.Text = dr["MinPercentage"].ToString();
                            txtMax.Text = dr["MaxPercentage"].ToString();
                            chkSelected.Checked = true;
                        }
                        dr.Close();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategory.SelectedValue == "0")
                {
                    lblMessage.Text = "Please select a category first.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Validate percentages
                bool hasError = false;
                string errorMsg = "";

                foreach (RepeaterItem item in rptServices.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        CheckBox chkSelected = (CheckBox)item.FindControl("chkSelected");
                        TextBox txtMin = (TextBox)item.FindControl("txtMinPercentage");
                        TextBox txtMax = (TextBox)item.FindControl("txtMaxPercentage");

                        if (chkSelected.Checked)
                        {
                            if (string.IsNullOrEmpty(txtMin.Text) || string.IsNullOrEmpty(txtMax.Text))
                            {
                                hasError = true;
                                errorMsg = "Please enter both Min and Max percentage for selected services.";
                                break;
                            }

                            decimal min = decimal.Parse(txtMin.Text);
                            decimal max = decimal.Parse(txtMax.Text);

                            if (min < 0 || min > 100 || max < 0 || max > 100)
                            {
                                hasError = true;
                                errorMsg = "Percentages must be between 0 and 100.";
                                break;
                            }

                            if (min >= max)
                            {
                                hasError = true;
                                errorMsg = "Min percentage must be less than Max percentage.";
                                break;
                            }
                        }
                    }
                }

                if (hasError)
                {
                    lblMessage.Text = errorMsg;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // Delete existing records for this category
                        SqlCommand del = new SqlCommand("DELETE FROM CategoryServices WHERE CategoryId=@c", con, transaction);
                        del.Parameters.AddWithValue("@c", ddlCategory.SelectedValue);
                        del.ExecuteNonQuery();

                        // Insert new records with percentages
                        foreach (RepeaterItem item in rptServices.Items)
                        {
                            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                            {
                                CheckBox chkSelected = (CheckBox)item.FindControl("chkSelected");
                                HiddenField hfServiceId = (HiddenField)item.FindControl("hfServiceId");
                                TextBox txtMin = (TextBox)item.FindControl("txtMinPercentage");
                                TextBox txtMax = (TextBox)item.FindControl("txtMaxPercentage");

                                if (chkSelected.Checked)
                                {
                                    SqlCommand cmd = new SqlCommand(
                                        "INSERT INTO CategoryServices(CategoryId, ServiceId, MinPercentage, MaxPercentage) VALUES(@c, @s, @min, @max)",
                                        con, transaction);

                                    cmd.Parameters.AddWithValue("@c", ddlCategory.SelectedValue);
                                    cmd.Parameters.AddWithValue("@s", hfServiceId.Value);
                                    cmd.Parameters.AddWithValue("@min", decimal.Parse(txtMin.Text));
                                    cmd.Parameters.AddWithValue("@max", decimal.Parse(txtMax.Text));

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        lblMessage.Text = "Configuration saved successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;

                        LoadGrid();
                        ClearAll();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblMessage.Text = "Error: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        void ClearAll()
        {
            ddlCategory.SelectedIndex = 0;

            foreach (RepeaterItem item in rptServices.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkSelected = (CheckBox)item.FindControl("chkSelected");
                    TextBox txtMin = (TextBox)item.FindControl("txtMinPercentage");
                    TextBox txtMax = (TextBox)item.FindControl("txtMaxPercentage");

                    chkSelected.Checked = false;
                    txtMin.Text = "";
                    txtMax.Text = "";
                }
            }

            lblMessage.Text = "";
        }

        void LoadEdit(string id)
        {
            ddlCategory.SelectedValue = id;

            // Load selected services with their percentages
            foreach (RepeaterItem item in rptServices.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfServiceId = (HiddenField)item.FindControl("hfServiceId");
                    CheckBox chkSelected = (CheckBox)item.FindControl("chkSelected");
                    TextBox txtMin = (TextBox)item.FindControl("txtMinPercentage");
                    TextBox txtMax = (TextBox)item.FindControl("txtMaxPercentage");

                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlCommand cmd = new SqlCommand(
                            "SELECT MinPercentage, MaxPercentage FROM CategoryServices WHERE CategoryId=@CategoryId AND ServiceId=@ServiceId",
                            con);
                        cmd.Parameters.AddWithValue("@CategoryId", id);
                        cmd.Parameters.AddWithValue("@ServiceId", hfServiceId.Value);

                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            chkSelected.Checked = true;
                            txtMin.Text = dr["MinPercentage"].ToString();
                            txtMax.Text = dr["MaxPercentage"].ToString();
                        }
                        dr.Close();
                    }
                }
            }
        }

        void DeleteRecord(string id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM CategoryServices WHERE CategoryId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                LoadGrid();
                lblMessage.Text = "Record deleted successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
        }

        void LoadGrid()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                        SELECT 
                            c.CategoryId,
                            c.CategoryName,
                            STRING_AGG(s.ServiceName + ' (' + CAST(cs.MinPercentage AS VARCHAR) + '%-' + CAST(cs.MaxPercentage AS VARCHAR) + '%)', ', ') AS Services
                        FROM CategoryServices cs
                        JOIN Category c ON cs.CategoryId = c.CategoryId
                        JOIN Services s ON cs.ServiceId = s.ServiceId
                        GROUP BY c.CategoryId, c.CategoryName
                        ORDER BY c.CategoryName";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Handle empty data
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // You can add additional formatting here if needed
            }
        }
    }
}