<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="DrTransactionForm.aspx.cs" Inherits="welfareSystem.DrTransactionForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

    <div class="card shadow p-4">

        <h3 class="mb-3">🔍 Clinician's Recommendation & Assent</h3>

        <!-- SEARCH BOX -->
        <div class="row mb-4">
            <div class="col-md-6">
                <asp:TextBox ID="txtSearch" runat="server"
                    CssClass="form-control"
                    AutoPostBack="true"
                    OnTextChanged="txtSearch_TextChanged"
                    placeholder="Type to search Name / MR No / Admission No">
                </asp:TextBox>
            </div>

            
        </div>
        <asp:Panel ID="pnlData" runat="server" Visible="false">

            <div class="row">

                <div class="col-md-6">
                    <p><b>MR No:</b> <asp:Label ID="lblMR" runat="server" /></p>
                    <p><b>Adm No:</b> <asp:Label ID="lblAdmNo" runat="server" /></p>
                    <p><b>Patient Name:</b> <asp:Label ID="lblName" runat="server" /></p>
                    <p><b>Age:</b> <asp:Label ID="lblAge" runat="server" /></p>
                    <p><b>Gender:</b> <asp:Label ID="lblGender" runat="server" /></p>
                    <p><b>CNIC:</b> <asp:Label ID="lblCNIC" runat="server" /></p>
                </div>

                <div class="col-md-6">
                    <p><b>Mobile:</b> <asp:Label ID="lblMobile" runat="server" /></p>
                    <p><b>City:</b> <asp:Label ID="lblCity" runat="server" /></p>
                    <p><b>Bill Amount:</b> <asp:Label ID="lblBill" runat="server" /></p>
                    <p><b>Advance:</b> <asp:Label ID="lblAdvance" runat="server" /></p>
                    <p class="text-danger"><b>Receivable:</b> <asp:Label ID="lblReceivable" runat="server" /></p>
                </div>

            </div>

        </asp:Panel>

        <div class="col-md-6 mt-3">
            <asp:HiddenField ID="hdnConsultantName" runat="server" />
            <label>Consultant</label>

            <!-- SEARCH INPUT -->
            <input type="text" 
               id="txtConsultantSearch" 
               runat="server"
               class="form-control" 
               placeholder="🔍 Type to search consultant..."
               onkeyup="filterConsultants()"
               onclick="showDropdown()" />

            <!-- DROPDOWN (HIDDEN BY DEFAULT) -->
            <asp:DropDownList 
                ID="ddlConsultant"
                class="form-control"
                runat="server" 
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlConsultant_SelectedIndexChanged">
                </asp:DropDownList>
        </div>
       <%-- <asp:Label ID="lblConsultantDetails" runat="server" CssClass="text-primary"></asp:Label>--%>
        <div class="col-md-6 mt-3">
</div>

         <!-- Procedure Fee -->
        <div class="row mt-3">
            <div class="col-md-4">
                <label>Procedure / Surgery Fee</label>
                <asp:TextBox ID="txtProcedureFee" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <!-- Visit Fee -->
            <div class="col-md-4">
                <label>Consultation Visit Fee</label>
                <asp:TextBox ID="txtVisitFee" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <!-- Total -->
            <div class="col-md-4">
                <label>Total Consultant Fee</label>
                <asp:TextBox ID="txtTotalFee" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <!-- Discount -->
            <div class="col-md-4 mt-3">
                <label>Discount (%)</label>
                <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"
                    onkeyup="calculateDiscount()"></asp:TextBox>
            </div>

            <!-- Final After Discount -->
            <div class="col-md-4 mt-3">
                <label>Final Amount</label>
                <asp:TextBox ID="txtFinalAmount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

        </div>

        <hr />

        <%--<!-- CHECKBOXES -->
        <div class="mt-3">
            <label><b>Eligibility Criteria</b></label><br />

            <asp:CheckBox ID="chkLowIncome" runat="server" Text=" Low-income household" /><br />
            <asp:CheckBox ID="chkChronic" runat="server" Text=" Chronic illness (long-term care)" /><br />
            <asp:CheckBox ID="chkBreadwinner" runat="server" Text=" Sole breadwinner unable to work" /><br />
            <asp:CheckBox ID="chkHighCost" runat="server" Text=" Treatment cost very high" />
        </div>--%>
        <div class="mt-4">
    <asp:Button ID="btnSave" runat="server" Text="Save"
        CssClass="btn btn-success"
        class="form-control"
        OnClick="btnSave_Click" />
    
