<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="PrintReviewForm.aspx.cs"
Inherits="welfareSystem.Mortality_Module.PrintReviewForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Daily Mortality Review Form</title>
  <style type="text/css">
    /* Global Styles */
    body {
      font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
      color: #1a1a1a;
      background-color: #f4f6f8;
      margin: 0;
      padding: 20px;
    }

    .form-container {
      max-width: 850px;
      margin: 0 auto;
      background-color: #ffffff;
      padding: 30px;
      border: 1px solid #dcdcdc;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
      box-sizing: border-box;
    }

    /* Header Section */
    .header-table {
      width: 100%;
      border-collapse: collapse;
      border-bottom: 2px solid #1a2b4c;
      padding-bottom: 15px;
      margin-bottom: 20px;
    }

    .hospital-logo {
      max-height: 70px;
      width: auto;
      display: block;
    }

    .hospital-title {
      margin: 0;
      font-size: 22px;
      color: #1a2b4c;
      font-weight: 700;
      letter-spacing: 0.5px;
    }

    .hospital-subtitle {
      margin: 2px 0 0 0;
      font-size: 13px;
      color: #4a5568;
      font-weight: 600;
    }

    .hospital-tagline {
      margin: 4px 0 0 0;
      font-size: 10px;
      font-style: italic;
      color: #718096;
      letter-spacing: 0.5px;
    }

    .patient-info-box {
      border: 1px solid #cbd5e0;
      background-color: #f8fafc;
      padding: 10px 12px;
      border-radius: 4px;
      font-size: 12px;
    }

    .patient-info-box header {
      font-weight: 700;
      color: #1a2b4c;
      border-bottom: 1px solid #e2e8f0;
      padding-bottom: 4px;
      margin-bottom: 6px;
      text-transform: uppercase;
      font-size: 11px;
    }

    .form-title {
      text-align: center;
      margin: 15px 0 20px 0;
      text-transform: uppercase;
      font-size: 18px;
      letter-spacing: 1px;
      color: #1a2b4c;
      font-weight: 800;
    }

    /* Form Data Grid */
    .data-table {
      width: 100%;
      border-collapse: collapse;
      margin-bottom: 15px;
    }

    .data-table td {
      padding: 6px 8px;
      font-size: 12px;
      vertical-align: top;
    }

    .data-table td.label-col {
      font-weight: 600;
      color: #2d3748;
      width: 20%;
    }

    .data-table td.value-col {
      border-bottom: 1px solid #cbd5e0;
      width: 30%;
    }

    /* Clinical Details Table */
    .section-table {
      width: 100%;
      border-collapse: collapse;
      margin: 15px 0;
    }

    .section-table td {
      border: 1px solid #cbd5e0;
      padding: 8px;
      font-size: 12px;
    }

    .section-table td.label-col {
      background-color: #f8fafc;
      font-weight: 600;
      color: #2d3748;
      width: 25%;
    }

    /* Review Grid Table */
    .review-grid {
      width: 100%;
      border-collapse: collapse;
      margin-top: 10px;
    }

    .review-grid th,
    .review-grid td {
      border: 1px solid #cbd5e0;
      padding: 6px 8px;
      font-size: 11px;
    }

    .review-grid th {
      background-color: #edf2f7;
      color: #2d3748;
      font-weight: 700;
      text-transform: uppercase;
    }

    .review-grid td.question-cell {
      text-align: left;
      color: #2d3748;
    }

    .review-grid td.choice-cell {
      text-align: center;
      font-weight: bold;
    }

    /* Footer */
    .footer-table {
      width: 100%;
      margin-top: 25px;
      border-top: 1px solid #cbd5e0;
      padding-top: 8px;
      font-size: 10px;
      color: #718096;
      text-align: center;
    }

    /* Print Specific Formatting */
    @media print {
      @page {
        size: A4 portrait;
        margin: 12mm;
      }

      body {
        background-color: #fff;
        padding: 0;
      }

      .form-container {
        border: none;
        box-shadow: none;
        padding: 0;
        width: 100%;
        max-width: 100%;
      }

      .review-grid, .section-table, .data-table {
        page-break-inside: avoid;
      }

      /* Repeat table headers on each page */
      thead {
        display: table-header-group;
      }

      tfoot {
        display: table-footer-group;
      }

      /* Ensure header table repeats on print */
      .header-table {
        page-break-after: avoid;
      }

      .form-title {
        page-break-after: avoid;
      }

      /* Main table structure for print */
      .main-table thead {
        display: table-header-group;
      }

      .main-table tbody {
        display: table-row-group;
      }
    }
  </style>
