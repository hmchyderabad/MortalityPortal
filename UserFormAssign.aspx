<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="AdminMaster.master" CodeBehind="UserFormAssign.aspx.cs" Inherits="welfareSystem.UserFormAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* ── MODERN DESIGN SYSTEM ── */
        :root {
            --brand-primary: #3b82f6;
            --brand-primary-hover: #2563eb;
            --brand-success: #10b981;
            --brand-success-hover: #059669;
            
            --surface-bg: #f8fafc;
            --surface-card: #ffffff;
            --surface-subtle: #f1f5f9;
            
            --text-main: #0f172a;
            --text-muted: #64748b;
            --text-light: #94a3b8;
            
            --border-subtle: #e2e8f0;
            --border-focus: #93c5fd;
            
            --shadow-xs: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
            --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
            --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
            
            --radius-sm: 8px;
            --radius-md: 12px;
            --radius-lg: 16px;
            --transition-fast: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
        }

        /* ── PAGE LAYOUT ── */
        .page-wrapper {
            background-color: var(--surface-bg);
            min-height: 100vh;
            padding: 32px 0 60px;
            font-family: system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
        }

        .assign-card {
            background: var(--surface-card);
            border-radius: var(--radius-lg);
            border: 1px solid var(--border-subtle);
            box-shadow: var(--shadow-md);
            overflow: hidden;
        }

        /* ── HEADER SECTION ── */
        .assign-header {
            padding: 28px 32px;
            background: var(--surface-card);
            border-bottom: 1px solid var(--border-subtle);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 16px;
        }
        .assign-header .header-info {
            display: flex;
            align-items: center;
            gap: 16px;
        }
        .assign-header .header-icon {
            width: 48px;
            height: 48px;
            border-radius: var(--radius-md);
            background: #eff6ff;
            color: var(--brand-primary);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.25rem;
            margin-left: 8px;
        }
        .assign-header h3 {
            margin: 0;
            font-size: 1.25rem;
            font-weight: 700;
            color: var(--text-main);
            letter-spacing: -0.02em;
        }
        .assign-header p {
            margin: 2px 0 0;
            font-size: 0.875rem;
            color: var(--text-muted);
        }

        /* ── CARD BODY ── */
        .assign-body {
            padding: 32px;
        }

        /* ── USER SELECT SECTION ── */
        .user-select-card {
            background: var(--surface-subtle);
            border: 1px solid var(--border-subtle);
            border-radius: var(--radius-md);
            padding: 20px 24px;
            margin-bottom: 32px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
        }
        .user-select-group {
            flex: 1;
            min-width: 280px;
        }
        .user-select-group label {
            display: block;
            font-weight: 600;
            font-size: 0.875rem;
            color: var(--text-main);
            margin-bottom: 8px;
        }
        .user-select-group .form-select {
            width: 100%;
            border-radius: var(--radius-sm);
            border: 1px solid var(--border-subtle);
            padding: 10px 14px;
            font-size: 0.925rem;
            color: var(--text-main);
            background-color: #fff;
            transition: var(--transition-fast);
            box-shadow: var(--shadow-xs);
        }
        .user-select-group .form-select:focus {
            border-color: var(--brand-primary);
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.15);
            outline: none;
        }

        .badge-count-pill {
            background: #ffffff;
            border: 1px solid var(--border-subtle);
            color: var(--text-muted);
            border-radius: 9999px;
            padding: 8px 18px;
            font-size: 0.875rem;
            font-weight: 600;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            box-shadow: var(--shadow-xs);
        }
        .badge-count-pill i {
            color: var(--brand-primary);
        }
        .badge-count-pill .count-num {
            color: var(--brand-primary);
            font-weight: 700;
        }

        /* ── PERMISSIONS GRID LAYOUT ── */
        .permissions-wrap {
            border: 1px solid var(--border-subtle);
            border-radius: var(--radius-md);
            overflow: hidden;
            background: #fff;
            box-shadow: var(--shadow-xs);
        }

        .permissions-header-grid {
            display: grid;
            grid-template-columns: 280px 1fr;
            padding: 14px 24px;
            background: #f8fafc;
            border-bottom: 1px solid var(--border-subtle);
            font-size: 0.75rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.05em;
            color: var(--text-muted);
        }

        .permissions-row-grid {
            display: grid;
            grid-template-columns: 280px 1fr;
            align-items: center;
            padding: 16px 24px;
            border-bottom: 1px solid var(--border-subtle);
            transition: var(--transition-fast);
        }
        .permissions-row-grid:last-child {
            border-bottom: none;
        }
        .permissions-row-grid:hover {
            background: #f8fafc;
        }

        .col-form-name {
            display: flex;
            align-items: center;
            gap: 12px;
            font-weight: 600;
            font-size: 0.925rem;
            color: var(--text-main);
            padding-right: 16px;
        }
        .col-form-name .form-icon {
            width: 36px;
            height: 36px;
            border-radius: var(--radius-sm);
            background: #f1f5f9;
            color: var(--text-muted);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 0.9rem;
            flex-shrink: 0;
            margin-left: 8px;
        }

        .col-permissions {
            display: flex;
            align-items: center;
            gap: 24px;
            flex-wrap: wrap;
        }

        /* ── FORM PERMISSIONS ROW LAYOUT (NEW) ── */
        .form-permissions-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
            padding: 16px 24px;
            border-bottom: 1px solid var(--border-subtle);
            background: #fff;
            transition: var(--transition-fast);
        }
        .form-permissions-row:last-child {
            border-bottom: none;
        }
        .form-permissions-row:hover {
            background: #f8fafc;
        }

        .form-name-cell {
            display: flex;
            align-items: center;
            gap: 12px;
            font-weight: 600;
            font-size: 0.925rem;
            color: var(--text-main);
            padding-top: 8px;
        }
        .form-name-cell i {
            width: 36px;
            height: 36px;
            border-radius: var(--radius-sm);
            background: #f1f5f9;
            color: var(--text-muted);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 0.9rem;
            flex-shrink: 0;
        }

        .form-check-group {
            width: 100%;
        }

        .permissions-cell {
            display: flex;
            flex-direction: row;
            gap: 8px;
            justify-content: flex-end;
        }

        .permission-row {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 6px 0;
        }
        .permission-row label {
            flex: 0 0 80px;
            font-size: 0.875rem;
            font-weight: 500;
            color: var(--text-main);
            cursor: pointer;
        }
        .permission-row input[type="checkbox"] {
            width: 2.25rem;
            height: 1.25rem;
            margin: 0;
            background-color: #cbd5e1;
            border: none;
            border-radius: 9999px;
            appearance: none;
            -webkit-appearance: none;
            transition: var(--transition-fast);
            cursor: pointer;
            position: relative;
        }
        .permission-row input[type="checkbox"]::before {
            content: "";
            position: absolute;
            top: 2px;
            left: 2px;
            width: 1rem;
            height: 1rem;
            border-radius: 50%;
            background-color: #ffffff;
            transition: var(--transition-fast);
            box-shadow: 0 1px 2px rgba(0,0,0,0.2);
        }
        .permission-row input[type="checkbox"]:checked {
            background-color: var(--brand-primary);
        }
        .permission-row input[type="checkbox"]:checked::before {
            transform: translateX(1rem);
        }
        .permission-row input[type="checkbox"]:focus {
            outline: none;
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.25);
        }

        /* ── MODERN TOGGLE SWITCHES ── */
        .perm-switch {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            font-size: 0.85rem;
            font-weight: 500;
            color: var(--text-main);
            cursor: pointer;
            user-select: none;
        }
        .perm-switch .form-check-input {
            width: 2.25rem;
            height: 1.25rem;
            margin: 0;
            background-color: #cbd5e1;
            border: none;
            border-radius: 9999px;
            appearance: none;
            -webkit-appearance: none;
            transition: var(--transition-fast);
            cursor: pointer;
            position: relative;
        }
        .perm-switch .form-check-input::before {
            content: "";
            position: absolute;
            top: 2px;
            left: 2px;
            width: 1rem;
            height: 1rem;
            border-radius: 50%;
            background-color: #ffffff;
            transition: var(--transition-fast);
            box-shadow: 0 1px 2px rgba(0,0,0,0.2);
        }
        .perm-switch .form-check-input:checked {
            background-color: var(--brand-primary);
        }
        .perm-switch .form-check-input:checked::before {
            transform: translateX(1rem);
        }
        .perm-switch .form-check-input:focus {
            outline: none;
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.25);
        }

        /* Row Quick Actions */
        .row-actions {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            margin-left: auto;
            border-left: 1px solid var(--border-subtle);
            padding-left: 16px;
        }
        .row-actions .action-link {
            font-size: 0.75rem;
            font-weight: 600;
            color: var(--brand-primary);
            cursor: pointer;
            padding: 4px 10px;
            border-radius: var(--radius-sm);
            transition: var(--transition-fast);
            background: transparent;
            border: none;
        }
        .row-actions .action-link:hover {
            background: #eff6ff;
            color: var(--brand-primary-hover);
        }
        .row-actions .action-link.clear-link {
            color: var(--text-muted);
        }
        .row-actions .action-link.clear-link:hover {
            background: var(--surface-subtle);
            color: var(--text-main);
        }

        /* ── EMPTY STATE ── */
        .empty-forms {
            text-align: center;
            padding: 64px 20px;
            color: var(--text-muted);
        }
        .empty-forms i {
            font-size: 2.5rem;
            color: var(--text-light);
            margin-bottom: 12px;
        }
        .empty-forms h5 {
            font-weight: 600;
            color: var(--text-main);
            margin-bottom: 4px;
        }

        /* ── ALERTS ── */
        .alert-message {
            display: none;
            padding: 14px 18px;
            border-radius: var(--radius-sm);
            margin-top: 24px;
            font-size: 0.875rem;
            font-weight: 500;
            align-items: center;
            gap: 10px;
        }
        .alert-message.show { display: flex; }
        .alert-message.success { background: #ecfdf5; color: #065f46; border: 1px solid #a7f3d0; }
        .alert-message.error { background: #fef2f2; color: #991b1b; border: 1px solid #fecaca; }
        .alert-message.warning { background: #fffbeb; color: #92400e; border: 1px solid #fde68a; }

        /* ── FOOTER ACTIONS ── */
        .action-buttons {
            margin-top: 32px;
            padding-top: 24px;
            border-top: 1px solid var(--border-subtle);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 16px;
            flex-wrap: wrap;
        }
        .btn-group-left {
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }
        .action-buttons .btn {
            border-radius: var(--radius-sm);
            padding: 10px 22px;
            font-weight: 600;
            font-size: 0.875rem;
            transition: var(--transition-fast);
            display: inline-flex;
            align-items: center;
            gap: 8px;
            cursor: pointer;
        }
        .btn-save {
            background: var(--brand-success);
            border: 1px solid var(--brand-success);
            color: #ffffff;
            box-shadow: var(--shadow-xs);
        }
        .btn-save:hover {
            background: var(--brand-success-hover);
            border-color: var(--brand-success-hover);
            color: #ffffff;
            transform: translateY(-1px);
            box-shadow: var(--shadow-md);
        }
        .btn-outline-custom {
            background: #ffffff;
            border: 1px solid var(--border-subtle);
            color: var(--text-main);
            box-shadow: var(--shadow-xs);
        }
        .btn-outline-custom:hover {
            background: var(--surface-subtle);
            border-color: #cbd5e1;
        }

        .badge-status {
            font-size: 0.85rem;
            color: var(--text-muted);
            font-weight: 500;
        }

        /* ── SPINNER OVERLAY ── */
        .spinner-overlay {
            display: none;
            position: fixed;
            inset: 0;
            background: rgba(255, 255, 255, 0.75);
            backdrop-filter: blur(2px);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }
        .spinner-overlay.show { display: flex; }

        /* ── RESPONSIVE BREAKPOINTS ── */
        @media (max-width: 868px) {
            .permissions-header-grid { display: none; }
            .permissions-row-grid {
                grid-template-columns: 1fr;
                gap: 16px;
                padding: 20px;
            }
            .col-form-name {
                padding-right: 0;
                padding-bottom: 12px;
                border-bottom: 1px dashed var(--border-subtle);
            }
            .row-actions {
                margin-left: 0;
                border-left: none;
                padding-left: 0;
                width: 100%;
                justify-content: flex-end;
                padding-top: 12px;
                border-top: 1px solid var(--surface-subtle);
            }
            .action-buttons {
                flex-direction: column;
                align-items: stretch;
            }
            .btn-group-left {
                flex-direction: column;
                width: 100%;
            }
            .btn-group-left .btn {
                width: 100%;
                justify-content: center;
            }
        }
    </style>

    <div class="page-wrapper">
        <div class="container-fluid">
            <div class="row justify-content-center">
                <div class="col-xl-12 col-lg-12">

                    <div class="assign-card">

                        <!-- HEADER -->
                        <div class="assign-header">
                            <div class="header-info">
                                <div class="header-icon">
                                    <i class="fa fa-user-shield" aria-hidden="true"></i>
                                </div>
                                <div>
                                    <h3>User Form Assignment</h3>
                                    <p>Configure user access levels and granular permissions for system forms.</p>
                                </div>
                            </div>
                        </div>

                        <!-- BODY -->
                        <div class="assign-body">

                            <!-- USER SELECT SECTION -->
                            <div class="user-select-card" role="region" aria-label="User selection area">
                                <div class="user-select-group">
                                    <label for="ddlUsers">
                                        <i class="fa fa-user me-2 text-muted" aria-hidden="true"></i>Select Target User
                                    </label>
                                    <asp:DropDownList
                                        ID="ddlUsers"
                                        runat="server"
                                        ClientIDMode="Static"
                                        CssClass="form-select"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged"
                                        EnableViewState="true"
                                        aria-required="true"
                                        aria-label="Select user to assign forms">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:Label
                                        ID="lblAssignedCount"
                                        runat="server"
                                        CssClass="badge-count-pill"
                                        role="status"
                                        aria-live="polite">
                                        <i class="fa fa-check-circle" aria-hidden="true"></i>
                                        <span>Assigned Forms: <span class="count-num" id="spanCount">0</span></span>
                                    </asp:Label>
                                </div>
                            </div>

                            <!-- PERMISSIONS TABLE -->
                            <div id="formsArea" runat="server" ClientIDMode="Static" role="region" aria-label="Forms assignment area">
                                <asp:Literal ID="litForms" runat="server"></asp:Literal>
                            </div>

                            <!-- NO FORMS EMPTY STATE -->
                            <div id="noFormsMessage" runat="server" visible="false" class="empty-forms" role="alert">
                                <i class="fa fa-folder-open" aria-hidden="true"></i>
                                <h5>No Forms Found</h5>
                                <p>There are currently no active system forms available for assignment.</p>
                            </div>

                            <!-- ALERT BANNER -->
                            <div id="alertMessage" class="alert-message" role="alert" aria-live="assertive">
                                <i id="alertIcon" class="fa" aria-hidden="true"></i>
                                <span id="alertText"></span>
                            </div>

                            <!-- BOTTOM ACTIONS -->
                            <div class="action-buttons" role="group" aria-label="Form assignment actions">
                                <div class="btn-group-left">
                                    <asp:Button
                                        ID="btnSave"
                                        runat="server"
                                        Text="Save Assignments"
                                        CssClass="btn btn-save"
                                        OnClick="btnSave_Click"
                                        OnClientClick="return validateBeforeSave();"
                                        aria-label="Save form assignments" />
                                    <asp:Button
                                        ID="btnSelectAll"
                                        runat="server"
                                        Text="Select All"
                                        CssClass="btn btn-outline-custom"
                                        OnClientClick="selectAllForms(); return false;"
                                        aria-label="Select all forms and permissions" />
                                    <asp:Button
                                        ID="btnClearAll"
                                        runat="server"
                                        Text="Clear All"
                                        CssClass="btn btn-outline-custom"
                                        OnClientClick="clearAllForms(); return false;"
                                        aria-label="Clear all form selections" />
                                </div>
                                <div>
                                    <asp:Label
                                        ID="lblStatus"
                                        runat="server"
                                        CssClass="badge-status"
                                        role="status"
                                        aria-live="polite"></asp:Label>
                                </div>
                            </div>

                        </div>
                        <!-- /assign-body -->

                    </div>
                    <!-- /assign-card -->

                </div>
            </div>
        </div>
    </div>

    <!-- SPINNER -->
    <div class="spinner-overlay" id="spinnerOverlay" role="status" aria-live="polite" aria-label="Loading">
        <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading…</span>
        </div>
    </div>

    <!-- JAVASCRIPT -->
    <script type="text/javascript">
        function safeQS(selector) {
            try { return document.querySelectorAll(selector); } catch (_) { return []; }
        }

        function updateCount() {
            try {
                var formIds = new Set();
                var switches = safeQS('.permission-row input[type="checkbox"]:checked');
                switches.forEach(function(chk) {
                    var fid = chk.getAttribute('data-formid');
                    if (fid) formIds.add(fid);
                });
                var span = document.getElementById('spanCount');
                if (span) span.textContent = formIds.size;
            } catch (e) { console.warn('updateCount error:', e); }
        }

        function selectAllForms() {
            try {
                var switches = safeQS('.permission-row input[type="checkbox"]');
                switches.forEach(function(chk) { chk.checked = true; });
                updateCount();
            } catch (e) { console.warn('selectAllForms error:', e); }
            return false;
        }

        function clearAllForms() {
            try {
                var switches = safeQS('.permission-row input[type="checkbox"]');
                switches.forEach(function(chk) { chk.checked = false; });
                updateCount();
            } catch (e) { console.warn('clearAllForms error:', e); }
            return false;
        }

        function selectAllPermissionsForForm(formId) {
            try {
                var switches = safeQS('.permission-row input[type="checkbox"][data-formid="' + formId + '"]');
                switches.forEach(function(chk) { chk.checked = true; });
                updateCount();
            } catch (e) { console.warn('selectAllPermissionsForForm error:', e); }
        }

        function clearAllPermissionsForForm(formId) {
            try {
                var switches = safeQS('.permission-row input[type="checkbox"][data-formid="' + formId + '"]');
                switches.forEach(function(chk) { chk.checked = false; });
                updateCount();
            } catch (e) { console.warn('clearAllPermissionsForForm error:', e); }
        }

        function toggleCategorySelectAll(selectAllCheckbox) {
            try {
                var isChecked = selectAllCheckbox.checked;
                var collapseDiv = selectAllCheckbox.closest('.accordion-body');
                if (!collapseDiv) return;

                var checkboxes = collapseDiv.querySelectorAll('.permission-row input[type="checkbox"]');
                checkboxes.forEach(function(chk) {
                    chk.checked = isChecked;
                });
                updateCount();
            } catch (e) { console.warn('toggleCategorySelectAll error:', e); }
        }

        function showSpinner() {
            var ov = document.getElementById('spinnerOverlay');
            if (ov) ov.classList.add('show');
        }

        function hideSpinner() {
            var ov = document.getElementById('spinnerOverlay');
            if (ov) ov.classList.remove('show');
        }

        function showAlert(message, type) {
            var el = document.getElementById('alertMessage');
            var txt = document.getElementById('alertText');
            var icon = document.getElementById('alertIcon');
            if (!el || !txt || !icon) return;

            el.classList.remove('success', 'error', 'warning', 'show');
            el.classList.add(type, 'show');

            var icons = {
                success: 'fa-check-circle',
                error: 'fa-exclamation-circle',
                warning: 'fa-exclamation-triangle'
            };
            icon.className = 'fa ' + (icons[type] || 'fa-info-circle');
            txt.textContent = message;

            if (type === 'success') setTimeout(hideAlert, 5000);
        }

        function hideAlert() {
            var el = document.getElementById('alertMessage');
            if (el) el.classList.remove('show');
        }

        function validateBeforeSave() {
            var ddl = document.getElementById('ddlUsers');
            if (!ddl || ddl.value === '' || ddl.value === '0') {
                showAlert('Please select a valid user before saving assignments.', 'warning');
                return false;
            }
            var checked = document.querySelectorAll('.permission-row input[type="checkbox"]:checked');
            if (checked.length === 0) {
                showAlert('Please select at least one permission switch before saving.', 'warning');
                return false;
            }
            showSpinner();
            return true;
        }

        function initPageScript() {
            var area = document.getElementById('formsArea');
            if (area) {
                area.addEventListener('change', function(e) {
                    var t = e.target;
                    if (t.type === 'checkbox' && t.closest('.permission-row')) {
                        updateCount();
                    }
                    if (t.classList.contains('select-all-category')) {
                        toggleCategorySelectAll(t);
                    }
                });
            }
            updateCount();
            hideSpinner();
        }

        document.addEventListener('DOMContentLoaded', initPageScript);

        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initPageScript);
        }
    </script>
</asp:Content>