<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListBooking.aspx.cs" Inherits="QuickWeb.Reports.ListBooking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Booking Reports</title>
    <script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>

 
    <script type="text/javascript">

        $(document).ready(function () {
            //
            // alert($('table[style="width:7.9in;border: thin solid #000000;"]').size());
            $('table[style="width:7.9in;border: thin solid #000000;"]').attr('style', 'width:7.9in;border: thin solid #000000; page-break-after: always;');
            $('table[style="width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000"]').attr('style', 'width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000');
            $('table[style="width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000;"]').attr('style', 'width:7.9in;height:1.1in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000;');
            $('table.TableData[style="width:7.9in;border: thin solid #000000"]').attr('style', 'width:7.9in;border: thin solid #000000; page-break-after: always;');
            $('table[style="width:7.6in;height:5.12in"]').remove();
            $('table[style="width:7.9in;height:5.12in"]').remove();
            $('table[style="width:7.6in;height:5.11in"]').remove();
            $('table[style="width:7.9in;height:5.11in"]').remove();
            $('table[style="width:7.9in;height:5.2in"]').remove();
            $('table[style="width:7.6in;height:5.2in"]').remove();
            
            //width:7.9in;height:1.2in;border-bottom-style: solid; border-bottom-width: thin;border-bottom-color: #000000
            $('#pnlButtons').remove();
            // insert page-break after to all tables with width = 
            //window.print();
        });
    
    </script>

    <script type="text/javascript">
        var PrnValue;
        function helpprint() {
            PrnValue = $('#hdnPrintValue').val();
            SetPrintOption(PrnValue);
        }
        // this fuction sets the printer
        // arg = the default printer name
        function SetPrintOption(PrnValue) {
            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {

                //                var _winName = '';
                //                if (window != '') {
                //                    _winName = window;
                //                }
                // alert('here');
//var win4Print = window.open('', 'Win4Print');
                //alert('here');
                //alert('here');
                //var win4Print = window.open('', 'Win4Print');
    //var msg = document.getElementById("divPrint").innerHTML;
                // alert(PrnName);
                jsPrintSetup.setPrinter(PrnValue);
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
                jsPrintSetup.setOption('printSilent', 1);
//                win4Print.document.write(msg);
//                win4Print.document.close();
//                win4Print.focus();
                jsPrintSetup.print();

                //                if ($('#hdnPrintBarcode').val() != 'true' || $('#hdnPrintBarcode').val() == null || $('#hdnPrintBarcode').val() == '' || $('#hdnPrintBarcode') == null) {
                // win4Print.close();
                //                }
                if ($('#hdncloseWindow').val() != 'false') {
                    win4Print.close();
                }


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
</head>
<body>
    <form id="form1" runat="server">
    <div>
     
    </div>
    <asp:HiddenField ID="hdnPrintValue" runat="server" />
    </form>
   
</body>
</html>
