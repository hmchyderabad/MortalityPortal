<%@ Page Language="C#"
MasterPageFile="AdminMaster.master"
AutoEventWireup="true"
CodeBehind="ViewDeclaration.aspx.cs"
Inherits="welfareSystem.ViewDeclaration" %>

<asp:Content ID="Content1"
ContentPlaceHolderID="MainContent"
runat="server">

<div class="container mt-4">

    <div class="d-flex justify-content-between mb-3">

        <h3>
            Patient Declaration Details
        </h3>

        <button
            type="button"
            class="btn btn-primary"
            onclick="window.print();">

            Print

        </button>

    </div>

    <!-- PATIENT INFO -->

    <table class="table table-bordered">

        <tr>
            <th width="25%">Request ID</th>
            <td>
                <asp:Label
                    ID="lblRequestID"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

        <tr>
            <th>Adm No</th>
            <td>
                <asp:Label
                    ID="lblAdmNo"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

        <tr>
            <th>MR No</th>
            <td>
                <asp:Label
                    ID="lblMRNo"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

        <tr>
            <th>Patient Name</th>
            <td>
                <asp:Label
                    ID="lblPatientName"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

        <tr>
            <th>Attendant Name</th>
            <td>
                <asp:Label
                    ID="lblAttendantName"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

        <tr>
            <th>Relationship</th>
            <td>
                <asp:Label
                    ID="lblRelationship"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

        <tr>
            <th>Submission Date</th>
            <td>
                <asp:Label
                    ID="lblDate"
                    runat="server">
                </asp:Label>
            </td>
        </tr>

    </table>

    <!-- SIGNATURE -->

    <div class="mb-4">

        <h4>
            Signature
        </h4>

        <img
            id="imgSignature"
            runat="server"
            width="300"
            height="120"
            style="border:1px solid #ccc;" />

    </div>

    <!-- DOCUMENTS -->

    <h4 class="mb-3">
        Uploaded Documents
    </h4>

    <asp:GridView
        ID="gvDocuments"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-bordered table-striped">

        <Columns>

            <asp:BoundField
                DataField="DocumentName"
                HeaderText="Document Name" />

            <asp:BoundField
                DataField="UploadedDate"
                HeaderText="Uploaded Date"
                DataFormatString="{0:dd-MM-yyyy}" />

            <asp:TemplateField HeaderText="Action">

                <ItemTemplate>

                    <a
                        href='<%# ResolveUrl(Eval("FilePath").ToString()) %>'
                        target="_blank"
                        class="btn btn-info btn-sm">

                        View

                    </a>

                    <a
                        href='<%# ResolveUrl(Eval("FilePath").ToString()) %>'
                        download
                        class="btn btn-success btn-sm">

                        Download

                    </a>

                </ItemTemplate>

            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

</asp:Content>