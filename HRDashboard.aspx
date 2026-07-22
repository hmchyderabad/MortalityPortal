<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HRDashboard.aspx.cs" Inherits="welfareSystem.HRDashboard" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HR Dashboard</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        /* Reset & Base */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        body {
            background: #f0f2f5;
            padding: 20px;
        }

        .dashboard-container {
            max-width: 1600px;
            margin: 0 auto;
        }

        /* Header */
        .header {
            background: linear-gradient(135deg, #1a2a6c, #2d4373);
            color: white;
            padding: 25px 35px;
            border-radius: 15px;
            margin-bottom: 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            box-shadow: 0 4px 20px rgba(26, 42, 108, 0.3);
        }

        .header h1 {
            font-size: 28px;
            font-weight: 600;
        }

        .header h1 i {
            margin-right: 15px;
        }

        .header .date-time {
            font-size: 16px;
            opacity: 0.9;
            background: rgba(255,255,255,0.1);
            padding: 8px 18px;
            border-radius: 8px;
        }

        /* Stats Cards */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }

        .stat-card {
            background: white;
            padding: 22px;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.08);
            transition: transform 0.2s, box-shadow 0.2s;
            border-left: 5px solid #1a2a6c;
        }

        .stat-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 6px 20px rgba(0,0,0,0.12);
        }

        .stat-card .stat-icon {
            font-size: 28px;
            color: #1a2a6c;
            margin-bottom: 8px;
        }

        .stat-card .stat-number {
            font-size: 32px;
            font-weight: 700;
            color: #1a2a6c;
        }

        .stat-card .stat-label {
            font-size: 14px;
            color: #6b7280;
            margin-top: 4px;
        }

        .stat-card.green { border-left-color: #10b981; }
        .stat-card.green .stat-icon { color: #10b981; }
        .stat-card.green .stat-number { color: #10b981; }

        .stat-card.orange { border-left-color: #f59e0b; }
        .stat-card.orange .stat-icon { color: #f59e0b; }
        .stat-card.orange .stat-number { color: #f59e0b; }

        .stat-card.purple { border-left-color: #8b5cf6; }
        .stat-card.purple .stat-icon { color: #8b5cf6; }
        .stat-card.purple .stat-number { color: #8b5cf6; }

        .stat-card.red { border-left-color: #ef4444; }
        .stat-card.red .stat-icon { color: #ef4444; }
        .stat-card.red .stat-number { color: #ef4444; }

        .stat-card.cyan { border-left-color: #06b6d4; }
        .stat-card.cyan .stat-icon { color: #06b6d4; }
        .stat-card.cyan .stat-number { color: #06b6d4; }

        .stat-card.pink { border-left-color: #ec4899; }
        .stat-card.pink .stat-icon { color: #ec4899; }
        .stat-card.pink .stat-number { color: #ec4899; }

        /* Grid Layout */
        .dashboard-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 25px;
        }

        .card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.08);
            overflow: hidden;
        }

        .card-full {
            grid-column: 1 / -1;
        }

        .card-header {
            padding: 18px 25px;
            background: #f8fafc;
            border-bottom: 1px solid #e5e7eb;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .card-header h3 {
            font-size: 16px;
            font-weight: 600;
            color: #1f2937;
        }

        .card-header h3 i {
            margin-right: 10px;
            color: #1a2a6c;
        }

        .card-body {
            padding: 20px 25px;
            max-height: 350px;
            overflow-y: auto;
        }

        .card-body::-webkit-scrollbar {
            width: 5px;
        }

        .card-body::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 10px;
        }

        .card-body::-webkit-scrollbar-thumb {
            background: #c1c7cd;
            border-radius: 10px;
        }

        /* Tables */
        .table-responsive {
            overflow-x: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
        }

        table thead th {
            background: #f8fafc;
            color: #4b5563;
            font-weight: 600;
            padding: 10px 12px;
            text-align: left;
            border-bottom: 2px solid #e5e7eb;
            white-space: nowrap;
        }

        table tbody td {
            padding: 9px 12px;
            border-bottom: 1px solid #f3f4f6;
            color: #1f2937;
        }

        table tbody tr:hover {
            background: #f9fafb;
        }

        /* Employee Type Badges */
        .badge {
            display: inline-block;
            padding: 3px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 500;
        }

        .badge-confirmed { background: #dbeafe; color: #1e40af; }
        .badge-contract { background: #d1fae5; color: #065f46; }
        .badge-probation { background: #fef3c7; color: #92400e; }
        .badge-wages { background: #fce4ec; color: #b71c1c; }
        .badge-internship { background: #f3e8ff; color: #6d28d9; }

        /* Shift Labels */
        .shift-label {
            display: inline-block;
            padding: 2px 10px;
            border-radius: 12px;
            font-size: 12px;
            background: #e0e7ff;
            color: #3730a3;
        }

        /* Bar Chart Visual */
        .bar-chart {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .bar-item {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .bar-item .bar-label {
            min-width: 100px;
            font-size: 13px;
            color: #374151;
        }

        .bar-item .bar-track {
            flex: 1;
            height: 20px;
            background: #e5e7eb;
            border-radius: 10px;
            overflow: hidden;
            position: relative;
        }

        .bar-item .bar-fill {
            height: 100%;
            border-radius: 10px;
            background: linear-gradient(90deg, #1a2a6c, #3b5998);
            transition: width 1s ease;
        }

        .bar-item .bar-value {
            min-width: 50px;
            font-size: 13px;
            font-weight: 600;
            color: #1f2937;
            text-align: right;
        }

        /* Responsive */
        @media (max-width: 1024px) {
            .dashboard-grid {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 600px) {
            .header {
                flex-direction: column;
                gap: 15px;
                text-align: center;
                padding: 20px;
            }
            
            .stats-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .card-body {
                padding: 15px;
            }
        }

        /* No Data */
        .no-data {
            text-align: center;
            color: #9ca3af;
            padding: 30px 0;
            font-size: 14px;
        }

        .no-data i {
            font-size: 30px;
            display: block;
            margin-bottom: 10px;
        }

        /* Scrollable table container */
        .scroll-table {
            max-height: 300px;
            overflow-y: auto;
        }

        .scroll-table::-webkit-scrollbar {
            width: 5px;
        }

        .scroll-table::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 10px;
        }

        .scroll-table::-webkit-scrollbar-thumb {
            background: #c1c7cd;
            border-radius: 10px;
        }

        /* Machine IDs */
        .machine-id {
            font-family: 'Courier New', monospace;
            font-weight: 600;
            color: #1a2a6c;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            
            <!-- Header -->
            <div class="header">
                <h1>
                    <i class="fas fa-chart-pie"></i>HR Dashboard
                </h1>
                <div class="date-time">
                    <i class="far fa-calendar-alt"></i> 
                    <%= DateTime.Now.ToString("dddd, MMMM dd, yyyy") %> &nbsp;|&nbsp;
                    <i class="far fa-clock"></i> 
                    <%= DateTime.Now.ToString("hh:mm tt") %>
                </div>
            </div>

            <!-- Stats Grid -->
            <div class="stats-grid">
                <div class="stat-card green">
                    <div class="stat-icon"><i class="fas fa-users"></i></div>
                    <div class="stat-number"><%= TotalEmployees %></div>
                    <div class="stat-label">Total Employees</div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon"><i class="fas fa-user-tie"></i></div>
                    <div class="stat-number"><%= ConfirmedEmployees %></div>
                    <div class="stat-label">Confirmed</div>
                </div>
                <div class="stat-card orange">
                    <div class="stat-icon"><i class="fas fa-file-signature"></i></div>
                    <div class="stat-number"><%= ContractEmployees %></div>
                    <div class="stat-label">Contract</div>
                </div>
                <div class="stat-card purple">
                    <div class="stat-icon"><i class="fas fa-hourglass-half"></i></div>
                    <div class="stat-number"><%= ProbationEmployees %></div>
                    <div class="stat-label">On Probation</div>
                </div>
                <div class="stat-card red">
                    <div class="stat-icon"><i class="fas fa-hand-holding-usd"></i></div>
                    <div class="stat-number"><%= DailyWageEmployees %></div>
                    <div class="stat-label">Daily Wages</div>
                </div>
                <div class="stat-card cyan">
                    <div class="stat-icon"><i class="fas fa-graduation-cap"></i></div>
                    <div class="stat-number"><%= InternshipEmployees %></div>
                    <div class="stat-label">Internship</div>
                </div>
            </div>

            <!-- Dashboard Grid -->
            <div class="dashboard-grid">

                <!-- Employee Type by Location -->
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-user-tag"></i>Employee Type by Location</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Emp Type</th>
                                        <th>Location</th>
                                        <th style="text-align:right;">Count</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% if (EmpTypeTable != null && EmpTypeTable.Rows.Count > 0) { 
                                        foreach (System.Data.DataRow row in EmpTypeTable.Rows) { %>
                                        <tr>
                                            <td><span class="badge badge-<%= row["EmpType"].ToString().ToLower().Replace(" ", "").Replace("onprobation","probation") %>"><%= row["EmpType"] %></span></td>
                                            <td><%= row["Location"] %></td>
                                            <td style="text-align:right; font-weight:600;"><%= row["TotalEmployees"] %></td>
                                        </tr>
                                    <% } 
                                    } else { %>
                                        <tr><td colspan="3" class="no-data">No data available</td></tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Department Wise Employees -->
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-building"></i>Department Wise Employees</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Department</th>
                                        <th>Location</th>
                                        <th style="text-align:right;">Count</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% if (DeptTable != null && DeptTable.Rows.Count > 0) { 
                                        foreach (System.Data.DataRow row in DeptTable.Rows) { %>
                                        <tr>
                                            <td><strong><%= row["MainDepartment"] %></strong></td>
                                            <td><%= row["Location"] %></td>
                                            <td style="text-align:right; font-weight:600; color:#1a2a6c;"><%= row["TotalEmployees"] %></td>
                                        </tr>
                                    <% } 
                                    } else { %>
                                        <tr><td colspan="3" class="no-data">No data available</td></tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Shift Wise Data -->
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-clock"></i>Shift Wise Employees (Today)</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Shift Code</th>
                                        <th>Description</th>
                                        <th style="text-align:right;">Employees</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% if (ShiftTable != null && ShiftTable.Rows.Count > 0) { 
                                        foreach (System.Data.DataRow row in ShiftTable.Rows) { %>
                                        <tr>
                                            <td><span class="shift-label"><%= row["ShiftCode"] %></span></td>
                                            <td><%= row["Description"] %></td>
                                            <td style="text-align:right; font-weight:600; color:#10b981;"><%= row["TotalEmployee"] %></td>
                                        </tr>
                                    <% } 
                                    } else { %>
                                        <tr><td colspan="3" class="no-data">No shift data for today</td></tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Attendance by Machine -->
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-fingerprint"></i>Attendance by Machine (Today)</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Machine ID</th>
                                        <th style="text-align:right;">Employees</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% if (MachineTable != null && MachineTable.Rows.Count > 0) { 
                                        foreach (System.Data.DataRow row in MachineTable.Rows) { %>
                                        <tr>
                                            <td><span class="machine-id"><%= row["MachineId"] %></span></td>
                                            <td style="text-align:right; font-weight:600; color:#8b5cf6;"><%= row["TotalEmployee"] %></td>
                                        </tr>
                                    <% } 
                                    } else { %>
                                        <tr><td colspan="2" class="no-data">No attendance data for today</td></tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Department Wise Salaries -->
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-money-bill-wave"></i>Department Wise Salaries</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Department</th>
                                        <th>Location</th>
                                        <th style="text-align:right;">Total Gross</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% if (SalaryTable != null && SalaryTable.Rows.Count > 0) { 
                                        foreach (System.Data.DataRow row in SalaryTable.Rows) { %>
                                        <tr>
                                            <td><strong><%= row["MainDepartment"] %></strong></td>
                                            <td><%= row["Location"] %></td>
                                            <td style="text-align:right; font-weight:600; color:#059669;">
                                                <%= string.Format("{0:N0}", Convert.ToDecimal(row["TotalGross"])) %>
                                            </td>
                                        </tr>
                                    <% } 
                                    } else { %>
                                        <tr><td colspan="3" class="no-data">No salary data available</td></tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Employees on Leave -->
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-umbrella-beach"></i>Employees on Leave (Today)</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Employee Name</th>
                                        <th>From</th>
                                        <th>To</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% if (LeaveTable != null && LeaveTable.Rows.Count > 0) { 
                                        foreach (System.Data.DataRow row in LeaveTable.Rows) { %>
                                        <tr>
                                            <td><strong><%= row["EmpName"] %></strong></td>
                                            <td><%= Convert.ToDateTime(row["DateFrom"]).ToString("dd-MMM-yyyy") %></td>
                                            <td><%= Convert.ToDateTime(row["DateTo"]).ToString("dd-MMM-yyyy") %></td>
                                        </tr>
                                    <% } 
                                    } else { %>
                                        <tr><td colspan="3" class="no-data">No employees on leave today</td></tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Leaving Employees (June 2026) -->
                <div class="card card-full">
                    <div class="card-header">
                        <h3><i class="fas fa-user-minus"></i>Employees Leaving (June 2026)</h3>
                    </div>
                    <div class="card-body">
                        <div style="display:flex; align-items:center; gap:20px; flex-wrap:wrap;">
                            <div style="font-size:48px; font-weight:700; color:#ef4444;">
                                <%= LeavingCount %>
                            </div>
                            <div style="color:#6b7280; font-size:16px;">
                                <i class="fas fa-calendar-alt"></i> 
                                Employees with leaving date between June 1 - June 30, 2026
                            </div>
                        </div>
                    </div>
                </div>

            </div><!-- End dashboard-grid -->

            <!-- Footer -->
            <div style="margin-top:30px; text-align:center; color:#9ca3af; font-size:13px; padding:15px 0; border-top:1px solid #e5e7eb;">
                <i class="fas fa-database"></i> HR Dashboard &bull; Data updated: <%= DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") %>
            </div>

        </div><!-- End dashboard-container -->
    </form>
</body>
</html>