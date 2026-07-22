<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="case_sent_from_billing.aspx.cs" Inherits="welfareSystem.case_sent_from_billing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">
    <div class="page-title">
            Patient Received in Welfare
        </div>
     <div class="card-box">
    <asp:GridView ID="gvPatients"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table"
                GridLines="None"
                AllowPaging="true"
                PageSize="10"
                OnPageIndexChanging="gvPatients_PageIndexChanging"
                OnRowCommand="gvPatients_RowCommand">

        <Columns>
             <asp:TemplateField HeaderText="Sr.No">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
            <%--<asp:BoundField DataField="RequestID" HeaderText="ID" />--%>
            <asp:BoundField DataField="AdmNo" HeaderText="Adm No" />
            <asp:BoundField DataField="MRNo" HeaderText="MR No" />
            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
            <asp:BoundField DataField="Age" HeaderText="Age" />
            <asp:BoundField DataField="City" HeaderText="City" />

            <asp:BoundField DataField="BillAmount" HeaderText="Bill" />
            <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" />
            <asp:BoundField DataField="ReceivableAmount" HeaderText="Receivable" />

            <asp:BoundField DataField="Status" HeaderText="Status" />

            <asp:TemplateField HeaderText="Action">
            <ItemTemplate>

                <asp:Button ID="btnApprove" runat="server"
                    Text="Receive"
                    CssClass="btn btn-success btn-sm"
                    CommandName="Approve"
                    CommandArgument='<%# Eval("RequestID") %>'
                    Enabled='<%# Eval("Status").ToString() == "In Process" %>' />
                <div hidden>
                <asp:Button ID="btnReject" runat="server"
                    Text="Reject"
                    CssClass="btn btn-danger btn-sm"
                    CommandName="Reject"
                    CommandArgument='<%# Eval("RequestID") %>'
                    Enabled='<%# Eval("Status").ToString() == "In Process" %>' />
                </div>
                <br />

                <asp:TextBox ID="txtComment" runat="server"
                    CssClass="form-control mt-2"
                    Placeholder="Enter Comment..."
                    Visible="false">
                </asp:TextBox>

            </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>
 </div>
</div>
<style>
        body {
            background: #f1f5f9;
        }

        .page-title {
            font-size: 32px;
            font-weight: 700;
            color: #0f172a;
            margin-bottom: 25px;
        }

        .card-box {
            background: white;
            border-radius: 20px;
            padding: 25px;
            box-shadow: 0 5px 25px rgba(0,0,0,0.08);
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            overflow: hidden;
            border-radius: 15px;
        }

        .table th {
            background: #0f172a;
            color: white;
            padding: 14px;
            text-align: center;
            font-size: 14px;
        }

        .table td {
            padding: 14px;
            text-align: center;
            border-bottom: 1px solid #e2e8f0;
            font-size: 14px;
            color: #334155;
            vertical-align: middle;
        }

        .table tr:hover {
            background: #f8fafc;
            transition: 0.3s;
        }

        .approve-btn {
            background: linear-gradient(135deg,#16a34a,#15803d);
            color: white;
            border: none;
            padding: 8px 15px;
            border-radius: 10px;
            font-weight: 600;
            transition: 0.3s;
        }

        .approve-btn:hover {
            transform: scale(1.05);
        }

        .reject-btn {
            background: linear-gradient(135deg,#dc2626,#b91c1c);
            color: white;
            border: none;
            padding: 8px 15px;
            border-radius: 10px;
            font-weight: 600;
            transition: 0.3s;
        }

        .reject-btn:hover {
            transform: scale(1.05);
        }

        .form-control {
            border-radius: 10px;
            border: 1px solid #cbd5e1;
        }

        .amount {
            font-weight: bold;
            color: #059669;
        }

        .status {
            font-weight: 700;
            color: #2563eb;
        }

        .btn[disabled] {
            opacity: 0.5;
            cursor: not-allowed;
        }

        /* Paging Design */
        .table a,
        .table span {
            padding: 7px 14px;
            margin: 3px;
            border-radius: 8px;
            text-decoration: none;
            font-weight: 600;
        }

        .table a {
            background: #2563eb;
            color: white;
        }

        .table span {
            background: #0f172a;
            color: white;
        }
    .btn[disabled] {
    opacity: 0.6;
    cursor: not-allowed;
}
</style>
</asp:Content>
