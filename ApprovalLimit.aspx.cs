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
    public partial class ApprovalLimit : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDonors();
                LoadData();
            }
        }

        void LoadDonors()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT DonorId, FirstName AS Name FROM Donors", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDonor.DataSource = dt;
                ddlDonor.DataTextField = "Name";
                ddlDonor.DataValueField = "DonorId";
                ddlDonor.DataBind();

                ddlDonor.Items.Insert(0, new ListItem("-- Select Donor --", ""));
            }
        }

        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT a.Id,
                   d.FirstName AS DonorName,
                   a.AmountFrom,
                   a.AmountTo
            FROM ApprovalLimit a
            INNER JOIN Donors d ON a.DonorId = d.DonorId", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // Format data for display (convert to percentage with 2 decimal places)
                foreach (DataRow row in dt.Rows)
                {
                    // Multiply by 100 and format to show decimal properly
                    decimal from = Convert.ToDecimal(row["AmountFrom"]) * 100;
                    decimal to = Convert.ToDecimal(row["AmountTo"]) * 100;

                    // Store as string with 2 decimal places
                    row["AmountFrom"] = from.ToString("0.00");
                    row["AmountTo"] = to.ToString("0.00");
                }

                gvData.DataSource = dt;
                gvData.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal from, to;

            if (!decimal.TryParse(txtFrom.Text, out from) ||
                !decimal.TryParse(txtTo.Text, out to))
            {
                ShowAlert("Enter valid percentages!", "error");
                return;
            }

            if (string.IsNullOrEmpty(ddlDonor.SelectedValue))
            {
                ShowAlert("Please select a donor!", "warning");
                return;
            }

            if (from < 0 || to < 0 || from > 100 || to > 100)
            {
                ShowAlert("Percentage must be between 0 to 100!", "error");
                return;
            }

            if (from >= to)
            {
                ShowAlert("'From' must be less than 'To'!", "error");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                // Check if donor already exists
                string checkQuery = "SELECT COUNT(*) FROM ApprovalLimit WHERE DonorId = @Donor";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Donor", ddlDonor.SelectedValue);

                con.Open();
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                con.Close();

                if (count > 0)
                {
                    // Update existing
                    SqlCommand cmd = new SqlCommand(@"
                        UPDATE ApprovalLimit 
                        SET AmountFrom = @From, AmountTo = @To 
                        WHERE DonorId = @Donor", con);

                    cmd.Parameters.AddWithValue("@Donor", ddlDonor.SelectedValue);
                    cmd.Parameters.AddWithValue("@From", from / 100);
                    cmd.Parameters.AddWithValue("@To", to / 100);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ShowAlert("Updated Successfully!", "success");
                }
                else
                {
                    // Insert new
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO ApprovalLimit (DonorId, AmountFrom, AmountTo)
                        VALUES (@Donor, @From, @To)", con);

                    cmd.Parameters.AddWithValue("@Donor", ddlDonor.SelectedValue);
                    cmd.Parameters.AddWithValue("@From", from / 100);
                    cmd.Parameters.AddWithValue("@To", to / 100);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ShowAlert("Saved Successfully!", "success");
                }
            }

            LoadData();
            ClearForm();
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvData.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvData.Rows[e.RowIndex];

                // Find TextBox controls properly
                TextBox txtFrom = (TextBox)row.Cells[1].Controls[0];
                TextBox txtTo = (TextBox)row.Cells[2].Controls[0];

                decimal from = Convert.ToDecimal(txtFrom.Text);
                decimal to = Convert.ToDecimal(txtTo.Text);

                // Validation
                if (from < 0 || to < 0 || from > 100 || to > 100)
                {
                    ShowAlert("Percentage must be between 0 to 100!", "error");
                    return;
                }

                if (from >= to)
                {
                    ShowAlert("'From' must be less than 'To'!", "error");
                    return;
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(@"
                        UPDATE ApprovalLimit 
                        SET AmountFrom = @F, AmountTo = @T 
                        WHERE Id = @Id", con);

                    cmd.Parameters.AddWithValue("@F", from / 100);
                    cmd.Parameters.AddWithValue("@T", to / 100);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        gvData.EditIndex = -1;
                        LoadData();
                        ShowAlert("Updated Successfully!", "success");
                    }
                    else
                    {
                        ShowAlert("No record updated!", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message, "error");
            }
        }

        protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvData.EditIndex = -1;
            LoadData();
        }

        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM ApprovalLimit WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        LoadData();
                        ShowAlert("Deleted Successfully!", "success");
                    }
                    else
                    {
                        ShowAlert("No record deleted!", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message, "error");
            }
        }

        void ClearForm()
        {
            ddlDonor.SelectedIndex = 0;
            txtFrom.Text = "";
            txtTo.Text = "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            ShowAlert("Fields cleared!", "info");
        }

        void ShowAlert(string message, string type)
        {
            string icon = type == "success" ? "success" :
                          type == "error" ? "error" :
                          type == "warning" ? "warning" : "info";

            string script = $"Swal.fire({{ icon: '{icon}', title: '{message}', showConfirmButton: false, timer: 2000 }});";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }

        // RowDataBound to format percentage display
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // For normal display mode
                if (e.Row.RowState == DataControlRowState.Normal ||
                    e.Row.RowState == DataControlRowState.Alternate)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    // Format with 2 decimal places and % sign
                    decimal from = Convert.ToDecimal(drv["AmountFrom"]);
                    decimal to = Convert.ToDecimal(drv["AmountTo"]);

                    e.Row.Cells[1].Text = from.ToString("0.00") + "%";
                    e.Row.Cells[2].Text = to.ToString("0.00") + "%";
                }

                // For edit mode - show values in TextBox with proper format
                if (e.Row.RowState == DataControlRowState.Edit)
                {
                    TextBox txtFrom = (TextBox)e.Row.Cells[1].Controls[0];
                    TextBox txtTo = (TextBox)e.Row.Cells[2].Controls[0];

                    if (txtFrom != null)
                    {
                        decimal from = Convert.ToDecimal(txtFrom.Text);
                        txtFrom.Text = from.ToString("0.00");
                    }

                    if (txtTo != null)
                    {
                        decimal to = Convert.ToDecimal(txtTo.Text);
                        txtTo.Text = to.ToString("0.00");
                    }
                }
            }
        }
    }
}