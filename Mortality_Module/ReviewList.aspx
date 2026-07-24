<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewList.aspx.cs" MasterPageFile="~/AdminMaster.Master" Inherits="welfareSystem.Mortality_Module.ReviewList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
      

        .container {
            max-width: 1400px;
            margin: 0 auto;
            background: white;
            border-radius: 15px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            overflow: hidden;
        }

        .header {
            background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
            color: white;
            padding: 20px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
        }

        .header h1 {
            font-size: 24px;
        }

        .header p {
            font-size: 12px;
            opacity: 0.9;
            margin-top: 5px;
        }

        .content {
            padding: 30px;
        }

        .section-title {
            font-size: 18px;
            font-weight: bold;
            color: #1e3c72;
            margin-bottom: 15px;
            padding-bottom: 10px;
            border-bottom: 2px solid #1e3c72;
        }

        .grid-container {
            overflow-x: auto;
            margin-bottom: 30px;
        }

        .grid-view {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
        }

        .grid-view th {
            background: #2a5298;
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

        .btn-create, .btn-view, .btn-edit {
            padding: 5px 10px;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            display: inline-block;
        }

        .btn-create {
            background: #28a745;
            color: white;
        }

        .btn-view {
            background: #17a2b8;
            color: white;
        }

        .btn-edit {
            background: #ffc107;
            color: #333;
        }

        .btn-create:hover, .btn-view:hover, .btn-edit:hover {
            opacity: 0.8;
        }

        .alert {
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 5px;
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 9999;
            min-width: 300px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
            animation: slideIn 0.5s ease-out;
        }

        @keyframes slideIn {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
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

        .empty-message {
            text-align: center;
            padding: 20px;
            color: #6c757d;
            font-style: italic;
        }
    </style>

    <div class="container">
            <div class="header">
                <div>
                    <h1>🏥 Review Form Management</h1>
                    <p>Hashim Medical City, Hyderabad</p>
                </div>
            </div>

            <div class="content">
                <asp:Panel ID="pnlMessage" runat="server" CssClass="alert" Visible="false">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </asp:Panel>

                <!-- Upper Table: Death Certificates Created but Review Form Not Created -->
                <div class="section-title">Pending Review Forms (Death Certificate Created)</div>
                <div class="grid-container">
                    <asp:GridView ID="gvPendingReviews" runat="server" AutoGenerateColumns="False" 
                        CssClass="grid-view" OnRowCommand="gvPendingReviews_RowCommand"
                        EmptyDataText="No pending review forms found.">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="DeathCertificateNo" HeaderText="Certificate No" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="MRNo" HeaderText="M.R. No" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="AdmNo" HeaderText="Adm. No" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="CreatedAt" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="150px" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <div class="action-buttons">
                                        <asp:LinkButton ID="btnCreateReview" runat="server" CommandName="CreateReview" 
                                            CommandArgument='<%# Eval("DeathCertificateNo") %>' CssClass="btn-create" Text="➕ Create Review" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Bottom Table: Review Forms Created -->
                <div class="section-title">Completed Review Forms</div>
                <div class="grid-container">
                    <asp:GridView ID="gvCompletedReviews" runat="server" AutoGenerateColumns="False" 
                        CssClass="grid-view" OnRowCommand="gvCompletedReviews_RowCommand"
                        EmptyDataText="No completed review forms found.">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="DeathCertificateNo" HeaderText="Certificate No" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="ReviewFormNo" HeaderText="Review Form No" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="MRNo" HeaderText="M.R. No" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="AdmNo" HeaderText="Adm. No" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="CreatedAt" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="150px" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <div class="action-buttons">
                                        <asp:LinkButton ID="btnViewReview" runat="server" CommandName="ViewReview" 
                                            CommandArgument='<%# Eval("ReviewFormNo") %>' CssClass="btn-view" Text="👁 View" />
                                        <asp:LinkButton ID="btnEditReview" runat="server" CommandName="EditReview" 
                                            CommandArgument='<%# Eval("ReviewFormNo") %>' CssClass="btn-edit" Text="✏ Edit" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
</asp:Content>
