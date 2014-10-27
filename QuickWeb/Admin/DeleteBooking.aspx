<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="DeleteBooking.aspx.cs" Inherits="QuickWeb.Admin.DeleteBooking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CheckBookingNumber() {
            var bn = document.getElementById("<%= txtReceiptNumber.ClientID %>").value;
            if (bn == "") {
                alert("Please enter Order number");
                document.getElementById("<%= txtReceiptNumber.ClientID %>").focus();
                return false;
            }
        }
        function ConfirmDelete() {

            return confirm("Are you sure you want to Delete this Order?");

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
                        document.getElementById("ctl00_ContentPlaceHolder1_btnSDetails").click();
                    }
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableData">
        <tr valign="top">
            <td class="TDCaption">
                Order Number :
            </td>
            <td>
                <asp:TextBox ID="txtReceiptNumber" runat="server" Width="75" MaxLength="15" TabIndex="1"
                     />
                <cc1:FilteredTextBoxExtender ID="txtReceiptNumber_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" FilterType="Numbers" 
                    TargetControlID="txtReceiptNumber">
                </cc1:FilteredTextBoxExtender>
                &nbsp;
                <asp:Button ID="btnSDetails" runat="server" OnClick="btnSDetails_Click" Text="Show Details"
                    TabIndex="2" OnClientClick="return CheckBookingNumber()" />
            </td>
        </tr>
        <tr>
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
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShowDetails" runat="server" Text="Delete Order" TabIndex="3" OnClick="btnShowDetails_Click"
                    OnClientClick="return ConfirmDelete()" Visible="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
            </td>
        </tr>
    </table>
</asp:Content>
