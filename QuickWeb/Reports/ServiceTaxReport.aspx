<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="ServiceTaxReport" Title="ServiceTaxReport" CodeBehind="ServiceTaxReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

    <%@ Register TagPrefix="uc" TagName="Duration" 
    Src="~/Controls/DurationControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>
                    Tax
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
                                    <tr>
                                        <td>
                                        <uc:Duration id="uc1" runat="server"></uc:Duration>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" />
                                        </td>
                                        <td>
                                           <asp:Button ID="btnPrintButton" runat="server" Text="Print" onclick="btnPrintButton_Click"  visible="false"
                                               
                                                 />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="center" colspan="4">
                                            <asp:Label ID="lblMsg" runat="server" CssClass="ErrorMessage" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td rowspan="2">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Select Process
                                            <div style="width: 200px; overflow: auto; border-style: solid; border-width: thin;
                                                height: 300px;">
                                                <asp:GridView ID="grdProcessSelection" runat="server" AutoGenerateColumns="False"
                                                    DataSourceID="SDTProcesses">
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
                                            <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>">
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="TableData">

                            <asp:Panel ID="RptPanel" runat="server">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="850px" 
                                    Visible="false" Height="414px">
                                    <LocalReport ReportPath="RDLC\ServicetaxReport.rdlc">
                                     <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                                            Name="ServiceTaxdataSet_sp_Service" />
                                    </DataSources>
                                        
                                    </LocalReport>
                                   
                                </rsweb:ReportViewer></asp:Panel>
                        
                                 <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="Copy" 
                                    TypeName="ServiceTaxDataSet"></asp:ObjectDataSource>
                               
                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
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
                                    SelectCommand="SELECT BookingDate, SUM(NetAmount) FROM [ViewBookingReport]">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
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
