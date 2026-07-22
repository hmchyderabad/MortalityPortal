using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace welfareSystem
{
    public partial class Signup : System.Web.UI.Page
    {
        string cs =
            ConfigurationManager
            .ConnectionStrings["welfare"]
            .ConnectionString;

        protected void Page_Load(
            object sender,
            EventArgs e
        )
        {
            if (!IsPostBack)
            {
                LoadRoles();
                LoadUsers();
            }
        }

        void LoadRoles()
        {
            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query =
                    "SELECT * FROM Roles";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                con.Open();

                ddlRole.DataSource =
                    cmd.ExecuteReader();

                ddlRole.DataTextField =
                    "RoleName";

                ddlRole.DataValueField =
                    "RoleId";

                ddlRole.DataBind();

                con.Close();
            }
        }

        void LoadUsers()
        {
            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

SELECT
    Users.UserId,
    Users.FullName,
    Users.Username,
    Users.Password,
    Roles.RoleName

FROM Users

INNER JOIN Roles
ON Users.RoleId = Roles.RoleId
";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, con);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                gvUsers.DataSource = dt;

                gvUsers.DataBind();
            }
        }

        protected void btnSignup_Click(
            object sender,
            EventArgs e
        )
        {
            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

                    INSERT INTO Users
                    (
                        FullName,
                        Username,
                        Password,
                        RoleId
                    )

                    VALUES
                    (
                        @FullName,
                        @Username,
                        @Password,
                        @RoleId
                    )";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FullName",
                    txtName.Text
                );

                cmd.Parameters.AddWithValue(
                    "@Username",
                    txtUsername.Text
                );

                cmd.Parameters.AddWithValue(
                    "@Password",
                    txtPassword.Text
                );

                cmd.Parameters.AddWithValue(
                    "@RoleId",
                    ddlRole.SelectedValue
                );

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                LoadUsers();

                Response.Write(
                    "<script>alert('Signup Success');</script>"
                );
            }
        }

        protected void gvUsers_RowEditing(
            object sender,
            GridViewEditEventArgs e
        )
        {
            gvUsers.EditIndex = e.NewEditIndex;

            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(
            object sender,
            GridViewCancelEditEventArgs e
        )
        {
            gvUsers.EditIndex = -1;

            LoadUsers();
        }

        protected void gvUsers_RowUpdating(
            object sender,
            GridViewUpdateEventArgs e
        )
        {
            int userId = Convert.ToInt32(
                gvUsers.DataKeys[e.RowIndex].Value
            );

            GridViewRow row =
                gvUsers.Rows[e.RowIndex];

            TextBox txtFullName =(TextBox)row.Cells[2].Controls[0];

            TextBox txtUsername =(TextBox)row.Cells[3].Controls[0];

            TextBox txtPassword =(TextBox)row.Cells[4].Controls[0];

            string fullName = txtFullName.Text;

            string username = txtUsername.Text;

            string password = txtPassword.Text;

            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

                    UPDATE Users

                    SET
                        FullName = @FullName,
                        Username = @Username,
                        Password = @Password

                    WHERE UserId = @UserId
                    ";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FullName",
                    fullName
                );

                cmd.Parameters.AddWithValue(
                    "@Username",
                    username
                );

                cmd.Parameters.AddWithValue(
                    "@Password",
                    password
                );

                cmd.Parameters.AddWithValue(
                    "@UserId",
                    userId
                );

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }

            gvUsers.EditIndex = -1;

            LoadUsers();
        }

        protected void gvUsers_RowDeleting(
            object sender,
            GridViewDeleteEventArgs e
        )
        {
            int userId = Convert.ToInt32(
                gvUsers.DataKeys[e.RowIndex].Value
            );

            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query =
                    "DELETE FROM Users WHERE UserId=@UserId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@UserId",
                    userId
                );

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }

            LoadUsers();
        }
    }
}