<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoicewithItemDetail.aspx.cs"
    Inherits="QuickWeb.Reports.InvoicewithItemDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="../js/jquery-1.8.1.min.js"></script>
<head id="Head1" runat="server">
    <script type="text/javascript" language="Javascript">

        function SetPrintOption(PrnName) {
            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {
                console.log('In Print 2');
                var win4Print = window.open('', 'Win4Print');
                var msg = document.getElementById("divPrint").innerHTML;
                jsPrintSetup.setPrinter(PrnName);
                jsPrintSetup.setOption('orientation', jsPrintSetup.kPortraitOrientation);
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
                console.log('In Print 3');
                win4Print.focus();
                jsPrintSetup.print();
                win4Print.close();
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
            if ($('#hdnReportPrinted').val() == 'true') {
                //console.log('In Print');
                //SetPrintOption($('#hdnPrintValue').val());
            }
            if ($('#hdnDirectPrint').val() == 'true') {
                //$('#ImgPrintButton').click();
                console.log('In Printing');
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table>
            <center>
                <div id="divPrintOptions">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"
                                    CssClass="SuccessMessage"></asp:Label>
                                <asp:Label ID="lblQuantity" runat="server" Visible="False"></asp:Label>
                                <asp:Label ID="lblStoreName" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ImgPrintButton" runat="server" Text="Print" ClientIDMode="Static"
                                    OnClick="ImgPrintButton_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="imgCancel" runat="server" Text="Cancel" OnClientClick="return window.close();" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <tr>
                <td>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Width="100%" Height="100%"
                        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                        <LocalReport ReportPath="RDLC\InvoicewithItemDetail.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnDirectPrint" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCloseWindow" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRedirectBack" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnReportPrinted" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPrintValue" runat="server" ClientIDMode="Static" />
    </div>
    </form>
</body>
</html>
