<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="PatientInfo.aspx.cs" Inherits="welfareSystem.PatientInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h3>Patient Information</h3>

<hr />

<div class="card">

<div class="card-body">

<div class="row">

<div class="col-md-4">
<label>Patient Name</label>
<asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-4">
<label>CNIC</label>
<asp:TextBox ID="txtCNIC" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-4">
<label>Mobile</label>
<asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"></asp:TextBox>
</div>

</div>

<br />

<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" />

</div>

</div>

</asp:Content>