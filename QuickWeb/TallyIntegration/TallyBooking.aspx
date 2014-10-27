<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="TallyBooking.aspx.cs" Inherits="QuickWeb.TallyIntegration.TallyBooking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        function SetUptoDate() {
            //  document.getElementById('<%= txtReportUpto.ClientID %>').value = document.getElementById('<%= txtReportFrom.ClientID %>').value;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Booking
                    Report Tally
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
            <tr valign="top">
                <td>
                    <table class="TableData" width="100%">
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
                                <asp:Button ID="btnShowReport" runat="server" Text="Export To Tally" 
                                    OnClientClick="return checkEntry();" onclick="btnShowReport_Click" />
                                <asp:Label ID="lblLastBooking" runat="server" Text="Last Report Generated For" Style="left: 40px;"></asp:Label>
                                <asp:Label ID="lblresult" runat="server"></asp:Label>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>
    </fieldset>
</asp:Content>
