<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="frmCustomerSummary.aspx.cs" Inherits="QuickWeb.Reports.frmCustomerSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
       <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>       
    <script type="text/javascript">
        $(document).ready(function () {

            disableButton();

            var custList;

            $('#<%=btnCompare.ClientID%>').click(function () {
                if ($(':checkbox:checked').size() == 0) {
                }
                else {
                    custList = $(':checkbox:checked').map(function () { return $(this).closest('tr').find('.grdCustName').text() }).get();
                    $('#<%=hdnCustCodes.ClientID %>').val(custList);
                    //                alert(custList);
                    //                alert($('#<%=hdnCustCodesClient.ClientID %>').val());
                }
            });

            $("body").delegate(":checkbox", "change", function () {
                if ($('#<%=hdnFromCustomer.ClientID %>').val() == "true") {
                    custList = $(':checkbox:checked').map(function () { return $(this).closest('tr').find('.grdCustName').text() }).get();
                    //var PrevVal = $('#<%=hdnPrevCodes.ClientID %>').val();
                    //$('#<%=hdnPrevCodes.ClientID %>').val(PrevVal + ',' + custList);
                }
            });

            function gridviewCheck() {
                var grid = document.getElementById("<%= grdReport.ClientID %>");

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

            $('#<% =btnSms.ClientID%>').click(function () {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one customer.');                   
                    return false;
                }
            });

            $('body').click(function (e) {
                disableButton();
            });

            function disableButton() {
                if ($(':checkbox:checked').size() > 0) {
                    $('#ctl00_ContentPlaceHolder1_btnCompare').attr('disabled', false).removeClass('disabledClass');
                    $('#ctl00_ContentPlaceHolder1_btnReset').hide();
                    //   $('#ctl00_ContentPlaceHolder1_btnShowReport').show();

                }
                else {
                    $('#ctl00_ContentPlaceHolder1_btnCompare').attr('disabled', true).addClass('disabledClass');
                    $('#ctl00_ContentPlaceHolder1_btnReset').hide();
                    // $('#ctl00_ContentPlaceHolder1_btnShowReport').hide();
                }
            }

        });

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
    <div class="panel panel-primary">
        <div class="panel-heading">
            <h3 class="panel-title">
                <asp:Label ID="lblCaption2" runat="server" Font-Bold="true"></asp:Label>&nbsp;-&nbsp;<asp:Label
                    ID="lblReportDesc" Font-Size="13px" runat="server"></asp:Label>
                     <div id="reportrange" class="datefilter">
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate"
                        runat="server" clientidmode="Static"></span><b class="caret"> </b>
                </div>                    
                    </h3>
        </div>
        <div class="panel-body">          
            <div class="row-fluid">   
                 <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                    <asp:TextBox ID="txtCustomerName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                        placeholder="Filter by customer Name" OnTextChanged="txtCustomerName_TextChanged" CssClass="form-control"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                    </cc1:AutoCompleteExtender>
                </div>
                 <div class="col-sm-6">
                    <asp:LinkButton  ID="btnCompare" runat="server" OnClick="btnCompare_Click"
                        EnableTheming="false" OnClientClick="hello();" CssClass="Button btn btn-primary" ><i class="fa fa-calculator fa-lg"></i>&nbsp;Compare Selected</asp:LinkButton>
                        &nbsp;&nbsp;            
                    <asp:LinkButton ID="btnShowReport" runat="server" OnClientClick="return checkEntry();" style="display:none"
                        EnableTheming="false" CssClass="btn btn-primary" OnClick="btnShowReport_Click"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show all Customers</asp:LinkButton>                      
                    <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" EnableTheming="false"
                        Style="display: none" CssClass="btn btn-primary"><i class="fa fa-list-alt fa-lg"></i>&nbsp;Show All</asp:LinkButton>  
                         &nbsp;&nbsp;                      
                 <asp:LinkButton ID="btnSms" runat="server" EnableTheming="false" ClientIDMode="Static"
                        OnClick="btnSms_Click" CssClass="btn btn-primary"><i class="fa fa-envelope-o fa-lg"></i>&nbsp;Send Reminder</asp:LinkButton>
                </div>
            </div>           
            <br />
            <div class="row-fluid">
                <div class="" style="overflow: auto; height: 365px;">
                    <asp:UpdatePanel ID="upGrd" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                                DataKeyNames="CustomerCode" ShowFooter="True" EmptyDataText="No accounts receivable entry found."
                                EnableTheming="false" PageSize="50" CssClass="Table Table-striped Table-bordered Table-hover"
                                OnRowDataBound="grdReport_RowDataBound" AllowSorting="True" OnSorted="grdReport_Sorted"
                                OnSorting="grdReport_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" FooterStyle-BackColor="#E4E4E4">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" FooterStyle-BackColor="#E4E4E4">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustCodes" runat="server" Text='<%# Eval("CustomerCode") %>' CssClass="grdCustName"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="CustCode" ItemStyle-Width="0px" HeaderText="CustCode" SortExpression="CustCode"  
                                                            ItemStyle-CssClass="grdCustName" ControlStyle-Width="0px" ItemStyle-Font-Size="0" HeaderStyle-Width="0px"/>--%>
                                    <asp:TemplateField HeaderText="Name" SortExpression="CustomerName" FooterStyle-BackColor="#E4E4E4">
                                        <ItemTemplate>
                                            <asp:HyperLink Target="_blank" Text='<%# Eval("CustomerName") %>' runat="server"
                                                ID="hplNavigate">
                                                
                                                  <%--NavigateUrl='<%#String.Format("~/Reports/BookingByCustomerReport.aspx?BC={0}{1}{2}",Eval("CustCode"),,Eval("DueDate")) %>' >--%>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" />   --%>
                                    <asp:BoundField DataField="CustomerAddress" HeaderText="Address" SortExpression="CustomerAddress"
                                        FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile" SortExpression="CustomerMobile"
                                        FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                    <asp:BoundField DataField="AreaLocation" HeaderText="Area Location" SortExpression="AreaLocation"
                                        FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                    <asp:BoundField DataField="CustomerPreference" HeaderText="Prefrence" SortExpression=" CustomerPreference"
                                        FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                    <asp:BoundField DataField="Count" HeaderText="Orders" HeaderStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="Right" SortExpression="Count">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QTY" HeaderText="Pcs" HeaderStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="Right" SortExpression="Qty">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Volume" HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="Right" SortExpression="Volume">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Pending" HeaderText="Balance Pcs" HeaderStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="Right" SortExpression="Pending">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PendingAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="Right" SortExpression="PendingAmount">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FirstBill" HeaderText="First Visit" HeaderStyle-HorizontalAlign="left"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="left" SortExpression="FirstBill">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LastBill" HeaderText="Last Visit" HeaderStyle-HorizontalAlign="left"
                                        FooterStyle-BackColor="#E4E4E4" ItemStyle-HorizontalAlign="left" SortExpression="LastBill">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle Font-Bold="true" ForeColor="Black" />
                                <HeaderStyle Font-Size="12px" ForeColor="White" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" runat="server" Visible="False" EnableTheming="false"
                CssClass="btn btn-primary" OnClick="btnExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export to Excel</asp:LinkButton>
        </div>
        <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlSourceItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT [ItemID], [ItemName] FROM [ItemMaster] ORDER BY ItemName">
        </asp:SqlDataSource>
        <asp:Button ID="btnClear" Text="Clear" runat="server" OnClick="btnClear_Click" EnableTheming="false"
            Visible="false" CssClass="btn btn-primary" />
        <asp:DropDownList ID="drpsmstemplate" runat="server" CssClass="form-control" Visible="false">
        </asp:DropDownList>
    </div>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
    <asp:HiddenField ID="hdnCustCodes" runat="server" />
    <asp:HiddenField ID="hdnPrevCodes" runat="server" />
    <asp:HiddenField ID="hdnFromCustomer" runat="server" />
    <asp:HiddenField ID="hdnCustCodesClient" runat="server" />
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
                //dateLimit: { days: 60 },
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
