<%@ Page Language="C#" AutoEventWireup="true" Inherits="ActivateUser" CodeBehind="ActivateUser.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>License Activation</title>
    <link href="css/MainStylesheet.css" rel="stylesheet" type="text/css" />
    <link href="Gridview/css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnl" runat="server">
        <table class="TableData" style="margin-top: 100px; margin-left: 240px; width: 500px;">
            <tr>
                <td>
                </td>
                <td colspan="4">
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/DRY.jpg" Height="86px"
                        Width="84px" />
                </td>
            </tr>
            <tr height="50Px">
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td class="TDCaption">
                    User Key
                </td>
                <td>
                    <asp:Label ID="lblMACAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="TDCaption">
                    Activation Key
                </td>
                <td>
                    <asp:TextBox ID="txtKey" runat="server" MaxLength="100" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtKey"
                        Display="None" ErrorMessage="Please enter activation key."></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                    </cc1:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="4" nowrap="nowrap">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="ErrorMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
        <div style="margin-top: 80px;">
        </div>
        <div class="TDCaption1 MasterFooter" style="color: #0476b4">
            © <a href="http://www.quickdrycleaning.com">DC Web Services Pvt Ltd.</a> For any
            Support, Feedback or Sales Inquiry contact: <a href="mailto:info@quickdrycleaning.com">
                info@quickdrycleaning.com</a>
        </div>
    </asp:Panel>
    </form>
</body>
</html>