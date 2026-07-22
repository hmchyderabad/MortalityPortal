<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="WalfareAssessment.aspx.cs" Inherits="welfareSystem.WalfareAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

<h3 class="text-center">Hospital Welfare Department Assessment</h3>

   

<h4 class="mb-3 text-primary">Patient Welfare Assessment</h4>
    <asp:TextBox 
    ID="txtSearch" 
    runat="server"
    ClientIDMode="Static"
    CssClass="form-control"
    placeholder="Search AdmNo / MRNo / Name">
</asp:TextBox>
    <asp:HiddenField ID="hdnRequestID" runat="server" ClientIDMode="Static" />
    <hr />

<h4>Patient Information</h4>

<table class="table table-bordered">
    <tr>
        <th>Adm No</th>
        <td><asp:Label ID="lblAdmNo" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>MR No</th>
        <td><asp:Label ID="lblMRNo" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>Patient Name</th>
        <td><asp:Label ID="lblPatientName" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>Admission Date</th>
        <td><asp:Label ID="lblAdmissionDate" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>
    <tr>
        <th>Party Name</th>
        <td><asp:Label ID="lblPartyName" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>
    <tr>
        <th>Gender</th>
        <td><asp:Label ID="lblGender" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>
    <tr>
        <th>Ward Name</th>
        <td><asp:Label ID="lblWardName" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>
    <tr>
        <th>Consultant</th>
        <td><asp:Label ID="lblConsultant" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>City</th>
        <td><asp:Label ID="lblCity" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>Bill Amount</th>
        <td><asp:Label ID="lblBillAmount" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>Advance Amount</th>
        <td><asp:Label ID="lblAdvanceAmount" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>

    <tr>
        <th>Receivable Amount</th>
        <td><asp:Label ID="lblReceivableAmount" runat="server" ClientIDMode="Static"></asp:Label></td>
    </tr>
</table>

<h4>Patient Assessment</h4>
<table class="table table-bordered">

<tr>
<td>Date Received</td>
<td>
<asp:TextBox ID="txtDateReceived" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
</td>

<td>HMC Employee ID</td>
<td>
    <asp:TextBox ID="txtEmpId" runat="server" CssClass="form-control"></asp:TextBox>
<%--<asp:DropDownList ID="ddlReceivedBy" runat="server" CssClass="form-control">
<asp:ListItem>Select</asp:ListItem>
<asp:ListItem>Ali</asp:ListItem>
<asp:ListItem>Ahmed</asp:ListItem>
<asp:ListItem>Sara</asp:ListItem>
</asp:DropDownList>
</td>--%>
</tr>

<tr>
<td>No of Children</td>
<td><asp:TextBox ID="txtChildren" runat="server" CssClass="form-control"></asp:TextBox></td>

<td>Under 18 Years</td>
<td><asp:TextBox ID="txtUnder18" runat="server" CssClass="form-control"></asp:TextBox></td>
</tr>

<tr>
<td>Total Dependents</td>
<td><asp:TextBox ID="txtDependents" runat="server" CssClass="form-control"></asp:TextBox></td>

<td>Patient Monthly Income</td>
<td><asp:TextBox ID="txtIncome" runat="server" CssClass="form-control"></asp:TextBox></td>
</tr>

<tr>
<td>Source of Income</td>
<td><asp:TextBox ID="txtIncomeSource" runat="server" CssClass="form-control"></asp:TextBox></td>

<td>Payer Relation & Income Source</td>
<td><asp:TextBox ID="txtPayerRelation" runat="server" CssClass="form-control"></asp:TextBox></td>
</tr>

<tr>
<td>Property Status</td>
<td>
<asp:DropDownList ID="ddlProperty" runat="server" CssClass="form-control">
<asp:ListItem>Select</asp:ListItem>
<asp:ListItem>Rented</asp:ListItem>
<asp:ListItem>Personal</asp:ListItem>
</asp:DropDownList>
</td>

<td>If Rented Mention Rent</td>
<td><asp:TextBox ID="txtRent" runat="server" CssClass="form-control"></asp:TextBox></td>
</tr>

<tr>
<td>Monthly Utility Expenses</td>
<td><asp:TextBox ID="txtUtility" runat="server" CssClass="form-control"></asp:TextBox></td>
<%--</tr>

</table>

<table class="table table-bordered">

<tr>--%>
<td>
<asp:CheckBox ID="chkGold" runat="server" Text="Gold Available" />
</td>
<td>
<asp:TextBox ID="txtGoldAmount" runat="server" CssClass="form-control" Placeholder="Estimated Amount & Duration"></asp:TextBox>
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkSilver" runat="server" Text="Silver Available" />
</td>
<td>
<asp:TextBox ID="txtSilverAmount" runat="server" CssClass="form-control"></asp:TextBox>
</td>
<%--</tr>

<tr>--%>
<td>
<asp:CheckBox ID="chkStock" runat="server" Text="Business Stock Available" />
</td>
<td>
<asp:TextBox ID="txtStockAmount" runat="server" CssClass="form-control"></asp:TextBox>
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkCash" runat="server" Text="Cash Available" />
</td>
<td>
<asp:TextBox ID="txtCashAmount" runat="server" CssClass="form-control"></asp:TextBox>
</td>
    <td><asp:CheckBox ID="chkLowIncome" runat="server" Text=" Low-income household" /><br /></td>
    <td><asp:CheckBox ID="chkChronic" runat="server" Text=" Chronic illness (long-term care)" /><br /></td>
