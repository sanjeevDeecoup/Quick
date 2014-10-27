﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeBehind="WorkshopOut.aspx.cs"
    Inherits="QuickWeb.Factory.WorkshopOut" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/loader.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="panel panel-primary well-sm-tiny1">
<div class="panel-heading">
<h3 class="panel-title"><span class="textBold">Workshop Out</span> 
  <a class="btn btn-info margin4 " runat="server" style="float:right;margin-top:-7px" clientidmode="Static" id="achruserDetail">
                        <i class="fa fa-th"></i>&nbsp;&nbsp;User Selected Garment</a> 
</h3>
</div>
<div class="panel-body well-sm-tiny">

    <div class="container-fluid margin-left-right">
        <div class="row-fluid">
            <div class="span10">
                <div class="row-fluid">
                    <div class="span3">
                        <asp:TextBox ID="txtBarcode" runat="server" placeholder="Barcode / Order No" EnableTheming="False"
                            ClientIDMode="Static" AutoComplete="off" OnTextChanged="txtBarcode_TextChanged"
                            CssClass="span12 form-control" MaxLength="20"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtBarcode_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="txtBarcode" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                        </cc1:FilteredTextBoxExtender>
                    </div>
                    <div class="span3">
                        <asp:DropDownList ID="drpBranch" runat="server" DataSourceID="SDTBranch" DataTextField="BranchName"
                            CssClass="span12 form-control" AppendDataBoundItems="true" DataValueField="BranchId"
                            AutoPostBack="True" OnSelectedIndexChanged="drpBranch_SelectedIndexChanged" EnableTheming="False">
                            <asp:ListItem Text="All - Branch" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="span3">
                        <asp:TextBox ID="txtBookingDate" runat="server" placeholder="Booking Date" EnableTheming="False"
                            onpaste="return false;" onkeypress="return false;" ClientIDMode="Static" AutoComplete="off"
                            CssClass="span10 form-control" OnTextChanged="txtBarcode_TextChanged"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtHolidayDate_CalendarExtender" runat="server" Enabled="True"
                            Format="dd-MMM-yyyy" TargetControlID="txtBookingDate">
                        </cc1:CalendarExtender>
                    </div>
                    <div class="span3">
                        <asp:TextBox ID="txtDelDate" runat="server" placeholder="Delivery Date" EnableTheming="False"
                            OnTextChanged="txtBarcode_TextChanged" onpaste="return false;" onkeypress="return false;"
                            ClientIDMode="Static" AutoComplete="off" CssClass="span10 form-control"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MMM-yyyy"
                            TargetControlID="txtDelDate">
                        </cc1:CalendarExtender>
                    </div>
                </div>
                <div class="row-fluid div-margin">
                    <div class="span3">
                        <asp:TextBox ID="txtWorkShopNote" runat="server" placeholder="Enter Workshop Remarks"
                            CssClass="span12 form-control" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="span2 form-inline">
                        <asp:CheckBox ID="chkUrgent" runat="server" Text="&nbsp;Show Urgent&nbsp;&nbsp;&nbsp;"
                            AutoPostBack="true" ClientIDMode="Static" OnCheckedChanged="chkUrgent_CheckedChanged" />
                    </div>
                    <div class="span2 form-inline">
                        <asp:CheckBox ID="chkPrintSticker" runat="server" Text="&nbsp;&nbsp;Print Stickers"
                            CssClass="span12" ClientIDMode="Static" />
                    </div>
                    <div class="span5 form-inline">
                        <div class="form-group">
                            <asp:CheckBox ID="chkPrintChallan" runat="server" Text="&nbsp;&nbsp;Print Delivery Note"
                                CssClass="span12" ClientIDMode="Static" />
                        </div>
                        <div class="form-group" style="display:none">
                            &nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-info" id="achrClearFilter" runat="server"
                                clientidmode="static">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Clear Filter &nbsp;&nbsp;&nbsp;&nbsp;</a>
                        </div>
                    </div>
                </div>
        </div>
        <div class="span2">
            <div class="row-fluid">
                <a class="span12 btn btn-primary" id="btnSave" runat="server" clientidmode="static">
                    <i class="fa fa-check icon-white"></i>&nbsp;Send to Store</a>
            </div>
            <div class="row-fluid" style="margin-top: 3px">
                <a class="span12 btn btn-primary" id="btnPrint" runat="server" clientidmode="static">
                    <i class="fa fa-print icon-white"></i>&nbsp;Send and Print</a>
            </div>
        </div>
    </div>
    <div class="row-fluid bottom-margin-small div-margin">
        <asp:Literal ID="ltDynamic" runat="server"></asp:Literal>
        <div class="row-fluid bottom-margin-small" id="staticDiv" runat="server" clientidmode="static">
            <div class="span1 well well-small bottom-margin-small">
                <div class="span label label-default aligncenter">
                    <h4 id="lblLeft">
                        &nbsp;
                    </h4>
                </div>
            </div>
            <div class="span10 well well-small bottom-margin-small" id="DivContainerStatus">
                <div class="span label label-default aligncenter">
                    <h4 id="lblStatusCloth" runat="server">
                        &nbsp;
                    </h4>
                </div>
            </div>
            <div class="span1 well well-small bottom-margin-small">
                <div class="span label label-default aligncenter">
                    <h4 id="lblQtyCount">
                        &nbsp;
                    </h4>
                </div>
            </div>
        </div>
        <!-- Div Container for GRid Start Here -->
        <div class="row-fluid form-signin4 no-bottom-margin">
            <div class="span6 well well-small no-bottom-margin">
                <div class="DivStyleWithScroll" style="overflow: scroll; height: 400px;">
                    <asp:GridView ID="grdNewChallan" runat="server" CssClass="mgrid" AutoGenerateColumns="False"
                        ShowFooter="False" EmptyDataText="There are no pending Cloths to send." EnableTheming="false"
                        OnDataBinding="grdNewChallan_DataBinding" OnDataBound="grdNewChallan_DataBound"
                        OnRowDataBound="grdNewChallan_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ControlStyle-CssClass="rowNumber">
                                <HeaderTemplate>
                                    <asp:Label ID="Label2" runat="server" Text="RowNumber"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRowNumber"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingNumber" HeaderText="Order" ReadOnly="True" SortExpression="BookingNumber">
                            </asp:BoundField>
                            <asp:BoundField DataField="BookingDeliveryDate" HeaderText="Due Date" ReadOnly="True"
                                SortExpression="BookingDeliveyDate"></asp:BoundField>
                            <asp:BoundField DataField="Barcode" HeaderText="Barcode" ReadOnly="True"></asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ReadOnly="True" SortExpression="BranchName">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Cloth" SortExpression="ItemName">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="U">
                                <ItemTemplate>
                                    <asp:Label ID="lblUrgent" runat="server" Text='<%# Bind("IsUrgent") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service">
                                <ItemTemplate>
                                    <asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Receive in WorkShop">
                                <ItemTemplate>
                                    <asp:Label ID="lblDatetime" runat="server" Text='<%# Bind("BookingTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="span1 well well-small  gridhight-noscroll no-bottom-margin">
                <div class="spacer">
                </div>
                <div class="btn btn-default btn-sm btn-block disabled">
                    Move
                </div>
                <div class="btn btn-info btn-sm btn-block" id="btnMoveRight">
                    <i class="fa fa-chevron-right fa-lg icon-white"></i>
                </div>
                <div class="btn btn-info btn-sm btn-block" id="btnMoveLeft">
                    <i class="fa fa-chevron-left fa-lg icon-white"></i>
                </div>
                <div class="spacer">
                </div>
                <div class="btn btn-default btn-sm btn-block disabled" id="lblMoveAllLbl"  runat="server" clientidmode="Static">
                    Move All</div>
                <div class="btn btn-info btn-sm btn-block" id="btnMoveRightAll" runat="server" clientidmode="Static">
                    <i class="fa fa-chevron-right fa-lg icon-white"></i><i class="fa fa-chevron-right fa-lg icon-white">
                    </i>
                </div>
                <div class="btn btn-info btn-sm btn-block" id="btnMoveLeftAll" runat="server" clientidmode="Static">
                    <i class="fa fa-chevron-left fa-lg icon-white"></i><i class="fa fa-chevron-left fa-lg icon-white">
                    </i>
                </div>
                <div class="spacer">
                </div>
            </div>
            <div class="span5 well well-small no-bottom-margin">
                <div class="DivStyleWithScroll" style="overflow: scroll; height: 400px;">
                    <asp:GridView ID="grdSelectedCloth" runat="server" CssClass="mgrid" AutoGenerateColumns="False"
                        OnRowDataBound="grdSelectedCloth_RowDataBound" EmptyDataText="There are no pending Cloths to send."
                        EnableTheming="false" ShowFooter="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ControlStyle-CssClass="rowNumber">
                                <HeaderTemplate>
                                    <asp:Label ID="Label2" runat="server" Text="RowNumber"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRowNumber"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingNumber" HeaderText="Order" ReadOnly="True" SortExpression="BookingNumber">
                            </asp:BoundField>
                            <asp:BoundField DataField="BookingDeliveryDate" HeaderText="Due Date" ReadOnly="True"
                                SortExpression="BookingDeliveyDate"></asp:BoundField>
                            <asp:BoundField DataField="Barcode" HeaderText="Barcode" ReadOnly="True"></asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ReadOnly="True" SortExpression="BranchName">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Cloth" SortExpression="ItemName">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="U">
                                <ItemTemplate>
                                    <asp:Label ID="lblUrgent" runat="server" Text='<%# Bind("IsUrgent") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service">
                                <ItemTemplate>
                                    <asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Receive in WorkShop">
                                <ItemTemplate>
                                    <asp:Label ID="lblDatetime" runat="server" Text='<%# Bind("BookingTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:Panel ID="pnlPanel" runat="server" CssClass="modalPopup" Style="display: none;
                margin-right: 15px;" ClientIDMode="Static" BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png"
                Width="450px">
                <div class="popup_Titlebar" id="Div8">
                    <div class="TitlebarLeft">
                        Print Packing Sticker
                    </div>
                </div>
                <div class="popup_Body">
                    <table class="TableData">
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                <asp:Label ID="LabelLbl" Text="Start Printing from Sticker Number " runat="server"></asp:Label>&nbsp;&nbsp;
                                <asp:DropDownList ID="drpPrintStartFrom" runat="server" ClientIDMode="Static" Width="100px">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnSaveAndPrintNew" Text="Print" runat="server" OnClick="btnSaveAndPrintNew_Click" />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:SqlDataSource ID="SDTBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                SelectCommand="SELECT [BranchId], [BranchName] FROM [BranchMaster] where IsFactory='False'">
            </asp:SqlDataSource>
        </div>
        <asp:HiddenField runat="server" ID="hdnAddedHeader" Value="false" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnRemovedEmptyMessage" Value="false" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllData" Value="false" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnBookingCount" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllBookingNumber" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllBookingCount" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllCheckBoxLeft" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllRowMoveNumFromLTR" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllRowMoveNumFromRTL" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnLTRPrevCount" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnRTLPrevCount" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnEmptyRow" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnAllHtml" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCloseMe" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnRmvReason" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnRmvReasonData" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnDynamicStores" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnStartValue" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCurrentStoreIdName" ClientIDMode="Static" />
    </div>
    <a class="span12 btn btn-primary" id="btnRefresh" runat="server" clientidmode="static"
        style="visibility: hidden"><i class="icon-refresh icon-white"></i>&nbsp;Refresh</a>
    <asp:Panel ID="pnlMsg" runat="server" Style="display: none" ClientIDMode="Static">
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div>
                    <br />
                    <span class="fa  textBold">Please Wait..</span>
                    <img src="../images/ajax-loader.gif" style="margin-top: 5px; margin-left: 25%" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    </div>
    </div>
