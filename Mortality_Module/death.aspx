<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="death.aspx.cs" Inherits="welfareSystem.Mortality_Module.death" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=yes">
    <title>Hashim Medical City - Death Certificate Management</title>
    <!-- Google Fonts & Font Awesome for modern icons -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:opsz,wght@14..32,300;14..32,400;14..32,500;14..32,600;14..32,700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <!-- DataTables CSS for advanced search grid -->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.4.1/css/responsive.dataTables.min.css" />
    <!-- SweetAlert2 for modern alerts -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <!-- jQuery & DataTables JS -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/responsive/2.4.1/js/dataTables.responsive.min.js"></script>
    <!-- html2pdf / print friendly library for clean printing -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js" integrity="sha512-GsLlZN/3F2ErC5ifS5QtgpiJtWd43JWSuIgh7mbzZ8zBps+dvLusV+eNQATqgA/HdeKFVgA5v3S/cIrLF7QnIg==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Inter', sans-serif;
            background: #eef2f9;
            padding: 30px 20px;
            color: #1a2634;
        }

        /* main container */
        .dashboard-container {
            max-width: 1400px;
            margin: 0 auto;
        }

        /* header card */
        .header-card {
            background: white;
            border-radius: 28px;
            box-shadow: 0 12px 30px rgba(0,0,0,0.05);
            padding: 20px 28px;
            margin-bottom: 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            border-left: 8px solid #1e6f5c;
        }
        .hospital-title h1 {
            font-size: 1.8rem;
            font-weight: 700;
            color: #1e6f5c;
            letter-spacing: -0.3px;
        }
        .hospital-title p {
            color: #5e6f8d;
            font-weight: 500;
            margin-top: 4px;
        }
        .badge-modern {
            background: #f0f4fa;
            padding: 10px 18px;
            border-radius: 60px;
            font-weight: 600;
            font-size: 0.9rem;
            color: #1e466e;
        }

        /* search panel */
        .search-panel {
            background: white;
            border-radius: 24px;
            padding: 18px 24px;
            margin-bottom: 28px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.03);
            display: flex;
            gap: 16px;
            flex-wrap: wrap;
            align-items: flex-end;
            border: 1px solid #e2e8f0;
        }
        .search-group {
            flex: 2;
            min-width: 240px;
        }
        .search-group label {
            font-size: 0.75rem;
            text-transform: uppercase;
            font-weight: 700;
            letter-spacing: 0.5px;
            color: #5b6e8c;
            margin-bottom: 6px;
            display: block;
        }
        .search-group input {
            width: 100%;
            padding: 12px 18px;
            border: 1.5px solid #e2edf2;
            border-radius: 18px;
            font-size: 0.9rem;
            transition: 0.2s;
            font-family: 'Inter', monospace;
        }
        .search-group input:focus {
            border-color: #1e6f5c;
            outline: none;
            box-shadow: 0 0 0 3px rgba(30,111,92,0.1);
        }
        .btn-modern {
            background: #1e6f5c;
            color: white;
            border: none;
            padding: 0 24px;
            border-radius: 40px;
            font-weight: 600;
            font-size: 0.9rem;
            height: 48px;
            cursor: pointer;
            transition: all 0.2s ease;
            display: inline-flex;
            align-items: center;
            gap: 10px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.05);
        }
        .btn-modern:hover {
            background: #0f5a4a;
            transform: translateY(-2px);
        }
        .btn-outline {
            background: transparent;
            border: 1.5px solid #cbdde6;
            color: #2c3e4e;
        }
        .btn-outline:hover {
            background: #f0f6fa;
            transform: none;
        }
        .btn-print {
            background: #2c3e66;
        }
        .btn-print:hover {
            background: #1f2c48;
        }

        /* certificate card grid */
        .certificate-grid {
            background: white;
            border-radius: 28px;
            padding: 20px;
            box-shadow: 0 18px 35px rgba(0,0,0,0.05);
        }
        table.dataTable {
            border-collapse: separate;
            border-spacing: 0;
            width: 100%;
            font-family: 'Inter', sans-serif;
        }
        table.dataTable thead th {
            background: #f8fafd;
            color: #1f3a4b;
            font-weight: 600;
            font-size: 0.85rem;
            padding: 16px 12px;
            border-bottom: 2px solid #e0edf2;
        }
        table.dataTable tbody td {
            padding: 14px 12px;
            font-size: 0.85rem;
            color: #203947;
            vertical-align: middle;
        }
        .action-btn {
            background: none;
            border: none;
            color: #1e6f5c;
            font-size: 1.2rem;
            cursor: pointer;
            margin: 0 6px;
            transition: 0.1s;
        }
        .action-btn:hover {
            color: #0b4237;
            transform: scale(1.1);
        }

        /* modern certificate print preview modal */
        .modal-cert {
            display: none;
            position: fixed;
            top: 0; left: 0;
            width: 100%; height: 100%;
            background: rgba(0,0,0,0.7);
            backdrop-filter: blur(4px);
            z-index: 2000;
            justify-content: center;
            align-items: center;
        }
        .modal-content-cert {
            background: #ffffff;
            width: 780px;
            max-width: 92%;
            border-radius: 32px;
            box-shadow: 0 30px 40px rgba(0,0,0,0.2);
            overflow: hidden;
            animation: fadeInUp 0.25s ease;
        }
        @keyframes fadeInUp {
            from { opacity: 0; transform: translateY(30px);}
            to { opacity: 1; transform: translateY(0);}
        }
        .certificate-preview {
            padding: 32px 30px;
            background: white;
            font-family: 'Inter', 'Segoe UI', sans-serif;
        }
        .cert-header {
            text-align: center;
            margin-bottom: 20px;
            border-bottom: 2px dashed #cbdde6;
            padding-bottom: 16px;
        }
        .cert-header h2 {
            color: #1a5a4c;
            font-weight: 800;
            font-size: 1.7rem;
        }
        .cert-header p {
            color: #64748b;
            font-weight: 500;
        }
        .cert-detail-row {
            display: flex;
            justify-content: space-between;
            margin: 12px 0;
            border-bottom: 1px solid #ecf3f8;
            padding: 8px 0;
        }
        .cert-label {
            font-weight: 700;
            color: #1e466e;
            width: 35%;
        }
        .cert-value {
            width: 65%;
            color: #2d3e50;
        }
        .sign-area {
            margin-top: 30px;
            display: flex;
            justify-content: space-between;
            padding-top: 20px;
        }
        .modal-footer-btns {
            display: flex;
            justify-content: flex-end;
            gap: 12px;
            padding: 18px 24px;
            background: #f9fbfd;
            border-top: 1px solid #e2edf2;
        }
        .btn-sm {
            padding: 8px 18px;
            border-radius: 40px;
            font-weight: 500;
            border: none;
            cursor: pointer;
        }
        .btn-primary-sm {
            background: #1e6f5c;
            color: white;
        }
        .btn-secondary-sm {
            background: #e2e8f0;
            color: #2c3e50;
        }
        .print-only-style {
            display: none;
        }
        @media print {
            body * {
                visibility: hidden;
            }
            .certificate-preview, .certificate-preview * {
                visibility: visible;
            }
            .certificate-preview {
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                margin: 0;
                padding: 20px;
            }
            .modal-footer-btns, .modal-content-cert > :not(.certificate-preview) {
                display: none;
            }
            .modal-content-cert {
                box-shadow: none;
                background: white;
            }
        }
    </style>
