<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    EnableEventValidation="False" CodeBehind="BarCodeSetting.aspx.cs" Inherits="QuickWeb.Config_Setting.BarCodeSetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/BarCodeSetting.css" rel="stylesheet" type="text/css" media="screen" />
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('.divTestConfigBarcode').css('font-family', '"Code 128"');

            $('#chkWet').click(function () {

                if ($('#chkWet').attr('checked')) {

                    $('#drpWidth, #drpHeight, #lblWidth,#lblHeight').hide();

                    $("#drpWidth option[value='17']").attr('selected', 'selected');
                    $("#drpHeight option[value='2']").attr('selected', 'selected');
                    $('#undersubitem').hide();
                    $('#undervariation').hide();

                }
                else {
                    $('#drpWidth, #drpHeight, #lblWidth,#lblHeight').show();
                    $('#undersubitem').show();
                    $('#undervariation').show();
                }
            });

            if ($('#chkWet').attr('checked')) {
                $('#drpWidth, #drpHeight, #lblWidth,#lblHeight').hide();
                $('#undersubitem').hide();

            }

            $('#btnSave').click(function () {
                $('#undervariation').show();
            });
            if ($('#<%=drpPrinterlist.ClientID %>').val() != '') {

                $('#<%=hdnPrint.ClientID %>').val($('#<%=drpPrinterlist.ClientID %>').val());
            }

            var printers = jsPrintSetup.getPrintersList();
            var prt = printers.split(',');
            var printValue = $('#hdnValue').val();
            $('#<%=drpPrinterlist.ClientID %>').append($("<option" + ">" + printValue + "</option>"));
            for (var i = 0; i < prt.length; i++) {

                if (prt[i] == printValue) {
                    continue;
                }
                else {
                    $('#<%=drpPrinterlist.ClientID %>').append($("<option " + ">" + prt[i] + "</option>"));
                }
            }

            $('#<%=drpPrinterlist.ClientID %>').change(function () {

                $('#<%=hdnPrint.ClientID %>').val($('#<%=drpPrinterlist.ClientID %>').val())
            });
            var search = $('#<%=txtShopName.ClientID %>');

            $('#btnPreview, #btnSave').click(function (e) {

                $('#<%=hdnPrint.ClientID %>').val($('#<%=drpPrinterlist.ClientID %>').val());
                return true;
            });

            search.focus(function () {
                if (search.val() == this.title) {
                    search.val("");
                }

            });
            search.blur(function () {
                if (search.val() == "") {
                    search.css(
                    {
                        'font-style': 'italic',
                        'color': '#cccccc'
                    });
                    search.val(this.title);

                }
            });

            search.blur();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main">
        <div id="NewItemgroup">
            <ul id="unorderlist">
                <li>
                    <label id="lblWidth" for="txtWidth">
                        Width :</label>
                    <asp:DropDownList ID="drpWidth" runat="server" Width="60px" ClientIDMode="Static">
                        <asp:ListItem Value="0">0.9in</asp:ListItem>
                        <asp:ListItem Value="1">1.0in</asp:ListItem>
                        <asp:ListItem Value="2">1.1in</asp:ListItem>
                        <asp:ListItem Value="3">1.2in</asp:ListItem>
                        <asp:ListItem Value="4">1.3in</asp:ListItem>
                        <asp:ListItem Value="5">1.4in</asp:ListItem>
                        <asp:ListItem Value="6">1.5in</asp:ListItem>
                        <asp:ListItem Value="7">1.6in</asp:ListItem>
                        <asp:ListItem Value="8">1.7in</asp:ListItem>
                        <asp:ListItem Value="9">1.8in</asp:ListItem>
                        <asp:ListItem Value="10">1.9in</asp:ListItem>
                        <asp:ListItem Value="11">2.0in</asp:ListItem>
                        <asp:ListItem Value="12">2.1in</asp:ListItem>
                        <asp:ListItem Value="13">2.2in</asp:ListItem>
                        <asp:ListItem Value="14">2.3in</asp:ListItem>
                        <asp:ListItem Value="15">2.4in</asp:ListItem>
                        <asp:ListItem Value="16">2.5in</asp:ListItem>
                        <asp:ListItem Value="17">3.0in</asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <label id="lblHeight" for="txtHeight">
                        Height :
                    </label>
                    <asp:DropDownList ID="drpHeight" runat="server" Width="60px" ClientIDMode="Static">
                        <asp:ListItem Value="0">0.8in</asp:ListItem>
                        <asp:ListItem Value="1">0.9in</asp:ListItem>
                        <asp:ListItem Value="2">1.0in</asp:ListItem>
                        <asp:ListItem Value="3">1.1in</asp:ListItem>
                        <asp:ListItem Value="4">1.2in</asp:ListItem>
                        <asp:ListItem Value="5">1.3in</asp:ListItem>
                        <asp:ListItem Value="6">1.4in</asp:ListItem>
                        <asp:ListItem Value="7">1.5in</asp:ListItem>
                        <asp:ListItem Value="8">1.6in</asp:ListItem>
                        <asp:ListItem Value="9">1.7in</asp:ListItem>
                        <asp:ListItem Value="10">1.8in</asp:ListItem>
                        <asp:ListItem Value="11">1.9in</asp:ListItem>
                        <asp:ListItem Value="12">2.0in</asp:ListItem>
                        <asp:ListItem Value="13">2.1in</asp:ListItem>
                        <asp:ListItem Value="14">2.2in</asp:ListItem>
                        <asp:ListItem Value="15">2.3in</asp:ListItem>
                        <asp:ListItem Value="16">2.4in</asp:ListItem>
                        <asp:ListItem Value="17">2.5in</asp:ListItem>
                        <asp:ListItem Value="18">2.6in</asp:ListItem>
                        <asp:ListItem Value="19">2.7in</asp:ListItem>
                        <asp:ListItem Value="20">2.8in</asp:ListItem>
                        <asp:ListItem Value="21">2.9in</asp:ListItem>
                        <asp:ListItem Value="22">3.0in</asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <label for="txtPrinter">
                        Default Printer :</label>
                    <asp:DropDownList ID="drpPrinterlist" runat="server" Width="150px">
                    </asp:DropDownList>
                </li>
                <li>
                    <asp:CheckBox ID="chkWet" runat="server" CssClass="TDCaption" ClientIDMode="Static" />
                </li>
                <li><span>Select this for Wet Strength Paper</span> </li>
                <li>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="set" OnClick="btnSave_Click"
                        ClientIDMode="Static" />
                </li>
                 <li>
                    <asp:Button ID="btnFont" runat="server" Text="Download Font" CssClass="set" OnClick="btnFont_Click"
                        ClientIDMode="Static" />
                </li>
                <%--<li>
                    <asp:Button ID="btnSet" runat="server" Text="Set" CssClass="set" OnClick="btnSet_Click" />
                </li>--%>
            </ul>
            <div id="miscPanel">
                <div class="container3">
                    <div id="undersubitem">
                        <div class="header">
                            <div class="icol8">
                                Option</div>
                            <div class="icol9">
                                Print</div>
                            <div class="icol10">
                                Position</div>
                            <div class="icol11">
                                Font
                            </div>
                            <div class="icol12">
                                Align
                            </div>
                            <div class="icol13">
                                Size
                            </div>
                            <div class="icol14">
                                Style
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtBarCode">
                                    BarCode</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoBarcode1" runat="server" GroupName="BarCode" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoBarcode2" runat="server" GroupName="BarCode" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpBarCodePosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpBarCodeFonts" runat="server" Width="110px" AutoPostBack="true"
                                    AppendDataBoundItems="true" OnSelectedIndexChanged="drpBarCodeFonts_SelectedIndexChanged">
                                    <asp:ListItem>C39HrP36DmTt</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpBarCodeAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpBarCodeSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtBooking">
                                    Booking No.
                                </label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoBookingNo1" runat="server" GroupName="Booking" Text="Yes"
                                    Checked="true" />
                                <asp:RadioButton ID="RdoBookingNo2" runat="server" GroupName="Booking" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpBookingPosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpBookingFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpBookingAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpBookingSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkBookingBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkBookingItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkBookingUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtCustomer">
                                    Customer</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoCustomer1" runat="server" GroupName="Customer" Text="Yes"
                                    Checked="true" />
                                <asp:RadioButton ID="RdoCustomer2" runat="server" GroupName="Customer" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpCustomerPosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpCustomerFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpCustomerAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpCustomerSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkCustomerBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkCustomerItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkCustomerUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtAddress">
                                    Address</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoAddress1" runat="server" GroupName="Address" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoAddress2" runat="server" GroupName="Address" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="DrpAddressPosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpAddressFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpAddressAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpAddressSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkAddressBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkAddressItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkAddressUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtProcess">
                                    Process</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoProcess1" runat="server" GroupName="Process" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoProcess2" runat="server" GroupName="Process" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpProcessPosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpProcessFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpProcessAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpProcessSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkProcessBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkProcessItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkProcessUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtItemTotal">
                                    Item Total</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoItemTotal1" runat="server" GroupName="ItemTotal" Text="Yes"
                                    Checked="true" />
                                <asp:RadioButton ID="RdoItemTotal2" runat="server" GroupName="ItemTotal" Text="No" />
                            </div>
                            <div class="icol3">
                                &nbsp;
                            </div>
                            <div class="icol4">
                                &nbsp;
                            </div>
                            <div class="icol5">
                                &nbsp;
                            </div>
                            <div class="icol6">
                                &nbsp;
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtRemark">
                                    Remark</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoRemark1" runat="server" GroupName="Remark" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoRemark2" runat="server" GroupName="Remark" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpRemarkPosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpRemarkFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpRemarkAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpRemarkSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkRemarkBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkRemarkItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkRemarkUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtColour">
                                    Colour</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoColour1" runat="server" GroupName="Colour" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoColour2" runat="server" GroupName="Colour" Text="No" />
                            </div>
                            <div class="icol3">
                                &nbsp;
                            </div>
                            <div class="icol4">
                                &nbsp;
                            </div>
                            <div class="icol5">
                                &nbsp;
                            </div>
                            <div class="icol6">
                                &nbsp;
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtBookingDate">
                                    Booking Date</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoBDate" runat="server" GroupName="BDate" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoBDate1" runat="server" GroupName="BDate" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpBDatePosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpBDateFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpBDateAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpBDateSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkBDateBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkBDateItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkBDateUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtDueDate">
                                    Due Date</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoDueDate1" runat="server" GroupName="DueDate" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoDueDate2" runat="server" GroupName="DueDate" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpDueDatePosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpDueDateFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpDueDateAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpDueDateSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkDueDateBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkDueDateItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkDueDateUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <asp:CheckBox ID="ChkOneDay" runat="server" CssClass="TDCaption" OnCheckedChanged="ChkOneDay_CheckedChanged"
                                    AutoPostBack="true" />
                                <asp:TextBox ID="txtday" runat="server" Text="1" Width="20px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                    ValidChars="0123456789" TargetControlID="txtday">
                                </cc1:FilteredTextBoxExtender>
                                <span class="lblDeductNew">Select to bring forward/Prepone Due Date for Workshop tags
                                    only.</span>
                            </div>
                            <div class="icol2">
                                &nbsp;
                            </div>
                            <div class="icol3">
                                &nbsp;
                            </div>
                            <div class="icol4">
                                &nbsp;
                            </div>
                            <div class="icol5">
                                &nbsp;
                            </div>
                            <div class="icol6">
                                &nbsp;
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtDueTime">
                                    Time</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoDateTime1" runat="server" GroupName="DateTime" Text="Yes"
                                    Checked="true" />
                                <asp:RadioButton ID="RdoDateTime2" runat="server" GroupName="DateTime" Text="No" />
                            </div>
                            <div class="icol3">
                                &nbsp;
                            </div>
                            <div class="icol4">
                                &nbsp;
                            </div>
                            <div class="icol5">
                                &nbsp;
                            </div>
                            <div class="icol6">
                                &nbsp;
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtItemName">
                                    Item Name</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoItemName1" runat="server" GroupName="ItemName" Text="Yes"
                                    Checked="true" />
                                <asp:RadioButton ID="RdoItemName2" runat="server" GroupName="ItemName" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:DropDownList ID="drpItemNamePosition" runat="server" Width="45px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpItemNameFont" runat="server" Width="110px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpItemNameAlign" runat="server" Width="80px">
                                    <asp:ListItem>Center</asp:ListItem>
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpItemNameSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                <asp:CheckBox ID="ChkItemNameBold" Text=" B" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkItemNameItalic" Text=" I" runat="server" CssClass="TDCaption" />
                                <asp:CheckBox ID="ChkItemNameUnderline" Text=" U" runat="server" CssClass="TDCaption" />
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtShopName">
                                    Shop Name</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoShop1" runat="server" GroupName="Shop" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoShop2" runat="server" GroupName="Shop" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:TextBox ID="txtposition" runat="server" Text="Top" Enabled="False" Width="45px"></asp:TextBox>
                            </div>
                            <div class="icol4">
                                <asp:TextBox ID="txtShopName" runat="server" Width="110px" ToolTip="Shop Name"></asp:TextBox>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpShopAlign" runat="server" Width="80px">
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem>Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                <asp:DropDownList ID="drpShopSize" runat="server" Width="40px">
                                </asp:DropDownList>
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <label for="txtLogo">
                                    Logo</label>
                            </div>
                            <div class="icol2">
                                <asp:RadioButton ID="RdoLogo1" runat="server" GroupName="Logo" Text="Yes" Checked="true" />
                                <asp:RadioButton ID="RdoLogo2" runat="server" GroupName="Logo" Text="No" />
                            </div>
                            <div class="icol3">
                                <asp:TextBox ID="txtLogoPosition" runat="server" Text="Top" Enabled="false" Width="45px"></asp:TextBox>
                            </div>
                            <div class="icol4">
                                <asp:DropDownList ID="drpBlank" runat="server" Enabled="True " Width="110px" OnSelectedIndexChanged="drpBlank_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem>32X32</asp:ListItem>
                                    <asp:ListItem>Full</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol5">
                                <asp:DropDownList ID="drpLogoAlign" runat="server" Width="80px">
                                    <asp:ListItem>Left</asp:ListItem>
                                    <asp:ListItem Selected="True">Right</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="icol6">
                                &nbsp;
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                            <div class="icol1">
                                <asp:CheckBox ID="ChkAutoCuttor" runat="server" CssClass="TDCaption" AutoPostBack="true" />
                                <span class="lblPageBreak">For Auto Cutter</span>
                            </div>
                            <div class="icol2">
                                &nbsp;
                            </div>
                            <div class="icol3">
                                &nbsp;
                            </div>
                            <div class="icol4">
                                &nbsp;
                            </div>
                            <div class="icol5">
                                &nbsp;
                            </div>
                            <div class="icol6">
                                &nbsp;
                            </div>
                            <div class="icol7">
                                &nbsp;
                            </div>
                            <div class="icol7i">
                                &nbsp;
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container4">
                    <div id="undervariation">
                        <%--<div id="OriginalBarCode">
                            <span>New BarCode</span>
                            <div class="demo" style="width: <%=BarCodeWidth %>; height: <%=BarCodeHeight %>;">
                                <% if (StrPreview.Length > 0)
                                   {
                                       Response.Write(StrPreview);

                                   }
                                %>
                            </div>
                        </div>--%>
                        <%--<div id="demoBarCode">--%>
                        <span>Barcode Design</span>
                        <div class="demo1" style="width: <%=OldBarCodeWidth %>; height: <%=OldBarCodeHeight %>;">
                            <% if (strPreviewbarcode.Length > 0)
                               {
                                   Response.Write(strPreviewbarcode);

                               }
                            %>
                        </div>
                    </div>
                    <%-- </div>--%>
                    <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="btnall" OnClick="btnPreview_Click"
                        ClientIDMode="Static" Visible="false" />
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblmsg" runat="server" Style="font-weight: bold;"></asp:Label>
                        <asp:HiddenField ID="hdnPrint" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnValue" runat="server" ClientIDMode="Static" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>