<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="MonthlyStatement" Title="Monthly Statement" CodeBehind="MonthlyStatement.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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


    </script>
    <style type="text/css">
        .style1
        {
            width: 853px;
        }
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
                    Process wise Booking
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
                            <td class="style1">
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
                                        </td>
                                        <td nowrap="nowrap" width="50">
                                        </td>
                                        <td>
                                            Process
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpProcess" runat="server" DataSourceID="SDTProcesses" DataTextField="ProcessName"
                                                DataValueField="ProcessCode" AppendDataBoundItems="true">
                                                <asp:ListItem Selected="True" Value="0" Text="ALL" />
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td rowspan="3">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdReport" runat="server" Caption="Process wise Booking" ShowFooter="True"
                            EmptyDataText="No record found" PageSize="50">
                        </asp:GridView>
                    </div>
                    <br />
                </td>
            </tr>
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
                    <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] ORDER BY [ProcessName]">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
         <table width="100%">
            <tr>
                <td width="50%">
                    <div id='chart_div'>
                    </div>
                </td>
                <td width="50%">
                    <div id='chart_div1'>
         
                  </div>       
                </td>
            </tr>
        </table>
        
         
       <%-- <script type='text/javascript' src='https://www.google.com/jsapi'></script>--%>
        <script src="../js/jsapi.js" type="text/javascript"></script>
        <script src="../JavaScript/javascript.js" type="text/javascript"></script>
        <script type='text/javascript'>

            function googlechart() {

                var grd = document.getElementById('<%= grdReport.ClientID %>');

                google.load('visualization', '1', { packages: ['corechart'] });
                google.setOnLoadCallback(drawChart);
                function drawChart() {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Process');
                    data.addColumn('number', 'Amount');
                    for (var i = 1; i < grd.rows.length - 1; i++) {
                        data.addRows([[grd.rows[i].cells[0].innerHTML, parseInt(grd.rows[i].cells[1].innerHTML)]]);
                    }
                    var options = { title: 'Process and Amount', 'is3D': true, pieSliceText: 'value' };
                    var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
                    chart.draw(data, options);
                }


                google.load('visualization', '1', { packages: ['corechart'] });
                google.setOnLoadCallback(drawChart1);
                function drawChart1() {
                    var data1 = new google.visualization.DataTable();
                    data1.addColumn('string', 'Process');
                    data1.addColumn('number', 'Quantity');
                    for (var i = 1; i < grd.rows.length - 1; i++) {
                        data1.addRows([[grd.rows[i].cells[0].innerHTML, parseInt(grd.rows[i].cells[2].innerHTML)]]);
                    }
                    var options1 = { title: 'Process and Quatity', 'is3D': true, pieSliceText: 'value' };
                    var chart1 = new google.visualization.PieChart(document.getElementById('chart_div1'));
                    chart1.draw(data1, options1);
                }
            }
                  
     </script>
    </fieldset>
</asp:Content>
