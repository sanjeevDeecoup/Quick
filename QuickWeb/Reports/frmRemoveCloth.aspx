<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="Reports_frmRemoveCloth" Title="Untitled Page" CodeBehind="frmRemoveCloth.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblMsg" runat="server" CssClass="ErrorMessage" EnableViewState="False"></asp:Label>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Removed Garments
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
                        <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                        <asp:TextBox ID="txtCName" runat="server" OnTextChanged="txtCName_TextChanged" placeholder="Filter by customer"
                            CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCName"
                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>
                <div class="form-group col-sm-2">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i>
                        </span>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="7" placeholder="Search Order"
                            CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group col-sm-4">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Remove Cloth</span>
                        <asp:DropDownList ID="drpSelectOption" runat="server"  CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:LinkButton ID="btnShowReport" runat="server" OnClick="btnShowReport_Click" class="btn btn-primary"
                        EnableTheming="false"><i class="fa fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid" style="height: 400px; background-color: White; overflow: auto">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" EmptyDataText="No remove garment record found."
                    PageSize="50" AllowSorting="True" EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover"
                    OnSorting="grdReport_Sorting" OnRowDataBound="grdReport_RowDataBound">
                    <Columns>                       
                        <asp:TemplateField HeaderText="Order No." FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNo") %>'
                                    Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/DeliverySlip.aspx?RS=0&BN={0}{1}{2}",Eval("BookingNo"),-1,Eval("ClothDeliveryDate")) %>' />
                                <asp:HiddenField ID="hdnDeliveryDate" runat="server" Value='<%# Bind("ClothDeliveryDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="BookingDate" HeaderText="Order Date" SortExpression="BookingDate"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="ClothDeliveryDate" HeaderText="Returned Date" SortExpression="ClothDeliveryDate" ItemStyle-ForeColor="Red"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="RemoveFrom" HeaderText=" Returned From" SortExpression="RemoveFrom"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="Item" HeaderText=" Garment" SortExpression="Item"></asp:BoundField>
                        <asp:BoundField DataField="DeliverItemStaus" HeaderText="Remark To Return" SortExpression="DeliverItemStaus"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="BookingAcceptedByUserId" HeaderText="Returned By" SortExpression="BookingAcceptedByUserId"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                    </Columns>
                    <FooterStyle Font-Bold="true" />
                    <HeaderStyle Font-Size="12px" ForeColor="White" />
                </asp:GridView>
                <asp:GridView ID="grdReport1" runat="server" AutoGenerateColumns="False" Visible="false"
                    EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover">
                    <Columns>
                        <asp:BoundField DataField="BNO" HeaderText="Sr.No" SortExpression="BNO" FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField DataField="BookingNo" HeaderText="Booking No." SortExpression="BookingNo"
                            FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="BookingDate" HeaderText="Booking Date" SortExpression="BookingDate"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="ClothDeliveryDate" HeaderText="Remove Date" SortExpression="ClothDeliveryDate"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="RemoveFrom" HeaderText="Remove From" SortExpression="RemoveFrom"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="Item" HeaderText="Item" SortExpression="Item"></asp:BoundField>
                        <asp:BoundField DataField="DeliverItemStaus" HeaderText="Reason to remove" SortExpression="DeliverItemStaus"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="BookingAcceptedByUserId" HeaderText="User Id" SortExpression="BookingAcceptedByUserId"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                    </Columns>
                    <FooterStyle Font-Bold="true" />
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" class="btn btn-primary"
                EnableTheming="false"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
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
    <asp:HiddenField ID="hdnCustId" runat="server" />
    <asp:HiddenField ID="hdnDate" runat="server" />
        <asp:HiddenField ID="hdnStartDate" runat="server" />
        <asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>
