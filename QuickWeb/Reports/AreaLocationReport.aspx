<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="AreaLocationReport" Title="Untitled Page" CodeBehind="AreaLocationReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Duration" Src="~/Controls/DurationControlDateWise.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
     <script type="text/javascript" language="javascript">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                Area Location
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="form-group col-sm-4">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor"><i class="fa fa-building-o fa-lg"></i></span>
                        <asp:TextBox ID="txtAreaLocation" runat="server" AutoPostBack="true" onfocus="javascript:select();" ClientIDMode="Static"
                            placeholder="Filter by area location" OnTextChanged="txtAreaLocation_TextChanged"
                            CssClass="form-control"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtAreaLocation"
                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetDetailOfArealocation" MinimumPrefixLength="1"
                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>
            </div>
            <div id="divGrid" class="row-fluid" style="height: 400px; width:1200Px; background-color: White; overflow: auto; white-space:nowrap">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" EnableTheming="false"
                    DataKeyNames="BookingNumber" CssClass="Table Table-striped Table-bordered Table-hover"
                    ShowFooter="True" EmptyDataText="No record found" OnRowDataBound="grdReport_RowDataBound"
                    PageSize="50">
                    <Columns>
                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date / Time" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="BookingDate" FooterText="Total" />
                        <asp:TemplateField HeaderText="Order No." FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                    Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                <asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("BookingStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="CustomerName" HeaderText="Name" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="CustomerName">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>      
                        <asp:BoundField DataField="CustomerAdress" HeaderText="Address" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="CustomerAdress">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>                        
                         <asp:TemplateField HeaderText="Mobile" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area Location" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerArea" runat="server" Text='<%# Bind("AreaLocation") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                        <asp:BoundField DataField="Qty" HeaderText="Pcs." ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="Quantity">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalCost" HeaderText="Gross Amount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="TotalCost">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DiscountOnPayment" HeaderText="B. Discount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="DiscountOnPayment">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ST" HeaderText="Tax" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="ST">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="NetAmount">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PaymentMade" HeaderText="Advance" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="PaymentMade">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Paid" HeaderText="Paid" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="Paid">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DeliveryDiscount" HeaderText="D. Discount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="DeliveryDiscount">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DuePayment" HeaderText="Balance" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                            SortExpression="DuePayment">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>                       
                    </Columns>
                      <FooterStyle  Font-Bold="true" />
                            <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" runat="server" class="btn btn-primary" EnableTheming="false"
                OnClick="btnExport_Click" Visible="False"><i class="fa fa-file-excel-o"></i>&nbsp;Export to Excel</asp:LinkButton>
            <asp:Button ID="btnShowReport" runat="server" Text="Show"  style="display:none"
                OnClick="btnShowReport_Click" />
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustId" Value="" runat="server" />
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
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="hdnDTOReportsBFlag" Value="false" runat="server" />
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>
