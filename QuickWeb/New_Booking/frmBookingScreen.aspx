<%@ Page Language="C#" AutoEventWireup="true" Inherits="QuickWeb.New_Booking.frmBookingScreen"
    CodeBehind="frmBookingScreen.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="Shortcut Icon" type="image/ico" href="../images/DRY.jpg" />

    <title>
        <%=AppTitle %></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <%--<script src="../JavaScript/code.js" type="text/javascript"></script>--%>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript" src="../js/tag-it.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.23.custom.css" />
    <link href="../css/jquery.tagit.css" rel="stylesheet" type="text/css" />
    <link href="../css/tagit.ui-zendesk.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function SetColor(colorName) {
            var count = document.getElementById('grdEntry_ctl01_txtColor').value;
            // alert(document.getElementById('grdEntry_ctl01_txtColor').value);
            if (count != '') {
                document.getElementById('grdEntry_ctl01_txtColor').value = document.getElementById('grdEntry_ctl01_txtColor').value + ', ' + colorName;
                document.getElementById('grdEntry_ctl01_txtColor').select();
            }
            else {
                document.getElementById('grdEntry_ctl01_txtColor').value = colorName;
                document.getElementById('grdEntry_ctl01_txtColor').select();
            }
        }

        function toggleDropDownList(source) {
            document.getElementById('drpsmstemplate').disabled = !source.checked;
        }

        // this fuction sets the printer
        // arg = the default printer name
        function SetPrintOption(PrnName) {
            if (typeof (jsPrintSetup) == 'undefined') {
                installjsPrintSetup();
            } else {
                var win4Print = window.open('', 'Win4Print');
                var msg = document.getElementById("divPrint").innerHTML;
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
                win4Print.document.write(msg);
                win4Print.document.close();
                win4Print.focus();
                jsPrintSetup.print();
            }
        }


        function installjsPrintSetup() {
            if (confirm("You don't have printer plugin.\nDo you want to install the Printer Plugin now?")) {
                var xpi = new Object();
                xpi['jsprintsetup'] = '/mirrors.ibiblio.org/mozdev.org/jsprintsetup/jsprintsetup-0.9.2.xpi';
                InstallTrigger.install(xpi);
            }
        }

    </script>

    

