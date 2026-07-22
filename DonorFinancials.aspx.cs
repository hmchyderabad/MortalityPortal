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
    public partial class DonorFinancials : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                LoadDonors();
            }

        }


        void LoadDonors()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT DonorId, FirstName AS Name
        FROM Donors", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDonor.DataSource = dt;
                ddlDonor.DataTextField = "Name";
                ddlDonor.DataValueField = "DonorId";
                ddlDonor.DataBind();

                ddlDonor.Items.Insert(0, new ListItem("-- Select Donor --", ""));
            }
        }

        protected void ddlDonor_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT CompanyName FROM Donors WHERE DonorId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", ddlDonor.SelectedValue);

                con.Open();
                object company = cmd.ExecuteScalar();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal muslim = 0, nonMuslim = 0, employee = 0;

            decimal.TryParse(txtMuslim.Text, out muslim);
            decimal.TryParse(txtNonMuslim.Text, out nonMuslim);
            decimal.TryParse(txtEmployee.Text, out employee);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        INSERT INTO DonorFinancials
        (DonorId, WelfareMuslim, WelfareNonMuslim, WelfareEmployee)
        VALUES (@DonorId, @M, @NM, @E)", con);

                cmd.Parameters.AddWithValue("@DonorId", ddlDonor.SelectedValue);
                cmd.Parameters.AddWithValue("@M", muslim);
                cmd.Parameters.AddWithValue("@NM", nonMuslim);
                cmd.Parameters.AddWithValue("@E", employee);
                //cmd.Parameters.AddWithValue("@C", txtCompany.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearForm();
            ShowAlert("Saved Successfully!", "success");
        }
        void ShowAlert(string message, string type)
        {
            string script = $"Swal.fire({{ icon: '{type}', title: '{message}', showConfirmButton: false, timer: 2000 }});";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT df.Id,
               d.FirstName AS DonorName,
               df.WelfareMuslim,
               df.WelfareNonMuslim,
               df.WelfareEmployee
        FROM DonorFinancials df
        INNER JOIN Donors d ON df.DonorId = d.DonorId", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvData.DataSource = dt;
                gvData.DataBind();
            }
        }
        void ClearForm()
        {
            ddlDonor.SelectedIndex = 0;

            //txtCompany.Text = "";
            txtMuslim.Text = "";
            txtNonMuslim.Text = "";
            txtEmployee.Text = "";

            //lblCompany.Text = ""; // reset label

            gvData.EditIndex = -1;
        }
        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvData.EditIndex = e.NewEditIndex;
            LoadData();
        }
        protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvData.EditIndex = -1;
            LoadData();
        }
        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvData.Rows[e.RowIndex];

            TextBox txtMuslim = (TextBox)row.FindControl("txtMuslim");
            TextBox txtNonMuslim = (TextBox)row.FindControl("txtNonMuslim");
            TextBox txtEmployee = (TextBox)row.FindControl("txtEmployee");

            decimal muslim = 0;
            decimal nonMuslim = 0;
            decimal employee = 0;

            decimal.TryParse(txtMuslim.Text.Trim(), out muslim);
            decimal.TryParse(txtNonMuslim.Text.Trim(), out nonMuslim);
            decimal.TryParse(txtEmployee.Text.Trim(), out employee);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        UPDATE DonorFinancials
        SET WelfareMuslim=@M,
            WelfareNonMuslim=@NM,
            WelfareEmployee=@E
        WHERE Id=@Id", con);

                cmd.Parameters.AddWithValue("@M", muslim);
                cmd.Parameters.AddWithValue("@NM", nonMuslim);
                cmd.Parameters.AddWithValue("@E", employee);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvData.EditIndex = -1;
            LoadData();

            ShowAlert("Updated Successfully!", "success");
        }
        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM DonorFinancials WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();

            ShowAlert("Deleted Successfully!", "success");
        }
    }
}