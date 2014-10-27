<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="Reports_VendorReport" Title="Vendor Report" Codebehind="VendorReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../JavaScript/javascript.js" type="text/javascript"></script>

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
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Vendor
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
                <td colspan="3">
                    <table class="TableData">
                        <tr>
                            <td nowrap="nowrap">
                                <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                                    Text="From" />
                                &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" onchange="return SetUptoDate();" />
                                <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                    Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                                </cc1:CalendarExtender>
                            </td>
                            <td nowrap="nowrap" width="150">
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
        </table>
        <table>
            <tr valign="top">
                <td colspan="3" style="width: 33%">
                    <table width="100%">
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Select Process
                                            <div style="width: 200px; overflow: auto; border-style: solid; border-width: thin;
                                                height: 200px;">
                                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                                    <asp:GridView ID="grdProcessSelection" runat="server" BackColor="White" AutoGenerateColumns="False"
                                                        BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="1" DataSourceID="SDTProcesses">
                                                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                                        <RowStyle BackColor="White" ForeColor="#330099" />
                                                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Process">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProcessName" runat="server" Text='<%# Bind("ProcessName") %>' />
                                                                    <asp:HiddenField ID="hdnProcessCode" runat="server" Value='<%# Bind("ProcessCode") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Select Item
                                            <div style="width: 200px; overflow: auto; border-style: solid; border-width: thin;
                                                height: 200px;">
                                                .
                                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                                    <asp:GridView ID="grdItemSelection" runat="server" BackColor="White" AutoGenerateColumns="False"
                                                        BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="1" DataSourceID="SDTItems">
                                                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                                        <RowStyle BackColor="White" ForeColor="#330099" />
                                                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Items">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' />
                                                                    <asp:HiddenField ID="hdnItemName" runat="server" Value='<%# Bind("ItemId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="100%">
                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                    <asp:GridView ID="grdVendorReport" runat="server" EmptyDataText="No Record found"
                                        ShowFooter="True" AutoGenerateColumns="False">
                                        <FooterStyle BackColor="#FAFAFA" Font-Bold="True" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" FooterText="Total"
                                                HeaderText="Booking Date" />
                                            <asp:TemplateField HeaderText="Booking Number">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                        Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}",Eval("BookingNumber")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="BookingNumber" HeaderText="Booking #" />--%>
                                            <asp:BoundField DataField="ProcessCost" HeaderText="Amount" />
                                            <asp:BoundField DataField="Pieces" HeaderText="Pieces" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
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
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
