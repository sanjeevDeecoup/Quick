<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="Pendingstockreport.aspx.cs" Inherits="Pendingstockreport" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_txtItemName').change(function (event) {
                clearMsg();
                var ItemName = $('#ctl00_ContentPlaceHolder1_txtItemName').val();
                var itemlength = ItemName.split("-");
                if (itemlength.length === 1) {
                    $('#ctl00_ContentPlaceHolder1_txtItemName').val('');
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('please select valid garment.');
                    $('#ctl00_ContentPlaceHolder1_txtItemName').focus();
                    return false;
                }
                __doPostBack('ctl00$ContentPlaceHolder1$txtItemName', null);

            });
        });
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
        function clearMsg() {
            $('#lblMsg').text('');
            $('#lblErr').text('');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                     <asp:Label ID="lblMsg" runat="server"  EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Pending Stock
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
                        <asp:TextBox ID="txtCName" runat="server" OnTextChanged="txtCName_TextChanged" placeholder="Filter by customer" onfocus="javascript:select();"
                            CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCName"
                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>
                <div class="form-group col-sm-4">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Search Garment</span>
                        <asp:TextBox ID="txtItemName" runat="server" 
                            placeholder="Search garment" onfocus="javascript:select();" CssClass="form-control"
                           ></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender2" TargetControlID="txtItemName"
                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>
                <div class="form-group col-sm-2">               
                 <asp:LinkButton ID="btnClear" runat="server" class="btn btn-primary" EnableTheming="false"
                        OnClick="btnClear_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Clear</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid" style="height: 400px; background-color: White; overflow: auto">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                    CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                    ShowFooter="True" EmptyDataText="There is no pending order" PageSize="50">
                    <Columns>
                     <asp:TemplateField HeaderText="Order No" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                    Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/Delivery.aspx?BN={0}",Eval("BookingNumber")) %>' />
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="BookingDate" />
                        <asp:BoundField DataField="DueDate" DataFormatString="{0:d}" HeaderText="Due Date"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="DueDate" />                       
                        <asp:TemplateField HeaderText="Name" FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="CustomerMobile">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="CustomerAddress" HeaderText="Address" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="CustomerAddress">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="item" HeaderText="Garment" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="item">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="Status">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField> 
                        <asp:BoundField DataField="DeliveryMsg" HeaderText="Delivery Remark" SortExpression="DeliveryMsg"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                    </Columns>
                       <FooterStyle  Font-Bold="true" />
                            <HeaderStyle Font-Size="12px" />
                </asp:GridView>
                <asp:GridView ID="Gridexcel" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                    CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                    ShowFooter="True" EmptyDataText="No record found" PageSize="50">
                    <Columns>
                        <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Booking Date"
                            FooterStyle-BackColor="#E4E4E4" SortExpression="BookingDate" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:TemplateField HeaderText="Booking No." FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblDatno" runat="server" Text='<%# Bind("BookingNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerName." FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="item" HeaderText="Item" SortExpression="item" FooterStyle-BackColor="#E4E4E4">
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" FooterStyle-BackColor="#E4E4E4">
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No" SortExpression="CustomerMobile"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                        <asp:BoundField DataField="CustomerAddress" HeaderText="Address" SortExpression="CustomerAddress"
                            FooterStyle-BackColor="#E4E4E4">
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DeliveryMsg" HeaderText="Delivery Msg" SortExpression="DeliveryMsg"
                            FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" runat="server" class="btn btn-primary" EnableTheming="false"
                OnClick="btnExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
            <asp:Button ID="btnReset" Text="Show" runat="server" OnClick="btnReset_Click" ClientIDMode="Static"
                Style="visibility: hidden" />
                    <asp:DropDownList ID="drpItemNames" runat="server" Width="150px" Visible="false">
    </asp:DropDownList>
    <asp:DropDownList ID="drpdats" runat="server" Visible="false">
        <asp:ListItem Selected="True" Value="1">Older All</asp:ListItem>
        <asp:ListItem Value="2">Older 30</asp:ListItem>
        <asp:ListItem Value="3">Older 60</asp:ListItem>
        <asp:ListItem Value="4">Older 90</asp:ListItem>
        <asp:ListItem Value="5">Older 180</asp:ListItem>
        <asp:ListItem Value="6">Older 360</asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox ID="txtInvoiceNo" runat="server" Visible="false"></asp:TextBox>
    <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClick="btnShowReport_Click"  Style="visibility: hidden"
         />    
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnFromDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnToDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustId" runat="server" />
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
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>
