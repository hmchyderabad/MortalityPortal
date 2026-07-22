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
    public partial class IncomeCategory : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {

            con = new SqlConnection(cs);

            if (Request.QueryString["delete"] != null)
            {
                string id = Request.QueryString["delete"].ToString();
                DeleteRecord(id);

                // 🔥 IMPORTANT: redirect to remove query string
                Response.Redirect("IncomeCategory.aspx");
            }

            if (!IsPostBack)
            {
                LoadData();
            }

        }

        // LOAD GRID
        void LoadData()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from HouseholdIncomeCategory", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // SAVE OR UPDATE
        protected void btnSave_Click(object sender, EventArgs e)
        {

            con = new SqlConnection(cs);

            if (hfCode.Value == "") // INSERT
            {
                SqlCommand cmd = new SqlCommand("insert into HouseholdIncomeCategory(Category,Description,IncomeMin,IncomeMax) values(@c,@d,@min,@max)", con);

                cmd.Parameters.AddWithValue("@c", ddlCategory.Text);
                cmd.Parameters.AddWithValue("@d", txtDescription.Text);
                cmd.Parameters.AddWithValue("@min", txtMin.Text);
                cmd.Parameters.AddWithValue("@max", txtMax.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else // UPDATE
            {
                SqlCommand cmd = new SqlCommand("update HouseholdIncomeCategory set Category=@c,Description=@d,IncomeMin=@min,IncomeMax=@max where Code=@id", con);

                cmd.Parameters.AddWithValue("@c", ddlCategory.Text);
                cmd.Parameters.AddWithValue("@d", txtDescription.Text);
                cmd.Parameters.AddWithValue("@min", txtMin.Text);
                cmd.Parameters.AddWithValue("@max", txtMax.Text);
                cmd.Parameters.AddWithValue("@id", hfCode.Value);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                hfCode.Value = "";
            }

            ClearForm();
            LoadData();

        }

        // DELETE
        void DeleteRecord(string id)
        {
            con = new SqlConnection(cs);

            SqlCommand cmd = new SqlCommand("DELETE FROM HouseholdIncomeCategory WHERE Code=@id", con);
            cmd.Parameters.AddWithValue("@id", id); // ✅ FIXED

            con.Open();
            int rows = cmd.ExecuteNonQuery();
            con.Close();
        }

        // CLEAR FORM
        void ClearForm()
        {
            ddlCategory.SelectedIndex = 0;
            txtDescription.Text = "";
            txtMin.Text = "";
            txtMax.Text = "";
        }
    }
}