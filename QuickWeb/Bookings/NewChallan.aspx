<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
	EnableEventValidation="false" Inherits="NewChallan" Title="Untitled Page" CodeBehind="NewChallan.aspx.cs"
	ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script src="../JavaScript/javascript.js" type="text/javascript"></script>
	<script src="../JavaScript/code.js" type="text/javascript"></script>
	<script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
	<script type="text/javascript" src="../js/jquery.blockUI.js"></script>
	<script type="text/javascript" src="../js/jBeep.min.js"></script>
	<script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
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
			chkPrintChallan.checked = false;
			chkPrintSticker.checked = false;
			var _RowsToMoveFromLeftToRight = new Array();
			var _RowsToMoveFromRightToLeft = new Array();
			if ($('#hdnAllHtml').val() != '' && $('#hdnAllHtml').val() != -1) {
				$('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html($('#hdnAllHtml').val());
			}
			else if ($('#hdnAllHtml').val() == '-1') {
				$('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:not(:first-child)');
			}
			setQtyInLabel();
			$('#hdnAllRowMoveNumFromLTR').val('');
			$('#hdnAllRowMoveNumFromRTL').val('');
			$('#hdnLTRPrevCount').val('');
			$('#hdnRTLPrevCount').val('');
		//	removeRedundantRowsFromGrid('ctl00_ContentPlaceHolder1_grdNewChallan', 'ctl00_ContentPlaceHolder1_grdSelectedCloth', 5, true);
			disableButtons();
			$('body').click(function (event) {
				// chkCounterLft = chkCounterRht = 0;
				if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_drpProcess') {
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
				if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtCustomerName') {
					return;
				}
				if ($(event.target).attr('id') == 'drpPrintStartFrom') {
					return;
				}
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
					$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan, :checkbox').attr('disabled', true).addClass('disabledClass');
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
					$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan, :checkbox').attr('disabled', false).removeClass('disabledClass');
				}
				e.stopImmediatePropagation();
				e.stopPropagation();
			});
			$('#btnSaveRemoveChallan').click(function (e) {
				e.preventDefault();
				$('input, submit, select').not('#txtRemoverChallan, #btnSaveRemoveChallan, :checkbox').attr('disabled', false).removeClass('disabledClass');
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
					// save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
					var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
					__doPostBack('txtRemoverChallan', _allHTMLToSave);
				}
				else {
					__doPostBack('txtRemoverChallan', null);
				}

			});

			var drpProcessChangeHanlder = function (e) {
				/* */
				var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				if (_allRowsCount > 1) {
					// save the html of this grid, and on load, show it, also set the empty row removed and header copied to true
					var _allHTMLToSave = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html();
					__doPostBack('ctl00$ContentPlaceHolder1$drpProcess', _allHTMLToSave);
				}
				else {
					__doPostBack('ctl00$ContentPlaceHolder1$drpProcess', null);
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
						  //  _curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
							_curRow.insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
							/* change color of current row */
						//    $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(1)').css('background-color', 'lime');
							/* Remove the checkbox */
							_curRow.find(':checkbox').attr('checked', false);
							var _trCur = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').children().children()[1];
							_itemName = _trCur.children[6].textContent.trim();
							_bkNum = _trCur.children[2].textContent.trim();
							$('#lblStatusCloth').text(_itemName + ' [Order No:' + _bkNum + ']' + ' ' + findWorkShopRemark(_bkNum));
							stateOfColor = true;
							setDivMouseOver('#00aa00', '#B0C4DE');
							/* stock recon like */
							changeChallanStatus(1, '*' + _myVal + '*');
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
					if (bkExists == false) {
						//alert('This booking number is not available.');
						$('#lblStatusCloth').text('ORDER NOT AVAILABLE');
						jBeep();
						//beepSound();
						stateOfColor = true;
						setDivMouseOver('#FF0000', '#B0C4DE');
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
						__doPostBack('ctl00$ContentPlaceHolder1$txtBarcode', _allHTMLToSave);
					}
					else {
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
				return false;
			});
			// the buttons
			$('#btnMoveLeft').click(function (event) {
				event.preventDefault();
				setLeftGridHeaders();
				window['RTL'].insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
				window['RTLRemove'].remove();
				setQtyInLabel();
				changeChallanStatus(0, null, 'ctl00_ContentPlaceHolder1_grdNewChallan');
				$('#txtBarcode').focus();
				return false;
			});
			// btn move right all
			$('#btnMoveRightAll').click(function (event) {
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
					setTimeout(function () {
						makeMoveLTR();
						window['LRTAll'].insertAfter('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(0)');
						window['LRTAllRemove'].remove();
						$('.DivStyleWithScroll').closest('table').unblock();
						setQtyInLabel();
						changeChallanStatus(1, null, 'ctl00_ContentPlaceHolder1_grdSelectedCloth');
					}, 25);

				}
				else {
					alert('No cloth available to move!');
				}
				setQtyInLabel();
				$('#txtBarcode').focus();
				return false;
			});

			// btnMoveLeftAll
			// the buttons
			$('#btnMoveLeftAll').click(function (event) {
				// find the checked ones and move them to right
				var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				var _i = '';
				var _k = 1;
				setLeftGridHeaders();
				if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() > 1) {
					$('.DivStyleWithScroll').closest('table').block({ fadeIn: 0, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
					setTimeout(function () {
						makeMoveRTL();
						window['RTLAll'].insertAfter('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr:eq(0)');
						window['RTLAllRemove'].remove();
						$('.DivStyleWithScroll').closest('table').unblock();
						setQtyInLabel();
						changeChallanStatus(0, null, 'ctl00_ContentPlaceHolder1_grdNewChallan');
					}, 25);
				}
				else {
					alert('No cloth available to move!');
				}
				setQtyInLabel();
				$('#txtBarcode').focus();
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
			$('a, #ctl00_btnF1, #ctl00_btnF4, #btnDelivery').click(function (e) {
				var _allRowsCount = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				if (_allRowsCount > 1 && e.clientX != 0 && e.clientY != 0) {
					jBeep();
					return true; // confirm("You have selected few cloths but not submitted to send to workshop. If you leave this page these changes will go unsaved.\nClick Cancel to Go Back to Workshop Note,  Click OK to Leave Anyway");
				}
			});

			$('#chkPrintSticker, #chkPrintChallan').on('change', function (e) {
				if (chkPrintChallan.checked || chkPrintSticker.checked) {
					$('#btnPrint').attr('disabled', false).removeClass('disabledClass');
					$('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
				}
				else {
					$('#btnPrint').attr('disabled', true).addClass('disabledClass');
					$('#btnSaveChallan').attr('disabled', false).removeClass('disabledClass');
				}
				return false;
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
				if ($('#ctl00_ContentPlaceHolder1_grdNewChallan').find(':checkbox').length > 0 /* && $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').length <= 1 */) {
					$('#btnMoveRightAll').attr('disabled', false);
				}
				else {
					$('#btnMoveRightAll').attr('disabled', true);
				}
				if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth').find(':checkbox').length > 0 /* && $('#ctl00_ContentPlaceHolder1_grdNewChallan > tbody > tr').length <= 1 */) {
					$('#btnMoveLeftAll').attr('disabled', false);
				}
				else {
					$('#btnMoveLeftAll').attr('disabled', true);
				}
			}

			function disableSaveButtons() {
				var _rightSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
				var _toggle = false;
				if ($('#chkRemove').is(':checked')) {
					return;
				}
				if (_rightSize <= 1) {
					$('#ctl00_ContentPlaceHolder1_btnStickKerPrint').attr('disabled', true).addClass('disabledClass');
					$('#btnPrint').attr('disabled', true).addClass('disabledClass');
					$('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
					$('#chkPrintSticker').attr('disabled', true).addClass('disabledClass');
					$('#chkPrintChallan').attr('disabled', true).addClass('disabledClass');
				}
				else {
					$('#ctl00_ContentPlaceHolder1_btnStickKerPrint').attr('disabled', false).addClass('disabledClass');
					// $('#btnPrint').attr('disabled', false).removeClass('disabledClass');
					$('#btnSaveChallan').attr('disabled', false).removeClass('disabledClass');
					$('#chkPrintSticker').attr('disabled', false).removeClass('disabledClass');
					$('#chkPrintChallan').attr('disabled', false).removeClass('disabledClass');
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
	</script>
	<script type="text/javascript">
	    $(document).ready(function (event) {

	        function BlankGrid() {
	            $('#lblMsg').text('Cloths Sent to Workshop Successfully');
	            //  $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:not(:first-child)').remove();
	            $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').html('<table class="mGrid" rules="all" id="ctl00_ContentPlaceHolder1_grdSelectedCloth" style="border-collapse:collapse;" cellspacing="0" border="1"><tbody><tr><td colspan="9">There are no pending cloth to be send to workshop</td></tr></tbody></table>');
	            $('#hdnRemovedEmptyMessage').val('false');
	            $('#hdnAddedHeader').val('false');
	            window['LTR'] = null;

	            var _rightSize1 = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
	            if (_rightSize1 <= 1) {
	                $('#ctl00_ContentPlaceHolder1_btnStickKerPrint').attr('disabled', true).addClass('disabledClass');
	                $('#btnPrint').attr('disabled', true).addClass('disabledClass');
	                $('#btnSaveChallan').attr('disabled', true).addClass('disabledClass');
	                $('#chkPrintSticker').attr('disabled', true).addClass('disabledClass');
	                $('#chkPrintChallan').attr('disabled', true).addClass('disabledClass');
	            }
	            else {
	                $('#ctl00_ContentPlaceHolder1_btnStickKerPrint').attr('disabled', false).addClass('disabledClass');
	                $('#btnSaveChallan').attr('disabled', false).removeClass('disabledClass');
	                $('#chkPrintSticker').attr('disabled', false).removeClass('disabledClass');
	                $('#chkPrintChallan').attr('disabled', false).removeClass('disabledClass');
	            }
	        }

	        $('#btnSaveChallan, #btnPrint ,#ctl00_ContentPlaceHolder1_btnStickKerPrint').click(function (e) {
	            //var sound = document.getElementById('sound1');
	            //sound.Play();
	            if ($('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size() <= 1) {
	                alert('No cloth selected to save!');
	                return false;
	            }
	            if ($('#ctl00_ContentPlaceHolder1_drpMulti').is(':visible') && $('#ctl00_ContentPlaceHolder1_drpMulti').val() == 'Select') {
	                alert('Please select a workshop.');
	                return false;
	            }
	            else {
	                // make the array
	                //	                makeTheArrayToStore();
	                window['clickedId'] = e.target.id;
	                if (e.target.id === 'btnPrint' && chkPrintSticker.checked) {
	                    // if printing stickers then ask where to print from
	                    $('#pnlPanel').dialog({ width: 400, modal: true });
	                    return false;
	                }	               
	                if (e.target.id === 'btnSaveChallan') {
	                    $.ajax({
	                        url: '../AutoComplete.asmx/SaveChallanData',
	                        data: "{EBID: '" + $('#ctl00_ContentPlaceHolder1_drpMulti').val() + "'}",
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
	                            }
	                        },
	                        error: function (response) {
	                            alert(response.toString())
	                        }
	                    });
	                }
	                if (e.target.id === 'btnPrint' && chkPrintChallan.checked) {
	                    $.ajax({
	                        url: '../AutoComplete.asmx/SaveChallanAndWorkShopNote',
	                        data: "{EBID: '" + $('#ctl00_ContentPlaceHolder1_drpMulti').val() + "'}",
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
	                                chkPrintChallan.checked = false;
	                                window.open('../Reports/InvoicewithItemDetail.aspx?Date=' + aryData[1] + '&Date1=' + aryData[1] + '&Challan=' + aryData[0] + '');
	                            }
	                        },
	                        error: function (response) {
	                            alert(response.toString())
	                        }
	                    });

	                    return false;
	                }
	                return false;
	            }
	        });

	        $('#ctl00_ContentPlaceHolder1_btnSaveAndPrintNew').click(function (e) {
	            //e.preventDefault();
	            $('#pnlPanel').dialog('close');
	            var t2 = $('#drpPrintStartFrom').val();
	            $('#hdnStartValue').val(t2);
	            //  __doPostBack('ctl00$ContentPlaceHolder1$btnSaveAndPrintNew', null);
	            $.ajax({
	                url: '../AutoComplete.asmx/SaveChallanDataAndPrintSticker',
	                data: "{EBID: '" + $('#ctl00_ContentPlaceHolder1_drpMulti').val() + "', Possition: '" + t2 + "'}",
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
	                        chkPrintSticker.checked = false;
	                        $('#drpPrintStartFrom').val('1');
	                        if (chkPrintChallan.checked) {
	                               GetChallnNo();
	                        }
	                        window.open('../Bookings/printlabels.aspx?DirectPrint=true&CloseWindow=true&RedirectBack=../Bookings/NewChallan.aspx');	                       
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
	                url: '../AutoComplete.asmx/GetChallnNoData',
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
	                    window.open('../Reports/InvoicewithItemDetail.aspx?Date=' + aryData[1] + '&Date1=' + aryData[1] + '&Challan=' + aryData[0] + '');
	                },
	                error: function (response) {
	                    alert(response.toString())
	                }
	            });
	            return result;
	        }
	        $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('position', 'relative');
	        $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').closest('div').closest('div').closest('div').closest('div').css('width', '-20px');
	        function makeTheArrayToStore() {
	            var work = new Worker('../js/newworker.js');
	            work.onmessage = function (e) {
	                console.log(e.data);
	                $('#hdnAllData').val(e.data);
	                console.log(window['clickedId']);
	                // __doPostBack(window['clickedId'], null);
	                if (window['clickedId'] === 'btnPrint' && chkPrintSticker.checked) {
	                    return false;
	                } else {

	                    __doPostBack(window['clickedId'], null);
	                }
	                return true;
	            }
	            work.postMessage(['NewChallanSave', $('#ctl00_ContentPlaceHolder1_grdSelectedCloth').html()]);
	            return false;
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
					<asp:Label ID="Label1" runat="server" Text="Send to Workshop" ForeColor="#FF9933"></asp:Label>
					<span class="" style="font-size: 12Px">Create Workshop Note for all the cloths you need
						to send to workshop for processing</span>
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
				<td class="TDCaption" style="display: none;">
					Delivery Note No.:
				</td>
				<td class="Legend" style="display: none;">
					<asp:Label ID="lblChallanNumber" runat="server" Text="12"></asp:Label>
					<asp:Label ID="lblCustomerCode" runat="server" Visible="false"></asp:Label>
				</td>
				<td class="TDCaption">
					Order/Barcode No:
				</td>
				<td>
					<asp:TextBox ID="txtBarcode" runat="server" OnTextChanged="txtBarcode_TextChanged" autocomplete="off"
						ClientIDMode="Static" MaxLength="20"></asp:TextBox>
					<cc1:FilteredTextBoxExtender ID="txtBarcode_FilteredTextBoxExtender" runat="server"
						Enabled="True" TargetControlID="txtBarcode" ValidChars="1234567890-">
					</cc1:FilteredTextBoxExtender>
				</td>
				<td style="width: 10Px">
				</td>
				<td class="TDCaption" style="display: none;">
					Shift:
				</td>
				<td>
				</td>
				<td class="TDCaption" style="width: 5%;">
					Service:
				</td>
				<td>
					<asp:DropDownList ID="drpProcess" runat="server" Width="100px" AppendDataBoundItems="true">
					</asp:DropDownList>
				</td>
				<td class="TDCaption">
					Customer:
				</td>
				<td>
					<asp:TextBox ID="txtCustomerName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
						Width="300px" Visible="true" OnTextChanged="txtCustomerName_TextChanged"></asp:TextBox>
					<cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtCustomerName"
						ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
						CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
						CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
					</cc1:AutoCompleteExtender>
				</td>
				<td class="TDCaption">
					Due Date:
				</td>
				<td style="width: 10Px" nowrap="nowrap">
					<asp:TextBox ID="txtHolidayDate" runat="server" MaxLength="11" onkeypress="return false;"
						ToolTip="Double Click To Clear The Date" onpaste="return false;"></asp:TextBox><asp:Button
							ID="btnClearDate" ClientIDMode="Static" runat="server" Text="X" />
					<cc1:CalendarExtender ID="txtHolidayDate_CalendarExtender" runat="server" Enabled="True"
						Format="dd-MMM-yyyy" TargetControlID="txtHolidayDate">
					</cc1:CalendarExtender>
				</td>
				<td>
					<asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />&nbsp;&nbsp;&nbsp;
					<asp:Button ID="bntPrintPrv" runat="server" Text="View all Workshop Notes" OnClick="btnPrintPrv_Click"
						ClientIDMode="Static" />
				</td>
				<td style="width: 2Px">
				</td>
				<td>
					&nbsp;
				</td>
				<td style="width: 10Px">
				</td>
				<td>
					&nbsp;
				</td>
			</tr>
		</table>
		<table>
			<tr valign="top">
				<td class="TDCaption" style="display: none;">
					&nbsp;
				</td>
				<td class="Legend" style="display: none;">
					&nbsp;
				</td>
				<td class="TDCaption">
					<asp:Label ID="lblSent" runat="server" Text="Workshop :"></asp:Label>
				</td>
				<td>
					<asp:DropDownList ID="drpMulti" runat="server" AppendDataBoundItems="true" Width="100px">
					</asp:DropDownList>
				</td>
				<td style="width: 10Px">
					&nbsp;
				</td>
				<td class="TDCaption" style="display: none;">
					&nbsp;
				</td>
				<td class="TDCaption">
				</td>
				<td class="TDCaption" style="width: 5%;">
					&nbsp;
				</td>
				<td class="TDCaption" colspan="2">
					<asp:CheckBox ID="chkPrintSticker" runat="server" Text=" Print Sticker" ClientIDMode="Static" />&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkPrintChallan" runat="server" Text=" Print Workshop Note" ClientIDMode="Static" />
				</td>
				<td style="width: 10Px" nowrap="nowrap">
					&nbsp;
				</td>
				<td colspan="5" class="challanMoveButtons" nowrap="nowrap">
					<asp:Button ID="btnPrint" runat="server" Text="Send to Workshop & Print" CssClass="disabledClass"
						 EnableTheming="false" ClientIDMode="Static" />
					<asp:Button ID="btnSaveChallan" runat="server" Text="Send to Workshop" CssClass="disabledClass"
						 EnableTheming="false" ClientIDMode="Static" />
				</td>
			</tr>
		</table>
		<table style="width: 100%;">
			<tr valign="top">
				<td class="style1">
					<div id="DivLeftContainer">
						<div id="DivLeftCounter">
							<asp:Label runat="server" ID="lblLeft" Text="" ClientIDMode="Static" CssClass="TDCaption TDCaption2"></asp:Label>
						</div>
					</div>
				</td>
				<td class="TDCaption" style="width: 20px;">
					<asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
					<asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" ClientIDMode="Static" />
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
							<asp:Label ID="lblQtyCount" runat="server" Text="" ClientIDMode="Static" CssClass="TDCaption TDCaption2"></asp:Label>
						</div>
					</div>
					&nbsp;
				</td>
			</tr>
		</table>
		<table style="width: 100%;">
			<tr>
				<td>
					<td>
						<asp:CheckBox ID="chkRemove" runat="server" Text=" Return Cloth" 
							ClientIDMode="Static" />
						<asp:Label ID="lblRemove" runat="server" Text="Cloth Return Cause  :" Visible="false"></asp:Label>
						<asp:TextBox ID="txtRemoverChallan" runat="server" Width="200px" MaxLength="250"
							ClientIDMode="Static" Style="display: none;"></asp:TextBox>
						<cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtRemoverChallan"
							ServicePath="~/AutoComplete.asmx" ServiceMethod="GetDetailRemoveReasonMaster"
							MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True"
							CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
							CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
						</cc1:AutoCompleteExtender>
						<asp:Button ID="btnSaveRemoveChallan" runat="server" OnClick="btnSaveRemoveChallan_Click"
							ClientIDMode="Static" Text="Return Unprocessed" Style="display: none;" />
					</td>
				</td>
			</tr>
		</table>
		<table style="width: 100%;">
			<%--<tr style="width: 100%;">
				<td class="H4" style="font-weight: bolder" align="center">
					Total Cloths To Be Sent for Processing
				</td>
				<td class="H4" style="font-weight: bolder" align="center" nowrap="nowrap">
					Selected Cloths To Send Within Current Lot
				</td>
			</tr>
			</table>
			<table>--%>
			<tr valign="top">
				<td class="TDCaption" style="text-align: left; width: 46%">
					<div class="DivStyleWithScroll" style="overflow: scroll; height: 350px;">
						<asp:GridView ID="grdNewChallan" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
							Width="40%" EmptyDataText="No Cloth available to display, Please select appropriate value from top."
							ShowFooter="False" OnDataBinding="grdNewChallan_DataBinding" OnRowDataBound="grdNewChallan_RowDataBound"
							OnDataBound="grdNewChallan_DataBound">
							<Columns>
								<asp:TemplateField>
									<HeaderTemplate>
										<asp:Label ID="Label1" runat="server" Text=""></asp:Label>
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
								<asp:BoundField DataField="BookingNumber" HeaderText="Order" ReadOnly="True" SortExpression="BookingNumber"
									ItemStyle-Width="2px" HeaderStyle-Width="2px" FooterStyle-Width="2px">
									<%-- <ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
								</asp:BoundField>
								<asp:BoundField DataField="BookingDeliveryDate" HeaderText="Due Date" ReadOnly="True"
									SortExpression="BookingDeliveyDate">
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:BoundField>
								<asp:BoundField DataField="Barcode" HeaderText="Barcode" ReadOnly="True">
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:BoundField>
								<asp:BoundField DataField="Customer" HeaderText="Customer" ReadOnly="True" SortExpression="Customer">
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:BoundField>
								<asp:TemplateField HeaderText="Cloth" SortExpression="Item" ItemStyle-Wrap="true">
									<ItemTemplate>
										<asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
										<asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
									<FooterStyle HorizontalAlign="Right" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="U" ItemStyle-ForeColor="Red" ItemStyle-Width="20px">
									<ItemTemplate>
										<asp:Label ID="lblUrgent" runat="server" Text='<%# Bind("IsUrgent") %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="2%" ForeColor="Red" />
									<HeaderStyle Width="2%" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Service">
									<ItemTemplate>
										<asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()
										%>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Qty" SortExpression="ItemTotalQuantity" Visible="false">
									<FooterTemplate>
										<asp:Label ID="lblItemscount" runat="server"></asp:Label><br />
									</FooterTemplate>
									<ItemTemplate>
										<asp:Label ID="lblItemQty" runat="server" Text='<%# Bind("ItemTotalQuantity") %>'></asp:Label>
									</ItemTemplate>
									<FooterStyle HorizontalAlign="Left" />
									<ItemStyle Width="5%" HorizontalAlign="Left" />
									<HeaderStyle Width="5%" />
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</div>
				</td>
				<td class="challanMoveButtons">
					<asp:Label ID="lblMoveLbl" runat="server" CssClass="TDCaption, TDCaption1 lblMoveBtnText">&nbsp;&nbsp;Move<br></br>&nbsp;Selected&nbsp;</asp:Label>
					<asp:Button ID="btnMoveRight" runat="server" Text="  >  " ClientIDMode="Static" CssClass="top20" />
					<asp:Button ID="btnMoveLeft" runat="server" Text="  <  " ClientIDMode="Static" CssClass="top20" />
					<asp:Label ID="lblMoveAllLbl" runat="server" CssClass="TDCaption TDCaption2 lblMoveBtnText">&nbsp;&nbsp;<br>Move All&nbsp;</asp:Label>
					<asp:Button ID="btnMoveRightAll" runat="server" Text=">>" ClientIDMode="Static" CssClass="top20" />
					<asp:Button ID="btnMoveLeftAll" runat="server" Text="<<" ClientIDMode="Static" CssClass="top20" />
					<asp:Button ID="btnDeleteAll" runat="server" Text="   X  " ClientIDMode="Static"
						Style="display: none;" />
				</td>
				<td class="TDCaption" style="text-align: left; width: 46%">
					<div class="DivStyleWithScroll" style="overflow: scroll; height: 350px;">
						<asp:GridView ID="grdSelectedCloth" runat="server" EmptyDataText="There are no pending cloth to be send to workshop" CssClass="mGrid" AutoGenerateColumns="False"
							ShowFooter="False" OnDataBinding="grdSelectedCloth_DataBinding" OnRowDataBound="grdSelectedCloth_RowDataBound">
							<Columns>
								<asp:TemplateField>
									<HeaderTemplate>
										<asp:Label ID="Label3" runat="server" Text=""></asp:Label>
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
								<asp:BoundField DataField="BookingNumber" HeaderText="Order" ReadOnly="True" SortExpression="BookingNumber"
									ItemStyle-Width="2px" HeaderStyle-Width="2px" FooterStyle-Width="2px">
									<%-- <ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />--%>
								</asp:BoundField>
								<asp:BoundField DataField="BookingDeliveryDate" HeaderText="Due Date" ReadOnly="True"
									SortExpression="BookingDeliveyDate">
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:BoundField>
								<asp:BoundField DataField="Barcode" HeaderText="Barcode" ReadOnly="True">
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:BoundField>
								<asp:BoundField DataField="Customer" HeaderText="Customer" ReadOnly="True" SortExpression="Customer">
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:BoundField>
								<asp:TemplateField HeaderText="Cloth" SortExpression="ItemName">
									<ItemTemplate>
										<asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
										<asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
									<FooterStyle HorizontalAlign="Right" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="U" ItemStyle-ForeColor="Red" ItemStyle-Width="20px">
									<ItemTemplate>
										<asp:Label ID="lblUrgent" runat="server" Text='<%# Bind("IsUrgent") %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="2%" ForeColor="Red" />
									<HeaderStyle Width="2%" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Service">
									<ItemTemplate>
										<asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()
										%>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:TemplateField>
								<%--                                <asp:TemplateField HeaderText="Ext. Process">
									<FooterTemplate>
									</FooterTemplate>
									<ItemTemplate>
										<asp:Label ID="lblExtraProcess" runat="server" Text='<%# Eval("ItemExtraProcessType1").ToString() == "None" ? "": Eval("ItemExtraProcessType1").ToString() %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Ext. Process 2">
									<FooterTemplate>
										Total Qty
									</FooterTemplate>
									<ItemTemplate>
										<asp:Label ID="lblExtraProcess1" runat="server" Text='<%# Eval("ItemExtraProcessType2").ToString() == "None" ? "": Eval("ItemExtraProcessType2").ToString() %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle Width="5%" />
									<HeaderStyle Width="5%" />
								</asp:TemplateField>--%>
								<%--                                <asp:TemplateField HeaderText="Qty" SortExpression="ItemTotalQuantity">
									<FooterTemplate>
										<asp:Label ID="lblItemscount" runat="server"></asp:Label><br />
									</FooterTemplate>
									<ItemTemplate>
										<asp:Label ID="lblItemQty" runat="server" Text='<%# Bind("ItemTotalQuantity") %>'></asp:Label>
									</ItemTemplate>
									<FooterStyle HorizontalAlign="Left" />
									<ItemStyle Width="5%" HorizontalAlign="Left" />
									<HeaderStyle Width="5%" />
								</asp:TemplateField>--%>
							</Columns>
						</asp:GridView>
					</div>
				</td>
			</tr>
			<tr valign="top" style="height: 0px;">
				<td>
					<asp:SqlDataSource ID="SqlSourceNewChallan" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
						ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT dbo.EntBookings.BookingNumber, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN '' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.BarcodeTable.StatusId = '1') AND (dbo.EntBookings.BookingStatus <> '5') ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN">
					</asp:SqlDataSource>
					<asp:Label ID="lblBranchCode" runat="server" Visible="False"></asp:Label>
				</td>
			</tr>
			<tr valign="top">
				<td>
					<asp:HiddenField ID="hdnCheckStatus" Value="0" runat="server" />
					<asp:HiddenField ID="hdnRowNo" runat="server" />
					<asp:HiddenField ID="hdnInvoiceNo" runat="server" />
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
					<asp:Button ID="btnStickKerPrint" runat="server" Text="Send to Workshop & Print Sticker"
						 ToolTip="Receive from Workshop, Mark Ready to be Delivered & Generate Packing Stickers"
						CssClass="disabledClass" Style="visibility: hidden" />
					<asp:Button ID="btnTemp" runat="server" OnClick="btnTemp_Click" Style="display: none" />
					<asp:DropDownList ID="drpShifts" runat="server" DataSourceID="SDTShifts" DataTextField="ShiftName"
						DataValueField="ShiftID" Width="100px" AutoPostBack="True" 
						Style="display: none;">
					</asp:DropDownList>
					<asp:SqlDataSource ID="SDTShifts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
						SelectCommand="SELECT [ShiftID], [ShiftName] FROM [ShiftMaster]"></asp:SqlDataSource>
				</td>
			</tr>
		</table>
	</fieldset>
	<asp:Panel ID="pnlPanel" runat="server" CssClass="modalPopup" Style="display: none"
		ClientIDMode="Static" BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png"
		Width="340px">
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
						<asp:Button ID="btnSaveAndPrintNew" Text="Print" runat="server" />
						<br />
					</td>
				</tr>
			</table>
		</div>
	</asp:Panel>
</asp:Content>