<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" CodeBehind="ChallanInwardItemWise.aspx.cs" Inherits="QuickWeb.Reports.ChallanInwardItemWise" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
<td><asp:Label ID="lbl" runat="server"></asp:Label>
<asp:Label ID="lblStoreName" runat="server"></asp:Label>
<asp:Label ID="lblQuantity" runat="server"></asp:Label></td></tr>
<tr>
<td><asp:Button ID="btnShow" Text="show" runat="server" onclick="btnShow_Click" />
<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="700px" Height="500px" 
        Font-Names="Verdana" Font-Size="8pt" 
        >
<LocalReport ReportPath="RDLC\ChallanInward.rdlc" ></LocalReport>
    </rsweb:ReportViewer></td></tr>
   <tr>
   <td>
   <asp:GridView ID="grdTmp" runat="server" CssClass="mGrid"></asp:GridView>
   </td>
   </tr>
</table>
</asp:Content>
