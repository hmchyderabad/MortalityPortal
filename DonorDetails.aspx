<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="DonorDetails.aspx.cs" Inherits="welfareSystem.DonorDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container mt-4">
    <h3>Donor Details</h3>
    <div class="row justify-content-center">
        <div class="col-xl-10">
            <div class="card shadow-sm border-0 overflow-hidden">
                <div style="height: 4px; background: linear-gradient(90deg, #c82333, #dc3545);"></div>


<asp:HiddenField ID="hfId" runat="server" />

<div class="row">

<div class="col-md-3">
<label>Donar Name</label>
<asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<%--<div class="col-md-3">
<label>Last Name</label>
<asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
</div>--%>

<%--<div class="col-md-3">
<label>Mobile</label>
<asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"></asp:TextBox>
</div>--%>

<div class="col-md-3">
<label>Phone</label>
<asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
</div>

</div>

<br/>

<div class="row">

<div class="col-md-4">
<label>Address</label>
<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-2">
<label>City</label>
<asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-2">
<label>Country</label>
<asp:TextBox ID="txtCountry" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<%--<div class="col-md-4">
<label>Company</label>
<asp:TextBox ID="txtOccupation" runat="server" CssClass="form-control"></asp:TextBox>
</div>--%>

</div>

<br/>

<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click"/>

<hr/>

<asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">

<Columns>

<asp:BoundField DataField="FirstName" HeaderText="First Name"/>
<%--<asp:BoundField DataField="LastName" HeaderText="Last Name"/>--%>
<asp:BoundField DataField="Phone" HeaderText="Phone"/>
<asp:BoundField DataField="City" HeaderText="City"/>
<asp:BoundField DataField="Country" HeaderText="Country"/>

<asp:TemplateField HeaderText="Action">
<ItemTemplate>

<button type="button" class="btn btn-primary btn-sm"
onclick="editRow('<%# Eval("DonorId") %>')">Edit</button>

<button type="button" class="btn btn-danger btn-sm"
onclick="deleteRow('<%# Eval("DonorId") %>')">Delete</button>

</ItemTemplate>
</asp:TemplateField>

</Columns>

</asp:GridView>
                </div>
            </div>
        </div>
</div>
<script>

function editRow(id)
{
window.location="DonorDetails.aspx?edit="+id;
}

function deleteRow(id)
{
if(confirm("Delete this donor?"))
{
window.location="DonorDetails.aspx?delete="+id;
}
}

</script>

</asp:Content>
