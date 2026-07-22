<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" 
    CodeBehind="ApprovalLimit.aspx.cs" Inherits="welfareSystem.ApprovalLimit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">
    <h3>Approval Limit Master</h3>
    <div class="row justify-content-center">
        <div class="col-xl-10">
            <div class="card shadow-sm border-0 overflow-hidden">
                <div style="height: 4px; background: linear-gradient(90deg, #c82333, #dc3545);"></div>
                <div class="card-body">
                    
                    <div class="row mb-3">
                        <!-- Donor -->
                        <div class="col-md-4">
                            <label>Donor</label>
                            <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>

                        <!-- Amount From -->
                        <div class="col-md-4">
                            <label>Amount From %</label>
                            <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <!-- Amount To -->
                        <div class="col-md-4">
                            <label>Amount To %</label>
                            <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Buttons -->
                    <asp:Button ID="btnSave" runat="server" Text="Save"
                        CssClass="btn btn-primary" OnClick="btnSave_Click" />

                    <asp:Button ID="btnClear" runat="server" Text="Clear"
                        CssClass="btn btn-secondary" OnClick="btnClear_Click" />

                    <hr />

                    <!-- GRID -->
                    <asp:GridView ID="gvData" runat="server" 
                        AutoGenerateColumns="false"
                        DataKeyNames="Id"
                        OnRowEditing="gv_RowEditing"
                        OnRowUpdating="gv_RowUpdating"
                        OnRowCancelingEdit="gv_RowCancelingEdit"
                        OnRowDeleting="gv_RowDeleting"
                        OnRowDataBound="gvData_RowDataBound"
                        CssClass="table table-bordered table-striped">

                        <Columns>
                            <asp:BoundField DataField="DonorName" HeaderText="Donor" ReadOnly="true" />
                            <asp:BoundField DataField="AmountFrom" HeaderText="From %" 
                                DataFormatString="{0:0.00}%" HtmlEncode="false" />
                            <asp:BoundField DataField="AmountTo" HeaderText="To %" 
                                DataFormatString="{0:0.00}%" HtmlEncode="false" />

                            <asp:CommandField ShowEditButton="true" 
                                ShowDeleteButton="true" 
                                ButtonType="Button"
                                ControlStyle-CssClass="btn btn-sm" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</div>

<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>