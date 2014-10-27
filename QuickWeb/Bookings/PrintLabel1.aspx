<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" CodeBehind="PrintLabel1.aspx.cs" Inherits="QuickWeb.Bookings.PrintLabel1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table style="width: 100%;">
        <tr valign="top">
            <td class="TDCaption" style="text-align: left">
                &nbsp;&nbsp;
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="RDLC/DynamicBarCodeReport.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Label ID="lblBranchCode" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:HiddenField ID="hdnCheckStatus" Value="0" runat="server" />
                <asp:HiddenField ID="hdnRowNo" runat="server" />
                <asp:HiddenField ID="hdnInvoiceNo" runat="server" />
                <asp:HiddenField runat="server" ID="hdnFirstTimeCheck" Value="0" />
            </td>
        </tr>
    </table>
</asp:Content>
