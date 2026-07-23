<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="PrintReviewForm.aspx.cs"
Inherits="welfareSystem.Mortality_Module.PrintReviewForm" %>

<style type="text/css">
  .form-container {
    font-family: Arial, sans-serif;
    max-width: 900px;
    margin: 20px auto;
    border: 2px solid #000;
    padding: 20px;
    background-color: #fff;
  }
  .header-table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 20px;
  }
  .header-table td {
    vertical-align: top;
  }
  .patient-info-box {
    border: 2px solid #000;
    padding: 10px;
  }
  .section-table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 15px;
  }
  .section-table td,
  .section-table th {
    border: 1px solid #000;
    padding: 6px 8px;
  }
  .field-row {
    margin-bottom: 12px;
    display: flex;
    align-items: center;
    justify-content: space-between;
  }
  .form-control {
    width: 100%;
    box-sizing: border-box;
    padding: 4px;
    border: none;
    background: transparent;
  }
  .review-grid {
    width: 100%;
    border-collapse: collapse;
    margin-top: 15px;
  }
  .review-grid th,
  .review-grid td {
    border: 1px solid #000;
    padding: 6px;
    text-align: center;
  }
  .review-grid td.question-cell {
    text-align: left;
  }
  .footer-table {
    width: 100%;
    margin-top: 20px;
    border-top: 2px solid #000;
    padding-top: 5px;
    font-size: 11px;
  }
  .print-header {
    display: none;
  }
  .content-wrapper {
    margin-top: 0;
  }
  @media print {
    .form-container {
      margin: 0;
      border: none;
    }
    body {
      margin: 0;
    }
    .print-header {
      display: block;
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      background-color: #fff;
      z-index: 1000;
    }
    .content-wrapper {
      margin-top: 10px;
    }
  }
</style>

