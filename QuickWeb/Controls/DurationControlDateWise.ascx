<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DurationControlDateWise.ascx.cs"
    Inherits="QuickWeb.Controls.DurationControlDateWise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div>
    <table>
        <tr>
            <td nowrap="nowrap">
                <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                    Text="From" CssClass="TDCaption" />
                &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                    onpaste="return false;" onchange="return SetUptoDate();" />
                <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                    Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                </cc1:CalendarExtender>
            </td>
            <td nowrap="nowrap">
                &nbsp;<span class="TDCaption">To</span>
                <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;"
                    onpaste="return false;" />
                <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                    Format="dd MMM yyyy" TargetControlID="txtReportUpto">
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
        </tr>
    </table>
</div>