</tr>
<tr>
    <td><asp:CheckBox ID="chkBreadwinner" runat="server" Text=" Sole breadwinner unable to work" /></td>
    <td><asp:CheckBox ID="chkHighCost" runat="server" Text=" Treatment cost very high" /></td>
    <td></td>
    <td></td>
</tr>
</table>
        
<asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Remarks"></asp:TextBox>

<hr />

<%--<h4>Required Document Checklist</h4>--%>

<table class="table table-bordered"  style="display:none;">

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

<%--<h4>Assessment Findings & Recommendation</h4>--%>

<table class="table table-bordered"  style="display:none;">

<tr>
<td>
<asp:CheckBox ID="chkFullSupport" runat="server" Text="Recommended for Full Welfare Support" />
</td>
<td>% Discount</td>
<td>
<asp:TextBox ID="txtFullDiscount" runat="server" CssClass="form-control"></asp:TextBox>
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkPartialSupport" runat="server" Text="Recommended for Partial Support" />
</td>
<td>% Discount</td>
<td>
<asp:TextBox ID="txtPartialDiscount" runat="server" CssClass="form-control"></asp:TextBox>
</td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkProfessional" runat="server" Text="Discount on Professional Fees Only" />
</td>
<td colspan="2"></td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkBabaFoundation" runat="server" Text="Recommended for BABA Foundation Support" />
</td>
<td colspan="2"></td>
</tr>

<tr>
<td>
<asp:CheckBox ID="chkNotRecommended" runat="server" Text="Not Recommended" />
</td>
<td>Reason</td>
<td>
<asp:TextBox ID="txtReason" runat="server" CssClass="form-control"></asp:TextBox>
</td>
</tr>

</table>

<div class="text-center">
<asp:Button 
ID="btnSaveAssessment" 
runat="server" 
Text="Save Assessment" 
CssClass="btn btn-success"
OnClick="btnSaveAssessment_Click" />
</div>


    <br />

<asp:GridView 
ID="gvAssessment" 
runat="server" 
CssClass="table table-bordered table-striped"
AutoGenerateColumns="false"
DataKeyNames="AssessmentID"

OnRowEditing="gvAssessment_RowEditing"
OnRowUpdating="gvAssessment_RowUpdating"
OnRowCancelingEdit="gvAssessment_RowCancelingEdit"
OnRowDeleting="gvAssessment_RowDeleting">

<Columns>

<asp:BoundField DataField="AssessmentID" HeaderText="ID" ReadOnly="true"/>

<asp:BoundField DataField="ChildrenCount" HeaderText="Children"/>

<asp:BoundField DataField="TotalDependents" HeaderText="Dependents"/>

<asp:BoundField DataField="MonthlyIncome" HeaderText="Income"/>

<asp:BoundField DataField="PropertyStatus" HeaderText="Property"/>

<asp:BoundField DataField="CreatedDate" HeaderText="Date" ReadOnly="true"/>

<asp:CommandField ShowEditButton="true" ShowDeleteButton="true"/>

</Columns>

</asp:GridView>

</div>



<link href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css" rel="stylesheet" />

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

<script>
    $(function () {

        $("#txtSearch").autocomplete({
            minLength: 1,
            source: function (request, response) {
                $.ajax({
                    url: "WalfareAssessment.aspx/GetPatients",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ prefix: request.term }),
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.DisplayText,  // 👈 ye show hoga
                                value: item.DisplayText,  // 👈 textbox me ye aayega
                                admNo: item.AdmNo         // 👈 hidden key (VERY IMPORTANT)
                            };
                        }));
                    }
                });
            },

            select: function (e, i) {

                var admNo = i.item.admNo;   // 👈 correct value

                LoadPatientDetails(admNo);  // 👈 details load
            }
        });

    });

    function LoadPatientDetails(admNo) {
        $.ajax({
            url: "WalfareAssessment.aspx/GetPatientDetails",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ admNo: admNo }),
            success: function (data) {

                var p = data.d;

                if (p != null) {

                    
                    $("#<%= hdnRequestID.ClientID %>").val(p.RequestID);

                    $("#lblAdmNo").text(p.AdmNo);
                    $("#lblMRNo").text(p.MRNo);
                    $("#lblPatientName").text(p.PatientName);
                    $("#lblCity").text(p.City);
                    $("#lblBillAmount").text(p.BillAmount);
                    $("#lblAdvanceAmount").text(p.AdvanceAmount);
                    $("#lblReceivableAmount").text(p.ReceivableAmount);
                    $("#lblWardName").text(p.wardName);
                    $("#lblConsultant").text(p.Consultant);
                    $("#lblAdmissionDate").text(p.AdmissionDate);
                    $("#lblGender").text(p.Gender);
                    $("#lblPartyName").text(p.PartyName);
                    console.log("RequestID =", p.RequestID);
                    $("#hdnRequestID").val(p.RequestID);
                    console.log("HiddenField Set =", $("#hdnRequestID").val());
                }
            }
        });
    }
</script>




</asp:Content>
