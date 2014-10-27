<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="BillDetailReport.aspx.cs" Inherits="QuickWeb.Reports.BillDetailReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="../js/jquery-1.8.1.min.js"></script>
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
    <script type="text/javascript" language="javascript">
        function checkEntry() {
            var ReportFrmChecked = document.getElementById("<%=radReportFrom.ClientID %>").checked;
            var frmDate = document.getElementById("<%=txtReportFrom.ClientID %>").value;
            var toDate = document.getElementById("<%=txtReportUpto.ClientID %>").value;

            if (ReportFrmChecked == true) {
                if (frmDate == "" || toDate == "") {
                    alert("Please select date from and upto which report is to be generated.");
                    document.getElementById("<%=txtReportFrom.ClientID %>").focus();
                    return false;

                }
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
               
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>
                    Bill Details
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
                                            <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                                                Text="From" CssClass="TDCaption" />
                                            &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                                onpaste="return false;" />
                                            <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                                Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                                            </cc1:CalendarExtender>
                                            &nbsp;<span class="TDCaption">To</span><asp:TextBox ID="txtReportUpto" runat="server"
                                                Width="80px" onkeypress="return false;" onpaste="return false;" />
                                            <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                                Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                                            </cc1:CalendarExtender>
                                            <asp:RadioButton ID="radReportMonthly" runat="server" GroupName="radReportOptions"
                                                Text="Monthly Report" CssClass="TDCaption" />
                                            &nbsp;<asp:DropDownList ID="drpMonthList" runat="server">
                                                <asp:ListItem Selected="True" Value="1">January</asp:ListItem>
                                                <asp:ListItem Value="2">February</asp:ListItem>
                                                <asp:ListItem Value="3">March</asp:ListItem>
                                                <asp:ListItem Value="4">April</asp:ListItem>
                                                <asp:ListItem Value="5">May</asp:ListItem>
                                                <asp:ListItem Value="6">June</asp:ListItem>
                                                <asp:ListItem Value="7">July</asp:ListItem>
                                                <asp:ListItem Value="8">August</asp:ListItem>
                                                <asp:ListItem Value="9">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;<asp:DropDownList ID="drpYearList" runat="server">
                                            </asp:DropDownList>
                                            <%--<asp:RadioButton ID="rdoCustomer" runat="server" Text="Customer Name" GroupName="radReportOptions"
                                                CssClass="TDCaption" Visible="false" />--%>
                                            <asp:CheckBox ID="chkShowCustomer" runat="server" Text="Customer" 
                                                AutoPostBack ="true" oncheckedchanged="chkShowCustomer_CheckedChanged" />
                                            <asp:TextBox ID="txtCustomerSearch" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                                                Width="450px" Visible="false"></asp:TextBox>
                                            <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerSearch"
                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" DelimiterCharacters=""
                                                Enabled="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                            </cc1:AutoCompleteExtender>
                                            
                                            
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();" OnClick ="btnShowReport_Click" />
                                            &nbsp; &nbsp;
                                       
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="true" OnClientClick="return checkEntry();"
                                                OnClick="btnPrint_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table>
            <tr valign="top">
                <td>
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
                </td>
            </tr>
            <tr valign="top">
                <td>
                    &nbsp;
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Visible="false" 
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
                        <LocalReport ReportPath="RDLC\BillDetails.rdlc">
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
</asp:Content>
