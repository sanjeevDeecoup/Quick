<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="CancelQuantityandPriceReport" Title="Untitled Page" CodeBehind="CancelQuantityandPriceReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Duration" Src="~/Controls/DurationControl.ascx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>--%>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var screenWidth = $(window).width();
            screenWidth = (screenWidth - 60) + 'px';
            $("#divGrid").css("width", screenWidth);
        });
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_btnDirectPrint').click(function (event) {
                var PrnValue = $('#hdnPrintValue').val();
                SetPrintOption(PrnValue);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Cancel Booking
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="form-group col-sm-2">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i>
                        </span>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" placeholder="Search Order" CssClass="form-control"
                            MaxLength="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                        <asp:TextBox ID="txtCustomerName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                            placeholder="Filter by customer" CssClass="form-control" OnTextChanged="txtCustomerName_TextChanged"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtCustomerName"
                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="input-group labelBorder divheight backcolor2">
                        <span class="input-group-addon IconBkColor">
                            <asp:CheckBox ID="chkShowOnlyHome" runat="server" /></span>
                        <p class="textmargin5">
                            &nbsp; <span>Home Delivery &nbsp;</span>
                        </p>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:LinkButton ID="btnShowReport" runat="server" Text="Show" class="btn btn-primary"
                        EnableTheming="false" OnClick="btnShowReport_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid" id="divGrid" style="height: 365px; width: 1100px; background-color: White;
                overflow: auto; white-space: nowrap">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                    CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                    ShowFooter="True" EmptyDataText="No cancel booking entry found." PageSize="50"
                    OnRowDataBound="grdReport_RowDataBound">
                    <Columns>                       
                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date / Time"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="BookingDate" FooterText="Total" />
                        <asp:TemplateField HeaderText="Order No." FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                    Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                <asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("BookingStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CancelDate" DataFormatString="{0:d}" HeaderText="Canceled On" ItemStyle-ForeColor="Red"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="CancelDate" FooterText="" />
                        <asp:BoundField DataField="CancelReason" HeaderText="Remark" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="CancelReason" FooterText="" />
                        <asp:TemplateField HeaderText="Customer Detail" SortExpression="CustomerName" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                -
                                <asp:Label ID="lblCustomerAddress" runat="server" Text='<%# Bind("CustomerAdress") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                            FooterStyle-BackColor="#E4E4E4" Visible="false"></asp:BoundField>
                        <asp:TemplateField HeaderText="Due Date" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DueDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblDeliverDate" runat="server" Text='<%# Bind("DeliverDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Qty" HeaderText="Pcs." ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="Quantity">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalCost" HeaderText="Gross Amount" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="TotalCost">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DiscountOnPayment" HeaderText="Discount" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="DiscountOnPayment">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ST" HeaderText="Tax" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="ST">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="NetAmount">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PaymentMade" HeaderText="Advance" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="PaymentMade">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Paid" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="Paid">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DeliveryDiscount" HeaderText="D. Discount" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="DeliveryDiscount">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DuePayment" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="DuePayment">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="BookingAcceptedByUserId" HeaderText="Booked By" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="BookingAcceptedByUserId">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="WorkShopNote" HeaderText="WorkShop Note" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="WorkShopNote">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="OrderNote" HeaderText="Order Note" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="OrderNote">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="HomeDelivery" HeaderText="Home Delivery" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="HomeDelivery">
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
                Visible="true" OnClick="btnExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
            <asp:LinkButton ID="btnPrintReport" runat="server" class="btn btn-primary" EnableTheming="false"
                OnClick="btnPrintReport_Click" Visible="false"><i class="fa fa-print fa-lg"></i>&nbsp;Print Report</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />     
      
        <div id="divPrint" style="display: none">
            <asp:GridView ID="gvUserInfo" runat="server" ShowFooter="true" BorderStyle="Solid"
                BorderWidth="2px">
                <HeaderStyle BackColor="#3CA3C1" Font-Bold="true" ForeColor="White" />
            </asp:GridView>
        </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="100%" Height="100%">
            <LocalReport ReportPath="RDLC\BookingReport.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    <asp:HiddenField ID="hdnPrintValue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
    <asp:HiddenField ID="hdnSelectedList" runat="server" />
    <asp:HiddenField ID="hdnDTOReportsBFlag" Value="false" runat="server" />
    <asp:HiddenField ID="hdnFirstSel" Value="No" runat="server" />
    <asp:HiddenField ID="hdnPassValue" runat="server" />
    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
    <asp:Label ID="lblCustomerCode" runat="server" Style="visibility: hidden"></asp:Label>
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
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
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
</asp:Content>
