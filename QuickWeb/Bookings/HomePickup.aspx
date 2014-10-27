<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="HomePickup.aspx.cs" Inherits="QuickWeb.Bookings.HomePickup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jsapi.js" type="text/javascript"></script>
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
        function CHeck() {
            if (document.getElementById("<%=chkInvoice.ClientID %>").checked) {
                document.getElementById("<%=txtInvoiceNo.ClientID %>").visible = true;
                var InoviceNo = document.getElementById("<%=txtInvoiceNo.ClientID %>").value;
                if (InoviceNo == "" || InoviceNo.length == 0) {
                    alert("Please enter Order no");
                    document.getElementById("<%=txtInvoiceNo.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=chkCustomer.ClientID %>").checked) {
                document.getElementById("<%=txtCustomerName.ClientID %>").visible = true;
                var CustomerName = document.getElementById("<%=txtCustomerName.ClientID %>").value;
                if (CustomerName == "" || CustomerName.length == 0) {
                    alert("Please enter Customer name");
                    document.getElementById("<%=txtCustomerName.ClientID %>").focus();
                    return false;
                }
            }
        }
    </script>
    <script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $("body").delegate(":checkbox", "change", function (event) {
                // if the id is 'chkAll' then select all the ids
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll') {
                    // alert('Its all');
                    list3 = $(':checkbox:checked').not(document.getElementById('ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll')).map(function () {
                        return $(this).closest('tr').find('td').eq(1).text().substring(0, 11) + ':' + $(this).closest('tr').find('a').text();
                    }).get();
                    // alert(list3);
                }
                else if ($(event.target).attr('id') != 'ctl00_ContentPlaceHolder1_chkInvoice') {
                    list3 = $(':checkbox:checked').not(document.getElementById('ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll')).map(function () {
                        return $(this).closest('tr').find('td').eq(1).text().substring(0, 11) + ':' + $(this).closest('tr').find('a').text();
                    }).get();
                    //  alert(list3);
                }
                //alert(list3);
                $('#<%=hdnSelectedList.ClientID %>').text(list3);
                $('#<%=hdnSelectedList.ClientID %>').val(list3);
            });

            $('#<% =btnPrint.ClientID%>').click(function () {
                if (confirm("Are you sure you want to print all selected records?")) {
                }
                else {
                    return false;
                }
            });
            $('#<% =btnPrintStore.ClientID%>').click(function () {
                if (confirm("Are you sure you want to print all selected records?")) {
                }
                else {
                    return false;
                }
            });

        });
    </script>
    <script type='text/javascript'>

        function googlechart() {

            var grd = document.getElementById('<%= grdReport.ClientID %>');
            var Dt = new Array();
            var Amt = new Array();
            var Dt1 = new Array();
            var Qty = new Array();
            var indx = new Array();
            for (var i = 0, j = 1; i < grd.rows.length, j < grd.rows.length - 1; i++, j++) {
                Dt1 = (grd.rows[j].cells[1].innerHTML).split(' ');
                Dt[i] = Dt1[0] + '/' + Dt1[1] + '/' + Dt1[2];
                Qty[i] = grd.rows[j].cells[6].innerHTML;
                Amt[i] = grd.rows[j].cells[7].innerHTML;

            }
            for (var i = 0; i < grd.rows.length; i++) {
                if (Dt[i] == Dt[i - 1]) {
                    indx[i] = i;
                }

            }
            for (var i = Dt.length; i >= 0; i--) {
                var chk = indx[i];
                if (chk != null) {
                    Dt.splice(chk, 1);
                    Amt[i - 1] = parseInt(Amt[i]) + parseInt(Amt[i - 1]);
                    Amt.splice(chk, 1);
                    Qty[i - 1] = parseInt(Qty[i]) + parseInt(Qty[i - 1]);
                    Qty.splice(chk, 1);
                }

            }

            // Start Datewise Amount
            google.load('visualization', '1', { packages: ['corechart'] });
            google.setOnLoadCallback(drawChart);
            function drawChart() {
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Date');
                data.addColumn('number', 'Amount');
                for (var i = 0; i < grd.rows.length - 1; i++) {
                    data.addRows([[Dt[i], parseInt(Amt[i])]]);
                }

                var options = { title: 'Datewise Gross Amt.', vAxis: { title: "Amount" }, 'pointSize': 3 };
                var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
                chart.draw(data, options);
            }
            // End Datewise Amount

            // Start Datewise Qty
            google.load('visualization', '1', { packages: ['corechart'] });
            google.setOnLoadCallback(drawChart1);
            function drawChart1() {
                var data1 = new google.visualization.DataTable();
                data1.addColumn('string', 'Date');
                data1.addColumn('number', 'Qty.');
                for (var i = 0; i < grd.rows.length - 1; i++) {
                    data1.addRows([[Dt[i], parseInt(Qty[i])]]);
                }

                var options1 = { title: 'Datewise Qty.', vAxis: { title: "Quantity" }, 'pointSize': 3 };
                var chart1 = new google.visualization.LineChart(document.getElementById('chart_div1'));
                chart1.draw(data1, options1);
                // End Datewise Qty
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Booking
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
                                <table class="TableData" width="100%">
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
                                            &nbsp;<span class="TDCaption">To</span><asp:TextBox ID="txtReportUpto" runat="server"
                                                Width="80px" onkeypress="return false;" onpaste="return false;" />
                                            <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                                Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                                            </cc1:CalendarExtender>
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
                                            <asp:CheckBox ID="chkInvoice" runat="server" Text="Search By Invoice No." AutoPostBack="True"
                                                OnCheckedChanged="chkInvoice_CheckedChanged" ClientIDMode="Static" Visible="false" />
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="7" Visible="False" Width="100px"></asp:TextBox>
                                            <asp:CheckBox ID="chkCustomer" runat="server" Text="Customer business Report" AutoPostBack="True"
                                                OnCheckedChanged="chkCustomer_CheckedChanged" />
                                            <asp:TextBox ID="txtCustomerName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                                                Width="250px" Visible="false" OnTextChanged="txtCustomerName_TextChanged"></asp:TextBox>
                                            <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName"
                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                            </cc1:AutoCompleteExtender>
                                            <asp:CheckBox ID="chkHomePickup" runat="server" Text="Home Pickup Number" AutoPostBack="true"
                                                OnCheckedChanged="chkHomePickup_CheckedChanged" />
                                            <asp:TextBox ID="txtHomePickup" runat="server" MaxLength="9" Visible="false" Width="100px"></asp:TextBox>
                                            &nbsp; &nbsp;
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return CHeck();"
                                                OnClick="btnShowReport_Click" />
                                        </td>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="TDCaption" style="text-align: left">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                    <div class="DivStyleWithScroll" style="width: 1224Px; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                            ShowFooter="True" EmptyDataText="No record found" PageSize="50" CssClass="mGrid">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date / Time"
                                    SortExpression="BookingDate" FooterText="Total" />
                                <asp:TemplateField HeaderText="Order No.">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                            Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                        <asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("BookingStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="HNumber" HeaderText="Home Pickup No." SortExpression="HNumber"
                                    Visible="True"></asp:BoundField>
                                <asp:TemplateField HeaderText="Customer Detail" SortExpression="CustomerName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label></br>
                                        <asp:Label ID="lblCustomerAddress" runat="server" Text='<%# Bind("CustomerAdress") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                                    Visible="false"></asp:BoundField>
                                <asp:TemplateField HeaderText="Due Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DueDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliverDate" runat="server" Text='<%# Bind("DeliverDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Qty" HeaderText="Pcs." ItemStyle-HorizontalAlign="Right"
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
                                <asp:BoundField DataField="Paid" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="Paid">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DeliveryDiscount" HeaderText="D. Discount" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="DeliveryDiscount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DuePayment" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="DuePayment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="BookingAcceptedByUserId" HeaderText="Booked By" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="BookingAcceptedByUserId">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="WorkShopNote" HeaderText="WorkShop Note" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="WorkShopNote">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="OrderNote" HeaderText="Order Note" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="OrderNote">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Visible="true" OnClick="btnExport_Click" />
                    &nbsp; &nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="true" OnClick="btnPrint_Click" />
                    &nbsp; &nbsp;
                    <asp:Button ID="btnPrintStore" runat="server" Text="Store Print" Visible="true" OnClick="btnPrintStore_Click" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td width="50%" id="chart_div">
                </td>
                <td width="50%" id="chart_div1">
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
    <asp:HiddenField ID="hdnSelectedList" runat="server" />
    <asp:HiddenField ID="hdnDTOReportsBFlag" Value="false" runat="server" />
    <asp:HiddenField ID="hdnFirstSel" Value="No" runat="server" />
    <asp:HiddenField ID="hdnPassValue" runat="server" />
    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
    <asp:Label ID="lblCustomerCode" runat="server" Style="visibility: hidden"></asp:Label>
    </table>
</asp:Content>