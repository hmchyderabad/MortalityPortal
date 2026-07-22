<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="WelfareDashboard.aspx.cs" Inherits="welfareSystem.WelfareDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .dashboard-card {
            border-radius: 15px;
            padding: 20px;
            color: white;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            cursor: pointer;
            position: relative;
            overflow: hidden;
        }

        .dashboard-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
        }

        .dashboard-card .card-icon {
            position: absolute;
            right: 15px;
            top: 15px;
            font-size: 40px;
            opacity: 0.3;
        }

        .dashboard-card .card-number {
            font-size: 32px;
            font-weight: 700;
            margin-bottom: 5px;
        }

        .dashboard-card .card-label {
            font-size: 14px;
            opacity: 0.9;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .dashboard-card .card-trend {
            font-size: 12px;
            margin-top: 10px;
            opacity: 0.8;
        }

        .card-primary { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
        .card-success { background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%); }
        .card-warning { background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); }
        .card-danger { background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); }
        .card-info { background: linear-gradient(135deg, #a18cd1 0%, #fbc2eb 100%); }
        .card-dark { background: linear-gradient(135deg, #2c3e50 0%, #3498db 100%); }

        .chart-container {
            background: white;
            border-radius: 15px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
            margin-bottom: 20px;
            height: 100%;
        }

        .chart-container h5 {
            color: #2c3e50;
            font-weight: 600;
            margin-bottom: 15px;
        }

        .chart-wrapper {
            position: relative;
            height: 300px;
        }

        .chart-wrapper-sm {
            height: 250px;
        }

        .table-container {
            background: white;
            border-radius: 15px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
        }

        .table-container h5 {
            color: #2c3e50;
            font-weight: 600;
            margin-bottom: 15px;
        }

        .status-badge {
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
        }

        .status-badge.pending { background: #fff3cd; color: #856404; }
        .status-badge.approved { background: #d4edda; color: #155724; }
        .status-badge.rejected { background: #f8d7da; color: #721c24; }
        .status-badge.in-progress { background: #cce5ff; color: #004085; }

        .refresh-btn {
            float: right;
            margin-top: -5px;
        }

        @media (max-width: 768px) {
            .dashboard-card .card-number { font-size: 24px; }
            .chart-wrapper { height: 200px; }
            .chart-wrapper-sm { height: 180px; }
        }
    </style>

    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>
            <i class="fa fa-chart-line text-primary me-2"></i>Welfare Dashboard
        </h2>
        <div>
            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="btnRefresh_Click">
                <i class="fa fa-sync-alt"></i> Refresh
            </asp:LinkButton>
            <asp:Label ID="lblLastUpdated" runat="server" CssClass="text-muted ms-2" Font-Size="12px"></asp:Label>
        </div>
    </div>

    <hr />

    <!-- ========================================= -->
    <!-- STATISTICS CARDS -->
    <!-- ========================================= -->
    <div class="row mb-4">
        <div class="col-xl-3 col-lg-6 col-md-6 mb-3">
            <div class="dashboard-card card-primary">
                <i class="fa fa-users card-icon"></i>
                <div class="card-number"><asp:Label ID="lblTotalPatients" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Total Patients</div>
                <div class="card-trend">
                    <i class="fa fa-arrow-up"></i> 
                    <asp:Label ID="lblPatientTrend" runat="server" Text="+12% this month"></asp:Label>
                </div>
            </div>
        </div>

        <div class="col-xl-3 col-lg-6 col-md-6 mb-3">
            <div class="dashboard-card card-success">
                <i class="fa fa-hand-holding-usd card-icon"></i>
                <div class="card-number"><asp:Label ID="lblTotalDonations" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Total Donations</div>
                <div class="card-trend">
                    <i class="fa fa-arrow-up"></i> 
                    <asp:Label ID="lblDonationTrend" runat="server" Text="+8% this month"></asp:Label>
                </div>
            </div>
        </div>

        <div class="col-xl-3 col-lg-6 col-md-6 mb-3">
            <div class="dashboard-card card-warning">
                <i class="fa fa-clock card-icon"></i>
                <div class="card-number"><asp:Label ID="lblPendingRequests" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Pending Requests</div>
                <div class="card-trend">
                    <i class="fa fa-arrow-down"></i> 
                    <asp:Label ID="lblPendingTrend" runat="server" Text="-5% this month"></asp:Label>
                </div>
            </div>
        </div>

        <div class="col-xl-3 col-lg-6 col-md-6 mb-3">
            <div class="dashboard-card card-danger">
                <i class="fa fa-check-circle card-icon"></i>
                <div class="card-number"><asp:Label ID="lblApprovedCases" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Approved Cases</div>
                <div class="card-trend">
                    <i class="fa fa-arrow-up"></i> 
                    <asp:Label ID="lblApprovedTrend" runat="server" Text="+15% this month"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <!-- ========================================= -->
    <!-- CHARTS ROW 1 -->
    <!-- ========================================= -->
    <div class="row mb-4">
        <div class="col-xl-6 col-lg-6 mb-3">
            <div class="chart-container">
                <h5><i class="fa fa-chart-bar text-primary me-2"></i>Monthly Patient Cases</h5>
                <div class="chart-wrapper">
                    <canvas id="monthlyChart" runat="server"></canvas>
                </div>
            </div>
        </div>

        <div class="col-xl-6 col-lg-6 mb-3">
            <div class="chart-container">
                <h5><i class="fa fa-chart-pie text-success me-2"></i>Case Status Distribution</h5>
                <div class="chart-wrapper">
                    <canvas id="statusPieChart" runat="server"></canvas>
                </div>
            </div>
        </div>
    </div>

    <!-- ========================================= -->
    <!-- CHARTS ROW 2 -->
    <!-- ========================================= -->
    <div class="row mb-4">
        <div class="col-xl-6 col-lg-6 mb-3">
            <div class="chart-container">
                <h5><i class="fa fa-chart-area text-info me-2"></i>Donations Trend (Last 6 Months)</h5>
                <div class="chart-wrapper">
                    <canvas id="donationLineChart" runat="server"></canvas>
                </div>
            </div>
        </div>

        <div class="col-xl-6 col-lg-6 mb-3">
            <div class="chart-container">
                <h5><i class="fa fa-chart-bar text-warning me-2"></i>Top Donors</h5>
                <div class="chart-wrapper">
                    <canvas id="donorsChart" runat="server"></canvas>
                </div>
            </div>
        </div>
    </div>

    <!-- ========================================= -->
    <!-- RECENT REQUESTS TABLE -->
    <!-- ========================================= -->
    <div class="row">
        <div class="col-12">
            <div class="table-container">
                <h5>
                    <i class="fa fa-list text-primary me-2"></i>Recent Welfare Requests
                    <span class="badge bg-primary ms-2"><asp:Label ID="lblTotalRequests" runat="server" Text="0"></asp:Label></span>
                </h5>
                <div class="table-responsive">
                    <asp:GridView ID="gvRecentRequests" runat="server" 
                        CssClass="table table-hover table-striped"
                        AutoGenerateColumns="False"
                        BorderStyle="None"
                        GridLines="None"
                        EmptyDataText="No recent requests found."
                        OnRowDataBound="gvRecentRequests_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="RequestId" HeaderText="Request ID" />
                            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                            <asp:BoundField DataField="CNIC" HeaderText="CNIC" />
                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="Department" HeaderText="Department" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N0}" />
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" 
                                        CssClass='<%# "status-badge " + GetStatusClass(Eval("Status").ToString()) %>'
                                        Text='<%# Eval("Status") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <a href='<%# "Patient_sent_from_billing.aspx?id=" + Eval("RequestId") %>' class="btn btn-sm btn-outline-primary">
                                        <i class="fa fa-eye"></i> View
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <!-- ========================================= -->
    <!-- CHARTS SCRIPTS -->
    <!-- ========================================= -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2"></script>

    <script>
        // ==============================
        // MONTHLY CASES BAR CHART
        // ==============================
        function initMonthlyChart(labels, data) {
            const ctx = document.getElementById('<%= monthlyChart.ClientID %>').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Cases',
                        data: data,
                        backgroundColor: [
                            'rgba(102, 126, 234, 0.8)',
                            'rgba(118, 75, 162, 0.8)',
                            'rgba(17, 153, 142, 0.8)',
                            'rgba(56, 239, 125, 0.8)',
                            'rgba(245, 87, 108, 0.8)',
                            'rgba(79, 172, 254, 0.8)'
                        ],
                        borderColor: [
                            '#667eea', '#764ba2', '#11998e', '#38ef7d', '#f5576c', '#4facfe'
                        ],
                        borderWidth: 2,
                        borderRadius: 5
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: false },
                        datalabels: {
                            anchor: 'end',
                            align: 'end',
                            color: '#2c3e50',
                            font: { weight: 'bold' }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: { stepSize: 1 }
                        }
                    }
                },
                plugins: [ChartDataLabels]
            });
        }

        // ==============================
        // STATUS PIE CHART
        // ==============================
        function initStatusPieChart(labels, data, colors) {
            const ctx = document.getElementById('<%= statusPieChart.ClientID %>').getContext('2d');
            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: colors,
                        borderWidth: 2,
                        borderColor: '#fff'
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: { padding: 15, usePointStyle: true }
                        },
                        datalabels: {
                            formatter: (value, ctx) => {
                                let total = ctx.dataset.data.reduce((a, b) => a + b, 0);
                                return total > 0 ? Math.round((value / total) * 100) + '%' : '0%';
                            },
                            color: '#fff',
                            font: { weight: 'bold', size: 12 }
                        }
                    }
                },
                plugins: [ChartDataLabels]
            });
        }

        // ==============================
        // DONATIONS LINE CHART
        // ==============================
        function initDonationLineChart(labels, data) {
            const ctx = document.getElementById('<%= donationLineChart.ClientID %>').getContext('2d');
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Donations (PKR)',
                        data: data,
                        borderColor: '#4facfe',
                        backgroundColor: 'rgba(79, 172, 254, 0.1)',
                        fill: true,
                        tension: 0.4,
                        pointBackgroundColor: '#4facfe',
                        pointRadius: 5,
                        pointHoverRadius: 8
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: false },
                        datalabels: {
                            anchor: 'end',
                            align: 'top',
                            color: '#2c3e50',
                            font: { weight: 'bold', size: 10 },
                            formatter: (value) => {
                                if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M';
                                if (value >= 1000) return (value / 1000).toFixed(0) + 'K';
                                return value;
                            }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                callback: function(value) {
                                    if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M';
                                    if (value >= 1000) return (value / 1000).toFixed(0) + 'K';
                                    return value;
                                }
                            }
                        }
                    }
                },
                plugins: [ChartDataLabels]
            });
        }

        // ==============================
        // TOP DONORS CHART
        // ==============================
        function initTopDonorsChart(labels, data) {
            const ctx = document.getElementById('<%= donorsChart.ClientID %>').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Donation Amount (PKR)',
                        data: data,
                        backgroundColor: [
                            'rgba(255, 193, 7, 0.8)',
                            'rgba(220, 53, 69, 0.8)',
                            'rgba(40, 167, 69, 0.8)',
                            'rgba(23, 162, 184, 0.8)',
                            'rgba(102, 16, 242, 0.8)'
                        ],
                        borderColor: [
                            '#ffc107', '#dc3545', '#28a745', '#17a2b8', '#6610f2'
                        ],
                        borderWidth: 2,
                        borderRadius: 5
                    }]
                },
                options: {
                    indexAxis: 'y',
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: false },
                        datalabels: {
                            anchor: 'end',
                            align: 'end',
                            color: '#2c3e50',
                            font: { weight: 'bold', size: 10 },
                            formatter: (value) => {
                                if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M';
                                if (value >= 1000) return (value / 1000).toFixed(0) + 'K';
                                return value;
                            }
                        }
                    },
                    scales: {
                        x: {
                            beginAtZero: true,
                            ticks: {
                                callback: function(value) {
                                    if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M';
                                    if (value >= 1000) return (value / 1000).toFixed(0) + 'K';
                                    return value;
                                }
                            }
                        }
                    }
                },
                plugins: [ChartDataLabels]
            });
        }
    </script>

    <script>
        // Initialize all charts on page load
        document.addEventListener('DOMContentLoaded', function() {
            // Get data from hidden fields or ViewState
            const monthlyLabels = JSON.parse(document.getElementById('hfMonthlyLabels').value || '[]');
            const monthlyData = JSON.parse(document.getElementById('hfMonthlyData').value || '[]');
            const statusLabels = JSON.parse(document.getElementById('hfStatusLabels').value || '[]');
            const statusData = JSON.parse(document.getElementById('hfStatusData').value || '[]');
            const statusColors = JSON.parse(document.getElementById('hfStatusColors').value || '[]');
            const donationLabels = JSON.parse(document.getElementById('hfDonationLabels').value || '[]');
            const donationData = JSON.parse(document.getElementById('hfDonationData').value || '[]');
            const donorLabels = JSON.parse(document.getElementById('hfDonorLabels').value || '[]');
            const donorData = JSON.parse(document.getElementById('hfDonorData').value || '[]');

            if (monthlyLabels.length > 0) initMonthlyChart(monthlyLabels, monthlyData);
            if (statusLabels.length > 0) initStatusPieChart(statusLabels, statusData, statusColors);
            if (donationLabels.length > 0) initDonationLineChart(donationLabels, donationData);
            if (donorLabels.length > 0) initTopDonorsChart(donorLabels, donorData);
        });
    </script>

    <!-- Hidden fields to pass data to JavaScript -->
    <asp:HiddenField ID="hfMonthlyLabels" runat="server" />
    <asp:HiddenField ID="hfMonthlyData" runat="server" />
    <asp:HiddenField ID="hfStatusLabels" runat="server" />
    <asp:HiddenField ID="hfStatusData" runat="server" />
    <asp:HiddenField ID="hfStatusColors" runat="server" />
    <asp:HiddenField ID="hfDonationLabels" runat="server" />
    <asp:HiddenField ID="hfDonationData" runat="server" />
    <asp:HiddenField ID="hfDonorLabels" runat="server" />
    <asp:HiddenField ID="hfDonorData" runat="server" />

</asp:Content>