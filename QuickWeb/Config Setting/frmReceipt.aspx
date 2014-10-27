<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    EnableEventValidation="false" Inherits="Config_Setting_frmReceipt" Title="Untitled Page"
    CodeBehind="frmReceipt.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {
            try {
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbItemCreationFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbItemCreationTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbRateChangeFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbRateChangeTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbDiscountChangeFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbDiscountChangeTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbDiscountDelChangeFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbDiscountDelChangeTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_btnPasswordSave').onclick = function (e) {
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbItemCreationTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtPasswordItemCreation').focus();
                        return false;
                    }
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbRateChangeTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtRateChange').focus();
                        return false;
                    }
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbDiscountChangeTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountChange').focus();
                        return false;
                    }
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_rdbDiscountDelChangeTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer6_TabPanel7_txtDiscountDelChange').focus();
                        return false;
                    }
                }

            }

            catch (e) {
                // whatever
            }

            // if inclusive, disallow after
            $('#btnServiceTaxSave').click(function (event) {

                // $('#rdbTaxAfter').is(':checked')
                if ($('#drpServiceTaxType').val() == 'Inclusive' && $('#rdbTaxAfter').is(':checked')) {
                    // disabled for now
                    //alert('can\'t have tax after discount, if tax calculation is inclusive');
                    //return false;
                }
            });
            var printers = jsPrintSetup.getPrintersList();
            var prt = printers.split(',');
            var printValue = $('#hdnValue').val();
            $('#<%=drpDefaultPrinter.ClientID %>').append($("<option value='" + prt[0] + "'" + ">" + printValue + "</option>"));
            for (var i = 0; i < prt.length; i++) {

                if (prt[i] == printValue)
                    continue;

                $('#<%=drpDefaultPrinter.ClientID %>').append($("<option value='" + prt[i] + "'" + ">" + prt[i] + "</option>"));

            }

            $('#<%=drpDefaultPrinter.ClientID %>').change(function () {

                $('#<%=hdnPrint.ClientID %>').val($('#<%=drpDefaultPrinter.ClientID %>').val())
            });

        });
    </script>
    <fieldset class="Fieldset">
        <legend class="Legend">Dry Soft Configuration</legend>
        <table class="TableData">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnSetTimeZone" runat="server" Text="Set Time Zone" Width="150" OnClick="btnSetTimeZone_Click"
                                    Style="text-align: left" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDefaultSetting" runat="server" Text="Pick Up/New Order" Width="150"
                                    Style="text-align: left" OnClick="btnDefaultSetting_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDispaly" runat="server" Text="Store Information" Width="150" Style="text-align: left"
                                    OnClick="btnDispaly_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnReceipt" runat="server" Text="Receipt" Width="150" Style="text-align: left"
                                    OnClick="btnReceipt_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnGeneralSetting" runat="server" Text="General" Width="150" Style="text-align: left"
                                    OnClick="btnGeneralSetting_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSetEmail" runat="server" Text="EMail" Width="150" Style="text-align: left"
                                    OnClick="btnSetEmail_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnBackUp" runat="server" Text="Backup Path" Width="150" Style="text-align: left"
                                    OnClick="btnBackUp_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnServiceTac" runat="server" Text="Default Tax Rates" Width="150"
                                    Style="text-align: left" OnClick="btnServiceTac_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPassWordProtected" runat="server" Text="Password Controls" Width="150"
                                    Style="text-align: left" OnClick="btnPassWordProtected_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="tdReceipt" runat="server">
                    <asp:Button ID="btnSave" runat="server" Text="Save Configuration" OnClick="btnSave_Click1" />
                    <asp:Label ID="lblMsg" runat="server" CssClass="SuccessMessage" EnableViewState="false"></asp:Label>
                    <cc1:TabContainer ID="tbReceipt" runat="server" ActiveTabIndex="0">
                        <cc1:TabPanel HeaderText="General" ID="TabPanel1" runat="server">
                            <HeaderTemplate>
                                General
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend">General Setting</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="Legend" colspan="3" style="width: 300Px">
                                                            Select Printer Type
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdrLaser" runat="server" Checked="True" GroupName="p" Text="  Laser (8.5 * 5.5 Inches)"
                                                                CssClass="TDCaption" AutoPostBack="True" OnCheckedChanged="rdrLaser_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdrDotMatrix" runat="server" GroupName="p" Text="  Thermal (80 mm Roll Paper)"
                                                                CssClass="TDCaption" AutoPostBack="True" OnCheckedChanged="rdrDotMatrix_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdrThermal" AutoPostBack="True" GroupName="p" Text=" DotMatrix (80 mm Roll Paper)"
                                                                CssClass="TDCaption" runat="server" OnCheckedChanged="rdrThermal_CheckedChanged"
                                                                Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trStoreCopy" runat="server">
                                                        <td id="Td1" class="Legend" runat="server">
                                                        </td>
                                                        <td id="Td2" class="TDCaption" runat="server">
                                                        </td>
                                                        <td id="Td5" runat="server">
                                                            <asp:CheckBox ID="chkStoreCopy" runat="server" CssClass="Legend" Text="Select to Print additional Store Copy of Invoice" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" class="Legend">
                                                            Print Previous Due
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbPreviousTrue" runat="server" Checked="True" CssClass="TDCaption"
                                                                GroupName="c" Text=" Yes" />
                                                            &nbsp;&nbsp;
                                                            <asp:RadioButton ID="rdbPreviousFalse" runat="server" CssClass="TDCaption" GroupName="c"
                                                                Text=" No" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="height: 10Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" class="Legend">
                                                            Print Header Slogan
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbHeaderSloganTrue" runat="server" Checked="True" CssClass="TDCaption"
                                                                GroupName="rtyu" Text=" Yes" />
                                                            &nbsp;&nbsp;
                                                            <asp:RadioButton ID="rdbHeaderSloganFalse" runat="server" CssClass="TDCaption" GroupName="rtyu"
                                                                Text=" No" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="height: 10Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" class="Legend">
                                                            Print Term &amp; Condition
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbTermConditionTrue" runat="server" Checked="True" CssClass="TDCaption"
                                                                GroupName="abc" Text=" Yes" />
                                                            &nbsp;&nbsp;<asp:RadioButton ID="rdbTermConditionFalse" runat="server" CssClass="TDCaption"
                                                                GroupName="abc" Text=" No" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td43" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Rate
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbRateTrue" runat="server" Text=" Yes" GroupName="WC" CssClass="TDCaption"
                                                                Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbRateFalse" runat="server" Text=" No"
                                                                    GroupName="WC" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td45" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print SubItems
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbSubItemTrue" runat="server" Text=" Yes" GroupName="ICC" CssClass="TDCaption"
                                                                Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbSubItemFalse" runat="server"
                                                                    Text=" No" GroupName="ICC" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td47" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print PhoneNo
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbPhoneNoTrue" runat="server" Text=" Yes" GroupName="ICCS"
                                                                CssClass="TDCaption" Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbPhoneNoFalse"
                                                                    runat="server" Text=" No" GroupName="ICCS" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td49" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Tax Bifurcation
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbTaxDetailTrue" runat="server" Text=" Yes" GroupName="POP"
                                                                CssClass="TDCaption" Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbTaxDetailFalse"
                                                                    runat="server" Text=" No" GroupName="POP" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="border-left-style: solid; border-left-width: thin; border-left-color: #000000;">
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Barcode
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdrBarcodeTrue" runat="server" GroupName="cd" Text=" Yes" Checked="True"
                                                                CssClass="TDCaption" />&#160;&#160;
                                                            <asp:RadioButton ID="rdrBarcodeFalse" runat="server" GroupName="cd" Text=" No" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Label ID="lblPrinter" runat="server" CssClass="TDDot" Visible="False"></asp:Label>
                                                            <asp:RadioButton ID="rdbDotMatrix40" runat="server" Text=" Dotmatrix(80 column)"
                                                                CssClass="Legend" Visible="False" GroupName="fgh" Checked="True" />
                                                            <asp:RadioButton ID="rdb3Inches" runat="server" Text=" Dotmatrix (3 Inches)" CssClass="Legend"
                                                                Visible="False" GroupName="fgh" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="height: 10Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Logo
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbShowOnReceiptTrue" runat="server" GroupName="afghj" Text=" Yes"
                                                                Checked="True" CssClass="TDCaption" />&#160;&#160;
                                                            <asp:RadioButton ID="rdbShowOnReceiptFalse" runat="server" GroupName="afghj" Text=" No"
                                                                CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="height: 10Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Tax
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td3" class="Legend" runat="server">
                                                        </td>
                                                        <td id="Td4" class="TDCaption" runat="server">
                                                        </td>
                                                        <td id="Td55" runat="server">
                                                            <asp:RadioButton ID="rdbServicetaxTrue" runat="server" GroupName="ac" Text=" Yes"
                                                                Checked="True" CssClass="TDCaption" />&#160;&#160;
                                                            <asp:RadioButton ID="rdbServicetaxFalse" runat="server" GroupName="ac" Text=" No"
                                                                CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td6" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Table Border
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbTableBorderTrue" runat="server" Text=" Yes" GroupName="abcghj"
                                                                CssClass="TDCaption" Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbTableBorderFalse"
                                                                    runat="server" Text=" No" GroupName="abcghj" CssClass="TDCaption" />
                                                            <asp:RadioButton ID="rdbFooterSloganTrue" runat="server" GroupName="asdfgh" Text=" Yes"
                                                                Checked="True" CssClass="TDCaption" Visible="False" />&#160;&#160;
                                                            <asp:RadioButton ID="rdbFooterSloganFalse" runat="server" GroupName="asdfgh" Text=" No"
                                                                CssClass="TDCaption" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td42" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Process
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbProcessTrue" runat="server" Text=" Yes" GroupName="DC" CssClass="TDCaption"
                                                                Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbProcessFalse" runat="server"
                                                                    Text=" No" GroupName="DC" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td44" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print DueDate
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbPrintDueDateTrue" runat="server" Text=" Yes" GroupName="DCD"
                                                                CssClass="TDCaption" Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbPrintDueDateFalse"
                                                                    runat="server" Text=" No" GroupName="DCD" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td46" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Terms & Condition On Store Copy
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbPrintTermsConditonTrue" runat="server" Text=" Yes" GroupName="Condition"
                                                                CssClass="TDCaption" Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbPrintTermsConditonFalse"
                                                                    runat="server" Text=" No" GroupName="Condition" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td48" colspan="3" style="height: 10Px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend" colspan="3">
                                                            Print Customer Signature
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Legend">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbCustomerSignatureTrue" runat="server" Text=" Yes" GroupName="Sign"
                                                                CssClass="TDCaption" Checked="True" />&#160;&#160;<asp:RadioButton ID="rdbCustomerSignatureFalse"
                                                                    runat="server" Text=" No" GroupName="Sign" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table>
                                        <tr>
                                            <td class="Legend">
                                                Booking No/Booking Date Format
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpBookingNoFontStyle" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td class="Legend">
                                                Size
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpFontBookingNoSize" runat="server">
                                                    <asp:ListItem Text="9" />
                                                    <asp:ListItem Text="10" />
                                                    <asp:ListItem Text="11" />
                                                    <asp:ListItem Text="12" />
                                                    <asp:ListItem Text="13" />
                                                    <asp:ListItem Text="14" />
                                                    <asp:ListItem Text="15" />
                                                    <asp:ListItem Text="16" />
                                                    <asp:ListItem Text="17" />
                                                    <asp:ListItem Text="18" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td class="Legend">
                                                Style
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkBookingB" Text=" B" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkBookingI" Text=" I" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkBookingU" Text=" U" runat="server" CssClass="TDCaption" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11" style="height: 10Px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend">
                                                Name/Address Format
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpNameFontStyle" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td class="Legend">
                                                Size
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpNameFontSize" runat="server">
                                                    <asp:ListItem Text="9" />
                                                    <asp:ListItem Text="10" />
                                                    <asp:ListItem Text="11" />
                                                    <asp:ListItem Text="12" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td class="Legend">
                                                Style
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkNameB" Text=" B" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkNameI" Text=" I" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkNameU" Text=" U" runat="server" CssClass="TDCaption" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend">
                                                Default printer
                                            </td>
                                            <td class="TDDot">
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpDefaultPrinter" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td class="Legend">
                                            </td>
                                            <td class="TDDot">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td class="Legend">
                                            </td>
                                            <td class="TDDot">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel HeaderText="Receipt Header" ID="ReceiptHeader" runat="server">
                            <ContentTemplate>
                                <table class="TableData">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="Legend">Header Setting</legend>
                                                <table>
                                                    <tr>
                                                        <td nowrap="nowrap">
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td align="center" colspan="4">
                                                            <asp:RadioButton ID="rdrLogoAndTest" runat="server" AutoPostBack="True" Checked="True"
                                                                CssClass="Legend" GroupName="a" OnCheckedChanged="rdrLogoAndTest_CheckedChanged"
                                                                Text="  Create Your Header" />&#160;&#160;&#160;&#160;
                                                            <asp:RadioButton ID="rdrBanner" runat="server" CssClass="Legend" GroupName="a" OnCheckedChanged="rdrBanner_CheckedChanged"
                                                                Text="  Pre Printed" AutoPostBack="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap">
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td nowrap="nowrap">
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap">
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                            <table id="tblLogo" runat="server">
                                                                <tr id="Tr1" runat="server">
                                                                    <td id="Td301" class="Legend" colspan="12" style="background-color: #C0C0C0" runat="server">
                                                                        TEXT
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr2" runat="server">
                                                                    <td id="Td7" class="Legend" runat="server">
                                                                        Name
                                                                    </td>
                                                                    <td id="Td8" runat="server" class="style1">
                                                                        :
                                                                    </td>
                                                                    <td id="Td9" runat="server">
                                                                        <asp:TextBox ID="txtName" runat="server" Width="150px" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                    <td class="Legend" nowrap="nowrap" runat="server" id="tdName" visible="false">
                                                                        Name
                                                                    </td>
                                                                    <td runat="server" id="tdName1" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td runat="server" id="tdName2" visible="false">
                                                                        <asp:DropDownList ID="drpFontName" runat="server" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="Legend" nowrap="nowrap" runat="server" id="tdName3" visible="false">
                                                                        Size
                                                                    </td>
                                                                    <td runat="server" id="tdName4" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td runat="server" id="tdName5" visible="false">
                                                                        <asp:DropDownList ID="drpFontsize" runat="server" Width="40px">
                                                                            <asp:ListItem Selected="True" Text="8" />
                                                                            <asp:ListItem Text="9" />
                                                                            <asp:ListItem Text="10" />
                                                                            <asp:ListItem Text="11" />
                                                                            <asp:ListItem Text="12" />
                                                                            <asp:ListItem Text="13" />
                                                                            <asp:ListItem Text="14" />
                                                                            <asp:ListItem Text="15" />
                                                                            <asp:ListItem Text="16" />
                                                                            <asp:ListItem Text="17" />
                                                                            <asp:ListItem Text="18" />
                                                                            <asp:ListItem Text="19" />
                                                                            <asp:ListItem Text="20" />
                                                                            <asp:ListItem Text="21" />
                                                                            <asp:ListItem Text="22" />
                                                                            <asp:ListItem Text="23" />
                                                                            <asp:ListItem Text="24" />
                                                                            <asp:ListItem Text="25" />
                                                                            <asp:ListItem Text="26" />
                                                                            <asp:ListItem Text="27" />
                                                                            <asp:ListItem Text="28" />
                                                                            <asp:ListItem Text="29" />
                                                                            <asp:ListItem Text="30" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="Legend" nowrap="nowrap" runat="server" id="tdName6" visible="false">
                                                                        Style
                                                                    </td>
                                                                    <td runat="server" id="tdName7" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td nowrap="nowrap" runat="server" id="tdName8" visible="false">
                                                                        <asp:CheckBox ID="chkNameBold" Text=" B" runat="server" CssClass="TDCaption" />
                                                                        <asp:CheckBox ID="chkNameItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                                                        <asp:CheckBox ID="chkNameUL" Text=" U" runat="server" CssClass="TDCaption" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr3" runat="server">
                                                                    <td id="Td10" class="Legend" runat="server">
                                                                        Address
                                                                    </td>
                                                                    <td id="Td11" class="style1" runat="server">
                                                                        :
                                                                    </td>
                                                                    <td id="Td12" runat="server">
                                                                        <asp:TextBox ID="txtLogoAddress" runat="server" Width="150px" MaxLength="100" Rows="2"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                    <td class="Legend" runat="server" id="tdName9" visible="false">
                                                                        Name
                                                                    </td>
                                                                    <td runat="server" id="tdName10" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td runat="server" id="tdName11" visible="false">
                                                                        <asp:DropDownList ID="drpAFontName" runat="server" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="Legend" runat="server" id="tdName12" visible="false">
                                                                        Size
                                                                    </td>
                                                                    <td runat="server" id="tdName13" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td runat="server" id="tdName14" visible="false">
                                                                        <asp:DropDownList ID="drpAFontSize" runat="server" Width="40px">
                                                                            <asp:ListItem Selected="True" Text="8" />
                                                                            <asp:ListItem Text="9" />
                                                                            <asp:ListItem Text="10" />
                                                                            <asp:ListItem Text="11" />
                                                                            <asp:ListItem Text="12" />
                                                                            <asp:ListItem Text="13" />
                                                                            <asp:ListItem Text="14" />
                                                                            <asp:ListItem Text="15" />
                                                                            <asp:ListItem Text="16" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="Legend" runat="server" id="tdName15" visible="false">
                                                                        Style
                                                                    </td>
                                                                    <td runat="server" id="tdName16" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td nowrap="nowrap" runat="server" id="tdName17" visible="false">
                                                                        <asp:CheckBox ID="chkAddressBold" Text=" B" runat="server" CssClass="TDCaption" />
                                                                        <asp:CheckBox ID="chkAddressItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                                                        <asp:CheckBox ID="chkAddressUL" Text=" U" runat="server" CssClass="TDCaption" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr4" runat="server">
                                                                    <td id="Td13" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td14" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td15" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td16" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td17" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td18" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td19" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td20" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td21" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td22" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td23" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td24" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="tdName42" visible="false">
                                                                    <td id="Td25" class="Legend" colspan="12" style="background-color: #C0C0C0" runat="server">
                                                                        LOGO
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr5" runat="server">
                                                                    <td class="Legend" runat="server" id="tdName36" visible="false">
                                                                        Logo
                                                                    </td>
                                                                    <td runat="server" id="tdName37" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td runat="server" id="tdName38" visible="false">
                                                                        <input id="fupLogo" runat="server" onchange="return fupLogo_onchange();" size="10"
                                                                            type="file" />
                                                                    </td>
                                                                    <td class="Legend" runat="server" id="tdName39" visible="false">
                                                                        Align
                                                                    </td>
                                                                    <td runat="server" id="tdName40" visible="false">
                                                                        :
                                                                    </td>
                                                                    <td runat="server" id="tdName41" visible="false">
                                                                        <asp:RadioButton ID="rdrLAlign" runat="server" Checked="True" CssClass="Legend" GroupName="b"
                                                                            Text="Left" />&#160;&#160;
                                                                        <asp:RadioButton ID="rdrRAlign" runat="server" CssClass="Legend" GroupName="b" Text="Right" />
                                                                    </td>
                                                                    <td class="Legend" nowrap="nowrap">
                                                                        Currency
                                                                    </td>
                                                                    <td class="TDDot">
                                                                        :
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:TextBox ID="txtCurrencyType" runat="server" CssClass="Textbox" MaxLength="50"
                                                                            Width="50Px"></asp:TextBox>
                                                                        <asp:DropDownList ID="drpLogoHeight" runat="server" Width="40px" Visible="False">
                                                                            <asp:ListItem Text="1" />
                                                                            <asp:ListItem Text="2" />
                                                                            <asp:ListItem Text="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="Legend" nowrap="nowrap">
                                                                    </td>
                                                                    <td class="TDDot">
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:DropDownList ID="txtLogoWeirht" runat="server" Width="40px" Visible="False">
                                                                            <asp:ListItem Text="1" />
                                                                            <asp:ListItem Text="2" />
                                                                            <asp:ListItem Text="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="tdName43" visible="false">
                                                                    <td id="Td26" class="TDCaption" colspan="12" runat="server">
                                                                        Please Upload a 100 X 100 Pixels Image. Allowed File Type are Jpg, Jpeg, Gif, Ico
                                                                        and Png. &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                                                        &nbsp;&nbsp; &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="Tr6" runat="server">
                                                                    <td id="Td27" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td28" class="style1" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td29" runat="server">
                                                                    </td>
                                                                    <td id="Td30" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td31" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td32" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td33" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td34" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td35" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td36" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td37" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                    <td id="Td38" runat="server">
                                                                        &#160;&nbsp;
                                                                    </td>
                                                                </tr>
                                                        </td>
                                                    </tr>
                                                </table>
                                </table>
                                </fieldset></td></tr></table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel HeaderText="Header Slogan" ID="HSlogan" runat="server">
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend">Header Slogan</legend>
                                    <table>
                                        <tr>
                                            <td class="Legend" colspan="12" style="background-color: #C0C0C0">
                                                BOOKING RECEIPT
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend" nowrap="nowrap">
                                                Label
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSloganName" runat="server" Width="150px" MaxLength="100" Rows="2"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td class="Legend" nowrap="nowrap" runat="server" visible="false" id="tdName18">
                                                Font
                                            </td>
                                            <td runat="server" visible="false" id="tdName19">
                                                :
                                            </td>
                                            <td runat="server" visible="false" id="tdName20">
                                                <asp:DropDownList ID="drpHeaderFontName" runat="server" Width="170px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Legend" nowrap="nowrap" runat="server" visible="false" id="tdName21">
                                                Size
                                            </td>
                                            <td runat="server" visible="false" id="tdName22">
                                                :
                                            </td>
                                            <td runat="server" visible="false" id="tdName35">
                                                <asp:DropDownList ID="drpHeaderFontSize" runat="server" Width="40px">
                                                    <asp:ListItem Selected="True" Text="8" />
                                                    <asp:ListItem Text="9" />
                                                    <asp:ListItem Text="10" />
                                                    <asp:ListItem Text="11" />
                                                    <asp:ListItem Text="12" />
                                                    <asp:ListItem Text="13" />
                                                    <asp:ListItem Text="14" />
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Legend" nowrap="nowrap" runat="server" visible="false" id="tdName23">
                                                Style
                                            </td>
                                            <td runat="server" visible="false" id="tdName24">
                                                :
                                            </td>
                                            <td nowrap="nowrap" runat="server" visible="false" id="tdName25">
                                                <asp:CheckBox ID="chkHeaderBold" Text=" B" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkHeaderItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkHeaderUL" Text=" U" runat="server" CssClass="TDCaption" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="12" style="height: 10Px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend" colspan="12" style="background-color: #C0C0C0">
                                                DELIVERY RECEIPT
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend" nowrap="nowrap">
                                                Label
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFooterSloganName" runat="server" Width="150px" MaxLength="100"
                                                    Rows="2" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td class="Legend" nowrap="nowrap" runat="server" visible="false" id="tdName26">
                                                Font
                                            </td>
                                            <td runat="server" visible="false" id="tdName27">
                                                :
                                            </td>
                                            <td runat="server" visible="false" id="tdName28">
                                                <asp:DropDownList ID="drpFooterFontName" runat="server" Width="170px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Legend" nowrap="nowrap" runat="server" visible="false" id="tdName29">
                                                Size
                                            </td>
                                            <td runat="server" visible="false" id="tdName30">
                                                :
                                            </td>
                                            <td runat="server" visible="false" id="tdName31">
                                                <asp:DropDownList ID="drpFooterFontSize" runat="server" Width="40px">
                                                    <asp:ListItem Selected="True" Text="8" />
                                                    <asp:ListItem Text="9" />
                                                    <asp:ListItem Text="10" />
                                                    <asp:ListItem Text="11" />
                                                    <asp:ListItem Text="12" />
                                                    <asp:ListItem Text="13" />
                                                    <asp:ListItem Text="14" />
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Legend" nowrap="nowrap" runat="server" visible="false" id="tdName32">
                                                Style
                                            </td>
                                            <td runat="server" visible="false" id="tdName33">
                                                :
                                            </td>
                                            <td nowrap="nowrap" runat="server" visible="false" id="tdName34">
                                                <asp:CheckBox ID="chkFooterBold" Text=" B" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkFooterItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                                <asp:CheckBox ID="chkFooterUL" Text=" U" runat="server" CssClass="TDCaption" />
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false" id="tdName100">
                                            <td class="Legend" colspan="12" style="background-color: #C0C0C0">
                                                Left Message
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false" id="tdName101">
                                            <td class="Legend" nowrap="nowrap">
                                                Label
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtleftMsg" runat="server" Width="150px" MaxLength="100" Rows="2"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false" id="tdName102">
                                            <td class="Legend" colspan="12" style="background-color: #C0C0C0">
                                                Right Message
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false" id="tdName103">
                                            <td class="Legend" nowrap="nowrap">
                                                Label
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRightMsg" runat="server" Width="150px" MaxLength="100" Rows="2"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel HeaderText="Term & Condition" ID="Term" runat="server">
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend">Term & Condition</legend>
                                    <table>
                                        <tr>
                                            <td class="Legend">
                                                1
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms1" runat="server" Width="500px" MaxLength="115"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend">
                                                2
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms2" runat="server" CssClass="Textbox" Width="500px" MaxLength="115"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Legend">
                                                3
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms3" runat="server" CssClass="Textbox" Width="500px" MaxLength="115"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms4" visible="false">
                                            <td class="Legend">
                                                4
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms4" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms5" visible="false">
                                            <td class="Legend">
                                                5
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms5" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms6" visible="false">
                                            <td class="Legend">
                                                6
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms6" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms7" visible="false">
                                            <td class="Legend">
                                                7
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms7" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms8" visible="false">
                                            <td class="Legend">
                                                8
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms8" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms9" visible="false">
                                            <td class="Legend">
                                                9
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms9" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms10" visible="false">
                                            <td class="Legend">
                                                10
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms10" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms11" visible="false">
                                            <td class="Legend">
                                                11
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms11" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms12" visible="false">
                                            <td class="Legend">
                                                12
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms12" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms13" visible="false">
                                            <td class="Legend">
                                                13
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms13" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms14" visible="false">
                                            <td class="Legend">
                                                14
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms14" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="terms15" visible="false">
                                            <td class="Legend">
                                                15
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTerms15" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &#160;&nbsp;
                                            </td>
                                            <td>
                                                &#160;&nbsp;
                                            </td>
                                            <td>
                                                &#160;&nbsp;
                                            </td>
                                            <td>
                                                &#160;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <%-- <cc1:TabPanel HeaderText="Preview" ID="TabPanel2" runat="server">
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend">Preview</legend>
                                    <table>
                                        <tr>
                                            <td class="Legend">
                                                <marquee>Under Construction</marquee>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>--%>
                    </cc1:TabContainer>
                </td>
                <td id="tdDisplay" runat="server" visible="false">
                    <cc1:TabContainer ID="tbDisplay" runat="server">
                        <cc1:TabPanel ID="pnlDisplay" runat="server" HeaderText="Display Setting">
                            <HeaderTemplate>
                                Store Information
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblErr" runat="server" CssClass="ErrorMessage" EnableViewState="False" /><asp:Label
                                                    ID="lblSuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr style="display: none;">
                                                        <td class="TDCaption">
                                                            Website Address
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtWebsiteName" runat="server" MaxLength="100" TabIndex="1"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Store Name
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStoreName" runat="server" MaxLength="100" TabIndex="2" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Address
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="100" TabIndex="3" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Store Timing
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTiming" runat="server" MaxLength="100" TabIndex="4" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Business Slogan
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFooterName" runat="server" MaxLength="100" TabIndex="6" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSlip" runat="server" visible="False">
                                                        <td id="Td39" class="TDCaption" runat="server">
                                                            Set&#160;Slip &#160;Inches
                                                        </td>
                                                        <td id="Td40" class="TDDot" runat="server">
                                                            :
                                                        </td>
                                                        <td id="Td41" runat="server">
                                                            <asp:TextBox ID="txtSSlipInch" runat="server" MaxLength="2" TabIndex="7"></asp:TextBox><asp:RangeValidator
                                                                ID="rv" runat="server" ControlToValidate="txtSSlipInch" ErrorMessage="Please Select Inches Between 0-12"
                                                                Type="Integer" MaximumValue="12" MinimumValue="0"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td class="TDDot">
                                                        </td>
                                                        <td>
                                                            <img src="~/Logo/DRY.jpg" id="abc" runat="server" style="width: 60px; height: 60px;" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Screen and Tags Logo
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <input id="fupStudentPhoto" runat="server" onchange="return fupStudentPhoto_onchange();"
                                                                size="10" tabindex="8" type="file" width="300px" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:CheckBox ID="chkImage" runat="server" Text=" Print Logo On Receipt" TabIndex="7"
                                                                Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption" colspan="3">
                                                            Please Upload a 60 X 60 Pixels Image. Allowed File Type are Jpg, Jpeg, Gif and Png.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &#160;&#160;
                                                        </td>
                                                        <td width="2px">
                                                            &#160;&#160;
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSetting" runat="server" Text=" Display this screen on start up. "
                                                                TabIndex="9" Visible="False" />
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
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="Button1" runat="server" Text="Save" TabIndex="10" OnClick="btnSave_Click"
                                                                            CausesValidation="False" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" TabIndex="11" OnClick="BtnCancel_Click"
                                                                            CausesValidation="False" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdDefaultSetting" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer1" runat="server">
                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Default Setting For New Booking">
                            <HeaderTemplate>
                                Default Setting For Pick Up/New Order
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblDefaultErr" runat="server" CssClass="ErrorMessage" EnableViewState="False" /><asp:Label
                                                    ID="lblDefaultSucess" runat="server" CssClass="SuccessMessage" EnableViewState="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Process
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpMainProcesses" runat="server" Width="150px" TabIndex="1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Item
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpItems" runat="server" Width="150px" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" visible="false">
                                                        <td class="TDCaption">
                                                            Color
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpColorName" runat="server" Width="150px" TabIndex="3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Delivery Time
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="txtDefaultTime" runat="server" TabIndex="4" Width="100px">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="drpAMPM" runat="server" AppendDataBoundItems="True" TabIndex="5">
                                                                <asp:ListItem Text="AM"></asp:ListItem>
                                                                <asp:ListItem Text="PM"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Delivery Date Offset
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDefaultDateSet" runat="server" MaxLength="2" TabIndex="6" Width="147px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                ID="txtDefaultDateSet_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                FilterType="Numbers" TargetControlID="txtDefaultDateSet">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Start Booking Number From
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStartBookingNo" runat="server" MaxLength="5" TabIndex="7" Width="147px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                ID="txtStartBookingNo_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                FilterType="Numbers" TargetControlID="txtStartBookingNo">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Urgent # 1
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUrgunt1" runat="server" MaxLength="50" TabIndex="7" Width="147px"></asp:TextBox>
                                                            <cc1:TextBoxWatermarkExtender ID="txtUrgunt1_TextBoxWatermarkExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtUrgunt1" WatermarkText="Label1">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Rate
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUrgunt1Rate" runat="server" MaxLength="3" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <span class="span">%</span>
                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt1Rate_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtUrgunt1Rate" ValidChars="1234567890">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td width="30Px">
                                                        </td>
                                                        <td class="TDCaption">
                                                            Day Offset
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUrgunt1Day" runat="server" MaxLength="2" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt1Day_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtUrgunt1Day">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            &nbsp;Urgent # 2
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUrgunt2" runat="server" MaxLength="50" TabIndex="7" Width="147px"></asp:TextBox>
                                                            <cc1:TextBoxWatermarkExtender ID="txtUrgunt2_TextBoxWatermarkExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtUrgunt2" WatermarkText="Label2">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Rate
                                                        </td>
                                                        <td>
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUrgunt2Rate" runat="server" MaxLength="3" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <span class="span">%</span>
                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt2Rate_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtUrgunt2Rate" ValidChars="1234567890">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td width="30Px">
                                                        </td>
                                                        <td class="TDCaption">
                                                            Day Offset
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUrgunt2Day" runat="server" MaxLength="2" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt2Day_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtUrgunt2Day">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" visible="false">
                                                        <td class="TDCaption">
                                                            Default Customer Search Criteria
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbName" runat="server" Text="Name" CssClass="Legend" GroupName="default"
                                                                Checked="True" TabIndex="8" /><asp:RadioButton ID="rdbAddress" runat="server" Text="Address"
                                                                    CssClass="Legend" GroupName="default" TabIndex="9" /><asp:RadioButton ID="rdbMobileNo"
                                                                        runat="server" Text="Mobile No" CssClass="Legend" GroupName="default" TabIndex="10" />
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td class="TDDot">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Net Amount
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbFlat" runat="server" Text=" Rounded" GroupName="NetAmount"
                                                                Checked="True" CssClass="Legend" ToolTip="Amount display like e.g(100)" />
                                                            <asp:RadioButton ID="rdbFloat" runat="server" Text=" In Decimal" GroupName="NetAmount"
                                                                CssClass="Legend" ToolTip="Amount display like e.g(100.00)" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Activate Description Field
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbEnterRemarkTrue" runat="server" Text=" Yes" GroupName="POPO"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbEnterRemarkFalse" runat="server" Text=" No" GroupName="POPO"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Activate Color Field
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbEnterColorTrue" runat="server" Text=" Yes" GroupName="SDFD"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbEnterColorFalse" runat="server" Text=" No" GroupName="SDFD"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Only allow color from Master
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbBindColorToMasterTrue" runat="server" Text=" Yes" GroupName="RED"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbBindColorToMasterFalse" runat="server" Text=" No" GroupName="RED"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Only allow Description from Master
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbBindToDescriptionTrue" runat="server" Text=" Yes" GroupName="ED"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbBindToDescriptionFalse" runat="server" Text=" No" GroupName="ED"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Default Qty
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpSetQty" runat="server" Width="50px">
                                                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Default Discount Type
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpDefaultDiscountType" runat="server">
                                                                <asp:ListItem Text="Percentage"></asp:ListItem>
                                                                <asp:ListItem Text="Flat"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Confirm Delivery Date
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbConfirmDateTrue" runat="server" Text=" Yes" GroupName="QQQ"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbConfirmDateFalse" runat="server" Text=" No" GroupName="QQQ"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td class="TDCaption">
                                                            Save/Update Rate in Price List (for next time use)
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbSaveRateInItemMasterTrue" runat="server" Text=" Yes" GroupName="CFC"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbSaveRateInItemMasterFalse" runat="server" Text=" No" GroupName="CFC"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Make Remarks Mandatory in case of Edit Booking
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbSaveEditRemarksTrue" runat="server" Text=" Yes" GroupName="SERS"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbSaveEditRemarksFalse" runat="server" Text=" No" GroupName="SERS"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Put capping on colors, that can be entered, as per the quantity booked
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbBindColorQtyTrue" runat="server" Text=" Yes" GroupName="MCD"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbBindColorQtyFalse" runat="server" Text=" No" GroupName="MCD"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Customer Search
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpDefaultCustomerSearch" runat="server">
                                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                <asp:ListItem Text="Name" Value="Name"></asp:ListItem>
                                                                <asp:ListItem Text="Address" Value="Address"></asp:ListItem>
                                                                <asp:ListItem Text="Mobile" Value="Mobile"></asp:ListItem>
                                                                <asp:ListItem Text="Membership Id" Value="MembershipId"></asp:ListItem>
                                                                <asp:ListItem Text="Customer Code" Value="CustCode"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td width="2px">
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnDefault" runat="server" CausesValidation="False" TabIndex="11"
                                                                            Text="Save" OnClick="btnDefault_Click" OnClientClick="return checkName();" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnReset" runat="server" CausesValidation="False" TabIndex="12" Text="Reset"
                                                                            OnClick="btnReset_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdSetTimeZone" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer2" runat="server">
                        <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Display Setting">
                            <HeaderTemplate>
                                Set Time Zone
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblSetTimeZoneErr" runat="server" CssClass="ErrorMessage" EnableViewState="False" />
                                                <asp:Label ID="lblSetTimeZoneSuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Time Zone
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpZoneList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpZoneList_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Date
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSetTimeZoneSucess" runat="server" CssClass="Legend" EnableViewState="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnZoneSave" runat="server" Text="Save" TabIndex="10" CausesValidation="False"
                                                                            OnClick="btnZoneSave_Click" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdGeneralSetting" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer3" runat="server">
                        <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Display Setting">
                            <HeaderTemplate>
                                General Setting
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="generalError" runat="server" CssClass="ErrorMessage" EnableViewState="False" /><asp:Label
                                                    ID="generalSuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Challan Type
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpChallanType" runat="server">
                                                                <asp:ListItem Text="Invoice Based Detailed"></asp:ListItem>
                                                                <asp:ListItem Text="Invoice Based"></asp:ListItem>
                                                                <asp:ListItem Text="Invoice With Item Detailed"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            Please Choose Options below, if you want to <span style="font-size: 14px; font-weight: bold">
                                                                Disable Move All </span>and <span style="font-size: 14px; font-weight: bold">Force use
                                                                    of Bar Code</span> on Following Screens.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="20Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Send To WorkShop
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbSendToWorkShopTrue" runat="server" Text=" Yes" GroupName="SST"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbSendToWorkShopFalse" runat="server" Text=" No" GroupName="SST"
                                                                CssClass="Legend" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Receive From WorkShop
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbReceiveTrue" runat="server" Text=" Yes" GroupName="RFT" Checked="True"
                                                                CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbReceiveFalse" runat="server" Text=" No" GroupName="RFT" CssClass="Legend" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Mark For Delivery
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbMarkDeliveryTrue" runat="server" Text=" Yes" GroupName="MDT"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbMarkDeliveryFalse" runat="server" Text=" No" GroupName="MDT"
                                                                CssClass="Legend" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Force User Pin Barcode On Mark Ready Screen
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbWorkRecieveTrue" runat="server" Text=" Yes" GroupName="WRT"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbWorkRecieveFalse" runat="server" Text=" No" GroupName="WRT"
                                                                CssClass="Legend" />
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td class="TDCaption">
                                                            WorkShop Ready
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbWorkReadyTrue" runat="server" Text=" Yes" GroupName="WWRT"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbWorkReadyFalse" runat="server" Text=" No" GroupName="WWRT"
                                                                CssClass="Legend" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnGeneralSetting1" runat="server" Text="Save" CausesValidation="False"
                                                                            OnClick="btnGeneralSetting1_Click" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdEmail" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer4" runat="server">
                        <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText="Display Setting">
                            <HeaderTemplate>
                                Email Setting
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblEmailSuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False" />
                                                <asp:Label ID="lblEmailError" runat="server" CssClass="ErrorMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Server Name
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHostName" runat="server" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Enable SSL
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSSL" runat="server" CssClass="TDCaption" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Email
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranchEmail" runat="server" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Password
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranchPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:CheckBox ID="chkmailActive" runat="server" CssClass="TDCaption" OnCheckedChanged="chkmailActive_CheckChanged" AutoPostBack="true"  />
                                                            <span class="Legend">Daily status mail/Backup will be sent on your email</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtStatusEmailID" runat="server" Width="200px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnEmail" runat="server" Text="Save" CausesValidation="False" OnClick="btnEmail_Click" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdbackup" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer7" runat="server">
                        <cc1:TabPanel ID="TabPanel8" runat="server" HeaderText="Backup Path Setting">
                            <HeaderTemplate>
                                Database Backup Path Setting
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbpatherror" runat="server" CssClass="ErrorMessage" EnableViewState="False" /><asp:Label
                                                    ID="lbpathsucess" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Choose Database Backup Path
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Drpdrive" runat="server">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <%-- <asp:DropDownList ID="Drpfloder" runat="server">
                                                            </asp:DropDownList>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btbackupsave" runat="server" Text="Save" CausesValidation="False"
                                                                            OnClick="btbackupsave_Click" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdServiceTax" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer5" runat="server">
                        <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText="Default Setting For Service Tax">
                            <HeaderTemplate>
                                Default Tax Rates
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                                <span>Default Tax: </span><span style="font-size: 12px">Default tax on Invoice Amount
                                                    for applicable Services</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                (Different Services can have different tax rates <a href="../ProcessMaster.aspx">Click
                                                    Here</a> to update Tax Amount for each Service)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10Px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblErrServiceTax" runat="server" CssClass="ErrorMessage" EnableViewState="False" /><asp:Label
                                                    ID="lblSucessServiceTax" runat="server" CssClass="SuccessMessage" EnableViewState="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Tax Label # 1
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceTaxText1" runat="server" MaxLength="50" TabIndex="7" Width="147px"></asp:TextBox>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Tax Value
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceTaxRate1" runat="server" MaxLength="5" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <span class="span">%</span>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtServiceTaxRate1" ValidChars="1234567890.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td width="30Px">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td class="TDDot">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="10" style="height: 10Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="10">
                                                            Default Tax on Tax: On Tax amount calculated on Invoiced Amount
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="10">
                                                            (Different Services can have different tax on tax rates <a href="../ProcessMaster.aspx">
                                                                Click Here</a> to update Tax on tax Amount for each Service)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="10" style="height: 10Px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            &nbsp;Tax Label # 2
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceText2" runat="server" MaxLength="50" TabIndex="7" Width="147px"></asp:TextBox>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Tax Value
                                                        </td>
                                                        <td>
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceTaxRate2" runat="server" MaxLength="5" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <span class="span">%</span>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                TargetControlID="txtServiceTaxRate2" ValidChars="1234567890.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td width="30Px">
                                                        </td>
                                                        <td class="TDCaption">
                                                        </td>
                                                        <td class="TDDot">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            &nbsp;Tax Label # 3
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceText3" runat="server" MaxLength="50" TabIndex="7" Width="147px"></asp:TextBox>
                                                        </td>
                                                        <td class="TDCaption">
                                                            Tax Value
                                                        </td>
                                                        <td>
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceTaxRate3" runat="server" MaxLength="5" TabIndex="7" Width="70px"></asp:TextBox>
                                                            <span class="span">%</span>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                TargetControlID="txtServiceTaxRate3" ValidChars="1234567890.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td width="30Px">
                                                        </td>
                                                        <td class="TDCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Tax Applicable
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbTaxBefore" runat="server" Text="Before Discount" GroupName="Tax"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbTaxAfter" runat="server" Text="After Discount" GroupName="Tax"
                                                                CssClass="Legend" ClientIDMode="Static" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Tax Calculation
                                                        </td>
                                                        <td class="TDDot">
                                                            &nbsp;:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" AppendDataBoundItems="True" ID="drpServiceTaxType"
                                                                ClientIDMode="Static">
                                                                <asp:ListItem Text="Inclusive"></asp:ListItem>
                                                                <asp:ListItem Text="Exclusive"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="9" align="center">
                                                            Note: <span style="font-size: 12px">If you are not sure of Tax Rates consult your Accountant
                                                                / Tax Attorney</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td width="2px">
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                            &#160;&nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnServiceTaxSave" runat="server" CausesValidation="False" TabIndex="11"
                                                                            Text="Save" OnClick="btnServiceTaxSave_Click" ClientIDMode="Static" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdPassword" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer6" runat="server">
                        <cc1:TabPanel ID="TabPanel7" runat="server" HeaderText="Password Controls">
                            <HeaderTemplate>
                                Password Controls
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <legend class="Legend"></legend>
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblPasswordErr" runat="server" CssClass="ErrorMessage" EnableViewState="False" /><asp:Label
                                                    ID="lblPasswordSucess" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Do you want Item Creation from Booking Screen to be controlled by Password
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbItemCreationTrue" runat="server" Text=" Yes" GroupName="PWD"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbItemCreationFalse" runat="server" Text=" No" GroupName="PWD"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPasswordItemCreation" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Do you want Price Change on Booking Screen to be controlled by Password
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbRateChangeTrue" runat="server" Text=" Yes" GroupName="Rate"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbRateChangeFalse" runat="server" Text=" No" GroupName="Rate"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRateChange" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Do you want Discount on Booking to be controlled by Password
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbDiscountChangeTrue" runat="server" Text=" Yes" GroupName="Discount"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbDiscountChangeFalse" runat="server" Text=" No" GroupName="Discount"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscountChange" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Do you want Discount on Delivery to be controlled by Password
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbDiscountDelChangeTrue" runat="server" Text=" Yes" GroupName="DiscountDel"
                                                                Checked="True" CssClass="Legend" />
                                                            <asp:RadioButton ID="rdbDiscountDelChangeFalse" runat="server" Text=" No" GroupName="DiscountDel"
                                                                CssClass="Legend" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscountDelChange" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnPasswordSave" runat="server" Text="Save" CausesValidation="False"
                                                                            OnClick="btnPasswordSave_Click" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnPassword" runat="server" />
    <asp:SqlDataSource ID="SDTItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [ItemName], [ItemID] FROM [ItemMaster] order by ItemName asc">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SDTColors" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT ID, ColorName FROM mstcolor where colorName!='/' order by colorName asc">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [ProcessCode], [ProcessName] FROM [ProcessMaster] order by ProcessName asc">
    </asp:SqlDataSource>
    <asp:HiddenField ID="hdnPrint" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnValue" runat="server" ClientIDMode="Static" />
</asp:Content>
