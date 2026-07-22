<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintCertificate.aspx.cs" Inherits="welfareSystem.Mortality_Module.PrintCertificate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Death Certificate - Hashim Medical City</title>
    <style>
        @media print {
            body { margin: 0; padding: 0; }
            .no-print { display: none !important; }
        }
        body {
            margin: 20px;
            background: #f5f5f5;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="no-print" style="text-align: right; margin-bottom: 15px;">
            <button onclick="window.print();" style="padding: 10px 20px; background: #1a5a4c; color: white; border: none; cursor: pointer; border-radius: 5px;">
                <i class="fas fa-print"></i> Print Certificate
            </button>
            <button onclick="window.close();" style="padding: 10px 20px; background: #6c757d; color: white; border: none; cursor: pointer; border-radius: 5px; margin-left: 10px;">
                Close
            </button>
        </div>
        <asp:Literal ID="litCertificate" runat="server"></asp:Literal>
    </form>
</body>
</html>
