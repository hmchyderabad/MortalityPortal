<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="finalApproval.aspx.cs"
    Inherits="welfareSystem.finalApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .card {
        background: #fff;
        padding: 20px;
        border-radius: 10px;
        margin-bottom: 20px;
        box-shadow: 0px 0px 10px #ddd;
    }

    .heading {
        font-size: 22px;
        font-weight: bold;
        margin-bottom: 15px;
        color: #0d6efd;
    }

    .lbl {
        font-weight: bold;
        color: #444;
    }

    .value {
        color: #000;
    }

    .row-space {
        margin-bottom: 12px;
    }
</style>

<div class="container-fluid">

    <!-- Patient Information -->
    <div class="card">
        <div class="heading">Patient Information</div>

        <div class="row row-space">
            <div class="col-md-3" hidden>
                <span class="lbl">Request ID:</span>
                <asp:Label ID="lblRequestID" runat="server" CssClass="value"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Adm No:</span>
                <asp:Label ID="lblAdmNo" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">MR No:</span>
                <asp:Label ID="lblMRNo" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Patient Name:</span>
                <asp:Label ID="lblPatientName" runat="server"></asp:Label>
            </div>
        </div>

        <div class="row row-space">
            <div class="col-md-3">
                <span class="lbl">Age:</span>
                <asp:Label ID="lblAge" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Gender:</span>
                <asp:Label ID="lblGender" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">City:</span>
                <asp:Label ID="lblCity" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Ward:</span>
                <asp:Label ID="lblWard" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <!-- Financial -->
    <div class="card">
        <div class="heading">Financial Details</div>

        <div class="row row-space">
            <div class="col-md-3">
                <span class="lbl">Bill Amount:</span>
                <asp:Label ID="lblBillAmount" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Advance Amount:</span>
                <asp:Label ID="lblAdvanceAmount" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Receivable Amount:</span>
                <asp:Label ID="lblReceivableAmount" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Monthly Income:</span>
                <asp:Label ID="lblMonthlyIncome" runat="server"></asp:Label>
            </div>
        </div>

        <div class="row row-space">
            <div class="col-md-3">
                <span class="lbl">Income Category:</span>
                <asp:Label ID="lblIncomeCategory" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Dependents:</span>
                <asp:Label ID="lblDependents" runat="server"></asp:Label>
            </div>

            <div class="col-md-3" hidden>
                <span class="lbl">Final Percentage:</span>
                <asp:Label ID="lblPercentage" runat="server"></asp:Label>
            </div>

            <div class="col-md-3">
                <span class="lbl">Donor:</span>
                <asp:Label ID="lblDonor" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <!-- Consultant -->
<div class="card">

    <div class="heading">Consultant Details</div>

    <asp:Repeater ID="rptConsultants" runat="server">

        <ItemTemplate>

            <div class="row row-space mb-3">

                <!-- Consultant -->
                <div class="col-md-2">

                    <span class="lbl">Consultant:</span>

                    <span class="value">

                        <asp:Literal ID="ltConsultantName"
                            runat="server"
                            Text='<%# Eval("ConsultantName") %>'>
                        </asp:Literal>

                    </span>

                </div>

                <!-- Visit Fee -->
                <div class="col-md-2">

                    <span class="lbl">Visit Fee:</span>

                    <span class="value">

                        <asp:Literal ID="ltVisitFee"
                            runat="server"
                            Text='<%# Eval("VisitFee") %>'>
                        </asp:Literal>

                    </span>

                </div>

                <!-- Procedure Fee -->
                <div class="col-md-2">

                    <span class="lbl">Procedure Fee:</span>

                    <span class="value">

                        <asp:Literal ID="ltProcedureFee"
                            runat="server"
                            Text='<%# Eval("ProcedureFee") %>'>
                        </asp:Literal>

                    </span>

                </div>

                <!-- Discount Percent -->
                <div class="col-md-2">

                    <span class="lbl">Discount %:</span>

                    <span class="value">

                        <asp:Literal ID="ltDiscountPercent"
                            runat="server"
                            Text='<%# Eval("DiscountPercent") %>'>
                        </asp:Literal> %

                    </span>

                </div>

                <!-- Discount Amount -->
                <div class="col-md-2">

                    <span class="lbl">Discount Amount:</span>

                    <span class="value consultant-discount">

                        <asp:Literal ID="ltDiscountAmount"
                            runat="server"
                            Text='<%# Eval("DiscountAmount") %>'>
                        </asp:Literal>

                    </span>

                </div>

                <!-- Final Amount -->
                <div class="col-md-2">

                    <span class="lbl">Final Amount:</span>

                    <span class="value">

                        <asp:Literal ID="ltFinalAmount"
                            runat="server"
                            Text='<%# Eval("FinalAmount") %>'>
                        </asp:Literal>

                    </span>

                </div>

            </div>

            <hr />

        </ItemTemplate>

    </asp:Repeater>

