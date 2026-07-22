<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeathCertificateEdit.aspx.cs" Inherits="welfareSystem.Mortality_Module.DeathCertificateEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Death Certificate - Hashim Medical City</title>
    <style>
        /* Copy same CSS from DeathCertificate.aspx */
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
            max-width: 1000px;
            margin: 0 auto;
            background: white;
            border-radius: 15px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            overflow: hidden;
        }

        .header {
            background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }

        .form-content {
            padding: 30px;
        }

        .section {
            background: #f8f9fa;
            padding: 20px;
            margin-bottom: 25px;
            border-radius: 10px;
            border-left: 4px solid #2a5298;
        }

        .section-title {
            font-size: 18px;
            font-weight: bold;
            color: #2a5298;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #dee2e6;
        }

        .form-row {
            display: flex;
            gap: 20px;
            margin-bottom: 15px;
            flex-wrap: wrap;
        }

        .form-group {
            flex: 1;
            min-width: 200px;
        }

        .form-group label {
            display: block;
            font-weight: 600;
            margin-bottom: 5px;
            color: #333;
            font-size: 14px;
        }

        .form-group input, .form-group select, .form-group textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 14px;
        }

        .button-group {
            text-align: center;
            margin-top: 20px;
        }

        .btn-update, .btn-cancel {
            padding: 12px 40px;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
        }

        .btn-update {
            background: #28a745;
            color: white;
        }

        .btn-cancel {
            background: #6c757d;
            color: white;
            margin-left: 10px;
        }

        .alert {
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 5px;
        }

        .alert-success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }

        .alert-danger {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>✏ Edit Death Certificate</h1>
                <p>Update death certificate information</p>
            </div>

            <div class="form-content">
                <asp:Panel ID="pnlMessage" runat="server" CssClass="alert" Visible="false">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </asp:Panel>

                <div class="section">
                    <div class="section-title">👤 PATIENT INFORMATION</div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>M.R. No *</label>
                            <asp:TextBox ID="txtMRNo" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Patient Name *</label>
                            <asp:TextBox ID="txtPatientName" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Age *</label>
                            <asp:TextBox ID="txtAge" runat="server" type="number"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Gender *</label>
                            <asp:DropDownList ID="ddlGender" runat="server">
                                <asp:ListItem Text="Select Gender" Value=""></asp:ListItem>
                                <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>CNIC No.</label>
                            <asp:TextBox ID="txtCNIC" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Contact No.</label>
                            <asp:TextBox ID="txtContactNo" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Address</label>
                            <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="section">
                    <div class="section-title">⚕️ DEATH INFORMATION</div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Diagnosis *</label>
                            <asp:TextBox ID="txtDiagnosis" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Date of Death *</label>
                            <asp:TextBox ID="txtDateOfDeath" runat="server" type="date"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Time of Death *</label>
                            <asp:TextBox ID="txtTimeOfDeath" runat="server" type="time"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Cause of Death *</label>
                            <asp:TextBox ID="txtCauseOfDeath" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Death Confirmed By *</label>
                            <asp:TextBox ID="txtConfirmedBy" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="section">
                    <div class="section-title">👨‍👩‍👧 DEAD BODY RECEIVED BY</div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Receiver Name *</label>
                            <asp:TextBox ID="txtReceiverName" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Relation *</label>
                            <asp:TextBox ID="txtRelation" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Receiver CNIC</label>
                            <asp:TextBox ID="txtReceiverCNIC" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Receiver Cell No *</label>
                            <asp:TextBox ID="txtReceiverCell" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="button-group">
                    <asp:Button ID="btnUpdate" runat="server" Text="💾 Update Certificate" CssClass="btn-update" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="❌ Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
