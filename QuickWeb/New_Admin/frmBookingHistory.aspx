<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmBookingHistory.aspx.cs" Inherits="QuickWeb.New_Admin.frmBookingHistory" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }          
    </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-primary well-sm-tiny1">
        <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="DivContainerStatus">
                <div id="DivContainerInnerStatus" class="span label label-default">
                    <h4 class="text-center">
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                    </h4>
                </div>
            </div>
        </div>
        <div class="panel-heading">
            <h3 class="panel-title">
                Editing History
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body well-sm-tiny">
            <div class="row-fluid">
                <div class="col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i>
                        </span>
                        <asp:TextBox ID="txtBookingNumber" CssClass="form-control" runat="server" MaxLength="20" ClientIDMode="Static"
                            placeholder="Search Order"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtBookingNumber_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="txtBookingNumber" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                        </cc1:FilteredTextBoxExtender>
                    </div>
                </div>
                <div class="col-sm-1 Textpadding">
                    <asp:LinkButton ID="btnShow" EnableTheming="False" runat="server" ClientIDMode="Static"
                        CssClass="btn btn-primary" OnClick="btnShow_Click"><i class="fa fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                </div>
                <div class="col-sm-2">
                 <asp:Label runat="server" ID="lblOrderName" Text="Order No :" ForeColor="Black" Font-Bold="True" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="lblOrderNo"></asp:Label>
                </div>
                <div class="col-sm-6" id="divScreenName" runat="server" visible="false">   
                 <div class="form-group col-sm-4">
                        <div class="input-group">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-users fa-lg"></i></span>
                            <asp:TextBox ID="txtUserID" runat="server" MaxLength="100" CssClass="form-control" onfocus="javascript:select();" 
                               OnTextChanged="txtUserID_TextChanged"  AutoPostBack="true" placeholder="Filter by User"></asp:TextBox>
                            <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtUserID"
                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetUserName" MinimumPrefixLength="1"
                                UseContextKey="true" ContextKey="All" CompletionInterval="10" CompletionSetCount="15"
                                FirstRowSelected="True" Enabled="True" CompletionListCssClass="AutoExtender_new"
                                CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                            </cc1:AutoCompleteExtender>
                        </div>
                    </div>             
                             <div class="col-sm-8">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Screen/ Activity</span>
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control" ClientIDMode="Static"
                            AutoPostBack="true" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Order Creation / Editing" Value="1"></asp:ListItem>
                            <asp:ListItem Text="workshop note creation" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Garments received at workshop" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Garments dispatched from workshop" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Garments received at store" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Garments marked ready for delivery" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Garments delivered or payment accepted" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Tags printed" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Order accessed through dashboard" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Delivery screen accessed through Pending Stock report" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Multiple orders delivered or payment accepted" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Order Cancellation" Value="12"></asp:ListItem>
                            <%--<asp:ListItem Text="Delete Order" Value="13"></asp:ListItem>--%>
                            <asp:ListItem Text="Order details opened from various reports/ screen" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Search order" Value="15"></asp:ListItem>
                            <asp:ListItem Text="Deliver order accessed through Home" Value="16"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    </div>
                   
                </div>
            </div>
            <div class="row-fluid div-margin">
                <div class="col-md-4" style="height: 410px; background-color: White; overflow: auto">
                    <asp:GridView ID="GrdEditHistoryBooking" runat="server" AutoGenerateColumns="false"
                        EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover"
                        ShowFooter="false" EmptyDataText="No edits have been done for this order.">
                        <Columns>
                            <asp:BoundField DataField="BookingNumber" HeaderText="Order No" SortExpression="BookingNumber">
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="BookingDate" HeaderText="Order Date" SortExpression="BookingDate">
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypBtnShowDetails" runat="server" Text="Edit History" CssClass="btn btn-info" style="padding:2px 3px 2px 3px"
                                        OnClick="hypBtnShowDetails_Click" CausesValidation="false" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle ForeColor="Black" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypBtnShowHistoryDetails" runat="server" Text="Order Transition" style="padding:2px 3px 2px 3px"
                                        CssClass="btn btn-info" OnClick="hypBtnShowHistoryDetails_Click" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Size="12px" />
                    </asp:GridView>
                </div>
                <div class="col-md-8 Textpadding" style="height: 410px; background-color: White; overflow: auto">
                    <asp:GridView ID="grdHistory" runat="server" OnRowDataBound="grdHistory_RowDataBound"
                        Visible="false" CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                        OnDataBinding="grdHistory_DataBinding" OnDataBound="grdHistory_DataBound" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label2" runat="server" Text="Revision"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRevisionNo" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingBackUpId" SortExpression="BookingBackUpId" HeaderText="Backup Booking Id"
                                Visible="true">
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:BoundField>                           
                            <asp:BoundField DataField="BookingAcceptedByUserId" SortExpression="BookingAcceptedByUserId"
                                HeaderText="Booked/Edited By">
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="BookingDate" SortExpression="BookingDate" HeaderText="Booked/Edited On">
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="EditBookingRemarks" SortExpression="EditBookingRemarks"
                                HeaderText="Editing Remarks">
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeader" runat="server">Compare</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hplNavigate" Target="_blank" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" />
                                <ItemStyle />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Size="12px" />
                    </asp:GridView>
                    <asp:GridView ID="grdInvoiceHistory" runat="server" EmptyDataText="No Invoice detail entry found."
                        Visible="false" OnRowDataBound="grdInvoiceHistory_RowDataBound" CssClass="Table Table-striped Table-bordered Table-hover"
                        EnableTheming="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%--<img src="../images/Close.jpg" width="15px" height="15px" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>                          
                            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="User">
                            </asp:BoundField>
                            <asp:BoundField DataField="ActionDate" SortExpression="ActionDate" HeaderText="Date" ItemStyle-Wrap="false">
                            </asp:BoundField>
                            <asp:BoundField DataField="ActionTime" SortExpression="ActionTime" HeaderText="Time" ItemStyle-Wrap="false">
                                <HeaderStyle Width="10%" />
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ScreenName" SortExpression="ScreenName" HeaderText="Screen Name">
                            </asp:BoundField>
                            <asp:BoundField DataField="ActionMsg" SortExpression="ActionMsg" HeaderText="Details">
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle Font-Size="12px" Font-Bold="true" />
                    </asp:GridView>
                </div>
                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Visible="false"
                    Text="Export to Excel" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnBookingNo" runat="server" />
    <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
    <!-- Start of js -->
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>    
    <script type="text/javascript">
        $(document).ready(function () {
            var cb = function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
                $('#reportrange span').html(start.format('DD MMM YYYY') + ' - ' + end.format('DD MMM YYYY'));
                $('#hdnDateFromAndTo').val($('#spnReportDate').text());
                document.getElementById("<%=btnShow.ClientID %>").click();
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
        <script type="text/javascript" language="javascript">
            $(document).ready(function (event) {

                $(document).keypress(function (event) {
                    var textval = $('#txtBookingNumber').val();
                    var keycode = (event.keyCode ? event.keyCode : event.which);
                    if (keycode == '13') {
                        if (textval == "") {
                            return false;
                        }
                        else {                           
                            document.getElementById("<%=btnShow.ClientID %>").click();
                        }
                    }
                });

            });
    </script>
</asp:Content>
