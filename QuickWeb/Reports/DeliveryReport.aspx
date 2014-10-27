<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="DeliveryReport" Title="Untitled Page" Codebehind="DeliveryReport.aspx.cs" %>

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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Delivery
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
                                    <tr id="tr" visible="false" runat="server">
                                        <td>
                                            <asp:RadioButton ID="rdbCustomerWise" runat="server" Text=" Name" GroupName="ABC"
                                                Checked="true" AutoPostBack="True" OnCheckedChanged="rdbCustomerWise_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbMobileWise" runat="server" Text=" Mobile No" GroupName="ABC"
                                                OnCheckedChanged="rdbMobileWise_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
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
                                        <td class="TDCaption" nowrap="nowrap">
                                            <asp:Label ID="lblName" runat="server" Text="Invoice Number : "></asp:Label>
                                            <asp:TextBox ID="txtCustomerWise" runat="server" CssClass="Textbox"></asp:TextBox>
                                           <%-- <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerWise"
                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCustomerName" MinimumPrefixLength="1"
                                                CompletionInterval="500" EnableCaching="true" CompletionSetCount="15" ShowOnlyCurrentWordInCompletionListItem="False"
                                                FirstRowSelected="True" CompletionListCssClass="accordionHeader" />--%>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" />
                                        </td>
                                        <td nowrap="nowrap">
                                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="Textbox" Visible="false"></asp:TextBox>
                                            <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtMobileNo"
                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetMobileNo" MinimumPrefixLength="1"
                                                CompletionInterval="500" EnableCaching="true" CompletionSetCount="15" ShowOnlyCurrentWordInCompletionListItem="False"
                                                FirstRowSelected="True" CompletionListCssClass="accordionHeader" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td class="TDCaption" style="text-align: left">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdReport" runat="server" Visible="False" AutoGenerateColumns="False"
                            ShowFooter="true" EmptyDataText="No Records found" CssClass="mGrid">
                            <Columns>
                                <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Date"
                                    SortExpression="BookingDate" />
                                <asp:TemplateField HeaderText="Booking Number">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                            Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/DeliverySlip.aspx?RS=0&BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DeliveryDate")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName">
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Delivery Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DeliveryDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Qty" HeaderText="Quantity" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="Quantity">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="TotalCost" HeaderText="Gross Amount" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="TotalCost">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="DiscountOnPayment" HeaderText="Discount" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="DiscountOnPayment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="ST" HeaderText="Tax" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="ST">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="NetAmount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PaymentMade" HeaderText="Advance" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="PaymentMade">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                              
                                <asp:BoundField DataField="DuePayment" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="DuePayment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                            <%-- <HeaderStyle BackColor="Maroon" ForeColor="White" VerticalAlign="Top" />
                                    <AlternatingRowStyle BackColor="#CCFFFF" />--%>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export to Excel"
                                    Visible="true" />
                                <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td>
                              <asp:SqlDataSource ID="SqlSourceChallanShifts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="SELECT CustomerCode,CustomerName FROM CustomerMaster"></asp:SqlDataSource>
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
