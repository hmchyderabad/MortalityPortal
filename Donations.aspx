<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="Donations.aspx.cs" Inherits="welfareSystem.Donations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

    <h3>Donations Entry</h3>
    <div class="row justify-content-center">
        <div class="col-xl-10">
            <div class="card shadow-sm border-0 overflow-hidden">
                <div style="height: 4px; background: linear-gradient(90deg, #c82333, #dc3545);"></div>
    <div class="row mb-3">
        <div class="col-md-4">
            <label>Donor Name</label>
            <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-select"></asp:DropDownList>
        </div>

        <div class="col-md-4">
            <label>Amount</label>
            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-4">
            <label>Date</label>
            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
        </div>
    </div>

    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
        OnClick="btnSave_Click" />

    <hr />

    <!-- GRID -->
    <asp:GridView ID="gvDonations" runat="server" AutoGenerateColumns="false"
    DataKeyNames="Id"
    OnRowEditing="gv_RowEditing"
    OnRowCancelingEdit="gv_RowCancelingEdit"
    OnRowUpdating="gv_RowUpdating"
    OnRowDeleting="gv_RowDeleting"
    CssClass="table table-bordered">

    <Columns>
        <asp:BoundField DataField="DonorName" HeaderText="Donor" ReadOnly="true" />
        <asp:BoundField DataField="Amount" HeaderText="Amount" />
        <asp:BoundField DataField="DonationDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />

        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
    </Columns>
</asp:GridView>
</div>
            </div>
        </div>
</div>

</asp:Content>