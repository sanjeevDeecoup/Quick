<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="BackUp_backUp" CodeBehind="backUp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label2" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Backup
                    Form
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
        </table>
        <%-- <table class="TableData">
            <tr>
                <td class="td">
                    Select Drive
                </td>
                <td style="width: 20px">
                </td>
                <td style="font-weight: bold">
                    :
                </td>
                <td style="width: 20px">
                </td>
                <td>
                    <asp:DropDownList ID="drpListofDrive" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpListofDrive_SelectedIndexChanged"
                        TabIndex="2">
                    </asp:DropDownList>
                </td>
                <td style="width: 20px">
                </td>
                <td>
                    <asp:Button ID="btnBackup" runat="server" Text="BackUp" TabIndex="3"
                        onclick="btnBackup_Click1" />
                </td>
            </tr>
        </table>--%>
    </fieldset>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:Label ID="Label1" runat="server"></asp:Label>
</asp:Content>