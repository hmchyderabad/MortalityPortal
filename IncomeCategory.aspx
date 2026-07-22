<%@ Page Language="C#" MasterPageFile="AdminMaster.master" AutoEventWireup="true" CodeBehind="IncomeCategory.aspx.cs" Inherits="welfareSystem.IncomeCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h3>Household Income Criteria</h3>
<hr />

<div class="card">
<div class="card-body">

<div class="row">

<div class="col-md-3">
<label>Criteria</label>
<asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
<asp:ListItem>Ultra Poor</asp:ListItem>
<asp:ListItem>Relatively Poor</asp:ListItem>
<asp:ListItem>Lower Middle</asp:ListItem>
<asp:ListItem>Relatively Affording</asp:ListItem>
<asp:ListItem>Affording</asp:ListItem>
</asp:DropDownList>
</div>

<div class="col-md-3">
<label>Description</label>
<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-2">
<label>Income Min</label>
<asp:TextBox ID="txtMin" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-2">
<label>Income Max</label>
<asp:TextBox ID="txtMax" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-2">
<br />
<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click"/>
</div>

</div>

<hr />

<asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered"
AutoGenerateColumns="false">

<Columns>

<asp:BoundField DataField="Code" HeaderText="Code" />
<asp:BoundField DataField="Category" HeaderText="Category" />
<asp:BoundField DataField="Description" HeaderText="Description" />
<asp:BoundField DataField="IncomeMin" HeaderText="Min Income" />
<asp:BoundField DataField="IncomeMax" HeaderText="Max Income" />

<asp:TemplateField HeaderText="Action">
<ItemTemplate>

<button class="btn btn-primary btn-sm"
onclick="editRow('<%# Eval("Code") %>','<%# Eval("Category") %>','<%# Eval("Description") %>','<%# Eval("IncomeMin") %>','<%# Eval("IncomeMax") %>')">
Edit
</button>

<button type="button" class="btn btn-danger btn-sm"
onclick="deleteRow('<%# Eval("Code") %>')">Delete</button>

</ItemTemplate>
</asp:TemplateField>

</Columns>

</asp:GridView>
<asp:HiddenField ID="hfCode" runat="server" />
</div>
</div>



<script>

    function editRow(code, category, description, min, max) {

        document.getElementById("<%=hfCode.ClientID%>").value = code;
    document.getElementById("<%=ddlCategory.ClientID%>").value = category;
    document.getElementById("<%=txtDescription.ClientID%>").value = description;
document.getElementById("<%=txtMin.ClientID%>").value = min;
    document.getElementById("<%=txtMax.ClientID%>").value = max;

}

function deleteRow(code)
{
if(confirm("Delete this record?"))
{
    window.location = "IncomeCategory.aspx?delete=" + encodeURIComponent(code);
}
}

</script>
</asp:Content>