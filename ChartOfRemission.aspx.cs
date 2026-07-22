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
    public partial class ChartOfRemission : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                LoadWelfareCodes();
                LoadDepCodes();
            }
        }
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ChartOfRemission", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRemission.DataSource = dt;
                gvRemission.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO ChartOfRemission
            (Qualifier, WelfareCode, BillFrom, BillTo, DependantFrom, DependantTo, DependantCode, PercentageFrom, PercentageTo)
            VALUES (@Qualifier, @WelfareCode, @BillFrom, @BillTo, @DepFrom, @DepTo, @DepCode, @PerFrom, @PerTo)", con);

                    cmd.Parameters.AddWithValue("@Qualifier", ddlQualifier.SelectedValue);
                    cmd.Parameters.AddWithValue("@WelfareCode", ddlWelfareCode.SelectedValue);

                    cmd.Parameters.AddWithValue("@BillFrom", Convert.ToDecimal(txtBillFrom.Text));
                    cmd.Parameters.AddWithValue("@BillTo", Convert.ToDecimal(txtBillTo.Text));

                    cmd.Parameters.AddWithValue("@DepFrom", Convert.ToInt32(txtDepFrom.Text));
                    cmd.Parameters.AddWithValue("@DepTo", Convert.ToInt32(txtDepTo.Text));

                    // ✅ UPDATED LINE (Dropdown se value)
                    cmd.Parameters.AddWithValue("@DepCode", ddlDepCode.SelectedValue);

                    cmd.Parameters.AddWithValue("@PerFrom", Convert.ToDecimal(txtPerFrom.Text));
                    cmd.Parameters.AddWithValue("@PerTo", Convert.ToDecimal(txtPerTo.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadData();

                // ✅ SweetAlert Success
                ShowAlert("Saved Successfully!", "success");

                ClearForm();
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message, "error");
            }
        }
        void ShowAlert(string message, string type)
        {
            string script = $"Swal.fire({{ icon: '{type}', title: '{message}', showConfirmButton: false, timer: 2000 }});";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }
        protected void gvRemission_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvRemission.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ChartOfRemission WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ShowAlert("Deleted Successfully!", "success");
        }

        protected void gvRemission_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRemission.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gvRemission_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRemission.EditIndex = -1;
            LoadData();
        }
        protected void gvRemission_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvRemission.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvRemission.Rows[e.RowIndex];

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
            UPDATE ChartOfRemission SET 
                Qualifier=@Qualifier,
                WelfareCode=@WelfareCode,
                BillFrom=@BillFrom,
                BillTo=@BillTo,
                DependantFrom=@DependantFrom,
                DependantTo=@DependantTo,
                DependantCode=@DependantCode,
                PercentageFrom=@PercentageFrom,
                PercentageTo=@PercentageTo
            WHERE Id=@Id", con);

                cmd.Parameters.AddWithValue("@Qualifier", ((TextBox)row.FindControl("txtQualifier")).Text);
                cmd.Parameters.AddWithValue("@WelfareCode", ((TextBox)row.FindControl("txtWelfareCode")).Text);
                cmd.Parameters.AddWithValue("@BillFrom", ((TextBox)row.FindControl("txtBillFrom")).Text);
                cmd.Parameters.AddWithValue("@BillTo", ((TextBox)row.FindControl("txtBillTo")).Text);
                cmd.Parameters.AddWithValue("@DependantFrom", ((TextBox)row.FindControl("txtDepFrom")).Text);
                cmd.Parameters.AddWithValue("@DependantTo", ((TextBox)row.FindControl("txtDepTo")).Text);
                cmd.Parameters.AddWithValue("@DependantCode", ((TextBox)row.FindControl("txtDepCode")).Text);
                cmd.Parameters.AddWithValue("@PercentageFrom", ((TextBox)row.FindControl("txtPerFrom")).Text);
                cmd.Parameters.AddWithValue("@PercentageTo", ((TextBox)row.FindControl("txtPerTo")).Text);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvRemission.EditIndex = -1;
            LoadData();
            ShowAlert("All fields updated successfully!", "success");
        }
        //protected void gvRemission_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    int id = Convert.ToInt32(gvRemission.DataKeys[e.RowIndex].Value);

        //    GridViewRow row = gvRemission.Rows[e.RowIndex];

        //    string qualifier = ((TextBox)row.Cells[0].Controls[0]).Text;
        //    string code = ((TextBox)row.Cells[1].Controls[0]).Text;

        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        SqlCommand cmd = new SqlCommand(@"UPDATE ChartOfRemission SET 
        //Qualifier=@Qualifier,
        //WelfareCode=@Code
        //WHERE Id=@Id", con);

        //        cmd.Parameters.AddWithValue("@Qualifier", qualifier);
        //        cmd.Parameters.AddWithValue("@Code", code);
        //        cmd.Parameters.AddWithValue("@Id", id);

        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //    }

        //    gvRemission.EditIndex = -1;
        //    LoadData();
        //    ShowAlert("Updated Successfully!", "success");
        //}

        protected void gvRemission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRemission.PageIndex = e.NewPageIndex;
            LoadData();
        }
        void LoadDepCodes()
        {
            ddlDepCode.Items.Clear();
            ddlDepCode.Items.Add(new ListItem("-- Select Code --", ""));

            for (int i = 1; i <= 10; i++)
            {
                ddlDepCode.Items.Add(new ListItem("C" + i, "C" + i));
            }
        }
        void LoadWelfareCodes()
        {
            ddlWelfareCode.Items.Clear();
            ddlWelfareCode.Items.Add(new ListItem("-- Select Code --", ""));

            for (int i = 1; i <= 100; i++)
            {
                ddlWelfareCode.Items.Add(new ListItem("R" + i, "R" + i));
            }
        }
        void ClearForm()
        {
            ddlQualifier.SelectedIndex = 0;
            ddlWelfareCode.SelectedIndex = 0;
            ddlDepCode.SelectedIndex = 0;

            txtBillFrom.Text = "";
            txtBillTo.Text = "";
            txtDepFrom.Text = "";
            txtDepTo.Text = "";
            txtPerFrom.Text = "";
            txtPerTo.Text = "";
        }
    }
}