<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="BookingHistory.aspx.cs" Inherits="QuickWeb.Reports.BookingHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function checkName() {
            var strname = document.getElementById("<%=txtBookingNumber.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please Enter a Valid Order No.");
                document.getElementById("<%=txtBookingNumber.ClientID %>").focus();
                return false;
            }
        }
        $(document).ready(function (eP) {

            $('#btnShow').click(function (e) {
                if ($('#txtBookingNumber').val() === '') {
                    $('#txtBookingNumber').focus();
                    return false;
                }
            });

            $('#txtBookingNumber').keydown(function (e) {
                if (e.which === 13 && this.value !== '') {
                    __doPostBack('txtBookingNumber', 'null');
                }
                else if (e.which === 13) {
                    $('#txtBookingNumber').focus();
                    return false;
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"
    ClientIDMode="Static">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="Search Order " ForeColor="#FF9933"></asp:Label><span
                        class="" style="font-size: 12Px"> View the history of the Seleceted Order </span>
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
                <td nowrap="nowrap" align="center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="ErrorMessage" EnableViewState="false"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableData" width="100%">
            <tr valign="top">
                <td>
                    <table class="TableData" width="100%">
                        <tr>
                            <td nowrap="nowrap">
                                <asp:RadioButton ID="radReportFrom" runat="server" GroupName="radReportOptions" Text="From"
                                    CssClass="TDCaption" />
                                &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" onchange="return SetUptoDate();" />
                                <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                    Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                                </cc1:CalendarExtender>
                                &nbsp;<span class="TDCaption">To</span>
                                <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" />
                                <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                    Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                                </cc1:CalendarExtender>
                                <asp:RadioButton ID="radReportMonthly" runat="server" Checked="True" GroupName="radReportOptions"
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
                                <asp:CheckBox ID="chkInvoiceNo" runat="server" Text="Search By Order No." Checked="false"
                                    AutoPostBack="True" OnCheckedChanged="chkInvoiceNo_CheckedChanged" />
                                <asp:TextBox ID="txtBookingNumber" runat="server" MaxLength="20" Visible="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtBookingNumber_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtBookingNumber" ValidChars="1234567890-">
                                </cc1:FilteredTextBoxExtender>
                                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return checkName();" />
                                &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="TableData" width="100%">
            <tr>
                <td>
                    <asp:GridView ID="GrdEditHistoryBooking" runat="server" AutoGenerateColumns="false"
                        ShowFooter="false" EmptyDataText="No records found." Width="260Px">
                        <Columns>
                            <asp:TemplateField HeaderText="Order No." HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                        OnClick="hypBtnShowDetails_Click" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingDate" HeaderText="Order Date" SortExpression="BookingDate"
                                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