</head>
<body>
<div class="dashboard-container">
    <div class="header-card">
        <div class="hospital-title">
            <h1><i class="fas fa-hospital-user"></i> HASHIM MEDICAL CITY</h1>
            <p>Digital Death Certificate Management | Hyderabad</p>
        </div>
        <div class="badge-modern">
            <i class="fas fa-calendar-alt"></i> Mortality Registry
        </div>
    </div>

    <div class="search-panel">
        <div class="search-group">
            <label><i class="fas fa-search"></i> Search (MR No. / Patient Name / Admission No.)</label>
            <input type="text" id="globalSearchBox" placeholder="Type MRNo, Name or AdmNo ..." autocomplete="off">
        </div>
        <div>
            <button id="searchBtn" class="btn-modern"><i class="fas fa-filter"></i> Search</button>
            <button id="resetSearchBtn" class="btn-modern btn-outline"><i class="fas fa-sync-alt"></i> Reset</button>
            <button id="createNewCertBtn" class="btn-modern" style="background:#3c6e71;"><i class="fas fa-plus-circle"></i> New Certificate</button>
        </div>
    </div>

    <div class="certificate-grid">
        <table id="deathCertTable" class="display responsive" width="100%">
            <thead>
                <tr>
                    <th>MR No.</th><th>Patient Name</th><th>Diagnosis</th><th>Death Date</th><th>Cause of Death</th><th>HandOver</th><th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <!-- dynamic data from backend -->
            </tbody>
        </table>
    </div>
</div>

