<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="CategoryServices.aspx.cs" Inherits="welfareSystem.CategoryServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="fw-bold text-primary">Category Services Management</h2>
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Admin</a></li>
                <li class="breadcrumb-item active">Category Services</li>
            </ol>
        </nav>
    </div>

    <asp:HiddenField ID="hfId" runat="server"/>

    <div class="card shadow-sm border-0 mb-4">
        <div class="card-header bg-white py-3">
            <h5 class="card-title mb-0 text-secondary">Add / Update Services</h5>
        </div>
        <div class="card-body">
            <div class="row g-3">
                <div class="col-md-6 col-lg-4">
                    <label class="form-label fw-semibold">Select Category</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <label class="form-label fw-semibold d-block mb-3">Available Services with Percentage</label>
                    <div class="p-3 bg-light rounded border">
                        <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                            <HeaderTemplate>
                                <table class="table table-borderless mb-0">
                                    <thead>
                                        <tr>
                                            <th style="width: 30%;">Service Name</th>
                                            <th style="width: 25%;">Min Percentage (%)</th>
                                            <th style="width: 25%;">Max Percentage (%)</th>
                                            <th style="width: 20%;">Select</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hfServiceId" runat="server" Value='<%# Eval("ServiceId") %>' />
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%# Eval("ServiceName") %>' CssClass="fw-semibold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMinPercentage" runat="server" CssClass="form-control form-control-sm" 
                                            TextMode="Number" min="0" max="100" step="0.01" 
                                            placeholder="Min %" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxPercentage" runat="server" CssClass="form-control form-control-sm" 
                                            TextMode="Number" min="0" max="100" step="0.01" 
                                            placeholder="Max %" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkSelected" runat="server" CssClass="form-check-input" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>

            <div class="mt-4">
                <asp:Button ID="btnSave" runat="server" Text="Save Configuration" 
                    CssClass="btn btn-primary px-4 py-2 fw-bold shadow-sm" OnClick="btnSave_Click"/>
                <asp:Button ID="btnClear" runat="server" Text="Clear All" 
                    CssClass="btn btn-secondary px-4 py-2 fw-bold shadow-sm ms-2" OnClick="btnClear_Click"/>
            </div>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-3 d-block" />
        </div>
    </div>

   <div class="card shadow-sm border-0">
    <div class="card-header bg-white py-3">
        <h5 class="card-title mb-0 text-secondary">Existing Records</h5>
    </div>
    <div class="card-body p-0">
        <div class="table-responsive">
            <asp:GridView ID="GridView1" runat="server" 
                CssClass="table table-hover align-middle mb-0" 
                GridLines="None"
                AutoGenerateColumns="false"
                OnRowDataBound="GridView1_RowDataBound">
                <HeaderStyle CssClass="table-light text-muted text-uppercase small" />
                <Columns>
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" ItemStyle-CssClass="fw-bold text-dark ps-4" HeaderStyle-CssClass="ps-4"/>
                    <asp:BoundField DataField="Services" HeaderText="Assigned Services with Percentage" />
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-4" HeaderStyle-CssClass="text-end pe-4">
                        <ItemTemplate>
                            <button type="button" class="btn btn-outline-primary btn-sm me-2 rounded-pill px-3"
                                onclick="editRow('<%# Eval("CategoryId") %>')">
                                <i class="bi bi-pencil"></i> Edit
                            </button>
                            <button type="button" class="btn btn-outline-danger btn-sm rounded-pill px-3"
                                onclick="deleteRow('<%# Eval("CategoryId") %>')">
                                <i class="bi bi-trash"></i> Delete
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</div>

<style>
    body { background-color: #f8f9fa; }
    .card { border-radius: 12px; }
    .form-select, .form-control { border-radius: 8px; border: 1px solid #dee2e6; padding: 10px; }
    .form-select:focus { border-color: #0d6efd; box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.1); }
    .table thead th { font-weight: 600; letter-spacing: 0.5px; border-bottom-width: 1px; }
    .table tbody tr:hover { background-color: #f8f9fa; }
</style>

<script>
    function editRow(id) {
        window.location = "CategoryServices.aspx?edit=" + id;
    }

    function deleteRow(id) {
        if (confirm("Are you sure you want to delete this record? This action cannot be undone.")) {
            window.location = "CategoryServices.aspx?delete=" + id;
        }
    }
</script>

</asp:Content>