<div class="form-container">
  <!-- Header Section -->
  <div class="print-header">
    <table class="header-table">
      <tr>
        <td style="width: 50%">
          <h2 style="margin: 0">Hashim Medical City</h2>
          <h4 style="margin: 0">Hospital, Hyderabad</h4>
          <p style="font-size: 10px; margin: 2px 0 0 0; font-style: italic">
            TRANSFORMING HEALTH, TRANSFORMING LIVES
          </p>
        </td>
        <td style="width: 20%; text-align: center">
          <div
            style="border: 1px dashed #000; padding: 5px; display: inline-block"
          >
            <strong>Serial No:</strong>
            <br />
            <asp:Label
              ID="lblSerialNo"
              runat="server"
              Font-Bold="true"
            ></asp:Label>
          </div>
        </td>
        <td style="width: 30%">
          <div class="patient-info-box">
            <strong style="display: block; margin-bottom: 5px"
              >Patient Identification</strong
            >
            <label>Patient Name:</label>
            <asp:Label
              ID="lblPatientName"
              runat="server"
              CssClass="form-control"
            ></asp:Label>
            <label>MR #:</label>
            <asp:Label
              ID="lblMRNo"
              runat="server"
              CssClass="form-control"
            ></asp:Label>
          </div>
        </td>
      </tr>
    </table>
  </div>

  <div class="content-wrapper">
    <h2 style="text-align: center; margin: 15px 0; text-transform: uppercase">
      Daily Mortality Review Form
    </h2>

    <!-- General Details Form Section -->
    <table
      style="width: 100%; border-collapse: separate; border-spacing: 0 8px"
    >
      <tr>
        <td style="width: 15%"><strong>Date of Review</strong></td>
        <td style="width: 35%">
          <asp:Label
            ID="lblDateOfReview"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
        <td style="width: 20%; padding-left: 15px">
          <strong>Date of Admission</strong>
        </td>
        <td style="width: 30%">
          <asp:Label
            ID="lblDateOfAdmission"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td><strong>Admitting Ward</strong></td>
        <td>
          <asp:Label
            ID="lblAdmittingWard"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
        <td style="padding-left: 15px"><strong>Attending Physician</strong></td>
        <td>
          <asp:Label
            ID="lblAttendingPhysician"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td><strong>Length of Stay</strong></td>
        <td>
          <asp:Label
            ID="lblLengthOfStay"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
        <td style="padding-left: 15px">
          <strong>Date Shifted into Critical Ward</strong>
        </td>
        <td>
          <asp:Label
            ID="lblDateShiftedCritical"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td><strong>Date & Time of Death</strong></td>
        <td>
          <asp:Label
            ID="lblDateTimeOfDeath"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
        <td style="padding-left: 15px">
          <strong>Death Certificate Serial #</strong>
        </td>
        <td>
          <asp:Label
            ID="lblDeathCertSerial"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
    </table>

    <!-- Clinical Details Table -->
    <table class="section-table" style="margin-top: 15px">
      <tr>
        <td style="width: 25%"><strong>Primary Diagnosis</strong></td>
        <td>
          <asp:Label
            ID="lblPrimaryDiagnosis"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td><strong>Co-morbidities</strong></td>
        <td>
          <asp:Label
            ID="lblCoMorbidities"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td><strong>Brief Clinical Summary & Event Note</strong></td>
        <td>
          <asp:Label
            ID="lblClinicalSummary"
            runat="server"
            CssClass="form-control"
          ></asp:Label>
        </td>
      </tr>
    </table>

    <!-- Review Criteria Section -->
    <div style="margin-top: 20px">
      <h3 style="margin-bottom: 5px">Review Criteria</h3>
      <p style="font-style: italic; margin-top: 0">
        Check the most appropriate box for each category:
      </p>

      <table class="review-grid">
        <thead>
          <tr>
            <th style="width: 60%; text-align: left">Category</th>
            <th style="width: 10%">Y</th>
            <th style="width: 10%">N</th>
            <th style="width: 10%">N/A</th>
            <th style="width: 10%; text-align: left">Comments</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td class="question-cell">
              Was the death expected based on the clinical path?
            </td>
            <td><asp:Label ID="lblExpY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblExpN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblExpNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblExpRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">
              Was complete & proper code decision made?
            </td>
            <td><asp:Label ID="lblCodeY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblCodeN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblCodeNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblCodeRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">Was there a delay in diagnosis?</td>
            <td><asp:Label ID="lblDiagY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblDiagN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblDiagNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblDiagRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">Was there a delay in treatment?</td>
            <td><asp:Label ID="lblTreatY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblTreatN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblTreatNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblTreatRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">
              Were clinical guidelines / protocols followed?
            </td>
            <td><asp:Label ID="lblProtoY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblProtoN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblProtoNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblProtoRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">
              Was there effective communication between the care team?
            </td>
            <td><asp:Label ID="lblCommY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblCommN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblCommNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblCommRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">
              Was there proper clinical documentation?
            </td>
            <td><asp:Label ID="lblDocY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblDocN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblDocNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblDocRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">
              Was an unplanned return to the ICU occur?
            </td>
            <td><asp:Label ID="lblIcuY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblIcuN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblIcuNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblIcuRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">Was any incident reported?</td>
            <td><asp:Label ID="lblIncY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblIncN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblIncNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblIncRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">Was Sentinel Event declared?</td>
            <td><asp:Label ID="lblSentY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblSentN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblSentNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblSentRemark" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="question-cell">
              Any HAI reported for the above patient?
            </td>
            <td><asp:Label ID="lblHaiY" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblHaiN" runat="server"></asp:Label></td>
            <td><asp:Label ID="lblHaiNA" runat="server"></asp:Label></td>
            <td style="text-align: left">
              <asp:Label ID="lblHaiRemark" runat="server"></asp:Label>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Footer Section -->
    <table class="footer-table">
      <tr>
        <td
          colspan="3"
          style="text-align: center; font-size: 10px; padding-top: 4px"
        >
          Palm Enclave Housing Scheme, Main Bypass Road, Hyderabad Sindh |
          Contact # 022 3418807-9 | Email: info@hmchyd.org
        </td>
      </tr>
    </table>
  </div>
</div>
