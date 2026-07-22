<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
AutoEventWireup="true"
CodeBehind="Death_Certificate.aspx.cs"
Inherits="welfareSystem.Death_Certificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">
    <div class="card shadow p-4">
        
        <h3 class="mb-3">🔍 Death Certificate</h3>
        
        <!-- SEARCH SECTION -->
        <div class="row mb-4">
            <div class="col-md-6">
                <label class="form-label fw-bold">Search Patient (MR No / Name)</label>
                <asp:TextBox ID="txtSearch" runat="server" 
                    ClientIDMode="Static" 
                    CssClass="form-control" 
                    placeholder="Type MR No or Patient Name...">
                </asp:TextBox>
                <asp:HiddenField ID="hfMRNo" runat="server" />
                <asp:HiddenField ID="hfAdmNo" runat="server" />
            </div>
        </div>
        
        <!-- HIDDEN BUTTON TO TRIGGER LOAD -->
        <asp:Button ID="btnLoadPatient" runat="server" 
            OnClick="btnLoadPatient_Click" 
            Style="display:none;" />
        
        <!-- PATIENT DATA PANEL -->
        <asp:Panel ID="pnlData" runat="server" Visible="false">
            
            <div class="row mt-3">
                <!-- LEFT COLUMN -->
                <div class="col-md-6">
                    <div class="mb-2">
                        <label class="fw-bold">MR No:</label>
                        <asp:Label ID="lblMR" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">Patient Name:</label>
                        <asp:Label ID="lblName" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">Age:</label>
                        <asp:Label ID="lblAge" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">Gender:</label>
                        <asp:Label ID="lblGender" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">CNIC:</label>
                        <asp:Label ID="lblCNIC" runat="server" CssClass="ms-2" />
                    </div>
                </div>
                
                <!-- RIGHT COLUMN -->
                <div class="col-md-6">
                    <div class="mb-2">
                        <label class="fw-bold">Mobile:</label>
                        <asp:Label ID="lblMobile" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">City:</label>
                        <asp:Label ID="lblCity" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">Admission No:</label>
                        <asp:Label ID="lblAdmNo" runat="server" CssClass="ms-2" />
                    </div>
                    <div class="mb-2">
                        <label class="fw-bold">Consultant:</label>
                        <asp:Label ID="lblConsultant" runat="server" CssClass="ms-2" />
                    </div>
                </div>
            </div>
            
            <hr />
            
            <div class="text-center mt-3">
                <asp:Button ID="btnPrint" runat="server" 
                    Text="🖨️ Print Certificate" 
                    CssClass="btn btn-primary me-2"
                    OnClick="btnPrint_Click" />
                    
                <asp:Button ID="btnClear" runat="server" 
                    Text="🗑️ Clear" 
                    CssClass="btn btn-secondary"
                    OnClick="btnClear_Click" />
            </div>
            
        </asp:Panel>
        
        <!-- NO RECORD FOUND MESSAGE -->
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-2" Visible="false" />
        
    </div>
</div>

<!-- JQUERY & AUTOCOMPLETE LIBRARIES -->
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

<!-- AUTOCOMPLETE SCRIPT -->
<script type="text/javascript">
    $(document).ready(function () {

        // Configure Autocomplete
        $("#txtSearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "Death_Certificate.aspx/GetPatients",
                    type: "POST",
                    data: JSON.stringify({ prefix: request.term }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        response(data.d);
                    },
                    error: function (xhr, status, error) {
                        console.log("Error: " + error);
                        console.log("Response: " + xhr.responseText);
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                var selectedValue = ui.item.value;
                var parts = selectedValue.split(' - '); // Hyphen separator to match web method format

                if (parts.length >= 3) {
                    var admNo = parts[0].trim();
                    var mrNo = parts[1].trim();
                    var patientName = parts[2].trim();

                    $("#<%= hfMRNo.ClientID %>").val(mrNo);
                    $("#<%= hfAdmNo.ClientID %>").val(admNo);
                    $("#txtSearch").val(selectedValue);

                    $("#<%= btnLoadPatient.ClientID %>").click();
                }

                return false;
            }
        });

    });
</script>

</asp:Content>