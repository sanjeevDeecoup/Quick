<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="VendorReport" Title="Vendor Report" Codebehind="OutSourcedVendorReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
function checkEntry()
{
    var ReportFrmChecked=document.getElementById("<%=radReportFrom.ClientID %>").checked;
    var frmDate=document.getElementById("<%=txtReportFrom.ClientID %>").value;
    var toDate=document.getElementById("<%=txtReportUpto.ClientID %>").value;
    
    if(ReportFrmChecked==true)
    {
        if(frmDate=="" || toDate=="")
        {
            alert("Please select date from and upto which report is to be generated.");
            document.getElementById("<%=txtReportFrom.ClientID %>").focus();
            return false;
        }
    }
}
function  SetUptoDate()
{
	document.getElementById('<%= txtReportUpto.ClientID %>').value = document.getElementById('<%= txtReportFrom.ClientID %>').value; 	
}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset"><legend class="Legend">Vendor Report</legend>
        <table class="TableData">
            <tr valign="top">
                <td colspan="3">
                    <table class="TableData">
                        <tr>
                        <td class="TDCaption">Select Vendor</td>
                        <td><asp:DropDownList ID="drpVendor" runat="server" DataSourceID="SDTShifts" DataTextField="VendorName" DataValueField="ID">
            </asp:DropDownList>  </td>
                        <td></td>
                            <td nowrap="nowrap">
                                <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                                    Text="From" />
                                &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" onchange="return SetUptoDate();" />
                                <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                    TargetControlID="txtReportFrom">
                                </cc1:CalendarExtender>
                            </td>
                            <td nowrap="nowrap" width="150">
                                &nbsp;<span class="TDCaption">To</span>
                                <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" />
                                <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                    TargetControlID="txtReportUpto">
                                </cc1:CalendarExtender>
                            </td>
                            <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
                                <asp:RadioButton ID="radReportMonthly" runat="server" GroupName="radReportOptions"
                                    Text="Monthly Report" />
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
            <tr valign="top">
                <td colspan="3">
                    <asp:Label ID="lblMsg" runat="server" CssClass="ErrorMessage" EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="3">
                    <table width="100%">
                        <tr>
                            <td width="50%">
                                <asp:GridView ID="grdVendorReport" runat="server" EmptyDataText="No Record found"
                                    ShowFooter="True" AutoGenerateColumns="False">
                                    <FooterStyle BackColor="#B8B4B5" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" FooterText="Total"
                                            HeaderText="Booking Date" />                                        
                                    <asp:TemplateField HeaderText="Booking Number">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>' Target="_blank" NavigateURL='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}",Eval("BookingNumber")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <%--<asp:BoundField DataField="BookingNumber" HeaderText="Booking #" />--%>
                                        <asp:BoundField DataField="ProcessCost" HeaderText="Amount" />
                                        <asp:BoundField DataField="Pieces" HeaderText="Pieces" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td width="100">
                    <asp:Button ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click"
                        Text="Export to Excel" Width="125px" Visible="False" />
                </td>
                <td colspan="2">
                    <asp:SqlDataSource ID="SDTItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT [ItemID], [ItemName] FROM [ItemMaster]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"                        
                        SelectCommand="SELECT ProcessCode, ProcessName FROM ProcessMaster WHERE (ProcessUsedForVendorReport = 1) ORDER BY ProcessName">
                    </asp:SqlDataSource>
                     <asp:SqlDataSource ID="SDTShifts" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                SelectCommand="SELECT [ID], [VendorName] FROM [mstVendor]">
            </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
