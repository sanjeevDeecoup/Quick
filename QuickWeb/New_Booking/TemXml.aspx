<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="TemXml.aspx.cs" Inherits="QuickWeb.New_Booking.TemXml" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript" src="../js/tag-it.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.23.custom.css" />
    <link href="../css/jquery.tagit.css" rel="stylesheet" type="text/css" />
    <link href="../css/tagit.ui-zendesk.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintGridData() {
            var prtGrid = $('#ctl00_ContentPlaceHolder1_gvUserInfo').html();
            prtGrid.border = 2;
            var prtwin = window.open('', 'PrintGridViewData', 'left=100,top=0,width=1000,height=1000,tollbar=0,scrollbars=1,status=0,resizable=1');
            prtwin.document.write(prtGrid.innerHTML);
            prtwin.document.close();
            prtwin.focus();
            prtwin.print();
            prtwin.close();
        }
</script>
<script type="text/javascript">
    $(document).ready(function () {


        $('#ctl00_ContentPlaceHolder1_btnPrint').click(function (event) {
            var PrnValue = $('#hdnPrintValue').val();
            SetPrintOption('doPDF v7');
            

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
             var win4Print = window.open('', 'Win4Print');
             var msg = document.getElementById('<%=gvUserInfo.ClientID %>');
//             alert(msg);
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
             win4Print.document.write(msg.outerHTML);
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <legend class="Legend"></legend>
        <asp:GridView ID="gvUserInfo" runat="server" BorderStyle="Solid" 
            BorderWidth="2px">
             <HeaderStyle BackColor="#3CA3C1" Font-Bold="true" ForeColor="White" />
        </asp:GridView>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <asp:Button ID="btnPrint" runat="server" Text="Start" />
        <asp:Button ID="btnStartBuildSiteMap" runat="server" Text="Start Site Map" ClientIDMode="Static" OnClick="doMe" />
        </br>
        </br>
        <asp:GridView ID="grdRpt" runat="server">
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server">
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>
