<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="welfareSystem.Mortality_Module.View" MasterPageFile="~/AdminMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
      .badge-death {
        background: #dc3545;
        padding: 5px 15px;
        border-radius: 30px;
        font-size: 12px;
        font-weight: 600;
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
                    <i class="fas fa-file-alt"></i> Death Certificate Details
                  </h3>
                  <small>Mortality Registry - Hashim Medical City</small>
                </div>
                <div>
                  <asp:Button
                    ID="btnReviewForm"
                    runat="server"
                    Text="Review Form"
                    CssClass="btn btn-warning"
                    OnClick="btnReviewForm_Click"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Patient Information -->
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

        <!-- Death Information -->
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

        <!-- Body Receiver Information -->
        <div class="info-card">
          <h5 class="mb-4 text-success">
            <i class="fas fa-user-check"></i> Body Receiver Information
          </h5>
          <div class="row">
            <div class="col-md-4">
              <div class="info-label">Receiver Name</div>
              <div class="info-value glow-text"><%= HandOverName %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Relation</div>
              <div class="info-value"><%= HandOverRelation %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Receiver CNIC</div>
              <div class="info-value"><%= HandOverCNIC %></div>
            </div>
            <div class="col-md-4">
              <div class="info-label">Receiver Contact</div>
              <div class="info-value"><%= HandOverCellNo %></div>
            </div>
          </div>
        </div>

        <!-- Certificate Info -->
        <div class="info-card">
          <h5 class="mb-4 text-secondary">
            <i class="fas fa-info-circle"></i> Certificate Information
          </h5>
          <div class="row">
            <div class="col-md-6">
              <div class="info-label">Medical Officer ID</div>
              <div class="info-value"><%= MedicalOfficerId %></div>
            </div>
            <div class="col-md-6">
              <div class="info-label">Created Date</div>
              <div class="info-value"><%= CreatedDate %></div>
            </div>
          </div>
        </div>

        <!-- Back Button -->
        <div class="row mb-4">
          <div class="col-12">
            <asp:Button
              ID="btnBack"
              runat="server"
              Text="← Back to Certificates"
              CssClass="btn btn-secondary"
              OnClick="btnBack_Click"
            />
          </div>
        </div>
      </div>
</asp:Content>
