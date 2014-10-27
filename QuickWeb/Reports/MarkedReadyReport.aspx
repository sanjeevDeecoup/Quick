<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="MarkedReadyReport.aspx.cs" Inherits="QuickWeb.Reports.MarkedReadyReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="QuickWeb" Namespace="QuickWeb.Controls" TagPrefix="ddChk" %>
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
                    <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                        Font-Bold="True" />
                    <asp:Label ID="lblErr" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                        Font-Bold="True" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Garment Ready By
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
                        <span class="input-group-addon IconBkColor">User</span>
                        <ddChk:DropDownCheckBoxList ID="ddlChkUsers" Width="160px" runat="server" Title="Select Users"
                            EnableTheming="false" CssClass="ddlchklst" ImageURL="/images/DropDown.PNG">
                        </ddChk:DropDownCheckBoxList>
                    </div>
                </div>
                <div class="form-group col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Garment</span>
                        <ddChk:DropDownCheckBoxList ID="ddlChkItems" runat="server" Width="160px" Title="Select Items"
                            CssClass="ddlchklst" EnableTheming="false" ImageURL="/images/DropDown.PNG">
                        </ddChk:DropDownCheckBoxList>
                    </div>
                </div>
                <div class="form-group col-sm-1 Textpadding">
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
                    <asp:TextBox ID="txtStartBkNo" runat="server" MaxLength="6" placeholder="Start" CssClass="form-control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBkNoStart_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtStartBkNo" ValidChars="1234567890">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-1 Textpadding" style="padding-left: 2px">
                    <asp:TextBox ID="txtEndBkNo" runat="server" MaxLength="6" placeholder="End" CssClass="form-control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBkNoEnd_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtEndBkNo" ValidChars="1234567890">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-2">
                    <asp:LinkButton ID="btnShowReport" runat="server" class="btn btn-primary" EnableTheming="false"
                        ClientIDMode="Static" OnClick="BtnShowReportClick"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid" style="height: 380px; background-color: White; overflow: auto">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="false" CssClass="Table Table-striped Table-bordered Table-hover"
                    EnableTheming="false" EmptyDataText="No garment ready by entry found." PageSize="50"
                    ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="Garment Ready By" DataField="UserName" FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField HeaderText="Order" DataField="BookingNumber" FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField HeaderText="Received From Workshop At" DataField="ReceivedFromWorkshopAt"
                            FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField HeaderText="Ready DateTime" DataField="ReadyAt" FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField HeaderText="Garment" DataField="Item" FooterStyle-BackColor="#E4E4E4" />
                        <asp:BoundField HeaderText="Entry By" DataField="EntryBy" FooterStyle-BackColor="#E4E4E4" />
                    </Columns>
                    <FooterStyle Font-Bold="true" />
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExportToExcel" runat="server" class="btn btn-primary" EnableTheming="false"
                OnClick="BtnExcel"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
            <asp:LinkButton ID="btnPrint" class="btn btn-primary" runat="server" Visible="false"
                OnClick="btnPrint_Click"><i class="fa fa-print fa-lg"></i>&nbsp;Print</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnFromDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnToDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnUsers" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnItems" runat="server" ClientIDMode="Static" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%"
            Font-Names="Verdana" Visible="False" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="RDLC\MarkedReadyReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="MarkedReady_Report" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="MarkedReadyTableAdapters.">
        </asp:ObjectDataSource>
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
    <script type="text/javascript">
        $(function () {

            $('.ddlchklst').find('ul').each(function (i, v) {
                $(v).find(':checkbox').eq(0).click();
            });

            $('body').on('click', function (e) {
                if ($(e.target).next().is('ul') || $(e.target).parent().is('li')) {
                    return;
                }
                $('.ddlchklst').find('ul').css('display', 'none');
            });

            $('#btnShowReport').on('click.Attached', clickHandler);

            function clickHandler() {

                var 
                    allUsers = '', allItems = '';

                if ($('.ddlchklst').eq(0).find(':checked').length === 0) {
                    // alert('Please select at least one user');
                    setDivMouseOver('Red', '#999999')
                    $('#lblMsg').text('Kindly select at least one user.');
                    return false;
                }

                if ($('.ddlchklst').eq(1).find(':checked').length === 0) {
                    setDivMouseOver('Red', '#999999')
                    $('#lblMsg').text('Kindly select at least one garment.');
                    return false;
                }

                $('.ddlchklst').eq(0).find(':checked').each(function (i, v) {
                    if (v.value === 'All') return;
                    allUsers += v.value + ':';
                });

                $('.ddlchklst').eq(1).find(':checked').each(function (i, v) {
                    if (v.value === 'All') return;
                    allItems += v.value + ':';
                });

                hdnUsers.value = allUsers.substr(0, allUsers.length - 1);
                hdnItems.value = allItems.substr(0, allItems.length - 1);
            }

        });
    </script>
</asp:Content>
