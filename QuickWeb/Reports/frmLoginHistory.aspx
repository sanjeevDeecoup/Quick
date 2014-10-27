<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmLoginHistory.aspx.cs" Inherits="QuickWeb.Reports.frmLoginHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <link href="../css/loader.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                Login History Details
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">              
                <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor"><i class="fa fa-users fa-lg"></i></span>
                    <asp:TextBox ID="txtUserID" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                        MaxLength="100" OnTextChanged="txtUserID_TextChanged" CssClass="form-control"
                        placeholder="Filter by User ID"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtUserID"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetUserIDCompletionList" MinimumPrefixLength="1"
                        UseContextKey="true" ContextKey="All" CompletionInterval="10" CompletionSetCount="15"
                        FirstRowSelected="True" Enabled="True" CompletionListCssClass="AutoExtender_new"
                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor"><i class="fa fa-gavel fa-lg"></i></span>
                    <asp:TextBox ID="txtReason" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                        MaxLength="100" OnTextChanged="txtReason_TextChanged" CssClass="form-control"
                        placeholder="Filter by Reason"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtReason"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetReasonList" MinimumPrefixLength="1"
                        UseContextKey="true" ContextKey="All" CompletionInterval="10" CompletionSetCount="15"
                        FirstRowSelected="True" CompletionListCssClass="AutoExtender_new" CompletionListItemCssClass="AutoExtenderList_new"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor">Status</span>
                    <asp:DropDownList ID="drpActive" runat="server" OnSelectedIndexChanged="drpActive_SelectedIndexChanged"
                        CssClass="form-control" AutoPostBack="true">
                        <asp:ListItem Text="ALL" Value="All" />
                        <asp:ListItem Text="Success" Value="Success" />
                        <asp:ListItem Text="Deny" Value="Deny" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2 input-group" style="display:none">
                    <asp:LinkButton ID="btnShow" class="btn btn-primary" EnableTheming="false" runat="server"
                        ClientIDMode="Static" Visible="true" OnClick="btnShow_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                </div>
            </div>
            <br />
            <div class="row-fluid" style="height: 355px; background-color: White; overflow: auto">
                <asp:GridView ID="grdReport" runat="server" EmptyDataText="No Login history record found."
                    AutoGenerateColumns="false" PageSize="50" CssClass="Table Table-striped Table-bordered Table-hover"
                    EnableTheming="false">
                    <Columns>
                        <asp:BoundField DataField="userid" HeaderText="User ID" />
                        <asp:BoundField DataField="LoginDate" HeaderText="Date" />
                        <asp:BoundField DataField="LoginTime" HeaderText="Time" />
                        <asp:BoundField DataField="Success" HeaderText="Success" />
                        <asp:BoundField DataField="Reason" HeaderText="Reason" />
                    </Columns>
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" class="btn btn-primary" EnableTheming="false" runat="server"
                OnClick="btnExport_Click" ClientIDMode="Static" Visible="true"><i class="fa fa-file-excel-o"></i> Export to Excel</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
    </div>    
       <script type="text/javascript">
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
    </script>
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
</asp:Content>
