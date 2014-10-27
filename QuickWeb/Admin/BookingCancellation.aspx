<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="Admin_BookingCancellation" Title="Untitled Page" CodeBehind="BookingCancellation.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CheckBookingNumber() {
            var bn = document.getElementById("<%= txtReceiptNumber.ClientID %>").value;
            if (bn == "") {
                alert("Please enter receipt number");
                return false;
            }
        }

        function ConfirmCancel() {
            var bn = document.getElementById("<%= txtRemarks.ClientID %>").value;
            if (bn == "") {
                alert("Please enter reason for cancellation");
                document.getElementById("<%=txtRemarks.ClientID %>").focus();
                return false;
            }
            else {
                return confirm("Are you sure you want to cancel this Order?");
            }
        }
        function setPrefix() {
            document.getElementById("<%= txtReceiptNumber.ClientID %>").value = document.getElementById("<%= hdnPrefixForCurrentYear.ClientID %>").value;

        }
        $(document).ready(function (event) {

            $(document).keypress(function (event) {
                var textval = $('#ctl00_ContentPlaceHolder1_txtReceiptNumber').val();
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    if (textval == "") {
                        return false;
                    }
                    else {
                        document.getElementById("ctl00_ContentPlaceHolder1_btnShowDetails").click();
                    }
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Booking
                    Cancellation
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
            <tr valign="top">
                <td>
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table class="TableData">
                        <tr valign="top">
                            <td class="TDCaption">
                                Order Number :
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptNumber" runat="server" onfocus="return setPrefix();" Width="75"
                                    MaxLength="15" TabIndex="1" />&nbsp;<asp:Button ID="btnShowDetails" runat="server"
                                        OnClick="btnShowDetails_Click" Text="Show Details" TabIndex="2" OnClientClick="return CheckBookingNumber()" />
                            </td>
                        </tr>
                        <tr valign="top" id="idRemakrs" runat="server" visible="false">
                            <td colspan="2">
                                <asp:DetailsView ID="dtvBookingDetails" runat="server" AutoGenerateRows="False" EmptyDataText="Incorrect receipt number"
                                    Visible="False">
                                    <Fields>
                                        <asp:BoundField DataField="BookingNumber" HeaderText="Order Number" SortExpression="BookingNumber">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="CustomerName" SortExpression="CustName">
                                            <ItemTemplate>
                                                (<asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerCode") %>' />)<asp:Label
                                                    ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustomerAddress" HeaderText="CustomerAddress" SortExpression="CustomerAddress" />
                                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="BookingDate"
                                            SortExpression="BookingDate">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NetAmount" HeaderText="NetAmount" SortExpression="NetAmount">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PaymentMade" HeaderText="PaymentMade" SortExpression="PaymentMade">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BookingStatus" SortExpression="BookingStatus">
                                            <HeaderStyle Font-Bold="True" BackColor="#CCCCCC" ForeColor="Black" />
                                            <ItemStyle BackColor="#FDF7E3" Font-Bold="True" />
                                        </asp:BoundField>
                                    </Fields>
                                </asp:DetailsView>
                                <span class="TDCaption">Reason :</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                                    ID="txtRemarks" runat="server" MaxLength="250" Width="185Px" TextMode="MultiLine"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtRemarks"
                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetDetailRemoveReasonMaster"
                                    MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                                <asp:SqlDataSource ID="SqlSourceBookingDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel this booking" OnClick="btnCancel_Click"
                                    OnClientClick="return ConfirmCancel()" TabIndex="3" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnPrefixForCurrentYear" runat="server" />
</asp:Content>