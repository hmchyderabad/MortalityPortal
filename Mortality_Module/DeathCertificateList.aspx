<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeathCertificateList.aspx.cs" Inherits="welfareSystem.Mortality_Module.DeathCertificateList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Death Certificate List - Hashim Medical City</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 20px;
            min-height: 100vh;
        }

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

        .btn-add {
            background: #28a745;
            color: white;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            transition: background 0.3s;
            display: inline-block;
        }

        .btn-add:hover {
            background: #218838;
        }

        .content {
            padding: 30px;
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

        .grid-container {
            overflow-x: auto;
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

        .btn-edit, .btn-delete, .btn-view {
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

        .btn-view:hover, .btn-edit:hover, .btn-delete:hover {
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

        .pagination {
            margin-top: 20px;
            text-align: center;
        }

        .pagination a, .pagination span {
            display: inline-block;
            padding: 8px 12px;
            margin: 0 3px;
            border: 1px solid #dee2e6;
            border-radius: 3px;
            text-decoration: none;
            color: #007bff;
        }

        .pagination .current {
            background: #007bff;
            color: white;
            border-color: #007bff;
        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            overflow: auto;
        }

        .modal-content {
            background: white;
            margin: 50px auto;
            padding: 0;
            width: 90%;
            max-width: 800px;
            border-radius: 10px;
            animation: slideDown 0.3s ease-out;
        }

        @keyframes slideDown {
            from {
                transform: translateY(-50px);
                opacity: 0;
            }
            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .modal-header {
            background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
            color: white;
            padding: 15px 20px;
            border-radius: 10px 10px 0 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-body {
            padding: 20px;
            max-height: 500px;
            overflow-y: auto;
        }

        .close {
            color: white;
            font-size: 28px;
            font-weight: bold;
            cursor: pointer;
        }

        .detail-row {
            margin-bottom: 15px;
            padding: 10px;
            background: #f8f9fa;
            border-radius: 5px;
        }

        .detail-label {
            font-weight: bold;
            color: #2a5298;
            width: 200px;
            display: inline-block;
        }

        @media (max-width: 768px) {
            .content {
                padding: 15px;
            }
            .action-buttons {
                flex-direction: column;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <div class="container">
            <div class="header">
                <div>
                    <h1>🏥 Death Certificate Management</h1>
                    <p>Hashim Medical City, Hyderabad</p>
                </div>
                <a href="DeathCertificate.aspx" class="btn-add">+ Add New Certificate</a>
            </div>

            <div class="content">
                <asp:Panel ID="pnlMessage" runat="server" CssClass="alert" Visible="false">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </asp:Panel>

                <!-- Search Section -->
                <div class="search-section">
                    <div class="search-group">
                        <label>Search by Name</label>
                        <asp:TextBox ID="txtSearchName" runat="server" placeholder="Patient Name..."></asp:TextBox>
                    </div>
                    <div class="search-group">
                        <label>Search by MR No</label>
                        <asp:TextBox ID="txtSearchMRNo" runat="server" placeholder="MR Number..."></asp:TextBox>
                    </div>
                    <div class="search-group">
                        <label>From Date</label>
                        <asp:TextBox ID="txtFromDate" runat="server" type="date"></asp:TextBox>
                    </div>
                    <div class="search-group">
                        <label>To Date</label>
                        <asp:TextBox ID="txtToDate" runat="server" type="date"></asp:TextBox>
                    </div>
                    <div class="search-group">
                        <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn-search" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn-reset" OnClick="btnReset_Click" Style="margin-left: 5px;" />
                    </div>
                </div>

                <!-- GridView -->
                <div class="grid-container">
                    <asp:GridView ID="gvDeathCertificates" runat="server" AutoGenerateColumns="False" 
                        CssClass="grid-view" OnRowCommand="gvDeathCertificates_RowCommand"
                        OnPageIndexChanging="gvDeathCertificates_PageIndexChanging" AllowPaging="True"
                        PageSize="10" EmptyDataText="No records found.">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="MRNo" HeaderText="M.R. No" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="Age" HeaderText="Age" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="DateOfDeath" HeaderText="Date of Death" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="CauseOfDeath" HeaderText="Cause of Death" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="ReceiverName" HeaderText="Received By" ItemStyle-Width="150px" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <div class="action-buttons">
                                        <asp:LinkButton ID="btnView" runat="server" CommandName="ViewRecord" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn-view" Text="👁 View" />
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn-edit" Text="✏ Edit" />
                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteRecord" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn-delete" Text="🗑 Delete" 
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pagination" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- View Modal -->
        <div id="viewModal" class="modal">
            <div class="modal-content">
                <div class="modal-header">
                    <h2>📄 Death Certificate Details</h2>
                    <span class="close" onclick="closeModal()">&times;</span>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="litDetails" runat="server"></asp:Literal>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function openModal() {
                document.getElementById('viewModal').style.display = 'block';
            }

            function closeModal() {
                document.getElementById('viewModal').style.display = 'none';
            }

            window.onclick = function(event) {
                var modal = document.getElementById('viewModal');
                if (event.target == modal) {
                    modal.style.display = 'none';
                }
            }

            function autoHideMessage() {
                setTimeout(function() {
                    var pnl = document.getElementById('<%= pnlMessage.ClientID %>');
                    if(pnl) {
                        pnl.style.display = 'none';
                    }
                }, 5000);
            }
        </script>
    </form>
</body>
</html>