<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewFormList.aspx.cs" MasterPageFile="~/AdminMaster.Master" Inherits="welfareSystem.Mortality_Module.ReviewFormList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .certificate-card {
            background: linear-gradient(135deg, #f5f7fa 0%, #fff 100%);
            border-radius: 20px;
            box-shadow: 0 20px 35px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }
        .cert-header {
            background: #1a5a4c;
            padding: 20px 30px;
            color: white;
        }
        .info-card {
            background: white;
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
        }
        .grid-container {
            overflow-x: auto;
        }
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
        }
        .grid-view th {
            background: #1a5a4c;
            color: white;
            padding: 12px;
            text-align: left;
            font-weight: 600;
        }
        .grid-view td {
            padding: 10px;
            border-bottom: 1px solid #dee2e6;
        }
        .grid-view tr:hover {
            background: #f8f9fa;
        }
        .action-buttons {
            display: flex;
            gap: 5px;
            flex-wrap: wrap;
        }
        .btn-view, .btn-edit, .btn-delete, .btn-print {
            padding: 5px 10px;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            display: inline-block;
        }
        .btn-view {
            background: #17a2b8;
            color: white;
        }
        .btn-edit {
            background: #ffc107;
            color: #333;
        }
        .btn-delete {
            background: #dc3545;
            color: white;
        }
        .btn-print {
            background: #28a745;
            color: white;
        }
        .btn-view:hover, .btn-edit:hover, .btn-delete:hover, .btn-print:hover {
            opacity: 0.8;
        }
        .alert {
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 5px;
        }
        .alert-success {
            background: #d4edda;
            color: #155724;
            border-left: 4px solid #28a745;
        }
        .alert-danger {
            background: #f8d7da;
            color: #721c24;
            border-left: 4px solid #dc3545;
        }
        .search-section {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 20px;
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
            align-items: flex-end;
        }
        .search-group {
            flex: 1;
            min-width: 200px;
        }
        .search-group label {
            display: block;
            font-weight: 600;
            margin-bottom: 5px;
            color: #333;
            font-size: 14px;
        }
        .search-group input, .search-group select {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }
        .btn-search, .btn-reset {
            padding: 8px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
        }
        .btn-search {
            background: #007bff;
            color: white;
        }
        .btn-reset {
            background: #6c757d;
            color: white;
        }
    </style>

    <div class="container mt-4">
        <!-- Page Header -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="certificate-card">
                    <div class="cert-header d-flex justify-content-between align-items-center">
                        <div>
                            <h3 class="mb-0">
                                <i class="fas fa-clipboard-list"></i> Death Certificate Review Forms
                            </h3>
                            <small>Mortality Registry - Hashim Medical City</small>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="info-card">
            <asp:Panel ID="pnlMessage" runat="server" CssClass="alert" Visible="false">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:Panel>

            <!-- GridView -->
            <div class="grid-container">
                <asp:GridView ID="gvReviewForms" runat="server" AutoGenerateColumns="False" 
                    CssClass="grid-view" OnRowCommand="gvReviewForms_RowCommand"
                    OnPageIndexChanging="gvReviewForms_PageIndexChanging" AllowPaging="True"
                    PageSize="10" EmptyDataText="No review forms found.">
                    <Columns>
                        <asp:BoundField DataField="ReviewID" HeaderText="ID" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="MRNo" HeaderText="M.R. No" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="PatientName" HeaderText="Patient Name" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="DateOfDeath" HeaderText="Date of Death" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Review Date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-Width="120px" />
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <div class="action-buttons">
                                    <asp:LinkButton ID="btnPrint" runat="server" CommandName="PrintRecord" 
                                        CommandArgument='<%# Eval("ReviewID") %>' CssClass="btn-print" Text="� Print" 
                                        CausesValidation="false" />
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" 
                                        CommandArgument='<%# Eval("ReviewID") %>' CssClass="btn-edit" Text="✏ Edit" 
                                        CausesValidation="false" />
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteRecord" 
                                        CommandArgument='<%# Eval("ReviewID") %>' CssClass="btn-delete" Text="🗑 Delete" 
                                        OnClientClick="return confirm('Are you sure you want to delete this review form?');" 
                                        CausesValidation="false" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>