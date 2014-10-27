<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="frmDailySummaryConfig.aspx.cs" Inherits="QuickWeb.Config_Setting.frmDailySummaryConfig" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Duration" Src="~/Controls/DurationControlDateWise.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (event) {

            $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_radReportFrom').click(function (e) {
                if ($(this).is(':checked')) {
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc1_radReportFrom').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc2_radReportFrom').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc3_radReportFrom').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc4_radReportFrom').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc5_radReportFrom').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc6_radReportFrom').attr('checked', true);
                }
            });
            $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_radReportMonthly').click(function (e) {
                if ($(this).is(':checked')) {
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc1_radReportMonthly').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc2_radReportMonthly').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc3_radReportMonthly').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc4_radReportMonthly').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc5_radReportMonthly').attr('checked', true);
                    $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc6_radReportMonthly').attr('checked', true);
                }
            });
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbBookingReportFalse').onclick = function (e) {
                document.getElementById('Booking').hidden = true;
                document.getElementById('Booking').style["display"] = "none"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbBookingReportTrue').onclick = function (e) {
                document.getElementById('Booking').hidden = false;
                document.getElementById('Booking').style["display"] = "block"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbSalesReportFalse').onclick = function (e) {
                document.getElementById('Sales').hidden = true;
                document.getElementById('Sales').style["display"] = "none"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbSalesReportTrue').onclick = function (e) {
                document.getElementById('Sales').hidden = false;
                document.getElementById('Sales').style["display"] = "block"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbDeliveryReportFalse').onclick = function (e) {
                document.getElementById('Delivery').hidden = true;
                document.getElementById('Delivery').style["display"] = "none"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbDeliveryReportTrue').onclick = function (e) {
                document.getElementById('Delivery').hidden = false;
                document.getElementById('Delivery').style["display"] = "block"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbPaymentTypeReportFalse').onclick = function (e) {
                document.getElementById('Payment').hidden = true;
                document.getElementById('Payment').style["display"] = "none"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbPaymentTypeReportTrue').onclick = function (e) {
                document.getElementById('Payment').hidden = false;
                document.getElementById('Payment').style["display"] = "block"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbabcTrue').onclick = function (e) {
                document.getElementById('DailyCustomer').hidden = false;
                document.getElementById('DailyCustomer').style["display"] = "block"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbabcFalse').onclick = function (e) {
                document.getElementById('DailyCustomer').hidden = true;
                document.getElementById('DailyCustomer').style["display"] = "none"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbDetailCashBookReportTrue').onclick = function (e) {
                document.getElementById('DetailCashbook').hidden = false;
                document.getElementById('DetailCashbook').style["display"] = "block"
            }
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbDetailCashBookReportFalse').onclick = function (e) {
                document.getElementById('DetailCashbook').hidden = true;
                document.getElementById('DetailCashbook').style["display"] = "none"
            }

            $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_txtReportFrom").change(function () {
                var contents = $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_txtReportFrom").val();
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc1_txtReportFrom").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc2_txtReportFrom").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc3_txtReportFrom").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc4_txtReportFrom").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc5_txtReportFrom").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc6_txtReportFrom").val(contents);
            });

            $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_txtReportUpto").change(function () {
                var contents = $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_txtReportUpto").val();
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc1_txtReportUpto").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc2_txtReportUpto").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc3_txtReportUpto").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc4_txtReportUpto").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc5_txtReportUpto").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc6_txtReportUpto").val(contents);
            });

            $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_drpMonthList").change(function () {
                var contents = $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_drpMonthList").val();
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc1_drpMonthList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc2_drpMonthList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc3_drpMonthList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc4_drpMonthList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc5_drpMonthList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc6_drpMonthList").val(contents);
            });

            $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_drpYearList").change(function () {
                var contents = $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc7_drpYearList").val();
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc1_drpYearList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc2_drpYearList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc3_drpYearList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc4_drpYearList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc5_drpYearList").val(contents);
                $("#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_uc6_drpYearList").val(contents);
            });

            $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_btnSendStatus').click(function () {
                if ($('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbBookingReportFalse').is(':checked') && $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbSalesReportFalse').is(':checked') && $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbDeliveryReportFalse').is(':checked') && $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbPaymentTypeReportFalse').is(':checked') && $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbabcFalse').is(':checked') && $('#ctl00_ContentPlaceHolder1_TabContainer5_TabPanel6_rdbDetailCashBookReportFalse').is(':checked')) {
                    alert('Please select atleast one report!');
                    return false;
                }                
            });
        });
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:TabContainer ID="TabContainer5" runat="server">
        <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText="Daily Summary">
            <ContentTemplate>
                <fieldset class="Fieldset">
                    <legend class="Legend">For All</legend>
                    <table class="TableData">
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblsummaryErr" runat="server" CssClass="ErrorMessage" EnableViewState="False" />
                                <asp:Label ID="lblsummarySucess" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                                <asp:Label ID="lblBranchMailId" runat="server" ClientIDMode="Static" Style="visibility: hidden"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="TableData">
                                    <tr>
                                        <td class="TDCaption1">
                                            For All Report
                                        </td>
                                        <td width="235px">
                                        </td>
                                        <td>
                                            <uc:Duration ID="uc7" runat="server"></uc:Duration>
                                        </td>
                                        <td width="2px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td width="2px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="Fieldset">
                    <legend class="Legend">Select Your Own</legend>
                    <table class="TableData">
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr width="100%">
                                        <td class="TDCaption">
                                            Booking Report
                                        </td>
                                        <td class="TDDot">
                                            :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbBookingReportTrue" runat="server" Text=" Yes" GroupName="PWD"
                                                Checked="True" CssClass="Legend" />
                                            <asp:RadioButton ID="rdbBookingReportFalse" runat="server" Text=" No" GroupName="PWD"
                                                CssClass="Legend" />
                                            &nbsp; &nbsp; &nbsp; &nbsp;
                                        </td>
                                        <td class="Booking" id="Booking">
                                            <uc:Duration ID="uc1" runat="server"></uc:Duration>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TDCaption">
                                            Sales Report
                                        </td>
                                        <td class="TDDot">
                                            :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSalesReportTrue" runat="server" Text=" Yes" GroupName="PWDa"
                                                Checked="True" CssClass="Legend" />
                                            <asp:RadioButton ID="rdbSalesReportFalse" runat="server" Text=" No" GroupName="PWDa"
                                                CssClass="Legend" />
                                        </td>
                                        <td class="Sales" id="Sales">
                                            <uc:Duration ID="uc2" runat="server"></uc:Duration>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TDCaption">
                                            Delivery Report
                                        </td>
                                        <td class="TDDot">
                                            :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbDeliveryReportTrue" runat="server" Text=" Yes" GroupName="PWDb"
                                                Checked="True" CssClass="Legend" />
                                            <asp:RadioButton ID="rdbDeliveryReportFalse" runat="server" Text=" No" GroupName="PWDb"
                                                CssClass="Legend" />
                                        </td>
                                        <td class="Delivery" id="Delivery">
                                            <uc:Duration ID="uc3" runat="server"></uc:Duration>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TDCaption">
                                            Payment Type Report
                                        </td>
                                        <td class="TDDot">
                                            :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbPaymentTypeReportTrue" runat="server" Text=" Yes" GroupName="PWDc"
                                                Checked="True" CssClass="Legend" />
                                            <asp:RadioButton ID="rdbPaymentTypeReportFalse" runat="server" Text=" No" GroupName="PWDc"
                                                CssClass="Legend" />
                                        </td>
                                        <td class="Payment" id="Payment">
                                            <uc:Duration ID="uc4" runat="server"></uc:Duration>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TDCaption">
                                            Daily Customer Addition Report
                                        </td>
                                        <td class="TDDot">
                                            :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbabcTrue" runat="server" Text=" Yes" GroupName="yuyu" Checked="True"
                                                CssClass="Legend" />
                                            <asp:RadioButton ID="rdbabcFalse" runat="server" Text=" No" GroupName="yuyu" CssClass="Legend" />
                                        </td>
                                        <td class="DailyCustomer" id="DailyCustomer">
                                            <uc:Duration ID="uc5" runat="server"></uc:Duration>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TDCaption">
                                            Details CashBook Report
                                        </td>
                                        <td class="TDDot">
                                            :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbDetailCashBookReportTrue" runat="server" Text=" Yes" GroupName="PWDe"
                                                Checked="True" CssClass="Legend" />
                                            <asp:RadioButton ID="rdbDetailCashBookReportFalse" runat="server" Text=" No" GroupName="PWDe"
                                                CssClass="Legend" />
                                        </td>
                                        <td class="DetailCashbook" id="DetailCashbook">
                                            <uc:Duration ID="uc6" runat="server"></uc:Duration>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="20Px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="900Px">
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSendStatus" runat="server" Text="Send Status" CausesValidation="False"
                                    Height="40px" Width="250px" OnClick="btnSendStatus_Click" />
                                <table cellpadding="0" cellspacing="0">
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                        InteractiveDeviceInfos="(Collection)" Visible="False" WaitMessageFont-Names="Verdana"
                                        WaitMessageFont-Size="14pt">
                                    </rsweb:ReportViewer>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </table>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
</asp:Content>
