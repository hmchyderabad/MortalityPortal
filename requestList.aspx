<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="requestList.aspx.cs"
    Inherits="welfareSystem.requestList" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

<style>

    .card {
        background: #fff;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0px 0px 10px #ddd;
    }

    .heading {
        font-size: 24px;
        font-weight: bold;
        margin-bottom: 20px;
        color: black;
    }

    .table th {
        background: black;
        color: white;
    }

</style>

<div class="card">

    <div class="heading">
        Welfare Requests
    </div>

    <asp:GridView ID="gvRequests"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-bordered table-striped"
        OnRowCommand="gvRequests_RowCommand">

        <Columns>

            <asp:BoundField DataField="RequestID"
                HeaderText="Request ID" />

            <asp:BoundField DataField="AdmNo"
                HeaderText="Adm No" />

            <asp:BoundField DataField="MRNo"
                HeaderText="MR No" />

            <asp:BoundField DataField="PatientName"
                HeaderText="Patient Name" />

            <asp:BoundField DataField="City"
                HeaderText="City" />

            <asp:BoundField DataField="ReceivableAmount"
                HeaderText="Receivable Amount" />

            <asp:BoundField DataField="Status"
                HeaderText="Status" />

            <asp:TemplateField HeaderText="Action">

                <ItemTemplate>

                    <asp:Button ID="btnView"
                        runat="server"
                        Text="View"
                        CssClass="btn btn-primary btn-sm"
                        CommandName="ViewRequest"
                        CommandArgument='<%# Eval("RequestID") %>' />

                </ItemTemplate>

            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

</asp:Content>