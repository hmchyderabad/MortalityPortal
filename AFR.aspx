<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
AutoEventWireup="true"
CodeBehind="AFR.aspx.cs"
Inherits="welfareSystem.AFR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

    <div class="card shadow p-4">
        <h3 class="mb-4 text-primary">Assessment Findings & Recommendation</h3>

        <div class="row g-3">

            <div class="col-md-6">
                <label>Discount in Room Charges</label>
                <asp:DropDownList ID="ddlRoom" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Pharmacy Charges</label>
                <asp:DropDownList ID="ddlPharmacy" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Radiology Charges</label>
                <asp:DropDownList ID="ddlRadiology" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Laboratory Charges</label>
                <asp:DropDownList ID="ddlLaboratory" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Internal Services Charges</label>
                <asp:DropDownList ID="ddlInternal" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Meal Charges</label>
                <asp:DropDownList ID="ddlMeal" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Consultant Charges</label>
                <asp:DropDownList ID="ddlConsultant" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Discount in Procedure Charges</label>
                <asp:DropDownList ID="ddlProcedure" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

        </div>

        <div class="mt-4">
            <asp:Button ID="btnSave" runat="server" Text="Save"
                CssClass="btn btn-success"
                OnClick="btnSave_Click" />

            <asp:Button ID="btnClear" runat="server" Text="Clear"
                CssClass="btn btn-secondary"
                OnClick="btnClear_Click" />
        </div>

        <hr />

        <asp:GridView ID="gvAFR" runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-bordered table-striped"
            DataKeyNames="Id"
            OnRowEditing="gvAFR_RowEditing"
            OnRowCancelingEdit="gvAFR_RowCancelingEdit"
            OnRowUpdating="gvAFR_RowUpdating"
            OnRowDeleting="gvAFR_RowDeleting">

            <Columns>

                <asp:BoundField DataField="RoomCharges" HeaderText="Room" />
                <asp:BoundField DataField="PharmacyCharges" HeaderText="Pharmacy" />
                <asp:BoundField DataField="RadiologyCharges" HeaderText="Radiology" />
                <asp:BoundField DataField="LaboratoryCharges" HeaderText="Lab" />
                <asp:BoundField DataField="InternalServicesCharges" HeaderText="Internal" />
                <asp:BoundField DataField="MealCharges" HeaderText="Meal" />
                <asp:BoundField DataField="ConsultantCharges" HeaderText="Consultant" />
                <asp:BoundField DataField="ProcedureCharges" HeaderText="Procedure" />

                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />

            </Columns>

        </asp:GridView>

    </div>

</div>

</asp:Content>
