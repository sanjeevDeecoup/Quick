<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
	CodeBehind="frmChallanReturn.aspx.cs" Inherits="QuickWeb.Bookings_New.frmChallanReturn"
	EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
     <script src="../js/tooltip.js" type="text/javascript"></script>     
	<script type="text/javascript">
		$.extend($.expr[':'], { excontains: function (obj, index, meta, stack) { return (obj.textContent || obj.innerText || $(obj).text() || "").toLowerCase() == meta[3].toLowerCase(); } });
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
			// removeRedundantRowsFromGrid('ctl00_ContentPlaceHolder1_grdNewChallan', 'ctl00_ContentPlaceHolder1_grdSelectedCloth', 5, true);
			disableButtons();
			$('body').click(function (event) {
				if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpProcess') {
					return;
				}
				if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtHolidayDate') {
					return;
				}
				if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpsmstemplate') {
					return;
				}
				if ($(event.target).attr('id') == 'drpPrintStartFrom') {
					return;
				}
				if (event.target.id === 'txtPnlBarCodeText') {
					return;
				}
				if (event.target.id === 'ddlChallanNumber') {
					return;
				}
				/*
				if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_btnSaveAndPrintNew') {
				__doPostBack('ctl00$ContentPlaceHolder1$btnSaveAndPrintNew', null);
				}
				*/
				$('#txtBarcode').focus();
			});

			$("#ctl00_ContentPlaceHolder1_txtHolidayDate").change(function () {
				var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				if (_allRowsCount > 1) {
					// save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
					var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
					__doPostBack('ctl00$ContentPlaceHolder1$btnTemp', _allHTMLToSave);
				}
				else {
					__doPostBack('ctl00$ContentPlaceHolder1$btnTemp', null);
				}
				//document.getElementById("ctl00_ContentPlaceHolder1_btnTemp").click();
			});
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
					$('#txtRemoverChallan').attr('disabled', false);
					$('#txtRemoverChallan').val('');
					$('#txtRemoverChallan').focus();
					$('#btnSaveRemoveChallan').attr('disabled', false);
					$('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
					$('#txtBarcode').attr('disabled', true);
					$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan, :checkbox').attr('disabled', true).addClass('disabledClass');
				}
				else {
					$('#lblRemove').hide();

					$('#txtRemoverChallan').attr('disabled', true);
					$('#txtRemoverChallan').val('');
					$('#txtBarcode').focus();
					$('#btnSaveRemoveChallan').attr('disabled', true);
					$('#btnSaveChallan').attr('disabled', false).removeClass('disabledClass');
					$('#txtBarcode').attr('disabled', true);
					$('#txtBarcode').focus();
					$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan,:checkbox').attr('disabled', false).removeClass('disabledClass');
				}
				e.stopImmediatePropagation();
				e.stopPropagation();
			});

			$('#btnSaveRemoveChallan').click(function (e) {
				e.preventDefault();
				$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan, :checkbox').attr('disabled', false).removeClass('disabledClass');
				if ($('#txtRemoverChallan').val() == '') {
					alert('Please select a reason to return cloth');
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
					var itemName = $(this).find('td:eq(6)').text().trim();
					var idx = sr.indexOf('-');
					var idx2 = sr.lastIndexOf('-');
					var str = sr.substring(idx + 1, idx2);
					if (allDataToRemove == '') {
					    allDataToRemove = bk + ':' + str + ':' + itemName;
					}
					else {
					    allDataToRemove += '_' + bk + ':' + str + ':' + itemName;
					}
				});

				//allDataToRemove = allDataToRemove.substr(0, allDataToRemove.length - 1);
				$('#hdnRmvReasonData').val(allDataToRemove);


				$.ajax({
					url: '../AutoComplete.asmx/CheckCorrectRemoveReasonData',
					data: "{retText: '" + $('#hdnRmvReason').val() + "'}",
					type: 'POST',
					timeout: 20000,
					contentType: 'application/json; charset=UTF-8',
					datatype: 'JSON',
					cache: true,
					async: false,
					success: function (response) {
						var result = response.d;
						if (result === true) {
							SaveRemoveChallanDetail();
							chkRemove.checked = false;
							stateOfColor = true;
							setDivMouseOver('#00aa00', '#999999');
							window['LTRRemove'].remove();
							window['LTR'] = null;
							$('#txtRemoverChallan').attr('disabled', true);
							$('#txtRemoverChallan').val('');
							$('#btnSaveRemoveChallan').attr('disabled', true);
							setQtyInLabel();
							disableSaveButtons();
							return false;
						}
						else {
							$('#lblMsg').text('Reason not available in pre defined cause list.');
							stateOfColor = true;
							setDivMouseOver('#FF0000', '#999999');
							$('#txtRemoverChallan').focus();
							$('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
							$('#txtBarcode').attr('disabled', true);
							$('#chkPrintChallan').attr('disabled', true);
							$('#chkPrintSticker').attr('disabled', true);
							$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan, :checkbox').attr('disabled', true).addClass('disabledClass');
							return false;
						}
					},
					error: function (response) {
						alert(response.toString())
					}
				});



//                var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
//                if (_allRowsCount > 1) {
//                    // save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
//                    var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
//                    __doPostBack('txtRemoverChallan', _allHTMLToSave);
//                }
//                else {
//                    __doPostBack('txtRemoverChallan', null);
//                }
			});

			function SaveRemoveChallanDetail() {
				var screenFlag = "35";
				$.ajax({
					url: '../AutoComplete.asmx/SaveRemoveChallanData',
					data: "{retText: '" + $('#hdnRmvReason').val() + "', DataToRemove: '" + $('#hdnRmvReasonData').val() + "', flag: '" + screenFlag + "',ScreenName:2}",
					type: 'POST',
					timeout: 20000,
					contentType: 'application/json; charset=UTF-8',
					datatype: 'JSON',
					cache: true,
					async: false,
					success: function (response) {
						var result = response.d;
						if (result === 'Record Saved') {
							$('#lblMsg').text('Cloth Returned Unprocessed.');
							$('#hdnRmvReason').val('');
						}
						else {
							alert('Reason not available in pre defined cause list.');
							$('#txtRemoverChallan').focus();
						}
					},
					error: function (response) {
						alert(response.toString())
					}
				});
			}

			var drpProcessChangeHanlder = function (e) {
				/* */
				var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				if (_allRowsCount > 1) {
					// save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
					var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
					__doPostBack('ctl00$ContentPlaceHolder1$drpProcess', _allHTMLToSave);
					$('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
				}
				else {
					__doPostBack('ctl00$ContentPlaceHolder1$drpProcess', null);
					$('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
				}
			};
			$('#ctl00_ContentPlaceHolder1_drpProcess').on('change', drpProcessChangeHanlder);

			$('#ctl00_ContentPlaceHolder1_txtHolidayDate').dblclick(function (e) {
				$(this).val('');
				var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				if (_allRowsCount > 1) {
					// save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
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
				$('#lblErr').text('');
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
					$('#ctl00_ContentPlaceHolder1_drpProcess').off();
					$('#ctl00_ContentPlaceHolder1_drpProcess option:eq(0)').attr('Selected', true);
					$('#ctl00_ContentPlaceHolder1_drpProcess').on('change', drpProcessChangeHanlder);
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
					error: function () { }
				});
				return _result;
			}


			// the buttons
			$('#btnMoveRight').click(function (event) {
				event.preventDefault();
				$('#lblMsg').text('');
				$('#lblErr').text('');
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
				$('#lblErr').text('');
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
				$('#lblErr').text('');
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
				$('#lblErr').text('');
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
			function setLeftGridHeaders() {
				if ($('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').length === 1) {
					$('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').clone().insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:first');
					$('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)').remove();
				}
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
				//   $('#DivContainerStatus').css('background-color', argColor);
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
					$('#' + gridId + ' td:nth-child(5)').each(function (i, v) { allBarCodes += this.textContent + ','; });
					if (allBarCodes.length > 0) {
						allBarCodes = allBarCodes.substr(0, allBarCodes.length - 1);
					}

				}
				else {
					allBarCodes = barcode;
				}

				$.ajax({
					url: '../Autocomplete.asmx/ChangeChallanStatus',
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
				if ($('#chkRemove').is(':checked') && _leftSize > 0) {
					return;
				}
				if (_leftSize == 0) {
					$('#btnMoveRight').attr('disabled', true);
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
					$('#ctl00_ContentPlaceHolder1_btnPrint').attr('disabled', true).addClass('disabledClass');
					$('#btnSaveChallanReturn').attr('disabled', true).addClass('disabledClass');
					$('#bntSendToSP').attr('disabled', true).addClass('disabledClass');
				}
				else {
					$('#ctl00_ContentPlaceHolder1_btnPrint').attr('disabled', false).removeClass('disabledClass');
					$('#btnSaveChallanReturn').attr('disabled', false).removeClass('disabledClass');
					$('#bntSendToSP').attr('disabled', false).removeClass('disabledClass');
				}
			}


			/* WARNING! WARNING! WARNING! */
			/* Not in corrsopondence with LSJA */
			var 
				chkCounterLft = 0,
				chkCounterRht = 0;
			/************************* 
			SHORTCUT KEYS 
			*************************/
			$('body').on('keydown', function (e) {
				if (e.which === 37) {
					if (e.altKey) {
						if (e.ctrlKey && !$('#btnMoveLeft').attr('disabled')) {
							/** Alt + Ctrl + Left => Move left **/
							$('#btnMoveLeft').click();
						}
						else if (e.shiftKey && !$('#btnMoveLeftAll').attr('disabled')) {
							/** Alt + Shift + Left => Move left all **/
							$('#btnMoveLeftAll').click();
						}
					}
				}
				else if (e.which === 39) {
					if (e.altKey) {
						if (e.ctrlKey && !$('#btnMoveRight').attr('disabled')) {
							/** Alt + Ctrl + Right => Move right **/
							$('#btnMoveRight').click();
						}
						else if (e.shiftKey && !$('#btnMoveRightAll').attr('disabled')) {
							/** Alt + Shift + Right => Move right all **/
							$('#btnMoveRightAll').click();
						}
					}
				}
				else if (e.which === 38) {
					if (e.altKey) {
						(function (index) {
							$('#ctl00_ContentPlaceHolder1_grdNewChallan :checkbox').eq(index).focus();
						})(chkCounterLft--)
					}
					else if (e.ctrlKey) {
						(function (index) {
							$('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checkbox').eq(index).focus();
						})(chkCounterRht--)
					}
				}
				else if (e.which === 40) {
					if (e.altKey) {
						(function (index) {
							$('#ctl00_ContentPlaceHolder1_grdNewChallan :checkbox').eq(index).focus();
						})(chkCounterLft++)
					}
					else if (e.ctrlKey) {
						(function (index) {
							$('#ctl00_ContentPlaceHolder1_grdSelectedCloth :checkbox').eq(index).focus();
						})(chkCounterRht++)
					}
				}
			});

			var leaveHandler = function (e) {
				var leaveHandler;
				var inFormLink;
				// var _activeElemt = ee.target.activeElement.attributes['id'].value;
				var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				/*
				if (_activeElemt == 'ctl00_ContentPlaceHolder1_drpProcess') {
				if (_allRowsCount > 1) {
				// save the html of this grid, and on load, show it, also set the emptry row removed and header copied to true
				var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
				$('#hdnAllHtml').val(_allHTMLToSave);
				}
				}
				if (_activeElemt == 'ctl00_ContentPlaceHolder1_drpProcess' || _activeElemt == 'ctl00_ContentPlaceHolder1_drpMulti' || _activeElemt == 'ctl00_ContentPlaceHolder1_btnRefresh' || _activeElemt == 'txtBarcode') {
				return;
				}
				*/
				if (_allRowsCount > 1) {
					return true; // confirm("You have selected few cloths but not submitted to send to workshop. If you leave this page these changes will go unsaved. Click OK to Go Back to Workshop Note,  Click Cancel to Leave Anyway");
				}
			}
			function checkIfSMSAvailable() {
				var result = '';
				$.ajax({
					url: '../Autocomplete.asmx/checkIfSMSAvailable',
					//data: 'bookingNumber = ' + argBookingNumber,
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

		});
	</script>
	<script type="text/javascript">
	    $(document).ready(function (event) {

	        function setDivColor(argColorOne1, argColorTwo2) {
	            if (stateOfColor2) {
	                $('#DivContainerInnerStatus').animate({ backgroundColor: argColorOne1 }, 200).delay(1000).animate({ backgroundColor: argColorTwo2 }, 100);
	            }
	            else {
	                $('#DivContainerInnerStatus').animate({ backgroundColor: argColorTwo2 }, 1000);
	                $('#DivContainerInnerStatus').animate({ backgroundColor: argColorOne1 }, 100);
	            }
	            stateOfColor2 = !stateOfColor2;
	        }

	        function BlankGrid() {
	            stateOfColor2 = true;
	            setDivColor('#00aa00', '#999999');
	            $('#lblStatusCloth').text('');
	            //$('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:not(:first-child)').remove();
	            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html('<table class="mgrid" rules="all" id="ctl00_ContentPlaceHolder1_grdSelectedCloth" style="border-collapse:collapse;" cellspacing="0" border="1"><tbody><tr><td colspan="9">There are no pending Cloth to receive/Mark Ready:</td></tr></tbody></table>');
	            $('#hdnRemovedEmptyMessage').val('false');
	            $('#hdnAddedHeader').val('false');
	            window['LTR'] = null;
	            var _rightSize1 = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
	            if (_rightSize1 <= 1) {
	                $('#ctl00_ContentPlaceHolder1_btnPrint').attr('disabled', true).addClass('disabledClass');
	                $('#btnSaveChallanReturn').attr('disabled', true).addClass('disabledClass');
	                $('#bntSendToSP').attr('disabled', true).addClass('disabledClass');
	            }
	            else {
	                $('#ctl00_ContentPlaceHolder1_btnPrint').attr('disabled', false).removeClass('disabledClass');
	                $('#btnSaveChallanReturn').attr('disabled', false).removeClass('disabledClass');
	                $('#bntSendToSP').attr('disabled', false).removeClass('disabledClass');
	            }

	            var _prvVal1 = $('#lblQtyCount').text();
	            var _qtyCount1 = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() - 1;
	            if (parseInt(_qtyCount1) < 0) {
	                _qtyCount1 = 0;
	            }
	            $('#lblQtyCount').text(_qtyCount1);
	            // $('#txtPnlBarCodeText').val('');
	            $('#txtBarcode').focus();
	        }
	        $('#btnSaveChallanReturn, #ctl00_ContentPlaceHolder1_btnPrint, #bntSendToSP').click(function (e) {
	            //var sound = document.getElementById('sound1');
	            //sound.Play();
	            $('#lblErr').text('');
	            if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() <= 1) {
	                alert('No cloth selected to save!');
	                return false;
	            }
	            //                if ($('#ctl00_ContentPlaceHolder1_drpMulti').is(':visible') && $('#ctl00_ContentPlaceHolder1_drpMulti').val() == 'Select') {
	            //                    alert('Please select a factory');
	            //                    return false;
	            //                }
	            else {
	                // make the array
	                //  makeTheArrayToStore();
	                //  var target = $(e.target);

	                window['clickedId'] = e.target.id;
	                /**** WARNING! REGEX AHEAD ****/
	                if (hdnAskForBarCode.value !== '' && !/^\s*(:*\s*)*\s*$/.test(hdnAskForBarCode.value) && e.target.id !== 'bntSendToSP') {
	                    // ask for password
	                    $('#pnlBarCode').dialog({ width: 430, modal: true });
	                    window['temp.AskForPrint'] = e.target.id !== 'btnSaveChallanReturn';
	                    return false;
	                }
	                //                                    else if (target.attr('id') == 'ctl00_ContentPlaceHolder1_btnPrint') {
	                //                                        window['temp.AskForPrint'] = true;
	                //                                        askForPrintStartNumber();
	                //                                        return false;
	                //                                    }
	                else {
	                    window['temp.AskForPrint'] = false;
	                }
	                //                    if ($(e.target).attr('id') == 'ctl00_ContentPlaceHolder1_btnPrint') {
	                //                        $('#pnlPanel').dialog({ width: 400, modal: true });
	                //                        e.preventDefault();
	                //                    }
	                //   }
	                var strStatus = 0;
	                if (e.target.id === 'bntSendToSP') {
	                    strStatus = 50;
	                    SaveReadyOrFinishingData(strStatus, e.target.id);
	                }
	                else if (e.target.id === 'btnSaveChallanReturn') {
	                    strStatus = 3;
	                    SaveReadyOrFinishingData(strStatus, e.target.id);
	                }
	                if (e.target.id === 'ctl00_ContentPlaceHolder1_btnPrint') {
	                    // if printing stickers then ask where to print from
	                    $('#pnlPanel').dialog({ width: 430, modal: true });
	                    return false;
	                }
	                return false;
	            }
	        });
	        function SaveReadyOrFinishingData(strStatus, strBtnEvent) {
	            var ScreenStatus = 2;
	            var IsSMS = false;
	            if ($('#chkSendsms').attr('checked')) {
	                IsSMS = true;
	            } else {
	                IsSMS = false;
	            }
	            $.ajax({
	                url: '../AutoComplete.asmx/SendForFinishingOrReady',
	                data: "{Status: '" + strStatus + "', ScreenStatus: '" + ScreenStatus + "',PinNo: '" + $('#hdnReadyByPin').val() + "',strSMS: '" + IsSMS + "',SMSType: '" + $('#ctl00_ContentPlaceHolder1_drpsmstemplate').val() + "'}",
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
	                        if (strBtnEvent === 'bntSendToSP') {
	                            $('#lblMsg').text('Cloths sent for Finishing.');
	                        }
	                        else if (strBtnEvent === 'btnSaveChallanReturn') {
	                            $('#lblMsg').text('Cloths received from Workshop & Marked Ready.');
	                        }
	                    }
	                },
	                error: function (response) {
	                    alert(response.toString())
	                }
	            });
	        }
	        $('#ctl00_ContentPlaceHolder1_btnSaveAndPrintNew').click(function (e) {
	            var ScreenStatus1 = 2;

	            var IsSMS2 = false;
	            if ($('#chkSendsms').attr('checked')) {
	                IsSMS2 = true;
	            } else {
	                IsSMS2 = false;
	            }	          
	            var t2 = $('#drpPrintStartFrom').val();
	            $('#hdnStartValue').val(t2);
	            $('#pnlPanel').dialog('close');
	            $.ajax({
	                url: '../AutoComplete.asmx/SaveChallanAndWorkShopNoteAndPrintSticker',
	                data: "{Possition: '" + t2 + "', ScreenStatus: '" + ScreenStatus1 + "',PinNo: '" + $('#hdnReadyByPin').val() + "',strSMS: '" + IsSMS2 + "',SMSType: '" +  $('#ctl00_ContentPlaceHolder1_drpsmstemplate').val() + "'}",
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
	                        $('#drpPrintStartFrom').val('1');
	                        $('#lblMsg').text('Cloths received from Workshop & Marked Ready.');
	                        window.open('../Bookings/printlabels.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Bookings/NewChallan.aspx');
	                    }
	                },
	                error: function (response) {
	                    alert(response.toString())
	                }
	            });
	            return;
	        });

	        $('#pnlBarCode').on('keydown', 'input', function (e) {
	            if (e.which !== 13 && e.which !== 9) return;

	            // creaate a list to hold the pins and barcodes
	            var lists = [];

	            // split value(s) populate the list
	            hdnAskForBarCode.value.split('~').forEach(function (v) {
	                v.split(':').forEach(function (v2) {
	                    lists.push(v2);
	                });
	            });

	            // filter list to remove empty string
	            lists = lists.filter(function (v) {
	                return v !== '';
	            });

	            // check if the entered value is in the list of pins and barcode
	            if (lists.indexOf(txtPnlBarCodeText.value) !== -1) {
	                setThePin(txtPnlBarCodeText.value);
	                // hdnReadyByPin.value =
	                //  txtPnlBarCodeText = '';
	                $('#txtPnlBarCodeText').val('');
	                pnlBarCodeMsg.textContent = '';
	                $('#pnlBarCode').dialog('close');

	                if (window['temp.AskForPrint']) {
	                    askForPrintStartNumber();
	                    return false;
	                }
	                else {
	                    SaveReadyOrFinishingData(3, 'btnSaveChallanReturn')
	                    return false;
	                }

	            }
	            else {
	                $('#pnlBarCodeMsg').text('Incorrrect PIN / BarCode').focus().select();
	                return false;
	            }
	        });

	        // This sets the value in hidden filed to the pin
	        /******************************/
	        /**** WARNING! REGEX AHEAD ****/
	        function setThePin(pinOrBarcode) {
	            var testRegex = new RegExp(pinOrBarcode + '~');
	            // check if the scanned value is PIN itself
	            if (hdnAskForBarCode.value.match(testRegex) !== null) {
	                // just store the PIN
	                hdnReadyByPin.value = pinOrBarcode;
	            }
	            else {
	                var regex = new RegExp('(' + pinOrBarcode + ':.*?)(?=~)');
	                var str = hdnAskForBarCode.value.match(regex);
	                var pin = str[0].replace(/.*:/, '').replace(/~/, '');
	                hdnReadyByPin.value = pin;
	            }
	        }

	        function askForPrintStartNumber() {
	            $('#pnlPanel').dialog({ width: 430, modal: true });
	            return false;
	        }
	        $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('position', 'relative');
	        //  $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('width', '-20px');
	        $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div');
	        function makeTheArrayToStore() {
	            var _rowData = '';
	            var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
	            for (var _i = 1; _i < _grdSize; _i++) {
	                // first the booking number
	                _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(2)').text() + ':';
	                // now the item serial number
	                _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(4)').text().split('-')[1] + ':';
	                // subItem name
	                _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(6) > span').text() + '';
	                /*
	                // now the qty, hardcoding 1
	                _rowData += '1' + ':';
	                // now the urgent
	                _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(7) > span').text() + ':';
	                */
	                // now add a '_' to seperate rows
	                _rowData += '_';
	            }
	            _rowData = _rowData.substr(0, _rowData.length - 1);
	            $('#hdnAllData').val(_rowData);
	            return true;
	        }
	        $('#ddlChallanNumber').change(function (e) {
	            $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
	        });

	        $('#btnSms').click(function (e) {
	            $('#lblMsg').text('');
	            $('#lblErr').text('');
	            $('#lblStatusCloth').text('');
	            var smsType = $('#ctl00_ContentPlaceHolder1_drpsmstemplate').val();
	            $.ajax({
	                url: '../AutoComplete.asmx/SendSMS',
	                data: "{smsEvent: '" + smsType + "'}",
	                type: 'POST',
	                timeout: 20000,
	                contentType: 'application/json; charset=UTF-8',
	                datatype: 'JSON',
	                cache: true,
	                async: false,
	                success: function (response) {
	                    var result = response.d;
	                    if (result === 'Done') {
	                        $('#lblMsg').text('Message sent successfully for all the orders with all cloths ready.');
	                        stateOfColor2 = true;
	                        setDivColor('#00aa00', '#999999');
	                    }
	                    else {
	                        $('#lblErr').text('No completed Order found to SEND SMS.');
	                        stateOfColor2 = true;
	                        setDivColor('#FF0000', '#999999');
	                    }
	                },
	                error: function (response) {
	                    alert(response.toString())
	                }
	            });
	            e.preventDefault();
	        });
	    });
	</script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#achruserDetail').click(function (e) {
                var status = "2";
                $.ajax({
                    url: '../AutoComplete.asmx/GetUserDetailsData',
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
			<h3 class="panel-title">
				<span class="textBold">Receive From WorkShop - </span>Mark all cloths that are READY
				TO BE DELIVERED or Send for FINISHING
                <a class="btn btn-info margin4 " runat="server" style="float:right;margin-top:-7px" clientidmode="Static" id="achruserDetail">
                        <i class="fa fa-th"></i>&nbsp;&nbsp;User Selected Garment</a>
                </h3>
		</div>
		<div class="panel-body well-sm-tiny">
			<div class="row-fluid">
				<div class="col-sm-2">
					<asp:TextBox ID="txtBarcode" runat="server" ClientIDMode="Static" OnTextChanged="txtBarcode_TextChanged"
						CssClass="form-control" placeholder="Order / Barcode No" MaxLength="20"></asp:TextBox>
					<cc1:FilteredTextBoxExtender ID="txtBarcode_FilteredTextBoxExtender" runat="server"
						Enabled="True" TargetControlID="txtBarcode"  ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-" >
					</cc1:FilteredTextBoxExtender>
				</div>
				<div class="col-sm-2">
					<asp:DropDownList ID="drpProcess" runat="server" AppendDataBoundItems="true" CssClass="form-control">
					</asp:DropDownList>
				</div>
				<div class="col-sm-2">
					<asp:DropDownList runat="server" ID="ddlChallanNumber" ClientIDMode="Static" AutoPostBack="true"
						OnSelectedIndexChanged="DdlChallanNumberSelectedIndexChanged" AppendDataBoundItems="true"
						CssClass="form-control">
						<asp:ListItem Text="All - WorkshopNote" Value="All"></asp:ListItem>
					</asp:DropDownList>
				</div>
				<div class="col-sm-2 form-inline nopadding">
					<div class="form-group">
						<asp:TextBox ID="txtHolidayDate" runat="server" MaxLength="11" onkeypress="return false;"
							placeholder="Delivery Date" CssClass="form-control" ToolTip="Double Click To Clear The Date"
							onpaste="return false;"></asp:TextBox>
						<cc1:CalendarExtender ID="txtHolidayDate_CalendarExtender" runat="server" Enabled="True"
							Format="dd-MMM-yyyy" TargetControlID="txtHolidayDate">
						</cc1:CalendarExtender>
					</div>
					<div class="form-group">
						<asp:Button ID="btnClearDate" ClientIDMode="Static" runat="server" Text="X" CssClass="btn btn-primary btn-block"
							Width="20px" />
					</div>
					<div class="form-group" style="display: none">
					</div>
				</div>

                <div class="col-sm-1 div-margin">
				<asp:CheckBox ID="chkSendsms" runat="server"  Text="&nbsp;&nbsp;SMS" ClientIDMode="Static"  Checked="true"/>                
				</div>
				<div class="col-sm-2">
					<asp:DropDownList ID="drpsmstemplate" runat="server" CssClass="form-control">
					</asp:DropDownList>
				</div>				
				<div>
					<asp:Button ID="btnSms" runat="server" Text="Send Sms" disabled="Disabled" Visible="false"  EnableTheming="false" CssClass="btn btn-primary"
						ClientIDMode="Static" />
				</div>
			</div>
			<div class="row-fluid">
				<div class="col-sm-4 form-inline">
					<div class="form-group">
						<asp:CheckBox ID="chkRemove" runat="server" Text="&nbsp;Return Cloth" 
							ClientIDMode="Static" />
					</div>
					<div class="form-group">
						<asp:Label ID="lblRemove" runat="server" Text="Cloth Return Cause  :" Visible="false"></asp:Label>
					</div>
					<div class="form-group">
						<asp:TextBox ID="txtRemoverChallan" runat="server" Width="190px" MaxLength="250"
							CssClass="form-control" disabled="Disabled" ClientIDMode="Static"></asp:TextBox>
						<cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtRemoverChallan"
							ServicePath="~/AutoComplete.asmx" ServiceMethod="GetDetailRemoveReasonMaster"
							MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True"
							CompletionListCssClass="AutoExtender_new" CompletionListItemCssClass="AutoExtenderList_new"
							CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
						</cc1:AutoCompleteExtender>
					</div>
					<div class="form-group">
						<asp:Button ID="btnSaveRemoveChallan" runat="server" 
							CssClass="btn btn-danger" disabled="Disabled" EnableTheming="false" ClientIDMode="Static"
							Text="Return" />
					</div>
				</div>
				<div class="col-sm-3">
					<asp:Button ID="bntSendToSP" runat="server" Text="Send for Finishing" ToolTip="Receive from Workshop & Send for Further Processing"
						EnableTheming="false" ClientIDMode="Static" CssClass="disabledClass btn btn-primary btn-block" />
				</div>
				<div class="col-sm-2 ">
					<asp:Button ID="btnSaveChallanReturn" runat="server" Text="Ready for Delivery" ToolTip="Receive from Workshop & Mark Ready to be Delivered"
						EnableTheming="false" ClientIDMode="Static" CssClass="disabledClass btn btn-primary btn-block" />
				</div>
				<div class="col-sm-3">
					<asp:Button ID="btnPrint" runat="server" Text="Ready for Delivery & Print Sticker"
						EnableTheming="false" ToolTip="Receive from Workshop, Mark Ready to be Delivered & Generate Packing Stickers"
						CssClass="disabledClass btn btn-primary btn-block" />
				</div>
			</div>
			<div class="row-fluid">
				<div class="col-sm-4">
				</div>
			</div>
			<div class="row-fluid">
				<div class="span1  well well-sm-tiny" id="DivLeftContainer">
					<div id="DivLeftCounter" class="span label label-default">
						<h4>
							<asp:Label runat="server" ID="lblLeft" Text="" ClientIDMode="Static"></asp:Label></h4>
					</div>
				</div>
				<div id="DivContainerStatus" class="span10  well well-sm-tiny">
					<div id="DivContainerInnerStatus" class="span label label-default">
						<h4 class="textmargin">							
							<span style="margin-left: 40%">
							<asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" ForeColor="White"
								ClientIDMode="Static" />
								<asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
									ForeColor="White" CssClass="SuccessMessage" /></span> <span style="margin-left: -20%">
										<asp:Label ID="lblStatusCloth" runat="server" EnableTheming="false" ClientIDMode="Static"></asp:Label>&nbsp;</span></h4>
						<%-- <asp:Label ID="lblStatusCloth" runat="server" ClientIDMode="Static"></asp:Label>
		  <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
		  <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />--%>
					</div>
				</div>
				<div id="DivRightContainer" class="span1  well well-sm-tiny" style="margin-left: 23px">
					<div id="DivRightCounter" class="span label label-default">
						<h4>
							<asp:Label ID="lblQtyCount" runat="server" Text="" ClientIDMode="Static"></asp:Label></h4>
					</div>
				</div>
			</div>
			<div class="row-fluid div-margin1">
				<div class="row-fluid form-signin4 no-bottom-margin">
					<div class="span6 well well-sm no-bottom-margin">
						<div class="DivStyleWithScroll" style="overflow: scroll; height: 400px;">
							<asp:GridView ID="grdNewChallan" runat="server" CssClass="mgrid" AutoGenerateColumns="False"
								EmptyDataText="No Cloth available to display, Please select appropriate value from top."
								ShowFooter="false" OnDataBinding="grdNewChallan_DataBinding" OnRowDataBound="grdNewChallan_RowDataBound"
								OnDataBound="grdNewChallan_DataBound" EnableTheming="false">
								<Columns>
									<asp:TemplateField>
										<HeaderTemplate>
											<asp:Label ID="Label1" runat="server" Text=""></asp:Label>
										</HeaderTemplate>
										<ItemTemplate>
											<asp:CheckBox ID="chkSelect" runat="server" />
										</ItemTemplate>
										<%--<ItemStyle Width="2%" />
									<HeaderStyle Width="2%" />--%>
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
										<%-- <ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:BoundField DataField="BookingDeliveryDate" HeaderText="Due Date" ReadOnly="True"
										SortExpression="BookingDeliveyDate">
										<%--	<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:BoundField DataField="Barcode" HeaderText="Barcode" ReadOnly="True">
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:BoundField DataField="Customer" HeaderText="Customer" ReadOnly="True" SortExpression="Customer">
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:TemplateField HeaderText="Cloth" SortExpression="Item" ItemStyle-Wrap="true">
										<ItemTemplate>
											<asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
											<asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
										</ItemTemplate>
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
										<FooterStyle HorizontalAlign="Right" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="U" ItemStyle-ForeColor="Red" ItemStyle-Width="20px">
										<ItemTemplate>
											<asp:Label ID="lblUrgent" runat="server" Text='<%# Bind("IsUrgent") %>'></asp:Label>
										</ItemTemplate>
										<ItemStyle ForeColor="Red" />
										<%--<HeaderStyle Width="2%" />--%>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Service">
										<ItemTemplate>
											<asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()
										%>'></asp:Label>
										</ItemTemplate>
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Qty" SortExpression="ItemTotalQuantity" Visible="false">
										<FooterTemplate>
											<asp:Label ID="lblItemscount" runat="server"></asp:Label><br />
										</FooterTemplate>
										<ItemTemplate>
											<asp:Label ID="lblItemQty" runat="server" Text='<%# Bind("ItemTotalQuantity") %>'></asp:Label>
										</ItemTemplate>
										<FooterStyle HorizontalAlign="Left" />
										<ItemStyle HorizontalAlign="Left" />
										<%--<HeaderStyle Width="5%" />--%>
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
								CssClass="btn btn-info btn-sm btn-block" EnableTheming="false" Font-Bold="True"
								Font-Size="Large" Style="display: none;" />
						</div>
						<div class="spacer">
						</div>
					</div>
					<div class="span5 well well-sm no-bottom-margin">
						<div class="DivStyleWithScroll" style="overflow: scroll; height: 400px;">
							<asp:GridView ID="grdSelectedCloth" runat="server" EmptyDataText="There are no pending cloth to be send to workshop"
								CssClass="mgrid" AutoGenerateColumns="False" ShowFooter="False" OnRowDataBound="grdSelectedCloth_RowDataBound"
								EnableTheming="false">
								<Columns>
									<asp:TemplateField>
										<HeaderTemplate>
											<asp:Label ID="Label3" runat="server" Text=""></asp:Label>
										</HeaderTemplate>
										<ItemTemplate>
											<asp:CheckBox ID="chkSelect" runat="server" />
										</ItemTemplate>
										<%--<ItemStyle Width="2%" />
									<HeaderStyle Width="2%" />--%>
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
										SortExpression="BookingDeliveyDate">
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:BoundField DataField="Barcode" HeaderText="Barcode" ReadOnly="True">
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:BoundField DataField="Customer" HeaderText="Customer" ReadOnly="True" SortExpression="Customer">
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:BoundField>
									<asp:TemplateField HeaderText="Cloth" SortExpression="ItemName">
										<ItemTemplate>
											<asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
											<asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
										</ItemTemplate>
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
										<FooterStyle HorizontalAlign="Right" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="U" ItemStyle-ForeColor="Red" ItemStyle-Width="20px">
										<ItemTemplate>
											<asp:Label ID="lblUrgent" runat="server" Text='<%# Bind("IsUrgent") %>'></asp:Label>
										</ItemTemplate>
										<ItemStyle ForeColor="Red" />
										<%--<HeaderStyle Width="2%" />--%>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Service">
										<ItemTemplate>
											<asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()
										%>'></asp:Label>
										</ItemTemplate>
										<%--<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="Legend" style="display: none;">
		<asp:Label ID="lblChallanNumber" runat="server" Text="12"></asp:Label>
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
	<div>
		<asp:SqlDataSource ID="SDTShifts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
			SelectCommand="SELECT [ShiftID], [ShiftName] FROM [ShiftMaster]"></asp:SqlDataSource>
		<asp:SqlDataSource ID="SqlSourceNewChallan" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
			ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT dbo.EntBookings.BookingNumber, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN '' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.BarcodeTable.StatusId = '1') AND (dbo.EntBookings.BookingStatus <> '5') ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN">
		</asp:SqlDataSource>
		<asp:Label ID="lblBranchCode" runat="server" Visible="False"></asp:Label>
		<asp:HiddenField ID="hdnCheckStatus" Value="0" runat="server" />
		<asp:HiddenField ID="hdnRowNo" runat="server" />
		<asp:HiddenField ID="HiddenField1" runat="server" />
		<asp:HiddenField runat="server" ID="hdnFirstTimeCheck" Value="0" />
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
		<asp:Button ID="Button6" runat="server" OnClick="btnTemp_Click" Style="display: none" />
		<asp:HiddenField ID="hdnInvoiceNo" runat="server" />
		<asp:HiddenField ID="hdnStoreId" runat="server" Value="1" />
		<asp:HiddenField ID="hdnSPBookingFrom" runat="server" />
		<asp:HiddenField ID="hdnSPBookingUpto" runat="server" />
		<asp:HiddenField ID="hdnSPShiftVal" runat="server" />
		<asp:HiddenField ID="hdnSelectedProcessType" runat="server" />
		<asp:HiddenField ID="hdnPrefixForCurrentYear" runat="server" />
		<asp:HiddenField ID="hdnAskForBarCode" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnReadyByPin" runat="server" ClientIDMode="Static" />
		<asp:SqlDataSource ID="SqlSourceChallanShifts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
			SelectCommand="SELECT DISTINCT ChallanSendingShift FROM EntChallan ORDER BY ChallanSendingShift">
		</asp:SqlDataSource>
		<asp:Button ID="btnTemp" runat="server" OnClick="btnTemp_Click" Style="display: none;" />
		<asp:Panel ID="pnlPanel" runat="server" CssClass="modalPopup" Style="display: none;margin-right:15px;"
			ClientIDMode="Static" BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png"
			Width="430px">
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
							<asp:DropDownList ID="drpPrintStartFrom" runat="server" ClientIDMode="Static">
							</asp:DropDownList>
							&nbsp;&nbsp;
							<asp:Button ID="btnSaveAndPrintNew" Text="Print" runat="server" CssClass="btn btn-info"/>
							<br />
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<asp:Panel ID="pnlBarCode" runat="server" CssClass="modalPopup" Style="display: none;margin-right:15px;"
			ClientIDMode="Static" BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png"
			Width="430px">
			<div class="popup_Titlebar" id="Div7">
				<div class="TitlebarLeft">
					PIN / BarCode
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
							<asp:Label ID="Label3" Text="Enter/Scan your PIN or BarCode " runat="server"></asp:Label>&nbsp;&nbsp;
							<asp:TextBox ID="txtPnlBarCodeText" runat="server" ClientIDMode="Static"></asp:TextBox><br />
							<asp:Label runat="server" ID="pnlBarCodeMsg" ClientIDMode="Static"></asp:Label>
							<br />
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
	</div>
</asp:Content>