</div>


<%--<div class="card">
    <div class="heading">Consultant Details</div>

    <asp:Repeater ID="rptConsultants" runat="server">
        <ItemTemplate>

            <div class="row row-space mb-3">
                
                <div class="col-md-2">
                    <span class="lbl">Consultant:</span>
                    <span class="value"><%# Eval("ConsultantName") %></span>
                </div>

                <div class="col-md-2">
                    <span class="lbl">Visit Fee:</span>
                    <span class="value"><%# Eval("VisitFee") %></span>
                </div>

                <div class="col-md-2">
                    <span class="lbl">Procedure Fee:</span>
                    <span class="value"><%# Eval("ProcedureFee") %></span>
                </div>

                <div class="col-md-2">
                    <span class="lbl">Discount %:</span>
                    <span class="value"><%# Eval("DiscountPercent") %>%</span>
                </div>

                <div class="col-md-2">
                    <span class="lbl">Discount Amount:</span>
                    <span class="value consultant-discount">
                        <%# Eval("DiscountAmount") %>
                    </span>
                </div>

                <div class="col-md-2">
                    <span class="lbl">Final Amount:</span>
                    <span class="value"><%# Eval("FinalAmount") %></span>
                </div>

            </div>

            <hr />

        </ItemTemplate>
    </asp:Repeater>

</div>--%>

  <!-- Services -->
<div class="card">

    <div class="heading">Approved Services</div>

    <!-- Header Row -->
    <div class="row mb-2 fw-bold">

        <div class="col-md-2">
            Service
        </div>

        <div class="col-md-2">
            Amount
        </div>

        <div class="col-md-2">
            Discount %
        </div>
        <div class="col-md-2">
            Custom Discount %
        </div>

        <div class="col-md-2">
            Discount Amount
        </div>

        <div class="col-md-2">
            Final Amount
        </div>

    </div>

    <hr />

    <!-- Repeater -->
    <asp:Repeater ID="rptServices"
        runat="server"
        OnItemDataBound="rptServices_ItemDataBound">

        <ItemTemplate>

            <div class="row mb-3">

                <!-- Service -->
                <div class="col-md-2">

                    <asp:DropDownList ID="ddlService"
                        runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>

                </div>

                <!-- Amount -->
                <div class="col-md-2">

                    <asp:TextBox ID="txtAmount"
                        runat="server"
                        CssClass="form-control"
                        ReadOnly="true">
                    </asp:TextBox>

                </div>

                <!-- Percentage -->
                <div class="col-md-2">

                    <asp:DropDownList ID="ddlPercentage"
                        runat="server"
                        CssClass="form-control mb-1"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlPercentage_SelectedIndexChanged">
                    </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                    <asp:TextBox ID="txtCustomPercentage"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Custom %"
                        AutoPostBack="true"
                        OnTextChanged="txtCustomPercentage_TextChanged">
                    </asp:TextBox>

                </div>

                <!-- Discount -->
                <div class="col-md-2">

                    <asp:TextBox ID="txtDiscount"
                        runat="server"
                        CssClass="form-control"
                        ReadOnly="true">
                    </asp:TextBox>

                </div>

                <!-- Final -->
                <div class="col-md-2">

                    <asp:TextBox ID="txtFinalAmount"
                        runat="server"
                        CssClass="form-control"
                        ReadOnly="true">
                    </asp:TextBox>

                </div>

            </div>

        </ItemTemplate>

    </asp:Repeater>

