<%@ Page Language="C#" AutoEventWireup="true" Inherits="Reports_Barcodet" CodeBehind="Barcodet.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/BarCodeSetting.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="Javascript">
       
        function SetPrintOption() {
            document.getElementById("divButtons").style.visibility = "hidden";
            window.print();

            //document.getElementById("divButtons").style.visibility="visible";
        }
        function SetPrintOption(PrnName, indexvalue) {

            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {



                var win4Print = window.open('', 'Win4Print');
                var msg = document.getElementById("divPrint").innerHTML;
               
                jsPrintSetup.setPrinter(PrnName);

                jsPrintSetup.setOption('orientation', jsPrintSetup.kPortraitOrientation);



                if (indexvalue != 0) {
                    jsPrintSetup.setOption('printRange', jsPrintSetup.kRangeSpecifiedPageRange);
                    jsPrintSetup.setOption('startPageRange', 1);
                    jsPrintSetup.setOption('endPageRange', 1);
                }
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
         $(document).ready(function () {
             $('.divTestBarcode').css('font-family', '"Code 128"');
             // check if direct printing
             if ($('#hdnComingFrom').val() == 'PrintBarCode') {
                 // we are directly printing
                 // just print
                 $('input').hide();
                 var _PrnValue = $('#hdnValue').val();
                 var indexvalue = $('#hdnIndexValue').val();
                 debugger;
                 SetPrintOption(_PrnValue, indexvalue);

                 // if asked to close window, close it
                 if ($('#hdnCloseWindow').val() == 'true' && $('#hdnRedirectBack').val() == 'true') {
                     debugger;
                     var me = window;
                     window.open('../New_Booking/frm_New_Booking.aspx');
                     // window.location('../New_Booking/frm_New_Booking.aspx');
                     debugger;
                     me.close();
                     window.close();
                     return;
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

                 // after this has been printed
                 // check if print booking slip is true
                 // if it is, then print the booking slip too..
                 //                if ($('#hdnPrintBookingSlip').val() == 'true') {
                 //                    window.setTimeout(openTheBookingSlipWindow, 10000);
                 //                }
                 //                else if ($('#hdnRedirectBack').val() == 'true') {
                 //                    // window.open('../New_Booking/frm_New_Booking.aspx');
                 //                    window.open('../New_Booking/frm_New_Booking.aspx');
                 //                    window.close();

                 //                }
                 return;
             }

             //            if ($('#hdnPrintBookingSlip').val() != 'true' || $('#hdnPrintBookingSlip').val() == null || $('#hdnPrintBookingSlip').val() == '' || $('#hdnPrintBookingSlip') == null) {
             //                // only if coming from bookingScreen
             //                if ($('#hdnRedirectBack').val() == 'true') {
             //                    // window.open('../New_Booking/frm_New_Booking.aspx');
             //                    window.open('../New_Booking/frm_New_Booking.aspx');
             //                    window.close();
             //                }
             //                // window.open('../New_Booking/frm_New_Booking.aspx');
             //            }


             function openTheBookingSlipWindow(argBookingNumber, argDate) {
                 //alert($('#hdnBookingNumber').val());

                 var _url = '../Bookings/BookingSlip.aspx?BN=' + $('#hdnBookingNumber').val() + '-1' + $('#hdnBookingDate').val() + '&DirectPrint=true';
                 window.open(_url);
                 window.close();
             }

             $('#btnPrint').click(function () {
                 var PrnValue = $('#hdnValue').val();
                 var indexvalue = $('#hdnIndexValue').val();


                 SetPrintOption(PrnValue, indexvalue);
                 SaveInvoiceHistoryData();
             });

         });

         function SaveInvoiceHistoryData() {
             $.ajax({
                 url: '../Autocomplete.asmx/InsertInvoiceHistory',
                 data: "{BookingNo: '" + $('#hdnTmpBookingNo').val() + "', ItemRowIndex: '" + $('#hdnTmpItemRowIndex').val() + "'}",
                 type: 'POST',
                 datatype: 'JSON',
                 contentType: 'application/json; charset=utf8',
                 async: true,
                 success: function (response) {
                     result = response.d;
                 },
                 error: function (response) {
                 
                 }
             });            
         }    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position: absolute">
        <table>
            <tr>
                <td>
                    &nbsp;<input type="button" id="btnPrint" value="Print" style="background-color: #ABDC28;
                        font-weight: bold; font-family: Tahoma" tabindex="1" onclick="return btnPrint_onclick()" />
                       
                </td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_Click" />
                    <asp:HiddenField ID="hdnValue" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnComingFrom" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnPrintBookingSlip" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnBookingNumber" runat="server" />
                    <asp:HiddenField ID="hdnBookingDate" runat="server" />
                    <asp:HiddenField ID="hdnRedirectBack" runat="server" />
                    <asp:HiddenField ID="hdnIndexValue" runat="server" />
                    <asp:HiddenField ID="hdnCloseWindow" runat="server" />
                    <asp:HiddenField ID="hdnTmpBookingNo" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnTmpItemRowIndex" runat="server" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
    </div>
    
    <div id="divPrint">
        <center>
            <% if (strPreview.Length > 0)
               {
                   Response.Write(strPreview);
               }
            %>
        </center>
    </div>
   
    </form>
</body>
</html>
