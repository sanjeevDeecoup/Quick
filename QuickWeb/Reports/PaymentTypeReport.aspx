<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="PaymentTypeReport" CodeBehind="PaymentTypeReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                Payment Type
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
                <span style="float:right;margin-top:-7px;"> <asp:LinkButton ID="btnShowReport" runat="server" class="btn btn-info" EnableTheming="false"
                            OnClick="btnShowReport_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                        <asp:LinkButton ID="btnPrint" runat="server" Visible="false" class="btn btn-info"
                            EnableTheming="false" OnClick="btnPrint_Click"><i class="fa fa-print fa-lg"></i>&nbsp;Print</asp:LinkButton>&nbsp;&nbsp;</span>
            </h3>
        </div>
        <div class="panel-body well-sm-tiny" style="min-height: 500px">
            <div class="col-sm-9">               
                <div class="row-fluid div-margin">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Height="100%"
                        Width="100%">
                        <LocalReport ReportPath="RDLC/PaymentTypeReport.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="PaymentTypeDataSet_sp_Payment" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="Copy" TypeName="PaymentTypeDataSet">
                    </asp:ObjectDataSource>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="panel panel-info">
                    <div class="panel-heading" style="padding: 2px">
                        <span class="textBold">Select Process</span>
                    </div>
                    <div class="panel-body well-sm-tiny">
                        <div class="row-fluid">
                            <asp:GridView ID="grdProcessSelection" runat="server" AutoGenerateColumns="False"
                                EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessName" runat="server" Text='<%# Bind("PaymentType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                  <HeaderStyle Font-Size="12px" />
                            </asp:GridView>
                        </div>
                        <div class="row-fluid">
                            <asp:GridView ID="grdSecondGrid" runat="server" AutoGenerateColumns="False" EnableTheming="false"
                                CssClass="Table Table-striped Table-bordered Table-hover">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcessName" runat="server" Text='<%# Bind("PaymentType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                  <HeaderStyle Font-Size="12px" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnStartDate" runat="server" />
        <asp:HiddenField ID="hdnEndDate" runat="server" />
        <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] ORDER BY [ProcessName]">
        </asp:SqlDataSource>
    </div>
    <script type="text/javascript">
        $(function () {

            $('#ctl00_ContentPlaceHolder1_grdProcessSelection').on('change', ':checkbox', function (e) {
                // if a single is checked
                if ($('#ctl00_ContentPlaceHolder1_grdProcessSelection').find(':checked').length !== 0) {
                    $('#ctl00_ContentPlaceHolder1_grdSecondGrid').find(':checkbox').each(function (i, v) {
                        v.checked = false;
                    });
                }
            });

            $('#ctl00_ContentPlaceHolder1_grdSecondGrid').on('change', ':checkbox', function (e) {
                // if a single is checked
                if ($('#ctl00_ContentPlaceHolder1_grdSecondGrid').find(':checked').length !== 0) {
                    $('#ctl00_ContentPlaceHolder1_grdProcessSelection').find(':checkbox').each(function (i, v) {
                        v.checked = false;
                    });
                }
            });

        });
    </script>
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
