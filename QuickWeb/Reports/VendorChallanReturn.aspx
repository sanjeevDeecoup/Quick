<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="Report_VendorChallanReturn" Title="Untitled Page" Codebehind="VendorChallanReturn.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/MainStyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript"> 

    




    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    &nbsp;<asp:HiddenField ID="hdnSPBookingFrom" runat="server" />
    <table>
    <tr>
    <td>
    <table>
           
            <tr>
                <td style="width:10px" ></td>
                <td class="TDCaption">
                    Invoice No&nbsp;&nbsp;
                </td>
                <td>
                    <%--<cc1:ComboBox ID="ddlinvoice" runat="server" AutoCompleteMode="Suggest" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlinvoice_SelectedIndexChanged" DropDownStyle="DropDownList"
                        Width="100px" RenderMode="Block">
                    </cc1:ComboBox>--%>
                    <asp:DropDownList ID="ddlinvoice" runat="server" Width="100px"  
                        AutoPostBack="true" onselectedindexchanged="ddlinvoice_SelectedIndexChanged"></asp:DropDownList>
                    &nbsp;&nbsp;
                </td>
                 <td>
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="ddlno" runat="server">
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                </td>
                <td>
          <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" 
        onclick="btnPrint_Click" />
   
                </td></td></tr></table>    
    <asp:HiddenField ID="hdnSelectedProcessType" runat="server" />
    <asp:HiddenField ID="hdnPrefixForCurrentYear" runat="server" />
    <table>
    <tr>
    <td>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="734px" Visible="false">
        <LocalReport ReportPath="RDLC/DynamicBarCodeReport.rdlc"></LocalReport>
        </rsweb:ReportViewer>
    </td></tr></table>
    <asp:SqlDataSource ID="SqlSourceChallanShifts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT ID,VendorName FROM mstVendor"></asp:SqlDataSource>
</asp:Content>
