<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="ChartOfRemission.aspx.cs" Inherits="welfareSystem.ChartOfRemission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h2 class="fw-bold text-dark mb-1">Chart of Remission</h2>
            <p class="text-muted">Configure income qualifiers, billing ranges, and welfare codes.</p>
        </div>
        <div class="badge bg-danger-subtle text-danger px-3 py-2 rounded-pill fs-6">
            <i class="fa fa-shield-halved me-1"></i> Master Configuration
        </div>
    </div>

    <div class="row justify-content-center">
        <div class="col-xl-10">
            <div class="card shadow-sm border-0 overflow-hidden">
                <div style="height: 4px; background: linear-gradient(90deg, #c82333, #dc3545);"></div>
                
                <div class="card-body p-4 p-md-5">
                    
                    <div class="row mb-4">
                        <div class="col-md-6 mb-3 mb-md-0">
                            <label class="form-label fw-bold text-secondary text-uppercase small">1. Qualifier (Income Category)</label>
                            <div class="input-group">
                                <span class="input-group-text bg-white"><i class="fa fa-ranking-star text-primary"></i></span>
                                <asp:DropDownList ID="ddlQualifier" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">-- Select Qualifier --</asp:ListItem>
                                    <asp:ListItem Value="Ultra Poor">Ultra Poor</asp:ListItem>
                                    <asp:ListItem Value="Relatively Poor">Relatively Poor</asp:ListItem>
                                    <asp:ListItem Value="Lower Middle">Lower Middle</asp:ListItem>
                                    <asp:ListItem Value="Relatively Affording">Relatively Affording</asp:ListItem>
                                    <asp:ListItem Value="Affording">Affording</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label fw-bold text-secondary text-uppercase small">2. Welfare Code</label>
                            <div class="input-group">
                                <span class="input-group-text bg-white"><i class="fa fa-qrcode text-primary"></i></span>
                                <asp:DropDownList ID="ddlWelfareCode" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <hr class="my-4 opacity-25">

                    <div class="row mb-4">
                        <div class="col-12 mb-2">
                            <label class="form-label fw-bold text-secondary text-uppercase small">3. Bill Amount Range (PKR)</label>
                        </div>
                        <div class="col-md-6 mb-3 mb-md-0">
                            <div class="input-group">
                                <span class="input-group-text">From</span>
                                <asp:TextBox ID="txtBillFrom" runat="server" CssClass="form-control text-end" placeholder="0.00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text">To</span>
                                <asp:TextBox ID="txtBillTo" runat="server" CssClass="form-control text-end" placeholder="Unlimited"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-12 mb-2">
                            <label class="form-label fw-bold text-secondary text-uppercase small">4. No. of Dependants</label>
                        </div>
                        <div class="col-md-4 mb-3 mb-md-0">
                            <div class="input-group">
                                <span class="input-group-text bg-light"><i class="fa fa-users-slash me-2 small"></i> Min</span>
                                <asp:TextBox ID="txtDepFrom" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4 mb-3 mb-md-0">
                            <div class="input-group">
                                <span class="input-group-text bg-light"><i class="fa fa-users me-2 small"></i> Max</span>
                                <asp:TextBox ID="txtDepTo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="input-group">
                                <span class="input-group-text bg-light border-danger-subtle"><i class="fa fa-tag me-2 small"></i> Code</span>
                                <asp:DropDownList ID="ddlDepCode" runat="server" CssClass="form-select bg-light border-danger-subtle fw-bold">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-5">
                        <div class="col-12 mb-2">
                            <label class="form-label fw-bold text-secondary text-uppercase small">5. Remission Percentage (%)</label>
                        </div>
                        <div class="col-md-6 mb-3 mb-md-0">
                            <div class="input-group">
                                <span class="input-group-text bg-white border-success-subtle text-success fw-bold">% Min</span>
                                <asp:TextBox ID="txtPerFrom" runat="server" CssClass="form-control border-success-subtle"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text bg-white border-success-subtle text-success fw-bold">% Max</span>
                                <asp:TextBox ID="txtPerTo" runat="server" CssClass="form-control border-success-subtle"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="d-grid d-md-flex justify-content-md-end gap-2 border-top pt-4">
                        <button type="reset" class="btn btn-light px-4 py-2 fw-semibold">Clear Form</button>
                        <asp:Button ID="btnSave" runat="server" Text="Save Configuration" 
                        CssClass="btn btn-primary px-5 py-2 fw-bold shadow-sm shadow-blue"
                        OnClick="btnSave_Click" />
                    </div>

                </div>
                <hr />
<div style="max-height:400px; overflow-y:auto; overflow-x:auto;">
<asp:GridView ID="gvRemission" runat="server" AutoGenerateColumns="false"
    CssClass="table table-bordered table-striped"
    DataKeyNames="Id"
    AllowPaging="true"
    PageSize="5"
    OnPageIndexChanging="gvRemission_PageIndexChanging"
    OnRowEditing="gvRemission_RowEditing"
    OnRowCancelingEdit="gvRemission_RowCancelingEdit"
    OnRowUpdating="gvRemission_RowUpdating"
    OnRowDeleting="gvRemission_RowDeleting">

    <Columns>

        <asp:TemplateField HeaderText="Qualifier">
            <ItemTemplate><%# Eval("Qualifier") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtQualifier" runat="server" Text='<%# Bind("Qualifier") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Welfare Code">
            <ItemTemplate><%# Eval("WelfareCode") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtWelfareCode" runat="server" Text='<%# Bind("WelfareCode") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Bill From">
            <ItemTemplate><%# Eval("BillFrom") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtBillFrom" runat="server" Text='<%# Bind("BillFrom") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Bill To">
            <ItemTemplate><%# Eval("BillTo") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtBillTo" runat="server" Text='<%# Bind("BillTo") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Dep From">
            <ItemTemplate><%# Eval("DependantFrom") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtDepFrom" runat="server" Text='<%# Bind("DependantFrom") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Dep To">
            <ItemTemplate><%# Eval("DependantTo") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtDepTo" runat="server" Text='<%# Bind("DependantTo") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Dep Code">
            <ItemTemplate><%# Eval("DependantCode") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtDepCode" runat="server" Text='<%# Bind("DependantCode") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="% From">
            <ItemTemplate><%# Eval("PercentageFrom") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtPerFrom" runat="server" Text='<%# Bind("PercentageFrom") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="% To">
            <ItemTemplate><%# Eval("PercentageTo") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtPerTo" runat="server" Text='<%# Bind("PercentageTo") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />

    </Columns>
</asp:GridView>
    </div>
            </div>
        </div>
    </div>
</div>

<style>
    body { background-color: #f0f2f5; }
    .card { border-radius: 15px; }
    .form-select, .form-control { 
        padding: 0.6rem 0.75rem;
        border: 1px solid #ced4da;
    }
    .form-control:focus, .form-select:focus {
        border-color: #c82333;
        box-shadow: 0 0 0 0.25rem rgba(200, 35, 51, 0.1);
    }
    .input-group-text {
        font-size: 0.85rem;
        font-weight: 600;
        color: #6c757d;
    }
    .shadow-blue { box-shadow: 0 4px 14px 0 rgba(13, 110, 253, 0.39) !important; }
    .btn-primary { background-color: #0d6efd; border: none; }
    .btn-primary:hover { background-color: #0b5ed7; }
    .table td input[type="text"]{
        width: 100%;
        min-width: 80px;
    }

    .grid-container{
        max-height:400px;
        overflow:auto;
    }
</style>



<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>