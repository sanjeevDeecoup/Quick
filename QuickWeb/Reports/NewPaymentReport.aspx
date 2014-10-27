<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="Reports_NewPaymentReport" CodeBehind="NewPaymentReport.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                Delivery and Sales
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body"  style="min-height:480px"> 
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
                <div class="form-group col-sm-2">
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
                <div class="form-group col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Delivery & Sales
                        </span>
                        <asp:DropDownList ID="drpSelectOption" runat="server"  CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">Delivery</asp:ListItem>
                            <asp:ListItem Value="2">Sales</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2">
                 <asp:LinkButton ID="btnShowReport" runat="server" OnClick="btnShowReport_Click" class="btn btn-primary"
                EnableTheming="false"><i class="fa fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
            <asp:LinkButton ID="btnPrint" runat="server" OnClick="btnPrint_Click" Visible="false"
                class="btn btn-primary" EnableTheming="false"><i class="fa fa-print fa-lg"></i>&nbsp;Print</asp:LinkButton>
                
                </div>



            </div>    
            <div class="row-fluid">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%"
                Font-Names="Verdana" Visible="false" Font-Size="8pt">
                <LocalReport ReportPath="RDLC/SalesDel.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ReportDelivery_SalesDel" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="ReportDeliveryTableAdapters.">
            </asp:ObjectDataSource>
        </div>      
        </div>           
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
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
    </div>
    <asp:HiddenField ID="hdnCustId" runat="server" />
    <asp:HiddenField ID="hdnDate" runat="server" />
    <asp:HiddenField ID="hdp_arr" runat="server" />
    <asp:HiddenField ID="hdd_arr" runat="server" />
    <asp:HiddenField ID="hdDateB" runat="server" />
    <asp:HiddenField ID="hdAmountB" runat="server" />
</asp:Content>
