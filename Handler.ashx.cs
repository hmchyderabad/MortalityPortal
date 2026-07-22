using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace welfareSystem
{
    public class Handler : IHttpHandler
    {
        public void ProcessRequest(
            HttpContext context
        )
        {
            string cs =
                ConfigurationManager
                .ConnectionStrings["welfare"]
                .ConnectionString;

            context.Response.Clear();

            context.Response.ContentType =
                "image/png";

            int id =
                Convert.ToInt32(
                    context.Request.QueryString["id"]
                );

            using (
                SqlConnection con =
                new SqlConnection(cs)
            )
            {
                string query = @"

SELECT SignatureData
FROM PatientDeclarations
WHERE ID = @ID
";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@ID",
                    id
                );

                con.Open();

                SqlDataReader dr =
                    cmd.ExecuteReader();

                if (dr.Read())
                {
                    byte[] bytes =
                        (byte[])dr["SignatureData"];

                    context.Response.BinaryWrite(
                        bytes
                    );
                }

                con.Close();
            }

            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}