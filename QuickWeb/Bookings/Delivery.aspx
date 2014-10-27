<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="Delivery" Title="Untitled Page" CodeBehind="Delivery.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <% Response.Expires = -1; %>
    <script type="text/javascript" language="javascript">
        function setPrefix() {
            var strname = document.getElementById("<%=txtBookingNumber.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter order no.");
                document.getElementById("<%=txtBookingNumber.ClientID %>").focus();
                return false;
            }
        }
        function checkKey(e) {
            var targ;
            var code;
            if (!e) var e = window.event;
            if (e.target) targ = e.target;
            if (e.keyCode) code = e.keyCode;
            else if (e.which) code = e.which;
            else if (e.srcElement) targ = e.srcElement;
            if (code == 13) {
                document.getElementById("ctl00_ContentPlaceHolder1_btnCheckGridBox").click();
            }
        }

        function checkForPackage() {
            $('#lblErr').text('');
            if (IsFirstDateBigger($('#hdnBookingDate').val(), $('#txtDeliveryDate').val())) {
                $('#lblErr').text("Delivery date can't be prior than booking date.");
                $('#txtDeliveryDate').focus();
                return false;
            }

            // check if dropdown has package in it..
            if (ctl00_ContentPlaceHolder1_dtvBookingDetails_drpPaymentType.value.indexOf('Package') == -1) return true;

            // it has package, first check if tender amt is not greater then the amt left for package
            if (parseFloat(ctl00_ContentPlaceHolder1_dtvBookingDetails_txtTenderAmt.value) > parseFloat(ctl00_ContentPlaceHolder1_lblPkgValue.textContent)) {
                alert('Tender amt can\'t be greater then remaining value of package');
                ctl00_ContentPlaceHolder1_dtvBookingDetails_txtTenderAmt.focus();
                return false;
            }

            // it has package, first check if tender amt is not greater then the amt left for package
            if (parseFloat(ctl00_ContentPlaceHolder1_dtvBookingDetails_txtPaidAmount.value) > parseFloat(ctl00_ContentPlaceHolder1_lblPkgValue.textContent)) {
                alert('Payment amt can\'t be greater then remaining value of package');
                ctl00_ContentPlaceHolder1_dtvBookingDetails_txtPaidAmount.focus();
                return false;
            }

            // if the package is expired don't let him accept the payment
            if (ctl00_ContentPlaceHolder1_dtvBookingDetails_drpPaymentType.value.indexOf('Package') != -1 && $('#hdnPackageExpired').val().split(':')[0] == 'true') {
                alert('Can\'t accept the payment from package, the packge is expired!');
                this.focus();
                return false;
            }
        }

        $(document).ready(function (e) {
            //$('#ctl00_ContentPlaceHolder1_txtBookingNumber').focus();

            $('body').not('input').click(function (ee) {
                if (ee.target.tagName != 'INPUT' && ee.target.tagName != 'SELECT' && ee.target.tagName != 'TEXTAREA') {
                    $('#ctl00_ContentPlaceHolder1_txtReadBarcode').focus();
                }
            });

            $('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtDiscountGiven').live('keypress', function (e) {
                if (e.keyCode === 9) {
                    $(this).trigger('change');
                }
            });



            $('body').on('keydown', function (e) {
                if ($('#pnlPwd').dialog('isOpen') && e.which == 9) return false;
            });

            $('#ctl00_ContentPlaceHolder1_dtvBookingDetails_lnkShowDiscount').click(function (e) {
                $.ajax({
                    url: '../Autocomplete.asmx/checkForDelDiscountPwd',
                    type: 'GET',
                    contentType: 'application/json; charset=utf8',
                    dataType: 'JSON',
                    timeout: 1000,
                    async: false,
                    success: function (response) {
                        if (response.d == '') __doPostBack('ctl00$ContentPlaceHolder1$dtvBookingDetails$lnkShowDiscount', '');
                        else {
                            if ($('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtDiscountGiven').size() != 1) VerifyPassword(response.d);
                            else $('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtDiscountGiven').focus();
                        }
                    },
                    error: function (response) { }
                });
                return false;
            });

            function VerifyPassword(password) {
                //                if (document.getElementById('txtPwd') !== null) return;
                //                var elem = document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_txtPaidAmount');
                //                var pwdElem = elem.parentNode.appendChild(elem.cloneNode());
                //                with (pwdElem) {
                //                    id = "txtPwd",
                //                   title = "Password",
                //                   value = '',
                //                   style["background"] = "yellow",
                //                   focus(),
                //                   onkeydown = function (e) {
                //                       if (e.which == 9 || e.which == 13) {
                //                           if (this.value == password) { elem.parentNode.removeChild(pwdElem); __doPostBack('ctl00$ContentPlaceHolder1$dtvBookingDetails$lnkShowDiscount', ''); }
                //                           else { alert('Wrong Password!'); this.focus(); this.select(); return false; }
                //                       }
                //                   }
                //                }
                //                return false;
                doDialog(password);
                return false;
            }
            function doDialog(password) {
                $('#pnlPwd').dialog({ width: 340, height: 280, modal: true, close: function (event, ui) { $('#btnAcceptPwd').off(); $('#btnCancelPwd').off(); $('#txtPwdForIRD').off(); $('#txtPwdForIRD').text(''); } });
                $('#txtPwdForIRD').on('keydown', function (e) { if (e.which == 13) $('#btnAcceptPwd').trigger('click'); });
                //$('#hdnPrvPwdFocus').val('txtName');
                $('#btnCancelPwd').on('click.AttachedEvent', function (e) {
                    $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; return false;
                });
                $('#btnAcceptPwd').on('click.AttachedEvent', function (e) {
                    if (document.getElementById('txtPwdForIRD').value == password) {
                        document.getElementById('lblWrongPwd').textContent = ''; __doPostBack('ctl00$ContentPlaceHolder1$dtvBookingDetails$lnkShowDiscount', '');
                    }
                    else {
                        document.getElementById('lblWrongPwd').textContent = 'Wrong Password';
                        $('#txtPwdForIRD').focus();
                        return false;
                    }
                });
            }

            $('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtDiscountGiven').keydown(function (e) {
                if (e.which == 9) $('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtDiscountGiven').trigger('change');
            });

            $('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtTenderAmt').keydown(function (event) {
                if (event.which == 9 || event.which == 13) {
                    if (document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_txtPaidAmount').value == '') return false;
                    if ((parseFloat(this.value) < parseFloat(document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_txtPaidAmount').value)) && this.value != '') {
                        alert('Tender amount can\'t be smaller then the Net Payment');
                        this.value = '';
                        document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_lblBalanceAmt').textContent = ''
                        setTimeout($(event.target).focus(), 100);
                        // return false;
                    }
                    else if (this.value != '') {
                        document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_lblBalanceAmt').textContent = parseFloat(this.value) - parseFloat(document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_txtPaidAmount').value);
                        // setTimeout($(event.target).focus(), 100);
                    }
                    else if (this.value == '') {
                        document.getElementById('ctl00_ContentPlaceHolder1_dtvBookingDetails_lblBalanceAmt').textContent = '';
                    }
                    else {
                        // return false;
                    }
                    // setTimeout($('#ctl00_ContentPlaceHolder1_dtvBookingDetails_txtTenderAmt').focus(), 100);
                    return false;
                }
            }).focus(function (ee) {
                $(this).select();
            });
        });
    </script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>    
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtDeliveryDate").change(function () {
                $('#lblErr').text('');
                if (IsFirstDateBigger($('#hdnBookingDate').val(), $('#txtDeliveryDate').val())) {
                    $('#lblErr').text("Delivery date can't be prior than booking date.");
                    $('#txtDeliveryDate').focus();
                    return false;
                }
            });
            $("#btnUpdateItemDelivery").click(function (e) {
                $('#lblErr').text('');
                if (IsFirstDateBigger($('#hdnBookingDate').val(), $('#txtDeliveryDate').val())) {
                    $('#txtDeliveryDate').focus();
                    $('#lblErr').text("Delivery date can't be prior than booking date.");                    
                    return false;
                }
            });

        });

        function IsFirstDateBigger(date1, date2) {
            var diff = 0;
            var dFst = new Date(date1);
            var dScnd;
            if (!date2)
                dScnd = new Date();
            else
                dScnd = new Date(date2);

            diff = (dFst.getFullYear() - dScnd.getFullYear()) * 365 + (dFst.getMonth() - dScnd.getMonth())

            var dFst = new Date(date1);
            var dScnd;
            if (!date2)
                dScnd = new Date();
            else
                dScnd = new Date(date2);

            if (dScnd.getFullYear() < dFst.getFullYear())
                return true;
            else if (dScnd.getFullYear() === dFst.getFullYear()) {
                if (dScnd.getMonth() < dFst.getMonth())
                    return true;
                else if (dScnd.getMonth() === dFst.getMonth()) {
                    if (dScnd.getDate() < dFst.getDate())
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="Pick Clothes/Delivery " ForeColor="#FF9933"></asp:Label>
                    <span class="" style="font-size: 12Px">Mark Clothes as Delivered After Giving Back to
                        Clients and Accept Payment </span>
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
        <table class="TableData">
            <tr>
                <td class="TDCaption">
                    Enter Receipt Number
                </td>
                <td width="300">
                    <asp:TextBox ID="txtBookingNumber" Width="75px" runat="server" MaxLength="20" ToolTip="Enter receipt/booking number here."
                        AutoPostBack="True" OnTextChanged="txtBookingNumber_TextChanged"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBookingNumber_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtBookingNumber" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                    </cc1:FilteredTextBoxExtender>
                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" Text="Show" OnClientClick="return setPrefix();" />
                    <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh"
                        Visible="False" CausesValidation="False" />
                </td>
                <td width="500">
                    <span class="TDCaption">Search Customer</span>
                    <asp:TextBox ID="txtCustomerName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                        onkeyPress="return checkKey(event);" OnTextChanged="txtCustomerName_TextChanged"
                        Width="250px" CssClass="Textbox"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                        CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                    </cc1:AutoCompleteExtender>
                </td>
            </tr>
            <tr id="DeliverSlip" runat="server" visible="false">
                <td>
                    <asp:DetailsView ID="DetailsViewDeliverSlip" runat="server" AutoGenerateRows="False"
                        Height="50px" Visible="False" CssClass="mGrid">
                        <Fields>
                            <asp:BoundField DataField="BookingNumber" HeaderText="Order No." SortExpression="BookingNumber">
                                <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Customer Name" SortExpression="CustomerName">
                                <ItemTemplate>
                                    (<asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerCode") %>'></asp:Label>)
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date"
                                SortExpression="BookingDate">
                                <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DeliveryDate" HeaderText="Due Date" SortExpression="DeliveryDate"
                                DataFormatString="{0:d}">
                                <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClothDeliverDate" HeaderText="Delivered On" SortExpression="ClothDeliverDate"
                                DataFormatString="{0:d}">
                                <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                            </asp:BoundField>
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
            <tr id="trBarcode" runat="server" visible="false">
                <td class="TDCaption">
                    Select Clothes
                </td>
                <td width="250">
                    <asp:TextBox ID="txtReadBarcode" runat="server" AutoPostBack="true" OnTextChanged="txtReadBarcode_TextChanged"></asp:TextBox>
                </td>
                <td width="250">
                    <span class="TDCaption">Delivery Date</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                        ID="txtDeliveryDate" runat="server" CssClass="Textbox" Enabled="False" ClientIDMode="Static"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtDeliveryDate_CalendarExtender" runat="server" Enabled="True"
                        Format="dd MMM yyyy" TargetControlID="txtDeliveryDate">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" ClientIDMode="Static" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                    <asp:Panel ID="Panel1" CssClass="staticPanel" runat="server" Visible="false">
                        <div class="DivStyleWithScroll" style="width: 430Px; overflow: scroll; height: 150px;">
                            <asp:GridView ID="GrdPendingClothesAndPayment" runat="server" AutoGenerateColumns="false"
                                ShowFooter="true" EmptyDataText="No records found." Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Order No." HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                OnClick="hypBtnShowDetails_Click" CausesValidation="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BookingDate" HeaderText="Order Date" SortExpression="BookingDate"
                                        HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate"
                                        HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="Balance" HeaderText="Amount" SortExpression="Balance"
                                        HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="BalQty" HeaderText="Pcs" SortExpression="BalQty" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" HeaderStyle-Wrap="false" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <cc1:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender1" TargetControlID="Panel1"
                        VerticalSide="Middle" VerticalOffset="150" HorizontalSide="Right" HorizontalOffset="2"
                        runat="server" />
                </td>
            </tr>
        </table>
        <table class="TableData">
            <tr visible="false" runat="server" id="sendsms">
                <td colspan="3">
                    <table style="width: 100%;">
                        <tr>
                            <td class="TDCaption2">
                                Send Sms&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkSendSms" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                Sms Template&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="drpsmstemplate" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPrint" runat="server" Text="Print Delivery Note with Amount" Width="250px"
                                    OnClick="btnPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPrintWithoutAmt" runat="server" Text="Print Delivery Note without Amount"
                                    Width="270px" OnClick="btnPrintWithoutAmt_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblPkgText" CssClass="TDCaptionHeader">Package Remaining Value</asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblPkgValue" runat="server" CssClass="ErrorMessage"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" width="200">
                                <asp:DetailsView ID="dtvBookingDetails" runat="server" AutoGenerateRows="False" Height="50px"
                                    Visible="False" CssClass="mGrid">
                                    <Fields>
                                        <asp:BoundField DataField="BookingNumber" HeaderText="Order Number" SortExpression="BookingNumber">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Customer Details" SortExpression="CustomerName">
                                            <ItemTemplate>
                                                (<asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerCode") %>'></asp:Label>)
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                <br></br>
                                                <asp:Label ID="lblCustomerAddress" runat="server" Text='<%# Bind("CustomerAdress") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date"
                                            SortExpression="BookingDate">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeliveryDate" HeaderText="Due Date" SortExpression="DeliveryDate"
                                            DataFormatString="{0:d}">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BookingAmount" HeaderText="TotalCost" SortExpression="BookingAmount">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Trade Discount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTradeDiscount" runat="server" Text='<%# Bind("Discount") %>'></asp:Label>%
                                                (<asp:Label ID="lblDiscountAmount" runat="server" CssClass="Legend" Text='<%# Bind("DiscountAmt") %>'></asp:Label>)
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ServiceTax" HeaderText="Tax" SortExpression="ServiceTax">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" SortExpression="NetAmount">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PaymentMade" HeaderText="Payment Made" SortExpression="PaymentMade">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DuePayment" HeaderText="Due Payment" SortExpression="DuePayment">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="New Payment">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="border-width: 0px">
                                                            <asp:TextBox ID="txtPaidAmount" runat="server" Width="50px" ToolTip="Enter Payment amount"></asp:TextBox>
                                                        </td>
                                                        <td style="border-width: 0px">
                                                            <asp:LinkButton ID="lnkShowDiscount" runat="server" Text="Discount" OnClick="lnkShowDiscount_OnClick" />
                                                        </td>
                                                        <td style="border-width: 0px">
                                                            <asp:TextBox ID="txtDiscountGiven" runat="server" Width="50px" MaxLength="7" Visible="false"
                                                                AutoPostBack="true" OnTextChanged="txtDiscountGiven_TextChanged" ClientIDMode="Static" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender122" runat="server" FilterMode="ValidChars"
                                                                ValidChars="0123456789." TargetControlID="txtDiscountGiven">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td style="border-width: 0px">
                                                            <asp:Button ID="btnAcceptPayment" runat="server" OnClick="btnAcceptPayment_Click"
                                                                OnClientClick="return checkForPackage()" Text="Accept Payment" ToolTip="Click to accept payment and update delivered items" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-width: 0px" colspan="3">
                                                        </td>
                                                        <td style="border-width: 0px">
                                                            <asp:Button ID="Button1" runat="server" Text="Accept & Print" OnClientClick="return checkForPackage()"
                                                                ToolTip="Click to accept payment and update delivered items and print receipt"
                                                                OnClick="Button1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tender Amount">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="border-width: 0px">
                                                            <asp:TextBox ID="txtTenderAmt" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender runat="server" FilterMode="ValidChars" ValidChars="0123456789."
                                                                TargetControlID="txtTenderAmt">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance Amount">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="border-width: 0px">
                                                            <asp:Label runat="server" ID="lblBalanceAmt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DiscountOnPayment" HeaderText="Discount" SortExpression="DiscountOnPayment">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Payment Type">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpPaymentType" runat="server">
                                                    <asp:ListItem Text="Cash" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Credit Card/Debit Card"></asp:ListItem>
                                                    <asp:ListItem Text="Cheque/Bank"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Details">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPaymentDetails" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtPaymentDetails_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="'" TargetControlID="txtPaymentDetails">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="WorkShopNote" HeaderText="Workshop Note" SortExpression="WorkShopNote">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderNote" HeaderText="Order Note" SortExpression="OrderNote">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                    </Fields>
                                </asp:DetailsView>
                                <span class="TDCaption">Delivery Without Slip</span>&nbsp;
                                <asp:CheckBox ID="chkWithoutSlip" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;<span
                                    class="TDCaption">Satisfied Customer</span>&nbsp;
                                <asp:CheckBox ID="chkSatisfiedCustomer" runat="server" />&nbsp;&nbsp;<asp:Label ID="lblMsgStatus"
                                    runat="server" CssClass="SuccessMessage" EnableViewState="false"></asp:Label></br>
                                <span class="TDCaption">Notes</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txtDeliveryRemarks" runat="server" Width="190px" MaxLength="300"
                                    TextMode="MultiLine"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtDeliveryRemarks_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="'" TargetControlID="txtDeliveryRemarks">
                                </cc1:FilteredTextBoxExtender>
                                <asp:Button ID="btnSaveRemarks" runat="server" Text="Save" OnClick="btnSaveRemarks_Click" />
                                <asp:SqlDataSource ID="SqlSourceBookingDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>">
                                </asp:SqlDataSource>
                                <div class="DivStyleWithScroll" style="width: 500Px; overflow: scroll; height: 100px;"
                                    runat="server" visible="false" id="divgrdNotes">
                                    <asp:GridView ID="grdNotes" runat="server" Caption="Notes Details" />
                                </div>
                                <asp:GridView ID="grdPaymentDetails" runat="server" Caption="Payment Details" />
                            </td>
                            <td>
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                    OnRowCommand="grdItemDetails_OnRowCommand" OnSelectedIndexChanged="grdItemDetails_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField FooterText="Total" HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' />
                                                <asp:HiddenField ID="hdnItemSN" runat="server" Value='<%# Bind("ISN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemTotalQuantity" HeaderText="Total Qty" SortExpression="ItemTotalQuantity" />
                                        <asp:BoundField DataField="BarCode" HeaderText="BarCode" SortExpression="BarCode" />
                                        <asp:BoundField DataField="ItemProcessType" HeaderText="Process" SortExpression="ItemProcessType" />
                                        <asp:TemplateField HeaderText="Select All">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="CheckAll" Text=" Select All" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="txtItemDelivered" runat="server" Checked='<%# Bind("DeliveredQty") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnUpdateItemDelivery" ClientIDMode="Static" runat="server" CommandName="UPDATEITEMDELIVERY"
                                                    Text="Deliver" CausesValidation="false" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ISN" HeaderText="SNo." SortExpression="ISN" />
                                        <%-- <asp:BoundField DataField="AllottedDrawl" HeaderText="Rack" SortExpression="AllottedDrawl" />--%>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("DeliverItemStaus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" SortExpression="StatusDate" />
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("ItemRemarks") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ready On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReadyOn" runat="server" Text='<%# Bind("ReadyOn") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delivery Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AcceptedUser" HeaderText="Accepted By" SortExpression="AcceptedUser" />
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="grdTmp" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                    Visible="false" OnRowCommand="grdItemDetails_OnRowCommand">
                                    <Columns>
                                        <asp:TemplateField FooterText="Total" HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' />
                                                <asp:HiddenField ID="hdnItemSN" runat="server" Value='<%# Bind("ISN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemTotalQuantity" HeaderText="Total Qty" SortExpression="ItemTotalQuantity" />
                                        <asp:BoundField DataField="ItemProcessType" HeaderText="Process" SortExpression="ItemProcessType" />
                                        <asp:BoundField DataField="ItemExtraProcessType1" HeaderText="Extra Process" SortExpression="ItemExtraProcessType1" />
                                        <asp:TemplateField HeaderText="Delivered">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="txtItemDelivered" runat="server" Checked='<%# Bind("DeliveredQty") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnUpdateItemDelivery" ClientIDMode="Static" runat="server" CommandName="UPDATEITEMDELIVERY"
                                                    Text="Update" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("DeliverItemStaus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Date" HeaderText="Delivery Date" SortExpression="Date" />
                                          <asp:BoundField DataField="BarCode" HeaderText="BarCode" SortExpression="BarCode" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlSourceItemDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="SELECT EntBookingDetails.BookingNumber, EntBookingDetails.ISN, EntBookingDetails.ItemName, ItemExtraProcessType1, EntBookingDetails.ItemTotalQuantity * ItemMaster.NumberOfSubItems As ItemTotalQuantity, EntBookingDetails.ItemProcessType FROM EntBookingDetails INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName WHERE (EntBookingDetails.BookingNumber = @BookingNumber)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtBookingNumber" DefaultValue="0" Name="BookingNumber"
                                            PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" width="200">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td nowrap="nowrap">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <input id="hdnDuePayment" runat="server" type="hidden" />
                    <asp:HiddenField ID="hdnRowIndex" runat="server" />
                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBookingDate" runat="server" ClientIDMode="Static"/>
                    <asp:Button ID="btnCheckGridBox" runat="server" OnClick="btnCheckGridBox_Click" Style="visibility: hidden" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlPwd" runat="server" Style="display: none" ClientIDMode="Static"
            BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="340px">
            <div class="popup_Titlebar" id="Div9">
                <div class="TitlebarLeft">
                    Password
                </div>
            </div>
            <div class="popup_Body">
                <table class="TableData">
                    <tr>
                        <td width="150px" align="left">
                            Enter the password:
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label10" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                            <asp:Label ID="Label11" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                            <asp:Label ID="Label13" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="Label15" runat="server" Visible="False"></asp:Label>
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
                            <table class="TableData">
                                <tr>
                                    <td class="TDCaption">
                                        Password :
                                    </td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <td class="TDDot" nowrap="nowrap">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtPwdForIRD" runat="server" MaxLength="10" TabIndex="1"
                                            Width="80px" Text="1" TextMode="Password" CssClass="Textbox" ClientIDMode="Static"></asp:TextBox>
                                        <span class="span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWrongPwd" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <table style="width: 265px;">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAcceptPwd" CausesValidation="false" runat="server" Text="Save"
                                            Style="position: relative; left: 30px;" ClientIDMode="Static" TabIndex="6" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnCancelPwd" CausesValidation="false" runat="server" Text="Cancel"
                                            ClientIDMode="Static" TabIndex="7" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </fieldset>
    <script type="text/javascript" language="javascript">
    </script>
    <asp:HiddenField ID="hdnPrefixForCurrentYear" runat="server" />
    <asp:HiddenField ID="hdnStoreAmount" runat="server" Value="0" />
    <asp:HiddenField ID="hdnReportType" runat="server" />
    <asp:HiddenField ID="hdnPackageExpired" runat="server" ClientIDMode="Static" />    
    <asp:HiddenField ID="hdnTmpBookingNo" runat="server" />
    <asp:HiddenField ID="hdntmpSatisfiedCust" runat="server" />       
    <asp:HiddenField ID="hdnTmpDelWithoupSlip" runat="server" />       
    <asp:HiddenField ID="hdnTmpDelDate" runat="server" ClientIDMode="Static" />    
</asp:Content>
