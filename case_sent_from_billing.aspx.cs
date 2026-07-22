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
    public partial class case_sent_from_billing : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT 
                            MAX(RequestID) AS RequestID,
                            MRNo,
                            PatientName,
                            MAX(AdmNo) AS AdmNo,
                            MAX(Age) AS Age,
                            MAX(City) AS City,
                            MAX(BillAmount) AS BillAmount,
                            MAX(AdvanceAmount) AS AdvanceAmount,
                            MAX(ReceivableAmount) AS ReceivableAmount,
                            MAX(Status) AS Status
                        FROM Billing_Send_Patient_For_Welfare
                        GROUP BY MRNo, PatientName
                        ORDER BY RequestID DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPatients.DataSource = dt;
                gvPatients.DataBind();
            }
        }


        protected void gvPatients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Ignore paging command
            if (e.CommandName == "Page")
            {
                return;
            }

            int requestId = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;

            TextBox txtComment = (TextBox)row.FindControl("txtComment");
            Button btnReject = (Button)row.FindControl("btnReject");

            if (e.CommandName == "Approve")
            {
                UpdateStatus(requestId, "Approved", "");

                Response.Redirect("DrTransactionForm.aspx?RequestID=" + requestId);
            }

            else if (e.CommandName == "Reject")
            {
                if (!txtComment.Visible)
                {
                    txtComment.Visible = true;
                    btnReject.Text = "Send";
                    return;
                }

                if (txtComment.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", "alert('Enter Comment');", true);

                    txtComment.Visible = true;
                    btnReject.Text = "Send";
                    return;
                }

                UpdateStatus(requestId, "Rejected", txtComment.Text);
            }

            LoadData();
        }

        //protected void gvPatients_RowCommand(object sender, GridViewCommandEventArgs e)
        //    {
        //        int requestId = Convert.ToInt32(e.CommandArgument);

        //        GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;

        //        TextBox txtComment = (TextBox)row.FindControl("txtComment");
        //        Button btnReject = (Button)row.FindControl("btnReject");

        //        if (e.CommandName == "Approve")
        //        {
        //            UpdateStatus(requestId, "Approved", "");
        //            Response.Redirect("WalfareAssessment.aspx?RequestID=" + requestId);
        //        }

        //    else if (e.CommandName == "Reject")
        //        {
        //        if (!txtComment.Visible)
        //        {
        //            txtComment.Visible = true;
        //            btnReject.Text = "Send";
        //            return;
        //        }

        //        if (txtComment.Text.Trim() == "")
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(),
        //                "msg", "alert('Enter Comment');", true);

        //                txtComment.Visible = true;
        //                btnReject.Text = "Send";
        //                return;
        //            }

        //            UpdateStatus(requestId, "Rejected", txtComment.Text);
        //        }

        //        LoadData();
        //    }

        //protected void gvPatients_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int requestId = Convert.ToInt32(e.CommandArgument);

        //    GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
        //    TextBox txtComment = (TextBox)row.FindControl("txtComment");

        //    if (e.CommandName == "Approve")
        //    {
        //        UpdateStatus(requestId, "Approved", "");
        //    }
        //    else if (e.CommandName == "Reject")
        //    {
        //        // Show comment box first
        //        if (!txtComment.Visible)
        //        {
        //            txtComment.Visible = true;
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(txtComment.Text))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter comment');", true);
        //            return;
        //        }

        //        UpdateStatus(requestId, "Rejected", txtComment.Text);
        //    }

        //    LoadData();
        //}

        void UpdateStatus(int id, string status, string comment)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
        UPDATE Billing_Send_Patient_For_Welfare 
        SET Status=@Status, Comments=@Comments
        WHERE RequestID=@ID AND Status = 'In Process'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Comments", comment);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void gvPatients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPatients.PageIndex = e.NewPageIndex;

            LoadData();
        }
    }
}