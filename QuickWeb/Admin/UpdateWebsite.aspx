<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="UpdateWebsite.aspx.cs" Inherits="QuickWeb.Admin.UpdateWebsite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label2" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Update
                    Website
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="../images/ajax-loaderBig.gif" width="50" height="40" runat="server" id="imgLoading"
                        visible="false" />
                </td>

            </tr>
            <tr>
            </tr>
            <tr></tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
</asp:Content>