</div>
    <!-- Start of js -->
    <%--<script src="../js/loader.js" type="text/javascript"></script>--%>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jBeep.js" type="text/javascript"></script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../JavaScript/code.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/jBeep.min.js"></script>
    <script src="../js/JSuccess.js" type="text/javascript"></script>
    <script src="../js/tooltip.js" type="text/javascript"></script>  
    <script type="text/javascript">

        $(document).ready(function () {
            if ($('#hdnCloseMe').val() == 'true') {
                var win = window.open("about:blank", "_self");
                win.close();
            }
            $('#hdnAddedHeader').val('false');
            $('#hdnRemovedEmptyMessage').val('false');
            chkPrintChallan.checked = false;
            chkPrintSticker.checked = false;
            var _globalStoreNames = new Array();
            $('#ctl00_ContentPlaceHolder1_drpBranch option:not(:first-child)').each(function () { _globalStoreNames.push($(this).text()); });

            var stateOfColor = true;
            setcolorOfDiv('LightSteelBlue');
            //setDivMouseOver('#B0C4DE', '#00aa00');
            $.extend($.expr[':'], { excontains: function (obj, index, meta, stack) { return (obj.textContent || obj.innerText || $(obj).text() || "").toLowerCase() == meta[3].toLowerCase(); } });
            var _RowsToMoveFromLeftToRight = new Array();
            var _RowsToMoveFromRightToLeft = new Array();
            if ($('#hdnAllHtml').val() != '' && $('#hdnAllHtml').val() != -1) {
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html($('#hdnAllHtml').val());
            }
            else if ($('#hdnAllHtml').val() == '-1') {
                //$('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html('');
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:not(:first-child)');
            }

            if (document.getElementById('hdnDynamicStores').value == '') {
                var _allDynamicHTML = '';
                var addDynamicDivs = function () {
                    var _htmlOfDivs = '<div class="well well-small bottom-margin-small" id="dynamicDivOuter"><div class="span9 label label-default disabled  aligncenter bottom-margin-small"  id="dynamicDivInnerName"><h5></h5></div><div class="span3 badge label-default aligncenter bottom-margin-small"  id="dynamicDivInnerNameCount"><h5></h5></div></div>';
                    var _divsToCreate = $('#ctl00_ContentPlaceHolder1_drpBranch option:not(:eq(0))').size();
                    var _spanNum = 0;
                    switch (_divsToCreate) {
                        case 1: _spanNum = 12;
                            break;
                        case 2: _spanNum = 6;
                            break;
                        case 3: _spanNum = 4;
                            break;
                        case 4: _spanNum = 3;
                            break;
                        // Not doing right now        
                        /*
                        case 5: _spanNum = 12;
                        break;
                        */ 
                        case 6: _spanNum = 2;
                            break;
                    }
                    var _classNameToAdd = 'span' + _spanNum;
                    var _iCounter = 0;
                    var _idToBeBefore = 'staticDiv';
                    $('#ctl00_ContentPlaceHolder1_drpBranch option:not(:eq(0))').each(function (index) {
                        var _storeName = $(this).text();
                        $('#' + _idToBeBefore).before(_htmlOfDivs);
                        $('#dynamicDivOuter:eq(' + _iCounter + ')').addClass(_classNameToAdd);
                        $('#dynamicDivInnerName:eq(' + _iCounter + ') > h5').text(_storeName);
                        $('#dynamicDivInnerNameCount:eq(' + _iCounter + ') > h5').text(0);
                        _idToBeBefore = $('#dynamicDivOuter:eq(' + _iCounter + ')').attr('id');
                        // _iCounter++;
                    });
                };
                addDynamicDivs();
                $('#staticDiv').prevAll().each(function (index) { _allDynamicHTML = this.outerHTML + _allDynamicHTML; });
                document.getElementById('hdnDynamicStores').value = _allDynamicHTML;
            }
            else {
                //$('#staticDiv').before($('#hdnDynamicStores').val());
                updateStoreCount();
            }

            setQtyInLabel();
            $('#hdnAllRowMoveNumFromLTR').val('');
            $('#hdnAllRowMoveNumFromRTL').val('');
            $('#hdnLTRPrevCount').val('');
            $('#hdnRTLPrevCount').val('');
            // removeRedundantRowsFromGrid('ctl00_ContentPlaceHolder1_grdNewChallan', 'ctl00_ContentPlaceHolder1_grdSelectedCloth', 5, true);
            //$('#btnClearDate').attr('disabled', true);
            disableButtons();
            $('body').click(function (event) {
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpBranch' || $(event.target).attr('id') === 'drpPrintStartFrom' || $(event.target).attr('id') === 'txtWorkShopNote') {
                    return;
                }
                if ($(event.target).attr('id') == 'txtBookingDate') {
                    return;
                }
                if ($(event.target).attr('id') == 'txtDelDate') {
                    return;
                }
                $('#txtBarcode').focus();
            });

            /******* HERE WAS THE CODE FOR DATE FILTER, IN CASE NEED TO ADD IT LATER *****/
            /******* HERE WAS THE CODE FOR REMOVE CLOTH FILTER, IN CASE NEED TO ADD IT LATER *****/

            $('#ctl00_ContentPlaceHolder1_drpBranch').change(function (e) {
                /* */
                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                    __doPostBack('ctl00$ContentPlaceHolder1$drpBranch', _allHTMLToSave);
                }
                else {
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                    __doPostBack('ctl00$ContentPlaceHolder1$drpBranch', null);
                }

            });

            var _bKNumToSearch;
            var _bkNumFind;
            var _bkFooterRowGridNew;

            $('form').submit(function (e) { return false; });

            $('#txtBarcode').keydown(function (event) {
                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('');
                if (event.which == 13 || event.which == 9) {
                    var _myVal = $(this).val().toUpperCase();
                    if (_myVal.indexOf('-') != -1) {
                        // first copy the header, just first time though
                        if ($('#hdnAddedHeader').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:first');
                            $('#hdnAddedHeader').val('true');
                        }
                        // first remove the empty text if not already removed
                        if ($('#hdnRemovedEmptyMessage').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').remove();
                            $('#hdnRemovedEmptyMessage').val('true');
                        }
                        console.warn('Could give error use excontains instead');
                        var _curRow = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                        var _brachName, _bkNum, _itemName;
                        if (_curRow.size() == 1) {
                            /* This will change previous colors */
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr > td').filter(function () { return $(this).css('background-color') != 'rgb(229, 229, 229)'; }).css('background-color', 'rgb(229, 229, 229)');
                            /* Insert current row */
                            _curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                            /* change color of current row */
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(1) > td').css('background-color', 'lime');
                            //                            jSuccess();
                            //                            _curRow.find(':checkbox').attr('checked', false);
                            var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
                            _brachName = _trCur.children[5].textContent.trim();
                            _itemName = _trCur.children[6].textContent.trim().toUpperCase();
                            _bkNum = _trCur.children[2].textContent.trim();
                            $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ', Store:' + _brachName + ']' + ' ' + findWorkShopRemark(_bkNum + '_' + _brachName));
                            jSuccess();
                            /* Remove the checkbox */
                            _curRow.find(':checkbox').attr('checked', false);
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#999999');
                            updateStoreCount('#00aa00', '#999999', 1);
                            changeChallanStatus(1, '*' + _myVal + '*');
                        }
                        else if (_curRow.size() == 0) {
                            var _newRow = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                            if (_newRow.size() == 1) {
                                // alert('Cloth Already Selected');
                                // $('#pnlPanel').dialog({ width: 250, modal: true });
                                stateOfColor = true;
                                setDivMouseOver('#FFA500', '#999999');
                                /* This will change previous colors */
                                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr > td').filter(function () { return $(this).css('background-color') != 'rgb(229, 229, 229)'; }).css('background-color', 'rgb(229, 229, 229)');
                                _newRow.find('td').css('background-color', 'orange');
                                _newRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                                var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
                                _brachName = _trCur.children[5].textContent.trim();
                                _itemName = _trCur.children[6].textContent.trim().toUpperCase();
                                _bkNum = _trCur.children[2].textContent.trim();
                                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ', Store:' + _brachName + ']' + ' ' + findWorkShopRemark(_bkNum + '_' + _brachName));
                                updateStoreCount('#FFA500', '#999999', 1);
                            }
                            else {
                                //alert('Cloth Not Available');
                                // $('#pnlPanel').dialog({ width: 250, modal: true });
                                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('CLOTH NOT AVAILABLE');
                                jBeep();
                                //beepSound();
                                stateOfColor = true;
                                setDivMouseOver('#FF0000', '#999999');
                            }
                        }
                        $(this).val('');
                        $(this).focus();
                        setQtyInLabel();
                        disableSaveButtons();
                        return false;
                    }
                    $('#hdnAddedHeader').val('false');
                    $('#hdnRemovedEmptyMessage').val('false');
                    /*
                    var _prc = $('#ctl00_ContentPlaceHolder1_drpProcess option[Selected]').val();
                    var _multi = $('#ctl00_ContentPlaceHolder1_drpMulti option[Selected]').val();
                    var _dt = $('ctl00_ContentPlaceHolder1_txtHolidayDate').val();
                    $('#ctl00_ContentPlaceHolder1_drpProcess option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_drpMulti option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_txtHolidayDate').val('');
                    */
                    var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    // check if booking number exists or not
                    var bkExists = checkIfBookingExists($('#txtBarcode').val());
                    // if there is no booking then
                    if (bkExists == false) {
                        //alert('This booking number is not available.');
                        $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('ORDER NOT AVAILABLE');
                        jBeep();
                        //beepSound();
                        stateOfColor = true;
                        setDivMouseOver('#FF0000', '#999999');
                        $('#txtBarcode').val('');
                        /**** set previous values *********/
                        return false;
                    }
                    $('#ctl00_ContentPlaceHolder1_drpProcess option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_drpMulti option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_txtHolidayDate').val('');
                    if (_allRowsCount > 1) {
                        // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                        var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                        $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                        __doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', _allHTMLToSave);
                    }
                    else {
                        $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                        __doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', null);
                    }
                }
            });

            // this finds the workshop remarks, added later
            function findWorkShopRemark(orderNumber) {
                var _result = '';
                $.ajax({
                    url: '../Autocomplete.asmx/findWorkShopRemark',
                    data: "bookingNumber='" + orderNumber + "'",
                    type: 'GET',
                    datatype: 'JSON',
                    contentType: 'application/json; charset=utf8',
                    async: false,
                    timeout: 1000,
                    success: function (result) { _result = result.d; },
                    error: function () { alert('Well! You are screwed!'); }
                });
                return _result;
            }

            function LTRfunc(e) {
                if (e.isTrigger || e.target.type !== 'checkbox') { return; }
                if ($(e.target).closest('table').attr('id') === 'ctl00_ContentPlaceHolder1_grdNewChallan') {
                    var _rowNum, localRow, localRows;
                    if ($(e.target).is(':checked')) {
                        localRow = $(e.target).closest('tr').clone();
                        localRow.html(localRow.html().replace(/grdNewChallan/gi, 'grdSelectedCloth'));
                        if (window['LTR'] == null) {
                            window['LTR'] = localRow;
                            window['LTRRemove'] = $(e.target).closest('tr'); // this wil be null when the window['ltr'] is null
                        }
                        else {
                            window['LTR'] = window['LTR'].add(localRow);
                            window['LTRRemove'] = window['LTRRemove'].add($(e.target).closest('tr'));
                        }
                    }
                    else {
                        _rowNum = $(e.target).closest('tr').find('td:eq(4)').text(); // this gets the barcode so we can filter on that basis
                        _rowNum = _rowNum.trim();
                        window['LTR'] = window['LTR'].filter(function (i, v) { return $(v).find('td').eq(4).text() != _rowNum; });
                        window['LTRRemove'] = window['LTRRemove'].filter(function (i, v) { return $(v).find('td').eq(4).text() != _rowNum; });
                    }
                    disableButtons();
                    e.stopPropagation();
                }
            }
            function RTLfunc(e) {
                if (e.isTrigger || e.target.type !== 'checkbox') { return; }
                if ($(e.target).closest('table').attr('id') === 'ctl00_ContentPlaceHolder1_grdSelectedCloth') {
                    var _rowNum, localRow, localRows;
                    if ($(e.target).is(':checked')) {
                        localRow = $(e.target).closest('tr').clone();
                        localRow.html(localRow.html().replace(/grdSelectedCloth/gi, 'grdNewChallan'));
                        if (window['RTL'] == null) {
                            window['RTL'] = localRow;
                            window['RTLRemove'] = $(e.target).closest('tr'); // this wil be null when the window['ltr'] is null
                        }
                        else {
                            window['RTL'] = window['RTL'].add(localRow);
                            window['RTLRemove'] = window['RTLRemove'].add($(e.target).closest('tr'));
                        }
                    }
                    else {
                        _rowNum = $(e.target).closest('tr').find('td:eq(4)').text(); // this gets the barcode so we can filter on that basis
                        _rowNum = _rowNum.trim();
                        window['RTL'] = window['RTL'].filter(function (i, v) { return $(v).find('td').eq(4).text() != _rowNum; });
                        window['RTLRemove'] = window['RTLRemove'].filter(function (i, v) { return $(v).find('td').eq(4).text() != _rowNum; });
                    }
                    disableButtons();
                    e.stopPropagation();
                }
            }

            /********** OLD CODE ***********/
            $('.DivStyleWithScroll').eq(0).on('click', /* ':checkbox', */function (e) {
                setTimeout(function (arg) { LTRfunc(arg) }, 10, e);
            });

            /******** OLD CODE *******/
            $('.DivStyleWithScroll').eq(1).on('click', /* ':checkbox', */function (e) {
                setTimeout(function (arg) { RTLfunc(arg) }, 10, e);
            });

            function makeMoveAll() {

            }

            function makeMoveLTR() {
                window['LRTAll'] = $('#ctl00_ContentPlaceHolder1_grdNewChallan tr').not(':eq(0)').clone();
                window['LRTAll'].each(function (i, v) { $(v).html($(v).html().replace(/grdNewChallan/gi, 'grdSelectedCloth')); });
                window['LRTAllRemove'] = $('#ctl00_ContentPlaceHolder1_grdNewChallan tr').not(':eq(0)');
            }

            function makeMoveRTL() {
                window['RTLAll'] = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth tr').not(':eq(0)').clone();
                window['RTLAll'].each(function (i, v) { $(v).html($(v).html().replace(/grdSelectedCloth/gi, 'grdNewChallan')); });
                window['RTLAllRemove'] = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth tr').not(':eq(0)');
            }
            function setLeftGridHeaders() {
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').length === 1) {
                    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:first');
                    $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').remove();
                }
            }
            //            /********** the buttons *************/

            var moveRight = function (event) {
                event.preventDefault();
                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('');
                if ($('#hdnAddedHeader').val() == 'false') {
                    /* DivStyleWithScroll').eq(1).find('table').remove();
                    var tbl = '<table cellspacing="0" border="1" style="width:40%;border-collapse:collapse;" id="ctl00_ContentPlaceHolder1_grdSelectedCloth" rules="all" class="mGrid"><tbody><tr style="color:White;font-size:Small;"><th style="width:2%;" scope="col"><span id="ctl00_ContentPlaceHolder1_grdNewChallan_ctl01_Label1"></span</th><th style="display: none" scope="col"><span id="ctl00_ContentPlaceHolder1_grdNewChallan_ctl01_Label2">RowNumber</span></th><th style="width:2px;" scope="col">Order</th><th style="width:5%;" scope="col">Due Date</th><th style="width:5%;" scope="col">Barcode</th><th style="width:5%;" scope="col">Customer</th><th style="width:5%;" scope="col">Cloth</th><th style="width:2%;" scope="col">U</th><th style="width:5%;" scope="col">Service</th></tr></table>'
                    var jTbl = $(tbl);
                    $('.DivStyleWithScroll').eq(1).prepend(jTbl); */
                    $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:first');
                    $('#hdnAddedHeader').val('true');
                }
                if ($('#hdnRemovedEmptyMessage').val() == 'false') {
                    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').remove();
                    $('#hdnRemovedEmptyMessage').val('true');
                }
                window['LTR'].insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                window['LTRRemove'].remove();
                setQtyInLabel();
                changeChallanStatus(1, null, 'ctl00_ContentPlaceHolder1_grdSelectedCloth');
                $('#txtBarcode').focus();
                updateStoreCount();
                window['LTR'] = null;
                window['RTL'] = null;
                return false;
            };
            $('#btnMoveRight').on('click', moveRight);

            /********** the buttons *************/

            var moveLeft = function (event) {

                event.preventDefault();
                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('');
                setLeftGridHeaders();
                window['RTL'].insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
                window['RTLRemove'].remove();
                setQtyInLabel();
                changeChallanStatus(0, null, 'ctl00_ContentPlaceHolder1_grdNewChallan');
                $('#txtBarcode').focus();
                updateStoreCount();
                window['LTR'] = null;
                window['RTL'] = null;
                return false;
            };
            //$('#btnMoveLeft').on('click', moveLeft);

            /*********** btn move right all ***********/

            var moveRightAllHandler = function (event) {
                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('');
                if ($('#hdnAddedHeader').val() == 'false') {
                    $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:first');
                    $('#hdnAddedHeader').val('true');
                }
                // first remove the empty text if not already removed
                if ($('#hdnRemovedEmptyMessage').val() == 'false') {
                    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').remove();
                    $('#hdnRemovedEmptyMessage').val('true')
                }
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size() > 1) {
                    //$('.DivStyleWithScroll').closest('table').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
                    $('.form-signin4').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
                    setTimeout(function () {
                        makeMoveLTR();
                        window['LRTAll'].insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                        window['LRTAllRemove'].remove();
                        $('.form-signin4').unblock();
                        setQtyInLabel();
                        changeChallanStatus(1, null, 'ctl00_ContentPlaceHolder1_grdSelectedCloth');
                    }, 25);
                    testRight();
                }
                else {
                    alert('No cloth available to move!');
                }
                setQtyInLabel();
                $('#txtBarcode').focus();
                window['LTR'] = null;
                window['RTL'] = null;
                return false;
            };
            if ($('#ctl00_ContentPlaceHolder1_grdNewChallan :checkbox').size() > 0) {
                $('#btnMoveRightAll').one('click', moveRightAllHandler);
            }

            function testRight() {
                var iCount = 0, busy = false;
                setInterval(function () {
                    if (!busy) {
                        busy = true;
                        updateStoreCount();
                        busy = false;
                    }
                }, 25);
            }

            /*********** btn move left all ***********/

            var moveLeftAllHandler = function (event) {
                // find the checked ones and move them to right
                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('');
                var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                var _i = '';
                var _k = 1;
                setLeftGridHeaders();
                if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() > 1) {
                    // $('.DivStyleWithScroll').closest('table').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
                    $('.form-signin4').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
                    setTimeout(function () {
                        makeMoveRTL();
                        window['RTLAll'].insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
                        window['RTLAllRemove'].remove();
                        $('.form-signin4').unblock();
                        setQtyInLabel();
                        changeChallanStatus(0, null, 'ctl00_ContentPlaceHolder1_grdNewChallan');
                        updateStoreCount();
                    }, 25);

                }
                else {
                    alert('No cloth available to move!');
                }
                setQtyInLabel();
                $('#txtBarcode').focus();
                window['LTR'] = null;
                window['RTL'] = null;
                return false;

            };
            if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checkbox').size() > 0) {
                $('#btnMoveLeftAll').one('click', moveLeftAllHandler);
            }


            // Button Delete
            $('#btnDeleteAll').click(function (e) {
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:not(:first)').remove();
                $('#hdnAddedHeader').val('false');
                $('#hdnRemovedEmptyMessage').val('false');
                $('#hdnAllRowMoveNumFromLTR').val('');
                $('#hdnAllRowMoveNumFromRTL').val('');
                $('#hdnLTRPrevCount').val('');
                $('#hdnRTLPrevCount').val('');
                $('#txtBarcode').focus();
                return false;
            });

            function setGridColor(grdID, colorValue, startRow, EndRow, rowToStepOver) {
                var _grdId = $('#' + grdID);
                for (var i = startRow; i <= EndRow; i++) {
                    $('#' + grdID + ' > tbody > tr:eq(' + i + ')').css('background-color', colorValue);
                }
            }

            function setQtyInLabel() {
                var _prvVal = $('#lblQtyCount').text();
                var _qtyCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() - 1;
                if (parseInt(_qtyCount) < 0) {
                    _qtyCount = 0;
                }
                $('#lblQtyCount').text(_qtyCount);
                //setQtyInFirstGrid();
                setQtyInLeftLabel();
                disableButtons();
                disableSaveButtons();
            }

            function setQtyInLeftLabel() {
                var _prvVal = $('#lblLeft').text();
                var _qtyCount = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size() - 1;
                if (parseInt(_qtyCount) < 0) {
                    _qtyCount = 0;
                }
                $('#lblLeft').text(_qtyCount);
            }

            function setcolorOfDiv(argColor) {
                //$('#DivContainerStatus, #DivContainerStatus > div').css('background-color', argColor);
            }

            function setDivMouseOver(argColorOne, argColorTwo) {
                // first call the update status for store name
                // updateStoreCount(argColorOne, argColorTwo);
                if (stateOfColor) {
                    // label-success
                    $('#DivContainerStatus > div').animate({ 'background-Color': argColorOne }, 500).delay(1000).animate({ 'background-Color': argColorTwo }, 100);
                    //$('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorTwo }, 1000);
                    //$('#DivContainerStatus > div').addClass('label-success').delay('1000').removeClass('label-success');
                }
                else {
                    $('#DivContainerStatus, #DivContainerStatus > div').animate({ backgroundColor: argColorTwo }, 10000);
                    $('#DivContainerStatus, #DivContainerStatus > div').animate({ backgroundColor: argColorOne }, 10000);
                }
                stateOfColor = !stateOfColor;
            }

            function updateStoreCount(argColorOne, argColorTwo, argRowNumber) {
                /********* OLD ********/
                /* find the name of store
                var _lastAddedName = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(1) > td:eq(5)').text();
                // find the store in div
                var _divToSet = $('div').find(':contains(' + _lastAddedName + ')').filter(function () { return $(this).hasClass('well'); }).find('#dynamicDivInnerNameCount');
                var _text = _divToSet.text();
                var _newCount = parseInt(_text) + parseInt(argCount);
                _divToSet.find('h5').text(_newCount);
                */
                /**********************
                console.log(new Date().getTime());
                var allstores = new Array();
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr > td:nth-child(6)[style="background-color: lime;"]').each(function (index) {
                allstores.push($(this).text());
                });
                console.log(new Date().getTime() + ' after this');
                var res = compressArray(allstores);
                console.log(new Date().getTime() + ' after that');
                // now for each one update the name
                Array.forEach(res, function () {
                var _store = res[0].value;
                var _curCount = res[0].count;

                var _divToSet = $('div.well h5:contains(' + _store + ')').closest('div').next().find('h5');

                var _text = _divToSet.text();
                var _newCount = parseInt(_text) + parseInt(_curCount);
                _divToSet.text(_newCount);

                });
                */

                /********** NEW ***********/
                var _curStoreName = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + argRowNumber + ') > td:nth-child(6)').text();
                var _divToColor = '';
                for (var i = 0; i < _globalStoreNames.length; i++) {
                    var _size = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr > td:nth-child(6):contains(' + _globalStoreNames[i] + ')').size();
                    var _divToSet = $('div.well h5:contains(' + _globalStoreNames[i] + ')').closest('div').next().find('h5');
                    _divToSet.text(_size);
                    if (_curStoreName == _globalStoreNames[i]) {
                        _divToColor = _divToSet;
                    }
                }
                if (argColorOne === undefined || argColorTwo === undefined)
                    return;

                if (argColorOne === null || argColorTwo === null)
                    return;

                // change the color of div, not doing in loop because this only happens when user types
                if (argColorOne !== '' && argColorTwo !== '' && argColorOne != '#FF0000') {
                    _divToColor.closest('div').animate({ 'background-Color': argColorOne }, 500).delay(1000).animate({ 'background-Color': argColorTwo }, 100);
                }
            }

            function compressArray(original) {

                var compressed = [];
                // make a copy of the input array
                var copy = original.slice(0);

                // first loop goes over every element
                for (var i = 0; i < original.length; i++) {

                    var myCount = 0;
                    // loop over every element in the copy and see if it's the same
                    for (var w = 0; w < copy.length; w++) {
                        if (original[i] == copy[w]) {
                            // increase amount of times duplicate is found
                            myCount++;
                            // sets item to undefined
                            delete copy[w];
                        }
                    }

                    if (myCount > 0) {
                        var a = new Object();
                        a.value = original[i];
                        a.count = myCount;
                        compressed.push(a);
                    }
                }

                return compressed;
            };

            function setQtyInFirstGrid() {
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan').find('tr').last().css('background-color') == 'rgb(173, 255, 47)') {
                    if (!$('#ctl00_ContentPlaceHolder1_grdNewChallan').find('tr').last().is(':visible')) {
                        $('#ctl00_ContentPlaceHolder1_grdNewChallan').find('tr').last().show();
                    }
                    if ($('#ctl00_ContentPlaceHolder1_grdNewChallan').find('tr').last().find('td:eq(2)').text() == 'Total') {
                        var _cnt = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size() - 2;
                        $('#ctl00_ContentPlaceHolder1_grdNewChallan').find('tr').last().find('td:eq(3)').text(_cnt);
                        return false;
                    }
                }
                else if (_bkFooterRowGridNew != '' && _bkFooterRowGridNew != null && _bkFooterRowGridNew != 'undefined') {
                    var tm = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').last();
                    _bkFooterRowGridNew.insertAfter(tm);
                    setQtyInFirstGrid();
                    return false;
                }
            }

            function removeRedundantRowsFromGrid(grdToRemoveFrom, grdToCheckFrom, colTextToSearchFor, boolRemove) {
                $('#' + grdToCheckFrom + ' > tbody > tr > td:nth-child(' + colTextToSearchFor + ')').each(function (index) {
                    var _txt = $(this).text();
                    var _tm = $('#' + grdToRemoveFrom + '').find(':excontains(' + _txt + ')').closest('tr');
                    if (boolRemove) {
                        _tm.remove();
                    }
                });
                setQtyInLabel();
            }

            //            $('a, #ctl00_btnF1, #ctl00_btnF4, #btnDelivery').not('#btnSave, #btnRefresh, #btnPrint, #btnSticker,#achrClearFilter').click(function (e) {
            //                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
            //                if (_allRowsCount > 1 && e.clientX != 0 && e.clientY != 0) {
            //                    jBeep();
            //                    return confirm("You have selected few cloths but not submitted to send to store. If you leave this page these changes will go unsaved.\nClick CANCEL to Go Back to Workshop Note,  Click OK to Leave Anyway");
            //                }
            //            });

            $('#btnSave, #btnRefresh, #btnPrint, #btnSticker').click(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
            });

            $('#chkPrintSticker, #chkPrintChallan').on('change', function (e) {
                if (chkPrintChallan.checked || chkPrintSticker.checked) {
                    $('#btnPrint').attr('disabled', false).removeClass('disabledClass').on('click', btnPrintSaveClickHandler);
                    $('#btnSave').attr('disabled', true).addClass('disabledClass').off('click');
                }
                else {
                    $('#btnPrint').attr('disabled', true).addClass('disabledClass').off('click');
                    $('#btnSave').attr('disabled', false).on('click', btnSaveClickHandler);
                }
                return false;
            });

            function checkIfBookingExists(argBookingNumber) {
                var result = '';
                $.ajax({
                    url: '../Autocomplete.asmx/CheckBookingNumberInFactory',
                    data: "bookingNumber='" + argBookingNumber + "'",
                    type: 'GET',
                    datatype: 'JSON',
                    contentType: 'application/json; charset=utf8',
                    async: false,
                    timeout: 5000,
                    success: function (response) {
                        result = response.d;
                    },
                    error: function (response) {
                        result = 'false';
                    }
                });
                return result;
            }
            function changeChallanStatus(status, barcode, gridId) {
                var allBarCodes = '';
                if (barcode == null) {
                    $('#' + gridId + ' td:nth-child(5)').each(function (i, v) { allBarCodes += this.textContent + ','; });
                    if (allBarCodes.length > 0) {
                        allBarCodes = allBarCodes.substr(0, allBarCodes.length - 1);
                    }
                }
                else {
                    allBarCodes = barcode;
                }
                $.ajax({
                    url: '../Autocomplete.asmx/ChangeWorkshopChallanStatusData',
                    data: "{barCodes: '" + allBarCodes + "', challanStatus: '" + status + "'}",
                    type: 'POST',
                    datatype: 'JSON',
                    contentType: 'application/json; charset=utf8',
                    async: true,
                    success: function (response) {
                        result = response.d;
                    },
                    error: function (response) {
                        //  alert(response.d);
                    }
                });
            }
            function disableButtons() {
                var _leftSize = $('#ctl00_ContentPlaceHolder1_grdNewChallan').find(':checked').size();
                var _rightSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').find(':checked').size()
                /* LEFT BUTTON */
                if (_leftSize == 0) {
                    $('#btnMoveRight').attr('disabled', true).off('click');
                }
                else {
                    $('#btnMoveRight').attr('disabled', true).off('click');
                    $('#btnMoveRight').attr('disabled', false).one('click', moveRight);
                }
                /* RIGHT BUTTON */
                if (_rightSize == 0) {
                    $('#btnMoveLeft').attr('disabled', true).off('click');
                }
                else {
                    $('#btnMoveLeft').attr('disabled', true).off('click');
                    $('#btnMoveLeft').attr('disabled', false).one('click', moveLeft);
                }
                disableMoveAllButtons();
            }

            function disableMoveAllButtons() {
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan :checkbox').size() <= 0) {
                    $('#btnMoveRightAll').attr('disabled', true).off('click');
                }
                else {
                    $('#btnMoveRightAll').attr('disabled', true).off('click');
                    $('#btnMoveRightAll').attr('disabled', false).one('click', moveRightAllHandler);
                }
                if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checkbox').size() <= 0) {
                    $('#btnMoveLeftAll').attr('disabled', true).off('click');
                }
                else {
                    $('#btnMoveLeftAll').attr('disabled', true).off('click');
                    $('#btnMoveLeftAll').attr('disabled', false).one('click', moveLeftAllHandler);
                }
            }

            function btnSaveClickHandler(e) {
                // error not in mine but in other systems, the one that is caused by clicked
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }

                //var sound = document.getElementById('sound1');
                //sound.Play();               

                var Possition = 0;
                var ScreenStatus = 30;
                var WorkShopNote = $('#txtWorkShopNote').val();
                $.ajax({
                    url: '../AutoComplete.asmx/SaveSendToStoreData',
                    data: "{Possition: '" + Possition + "', ScreenStatus: '" + ScreenStatus + "',WorkShopNote: '" + WorkShopNote + "'}",
                    type: 'POST',
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var result = response.d;
                        if (result === true) {
                            BlankGrid();
                            $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('Checkout Successful.');
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            };

            function BlankGrid() {
                stateOfColor = true;
                setDivMouseOver('#00aa00', '#999999');
                $('#lblStatusCloth').text('');
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html('<table class="mgrid" rules="all" id="ctl00_ContentPlaceHolder1_grdSelectedCloth" style="border-collapse:collapse;" cellspacing="0" border="1"><tbody><tr><td colspan="9">There are no pending Cloths to accept.</td></tr></tbody></table>');
                $('#hdnRemovedEmptyMessage').val('false');
                $('#hdnAddedHeader').val('false');
                window['LTR'] = null;
                setQtyInLabel();
                disableSaveButtons();
                $('#txtBarcode').focus();
            }
            function btnPrintSaveClickHandler(e) {
                // error not in mine but in other systems, the one that is caused by clicked
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }

                //var sound = document.getElementById('sound1');
                //sound.Play();          
                // make the array
                // makeTheArrayToStore();
                if (e.target.id === 'btnPrint' && chkPrintSticker.checked) {
                    // if printing stickers then ask where to print from
                    $('#pnlPanel').dialog({ width: 450, modal: true });
                    return false;
                }
                // __doPostBack('ctl00$ContentPlaceHolder1$btnPrint', null);

                if (e.target.id === 'btnPrint' && chkPrintChallan.checked) {
                    var ScreenStatus = 30;
                    $.ajax({
                        url: '../AutoComplete.asmx/SaveSendToStoreDataAndPrint',
                        data: "{ScreenStatus: '" + ScreenStatus + "',WorkShopNote: '" + $('#txtWorkShopNote').val() + "'}",
                        type: 'POST',
                        timeout: 20000,
                        contentType: 'application/json; charset=UTF-8',
                        datatype: 'JSON',
                        cache: true,
                        async: false,
                        success: function (response) {
                            var result = response.d;
                            if (result != "") {
                                var aryData = result.split(':');
                                BlankGrid();
                                $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('Checkout Successful.');
                                chkPrintChallan.checked = false;
                                window.open('frmWorkShopInChallanSummary.aspx?Challan=' + aryData[1] + '&ShopId=' + aryData[0] + '');
                            }
                        },
                        error: function (response) {
                            alert(response.toString())
                        }
                    });

                    return false;
                }

            };

            function disableSaveButtons() {
                var _rightSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                var _toggle = false;
                if (_rightSize <= 1) {
                    $('#btnSave').attr('disabled', true).off('click');
                    $('#btnPrint').attr('disabled', true).off('click');
                    $('#chkPrintSticker').attr('disabled', true).attr('checked', false).addClass('disabledClass');
                    $('#chkPrintChallan').attr('disabled', true).attr('checked', false).addClass('disabledClass');
                }
                else {
                         if ($('#chkPrintSticker').attr('checked') || $('#chkPrintChallan').attr('checked') ) {                                     
                        } else {
                            $('#btnPrint').attr('disabled', true).off('click');
                        }
                        $('#btnSave').attr('disabled', false).one('click', btnSaveClickHandler);                
                        $('#chkPrintSticker').attr('disabled', false).removeClass('disabledClass');
                        $('#chkPrintChallan').attr('disabled', false).removeClass('disabledClass');
                }
            }

            $('#btnRefresh').click(function (e) {
                __doPostBack('ctl00$ContentPlaceHolder1$btnRefresh', null);
            });

            $('#ctl00_ContentPlaceHolder1_btnSaveAndPrintNew').click(function (e) {
                //e.preventDefault();
                if (e.clientX === 0 || e.clientY === 0) return false;
                $('#pnlPanel').dialog('close');
                var t2 = $('#drpPrintStartFrom').val();
                $('#hdnStartValue').val(t2);
                //$('#hdnCurrentStoreIdName').val()
                //   __doPostBack('ctl00$ContentPlaceHolder1$btnSaveAndPrintNew', null);

                var Possition = 0;
                var ScreenStatus = 30;
                var WorkShopNote = $('#txtWorkShopNote').val();
                $.ajax({
                    url: '../AutoComplete.asmx/SaveSendToStoreData',
                    data: "{Possition: '" + t2 + "', ScreenStatus: '" + ScreenStatus + "',WorkShopNote: '" + WorkShopNote + "'}",
                    type: 'POST',
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var result = response.d;
                        if (result === true) {
                            $('#ctl00_ContentPlaceHolder1_lblStatusCloth').text('Checkout Successful.');
                            $('#drpPrintStartFrom').val('1');
                            if (chkPrintChallan.checked) {
                                GetChallnNo();
                            }
                            BlankGrid();
                            window.open('../Bookings/printlabel1.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Factory/WorkshopOut.aspx');
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });

                return;
            });

            function GetChallnNo() {
                $.ajax({
                    url: '../AutoComplete.asmx/GetChallnNoDataWorkshopOut',
                    data: "{}",
                    type: 'POST',
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var result = response.d;
                        var aryData = result.split(':');
                        chkPrintChallan.checked = false;
                        window.open('frmWorkShopInChallanSummary.aspx?Challan=' + aryData[1] + '&ShopId=' + aryData[0] + '');
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            }
            /*
            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('position', 'relative');
            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('width', '-20px');
            */
            function makeTheArrayToStore() {
                var _rowData = '';
                var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                for (var _i = 1; _i < _grdSize; _i++) {
                    // first the booking number
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(2)').text() + ':';
                    // now the item serial number
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(4)').text().split('-')[1] + ':';
                    // subItem name
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(6) > span').text() + ':';
                    // now the qty, hard coding 1
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(4)').text().split('-')[2] + ':';
                    // now the urgent
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(7) > span').text() + ':';
                    // now add a '_' to separate rows
                    _rowData += '_';
                }
                _rowData = _rowData.substr(0, _rowData.length - 1);
                $('#hdnAllData').val(_rowData);
                return true;
            }

            $('#txtBookingDate,#txtDelDate,#achrClearFilter').change(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
            });
            $('#txtBookingDate').change(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                $('#txtDelDate').val("");

                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                    __doPostBack('ctl00$ContentPlaceHolder1$txtBookingDate', _allHTMLToSave);
                }
                else {
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                    __doPostBack('ctl00$ContentPlaceHolder1$txtBookingDate', null);
                }
            });

            $('#txtDelDate').change(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                $('#txtBookingDate').val("");

                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                    __doPostBack('ctl00$ContentPlaceHolder1$txtDelDate', _allHTMLToSave);
                }
                else {
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                    __doPostBack('ctl00$ContentPlaceHolder1$txtDelDate', null);
                }

            });


            $('#achrClearFilter').click(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }

                $('#txtBarcode').val("");
                $('#txtBookingDate').val("");
                $('#txtDelDate').val("");
                $('#ctl00_ContentPlaceHolder1_drpBranch').val('0');
                $('#txtWorkShopNote').val("");
                chkUrgent.checked = false;

                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    __doPostBack('ctl00$ContentPlaceHolder1$achrClearFilter', _allHTMLToSave);
                }
                else {
                    __doPostBack('ctl00$ContentPlaceHolder1$achrClearFilter', null);
                }
            });
            $('#chkUrgent').click(function (e) {
                $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
            });
            updateStoreCount();
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#achruserDetail').click(function (e) {
                var status = "30";
                $.ajax({
                    url: '../AutoComplete.asmx/GetUserDetailsDataForWorkshop',
                    type: 'GET',
                    data: "Status='" + status + "' ",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var result = response.d;
                        if (result != "") {
                            var AryUserAndGrment = result.split('@');
                            var strUserName, strtotalGarment, htmldata = "";
                            htmldata = '<div class="row-fluid"><div class="col-sm-8 textBold user">User Name</div><div class="col-sm-4  textBold  user">Garment</div></div>';
                            for (var j = 0; j < AryUserAndGrment.length; j += 1) {
                                var strTempdata = AryUserAndGrment[j];
                                var arytempData = strTempdata.split(',');
                                strUserName = arytempData[0];
                                strtotalGarment = arytempData[1];
                                htmldata = htmldata + '<div class="row-fluid text textBold"><div class="col-sm-8 border2"> ' + strUserName + '</div><div class="col-sm-4  textCenter"><span>' + strtotalGarment + '</span></div></div>';
                            }
                            $('#achruserDetail').tooltip('destroy');
                            $('#achruserDetail').tooltip(
                            {
                                title: htmldata,
                                html: true,
                                placement: 'left'
                            });
                            $('#achruserDetail').tooltip('show');
                        }
                        else {
                            $('#achruserDetail').tooltip('destroy');
                            $('#achruserDetail').tooltip(
                            {
                                title: "<b>No garment selected by any user.<b>",
                                html: true,
                                placement: 'left'
                            });
                            $('#achruserDetail').tooltip('show');
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            });
            $('#achruserDetail').mouseout(function (e) {
                $('#achruserDetail').tooltip('destroy');
            });
        });

    </script>
</asp:Content>
