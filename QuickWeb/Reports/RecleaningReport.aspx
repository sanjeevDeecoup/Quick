<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="RecleaningReport" CodeBehind="RecleaningReport.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Duration" Src="~/Controls/DurationControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $('#ctl00_ContentPlaceHolder1_grdProcessSelection').on('click', ':checkbox', function (e) {
                if (e.target.id === 'ctl00_ContentPlaceHolder1_grdProcessSelection_ctl01_CheckAll') {
                    if (e.target.checked) {
                        $('#ctl00_ContentPlaceHolder1_grdProcessSelection').find(':checkbox').attr('checked', true);
                    }
                    else {
                        $('#ctl00_ContentPlaceHolder1_grdProcessSelection').find(':checkbox').attr('checked', false);
                    }
                }
            });

            $('#btnShowAll, #btnShowNotReady, #btnShowNotDelivered').on('click', function (e) {
                clearMsg();
                if ($('#ctl00_ContentPlaceHolder1_grdProcessSelection').find(':checked').length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one service.');
                    return false;
                }
            });

        });
    </script>
    <script type="text/javascript">
        function clearMsg() {
            $('#lblMsg').text('');
        }
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
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Garment Pending Status Report&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCustName" runat="server"
                    Font-Bold="true"></asp:Label>
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body well-sm-tiny" style="min-height: 500px">
            <div class="col-sm-9">
                <div class="row-fluid">
                    <div class="form-group col-sm-4">
                        <div class="input-group">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                            <asp:TextBox ID="txtCustomerName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                                placeholder="Filter by customer" CssClass="form-control" OnTextChanged="txtCustomerName_TextChanged">
                            </asp:TextBox>
                            <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtCustomerName"
                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                                CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                            </cc1:AutoCompleteExtender>
                        </div>
                    </div>
                    <div class="form-group col-sm-2 Textpadding">
                        <div class="input-group">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i>
                            </span>
                            <asp:DropDownList ID="drpBookingPreFix" runat="server" CssClass="form-control" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Text=" " Value=" " />
                                <asp:ListItem Text="A" Value="A" />
                                <asp:ListItem Text="B" Value="B" />
                                <asp:ListItem Text="C" Value="C" />
                                <asp:ListItem Text="D" Value="D" />
                                <asp:ListItem Text="E" Value="E" />
                                <asp:ListItem Text="F" Value="F" />
                                <asp:ListItem Text="G" Value="G" />
                                <asp:ListItem Text="H" Value="H" />
                                <asp:ListItem Text="I" Value="I" />
                                <asp:ListItem Text="J" Value="J" />
                                <asp:ListItem Text="K" Value="K" />
                                <asp:ListItem Text="L" Value="L" />
                                <asp:ListItem Text="M" Value="M" />
                                <asp:ListItem Text="N" Value="N" />
                                <asp:ListItem Text="O" Value="O" />
                                <asp:ListItem Text="P" Value="P" />
                                <asp:ListItem Text="Q" Value="Q" />
                                <asp:ListItem Text="R" Value="R" />
                                <asp:ListItem Text="S" Value="S" />
                                <asp:ListItem Text="T" Value="T" />
                                <asp:ListItem Text="U" Value="U" />
                                <asp:ListItem Text="V" Value="V" />
                                <asp:ListItem Text="W" Value="W" />
                                <asp:ListItem Text="X" Value="X" />
                                <asp:ListItem Text="Y" Value="Y" />
                                <asp:ListItem Text="Z" Value="Z" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-1 Textpadding" style="padding-left: 2px">
                        <asp:TextBox ID="txtStartBkNo" runat="server" placeholder="Start" CssClass="form-control"
                            MaxLength="7">
                        </asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtStartBkNo_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="txtStartBkNo" ValidChars="1234567890">
                        </cc1:FilteredTextBoxExtender>
                    </div>
                    <div class="col-sm-1 Textpadding" style="padding-left: 2px">
                        <asp:TextBox ID="txtEndBkNo" runat="server" placeholder="End" CssClass="form-control"
                            MaxLength="7">
                        </asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtEndBkNo_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="txtEndBkNo" ValidChars="1234567890">
                        </cc1:FilteredTextBoxExtender>
                    </div>
                    <div class="col-sm-3">
                        <div class="input-group labelBorder divheight backcolor2">
                            <span class="input-group-addon IconBkColor">
                                <asp:CheckBox ID="chkShowOnlyHome" runat="server" /></span>
                            <p class="textmargin5">
                                &nbsp; <span>Home Delivery &nbsp;</span>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="row-fluid">
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="btnShowAll" runat="server" OnClick="BtnShowClick" class="btn btn-primary"
                        EnableTheming="false" ClientIDMode="Static"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                    <asp:LinkButton ID="btnShowNotReady" runat="server" OnClick="BtnShowClick" class="btn btn-primary"
                        EnableTheming="false" ClientIDMode="Static"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Not Ready Garments</asp:LinkButton>
                    <asp:LinkButton ID="btnShowNotDelivered" runat="server" class="btn btn-primary" EnableTheming="false"
                        OnClick="BtnShowClick" ClientIDMode="Static"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Not Delivered Garments</asp:LinkButton>
                    <asp:LinkButton ID="btnPrint" runat="server" class="btn btn-primary" EnableTheming="false"
                        OnClick="btnPrint_Click" Visible="false"><i class="fa fa-print fa-lg"></i>&nbsp;Print</asp:LinkButton>
                    <asp:LinkButton ID="btnShowReport" runat="server" Visible="false" OnClick="btnShowReport_Click"><i class="fa fa-print fa-lg"></i>&nbsp;All Booking</asp:LinkButton>
                    <asp:LinkButton ID="btnUpdate" runat="server" Visible="false" OnClick="btnUpdate_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Remaining Bookings</asp:LinkButton>
                </div>
                <div class="row-fluid div-margin">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" Width="100%"
                        Height="100%" Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                        <LocalReport ReportPath="RDLC\TodayDeliveryReport.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="panel panel-info">
                    <div class="panel-heading" style="padding: 2px">
                        <span class="textBold">Select Service</span>
                    </div>
                    <div class="panel-body well-sm-tiny">
                        <div class="row-fluid">
                            <asp:GridView ID="grdProcessSelection" runat="server" AutoGenerateColumns="False"
                                EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CheckAll" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessCode" runat="server" Text='<%# Bind("ProcessCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:Label ID="lblQuantity" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblStoreName" runat="server" Visible="False"></asp:Label>
        <asp:HiddenField ID="hdnStartDate" runat="server" />
        <asp:HiddenField ID="hdnEndDate" runat="server" />
        <asp:HiddenField runat="server" ID="hdnCustCode" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var cb = function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
                $('#reportrange span').html(start.format('DD MMM YYYY') + ' - ' + end.format('DD MMM YYYY'));
                $('#hdnDateFromAndTo').val($('#spnReportDate').text());
                document.getElementById("<%=btnShowAll.ClientID %>").click();
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
