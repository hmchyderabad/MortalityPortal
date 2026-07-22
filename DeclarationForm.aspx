<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="DeclarationForm.aspx.cs" Inherits="welfareSystem.DeclarationForm" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    

    <div class="container">
        <div class="declaration-box">
            <h3 style="text-align:center; text-decoration: underline;">PART C: PATIENT DECLARATION & CONSENT</h3>
            
            <p class="english-text">
                I hereby declare that the information I will provide to the Welfare Department for assessment is true and correct to the best of my knowledge. 
                I understand and consent that this information will be used solely to determine my eligibility for financial assistance from 
                <strong>Hashim Medical City Hospital</strong> and its partner organizations, including the <strong>BABA FOUNDATION</strong>.
            </p>

            <div class="rtl-text urdu">
                میں حلفیہ بیان دیتا / دیتی ہوں کہ فلاحی شعبہ کو پیش کردہ تمام معلومات میرے علم کے مطابق درست ہیں۔ میں سمجھتا / سمجھتی ہوں اور اجازت دیتا / دیتی ہوں کہ میری معلومات کا استعمال صرف ہاشم میڈیکل سٹی ہسپتال اور اس کے ساتھی اداروں (بابا فاؤنڈیشن) سے مالی امداد کی اہلیت جانچنے کے لیے کیا جائے گا۔
            </div>

            <div class="rtl-text sindhi">
                مان حلفي بيان ڏيان ٿو / ٿي ته فلاحي شعبي کي ڏنل سموري معلومات منهنجي علم مطابق درست آهي. مان سمجهان ٿو / ٿي ۽ اجازت ڏيان ٿو / ٿي ته منهنجي معلومات جو استعمال صرف هاشم ميڊيڪل سٽي هسپتال ۽ ان جي ساٿي ادارن (بابا فائونڊيشن) کان مالي امداد جي اهليت معلوم ڪرڻ لاءِ ڪيو ويندو.
            </div>

            <hr />

            <div class="row" style="display: flex; flex-wrap: wrap;">
                <div class="col-md-7" style="flex: 1; min-width: 300px; padding: 10px;">
                    <label class="form-label">Signature / دستخط / صحيح:</label>
                    <div class="sig-container">
                        <canvas id="sig-canvas" width="450" height="150"></canvas>
                    </div>
                    <div style="margin-top: 5px;">
                        <button type="button" class="btn btn-sm btn-danger" onclick="clearSig()">Clear / صاف کریں</button>
                    </div>
                    <asp:HiddenField ID="hfSignatureData" runat="server" />
                </div>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hfRequestID" runat="server" />
                <asp:HiddenField ID="hfAdmNo" runat="server" />
                <asp:HiddenField ID="hfMRNo" runat="server" />
                <asp:HiddenField ID="hfPatientName" runat="server" />
                <div class="col-md-5" style="flex: 1; min-width: 250px; padding: 10px;">

                    <div hidden>
    <label class="form-label">
        Request ID:
    </label>

    <asp:TextBox
        ID="txtRequestID"
        runat="server"
        CssClass="form-control-custom"
        ReadOnly="true">
    </asp:TextBox>
</div>
    <label class="form-label">
        Admission No:
    </label>

    <asp:TextBox
        ID="txtAdmNo"
        runat="server"
        CssClass="form-control-custom"
        ReadOnly="true">
    </asp:TextBox>

    <label class="form-label">
        MR No:
    </label>

    <asp:TextBox
        ID="txtMRNo"
        runat="server"
        CssClass="form-control-custom"
        ReadOnly="true">
    </asp:TextBox>

    <label class="form-label">
        Patient Name:
    </label>

    <asp:TextBox
        ID="txtPatientName"
        runat="server"
        CssClass="form-control-custom"
        ReadOnly="true">
    </asp:TextBox>

    <label class="form-label">
        Attendant Name (نالو):
    </label>

    <asp:TextBox
        ID="txtName"
        runat="server"
        CssClass="form-control-custom">
    </asp:TextBox>

    <label class="form-label">
        Relationship (رشتو):
    </label>

    <asp:TextBox
        ID="txtRelationship"
        runat="server"
        CssClass="form-control-custom">
    </asp:TextBox>

    <label class="form-label">
        Date (تاريخ):
    </label>

    <asp:TextBox
        ID="txtDate"
        runat="server"
        TextMode="Date"
        CssClass="form-control-custom">
    </asp:TextBox>

</div>
            </div>

<h4>Required Document Checklist</h4>

<table class="table table-bordered">

<tr>
<td>
<asp:CheckBox ID="chkCNIC" runat="server" Text="CNIC Copy (Patient & Guarantor)" />
</td>
<td>
<asp:FileUpload ID="fileCNIC" runat="server" />
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkUtilityBills" runat="server" Text="Utility Bills (Electricity, Gas)" />
</td>
<td>
<asp:FileUpload ID="fileUtility" runat="server" />
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkIncomeProof" runat="server" Text="Proof of Income" />
</td>
<td>
<asp:FileUpload ID="fileIncome" runat="server" />
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkHardship" runat="server" Text="Proof of Financial Hardship" />
</td>
<td>
<asp:FileUpload ID="fileHardship" runat="server" />
</td>
</tr>

</table>

            <div style="text-align: center; margin-top: 30px;">
                <asp:Button ID="btnSubmit" runat="server" 
                    Text="Submit Declaration" 
                    CssClass="btn btn-success" 
                    OnClick="btnSubmit_Click" 
                    OnClientClick="return saveSignature();" />
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script src="https://cdn.jsdelivr.net/npm/signature_pad@4.0.0/dist/signature_pad.umd.min.js"></script>
    <script>
        var canvas = document.getElementById('sig-canvas');
        var signaturePad = new SignaturePad(canvas, {
            backgroundColor: 'rgb(255, 255, 255)',
            penColor: 'rgb(0, 0, 0)'
        });

        function clearSig() {
            signaturePad.clear();
        }

        function saveSignature() {
            if (signaturePad.isEmpty()) {
                alert("Please provide a signature (صحي/دستخط لازمی ہے)");
                return false;
            }
            var dataUrl = signaturePad.toDataURL('image/png');
            document.getElementById('<%= hfSignatureData.ClientID %>').value = dataUrl;
            return true;
        }
    </script>

    <style>
        .declaration-box { border: 2px solid #333; padding: 30px; margin: 20px 0; background-color: #fff; font-family: 'Segoe UI', Arial, sans-serif; }
        
        .rtl-text { direction: rtl; text-align: right; line-height: 1.8; margin-bottom: 15px; }
        .urdu { font-size: 20px; color: #1a237e; font-weight: 500; }
        .sindhi { font-size: 20px; color: #b71c1c; font-weight: 500; border-top: 1px solid #eee; pt: 10px; }
        
        .english-text { font-size: 15px; text-align: justify; color: #333; margin-bottom: 20px; font-style: italic; }
        .sig-container { border: 2px solid #ddd; background: #fafafa; display: inline-block; border-radius: 5px; }
        .form-label { font-weight: bold; margin-top: 10px; display: block; }
        .form-control-custom { width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 4px; }
    </style>
</asp:Content>