</head>
<body onkeydown="if(event.keyCode==112){document.getElementById('btnNewBooking').click();}else{if(event.keyCode==115){document.getElementById('btnEditBooking').click();}">
    <form id="form1" runat="server">

    
    <script type="text/javascript">

        $(document).ready(function () {
            // taking the grid caluclations in account
            $('#txtCurrentDue').attr('disabled', true);
            $('#txtSrTax').attr('disabled', true);
            $('#txtTotal').attr('disabled', true);
            $('#txtBalance').attr('disabled', true);

            $('body').keydown(function (event) {

                // open the delivery screen
                if (event.which == 117) {
                    window.location = "../Bookings/Delivery.aspx";
                    return false;
                }

                var _idToCheck = $(event.target).attr('id');

                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {

                    if (_idToCheck == 'txtRate' && event.which == 9) {
                        // change here
                        $('#grdEntry_ctl01_imgBtnGridEntry').focus();
                        //$('#imgBtnAddMoreProcess').focus();
                        // $('#mytags').find('input').focus();
                    }
                    //else if (_idToCheck == 'txtRate' && event.which == 13) {
                    //return false;
                    //}
                    // not now since the desc and color box is moved to left
                    //else if (_idToCheck == 'imgBtnAddMoreProcess' && event.which == 9) {
                    //  $('#mytags').find('input').focus();
                    //}
                    else if (_idToCheck == 'txtEdit' && event.which == 13) {
                        $('#btnEdit').click();
                    }
                    // if id is imgBtnAddMoreProcess focus on add
                    else if (_idToCheck == 'imgBtnAddMoreProcess' && event.which == 9) {
                        $('#grdEntry_ctl01_imgBtnGridEntry').focus();
                    }
                    // if tab is pressed on any of the key where we haven't introduced anything, than just return true
                    if (event.which == 9 && (_idToCheck == 'txtDate' || _idToCheck == 'txtDueDate' || _idToCheck == 'txtTime' || _idToCheck == 'rdrPercentage' || _idToCheck == 'rdrAmt' || _idToCheck == 'chkHD'
                            || _idToCheck == 'chkSendsms' || _idToCheck == 'txtRemarks' || _idToCheck == 'drpCheckedBy' || _idToCheck == 'btnSaveBooking' || _idToCheck == 'btnSavePrint' || _idToCheck == 'btnSavePrintBarCode' || _idToCheck == 'btnPrintBarCode')) {
                        return;
                    }
                    return false;
                }
                else if ((event.which == 9 && event.shiftKey) && _idToCheck == 'txtAdvance') {
                    // on advance set the focus to whatever is enabled
                    if ($('#txtDiscountAmt').is(':visible')) {
                        $('#txtDiscountAmt').focus();
                    }
                    else {
                        $('#txtDiscount').focus();
                    }
                    // clickThePreviousInput(event);
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'grdEntry_ctl01_imgBtnGridEntry') {
                    // alert('cant');
                    //$('#imgBtnAddMoreProcess').focus();
                    $('#txtRate').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'imgBtnAddMoreProcess') {
                    // alert('cant');
                    $('#txtRate').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtRate') {
                    // alert('cant');
                    $('#txtProcess').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtProcess') {
                    // alert('cant');
                    // if color enabled set focus to it, else to desc, else to name
                    if ($('#hdnColorEnabled').val() == 'True') {
                        $('#mytagsColor input').focus();
                        return false;
                    }
                    else if ($('#hdnDescEnabled').val() == 'True') {
                        $('#mytags input').focus();
                        return false;
                    }
                    else {
                        $('#txtName').focus();
                        return false;
                    }
                }
                // if its Ctrl+D swap btw discount and discount perc
                else if ((event.which == 68 || event.which == 100) && event.ctrlKey) {
                    // SHORT CUTS
                    // swap the perc

                    // check which one has focus
                    // if its the discount
                    if ($('#txtDiscount').is(':visible')) {
                        // check if it already has focus
                        if ($('#txtDiscount').is(':focus')) {
                            // this has focus, just swap them
                            $('#rdrAmt').click();
                        }
                        else {
                            // this doesn't has focus, set focus to it
                            $('#txtDiscount').focus();
                        }
                    }
                    // else its the discount amt
                    else {
                        // check if it already has focus
                        if ($('#txtDiscountAmt').is(':focus')) {
                            // this has focus, just swap them
                            $('#rdrPercentage').click();
                        }
                        else {
                            // this doesn't has focus, set focus to it
                            $('#txtDiscountAmt').focus();
                        }
                    }


                    //                    if ($('#rdrPercentage').is(':checked')) {
                    //                        $('#rdrAmt').click();
                    //                    }
                    //                    else {
                    //                        $('#rdrPercentage').click();
                    //                    }
                    //$('#rdrPercentage').change();
                    return false;
                }

                // Ctrl + H for Home Delivery
                else if ((event.which == 72 || event.which == 104) && event.ctrlKey) {
                    // check if home delivery is already checked
                    if ($('#chkHD').is(':checked')) {
                        // uncheck it
                        $('#chkHD').click();
                        $('#chkHD').focus();
                        // $('#txtQty').focus();
                    }
                    else {
                        $('#chkHD').click();
                        $('#chkHD').focus();
                    }
                    return false;
                }

                // esc for closing dialog box
                else if (event.which == 27) {
                    if ($('#pnlNewCustomer').dialog('isOpen') == true) {
                        $('#pnlNewCustomer').dialog('close');
                        // focus on customer name if not added
                        $('#txtCustomerName').focus();
                    }
                    else if ($('#pnlItem').dialog('isOpen') == true) {
                        $('#pnlItem').dialog('close');
                    }
                    else if ($('#plnAddExtraProcess').dialog('isOpen') == true) {
                        $('#plnAddExtraProcess').dialog('close');
                    }
                    return false;
                }
                //return false;

                // save booking
                else if (event.keyCode == 119) {
                    // store the last button clicked
                    $('#hdnButtonClicked').val('btnSaveBooking');
                    // store the last span clicked
                    $('#hdnSpanClicked').val('btnSaveBookingContainder');
                    if (btnSaveBookingClickHandler(event)) {
                        $('.btnSaveBookingContainder').off('click.AttachedEvent');
                        $('#btnSaveBooking').click();
                        return true;
                    }
                    else {
                        return false;
                    }
                }

                // save and print
                else if (event.keyCode == 120) {
                    // store the last button clicked
                    $('#hdnButtonClicked').val('btnSavePrint');
                    // store the last span clicked
                    $('#hdnSpanClicked').val('btnPrintBookingContainder');
                    if (btnSaveBookingClickHandler(event)) {
                        $('.btnPrintBookingContainder').off('click.AttachedEvent');
                        $('#btnSavePrint').click();
                        return true;
                    }
                    else {
                        return false;
                    }
                }

                // save and print tags
                else if (event.keyCode == 123) {
                    // store the last button clicked
                    $('#hdnButtonClicked').val('btnPrintBarCode');
                    // store the last span clicked
                    $('#hdnSpanClicked').val('btnSavePrintTagsBookingContainder');
                    if (btnSaveBookingClickHandler(event)) {
                        $('.btnSavePrintTagsBookingContainder').off('click.AttachedEvent');
                        $('#btnPrintBarCode').click();
                        return true;
                    }
                    else {
                        return false;
                    }
                }

                // save and print slip and print tags
                else if (event.keyCode == 121) {
                    // store the last button clicked
                    $('#hdnButtonClicked').val('btnSavePrintBarCode');
                    // store the last span clicked
                    $('#hdnSpanClicked').val('btnSavePrintTagsBookingContainder');
                    if (btnSaveBookingClickHandler(event)) {
                        $('.btnSavePrintPrintTagsBookingContainder').off('click.AttachedEvent');
                        $('#btnSavePrintBarCode').click();
                        return true;
                    }
                    else {
                        return false;
                    }
                }


            });

            // hide the + btn on clicking the date
            $('#txtDate').focus(function (event) {
                //$('#CalendarExtender1_popupDiv').on('show.AttachedEvent', function (event) { $('#CalendarExtender1_popupDiv').css('z-index', 200); });
                $('#imgBtnAddNewItem').css('z-index', -1);
            }).blur(function (event) {
                $('#imgBtnAddNewItem').css('z-index', 0);
            });

            // all the inputs
            var GAllInputs = $('input[type="text"]:visible').map(function (el, index) { return ($(this).attr('id')) });

            // Step. Pre.
            // This function focuses on previous input when shift + tab is pressed
            function clickThePreviousInput(event) {
                if (GAllInputs == '') {
                    GAllInputs = $('input[type="text"]:visible').map(function (el, index) { return ($(this).attr('id')) });
                }
                // find if idExists, that is if its not one the the tag-it class input
                var _idToTest = $(event.target).attr('id');
                // if id is null, it must be one of the tag-it input
                if (_idToTest != '') {
                    // find the index of current input in the text
                    var _idx = $.inArray($(event.target).attr('id'), GAllInputs);
                    // find the new index, if this index is 0
                    // if its 0, just return
                    if (_idx == 0) return;
                    // else substract 1
                    _idx = _idx - 1;
                    // find the id at this index
                    $('#' + GAllInputs[_idx]).focus();
                }
                else {
                    // find the closest ul and check its id
                    var _prvId = $(event.target).closest('ul').attr('id');
                    if (_prvId == 'mytags') {
                        $('#imgBtnAddMoreProcess').focus();
                    }
                    else if (_prvId == 'mytagsColor') {
                        $('#mytags').find('input').focus();
                    }
                }
            };

            // Step btnNewBooking virtual click handler 
            $('#btnNewBooking').click(function (e) {
                // if the x and y is 0, just return false
                // don't know why its happening, but its causing the page to reload when user presses enter at rate column when editing a row
                if ((e.originalEvent != null) && (e.originalEvent.clientX == 0 && e.originalEvent.clientY == 0)) {
                    return false;
                }
            });


            // on the click of delivery
            $('#btnDelivery').click(function (event) {
                window.location = "../Bookings/Delivery.aspx";
                return false;
            });

            // Step 0.z this will check if desc and color are to be bind to master or not
            function checkDescAndColorForBinding() {
                $.ajax({
                    url: '../AutoComplete.asmx/checkDescAndColorForBinding',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    async: false,
                    success: function (response) {
                        $('#hdnBindDesc').val($(response).find('string').text().split(':')[0].toLowerCase());
                        $('#hdnBindColor').val($(response).find('string').text().split(':')[1].toLowerCase());
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            };

            // Step 0.z.1 call the above function
            checkDescAndColorForBinding();

            // Step 0.z.2 check if confirmation for delivery date
            function checkDeliveryDateForConfirmation() {
                $.ajax({
                    url: '../AutoComplete.asmx/ConfirmDelivery',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#hdnConfirmDelivery').val($(response).find('boolean').text());
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            }

            checkDeliveryDateForConfirmation();

            // Step 1.
            // On page load, load:
            // A. Default Item
            // B. Default Process
            // C. Default Rate For Default Item And Process.
            // D. Also, this loads the default qty, weather 1 or blank
            $.ajax({
                url: '../AutoComplete.asmx/LoadDefaultItemProcessAndRate',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                cache: false,
                async: false,
                success: function (response) {
                    // Set default Item
                    $('#hdnDefaultItem').val($(response).find('string:eq(0)').text().toUpperCase());
                    // Set default Process
                    $('#hdnDefaultProcess').val($(response).find('string:eq(1)').text());
                    // Set default Rate
                    $('#hdnDefaultItemProcessRate').val($(response).find('string:eq(2)').text());
                    // Set default qty
                    $('#hdnDefaultQty').val($(response).find('string:eq(3)').text());
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });

            // Call that function to set the default value
            setTheDefaults();
            // set focus to customer name
            $('#txtCustomerName').focus();

            // D. If tax is calculated before or after discount, if method returns ture, then before else if false then after
            $.ajax({
                url: '../AutoComplete.asmx/TaxBeforeOrAfter',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                cache: false,
                success: function (response) {
                    $('#hdnTaxBefore').val($(response).find('boolean').text());
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });

            // D.1 this loads all 3 tax rates
            // D. If tax is calculated before or after discount, if method returns ture, then before else if false then after
            $.ajax({
                url: '../AutoComplete.asmx/LoadAllThreeTaxes',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                cache: false,
                success: function (response) {
                    $('#hdnThreeTaxRates').val($(response).find('string').text());
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });

            // D.2 this loads all 3 tax rates
            // D. If tax is calculated before or after discount, if method returns ture, then before else if false then after
            $.ajax({
                url: '../AutoComplete.asmx/IsTaxExclusive',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                cache: false,
                success: function (response) {
                    $('#hdnIsTaxExclusive').val($(response).find('string').text());
                    // if the value is false, that is tax is inclusive
                    // hide the tax textbox
                    // and calc its value as 0

                    // inclusive case
                    //                    if ($(response).find('string').text() == 'false') {
                    //                        // tax is inclusive
                    //                        // hide the text box;
                    //                        // rather hide the entrire row
                    //                        $('.calcGrid > tbody > tr:eq(2)').hide();
                    //                    }
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });


            // E. Set a urgent booking rate, default to 0, onCheckChanged set the value, so that on check change 
            // we don't have to call different method for calculation, just using 0% as default is same as
            // no extra charge, when checkchanged, then set the value and calcuation runs in same method fine
            $('#hdnUrgentRateApplied').val('0');

            // F. Load the priorities in case user adds customers, even if not called here directly, might be called at some other time
            function LoadPriorities() {
                //                $.ajax({
                //                    url: '../AutoComplete.asmx/TaxBeforeOrAfter',
                //                    type: 'POST',
                //                    timeout: 20000,
                //                    datatype: 'xml',
                //                    cache: false,
                //                    success: function (response) {
                //                        $('#hdnTaxAfter').val($(response).find('boolean').text());
                //                    },
                //                    error: function (response) {
                //                        alert('server returned ' + response);
                //                    }
                //                });
            };

            // G. make the list(s) taggalge
            // make remarks taggable
            makeLiTaggable('mytags', 'mySingleField', 'hdnValues', '', $('#hdnBindDesc').val(), true, true, -1);
            // make color taggable
            makeLiTaggable('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', '', $('#hdnBindColor').val(), true, true, -1);

            // H.this will tell weather to show the net amount in round or decimal
            $.ajax({
                url: '../AutoComplete.asmx/isNetAmountInDecimal',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                cache: false,
                success: function (response) {
                    $('#isNetAmountInDecimal').val($(response).find('boolean').text());
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });

            // H.1 this will load all the tax and discount
            function loadAllTax() {
                $.ajax({
                    url: '../AutoComplete.asmx/FindTaxActive',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#totalTaxWithActive').val($(response).find('string').text());
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            }

            // H.2 this will load all the tax and discount
            function loadAllDis() {
                $.ajax({
                    url: '../AutoComplete.asmx/FindDisActive',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#totalDisWithActive').val($(response).find('string').text());
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            }

            // H.3 call these function
            loadAllDis();
            loadAllTax();

            // H.4 this load all the default items
            // so that we don't have to check when user presses enter again
            function LoadAllItems() {
                $.ajax({
                    url: '../AutoComplete.asmx/LoadAllItems',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#hdnAllItems').val($(response).find('string').text());
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            }

            // H.4.1 this loads all items
            LoadAllItems();

            // this will laod all the data
            findPriorityCode();


            // H.5 this load all the default processes
            // so that we don't have to check when user presses enter again
            function LoadAllProcesses() {
                $.ajax({
                    url: '../AutoComplete.asmx/LoadAllProcesses',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#hdnAllPrcs').val($(response).find('string').text());
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            }


            // H.5.2 call the above function
            LoadAllProcesses();

            // I. this will set the values to zero in the lower grid
            setLowerGridZero();

            // Step 1.J this will check if this booking is in edit mode,
            // if yes, then we need to change id
            if ($('#isInEditMode').val() == 'true') {
                changeTheGridIds();
                changeBackGroundColor();
                // recompute all grid to find the discount and what not
                // first check which one is clicked
                if ($('#rdrPercentage').is(':checked')) {
                    $('#txtDiscount').show();
                    $('#txtDiscountAmt').hide();
                }
                else {
                    $('#txtDiscount').hide();
                    $('#txtDiscountAmt').show();
                }
                recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), false, true);
                // findAllQtyOfItems();
                showHideDescColor(1, $('#grdEntry > tbody > tr').size(), true, true);
            }
            else {
                // nothing just set the default discount type
                // only setting here, because we don't wanna interrupt the uesr with changing the discount when he is editing
                setDefaultDiscountType();
                $('#txtCustomerName').focus();
            }

            // Step 1.K
            // this function changes the id's of new rows that are added at the time of editing
            function changeTheGridIds() {
                // find grid size
                var _grdSize = $('#grdEntry > tbody > tr').size();
                var _i = 2;
                var _j = 1;
                var _idToSet;
                // after each ten loop, the id will change as
                //  $('#grdEntry').find('#grdEntry_ctl11_lblSNO
                var _iCounterForTen = 0;
                var _placeHolderForID = '2';
                for (_i = 2; _i <= _grdSize; _i++) {
                    // find the id to set
                    _idToSet = (_grdSize - _j);

                    // increment the itemcounter
                    // _iCounterForTen++;

                    // set the pageHolder
                    _placeHolderForID = pad(_i.toString(), 2);


                    // the id(s) would be of form grdEntry_ctl0[2+rownumber]_lblSNO
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblSNO').attr('id', 'SNo_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblQty').attr('id', 'Qty_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblItemName').attr('id', 'ItemName_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblProcess').attr('id', 'Prc_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblRemarks').attr('id', 'Remarks_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblColor').attr('id', 'Color_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_lblAmount').attr('id', 'Amount_' + _idToSet + '');
                    // change the id of edit and delete button
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_imgbtnEdit').attr('id', 'Edit_' + _idToSet + '');
                    $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_imgbtnDeleteItemDetails').attr('id', 'Delete_' + _idToSet + '');
                    // now update the coutner that is updating the required id(s)
                    _j++;
                }
            }

            // Step 1.V. call the function below
            checkIfDesAndColorEnabled();

            // Step 1.V.1 this will check if desc is enabled, and color is enabled
            function checkIfDesAndColorEnabled() {
                $.ajax({
                    url: '../AutoComplete.asmx/checkIfDesAndColorEnabled',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#hdnDescEnabled').val($(response).find('string').text().split(':')[0]);
                        $('#hdnColorEnabled').val($(response).find('string').text().split(':')[1]);
                        // this function will hide the color or desc box, depending on the value returned from server
                        showHideDescColor(-1, -1, false, true);
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            }

            // Step 1.V.2 this is the function that will show or hide the values depending on the values returned
            // argStart : start row index
            // argEnd : end row index
            // argRecomputeForAll : boolean indicating that weather the grid is in edit mode, 
            // if so, then do the hiding for all the grid
            function showHideDescColor(argStart, argEnd, argRecomputeForAll, argHideHeader) {
                var _bColorDis = false;
                var _bDescDis = false;
                if ($('#hdnDescEnabled').val() == 'False') {
                    // hide the 3rd col
                    $('#grdEntry > tbody > tr > th:eq(3)').hide();
                    _bDescDis = true;
                }
                // not required, already shown
                //else if ($('#hdnDescEnabled').val() == 'True') {
                //
                //}
                if ($('#hdnColorEnabled').val() == 'False') {
                    // hide the 3rd col
                    $('#grdEntry > tbody > tr > th:eq(4)').hide();
                    _bColorDis = true;
                }
                // if both are disabled, resize the grid
                if (_bDescDis && _bColorDis) {
                    resizeTheGridForDiabled();
                }
                // if both, the start and the end are not -1,
                // then hide for all row
                if (argStart != -1 && argEnd != -1) {
                    for (_iCounter = argStart; _iCounter < argEnd; _iCounter++) {
                        // find which to hide
                        if (_bDescDis) {
                            $('#grdEntry > tbody > tr:eq(' + _iCounter + ') > td:eq(3)').hide();
                        }
                        if (_bColorDis) {
                            $('#grdEntry > tbody > tr:eq(' + _iCounter + ') > td:eq(4)').hide();
                        }
                    }
                }
            }

            // Step 1.V.3. this fucntion returns the values that will be used to resize,
            // not hard coding it, cause vivek sir might ask to change
            function theGridResetValues() {
                var _valToReturn =
                                    {


                                }
            }

            // Step 1.V.4 this is the fucntion that resizes the grid
            function resizeTheGridForDiabled() {
                $('#grdEntry > tbody > tr:eq(0) > th:eq(0)').width(60);
                $('#grdEntry > tbody > tr:eq(0) > th:eq(1)').width(60);
                $('#txtQty').width(60);
                $('#grdEntry > tbody > tr:eq(0) > th:eq(2)').width(400);
                $('#txtName').width(400);
                $('#grdEntry > tbody > tr:eq(0) > th:eq(6)').width(200);
            }

            // Step 1.W this will change the background color
            function changeBackGroundColor() {
                $('#grdEntry > tbody > tr:not(:eq(0))').css('background-color', 'white');
            }

            // Step 1.W.1 this wil set the default discount type when not in editing mode
            function setDefaultDiscountType() {
                $.ajax({
                    url: '../Autocomplete.asmx/FindDefaultDiscountType',
                    type: 'Post',
                    cache: false,
                    datatype: 'xml',
                    timeout: 20000,
                    success: function (response) {
                        var _result = $(response).find("string").text();
                        // set the discount to 0, disamt to 0, and dislbl to 0
                        $('#txtDiscount').val('0');
                        $('#txtDiscountAmt').val('0');
                        $('#lblDisAmt').text('0');
                        if (_result == 'Percentage') {
                            $('#rdrPercentage').click();
                        }
                        else {
                            $('#rdrAmt').click();
                            $('#txtQty').focus();
                        }
                        $('#txtCustomerName').focus();
                    },
                    error: function (response) {
                        // alert('do nothing');
                    }
                });
            };

            // Step 1.X this padds the reuired 
            function pad(str, max) {
                return str.length < max ? pad("0" + str, max) : str;
            }


            // Step 1.Y this will select the item on click
            $('*').focus(function (event) {
                $(this).select();
            });

            $('input').mousedown(function (event) {
                $(this).select();
                return false;
            });

            // Step 1.Z this opens up the modal pop up dialog when
            // CODE FOR "+" SYMBOL
            // user pressed + or + key
            $('body').keydown(function (event) {

                if (event.which == 107 || event.which == 61) {

                    // Customer add
                    // if user is pressing plus or shift plus, show the dialog
                    if ($(event.target).attr('id') == 'txtCustomerName') {

                        $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                        $('#txtCAddress').focus();
                        $('#pnlNewCustomer').dialog({ width: 520, height: 350, modal: true });
                        return false;
                    }

                    // Item Add
                    if ($(event.target).attr('id') == 'txtName') {

                        $('#txtItemName').val($('#txtName').val().toUpperCase());
                        //$('#txtItemCode').focus();
                        $('#pnlItem').dialog({ width: 500, modal: true });
                        return false;
                    }

                    // Extra Prc Add
                    if ($(event.target).attr('id') == 'txtRate') {

                        //                        $('#txtItemName').val($('#txtName').val().toUpperCase());
                        //                        $('#txtItemCode').focus();
                        $('#plnAddExtraProcess').dialog({ width: 500, modal: true });
                        return false;
                    }
                }
            });

            /// CODE UPDATE

            //            // Step 2.
            //            // On change of customer, loads its data, if customer doesn't exists show the add customer dialog
            //            var txtCustomerNamekeydown = function (event) {
            //                // event.preventDefault();
            //                // work only if tab or enter is pressed
            //                if (event.which == 13 || event.which == 9) {

            //                    // if hdnCust value is null
            //                    //                    if ($('#hdnCustChanged').val() == '') {
            //                    //                        // just focus on next
            //                    //                        $('#txtQty').focus();
            //                    //                        return false;
            //                    //                    }
            //                    //                    else {
            //                    // write to hdncust
            //                    // $('#hdnCustChanged').val('dummy');

            //                    var custArray = $('#txtCustomerName').val().split('-')[0];
            //                    var custCode = custArray.substring(0, custArray.length - 1).trim();
            //                    if ($('#txtCustomerName').val() == '') { return; }
            //                    if ($('#txtCustomerName').val().indexOf('-') < 0) {
            //                        // This is a new customer, show the add handler and do the deed
            //                        // Also remember to validate and add event handler for this save button
            //                        // Also, on close handler for this dialog, set the focus to qty
            //                        $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
            //                        $('#txtCAddress').focus();
            //                        if ($('#hdnCustChanged').val() != '') {
            //                            $('#pnlNewCustomer').dialog({ width: 520, height: 350, modal: true });
            //                        }
            //                        else {
            //                            $('#txtQty').focus();
            //                        }
            //                        return false;
            //                    }
            //                    $.ajax({
            //                        url: '../AutoComplete.asmx/GetPriorityAndRemarks',
            //                        type: 'POST',
            //                        timeout: 20000,
            //                        datatype: 'xml',
            //                        cache: false,
            //                        data: 'arg=' + custCode,
            //                        success: function (response) {
            //                            var result = $(response).find("string").text();
            //                            // the values is in form of name, address, mobile, priority and remark, and discount
            //                            var resultAry = result.split(':');
            //                            //alert(resultAry);
            //                            $('#txtCustomerName').val(resultAry[0].trim());
            //                            $('#lblAddress').text(resultAry[1]);
            //                            $('#lblMobileNo').text(resultAry[2]);
            //                            $('#lblPriority').text(resultAry[3]);
            //                            $('#lblRemarks').text(resultAry[4]);
            //                            $('#txtDiscount').val(resultAry[5]);
            //                            $('#txtQty').focus();
            //                            $('#txtQty').select();
            //                            $('#hdnCustCode').val(custCode);
            //                            // remove the handler
            //                            // $('body').off('keydown', '#txtCustomerName', txtCustomerNameKeydown);
            //                            // $('body').off('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNamekeydown);
            //                            //  fire the focusout handler
            //                            $('#txtDiscount').focusout();

            //                            // set the cust
            //                            $('#hdnCustChanged').val('');

            //                            return false;
            //                            //                        if (event.originalEvent == null) {
            //                            //                            event.stopImmediatePropagation();
            //                            //                            return false;
            //                            //                        }
            //                        },
            //                        error: function (response) {
            //                            // alert('some error occured');
            //                        }
            //                    });
            //                }
            //                //                }
            //                //                else {
            //                //                    $('#hdnCustChanged').val('dummy');
            //                //                    return;
            //                //                }
            //            };


            //            // Step 2.A Attach the event handler on keydown of txtcustomer
            //            // because if always attached, and user click the customername 
            //            // and without doing anything, just returns, it will keep showing up that popu
            //            $('#txtCustomerName').change(function (event) {
            //                // attach the keyup, if keydown if not enter or tab
            //                // if (event.which != 9 && event.which != 13) {
            //                $('body').on('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNamekeydown);
            //                // }
            //            });

            //            // Step 2.A.1 this will add the keydownhandler on start
            //            $('body').on('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNamekeydown);

            /// CODE UPDATE


            /*    ADDED CODE    */

            // Step 2. on keydown at txtcustomername
            $('#txtCustomerName').keydown(function (event) {
                // if enter add one 
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtCustomerName').one('keyup.AttachedEvent', txtCustomerNameKeyupHandler);
                }
                //                else if (event.which == 9) {
                //                    // $('#txtCustomerName').focusout();
                //                    $('#txtQty').focus();
                //                    txtCustomerNameKeyupHandler();
                //                }
            });

            // Step 2.A.1 This is the event handler for customer name event handler
            var txtCustomerNameKeyupHandler = function (event) {

                var custArray = $('#txtCustomerName').val().split('-')[0];
                var custCode = custArray.substring(0, custArray.length - 1).trim();
                if ($('#txtCustomerName').val() == '') { $('#txtQty').focus(); return; }
                if ($('#txtCustomerName').val().indexOf('-') < 0) {
                    // check if this customer exists, if no show the add customer dialog
                    $.ajax({
                        url: '../AutoComplete.asmx/CheckIfCustomerExists',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'argCustName=' + $('#txtCustomerName').val().trim().toUpperCase(),
                        success: function (response) {
                            var _res = $(response).find('boolean').text();
                            if (_res == 'false') {
                                $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                                $('#pnlNewCustomer').dialog({ width: 520, height: 350, modal: true });
                            }
                            else {
                                $('#txtQty').focus();
                            }
                        },
                        error: function (response) { }
                    });


                    //                    // This is a new customer, show the add handler and do the deed
                    //                    // Also remember to validate and add event handler for this save button
                    //                    // Also, on close handler for this dialog, set the focus to qty
                    //                    $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                    //                    $('#txtCAddress').focus();
                    //                    if ($('#hdnCustChanged').val() != '') {
                    //                        $('#pnlNewCustomer').dialog({ width: 520, height: 350, modal: true });
                    //                    }
                    //                    else {
                    //                        $('#txtQty').focus();
                    //                    }
                    //                    return false;
                }
                else {
                    $.ajax({
                        url: '../AutoComplete.asmx/GetPriorityAndRemarks',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'arg=' + custCode,
                        success: function (response) {
                            var result = $(response).find("string").text();
                            // the values is in form of name, address, mobile, priority and remark, and discount
                            var resultAry = result.split(':');
                            //alert(resultAry);
                            $('#txtCustomerName').val(resultAry[0].trim());
                            $('#lblAddress').text(resultAry[1]);
                            $('#lblMobileNo').text(resultAry[2]);
                            $('#lblPriority').text(resultAry[3]);
                            $('#lblRemarks').text(resultAry[4]);
                            // if discount is not null or 0
                            if (resultAry[5] != '' && resultAry[5] != '0') {
                                $('#rdrPercentage').click();
                                $('#txtDiscount').val(resultAry[5]);
                            }
                            $('#txtQty').focus();
                            $('#txtQty').select();
                            $('#hdnCustCode').val(custCode);
                            // remove the handler
                            // $('body').off('keydown', '#txtCustomerName', txtCustomerNameKeydown);
                            // $('body').off('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNamekeydown);
                            //  fire the focusout handler
                            $('#txtDiscount').focusout();

                            // set the cust
                            // $('#hdnCustChanged').val('');

                            return false;
                            //                        if (event.originalEvent == null) {
                            //                            event.stopImmediatePropagation();
                            //                            return false;
                            //                        }
                        },
                        error: function (response) {
                            // alert('some error occured');
                        }
                    });
                }
            };

            /*    ADDED CODE    */



            // Step 2.A.2 if user clicked the litte "+" icon to add open the pop up of customer panel
            $('#imgBtnCustAdd').click(function (event) {
                $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                $('#txtCAddress').focus();
                $('#pnlNewCustomer').dialog({ width: 520, height: 350, modal: true });
                return false;
            });

            // Step 2.A.2.1 on the keydown, if the key is enter
            $('#imgBtnCustAdd').keydown(function (event) {
                if (event.which == 13) {
                    $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                    $('#txtCAddress').focus();
                    $('#pnlNewCustomer').dialog({ width: 520, height: 350, modal: true });
                    return false;
                }
            });

            // Step 2.A.3
            $('#imgBtnCustAdd, #imgBtnAddNewItem, #imgBtnAddMoreProcess').hover(function (event) {
                $(this).attr('ImageUrl', '../images/plusN.png');
                $(this).attr('src', '../images/plusN.png');
            },
            function (event) {
                $(this).attr('ImageUrl', '../images/icons.png');
                $(this).attr('src', '../images/icons.png');
            }).focusin(function (e) { $(this).attr('src', '../images/plusN.png'); }).focusout(function (e) {
                $(this).attr('src', '../images/icons.png');
            });

            // $('body').on('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNameKeydown);

            // Step 2.B.1 make a call to ajax function that loads the customer,
            // because this is the first call, so that we won't get any delays in next calls
            $.ajax({
                url: '../AutoComplete.asmx/GetPriorityAndRemarks',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                cache: false,
                data: 'arg=cust1',
                success: function (response) {
                    var _tmpResult = $(response).find("string").text();
                    // the values is in form of name, address, mobile, priority and remark, and discount
                    var _tmpResultAry = _tmpResult.split(':');
                },
                error: function (response) {
                    // alert('some error occured');
                }
            });


            // Step 2.B Attach the close event handler
            var pnlNewCustomerKeydownHandler = function (event) {
                //                // Set the name in the box
                //                $('#txtCustomerName').val($('#txtCName').val().trim().toUpperCase());
                //                $('#txtQty').val('1');
                //                $('#txtQty').focus();
                //                $('#txtQty').select();

                // if the key is f10 save this data
                if (event.which == 121) {
                    // call the save button
                    $('#btnOkay').click();
                    return false;
                }
            };

            $('#pnlNewCustomer').on('keydown.AttachedEvent', pnlNewCustomerKeydownHandler);

            // Step 2.C Attach the close event handler
            $('body').on('dialogclose.AttachedEvent', '#pnlNewCustomer', clearDataFromAddCustDialog);

            // Step 2.C.1 eventhandler for opening the dialog, this will set the focus to customer address
            $('body').on('dialogopen.AttachedEvent', '#pnlNewCustomer', function (event) {
                // if txtCustomerName is null, set focus to it, else to address
                if ($('#txtCName').val() == '') {
                    $('#txtCName').focus();
                }
                else {
                    // set the focus to customer address
                    $('#txtCAddress').focus();
                }
                $('#txtCName').click();
            });

            // Call the next function
            findPriorityCode();

            // Step 2.C.2 Find All Priority and Code
            function findPriorityCode() {
                $.ajax({
                    url: '../AutoComplete.asmx/FindAllPriority',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        var _result = $(response).find("string").text();
                        $('#hdnAllPriorities').val(_result);
                    },
                    error: function (response) {
                        // alert('some error occured');
                    }
                });
            }

            // Step 2.C.3 this function calls the code for priority
            function findThePriorityCode(argPriority) {
                // if priority is null,
                // just return the 0 values
                if (argPriority == '') {
                    return 0;
                }
                // new
                // if the discount is applicable
                var _priorityCode = '';
                var _allPriority = $('#hdnAllPriorities').val();
                var _priorityListStr = _allPriority.split('_')[0].split(':');
                var _priorityListAry = $.makeArray(_priorityListStr);
                if ($.inArray(argPriority, _priorityListAry) >= 0) {
                    var _prcAppliedIndex = $.inArray(argPriority, _priorityListAry);
                    // now if not applied return 0
                    // else return the caluculation
                    _priorityCode = _allPriority.split('_')[1].split(':')[_prcAppliedIndex];

                }
                else {
                    // it is a new priority add it to database
                    $.ajax({
                        url: '../AutoComplete.asmx/AddPriority',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        async: false,
                        data: 'arg=' + argPriority,
                        success: function (response) {
                            _priorityCode = $(response).find("string").text();
                            // Add this to priority list
                            var _allPr = $('#hdnAllPriorities').val().split('_')[0];
                            // new pr
                            _allPr = _allPr + ':' + argPriority;
                            var _allVal = $('#hdnAllPriorities').val().split('_')[1];
                            _allVal = _allVal + ':' + _priorityCode;
                            var _fullPriorityString = _allPr + '_' + _allVal;
                            $('#hdnAllPriorities').val(_fullPriorityString);
                            // LoadPriorities();
                            // $('#txtPriority').hide();
                        },
                        error: function (response) {
                            // alert('some error occured');
                        }
                    });
                }
                return _priorityCode;

            }

            // Step 2.D Attach event handler for saving new customer
            var btnOkayClickHandler = function (event) {
                // if either name or address is null, warn the user
                if (!checkIfCustomerIsValid(event)) {
                    event.preventDefault();
                    return false;
                }
                // make the args and set their values
                // var _priority = findPriorityCode();
                var _argList = 'args=' + $('#txtPriority').val();
                var _prcCode = findThePriorityCode($('#txtPriority').val());
                if (_prcCode == '') {
                    _prcCode = 0;
                }
                _argList = _argList + '&args=' + parseInt(_prcCode);
                _argList = _argList + '&args=' + $('#txtCAddress').val().toUpperCase();
                _argList = _argList + '&args=' + $('#txtAreaLocaton').val().toUpperCase();
                _argList = _argList + '&args=' + $('#txtMobile').val().toUpperCase();
                _argList = _argList + '&args=' + $('#txtCName').val().toUpperCase();
                _argList = _argList + '&args=' + $('#txtRemarks1').val().toUpperCase();
                _argList = _argList + '&args=' + $('#drpTitle').val();
                //                _argList = _argList + '&args=' + $('#txtBDate').val().toUpperCase();
                //                _argList = _argList + '&args=' + $('#txtADate').val().toUpperCase();

                // remove the focus out handler from txtCutomerName, so that when losing focus this time
                // doesn't causes a infinite loop
                // Step 2.E remove handler, attach at txtQty focus
                // $('body').off('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNamekeydown);

                // Step 2.F Add the customer

                $.ajax({
                    url: '../AutoComplete.asmx/AddCustomer',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    data: '' + _argList,
                    success: function (response) {
                        var result = $(response).find("string").text();
                        // the values is in form of name, address, mobile, priority and remark
                        if (result == 'Added!') {
                            //alert(resultAry);
                            $('#txtCustomerName').val($('#txtCName').val().trim().toUpperCase());
                            $('#lblAddress').text($('#txtCAddress').val().toUpperCase());
                            $('#lblMobileNo').text($('#txtMobile').val().toUpperCase());
                            // get the priority for this drop down value
                            // ERROR
                            $('#lblPriority').text($('#txtPriority').val().toUpperCase());
                            $('#lblRemarks').text($('#txtRemarks1').val().toUpperCase());
                            $('#pnlNewCustomer').dialog('close');
                            clearDataFromAddCustDialog();
                            $('#txtQty').focus();
                            //$('body').off('keydown', '#txtCustomerName', txtCustomerNamekeydown);
                            // $('body').off('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNameKeydown);
                        }
                        else if (result == 'Exists!') {
                            alert('A customer with given details already exists!');
                            clearDataFromAddCustDialog();
                            $('#txtCName').focus();
                        }
                    },
                    error: function (response) {
                        // alert('some error occured');
                    }
                });
            };

            // Step 2.F.1 add the event handler for saving new customer
            $('body').on('click.AttachedEvent', '#btnOkay', btnOkayClickHandler);

            // Step 2.F.2 add the event handler for saving new customer
            $('body').on('keydown.AttachedEvent', '#btnOkay', function (event) {
                if (event.which == 13) {
                    $('#btnOkay').click();
                    $('#pnlNewCustomer').dialog('close');
                    $('#txtQty').focus();
                }
            });

            //  Step 2.F.3 Attach event handler for adding priority, no need to add a separate handler as won't be binding and removing this
            $('body').delegate('#btnAddNewPriority', 'click', function () {
                // check if value is not blank
                if ($('#txtNewPriority').val() == '') {
                    $('#txtNewPriority').focus();
                    return;
                }
                // add the priority
                $.ajax({
                    url: '../AutoComplete.asmx/AddPriority',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    data: 'arg=' + $('#txtNewPriority').val(),
                    success: function (response) {
                        var result = $(response).find("string").text();
                        if (result == 'Record Saved') {
                            LoadPriorities();
                            $('#txtNewPriority').hide();
                        }
                    },
                    error: function (response) {
                        // alert('some error occured');
                    }
                });
            });


            // Step 2.H this fucntion will clear the previous data that was enterd in the add customer dialog box
            function clearDataFromAddCustDialog() {
                $('#drpPriority').val('');
                $('#txtCAddress').val('');
                $('#txtAreaLocaton').val('');
                $('#txtMobile').val('');
                $('#txtPriority').val('');
                $('#txtCName').val('');
                $('#txtRemarks1').val('');
                $('#drpTitle').val('');
                $('#txtCustomerName').focus();
            };

            // Step 2.H.1 this is handler that makes the addcusotmer dialog box work for enter key
            // on all the fields on add customer diaglog box, add the keyevent

            $('body').on('keydown.AttachedEvent', '#drpTitle, #txtCName, #txtCAddress, #txtMobile, #txtPriority, #txtAreaLocaton, #txtRemarks1', function (event) {
                // if enter key is pressed
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    //                    if ($(event.target).attr('id') == 'txtCAddress') {
                    //                        if (!checkIfCustomerIsValid(event)) {
                    //                            return false;
                    //                        }
                    //                    }
                    if ($(event.target).attr('id') != 'txtRemarks1' && $(event.target).attr('id') != 'drpTitle') {
                        // find the previous tr
                        var _nextInput = $(event.target).closest('tr').next('tr').find('td:eq(2) input');
                        // focus on next input
                        $(_nextInput).focus();
                    }
                    else if ($(event.target).attr('id') == 'drpTitle') {
                        $('#txtCName').focus();
                        return;
                    }
                    else {
                        // its the last one, the txtAdate, all the btnOkay
                        // check here for address and remark
                        if (event.which == 9) {
                            event.which = 13;
                        }
                        if (!checkIfCustomerIsValid(event)) {
                            //                            event.preventDefault();
                            //                            event.stopImmediatePropagation();      
                            return false;
                        }
                        else {
                            $('#btnOkay').click();
                        }
                    }
                }
                if ((event.which == 9) && event.shiftKey && $(event.target).attr('id') == 'drpTitle') {
                    return false;
                }
            });


            // Step 2.I. this function checks if there is valid entry in customer adding dialog box
            function checkIfCustomerIsValid(event) {
                if ($('#txtCName').val() == '') {
                    //                    event.preventDefault();
                    //                    event.stopImmediatePropagation();                    
                    //                    $('#txtCName').attr('disabled', true);
                    //                    $('#txtCAddress').attr('disabled', true);
                    //                    $('#txtMobile').attr('disabled', true);
                    //                    $('#txtPriority').attr('disabled', true);
                    //                    $('#txtAreaLocaton').attr('disabled', true);
                    //                    $('#txtRemarks1').attr('disabled', true);
                    $('#btnOkay').attr('disabled', true);
                    alert('Please enter a valid value for customer name!');
                    //                    $('#txtCName').attr('disabled', false);
                    //                    $('#txtCAddress').attr('disabled', false);
                    //                    $('#txtMobile').attr('disabled', false);
                    //                    $('#txtPriority').attr('disabled', false);
                    //                    $('#txtAreaLocaton').attr('disabled', false);
                    //                    $('#txtRemarks1').attr('disabled', false);
                    $('#btnOkay').attr('disabled', false);
                    $('#txtCName').focus();
                    return false;
                }
                if ($('#txtCAddress').val() == '') {
                    //                    $('#txtCName').attr('disabled', true);
                    //                    $('#txtCAddress').attr('disabled', true);
                    //                    $('#txtMobile').attr('disabled', true);
                    //                    $('#txtPriority').attr('disabled', true);
                    //                    $('#txtAreaLocaton').attr('disabled', true);
                    //                    $('#txtRemarks1').attr('disabled', true);
                    $('#btnOkay').attr('disabled', true);
                    alert('Please enter a valid value for customer address!');
                    //                    $('#txtCName').attr('disabled', false);
                    //                    $('#txtCAddress').attr('disabled', false);
                    //                    $('#txtMobile').attr('disabled', false);
                    //                    $('#txtPriority').attr('disabled', false);
                    //                    $('#txtAreaLocaton').attr('disabled', false);
                    //                    $('#txtRemarks1').attr('disabled', false);
                    $('#btnOkay').attr('disabled', false);
                    $('#txtCAddress').focus();
                    return false;
                }
                return true;
            }

            // Step 3. On selecting process, find discount applicable tax applicable
            // rather find the default rate associated with given process and item
            $('#txtProcess, #txtExtraProcess1, #txtExtraProcess2').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $(event.target).one('keyup.AttachedEvent', txtPrckeyupHandler);
                }
            });

            // Step 3.A
            // the keyup event handler
            var txtPrckeyupHandler = function (event) {
                var prc = '' + $(event.target).val() + '';
                if (prc == '') {
                    if ($(event.target).attr('id') == 'txtProcess') {
                        $('#txtRate').focus();
                        return false;
                    }
                    else if ($(event.target).attr('id') == 'txtExtraProcess1') {
                        $('#txtExtraRate1').focus();
                        return false;
                    }
                    else if ($(event.target).attr('id') == 'txtExtraProcess2') {
                        $('#txtExtraRate2').focus();
                        return false;
                    }
                }

                // if the discount is applicable
                var _allPrcLst = $('#hdnAllPrcs').val() + '';
                var _PrcListStr = _allPrcLst.split(':');
                var _prcListAry = $.makeArray(_PrcListStr);
                if ($.inArray($(event.target).val().toUpperCase(), _prcListAry) >= 0) {
                    // if (_allPrcLst.indexOf($('#txtProcess').val()) > -1) {
                    // find the rate of current item and current prc(s)
                    findItemAndProcessRate($('#txtName').val().trim().toUpperCase(), $(event.target).val(), $(event.target).closest('tr').find('.clsRate'));
                    $('#txtRate').focus();
                    return;
                }
                else {
                    $('#txtProcess').focus();
                    alert('The entered value: "' + $(event.target).val() + '" doesn\'t exists in the database! Please enter a valid value!');
                    $(event.target).focus();
                    return false;
                }


                // 3.A Find if process exists
                $.ajax({
                    url: '../AutoComplete.asmx/CheckIfProcessExists',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    data: 'processName=' + prc,
                    success: function (response) {
                        if ($(response).find('string').text() == "false") {
                            alert('The entered value: "' + $(event.target).val() + '" doesn\'t exists in the database! Please enter a valid value!');
                            $(event.target).val();
                            $(event.target).focus();
                            $(event.target).select();
                            return false;
                            // $('#plnAddExtraProcess').show();
                        }
                        else {
                            // 3.B the process exists, load the tax and discount
                            // 3.B but first check if spliting the data in hdnAllTax (or hdnAllDis) is equal to current row index
                            // if it is, then user just changed the process, so update at that particular location
                            // if not, then append to that list

                            // ACTUALLY, you know what? Don't load anything here,
                            // do everything on addbutton/Update button

                            // find the rate for given process                            
                            findItemAndProcessRate($('#txtName').val(), $(event.target).val(), $(event.target).closest('tr').find('.clsRate'));

                        }
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });
            };



            // Step 3.A This will open up the dialog box for adding extraprocess
            $('body').on('click.AttachedEvent', '#imgBtnAddMoreProcess', function (event) {
                $('#plnAddExtraProcess').dialog({ width: 500, modal: true });
                return false;
            });


            // Step 3.A.1 on the keydown if it is enter open the dialog
            $('body').on('keydown.AttachedEvent', '#imgBtnAddMoreProcess', function (event) {
                if (event.which == 13) {
                    $('#imgBtnAddMoreProcess').click();
                }
            });

            // Step 3.A.2 this is the close handler for the panel of adding more processes
            $('body').on('dialogclose.AttachedEvent', '#plnAddExtraProcess', function (event) {
                // focus on txtrate and select it
                $('#txtRate').focus();
            });

            // Step 3.A.3 on keydown on rate move next
            $('body').on('keydown.AttachedEvent', '#txtExtraRate1', function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtExtraProcess2').focus();
                    return false;
                }
            });

            // Step 3.B Find row calc
            // arg is the row S.No, not using this right now, because add or update, both are happening at the grdHeader
            // if arg == -1, then calc for grid header, else calc for the given row
            function findRowCalc(argSNo) {
                // find that particular row and calculate
                // now any of rate(s), txtRate, txtExtraRate1, and txtExtraRate2 can't be blank because of next handler,
                // also, at the load time, set their values to 0 
                // (except for txtRate, that defaults to rate of default item and default process!)
                // Apply the urgent rate here, if not selected, then just the default (or 0) would be applied,
                // if there is something, then that value would be applied

                var _returnAmtForGivenRow;
                if (argSNo == -1) {
                    _returnAmtForGivenRow = parseFloat($('#txtQty').val()) * parseFloat($('#txtRate').val())
                                            + parseFloat($('#txtQty').val()) * (parseFloat($('#hdnUrgentRateApplied').val()) * parseFloat($('#txtRate').val()) / 100)
                                            + parseFloat($('#txtExtraRate1').val()) +
                                            +(parseFloat($('#hdnUrgentRateApplied').val()) * parseFloat($('#txtExtraRate1').val()) / 100)
                                            + parseFloat($('#txtExtraRate2').val())
                                            + (parseFloat($('#hdnUrgentRateApplied').val()) * parseFloat($('#txtExtraRate2').val()) / 100);

                }
                else {
                    // find the id, that would be Prc_ + argSNo, in that split by , then by @ then remove any ( or ) 
                    var _prcRateAndQtyStr = $('#Prc_' + argSNo).text() + '';
                    // split by ','
                    var _allPrcsAndRate = _prcRateAndQtyStr.split(',');
                    var _firstPrcRate, _secPrcRate, _thirdPrcRate;
                    // the length would be one, even if there is no ',', so check if length >1
                    // first check if len == 1, then if len == 2, then if len == 3
                    if (_allPrcsAndRate.length == 1) {
                        _firstPrcRate = _allPrcsAndRate[0].split('@')[1].substring(0, (_allPrcsAndRate[0].split('@')[1].length) - 1);
                        _returnAmtForGivenRow = parseFloat(_firstPrcRate) * parseFloat($('#Qty_' + argSNo).text());
                    }
                    else if (_allPrcsAndRate.length == 2) {
                        _firstPrcRate = _allPrcsAndRate[0].split('@')[1].substring(0, (_allPrcsAndRate[0].split('@')[1].length) - 1);
                        _secPrcRate = _allPrcsAndRate[1].split('@')[1].substring(0, (_allPrcsAndRate[1].split('@')[1].length) - 1);
                        _returnAmtForGivenRow = parseFloat(_firstPrcRate) * parseFloat($('#Qty_' + argSNo).text()) + parseFloat(_secPrcRate);
                    }
                    else if (_allPrcsAndRate.length == 3) {
                        _firstPrcRate = _allPrcsAndRate[0].split('@')[1].substring(0, (_allPrcsAndRate[0].split('@')[1].length) - 1);
                        _secPrcRate = _allPrcsAndRate[1].split('@')[1].substring(0, (_allPrcsAndRate[1].split('@')[1].length) - 1);
                        _thirdPrcRate = _allPrcsAndRate[2].split('@')[1].substring(0, (_allPrcsAndRate[2].split('@')[1].length) - 1);
                        _returnAmtForGivenRow = parseFloat(_firstPrcRate) * parseFloat($('#Qty_' + argSNo).text()) + parseFloat(_secPrcRate) + parseFloat(_thirdPrcRate);
                    }
                }
                return _returnAmtForGivenRow;
            };

            // Step 3.C Find amount for a process
            // It finds the amount for a particular process taking into account the urgent booking
            // argPrc : The process rate
            // argQty : The qty
            // bApplyQty : The boolean indicating weather to apply qty or not
            function findAmountForProcess(argPrcRate, argQty, bApplyQty) {
                var _prcAmt, _prcAmtTmp1, _prcAmtTmp2;
                if (bApplyQty) {
                    _prcAmtTmp1 = argQty * argPrcRate;
                    _prcAmtTmp2 = argQty * (parseFloat($('#hdnUrgentRateApplied').val()) * argPrcRate) / 100;
                    _prcAmt = parseFloat(_prcAmtTmp1) + parseFloat(_prcAmtTmp2);
                }
                else {
                    _prcAmtTmp1 = argPrcRate;
                    _prcAmtTmp2 = (parseFloat($('#hdnUrgentRateApplied').val()) * argPrcRate) / 100;
                    _prcAmt = parseFloat(_prcAmtTmp1) + parseFloat(_prcAmtTmp2);
                }
                return _prcAmt;
            }

            // Step 3.D Find row discount
            // It caculates the discount of current row/process
            // argPrc : The process
            // argPrcRate : Rate of the Process
            // bApplyQty : Indicates weather to take qty into accout, that would be the case in for first process
            // argRowNumber : In case of add, row is -1, else the grid's row
            function findDiscountForProcess(argPrc, argPrcRate, bApplyQty, argRowNumber) {
                // if prc is null, just return 0
                if (argPrc == '') { return 0; };

                // new
                // if the discount is applicable
                var _allPrcLst = $('#totalDisWithActive').val();
                var _prcListStr = _allPrcLst.split('_')[0].split(':');
                var _prcListAry = $.makeArray(_prcListStr);
                if ($.inArray(argPrc, _prcListAry) >= 0) {
                    var _disAppliedIndex = $.inArray(argPrc, _prcListAry);
                    // now if not applied return 0
                    // else return the caluculation
                    var _disApplied = _allPrcLst.split('_')[1].split(':')[_disAppliedIndex];
                    if (_disApplied == 'FALSE') {
                        return parseInt('0');
                    }
                }

                // find the rate in discount percentage
                var _discountPerc = $('#txtDiscount').val();
                //alert($(document.activeElement).attr('id'));
                var _qty;
                if (argRowNumber == -1) {
                    _qty = parseFloat($('#txtQty').val());
                }
                else {
                    _qty = parseFloat($('#grdEntry').find('#Qty_' + argRowNumber).text());
                }
                var _amtToCalcDisOn;
                _amtToCalcDisOn = findAmountForProcess(argPrcRate, _qty, bApplyQty);
                var _theDis = parseFloat(_amtToCalcDisOn) * _discountPerc / 100;
                // alert('rate ' + argPrcRate + ' qty ' + _qty + ' amount ' + _amtToCalcDisOn + ' dis ' + _theDis + ' row' + argRowNumber + ' bool ' + bApplyQty);
                return _theDis;
            };

            // Step 3.E
            // Find the tax on current row/process
            // argPrc : the process to calulate tax on
            // argPrcAmount : the amount of process (qty * process or processrate * 1 depending on weather its first or other process
            // taking ito acount the urgentRate
            // argPrcDiscount : the discount amt NOT THE PERCENTAGE BUT THE AMOUNT

            // MODIFIED : To take into account 3 taxes
            // RETURNS : json object in format
            // tax:tax1:tax2:totaltax;
            function findTaxForProcess(argPrc, argPrcAmount, argPrcDiscount) {
                var _valToReturn;


                if (argPrc == '') {
                    _valToReturn =
                                    {
                                        'tax': 0,
                                        'tax1': 0,
                                        'tax2': 0,
                                        'totalTax': 0
                                    }
                    setTaxableAmt(0);
                    return _valToReturn;
                }

                // if tax is not applicable then return 0
                // new
                var _allPrcLst = $('#totalTaxWithActive').val();
                var _prcListStr = _allPrcLst.split('_')[0].split(':');
                var _prcListAry = $.makeArray(_prcListStr);
                if ($.inArray(argPrc, _prcListAry) >= 0) {
                    var _taxAppliedIndex = $.inArray(argPrc, _prcListAry);
                    // now if not applied return 0
                    // else return the caluculation
                    var _taxApplied = _allPrcLst.split('_')[1].split(':')[_taxAppliedIndex];
                    if (_taxApplied == 'FALSE') {
                        _valToReturn =
                                    {
                                        'tax': 0,
                                        'tax1': 0,
                                        'tax2': 0,
                                        'totalTax': 0
                                    }
                        setTaxableAmt(0);
                        return _valToReturn;
                    }
                }


                // delare more vars
                var _taxRate, _taxRate1, _taxRate2;

                // PREVIOUS WHEN THE TAX WAS DEFINED AT ONCE
                /* 
                _taxRate = $('#hdnThreeTaxRates').val().split(':')[0];
                _taxRate1 = $('#hdnThreeTaxRates').val().split(':')[1];
                _taxRate2 = $('#hdnThreeTaxRates').val().split(':')[2];
                */

                // NEW, Tax can be different processwise
                // the returned result is in the format of tax^tax1^tax3
                var _allRates = _allPrcLst.split('_')[2].split(':')[_taxAppliedIndex] + '';
                _taxRate = _allRates.split('^')[0];
                _taxRate1 = _allRates.split('^')[1];
                _taxRate2 = _allRates.split('^')[2];

                // declare taxes
                var _stMain, _stCess, _stLast, _stTotal;

                // if tax is exclusive
                if ($('#hdnIsTaxExclusive').val() == 'true') {

                    // if before
                    if ($('#hdnTaxBefore').val() == "true") {


                        // just calc here
                        _stMain = ((parseFloat(argPrcAmount) * parseFloat(_taxRate)) / 100);
                        _stCess = ((_stMain * _taxRate1) / 100);
                        _stLast = ((_stMain * _taxRate2) / 100);
                        _stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
                        _valToReturn =
                                    {
                                        'tax': _stMain,
                                        'tax1': _stCess,
                                        'tax2': _stLast,
                                        'totalTax': _stTotal
                                    };
                        setTaxableAmt(argPrcAmount);
                    }
                    else {

                        //var _disAmt = (argPrcDiscount * argPrcAmount) / 100;
                        var _disAmt = argPrcDiscount;
                        var _newAmtForPrc = parseFloat(argPrcAmount) - parseFloat(_disAmt);

                        _stMain = ((parseFloat(_newAmtForPrc) * parseFloat(_taxRate)) / 100);
                        _stCess = ((_stMain * _taxRate1) / 100);
                        _stLast = ((_stMain * _taxRate2) / 100);
                        _stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
                        _valToReturn =
                                    {
                                        'tax': _stMain,
                                        'tax1': _stCess,
                                        'tax2': _stLast,
                                        'totalTax': _stTotal
                                    };
                        setTaxableAmt(_newAmtForPrc);
                    }

                }
                // else tax is inclusive
                else {
                    if ($('#hdnTaxBefore').val() == "true") {
                        var _totalTaxPerc = parseFloat(_taxRate) + parseFloat((_taxRate) * (_taxRate1) / 100) + parseFloat((_taxRate) * (_taxRate2) / 100);
                        // now the formula is 
                        // argAmt = x + _totalTaxPerc * x /100
                        var _bookingAmt = 0;
                        // thus x : x = ( argAmtPrc * 100 ) / ( 100 + totalTaxPerc ) 
                        _bookingAmt = (argPrcAmount * 100) / (parseFloat(100) + parseFloat(_totalTaxPerc));

                        // now calc the tax
                        // just calc here
                        _stMain = ((parseFloat(_bookingAmt) * parseFloat(_taxRate)) / 100);
                        _stCess = ((_stMain * _taxRate1) / 100);
                        _stLast = ((_stMain * _taxRate2) / 100);
                        _stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
                        _valToReturn =
                                    {
                                        'tax': _stMain,
                                        'tax1': _stCess,
                                        'tax2': _stLast,
                                        'totalTax': _stTotal
                                    };
                        setTaxableAmt(_bookingAmt);

                    }
                    else {
                        // not applicable

                        //var _disAmt = (argPrcDiscount * argPrcAmount) / 100;
                        var _disAmt = argPrcDiscount;
                        var _newAmtIncludinTax = parseFloat(argPrcAmount) - parseFloat(_disAmt);

                        var _totalTaxPerc = parseFloat(_taxRate) + parseFloat((_taxRate) * (_taxRate1) / 100) + parseFloat((_taxRate) * (_taxRate2) / 100);
                        // now the formula is 
                        // argAmt = x + _totalTaxPerc * x /100
                        var _bookingAmt = 0;
                        // thus x : x = ( argAmtPrc * 100 ) / ( 100 + totalTaxPerc ) 
                        // same as above, just that the _newAmtIncludinTax would be used instead of argAmtPrc
                        _bookingAmt = (_newAmtIncludinTax * 100) / (parseFloat(100) + parseFloat(_totalTaxPerc));

                        _stMain = ((parseFloat(_bookingAmt) * parseFloat(_taxRate)) / 100);
                        _stCess = ((_stMain * _taxRate1) / 100);
                        _stLast = ((_stMain * _taxRate2) / 100);
                        _stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
                        _valToReturn =
                                    {
                                        'tax': _stMain,
                                        'tax1': _stCess,
                                        'tax2': _stLast,
                                        'totalTax': _stTotal
                                    };
                        setTaxableAmt(_bookingAmt);

                    }
                }

                return _valToReturn;
            };

            // Step 3.E.1 this function set the taxable amt
            // argVal : the value to set/add to previous value
            function setTaxableAmt(argVal) {
                var _prvTaxable = $('#hdnTaxableAmt').val();
                if (_prvTaxable == '') {
                    _prvTaxable = '0';
                }
                var _amt = parseFloat(_prvTaxable) + parseFloat(argVal);
                $('#hdnTaxableAmt').val(_amt.toFixed(2));
            }

            // Step 3.F
            // Make the array to be stored in form of [(prc@rate),(prc2@rate2)]
            // prc : First prc
            // rate : First rate
            // So on....
            function makeProcessAndRateString(prc, rate, prc1, rate1, prc2, rate2) {
                var _allPrcArray;
                // if the urgent box is not checked
                if ($('#hdnUrgentRateApplied').val() == '0') {
                    _allPrcArray = '(' + prc + '@' + rate + ')';
                    if (prc1 != '') {
                        _allPrcArray = _allPrcArray + ',(' + prc1 + '@' + rate1 + ')';
                    }
                    if (prc2 != '') {
                        _allPrcArray = _allPrcArray + ',(' + prc2 + '@' + rate2 + ')';
                    }
                }
                // urgent rate is applied, the rate shown will be in the form of 
                // prc@(rate+rate*urgent/100) + prc1@(rate1+rate1*urgent/100)
                else {
                    // the all rates will be rate + urgent * rate / 100, 
                    // so we need to find urgent * rate /100
                    // then we can add that to every rate
                    var _urgentRateAmt = (parseFloat(rate) * parseFloat($('#hdnUrgentRateApplied').val())) / 100;
                    var _urgentRateAmt1 = (parseFloat(rate1) * parseFloat($('#hdnUrgentRateApplied').val())) / 100;
                    var _urgentRateAmt2 = (parseFloat(rate2) * parseFloat($('#hdnUrgentRateApplied').val())) / 100;

                    var _rate, _rate1, _rate2;

                    _rate = parseFloat(rate) + parseFloat(_urgentRateAmt);
                    _allPrcArray = '(' + prc + '@' + _rate + ')';

                    if (prc1 != '') {
                        _rate1 = parseFloat(rate1) + parseFloat(_urgentRateAmt1)
                        _allPrcArray = _allPrcArray + ',(' + prc1 + '@' + _rate1 + ')';
                    }

                    if (prc2 != '') {
                        _rate2 = parseFloat(rate2) + parseFloat(_urgentRateAmt2)
                        _allPrcArray = _allPrcArray + ',(' + prc2 + '@' + _rate2 + ')';
                    }

                }
                return _allPrcArray;
            }

            // Step 3.G
            // this wil call findAmt, findDis, and findTax 3 times for each prc and rate
            // and will add the values in hidden field, or update them based on parameter passed
            // params are just default as can be seen from their args
            function ComputeRowDisTaxAmt(argPrc, argRate, argPrc1, argRate1, argPrc2, argRate2, argQty, argIsUpdating, argRowNumber, argIsRecomputing) {
                var _curPrcDis = 0;
                var _curPrcTax = 0;
                var _curPrcTaxAry = '';
                var _curPrcAmount = 0;
                var _curPrcDis1 = 0;
                var _curPrcTax1 = 0;
                var _curPrcTax1Ary = '';
                var _curPrcAmount1 = 0;
                var _curPrcDis2 = 0;
                var _curPrcTax2 = 0;
                var _curPrcTax2Ary = '';
                var _curPrcAmount2 = 0;
                var _curDisStr = '';
                var _curTaxStr = '';
                var _iCounter;
                var _totalDiscOfRow = 0;
                var _totalTaxOfRow = 0;

                var _theDisPercToSendInTax = $('#txtDiscount').val();
                if (_theDisPercToSendInTax == '') { _theDisPercToSendInTax == '0'; };

                // NEW : 
                // SEND THE DISCOUNT AMT TO findTaxForProcess and not the discount perc, because the discount amt takes into account weather
                // the discount is applicable on the process or not..

                // First time
                _curPrcDis = findDiscountForProcess(argPrc, argRate, true, argRowNumber);
                _curPrcAmount = findAmountForProcess(argRate, argQty, true);
                _curPrcTaxAry = findTaxForProcess(argPrc, _curPrcAmount, _curPrcDis);
                _curPrcTax = _curPrcTaxAry.totalTax;
                _curDisStr = _curPrcDis + '';
                _curTaxStr = _curPrcTaxAry.tax + ':' + _curPrcTaxAry.tax1 + ':' + _curPrcTaxAry.tax2 + '';
                // second time
                _curPrcDis1 = findDiscountForProcess(argPrc1, argRate1, false, argRowNumber);
                _curPrcAmount1 = findAmountForProcess(argRate1, argQty, false);
                _curPrcTax1Ary = findTaxForProcess(argPrc1, _curPrcAmount1, _curPrcDis1);
                _curPrcTax1 = _curPrcTax1Ary.totalTax;
                _curDisStr = _curDisStr + ':' + _curPrcDis1;
                _curTaxStr = _curTaxStr + '_' + _curPrcTax1Ary.tax + ':' + _curPrcTax1Ary.tax1 + ':' + _curPrcTax1Ary.tax2 + '';
                // third time
                _curPrcDis2 = findDiscountForProcess(argPrc2, argRate2, false, argRowNumber);
                _curPrcAmount2 = findAmountForProcess(argRate2, argQty, false);
                _curPrcTax2Ary = findTaxForProcess(argPrc2, _curPrcAmount2, _curPrcDis2);
                _curPrcTax2 = _curPrcTax2Ary.totalTax;
                _curDisStr = _curDisStr + ':' + _curPrcDis2;
                _curTaxStr = _curTaxStr + '_' + _curPrcTax2Ary.tax + ':' + _curPrcTax2Ary.tax1 + ':' + _curPrcTax2Ary.tax2 + '';
                // add discont of row
                _totalDiscOfRow = parseFloat(_curPrcDis) + parseFloat(_curPrcDis1) + parseFloat(_curPrcDis2);
                _totalTaxOfRow = parseFloat(_curPrcTax) + parseFloat(_curPrcTax1) + parseFloat(_curPrcTax2);
                // store these values in previos values of hidden fields

                if (argIsUpdating == false && argIsRecomputing == false) {
                    // the user is adding
                    // this will append the tax and discount amount,
                    // that will be saved in backend for each row
                    // if value is not null
                    if ($('#hdnAllTax').val() == '') {
                        $('#hdnAllTax').val(_curTaxStr);
                    }
                    else {
                        $('#hdnAllTax').val($('#hdnAllTax').val() + '`' + _curTaxStr);
                    }
                    // same for discount
                    if ($('#hdnAllDiscount').val() == '') {
                        $('#hdnAllDiscount').val(_curDisStr);
                    }
                    else {
                        $('#hdnAllDiscount').val($('#hdnAllDiscount').val() + '_' + _curDisStr);
                    }


                    // alert(_totalDiscOfRow + ' ` ' + _totalTaxOfRow);
                }
                else if (argIsRecomputing == true && argIsUpdating == false) {
                    // don't set here, set in a temp field
                    if ($('#hdnAllTax').val() == '') {
                        $('#hdnAllTax').val(_curTaxStr);
                    }
                    else {
                        $('#hdnAllTax').val($('#hdnAllTax').val() + '`' + _curTaxStr);
                    }
                    // same for discount
                    if ($('#hdnAllDiscount').val() == '') {
                        $('#hdnAllDiscount').val(_curDisStr);
                    }
                    else {
                        $('#hdnAllDiscount').val($('#hdnAllDiscount').val() + '_' + _curDisStr);
                    }


                }
                else { // same as (argIsRecomputing == false && argIsUpdating == true)
                    // the user is editing
                    // the diffrences would be, the discount and tax would be RETURNED
                    // instead of being performed calculations upon
                    // also, the row index would be found and hdnAllTax, 
                    // and hdnAllDiscount would be updated and not addended


                    // find the row index of current hdnAllTax and CurrentAllDis
                    // that is in the form of dis:dis1:dis2_rdis:rdis1:rdis2_r1dis:r1dis1:r1dis2_r2dis:r2dis1:r2:dis2... and so on
                    setAllTaxDisOnUpdating(argRowNumber, _curDisStr, _curTaxStr, '_', '`');

                    // store these values in a json object and then return that obj
                    var _valToBeReturned =
                    {
                        'dis': _totalDiscOfRow,
                        'tax': _totalTaxOfRow
                    }
                    // return this value
                    return _valToBeReturned;
                    // NB: WILL NOT REACH THE NEXT STATEMENT

                }

                // after add or update, we need to show in grid the current discount and tax
                // and that would be equal to previous discount + current dicount and previous tax + current tax
                calculateLowerGridDetails(_totalDiscOfRow, _totalTaxOfRow, true, true, argIsRecomputing);
            }

            // Step 3. H
            // calculate the lower grid details
            function calculateLowerGridDetails(argCurRowDiscount, argCurRowTax, argAddDis, argAddTax, argIsRecomputing) {
                //alert($(document.activeElement).attr('id'));
                var _prvDis, _prvTax;
                if (argIsRecomputing) {
                    _prvDis = $('#hdnDisAmtRecomp').val();
                    _prvTax = $('#hdnTaxAmtRecomp').val();
                }
                else {
                    _prvDis = $('#lblDisAmt').text();
                    _prvTax = $('#txtSrTax').val();
                }
                var _disVal, _taxValue;

                // Step 3.H.1
                // set the discount value
                if (_prvDis == '') { _prvDis = 0; };
                if (argAddDis) {
                    _disVal = parseFloat(_prvDis) + parseFloat(argCurRowDiscount);
                }
                else {
                    _disVal = parseFloat(_prvDis) - parseFloat(argCurRowDiscount);
                }

                // Step 3.H.1.1
                // if recomputing
                if (argIsRecomputing) {
                    // then don't set the discount here, but set in a different field,
                    // so that at the end of loop it can be accessed and updated as required

                    // also, only set if discountamt is NOT visible, if it is,
                    // then discount will not be touched
                    if ($('#txtDiscountAmt').is(':visible') == false) {
                        $('#hdnDisAmtRecomp').val(_disVal.toFixed(2));
                        $('#hdnDisAmtRecomp').text(_disVal.toFixed(2));
                    }
                }
                else {
                    // also, only set if discountamt is NOT visible, if it is,
                    // then discount will not be touched
                    if ($('#txtDiscountAmt').is(':visible') == false) {
                        $('#lblDisAmt').text(_disVal.toFixed(2));
                        $('#hdnDiscountValue').val(_disVal.toFixed(2));
                    }
                }

                // Step 3.H.2
                // set the tax value
                if (argAddTax) {
                    _taxValue = parseFloat(_prvTax) + parseFloat(argCurRowTax);
                }
                else {
                    _taxValue = parseFloat(_prvTax) - parseFloat(argCurRowTax);
                }
                // if recomputing
                if (argIsRecomputing) {
                    // then don't set the discount here, but set in a different field,
                    // so that at the end of loop it can be accessed and updated as required

                    //inclusive case
                    if ($('#hdnIsTaxExclusive').val() == 'false') {
                        $('#hdnTaxAmtRecomp').val('0');
                    }
                    else {
                        $('#hdnTaxAmtRecomp').val(_taxValue.toFixed(2));
                    }
                }
                else {
                    // if tax in inclusive, just set to 0

                    //inclusive case
                    if ($('#hdnIsTaxExclusive').val() == 'false') {
                        $('#txtSrTax').val('0');
                    }
                    else {
                        $('#txtSrTax').val(_taxValue.toFixed(2));
                        //$('#txtSrTax').text(_taxValue.toFixed(2));
                    }
                }

                var _TotalAmt, _NetAmt
                // calculate gross and net amount
                if (argIsRecomputing) {
                    _TotalAmt = parseFloat($('#txtCurrentDue').val()) - parseFloat($('#hdnDisAmtRecomp').val()) + parseFloat($('#hdnTaxAmtRecomp').val());
                    $('#hdnTotal').val(parseFloat(_TotalAmt).toFixed(2));
                    $('#hdnTotal').text(parseFloat(_TotalAmt).toFixed(2));
                }
                else {
                    _TotalAmt = parseFloat($('#txtCurrentDue').val()) - parseFloat($('#lblDisAmt').text()) + parseFloat($('#txtSrTax').val());
                    $('#txtTotal').val(parseFloat(_TotalAmt).toFixed(2));
                    //$('#txtTotal').text(parseFloat(_TotalAmt).toFixed(2));
                }

                _NetAmt = parseFloat(_TotalAmt) - parseFloat($('#txtAdvance').val());
                // $('#txtBalance').val(parseFloat(NetAmt).toFixed(2));
                setNetBal(_NetAmt, argIsRecomputing);
            }

            // Step 3.I this will set the current dis and tax to allTax and allDiscount
            // jaadui method
            function setAllTaxDisOnUpdating(argRow, argNewDis, argNewTax, argDelimeter, argDelimeterTax) {
                //alert($(document.activeElement).attr('id'));
                // set the discount
                var _prvAllDis = $('#hdnAllDiscount').val();
                var _prvAllDisAry = _prvAllDis.split('' + argDelimeter + '');
                _prvAllDisAry[argRow - 1] = argNewDis;
                var _newDis = _prvAllDisAry.join('' + argDelimeter + '');
                $('#hdnAllDiscount').val(_newDis);
                //alert($(document.activeElement).attr('id'));
                // set the tax
                var _prvAllTax = $('#hdnAllTax').val();
                var _prvAllTaxAry = _prvAllTax.split('' + argDelimeterTax + '');
                _prvAllTaxAry[argRow - 1] = argNewTax;
                var _newTax = _prvAllTaxAry.join('' + argDelimeterTax + '');
                $('#hdnAllTax').val(_newTax);

            };

            // Step 3.J this will set the current net amount, 
            // based on the settings weather the amount to show is in decimal or round
            // argAmount = the amount to set
            function setNetBal(argAmount, argIsRecomputing) {
                var _amtToSet;
                if (argAmount == '') {
                    _amtToSet = '0';
                }
                else {
                    _amtToSet = parseFloat(argAmount);
                }
                // check if hdn field is true
                if ($('#isNetAmountInDecimal').val() == 'true') {
                    // if recomputing 
                    if (argIsRecomputing) {
                        $('#hdnBalance').val(_amtToSet.toFixed(2));
                        $('#hdnBalance').text(_amtToSet.toFixed(2));
                    }
                    else {
                        $('#txtBalance').val(_amtToSet.toFixed(2));
                        //$('#txtBalance').text(_amtToSet.toFixed(2));
                    }
                }
                else {
                    if (argIsRecomputing) {
                        $('#hdnBalance').val(Math.round(_amtToSet));
                        $('#hdnBalance').text(Math.round(_amtToSet));
                    }
                    else {
                        $('#txtBalance').val(Math.round(_amtToSet));
                        //$('#txtBalance').text(Math.round(_amtToSet));
                    }
                }
            }

            // Step 4. On change of rate, extrarate1, and extrarate2, if they are null, set to 0
            // not using that on syntax on this and process change, becuz if following naming till now
            // they would be prefixed with all items they work on
            // and that would becomre really huge!
            //            var rateFocusOutHandler = function (event) {
            //                if ($(event.target).val() == '') {
            //                    $(event.target).val('0');
            //                    // though it could be checked that if the process associated with given rate box in 
            //                    // default process, and the item in default item,
            //                    // in that case, we could also set the value to the default value
            //                    // rather then 0, but that includes much overhead for now!
            //                }
            //                else {
            //                    // check if it contains more than one decimal,
            //                    // not checking for other values, because they are already filtered
            //                    var _rtVal = $(event.target).val() + '';
            //                    var _count = _rtVal.match(/\./g);
            //                    if (_count > 1) {
            //                        // the value is invalid
            //                        alert('Please correct the value entered in ' + $(event.target).attr('id').substring(4) + '');
            //                        $(event.target).focus();
            //                        $(event.target).select();
            //                    }
            //                };
            //            

            // Step 4.A this is the function that checks for invalid value in rate(s)
            // argEvent is the event that raised/caused the error
            // argRate is the rate
            function checkForInvalidRate(argEvent) {
                if ($(argEvent).val() == '') {
                    $(argEvent).val('0');
                    // though it could be checked that if the process associated with given rate box in 
                    // default process, and the item in default item,
                    // in that case, we could also set the value to the default value
                    // rather then 0, but that includes much overhead for now!
                }
                // check if it contains more than one decimal,
                // not checking for other values, because they are already filtered
                var _rtVal = $(argEvent).val() + '';
                var _count = '';
                _count = _rtVal.match(/\./g);
                if (_count == null) {
                    return true;
                }
                if (_count.length > 1) {
                    // the value is invalid
                    alert('Please correct the value entered in ' + $(argEvent).attr('id').substring(3) + '');
                    $(argEvent).focus();
                    $(argEvent).select();
                    return false;
                }
                else {
                    return true;
                }
            }

            // Step 4.B
            // this checks if the item added in valid or not
            // argElement : the element that contains the itemname,
            // argVerifyElement : the element that contains all the items
            function checkForInvalidItem(argElement, argVerifyElement) {
                var _itemNameForCheck = $(argElement).val()
                if ($(argVerifyElement).val().indexOf(_itemNameForCheck) == -1) {
                    alert('Item Name is invalid!');
                    $(argElement).focus();
                    return false;
                }
                return true;
            }

            // Step 4.C
            // this checks if the processes added are valid or not
            // argElement : the element that contains the processName,
            // argElement1 : the element that contains the extra processName 1,
            // argElement2 : the element that contains the extra processName 2,
            // argVerifyElement : the element that contains all the items
            function checkForInvalidProcess(argElement, argElement1, argElement2, argVerifyElement) {
                var _prcNameForCheck = $(argElement).val();
                if ($(argVerifyElement).val().indexOf(_prcNameForCheck) == -1) {
                    alert('Process Name is invalid!');
                    $(argElement).focus();
                    return false;
                }
                _prcNameForCheck = $(argElement1).val();
                if ($(argVerifyElement).val().indexOf(_prcNameForCheck) == -1) {
                    alert('Extra Process 1 Name is invalid!');
                    $(argElement).focus();
                    return false;
                }
                _prcNameForCheck = $(argElement2).val();
                if ($(argVerifyElement).val().indexOf(_prcNameForCheck) == -1) {
                    alert('Extra Process 2 Name is invalid!');
                    $(argElement).focus();
                    return false;
                }
                return true;
            }

            // Step 5. For gridEntry add event handler for add
            /***************************************************/
            /********    THE MAJOR METHOD 1  *******************/
            var imgBtnGridEntryClickHandler = function () {
                // write short notions for Id(s), (like c# using statement) because without that the code would get too much cluttered!


                var _SnoGrdID = '#grdEntry_ctl01_lblHSNo';
                var _QtyGrdID = '#txtQty';
                var _ItemNameGrdID = '#txtName';
                var _ProcessGrdID = '#txtProcess';
                var _RateGrdID = '#txtRate'
                var _Process1GrdID = '#txtExtraProcess1';
                var _Rate1GrdID = '#txtExtraRate1';
                var _Process2GrdID = '#txtExtraProcess2';
                var _Rate2GrdID = '#txtExtraRate2';
                var _DescGrdID = '#mySingleField';
                var _ColorGrdID = '#mySingleFieldColor';
                var _AmtGrdID = '#grdEntry_ctl01_lblHAmount';
                var _InsertionID = '#grdEntry > tbody > tr:eq(0)';

                // first check if qty, itemname, process and rate is not null
                // because they are required field

                if ($(_QtyGrdID).val() == '') {
                    alert('Qty can\'t be left blank!');
                    $(_QtyGrdID).focus();
                    return false;
                }

                if ($(_QtyGrdID).val() == '0') {
                    alert('Qty can\'t be zero!');
                    $(_QtyGrdID).focus();
                    return false;
                }

                if ($(_ItemNameGrdID).val() == '') {
                    alert('Item name can\'t be left blank!');
                    $(_ItemNameGrdID).focus();
                    return false;
                }


                if ($(_ProcessGrdID).val() == '') {
                    alert('Process can\'t be left blank!');
                    $(_ProcessGrdID).focus();
                    return false;
                }


                if ($(_RateGrdID).val() == '') {
                    alert('Rate can\'t be left blank!');
                    $(_RateGrdID).focus();
                    return false;
                }

                // cause the focusout event of txtRate etc so that we can check for invalid value


                if (!checkForInvalidRate('#txtRate')) {
                    return false;
                }

                if (!checkForInvalidRate('#txtExtraRate1')) {
                    return false;
                }

                if (!checkForInvalidRate('#txtExtraRate2')) {
                    return false;
                }

                if (!checkForInvalidItem('#txtName', '#hdnAllItems')) {
                    return false;
                }

                if (!checkForInvalidProcess('#txtProcess', '#txtExtraProcess1', '#txtExtraProcess2', '#hdnAllPrcs')) {
                    return false;
                }


                // disable the button
                $('#grdEntry_ctl01_imgBtnGridEntry').attr('disabled', true);

                // Step 5.A If the text is not "Update" the user is in add mode
                // if its not update, then its a new entry
                if ($('#grdEntry_ctl01_imgBtnGridEntry').text() != "Update") {
                    // check if process is not null
                    if ($(_ProcessGrdID).val() == '') {
                        alert('first process can\'t be null');
                        $(_ProcessGrdID).focus();
                        return false;
                    }

                    // validate process, if prc2 is not null and prc1 is null, then its an error!
                    if ($(_Process2GrdID).val() != '' && $(_Process1GrdID).val() == '') {
                        alert('Please enter extra process 1 before entering extra process 2');
                        $(_Process1GrdID).focus();
                        return false;
                    }

                    // set lower grid to 0
                    // setLowerGridToZero();

                    // disable the checktoady and checknextday checkbox
                    $('#chkToday, #chkNextDay').attr('disabled', true);


                    // Step 5.A.1 load the html for the grdRow from the page
                    $('<div class=".grdLoaded">').load("GridRow.htm", function () {
                        // insert the html
                        $(_InsertionID).after($(this).html());
                        // find the values in current text box
                        var _insSNo, _insQty, _insItemName, _insPrc, _insRate, _insPrc1, _insRate1, _insPrc2, _insRate2, _insDesc, _insColor, _insAmt;
                        var _insPrcRate;
                        _insSNo = $(_SnoGrdID).text();
                        _insQty = $(_QtyGrdID).val();
                        _insItemName = $(_ItemNameGrdID).val();
                        _insPrc = $(_ProcessGrdID).val();
                        _insRate = $(_RateGrdID).val();
                        _insPrc1 = $(_Process1GrdID).val();
                        _insRate1 = $(_Rate1GrdID).val();
                        _insPrc2 = $(_Process2GrdID).val();
                        _insRate2 = $(_Rate2GrdID).val();
                        _insDesc = $(_DescGrdID).val();
                        _insColor = $(_ColorGrdID).val();
                        _insAmt = $(_AmtGrdID).text();

                        // Step 5.A.1.1 this will check if the color and remark is hidden, and if so
                        // the color and desc box will be disabled or enabled accordingly
                        // this is calc as allRows - currentIndex
                        var _enabledRowIndex = $('#grdEntry > tbody > tr').size() - _insSNo;
                        // hide the desc box
                        if ($('#hdnDescEnabled').val() == 'False') {
                            $('#grdEntry > tbody > tr:eq(' + _enabledRowIndex + ') > td:eq(3)').hide();
                        }
                        if ($('#hdnColorEnabled').val() == 'False') {
                            $('#grdEntry > tbody > tr:eq(' + _enabledRowIndex + ') > td:eq(4)').hide();
                        }


                        // Step 5.A.2 check if any of the rate is blank, then set it to 0
                        if (_insRate == '') { _insRate = '0' };
                        if (_insRate1 == '') { _insRate1 = '0' };
                        if (_insRate2 == '') { _insRate2 = '0' };

                        // Step 5.A.3 make the process and rate string to store
                        _insPrcRate = makeProcessAndRateString(_insPrc, _insRate, _insPrc1, _insRate1, _insPrc2, _insRate2);
                        var _insCurAllAmount = findRowCalc(-1);

                        // set the text in Gross Amount
                        var _allAmtOfEntireGrid = parseFloat($('#txtCurrentDue').val()) + parseFloat(_insCurAllAmount);
                        $('#txtCurrentDue').val(_allAmtOfEntireGrid.toFixed(2));
                        //$('#txtCurrentDue').text(_allAmtOfEntireGrid.toFixed(2));

                        makeThePercentage();
                        /********** NOW THE HARD PART ************/
                        /****** THE DISCOUNT AND TAX PART ********/

                        // Step 5.A.4 for all three processes find discount
                        ComputeRowDisTaxAmt(_insPrc, _insRate, _insPrc1, _insRate1, _insPrc2, _insRate2, _insQty, false, -1, false);
                        // since this is being added 
                        // find the current row added
                        // Step 5.A.5 now set the values
                        $('#grdEntry_ctl00_lblSNO').text(_insSNo);
                        $('#grdEntry_ctl00_lblQty').text(_insQty);
                        $('#grdEntry_ctl00_lblItemName').text(_insItemName);
                        $('#grdEntry_ctl00_lblProcess').text(_insPrcRate);
                        $('#grdEntry_ctl00_lblRemarks').text(_insDesc);
                        $('#grdEntry_ctl00_lblColor').text(_insColor);
                        $('#grdEntry_ctl00_lblAmount').text(parseFloat(_insCurAllAmount).toFixed(2));

                        // this is the current serial number that will be for now!
                        var _insCurRowNumber = $('#hdnCurrentValue').val();

                        // Step 5.A.5.1 this will add any new remark to the database
                        if (_insDesc != '') {
                            addNewRemarksToDataBase(_insDesc);
                        }
                        // Step 5.A.5.2 this will add new color to database
                        if (_insColor != '') {
                            addNewColorsToDataBase(_insColor);
                        }

                        // Step 5.A.6 set the new id(s)
                        // the id(s) will be in fromat Label + current SNo
                        $('#grdEntry').find('#grdEntry_ctl00_lblSNO').attr('id', 'SNo_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_lblQty').attr('id', 'Qty_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_lblItemName').attr('id', 'ItemName_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_lblProcess').attr('id', 'Prc_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_lblRemarks').attr('id', 'Remarks_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_lblColor').attr('id', 'Color_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_lblAmount').attr('id', 'Amount_' + _insCurRowNumber + '');
                        // change the id of edit and delete button
                        $('#grdEntry').find('#grdEntry_ctl00_imgbtnEdit').attr('id', 'Edit_' + _insCurRowNumber + '');
                        $('#grdEntry').find('#grdEntry_ctl00_imgbtnDeleteItemDetails').attr('id', 'Delete_' + _insCurRowNumber + '');

                        // Step 5.A.7 increment the hidden field value, that will indicate the current row/serial number
                        // so that it can be used later!
                        var _newHdnValue = parseInt($('#hdnCurrentValue').val()) + 1;
                        $('#hdnCurrentValue').val(_newHdnValue);

                        // set the default data
                        setTheDefaults();

                        // update the amount label in the header to reflect current amount
                        $('#grdEntry_ctl01_lblHAmount').text(parseFloat(_allAmtOfEntireGrid).toFixed(2));

                        // set the qty
                        setQtyInLabel(_insItemName, _insQty, true);

                        // set the focus
                        $('#txtQty').focus();

                        // Step 5.A.8
                        // save the rates to database
                        saveNewRatesToDataBase(_insItemName, _insPrc, _insRate, _insPrc1, _insRate1, _insPrc2, _insRate2);

                    });

                    $('#grdEntry_ctl01_imgBtnGridEntry').attr('disabled', false);
                    return false;
                }
                else {
                    // the grid is being edited
                    // Step 5.B If the text is not "Update" the user is in add mode

                    // the SNo. entered is the row to be updated from the last
                    var _totalrowsCount = $('#grdEntry > tbody > tr').size();
                    var _currentRow = $('#grdEntry_ctl01_lblHSNo').text();

                    // Step 5.B.1 find the current values that are to be updated
                    var _upSNo, _upQty, _upItemName, _upPrc, _upRate, _upPrc1, _insRate1, _upPrc2, _upRate2, _upDesc, _upColor, _upAmt;
                    var _upPrcRate;
                    _upSNo = $(_SnoGrdID).text();
                    _upQty = $(_QtyGrdID).val();
                    _upItemName = $(_ItemNameGrdID).val();
                    _upPrc = $(_ProcessGrdID).val();
                    _upRate = $(_RateGrdID).val();
                    _upPrc1 = $(_Process1GrdID).val();
                    _upRate1 = $(_Rate1GrdID).val();
                    _upPrc2 = $(_Process2GrdID).val();
                    _upRate2 = $(_Rate2GrdID).val();
                    _upDesc = $(_DescGrdID).val();
                    _upColor = $(_ColorGrdID).val();
                    _upAmt = $(_AmtGrdID).text();

                    // Step 5.B.2 check if any of the rate is blank, then set it to 0
                    if (_upRate == '') { _upRate = '0' };
                    if (_upRate1 == '') { _upRate1 = '0' };
                    if (_upRate2 == '') { _upRate2 = '0' };



                    // Step 5.B.3 make the process and rate string to store
                    _upPrcRate = makeProcessAndRateString(_upPrc, _upRate, _upPrc1, _upRate1, _upPrc2, _upRate2);
                    _upCurAllAmt = findRowCalc(-1);

                    // update the gross amount
                    var _upAllAmtOfEntireGrid = parseFloat($('#txtCurrentDue').val()) + parseFloat(_upCurAllAmt);
                    $('#txtCurrentDue').val(_upAllAmtOfEntireGrid.toFixed(2));
                    //$('#txtCurrentDue').text(_upAllAmtOfEntireGrid.toFixed(2));



                    // Step 5.B.4 set the current values
                    $('#grdEntry').find('#SNo_' + _currentRow + '').text(_upSNo);
                    $('#grdEntry').find('#Qty_' + _currentRow + '').text(_upQty);
                    $('#grdEntry').find('#ItemName_' + _currentRow + '').text(_upItemName);
                    $('#grdEntry').find('#Prc_' + _currentRow + '').text(_upPrcRate);
                    $('#grdEntry').find('#Remarks_' + _currentRow + '').text(_upDesc);
                    $('#grdEntry').find('#Color_' + _currentRow + '').text(_upColor);
                    $('#grdEntry').find('#Amount_' + _currentRow + '').text(parseFloat(_upCurAllAmt).toFixed(2));

                    $('#grdEntry_ctl01_imgBtnGridEntry').text('Add');
                    $('#grdEntry_ctl01_imgBtnGridEntry').val('Add');
                    $('#grdEntry_ctl01_lblHAmount').text(_upAllAmtOfEntireGrid);

                    // REMOVE THE HIGHLIGHT
                    var _rowToRemoveHighLight = parseInt(_totalrowsCount) - parseInt(_currentRow);
                    $('#grdEntry > tbody > tr:eq(' + _rowToRemoveHighLight + ')').css('background-color', 'white');
                    // REMOVE THE HIGHLIGHT



                    // Step 5.B.4.1 this will add new remakrks to the database
                    if (_upDesc != '') {
                        addNewRemarksToDataBase(_upDesc);
                    }

                    // Step 5.B.4.2 this will add new remakrks to the database
                    if (_upColor != '') {
                        addNewColorsToDataBase(_upColor);
                    }



                    // Step 5.B.6 find the current tax and amount
                    // add it to the previous discount
                    var _upAllCurDisTax = ComputeRowDisTaxAmt(_upPrc, _upRate, _upPrc1, _upRate, _upPrc2, _upRate2, _upQty, true, _currentRow, false);
                    var _upCDis = _upAllCurDisTax.dis;
                    var _upCTax = _upAllCurDisTax.tax;



                    // calculate the lower grid details
                    //                    calculateLowerGridDetails(_upCDis, _upCTax, true, true);

                    //                    // Step 5.B.7 find all amount
                    //                    var _upCurAllAmt = findRowCalc(_currentRow);

                    //                    // Step 5.B.8 update the gross amount
                    //                    var _upAllAmtOfEntireGrid = parseFloat($('#txtCurrentDue').val()) + parseFloat(_upCurAllAmt);
                    //                    $('#txtCurrentDue').val(_upAllAmtOfEntireGrid.toFixed(2));

                    //                    // set the amount in grdEntry_ctl01_lblHAmount
                    //                    $('#grdEntry_ctl01_lblHAmount').text(_upAllAmtOfEntireGrid.toFixed(2));


                    // Step 5.B.9
                    // calculate the lower grid details
                    calculateLowerGridDetails(_upCDis, _upCTax, true, true, false);



                    // set the qty
                    setQtyInLabel(_upItemName, _upQty, true);



                    // Step 5.B.10
                    // before recomputing, make sure that the total amount reflect the current values
                    // for eg. what if user set discount to manaul, and after that edited the row?
                    // in that case, the percent will reflect the percent amount of pervious total not current
                    makeThePercentage();



                    // recompute the grid if discount was given
                    // won't be used in adding, becase row already considers all those that are alerady added
                    // but in case of edit, thi is not like this
                    recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), false, false);

                    // Step 5.B.11. set the defaults
                    setTheDefaults();

                    // Step 5.B.12
                    // save the rates to database
                    saveNewRatesToDataBase(_upItemName, _upPrc, _upRate, _upPrc1, _upRate1, _upPrc2, _upRate2);

                }

                $('#grdEntry_ctl01_imgBtnGridEntry').attr('disabled', false);
                return false;

            };

            // Step 5.C Attach the event handler for add/update button
            $('body').on('click.AttachedEvent', '#grdEntry_ctl01_imgBtnGridEntry', imgBtnGridEntryClickHandler);

            // Step 5.C.1 Attach the event handler for add/update button on keydown enter
            $('body').on('keydown.AttachedEvent', '#grdEntry_ctl01_imgBtnGridEntry',
                function (event) {
                    if (event.which == 13) {
                        //alert('bad');
                        $('#grdEntry_ctl01_imgBtnGridEntry').click();
                    }
                    else if (event.which == 9) {
                        return true;
                    }
                    return false;
                });

            // Step 5.C.2 this is the fucntion that saves the new rates to database
            // argItemName: the name of the item
            // argProcess : the first process
            // argRate : the first rate
            // argExtraProcess1 : the second process
            // argRate1 : second rate
            // argExtraProcess2 : third process
            // argRate2 : third rate
            function saveNewRatesToDataBase(argItemName, argProcess, argRate, argExtraProcess1, argRate1, argExtraProcess2, argRate2) {
                $.ajax({
                    url: '../AutoComplete.asmx/SetItemRateForProcess',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    data: 'ItemName=' + argItemName + '&ProcessName=' + argProcess + '&argExtraProcess=' + argExtraProcess1 + '&argExtraProcess2=' + argExtraProcess2
                            + '&rate=' + argRate + '&rate1=' + argRate1 + '&rate2=' + argRate2,
                    success: function (response) {
                        // don't do anything, just added and we are good
                        // rather update the field that is source of remarks
                        updateRemarksSource();
                    },
                    error: function (response) {
                        // alert('Error : server returned ' + response);
                    }
                });
            }


            // Step 5.D this fuction sets the default
            function setTheDefaults() {
                // Step 5.A.8 clear all fields in the header, and set the SNo to new current value, and focus on qty
                $('#grdEntry_ctl01_lblHSNo').text($('#hdnCurrentValue').val());
                $('#txtQty').val('' + $('#hdnDefaultQty').val() + '');
                $('#txtName').val('' + $('#hdnDefaultItem').val() + '');
                $('#txtProcess').val('' + $('#hdnDefaultProcess').val() + '');
                $('#txtRate').val('' + $('#hdnDefaultItemProcessRate').val() + '');
                $('#txtExtraProcess1').val('');
                $('#txtExtraProcess2').val('');
                $('#txtExtraRate1').val('0');
                $('#txtExtraRate2').val('0');
                $('#mySingleField').val('');
                $('#mySingleFieldColor').val('');
                $('#grdEntry_ctl01_lblHAmount').text($('#txtCurrentDue').val());
                $('#txtQty').focus();
                $('#txtQty').select();
                // remove the tag-it list
                $('li.tagit-choice').remove();
                // clear the tag-it fields
                $('#mysingleField').val('');
                $('#mysingleFieldColor').val('');
                // also set the event handler for discount focus out button
                //$('#txtCustomerName').focus();
            };

            // Step 5.E this function makes the percentage, if the discount is net
            // and user edited or deleted the row
            function makeThePercentage() {
                // first check if discount amount is visible,
                // if its not, the the rate is already in discount percentage
                // and will be calculated according to current amount
                if ($('#txtDiscountAmt').is(':visible')) {
                    $('#txtDiscountAmt').focusout();
                }
            }

            // Step 5.F set lower grid to zero
            function setLowerGridZero() {
                if ($('#txtCurrentDue').val() == '') {
                    $('#txtCurrentDue').val('0');
                }
                if ($('#txtSrTax').val() == '') {
                    $('#txtSrTax').val('0');
                }
                if ($('#txtTotal').val() == '') {
                    $('#txtTotal').val('0');
                }
                if ($('#txtAdvance').val() == '') {
                    $('#txtAdvance').val('0');
                }
                if ($('#txtBalance').val() == '') {
                    $('#txtBalance').val('0');
                }
            }

            // Step 5.G this will add new remakrs to the database
            function addNewRemarksToDataBase(argRemark) {
                if (argRemark != '') {
                    // if not blank
                    $.ajax({
                        url: '../AutoComplete.asmx/AddNewRemarks',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'argRemarks=' + argRemark,
                        success: function (response) {
                            // don't do anything, just added and we are good
                            // rather update the field that is source of remarks
                            updateRemarksSource();
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });
                }
            };

            // Step 5.G.1 this will add new colors to the database
            function addNewColorsToDataBase(argColor) {
                if (argColor != '') {
                    // if not blank
                    $.ajax({
                        url: '../AutoComplete.asmx/AddNewColors',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'argColors=' + argColor,
                        success: function (response) {
                            // don't do anything, just added and we are good
                            // rather update the field that is source of remarks
                            updateColorSource();
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });
                }
            };

            // Step 5.H this updates the remarks source
            function updateRemarksSource() {
                $.ajax({
                    url: '../AutoComplete.asmx/FindRemarksSource',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        $('#hdnValues').val($(response).find('string').text());
                        $('#hdnValues').text($(response).find('string').text());
                        makeLiTaggable('mytags', 'mySingleField', 'hdnValues', '', $('#hdnBindDesc').val(), true, true, -1);
                    },
                    error: function (response) {
                        // alert('Error : server returned ' + response);
                    }
                });
            }

            // Step 5.H.1 this updates the color source
            function updateColorSource() {
                $.ajax({
                    url: '../AutoComplete.asmx/FindColorsSource',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    cache: false,
                    success: function (response) {
                        //$('#hdnValues').val($(response).find('string').text());
                        $('#LblValuesColor').text($(response).find('string').text());
                        //makeLiTaggable('mytags', 'mySingleField', 'hdnValues', '', $('#hdnBindDesc').val(), true, true, -1);
                        var _qty = $('#txtQty').val();
                        if (_qty == '') {
                            _qty = 1;
                        }
                        makeLiTaggable('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', '', $('#hdnBindColor').val(), true, true, -1);
                    },
                    error: function (response) {
                        // alert('Error : server returned ' + response);
                    }
                });
            }

            // Step 6. Event Hanlder for edit button
            // using data='value' because the id of edit button will be changed
            // and each button will have a different id
            // so the only option is to use a class or a cutom attrib
            $('body').delegate('[Data="value"]', 'click', function () {

                var _currentRowBtnId = '' + $(this).attr("id") + '';
                var _idContainerAry = _currentRowBtnId.split('_');
                var _rowNumberContainingString = '' + _idContainerAry[1] + '';

                // if the text in add button is update, do nothing, because a previous row is already being updated
                if ($('#grdEntry_ctl01_imgBtnGridEntry').text() == 'Update') {

                    // if edit S.No is same as prev then don't 
                    if ($('body').find('#grdEntry > tbody').find('#SNo_' + _rowNumberContainingString + '').text() == $('#grdEntry_ctl01_lblHSNo').text()) {
                        imgBtnGridEntryClickHandler();
                    }
                    else {
                        imgBtnGridEntryClickHandler();
                        setQtyInLabel($('#txtName').val(), $('#txtQty').val(), true);
                    }

                }

                // now the value to look for is NoOfGridRows - CurrentId + 1 (eg. 4-1+1 = 4, 4-2+1=3, 4-3+1=2)
                // find and store all elements value which are to be updated
                var _edtSNo = $('body').find('#grdEntry > tbody').find('#SNo_' + _rowNumberContainingString + '').text();
                var _edtQty = $('body').find('#grdEntry > tbody').find('#Qty_' + _rowNumberContainingString + '').text();
                var _edtItemName = $('body').find('#grdEntry > tbody').find('#ItemName_' + _rowNumberContainingString + '').text();
                var _edtProcessArray = $('body').find('#grdEntry > tbody').find('#Prc_' + _rowNumberContainingString + '').text() + '';
                var _edtDescArray = $('body').find('#grdEntry > tbody').find('#Remarks_' + _rowNumberContainingString + '').text() + '';
                var _edtColorArray = $('body').find('#grdEntry > tbody').find('#Color_' + _rowNumberContainingString + '').text() + '';
                var _edtCurrentEditingAmt = $('body').find('#grdEntry > tbody').find('#Amount_' + _rowNumberContainingString + '').text() + '';

                var _edtPrc, _edtRate, _edtPrc1, _edtRate1, _edtPrc2, _edtRate2;
                var _allSplittedPrcRate = splitPrcRateFromArray(_edtProcessArray);
                // set the values, the value returned is in format of json
                // for more details see http://stackoverflow.com/a/5642352/710925
                _edtPrc = _allSplittedPrcRate.prc;
                _edtPrc1 = _allSplittedPrcRate.prc1;
                _edtPrc2 = _allSplittedPrcRate.prc2;
                _edtRate = _allSplittedPrcRate.rate;
                _edtRate1 = _allSplittedPrcRate.rate1;
                _edtRate2 = _allSplittedPrcRate.rate2;

                // set the three prcs and rates, and other values in the grd
                $('#txtProcess').val('' + _edtPrc + '');
                $('#txtRate').val('' + _edtRate + '');
                $('#txtExtraProcess1').val('' + _edtPrc1 + '');
                $('#txtExtraRate1').val('' + _edtRate1 + '');
                $('#txtExtraProcess2').val('' + _edtPrc2 + '');
                $('#txtExtraRate2').val('' + _edtRate2 + '');
                $('#grdEntry_ctl01_lblHSNo').text(_edtSNo);
                $('#txtQty').val(_edtQty);
                $('#txtName').val(_edtItemName);

                // HIGHLIGHT
                $('#grdEntry > tbody > tr:eq(0)').fadeOut(100).fadeIn(100);
                var _rowToHighLight = parseInt($('#grdEntry > tbody > tr').size());
                _rowToHighLight = _rowToHighLight - parseInt(_rowNumberContainingString);
                // $('#grdEntry > tbody > tr:eq(' + _rowToHighLight  + ')').effect("highlight", {}, 3000);
                $('#grdEntry > tbody > tr:eq(' + _rowToHighLight + ')').css('background-color', 'yellow');
                // HIGHLIGHT

                // make the remark array
                makeAndSetRemarks('mytags', 'mySingleField', 'hdnValues', _edtDescArray, $('#hdnBindDesc').val(), -1);
                // make the color array 
                makeAndSetRemarks('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', _edtColorArray, $('#hdnBindColor').val(), -1);


                // change the add button label to update
                $('#grdEntry').find('#grdEntry_ctl01_imgBtnGridEntry').text('Update');
                $('#grdEntry').find('#grdEntry_ctl01_imgBtnGridEntry').val('Update');

                // set the amount, previous value - current row value
                $('#txtCurrentDue').val(parseFloat($('#txtCurrentDue').val()) - parseFloat(_edtCurrentEditingAmt).toFixed(2));
                //$('#txtCurrentDue').text(parseFloat($('#txtCurrentDue').val()) - parseFloat(_edtCurrentEditingAmt).toFixed(2));
                // calcuate the discount and tax for this row, subtract it from the previous discount
                var _allCurDisTax = ComputeRowDisTaxAmt(_edtPrc, _edtRate, _edtPrc1, _edtRate1, _edtPrc2, _edtRate2, _edtQty, true, _rowNumberContainingString, false);
                var _cDis = _allCurDisTax.dis;
                var _cTax = _allCurDisTax.tax;

                // calculate the lower grid details
                calculateLowerGridDetails(_cDis, _cTax, false, false, false);

                // set the qtyinLabel
                setQtyInLabel(_edtItemName, _edtQty, false);

                // select the qty
                $('#txtQty').focus();

                //                // set the hidden field value to indicate next serial number
                //                var _newHdnValue = parseInt($('#hdnCurrentValue').val()) - 1;
                //                $('#hdnCurrentValue').val(_newHdnValue);

                return false;
            });

            // Step 6.B this function splits the array passed of rate and prc into individuals
            // the format of ary is : (prc@rate),(prc1@rate1), and so on
            function splitPrcRateFromArray(argPrcRateArray) {
                var firstPrc, firstRate;
                var SecondPrc = '';
                var SecondRate = '';
                var ThirdPrc = '';
                var ThirdRate = '';

                if (argPrcRateArray.indexOf(',') > 0) {
                    // there is more than 1 process
                    var PrcNameAndRateArray = argPrcRateArray.split(',');
                    // split 3 times
                    // first rate and prc
                    firstPrc = PrcNameAndRateArray[0].split("@")[0].substring(1) + '';
                    firstRate = PrcNameAndRateArray[0].split("@")[1] + '';
                    firstRate = firstRate.substring(0, firstRate.length - 1);

                    // check for the count 
                    if (PrcNameAndRateArray.length > 1) {
                        // second rate and prc
                        SecondPrc = PrcNameAndRateArray[1].split("@")[0].substring(1);
                        SecondRate = PrcNameAndRateArray[1].split("@")[1];
                        SecondRate = SecondRate.substring(0, SecondRate.length - 1);
                    }

                    // again check
                    if (PrcNameAndRateArray.length > 2) {
                        // third rate and prc
                        ThirdPrc = PrcNameAndRateArray[2].split("@")[0].substring(1);
                        ThirdRate = PrcNameAndRateArray[2].split("@")[1];
                        ThirdRate = ThirdRate.substring(0, ThirdRate.length - 1);
                    }

                    // check for null
                    if (firstPrc == '') { firstPrc = '' };
                    if (SecondPrc == '') { SecondPrc = '' };
                    if (ThirdPrc == '') { ThirdPrc = '' };
                    if (firstRate == '') { firstRate = '0' };
                    if (SecondRate == '') { SecondRate = '0' };
                    if (ThirdRate == '') { ThirdRate = '0' };
                }
                else {
                    // there is just one item
                    firstPrc = argPrcRateArray.split("@")[0].substring(1) + '';
                    firstRate = argPrcRateArray.split("@")[1] + '';
                    firstRate = firstRate.substring(0, firstRate.length - 1);
                    SecondPrc = '';
                    SecondRate = '0';
                    ThirdPrc = '';
                    ThirdRate = '0';
                }
                // json object returned
                var _returnValue =
                 { 'prc': firstPrc,
                     'prc1': SecondPrc,
                     'prc2': ThirdPrc,
                     'rate': firstRate,
                     'rate1': SecondRate,
                     'rate2': ThirdRate
                 };
                return _returnValue;
            }

            // Step 6.C this function makes the Remark/Desc value to be set to the 
            // new col of row being edited in the grid
            // the argument is the array
            function makeAndSetRemarks(argElementToBeMadeTaggalbe, argSingleField, argSourceAry, argCurrentSrc, argAllowCustom, argTagsQtyAllowed) {
                // first clear the value of perv ary
                // $('#' + argSingleField + '').val('');

                // define the var to be added
                var _htmlTagIt = '<li class="tagit-choice ui-widget-content ui-state-default ui-corner-all"><span class="tagit-label">Cut marks</span><a class="tagit-close"><span class="text-icon">×</span><span class="ui-icon ui-icon-close"></span></a></li>' + '';
                // old mode
                // var _htmlTagItNewTag = '<li class="tagit-new"><input type="text" class="ui-widget-content ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true"></li>' + '';
                var _ItemCountInTagitList = argCurrentSrc.split(',').length;
                // this repsents all the html that will be added to the list of the ul
                var _allHTML = '';
                var _iCounter;
                // for all items, make the html
                // do the loop, but not for the case when argRemarkAry == ''
                if (argCurrentSrc == '') {
                    // just add once
                    _allHTML = _htmlTagIt;
                }
                else {
                    //  add as much as items in list
                    for (_iCounter = 0; _iCounter < _ItemCountInTagitList; _iCounter++) {
                        _allHTML = _allHTML + _htmlTagIt;
                    }
                }
                // add the new class
                // old mode
                // _allHTML = _allHTML + _htmlTagItNewTag;

                //  set the ul's items
                $('#grdEntry').find('#' + argElementToBeMadeTaggalbe + ' > li.tagit-new').before(_allHTML);

                //  now for each item in the ary, replace the text
                for (_iCounter = 0; _iCounter < _ItemCountInTagitList; _iCounter++) {
                    //   select the element 
                    $('#grdEntry').find('#' + argElementToBeMadeTaggalbe + ' > li.tagit-choice:eq(' + _iCounter + ') > span').text(argCurrentSrc.split(',')[_iCounter]);
                }

                //   all replaced, if arg passed is null
                //  then remove all li.tagit-choice
                if (argCurrentSrc == '') {
                    // remove
                    $('#grdEntry').find('#' + argElementToBeMadeTaggalbe + ' > li.tagit-choice').remove();
                }

                //     now make the li taggable
                makeLiTaggable(argElementToBeMadeTaggalbe, argSingleField, argSourceAry, argCurrentSrc, argAllowCustom, true, false, argTagsQtyAllowed);
            }

            // Step 6.D  this function makes the given item taggable
            function makeLiTaggable(argElement, argSingleNodeField, argAvailableTagsField, argCurrentlyAvailableTags, argAllowCustom, argAllowSpaces, argbMakeTaggable, argTagsQtyAllowed) {
                // the array of available tags
                var _argTagsAry = $('#' + argAvailableTagsField + '').text().split(',');
                var _availableTags = $.makeArray(_argTagsAry);

                // if currently availables tag is not null
                if (argCurrentlyAvailableTags != '') {
                    // then set the values of current field to currently available tags
                    $('#' + argSingleNodeField + '').val(argCurrentlyAvailableTags);
                    //                    // also add the html for tag-it choice and what not
                    //                    var _htmlTagIt = '<li class="tagit-choice ui-widget-content ui-state-default ui-corner-all"><span class="tagit-label">Cut marks</span><a class="tagit-close"><span class="text-icon">×</span><span class="ui-icon ui-icon-close"></span></a></li>' + '';
                    //                    var _htmlTagItNewTag = '<li class="tagit-new"><input type="text" class="ui-widget-content ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true"></li>' + '';
                    //                    // check if one or more, if 1, then indexOf(',') == -1
                    //                    var _curTagsAry = argCurrentlyAvailableTags.split(',');
                    //                    // run the loop
                    //                    var _iCounter, _allHTML;
                    //                    for (_iCounter = 0; _iCounter < _curTagsAry.length; _iCounter++) {
                    //                        _allHTML = _allHTML + _htmlTagIt;
                    //                        //   select the element 

                    //                    }
                    //                    _allHTML = _allHTML + _htmlTagItNewTag;
                    //                    $('#grdEntry').find('#' + argElementToBeMadeTaggalbe + ' > li.tagit-choice:eq(' + _iCounter + ') > span').text(argCurrentSrc.split(',')[_iCounter]);
                }
                var _me = argAllowCustom + '';

                // just tags the list, if make taggable is true
                if (argbMakeTaggable == true) {
                    $('#' + argElement + '').tagit({
                        availableTags: _availableTags,
                        singleField: true,
                        singleFieldNode: $('#' + argSingleNodeField + ''),
                        allowCustom: argAllowCustom,
                        allowSpaces: argAllowSpaces,
                        tagsQtyAllowed: argTagsQtyAllowed
                    });
                }

            }


            // Step 6.E this function finds the qty and sets it in the label
            function setQtyInLabel(argItemName, argQty, argDoAddtion) {
                var _numSubItems, _totalItems, _prvItems;
                _prvItems = $('#lblCount').text();
                if (_prvItems == '') {
                    _prvItems = 0;
                }
                // find the no of subitems
                $.ajax({
                    url: '../AutoComplete.asmx/CountNoOfSubItems',
                    type: 'POST',
                    timeout: 20000,
                    datatype: 'xml',
                    data: 'argItemName=' + argItemName,
                    cache: false,
                    async: false,
                    success: function (response) {

                        // Set items qty
                        _numSubItems = $(response).find('int').text();
                        _totalItems = parseInt(_numSubItems) * parseInt(argQty);
                        if (argDoAddtion) {
                            _totalItems = parseInt(_totalItems) + parseInt(_prvItems);
                        }
                        else {
                            _totalItems = parseInt(_prvItems) - parseInt(_totalItems);
                        }
                        $('#lblCount').text(_totalItems);
                    },
                    error: function (response) {
                        // alert('server returned ' + response);
                    }
                });

            };

            // Step 7. Event handler for delete button
            // this is the handler for delete button
            $('body').delegate('[Data="deleteMe"]', 'click', function (event) {
                // just find the row, and delete it
                var _rowId = '' + $(event.target).attr('id') + '';
                // alert(rowId);
                _rowId = _rowId.split('_');
                //alert(rowId);
                _rowId = _rowId[1];

                var _totalRowCount = $('#grdEntry > tbody > tr').size();
                if (confirm("Are you sure you want to remove this record?") == false) { return false; };


                // Step 7.A set the hidden filed value that will represent the next id
                var _prvID = parseInt($('#hdnCurrentValue').val());
                $('#hdnCurrentValue').val(_prvID - 1);

                // Step 7.A.1 find the itemname and qty of current row,
                // so the update the qty label count
                var _delItemName = $('body').find('#grdEntry > tbody').find('#ItemName_' + _rowId + '').text() + '';
                var _delQty = $('body').find('#grdEntry > tbody').find('#Qty_' + _rowId + '').text() + '';

                // Step 7.B find the current row amount, and delete it
                // Step 7.B.1 first find the amount of dis and tax of current row
                // because that would be substarced from the lower grid
                var _delProcessArray = $('body').find('#grdEntry > tbody').find('#Prc_' + _rowId + '').text() + '';

                var _delPrc, _delRate, _delPrc1, _delRate1, _delPrc2, _delRate2;
                var _allSplittedPrcRate = splitPrcRateFromArray(_delProcessArray);
                // Step 7.B.2 set the values, the value returned is in format of json
                // for more details see http://stackoverflow.com/a/5642352/710925
                _delPrc = _allSplittedPrcRate.prc;
                _delPrc1 = _allSplittedPrcRate.prc1;
                _delPrc2 = _allSplittedPrcRate.prc2;
                _delRate = _allSplittedPrcRate.rate;
                _delRate1 = _allSplittedPrcRate.rate1;
                _delRate2 = _allSplittedPrcRate.rate2;
                var _delQty = $('body').find('#grdEntry > tbody').find('#Qty_' + _rowId + '').text();

                // Step 7.B.3 calcuate the discount and tax for this row, subtract it from the previous discount
                var _delAllCurDisTax = ComputeRowDisTaxAmt(_delPrc, _delRate, _delPrc1, _delRate1, _delPrc2, _delRate2, _delQty, true, _rowId, false);
                var _delCDis = _delAllCurDisTax.dis;
                var _delCTax = _delAllCurDisTax.tax;

                // Step 7.B.4 find all amount
                var _delCurAllAmt = findRowCalc(_rowId);

                // Step 7.B.5 update the gross amount
                var _delAllAmtOfEntireGrid = parseFloat($('#txtCurrentDue').val()) - parseFloat(_delCurAllAmt);
                $('#txtCurrentDue').val(_delAllAmtOfEntireGrid.toFixed(2));
                //$('#txtCurrentDue').text(_delAllAmtOfEntireGrid.toFixed(2));

                // set the amount in grdEntry_ctl01_lblHAmount
                $('#grdEntry_ctl01_lblHAmount').text(_delAllAmtOfEntireGrid.toFixed(2));

                // calculate the delete amount, in case its discount amount, and not percentage
                makeThePercentage();

                // Step 7.B.6 calculate the lower grid details
                calculateLowerGridDetails(_delCDis, _delCTax, false, false, false);

                // Step 7.C reove the row, the id would be
                // row size - _rowId +1
                var _rowToRemove = parseInt(_totalRowCount) - parseInt(_rowId);
                $('#grdEntry > tbody > tr:eq(' + _rowToRemove + ')').remove();

                // Step 7.B.6 reset all grid id(s)
                resetGridId('grdEntry', parseInt(_rowId) + 1, _totalRowCount);

                // calc the qty count
                setQtyInLabel(_delItemName, _delQty, false);

                // focus in txtqty
                $('#txtQty').focus();
                return false;
            });


            // Step 7.Y this function will reset the grid's id
            // when a particular row is delete
            // argGrdId = the id of grd, not really necessary, not used before, 
            // but taken into account here
            // argStartId = the start id to START REPLACING THE ID(s)
            // EXCLUDING THE ID that was deleted
            // this means if 4th row is deleted, the id passed would be 5,
            // and would be set to 4, and so on
            // argEndId = the end id
            function resetGridId(argGrdId, argStartId, argEndId) {
                var _iCounter, _newID;
                for (_iCounter = argStartId; _iCounter < argEndId; _iCounter++) {
                    _newID = _iCounter - 1;
                    // change the text
                    $('#' + argGrdId).find('#SNo_' + _iCounter).text(_newID);
                    // find the row id 
                    $('#' + argGrdId).find('#SNo_' + _iCounter).attr('id', 'SNo_' + _newID);
                    $('#' + argGrdId).find('#Qty_' + _iCounter).attr('id', 'Qty_' + _newID);
                    $('#' + argGrdId).find('#ItemName_' + _iCounter).attr('id', 'ItemName_' + _newID);
                    $('#' + argGrdId).find('#Prc_' + _iCounter).attr('id', 'Prc_' + _newID);
                    $('#' + argGrdId).find('#Remarks_' + _iCounter).attr('id', 'Remarks_' + _newID);
                    $('#' + argGrdId).find('#Color_' + _iCounter).attr('id', 'Color_' + _newID);
                    $('#' + argGrdId).find('#Amount_' + _iCounter).attr('id', 'Amount_' + _newID);
                    $('#' + argGrdId).find('#Edit_' + _iCounter).attr('id', 'Edit_' + _newID);
                    $('#' + argGrdId).find('#Delete_' + _iCounter).attr('id', 'Delete_' + _newID);
                };
                // set the new S.No. to appropriate value
                $('#grdEntry_ctl01_lblHSNo').text($('#hdnCurrentValue').val());

            };

            // Step 7.Z Attacht the event handler for delete button

            // Step 8. Event handler for extra prc button

            // Step 8.Z Attach the event handler for the add extra prc button
            $('#grdEntry_ctl01_btnExtra').click(function (event) {
                $('#plnAddExtraProcess').dialog({ width: 500, modal: true });
                return false;
            });

            // Step 8.Z.1 attach the same event handler to click event
            $('#grdEntry_ctl01_btnExtra').keydown(function (event) {
                if (event.which == 13) {
                    $('#grdEntry_ctl01_btnExtra').click();
                }
            });

            // Step 9. create a event handler for extraprocess save
            $('#btnExtraProcSave').click(function (event) {
                // if prc1 is null and prc2 is not null raise and error
                if ($('#txtExtraProcess1').val() == '' && $('#txtExtraProcess2').val() != '') {
                    alert('can not have second extra process without first extra process!');
                    $('#txtExtraProcess1').focus();
                    return false;
                }
                // if any of the rate is '' set them to 0
                if ($('#txtExtraRate1').val() == '') {
                    $('#txtExtraRate1').val(0);
                }
                if ($('#txtExtraRate2').val() == '') {
                    $('#txtExtraRate2').val(0);
                }
                // all validation checked, now save the result and close the dialog!
                $('#plnAddExtraProcess').dialog('close');
                $('#txtRate').focus();
                return false;
            });

            // Step 9.A attach same event to keydown
            $('#btnExtraProcSave').keydown(function (event) {
                if (event.which == 13) {
                    $('#btnExtraProcSave').click();
                }
            });

            // Step 10.1 On pressing enter on qty, desc, and color, not on color, because this will be used to add the row
            $('#txtQty, #mytags').on('keydown.AttachedEvent', function (event) {

                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    if ($(event.target).attr('id') == 'txtQty') {
                        if ($(event.target).val() == '0' || $(event.target).val() == '') {
                            $(event.target).val('1');
                        }
                        $('#txtName').focus();
                        makeLiTaggable('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', '', $('#hdnBindColor').val(), true, true, -1);
                        return;
                    }
                }

                if (event.which == 9 && event.shiftKey && $(event.target).attr('id') == 'txtQty') {
                    $('#txtCustomerName').focus();
                    return false;
                }

                if ($(event.target).attr('id') != 'txtQty' && event.which != 13 && event.which != 9) {
                    $('#hdnDummy').val(event.which);
                    return;
                }
                else if ($(event.target).attr('id') != 'txtQty' && (event.which == 13 || event.which == 9) && !event.shiftKey) {

                    // check for all six cases
                    if (
                        ($('#hdnDummy').val() == event.which) ||
                        (($('#hdnDummy').val() == 9 || $('#hdnDummy').val() == 13 || $('#hdnDummy').val() == 61 || $('#hdnDummy').val() == 107)
                        &&
                        (event.which == 13 || event.which == 9))
                       ) {
                        if ($('#hdnColorEnabled').val() == 'True') {
                            $('#mytagsColor').find('.tagit-new input').focus();
                        }
                        else {
                            $('#txtProcess').focus();
                        }
                        return;
                    }
                    else {
                        $('#hdnDummy').val(event.which);
                        return;
                    }
                }


                if ($(event.target).attr('id') != 'txtQty' && (event.which == 13 || event.which == 9) && event.shiftKey) {
                    $('#txtName').focus();
                    return false;
                }


                return;
            });


            // Step 10.A.1.Pre
            // on focus out of input box, remove any span
            $('#mytags').focusout(function (e) {
                $('#mytags input').val('');
            });


            // Step 10.A.1.1 On pressing enter at color add the row
            $('#mytagsColor input').keydown(function (event) {
                /* Old
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                // check if the count is eqaul to qty
                // if yes, then okay, else remove it

                if (parseInt($('#mytagsColor > .tagit-choice').size()) >= parseInt($('#txtQty').val())) {
                $('.tagit-new').find('input').val('');
                }
                // changed for enabled
                // $('#grdEntry_ctl01_imgBtnGridEntry').click();
                $('#txtProcess').focus();
                }
                */

                /* New */

                //                if (event.which != 13 && event.which != 9) {
                //                    $('#hdnDummyColor').val(event.which);
                //                    return;
                //                }

                if (event.which != 13 && event.which != 9) {
                    $('#hdnDummyColor').val(event.which);
                    return;
                }

                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {

                    // check for all six cases
                    if (
                        ($('#hdnDummyColor').val() == event.which) ||
                        (($('#hdnDummyColor').val() == 9 || $('#hdnDummyColor').val() == 13 || $('#hdnDummyColor').val() == 61 || $('#hdnDummyColor').val() == 107)
                        &&
                        (event.which == 13 || event.which == 9))
                       ) {
                        $('#txtProcess').focus();
                        //return;
                    }
                    else {
                        $('#hdnDummyColor').val(event.which);
                        return;
                    }
                }


                if ((event.which == 9) && event.shiftKey) {
                    if ($('#hdnDescEnabled').val() == 'True') {
                        $('#mytags').find('.tagit-new input').focus();
                    }
                    else {
                        $('#txtName').focus();
                    }
                    return false;
                }

                $('#hdnDummyColor').val(event.which);
                return;
            });

            // Step 10.A.1.1.Post On losing focus at color, remove any unmade tag
            $('#mytagsColor').focusout(function (e) {
                $('#mytagsColor input').val('');
            });

            // Step 10.B.1 On ItemName change check if item exits
            // if not ask the user to enter the item
            $('#txtName').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtName').one('keyup.AttachedEvent', txtNameKeyUpHanlder);
                }
            });

            // Step 10.B.2 this is the event handler for the keyup event of itemName
            var txtNameKeyUpHanlder;
            txtNameKeyUpHanlder = function (event) {
                if ($(event.target).val() == '') {
                    // Enabled code
                    // if the desc box is enabled, focus it,
                    // else if the color box is enabled, focus on it
                    // else focus on process
                    if ($('#hdnDescEnabled').val() == 'True') {
                        $('#mytags').find('input').focus();
                    }
                    else if ($('#hdnColorEnabled').val() == 'True') {
                        $('#mytagsColor').find('input').focus();
                    }
                    else {
                        $('#txtProcess').focus();
                    }
                    return;
                }

                var item = '' + $(event.target).val() + '';
                var MainItem, newData;
                if (item.indexOf('-') >= 0) {
                    MainItem = item.split('-')[1].trim().toUpperCase();
                    newData = MainItem;
                }
                else {
                    newData = item.toUpperCase();
                }

                if (item.indexOf('-') >= 0) {
                    // check if item exits
                    // Step 10.A.1 find if item exists
                    $.ajax({
                        url: '../AutoComplete.asmx/CheckIfItemExists',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'itemName=' + newData,
                        success: function (response) {
                            // if the value is false, the item don't exists
                            // show the pop up to add new item, and add the item
                            if ($(response).find('string').text() == "false") {
                                // first remove the focusout handler from this(txtName) because that might cause an infinite recursion
                                // $('body').off("focusout.AttachedEvent", '#txtName', txtNameKeydownHandler);
                                $('#txtItemName').val(newData);
                                $('#txtItemSubQty').focus();
                                $('#pnlItem').dialog({ width: 500, modal: true });
                                $(event.target).val('');
                                $(event.target).focus();
                                // $('#hdnItemNameChanged').val('dummy');
                            }
                            else {
                                // if item exists, then just set the text to trimmed upper text
                                $(event.target).val(newData);
                                // find the rate of current item and current prc(s)
                                findItemAndProcessRate(newData, -1, '');
                                // set the focus
                                if ($('#hdnDescEnabled').val() == 'True') {
                                    $('#mytags').find('input').focus();
                                }
                                else if ($('#hdnColorEnabled').val() == 'True') {
                                    $('#mytagsColor').find('input').focus();
                                }
                                else {
                                    $('#txtProcess').focus();
                                }
                            }
                        },
                        error: function (response) {
                            // alert the user of error
                            // alert('Error : server returned ' + response);
                        }

                    });
                }
                else {
                    // check if in array
                    // new
                    // if the discount is applicable
                    var _allItemLst = $('#hdnAllItems').val() + '';
                    //                    var _ItemListStr = _allItemLst.split(':');
                    //                    var _itemListAry = $.makeArray(_ItemListStr);
                    //                    if ($.inArray(newData, _itemListAry) >= 0) {
                    //                        // find the rate of current item and current prc(s)
                    //                        findItemAndProcessRate(newData, -1, '');
                    //                        $('#txtProcess').focus();
                    //                    }
                    if (_allItemLst.indexOf(newData) > -1) {
                        findItemAndProcessRate(newData, -1, '');
                        // set the focus
                        if ($('#hdnDescEnabled').val() == 'True') {
                            $('#mytags').find('input').focus();
                        }
                        else if ($('#hdnColorEnabled').val() == 'True') {
                            $('#mytagsColor').find('input').focus();
                        }
                        else {
                            $('#txtProcess').focus();
                        }
                    }
                    else {
                        $('#txtItemName').val(newData);
                        $('#txtItemCode').focus();
                        $('#pnlItem').dialog({ width: 500, modal: true });
                    }
                }
            };

            // Step 10.B.1 add handler to open of dialog, this will set the appropriate focus
            $('body').on('dialogopen.AttachedEvent', '#pnlItem', function (event) {
                // if txtName is empty set focus to it
                if ($('#txtItemName').val() != '') {
                    // check if given item is not in the list
                    var _allItems = $('#hdnAllItems').val().split(':');
                    if ($.inArray($(event.target).val().toUpperCase(), _allItems) != -1) {
                        // item is already present
                        // keep the focus to it
                        return;
                    }
                    else {
                        // item is not present
                        $('#txtItemSubQty').val('1');
                        $('#txtItemSubQty').focus();
                    }
                    //$('#txtItemName').trigger('blur');
                }
                else {
                    // just set the focus on txtItemCode
                    $('#txtItemName').focus();
                }
            });

            // Step 10.B.2 Attach the event handler
            // $('body').on("focusout.AttachedEvent", '#txtName', txtNameKeydownHandler);

            // Step 10.B.3 Attach the event handler
            // $('body').on("keydown.AttachedEvent", '#txtName', txtNameKeydownHandler);

            // Step 10.B.3.1 On focus on txtName select it
            //            $('#txtName').focus(function (event) {
            //                $('#txtName').select();
            //                $('body').on("keydown.AttachedEvent", '#txtName', txtNameKeydownHandler);
            //            });

            // Step 10.B.4 close handler for add new item dialog box
            // on close set focus to prc
            $('body').on('dialogclose.AttachedEvent', '#pnlItem', function (event) {
                // just set the focus on txtItemCode
                // $('#txtProcess').focus();
                $('#txtName').focus();
            });

            // Step 10.B.4 on the click of imgAddButton show the dalog box
            $('#imgBtnAddNewItem').click(function (event) {
                // open the new item panel
                $('#txtItemName').val($('#txtName').val().toUpperCase());
                $('#pnlItem').dialog({ width: 500, modal: true });
                return false;
            });

            // Step 10.B.5 on the click of imgAddButton show the dalog box
            $('#imgBtnAddNewItem').keydown(function (event) {
                if (event.which == 13) {
                    $('#imgBtnAddNewItem').click();
                }
            });

            // Step 10.C function that finds the rate for given process and item
            // the last field is used to find which field will be set by the text
            // not used till now
            function findItemAndProcessRate(argItem, argPrc, argFieldToSet) {
                if (argItem == '' || argPrc == '') { return; };
                // $('#txtProcess').focus();
                if (argPrc == -1) {
                    // the item is changed, not the process
                    // so find rate for all 3 processes
                    var _prc, _prc1, _prc2, _mainData, _mainData1, _mainData2,
                    _prc = $('#txtProcess').val();
                    _prc1 = $('#txtExtraProcess1').val();
                    _prc2 = $('#txtExtraProcess2').val();
                    _mainData = argItem + ':' + _prc;
                    _mainData1 = argItem + ':' + _prc1;
                    _mainData2 = argItem + ':' + _prc2;

                    // doing this
                    // make 3 calls
                    $.ajax({
                        url: '../AutoComplete.asmx/GetItemRateForProcess',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'arg=' + _mainData,
                        success: function (response) {
                            var result = $(response).find("double").text();
                            $('#txtRate').val(result);
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });

                    if (_prc1 == '') { return; };
                    $.ajax({
                        url: '../AutoComplete.asmx/GetItemRateForProcess',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'arg=' + _mainData1,
                        success: function (response) {
                            var result = $(response).find("double").text();
                            $('#txtExtraRate1').val(result);
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });

                    if (_prc2 == '') { return; };
                    $.ajax({
                        url: '../AutoComplete.asmx/GetItemRateForProcess',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'arg=' + _mainData2,
                        success: function (response) {
                            var result = $(response).find("double").text();
                            $('#txtExtraRate2').val(result);
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });
                    // all calls made
                    // set the focus
                    if ($('#hdnDescEnabled').val() == 'True') {
                        $('#mytags').find('input').focus();
                    }
                    else if ($('#hdnColorEnabled').val() == 'True') {
                        $('#mytagsColor').find('input').focus();
                    }
                    else {
                        $('#txtProcess').focus();
                    }
                }
                // the item is not changed
                else {
                    // the process is changed find the rate of given process
                    // form the string to pass to method
                    var _newData = argItem + ":" + argPrc;
                    if (_newData == "") {

                        return;
                    }


                    $.ajax({
                        url: '../AutoComplete.asmx/GetItemRateForProcess',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: 'arg=' + _newData,
                        success: function (response) {
                            var result = $(response).find("double").text();
                            if (argFieldToSet != '') {
                                $(argFieldToSet).val(result);
                                $(argFieldToSet).focus();
                                $(argFieldToSet).select();
                            }
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });
                    return false;
                }
            };

            // Step 11. this is for adding new item
            // on lost focus, we just caps the item
            var txtItemNameFocusOutHandler = function (event) {
                if ($(event.target).val() != '') {
                    var _allItems = $('#hdnAllItems').val().split(':');
                    if ($.inArray($(event.target).val().toUpperCase(), _allItems) != -1) {
                        //event.preventDefault();
                        alert('Item Already Exist, Please Enter A New Name.');
                        $(event.target).val('');
                        //$('#txtItemName').focus();
                        $(event.target).focus();
                        //alert($('#txtItemName').is(':focus'));
                        //$('#txtItemName').focus();
                        //alert($('#txtItemName').is(':focus'));

                        return false;
                    }
                    else {
                        $(event.target).val($(event.target).val().toUpperCase());
                        return true;
                    }
                    // check if given item exits in the list
                }
            };

            // Step 11.A attach the previous handler
            //$('body').on("focusout.AttachedEvent", '#txtItemName', txtItemNameFocusOutHandler);
            //$('body').on("keydown.AttachedEvent", '#txtItemName', function (e) { 

            //});

            // Step 11.A.1 move to next field on pressing enter
            $('body').on("keydown.AttachedEvent", '#txtItemName', function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    if (txtItemNameFocusOutHandler(event)) {
                        $('#txtItemSubQty').focus();
                    }
                }
                else if ((event.which == 13 || event.which == 9) && event.shiftKey) {
                    return false;
                }
            });

            // Step 11.A.2 event handler for subqty focus, this will select it
            $('#txtItemSubQty').focus(function (event) {
                $(this).select();
            });


            // Step 12.A this handles the enter key and adds the keyup for enter
            $('#txtItemSubQty').keydown(function (event) {
                if ((event.which == 13 || event.which == 9)) {
                    $('#txtItemSubQty').one('keyup.AttachedEvent', txtItemSubQtykeyupHandler);
                }
            });

            // Step 12. event handler for subqty focus out
            // what it does is, shows/hides the listbox and subitemtextbox depending on the qty of subitems
            var txtItemSubQtykeyupHandler = function (event) {

                // if just one qty, set focus to next field
                if ($(this).val() == '1' && !$('#lstSubItem').is(':visible')) {
                    $('#txtItemCode').focus();
                    return;
                }

                if ($(this).val() == '0' && !$('#lstSubItem').is(':visible')) {
                    $(this).val('1');
                    $('#txtItemCode').focus();
                    return;
                }

                // check for if the value is already in listbox, and if the qty entered in less then previously entered, trim the last one
                if ($(event.target).val() != '' && $(event.target).val() > 15) {
                    alert("Invalid value for subitems, can't be more than 15");
                    $(event.target).focus();
                    return false;
                }

                // check is listbox is visible
                if ($('#lstSubItem').is(':visible')) {
                    // it is visible, check if the qty entered is less then items in listbox
                    if ($('#txtItemSubQty').val() == 1) {
                        // remove all but 1 item
                        var _qtyToRemove = $('#lstSubItem option').size() - $('#txtItemSubQty').val();
                        var _totalSize = $('#lstSubItem option').size();
                        var _i;
                        for (_i = 0; _i < _qtyToRemove; _i++) {
                            $('#lstSubItem option:eq(' + (_totalSize - _i - 1) + ')').remove();
                        }
                        $('#txtNewItemName').hide();
                        $('#lstSubItem').hide();
                        $('#txtItemCode').focus();
                    }
                    else if ($('#txtItemSubQty').val() < $('#lstSubItem option').size()) {
                        // it is less, starting from last, remove (option.count - qty)
                        var _qtyToRemove = $('#lstSubItem option').size() - $('#txtItemSubQty').val();
                        var _totalSize = $('#lstSubItem option').size();
                        var _i;
                        for (_i = 0; _i < _qtyToRemove; _i++) {
                            $('#lstSubItem option:eq(' + (_totalSize - _i - 1) + ')').remove();
                        }
                        $('#txtNewItemName').hide();
                        $('#lstSubItem').show();
                        $('#txtItemCode').focus();
                    }
                    else if ($('#txtItemSubQty').val() == $('#lstSubItem option').size()) {
                        // the values are equal, just hide the text
                        $('#txtNewItemName').hide();
                        $('#lstSubItem').show();
                        $('#txtItemCode').focus();
                    }
                    else {
                        // if it is neither less, nor equal then it must be big
                        $('#txtNewItemName').show();
                        $('#txtNewItemName').val('');
                        $('#txtNewItemName').focus();
                        $('#lstSubItem').show();
                    }
                    return;
                }

                // if listbox is hidden, and noOfSubItemsQty is less then options count
                // then show the apporpriate elements
                if ($(event.target).val() != '' && $(event.target).val() > 1) {
                    $('#txtNewItemName').show();
                    $('#txtNewItemName').val('');
                    $('#txtNewItemName').focus();
                    $('#lstSubItem').show();
                    return;
                }
                // if listbox is hidden(or not!), and noOfSubItemsQty is greater then options count
                // then show the apporpriate elements
                else {
                    $('#txtNewItemName').hide();
                    $('#lstSubItem').hide();
                }
            }

            // Step 12.B Attach the event handler
            $('#txtItemSubQty').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtItemSubQty').one('keyup.AttachedEvent', txtItemSubQtyKeyUpHandler);
                }
            });


            // $('body').on("focusout.AttachedEvent", '#txtItemSubQty', txtItemSubQtyFocusOutHandler);

            //            // Step 12.B.1 on the keydown of enter do the same things too..
            //            $('body').on("keydown.AttachedEvent", '#txtItemSubQty', function (event) {
            //                if (event.which == 13) {
            //                    // if item qty is 1, just set focus to next field
            //                    if ($(this).val() == '1') {
            //                        $('#txtItemCode').focus();
            //                    }
            //                    else {
            //                        $('#txtItemSubQty').focusout();
            //                    }
            //                }
            //            });

            // Step 13. Event handler for NewItemFocusOut
            // what it does is caps the input,
            // verifes if item count in listbox is less then qtyOfSubItems
            // and then do the appropriate handling
            var txtNewItemNameKeyUpHandler = function (event) {

                // this one's complicated, if listitem is visible
                if ($('#lstSubItem').is(':visible')) {
                    if ($('#lstSubItem option').size() == $('#txtItemSubQty').val()) {
                        // if the size is same, hide the textbox
                        $('#txtNewItemName').hide();
                        $('#txtItemCode').focus();
                        return;
                    }
                }
                // 1. if item is blank, set focus to this
                if ($(event.target).val() == '') {
                    $(event.target).focus();
                    return;
                }
                // 2. if item is not blank
                else {
                    // 3. Caps the input
                    // Doing inline
                    // 4.  add it to the list
                    if ($('#lstSubItem option').size() < $('#txtItemSubQty').val()) {
                        $('#lstSubItem').append('<option>' + $(event.target).val().toUpperCase() + '</option>');
                        $(event.target).val('');
                        $(event.target).focus();

                    }
                    else {
                        // 5. count the items in list
                        var itemAddedCount = $('#lstSubItem option').size();
                        // 6. check if that count is equal to the value specified in subItmeQtybox
                        // ERROR ISSUE, same as above!
                        if (parseInt(itemAddedCount) < parseInt($('#txtItemSubQty').val())) {
                            // 7. if no, focus on this, and return
                            $(event.target).val('');
                            $(event.target).focus();

                        }
                        else {
                            // 8. if yes, hide this, and set the focus to itemcode
                            $('#txtNewItemName').hide();
                            $('#txtItemCode').focus();

                        }
                    }
                }
                // check here to hide
                if ($('#lstSubItem option').size() == $('#txtItemSubQty').val()) {
                    // if the size is same, hide the textbox
                    $('#txtNewItemName').hide();
                    $('#txtItemCode').focus();

                }
            };

            // Step 13.A Attach the event handler
            $('#txtNewItemName').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtNewItemName').one('keyup.AttachedEvent', txtNewItemNameKeyUpHandler);
                }
            });



            // Step 14. Item code focus out handler
            var txtItemCodeKeyUpHandler = function (event) {
                // caps the value
                var _retValue = '';
                if ($(event.target).val() != '') {
                    // $(event.target).val($(event.target).val().toUpperCase());


                    // check if this item code exits
                    $.ajax({
                        url: '../AutoComplete.asmx/checkIfItemCodeExits',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        async: false,
                        data: 'argItemCode=' + $(event.target).val().toUpperCase(),
                        success: function (response) {
                            var _result = $(response).find('boolean').text()
                            // if result is true, the given item code is present
                            // else add to database
                            if (_result == 'true') {
                                alert('Supplied code already exists, please enter a new code!');
                                $(event.target).val('');
                                $(event.target).focus();
                                _retValue = false;
                            }
                            else {
                                _retValue = true;
                            }
                        },
                        error: function (response) {
                            // alert('Error : server returned ' + response);
                        }
                    });
                }
                return _retValue;
            };

            // Step 14. A Attach the event handler
            $('#txtItemCode').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    var _res = txtItemCodeKeyUpHandler(event);
                    if (_res) {
                        // $('#btnItemSave').click();
                        btnItemSaveClickHander(event);
                    }
                }
                //                else if (event.which == 9 && !event.shiftKey) {
                //                    var _res = txtItemCodeKeyUpHandler(event);
                //                    if (_res) {
                //                        // $('#btnItemSave').click();
                //                        $('#btnItemSave').focus();
                //                    }
                //                }
            });

            // $('body').on("focusout.AttachedEvent", '#txtItemCode', txtItemCodeFocusOutHandler);

            // function validateitem, this func validates the newItem before adding it to database
            // event : the target that raised the event
            function validateItemData(event) {
                // 1. check if there is no null value
                if ($('#txtItemName').val() == '') {
                    $('#btnItemSave').attr('disabled', true);
                    alert('error in itemname');
                    $('#btnItemSave').attr('disabled', false);
                    return false;
                }
                if ($('#txtItemSubQty').val() == '') {
                    $('#btnItemSave').attr('disabled', true);
                    alert('error in item qty');
                    $('#btnItemSave').attr('disabled', false);
                    return false;
                }
                if ($('#txtItemCode').val() == '') {
                    $('#btnItemSave').attr('disabled', true);
                    alert('error in itemcode');
                    $('#btnItemSave').attr('disabled', false);
                    return false;
                }
                // 2. check if the value in qty is > 1, if yes, then check if value = count in list
                if ($('#txtItemSubQty').val() > 1) {
                    if ($('#lstSubItem option').size() != $('#txtItemSubQty').val()) {
                        alert('error in subitemcount');
                        return false;
                    }
                }
                return true;
            };

            // Step 15. Save handler for new item
            var btnItemSaveClickHander = function (event) {
                // validate the items
                if (validateItemData(event)) {
                    var _subItemList = '&subItems=';
                    var _MainSubItemList;
                    // for all items in listbox, add them to the subitem list
                    $('#lstSubItem option').each(function (index) {
                        _subItemList = _subItemList + $('#lstSubItem option:eq(' + index + ')').val() + '&subItems=';
                    });
                    _MainSubItemList = _subItemList.substring(0, _subItemList.length - 10);
                    // formulate the data to be passed to server
                    var _dataString = 'itemName=' + $('#txtItemName').val() + '&itemCode=' + $('#txtItemCode').val() +
                                            '&qtyOfSubItems=' + $('#txtItemSubQty').val() + _MainSubItemList;
                    // in case there is no subitem, we need to add this item as subitem
                    if (_dataString.indexOf('subItems' < 0)) {
                        _dataString = _dataString + '&subItems=\'\'';
                    }
                    // add item to master
                    $.ajax({
                        url: '../AutoComplete.asmx/AddItemsToMaster',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'xml',
                        cache: false,
                        data: _dataString,
                        success: function (response) {
                            if ($(response).find('string').text() == "Record Saved") {
                                // set the name in main name
                                $('#txtName').val($('#txtItemName').val());
                                $('#pnlItem').dialog("close");
                                $('#mytags').find('input').focus();
                                // add the item to the list of already added items
                                var _val = $('#hdnAllItems').val();
                                _val = _val + ':' + $('#txtName').val().toUpperCase();
                                $('#hdnAllItems').val(_val);
                            }
                            else {
                                alert('Error : ' + $(response).find('string').text());
                            }
                        },
                        error: function (response) {
                            // alert('server returned ' + response);
                        }
                    });
                }
            };

            // Step 15.A attach the event handler
            $('body').on("click.AttachedEvent", '#btnItemSave', btnItemSaveClickHander);

            // Step 15.A.1 attach the event handler
            $('body').on("keydown.AttachedEvent", '#btnItemSave', function (event) {
                if (event.which == 13) {
                    $('#btnItemSave').click();
                }
            });

            // Step 16. Handler for Item save
            // does nothing but againg sets the txtItemFocusOut hanlder, 
            // that we removed for the fear of causing infinite recursion
            var pnlItemSavedialogcloseHander = function () {
                //$('body').on("focusout.AttachedEvent", '#txtName', txtNameKeydownHandler);
                // clear the fields in that dialog box
                //$('#txtItemName').val('');
                $('#txtItemSubQty').val('');
                $('#txtItemCode').val('');
                $('#txtNewItemName').val('');
                $('#txtNewItemName').hide();
                $('#lstSubItem option').remove();
                $('#lstSubItem').hide();
            };

            // Step 16.A Attach that event handler
            $('body').on("dialogclose.AttachedEvent", '#pnlItem', pnlItemSavedialogcloseHander);


            // Step 17. Add the event handler for txtAdvance lost amount
            $('#txtAdvance').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtAdvance').one('keyup.AttachedEvent', txtAdvanceKeyUpHandler);
                }
            });

            var txtAdvanceKeyUpHandler = function (event) {
                if ($(this).val() == '') { return; };
                var _allTotal = parseFloat($('#txtTotal').val());
                if (_allTotal == '') { _allTotal = 0; };
                var _curAdvance = $(this).val();
                if (_curAdvance == '') { _curAdvance == 0; };
                // check if advance is not greater than total
                if (parseFloat(_allTotal) < parseFloat(_curAdvance)) {
                    alert('Advance can\'t be greater than total amount!');
                    $(this).val('');
                    $(this).focus();
                    //$('#txtAdvance').select();
                    return false;
                }
                var _balValue = _allTotal - _curAdvance;
                // $('#txtBalance').val(_balValue.toFixed(2));
                setNetBal(_balValue);
                $('#txtQty').focus();
            };

            // Step 18. if rdrAmt is checked, show the txtDiscountAmt,
            // and hide the txtDiscount because, this is flat discount
            $('#rdrAmt').change(function (event) {
                $('#txtDiscountAmt').show();
                $('#txtDiscount').hide();
                $('#txtDiscountAmt').focus();
            });

            // Step 19. if rdrAmt is checked, show the txtDiscountAmt,
            // and hide the txtDiscount because, this is flat discount
            $('#rdrPercentage').change(function (event) {
                $('#txtDiscountAmt').hide();
                $('#txtDiscount').show();
                $('#txtDiscount').focus();
            });

            // Step 20. on change of txtDiscountAmt, set txtDiscount
            // and fire its change event
            $('#txtDiscountAmt').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtDiscountAmt').one('keyup.AttachedEvent', txtDiscountAmtKeyUpHandler);
                }
            });

            var txtDiscountAmtKeyUpHandler = function () {
                if ($(this).val() == '') {
                    $(this).val('0');
                }
                // check if the amount entered is not greater than gross amt
                // make sure that this is not null, and is a number
                // check for parseFloat
                if (parseFloat($(this).val()) > parseFloat($('#txtCurrentDue').val())) {
                    alert('discount amount can\'t be greater than gross amount!');
                    $(this).focus();
                    return false;
                }
                // Step 20.A
                // make the percentage
                var _disPerc = 100 * parseFloat($('#txtDiscountAmt').val()) / parseFloat($('#txtCurrentDue').val());
                _disPerc = _disPerc.toFixed(2);
                // set this perctage to text amount
                $('#txtDiscount').val(_disPerc);
                // set the label to current value
                $('#lblDisAmt').text($(this).val());
                $('#hdnDiscountValue').text($(this).val());

                // set the hidden recomp value to this
                $('#hdnDisAmtRecomp').val($(this).val());
                // if there are just 2 rows, the header and the current, then
                if ($('#grdEntry > tbody > tr').size() == 2) {
                    recomputeAllGrid(1, 2, false, true);
                    $('#txtAdvance').focus();
                    return false;
                }
                else {
                    recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true);
                    $('#txtAdvance').focus();
                    return false;
                }
            };

            // Step 20.B on changing discount
            var txtDiscountkeyUpHandler = function (event) {
                if ($(this).val() == '') {
                    $(this).val('0');
                }
                // find the amount and save it in txtDiscountAmt
                recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true);
                // focus on advance
                $('#txtAdvance').focus();
                return false;

            };



            //            // Step 20.D don't add the handler here,
            //            // add on keydown, that means user has explicitly changed the discount
            //            var txtDiscountKeyDownHandler = function (event) {
            //                // attach the handler here
            //                //$('#hdnAllDiscountFocusOut').val('1');
            //                $('body').on("focusout.AttachedEvent", '#txtDiscount', txtDiscountFocusOutHandler);
            //            };

            // Step 20.E add this handler to keydown event
            $('#txtDiscount').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtDiscount').one('keyup.AttachedEvent', txtDiscountkeyUpHandler);
                }
            });


            //            // $('body').on("keydown.AttachedEvent", '#txtDiscount, #txtDiscountAmt', txtDiscountKeyDownHandler);
            //            $('body').on("focusout.AttachedEvent", '#txtDiscount', txtDiscountFocusOutHandler);

            // Step 20.F this will remove the event handler, on the 
            // keypress in txtBalance, will fire that event manually at the end of recomputeAllGrid
            var txtBalanceKeyDownHandler = function (event) {
                // remove the handler here
                $('body').off("focusout.AttachedEvent", '#txtDiscount', txtDiscountKeyDownHandler);
            };

            //            // Step 20.G, handler for txtDiscount change, and txtDiscountAmt change
            //            var txtDisChangeHandler = function (event) {
            //                alert('hey');
            //                return false;
            //            }

            //            $('body').on('change.AttachedEvent', '#txtDiscount, #txtDiscountAmt', txtDisChangeHandler);

            // Step 20.H this will return false on dis change
            // so that no postback occurs
            // $('body').delegate('#txtDiscount, #txtDiscountAmt', 'change.AttachedEvent', function (event) { return false; });

            // Step 21. this will recalculate all the data of lower grid
            // argStart = the starting number to being counting from
            // argEnd = the row to stop count at
            // argCallEachRowAsUpdate = call row as updadte or not
            // argAddDisAndTax = should dis and tax be added or subtracted?
            function recomputeAllGrid(argStartRow, argEndRow, argCallEachRowAsUpdate, argAddDisAndTax) {
                var _iCounter;
                // remove the event handler
                // $('body').off("focusout.AttachedEvent", '#txtDiscount', txtDiscountFocusOutHandler);
                //alert($(document.activeElement).attr('id'));
                //alert('set off 2');
                // let the txtCurrentDue remain the same, because it will not change
                // set the lblDisAmt = 0;

                // only set lblDisAmt to 0, if disocuntAmt is not visible
                if ($('#txtDiscountAmt').is(':visible') == false) {
                    $('#lblDisAmt').text('0');
                    $('#hdnDiscountValue').val('0');
                    $('#hdnDisAmtRecomp').val('0');
                }

                // set the tax = 0
                $('#txtSrTax').val('0');
                // set the total to 0
                $('#txtTotal').val('0');
                // don't change the value of advance
                // change the value of balance
                // $('#txtBalance').val('0');
                setNetBal('0');
                // clear the alltax, and alldiscount
                $('#hdnAllTax').val('');
                $('#hdnAllDiscount').val('');
                //$('#hdnAllDiscountFocusOut').val('0');



                /*******************************************/
                // set the hidden values 0, that will be used for computation in this case
                $('#hdnTaxAmtRecomp').val('0');


                $('#hdnBalance').val('0');
                $('#hdnTotal').val('0');

                $('#hdnTaxableAmt').val('0');

                // for the given number of rows
                var _reCompProcessArray, _reCompQty;
                var _reCompPrc, _reCompRate, _reCompPrc1, _reCompRate1, _reCompPrc2, _reCompRate2;
                var _reCompAllSplittedPrcRate;

                for (_iCounter = argStartRow; _iCounter < argEndRow; _iCounter++) {
                    // find qty
                    _reCompQty = $('body').find('#grdEntry > tbody').find('#Qty_' + _iCounter + '').text() + '';
                    // find the process array for each
                    var _reCompProcessArray = $('body').find('#grdEntry > tbody').find('#Prc_' + _iCounter + '').text() + '';
                    _reCompAllSplittedPrcRate = splitPrcRateFromArray(_reCompProcessArray);
                    // set the variables
                    // alert(_reCompAllSplittedPrcRate);
                    _reCompPrc = _reCompAllSplittedPrcRate.prc;
                    _reCompPrc1 = _reCompAllSplittedPrcRate.prc1;
                    _reCompPrc2 = _reCompAllSplittedPrcRate.prc2;
                    _reCompRate = _reCompAllSplittedPrcRate.rate;
                    _reCompRate1 = _reCompAllSplittedPrcRate.rate1;
                    _reCompRate2 = _reCompAllSplittedPrcRate.rate2;
                    // alert(_reCompPrc2);
                    //alert($(document.activeElement).attr('id'));
                    // find the disAndTax
                    ComputeRowDisTaxAmt(_reCompPrc, _reCompRate, _reCompPrc1, _reCompRate1, _reCompPrc2, _reCompRate2, _reCompQty, false, _iCounter, true);
                    //alert($(document.activeElement).attr('id'));
                }

                /*************************************/
                // set the values from hidden field

                // inclusive case
                if ($('#hdnIsTaxExclusive').val() == 'false') {
                    $('#txtSrTax').val('0');
                }
                else {
                    $('#txtSrTax').val($('#hdnTaxAmtRecomp').val());
                }

                $('#lblDisAmt').text($('#hdnDisAmtRecomp').val());
                $('#hdnDiscountValue').val($('#hdnDisAmtRecomp').val());
                $('#txtTotal').val($('#hdnTotal').val());
                $('#txtBalance').val($('#hdnBalance').val());


                // reset all values
                setTheDefaults();
                $('#grdEntry_ctl01_lblHAmount').text($('#txtCurrentDue').val());
                // attach the event handler again
                // $('body').on("focusout.AttachedEvent", '#txtDiscount', txtDiscountFocusOutHandler);
            };

            // Step 22. on keypress at rate, if keypress is enter, call the add button
            $('#txtRate').keydown(function (event) {
                //alert(event.which);
                // check for if key is enter
                if (event.which == 13) {
                    $('#grdEntry_ctl01_imgBtnGridEntry').trigger('click.AttachedEvent');
                    return false;
                }
            });

            //            $('body').on('keydown.AttachedEvent', '#txtRate', function (event) {
            //                if (event.which == 13 || event.which == 9) {
            //                    $('#grdEntry_ctl01_imgBtnGridEntry').click();
            //                    return false;
            //                }
            //            });

            // Step 22.A on the keydown of txtExtraRate1 or txtExtrarate2 close the dialog
            // and set focus to txtrate
            $('#txtExtraRate1, #txtExtraRate2').keydown(function (event) {
                // check for if key is enter
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    if ($(event.target).attr('id') == 'txtExtraRate1') {
                        $('#txtExtraProcess1').focus();
                    }
                    else {
                        $('#plnAddExtraProcess').dialog('close');
                        $('#txtRate').focus();
                        return false;
                    }
                }
            });

            // Step 23. pre, this is the handler for the clicking of the butn in the menu
            $('#btnF8').click(function (e) {
                // store the last button clicked
                $('#hdnButtonClicked').val('btnF8');
                // store the last span clicked
                $('#hdnSpanClicked').val('btnF8');
                if (btnSaveBookingClickHandler(e)) {
                    return true;
                }
                else {
                    return false;
                }
            });

            // Step 23. on btnSaveBooking if no customer return false
            var btnSaveBookingClickHandler = function (event) {
                if ($('#txtCustomerName').val() == '' || $('#lblAddress').text() == '') {
                    alert('no customer selected! Please select a customer!');
                    $('#txtCustomerName').focus();
                    //event.stopimmediatepropagation();
                    return false;
                }
                // check the grid data, if size is less then 2, its not a valid booking
                if ($('#grdEntry > tbody > tr').size() < 2) {
                    alert('Not enough data to add! Please add more entries!');
                    return false;
                }
                // validate lower right grid
                if (!validateLowerCalcGrid()) {
                    return false;
                }
                // set the grid count
                var _grdRowSize = parseInt($('#grdEntry > tbody > tr').size()) - parseInt('1');
                $('#hdnGrdRowCount').val(_grdRowSize);

                maketheArrayToStore();

                
                // set the new tax fields
                // set in find tax for process
                if ($('#hdnIsTaxExclusive').val() == 'true') {
                    $('#hdnTaxType').val('EXCLUSIVE');
                }
                else {
                    $('#hdnTaxType').val('INCLUSIVE');
                }

                // verify the date
                if (!validateDeliveryDate(false, 'txtDueDate')) {
                    alert('Due date can\'t be before the booking date');
                    return false;
                }


                // check if asked for confirmation
                if ($('#hdnConfirmDelivery').val() == 'true') {
                    //alert('me');
                    $('#pnlConfirmDate').dialog({ width: 250, modal: true, closeOnEscape: false });
                    return false;
                }

                // enable the attrs
                $('#txtCurrentDue').attr('disabled', false);
                $('#txtSrTax').attr('disabled', false);
                $('#txtTotal').attr('disabled', false);
                $('#txtBalance').attr('disabled', false);


                return true;

                //                if (event.currentTarget.className == 'btnSaveBookingContainder') {
                //                    $('#btnSaveBooking').click();
                //                }
                //                else if (event.currentTarget.className == 'btnPrintBookingContainder') {
                //                    //$('#btnSavePrint')
                //                }
                //                else if (event.currentTarget == 'div.btnSavePrintPrintTagsBookingContainder') {

                //                }
                //                else if (event.currentTarget == 'div.btnSavePrintTagsBookingContainder') {

                //                }
            };

            // Step 23.A.1 this function validates the lower right grid calculation
            function validateLowerCalcGrid() {
                // check all values are greater then 0
                var _CurrentDue = $('#txtCurrentDue').val();
                if (parseFloat(_CurrentDue) < 0) {
                    alert('Invalid value in Gross Amount');
                    return false;
                }
                var _dis = $('#lblDisAmt').text();
                if (_dis == '') {
                    _dis = '0';
                }
                if (parseFloat(_dis) < 0) {
                    alert('Invalid value in discount');
                    return false;
                }
                var _tax = $('#txtSrTax').val();
                if (parseFloat(_tax) < 0) {
                    alert('Invalid value in tax');
                    return false;
                }
                var _total = $('#txtTotal').val();
                if (parseFloat(_total) < 0) {
                    alert('Invalid value in total');
                    return false;
                }
                var _advance = $('#txtAdvance').val();
                if (parseFloat(_advance) < 0) {
                    alert('Invalid value in advance');
                    return false;
                }
                var _bal = $('#txtBalance').val();
                if (parseFloat(_bal) < 0) {
                    alert('Invalid value in balance');
                    return false;
                }

                // check if gross total - discount + tax == total
                if (parseFloat(_CurrentDue) - parseFloat(_dis) + parseFloat(_tax) != parseFloat(_total)) {
                    //alert('Error in discount');
                    //return false;
                }

                // check if total - advance == net
                if (parseFloat(_total) - parseFloat(_advance) != parseFloat(_bal)) {
                    //alert('Error in advance');
                    //return false;
                }

                // all validation complete, return true
                return true;
            }

            // Step 23.A attach the previous handler
            //$('body').on('click.AttachedEvent', '#btnSaveBooking, #btnSaveBooking.btnsavebookingspan, .btnSaveBookingContainder', btnSaveBookingClickHandler);

            // Step 23.A.1 
            $('.btnSaveBookingContainder').on('click.AttachedEvent', function (e) {
                // store the last button clicked
                $('#hdnButtonClicked').val('btnSaveBooking');
                // store the last span clicked
                $('#hdnSpanClicked').val('btnSaveBookingContainder');
                //$('#btnSaveBooking').click();
                if (btnSaveBookingClickHandler(e)) {
                    $('.btnSaveBookingContainder').off('click.AttachedEvent');
                    $('#btnSaveBooking').click();
                    return true;
                }
                else {
                    return false;
                }
            });



            // Step 23.B event handler for btnSaveAndPrint
            // nothing but same as previous handler
            //$('body').on('click.AttachedEvent', '#btnSavePrint, #btnSavePrint.btnsavebookingspan, .btnPrintBookingContainder', btnSaveBookingClickHandler);

            // Step 23.B.1 
            $('.btnPrintBookingContainder').on('click.AttachedEvent', function (e) {
                // store the last button clicked
                $('#hdnButtonClicked').val('btnSavePrint');
                // store the last span clicked
                $('#hdnSpanClicked').val('btnPrintBookingContainder');
                if (btnSaveBookingClickHandler(e)) {
                    $('.btnPrintBookingContainder').off('click.AttachedEvent');
                    $('#btnSavePrint').click();
                    return true;
                }
                else {
                    return false;
                }
            });


            // Step 23.C event handler for btnPrintBarcode
            // nothing but same as previous handler
            //$('body').on('click.AttachedEvent', '#btnPrintBarCode, #btnPrintBarCode.btnsavebookingspan', btnSaveBookingClickHandler);

            // Step 23.C.1 
            $('.btnSavePrintTagsBookingContainder').on('click.AttachedEvent', function (e) {
                // store the last button clicked
                $('#hdnButtonClicked').val('btnPrintBarCode');
                // store the last span clicked
                $('#hdnSpanClicked').val('btnSavePrintTagsBookingContainder');
                if (btnSaveBookingClickHandler(e)) {
                    $('.btnSavePrintTagsBookingContainder').off('click.AttachedEvent');
                    $('#btnPrintBarCode').click();
                    return true;
                }
                else {
                    return false;
                }
            });

            // Step 23.D event handler for btnPrintBarcode
            // nothing but same as previous handler
            //$('body').on('click.AttachedEvent', '#btnSavePrintBarCode, #btnSavePrintBarCode.btnsavebookingspan', btnSaveBookingClickHandler);

            // Step 23.D.1 
            $('.btnSavePrintPrintTagsBookingContainder').on('click.AttachedEvent', function (e) {
                // store the last button clicked
                $('#hdnButtonClicked').val('btnSavePrintBarCode');
                // store the last span clicked
                $('#hdnSpanClicked').val('btnSavePrintPrintTagsBookingContainder');
                if (btnSaveBookingClickHandler(e)) {
                    $('.btnSavePrintPrintTagsBookingContainder').off('click.AttachedEvent');
                    $('#btnSavePrintBarCode').click();
                    return true;
                }
                else {
                    return false;
                }
            });

            // Step 23.E.
            // this the the handler for pnlConfirmDate
            var handlerDrpList = function (event) {
                // if key is enter, press the confirm button and save it
                if (event.which == 13) {
                    // first set the date in delivery text box
                    var _delDate = $('#drpDate').val();
                    _delDate += ' ' + $('#drpMonth').val();
                    _delDate += ' ' + $('#drpYear').val();
                    //alert(_delDate);
                    $('#txtDueDate').val(_delDate);
                    //$('#btnConfirmDate').click();

                    // validate the date
                    if (!validateDeliveryDate(true, 'drpDate')) {
                        alert('due date can\'t be before booking date');
                        $('#drpDate').focus();
                        return false;
                    }

                    // close the dialog
                    $('#pnlConfirmDate').dialog('close');
                    // click the last button
                    var _btnToclick = $('#hdnButtonClicked').val();
                    var _spanToClick = $('#hdnSpanClicked').val();
                    //alert(_btnToclick);
                    // remove the handler
                    $('#' + _btnToclick).off('click');
                    // remove handler on span
                    $('.' + _spanToClick).off('click');
                    //alert(_spanToClick);
                    event.preventDefault();

                    // for that error where it clears the txtDiscount
                    $('#hdnDiscountPerc').val($('#txtDiscount').val());

                    // enable the button
                    // enable the attrs
                    $('#txtCurrentDue').attr('disabled', false);
                    $('#txtSrTax').attr('disabled', false);
                    $('#txtTotal').attr('disabled', false);
                    $('#txtBalance').attr('disabled', false);

                    //alert($('#hdnDiscountPerc').val());
                    $('#' + _btnToclick).click();
                    return true;
                }

                // if key is tab
                else if (event.which == 9 && event.shiftKey) {
                    // if target is first return false
                    if ($(event.target).attr('id') == 'drpDate') {
                        return false;
                    }
                    else if ($(event.target).attr('id') == 'drpMonth') {
                        $('#drpDate').focus();
                    }
                    else {
                        $('#drpMonth').focus();
                    }
                    return false;
                }

                else if (event.which == 9 && !event.shiftKey) {
                    if ($(event.target).attr('id') == 'drpYear') {
                        return false;
                    }
                    else if ($(event.target).attr('id') == 'drpMonth') {
                        $('#drpYear').focus();
                    }
                    else {
                        $('#drpMonth').focus();
                    }
                    return false;
                }
                // left arrow
                else if (event.which == 37) {
                    event.preventDefault();
                    console.log('At the time of 37 in ' + $(event.target).attr('id') + ' and val ' + $(event.target).val());
                    console.log(event.which + ' and ' + $(event.target).attr('id'));
                    if ($(event.target).attr('id') == 'drpYear') {
                        //$('#drpDate, #drpMonth, #drpYear').off('keydown');
                        $(this).blur();
                        setTimeout(function () { $('#drpMonth').focus().select(); }, 10);
                        return false;
                    }
                    else if ($(event.target).attr('id') == 'drpMonth') {
                        ///$('#drpDate, #drpMonth, #drpYear').off('keydown');
                        $(this).blur();
                        setTimeout(function () { $('#drpDate').focus().select(); }, 10);
                        return false;
                    }
                    else {
                        //$('#drpDate, #drpMonth, #drpYear').off('keydown');
                        return false;
                    }
                    return false;
                }

                // right arrow
                else if (event.which == 39) {
                    event.preventDefault();
                    //                    $('#hdnPrevYear').val($('#drpYear').val());
                    //                    $('#hdnPrevYear').val($('#drpYear').attr('SelectedIndex'));
                    console.log('At the time of 39 in ' + $(event.target).attr('id') + ' and val ' + $(event.target).val());
                    console.log(event.which + ' and ' + $(event.target).attr('id'));
                    if ($(event.target).attr('id') == 'drpDate') {
                        //$('#drpDate, #drpMonth, #drpYear').off('keydown');
                        $(this).blur();
                        setTimeout(function () { $('#drpMonth').focus().select(); }, 10);
                        return false;
                    }
                    else if ($(event.target).attr('id') == 'drpMonth') {
                        //$('#drpDate, #drpMonth, #drpYear').off('keydown');
                        $(this).blur();
                        setTimeout(function () { $('#drpYear').focus().select(); }, 10);
                        return false;
                    }
                    else {
                        //$('#drpDate, #drpMonth, #drpYear').off('keydown');
                        return false;
                    }

                    return false;
                }

            };

            $('#drpDate, #drpMonth, #drpYear').on('keydown.AttachedEvent', handlerDrpList);

            // Step 23.E.1 On the click of confirm button, do the same as above
            $('#btnConfirmDate').on('click.AttachedEvent', function (event) {
                var _delDate = $('#drpDate').val();
                _delDate += ' ' + $('#drpMonth').val();
                _delDate += ' ' + $('#drpYear').val();
                $('#txtDueDate').val(_delDate);


                // validate the date
                if (!validateDeliveryDate(true, 'drpDate')) {
                    alert('due date can\'t be before booking date');
                    $('#drpDate').focus();
                    return false;
                }


                // close the dialog
                $('#pnlConfirmDate').dialog('close');
                // click the last button
                var _btnToclick = $('#hdnButtonClicked').val();
                var _spanToClick = $('#hdnSpanClicked').val();
                //alert(_btnToclick);
                // remove the handler
                $('#' + _btnToclick).off('click');
                // remove handler on span
                $('.' + _spanToClick).off('click');
                //alert(_spanToClick);
                event.preventDefault();

                // for that error where it clears the txtDiscount
                $('#hdnDiscountPerc').val($('#txtDiscount').val());

                // enable the attrs
                $('#txtCurrentDue').attr('disabled', false);
                $('#txtSrTax').attr('disabled', false);
                $('#txtTotal').attr('disabled', false);
                $('#txtBalance').attr('disabled', false);

                //alert($('#hdnDiscountPerc').val());
                $('#' + _btnToclick).click();
                return true;
            });

            // Step 23.E.3
            // argComingFromPanel: boolean indicating weather coming from panel
            // argElementToFocus: element on which to set focus if returns false
            // this checks if due date is after the booking date or not
            function validateDeliveryDate(argComingFromPanel, argElementToFocus) {
                var _bkDate = $('#txtDate').val() + '';
                var _bkDateMil = Date.parse(_bkDate);
                var _dlDate = $('#txtDueDate').val() + '';
                var _dlDateMil = Date.parse(_dlDate);
                if (_dlDateMil >= _bkDateMil) {
                    return true;
                }
                else {
                    return false;
                }
            }

            // Step 23.E.4 on the focus of the drop down, color the background
            //            $('#drpDate, #drpMonth, #drpYear').focus(function (event) {
            //                //$('#drpDate, #drpMonth, #drpYear').on('keydown', handlerDrpList);
            //                //$(event.target).attr('disabled', false);
            //                //$(event.target).attr('selectedIndex',-
            //                console.log('At the time of focus in ' + $(event.target).attr('id') + ' and val ' + $(event.target).val());
            //                console.log(event.which + ' and you ' + $(event.target).attr('id'));
            //                $(event.target).css('background-color', 'yellow');
            //                console.log(event.which + ' and me ' + $(event.target).attr('id'));


            //                if ($(event.target).attr('id') == 'drpYear') {
            //                    //  INDEX
            //                    var _in = 0;
            //                    var _tmp = '';
            //                    for (var i = 0; i < $('#drpYear option').size(); i++) {
            //                        if ($('#drpYear option:eq(' + i + ')').attr('selected') != null) {
            //                            _in = i;
            //                            _tmp = $('#drpYear option:eq(' + i + ')').val();
            //                            //alert(_in);
            //                        }
            //                    }

            //                    //var _pvIndex = $('#drpYear').attr('selectedIndex');
            //                    console.log('Index is ' + _in);
            //                    if (_in != 0) {
            //                        _in = _in - 1;
            //                    }

            //                    //_in = 0;

            //                    console.log('Index is ' + _in);
            //                    $('#drpYear option:eq(' + _in + ')').attr('selected', true);
            //                    $('#drpYear').val(_tmp);
            //                }

            //            }).focusout(function (event) {
            //                //$('#drpDate, #drpMonth, #drpYear').off('keydown');
            //                $(event.target).css('background-color', 'white');
            //                console.log('At the time of focus out ' + $(event.target).attr('id') + ' and val ' + $(event.target).val());
            //            });


            //            $('#drpDate, #drpMonth, #drpYear').keyup(function (event) {
            //                //if (event.which == null || event.which == 'undefined') return false;
            //                //alert(event.which);
            //                if (event.which == 37 || event.which == 39) {
            //                    if ($(event.target).attr('id') == 'drpDate') {
            //                        $('#drpMonth, #drpYear').attr('disabled', true);
            //                    }
            //                    else if ($(event.target).attr('id') == 'drpMonth') {
            //                        $('#drpDate, #drpYear').attr('disabled', true);
            //                    }
            //                    else {
            //                        $('#drpDate, #drpMonth').attr('disabled', true);
            //                    }
            //                } return false;
            //                console.log('Key up  change' + event.which + ' and you ' + $(event.target).attr('id') + ' and val ' + $(event.target).val());
            //                $(event.target).css('background-color', 'yellow');
            //                console.log('Key up For change' + event.which + ' and me ' + $(event.target).attr('id') + ' and val ' + $(event.target).val());
            //            });

            //                        $('#drpDate, #drpMonth, #drpYear').change(function (event) {

            //                          
            //                        });


            // On dialog open of confirm date
            $('body').on('dialogopen.AttachedEvent', '#pnlConfirmDate', function (event) {
                // load the date that is selected in the box
                var _dt = $('#txtDueDate').val() + '';
                var _datePart = _dt.split(' ')[0];
                var _mnth = _dt.split(' ')[1];
                var _yr = _dt.split(' ')[2];
                // if date part contains 2 digits, as in 04 or something, trim the first one
                if (_datePart[0] == '0') {
                    _datePart = _datePart[1];
                }
                $('#drpDate').val(_datePart).find("option[value='" + _datePart + "']").attr("selected", true);
                $('#drpMonth').val(_mnth).find("option[value='" + _mnth + "']").attr("selected", true); ;
                $('#drpYear').val(_yr).find("option[value='" + _yr + "']").attr("selected", true); ;
                $('#drpDate').focus();
            });

            // Step 23.E On click of span
            //            $('.btnsavebookingspan').one('click.AttachedEvent', function (event) {
            //                var _btnToClick = $(this).closest('td').find('input').attr('id');
            //                $('#' + _btnToClick + '').click();
            //            });

            // Step 24. this function makes the array for each row in the grid and stores it a hidden field
            // the struct is in the form of row1item1:row1item2:........_row2item1:row2Item2..........
            function maketheArrayToStore() {
                var _mainDataAll = '';
                var _mainDataRow = '';
                var _rowCount = $('#grdEntry > tbody > tr').size() - parseInt('1');
                var _i;
                for (_i = 1; _i <= _rowCount; _i++) {
                    _mainDataRow = '';
                    _mainDataRow = $('#grdEntry').find('#SNo_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#Qty_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#ItemName_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#Prc_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#Remarks_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#Color_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#Amount_' + _i + '').text();
                    if (_i == 1) {
                        _mainDataAll = _mainDataRow;
                    }
                    else {
                        _mainDataAll = _mainDataAll + '_' + _mainDataRow;
                    }

                }
                // set the value in hidden field
                $('#hdnAllGridData').val(_mainDataAll);
            }

            // Step 25. on change of chkToday or chkNextDay, set the urgent rate
            var chkUrgentChangeHandler = function (event) {

                // find which checkbox clicked
                var _chkClicked = $(event.target).attr('id');

                // if flag is 37 its first one else second one
                var _urgRate = 0;
                var _dateOffset = '';
                var _curDate = $('#txtDate').val();
                var _dateToSet = '';

                // if its first checkbox
                if (_chkClicked == 'chkToday') {
                    if ($(event.target).is(':checked')) {
                        _urgRate = $('#urgentRateAndDateOffset1').val().split(':')[0];
                        _dateOffset = $('#urgentRateAndDateOffset1').val().split(':')[1];
                        $('#chkNextDay').attr('checked', false);
                    }
                    else {
                        _urgRate = '0';
                        _dateOffset = $('#hdnDefaultDateOffset').val();
                    }
                }

                // if its second checkbox
                else if (_chkClicked == 'chkNextDay') {
                    if ($(event.target).is(':checked')) {
                        _urgRate = $('#urgentRateAndDateOffset2').val().split(':')[0];
                        _dateOffset = $('#urgentRateAndDateOffset2').val().split(':')[1];
                        // $('#chkNextDay').prop("checked", true);
                        $('#chkToday').attr('checked', false);
                    }
                    else {
                        _urgRate = '0';
                        _dateOffset = $('#hdnDefaultDateOffset').val();
                    }
                }

                // set the values
                $('#hdnUrgentRateApplied').val(_urgRate);
                _dateToSet = makeTheDueDate('' + _curDate, '' + _dateOffset);
                $('#txtDueDate').val(_dateToSet);

            };

            // Step 25.A add the previous hander
            $('body').on('change.AttachedEvent', '#chkToday, #chkNextDay', chkUrgentChangeHandler);

            // Step 25.A.1 this will make the date in due date box
            function makeTheDueDate(argCurDate, argDayOffset) {
                var _dt = new Date(argCurDate);
                _dt.setDate(_dt.getDate() + parseInt(argDayOffset));
                var _dtstr = _dt + '';
                var _day = _dtstr.substring(8, 10)
                var _month = _dtstr.substring(4, 7)
                var _year = _dtstr.substring(11, 16)
                // var _monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                var _retValue = _day + ' ' + _month + ' ' + _year;
                return _retValue;
            }

            // Step 25.B.1 this will load the data for urgent rate and date offset
            // this will be called at load, so that it doesn't affect the check change perf
            // call once for 1st data, second for 2nd data
            $.ajax({
                url: '../AutoComplete.asmx/LoadUrgentRate',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                data: 'argFlag=37',
                cache: false,
                success: function (response) {
                    // don't set in the check field, set in first urgentRateAndDateOffset1
                    $('#urgentRateAndDateOffset1').val($(response).find('string').text());
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });

            // Step 25.B.2 this will load the data for urgent rate and date offset
            // this will be called at load, so that it doesn't affect the check change perf
            // call once for 1st data, second for 2nd data
            $.ajax({
                url: '../AutoComplete.asmx/LoadUrgentRate',
                type: 'POST',
                timeout: 20000,
                datatype: 'xml',
                data: 'argFlag=38',
                cache: false,
                success: function (response) {
                    // don't set in the check field, set in first urgentRateAndDateOffset2
                    $('#urgentRateAndDateOffset2').val($(response).find('string').text());
                },
                error: function (response) {
                    // alert('server returned ' + response);
                }
            });

            // Step 1.y.2
            // set the focus to qty
            //$('#txtCustomerName').focus();
        });
    
    </script>

    <script src="../JavaScript/modalPopup.js" type="text/javascript"></script>
    <asp:HiddenField ID="check" runat="server" Value="0" />

    <table class="BaseTableStyle" border="2" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table class="TableForHeader" border="0" cellpadding="0" cellspacing="0">
                    <tr valign="top">
                        <td width="100px">
                        </td>
                        <td align="center" valign="top">
                            <div style="font-family: 'Bauhaus 93'; font-size: 20px;">
                                <asp:Label ID="lblStoreName" runat="server" ForeColor="#6086ac"></asp:Label></div>
                        </td>
                        <td align="right" width="75px" nowrap="nowrap" height="10" valign="top" style="color: Black;
                            font-weight: bold">
                            Version : 1.4.6
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top" style="background-color: White; color: White; font-weight: bold;">
            <td align="left" style="background-color: White">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                             <asp:Menu ID="MainMenu" runat="server" StaticEnableDefaultPopOutImage="False" Font-Size="1.4em"
                                ForeColor="Black" Orientation="Horizontal" Font-Names="Arial Black" BackColor="White"
                                DynamicHorizontalOffset="2" StaticSubMenuIndent="10px">
                                <StaticMenuStyle BorderColor="#6086ac" BorderStyle="Solid" BorderWidth="0px" />
                                <StaticSelectedStyle BackColor="#6086ac" />
                                <StaticMenuItemStyle HorizontalPadding="5px" BorderColor="White" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Arial" Font-Size="Large" VerticalPadding="2px" />
                                <DynamicHoverStyle BackColor="#6086ac" BorderColor="#6086ac" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Arial" Font-Size="Large" ForeColor="Orange" />
                                <DynamicMenuStyle BackColor="#6086ac" />
                                <DynamicSelectedStyle BackColor="#6086ac" />
                                <DynamicMenuItemStyle BackColor="#6086ac" Font-Size="Large" ForeColor="White" VerticalPadding="2px"
                                    BorderColor="White" BorderWidth="1px" Font-Names="Arial" HorizontalPadding="5px" />
                                <Items>
                                </Items>
                                <StaticHoverStyle BorderColor="#6086ac" BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial"
                                    Font-Size="Large" ForeColor="White" BackColor="#6086ac" />
                            </asp:Menu>
                        </td>
                        <td align="right">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="font-weight: bolder; border-style: solid; border-width: 0px; border-color: #FFFFFF #006600 #006600 #006600;
                                        background-color: White; color: Black; font-family: Arial; font-size: small"
                                        align="right" nowrap="nowrap">
                                        <a href="../Help.html" target="_blank">Help</a>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnNewBooking" runat="server" Text="New (F1)"
                                            CausesValidation="false" OnClick="btnNewBooking_Click" />
                                        <asp:Button ID="btnEditBooking" runat="server" Text="Edit (F4)" OnClick="btnEditBooking_Click"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnF8" runat="server" Text="Save (F8)" OnClick="btnF8_Click"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnDelivery" runat="server" Text="Delivery (F6)"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnF2" runat="server" Text="F2-Advance Search" Style="display: none"
                                            OnClick="btnF2_Click" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="font-weight: bolder; border-style: solid; border-width: 0px; border-color: #FFFFFF #006600 #006600 #006600;
                                        background-color: White; color: Black; font-family: Arial; font-size: small"
                                        align="right" nowrap="nowrap">
                                        Welcome
                                        <%=CurrentUserName %>,&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" Text="Log Out"
                                            ForeColor="Red" Font-Bold="true" runat="server" OnClick="lnkLogOut_Click" CausesValidation="False" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="Container">
                                <div class="ToolBar">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                                    <table width="100%">
                                        <tr>
                                            <td align="center" colspan="15">
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" visible="false" id="tblSearch" runat="server">
                                        <tr>
                                            <td align="center" colspan="15" class="TableData">
                                                Booking Number :
                                                <asp:TextBox ID="txtEdit" runat="server" MaxLength="10" AutoCompleteType="None" AutoComplete="off"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtEdit_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtEdit">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Style="display: none"
                                                    CausesValidation="false" Text="Edit booking" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="15">
                                                <asp:Label ID="lblSearchError" runat="server" EnableViewState="false" CssClass="ErrorMessage"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" id="tblEntry" runat="server" style="height: 20px;">
                                        <tr class="mainRowTblEntry">
                                            <td nowrap="nowrap" class="TableData" width="20px">
                                                L. Invoice : 
                                            </td>
                                            <td nowrap="nowrap">
                                                <%--<asp:Timer runat="server" id="UpdateTimer" interval="20000" ontick="UpdateTimer_Tick"  />
                                                <asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional" >
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger controlid="UpdateTimer" eventname="Tick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                          <asp:Label ID="lblLastBooking" runat="server" Font-Bold="True" Font-Size="Small"
                                                    Font-Underline="True" ForeColor="Blue"></asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                                <asp:Label ID="lblLastBooking" runat="server" Font-Bold="True"
                                                    ForeColor="Blue"></asp:Label>
                                            </td>
                                            <td class="TableData" width="40px;">
                                                Date
                                            </td>                                           
                                            <td style="width: 100px;">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtDate" runat="server" Width="80px" MaxLength="11" ></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtDate" Format="dd MMM yyyy" > 
                                                        </cc1:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td nowrap="nowrap" class="TableData" style="width: 60px;">
                                                Due Date
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtDueDate" runat="server" Width="80px" MaxLength="11" ></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtDueDate" Format="dd MMM yyyy">
                                                        </cc1:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td class="TableData" style="width: 35px;">
                                                Time
                                            </td>
                                            <td style="width: 50;">
                                                <asp:TextBox ID="txtTime" runat="server" Width="49px" MaxLength="10" ></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtTime_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtTime" ValidChars="0123456789AMPamp: ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="TableData">
                                                <div>
                                                    <span id="lblCustomer">
                                                        Customer
                                                    </span>
                                                </div>
                                            </td>
                                            <td  >
                                                <table >
                                                    <tr >
                                                        <td >
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static"
                                                                        MaxLength="300" onfocus="javascript:select();"
                                                                       ></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName" EnableCaching="false"
                                                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="1"
                                                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                        CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                        >
                                                                    </cc1:AutoCompleteExtender>
                                                                    
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <cc1:HoverMenuExtender ID="CustHover" runat="server" TargetControlID="imgBtnCustomerAdd"
                                                                        PopupControlID="CustPanal1" PopupPosition="Bottom" OffsetX="6" PopDelay="25"
                                                                        HoverCssClass="popupHover">
                                                                    </cc1:HoverMenuExtender>
                                                                    <asp:Panel ID="CustPanal1" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                        <table style="font-size: smaller; color: Black">
                                                                            <tr>
                                                                                <td align="left" style="color: Black">
                                                                                    Add new customer.
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:Button ID="imgBtnCustomerAdd" runat="server" Style="border-radius: 7px; display: none;" Text="Add"
                                                                        Enabled="True"
                                                                        CausesValidation="False"/>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <span><asp:ImageButton ID="imgBtnCustAdd" runat="server" 
                                                    ImageUrl="~/images/icons.png" AlternateText="Add customer" ImageAlign="Right" /></span></td>
                                        </tr>
                                        
                                    </table>
                                    <table width="100%" id="tblEntry1" runat="server">
                                        <tr>
                                            <td colspan="10">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <table class="TableData">
                                                            <tr>
                                                                <td>
                                                                    Address :
                                                                </td>
                                                                <td style="width: 475px" nowrap="nowrap" id="td1" runat="server">
                                                                    <asp:Label ID="lblAddress" runat="server" Font-Bold="True" Width="250px" ClientIDMode="Static" CssClass="LabelBkgScrn"></asp:Label>
                                                                </td>                                                            
                                                               
                                                                <td>
                                                                    Priority :
                                                                </td>
                                                                <td style="width: 260px" nowrap="nowrap" id="td2" runat="server">
                                                                    <asp:Label ID="lblPriority"  ClientIDMode="Static" runat="server" Font-Bold="True" Width="60px" CssClass="LabelBkgScrn"></asp:Label>
                                                                </td>

                                                                <td style="width: 160px" nowrap="nowrap">
                                                                    <span><asp:CheckBox ID="chkToday" runat="server" /></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Remarks :
                                                                </td>
                                                                <td style="width: 470px" nowrap="nowrap">
                                                                    <asp:Label ID="lblRemarks" ClientIDMode="Static" runat="server" Font-Bold="True" Width="160px" CssClass="LabelBkgScrn"></asp:Label>
                                                                </td>
                                                               
                                                                
                                                                    <td>
                                                                Mob. No. :
                                                                </td>
                                                                <td style="width: 250px" nowrap="nowrap">
                                                                    <asp:Label ID="lblMobileNo" ClientIDMode="Static" runat="server" Font-Bold="True" Width="160px" CssClass="LabelBkgScrn"></asp:Label>
                                                                </td>

                                                                <td style="width: 160px" nowrap="nowrap">
                                                                    <span><asp:CheckBox ID="chkNextDay" runat="server" /></span>
                                                                </td>                                                                
                                                        </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    <table width="100%" id="tblEntry2" runat="server">
                                        <tr>
                                            <td colspan="15">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlEntry" runat="server" ScrollBars="Vertical" Height="200px">
                                                                <asp:GridView ID="grdEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                    CssClass="mGridEntry" GridLines="Both"
                                                                    OnRowDataBound="grdEntry_RowDataBound" >
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate >
                                                                                <table>
                                                                                    <tr class="mGridCustomHeader">
                                                                                        <td style="color: orange; border-width: 0px">
                                                                                            S.No.
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:Label ID="lblHSNo" runat="server" Text='<%# Bind("SNO") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:Label ID="lblSNO" runat="server" Text='<%# Bind("SNO") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="color: Orange; border-width: 0px" class="mGridCustomHeader">
                                                                                            Qty
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:TextBox ID="txtQty" runat="server" Width="25px" Text='<%# Bind("QTY") %>' MaxLength="4" ClientIDMode="Static"
                                                                                               ></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="txtQty_FilteredTextBoxExtender" runat="server" FilterType="Numbers" 
                                                                                                Enabled="True" TargetControlID="txtQty">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td align="left" style="border-width: 0px">
                                                                                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QTY") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ControlStyle-Width="240px" ItemStyle-Width="240px" HeaderStyle-Width="240px">
                                                                            <HeaderTemplate>
                                                                                <asp:UpdatePanel ID="upItemName" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <table width="inherit">
                                                                                            <tr class="mGridCustomHeader">
                                                                                                <td colspan="2" align="center" style="color: Orange; border-width: 0px">
                                                                                                    Item Name
                                                                                                </td>
                                                                                                <td colspan="2" align="center" style="color: Orange; border-width: 0px">
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" style="border-width: 0px">
                                                                                                    <asp:TextBox ID="txtName" runat="server" Width="200px" Text='<%# Bind("ITEMNAME") %>'
                                                                                                        CausesValidation="false" MaxLength="300" ClientIDMode="Static"></asp:TextBox>&nbsp;&nbsp;
                                                                                                    
                                                                                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtName" TargetControlID="txtName"  EnableCaching="false"
                                                                                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                                                                                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                                                        CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                </td>
                                                                                                <td style="border-width: 0px">
                                                                                                <asp:ImageButton ID="imgBtnAddNewItem" runat="server" ImageUrl="~/images/icons.png" AlternateText="Add customer" style="width:20px; position:relative;" ClientIDMode="Static"/>
                                                                                                </td>
                                                                                                <td style="border-width: 0px">
                                                                                                    <cc1:HoverMenuExtender ID="ItemHover" runat="server" TargetControlID="imgBtnItemAdd"
                                                                                                        PopupControlID="ItemPanal" PopupPosition="Bottom" OffsetX="6" PopDelay="25" HoverCssClass="popupHover">
                                                                                                    </cc1:HoverMenuExtender>
                                                                                                    <asp:Panel ID="ItemPanal" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                                                        <table style="font-size: smaller; color: Black">
                                                                                                            <tr>
                                                                                                                <td align="left" style="color: Black">
                                                                                                                    Add new item When you are not found in the list.
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                    <asp:Button ID="imgBtnItemAdd" runat="server" Text="Add" CausesValidation="false"
                                                                                                        Style="border-radius: 7px; display: none;" 
                                                                                                        Enabled="True" 
                                                                                                       />
                                                                                                </td>
                                                                                                <td style="border-width: 0px">
                                                                                                    <cc1:HoverMenuExtender ID="RemarkHoverPopup" runat="server" TargetControlID="btnAddRAndC"
                                                                                                        PopupControlID="RemarkPopUp" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                                        HoverCssClass="popupHover">
                                                                                                    </cc1:HoverMenuExtender>
                                                                                                    <asp:Panel ID="RemarkPopUp" runat="server" Height="50px" Width="125px" CssClass="popupMenu">
                                                                                                        <table style="font-size: smaller;">
                                                                                                            <tr>
                                                                                                                <td align="left" style="color: Black">
                                                                                                                    There you can add remarks.
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                    <asp:Button ID="btnAddRAndC" runat="server" CausesValidation="false"
                                                                                                        Text="Remarks" ToolTip="" Style="border-radius: 7px; display: none" 
                                                                                                       Enabled="True"
                                                                                                        />
                                                                                                </td>
                                                                                                
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td align="left" style="border-width: 0px">
                                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ITEMNAME") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ControlStyle-Width="240px" ItemStyle-Width="240px" HeaderStyle-Width="240px">
                                                                            <HeaderTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="color: Orange; border-width: 0px" class="mGridCustomHeader">
                                                                                            Description
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px; text-align: left">
                                                                                            <asp:TextBox ID="txtRemarks" runat="server"
                                                                                                 style="display:none;"></asp:TextBox>
                                                                                            <input name="tags" id="mySingleField" style="display:none; width: 80px;"/>
                                                                                            <ul id="mytags" class="ul.Tagit">
                                                                                            </ul>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td align="left" style="border-width: 0px">
                                                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>' ForeColor="Black" 
                                                                                             CssClass="grdCustName" ></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ControlStyle-Width="240px" ItemStyle-Width="240px" HeaderStyle-Width="240px">
                                                                            <HeaderTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="color: Orange; border-width: 0px" class="mGridCustomHeader">
                                                                                            Color
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px; text-align: left">
                                                                                            <asp:TextBox ID="txtColor" runat="server" Width="120px"
                                                                                                 style="display:none;"></asp:TextBox>
                                                                                            <input name="tags" id="mySingleFieldColor" style="display:none;"/>
                                                                                            <ul id="mytagsColor">
                                                                                            </ul>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td align="left" style="border-width: 0px">
                                                                                            <asp:Label ID="lblColor" runat="server" Text='<%# Bind("COLOR") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ControlStyle-Width="180px" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                                            <HeaderTemplate>
                                                                                <asp:UpdatePanel ID="upProcess" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <table>
                                                                                            <tr class="mGridCustomHeader">
                                                                                                <td colspan="3" align="center" style="color: Orange; border-width: 0px">
                                                                                                    Process and Rate
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" style="border-width: 0px">
                                                                                                    <asp:TextBox ID="txtProcess" runat="server" Width="50px" Text='<%# Bind("PROCESS") %>' ClientIDMode="Static"
                                                                                                        CausesValidation="false" MaxLength="3"></asp:TextBox>
                                                                                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtProcess" TargetControlID="txtProcess"  EnableCaching="false"
                                                                                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1"
                                                                                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                                                        CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                                                                        >
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                </td>
                                                                                                <td style="color: Black; border-width: 0px">
                                                                                                    @
                                                                                                </td>
                                                                                                <td style="border-width: 0px">
                                                                                                    <asp:TextBox ID="txtRate" runat="server" Width="60px" Text='<%# Bind("RATE") %>' ClientIDMode="Static"
                                                                                                        Style="text-align: right" MaxLength="6" CssClass="clsRate">0</asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="txtRate_FilteredTextBoxExtender" runat="server"
                                                                                                        Enabled="True" TargetControlID="txtRate" FilterType="Custom" ValidChars=".0123456789">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </td>
                                                                                               
                                                                                                <td style="border-width: 0px;">
                                                                                                    <%--<cc1:HoverMenuExtender ID="ExtraHoverPopup" runat="server" TargetControlID="btnExtra"
                                                                                                        PopupControlID="PanelPopUp" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                                        HoverCssClass="popupHover">
                                                                                                    </cc1:HoverMenuExtender>
                                                                                                    <asp:Panel ID="PanelPopUp" runat="server" Height="50px" Width="125px" CssClass="popupMenu">
                                                                                                        <table style="font-size: smaller; color: Black">
                                                                                                            <tr>
                                                                                                                <td align="left" style="color: Black">
                                                                                                                    Extra process add extra processing for individual clothes.
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                    <asp:Button ID="btnExtra" runat="server" Text=" Ext Pr " CausesValidation="false"
                                                                                                        ToolTip="" Style="border-radius: 7px;" 
                                                                                                       Enabled="True"
                                                                                                         />--%>
                                                                                                    <asp:ImageButton ID="imgBtnAddMoreProcess" runat="server" ImageUrl="~/images/icons.png" AlternateText="Add More Process" style="width:20px; position:relative;" ClientIDMode="Static"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td align="left" style="border-width: 0px">
                                                                                            <asp:Label ID="lblProcess" runat="server" Text='<%# Bind("PROCESS") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        
                                                                        
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                                            <HeaderTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="color: Orange; border-width: 0px" class="mGridCustomHeader">
                                                                                            Amount
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:Label ID="lblHAmount" runat="server" Text='<%# Bind("AMOUNT") %>' Style="text-align: right"
                                                                                                ForeColor="Black">0</asp:Label>
                                                                                        </td>
                                                                                        <td style="border-width: 0px">
                                                                                            <cc1:HoverMenuExtender ID="AddPopup" runat="server" TargetControlID="imgBtnGridEntry" 
                                                                                                PopupControlID="AddButtonPopUp" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                                HoverCssClass="popupHover">
                                                                                            </cc1:HoverMenuExtender>
                                                                                            <asp:Panel ID="AddButtonPopUp" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                                                <table style="font-size: smaller; color: Black">
                                                                                                    <tr>
                                                                                                        <td align="left" style="color: Black">
                                                                                                            To add or update record click here.
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </asp:Panel>
                                                                                            <asp:Button ID="imgBtnGridEntry" runat="server" CausesValidation="false" ToolTip=""
                                                                                                Text="Add" Style="border-radius: 7px;"
                                                                                                Enabled="True"
                                                                                               />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("AMOUNT") %>' ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:ImageButton ID="imgbtnEdit" CommandName="EDITItemDetails" runat="server" ImageUrl="~/images/EditInformationHS.png"
                                                                                                CausesValidation="false"
                                                                                                TabIndex="22" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Click to edit the item details in list." 
                                                                                                Data="value"/>
                                                                                        </td>
                                                                                        <td style="border-width: 0px">
                                                                                            <asp:ImageButton ID="imgbtnDeleteItemDetails" CommandName="DeleteItemDetails" runat="server"
                                                                                                CausesValidation="false" ImageUrl="~/images/Delete.gif" Text="Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                                Data="deleteMe"
                                                                                                ToolTip="Click to delete the item details in list." />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" colspan="7" style="font-size: large; color: Black">
                                                Qty :&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCount" runat="server" Text="0" ForeColor="Black"
                                                    Font-Size="Large"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" class="TableData" colspan="7">
                                            <div id="lowerRowLeft" class="lowerLeftTd">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                    <span>Home Delivery : Ctrl+H</span>
                                                        <table class="TDCaption GridLowerLeft" style="border-style: solid;">
                                                            <%--<tr>
                                                                <td style="color: Black">
                                                                    <asp:Label runat="server" ID="lblToday"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkToday" runat="server" TextAlign="Left"  OnCheckedChanged="chkToday_CheckedChanged" />
                                                                </td>
                                                                <td style="color: Black">
                                                                    <asp:Label runat="server" ID="lblNextDay"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkNextDay" runat="server" TextAlign="Left" 
                                                                        OnCheckedChanged="chkNextDay_CheckedChanged" />
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td style="color: Black">
                                                                    Home Delivery :
                                                                </td>
                                                                <td align="left" style="width: 10px;">
                                                                    <asp:CheckBox ID="chkHD" runat="server" TextAlign="Left" />
                                                                </td>
                                                                <td style="width: 5px !important;" align="left">
                                                                    &nbsp;&nbsp;&nbsp;SMS :
                                                                </td>
                                                                <td align="left" style="width: 10px;">
                                                                    <asp:CheckBox ID="chkSendsms" runat="server" TextAlign="Left" onclick="toggleDropDownList(this);" />
                                                                </td>
                                                                <td style="color: Black; width: 20px;" align="left">
                                                                    &nbsp;&nbsp;&nbsp;Template :
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="drpsmstemplate" runat="server" Enabled="false" Width="80px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption" style="color: Black">
                                                                    Notes :
                                                                </td>
                                                                <td align="left" colspan="5">
                                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption" style="color: Black">
                                                                    Checked By :
                                                                </td>
                                                                <td align="left" colspan="5">
                                                                    <asp:DropDownList ID="drpCheckedBy" runat="server" Width="250px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr><td></td><td></td></tr>
                                                            <tr><td></td><td></td></tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            </td>                                            
                                            <td colspan="2" class="BookingBtns">
                                            <div id="lowerRowCenter" class="lowerCenterTd">
                                                <asp:Label ID="lblSave" CssClass="SuccessMessage" runat="server" EnableViewState="false" />
                                                <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                                <br />
                                                <br />

                                                <table id="tblBtnSavings">
                                                    <tr>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="save">
                                                                    <span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnSaveBooking" runat="server" OnClick="btnSaveBooking_Click" CausesValidation="false"
                                                                            Enabled="True" />
                                                                        (F8)</span></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnPrintBookingContainder">
                                                                <div id="print">
                                                                    <span class="btnImage"></span><span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnSavePrint" runat="server" CausesValidation="false" Enabled="True"
                                                                            OnClick="btnSavePrint_Click" ClientIDMode="Static" />
                                                                        (F9)</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSavePrintPrintTagsBookingContainder">
                                                                <div id="saveprinttag">
                                                                    <span class="btnImage"></span><span class="btnImage"></span><span class="btnImage">
                                                                    </span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnSavePrintBarCode" runat="server" OnClick="btnSavePrintBarCode_Click"
                                                                            CausesValidation="false" Enabled="True" />
                                                                        (F10)</span></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSavePrintTagsBookingContainder">
                                                                <div id="printtag">
                                                                    <span class="btnImage"></span><span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnPrintBarCode" runat="server" OnClick="btnPrintBarCode_Click" CausesValidation="false"
                                                                            Enabled="True" />
                                                                        (F12)</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <%--       <tr>
                                                        <td class="btnSaveBooking" colspan="2" align="center">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="reset">
                                                                    <span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnReset" runat="server" CausesValidation="false" Enabled="True"
                                                                            OnClick="btnReset_Click" />
                                                                        (F12)</span></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                                </div>
                                            </td>
                                            <td align="right">
                                            <div id="divCalc" class="lowerRightTd">
                                                <asp:UpdatePanel ID="updcheckservice" runat="server">
                                                    <ContentTemplate>
                                                    <span class="TableData calcGrid">Discount : Ctrl+D</span>
                                                        <table class="TDCaption calcGrid" style="border-style: solid;">
                                                            <tr>
                                                                <td style="color: Black">
                                                                    G. Total :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCurrentDue" runat="server" Width="167px" Style="text-align: right; font-weight:bold;" ClientIDMode="Static"
                                                                       ></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: Red">
                                                                    Discount :
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdrPercentage" runat="server" Checked="True"
                                                                        GroupName="a" Text="  %" />
                                                                    <asp:RadioButton ID="rdrAmt" runat="server" GroupName="a"
                                                                        Text=" Flat" />
                                                                    <asp:DropDownList ID="drpDiscountOption" runat="server"
                                                                        Visible="False">
                                                                        <asp:ListItem Selected="True" Text="In Percentage" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="In Amount" Value="1"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtDiscount" runat="server" Width="35px" Style="text-align: right"
                                                                         ClientIDMode="Static" MaxLength="2"></asp:TextBox>
                                                                    <cc1:TextBoxWatermarkExtender ID="txtDiscount_TextBoxWatermarkExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtDiscount" WatermarkText="0">
                                                                    </cc1:TextBoxWatermarkExtender>
                                                                    <asp:Label ID="lblDisAmt" runat="server" ForeColor="Red"></asp:Label>
                                                                    <cc1:FilteredTextBoxExtender ID="txtDiscount_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtDiscount" ValidChars=".0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:TextBox ID="txtDiscountAmt" runat="server" Width="38px" MaxLength="6" Style="text-align: right; display: none"
                                                                        ClientIDMode="Static"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtDiscountAmt_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtDiscountAmt" FilterType="Custom" ValidChars=".0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: Black">
                                                                    Tax :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSrTax" runat="server" Width="167px" Style="text-align: right; font-weight:bold;"
                                                                        ></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: Black">
                                                                    Total :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTotal" runat="server" Width="167px" Style="text-align: right; font-weight:bold;"
                                                                        ></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: Green">
                                                                    Advance :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAdvance" runat="server" Width="167px" Style="text-align: right"
                                                                        MaxLength="6"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtAdvance_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtAdvance">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: Black">
                                                                    Balance :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBalance" runat="server" Width="167px" Style="text-align: right; font-weight: bold; font-size: larger; color: #FF0000"
                                                                        ></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </div>
                            </div>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender7" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="plnAddProcess" Drag="true" TargetControlID="btnPopUpProcess">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnPopUpProcess" runat="server" Style="display: none" Text="a" CausesValidation="false"
                               />
                            <asp:Panel ID="plnAddProcess" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="450PX">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="PopupHeader">
                                            <div class="TitlebarLeft">
                                                Add Process
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td colspan="3" align="left">
                                                        <asp:Label ID="lblPrcErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                                        <asp:Label ID="lblPrcSucess" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Process Code &nbsp;:
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtProcessCode" runat="server" MaxLength="4" CssClass="Textbox"
                                                            TabIndex="1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 5Px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Process Name :
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox TabIndex="2" ID="txtProcessName" MaxLength="20" runat="server" CssClass="Textbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 10Px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnProcessSave" runat="server" Text="Save" 
                                                            CausesValidation="false" TabIndex="3" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <asp:Button ID="BtnItem" runat="server" Text="Button" Style="display: none" />
                            <asp:Panel ID="pnlItem" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="500PX">
                                <asp:UpdatePanel ID="upMakeingItem" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div3">
                                            <div class="TitlebarLeft">
                                                Add Items
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TableData">
                                                <tr>
                                                    <td width="150px" align="right">
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                                        <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table class="TableData">
                                                            <tr>
                                                                <td class="TDCaption">
                                                                    Item Name :
                                                                </td>
                                                                <td class="TDDot" nowrap="nowrap">
                                                                    <asp:TextBox ID="txtItemName" runat="server" MaxLength="50" TabIndex="1" Width="200px"
                                                                        CssClass="Textbox"></asp:TextBox>
                                                                    </td>
                                                                    <cc1:FilteredTextBoxExtender ID="txtItemName_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtItemName" ValidChars=" abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <span class="span"></span>
                                                                </td>
                                                                <td width="100%">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption">
                                                                    Sub Items :
                                                                </td>
                                                                <td align="left" style="margin-left: 160px">
                                                                    <asp:TextBox ID="txtItemSubQty" runat="server" MaxLength="2" TabIndex="2" Width="50px"
                                                                        ToolTip="No. of sub items for Item" >1</asp:TextBox>
                                                                   <asp:TextBox ID="txtNewItemName" runat="server" TabIndex="3" ClientIDMode="Static"
                                                                       style="display: none;" CssClass="Textbox"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtItemSubQty_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtItemSubQty" ValidChars="0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td rowspan="3">
                                                                    <asp:ListBox ID="lstSubItem" runat="server" CssClass="Textbox" ClientIDMode="Static"
                                                                        TabIndex="4"  style="display: none;"
                                                                        Width="150px"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption">
                                                                    Item Code :
                                                                </td>
                                                                <td class="TDDot" nowrap="nowrap">
                                                                    <asp:TextBox ID="txtItemCode" runat="server" Width="50Px" CssClass="Textbox" ToolTip="Please enter item code"
                                                                        TabIndex="5" MaxLength="4"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtItemCode_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtItemCode" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <table style="width: 400px;">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnItemSave" CausesValidation="false" runat="server" Text="Save"
                                                                       OnClientClick="return checkName();" TabIndex="6" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span><asp:HiddenField
                                                                ID="hdntemp" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnItemCode" runat="server" Value="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <%--<cc1:ModalPopupExtender ID="ModalPopupExtender4" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="pnlItem" Drag="true" TargetControlID="btnAddItem">
                            </cc1:ModalPopupExtender>--%>
                            <asp:Button ID="btnAddItem" runat="server" Text="ReloadGrid" Style="display: none"
                                OnClick="btnAddItem_Click" />
                            <asp:SqlDataSource ID="SqlDataProcessTypes" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                SelectCommand="SELECT ProcessCode, ProcessName FROM ProcessMaster Order By ProcessName">
                            </asp:SqlDataSource>
                            <asp:Button ID="btnSearch" runat="server" Text="ReloadGrid" Style="display: none"
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="btnCustomerSearch" runat="server" Text="ReloadGrid" Style="display: none"
                                CausesValidation="false" />
                            <cc1:ModalPopupExtender ID="ModalPopupExtender5" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="pnlCustomerSearch" Drag="true"
                                TargetControlID="btnCustomerSearch">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pnlCustomerSearch" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="750PX">
                                <div class="popup_Titlebar" id="Div4">
                                    <div class="TitlebarLeft">
                                        Search Customer
                                    </div>
                                </div>
                                <div class="popup_Body">
                                    <table class="TDCaption">
                                        <tr>
                                            <td align="center">
                                                Customer Name
                                            </td>
                                            <td align="center">
                                                Address
                                            </td>
                                            <td align="center">
                                                Mobile No
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtCNameSearch" runat="server" Width="150" onfocus="javascript:select();"
                                                            TabIndex="1" Text="" ></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%--onfocus="javascript:select();" ontextchanged="txtCustomerName_TextChanged"--%>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Width="220" onfocus="javascript:select();"
                                                            TabIndex="2" ></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtPhoneNo" runat="server" Width="80"
                                                            TabIndex="3" ></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:UpdatePanel ID="upCustSearch" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="250" Width="730">
                                                            <asp:GridView ID="grdCustomerSearch" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                                                ToolTip="List of found customers. Click on the link to select the customer."
                                                                Width="710" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                                                                CellPadding="4" ForeColor="Black" GridLines="Vertical" OnRowCommand="grdCustomerSearch_OnRowCommand">
                                                                <RowStyle BackColor="#F7F7DE" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Name" ShowHeader="False" HeaderStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerName" runat="server" CausesValidation="false" TabIndex="4"
                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustName") %>'></asp:LinkButton>
                                                                            <asp:HiddenField ID="lnkBtnCustomerCode" runat="server" Value='<%# Eval("CustomerCode") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Address" ShowHeader="False" HeaderStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerAddress" runat="server" CausesValidation="false"
                                                                                TabIndex="5" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustAddress") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Mobile" ShowHeader="False" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerMobile" runat="server" CausesValidation="false"
                                                                                TabIndex="6" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustMobile") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks" ShowHeader="False" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerRemark" runat="server" CausesValidation="false"
                                                                                TabIndex="7" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustRemarks") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Priority" ShowHeader="False" HeaderStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerPriority" runat="server" CausesValidation="false"
                                                                                TabIndex="8" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustPriority") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle VerticalAlign="Top" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#CCCC99" />
                                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                                                <AlternatingRowStyle BackColor="White" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Button ID="btnHideCustSearch" CausesValidation="false" runat="server" Text="Cust Search"
                                                            Style="display: none" OnClick="btnHideCustSearch_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender8" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="plnAddExtraProcess" Drag="true"
                                TargetControlID="btnAddExtraProcess">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnAddExtraProcess" runat="server" Text="ReloadGrid" CausesValidation="false"
                                Style="display: none" />
                            <asp:Panel ID="plnAddExtraProcess" runat="server" CssClass="modalPopup" Style="display: none;" ClientIDMode="Static"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="450PX">
                                <asp:UpdatePanel ID="upAddExtraProcess" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div6">
                                            <div class="TitlebarLeft">
                                                Add Extra Process
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td align="left">
                                                        Second Process
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraProcess1" runat="server" Width="100"
                                                            MaxLength="3"  ClientIDMode="Static"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtExtraProcess1" TargetControlID="txtExtraProcess1"  EnableCaching="false"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        Rate
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraRate1" runat="server" ClientIDMode="Static" Width="100" MaxLength="6" Style="text-align: right" CssClass="clsRate">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="txtExtraRate1_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtExtraRate1">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Third Process
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraProcess2" runat="server" Width="100" ClientIDMode="Static"
                                                            MaxLength="3" ></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtExtraProcess2"  EnableCaching="false"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        Rate
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraRate2" runat="server"  ClientIDMode="Static" Width="100" MaxLength="6" Style="text-align: right" CssClass="clsRate">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="txtExtraRate2_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtExtraRate2">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnExtraProcSave" Style="border-radius: 7px;" Text="Save" 
                                                           
                                                            CausesValidation="false" 
                                                            runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="center">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="Remarks_ModalPopup" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="plnRemarks" Drag="true" TargetControlID="btnAddRemarks">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnAddRemarks" runat="server" Text="ReloadGrid" CausesValidation="false"
                                Style="display: none" OnClick="btnAddRemarks_Click" />
                            <asp:Panel ID="plnRemarks" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="450PX">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div1">
                                            <div class="TitlebarLeft">
                                                Add Remarks
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td align="left">
                                                        Remarks :&nbsp;
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtWholeRemark" runat="server" Width="200" MaxLength="200"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="autoRemarks" TargetControlID="txtWholeRemark"  EnableCaching="false"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemRemarks" MinimumPrefixLength="1"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>                   
                    <tr>
                        <td>
                            
                            <%--<asp:Button ID="btnCustAdd" runat="server" Text="a" Style="display: none" OnClick="btnCustAdd_Click" />--%>
                            <asp:Panel ID="pnlNewCustomer" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="500PX">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div7">
                                            <div class="TitlebarLeft">
                                                Add Customer
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Name
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drpTitle" runat="server" TabIndex="1">
                                                            <asp:ListItem Value=" " Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="Mr"></asp:ListItem>
                                                            <asp:ListItem Value="Mrs"></asp:ListItem>
                                                            <asp:ListItem Value="Ms"></asp:ListItem>
                                                            <asp:ListItem Value="Dr"></asp:ListItem>
                                                            <asp:ListItem Value="M/S"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtCName" runat="server" Width="200px" TabIndex="2" MaxLength="50"></asp:TextBox>&nbsp;<span
                                                            class="span">*</span>
                                                        <%--<asp:RequiredFieldValidator ID="RQName" runat="server" ControlToValidate="txtCName"
                                                            Display="None" ErrorMessage="Please enter customer name." SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Address
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCAddress" runat="server" Width="245" TabIndex="3" MaxLength="100"></asp:TextBox>&nbsp;<span
                                                            class="span">*</span>
                                                        <%--<asp:RequiredFieldValidator ID="RQAddress" runat="server" ControlToValidate="txtCAddress"
                                                            Display="None" ErrorMessage="Please enter address." SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Mobile
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMobile" runat="server" Width="245" TabIndex="4"></asp:TextBox>&nbsp;
                                                        <cc1:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtMobile" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Priority
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <%--<asp:DropDownList ID="drpPriority" runat="server" Width="250" TabIndex="5">
                                                        </asp:DropDownList>
                                                        <input id="btnShowNewPriority" onclick="javascript: var val=document.getElementById('btnShowNewPriority').value;if(val=='Add'){document.getElementById('divNewPriority').style.visibility='Visible';document.getElementById('<%= txtNewPriority.ClientID %>').focus(); document.getElementById('btnShowNewPriority').value='Close';} else {document.getElementById('divNewPriority').style.visibility='Hidden';document.getElementById('btnShowNewPriority').value='Add';document.getElementById('<%= drpPriority.ClientID %>').focus();}"
                                                            tabindex="6" size="2" type="button" value="Add" style="background-color: #CCCCCC;
                                                            font-weight: bold; font-family: Tahoma" />--%>
                                                            <asp:TextBox ID="txtPriority" runat="server" ClientIDMode="Static" Width="245px"  TabIndex="5"
                                                                MaxLength="300" onfocus="javascript:select();"
                                                                ></asp:TextBox>
                                                            <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender2" TargetControlID="txtPriority" EnableCaching="false"
                                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetPriorityList" MinimumPrefixLength="1"
                                                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                >
                                                            </cc1:AutoCompleteExtender>
                                                            <asp:HiddenField ID="txtPriorityCode" runat="server" />

                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <div id="divNewPriority" style="visibility: hidden;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <input id="txtNewPriority" maxlength="50" runat="server" size="37" type="text" style=""
                                                                            tabindex="7" />
                                                                    </td>
                                                                    <td>
                                                                        <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" TargetControlID="btnAddNewPriority"
                                                                            PopupControlID="Panel3" PopupPosition="Bottom" OffsetX="6" PopDelay="25" HoverCssClass="popupHover">
                                                                        </cc1:HoverMenuExtender>
                                                                        <asp:Panel ID="Panel3" runat="server" Height="50px" Width="80px" CssClass="popupMenu">
                                                                            <table style="font-size: smaller; color: Black">
                                                                                <tr>
                                                                                    <td align="left" style="color: Black">
                                                                                        To add priority
                                                                                        <br />
                                                                                        click here.
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Button ID="btnAddNewPriority" Style="border-radius: 7px;" Text="Add" 
                                                                             Enabled="True" 
                                                                  CausesValidation="false" OnClientClick="return checkNewPriorityBox();"
                                                                            TabIndex="11" runat="server" OnClick="btnAddNewPriority_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td nowrap="nowrap">
                                                        Area / Location
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAreaLocaton" runat="server" Width="245" TabIndex="6" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Remarks
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemarks1" runat="server" TabIndex="7" Width="245" MaxLength="100"></asp:TextBox>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                            ShowSummary="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblCustSave" CssClass="SuccessMessage" runat="server" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:Button ID="btnOkay" Text="Save" runat="server"  TabIndex="12" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG3%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <asp:HiddenField ID="hdnMAC" runat="server" />
                    <asp:HiddenField ID="hdnEditItemId" runat="server" Value="-1" />
                    <asp:HiddenField ID="hdnOption" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnUpdate" runat="server" Value="0" />
                    <asp:HiddenField ID="BranchId" runat="server" Value="0" />
                </table>
            </td>
        </tr>
    </table>
    
    <asp:Panel ID="pnlConfirmDate" runat="server" CssClass="modalPopup" Style="display: none;" ClientIDMode="Static"
BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="250PX">
        <div class="popup_Titlebar" id="Div2">
            <div class="TitlebarLeft">
               Confirm Delivery Date
            </div>
        </div>
        <div class="popup_Body">
            <table class="TDCaption deliveryDate">
                <tr>
                    <td>
               <asp:DropDownList ID="drpDate" runat="server" AutoPostBack="false">
               </asp:DropDownList>
                        </td>
                    <td style="width:10px;"></td>
                    <td>
                        <asp:DropDownList ID="drpMonth" runat="server">
                            <asp:ListItem Text="Jan" Value="Jan"></asp:ListItem>
                            <asp:ListItem Text="Feb" Value="Feb"></asp:ListItem>
                            <asp:ListItem Text="Mar" Value="Mar"></asp:ListItem>
                            <asp:ListItem Text="Apr" Value="Apr"></asp:ListItem>
                            <asp:ListItem Text="May" Value="May"></asp:ListItem>
                            <asp:ListItem Text="Jun" Value="Jun"></asp:ListItem>
                            <asp:ListItem Text="Jul" Value="Jul"></asp:ListItem>
                            <asp:ListItem Text="Aug" Value="Aug"></asp:ListItem>
                            <asp:ListItem Text="Sep" Value="Sep"></asp:ListItem>
                            <asp:ListItem Text="Oct" Value="Oct"></asp:ListItem>
                            <asp:ListItem Text="Nov" Value="Nov"></asp:ListItem>
                            <asp:ListItem Text="Dec" Value="Dec"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:10px;"></td>
                     <td>
                        <asp:DropDownList ID="drpYear" runat="server">
                            
                        </asp:DropDownList>
                    </td>
                     <td style="width:10px;"></td>
                </tr>
        
                <tr>
                    
                    <td align="center" colspan="6">
                        <asp:Button ID="btnConfirmDate" Style="border-radius: 7px;" Text="Confirm" 
                                                           
                            CausesValidation="false" 
                            runat="server" />
                    </td>
                   
                </tr>

            </table>
        </div>
        </asp:Panel>

    <asp:HiddenField ID="hdnItems" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataValues" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnFirstAdd" runat="server" Value="NA" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCustomerNameFocusOut" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrentValue" runat="server" Value="1" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrentGrossAmt" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="isInEditMode" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDefaultItem" runat="server" Value="NA" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDefaultProcess" runat="server" Value="NA" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDefaultItemProcessRate" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDefaultQty" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTaxBefore" runat="server" Value="true" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllDiscount" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllTax" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnUrgentRateApplied" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllDiscountFocusOut" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="isNetAmountInDecimal" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRecompTax" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRecompDiscount" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCustCode" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTaxAmtRecomp" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDisAmtRecomp" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBalance" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTotal" runat="server" Value="0" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDiscountValue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDiscountPerc" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnGrdRowCount" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllGridData" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="urgentRateAndDateOffset1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="urgentRateAndDateOffset2" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDefaultDateOffset" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCustChanged" runat="server" ClientIDMode="Static" Value="dummy"/>
    <asp:HiddenField ID="hdnItemNameChanged" runat="server" ClientIDMode="Static" Value="dummy"/>
    <asp:HiddenField ID="totalDisWithActive" runat="server" ClientIDMode="Static" Value="dummy"/>
    <asp:HiddenField ID="totalTaxWithActive" runat="server" ClientIDMode="Static" Value="dummy"/>
    <asp:HiddenField ID="hdnAllCusts" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllItems" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllPrcs" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAllPriorities" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnIsTaxExclusive" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnThreeTaxRates" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnColorEnabled" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDescEnabled" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBindDesc" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBindColor" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDummy" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDummyColor" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTaxableAmt" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTaxType" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnConfirmDelivery" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="hdnButtonClicked" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="hdnSpanClicked" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="hdnPrevYear" runat="server" ClientIDMode="Static" />
     <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <asp:Label ID="hdnValues" Text="c++,java,php,coldfusion,javascript,asp,ruby,python,c" ClientIDMode="Static"
            runat="server" />
            <asp:Label ID="LblValuesColor" runat="server" style="display: none;" ClientIDMode="Static"></asp:Label>
    </asp:PlaceHolder>
    </form>
   
</body>
</html>
