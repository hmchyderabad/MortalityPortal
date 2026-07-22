<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="DonorFinancials.aspx.cs" Inherits="welfareSystem.DonorFinancials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

    <h3>Donor Financials</h3>
    <div class="row justify-content-center">
        <div class="col-xl-10">
            <div class="card shadow-sm border-0 overflow-hidden">
                <div style="height: 4px; background: linear-gradient(90deg, #c82333, #dc3545);"></div>
    <div class="row mb-3">

        <div class="col-md-4">
            <label>Donor</label>
            <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-select"
                AutoPostBack="true" OnSelectedIndexChanged="ddlDonor_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

       <%-- <div class="col-md-4">
            <label>Company</label>
            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>--%>

    </div>

    <div class="row mb-3">

        <div class="col-md-4">
            <label>Welfare (Muslims)</label>
            <asp:TextBox ID="txtMuslim" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-4">
            <label>Welfare (Non Muslims)</label>
            <asp:TextBox ID="txtNonMuslim" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="col-md-4">
            <label>Welfare Employee(HMC)</label>
            <asp:TextBox ID="txtEmployee" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

    </div>

    <asp:Button ID="btnSave" runat="server" Text="Save"
        CssClass="btn btn-primary" OnClick="btnSave_Click" />

    <hr />

    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false"
    DataKeyNames="Id"
    OnRowEditing="gv_RowEditing"
    OnRowUpdating="gv_RowUpdating"
    OnRowCancelingEdit="gv_RowCancelingEdit"
    OnRowDeleting="gv_RowDeleting"
    CssClass="table table-bordered">

    <Columns>

        <asp:BoundField DataField="DonorName" HeaderText="Donor" ReadOnly="true" />

        <asp:TemplateField HeaderText="Muslim">
            <ItemTemplate>
                <%# Eval("WelfareMuslim") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtMuslim" runat="server"
                    Text='<%# Bind("WelfareMuslim") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Non Muslim">
            <ItemTemplate>
                <%# Eval("WelfareNonMuslim") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtNonMuslim" runat="server"
                    Text='<%# Bind("WelfareNonMuslim") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Employee">
            <ItemTemplate>
                <%# Eval("WelfareEmployee") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEmployee" runat="server"
                    Text='<%# Bind("WelfareEmployee") %>' />
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
