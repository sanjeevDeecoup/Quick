<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliverySlipWithoutAmt.aspx.cs" Inherits="QuickWeb.LaserPrinter.DeliverySlipWithoutAmt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=AppTitle %></title>
    <link href="../css/MainStylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../Gridview/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
   
    <script type="text/javascript">

        $(document).ready(function () {
            $('table[style="width:7.6in;height:5.12in"]').remove();
            $('table[style="width:7.9in;height:5.12in"]').remove();
            $('table[style="width:7.9in;height:5.11in"]').remove();
            $('table[style="width:7.9in;height:5.2in"]').remove();
            $('table[style="width:7.9in;border: thin solid #000000;"]').attr('style', 'width:7.9in;border: thin solid #000000; page-break-after: always;');
            $('table[style="width:7.9in;border: thin solid #000000"]').attr('style', 'width:7.9in;border: thin solid #000000;  page-break-after: always;');

            // direct print if asked
            if ($('#hdnDirectPrint').val() == 'true') {
                if ($('#hdnPrintValue').val() != '') {
                    SetPrintOption($('#hdnPrintValue').val());
                    window.close();
                }
            }

        });
    </script>
    <script type="text/javascript">

        // this fuction sets the printer
        // arg = the default printer name
        function SetPrintOption(PrnName) {
            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {

                //                var _winName = '';
                //                if (window != '') {
                //                    _winName = window;
                //                }
                // alert('here');
                var win4Print = window.open('', 'Win4Print');
                //alert('here');
                //alert('here');
                //var win4Print = window.open('', 'Win4Print');
                var msg = document.getElementById("divPrint").innerHTML;
                // alert(PrnName);
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
                jsPrintSetup.setOption('printSilent', 1);
                win4Print.document.write(msg);
                win4Print.document.close();
                win4Print.focus();
                jsPrintSetup.print();

                //                if ($('#hdnPrintBarcode').val() != 'true' || $('#hdnPrintBarcode').val() == null || $('#hdnPrintBarcode').val() == '' || $('#hdnPrintBarcode') == null) {
                // win4Print.close();
                //                }
                if ($('#hdnCloseWindow').val() != 'false') {
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
<body onkeydown="if(event.keyCode==115){window.location='../New_Booking/frm_New_Booking.aspx?option=Edit';}else if(event.keyCode==112){window.location='../New_Booking/frm_New_Booking.aspx';}">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <div id="divButtons">
        </div>
    <div id="divPrint" class="paper" style="page-break-after: always;">
        <% if (strPreview.Length > 0)
           {
               Response.Write(strPreview);
           }
        %>
    </div>
    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="ErrorMessage" />
    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTemp" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDirectPrint" runat="server" />
    <asp:HiddenField ID="hdnRedirectBack" runat="server" />
    <asp:HiddenField ID="hdnPrintValue" runat="server" />
    <asp:HiddenField ID="hdnCloseWindow" runat="server" />
    </form>
</body>

</html>
