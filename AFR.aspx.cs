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
    public partial class AFR : System.Web.UI.Page
    {
        private string cs => ConfigurationManager.ConnectionStrings["welfare"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadData();
            }
        }

        // ---------------- LOAD DROPDOWN (MASTER DATA) ----------------
        void LoadDropdowns()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT Id, DiscountName, DiscountValue 
                    FROM DiscountMaster", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                BindDropDown(ddlRoom, dt);
                BindDropDown(ddlPharmacy, dt);
                BindDropDown(ddlRadiology, dt);
                BindDropDown(ddlLaboratory, dt);
                BindDropDown(ddlInternal, dt);
                BindDropDown(ddlMeal, dt);
                BindDropDown(ddlConsultant, dt);
                BindDropDown(ddlProcedure, dt);
            }
        }

        void BindDropDown(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "DiscountName";
            ddl.DataValueField = "Id";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        // ---------------- CREATE ----------------
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO AFRMaster
                    (RoomCharges, PharmacyCharges, RadiologyCharges, LaboratoryCharges,
                     InternalServicesCharges, MealCharges, ConsultantCharges, ProcedureCharges)
                    VALUES
                    (@Room, @Pharmacy, @Radiology, @Lab, @Internal, @Meal, @Consultant, @Procedure)", con);

                cmd.Parameters.AddWithValue("@Room", ddlRoom.SelectedValue);
                cmd.Parameters.AddWithValue("@Pharmacy", ddlPharmacy.SelectedValue);
                cmd.Parameters.AddWithValue("@Radiology", ddlRadiology.SelectedValue);
                cmd.Parameters.AddWithValue("@Lab", ddlLaboratory.SelectedValue);
                cmd.Parameters.AddWithValue("@Internal", ddlInternal.SelectedValue);
                cmd.Parameters.AddWithValue("@Meal", ddlMeal.SelectedValue);
                cmd.Parameters.AddWithValue("@Consultant", ddlConsultant.SelectedValue);
                cmd.Parameters.AddWithValue("@Procedure", ddlProcedure.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearForm();
        }

        // ---------------- READ ----------------
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT * FROM AFRMaster ORDER BY Id DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAFR.DataSource = dt;
                gvAFR.DataBind();
            }
        }

        // ---------------- DELETE ----------------
        protected void gvAFR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvAFR.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM AFRMaster WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadData();
        }

        // ---------------- EDIT ----------------
        protected void gvAFR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAFR.EditIndex = e.NewEditIndex;
            LoadData();
        }

        // ---------------- CANCEL EDIT ----------------
        protected void gvAFR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAFR.EditIndex = -1;
            LoadData();
        }

        // ---------------- UPDATE ----------------
        protected void gvAFR_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvAFR.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvAFR.Rows[e.RowIndex];

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE AFRMaster SET
                    RoomCharges=@Room,
                    PharmacyCharges=@Pharmacy,
                    RadiologyCharges=@Radiology,
                    LaboratoryCharges=@Lab,
                    InternalServicesCharges=@Internal,
                    MealCharges=@Meal,
                    ConsultantCharges=@Consultant,
                    ProcedureCharges=@Procedure
                    WHERE Id=@Id", con);

                cmd.Parameters.AddWithValue("@Room", ((TextBox)row.Cells[0].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Pharmacy", ((TextBox)row.Cells[1].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Radiology", ((TextBox)row.Cells[2].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Lab", ((TextBox)row.Cells[3].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Internal", ((TextBox)row.Cells[4].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Meal", ((TextBox)row.Cells[5].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Consultant", ((TextBox)row.Cells[6].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Procedure", ((TextBox)row.Cells[7].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvAFR.EditIndex = -1;
            LoadData();
        }

        // ---------------- CLEAR ----------------
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        void ClearForm()
        {
            ddlRoom.SelectedIndex = 0;
            ddlPharmacy.SelectedIndex = 0;
            ddlRadiology.SelectedIndex = 0;
            ddlLaboratory.SelectedIndex = 0;
            ddlInternal.SelectedIndex = 0;
            ddlMeal.SelectedIndex = 0;
            ddlConsultant.SelectedIndex = 0;
            ddlProcedure.SelectedIndex = 0;
        }
    }
}