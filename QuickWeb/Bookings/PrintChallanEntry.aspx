<%@ Page Language="C#" AutoEventWireup="true" Inherits="PrintChallanEntry" CodeBehind="PrintChallanEntry.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="../js/jquery-1.8.1.min.js"></script>
    <script type="text/javascript" language="Javascript">

        function SetPrintOption(PrnName) {

            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {

                var win4Print = window.open('', 'Win4Print');
                var msg = document.getElementById("litChallanDiv").innerHTML;

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

                win4Print.focus();

                jsPrintSetup.print();
                win4Print.close();
                //window.close();
                //                win4Print.document.close();

                //                if ($('#hdnPrintBookingSlip').val() != 'true' || $('#hdnPrintBookingSlip').val() == null || $('#hdnPrintBookingSlip').val() == '' || $('#hdnPrintBookingSlip') == null) {
                //                    win4Print.document.write(msg);

                // win4Print.close();
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
                SetPrintOption($('#hdnPrinterName').val());
                //$('#ImgPrintButton').click();
                if ($('#hdnRedirectBack').val() != '') {
                    var me = window;
                    var _win = $('#hdnRedirectBack').val();
                    console.log(_win);
                    window.open(_win);
                    me.close();
                }
            }
        });
    </script>
    <title>Print Challan Entry</title>
    <%-- <script type="text/javascript" language="javascript">
        function PrintChallan(sender)
        {
            sender.style.visibility='hidden';
            window.print();
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <%-- <input type="button" value="Print" onclick="PrintChallan(this);" />--%>
    <div id="litChallanDiv">
        <asp:Literal ID="litChallan" runat="server" ClientIDMode="Static"></asp:Literal>
    </div>
    <asp:HiddenField ID="hdnDirectPrint" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCloseWindow" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRedirectBack" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPrinterName" runat="server" ClientIDMode="Static" />
    </form>
</body>
</html>