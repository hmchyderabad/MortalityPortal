<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="EmployeeDiscount.aspx.cs" Inherits="welfareSystem.EmployeeDiscount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

    <h3>Employee Discount Master</h3>
     <div class="row justify-content-center">
        <div class="col-xl-10">
            <div class="card shadow-sm border-0 overflow-hidden">
                <div style="height: 4px; background: linear-gradient(90deg, #c82333, #dc3545);"></div>
    <div class="row mb-3">

        <!-- Donor -->
        <%--<div class="col-md-3">
            <label>Donor</label>
            <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-select"></asp:DropDownList>
        </div>--%>

        <!-- Employee Type -->
        <div class="col-md-3">
            <label>Employee Type</label>
            <asp:DropDownList ID="ddlEmployeeType" runat="server" CssClass="form-select">
                <asp:ListItem>Full Time Permanent</asp:ListItem>
                <asp:ListItem>Full Time Contractual</asp:ListItem>
                <asp:ListItem>Part Time & Daily Wager</asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Treatment -->
        <div class="col-md-3">
            <label>Treatment</label>
            <asp:DropDownList ID="ddlTreatment" runat="server" CssClass="form-select">
                <asp:ListItem>Admission</asp:ListItem>
                <asp:ListItem>Laboratory</asp:ListItem>
                <asp:ListItem>Radiology</asp:ListItem>
                <asp:ListItem>Pharmacy</asp:ListItem>
                <asp:ListItem>Consultation</asp:ListItem>
                <asp:ListItem>Physiotherapy</asp:ListItem>
                <asp:ListItem>OT Room Charges</asp:ListItem>
                <asp:ListItem>Procedure (Hospital Share)</asp:ListItem>
                <asp:ListItem>Procedure (Consultant Share)</asp:ListItem>
                <asp:ListItem>Dental Services</asp:ListItem>
                <asp:ListItem>Emergency Consultation</asp:ListItem>
                <asp:ListItem>Cosmetic Services</asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Relation -->
        <div class="col-md-3">
            <label>Relation</label>
            <asp:DropDownList ID="ddlRelation" runat="server" CssClass="form-select">
                <asp:ListItem>Self</asp:ListItem>
                <asp:ListItem>Spouse</asp:ListItem>
                <asp:ListItem>Children</asp:ListItem>
                <asp:ListItem>Parent</asp:ListItem>
                <asp:ListItem>Sibling</asp:ListItem>
            </asp:DropDownList>
        </div>

    </div>

    <!-- Discount -->
    <div class="row mb-3">
        <div class="col-md-3">
            <label>Discount %</label>
            <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>

    <!-- Buttons -->
    <asp:Button ID="btnSave" runat="server" Text="Save"
        CssClass="btn btn-primary" OnClick="btnSave_Click" />

    <asp:Button ID="btnClear" runat="server" Text="Clear"
        CssClass="btn btn-secondary" OnClick="btnClear_Click" />

    <hr />

    <!-- GRID -->
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false"
        DataKeyNames="Id"
        OnRowEditing="gv_RowEditing"
        OnRowUpdating="gv_RowUpdating"
        OnRowCancelingEdit="gv_RowCancelingEdit"
        OnRowDeleting="gv_RowDeleting"
        CssClass="table table-bordered">

        <Columns>
            <asp:BoundField DataField="EmployeeType" HeaderText="Employee Type" />
            <asp:BoundField DataField="Treatment" HeaderText="Treatment" />
            <asp:BoundField DataField="Relation" HeaderText="Relation" />
            <asp:TemplateField HeaderText="Discount %">
    <ItemTemplate>
        <%# Eval("DiscountPercent") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtDiscount" runat="server"
            Text='<%# Bind("DiscountPercent") %>' />
    </EditItemTemplate>
</asp:TemplateField>

            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</div>
            </div>
         </div>
</div>

</asp:Content>
