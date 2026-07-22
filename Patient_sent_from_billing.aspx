<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
AutoEventWireup="true"
CodeBehind="Patient_sent_from_billing.aspx.cs"
Inherits="welfareSystem.Patient_sent_from_billing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">
<div class="card shadow p-4">

<h3 class="mb-3">🔍 Patient Send For Welfare</h3>

<!-- SEARCH -->
<div class="row mb-4">
<div class="col-md-6">

<asp:TextBox ID="txtSearch" runat="server"
ClientIDMode="Static"
CssClass="form-control"
placeholder="Search Patient">
</asp:TextBox>
<asp:HiddenField ID="hfMRNo" runat="server" />
</div>
</div>
<asp:Button ID="btnLoadPatient" runat="server"
    OnClick="btnLoadPatient_Click"
    Style="display:none;" />
<!-- PATIENT PANEL -->
<asp:Panel ID="pnlData" runat="server" Visible="false">

<div class="row">

    <!-- LEFT SIDE -->
    <div class="col-md-6">
        <p><b>MR No:</b> <asp:Label ID="lblMR" runat="server" /></p>
        <p><b>Adm No:</b> <asp:Label ID="lblAdmNo" runat="server" /></p>
        <p><b>Patient Name:</b> <asp:Label ID="lblName" runat="server" /></p>
        <p><b>Age:</b> <asp:Label ID="lblAge" runat="server" /></p>
        <p><b>Gender:</b> <asp:Label ID="lblGender" runat="server" /></p>
        <p><b>CNIC:</b> <asp:Label ID="lblCNIC" runat="server" /></p>

        <!-- NEW FIELDS -->
        <p><b>Admission Date:</b> <asp:Label ID="lblAdmissionDate" runat="server" /></p>
        <p><b>Party Name:</b> <asp:Label ID="lblPartyName" runat="server" /></p>
        
        <!-- PRE-ADMISSION CHECKBOX -->
        <div class="form-check mt-3">
            <asp:CheckBox ID="chkPreAdmission" runat="server" CssClass="form-check-input" />
            <label class="form-check-label" for="<%= chkPreAdmission.ClientID %>">
                <b>Pre-Admission</b> 
                <small class="text-muted">(Patient not admitted yet, only MR No created)</small>
            </label>
        </div>
    </div>

    <!-- RIGHT SIDE -->
    <div class="col-md-6">
        <p><b>Mobile:</b> <asp:Label ID="lblMobile" runat="server" /></p>
        <p><b>City:</b> <asp:Label ID="lblCity" runat="server" /></p>
        <p><b>Bill Amount:</b> <asp:Label ID="lblBill" runat="server" /></p>
        <p><b>Advance:</b> <asp:Label ID="lblAdvance" runat="server" /></p>
        <p class="text-danger"><b>Receivable:</b> <asp:Label ID="lblReceivable" runat="server" /></p>

        <!-- NEW FIELDS -->
        <p><b>Ward Name:</b> <asp:Label ID="lblWardName" runat="server" /></p>
        <p><b>Consultant:</b> <asp:Label ID="lblConsultant" runat="server" /></p>
    </div>

</div>

<hr />

<!-- CONSULTANT -->
<%--<h4 class="mt-4">Consultant Details</h4>

<asp:GridView ID="gvConsultants"
runat="server"
CssClass="table table-bordered table-striped"
AutoGenerateColumns="false">

<Columns>

<asp:BoundField DataField="ConsultantName" HeaderText="Consultant Name" />

<asp:BoundField DataField="ProcedureFee" HeaderText="Procedure Fee" />

<asp:BoundField DataField="VisitFee" HeaderText="Visit Fee" />

<asp:BoundField DataField="TotalFee" HeaderText="Total Fee" />

</Columns>

</asp:GridView>--%>

<hr />

<asp:Button ID="btnProceed" runat="server"
Text="Proceed To Welfare"
CssClass="btn btn-success"
OnClick="btnProceed_Click" />

<hr />
</asp:Panel>
 

</div>

     <h4 class="mt-3">Patient Welfare Status</h4>

<asp:GridView ID="gvPatients" runat="server"
    CssClass="table table-bordered table-striped"
    AutoGenerateColumns="false"
    DataKeyNames="MRNo,AdmNo"
    OnRowCommand="gvPatients_RowCommand">

    <Columns>
        <asp:BoundField DataField="MRNo" HeaderText="MR No" />
        <asp:BoundField DataField="AdmNo" HeaderText="Adm No" />
        <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
        <asp:BoundField DataField="Age" HeaderText="Age" />
        <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" />
        <asp:BoundField DataField="Status" HeaderText="Status" />
        <asp:BoundField DataField="Comments" HeaderText="Comments" />
        
        <asp:TemplateField HeaderText="Pre-Admission">
            <ItemTemplate>
                <asp:Label ID="lblPreAdmission" runat="server" 
                    Text='<%# Eval("IsPreAdmission") != DBNull.Value && Convert.ToBoolean(Eval("IsPreAdmission")) ? "✅ Yes" : "❌ No" %>'
                    CssClass='<%# Eval("IsPreAdmission") != DBNull.Value && Convert.ToBoolean(Eval("IsPreAdmission")) ? "text-success" : "text-danger" %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:Button ID="btnResend" runat="server"
                    Text="Resend"
                    CssClass="btn btn-warning btn-sm"
                    CommandName="Resend"
                    CommandArgument='<%# Eval("AdmNo") %>'
                    Visible='<%# Eval("Status").ToString().ToLower().Contains("reject") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</div>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

<script>
    $(function () {

        $("#txtSearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "Patient_sent_from_billing.aspx/GetPatients",
                    type: "POST",
                    data: JSON.stringify({ prefix: request.term }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        response(data.d);
                    },
                    error: function (xhr) {
                        console.log(xhr.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {

                $("#txtSearch").val(ui.item.value);

                $("#<%= hfMRNo.ClientID %>").val(ui.item.value);

    $("#<%= btnLoadPatient.ClientID %>").click();
}
        });

    });
</script>
</asp:Content>