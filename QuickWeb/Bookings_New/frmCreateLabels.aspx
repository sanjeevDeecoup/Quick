<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="frmCreateLabels.aspx.cs" Inherits="QuickWeb.Bookings_New.frmCreateLabels"
    EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/loader.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min-extended.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../JavaScript/code.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/jBeep.min.js"></script>
    <script src="../js/loader.js" type="text/javascript"></script>
    <script src="../js/JSuccess.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkName() {
            //
            var bookingFrom = document.getElementById("<%=txtInvoiceFrom.ClientID %>").value;
            var bookingUpto = document.getElementById("<%=txtInvoiceUpto.ClientID %>").value;
            if (bookingFrom == "") {
                alert("Please enter from booking number");
                document.getElementById("<%=txtInvoiceFrom.ClientID %>").focus();
                return false;
            }
            if (bookingUpto == "") {
                alert("Please enter from booking number");
                document.getElementById("<%=txtInvoiceUpto.ClientID %>").focus();
                return false;
            }

            if (bookingFrom > bookingUpto) {
                alert("Please enter correct Order No to search");
                document.getElementById("<%=txtInvoiceUpto.ClientID %>").focus();
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#hdnCloseMe').val() == 'true') {
                var win = window.open("about:blank", "_self");
                win.close();
            }
            var stateOfColor = true;
            setcolorOfDiv('LightSteelBlue');
            //setDivMouseOver('#B0C4DE', '#00aa00');
            $.extend($.expr[':'], { excontains: function (obj, index, meta, stack) { return (obj.textContent || obj.innerText || $(obj).text() || "").toLowerCase() == meta[3].toLowerCase(); } });
            $('#hdnAddedHeader').val('false');
            $('#hdnRemovedEmptyMessage').val('false');
            var _RowsToMoveFromLeftToRight = new Array();
            var _RowsToMoveFromRightToLeft = new Array();
            if ($('#hdnAllHtml').val() != '' && $('#hdnAllHtml').val() != -1) {
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html($('#hdnAllHtml').val());
            }
            else if ($('#hdnAllHtml').val() == '-1') {
                //$('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html('');
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:not(:first-child)');
            }
            setQtyInLabel();
            $('#hdnAllRowMoveNumFromLTR').val('');
            $('#hdnAllRowMoveNumFromRTL').val('');
            $('#hdnLTRPrevCount').val('');
            $('#hdnRTLPrevCount').val('');
            // removeRedundantRowsFromGrid('ctl00_ContentPlaceHolder1_grdNewChallan', 'ctl00_ContentPlaceHolder1_grdSelectedCloth', 6, true);
            //$('#btnClearDate').attr('disabled', true);
            disableButtons();
            $('body').click(function (event) {
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpprintstart') {
                    return;
                }
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpShifts') {
                    return;
                }
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtHolidayDate') {
                    return;
                }
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpMulti') {
                    return;

                }
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtInvoiceFrom') {
                    return;

                }
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtInvoiceUpto') {
                    return;
                }
                if ($(event.target).attr('id') == 'drpBookingPreFix') {
                    return;
                }
                $('#txtBarcode').focus();
            });
            $("#ctl00_ContentPlaceHolder1_txtHolidayDate").change(function () {
                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    __doPostBack('ctl00$ContentPlaceHolder1$btnTemp', _allHTMLToSave);
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                }
                else {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnTemp', null);
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                }
                //document.getElementById("ctl00_ContentPlaceHolder1_btnTemp").click();
            });
            /*
            $("#ctl00_ContentPlaceHolder1_txtHolidayDate").keydown(function (e) {
            if ($(this).val() == '') {
            $('#btnClearDate').attr('disabled', true);
            }
            else {
            $('#btnClearDate').attr('disabled', false);
            }
            });
            */
            if ($('#ctl00_ContentPlaceHolder1_txtHolidayDate').val() == '') {
                $('#btnClearDate').attr('disabled', true).addClass('disabledClass');
            }
            else {
                $('#btnClearDate').attr('disabled', false).removeClass('disabledClass');
            }
            $('#btnClearDate').click(function (e) {
                e.preventDefault();
                if ($("#ctl00_ContentPlaceHolder1_txtHolidayDate").val() == '') {
                    return false;
                }
                $("#ctl00_ContentPlaceHolder1_txtHolidayDate").val('');
                $("#ctl00_ContentPlaceHolder1_txtHolidayDate").trigger('change');
            });

            $('#chkRemove').click(function (e) {
                if ($(this).is(':checked')) {
                    $('#lblRemove').show();
                    $('#txtRemoverChallan').show();
                    $('#txtRemoverChallan').val('');
                    $('#txtRemoverChallan').focus();
                    //$('#btnSaveChallan').attr('disabled', true);
                    $('#btnSaveRemoveChallan').show();
                    $('#txtBarcode').attr('disabled', true);
                }
                else {
                    $('#lblRemove').hide();
                    $('#txtRemoverChallan').hide();
                    $('#txtRemoverChallan').val('');
                    $('#txtBarcode').focus();
                    //$('#btnSaveChallan').attr('disabled', false);
                    $('#btnSaveRemoveChallan').hide();
                    $('#txtBarcode').attr('disabled', false);
                    $('#txtBarcode').focus();
                }
                e.stopImmediatePropagation();
                e.stopPropagation();
            });
            $('#btnSaveRemoveChallan').click(function (e) {
                e.preventDefault();
                if ($('#txtRemoverChallan').val() == '') {
                    alert('Please select a reason to return cloth');
                    $('#txtRemoverChallan').focus();
                    return false;
                }
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan').find(':checked').size() == 0) {
                    alert('Please select at least one cloth to return.');
                    return false;
                }
                $('#hdnRmvReason').val($('#txtRemoverChallan').val());

                var allDataToRemove = '';
                $('#ctl00_ContentPlaceHolder1_grdNewChallan').find(':checked').closest('tr').each(function (index) {
                    var bk = $(this).find('td:eq(2)').text();
                    var sr = $(this).find('td:eq(4)').text();
                    var idx = sr.indexOf('-');
                    var idx2 = sr.lastIndexOf('-');
                    var str = sr.substring(idx + 1, idx2);
                    if (allDataToRemove == '') {
                        allDataToRemove = bk + ':' + str;
                    }
                    else {
                        allDataToRemove += '_' + bk + ':' + str;
                    }
                });

                //allDataToRemove = allDataToRemove.substr(0, allDataToRemove.length - 1);
                $('#hdnRmvReasonData').val(allDataToRemove);

                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    __doPostBack('txtRemoverChallan', _allHTMLToSave);
                }
                else {
                    __doPostBack('txtRemoverChallan', null);
                }

            });
            $('#ctl00_ContentPlaceHolder1_drpProcess').change(function (e) {
                /* */
                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    __doPostBack('ctl00$ContentPlaceHolder1$drpProcess', _allHTMLToSave);
                }
                else {
                    __doPostBack('ctl00$ContentPlaceHolder1$drpProcess', null);
                }

            });
            $('#ctl00_ContentPlaceHolder1_txtHolidayDate').dblclick(function (e) {
                $(this).val('');
                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    __doPostBack('ctl00$ContentPlaceHolder1$btnTemp', _allHTMLToSave);
                }
                else {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnTemp', null);
                }
            });
            var _bKNumToSearch;
            var _bkNumFind;
            var _bkFooterRowGridNew;
            $('#txtBarcode').keydown(function (event) {
                $('#lblMsg').text('');
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
                        var _curRow = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                        if (_curRow.size() == 1) {
                            /* This will change previous colors */
                            // $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr >td').filter(function () { return $(this).css('background-color') != 'rgb(229, 229, 229)'; }).css('background-color', 'rgb(229, 229, 229)');
                            /* Insert current row */
                            //  _curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                            _curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                            /* change color of current row */
                            //  $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody').css('background-color', 'lime');
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(1) >td').css('background-color', 'lime');
                            jSuccess();
                            /* Remove the checkbox */
                            _curRow.find(':checkbox').attr('checked', false);
                            var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
                            _itemName = _trCur.children[6].textContent.trim();
                            _bkNum = _trCur.children[2].textContent.trim();
                            $('#lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#999999');
                            /* stock recon like */
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
                                //  $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr >td').filter(function () { return $(this).css('background-color') != 'rgb(229, 229, 229)'; }).css('background-color', 'rgb(229, 229, 229)');
                                //  _newRow.css('background-color', 'orange');
                                _newRow.find('td').css('background-color', 'orange');
                                _newRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                                var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
                                _itemName = _trCur.children[6].textContent.trim();
                                _bkNum = _trCur.children[2].textContent.trim();
                                $('#lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));

                            }
                            else {
                                //alert('Cloth Not Available');
                                // $('#pnlPanel').dialog({ width: 250, modal: true });
                                $('#lblStatusCloth').text('CLOTH NOT AVAILABLE');
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
                    var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    // check if booking number exists or not
                    var bkExists = checkIfBookingExists($('#txtBarcode').val());
                    if (bkExists == false) {
                        //alert('This booking number is not available.');
                        $('#lblStatusCloth').text('ORDER NOT AVAILABLE');
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

            // the buttons


            // the buttons
            $('#btnMoveRight').click(function (event) {
                event.preventDefault();
                $('#lblMsg').text('');
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
                window['LTR'] = null;
                window['RTL'] = null;
                return false;
            });
            // the buttons
            $('#btnMoveLeft').click(function (event) {
                event.preventDefault();
                $('#lblMsg').text('');
                setLeftGridHeaders();
                window['RTL'].insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
                window['RTLRemove'].remove();
                setQtyInLabel();
                changeChallanStatus(0, null, 'ctl00_ContentPlaceHolder1_grdNewChallan');
                $('#txtBarcode').focus();
                window['LTR'] = null;
                window['RTL'] = null;
                return false;
            });
            // btn move right all

            $('#btnMoveRightAll').click(function (event) {
                // first copy the header, just first time though
                $('#lblMsg').text('');
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

                }
                else {
                    alert('No cloth available to move!');
                }
                setQtyInLabel();
                $('#txtBarcode').focus();
                window['LTR'] = null;
                window['RTL'] = null;
                return false;
            });

            // btnMoveLeftAll
            // the buttons
            $('#btnMoveLeftAll').click(function (event) {
                // find the checked ones and move them to right
                $('#lblMsg').text('');
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
            });

            function testRight() {
                //var result2 = document.getElementById('result2');
                //var start = new Date().getTime();
                var iCount = 0, limit = 200, busy = false;
                var processor = setInterval(function () {
                    if (!busy) {
                        busy = true;
                        /*result2.value = 'time=' +
                        (new Date().getTime() - start) + ' [i=' + i + ']';
                        process();*/
                        moveRightAll();
                        if (++iCount == limit) {
                            clearInterval(processor);
                            /*result2.value = 'time=' +
                            (new Date().getTime() - start) + ' [done]';*/
                        }
                        busy = false;
                        if ($('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size() == 1) {
                            clearInterval(processor);
                            processor = null;
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checked').each(function (index) { $(this).attr('checked', false); });
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(1)').insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
                            setQtyInLabel();
                            $('.DivStyleWithScroll').closest('table').unblock();
                        }
                    }
                }, 100);
            }



            function testLeft() {
                //var result2 = document.getElementById('result2');
                //var start = new Date().getTime();
                var iCount = 0, limit = 200, busy = false;
                var processor2 = setInterval(function () {
                    if (!busy) {
                        busy = true;
                        /*result2.value = 'time=' +
                        (new Date().getTime() - start) + ' [i=' + i + ']';
                        process();*/
                        moveLeftAll();
                        if (++iCount == limit) {
                            clearInterval(processor2);
                            /*result2.value = 'time=' +
                            (new Date().getTime() - start) + ' [done]';*/
                        }
                        busy = false;
                        if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() == 1) {
                            clearInterval(processor2);
                            processor2 = null;
                            $('#ctl00_ContentPlaceHolder1_grdNewChallan :checked').each(function (index) { $(this).attr('checked', false); });
                            setQtyInLabel();
                            $('.DivStyleWithScroll').closest('table').unblock();
                        }
                    }
                }, 100);
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


            function setLeftGridHeaders() {
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').length === 1) {
                    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:first');
                    $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').remove();
                }
            }


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
                $('#DivContainerStatus').closest('td').css('background-color', argColor);
            }
            function setDivMouseOver(argColorOne, argColorTwo) {
                /*
                $('#DivContainerStatus').closest('td').off();

                $('#DivContainerStatus').closest('td').mouseover(function (e) {
                if (state) {
                $(this).animate({ backgroundColor: argColorOne }, 1000);
                }
                else {
                $(this).animate({ backgroundColor: argColorTwo }, 1000);
                }
                state = !state;
                });
                */
                //$('#DivContainerStatus').closest('td').mouseover(function (e) {
                if (stateOfColor) {
                    $('#DivContainerInnerStatus').animate({ backgroundColor: argColorOne }, 200).delay(1000).animate({ backgroundColor: argColorTwo }, 100);

                }
                else {
                    $('#DivContainerInnerStatus').animate({ backgroundColor: argColorTwo }, 1000);
                    $('#DivContainerInnerStatus').animate({ backgroundColor: argColorOne }, 100);
                }
                stateOfColor = !stateOfColor;
                //});
            }
            function beepSound() {
                //document.write("\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07");
            }
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

            function checkIfBookingExists(argBookingNumber) {
                var result = '';
                $.ajax({
                    url: '../Autocomplete.asmx/checkIfBookingNumberExists',
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
                    $('#' + gridId + ' td:nth-child(6)').each(function (i, v) { allBarCodes += this.textContent + ','; });
                    if (allBarCodes.length > 0) {
                        allBarCodes = allBarCodes.substr(0, allBarCodes.length - 1);
                    }

                }
                else {
                    allBarCodes = barcode;
                }

                $.ajax({
                    url: '../Autocomplete.asmx/ChangeChallanStatusForPrintSticker',
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
                    $('#btnMoveRight').attr('disabled', true);
                    /* CHECK BOX FOR RETURN CLOTH */
                    //$('#chkRemove').attr('checked', false);
                    if ($('#chkRemove').is(':checked')) {
                        $('#chkRemove').trigger('click');
                        $('#chkRemove').trigger('click');
                    }
                    $('#chkRemove').attr('checked', false);
                    $('#chkRemove').attr('disabled', true);
                }
                else {
                    $('#btnMoveRight').attr('disabled', false);
                    $('#chkRemove').attr('disabled', false);
                }
                /* RIGHT BUTTON */
                if (_rightSize == 0) {
                    $('#btnMoveLeft').attr('disabled', true);
                }
                else {
                    $('#btnMoveLeft').attr('disabled', false);
                }
                disableMoveAllButtons();
            }

            function disableMoveAllButtons() {
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan :checkbox').size() > 0) {
                    $('#btnMoveRightAll').attr('disabled', false);
                }
                else {
                    $('#btnMoveRightAll').attr('disabled', true);
                }
                if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checkbox').size() > 0) {
                    $('#btnMoveLeftAll').attr('disabled', false);
                }
                else {
                    $('#btnMoveLeftAll').attr('disabled', true);
                }
            }

            function disableSaveButtons() {
                var _rightSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                var _toggle = false;
                if (_rightSize <= 1) {
                    $('#btnPrint').attr('disabled', true).addClass('disabledClass');
                    $('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
                }
                else {
                    $('#btnPrint').attr('disabled', false).removeClass('disabledClass');
                    $('#btnSaveChallan').attr('disabled', false).removeClass('disabledClass');
                }
            }

            var leaveHandler = function (e) {
                var leaveHandler;
                var inFormLink;
                // var _activeElemt = ee.target.activeElement.attributes['id'].value;
                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                /*
                if (_activeElemt == 'ctl00_ContentPlaceHolder1_drpProcess') {
                if (_allRowsCount > 1) {
                // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                $('#hdnAllHtml').val(_allHTMLToSave);
                }
                }
                if (_activeElemt == 'ctl00_ContentPlaceHolder1_drpProcess' || _activeElemt == 'ctl00_ContentPlaceHolder1_drpMulti' || _activeElemt == 'ctl00_ContentPlaceHolder1_btnRefresh' || _activeElemt == 'txtBarcode') {
                return;
                }
                */
                if (_allRowsCount > 1) {
                    return confirm("You have selected few cloths but not submitted to send to workshop. If you leave this page these changes will go unsaved. Click OK to Go Back to Workshop Note,  Click Cancel to Leave Anyway");
                }
            }

            $('#btnShow').click(function (e) {
                var strBookingFrom = $('#ctl00_ContentPlaceHolder1_txtInvoiceFrom').val();
                var strBookingTo = $('#ctl00_ContentPlaceHolder1_txtInvoiceUpto').val();
                if (strBookingFrom != "" && strBookingTo != "") {
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                }
            });
        });
    </script>
    <script type="text/javascript">
        var leaveHandler = function (e) {
            var leaveHandler;
            var inFormLink;
            // var _activeElemt = ee.target.activeElement.attributes['id'].value;
            var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
            /*
            if (_activeElemt == 'ctl00_ContentPlaceHolder1_drpProcess') {
            if (_allRowsCount > 1) {
            // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
            var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
            $('#hdnAllHtml').val(_allHTMLToSave);
            }
            }
            if (_activeElemt == 'ctl00_ContentPlaceHolder1_drpProcess' || _activeElemt == 'ctl00_ContentPlaceHolder1_drpMulti' || _activeElemt == 'ctl00_ContentPlaceHolder1_btnRefresh' || _activeElemt == 'txtBarcode') {
            return;
            }
            */
            if (_allRowsCount > 1) {
                return confirm("You have selected few cloths but not submitted to send to workshop. If you leave this page these changes will go unsaved. Click OK to Go Back to Workshop Note,  Click Cancel to Leave Anyway");
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function (event) {

            function BlankGrid() {
                //                stateOfColor2 = true;
                //                setDivColor('#00aa00', '#999999'); 
                MoveLeftAllData();
                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html('<table class="mgrid" rules="all" id="ctl00_ContentPlaceHolder1_grdSelectedCloth" style="border-collapse:collapse;" cellspacing="0" border="1"><tbody><tr><td colspan="9">Please select some cloth(s) from the left section to print stickers for.</td></tr></tbody></table>');
                $('#hdnRemovedEmptyMessage').val('false');
                $('#hdnAddedHeader').val('false');
                window['LTR'] = null;

                var _prvVal1 = $('#lblQtyCount').text();
                var _qtyCount1 = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() - 1;
                if (parseInt(_qtyCount1) < 0) {
                    _qtyCount1 = 0;
                }
                $('#lblQtyCount').text(_qtyCount1);
                // $('#txtPnlBarCodeText').val('');
                setQtyInLeftLabel2();
                disableButtons2();
                disableSaveButtons2();
                $('#txtBarcode').focus();
            }

            function MoveLeftAllData() {
                if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() > 1) {                   

                    makeMoveRTL1();
                    window['RTLAll'].insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
                    window['RTLAllRemove'].remove();
                }
                else {
                    alert('No cloth available to move!');
                }
            }

            function makeMoveRTL1() {
                window['RTLAll'] = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth tr').not(':eq(0)').clone();
                window['RTLAll'].each(function (i, v) { $(v).html($(v).html().replace(/grdSelectedCloth/gi, 'grdNewChallan')); });
                window['RTLAllRemove'] = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth tr').not(':eq(0)');
            }

            function disableButtons2() {
                var _leftSizetemp = $('#ctl00_ContentPlaceHolder1_grdNewChallan').find(':checked').size();
                var _rightSizetemp = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').find(':checked').size()
                /* LEFT BUTTON */
                if (_leftSizetemp == 0) {
                    $('#btnMoveRight').attr('disabled', true);
                    /* CHECK BOX FOR RETURN CLOTH */
                    //$('#chkRemove').attr('checked', false);
                    if ($('#chkRemove').is(':checked')) {
                        $('#chkRemove').trigger('click');
                        $('#chkRemove').trigger('click');
                    }
                    $('#chkRemove').attr('checked', false);
                    $('#chkRemove').attr('disabled', true);
                }
                else {
                    $('#btnMoveRight').attr('disabled', false);
                    $('#chkRemove').attr('disabled', false);
                }
                /* RIGHT BUTTON */
                if (_rightSizetemp == 0) {
                    $('#btnMoveLeft').attr('disabled', true);
                }
                else {
                    $('#btnMoveLeft').attr('disabled', false);
                }
                disableMoveAllButtons2();
            }

            function disableMoveAllButtons2() {
                if ($('#ctl00_ContentPlaceHolder1_grdNewChallan :checkbox').size() > 0) {
                    $('#btnMoveRightAll').attr('disabled', false);
                }
                else {
                    $('#btnMoveRightAll').attr('disabled', true);
                }
                if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checkbox').size() > 0) {
                    $('#btnMoveLeftAll').attr('disabled', false);
                }
                else {
                    $('#btnMoveLeftAll').attr('disabled', true);
                }
            }

            function disableSaveButtons2() {
                var _rightSize2 = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                if (_rightSize2 <= 1) {
                    $('#btnPrint').attr('disabled', true).addClass('disabledClass');
                    $('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
                }
                else {
                    $('#btnPrint').attr('disabled', false).removeClass('disabledClass');
                    $('#btnSaveChallan').attr('disabled', false).removeClass('disabledClass');
                }
            }
            function setQtyInLeftLabel2() {
                var _prvVal2 = $('#lblLeft').text();
                var _qtyCount2 = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size() - 1;
                if (parseInt(_qtyCount2) < 0) {
                    _qtyCount2 = 0;
                }
                $('#lblLeft').text(_qtyCount2);
            }

            $('#btnSaveChallan, #btnPrint').click(function (e) {
                //var sound = document.getElementById('sound1');
                //sound.Play();
                if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() <= 1) {
                    alert('No cloth selected to save!');
                    return false;
                }
                else {
                    // make the array
                    var t2 = $('#ctl00_ContentPlaceHolder1_drpprintstart').val();
                    $('#hdnStartValue').val(t2);
                    makeTheArrayToStore();
                    PackingStickerData();
                    $('#drpprintstart').val('0');
                    e.preventDefault();
                }
            });

            function PackingStickerData() {
                var t3 = $('#ctl00_ContentPlaceHolder1_drpprintstart').val();
                $.ajax({
                    url: '../AutoComplete.asmx/PackingSticker',
                    data: "{possition: '" + t3 + "'}",
                    type: 'POST',
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var result = response.d;
                        if (result === 'Record Saved') {
                            BlankGrid();
                            window.open('../Bookings/printlabels.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Bookings/CreateLabels.aspx');

                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            }

            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('position', 'relative');
            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div');
            function makeTheArrayToStore() {
                var _rowData = '';
                var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                for (var _i = 1; _i < _grdSize; _i++) {
                    // first the booking number
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(2)').text() + ':';
                    // now the item serial number
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(5)').text().split('-')[1] + ':';
                    // subItem name
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(4)').text() + ':';
                    // now the qty, hardcoding 1
                    _rowData += '1' + '';
                    // now the urgent
                    /*
                    _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(7) > span').text() + ':';
                    */
                    // now add a '_' to separate rows
                    _rowData += '_';
                }
                _rowData = _rowData.substr(0, _rowData.length - 1);
                $('#hdnAllData').val(_rowData);
                return true;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-primary well-sm-tiny1">
        <div class="panel-heading">
            <h3 class="panel-title">
                <span class="textBold">Packing Stickers - </span>Print Tags for Ready to Be Delivered
                Cloths for Hangers/Fold Packings</h3>
        </div>
        <div class="panel-body well-sm-tiny">
            <div class="row-fluid">
                <div class="col-sm-2">
                    <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" OnTextChanged="txtBarcode_TextChanged"
                        placeholder="Order / Barcode No" ClientIDMode="Static" MaxLength="20"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBarcode_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtBarcode"  ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                    </cc1:FilteredTextBoxExtender>
                    <asp:TextBox ID="txtStartLabel" runat="server" Visible="false"></asp:TextBox>
                </div>
                <div class="col-sm-2  nopadding">
                <div class="input-group">
                    <span class="input-group-addon">Printing from Sticker</span>
                    <asp:DropDownList ID="drpprintstart" runat="server" CssClass="form-control" Width="60px">
                        </asp:DropDownList>
                </div>
                </div>
                <div class="col-sm-2 form-inline">
                    <div class="form-group">
                        <asp:TextBox ID="txtHolidayDate" runat="server" MaxLength="11" onkeypress="return false;"
                            placeholder="Delivery Date" CssClass="form-control" ToolTip="Double Click To Clear The Date"
                            onpaste="return false;"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtHolidayDate_CalendarExtender" runat="server" Enabled="True"
                            Format="dd-MMM-yyyy" TargetControlID="txtHolidayDate">
                        </cc1:CalendarExtender>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnClearDate" ClientIDMode="Static" runat="server" CssClass="btn btn-primary btn-block"
                            Width="20px" Text="X" />
                    </div>
                </div>
                <div class="col-sm-3 form-inline nopadding">
                  <div class="form-group">
                      <asp:DropDownList ID="drpBookingPreFix" runat="server" Width="90px" CssClass="form-control" ClientIDMode="Static">
								<asp:ListItem Selected="True" Text=" " Value=" "/>
								<asp:ListItem Text="A"  Value="A"/>
								<asp:ListItem Text="B"  Value="B" />
								<asp:ListItem Text="C"  Value="C"/>
								<asp:ListItem Text="D"  Value="D"/>
								<asp:ListItem Text="E"  Value="E"/>
								<asp:ListItem Text="F"  Value="F"/>
								<asp:ListItem Text="G"  Value="G"/>
								<asp:ListItem Text="H"  Value="H"/>
								<asp:ListItem Text="I"  Value="I"/>
								<asp:ListItem Text="J"  Value="J"/>
								<asp:ListItem Text="K"  Value="K"/>
								<asp:ListItem Text="L"  Value="L"/>
								<asp:ListItem Text="M"  Value="M"/>
								<asp:ListItem Text="N"  Value="N"/>
								<asp:ListItem Text="O"  Value="O"/>
								<asp:ListItem Text="P"  Value="P"/>
								<asp:ListItem Text="Q"  Value="Q"/>
								<asp:ListItem Text="R"  Value="R"/>
								<asp:ListItem Text="S"  Value="S" />
								<asp:ListItem Text="T"  Value="T" />
								<asp:ListItem Text="U"  Value="U"/>
								<asp:ListItem Text="V"  Value="V"/>
								<asp:ListItem Text="W"  Value="W"/>
								<asp:ListItem Text="X"  Value="X"/>
								<asp:ListItem Text="Y"  Value="Y" />
								<asp:ListItem Text="Z"  Value="Z"/>
							</asp:DropDownList>                     
                      
                      </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtInvoiceFrom" runat="server" MaxLength="5" CssClass="form-control" Width="110px"
                            placeholder="Order no From"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtInvoiceFrom_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="txtInvoiceFrom" ValidChars="1234567890">
                        </cc1:FilteredTextBoxExtender>
                    </div>
                      
                    <div class="form-group">
                        <asp:TextBox ID="txtInvoiceUpto" runat="server" CssClass="form-control" placeholder="Order no To"  Width="100px"
                            MaxLength="5"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtInvoiceUpto_FilteredTextBoxExtender" runat="server" ValidChars="1234567890"
                            Enabled="True"  TargetControlID="txtInvoiceUpto">
                        </cc1:FilteredTextBoxExtender>
                    </div>
                </div>
                <div class="col-sm-1">
                    <asp:Button ID="btnShow" runat="server" Text="Show" ClientIDMode="Static" CssClass="btn btn-primary"
                        EnableTheming="false" OnClick="btnShow_Click" OnClientClick="return checkName();" />
                </div>
                <div class="col-sm-1">
                    <asp:Button ID="btnPrint" runat="server" Text="Print Sticker" ClientIDMode="Static"
                        EnableTheming="false" CssClass="btn btn-primary" />
                </div>
                <div class="col-sm-1">
                    <asp:Button ID="btrefresh" runat="server" OnClick="btrefresh_Click" Text="Refresh"
                        EnableTheming="false" CssClass="btn btn-primary" />
                </div>
            </div>
            <div class="row-fluid">
                <div class="span1  well well-sm-tiny" id="DivLeftContainer">
                    <div id="DivLeftCounter" class="span label label-default">
                        <h4>
                            <asp:Label runat="server" ID="lblLeft" Text="" ClientIDMode="Static"></asp:Label>
                        </h4>
                    </div>
                </div>
                <div id="DivContainerStatus" class="span10  well well-sm-tiny">
                    <div id="DivContainerInnerStatus" class="span label label-default">
                        <h4 class="textmargin">
                            <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                            <span style="margin-left: 40%">
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" /></span>
                            <span style="margin-left: -20%">
                                <asp:Label ID="lblStatusCloth" runat="server" ClientIDMode="Static"></asp:Label></span>
                            &nbsp;
                        </h4>
                    </div>
                </div>
                <div id="DivRightContainer" class="span1  well well-sm-tiny" style="margin-left: 23px">
                    <div id="DivRightCounter" class="span label label-default">
                        <h4>
                            <asp:Label ID="lblQtyCount" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                        </h4>
                    </div>
                </div>
            </div>
            <div class="row-fluid div-margin1">
                <div class="row-fluid form-signin4 no-bottom-margin">
                    <div class="span6 well well-sm no-bottom-margin">
                        <div class="DivStyleWithScroll" style="overflow: scroll; height: 400px;">
                            <asp:GridView ID="grdNewChallan" runat="server" CssClass="mgrid" AutoGenerateColumns="False"
                                EmptyDataText="No Cloth available to display, Please select appropriate value from top."
                                ShowFooter="False" OnDataBinding="grdNewChallan_DataBinding" OnRowDataBound="grdNewChallan_RowDataBound"
                                EnableTheming="false" OnDataBound="grdNewChallan_DataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                        </HeaderTemplate>
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
                                    <asp:BoundField DataField="BookingNo" HeaderText="Order" ReadOnly="True" SortExpression="BookingNo">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" SortExpression="CustomerName">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Item" HeaderText="Cloth" ReadOnly="true" SortExpression="Item">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BarCode" HeaderText="BarCode" ReadOnly="true" SortExpression="BarCode">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Process" HeaderText="Service" ReadOnly="true" SortExpression="Process">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" ReadOnly="true" SortExpression="DueDate">
                                    </asp:BoundField>
                                    <%-- <asp:BoundField DataField="RowIndex"  HeaderText="Row Index" ReadOnly="true" SortExpression="RowIndex" />--%>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrowindex" runat="server" Text='<%# Eval("RowIndex") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="span1 well well-sm  gridhight-noscroll no-bottom-margin">
                        <div class="space">
                        </div>
                        <div>
                            <asp:Label ID="lblMoveLbl" runat="server"><span class="btn btn-default btn-sm btn-block disabled">Move<br />Selected </span></asp:Label>
                        </div>
                        <div class="div-margin">
                            <asp:Button ID="btnMoveRight" runat="server" Text="  >  " ClientIDMode="Static" CssClass="btn btn-info btn-sm btn-block"
                                EnableTheming="false" Font-Bold="True" Font-Size="Large" />
                        </div>
                        <div class="div-margin">
                            <asp:Button ID="btnMoveLeft" runat="server" Text="  <  " ClientIDMode="Static" CssClass="btn btn-info btn-sm btn-block"
                                EnableTheming="false" Font-Bold="True" Font-Size="Large" />
                        </div>
                        <div class="space">
                        </div>
                        <div>
                            <asp:Label ID="lblMoveAllLbl" runat="server" CssClass="TDCaption TDCaption2 lblMoveBtnText"><span class="btn btn-default btn-sm btn-block disabled">Move All</span></asp:Label>
                        </div>
                        <br />
                        <div class="div-margin">
                            <asp:Button ID="btnMoveRightAll" runat="server" Text=">>" ClientIDMode="Static" CssClass="btn btn-info btn-sm btn-block"
                                EnableTheming="false" Font-Bold="True" Font-Size="Large" />
                        </div>
                        <div class="div-margin">
                            <asp:Button ID="btnMoveLeftAll" runat="server" Text="<<" ClientIDMode="Static" CssClass="btn btn-info btn-sm btn-block"
                                EnableTheming="false" Font-Bold="True" Font-Size="Large" />
                        </div>
                        <div>
                            <asp:Button ID="btnDeleteAll" runat="server" Text="   X  " ClientIDMode="Static"
                                Style="display: none;" />
                        </div>
                        <div class="spacer">
                        </div>
                    </div>
                    <div class="span5 well well-sm no-bottom-margin">
                        <div class="DivStyleWithScroll" style="overflow: scroll; height: 400px;">
                            <asp:GridView ID="grdSelectedCloth" runat="server" CssClass="mgrid" AutoGenerateColumns="False"
                                EnableTheming="false" EmptyDataText="Please select some cloth(s) from the left section to print stickers for."
                                ShowFooter="False" OnRowDataBound="grdSelectedCloth_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                        </HeaderTemplate>
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
                                    <asp:BoundField DataField="BookingNo" HeaderText="Order" ReadOnly="True" SortExpression="BookingNo">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" SortExpression="CustomerName">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Item" HeaderText="Cloth" ReadOnly="true" SortExpression="Item">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BarCode" HeaderText="BarCode" ReadOnly="true" SortExpression="BarCode">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Process" HeaderText="Service" ReadOnly="true" SortExpression="Process">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" ReadOnly="true" SortExpression="DueDate">
                                    </asp:BoundField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrowindex" runat="server" Text='<%# Eval("RowIndex") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblBranchCode" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblLotNo" runat="server" Text="Lot Number:" Visible="false"></asp:Label>
            <asp:Label ID="lblLotNumber" runat="server" Text="12" Visible="false"></asp:Label>
            <asp:Label ID="lblStart" runat="server" Text="Start From" Visible="false"></asp:Label>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="RDLC/DynamicBarCodeReport.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>
    </div>
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
    <asp:HiddenField ID="hdnCheckStatus" Value="0" runat="server" />
    <asp:HiddenField ID="hdnRowNo" runat="server" />
    <asp:HiddenField ID="storecheckrow" runat="server" />
    <asp:HiddenField ID="rowexit" runat="server" />
    <asp:HiddenField ID="hdnInvoiceNo" runat="server" />
    <asp:HiddenField runat="server" ID="hdnFirstTimeCheck" Value="0" />
    <asp:Button ID="btnTemp" runat="server" OnClick="btnTemp_Click" Style="display: none" />
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
    <asp:HiddenField runat="server" ID="hdnStartValue" ClientIDMode="Static" />
</asp:Content>
