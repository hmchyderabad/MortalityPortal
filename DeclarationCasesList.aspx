<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="DeclarationCasesList.aspx.cs"
    Inherits="welfareSystem.DeclarationCasesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        body {
            background: #f4f6f9;
        }

        .page-title {
            font-size: 32px;
            font-weight: 700;
            color: #1e293b;
            margin-bottom: 25px;
        }

        .card-box {
            background: #fff;
            border-radius: 18px;
            padding: 25px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
        }

        .table {
            width: 100%;
            border-collapse: collapse;
        }

        .table th {
            background: #0f172a;
            color: white;
            padding: 14px;
            text-align: center;
            font-size: 15px;
        }

        .table td {
            padding: 14px;
            text-align: center;
            border-bottom: 1px solid #e5e7eb;
            font-size: 14px;
            color: #374151;
        }

        .table tr:hover {
            background: #f8fafc;
            transition: 0.3s;
        }

        .view-btn {
            background: linear-gradient(135deg, #2563eb, #1d4ed8);
            color: white;
            border: none;
            padding: 10px 18px;
            border-radius: 10px;
            cursor: pointer;
            font-weight: 600;
            transition: 0.3s;
        }

        .view-btn:hover {
            transform: scale(1.05);
            background: linear-gradient(135deg, #1d4ed8, #1e40af);
        }

        .amount {
            font-weight: bold;
            color: #059669;
        }
        .table td,
.table th {
    vertical-align: middle;
}

.table a,
.table span {
    padding: 6px 12px;
    margin: 2px;
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
    </style>

    <div class="container-fluid mt-4">

        <div class="page-title">
            Declaration Cases List
        </div>

        <div class="card-box">

            <asp:GridView ID="gvApprovedCases"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table"
                GridLines="None"
                AllowPaging="true"
                PageSize="10"
                OnPageIndexChanging="gvApprovedCases_PageIndexChanging">

                <Columns>

                    <asp:TemplateField HeaderText="Sr.No">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:BoundField DataField="FinalApprovalId"
                        HeaderText="Approval ID" />

                    <asp:BoundField DataField="RequestID"
                        HeaderText="Request ID" />--%>

                    <asp:BoundField DataField="AdmNo"
                        HeaderText="Admission No" />

                    <asp:BoundField DataField="MRNo"
                        HeaderText="MR No" />
                    <asp:BoundField DataField="PatientName"
                        HeaderText="Patient Name" />

                    <asp:BoundField DataField="ApprovedBy"
                        HeaderText="Approved By" />

                    <asp:BoundField DataField="ApprovalDate"
                        HeaderText="Approval Date"
                        DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />

                    <%--<asp:TemplateField HeaderText="Discount">
                        <ItemTemplate>
                            <span class="amount">
                                Rs. <%# Eval("TotalDiscountAmount") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="View Details">
                        <ItemTemplate>

                            <asp:Button ID="btnView"
                                runat="server"
                                Text="View"
                                CssClass="view-btn"
                                CommandArgument='<%# Eval("RequestID") %>'
                                OnClick="btnView_Click" />

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

    </div>

</asp:Content>