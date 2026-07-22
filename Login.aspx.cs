using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class Login : System.Web.UI.Page
    {
        private string cs =
            ConfigurationManager
            .ConnectionStrings["welfare"]
            .ConnectionString;
        protected void Page_Load(object sender,EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                Response.Redirect("AdminIndex.aspx");
            }
        }
        protected void btnLogin_Click(
            object sender,
            EventArgs e
        )
        {
            using (SqlConnection con =
                new SqlConnection(cs))
            {
                string query = @"

SELECT
    U.UserId,
    U.FullName,
    U.Username,
    U.RoleId,
    U.IsActive,
    R.RoleName

FROM Users U

INNER JOIN Roles R
ON U.RoleId = R.RoleId

WHERE
    U.Username = @Username
    AND U.Password = @Password
    AND U.IsActive = 1
";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Username",
                    txtUsername.Text.Trim()
                );

                cmd.Parameters.AddWithValue(
                    "@Password",
                    txtPassword.Text.Trim()
                );

                con.Open();

                SqlDataReader dr =
                    cmd.ExecuteReader();

                if (dr.Read())
                {
                    Session["UserId"] =
                        dr["UserId"].ToString();

                    Session["FullName"] =
                        dr["FullName"].ToString();

                    Session["Username"] =
                        dr["Username"].ToString();

                    Session["RoleId"] =
                        dr["RoleId"].ToString();

                    Session["RoleName"] =
                        dr["RoleName"].ToString();

                    Response.Redirect(
                        "AdminIndex.aspx"
                    );
                }
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "msg",
                        "alert('Invalid Username Or Password');",
                        true
                    );
                }
            }
        }
    }
}