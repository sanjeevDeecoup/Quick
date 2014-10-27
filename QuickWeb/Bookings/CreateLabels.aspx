<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="CreateLabels" MaintainScrollPositionOnPostback="true" Title="Untitled Page"  EnableEventValidation="false" ValidateRequest="false"
    CodeBehind="CreateLabels.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script src="../JavaScript/javascript.js" type="text/javascript"></script>--%>
    <script src="../JavaScript/code.js" type="text/javascript"></script>
     <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/jBeep.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
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
                removeRedundantRowsFromGrid('ctl00_ContentPlaceHolder1_grdNewChallan', 'ctl00_ContentPlaceHolder1_grdSelectedCloth', 6, true);
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
                    $('#txtBarcode').focus();
                });
                $("#ctl00_ContentPlaceHolder1_txtHolidayDate").change(function () {
                    var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    if (_allRowsCount > 1) {
                        // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
                        var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                        __doPostBack('ctl00$ContentPlaceHolder1$btnTemp', _allHTMLToSave);
                    }
                    else {
                        __doPostBack('ctl00$ContentPlaceHolder1$btnTemp', null);
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
                    if (event.which == 13 || event.which == 9) {
                        var _myVal = $(this).val();
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
                                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                                /* Insert current row */
                                _curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                                /* change color of current row */
                                $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(1)').css('background-color', 'lime');
                                /* Remove the checkbox */
                                _curRow.find(':checkbox').attr('checked', false);
                                var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
                                _itemName = _trCur.children[4].textContent.trim();
                                _bkNum = _trCur.children[2].textContent.trim();
                                $('#lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));
                                stateOfColor = true;
                                setDivMouseOver('#00aa00', '#B0C4DE');
                            }
                            else if (_curRow.size() == 0) {
                                var _newRow = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                                if (_newRow.size() == 1) {
                                    // alert('Cloth Already Selected');
                                    // $('#pnlPanel').dialog({ width: 250, modal: true });

                                    stateOfColor = true;
                                    setDivMouseOver('#FFA500', '#B0C4DE');
                                    /* This will change previous colors */
                                    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                                    _newRow.css('background-color', 'orange');
                                    _newRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                                    var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
                                    _itemName = _trCur.children[4].textContent.trim();
                                    _bkNum = _trCur.children[2].textContent.trim();
                                    $('#lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));
                                    stateOfColor = true;
                                }
                                else {
                                    //alert('Cloth Not Available');
                                    // $('#pnlPanel').dialog({ width: 250, modal: true });
                                    $('#lblStatusCloth').text('CLOTH NOT AVAILABLE');
                                    jBeep();
                                    //beepSound();
                                    stateOfColor = true;
                                    setDivMouseOver('#FF0000', '#B0C4DE');
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
                            setDivMouseOver('#FF0000', '#B0C4DE');
                            $('#txtBarcode').val('');
                            return false;
                        }
                        $('#ctl00_ContentPlaceHolder1_drpProcess option:eq(0)').attr('Selected', true);
                        $('#ctl00_ContentPlaceHolder1_drpMulti option:eq(0)').attr('Selected', true);
                        $('#ctl00_ContentPlaceHolder1_txtHolidayDate').val('');
                        if (_allRowsCount > 1) {
                            // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
                            var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                            __doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', _allHTMLToSave);
                        }
                        else {
                            __doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', null);
                        }
                    }
                });
                $('#ctl00_ContentPlaceHolder1_grdNewChallan').find(':checkbox').click(function (e) {
                    if (e.isTrigger) { return; }
                    if ($(this).closest('table').attr('id') != 'ctl00_ContentPlaceHolder1_grdSelectedCloth') {
                        var _prvLTR = $('#hdnAllRowMoveNumFromLTR').val();
                        var _rowNum;
                        if ($(this).is(':checked')) {
                            /* Dont' push it */
                            _rowNum = $(this).parent('td').parent('tr').find('td:eq(1)').text();
                            _rowNum = _rowNum.trim();
                            if (_prvLTR == '') {
                                _prvLTR = _rowNum;
                            }
                            else {
                                _prvLTR = _prvLTR + ':' + _rowNum;
                            }
                            $('#hdnAllRowMoveNumFromLTR').val(_prvLTR);
                        }
                        else {
                            _rowNum = $(this).parent('td').parent('tr').find('td:eq(1)').text();
                            _rowNum = _rowNum.trim();
                            var _prvAry = _prvLTR.split(':');
                            var _idxRowNum = _prvAry.indexOf(_rowNum);
                            if (_idxRowNum != -1) {
                                _prvAry.splice(_idxRowNum, 1);
                                _prvLTR = _prvAry.join(':');
                            }
                            $('#hdnAllRowMoveNumFromLTR').val(_prvLTR);
                        }
                        disableButtons();
                        e.stopPropagation();
                    }
                });
                //$('table').on('click', ':checkbox', function (e) {
                $('table').find(':checkbox').on('click', function (e) {
                    if ($(e.target).closest('table').attr('id') == 'ctl00_ContentPlaceHolder1_grdNewChallan') { return; }
                    var _prvRTL = $('#hdnAllRowMoveNumFromRTL').val();
                    var _rowNum;
                    if ($(this).is(':checked')) {
                        /* Dont' push it */
                        _rowNum = $(this).parent('td').parent('tr').find('td:eq(1)').text();
                        _rowNum = _rowNum.trim();
                        if (_prvRTL == '') {
                            _prvRTL = _rowNum;
                        }
                        else {
                            _prvRTL = _prvRTL + ':' + _rowNum;
                        }
                        $('#hdnAllRowMoveNumFromRTL').val(_prvRTL);
                    }
                    else {
                        _rowNum = $(this).parent('td').parent('tr').find('td:eq(1)').text();
                        _rowNum = _rowNum.trim();
                        var _prvAry = _prvRTL.split(':');
                        var _idxRowNum = _prvAry.indexOf(_rowNum);
                        if (_idxRowNum != -1) {
                            _prvAry.splice(_idxRowNum, 1);
                            _prvRTL = _prvAry.join(':');
                        }
                        $('#hdnAllRowMoveNumFromRTL').val(_prvRTL);
                    }
                    disableButtons();
                    e.stopPropagation();
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

                // the buttons
                $('#btnMoveRight').click(function (event) {
                    // find the checked ones and move them to right
                    event.preventDefault();
                    var _grdSize = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size();
                    var _i = '';
                    var _k = 1;
                    var _idx = $('#txtBarcode').val().indexOf('-');
                    _bKNumToSearch = $('#txtBarcode').val().substr(0, _idx);
                    var _aryIndex = $.inArray(_bKNumToSearch, $('#hdnAllBookingNumber').val().split('_'));
                    if (_aryIndex == -1) {
                    }
                    // if 0, its the first one, start is from 1 and end at the value
                    var _argStartRow, _argEndRow, _argIdxToCheckForRow;
                    var _bChangedColor = false;
                    var _tt = $('#hdnAllRowMoveNumFromLTR').val().split(':');
                    _tt.sort(function (a, b) { return a - b });
                    var _removedCountPrev = $('#hdnLTRPrevCount').val();
                    if (_removedCountPrev == '') {
                        _removedCountPrev = 0;
                    }
                    var _rowToMove;
                    if (_tt.length > 0 && _tt != '') {
                        /* This will change previous colors */
                        $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                    }
                    else {
                        alert('Please select atleast one cloth to Move!');
                    }
                    for (_i = 0; _i < _tt.length; _i++) {
                        if ($('#hdnAddedHeader').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:first');
                            $('#hdnAddedHeader').val('true');
                        }
                        // first remove the empty text if not already removed
                        if ($('#hdnRemovedEmptyMessage').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').remove();
                            $('#hdnRemovedEmptyMessage').val('true')
                        }
                        // change the color just one time
                        if (!_bChangedColor) {
                        }
                        var _curTr = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody').find(':excontains(' + _tt[_i] + ')').filter(function (index) { return $(this).hasClass('rowNumber') }).closest('tr');
                        _curTr.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').fadeOut(100).fadeIn(100).css('background-color', 'lime');
                    }
                    $('#hdnAllRowMoveNumFromLTR').val('');
                    var _prvLTRCount = $('#hdnLTRPrevCount').val();
                    if (_prvLTRCount == '') {
                        _prvLTRCount = _tt.length;
                    }
                    else {
                        _prvLTRCount = parseInt(_prvLTRCount) + parseInt(_tt.length);
                    }
                    $('#hdnLTRPrevCount').val('' + _prvLTRCount);
                    // change the color back
                    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checked').each(function (index) { $(this).attr('checked', false); });
                    setQtyInLabel();
                    $('#txtBarcode').focus();
                    return false;
                });
                // the buttons
                $('#btnMoveLeft').click(function (event) {
                    // find the checked ones and move them to right
                    event.preventDefault();
                    var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    var _i = '';
                    var _k = 1;
                    var _bChangedColor = false;
                    var _bChangedColor = false;
                    var _tt = $('#hdnAllRowMoveNumFromRTL').val().split(':');
                    _tt.sort(function (a, b) { return a - b });
                    var _removedCountPrev = $('#hdnRTLPrevCount').val();
                    if (_removedCountPrev == '') {
                        _removedCountPrev = 0;
                    }
                    var _rowToMove;
                    if (_tt.length > 0 && _tt != '') {
                        /* This will change previous colors */
                        $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                    }
                    else {
                        alert('Please select atleast one cloth to Move!');
                    }
                    for (_i = 0; _i < _tt.length; _i++) {
                        var _curTr = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody').find(':excontains(' + _tt[_i] + ')').filter(function (index) { return $(this).hasClass('rowNumber') }).closest('tr').filter(function (indexInner) { return $(this).find(':checkbox').is(':checked'); });
                        _curTr.insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                    }
                    if (_tt.length > 0) {
                        /* This will change previous colors */
                        $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                    }
                    $('#hdnAllRowMoveNumFromRTL').val('');
                    var _prvRTLCount = $('#hdnRTLPrevCount').val();
                    if (_prvRTLCount == '') {
                        _prvRTLCount = _tt.length;
                    }
                    else {
                        _prvRTLCount = parseInt(_prvRTLCount) + parseInt(_tt.length);
                    }
                    $('#hdnRTLPrevCount').val('' + _prvRTLCount);
                    $('#ctl00_ContentPlaceHolder1_grdNewChallan :checked').each(function (index) { $(this).attr('checked', false); });
                    setQtyInLabel();
                    $('#txtBarcode').focus();
                    return false;
                });
                // btn move right all
                $('#btnMoveRightAll').click(function (event) {

                    var _grdSize = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size();
                    var _i = '';
                    var _k = 1;
                    // first copy the header, just first time though
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
                        $('.DivStyleWithScroll').closest('table').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
                        testRight();
                    }
                    else {
                        alert('No cloth available to move!');
                    }
                    $('#txtBarcode').focus();
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

                function moveRightAll() {
                    var _k = 1;
                    for (_i = 1; _i < 500; _i++) {
                        // now copy this row over
                        $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(' + _i + ')').insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
                        // check it
                        _k++;
                        _i = _i - 1;
                        if (_k == 500)
                            break;
                    }
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

                function moveLeftAll() {
                    var _k = 1;
                    for (_i = 1; _i < 500; _i++) {
                        // now copy this row over
                        $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ')').insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
                        //$('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(' + _i + ')').insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:last');
                        // check it
                        _k++;
                        _i = _i - 1;
                        if (_k == 500)
                            break;
                    }
                }

                // btnMoveLeftAll
                // the buttons
                $('#btnMoveLeftAll').click(function (event) {
                    // find the checked ones and move them to right
                    var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    var _i = '';
                    var _k = 1;
                    if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() > 1) {
                        $('.DivStyleWithScroll').closest('table').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
                        testLeft();
                    }
                    else {
                        alert('No cloth available to move!');
                    }
                    setQtyInLabel();
                    $('#txtBarcode').focus();
                    return false;
                });
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
                    var _qtyCount = $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').size() - 2;
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
                        $('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorOne }, 200).delay(1000).animate({ backgroundColor: argColorTwo }, 100);
                        //$('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorTwo }, 1000);
                    }
                    else {
                        $('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorTwo }, 1000);
                        $('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorOne }, 100);
                    }
                    stateOfColor = !stateOfColor;
                    //});
                }
                function beepSound() {
                    document.write("\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07" + "\x07");
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
                $('a, #ctl00_btnF1, #ctl00_btnF4, #btnDelivery').click(function (e) {
                    var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    if (_allRowsCount > 1 && e.clientX != 0 && e.clientY != 0) {
                        jBeep();
                        return confirm("You have selected few cloths but generated the stickers. If you leave this page these changes will go unsaved.\nClick Cancel to Go Back OR  Click OK to Leave Anyway.");
                    }
                });

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
                }
            });
            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('position', 'relative');
            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('width', '-20px');
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
             <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="Packing Stickers" ForeColor="#FF9933"></asp:Label>
                    <span class="" style="font-size: 12Px">Print Tags for Ready to Be Delivered Cloths for Hangers/Fold Packings</span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
             <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
        </table>
        <table>
            <tr valign="top">
                <td class="TDCaption">
                    <asp:Label ID="lblLotNo" runat="server" Text="Lot Number:" Visible="false"></asp:Label>
                </td>
                <td class="Legend">
                    <asp:Label ID="lblLotNumber" runat="server" Text="12" Visible="false"></asp:Label>
                </td>
                <td style="width: 10Px">
                </td>
                <td class="TDCaption">
                    <asp:Label ID="lblBarcode" runat="server" Text="Order/Barcode No"></asp:Label>
                    <asp:Label ID="lblStart" runat="server" Text="Start From" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBarcode" runat="server" OnTextChanged="txtBarcode_TextChanged"  ClientIDMode="Static" MaxLength="20"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtBarcode_FilteredTextBoxExtender"
                        runat="server" Enabled="True" TargetControlID="txtBarcode"
                        ValidChars="1234567890-">
                    </cc1:FilteredTextBoxExtender>
                    <asp:TextBox ID="txtStartLabel" runat="server" Visible="false"></asp:TextBox>
                </td>
                <td style="width: 10Px">
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="TDCaption">
                    Printing from Sticker
                </td>
                <td style="width: 10Px">
                    &nbsp;
                </td>
                <td>
                    <asp:DropDownList ID="drpprintstart" runat="server" Width="45">
                    </asp:DropDownList>
                </td>
                <td class="TDCaption">
                    Due Delivery Date:
                </td>
                <td style="width: 10Px" nowrap="nowrap">
                    <asp:TextBox ID="txtHolidayDate" runat="server" MaxLength="11" onkeypress="return false;"
                        ToolTip="Double Click To Clear The Date" onpaste="return false;"></asp:TextBox><asp:Button
                            ID="btnClearDate" ClientIDMode="Static" runat="server"
                            Text="X" />
                    <cc1:CalendarExtender ID="txtHolidayDate_CalendarExtender" runat="server" Enabled="True"
                        Format="dd-MMM-yyyy" TargetControlID="txtHolidayDate">
                    </cc1:CalendarExtender>
                </td>
                <td nowrap="nowrap">
                From : <asp:TextBox ID="txtInvoiceFrom" runat="server" Width="70Px" MaxLength="5"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtInvoiceFrom_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="txtInvoiceFrom">
                    </cc1:FilteredTextBoxExtender>
                    Upto : <asp:TextBox ID="txtInvoiceUpto" runat="server" Width="70Px"
                        MaxLength="5"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtInvoiceUpto_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="txtInvoiceUpto">
                    </cc1:FilteredTextBoxExtender>
                    <asp:Button ID="btnShow" runat="server" Text="Show" ClientIDMode="Static"
                        onclick="btnShow_Click" OnClientClick="return checkName();" />
                </td>
                <td>
                    <asp:Button ID="btnPrint" runat="server" Text="Print Sticker" OnClick="btnPrint_Click" ClientIDMode="Static" />
                </td>
                <td style="width: 10Px">
                </td>
                <td>
                   <asp:Button ID="btrefresh" runat="server" OnClick="btrefresh_Click" Text="Refresh" />
                </td>
            </tr>
            </table>
            <table style="width: 100%;">
            <tr valign="top">
             <td class="style1">
                    <div id="DivLeftContainer">
                        <div id="DivLeftCounter">
                            <asp:Label runat="server" ID="lblLeft" Text="" ClientIDMode="Static"
                                CssClass="TDCaption TDCaption2"></asp:Label>
                        </div>
                    </div>
                </td>
                <td class="style1" colspan="6">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                </td>
                <td style="width: 40%;">
                    <div id="DivContainerStatus">
                        <div id="DivContainerInnerStatus">
                            <asp:Label ID="lblStatusCloth" runat="server" ClientIDMode="Static"></asp:Label>
                        </div>
                    </div>
                </td>
                <td class="style4" align="right">
                    <div id="DivRightContainer">
                        <div id="DivRightCounter">
                            <asp:Label ID="lblQtyCount" runat="server" Text="" ClientIDMode="Static"
                                CssClass="TDCaption TDCaption2"></asp:Label>
                        </div>
                    </div>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr valign="top">
                <td class="TDCaption" style="text-align: left; width: 46%"">
                    <div class="DivStyleWithScroll" style="overflow: scroll; height: 350px;">
                        <asp:GridView ID="grdNewChallan" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                            EmptyDataText="No Cloth available to display, Please select appropriate value from top." ShowFooter="True" OnDataBinding="grdNewChallan_DataBinding"
                            OnRowDataBound="grdNewChallan_RowDataBound"
                            OnDataBound="grdNewChallan_DataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                    <HeaderStyle Width="2%" />
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
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" SortExpression="CustomerName">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Item" HeaderText="Cloth" ReadOnly="true" SortExpression="Item">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BarCode" HeaderText="BarCode" ReadOnly="true" SortExpression="BarCode">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Process" HeaderText="Service" ReadOnly="true" SortExpression="Process">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" ReadOnly="true" SortExpression="DueDate">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
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
                    </td>

                    <td class="challanMoveButtons">
                    <asp:Label ID="lblMoveLbl" runat="server" CssClass="TDCaption, TDCaption1 lblMoveBtnText">&nbsp;&nbsp;Move<br></br>&nbsp;Selected&nbsp;</asp:Label>
                    <asp:Button ID="btnMoveRight" runat="server" Text="  >  " ClientIDMode="Static"  CssClass="top20" />
                    <asp:Button ID="btnMoveLeft" runat="server" Text="  <  " ClientIDMode="Static" CssClass="top20" />
                    <asp:Label ID="lblMoveAllLbl" runat="server" CssClass="TDCaption TDCaption2 lblMoveBtnText">&nbsp;&nbsp;<br></br><br></br>Move All&nbsp;</asp:Label>
                    <asp:Button ID="btnMoveRightAll" runat="server" Text=">>" ClientIDMode="Static"  CssClass="top20" />
                    <asp:Button ID="btnMoveLeftAll" runat="server" Text="<<" ClientIDMode="Static"  CssClass="top20" />
                    <asp:Button ID="btnDeleteAll" runat="server" Text="   X  " ClientIDMode="Static"
                        Style="display: none;" />
                </td>

                    <td class="TDCaption" style="text-align: left; width: 46%"">
                    <div class="DivStyleWithScroll" style="overflow: scroll; height: 350px;">
                        <asp:GridView ID="grdSelectedCloth" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                            EmptyDataText="Please select some cloth(s) from the left section to print stickers for." ShowFooter="True">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-CssClass="rowNumber">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label2" runat="server" Text="RowNumber"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblRowNumber"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BookingNo" HeaderText="Booking No." ReadOnly="True" SortExpression="BookingNo">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" SortExpression="CustomerName">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Item" HeaderText="Item" ReadOnly="true" SortExpression="Item">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BarCode" HeaderText="BarCode" ReadOnly="true" SortExpression="BarCode">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Colour" HeaderText="Colour" ReadOnly="true" SortExpression="Colour">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Process" HeaderText="Process" ReadOnly="true" SortExpression="Process">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" ReadOnly="true" SortExpression="DueDate">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
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
                    </td>
            </tr>
            <tr valign="top">
                <td class="TDCaption" style="text-align: left">
                    &nbsp;&nbsp;
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt">
                        <LocalReport ReportPath="RDLC/DynamicBarCodeReport.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:Label ID="lblBranchCode" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td>
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
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>