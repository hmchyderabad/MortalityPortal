using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace welfareSystem
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string cs =
            ConfigurationManager
            .ConnectionStrings["welfare"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

                lblUser.Text =
                    Session["FullName"].ToString();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Trim() == "" ||
                txtConfirmPassword.Text.Trim() == "")
            {
                ShowSuccessMessage("Fill all fields");
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                ShowSuccessMessage("Passwords do not match");
                return;
            }

            using (SqlConnection con =
                new SqlConnection(cs))
            {
                string checkQuery = @"

SELECT *
FROM Users
WHERE UserId=@UserId
AND Password=@Password
";

                SqlCommand checkCmd =
                    new SqlCommand(checkQuery, con);

                checkCmd.Parameters.AddWithValue(
                    "@UserId",
                    Session["UserId"]
                );

                checkCmd.Parameters.AddWithValue(
                    "@Password",
                    txtCurrentPassword.Text.Trim()
                );

                con.Open();

                SqlDataReader dr =
                    checkCmd.ExecuteReader();

                if (dr.Read())
                {
                    dr.Close();

                    string updateQuery = @"

UPDATE Users
SET Password=@NewPassword
WHERE UserId=@UserId
";

                    SqlCommand updateCmd =
                        new SqlCommand(updateQuery, con);

                    updateCmd.Parameters.AddWithValue(
                        "@NewPassword",
                        txtNewPassword.Text.Trim()
                    );

                    updateCmd.Parameters.AddWithValue(
                        "@UserId",
                        Session["UserId"]
                    );

                    updateCmd.ExecuteNonQuery();

                    ShowSuccessMessage("Password Reset Successfully");
                }
                else
                {
                    ShowSuccessMessage("Current Password Incorrect");
                }
            }
        }

        private void ShowSuccessMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "msg",
                "alert('" + msg + "'); window.location='Login.aspx';",
                true
            );
        }

        private void ShowErrorMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "msg",
                "alert('" + msg + "');",
                true
            );
        }
    }
}