<!-- Modal for preview/print certificate -->
<div id="certificateModal" class="modal-cert">
    <div class="modal-content-cert">
        <div class="certificate-preview" id="certificateContent">
            <!-- dynamic certificate loaded here -->
        </div>
        <div class="modal-footer-btns">
            <button class="btn-sm btn-secondary-sm" id="closeModalBtn"><i class="fas fa-times"></i> Close</button>
            <button class="btn-sm btn-primary-sm" id="printCertificateBtn"><i class="fas fa-print"></i> Print / PDF</button>
            <button class="btn-sm btn-primary-sm" id="downloadPdfBtn"><i class="fas fa-download"></i> Download PDF</button>
        </div>
    </div>
</div>

<script>
    // Global dataTable instance
    let certDataTable;
    // Use a proxy to handle backend calls: We'll implement via fetch to server-side generic handler (WebMethod)
    // Since you require .NET Web Forms, I will simulate local data via AJAX calls to .aspx endpoint.
    // For actual integration, you will map these endpoints to PageMethods or generic handlers.
    // For demo completeness, I'll implement client/server simulation with actual web methods placeholders.
    // The query merges IPD_DeathCertificate, EMR_Patients, Gen_Consultants and also IPD_Admission info.
    // Backend must provide JSON from combined SQL.
    
    // We'll create a function loadData that fetches from GetDeathCertificates.asmx/GetList or PageMethod.
    // Let's design endpoints (you can create a WebMethod in your .aspx.cs)
    
    // For clarity: We'll assume server side method: "FetchDeathCertificateList" returns JSON.
    // And method: "GenerateDeathCertificate" saves into MortalityDB table and returns merged details.
    
    // Because this is a full interactive code, I will write JavaScript to call server methods defined in your code-behind.
    // I will also include saving logic for NEW certificate: uses MortalityDB connection to insert into a new table.
    
    // Table schema suggestion for MortalityDB: DeathCertificateStore (CertID, MRNo, AdmNo, PatientName, Age, Gender, CNIC, Diagnosis, DateOfDeath, TimeOfDeath, CauseOfDeath, HandOverName, HandOverRelation, HandOverCNIC, HandOverCell, ConsultantName, CertificateDate, CompCode, etc)
    
    // Search functionality will call same API with search param.
    
    // Implementation starts:
    function loadDeathCertificates(searchTerm = '') {
        // Call server side WebMethod: "DeathCertificateHandler.aspx/GetCertificates"
        $.ajax({
            type: "POST",
            url: window.location.pathname + "/GetCertificates", // using PageMethod in ASPX
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ search: searchTerm }),
            dataType: "json",
            success: function(response) {
                let data = response.d || response;
                if (certDataTable) {
                    certDataTable.clear().rows.add(data).draw();
                } else {
                    initializeDataTable(data);
                }
            },
            error: function(xhr) {
                console.error(xhr);
                Swal.fire("Error", "Unable to fetch certificate data. Ensure backend methods exist.", "error");
                // Fallback empty
                if(certDataTable) certDataTable.clear().draw();
            }
        });
    }
    
    function initializeDataTable(dataSet) {
        certDataTable = $('#deathCertTable').DataTable({
            data: dataSet,
            responsive: true,
            columns: [
                { data: "MRNo" },
                { data: "PatientFullName" },
                { data: "Diagnosis" },
                { data: "DateOfDeath", render: function(data) { return data ? formatDate(data) : '-'; } },
                { data: "CauseOfDeath" },
                { data: "HandOverName" },
                { 
                    data: null,
                    render: function(row) {
                        return `<button class="action-btn preview-cert" data-mrno="${row.MRNo}" data-admno="${row.AdmNo || ''}" title="View Certificate"><i class="fas fa-file-certificate"></i> View</button>
                                <button class="action-btn edit-cert" data-mrno="${row.MRNo}" title="Generate/Edit"><i class="fas fa-edit"></i></button>`;
                    }
                }
            ],
            order: [[3, 'desc']],
            language: { search: "Quick Filter:", searchPlaceholder: "Filter records..." }
        });
        
        $('#deathCertTable tbody').on('click', '.preview-cert', function() {
            let rowData = certDataTable.row($(this).closest('tr')).data();
            showCertificatePreview(rowData);
        });
        
        $('#deathCertTable tbody').on('click', '.edit-cert', function() {
            let rowData = certDataTable.row($(this).closest('tr')).data();
            openCreateOrEdit(rowData);
        });
    }
    
    function formatDate(dateStr) {
        if(!dateStr) return '';
        let d = new Date(dateStr);
        return d.toLocaleDateString('en-PK');
    }
    
    function showCertificatePreview(certData) {
        // Build modern certificate layout using merged data from query
        let html = `
            <div class="cert-header">
                <h2>⚕️ HASHIM MEDICAL CITY</h2>
                <p>The Hashim Medical City Hospital, By-Pass Hyderabad - Death Certificate</p>
                <hr style="margin:10px 0">
                <strong>Certificate of Death</strong>
            </div>
            <div class="cert-detail-row"><span class="cert-label">M.R. No:</span><span class="cert-value">${certData.MRNo || '______'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Patient Full Name:</span><span class="cert-value">${certData.PatientFullName || 'N/A'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Age / Gender:</span><span class="cert-value">${certData.Age || '—'} yrs / ${certData.Gender || '—'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">CNIC No:</span><span class="cert-value">${certData.CNIC || '—'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Diagnosis:</span><span class="cert-value">${certData.Diagnosis || '—'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Date of Death:</span><span class="cert-value">${certData.DateOfDeath || '—'} at ${certData.TimeOfDeath || '—'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Cause of Death:</span><span class="cert-value">${certData.CauseOfDeath || '—'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Attending Consultant:</span><span class="cert-value">${certData.ConsultantName || 'Medical Officer'}</span></div>
            <div class="cert-detail-row"><span class="cert-label">Body Received By:</span><span class="cert-value">${certData.HandOverName || ''} (${certData.HandOverRelation || ''})</span></div>
            <div class="cert-detail-row"><span class="cert-label">CNIC / Contact:</span><span class="cert-value">${certData.HandOverCNIC || ''} | ${certData.HandOverCellNo || ''}</span></div>
            <div class="sign-area">
                <div><strong>Medical Superintendent</strong><br>Hashim Medical City</div>
                <div><strong>Medical Officer</strong><br>${certData.DeathConfirmedBy || 'Hospital Authority'}</div>
            </div>
            <div style="font-size: 11px; text-align:center; margin-top:30px; color:#8ba0bc;">Issued on: ${new Date().toLocaleDateString()} - Computer Generated Certificate</div>
        `;
        $('#certificateContent').html(html);
        $('#certificateModal').css('display', 'flex');
        // attach print & pdf functions
        $('#printCertificateBtn').off('click').on('click', function() {
            window.print();
        });
        $('#downloadPdfBtn').off('click').on('click', function() {
            const element = document.getElementById('certificateContent');
            html2pdf().from(element).set({ margin: 0.5, filename: `DeathCert_${certData.MRNo}.pdf`, html2canvas: { scale: 2 }, jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' } }).save();
        });
    }
    
    // For New Certificate: fetch admission records (IPD_Admission) that are not billed yet or custom logic
    function openCreateOrEdit(existingData) {
        // if existingData contains MRNo, we redirect to certificate generation (open modal to fill missing fields and save)
        // For demo simplicity, we'll show a sweet alert input form to capture required death info and then save to MortalityDB
        Swal.fire({
            title: existingData ? `Generate Death Certificate for ${existingData.PatientFullName}` : 'New Death Certificate',
            html: `
                <input id="swal-mrno" class="swal2-input" placeholder="MR Number" value="${existingData?.MRNo || ''}" ${existingData ? 'readonly' : ''}>
                <input id="swal-admno" class="swal2-input" placeholder="Admission No" value="${existingData?.AdmNo || ''}">
                <input id="swal-diagnosis" class="swal2-input" placeholder="Diagnosis" value="${existingData?.Diagnosis || ''}">
                <input id="swal-dod" type="date" class="swal2-input" placeholder="Date of Death">
                <input id="swal-tod" type="time" class="swal2-input" placeholder="Time of Death">
                <input id="swal-cause" class="swal2-input" placeholder="Cause of Death" value="Cardiac Arrest">
                <input id="swal-handoverName" class="swal2-input" placeholder="Receiver Name">
                <input id="swal-relation" class="swal2-input" placeholder="Relation">
                <input id="swal-handoverCNIC" class="swal2-input" placeholder="Receiver CNIC">
                <input id="swal-cell" class="swal2-input" placeholder="Cell No">
            `,
            focusConfirm: false,
            preConfirm: () => {
                return {
                    mrNo: document.getElementById('swal-mrno').value,
                    admNo: document.getElementById('swal-admno').value,
                    diagnosis: document.getElementById('swal-diagnosis').value,
                    dateOfDeath: document.getElementById('swal-dod').value,
                    timeOfDeath: document.getElementById('swal-tod').value,
                    causeOfDeath: document.getElementById('swal-cause').value,
                    handOverName: document.getElementById('swal-handoverName').value,
                    relation: document.getElementById('swal-relation').value,
                    handOverCNIC: document.getElementById('swal-handoverCNIC').value,
                    cellNo: document.getElementById('swal-cell').value
                };
            },
            showCancelButton: true,
            confirmButtonText: 'Save Certificate'
        }).then((result) => {
            if (result.isConfirmed) {
                let payload = result.value;
                if(!payload.mrNo) { Swal.fire('Error','MR No required','error'); return; }
                // call server SaveDeathCertificate using MortalityDB connection
                $.ajax({
                    type: "POST",
                    url: window.location.pathname + "/SaveCertificate",
                    contentType: "application/json",
                    data: JSON.stringify(payload),
                    success: function(resp) {
                        let data = resp.d || resp;
                        Swal.fire('Success', 'Death certificate saved successfully', 'success').then(()=> {
                            loadDeathCertificates(); // refresh grid
                        });
                    },
                    error: function() {
                        Swal.fire('Failed','Could not save certificate','error');
                    }
                });
            }
        });
    }
    
    // search and reset
    $('#searchBtn').click(() => {
        let term = $('#globalSearchBox').val();
        loadDeathCertificates(term);
    });
    $('#resetSearchBtn').click(() => {
        $('#globalSearchBox').val('');
        loadDeathCertificates('');
    });
    $('#createNewCertBtn').click(() => {
        openCreateOrEdit(null);
    });
    $('#closeModalBtn').click(() => {
        $('#certificateModal').hide();
    });
    $(window).click(function(e) {
        if ($(e.target).is('#certificateModal')) $('#certificateModal').hide();
    });
    
    // initial load
    loadDeathCertificates('');
</script>

<!-- Server-side (C#) Integration Note: In your .aspx.cs page, add WebMethods corresponding to GetCertificates & SaveCertificate 
     Using the three connections: HospitalDBConnection, welfare, MortalityDB.
     Implement the merged SQL query from IPD_DeathCertificate, EMR_Patients, Gen_Consultants and also bring relevant fields from IPD_Admission.
     SaveCertificate method will insert into MortalityDB custom table.
     
     Below is a skeleton example for page methods: 
     
     [WebMethod]
     public static List<object> GetCertificates(string search) {
         using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HospitalDBConnection"].ConnectionString)) {
             string sql = @"SELECT a.MRNo, a.DeathCertificateNo, a.DateOfDeath, a.TimeOfDeath, a.CauseOfDeath, a.Diagnosis, a.HandOverName, a.HandOverRelation, a.HandOverCNIC, a.HandOverCellNo, 
                           c.PatientTitle+' '+c.PatientName+' '+c.FatherTitle+' '+c.FatherName as PatientFullName, 
                           DATEDIFF(YEAR, c.DoB, GETDATE()) as Age, c.Gender, c.CNIC, 
                           ISNULL(d.CityName,'') as CityName, b.ConsultantName, 
                           adm.AdmNo
                    FROM IPD_DeathCertificate a
                    INNER JOIN EMR_Patients c ON a.MRNo=c.MRNo
                    LEFT JOIN Gen_Consultants b ON a.ConsultantCode=b.ConsultantCode
                    LEFT JOIN IPD_Admission adm ON a.MRNo=adm.MRNo
                    LEFT JOIN Global_Cities d ON c.City=d.CityCode
                    WHERE a.DelFlag=0 AND (a.MRNo LIKE @search OR c.PatientName LIKE @search OR adm.AdmNo LIKE @search)";
             // add parameter logic
             // return json list
         }
     }
     
     [WebMethod]
     public static string SaveCertificate(object data) { // Use MortalityDB connection to insert into DeathCertificateStore table }
     
     Please create table in MortalityDB: CREATE TABLE DeathCertificateStore (Id INT IDENTITY, MRNo VARCHAR(50), AdmNo VARCHAR(50), PatientFullName NVARCHAR(200), Diagnosis NVARCHAR(500), DateOfDeath DATE, TimeOfDeath VARCHAR(10), CauseOfDeath NVARCHAR(200), HandOverName NVARCHAR(150), HandOverRelation NVARCHAR(100), HandOverCNIC VARCHAR(30), HandOverCellNo VARCHAR(30), ConsultantName NVARCHAR(100), CreatedAt DATETIME DEFAULT GETDATE())
     
     I have provided full client solution; merge to your .aspx and add server methods accordingly.
-->
</body>
</html>