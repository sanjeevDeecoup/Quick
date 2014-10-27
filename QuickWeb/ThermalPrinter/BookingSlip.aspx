<%@ Page Language="C#" AutoEventWireup="true" Inherits="Thermal_BookingSlip" CodeBehind="BookingSlip.aspx.cs" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <meta http-equiv="Expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="Pragma" content="no-cache" />
    <title>
        <%=AppTitle %></title>    
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="Javascript">
        function SetPrintOption() {
            document.getElementById("pnlButtons").style.visibility = "hidden";
            window.print();
            //document.getElementById("pnlButtons").style.visibility="visible";
        }
        function SetPrintOption() {                 
            var win4Print = window.open('', 'Win4Print');
            var msg = document.getElementById("divPrint").innerHTML;
            win4Print.document.write(msg);
            win4Print.document.close();
            win4Print.focus();
            win4Print.print();
            win4Print.close();
        }
    </script>


        <script type="text/javascript">

            $(document).ready(function () {

                var _res = '';
                // if there is cookie print the page
                if ($('#hdnTheCookie').val() == 'true') {
                    // hide the divpanel
                    $('#pnlButtons').hide();
                    // set the printer name
                    var PrnValue = $('#hdnPrintValue').val();
                    // call the print function
                    SetPrintOption(PrnValue);

                    // window.open('../New_Booking/frmBookingScreen.aspx');

                    // after this has been printed

                    // check if print barcode is true
                    // if it is, then print the barcode too..
                    if ($('#hdnPrintBarcode').val() == 'true') {
                        window.setTimeout(openTheBarCodeWindow, 10000);
                        return;
                    }
                    else if ($('#hdncloseWindow').val() == 'true' && $('#hdnRedirectBack').val() == 'true') {
                        //window.open('../New_Booking/frmBookingScreen.aspx');
                        window.open('../New_Booking/frm_New_Booking.aspx');
                        window.close();
                    }
                    else if ($('#hdnCloseWindow').val() == 'true' && $('#hdnRedirectBack').val() != 'true') {
                        window.close();
                        return;
                    }
                    else if ($('#hdnCloseWindow').val() != 'true' && $('#hdnRedirectBack').val() == 'true') {
                        window.open('../New_Booking/frm_New_Booking.aspx');
                        // window.location('../New_Booking/frm_New_Booking.aspx');
                        window.close();
                        return;
                    }

                }


                $('body').keydown(function (e) {
                    if (e.which == 115) {
                        var bkNum = window.location.href.substr(window.location.href.indexOf("=") + 1, window.location.href.indexOf("-1")).split('-')[0];
                        window.location = '../New_Booking/frm_New_Booking.aspx?option=Edit&BkNo=' + bkNum;
                    }
                });


                function openTheBarCodeWindow() {
                    //alert($('#hdnBookingNumber').val());
                    var _url = '../Reports/Barcodet.aspx?bookingNo=' + $('#hdnBookingNumber').val() + "&id=0&Index=0&PrintBarCode=true&CloseWindow=true&RedirectBack=true";
                    window.open(_url);
                    window.close();
                }

                $('#btnPrint').click(function (event) {
                    var PrnValue = $('#hdnPrintValue').val();
                    SetPrintOption(PrnValue);

                    if ($('#hdnRedirectBack').val() == 'true') {
                        // window.open('../New_Booking/frm_New_Booking.aspx');
                        window.open('../New_Booking/frm_New_Booking.aspx');

                    }

                    return false;
                });

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
<body onkeydown="if(event.keyCode==112){window.location='../New_Booking/frm_New_Booking.aspx';}">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel runat="server" id="pnlButtons">
        <input type="button" id="btnPrint" value="Print" style="border-left: 2px solid White; 
            color: White; font-weight: bold; font-size: 12px; background-color: #6086ac;
            font-family: Verdana; border-right-color: White; border-right-width: 2px; border-top-color: White;
            border-top-width: 2px; border-bottom-color: White; border-bottom-width: 2px;"
            tabindex="1" />
           <asp:Button ID="btnGoHome" runat="server" Text="Go Home" 
            onclick="btnGoHome_Click" />
        <asp:Button ID="btnEmail" runat="server" Text="Send Email" TabIndex="1" OnClick="btnEmail_Click"  />
        <asp:Button ID="btnGoForNewOrder" runat="server" OnClick="btnGoForNewOrder_Click"
            Text="New Booking (F1)" TabIndex="3" />
        &nbsp;<asp:Button ID="btnf2" runat="server" Text="Edit Booking (F4)" TabIndex="4"
            />       
        &nbsp;</asp:Panel>
    <table>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
    </table>    
    <div id="div1" class="paper" style="page-break-after: always;">
         <% if (strPreview.Length > 0)
           {
               Response.Write(strPreview);
           }
        %>
        <% if (strPreview1.Length > 0)
           {
               Response.Write(strPreview1);
           }
        %>
    </div>
    <div id="divPrint" style="display:none">
     <% if (strPreview.Length > 0)
           {
               Response.Write(strPreview);
           }
        %>
       <% if (strPreview1.Length > 0)
           {
               Response.Write(strPreview1);
           }
        %>
    </div>
    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="ErrorMessage" />
    <asp:HiddenField ID="hdnId" runat="server" />
    <asp:HiddenField ID="hdnEmailId" runat="server" />
    <asp:HiddenField ID="hdnTheCookie" runat="server" />
    <asp:HiddenField ID="hdnPrintValue" runat="server" />
    <asp:HiddenField ID="hdnPrintBarcode" runat="server" />
    <asp:HiddenField ID="hdnBookingNumber" runat="server" />
    <asp:HiddenField ID="hdncloseWindow" runat="server" />
    <asp:HiddenField ID="hdnRedirectBack" runat="server" />
    </form>
</body>
</html>
