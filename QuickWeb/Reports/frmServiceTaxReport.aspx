<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmServiceTaxReport.aspx.cs" Inherits="QuickWeb.Reports.frmServiceTaxReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="Label2" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
               Service Tax Report
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="form-group col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Report Type
                           </span>
                        <asp:DropDownList ID="drpReportFrom" runat="server" OnSelectedIndexChanged="drpReportFrom_SelectedIndexChanged" CssClass="form-control"
                                                AutoPostBack="true">
                                                <asp:ListItem Text="Booking" />
                                                <asp:ListItem Text="Delivery" />
                                                <asp:ListItem Text="Summarised Booking"></asp:ListItem>
                                                <asp:ListItem Text="Summarised Delivery"></asp:ListItem>
                                            </asp:DropDownList>
                    </div>
                </div>               
            </div>
            <div class="row-fluid" style="height: 400px; background-color: White; overflow: auto">
                        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber" CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                            ShowFooter="True" EmptyDataText="No service tax entry found." PageSize="50" OnRowDataBound="grdReport_RowDataBound"
                            OnDataBinding="grdReport_DataBinding" OnDataBound="grdReport_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Order No." FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                            Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DeliveryDate")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustCode" HeaderText="Customer Details" SortExpression="CustCode" FooterStyle-BackColor="#E4E4E4">
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Order Date" FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="DeliveryDate">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GrossAmt" HeaderText="Gross Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="GrossAmt">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BookingAmount" HeaderText="Taxable Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="BookingAmount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="AmtBeforeAnything" HeaderText="Amt Before Tax & Dis" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="AmtBeforeAnything">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Discountamt" HeaderText="Discount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Discountamt">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Payment" HeaderText="Payment" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Payment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NetAmtDelivery" HeaderText="Net Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="NetAmtDelivery">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ServiceTax" HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="ServiceTax">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Ecess" HeaderText="Ecess" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Ecess">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField> 
                                <asp:BoundField DataField="SHECess" HeaderText="SHECess" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="SHECess">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="AdujustedAmt" HeaderText="Adujusted Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="AdujustedAmt">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="BalanceDue" HeaderText="Balance Due" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="BalanceDue">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="TotalAmount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Status">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                              <FooterStyle  Font-Bold="true" />
                            <HeaderStyle Font-Size="12px" />
                        </asp:GridView>
                        <asp:GridView ID="grdReportSummary" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdReport_RowDataBound" CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                            ShowFooter="True" EmptyDataText="No service tax entry found." PageSize="50">
                            <Columns>
                                 <asp:BoundField DataField="Month" HeaderText="Order Month" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Month">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                              <asp:BoundField DataField="BookingDate" HeaderText="Order Date" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="BookingDate">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Discount">
                                    <FooterStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="GrossAmt" HeaderText="Gross Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="GrossAmt">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="BookingAmount" HeaderText="Taxable Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="BookingAmount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="AmtBeforeAnything" HeaderText="Amt Before Tax & Dis" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="AmtBeforeAnything">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="Payment" HeaderText="Payment" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Payment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                 
                                 <asp:BoundField DataField="NetAmtDelivery" HeaderText="NetAmtDelivery" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="NetAmtDelivery">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Discount" HeaderText="Discount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Discount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="ServiceTax" HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="ServiceTax">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="Ecess" HeaderText="Ecess" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="Ecess">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="SHECess" HeaderText="SHECess" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    SortExpression="SHECess"> 
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                </Columns>
                                  <FooterStyle Font-Bold="true" />
                            <HeaderStyle Font-Size="12px" />
                            </asp:GridView>
                
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" runat="server" class="btn btn-primary" EnableTheming="false"
                OnClick="btnExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
                 <asp:LinkButton ID="btnPrint" class="btn btn-primary" runat="server"  
                        OnClick="btnPrint_Click"><i class="fa fa-print fa-lg"></i>&nbsp;Print</asp:LinkButton>  
                 <asp:LinkButton ID="btnShowReport" runat="server" class="btn btn-primary" EnableTheming="false" style="visibility:hidden"
                        OnClick="btnShowReport_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
         <asp:HiddenField ID="hdnFromDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnToDate" runat="server" ClientIDMode="Static" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT BookingDate, SUM(NetAmount) FROM [ViewBookingReport]">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] ORDER BY [ProcessName]">
        </asp:SqlDataSource>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var cb = function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
                $('#reportrange span').html(start.format('DD MMM YYYY') + ' - ' + end.format('DD MMM YYYY'));
                $('#hdnDateFromAndTo').val($('#spnReportDate').text());
                document.getElementById("<%=btnShowReport.ClientID %>").click();
            }
            var strTmpDate = $('#hdnDateFromAndTo').val().split('-');
            var optionSet1 = {
                startDate: moment(strTmpDate[0]),
                endDate: moment(strTmpDate[1]),
                minDate: '01/01/2010',
                maxDate: '12/31/2050',
                // dateLimit: { days: 60 },
                showDropdowns: true,
                showWeekNumbers: true,
                timePicker: false,
                timePickerIncrement: 1,
                timePicker12Hour: true,
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'This Year': [moment().startOf('year'), moment().endOf('year')]                    
                },
                opens: 'left',
                buttonClasses: ['btn btn-default'],
                applyClass: 'btn-small btn-primary',
                cancelClass: 'btn-small',
                format: 'MM/DD/YYYY',
                separator: ' to ',
                locale: {
                    applyLabel: 'Submit',
                    cancelLabel: 'Clear',
                    fromLabel: 'From',
                    toLabel: 'To',
                    customRangeLabel: 'Date Range',
                    daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                    monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                    firstDay: 1
                }
            };
            var optionSet2 = {
                startDate: moment().subtract(7, 'days'),
                endDate: moment(),
                opens: 'left',
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                }
            };
            $('#reportrange span').html($('#hdnDateFromAndTo').val());
            $('#reportrange').daterangepicker(optionSet1, cb);

            $('#reportrange').on('show.daterangepicker', function () { console.log("show event fired"); });
            $('#reportrange').on('hide.daterangepicker', function () { console.log("hide event fired"); });
            $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
                console.log("apply event fired, start/end dates are "
                      + picker.startDate.format('MMMM D, YYYY')
                      + " to "
                      + picker.endDate.format('MMMM D, YYYY')
                    );
            });
            $('#reportrange').on('cancel.daterangepicker', function (ev, picker) { console.log("cancel event fired"); });

            $('#options1').click(function () {
                $('#reportrange').data('daterangepicker').setOptions(optionSet1, cb);
            });

            $('#options2').click(function () {
                $('#reportrange').data('daterangepicker').setOptions(optionSet2, cb);
            });

            $('#destroy').click(function () {
                $('#reportrange').data('daterangepicker').remove();
            });
        });
       </script>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" Width="100%"
                                    Height="100%" Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                                    <LocalReport ReportPath="RDLC\BookingServiceTax.rdlc">
                                    </LocalReport>
                                </rsweb:ReportViewer>

<asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>
