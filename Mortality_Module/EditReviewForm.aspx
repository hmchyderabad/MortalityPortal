<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditReviewForm.aspx.cs" MasterPageFile="~/AdminMaster.Master" Inherits="welfareSystem.Mortality_Module.EditReviewForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>

    <style>
      .certificate-card {
        background: linear-gradient(135deg, #f5f7fa 0%, #fff 100%);
        border-radius: 20px;
        box-shadow: 0 20px 35px rgba(0, 0, 0, 0.1);
        overflow: hidden;
      }
      .cert-header {
        background: #1a5a4c;
        padding: 20px 30px;
        color: white;
      }
      .info-card {
        background: white;
        border-radius: 15px;
        padding: 25px;
        margin-bottom: 20px;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
      }
      .info-label {
        font-weight: 600;
        color: #555;
        margin-bottom: 5px;
      }
      .info-value {
        color: #333;
        font-size: 16px;
        margin-bottom: 15px;
      }
      .glow-text {
        color: #1a5a4c;
        font-weight: 700;
      }
      .review-section {
        background: white;
        border-radius: 15px;
        padding: 25px;
        margin-bottom: 20px;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
        border-left: 5px solid #ffc107;
      }
      .review-grid {
        width: 100%;
        border-collapse: collapse;
        margin-top: 15px;
      }
      .review-grid th {
        background-color: #f8f9fa;
        padding: 12px;
        border: 1px solid #dee2e6;
        font-weight: 600;
        color: #495057;
      }
      .review-grid td {
        padding: 10px;
        border: 1px solid #dee2e6;
        vertical-align: middle;
      }
      .review-grid .question-cell {
        padding-left: 15px;
        font-size: 14px;
      }
      .review-grid input[type="text"] {
        width: 100%;
        padding: 5px;
        border: 1px solid #ced4da;
        border-radius: 4px;
        font-size: 13px;
      }
      .review-grid input[type="text"]:focus {
        outline: none;
        border-color: #1a5a4c;
        box-shadow: 0 0 0 2px rgba(26, 90, 76, 0.1);
      }
    </style>

    <div class="container mt-4">
        <!-- Page Header -->
        <div class="row mb-4">
          <div class="col-12">
            <div class="certificate-card">
              <div
                class="cert-header d-flex justify-content-between align-items-center"
              >
                <div>
                  <h3 class="mb-0">
                    <i class="fas fa-edit"></i> Edit Death Certificate Review Form
                  </h3>
                  <small>Mortality Registry - Hashim Medical City</small>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Patient Information Section -->
        <div class="info-card">
          <h5 class="mb-4 text-primary">
            <i class="fas fa-user-circle"></i> Patient Information
          </h5>
          <div class="row">
            <div class="col-md-4">
              <div class="info-label">Certificate No</div>
              <div class="info-value glow-text"><%= CertificateNo %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">MR No</div>
              <div class="info-value"><%= MRNo %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Admission No</div>
              <div class="info-value"><%= AdmNo %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Patient Name</div>
              <div class="info-value glow-text"><%= PatientName %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Age / Gender</div>
              <div class="info-value"><%= Age %> yrs / <%= Gender %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">CNIC</div>
              <div class="info-value"><%= CNIC %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Contact No</div>
              <div class="info-value"><%= ContactNo %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Consultant</div>
              <div class="info-value"><%= ConsultantName %></div>
            </div>
          </div>
        </div>

        <!-- Death Information Section -->
        <div class="info-card">
          <h5 class="mb-4 text-danger">
            <i class="fas fa-notes-medical"></i> Death Information
          </h5>
          <div class="row">
            <div class="col-md-4">
              <div class="info-label">Date of Death</div>
              <div class="info-value glow-text"><%= DateOfDeath %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Time of Death</div>
              <div class="info-value"><%= TimeOfDeath %></div>
            </div>
            <div class="col-md-12">
              <div class="info-label">Cause of Death</div>
              <div class="info-value"><%= CauseOfDeath %></div>
            </div>
            <div class="col-md-12">
              <div class="info-label">Final Diagnosis</div>
              <div class="info-value"><%= Diagnosis %></div>
            </div>
          </div>
        </div>

        <!-- Review Form Section -->
        <div class="review-section">
          <h5 class="mb-4 text-warning">
            <i class="fas fa-edit"></i> Clinical Review Details
          </h5>
          <div class="row">
            <div class="col-md-6">
              <div class="mb-3">
                <label class="fw-bold">Date of Shifted into Critical Ward</label>
                <asp:TextBox
                  ID="txtDateShifted"
                  runat="server"
                  CssClass="form-control"
                  TextMode="Date"
                ></asp:TextBox>
              </div>
            </div>
            <div class="col-md-6">
              <div class="mb-3">
                <label class="fw-bold">Co-morbidites</label>
                <asp:TextBox
                  ID="txtCoMorbidities"
                  runat="server"
                  CssClass="form-control"
                  placeholder="Enter co-morbidites"
                ></asp:TextBox>
              </div>
            </div>
            <div class="col-md-12">
              <div class="mb-3">
                <label class="fw-bold">Brief Clinical Summary</label>
                <asp:TextBox
                  ID="txtBriefClinicalSummary"
                  runat="server"
                  CssClass="form-control"
                  TextMode="MultiLine"
                  Rows="5"
                  placeholder="Enter brief clinical summary"
                ></asp:TextBox>
              </div>
            </div>
          </div>
        </div>

        <!-- Review Criteria Section -->
        <div class="review-section">
          <h5 class="mb-4 text-warning">
            <i class="fas fa-clipboard-check"></i> Review Criteria
          </h5>
          <p style="font-style: italic; margin-top: 0; margin-bottom: 15px;">Check the most appropriate box for each category:</p>

          <table class="review-grid">
            <thead>
              <tr>
                <th style="width: 60%; text-align: left;">Category</th>
                <th style="width: 10%;">Y</th>
                <th style="width: 10%;">N</th>
                <th style="width: 10%;">N/A</th>
                <th style="width: 10%; text-align: left;">Remark</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td class="question-cell">Was the death expected based on the clinical path?</td>
                <td><asp:RadioButton ID="rbExpY" runat="server" GroupName="q1" /></td>
                <td><asp:RadioButton ID="rbExpN" runat="server" GroupName="q1" /></td>
                <td><asp:RadioButton ID="rbExpNA" runat="server" GroupName="q1" /></td>
                <td><asp:TextBox ID="txtExpRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was complete & proper code decision made?</td>
                <td><asp:RadioButton ID="rbCodeY" runat="server" GroupName="q2" /></td>
                <td><asp:RadioButton ID="rbCodeN" runat="server" GroupName="q2" /></td>
                <td><asp:RadioButton ID="rbCodeNA" runat="server" GroupName="q2" /></td>
                <td><asp:TextBox ID="txtCodeRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was there a delay in diagnosis?</td>
                <td><asp:RadioButton ID="rbDiagY" runat="server" GroupName="q3" /></td>
                <td><asp:RadioButton ID="rbDiagN" runat="server" GroupName="q3" /></td>
                <td><asp:RadioButton ID="rbDiagNA" runat="server" GroupName="q3" /></td>
                <td><asp:TextBox ID="txtDiagRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was there a delay in treatment?</td>
                <td><asp:RadioButton ID="rbTreatY" runat="server" GroupName="q4" /></td>
                <td><asp:RadioButton ID="rbTreatN" runat="server" GroupName="q4" /></td>
                <td><asp:RadioButton ID="rbTreatNA" runat="server" GroupName="q4" /></td>
                <td><asp:TextBox ID="txtTreatRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Were clinical guidelines / protocols followed?</td>
                <td><asp:RadioButton ID="rbProtoY" runat="server" GroupName="q5" /></td>
                <td><asp:RadioButton ID="rbProtoN" runat="server" GroupName="q5" /></td>
                <td><asp:RadioButton ID="rbProtoNA" runat="server" GroupName="q5" /></td>
                <td><asp:TextBox ID="txtProtoRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was there effective communication between the care team?</td>
                <td><asp:RadioButton ID="rbCommY" runat="server" GroupName="q6" /></td>
                <td><asp:RadioButton ID="rbCommN" runat="server" GroupName="q6" /></td>
                <td><asp:RadioButton ID="rbCommNA" runat="server" GroupName="q6" /></td>
                <td><asp:TextBox ID="txtCommRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was there proper clinical documentation?</td>
                <td><asp:RadioButton ID="rbDocY" runat="server" GroupName="q7" /></td>
                <td><asp:RadioButton ID="rbDocN" runat="server" GroupName="q7" /></td>
                <td><asp:RadioButton ID="rbDocNA" runat="server" GroupName="q7" /></td>
                <td><asp:TextBox ID="txtDocRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was an unplanned return to the ICU occur?</td>
                <td><asp:RadioButton ID="rbIcuY" runat="server" GroupName="q8" /></td>
                <td><asp:RadioButton ID="rbIcuN" runat="server" GroupName="q8" /></td>
                <td><asp:RadioButton ID="rbIcuNA" runat="server" GroupName="q8" /></td>
                <td><asp:TextBox ID="txtIcuRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was any incident reported?</td>
                <td><asp:RadioButton ID="rbIncY" runat="server" GroupName="q9" /></td>
                <td><asp:RadioButton ID="rbIncN" runat="server" GroupName="q9" /></td>
                <td><asp:RadioButton ID="rbIncNA" runat="server" GroupName="q9" /></td>
                <td><asp:TextBox ID="txtIncRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Was Sentinel Event declared?</td>
                <td><asp:RadioButton ID="rbSentY" runat="server" GroupName="q10" /></td>
                <td><asp:RadioButton ID="rbSentN" runat="server" GroupName="q10" /></td>
                <td><asp:RadioButton ID="rbSentNA" runat="server" GroupName="q10" /></td>
                <td><asp:TextBox ID="txtSentRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
              <tr>
                <td class="question-cell">Any HAI reported for the above patient?</td>
                <td><asp:RadioButton ID="rbHaiY" runat="server" GroupName="q11" /></td>
                <td><asp:RadioButton ID="rbHaiN" runat="server" GroupName="q11" /></td>
                <td><asp:RadioButton ID="rbHaiNA" runat="server" GroupName="q11" /></td>
                <td><asp:TextBox ID="txtHaiRemark" runat="server" CssClass="form-control form-control-sm" /></td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="d-flex gap-3 mt-3">
            <asp:Button
              ID="btnUpdateReview"
              runat="server"
              Text="💾 Update Review"
              CssClass="btn btn-success px-5"
              OnClick="btnUpdateReview_Click"
            />
            <asp:Button
              ID="btnBack"
              runat="server"
              Text="← Back"
              CssClass="btn btn-secondary px-4"
              OnClick="btnBack_Click"
            />
          </div>
      </div>
</asp:Content>