</div>
        <asp:GridView 
    ID="gvTransactions" 
    runat="server"
    CssClass="table table-striped table-bordered table-hover"
    AutoGenerateColumns="false"
    DataKeyNames="TransactionID"

    OnRowEditing="gvTransactions_RowEditing"
    OnRowUpdating="gvTransactions_RowUpdating"
    OnRowCancelingEdit="gvTransactions_RowCancelingEdit"
    OnRowDeleting="gvTransactions_RowDeleting">

    <Columns>

        <asp:BoundField DataField="AdmNo" HeaderText="Admission No" ReadOnly="true" />

        <asp:BoundField DataField="MRNo" HeaderText="MR No" ReadOnly="true" />

        <asp:BoundField DataField="ConsultantName" HeaderText="Consultant" ReadOnly="true" />

        <asp:BoundField DataField="VisitFee" HeaderText="Visit Fee" />

        <asp:BoundField DataField="ProcedureFee" HeaderText="Procedure Fee" />

        <asp:BoundField DataField="TotalFee" HeaderText="Total Fee" ReadOnly="true" />

        <asp:BoundField DataField="DiscountPercent" HeaderText="Discount %" />

        <asp:BoundField DataField="FinalAmount" HeaderText="Final Amount" ReadOnly="true" />

        <asp:CommandField 
            ShowEditButton="true"
            ShowDeleteButton="true"
            ButtonType="Button"
            EditText="Edit"
            UpdateText="Update"
            CancelText="Cancel"
            DeleteText="Delete"/>

    </Columns>

</asp:GridView>
    </div>

</div>

<script>

    function calculateDiscount() {

        var total = parseFloat(document.getElementById('<%= txtTotalFee.ClientID %>').value) || 0;
        var discount = parseFloat(document.getElementById('<%= txtDiscount.ClientID %>').value) || 0;

    var finalAmount = total - (total * discount / 100);

        document.getElementById('<%= txtFinalAmount.ClientID %>').value = finalAmount.toFixed(0);
    }
    window.onload = function () {
        var name = document.getElementById('<%= hdnConsultantName.ClientID %>').value;

        if (name !== "") {
            document.getElementById("txtConsultantSearch").value = name;
        }
    };
    function selectConsultant(item) {
        document.getElementById("txtConsultantSearch").value = item.text;
        document.getElementById("ddlConsultant").value = item.value;
        document.getElementById("ddlConsultant").style.display = "none";
    }

    // SELECT KARNE PE CLOSE + VALUE SET
    document.addEventListener("DOMContentLoaded", function () {
        var list = document.getElementById("ddlConsultant");
        var input = document.getElementById("txtConsultantSearch");

        list.onchange = function () {

            var selectedText = this.options[this.selectedIndex].text;

            document.getElementById("txtConsultantSearch").value = selectedText;

            list.style.display = "none";
        };
    });

    // OUTSIDE CLICK PE CLOSE
    document.addEventListener("click", function (e) {
        var list = document.getElementById("ddlConsultant");
        var input = document.getElementById("txtConsultantSearch");

        if (!input.contains(e.target) && !list.contains(e.target)) {
            list.style.display = "none";
        }
    });
let timer;
function searchTyping() {
    clearTimeout(timer);
    timer = setTimeout(function () {
        __doPostBack('<%= txtSearch.UniqueID %>', '');
    }, 500);
    }
    function calculateDiscount() {
        var total = parseFloat(document.getElementById('<%= txtTotalFee.ClientID %>').value) || 0;
        var discount = parseFloat(document.getElementById('<%= txtDiscount.ClientID %>').value) || 0;

        var finalAmount = total - (total * discount / 100);

        document.getElementById('<%= txtFinalAmount.ClientID %>').value = finalAmount.toFixed(0);
    }


    function showDropdown() {
        document.getElementById("ddlConsultant").style.display = "block";
    }

    function filterConsultants() {

        var input = document.getElementById("txtConsultantSearch").value.toLowerCase();
        var list = document.getElementById("ddlConsultant");
        var options = list.options;

        for (var i = 0; i < options.length; i++) {

            var txt = options[i].text.toLowerCase();

            if (txt.indexOf(input) > -1)
                options[i].style.display = "";
            else
                options[i].style.display = "none";
        }
    }

    document.getElementById("ddlConsultant").addEventListener("click", function () {

        var selected = this.options[this.selectedIndex].text;

        document.getElementById("txtConsultantSearch").value = selected;

        this.style.display = "none";

    });
</script>
</asp:Content>