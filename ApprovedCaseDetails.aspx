<%@ Page Language="C#" MasterPageFile="AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="ApprovedCaseDetails.aspx.cs"
    Inherits="welfareSystem.ApprovedCaseDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        body {
            background: #f4f7fb;
        }

        .main-card {
            background: #fff;
            border-radius: 18px;
            padding: 25px;
            margin-top: 20px;
            box-shadow: 0 4px 18px rgba(0,0,0,0.08);
        }

        .page-title {
            font-size: 30px;
            font-weight: bold;
            color: #0f172a;
            margin-bottom: 25px;
        }

        .info-box {
            background: linear-gradient(135deg, #1e3a8a, #2563eb);
            color: white;
            padding: 20px;
            border-radius: 15px;
            margin-bottom: 25px;
        }

        .info-box h4 {
            margin: 0;
            font-size: 24px;
        }

        .section-title {
            font-size: 22px;
            font-weight: 700;
            color: #1e293b;
            margin-top: 25px;
            margin-bottom: 15px;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            overflow: hidden;
            border-radius: 12px;
        }

        .table th {
            background: #0f172a;
            color: white;
            padding: 14px;
            text-align: center;
        }

        .table td {
            padding: 12px;
            text-align: center;
            border-bottom: 1px solid #e5e7eb;
        }

        .table tr:hover {
            background: #f8fafc;
        }

        .amount {
            font-weight: bold;
            color: #059669;
        }

        /* Print Styles */
        @media print {
            body {
                background: white;
                margin: 0;
                padding: 0;
            }
            
            .no-print {
                display: none !important;
            }
            
            .main-card {
                box-shadow: none;
                padding: 10px;
                margin: 0;
            }
            
            .info-box {
                background: #1e3a8a !important;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
            
            .table th {
                background: #0f172a !important;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
            
            .btn-print {
                display: none;
            }
        }

        /* Print Button Styles */
        .btn-print {
            background: linear-gradient(135deg, #059669, #10b981);
            color: white;
            border: none;
            padding: 12px 28px;
            font-size: 16px;
            font-weight: bold;
            border-radius: 10px;
            cursor: pointer;
            transition: all 0.3s ease;
            margin-bottom: 20px;
            float: right;
        }

        .btn-print:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(5,150,105,0.3);
        }

        .clearfix::after {
            content: "";
            clear: both;
            display: table;
        }
    </style>

    <div class="container-fluid">

        <div class="main-card">

            <div class="clearfix">
                <div class="page-title" style="float: left;">
                    Approved Case Details
                </div>

                <!-- Print Button -->
                <button type="button" class="btn-print no-print" onclick="window.print();">
                    🖨️ Print / Download PDF
                </button>
            </div>

            <div class="info-box">

                <h4>
                    Welfare ID:
                    <asp:Label ID="lblRequestID" runat="server" Font-Bold="true"></asp:Label>
                </h4>
                <h4>
                    Admission No:
                    <asp:Label ID="lblAdmNo" runat="server"></asp:Label>
                </h4>
               <h4>
                    MR No:
                    <asp:Label ID="lblMRNo" runat="server"></asp:Label>
                </h4>

                <h4>
                    Patient Name:
                    <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                </h4>

            </div>

            <!-- Consultants -->

            <div class="section-title">
                Consultant Details
            </div>

            <asp:GridView ID="gvConsultants"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table"
                GridLines="None">

                <Columns>

                    <asp:BoundField DataField="ConsultantName"
                        HeaderText="Consultant Name" />

                    <asp:BoundField DataField="VisitFee"
                        HeaderText="Visit Fee" />

                    <asp:BoundField DataField="ProcedureFee"
                        HeaderText="Procedure Fee" />

                    <asp:BoundField DataField="DiscountPercent"
                        HeaderText="Discount %" />

                    <asp:BoundField DataField="DiscountAmount"
                        HeaderText="Discount Amount" />

                    <asp:TemplateField HeaderText="Final Amount">
                        <ItemTemplate>

                            <span class="amount">
                                Rs. <%# Eval("FinalAmount") %>
                            </span>

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <!-- Services -->

            <div class="section-title">
                Services Details
            </div>

            <asp:GridView ID="gvServices"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table"
                GridLines="None">

                <Columns>

                    <asp:BoundField DataField="ServiceName"
                        HeaderText="Service Name" />

                    <asp:BoundField DataField="Amount"
                        HeaderText="Amount" />

                    <asp:BoundField DataField="DiscountPercent"
                        HeaderText="Discount %" />

                    <asp:BoundField DataField="DiscountAmount"
                        HeaderText="Discount Amount" />

                    <asp:TemplateField HeaderText="Final Amount">
                        <ItemTemplate>

                            <span class="amount">
                                Rs. <%# Eval("FinalAmount") %>
                            </span>

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <!-- Summary Section -->

            <div class="section-title">
                Financial Summary
            </div>

            <div class="row">

                <div class="col-md-4">
                    <div class="info-box">

                        <h4>Bill Amount</h4>

                        <h3>
                            Rs.
                            <asp:Label ID="lblReceivable"
                                runat="server"></asp:Label>
                        </h3>

                    </div>
                </div>

                <div class="col-md-4">
                    <div class="info-box">

                        <h4>Total Discount</h4>

                        <h3>
                            Rs.
                            <asp:Label ID="lblTotalDiscount"
                                runat="server"></asp:Label>
                        </h3>

                    </div>
                </div>

                <div class="col-md-4">
                    <div class="info-box">

                        <h4>Net Total</h4>

                        <h3>
                            Rs.
                            <asp:Label ID="lblGrandTotal"
                                runat="server"></asp:Label>
                        </h3>

                    </div>
                </div>

            </div>

        </div>

    </div>

</asp:Content>