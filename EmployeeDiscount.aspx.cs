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
    public partial class EmployeeDiscount : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                //LoadDonors();
            }
        }
        //void LoadDonors()
        //{
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(@"
        //SELECT DonorId, FirstName + ' ' + LastName AS Name FROM Donors", con);

        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        ddlDonor.DataSource = dt;
        //        ddlDonor.DataTextField = "Name";
        //        ddlDonor.DataValueField = "DonorId";
        //        ddlDonor.DataBind();

        //        ddlDonor.Items.Insert(0, "-- Select Donor --");
        //    }
        //}
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT ed.Id,
               ed.EmployeeType,
               ed.Treatment,
               ed.Relation,
               ed.DiscountPercent
        FROM EmployeeDiscount ed", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvData.DataSource = dt;
                gvData.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal discount;

            if (!decimal.TryParse(txtDiscount.Text, out discount))
            {
                ShowAlert("Enter valid discount!", "error");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        INSERT INTO EmployeeDiscount
        (EmployeeType, Treatment, Relation, DiscountPercent)
        VALUES (@Emp, @Treat, @Rel, @Disc)", con);

                //cmd.Parameters.AddWithValue("@Donor", ddlDonor.SelectedValue);
                cmd.Parameters.AddWithValue("@Emp", ddlEmployeeType.SelectedValue);
                cmd.Parameters.AddWithValue("@Treat", ddlTreatment.SelectedValue);
                cmd.Parameters.AddWithValue("@Rel", ddlRelation.SelectedValue);
                cmd.Parameters.AddWithValue("@Disc", discount);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearForm();
            ShowAlert("Saved!", "success");
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

            TextBox txtDiscount = (TextBox)row.FindControl("txtDiscount");

            decimal discount = 0;
            decimal.TryParse(txtDiscount.Text.Trim(), out discount);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        UPDATE EmployeeDiscount 
        SET DiscountPercent=@D 
        WHERE Id=@Id", con);

                cmd.Parameters.AddWithValue("@D", discount);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvData.EditIndex = -1;
            LoadData();
            ShowAlert("Updated!", "success");
        }
        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EmployeeDiscount WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ShowAlert("Deleted!", "success");
        }
        void ClearForm()
        {
            //ddlDonor.SelectedIndex = 0;
            ddlEmployeeType.SelectedIndex = 0;
            ddlTreatment.SelectedIndex = 0;
            ddlRelation.SelectedIndex = 0;

            txtDiscount.Text = "";
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        void ShowAlert(string message, string type)
        {
            string script = $"Swal.fire({{ icon: '{type}', title: '{message}', showConfirmButton: false, timer: 2000 }});";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }
    }
}