</div>

    <!-- Grand Summary -->
<div class="card">

    <div class="heading">Final Calculation</div>

    <div class="row">


        <div class="col-md-4">
            <span class="lbl">Bill Amount:</span>
            <asp:Label ID="lblReceivable"
                runat="server"
                CssClass="value">
            </asp:Label>
        </div>

        <div class="col-md-4">
            <span class="lbl">Total Discount Amount:</span>
            <asp:Label ID="lblTotalDiscount"
                runat="server"
                CssClass="value">
            </asp:Label>
        </div>

        

        <div class="col-md-4">
            <span class="lbl">Net Total:</span>
            <asp:Label ID="lblGrandTotal"
                runat="server"
                CssClass="value"
                Style="font-size:22px;color:green;font-weight:bold;">
            </asp:Label>
        </div>

    </div>

</div>
    <asp:HiddenField ID="hfDonorId" runat="server" />
    <div class="text-center mt-4">

    <asp:Button ID="btnSave"
        runat="server"
        Text="Save Final Approval"
        CssClass="btn btn-success btn-lg"
        OnClick="btnSave_Click" />

</div>


</div>



<script>

    function calculateAll() {

        let totalDiscount = 0;

        // ===== SERVICES =====

        let discountBoxes =
            document.querySelectorAll("input[id*='txtDiscount']");

        discountBoxes.forEach(function (box) {

            let value =
                parseFloat(
                    box.value.replace(/,/g, '')
                ) || 0;

            totalDiscount += value;

        });

        // ===== CONSULTANT =====

        let consultantDiscounts =
            document.querySelectorAll(".consultant-discount");

        consultantDiscounts.forEach(function (lbl) {

            let value =
                parseFloat(
                    lbl.innerText.replace(/,/g, '')
                ) || 0;

            totalDiscount += value;

        });

        // ===== RECEIVABLE =====

        let receivable =
            parseFloat(
                document.getElementById("<%= lblReceivable.ClientID %>")
                    .innerText.replace(/,/g, '')
            ) || 0;

        // ===== GRAND TOTAL =====

        let grandTotal =
            receivable - totalDiscount;

        // ===== LABELS =====

        document.getElementById("<%= lblTotalDiscount.ClientID %>")
            .innerText =
            totalDiscount.toLocaleString();

        document.getElementById("<%= lblGrandTotal.ClientID %>")
            .innerText =
            grandTotal.toLocaleString();

    }

    // ===== RECALCULATE SERVICE ROW =====

    function recalculateRow(row) {

        let amountBox =
            row.querySelector("input[id*='txtAmount']");

        let perBox =
            row.querySelector("input[id*='txtCustomPercentage']");

        let discountBox =
            row.querySelector("input[id*='txtDiscount']");

        let finalBox =
            row.querySelector("input[id*='txtFinalAmount']");

        let amount =
            parseFloat(
                amountBox.value.replace(/,/g, '')
            ) || 0;

        let percentage =
            parseFloat(perBox.value) || 0;

        let discount =
            (amount * percentage) / 100;

        let finalAmount =
            amount - discount;

        discountBox.value =
            discount.toLocaleString();

        finalBox.value =
            finalAmount.toLocaleString();

        calculateAll();
    }

    // ===== TEXTBOX KEYUP =====

    document.addEventListener("keyup", function (e) {

        if (
            e.target.id.includes("txtCustomPercentage")
        ) {

            let row =
                e.target.closest(".row.mb-3");

            recalculateRow(row);
        }

    });

    // ===== DROPDOWN CHANGE =====

    document.addEventListener("change", function (e) {

        if (
            e.target.id.includes("ddlPercentage")
        ) {

            let row =
                e.target.closest(".row.mb-3");

            let txt =
                row.querySelector(
                    "input[id*='txtCustomPercentage']"
                );

            txt.value =
                e.target.value.replace("%", "");

            recalculateRow(row);
        }

    });

    // ===== PAGE LOAD =====

    window.onload = function () {

        calculateAll();

    };

</script>

</asp:Content>