<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ResetPassword.aspx.cs"
    Inherits="welfareSystem.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
        rel="stylesheet" />

    <style>
        body {
            background: linear-gradient(135deg,#0f172a,#1e3a8a);
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            font-family: 'Segoe UI';
        }

        .reset-card {
            width: 420px;
            background: white;
            padding: 35px;
            border-radius: 22px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.25);
        }

        .title {
            text-align: center;
            font-size: 30px;
            font-weight: bold;
            color: #0f172a;
            margin-bottom: 10px;
        }

        .subtitle {
            text-align: center;
            color: gray;
            margin-bottom: 30px;
        }

        .form-control {
            height: 50px;
            border-radius: 12px;
            margin-bottom: 18px;
        }

        .btn-reset {
            width: 100%;
            height: 50px;
            border: none;
            border-radius: 12px;
            background: #2563eb;
            color: white;
            font-size: 16px;
            font-weight: 600;
        }

        .btn-reset:hover {
            background: #1d4ed8;
        }

        .user-box {
            background: #f1f5f9;
            padding: 12px;
            border-radius: 10px;
            margin-bottom: 20px;
            text-align: center;
            font-weight: 600;
            color: #0f172a;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">

        <div class="reset-card">

            <div class="title">
                Reset Password
            </div>

            <div class="subtitle">
                Welfare Management System
            </div>

            <div class="user-box">
                Welcome,
                <asp:Label ID="lblUser"
                    runat="server">
                </asp:Label>
            </div>

            <asp:TextBox ID="txtCurrentPassword"
                runat="server"
                CssClass="form-control"
                TextMode="Password"
                placeholder="Current Password">
            </asp:TextBox>

            <asp:TextBox ID="txtNewPassword"
                runat="server"
                CssClass="form-control"
                TextMode="Password"
                placeholder="New Password">
            </asp:TextBox>

            <asp:TextBox ID="txtConfirmPassword"
                runat="server"
                CssClass="form-control"
                TextMode="Password"
                placeholder="Confirm Password">
            </asp:TextBox>

            <asp:Button ID="btnReset"
                runat="server"
                Text="Reset Password"
                CssClass="btn-reset"
                OnClick="btnReset_Click" />

        </div>

    </form>

</body>
</html>