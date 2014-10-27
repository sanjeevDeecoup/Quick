<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="New_Booking_Multiplepayment" CodeBehind="Multiplepayment.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.23.custom.css" />
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function checkKey(e) {
            var targ;
            var code;
            if (!e) var e = window.event;
            if (e.target) targ = e.target;
            if (e.keyCode) code = e.keyCode;
            else if (e.which) code = e.which;
            else if (e.srcElement) targ = e.srcElement;
            if (code == 13) {
                document.getElementById("ctl00_ContentPlaceHolder1_btnCheckGridBox").click();
            }
        }
        function clearMsg() {
            $('#lblErr').text('');
            $('#lblMsg').text('');
        }
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
    </script>
    <script type="text/javascript">

        function Check_Click(objRef) {

            //Get the Row based on checkbox

            var row = objRef.parentNode.parentNode;

            if (objRef.checked) {

                //If checked change color to Aqua

                row.style.backgroundColor = "red";

            }

            else {

                //If not checked change back to original color

                if (row.rowIndex % 2 == 0) {

                    //Alternating Row Color

                    row.style.backgroundColor = "#C2D69B";

                }

                else {

                    row.style.backgroundColor = "white";

                }

            }



            //Get the reference of GridView

            var GridView = row.parentNode;



            //Get all input elements in Gridview

            var inputList = GridView.getElementsByTagName("input");



            for (var i = 0; i < inputList.length; i++) {

                //The First element is the Header Checkbox

                var headerCheckBox = inputList[0];



                //Based on all or none checkboxes

                //are checked check/uncheck Header Checkbox

                var checked = true;

                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                    if (!inputList[i].checked) {

                        checked = false;

                        break;

                    }

                }

            }

            headerCheckBox.checked = checked;



        }

    </script>
    <script type="text/javascript">

        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;

            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;

                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {

                        //If the header checkbox is checked

                        //check all checkboxes

                        //and highlight all rows

                        row.style.backgroundColor = "red";

                        inputList[i].checked = true;

                    }

                    else {

                        //If the header checkbox is checked

                        //uncheck all checkboxes

                        //and change rowcolor back to original

                        if (row.rowIndex % 2 == 0) {

                            //Alternating Row Color

                            row.style.backgroundColor = "#C2D69B";

                        }

                        else {

                            row.style.backgroundColor = "white";

                        }

                        inputList[i].checked = false;

                    }

                }

            }

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
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <h3 class="panel-title">
                Multiple Delivery and Payment
                <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="col-sm-2 input-group">
                   <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i>
                    </span>
                    <asp:TextBox ID="txtSearchInoviceNo" runat="server" onkeyPress="return checkKey(event);"
                        placeholder="Search Order" AutoPostBack="True" OnTextChanged="txtSearchInoviceNo_TextChanged"
                        CssClass="form-control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtSearchInoviceNo_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtSearchInoviceNo" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-3 input-group">
                     <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                    <asp:TextBox ID="txtCName" runat="server" OnTextChanged="txtCName_TextChanged" placeholder="Filter by customer Name"
                        CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCName"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor"><i class="fa fa-question-circle fa-lg">
                    </i></span>
                    <asp:DropDownList ID="drpSelectOption" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="1">Pending Garment</asp:ListItem>
                        <asp:ListItem Value="2">Pending Payment</asp:ListItem>
                        <asp:ListItem Value="3">Garment and Payment</asp:ListItem>                         
                          <asp:ListItem Value="4">Outstanding Garment Only</asp:ListItem>
                          <asp:ListItem Value="5">Outstanding Payment Only</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-4">
                    <asp:LinkButton ID="btnShowReport" runat="server" class="btn btn-primary" OnClick="btnShowReport_Click">
                  <i class="fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                    <asp:LinkButton ID="btnDeliverClothes" runat="server" class="btn btn-primary" Visible="false"
                        ClientIDMode="Static" OnClick="btnDeliverClothes_Click"><i class="fa fa-check-circle-o fa-lg"></i>&nbsp;Deliver Clothes</asp:LinkButton>
                    <asp:LinkButton ID="btnAcceptPayment" runat="server" Visible="false" class="btn btn-primary"
                        ClientIDMode="Static" OnClick="btnAcceptPayment_Click"><i class="fa fa-check-circle-o fa-lg"></i>&nbsp;Accept Payment</asp:LinkButton>
                    <asp:LinkButton ID="btnDeliverAndPayment" runat="server" Visible="false" class="btn btn-primary"
                        ClientIDMode="Static" OnClick="btnDeliverAndPayment_Click"><i class="fa fa-check-circle-o fa-lg"></i>&nbsp;Deliver and Accept</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid div-margin">
                <div id="divGrid" style="overflow: auto; height: 365px;">
                    <asp:GridView ID="Grdpayment" runat="server" ClientIDMode="Static" AutoGenerateColumns="false"
                        EmptyDataText="No multiple delivery and payment entry found." CssClass="Table Table-striped Table-bordered Table-hover"
                        ShowFooter="true" EnableTheming="false">
                        <Columns>
                            <asp:TemplateField FooterStyle-BackColor="#E4E4E4">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order No." FooterStyle-BackColor="#E4E4E4">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hypBtnShowDetails" runat="server" NavigateUrl='<%# String.Format("~/Bookings/Delivery.aspx?BN={0}",Eval("BookingNumber")) %>'
                                        Target="_blank" Text='<%# Bind("BookingNumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingDate" HeaderText="Order Date" SortExpression="BookingDate"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="DeliveryDate" HeaderText="Due Date" SortExpression="DeliveryDate"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Details" SortExpression="CustomerName"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="TotalQty" HeaderText="Total Qty" SortExpression="TotalQty"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="ReadyClothes" HeaderText="Ready Qty" SortExpression="ReadyClothes"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="AlreadyDelivered" HeaderText="Del Qty" SortExpression="AlreadyDelivered"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="BalQty" HeaderText="Bal Qty" SortExpression="BalQty" FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="TotalPay" HeaderText="Total Payment" SortExpression="TotalPay"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="AllReadyPaid" HeaderText="Payment Received" SortExpression="AllReadyPaid"
                                FooterStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="Balance" HeaderText="Balance Payment" SortExpression="Balance"
                                FooterStyle-BackColor="#E4E4E4" />
                        </Columns>
                        <FooterStyle Font-Bold="true" />
                        <HeaderStyle Font-Size="12px" />
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click"
                class="btn btn-primary" Visible="False"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustId" Value="0" runat="server" />
        <asp:HiddenField ID="hdnFromDate" Value="0" runat="server" />
        <asp:HiddenField ID="hdnToDate" Value="0" runat="server" />
        <div class="modal fade" id="pnlCopyRateList" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-dialog-center">
                <div class="modal-content">
                    <div class="panel panel-primary popup2">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Payment Details
                                <button type="button" class="close" data-dismiss="modal">
                                    <span aria-hidden="true"><i class="fa fa-times-circle"></i></span><span class="sr-only">
                                        Close</span></button>
                            </h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid">
                                <div class="col-sm-1">
                                </div>
                                <div class="col-sm-10">
                                    <div class="row-fluid">
                                        <div class="input-group">
                                            <span class="input-group-addon IconBkColor"><i class="fa fa-credit-card fa-lg"></i>&nbsp;
                                                <asp:Label ID="Label3" Text="Payment Mode" runat="server"></asp:Label></span>
                                            <asp:DropDownList ID="drpPaymentType" runat="server" ClientIDMode="Static" CssClass="form-control">
                                                <asp:ListItem Text="Cash" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Credit Card/Debit Card"></asp:ListItem>
                                                <asp:ListItem Text="Cheque/Bank"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="input-group">
                                            <span class="input-group-addon IconBkColor"><i class="fa fa-file-text fa-lg"></i>&nbsp;Payment
                                                Details</span>
                                            <asp:TextBox ID="txtPaymentDetails" runat="server" MaxLength="100" CssClass="form-control"
                                                TextMode="MultiLine" placeholder="Payment Details"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtPaymentDetails_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" FilterMode="InvalidChars" InvalidChars="'" TargetControlID="txtPaymentDetails">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <asp:LinkButton ID="btnAccountDetails" runat="server" ClientIDMode="Static" class="btn btn-primary"
                                            OnClick="btnAccountDetails_Click"><i class="fa fa-check-circle-o"></i>&nbsp;Accept Payment</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:CheckBox ID="chkPrevious" runat="server" Text=" Include earlier Pending Clothes."
        CssClass="Legend" Style="visibility: hidden" />
    <asp:Button ID="btnCheckGridBox" runat="server" OnClick="btnCheckGridBox_Click" Style="visibility: hidden" />
    <script type="text/javascript">
        $(function (e) {

            function gridviewCheck() {
                var grid = document.getElementById("<%= Grdpayment.ClientID %>");
                //variable to contain the cell of the grid
                var cell;
                var count = 0;
                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (i = 1; i < grid.rows.length; i++) {

                        //get the reference of first column
                        cell = grid.rows[i].cells[0];
                        //loop according to the number of childNodes in the cell
                        for (j = 0; j < cell.childNodes.length; j++) {
                            //if childNode type is CheckBox  
                            if (cell.childNodes[j].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell checkbox within the grid
                                if (cell.childNodes[j].checked == true) {
                                    count = +1;
                                }
                            }
                        }
                    }
                }
                return count;
            }

            $('#btnAcceptPayment,#btnDeliverAndPayment').click(function (e) {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one order.');
                    return false;
                }
                //  $('#pnlCopyRateList').dialog({ width: 350, modal: true }).parent().appendTo($('form:first'));
                $('#pnlCopyRateList').modal('toggle');
                $('#drpPaymentType').focus();
                return false;
            });

            $('#btnDeliverClothes').click(function (e) {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one order.');
                    return false;
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
