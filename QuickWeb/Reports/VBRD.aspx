<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true" CodeBehind="VBRD.aspx.cs" Inherits="QuickWeb.Reports.VBRD" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
               
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>
                    Flat Qty Package Details
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
        <table class="TableData" width="100%">
        <tr>
        <td>
         <asp:Label ID="lblMsg" runat="server" CssClass="ErrorMessage"></asp:Label>
        </td>
        </tr>
        </table>
        <table class="TableData" width="100%">
            <tr valign="top">
                <td>
                    <table class="TableData" width="100%">
                        <tr>
                            <td>
                                <table class="TableData" width="100%">
                                    <tr>
                                        <td nowrap="nowrap">                                         
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="true" OnClientClick="return checkEntry();"
                                                OnClick="btnPrint_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>                       
                    </table>
                </td>
            </tr>
        </table>
        <table>           
            <tr valign="top">
                <td>
                    &nbsp;
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Visible="false" 
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" Height="100%">
                        <LocalReport ReportPath="RDLC\PQDR.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
     <asp:HiddenField ID="hdnDirectPrint" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCloseWindow" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRedirectBack" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnReportPrinted" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPrintValue" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCloseMe" runat="server" ClientIDMode="Static" />
        
        <script type="text/javascript">
            window.onload = testForClose();

            function testForClose() {
                try {
                    if (document.getElementById('hdnCloseMe').value === 'true')
                        window.open('../Docs/OutPut.pdf');
                    window.close();
                }
                catch (ex) {

                }
            }
        </script>
</asp:Content>
