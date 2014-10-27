<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="Reports_AllDelivery" Title="Untitled Page" Codebehind="AllDelivery.aspx.cs" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset class="Fieldset"><legend class="Legend">Delivery Report</legend>
<table class="TableData">
    <tr valign="top">
        <td>
            <table class="TableData">
                <tr>
                    <td>
                  <table class="TableData">
                   <tr>
                    <td nowrap="nowrap">
                        <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" 
                            GroupName="radReportOptions" Text="From" CssClass="TDCaption" />
                            &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;" onpaste="return false;"  onchange="return SetUptoDate();"/>
                        <asp:Image ID="imgReportFrom" runat="server" 
                            ImageUrl="~/images/Calendar_scheduleHS.png" />
                             <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" 
                            PopupButtonID="imgReportFrom" TargetControlID="txtReportFrom">
                        </cc1:CalendarExtender>
                    </td>
                    <td nowrap="nowrap" width="150" class="TDCaption">
                        &nbsp;<span class="TDCaption">To</span> <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;" onpaste="return false;" />
                        <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                        <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" 
                            PopupButtonID="imgReportUpto" TargetControlID="txtReportUpto">
                        </cc1:CalendarExtender>
                    </td>
                    <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
                        <asp:RadioButton ID="radReportMonthly" runat="server" 
                            GroupName="radReportOptions" Text="Monthly Report" CssClass="TDCaption" />
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
                        <asp:Button ID="btnShowReport" runat="server" Text="Show" 
                            OnClientClick="return checkEntry();" onclick="btnShowReport_Click"
                             />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>

                </table>    
                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" 
                            ForeColor="#CC0000"></asp:Label>
                        <asp:GridView ID="grdReport" runat="server" Visible="False" 
                            AutoGenerateColumns="False" ShowFooter="False"
                            EmptyDataText="No record found" >
                            <FooterStyle BackColor="#999966" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" 
                                    HeaderText="Date" SortExpression="BookingDate" />
                                
                                <asp:TemplateField HeaderText="Booking Number">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>' Target="_blank" NavigateURL='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}",Eval("BookingNumber")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="BookingNumber" DataFormatString="{0:d}" 
                                    HeaderText="Booking #" SortExpression="BookingNumber" />--%>
                                <asp:BoundField DataField="DeliveryDate" DataFormatString="{0:d}" 
                                    HeaderText="Delivery Date" SortExpression="DeliveryDate" />
                            </Columns>
                            <HeaderStyle BackColor="Maroon" ForeColor="White" VerticalAlign="Top" />
                            <AlternatingRowStyle BackColor="#CCFFFF" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </td>       
    </tr>
    <tr valign="top">
        <td>
            
            <table style="width:100%;">
                <tr>
                    <td>
                        <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                            Text="Export to Excel" Visible="False" />
                        <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                            ></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            
        </td>
    </tr>
    <tr valign="top">
        <td>
            
            &nbsp;</td>
    </tr>
</table>
</fieldset>
</asp:Content>

