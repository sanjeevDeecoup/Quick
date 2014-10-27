<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" CodeBehind="frmChallanInward.aspx.cs" Inherits="QuickWeb.Reports.frmChallanInward" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
        function SetUptoDate() {
            //document.getElementById('<%= txtReportUpto.ClientID %>').value = document.getElementById('<%= txtReportFrom.ClientID %>').value; 	
        }
    </script>
     <style type="text/css">
     .ajax__calendar_container { z-index : 1000 ; }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Cloths Ready Challan
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
                                <table class="TableData">
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
                                            &nbsp;<asp:DropDownList ID="drpOption" runat="server">
                                            <asp:ListItem Selected="True" Value="1">Summary</asp:ListItem>
                                            <asp:ListItem Value="2" >Detailed</asp:ListItem></asp:DropDownList>
                                        </td>
                                        <td style="width: 10Px">
                                        </td>
                                        
                                        <td>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" Height="26px" />
                                              &nbsp  
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="return checkEntry();"
                                                 Height="26px" onclick="btnPrint_Click" Visible="false" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                    <td align="center"><asp:Label ID="lblmsg" runat="server" CssClass="ErrorMessage" ></asp:Label>
                                    <asp:Label ID="lblItem" runat="server" CssClass="ErrorMessage"  Visible="false"></asp:Label>
                                    <asp:Label ID="lblQuantity" runat="server" CssClass="ErrorMessage" Visible="false" ></asp:Label></td></tr></table>
                                    <table>
                                    <tr>
                                    <td></td>
                                        
                                    <td> <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="800px" 
                                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Visible="false">
                                    <LocalReport ReportPath="RDLC\ChallanInward.rdlc">
                                        <DataSources>
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                                        </DataSources>
                                        </LocalReport>
                                        </rsweb:ReportViewer>
                                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                            SelectMethod="GetChanges" 
                                            TypeName="QuickWeb.ChallanInwardDataSet+sp_InwardDataTable">
                                        </asp:ObjectDataSource>
                                        </td></tr>
                                        
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
                                <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="SELECT ViewChallanDetails.* FROM [ViewChallanDetails] Inner Join [EntBookings] On EntBookings.BookingNumber=[ViewChallanDetails].BookingNumber Where [EntBookingStatus].BookingStatus<>5">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
       
    </fieldset>
</asp:Content>
