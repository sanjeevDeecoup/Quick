<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="SalesLedger" Title="Sales Ledger" CodeBehind="SalesLedger.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Sales
                    Ledger
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
                    <table class="TableData">
                        <tr>
                            <td>
                                <table class="TableData">
                                    <tr>
                                        <td nowrap="nowrap">
                                            <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                                                Text="From" CssClass="TDCaption" />
                                            &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                                onpaste="return false;" onchange="return SetUptoDate();" />
                                            <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                                TargetControlID="txtReportFrom" Format="dd MMM yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td nowrap="nowrap" width="150">
                                            &nbsp;<span class="TDCaption">To</span>
                                            <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;"
                                                onpaste="return false;" />
                                            <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                                TargetControlID="txtReportUpto" Format="dd MMM yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
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
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdReport" runat="server" Visible="False" AutoGenerateColumns="False"
                                                OnRowCommand="grdReport_OnRowCommand" ShowFooter="True" EmptyDataText="No record found">
                                                <FooterStyle BackColor="#999966" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="PaymentDate" DataFormatString="{0:d}" HeaderText="Date"
                                                        SortExpression="PaymentDate" FooterText="Total" />
                                                    <asp:BoundField DataField="PaymentMade" HeaderText="Sale" SortExpression="PaymentMade">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgPaymentDetail" runat="server" ImageUrl="~/images/ZoomHS.png"
                                                                ImageAlign="AbsMiddle" CommandName="CMDShowPaymentDetail" CommandArgument='<%# Bind("PaymentDate") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="Maroon" ForeColor="White" VerticalAlign="Top" />
                                                <AlternatingRowStyle BackColor="#CCFFFF" />
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:GridView ID="grdPaymentDetails" runat="server" Visible="False" AutoGenerateColumns="False"
                                                EmptyDataText="No record found" Caption="Details" OnRowCommand="grdPaymentDetails_RowCommand">
                                                <FooterStyle BackColor="#999966" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="PaymentDate" DataFormatString="{0:d}" HeaderText="Date"
                                                        SortExpression="PaymentDate" FooterText="Total" />
                                                    <asp:BoundField DataField="BookingNumber" HeaderText="Booking No." SortExpression="BookingNumber">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PaymentMade" HeaderText="Sale" SortExpression="PaymentMade">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgPaymentDetail" runat="server" ImageUrl="~/images/ZoomHS.png"
                                                                ImageAlign="AbsMiddle" CommandName="ShowPaymentDetail" CommandArgument='<%# Bind("BookingNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="Maroon" ForeColor="White" VerticalAlign="Top" />
                                                <AlternatingRowStyle BackColor="#CCFFFF" />
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            <asp:GridView runat="server" ID="grdPaymentDate" AutoGenerateColumns="false" Visible="true"
                                                CssClass="mGrid">
                                                <Columns>
                                                    <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" SortExpression="PaymentDate" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export to Excel"
                                    Visible="False" />
                                <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="Sp_Sel_PaymentReport" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="PaymentDate1" DefaultValue="" />
                                        <asp:Parameter Name="PaymentDate2" DefaultValue="" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>