<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeathCertificate.aspx.cs" Inherits="welfareSystem.Mortality_Module.DeathCertificate" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Death Certificate - Hashim Medical City</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <style>
        body {
            background: #eef2f9;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        .certificate-card {
            background: linear-gradient(135deg, #f5f7fa 0%, #fff 100%);
            border-radius: 20px;
            box-shadow: 0 20px 35px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        .cert-header {
            background: #1a5a4c;
            padding: 20px 30px;
            color: white;
        }
        .badge-death {
            background: #dc3545;
            padding: 5px 15px;
            border-radius: 30px;
            font-size: 12px;
            font-weight: 600;
        }
        .search-card {
            background: white;
            border-radius: 15px;
            padding: 20px;
            margin-bottom: 25px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.05);
        }
        .modern-table th {
            background: #2c3e50;
            color: white;
            font-weight: 600;
        }
        .cert-modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.6);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }
        .cert-modal-content {
            background: white;
            width: 800px;
            max-width: 90%;
            border-radius: 20px;
            max-height: 90%;
            overflow-y: auto;
            box-shadow: 0 30px 50px rgba(0,0,0,0.3);
        }
        @media print {
            .no-print { display: none; }
            .cert-modal-content { 
                width: 100%; 
                margin: 0; 
                box-shadow: none;
            }
            .certificate-print-area {
                padding: 20px;
            }
        }
        .glow-text {
            color: #1a5a4c;
            font-weight: 700;
        }
        .navbar-custom {
            background: #1a5a4c;
            padding: 10px 20px;
            color: white;
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <!-- Simple Navbar -->
        <div class="navbar-custom">
            <div class="container">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h4 class="mb-0"><i class="fas fa-hospital"></i> Hashim Medical City</h4>
                        <small>Death Certificate Management System</small>
                    </div>
                   <%-- <div>
                        <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn btn-light btn-sm" OnClick="btnLogout_Click">
                            <i class="fas fa-sign-out-alt"></i> Logout
                        </asp:LinkButton>
                    </div>--%>
                </div>
            </div>
        </div>

        <div class="container mt-4">
            
            <!-- Page Header -->
            <div class="row mb-4">
                <div class="col-12">
                    <div class="certificate-card">
                        <div class="cert-header d-flex justify-content-between align-items-center">
                            <div>
                                <h3 class="mb-0"><i class="fas fa-file-alt"></i> Death Certificate Management</h3>
                                <small>Mortality Registry - Hashim Medical City</small>
                            </div>
                            <div>
                                <span class="badge-death"><i class="fas fa-heartbeat"></i> Issued by Medical Officer</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Search Panel -->
            <div class="search-card">
                <div class="row align-items-end">
                    <div class="col-md-5">
                        <label class="fw-bold text-muted"><i class="fas fa-search"></i> Search Patient</label>
                        <asp:TextBox ID="txtSearch" runat="server" 
                            CssClass="form-control form-control-lg"
                            placeholder="MR No. / Patient Name / Admission No. / CNIC"
                            AutoPostBack="true"
                            OnTextChanged="txtSearch_TextChanged">
                        </asp:TextBox>
                        <small class="text-muted">Type at least 2 characters to search</small>
                    </div>
                    <div class="col-md-3">
                        <label class="fw-bold text-muted"><i class="fas fa-filter"></i> Filter by Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Text="-- All Certificates --" Value=""></asp:ListItem>
                            <asp:ListItem Text="Issued Certificates" Value="Issued"></asp:ListItem>
                            <asp:ListItem Text="Pending (No Certificate)" Value="Pending"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnReset" runat="server" Text="🔄 Reset" CssClass="btn btn-secondary w-100" OnClick="btnReset_Click" />
                            <asp:Button ID="btnNewCertificate" runat="server" Text="📝 New Certificate" CssClass="btn btn-success w-100" OnClick="btnNewCertificate_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Patient Details Panel -->
            <asp:Panel ID="pnlPatientDetails" runat="server" Visible="false" CssClass="mb-4">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-info text-white">
                        <h5 class="mb-0"><i class="fas fa-user-circle"></i> Patient Information</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4">
                                <p><b>MR No:</b> <asp:Label ID="lblMRNo" runat="server" CssClass="glow-text" /></p>
                            </div>
                            <div class="col-md-4">
                                <p><b>Admission No:</b> <asp:Label ID="lblAdmNo" runat="server" /></p>
                            </div>
                            <div class="col-md-4">
                                <p><b>Patient Name:</b> <asp:Label ID="lblPatientName" runat="server" /></p>
                            </div>
                            <div class="col-md-4">
                                <p><b>Age / Gender:</b> <asp:Label ID="lblAgeGender" runat="server" /></p>
                            </div>
                            <div class="col-md-4">
                                <p><b>CNIC:</b> <asp:Label ID="lblCNIC" runat="server" /></p>
                            </div>
                            <div class="col-md-4">
                                <p><b>Contact:</b> <asp:Label ID="lblContact" runat="server" /></p>
                            </div>
                            <div class="col-md-4">
                                <p><b>Consultant:</b> <asp:Label ID="lblConsultant" runat="server" /></p>
                            </div>
                            <div class="col-md-8">
                                <p><b>Diagnosis:</b> <asp:Label ID="lblDiagnosis" runat="server" /></p>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Death Certificate Form -->
            <asp:Panel ID="pnlCertificateForm" runat="server" Visible="false" CssClass="mb-4">
                <div class="card shadow-lg border-0">
                    <div class="card-header bg-danger text-white">
                        <h5 class="mb-0"><i class="fas fa-notes-medical"></i> Death Certificate Details</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="fw-bold">Date of Death <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtDateOfDeath" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="fw-bold">Time of Death <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtTimeOfDeath" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label class="fw-bold">Cause of Death <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtCauseOfDeath" runat="server" CssClass="form-control" 
                                        placeholder="e.g., Cardiac Arrest, Respiratory Failure, etc."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label class="fw-bold">Final Diagnosis</label>
                                    <asp:TextBox ID="txtFinalDiagnosis" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <hr />
                                <h6><i class="fas fa-user-check"></i> Body Receiver Information</h6>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <label>Receiver Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtReceiverName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <label>Relation <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtRelation" runat="server" CssClass="form-control" placeholder="Son / Daughter / Spouse / etc."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <label>Receiver CNIC</label>
                                    <asp:TextBox ID="txtReceiverCNIC" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <label>Receiver Contact</label>
                                    <asp:TextBox ID="txtReceiverContact" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="d-flex gap-3 mt-3">
                            <asp:Button ID="btnSaveCertificate" runat="server" Text="💾 Save Certificate" CssClass="btn btn-success px-5" OnClick="btnSaveCertificate_Click" />
                            <asp:Button ID="btnCancelForm" runat="server" Text="Cancel" CssClass="btn btn-secondary px-4" OnClick="btnCancelForm_Click" />
                            <asp:Button ID="btnPrintPreview" runat="server" Text="🖨️ Print Preview" CssClass="btn btn-info px-4" OnClick="btnPrintPreview_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Certificates Grid -->
            <div class="card shadow-sm">
                <div class="card-header bg-dark text-white">
                    <h5 class="mb-0"><i class="fas fa-list"></i> Issued Death Certificates</h5>
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvCertificates" runat="server" 
                        CssClass="table table-bordered table-hover mb-0"
                        AutoGenerateColumns="false"
                        DataKeyNames="CertificateID,MRNo"
                        OnRowCommand="gvCertificates_RowCommand"
                        OnRowDataBound="gvCertificates_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No" />
                            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                            <asp:BoundField DataField="MRNo" HeaderText="MR No" />
                            <asp:BoundField DataField="DateOfDeath" HeaderText="Date of Death" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="CauseOfDeath" HeaderText="Cause of Death" />
                            <asp:BoundField DataField="HandOverName" HeaderText="Received By" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewCert" CommandArgument='<%# Eval("CertificateID") %>' 
                                        CssClass="btn btn-sm btn-info me-1" ToolTip="View Certificate">
                                        <i class="fas fa-eye"></i> View
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrint" runat="server" CommandName="PrintCert" CommandArgument='<%# Eval("CertificateID") %>'
                                        CssClass="btn btn-sm btn-secondary me-1" ToolTip="Print">
                                        <i class="fas fa-print"></i> Print
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteCert" CommandArgument='<%# Eval("CertificateID") %>'
                                        CssClass="btn btn-sm btn-danger" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this certificate?');">
                                        <i class="fas fa-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center p-5">
                                <i class="fas fa-file-alt fa-3x text-muted mb-3"></i>
                                <h5>No Death Certificates Found</h5>
                                <p>Search for a patient and create a new certificate</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

            <!-- Certificate Print Modal -->
            <div id="certificateModal" class="cert-modal" style="display: none;">
                <div class="cert-modal-content">
                    <div class="certificate-print-area" id="certificatePrintArea">
                    </div>
                    <div class="p-3 bg-light border-top d-flex justify-content-end gap-2 no-print">
                        <button type="button" class="btn btn-secondary" onclick="closeModal()"><i class="fas fa-times"></i> Close</button>
                        <button type="button" class="btn btn-primary" onclick="printCertificate()"><i class="fas fa-print"></i> Print Certificate</button>
                        <button type="button" class="btn btn-success" onclick="downloadPDF()"><i class="fas fa-download"></i> Download PDF</button>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hdnCertData" runat="server" />
            <asp:HiddenField ID="hdnCertificateID" runat="server" />

        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <script>
        function showCertificateModal() {
            document.getElementById('certificateModal').style.display = 'flex';
        }

        function closeModal() {
            document.getElementById('certificateModal').style.display = 'none';
        }

        function printCertificate() {
            window.print();
        }

        function downloadPDF() {
            var element = document.getElementById('certificatePrintArea');
            var opt = {
                margin: [0.5, 0.5, 0.5, 0.5],
                filename: 'DeathCertificate.pdf',
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: { scale: 2 },
                jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
            };
            html2pdf().set(opt).from(element).save();
        }

        window.onclick = function (event) {
            var modal = document.getElementById('certificateModal');
            if (event.target === modal) {
                closeModal();
            }
        }
    </script>
</body>
</html>