</head>
<body>
  <form id="form1" runat="server">
    <div class="form-container">
      
      <!-- Main Table with Repeating Header -->
      <table class="main-table" style="width: 100%; border-collapse: collapse;">
        <!-- Header Section (thead for print repetition) -->
        <thead>
          <tr>
            <td colspan="2" style="padding: 0; border: none;">
              <table class="header-table">
                <tr>
                  <!-- Logo Cell (Left) -->
                  <td style="width: 15%; vertical-align: middle;">
                    <asp:Image ID="imgHospitalLogo" runat="server" ImageUrl="~/assets/hmc_logo.png" AlternateText="Logo" CssClass="hospital-logo" />
                  </td>
                  <!-- Title Cell (Center Left) -->
                  <td style="width: 50%; vertical-align: middle; padding-left: 10px;">
                    <h1 class="hospital-title">Hashim Medical City</h1>
                    <h3 class="hospital-subtitle">Hospital, Hyderabad</h3>
                    <p class="hospital-tagline">TRANSFORMING HEALTH, TRANSFORMING LIVES</p>
                  </td>
                  <!-- Patient Info Box Cell (Right) -->
                  <td style="width: 35%; vertical-align: top;">
                    <div class="patient-info-box">
                      <header>Patient Identification</header>
                      <div>
                        <strong>Name:</strong> 
                        <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                      </div>
                      <div style="margin-top: 4px;">
                        <strong>MR #:</strong> 
                        <asp:Label ID="lblMRNo" runat="server"></asp:Label>
                      </div>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </thead>
        
        <!-- Content Section -->
        <tbody>
          <tr>
            <td colspan="2" style="padding: 0; border: none;">
              <h2 class="form-title">Daily Mortality Review Form</h2>
            </td>
          </tr>
          <tr>
            <td colspan="2" style="padding: 0; border: none;">
        <!-- Form Info Table -->
        <table class="data-table">
          <tr>
            <td class="label-col">Serial No:</td>
            <td class="value-col">
              <asp:Label ID="lblSerialNo" runat="server" Font-Bold="true"></asp:Label>
            </td>
            <td class="label-col" style="padding-left: 15px;">Date of Review:</td>
            <td class="value-col">
              <asp:Label ID="lblDateOfReview" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="label-col">Date of Admission:</td>
            <td class="value-col">
              <asp:Label ID="lblDateOfAdmission" runat="server"></asp:Label>
            </td>
            <td class="label-col" style="padding-left: 15px;">Admitting Ward:</td>
            <td class="value-col">
              <asp:Label ID="lblAdmittingWard" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="label-col">Attending Physician:</td>
            <td class="value-col">
              <asp:Label ID="lblAttendingPhysician" runat="server"></asp:Label>
            </td>
            <td class="label-col" style="padding-left: 15px;">Length of Stay:</td>
            <td class="value-col">
              <asp:Label ID="lblLengthOfStay" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="label-col">Shifted Critical Ward:</td>
            <td class="value-col">
              <asp:Label ID="lblShiftedIntoCriticalWard" runat="server"></asp:Label>
              <asp:Label ID="lblDateShiftedCritical" runat="server"></asp:Label>
            </td>
            <td class="label-col" style="padding-left: 15px;">Date & Time of Death:</td>
            <td class="value-col">
              <asp:Label ID="lblDateTimeOfDeath" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="label-col">Death Cert #:</td>
            <td class="value-col" colspan="3">
              <asp:Label ID="lblDeathCertSerial" runat="server"></asp:Label>
            </td>
          </tr>
        </table>

        <!-- Clinical Details Section -->
        <table class="section-table">
          <tr>
            <td class="label-col">Primary Diagnosis</td>
            <td><asp:Label ID="lblPrimaryDiagnosis" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="label-col">Co-morbidities</td>
            <td><asp:Label ID="lblCoMorbidities" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="label-col">Brief Clinical Summary & Event Note</td>
            <td><asp:Label ID="lblClinicalSummary" runat="server"></asp:Label></td>
          </tr>
        </table>

        <!-- Review Criteria Section -->
        <div style="margin-top: 15px;">
          <h3 style="margin: 0 0 4px 0; font-size: 14px; color: #1a2b4c;">Review Criteria</h3>
          <p style="font-style: italic; margin: 0 0 8px 0; font-size: 11px; color: #718096;">
            Select the most appropriate assessment for each parameter below:
          </p>

          <table class="review-grid">
            <thead>
              <tr>
                <th style="width: 52%; text-align: left;">Category</th>
                <th style="width: 8%;">Y</th>
                <th style="width: 8%;">N</th>
                <th style="width: 8%;">N/A</th>
                <th style="width: 24%; text-align: left;">Comments</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td class="question-cell">Was the death expected based on the clinical path?</td>
                <td class="choice-cell"><asp:Label ID="lblExpY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblExpN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblExpNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblExpRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was complete & proper code decision made?</td>
                <td class="choice-cell"><asp:Label ID="lblCodeY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblCodeN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblCodeNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblCodeRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was there a delay in diagnosis?</td>
                <td class="choice-cell"><asp:Label ID="lblDiagY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblDiagN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblDiagNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblDiagRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was there a delay in treatment?</td>
                <td class="choice-cell"><asp:Label ID="lblTreatY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblTreatN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblTreatNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblTreatRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Were clinical guidelines / protocols followed?</td>
                <td class="choice-cell"><asp:Label ID="lblProtoY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblProtoN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblProtoNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblProtoRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was there effective communication between the care team?</td>
                <td class="choice-cell"><asp:Label ID="lblCommY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblCommN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblCommNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblCommRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was there proper clinical documentation?</td>
                <td class="choice-cell"><asp:Label ID="lblDocY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblDocN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblDocNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblDocRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Did an unplanned return to the ICU occur?</td>
                <td class="choice-cell"><asp:Label ID="lblIcuY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblIcuN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblIcuNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblIcuRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was any incident reported?</td>
                <td class="choice-cell"><asp:Label ID="lblIncY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblIncN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblIncNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblIncRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Was Sentinel Event declared?</td>
                <td class="choice-cell"><asp:Label ID="lblSentY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblSentN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblSentNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblSentRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td class="question-cell">Any HAI reported for the above patient?</td>
                <td class="choice-cell"><asp:Label ID="lblHaiY" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblHaiN" runat="server"></asp:Label></td>
                <td class="choice-cell"><asp:Label ID="lblHaiNA" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblHaiRemark" runat="server"></asp:Label></td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Footer Section -->
        <table class="footer-table">
          <tr>
            <td>
              Palm Enclave Housing Scheme, Main Bypass Road, Hyderabad Sindh &nbsp;|&nbsp; 
              Contact: 022 3418807-9 &nbsp;|&nbsp; Email: info@hmchyd.org
            </td>
          </tr>
        </table>
            </td>
          </tr>
        </tbody>
      </table>

    </div>
  </form>
</body>
</html>