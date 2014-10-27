<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="DeliveryQuantityandPriceReport" Title="Untitled Page" Codebehind="DeliveryQuantityandPriceReport.aspx.cs" %>

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
&nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;" onpaste="return false;" onchange="return SetUptoDate();" />
                        <asp:Image ID="imgReportFrom" runat="server" 
                            ImageUrl="~/images/Calendar_scheduleHS.png" />
                             <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" 
                            PopupButtonID="imgReportFrom" TargetControlID="txtReportFrom">
                        </cc1:CalendarExtender>
                    </td>
                    <td nowrap="nowrap" width="150">
                        &nbsp;<span class="TDCaption">To</span><asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;" onpaste="return false;" />
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
                        &nbsp;<asp:DropDownList ID="drpYearList" runat="server" >
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnShowReport" runat="server" Text="Show" 
                            OnClientClick="return checkEntry();" onclick="btnShowReport_Click"
                             />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnExport" runat="server" Text="Export to Excel" 
                            Visible="False" onclick="btnExport_Click" />
                       </td>
                </tr>

                </table>    

                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" 
                            ForeColor="#CC0000"></asp:Label>
                        <asp:GridView ID="grdReport" runat="server"
                            AutoGenerateColumns="False" DataKeyNames="BookingNumber" 
                            ShowFooter="True"
                            EmptyDataText="No record found" PageSize="50" > 
                            <FooterStyle BackColor="#DEC3C6" ForeColor="White" Font-Bold="true" />                           
                            <Columns>
                                <asp:BoundField DataField="BookingDeliveryDate" DataFormatString="{0:d}" 
                                    HeaderText="Delivery Date" SortExpression="BookingDeliveryDate" FooterText="Total" />
                                <asp:TemplateField HeaderText="Booking Number">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>' Target="_blank" NavigateURL='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}",Eval("BookingNumber")) %>' />
                                        <asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("BookingStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                    <asp:BoundField DataField="Qty" HeaderText="Total Quantity" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="Quantity">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NetAmount" HeaderText="Total Amount" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="NetAmount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PaymentMade" HeaderText="Payment" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="PaymentMade">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DuePayment" HeaderText="Balance Amount" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="DuePayment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField> 
                               <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="PaymentDate">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>                          
                            </Columns>
                            <HeaderStyle BackColor="Maroon" ForeColor="White" />
                            <AlternatingRowStyle BackColor="#CCFFFF" />
                        </asp:GridView>
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
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                            SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
        </td>
    </tr>
    <tr valign="top">
        <td>
            
            &nbsp;</td>
    </tr>
</table>
</fieldset>
<asp:HiddenField ID="hdnStartDate" runat="server" />
<asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>

