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
    public partial class DonorDetails : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
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

        void LoadGrid()
        {

            SqlConnection con = new SqlConnection(cs);

            SqlDataAdapter da = new SqlDataAdapter("select * from Donors", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd;

            if (hfId.Value == "")
            {
                cmd = new SqlCommand(@"
        INSERT INTO Donors
        (FirstName, Phone, Address, City, Country)
        VALUES
        (@f,@p,@a,@c,@co)", con);
            }
            else
            {
                cmd = new SqlCommand(@"
        UPDATE Donors SET
        FirstName=@f,
        Phone=@p,
        Address=@a,
        City=@c,
        Country=@co
        WHERE DonorId=@id", con);

                cmd.Parameters.AddWithValue("@id", hfId.Value);
            }

            cmd.Parameters.AddWithValue("@f", txtFirstName.Text);
            cmd.Parameters.AddWithValue("@p", txtPhone.Text);
            cmd.Parameters.AddWithValue("@a", txtAddress.Text);
            cmd.Parameters.AddWithValue("@c", txtCity.Text);
            cmd.Parameters.AddWithValue("@co", txtCountry.Text);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            ClearForm();
            LoadGrid();
        }

        void LoadEdit(string id)
        {

            SqlConnection con = new SqlConnection(cs);

            SqlCommand cmd = new SqlCommand("select * from Donors where DonorId=@id", con);

            cmd.Parameters.AddWithValue("@id", id);

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {

                hfId.Value = id;

                txtFirstName.Text = dr["FirstName"].ToString();
                //txtLastName.Text = dr["LastName"].ToString();
                //txtMobile.Text = dr["Mobile"].ToString();
                txtPhone.Text = dr["Phone"].ToString();
                txtAddress.Text = dr["Address"].ToString();
                txtCity.Text = dr["City"].ToString();
                txtCountry.Text = dr["Country"].ToString();
                //txtOccupation.Text = dr["CompanyName"].ToString();

            }

            con.Close();

        }

        void DeleteRecord(string id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    SqlCommand cmd1 = new SqlCommand(
                        "DELETE FROM DonorFinancials WHERE DonorId=@id", con, tran);
                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand(
                        "DELETE FROM ApprovalLimit WHERE DonorId=@id", con, tran);
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand(
                        "DELETE FROM Donors WHERE DonorId=@id", con, tran);
                    cmd3.Parameters.AddWithValue("@id", id);
                    cmd3.ExecuteNonQuery();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Unable to delete donor. Related records exist.');", true);
                }
            }

            LoadGrid();
        }
        void ClearForm()
        {

            txtFirstName.Text = "";
            //txtLastName.Text = "";
            //txtMobile.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            //txtOccupation.Text = "";

        }
    }
}