<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="AdminMaster.master"
    CodeBehind="UserFormAssign.aspx.cs"
    Inherits="welfareSystem.UserFormAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <style>
        .form-assign-container {
            background: #f8f9fa;
            min-height: 100vh;
            padding: 20px 0;
        }

        .form-assign-card {
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.08);
            border: none;
            overflow: hidden;
        }

        .form-assign-header {
            background: linear-gradient(135deg, #2c3e50 0%, #3498db 100%);
            color: white;
            padding: 20px 25px;
        }

        .form-assign-header h3 {
            margin: 0;
            font-weight: 600;
        }

        .form-assign-header h3 i {
            margin-right: 10px;
        }

        .form-assign-body {
            padding: 25px;
        }

        .user-select-area {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 25px;
            border: 2px dashed #dee2e6;
        }

        .user-select-area label {
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 8px;
        }

        .user-select-area .form-select {
            border-radius: 8px;
            border: 2px solid #dee2e6;
            padding: 10px 15px;
            font-size: 14px;
        }

        .user-select-area .form-select:focus {
            border-color: #3498db;
            box-shadow: 0 0 0 0.2rem rgba(52, 152, 219, 0.25);
        }

        .category-accordion {
            margin-top: 10px;
        }

        .category-accordion .accordion-item {
            border: none;
            margin-bottom: 8px;
            border-radius: 10px !important;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
        }

        .category-accordion .accordion-header {
            background: #fff;
        }

        .category-accordion .accordion-button {
            background: #fff;
            padding: 12px 20px;
            font-weight: 600;
            font-size: 14px;
            color: #2c3e50;
            border: none;
            box-shadow: none;
            cursor: pointer;
        }

        .category-accordion .accordion-button:not(.collapsed) {
            background: #e8f4fd;
            color: #0d6efd;
        }

        .category-accordion .accordion-button:focus {
            box-shadow: none;
        }

        .category-accordion .accordion-button i {
            margin-right: 10px;
            font-size: 16px;
        }

        .category-accordion .accordion-button .badge-count {
            background: #e9ecef;
            color: #495057;
            border-radius: 20px;
            padding: 2px 10px;
            font-size: 11px;
            margin-left: 10px;
        }

        .category-accordion .accordion-body {
            padding: 5px 20px 20px 20px;
            background: #fafbfc;
        }

        .sub-category {
            margin-bottom: 10px;
        }

        .sub-category-title {
            font-weight: 600;
            font-size: 13px;
            color: #6c757d;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            padding: 8px 0;
            border-bottom: 1px solid #e9ecef;
            margin-bottom: 8px;
        }

        .sub-category-title i {
            margin-right: 6px;
            font-size: 12px;
        }

        .form-check-group {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 4px 15px;
            padding: 5px 0;
        }

        .form-check-group .form-check {
            padding: 4px 0;
            margin: 0;
        }

        .form-check-group .form-check-input {
            margin-top: 6px;
            cursor: pointer;
        }

        .form-check-group .form-check-input:checked {
            background-color: #0d6efd;
            border-color: #0d6efd;
        }

        .form-check-group .form-check-label {
            font-size: 13px;
            cursor: pointer;
            color: #333;
            padding-left: 5px;
        }

        .form-check-group .form-check-label:hover {
            color: #0d6efd;
        }

        .select-all-container {
            margin-bottom: 10px;
            padding: 8px 0;
            border-bottom: 1px dashed #dee2e6;
        }

        .select-all-container .form-check {
            padding: 0;
        }

        .select-all-container .form-check-label {
            font-weight: 600;
            color: #0d6efd;
            font-size: 13px;
        }

        .empty-forms {
            text-align: center;
            padding: 30px;
            color: #6c757d;
        }

        .empty-forms i {
            font-size: 40px;
            margin-bottom: 10px;
            opacity: 0.5;
        }

        .action-buttons {
            margin-top: 25px;
            padding-top: 20px;
            border-top: 2px solid #e9ecef;
        }

        .action-buttons .btn {
            border-radius: 8px;
            padding: 10px 30px;
            font-weight: 600;
            font-size: 14px;
        }

        .btn-save {
            background: linear-gradient(135deg, #28a745, #20c997);
            border: none;
            color: white;
        }

        .btn-save:hover {
            background: linear-gradient(135deg, #218838, #1aa87a);
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(40, 167, 69, 0.3);
        }

        .btn-clear {
            background: #e9ecef;
            border: none;
            color: #495057;
        }

        .btn-clear:hover {
            background: #dee2e6;
            color: #212529;
        }

        @media (max-width: 768px) {
            .form-check-group {
                grid-template-columns: 1fr;
            }
            .form-assign-body {
                padding: 15px;
            }
            .action-buttons .btn {
                width: 100%;
                margin-bottom: 10px;
            }
        }

        .spinner-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255,255,255,0.7);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }
        .spinner-overlay.show {
            display: flex;
        }
        .spinner-overlay .spinner-border {
            width: 50px;
            height: 50px;
        }

        .module-icon-welfare { color: #dc3545; }
        .module-icon-security { color: #0d6efd; }
        .module-icon-transactions { color: #fd7e14; }
        .module-icon-reports { color: #198754; }
        .module-icon-dashboards { color: #6f42c1; }

        .accordion-button::after {
            flex-shrink: 0;
            width: 1.25rem;
            height: 1.25rem;
            margin-left: auto;
            content: "";
            background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23212529'%3e%3cpath fill-rule='evenodd' d='M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z'/%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-size: 1.25rem;
            transition: transform .2s ease-in-out;
        }

        .accordion-button:not(.collapsed)::after {
            transform: rotate(-180deg);
        }

        .accordion-button.collapsed::after {
            transform: rotate(0deg);
        }
    </style>

    <div class="form-assign-container">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-xl-10 col-lg-11">

                    <div class="form-assign-card card">
                        <!-- Header -->
                        <div class="form-assign-header">
                            <h3>
                                <i class="fa fa-user-cog"></i>User Form Assignment
                            </h3>
                            <small class="opacity-75">Assign forms to users with role-based access</small>
                        </div>

                        <!-- Body -->
                        <div class="form-assign-body">

                            <!-- User Selection -->
                            <div class="user-select-area">
                                <div class="row align-items-end">
                                    <div class="col-md-8">
                                        <label for="ddlUsers">
                                            <i class="fa fa-user me-2"></i>Select User
                                        </label>
                                        <asp:DropDownList ID="ddlUsers" runat="server"
                                            CssClass="form-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 text-md-end mt-3 mt-md-0">
                                        <asp:Label ID="lblAssignedCount" runat="server" 
                                            CssClass="badge bg-info text-white p-2 fs-6">
                                            <i class="fa fa-check-circle me-1"></i>
                                            <span id="spanCount">0</span> forms assigned
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>

                            <!-- Forms Area - Using Literal to render HTML -->
                            <div id="formsArea" runat="server">
                                <asp:Literal ID="litForms" runat="server"></asp:Literal>
                            </div>

                            <!-- No Forms Message -->
                            <div id="noFormsMessage" runat="server" visible="false" class="empty-forms">
                                <i class="fa fa-clipboard-list"></i>
                                <h5>No Forms Available</h5>
                                <p class="text-muted">Please add forms to the system first.</p>
                            </div>

                            <!-- Action Buttons -->
                            <div class="action-buttons">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:Button ID="btnSave" runat="server" 
                                            Text="💾 Save Assignments" 
                                            CssClass="btn btn-save me-2"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnSelectAll" runat="server" 
                                            Text="☑️ Select All" 
                                            CssClass="btn btn-outline-primary me-2"
                                            OnClientClick="selectAllForms(); return false;" />
                                        <asp:Button ID="btnClearAll" runat="server" 
                                            Text="🔲 Clear All" 
                                            CssClass="btn btn-outline-secondary"
                                            OnClientClick="clearAllForms(); return false;" />
                                    </div>
                                    <div class="col-md-4 text-md-end">
                                        <asp:Label ID="lblStatus" runat="server" 
                                            CssClass="text-muted small"></asp:Label>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- Loading Spinner -->
    <div class="spinner-overlay" id="spinnerOverlay">
        <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>


    <script type="text/javascript">
        // ==============================
        // UPDATE SELECTED COUNT
        // ==============================
        function updateCount() {
            var selected = document.querySelectorAll('.form-check-option:checked').length;
            var span = document.getElementById('spanCount');
            if (span) {
                span.textContent = selected;
            }
        }

        // ==============================
        // SELECT ALL FORMS
        // ==============================
        function selectAllForms() {
            var checkboxes = document.querySelectorAll('.form-check-option');
            checkboxes.forEach(function (chk) {
                chk.checked = true;
            });
            document.querySelectorAll('.select-all-category').forEach(function (chk) {
                chk.checked = true;
            });
            updateCount();
            return false;
        }

        // ==============================
        // CLEAR ALL FORMS
        // ==============================
        function clearAllForms() {
            var checkboxes = document.querySelectorAll('.form-check-option');
            checkboxes.forEach(function (chk) {
                chk.checked = false;
            });
            document.querySelectorAll('.select-all-category').forEach(function (chk) {
                chk.checked = false;
            });
            updateCount();
            return false;
        }

        // ==============================
        // SELECT ALL IN CATEGORY
        // ==============================
        document.addEventListener('DOMContentLoaded', function () {
            // Individual checkbox change
            document.querySelectorAll('.form-check-option').forEach(function (checkbox) {
                checkbox.addEventListener('change', function () {
                    updateCount();
                });
            });

            // Select All in Category
            document.querySelectorAll('.select-all-category').forEach(function (checkbox) {
                checkbox.addEventListener('change', function () {
                    var parentBody = this.closest('.accordion-body');
                    if (parentBody) {
                        var formChecks = parentBody.querySelectorAll('.form-check-option');
                        formChecks.forEach(function (chk) {
                            chk.checked = checkbox.checked;
                        });
                        updateCount();
                    }
                });
            });

            // Update count on page load
            updateCount();
            hideSpinner();
        });

        function showSpinner() {
            document.getElementById('spinnerOverlay').classList.add('show');
        }

        function hideSpinner() {
            document.getElementById('spinnerOverlay').classList.remove('show');
        }
    </script>

</asp:Content>