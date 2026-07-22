<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="PatientRegistration.aspx.cs" Inherits="welfareSystem.PatientRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

<h2 class="text-center text-danger fw-bold">
Hashim Medical City Hospital, Hyderabad
</h2>

<h4 class="text-center text-success mb-4">
Criteria of Patient Welfare
</h4>

<!-- ================= Patient Info ================= -->

<div class="card shadow mb-4">

<div class="card-header bg-primary text-white">
Patient Information
</div>

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

<div class="row">

<div class="col-md-4">
<label>Address</label>
<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-4">
<label>Monthly Income</label>
<asp:TextBox ID="txtIncome" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-4">
<label>Family Members</label>
<asp:TextBox ID="txtFamily" runat="server" CssClass="form-control"></asp:TextBox>
</div>

</div>

</div>

</div>


<!-- ================= Income Category ================= -->

<div class="card shadow mb-4">

<div class="card-header bg-success text-white">
Household Income Category
</div>

<div class="card-body">

<table class="table table-bordered table-striped">

<thead class="table-dark">

<tr>
<th>Code</th>
<th>Category</th>
<th>Description</th>
<th>Income</th>
</tr>

</thead>

<tbody>

<tr>
<td>1</td>
<td>Ultra Poor</td>
<td>Income less than basic needs</td>
<td>Less than 16,360</td>
</tr>

<tr>
<td>2</td>
<td>Relatively Poor</td>
<td>Household receives 50% less income than average</td>
<td>Less than 32,000</td>
</tr>

<tr>
<td>3</td>
<td>Lower Middle</td>
<td>Income between lower and average household</td>
<td>32,000 – 65,000</td>
</tr>

<tr>
<td>4</td>
<td>Relatively Affording</td>
<td>Middle and upper middle class</td>
<td>65,000 – 266,000</td>
</tr>

<tr>
<td>5</td>
<td>Affording</td>
<td>High income household</td>
<td>Above 266,000</td>
</tr>

</tbody>

</table>

</div>

</div>



<!-- ================= Discount Table ================= -->

<div class="card shadow mb-4">

<div class="card-header bg-warning text-dark">
Full Time Permanent Staff Discount
</div>

<div class="card-body">

<table class="table table-bordered text-center">

<thead class="table-secondary">

<tr>
<th>Sr</th>
<th>Treatment</th>
<th>Self</th>
<th>Spouse & Kids</th>
<th>Parents</th>
</tr>

</thead>

<tbody>

<tr>
<td>1</td>
<td>Admission</td>
<td>100%</td>
<td>70%</td>
<td>50%</td>
</tr>

<tr>
<td>2</td>
<td>Lab</td>
<td>70%</td>
<td>70%</td>
<td>50%</td>
</tr>

<tr>
<td>3</td>
<td>Radiology</td>
<td>70%</td>
<td>50%</td>
<td>40%</td>
</tr>

<tr>
<td>4</td>
<td>Pharmacy</td>
<td colspan="3">12% majority of medical supplies</td>
</tr>

<tr>
<td>5</td>
<td>Consultation</td>
<td>Rs 300</td>
<td>Rs 300</td>
<td>50%</td>
</tr>

<tr>
<td>6</td>
<td>Physiotherapy</td>
<td>50%</td>
<td>50%</td>
<td>30%</td>
</tr>

<tr>
<td>7</td>
<td>OT</td>
<td>70%</td>
<td>50%</td>
<td>30%</td>
</tr>

</tbody>

</table>

</div>

</div>


<!-- ================= Submit Button ================= -->

<div class="text-center">

<asp:Button ID="btnSave" runat="server" Text="Register Patient"
CssClass="btn btn-lg btn-success" />

</div>

</div>

</asp:Content>
