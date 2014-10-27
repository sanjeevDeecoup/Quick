<%@ Page Language="C#" AutoEventWireup="true" Inherits="Reports_ItemWise" Codebehind="ItemWise.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script type="text/javascript" src="../js/jquery-1.8.1.min.js"></script>
    <script type="text/javascript" language="Javascript">
        function SetPrintOption(PrnName) {
            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {
                var win4Print = window.open('', 'Win4Print');
                var msg = document.getElementById("divPrint").innerHTML;
                jsPrintSetup.setPrinter(PrnName);
                jsPrintSetup.setOption('orientation', jsPrintSetup.kPortraitOrientation);
                /* if (indexvalue != 0) {
                    jsPrintSetup.setOption('printRange', jsPrintSetup.kRangeSpecifiedPageRange);
                    jsPrintSetup.setOption('startPageRange', 1);
                    jsPrintSetup.setOption('endPageRange', 1);
                }
                */
                jsPrintSetup.setOption('marginTop', 0);
                jsPrintSetup.setOption('marginBottom', 0);
                jsPrintSetup.setOption('marginLeft', 0);
                jsPrintSetup.setOption('marginRight', 0);
                // set empty page header
                jsPrintSetup.setOption('headerStrLeft', '');
                jsPrintSetup.setOption('headerStrCenter', '');
                jsPrintSetup.setOption('headerStrRight', '');
                // set empty page footer
                jsPrintSetup.setOption('footerStrLeft', '');
                jsPrintSetup.setOption('footerStrCenter', '');
                jsPrintSetup.setOption('footerStrRight', '');
                jsPrintSetup.definePaperSize(100, 100, 'label', 'label', 'label', 39, 86, jsPrintSetup.kPaperSizeMillimeters);
                jsPrintSetup.setPaperSizeData(100);
                jsPrintSetup.setOption('printBGColors', 1);
                jsPrintSetup.setOption('printBGImages', 1);
                jsPrintSetup.setOption('printSilent', 1);
                win4Print.document.write(msg);
                win4Print.focus();
                jsPrintSetup.print();
                //window.close();
                //                win4Print.document.close();
                //                if ($('#hdnPrintBookingSlip').val() != 'true' || $('#hdnPrintBookingSlip').val() == null || $('#hdnPrintBookingSlip').val() == '' || $('#hdnPrintBookingSlip') == null) {
                //                    win4Print.document.write(msg);
                win4Print.close();
                //                }
            }
        }
        function installjsPrintSetup() {
            if (confirm("You don't have printer plugin.\nDo you want to install the Printer Plugin now?")) {
                var xpi = new Object();
                xpi['jsprintsetup'] = '/mirrors.ibiblio.org/mozdev.org/jsprintsetup/jsprintsetup-0.9.2.xpi';
                InstallTrigger.install(xpi);
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            if ($('#hdnDirectPrint').val() == 'true') {
                //SetPrintOption($('#hdnPrinterName').val());
                //$('#ImgPrintButton').click();
                if ($('#hdnRedirectBack').val() != '') {
                    var me = window;
                    var _win = $('#hdnRedirectBack').val();
                    window.open(_win);
                    me.close();
                }
            }
        });    
    </script>
  <title>
        <%Response.Write(ConfigurationManager.AppSettings["AppTitle"]); %></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <div>
        <table>
        <center>
            <div id="divPrintOptions">
                <table>
                 <tr>
                <td>
                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" 
                            ForeColor="#CC0000" CssClass="SuccessMessage"></asp:Label>
<asp:Label ID="lblQuantity" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblStoreName" runat="server" Visible="False"></asp:Label></td></tr>
                    <tr>
                        <td align="center"> 
                           <asp:Button ID="ImgPrintButton" runat="server" Text="Print"  ClientIDMode="Static"
                                onclick="ImgPrintButton_Click" Visible="false" />
                        </td>
                        <td> 
                            <asp:Button ID="imgCancel" runat="server" Text="Cancel" OnClientClick= "return window.close();"  /> </td>
                            <td>
                                &nbsp;</td>
                    </tr>
                </table>
             </div> 
        </center>    
            <tr>
                <td>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="808px" Visible="false" 
                        Height="660px" Font-Names="Verdana" Font-Size="8pt">
                        <LocalReport ReportPath="RDLC/Report2.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                                    Name="ItemWiseDataSet_sp_Challan_ClothesWise" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                        SelectMethod="GetData" 
                        TypeName="ItemWiseDataSetTableAdapters.sp_Challan_ClothesWiseTableAdapter">
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnDirectPrint" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCloseWindow" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRedirectBack" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPrinterName" runat="server" ClientIDMode="Static" />
    </div>
    </form>
</body>
</html>
