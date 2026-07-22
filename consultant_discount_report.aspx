<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="consultant_discount_report.aspx.cs"
    Inherits="welfareSystem.consultant_discount_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>Consultant Discount Report</h2>

<hr />

<!-- Filter Panel -->
<div class="card mb-4">
    <div class="card-header bg-primary text-white">
        <h5>Filter Criteria</h5>
    </div>
    <div class="card-body">
        <div class="row">
            <div class="col-md-3">
                <label>From Date:</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label>To Date:</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label>Select Consultant:</label>
                <asp:DropDownList ID="ddlConsultant" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label>&nbsp;</label><br />
                <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CssClass="btn btn-success" OnClick="btnExport_Click" />
            </div>
        </div>
    </div>
</div>

<!-- Summary Cards -->
<div class="row mb-4">
    <div class="col-md-3">
        <div class="card text-white bg-info">
            <div class="card-body">
                <h5>Total Discount Amount</h5>
                <h3>Rs. <asp:Label ID="lblTotalDiscount" runat="server" Text="0"></asp:Label></h3>
            </div>
        </div>
    </div>
    <div class="col-md-3">
        <div class="card text-white bg-success">
            <div class="card-body">
                <h5>Total Final Amount</h5>
                <h3>Rs. <asp:Label ID="lblTotalFinalAmount" runat="server" Text="0"></asp:Label></h3>
            </div>
        </div>
    </div>
    <div class="col-md-3">
        <div class="card text-white bg-warning">
            <div class="card-body">
                <h5>Total Transactions</h5>
                <h3><asp:Label ID="lblTotalPatients" runat="server" Text="0"></asp:Label></h3>
            </div>
        </div>
    </div>
    <div class="col-md-3">
        <div class="card text-white bg-danger">
            <div class="card-body">
                <h5>Average Discount %</h5>
                <h3><asp:Label ID="lblAvgDiscount" runat="server" Text="0%"></asp:Label></h3>
            </div>
        </div>
    </div>
</div>

<!-- Report Grid -->
<div class="card">
    <div class="card-header bg-dark text-white">
        <h5>Discount Report Details</h5>
    </div>
    <div class="card-body">
        <div class="table-responsive">
            <asp:GridView ID="gvDiscountReport" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered table-striped table-hover" AllowPaging="True" 
                PageSize="20" OnPageIndexChanging="gvDiscountReport_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" />
                    <asp:BoundField DataField="AdmNo" HeaderText="Admission No" />
                    <asp:BoundField DataField="MRNo" HeaderText="MR No" />
                    <asp:BoundField DataField="ConsultantCode" HeaderText="Consultant Code" />
                    <asp:BoundField DataField="ConsultantName" HeaderText="Consultant Name" />
                    <asp:BoundField DataField="VisitFee" HeaderText="Visit Fee" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="ProcedureFee" HeaderText="Procedure Fee" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="TotalFee" HeaderText="Total Fee" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="DiscountPercent" HeaderText="Discount %" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="FinalAmount" HeaderText="Final Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="DiscountAmount" HeaderText="Discount Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info text-center">No records found for the selected criteria.</div>
                </EmptyDataTemplate>
                <PagerStyle CssClass="pagination-ys" />
            </asp:GridView>
        </div>
    </div>
</div>

</asp:Content>