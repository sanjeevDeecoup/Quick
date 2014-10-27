<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="TimeandColthReport" Title="Untitled Page" Codebehind="TimeandColthReport.aspx.cs" %>

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
	//document.getElementById('<%= txtReportUpto.ClientID %>').value = document.getElementById('<%= txtReportFrom.ClientID %>').value; 	
}

function CheckCorrectEntry()
{
    var BookingTime=document.getElementById("<%=txtAreaLocation.ClientID %>").value;
    var BookingTime1=document.getElementById("<%=txtToArea.ClientID %>").value;
    
    if(BookingTime=="")
    {
        alert("Please enter Booking Time.");
        document.getElementById("<%=txtAreaLocation.ClientID %>").focus();
        return false;
    }
    if(BookingTime1=="")
    {
        alert("Please enter To Booking Time.");
        document.getElementById("<%=txtToArea.ClientID %>").focus();
        return false;
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Time
                    Wise Cloth
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
                            <td>
                                <table class="TableData" >
                                    <tr>
                                        <td nowrap="nowrap" class="TDCaption">
                                            Booking Time
                                        </td>
                                        <td nowrap="nowrap" width="150">
                                            <asp:TextBox ID="txtAreaLocation" runat="server" Width="50Px" MaxLength="2"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtAreaLocation_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" FilterType="Numbers" TargetControlID="txtAreaLocation">
                                            </cc1:FilteredTextBoxExtender>
                                            To
                                            <asp:TextBox ID="txtToArea" runat="server" Width="50Px" MaxLength="2"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtToArea_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" FilterType="Numbers" TargetControlID="txtToArea">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="drpTime" runat="server">
                                                <asp:ListItem Text="AM"></asp:ListItem>
                                                <asp:ListItem Text="PM"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
                                            <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                                                Text="From" CssClass="TDCaption" />
                                            &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                                onpaste="return false;" onchange="return SetUptoDate();" />
                                            <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                                Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td nowrap="nowrap" width="150">
                                            &nbsp;<span class="TDCaption">To</span><asp:TextBox ID="txtReportUpto" runat="server"
                                                Width="80px" onkeypress="return false;" onpaste="return false;" />
                                            <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                                Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return CheckCorrectEntry();"
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
                            <td class="TDCaption" style="text-align: left">
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                    <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        EmptyDataText="No record found" PageSize="50" CssClass="mGrid">
                                        <Columns>
                                            <asp:BoundField DataField="Item" HeaderText="Item Name" ItemStyle-HorizontalAlign="Left"
                                                SortExpression="Item" FooterText="Total Qty"></asp:BoundField>
                                            <asp:BoundField DataField="Qty" HeaderText="Qty" ItemStyle-HorizontalAlign="Left"
                                                SortExpression="Qty"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Visible="False"
                                    OnClick="btnExport_Click" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>
