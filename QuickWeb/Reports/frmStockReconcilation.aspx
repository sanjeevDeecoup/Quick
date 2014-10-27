<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeBehind="frmStockReconcilation.aspx.cs"
    Inherits="QuickWeb.Reports.frmStockReconcilation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/jBeep.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $.extend($.expr[':'], { excontains: function (obj, index, meta, stack) { return (obj.textContent || obj.innerText || $(obj).text() || "").toLowerCase() == meta[3].toLowerCase(); } });
    </script>
    <script type="text/javascript">
        function PrintGridData() {
            var prtGrid = document.getElementById('<%=grdMatchClothes.ClientID %>');
            prtGrid.border = 2;
            var prtwin = window.open('', 'PrintGridViewData', 'left=100,top=0,width=1000,height=1000,tollbar=0,scrollbars=1,status=0,resizable=1');
            prtwin.document.write(prtGrid.outerHTML);
            prtwin.document.close();
            prtwin.focus();
            prtwin.print();
            prtwin.close();
            return false;
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            var stateOfColor = true;
            //  setcolorOfDiv('LightSteelBlue');

            $('#ctl00_ContentPlaceHolder1_grdMatchClothes').find('th').length == 0 && $('#hdnAddedHeader').val('false');
            $('#ctl00_ContentPlaceHolder1_grdMatchClothes :contains("No Matching Reconds")').length !== 0 && $('#hdnRemovedEmptyMessage').val('false');
            $('#ctl00_ContentPlaceHolder1_grdMatchNotFound').find('th').length == 0 && $('#hdnAddedHeader2').val('false');
            $('#ctl00_ContentPlaceHolder1_grdMatchNotFound :contains("No Matching Reconds")').length !== 0 && $('#hdnRemovedEmptyMessage2').val('false');
            if ($('#hdnAllHtml').val() != '' && $('#hdnAllHtml').val() != -1) {
                $('#ctl00_ContentPlaceHolder1_grdMatchClothes').closest('div').html($('#hdnAllHtml').val());
                $('#hdnAddedHeader').val('true');
                $('#hdnRemovedEmptyMessage').val('true');
            }
            else if ($('#hdnAllHtml').val() == '-1') {

            }
            if ($('#hdnAllHtmlNot').val() != '' && $('#hdnAllHtmlNot').val() != -1) {
                $('#ctl00_ContentPlaceHolder1_grdMatchNotFound').closest('div').html($('#hdnAllHtmlNot').val());
                $('#hdnAddedHeader2').val('true');
                $('#hdnRemovedEmptyMessage2').val('true');
            }
            else if ($('#hdnAllHtmlNot').val() == '-1') {
                //$('#ctl00_ContentPlaceHolder1_grdMatchNotFound').closest('div').html('');
                //$('#ctl00_ContentPlaceHolder1_grdMatchNotFound > tbody > tr:not(:first-child)');
            }
            updateCount();
            $('#hdnAllRowMoveNumFromLTR').val('');
            $('#hdnAllRowMoveNumFromRTL').val('');
            $('#hdnLTRPrevCount').val('');
            $('#hdnRTLPrevCount').val('');
            //removeRedundantRowsFromGrid('ctl00_ContentPlaceHolder1_grdNewChallan', 'ctl00_ContentPlaceHolder1_grdSelectedCloth', 5, true);

            $('body').click(function (event) {
                if ($(event.target).attr('id') == 'drpStatus') {
                    return;
                }
                $('#txtInvoiceNo').focus();
            });


            var drpProcessChangeHanlder = function (e) {
                /* */
                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr').length;
                var _allNotMatch = $('#ctl00_ContentPlaceHolder1_grdMatchNotFound > tbody > tr').length;
                var _htm1, _htm2;
                if (_allRowsCount <= 1 && _allNotMatch <= 1) {
                    __doPostBack('drpStatus', null);
                }
                else if (_allRowsCount > 1 && _allNotMatch <= 1) {
                    // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                    _htm1 = $('#ctl00_ContentPlaceHolder1_grdMatchClothes').closest('div').html();
                    _htm2 = '';
                    __doPostBack('drpStatus', _htm1 + '#' + _htm2);
                }
                else if (_allRowsCount <= 1 && _allNotMatch > 1) {
                    _htm1 = ''
                    _htm2 = $('#ctl00_ContentPlaceHolder1_grdMatchNotFound').closest('div').html();
                    __doPostBack('drpStatus', _htm1 + '#' + _htm2);
                }
                else {
                    _htm1 = $('#ctl00_ContentPlaceHolder1_grdMatchClothes').closest('div').html();
                    _htm2 = $('#ctl00_ContentPlaceHolder1_grdMatchNotFound').closest('div').html();
                    __doPostBack('drpStatus', _htm1 + '#' + _htm2);
                }

            };

            $('#drpStatus').on('change', drpProcessChangeHanlder);

            var _bKNumToSearch;
            var _bkNumFind;
            var _bkFooterRowGridNew;
            $('#txtInvoiceNo').keydown(function (event) {
                if (event.which == 13 || event.which == 9) {
                    var _myVal = $(this).val().toUpperCase();
                    if (_myVal.indexOf('-') != -1) {
                        // first copy the header, just first time though
                        if ($('#hdnAddedHeader').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdAllClothes > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr:first');
                            $('#hdnAddedHeader').val('true');
                        }
                        // first remove the empty text if not already removed
                        if ($('#hdnRemovedEmptyMessage').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr:eq(0)').remove();
                            $('#hdnRemovedEmptyMessage').val('true');
                        }
                        if ($('#hdnAddedHeader2').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdAllClothes > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdMatchNotFound > tbody > tr:first');
                            $('#hdnAddedHeader2').val('true');
                        }
                        // first remove the empty text if not already removed
                        if ($('#hdnRemovedEmptyMessage2').val() == 'false') {
                            $('#ctl00_ContentPlaceHolder1_grdMatchNotFound > tbody > tr:eq(0)').remove();
                            $('#hdnRemovedEmptyMessage2').val('true');
                        }
                        var _curRow = $('#ctl00_ContentPlaceHolder1_grdAllClothes > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                        if (_curRow.size() == 1) {
                            /* This will change previous colors */
                            $('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                            /* Insert current row */
                            _curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                            /* change color of current row */
                            $('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr:eq(1)').css('background-color', 'lime');
                            /* Remove the checkbox */
                            _curRow.find(':checkbox').attr('checked', false);
                            var _trCur = $('#ctl00_ContentPlaceHolder1_grdMatchClothes').children().children()[1];
                            _itemName = _trCur.children[3].textContent.trim();
                            _bkNum = _trCur.children[1].textContent.trim();
                            $('#lblStatusClothMsg').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#B0C4DE');
                            saveBarCodeInData(_myVal, 'yes');
                            updateCount();
                        }
                        else if (_curRow.size() == 0) {
                            var _newRow = $('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                            if (_newRow.size() == 1) {
                                // alert('Cloth Already Selected');
                                // $('#pnlPanel').dialog({ width: 250, modal: true });
                                stateOfColor = true;
                                setDivMouseOver('#FFA500', '#B0C4DE');
                                /* This will change previous colors */
                                $('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr').filter(function () { return $(this).css('background-color') != 'transparent'; }).css('background-color', 'transparent');
                                _newRow.css('background-color', 'orange');
                                _newRow.insertAfter('#ctl00_ContentPlaceHolder1_grdMatchClothes > tbody > tr:eq(0)');
                                var _trCur = $('#ctl00_ContentPlaceHolder1_grdMatchClothes').children().children()[1];
                                _itemName = _trCur.children[3].textContent.trim();
                                _bkNum = _trCur.children[1].textContent.trim();
                                $('#lblStatusClothMsg').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));
                                updateCount();
                            }
                            else {
                                //alert('Cloth Not Available');
                                // $('#pnlPanel').dialog({ width: 250, modal: true });
                                $('#lblStatusClothMsg').text('Cloth Not Matched');
                                jBeep();
                                var _notSize = $('#ctl00_ContentPlaceHolder1_grdMatchNotFound > tbody').find(':contains(' + '*' + _myVal + '*' + ')').closest('tr');
                                if (_notSize.size() == 0) {
                                    loadIntoNotMatched(_myVal);
                                    $(this).val('');
                                    $(this).focus();
                                    updateCount();
                                    //beepSound();
                                    stateOfColor = true;
                                    setDivMouseOver('#FF0000', '#B0C4DE');
                                }
                            }
                        }
                        $(this).val('');
                        $(this).focus();
                        return false;
                    }
                    $('#hdnAddedHeader').val('false');
                    $('#hdnRemovedEmptyMessage').val('false');
                    $('#hdnAddedHeader2').val('false');
                    $('#hdnRemovedEmptyMessage2').val('false');
                    /*
                    var _prc = $('#ctl00_ContentPlaceHolder1_drpProcess option[Selected]').val();
                    var _multi = $('#ctl00_ContentPlaceHolder1_drpMulti option[Selected]').val();
                    var _dt = $('ctl00_ContentPlaceHolder1_txtHolidayDate').val();
                    $('#ctl00_ContentPlaceHolder1_drpProcess option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_drpMulti option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_txtHolidayDate').val('');
			
                    var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
                    // check if booking number exists or not
                    var bkExists = checkIfBookingExists($('#txtBarcode').val());
                    if (bkExists == false) {
                    //alert('This booking number is not available.');
                    $('#lblStatusClothMsg').text('ORDER NOT AVAILABLE');
                    jBeep();
                    //beepSound();
                    stateOfColor = true;
                    setDivMouseOver('#FF0000', '#B0C4DE');
                    $('#txtBarcode').val('');
                    /**** set previous values *********
                    return false;
                    }
                    $('#ctl00_ContentPlaceHolder1_drpProcess').off();
                    $('#ctl00_ContentPlaceHolder1_drpProcess option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_drpProcess').on('change', drpProcessChangeHanlder);
                    $('#ctl00_ContentPlaceHolder1_drpMulti option:eq(0)').attr('Selected', true);
                    $('#ctl00_ContentPlaceHolder1_txtHolidayDate').val('');
                    if (_allRowsCount > 1) {
                    // save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
                    __doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', _allHTMLToSave);
                    }
                    else {
                    __doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', null);
                    }
                    */
                }
            });

            function loadIntoNotMatched(barCode) {
                $.ajax({
                    url: '../Autocomplete.asmx/stockReconNotMatched',
                    data: "barcode='" + barCode + "'",
                    type: 'GET',
                    datatype: 'JSON',
                    contentType: 'application/json; charset=utf8',
                    async: false,
                    timeout: 1000,
                    success: function (result) { updateNotMachedGrid(result.d); },
                    error: function () { }
                });
            }

            function updateNotMachedGrid(data) {
                var node, nodeTmp;
                $('#txtInvoiceNo').val('');
                $('#txtInvoiceNo').focus();
                if (data.length != 6) return;
                node = $('#ctl00_ContentPlaceHolder1_grdAllClothes').children('tbody').children('tr:eq(1)').clone();
                if (node.length == 0) {
                    nodeTmp = "<td>26 Apr 2013</td><td style=\"display: none\"><a id=\"ctl00_ContentPlaceHolder1_grdAllClothes_ctl02_hypBtnShowDetails\" href=\"../Bookings/BookingSlip.aspx?BN=42-127 Apr 2013\" target=\"_blank\">42</a></td><td><span id=\"ctl00_ContentPlaceHolder1_grdAllClothes_ctl02_lblDate\">27 Apr 2013</span></td><td align=\"right\">Payajami</td><td align=\"right\">Pending For Finishing</td><td align=\"right\">*42-2-1*</td>"
                    node = $('<tr>' + nodeTmp + '</tr>')
                }
                node.find('td:eq(0)').text(data[0]);
                node.find('td:eq(1)').text(data[1]);
                node.find('td:eq(2)').text(data[2]);
                node.find('td:eq(3)').text(data[3]);
                node.find('td:eq(4)').text(data[4]);
                node.find('td:eq(5)').text(data[5]);
                node.insertAfter('#ctl00_ContentPlaceHolder1_grdMatchNotFound > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                $('#lblStatusClothMsg').text(data[3] + ' [Order No:' + data[1] + ']' + ' ' + findWorkShopRemark(data[1]));
                saveBarCodeInData(data[5], 'no');
                updateCount();
            }

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
                    success: function (result) { _result = result.d; if (result.d === 'Object are not set anywhere') _result = ''; },
                    error: function () { }
                });
                return _result;
            }

            function setcolorOfDiv(argColor) {
                $('#DivContainerStatus').closest('td').css('background-color', argColor);
            }
            //            function setDivMouseOver(argColorOne, argColorTwo) {

            //                //$('#DivContainerStatus').closest('td').mouseover(function (e) {
            //                if (stateOfColor) {
            //                    $('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorOne }, 200).delay(1000).animate({ backgroundColor: argColorTwo }, 100);
            //                    //$('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorTwo }, 1000);
            //                }
            //                else {
            //                    $('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorTwo }, 1000);
            //                    $('#DivContainerStatus').closest('td').animate({ backgroundColor: argColorOne }, 100);
            //                }
            //                stateOfColor = !stateOfColor;
            //            }

            function saveBarCodeInData(barCode, saveInMatchFound) {
                $.ajax({
                    url: '../AutoComplete.asmx/SaveStockBarCodes',
                    type: 'POST',
                    async: false,
                    contentType: 'application/json; charset=utf8',
                    timeout: 5000,
                    data: "{BarCodes: '" + barCode + "',flag: '" + saveInMatchFound + "'}",
                    dataType: 'JSON',
                    success: function (data, status, jqXHR) {
                        if (data.d === 'Failed') {
                            alert('Your session has expired. Please login again.');
                            __doPostBack('ctl00$LinkButton1', '');
                        }
                    },
                    failure: function (jqXHR, status, error) {
                        alert('Your session has expired. Please login again.');
                        __doPostBack('ctl00$LinkButton1', '');
                    }
                });
            }

            function removeRedundantRowsFromGrid(grdToRemoveFrom, grdToCheckFrom, colTextToSearchFor, boolRemove) {
                $('#' + grdToCheckFrom + ' > tbody > tr > td:nth-child(' + colTextToSearchFor + ')').each(function (index) {
                    var _txt = $(this).text();
                    var _tm = $('#' + grdToRemoveFrom + '').find(':excontains(' + _txt + ')').closest('tr');
                    if (boolRemove) {
                        _tm.remove();
                    }
                });
            }

            $('#ctl00_ContentPlaceHolder1_btnMatchClothesExport').click(function (e) {
                if ($('#ctl00_ContentPlaceHolder1_grdMatchClothes tr').size() <= 1)
                    return false;

                makeHead('ctl00_ContentPlaceHolder1_grdMatchClothes');
                makeData('ctl00_ContentPlaceHolder1_grdMatchClothes');
                makeFoot('ctl00_ContentPlaceHolder1_grdMatchClothes');
                return;
            });

            $('#ctl00_ContentPlaceHolder1_btnNotMatchClothesExport').click(function (e) {
                if ($('#ctl00_ContentPlaceHolder1_grdMatchNotFound tr').size() <= 1)
                    return false;

                makeHead('ctl00_ContentPlaceHolder1_grdMatchNotFound');
                makeData('ctl00_ContentPlaceHolder1_grdMatchNotFound');
                makeFoot('ctl00_ContentPlaceHolder1_grdMatchNotFound');
                return;
            });

            function makeHead(gridId) {
                var _heads = '';
                $('#' + gridId + ' tr:eq(0)').find('th').each(function (index, elem) {
                    _heads += ':' + elem.textContent.trim();
                });
                $('#hdnHead').val(_heads.substr(1));
            }

            function makeData(gridId) {
                var _data = '', _sanatizedData;

                $('#' + gridId + ' tr:not(:eq(0))').each(function (index, elem) {
                    _data += '_'; $(elem).find('td').each(function (index, elem) {
                        _data += ':' + elem.textContent.trim();
                    });
                });

                _sanatizedData = _data.replace(/_:/gi, "_");

                $('#hdnData').val(_sanatizedData.substr(1));
            }

            function makeFoot(gridId) {

            }

            /*
            $('#DoneRecon').on('click', function (e) {
            window.isPendingRecon = false;
            ['matchesClothes.htm', 'notMatchesClothes.htm'].forEach(function (value, index) {
            $.ajax({
            url: '../AutoComplete.asmx/DeleteSerializedGridFile',
            type: 'POST',
            contentType: 'application/json; charset=utf8',
            data: "{fileName: '" + value + "'}",
            dataType: 'JSON',
            failure: function (jqXHR, status, error) { alert('Couldn\'t delete serialized file! Please contact system administrator.'); }
            })
            });
            });
            */

            // $('#RefreshStatus').on('click', refeshStatusOfGrids);

            function refeshStatusOfGrids(GridIDs, colToMatchUpon, fieldToCompare, compareWithText, comparisonOp) {
                var cnfrm = confirm('Are you sure you want to update the stauts?');
                if (!cnfrm) return false;

                var localGrids = ['ctl00_ContentPlaceHolder1_grdMatchClothes', 'ctl00_ContentPlaceHolder1_grdMatchNotFound'], // remember to add GridId || []
                    localCols = colToMatchUpon || [6, 6],
                    field = fieldToCompare || 'BarCode',
                    compare = compareWithText || 'Delivered',
                    Op = '!=',
                    matchContent = [],
                    curGridCount = 0,
                    curGrid,
                    allRes,
                    curLen;

                showBlockMessage();

                //$.ajax();

                Array.prototype.forEach.call(localGrids, function (val, index) {
                    $('#' + val + ' tr  td:nth-child(' + localCols[index] + ')').each(function (i, v) {
                        console.log(i); console.log(v.textContent);
                        matchContent.push(v.textContent.trim());
                    });
                });
                /*
                matchContent = matchContent.filter(function (value, index, self) {
                return self.indexOf(value) === index; 
                });
                */
                $.ajax({
                    url: '../AutoComplete.asmx/UpdateConsolidatedStatus',
                    type: 'GET',
                    contentType: 'application/json; charset=utf8',
                    data: 'barCodes="' + matchContent.join(',') + '"',
                    dataType: 'JSON',
                    timeout: 5000
                }).done(function (data) {
                    var k = 1, removeRows = [], updateStatus = [];
                    curGrid = localGrids[curGridCount];
                    allRes = data.d.split(',');
                    curLen = $('#' + curGrid + ' tr').not(':first').length;
                    if (curLen === 0) {
                        curGrid = localGrids[++curGridCount];
                        curLen = $('#' + curGrid + ' tr').not(':first').length;
                    }
                    allRes.forEach(function (v, i) {
                        if (allRes[i].toLowerCase() === 'delivered' && $('#' + curGrid + ' tr').eq(k).find('td').eq(4).text() !== 'Already Delivered'/* && curGrid !== 'ctl00_ContentPlaceHolder1_grdMatchNotFound' */) {
                            removeRows.push($('#' + curGrid + ' tr').eq(k));
                        }
                        else {
                            if ($('#' + curGrid + ' tr').eq(k).find('td').eq(4).text() !== 'Already Delivered') {
                                $('#' + curGrid + ' tr').eq(k).find('td').eq(4).text(allRes[i]);
                            }
                        }
                        if (curLen - 1 === i && curGrid !== 'ctl00_ContentPlaceHolder1_grdMatchNotFound') {
                            curGrid = localGrids[++curGridCount];
                            curLen = $('#' + curGrid + ' tr').not(':first').length;
                            k = 0;
                        }
                        k++;
                    });

                    if (removeRows.length > 0) {
                        removeRows.forEach(function (valRow, idxRow) {
                            valRow.remove();
                        });
                    }
                    updateCount();
                });

                showBlockMessage(false);
                updateCount();
                return false;
            };
            /*
            $('#ctl00_ContentPlaceHolder1_btnSendToMainGrid').click(function (e) {
                
            var ary = [];
            $('#drpStatus').val('All');
            $('#ctl00_ContentPlaceHolder1_grdMatchNotFound td:nth-child(5)').each(function (i, v) {
            if (this.textContent.trim() !== 'Already Delivered') {
            ary.push($(this).parent().find('td').eq(5).text());
            }
            });
            $.ajax({
            url: '../AutoComplete.asmx/DeleteBarCodesFrom',
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf8',
            timeout: 5000,
            data: "{barCodes: '" + ary.join(',') + "',table: 'StockNotMatch'}",
            dataType: 'JSON',
            success: function (data, status, jqXHR) {
            data.d === 'Failed' && alert('Couldn\'t reset data! Please contact system administrator.');
            },
            failure: function (jqXHR, status, error) { alert('Couldn\'t reset data! Please contact system administrator.'); }
            });
            $('#ctl00_ContentPlaceHolder1_grdMatchNotFound td:nth-child(5)').filter(function (index, value) {
            return this.textContent.trim() !== 'Already Delivered';
            }).closest('tr').remove();
            $('#drpStatus').trigger('change');
                
            });*/

            $('#btnMatchClothesPrint, #btnNotMatchClothesPrint, #btnMainPrint').click(function (event) {
                var PrnValue = $('#hdnPrintValue').val();
                var ButtonID = $(event.target).attr('id');
                var strGridID = "";
                if (ButtonID == 'btnMainPrint') {
                    strGridID = "ctl00_ContentPlaceHolder1_grdAllClothes";
                }
                else if (ButtonID == 'btnMatchClothesPrint') {
                    strGridID = "ctl00_ContentPlaceHolder1_grdMatchClothes";
                }
                else if (ButtonID == 'btnNotMatchClothesPrint') {
                    strGridID = "ctl00_ContentPlaceHolder1_grdMatchNotFound";
                }
                // SetPrintOption(PrnValue, $(event.target).parent().find('table').attr('id'));
                SetPrintOption(PrnValue, strGridID);
            });

            function showBlockMessage(bHide) {

            }

            function SetPrintOption(PrnName, GridId) {
                if (typeof (jsPrintSetup) == 'undefined') {
                    installjsPrintSetup();
                } else {
                    var win4Print = window.open('', 'Win4Print');
                    var win4Print = window.open('', 'Win4Print');
                    var msg = document.getElementById(GridId);
                    jsPrintSetup.setPrinter(PrnName);
                    jsPrintSetup.setOption('orientation', jsPrintSetup.kPortraitOrientation);
                    jsPrintSetup.setOption('marginTop', 0);
                    jsPrintSetup.setOption('marginBottom', 0);
                    jsPrintSetup.setOption('marginLeft', 0);
                    jsPrintSetup.setOption('marginRight', 0);
                    // set empty page header
                    jsPrintSetup.setOption('headerStrLeft', '');
                    jsPrintSetup.setOption('headerStrCenter', '');
                    jsPrintSetup.setOption('headerStrRight', '');
                    // set empty page footer
                    jsPrintSetup.setOption('footerStrLeft', '');
                    jsPrintSetup.setOption('footerStrCenter', '');
                    jsPrintSetup.setOption('footerStrRight', '');
                    jsPrintSetup.setOption('printSilent', 1);
                    win4Print.document.write(msg.outerHTML);
                    win4Print.document.close();
                    win4Print.focus();
                    jsPrintSetup.print();
                    win4Print.close();
                }
            }


            function installjsPrintSetup() {
                if (confirm("You don't have printer plugin.\nDo you want to install the Printer Plugin now?")) {
                    var xpi = new Object();
                    xpi['jsprintsetup'] = '/mirrors.ibiblio.org/mozdev.org/jsprintsetup/jsprintsetup-0.9.2.xpi';
                    InstallTrigger.install(xpi);
                }
            }

        });
    </script>
    <script type="text/javascript" language="javascript">
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }  
    </script>
    <script type="text/javascript">

        window.isPendingRecon = true;

        /*
        window.onbeforeunload = function (e) {
        $('#ctl00_ContentPlaceHolder1_grdMatchClothes tr').length > 1 && isPendingRecon && serializeGrid('ctl00_ContentPlaceHolder1_grdMatchClothes', 'matchesClothes.htm');
        $('#ctl00_ContentPlaceHolder1_grdMatchNotFound tr').length > 1 && isPendingRecon && serializeGrid('ctl00_ContentPlaceHolder1_grdMatchNotFound', 'notMatchesClothes.htm');
        }
        */

        window.onbeforeunload = function (e) {
            SetOnlyOneTab(true);
        }

        function SetOnlyOneTab(bSetTo) {
            $.ajax({
                url: '../AutoComplete.asmx/SetOnlyOneTab',
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf8',
                timeout: 5000,
                data: "{bSetFalse: '" + bSetTo + "', branchId: '" + $('#hdnBranchId').val() + "'}",
                dataType: 'JSON',
                success: function (data, status, jqXHR) { if (data.d !== 'Record Saved') { alert('Couldn\'t set the tab view right. Please contact administrator'); } },
                failure: function (jqXHR, status, error) { alert('Couldn\'t set the tab view right. Please contact administrator'); }
            });
        }


        function updateCount(substractFromLast) {
            var _mtchCount, _ntMatchCount;
            var sub = substractFromLast || 1;
            _mtchCount = ($('#ctl00_ContentPlaceHolder1_grdMatchClothes tr').length - 1) >= 0 ? ($('#ctl00_ContentPlaceHolder1_grdMatchClothes tr').length - 1) : 0;
            _ntMatchCount = ($('#ctl00_ContentPlaceHolder1_grdMatchNotFound tr').length - 1) >= 0 ? ($('#ctl00_ContentPlaceHolder1_grdMatchNotFound tr').length - 1) : 0;
            $('#ctl00_ContentPlaceHolder1_grdMatchCount').text(_mtchCount);
            $('#ctl00_ContentPlaceHolder1_grdNotMatchedCount').text(_ntMatchCount);
            $('#ctl00_ContentPlaceHolder1_grdAllCount').text(($('#ctl00_ContentPlaceHolder1_grdAllClothes tr').length - sub) < 0 ? 0 : ($('#ctl00_ContentPlaceHolder1_grdAllClothes tr').length - sub));
        }

        function serializeGrid(gridToSerialize, fileNameToSaveAs) {
            $.ajax({
                url: '../AutoComplete.asmx/SerializeGridToFile',
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf8',
                timeout: 5000,
                data: "{gridText: '" + $('#' + gridToSerialize + '').closest('div').html() + "',fileName: '" + fileNameToSaveAs + "',bOverride:true}",
                dataType: 'JSON',
                success: function (data, status, jqXHR) {
                    data.d === 'Failed' && alert('Couldn\'t save data! Please contact system administrator.');
                    if (data.d !== 'Failed') {
                        var _allBarCodes = '', _flag = '';
                        $('#' + gridToSerialize + ' > tbody > tr td:nth-child(6)').each(function (i, v) {
                            _allBarCodes += this.textContent.trim() + '_';
                        });
                        if (_allBarCodes.split('_').length === 1)
                            _allBarCodes = _allBarCodes + '_';
                        else
                            _allBarCodes = _allBarCodes.substr(0, _allBarCodes.length - 1);
                        if (gridToSerialize === 'ctl00_ContentPlaceHolder1_grdMatchNotFound')
                            _flag = 'not';
                        else
                            _flag = 'yes';
                        $.ajax({
                            url: '../AutoComplete.asmx/SaveStockBarCodes',
                            type: 'POST',
                            async: false,
                            contentType: 'application/json; charset=utf8',
                            timeout: 5000,
                            data: "{BarCodes: '" + _allBarCodes + "',flag: '" + _flag + "'}",
                            dataType: 'JSON',
                            success: function (data, status, jqXHR) {
                                data.d === 'Failed' && alert('Couldn\'t save data! Please contact system administrator.');
                            },
                            failure: function (jqXHR, status, error) { alert('Couldn\'t save data! Please contact system administrator.'); }
                        });
                    }
                },
                failure: function (jqXHR, status, error) { alert('Couldn\'t save data! Please contact system administrator.'); }
            });
        }
         
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblStatusClothMsg" runat="server" ClientIDMode="Static" EnableViewState="False"
                        ForeColor="White"></asp:Label>
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary " >
        <div class="panel-heading">
            <h3 class="panel-title">
                Stock Reconciliation Report
            </h3>
        </div>
        <div class="panel-body well-sm-tiny">
            <div class="row-fluid">
                <div class="col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Status</span>
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control" ClientIDMode="Static"
                            OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Unprocessed" Value="1"></asp:ListItem>
                            <asp:ListItem Text="To be Recieve from WorkShop" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Pending For Finishing" Value="50"></asp:ListItem>
                            <asp:ListItem Text="Ready" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Barcode</span>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" ClientIDMode="Static" MaxLength="30"
                            CssClass="form-control" placeholder="Search Barcode"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtInvoiceNo_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="txtInvoiceNo" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                        </asp:FilteredTextBoxExtender>
                    </div>
                </div>
            </div>
            <div class="row-fluid div-margin2">
                <div class="col-sm-4 Textpadding">
                    &nbsp;Count :
                    <asp:Label ID="grdAllCount" runat="server" Font-Bold="true"></asp:Label>
                    <div class="panel panel-primary no-bottom-margin ">
                        <div class="panel-heading" style="padding: 2px">
                            <h3 class="panel-title text-center">
                                Garments in Inventory / Garments physically not available
                            </h3>
                        </div>
                        <div class="panel-body well-sm-tiny" style="height: 300px; background-color: White;
                            overflow: auto; white-space: nowrap">
                            <asp:GridView ID="grdAllClothes" runat="server" CssClass="Table Table-striped Table-bordered Table-hover"
                                EnableTheming="false" AutoGenerateColumns="false" OnDataBinding="grdAllClothes_DataBinding"
                                OnDataBound="grdAllClothes_DataBound" OnRowDataBound="grdAllClothes_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="B. Date"
                                        SortExpression="BookingDate" />
                                    <asp:TemplateField HeaderText="Inv No.">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="item" HeaderText="Clothes" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="item">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="Status">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BarCode" HeaderText="BarCode" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="BarCode">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle Font-Size="12px" />
                            </asp:GridView>
                        </div>
                        <div class="panel-footer">
                            <asp:LinkButton ID="btnAllClothesExport" runat="server" OnClick="btnAllClothesExport_Click"
                                class="btn btn-primary" EnableTheming="false">
          <i class="fa fa-file-excel-o"></i>&nbsp;Export to Excel</asp:LinkButton>
                            <asp:LinkButton ID="btnMainPrint" runat="server" ClientIDMode="Static" class="btn btn-primary"
                                EnableTheming="false">
                    <i class="fa fa-print"></i>&nbsp;Print
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4 Textpadding2">
                    &nbsp;Count :
                    <asp:Label ID="grdMatchCount" runat="server" Font-Bold="true"></asp:Label>
                    <div class="panel panel-primary no-bottom-margin" >
                        <div class="panel-heading" style="padding: 2px;background-color:#95FF95;color:Black">
                            <h3 class="panel-title text-center">
                                Garments found in inventory and physically available
                            </h3>
                        </div>
                        <div class="panel-body well-sm-tiny" style="height: 300px; background-color: White;
                            overflow: auto; white-space: nowrap">
                            <asp:GridView ID="grdMatchClothes" runat="server" CssClass="Table Table-striped Table-bordered Table-hover"
                                EnableTheming="false" OnRowDataBound="grdMatchClothes_RowDataBound" AutoGenerateColumns="false"
                                EmptyDataText="No matching entry found.">
                                <Columns>
                                    <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="B. Date"
                                        SortExpression="BookingDate" />
                                    <asp:TemplateField HeaderText="Inv No.">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="item" HeaderText="Clothes" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="item">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="Status">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BarCode" HeaderText="BarCode" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="BarCode">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle Font-Size="12px" />
                            </asp:GridView>
                        </div>
                        <div class="panel-footer">
                            <asp:LinkButton ID="btnMatchClothesExport" runat="server" class="btn btn-primary"
                                EnableTheming="false" OnClick="btnMatchClothesExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
                            <asp:LinkButton ID="btnMatchClothesPrint" runat="server" class="btn btn-primary"
                                EnableTheming="false" ClientIDMode="Static"><i class="fa fa-print"></i>&nbsp;Print</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4 Textpadding2">
                    &nbsp;Count :
                    <asp:Label ID="grdNotMatchedCount" runat="server" Font-Bold="true"></asp:Label>
                    <div class="panel panel-primary no-bottom-margin">
                        <div class="panel-heading" style="padding: 2px;background-color:#FF4A4A">
                            <h3 class="panel-title text-center">
                                Garments not in inventory but physically available
                            </h3>
                        </div>
                        <div class="panel-body well-sm-tiny" style="height: 300px; background-color: White;
                            overflow: auto; white-space: nowrap">
                            <asp:GridView ID="grdMatchNotFound" runat="server" CssClass="Table Table-striped Table-bordered Table-hover"
                                EnableTheming="false" OnRowDataBound="grdMatchNotFound_RowDataBound" AutoGenerateColumns="false"
                                EmptyDataText="No matching entry found.">
                                <Columns>
                                    <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="B. Date"
                                        SortExpression="BookingDate" />
                                    <asp:TemplateField HeaderText="Inv No.">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="item" HeaderText="Clothes" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="item">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="Status">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BarCode" HeaderText="BarCode" ItemStyle-HorizontalAlign="Right"
                                        SortExpression="BarCode">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle Font-Size="12px" />
                            </asp:GridView>
                        </div>
                        <div class="panel-footer">
                            <asp:LinkButton ID="btnNotMatchClothesExport" runat="server" class="btn btn-primary"
                                EnableTheming="false" OnClick="btnNotMatchClothesExport_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
                            <asp:LinkButton ID="btnNotMatchClothesPrint" runat="server" ClientIDMode="Static"
                                class="btn btn-primary" EnableTheming="false"><i class="fa fa-print"></i>&nbsp;Print</asp:LinkButton>
                            <asp:LinkButton ID="btnSendToMainGrid" runat="server" OnClick="btnSendToMainGrid_Click"
                                class="btn btn-primary" EnableTheming="false"><i class="fa fa-refresh"></i>&nbsp;Reset</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer well-sm-tiny">
            <div class="row-fluid">
                <div class="col-sm-2">
                </div>
                <div class="col-sm-4">
                    <asp:LinkButton ID="DoneRecon" runat="server" ClientIDMode="Static" class="btn btn-info btn-lg btn-block "
                        EnableTheming="false" OnClick="DoneReconClick"><i class="fa fa-refresh"></i>&nbsp;Reconciliation Done</asp:LinkButton>
                    <asp:ConfirmButtonExtender ID="DoneRecon_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure want to reset this report ?"
                        Enabled="True" TargetControlID="DoneRecon">
                    </asp:ConfirmButtonExtender>
                </div>
                <div class="col-sm-4">
                    <asp:LinkButton ID="RefreshStatus" runat="server" class="btn btn-info btn-lg btn-block"
                        EnableTheming="false" ClientIDMode="Static" OnClick="UpdateStausClick"><i class="fa fa-check-square-o"></i>&nbsp;Update Status</asp:LinkButton>
                    <asp:ConfirmButtonExtender ID="RefreshStatus_ConfirmButtonExtender" runat="server"
                        ConfirmText="Are you sure you want update status ?" Enabled="True" TargetControlID="RefreshStatus">
                    </asp:ConfirmButtonExtender>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnPrintValue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnFirstTimeCheck" Value="0" />
    <asp:HiddenField runat="server" ID="hdnAddedHeader" Value="false" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnRemovedEmptyMessage" Value="false" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnAddedHeader2" Value="false" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnRemovedEmptyMessage2" Value="false" ClientIDMode="Static" />
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
    <asp:HiddenField runat="server" ID="hdnAllHtmlNot" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnHead" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnData" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnFoot" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnBranchId" ClientIDMode="Static" />
    <script type="text/javascript">
        /*
        $(function (e) {

        LoadSerializedGrid('ctl00_ContentPlaceHolder1_grdMatchClothes', 'matchesClothes.htm', true);
        LoadSerializedGrid('ctl00_ContentPlaceHolder1_grdMatchNotFound', 'notMatchesClothes.htm', true);


        function LoadSerializedGrid(gridToLoad, fileNameToLoadFrom) {
        $.get('../SerializedGrids/' + fileNameToLoadFrom).done(function (data) {
        $('#' + gridToLoad).html(data);
        }).done(function (data) {
        $.ajax({
        url: '../AutoComplete.asmx/DeleteSerializedGridFile',
        type: 'POST',
        contentType: 'application/json; charset=utf8',
        data: "{fileName: '" + fileNameToLoadFrom + "'}",
        dataType: 'JSON',
        timeout: 5000,
        failure: function (jqXHR, status, error) { alert('Couldn\'t delete serialized file! Please contact system administrator.'); }
        }).done(function (data) {
        SetHeadersOfGrids();
        gridToLoad === 'ctl00_ContentPlaceHolder1_grdMatchClothes' && RemoveDuplicateRows(gridToLoad, 'ctl00_ContentPlaceHolder1_grdAllClothes');
        });
        });
        }

        function SetHeadersOfGrids() {
        if ($('#ctl00_ContentPlaceHolder1_grdMatchClothes').find('th').length == 0)
        $('#hdnAddedHeader').val('false');
        else 
        $('#hdnAddedHeader').val('true');

        if ($('#ctl00_ContentPlaceHolder1_grdMatchClothes :contains("No Matching Reconds")').length !== 0)
        $('#hdnRemovedEmptyMessage').val('false');
        else 
        $('#hdnRemovedEmptyMessage').val('true');


        if ($('#ctl00_ContentPlaceHolder1_grdMatchNotFound').find('th').length == 0)
        $('#hdnAddedHeader2').val('false');
        else 
        $('#hdnAddedHeader2').val('true');
                    
        if ($('#ctl00_ContentPlaceHolder1_grdMatchNotFound :contains("No Matching Reconds")').length !== 0) 
        $('#hdnRemovedEmptyMessage2').val('false');
        else 
        $('#hdnRemovedEmptyMessage2').val('true');
        }

        function RemoveDuplicateRows(gridToCheckFrom, gridToRemoveFrom, colInFirtGrid, colInSecondGrid) {
        /*
        var colfirst = colInFirtGrid || 6;
        var barCodes = [];
        $('#' + gridToCheckFrom + ' tr td:nth-child(' + colfirst + ')').each(function (i, val) {
        barCodes.push(this.textContent.trim());
        });
        barCodes.forEach(function (value, index) {
        $('#' + gridToRemoveFrom + ' td:excontains(' + value + ')').closest('tr').remove();
        });
                
        updateCount();
        }

        });  */  
    </script>
</asp:Content>
