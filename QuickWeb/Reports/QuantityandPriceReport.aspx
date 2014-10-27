<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="QuantityandPriceReport" Title="Untitled Page" CodeBehind="QuantityandPriceReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">     
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>    
    <script type="text/javascript">

        $(document).ready(function () {           

            $("body").delegate(":checkbox", "change", function (event) {
                // if the id is 'chkAll' then select all the ids
                //  if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll') {
                if ($(event.target).attr('id') == 'CheckAll') {
                    // alert('Its all');
                    list3 = $(':checkbox:checked').not(document.getElementById('ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll')).map(function () {
                        return $(this).closest('tr').find('td').eq(1).text().substring(0, 11) + ':' + $(this).closest('tr').find('a').text();
                    }).get();
                    // alert(list3);
                    if ($('#CheckAll').prop('checked')) {
                        $('#<%=hdnTempInvoice.ClientID %>').val('1');
                    } else {
                        $('#<%=hdnTempInvoice.ClientID %>').val('0');
                    }
                }
                // else if ($(event.target).attr('id') != 'ctl00_ContentPlaceHolder1_chkInvoice') {
                else {
                    list3 = $(':checkbox:checked').not(document.getElementById('ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll')).map(function () {
                        return $(this).closest('tr').find('td').eq(1).text().substring(0, 11) + ':' + $(this).closest('tr').find('a').text();
                    }).get();
                    //  alert(list3);
                }
                //alert(list3);
                $('#<%=hdnSelectedList.ClientID %>').text(list3);
                $('#<%=hdnSelectedList.ClientID %>').val(list3);
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

            $('#<% =btnPrint.ClientID%>').click(function () {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one order.');
                    return false;
                }                
            });

            $('#<% =btnPrintStore.ClientID%>').click(function () {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one order.');
                    return false;
                }
            });
            $('#<% =btnPrintOrderPreview.ClientID%>').click(function () {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one order.');
                    return false;
                }
            });

            $('#ctl00_ContentPlaceHolder1_txtCustomerName').change(function (event) {
                var CustCode = $('#ctl00_ContentPlaceHolder1_txtCustomerName').val();
                var CustCode1 = CustCode.split("-");
                if (CustCode1.length === 1) {
                    $('#ctl00_ContentPlaceHolder1_txtCustomerName').val('');
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select a valid customer.');
                    $('#ctl00_ContentPlaceHolder1_txtCustomerName').focus();
                    return false;
                }
                __doPostBack('ctl00$ContentPlaceHolder1$txtCustomerName', null);
            });
        });
   
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            HideGridColumn();
            function HideGridColumn() {
                $('#achrExpand').show();
                $('#achrshrink').hide();
                $("#divGrid").css("width", '').css("white-space", "normal");
                $("table[id*=grdReport] tr").each(function (index, item) {
                    $(item).children("th:eq(6), td:eq(6)").hide();
                    $(item).children("th:eq(8), td:eq(8)").hide();                  
                    $(item).children("th:eq(12), td:eq(12)").hide();
                    $(item).children("th:eq(13), td:eq(13)").hide();
                    $(item).children("th:eq(14), td:eq(14)").hide();
                    $(item).children("th:eq(15), td:eq(15)").hide();
                    $(item).children("th:eq(16), td:eq(16)").hide();
                    $(item).children("th:eq(18), td:eq(18)").hide();
                    $(item).children("th:eq(19), td:eq(19)").hide();
                    $(item).children("th:eq(20), td:eq(20)").hide();
                    $(item).children("th:eq(21), td:eq(21)").hide();
                    $(item).children("th:eq(22), td:eq(22)").hide();
                });
            }
            $('#achrExpand').click(function () {
                $('#achrExpand').hide();
                $('#achrshrink').show();
                var screenWidth = $(window).width();
                screenWidth = (screenWidth - 60) + 'px';
                $("#divGrid").css("width", screenWidth).css("white-space", "nowrap");
                $("table[id*=grdReport] tr").each(function (index, item) {
                    $(item).children("th:eq(6), td:eq(6)").show();
                    $(item).children("th:eq(8), td:eq(8)").show();                  
                    $(item).children("th:eq(12), td:eq(12)").show();
                    $(item).children("th:eq(13), td:eq(13)").show();
                    $(item).children("th:eq(14), td:eq(14)").show();
                    $(item).children("th:eq(15), td:eq(15)").show();
                    $(item).children("th:eq(16), td:eq(16)").show();
                    $(item).children("th:eq(18), td:eq(18)").show();
                    $(item).children("th:eq(19), td:eq(19)").show();
                    $(item).children("th:eq(20), td:eq(20)").show();
                    $(item).children("th:eq(21), td:eq(21)").show();
                    $(item).children("th:eq(22), td:eq(22)").show();
                });
            });

            $('#achrshrink').click(function () {
                HideGridColumn();
            });
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
    <script type="text/javascript">
        $(document).ready(function () {
//            var screenWidth = $(window).width();
//            screenWidth = (screenWidth - 60) + 'px';
          //  $("#divGrid").css("width", screenWidth);
        });
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
                    Booking Report
 <div id="reportrange"  class="datefilter" >
                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;<span id="spnReportDate" runat="server" clientidmode="Static"></span><b class="caret">
                    </b>
                </div>
                &nbsp;&nbsp;
                  <span  style="float:right;cursor: pointer;display:none" class="margin4 HoverClass" id="achrExpand">
                       &nbsp;Expand &nbsp;</span>
                        <span  style="float:right;cursor: pointer;display:none" class="margin4 HoverClass" id="achrshrink">
                       &nbsp;Shrink &nbsp;</span>
                    </h3>
            </div>
            <div class="panel-body">
             <%--   <div class="row-fluid">
                    <uc:Duration ID="uc1" runat="server"></uc:Duration>
                </div>--%>
                <div class="row-fluid">

                  <div class="form-group col-sm-2">
                        <div class="input-group">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i></span>
                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="7" CssClass="form-control"
                                OnTextChanged="txtInvoiceNo_TextChanged" AutoPostBack="true" placeholder="Search Order"></asp:TextBox>
                        </div>
                    </div>
                     <div class="form-group col-sm-3">
                        <div class="input-group">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                            <asp:TextBox ID="txtCustomerName" runat="server" onfocus="javascript:select();" 
                                placeholder="Filter by customer" CssClass="form-control"></asp:TextBox>
                            <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName"
                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                                CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                            </cc1:AutoCompleteExtender>
                        </div>
                    </div>
                    <div class="form-group col-sm-2">
                        <div class="input-group">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-users fa-lg"></i></span>
                            <asp:TextBox ID="txtUserID" runat="server" MaxLength="100" CssClass="form-control" onfocus="javascript:select();" 
                                OnTextChanged="txtUserID_TextChanged" AutoPostBack="true" placeholder="Filter by User"></asp:TextBox>
                            <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtUserID"
                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetUserName" MinimumPrefixLength="1"
                                UseContextKey="true" ContextKey="All" CompletionInterval="10" CompletionSetCount="15"
                                FirstRowSelected="True" Enabled="True" CompletionListCssClass="AutoExtender_new"
                                CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                            </cc1:AutoCompleteExtender>
                        </div>
                    </div>

                    <div class="col-sm-2">
                        <div class="input-group input-sm Textpadding">
                            <span class="input-group-addon IconBkColor">
                                <asp:Label ID="txtBookingDelivery" runat="server" Text="Discount"></asp:Label></span>
                            <asp:DropDownList ID="drpDiscount" runat="server" CssClass="form-control">
                                <asp:ListItem Value="1" Selected="True">All</asp:ListItem>
                                <asp:ListItem Value="2">Booking</asp:ListItem>
                                <asp:ListItem Value="3">Delivery</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                     <div class="col-sm-2">
                     <div class="input-group labelBorder divheight backcolor2">
                <span class="input-group-addon IconBkColor">
                    <asp:CheckBox ID="chkShowOnlyHome" runat="server" /></span>
                <p class="textmargin5">
                    &nbsp; <span>Home Delivery &nbsp;</span>
                </p>
            </div>
            </div>
                    <div class="col-sm-1 Textpadding">                  
                        <asp:LinkButton ID="btnShowReport" runat="server" OnClientClick="return CHeck();"
                            class="btn btn-primary" OnClick="btnShowReport_Click"><i class="fa fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                    </div>
                </div>            
                <div class="row-fluid">
                    <div id="divGrid" style="overflow: auto; height: 365px;">
                        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                            EnableTheming="false" OnRowDataBound="grdReport_RowDataBound" OnDataBound="grdReportDataBound"
                            ShowFooter="True" EmptyDataText="No order found" ClientIDMode="Static" PageSize="50"
                            CssClass="Table Table-striped Table-bordered Table-hover">
                            <Columns>
                                <asp:TemplateField FooterStyle-BackColor="#E4E4E4">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date / Time"
                                    SortExpression="BookingDate" FooterText="Total" FooterStyle-BackColor="#E4E4E4" />
                                <asp:TemplateField HeaderText="Order No." FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                            Target="_blank" NavigateUrl="" />
                                        <asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("BookingStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" SortExpression="CustomerName" FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustomerAdress" HeaderText="Address" SortExpression="CustomerName"
                                    FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                <asp:BoundField DataField="CustomerMobile" HeaderText="Phone" SortExpression="CustomerMobile"
                                    FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                <asp:BoundField DataField="CustomerPreference" HeaderText="Preference" SortExpression="CustomerPreference"
                                    FooterStyle-BackColor="#E4E4E4"></asp:BoundField>
                                <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                                    FooterStyle-BackColor="#E4E4E4" Visible="false"></asp:BoundField>
                                <asp:TemplateField HeaderText="Due Date" FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DueDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                                    FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliverDate" runat="server" Text='<%# Bind("DeliverDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Qty" HeaderText="Pcs." ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="Quantity">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalCost" HeaderText="Gross Amount" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="TotalCost">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DiscountOnPayment" HeaderText="Discount" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="DiscountOnPayment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ST" HeaderText="Tax" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="ST">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="NetAmount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PaymentMade" HeaderText="Advance" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="PaymentMade">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Paid" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="Paid">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DeliveryDiscount" HeaderText="D. Discount" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="DeliveryDiscount">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DuePayment" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="DuePayment">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="BookingAcceptedByUserId" HeaderText="Booked By" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="BookingAcceptedByUserId">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="WorkShopNote" HeaderText="WorkShop Note" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="WorkShopNote">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="OrderNote" HeaderText="Order Note" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="OrderNote">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="HomeDelivery" HeaderText="Home Delivery" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="HomeDelivery">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="AreaLocation" HeaderText="Area Location" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-BackColor="#E4E4E4" SortExpression="AreaLocation">
                                    <FooterStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#F00F0" Font-Bold="true" />
                            <HeaderStyle Font-Size="12px" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="panel-footer">
                <span id="btnRight" runat="server" visible="false">
                  
                    <asp:LinkButton ID="btnPrint" class="btn btn-primary" runat="server" Visible="true"
                        OnClick="btnPrint_Click"><i class="fa fa-print  fa-lg"></i>&nbsp;Print Order Tickets</asp:LinkButton>
                    <asp:LinkButton ID="btnPrintOrderPreview" runat="server" class="btn btn-primary"
                        Visible="true" OnClick="btnPrintOrderPreview_Click"><i class="fa fa-binoculars"></i>&nbsp;Preview Order Tickets</asp:LinkButton>
                    <asp:LinkButton ID="btnPrintStore" runat="server" Visible="true" class="btn btn-primary"
                        OnClick="btnPrintStore_Click"><i class="fa fa-print fa-lg"></i>&nbsp;Print Store Copy</asp:LinkButton>
                    <%--<asp:Button ID="btnDirectPrint" runat="server" Text="Print Report" />--%>
                    <asp:LinkButton ID="btnPrintReport" runat="server" class="btn btn-primary" OnClick="btnPrintReport_Click"><i class="fa fa-print fa-lg"></i>&nbsp;Print Report</asp:LinkButton>
                    <asp:LinkButton ID="btnPrintSummary" runat="server" class="btn btn-primary" OnClick="BtnPrintSummaryClick"><i class="fa fa-print fa-lg">&nbsp;</i>Print garment details</asp:LinkButton>
                      <asp:LinkButton ID="btnExport" class="btn btn-primary" runat="server" Visible="true"
                        OnClick="btnExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export to Excel</asp:LinkButton>
                </span>
            </div>
        </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="100%" Height="100%">
            <LocalReport ReportPath="RDLC\BookingReport.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:HiddenField ID="hdnPrintValue" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnStartDate" runat="server" />
        <asp:HiddenField ID="hdnEndDate" runat="server" />
        <asp:HiddenField ID="hdnSelectedList" runat="server" />
        <asp:HiddenField ID="hdnDTOReportsBFlag" Value="false" runat="server" />
        <asp:HiddenField ID="hdnFirstSel" Value="No" runat="server" />
        <asp:HiddenField ID="hdnPassValue" runat="server" />
        <asp:HiddenField ID="hdnTempInvoice" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnFromDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnToDate" runat="server" ClientIDMode="Static" />        
        <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
        <asp:Label ID="lblCustomerCode" runat="server" Style="visibility: hidden"></asp:Label>
        <iframe id="frmPrint" name="PrintDirect" width="500" height="200" runat="server"
            style="display: none;"></iframe>
             <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
            
            <script type="text/javascript">
                $(document).ready(function () {

                    var cb = function (start, end, label) {
                        console.log(start.toISOString(), end.toISOString(), label);
                        $('#reportrange span').html(start.format('DD MMM YYYY') + ' - ' + end.format('DD MMM YYYY'));
                        $('#hdnDateFromAndTo').val($('#spnReportDate').text());                      
                        document.getElementById("<%=btnShowReport.ClientID %>").click();
                        // alert("Callback has fired: [" + start.format('DD MMM YYYY') + " to " + end.format('DD MMM YYYY') + ", label = " + label + "]");
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

                    //  $('#reportrange span').html(moment().subtract(0, 'days').format('DD MMM YYYY') + ' - ' + moment().format('DD MMM YYYY'));
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
