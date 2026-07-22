<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="Services_discount_report.aspx.cs"
    Inherits="welfareSystem.Services_discount_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>Services Discount Report</h2>

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
                <label>Select Service:</label>
                <asp:DropDownList ID="ddlService" runat="server" CssClass="form-control"></asp:DropDownList>
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
                <h5>Total Services</h5>
                <h3><asp:Label ID="lblTotalServices" runat="server" Text="0"></asp:Label></h3>
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
        <h5>Services Discount Report Details</h5>
    </div>
    <div class="card-body">
        <div class="table-responsive">
            <asp:GridView ID="gvServicesReport" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered table-striped table-hover" AllowPaging="True" 
                PageSize="20" OnPageIndexChanging="gvServicesReport_PageIndexChanging"
                OnRowDataBound="gvServicesReport_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Service ID" />
                    <asp:BoundField DataField="FinalApprovalId" HeaderText="Final Approval ID" />
                    <asp:BoundField DataField="ServiceName" HeaderText="Service Name" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="DiscountPercent" HeaderText="Discount %" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="DiscountAmount" HeaderText="Discount Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="FinalAmount" HeaderText="Final Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="ApprovalDate" HeaderText="Approval Date" DataFormatString="{0:yyyy-MM-dd}" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info text-center">No records found for the selected criteria.</div>
                </EmptyDataTemplate>
                <PagerStyle CssClass="pagination-ys" />
            </asp:GridView>
        </div>
    </div>
</div>

<!-- Service-wise Summary Table -->
<div class="card mt-4" id="divServiceSummary" runat="server" visible="false">
    <div class="card-header bg-secondary text-white">
        <h5>Service-wise Summary</h5>
    </div>
    <div class="card-body">
        <div class="table-responsive">
            <asp:GridView ID="gvServiceSummary" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered table-striped">
                <Columns>
                    <asp:BoundField DataField="ServiceName" HeaderText="Service Name" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="TotalDiscount" HeaderText="Total Discount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="TotalFinalAmount" HeaderText="Total Final Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="ServiceCount" HeaderText="No. of Services" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AvgDiscountPercent" HeaderText="Avg Discount %" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

</asp:Content>