<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="ExpenseReport" Title="Expense Report" CodeBehind="ExpenseReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Expense
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
        <table class="TableData" style="width: 100%;">
            <tr valign="top">
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <table>
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
                                            &nbsp; <span class="TDCaption">To</span><asp:TextBox ID="txtReportUpto" runat="server"
                                                Width="80px" onkeypress="return false;" onpaste="return false;" />
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
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap" class="TDCaption">
                                            Report type :
                                            <asp:DropDownList ID="drpExpReportType" runat="server">
                                                <asp:ListItem Text="Summary" Value="1" />
                                                <asp:ListItem Text="Detailed" Value="2" />
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Ledger Name :
                                            <asp:DropDownList ID="drpLedger" runat="server" AppendDataBoundItems="true" DataSourceID="SDTExpenseLedgers"
                                                DataTextField="LedgerName" DataValueField="LedgerName">
                                                <asp:ListItem Text="All"></asp:ListItem>
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
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="ErrorMessage"></asp:Label>
                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                    <asp:GridView ID="grdReport" runat="server" Visible="False" AutoGenerateColumns="True"
                                        ShowFooter="True" EmptyDataText="No record found" PageSize="50" AllowPaging="false"
                                        FooterStyle-CssClass="pgr">
                                    </asp:GridView>
                                </div>
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
                                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Visible="False"
                                    OnClick="btnExport_Click" />
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
        <div id="chart_div">
        </div>
        <%--  <script type='text/javascript' src='https://www.google.com/jsapi'></script>--%>
        <script src="../js/jsapi.js" type="text/javascript"></script>
        <script src="../JavaScript/javascript.js" type="text/javascript"></script>
        <script type='text/javascript'>

            function googlechart() {

                var grd = document.getElementById('<%= grdReport.ClientID %>');

                google.load('visualization', '1', { packages: ['corechart'] });
                google.setOnLoadCallback(drawChart);
                function drawChart() {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Date');
                    data.addColumn('number', 'Amount');
                    for (var i = 1; i < grd.rows.length - 1; i++) {
                        data.addRows([[grd.rows[i].cells[0].innerHTML, parseInt(grd.rows[i].cells[1].innerHTML)]]);
                    }

                    var options = { title: 'Datewise Expenses', vAxis: { title: "Amount" }, 'pointSize': 3 };
                    var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
                    chart.draw(data, options);
                }

            }
        </script>
        <asp:SqlDataSource ID="SDTExpenseLedgers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT [LedgerName] FROM [LedgerMaster] WHERE (([LedgerName] NOT LIKE '%' + @LedgerName + '%') AND ([LedgerName] NOT LIKE '%' + @LedgerName2 + '%'))">
            <SelectParameters>
                <asp:Parameter DefaultValue="CASH" Name="LedgerName" Type="String" />
                <asp:Parameter DefaultValue="CUST" Name="LedgerName2" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </fieldset>
</asp:Content>