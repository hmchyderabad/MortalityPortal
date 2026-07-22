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
    public partial class Donations : System.Web.UI.Page
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

        // 🔹 LOAD DONORS DROPDOWN
        void LoadDonors()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT DonorId, (FirstName) AS FullName 
        FROM Donors", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDonor.DataSource = dt;
                ddlDonor.DataTextField = "FullName"; // 👈 Name show hoga
                ddlDonor.DataValueField = "DonorId"; // 👈 ID save hogi
                ddlDonor.DataBind();

                ddlDonor.Items.Insert(0, new ListItem("-- Select Donor --", ""));
            }
        }

        // 🔹 LOAD GRID
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT d.Id,
               (dn.FirstName) AS DonorName,
               d.Amount,
               d.DonationDate
        FROM Donations d
        INNER JOIN Donors dn ON d.DonorId = dn.DonorId", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDonations.DataSource = dt;
                gvDonations.DataBind();
            }
        }

        // 🔹 INSERT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlDonor.SelectedIndex == 0)
            {
                ShowAlert("Please select donor!", "warning");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        INSERT INTO Donations (DonorId, Amount, DonationDate)
        VALUES (@DonorId, @Amount, @Date)", con);

                cmd.Parameters.AddWithValue("@DonorId", ddlDonor.SelectedValue);
                cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Text));
                cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearForm();
            ShowAlert("Saved Successfully!", "success");
        }

        // 🔹 DELETE
        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvDonations.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Donations WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ShowAlert("Deleted!", "success");
        }

        // 🔹 EDIT
        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDonations.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDonations.EditIndex = -1;
            LoadData();
        }



        // 🔹 UPDATE
        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvDonations.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvDonations.Rows[e.RowIndex];

            decimal amount = Convert.ToDecimal(((TextBox)row.Cells[1].Controls[0]).Text);
            DateTime date = Convert.ToDateTime(((TextBox)row.Cells[2].Controls[0]).Text);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        UPDATE Donations 
        SET Amount=@Amount, DonationDate=@Date
        WHERE Id=@Id", con);

                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvDonations.EditIndex = -1;
            LoadData();

            ShowAlert("Updated Successfully!", "success");
        }
        void ShowAlert(string message, string type)
        {
            string script = $"Swal.fire({{ icon: '{type}', title: '{message}', showConfirmButton: false, timer: 2000 }});";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }
        void ClearForm()
        {
            ddlDonor.SelectedIndex = 0;

            txtAmount.Text = "";
            txtDate.Text = "";

            // optional: today date set
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}