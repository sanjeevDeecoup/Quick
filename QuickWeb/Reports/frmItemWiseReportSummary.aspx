<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="frmItemWiseReportSummary" Title="Untitled Page" CodeBehind="frmItemWiseReportSummary.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            $('#ctl00_ContentPlaceHolder1_btnDisplay').click(function (e) {
                clearMsg();
                if ($('#ctl00_ContentPlaceHolder1_grdReport tr').size() <= 3) {
                    //  alert('not enough data');
                    setDivMouseOver('Red', '#999999')
                    $('#lblMsg').text('Not enough data to display record.');
                    return false;
                }
            });
        });
    
    </script>
    <script type="text/javascript" language="javascript">
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
        function clearMsg() {           
            $('#lblMsg').text('');           
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                      <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                            Font-Bold="True" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Item Wise Summary Report
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
                        <span class="input-group-addon IconBkColor">Item Name</span>
                        <asp:DropDownList ID="drpItemNames" runat="server" DataSourceID="SqlSourceItems"
                            CssClass="form-control" DataTextField="ItemName" DataValueField="ItemID" MaxLength="0"
                            AppendDataBoundItems="true">
                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        </asp:DropDownList>
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
                    <asp:TextBox ID="txtBkNoStart" runat="server" MaxLength="6" placeholder="Start" CssClass="form-control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBkNoStart_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtBkNoStart" ValidChars="1234567890">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-1 Textpadding" style="padding-left: 2px">
                    <asp:TextBox ID="txtBkNoEnd" runat="server" MaxLength="6" placeholder="End" CssClass="form-control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBkNoEnd_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtBkNoEnd" ValidChars="1234567890">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-3 form-group">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Item Name</span>
                        <asp:TextBox ID="txtItemName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                            CssClass="form-control" OnTextChanged="txtItemName_TextChanged" placeholder="Filter by Item name"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtItemName"
                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:LinkButton ID="btnShowReport" runat="server" class="btn btn-primary" EnableTheming="false"
                        OnClick="btnShowReport_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid" style="height: 400px; background-color: White; overflow: auto">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemName"
                    EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover"
                    EmptyDataText="There is no order for selected item." PageSize="50" OnRowDataBound="grdReport_RowDataBound">
                    <Columns>
                        <asp:TemplateField FooterStyle-BackColor="#E4E4E4">
                            <ItemTemplate>
                                <asp:HyperLink Target="_blank" Text='<%#Eval("ItemName") %>' ID="hplProcessLink"
                                    runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="QTY" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4" SortExpression="QTY">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4" SortExpression="AMOUNT">
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
            <asp:LinkButton ID="btnExport" runat="server" Text="Export to Excel" Visible="False"
                class="btn btn-primary" EnableTheming="false" OnClick="btnExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
            <asp:LinkButton ID="btnDisplay" runat="server" Visible="true" class="btn btn-primary"
                EnableTheming="false" OnClick="btnDisplay_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Display Details</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnStartDate" runat="server" />
        <asp:HiddenField ID="hdnEndDate" runat="server" />
        <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlSourceItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT [ItemID], [ItemName] FROM [ItemMaster] ORDER BY ItemName">
        </asp:SqlDataSource>
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
</asp:Content>
