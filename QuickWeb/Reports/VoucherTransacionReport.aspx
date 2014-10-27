<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="Accounts_VoucherTransacionReport"  Codebehind="VoucherTransacionReport.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
     .ajax__calendar_container { z-index : 1000 ; }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
    <td></td>
    <td></td></tr>
              <tr>

              
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>&nbsp;Statement
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
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
        <td>
            <table style="width:100%;">
                <tr>
                    <td>
                <table>
                   <tr>
                    <td nowrap="nowrap">
&nbsp;<asp:RadioButton ID="radReportFrom" runat="server" Checked="True" 
                            GroupName="radReportOptions" Text="From" 
                            oncheckedchanged="radReportFrom_CheckedChanged" />
                        <asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;" onpaste="return false;" onchange="return SetUptoDate();" />
                        <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender0" runat="server" PopupButtonID="imgReportFrom" format="dd MMM yyyy"
                            TargetControlID="txtReportFrom">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgReportFrom" runat="server" 
                            ImageUrl="~/images/Calendar_scheduleHS.png" />
                             
                    </td>
                    <td nowrap="nowrap" width="125">
                        &nbsp;To <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;" onpaste="return false;" />
                        <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender0" runat="server" PopupButtonID="imgReportUpto" Format="dd MMM yyyy"
                            TargetControlID="txtReportUpto">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />                       
                    </td>
                    <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
                        <asp:RadioButton ID="radReportMonthly" runat="server" 
                            GroupName="radReportOptions" Text="Monthly Report" />
                        &nbsp;<asp:DropDownList ID="drpMonthList" runat="server" >
                        <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                            <asp:ListItem  Value="1">January</asp:ListItem>
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
                                <asp:CheckBox ID="ChkCustomer" runat="server" Text=" By Customer : " AutoPostBack="True"
                        OnCheckedChanged="ChkCustomer_CheckedChanged"  /></td>
                        <td></td>
                        <td>
                    <asp:TextBox ID="txtCustomerName" runat="server" Width="200px" Visible="false" 
                        OnTextChanged="txtCustomerName_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                        CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                    </cc1:AutoCompleteExtender>
                            </td>
                         
                    <td></td>
                    <td> <asp:DropDownList ID="drpOption" runat="server" 
                            onselectedindexchanged="drpOption_SelectedIndexChanged" >
                                            <asp:ListItem Text="Summary"></asp:ListItem>
                                            <asp:ListItem Text="Detailed"></asp:ListItem>
                                        </asp:DropDownList></td>
                    <td>
                        <asp:Button ID="btnShowReport" runat="server" Text="Show" 
                            OnClientClick="return checkEntry();" onclick="btnShowReport_Click"
                             />
                    </td>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Text="Print" 
                            onclick="btnPrint_Click" Visible="False" /> </td>
                </tr>

                </table>    

                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" 
                            ForeColor="#CC0000" CssClass="SuccessMessage"></asp:Label>
                        <asp:Label ID="lblStoreName" runat="server" Text="lblStoreName" Visible="False"></asp:Label>
                        <asp:Label ID="lblAddress" runat="server" Text="lblAddress" Visible="False"></asp:Label>
                    </td> 
                </tr>
             </table>
            <table>
             <tr>
             <td></td>
                    <td>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="950px"  Visible="false">
                        <LocalReport ReportPath="RDLC\Report4.rdlc">
                                             </LocalReport>          
                        </rsweb:ReportViewer>
                        
                                          
                      
                        
                       
                        
                         
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            SelectMethod="NewRow" 
                            TypeName="QuickWeb.PaymentTypeDataSet+sp_PaymentDataTable">
                        </asp:ObjectDataSource>
                        
                       
                        
                         
                    </td>
                </tr></table>
        </td>       
    </tr>
    <tr valign="top">
        <td>
            
            <table style="width:100%;">
                <tr>
                    <td>
                        <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                            SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
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


<asp:HiddenField ID="hdnCustId" runat="server" />

</fieldset>
</asp:Content>

