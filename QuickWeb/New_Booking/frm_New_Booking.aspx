<%@ Page Language="C#" AutoEventWireup="true" Inherits="QuickWeb.New_Booking.frmBookingScreen" EnableEventValidation="false" ValidateRequest="false"
    CodeBehind="frm_New_Booking.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="Shortcut Icon" type="image/ico" href="../images/favicon.ico" />

    <title>
        <%=AppTitle %></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/BookingTour.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-tour.min.css" rel="stylesheet" type="text/css" />
    <%--<script src="../JavaScript/code.js" type="text/javascript"></script>--%>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>`
    <script type="text/javascript" src="../js/tag-it.js"></script>
    <%--<script src="../js/newXHR.js" type="text/javascript"></script>
    <script src="../js/TPL.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.23.custom.css" />
    <link href="../css/jquery.tagit.css" rel="stylesheet" type="text/css" />
    <link href="../css/tagit.ui-zendesk.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery-blink.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/bootstrap-tour.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
    </style>
    <style type="text/css">
        .AutoExtender { z-index : 1000 !important; }
    </style>
    <script language="javascript" type="text/javascript">
        //function to disable browser back button
        function DisableBackButton() {
            window.history.forward();
        }
        setTimeout("DisableBackButton()", 0);
        window.onunload = function () { null };
</script>
<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $('.blink').blink(); // default is 500ms blink interval.
        //$('.blink').blink({delay:100}); // causes a 100ms blink interval.
    });
 
</script>
    <script language="javascript" type="text/javascript">

        function SetColor(colorName) {
            var count = document.getElementById('grdEntry_ctl01_txtColor').value;
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
        function setContextKey(sender, args) {
            sender.set_contextKey(document.getElementById('drpSearchCustBy').value);
            // alert(sender.get_contextKey());
        }

        function setMe(sender, args) {
            setTimeout(function () {
            console.log($.active);
            if ($.active === 0)
            txtCustomerNameKeyupHandler();
            }, 40);
        }

        function sel_ItemName(sender, args) {
            /*setTimeout(function () {
            if ($.active === 0) {
            var e2 = jQuery.event;
            e2.target = $('#txtName');
            txtNameKeyUpHanlder(e2);
            }
            }, 5);*/
        }

        function set_prc(sender, args) {
            /*setTimeout(function () {
            if ($.active === 0) {
            var e2 = jQuery.event;
            e2.target = sender._element;
            txtPrckeyupHandler(e2);
            }
            }, 5);*/
        }

    </script>
    <script src="../js/BkScrFunctions.js" type="text/javascript"></script>
</head>
<body onload="ShowReturnMsg()" class="frmBkScr">
    <form id="form1" runat="server">
    <script type="text/javascript">
        var _tempdis;
        $(document).ready(function () {

            // taking the grid caluclations in account
            $('#txtCurrentDue').attr('disabled', true);
            $('#txtSrTax').attr('disabled', true);
            $('#txtTotal').attr('disabled', true);
            $('#txtBalance').attr('disabled', true);
            //$('#txtDueDate_CalendarExtender_popupDiv').css('z-index', 2000);
            $('#txtDueDate_CalendarExtender_popupDiv').on('ready', function (event) { $('#CalendarExtender1_popupDiv').css('z-index', 200); });
            //$('#txtDueDate_CalendarExtender_popupDiv').on('show.AttachedEvent', function (event) { $('#CalendarExtender1_popupDiv').css('z-index', 200); });
            $('body').keydown(function (event) {
                // open the delivery screen
                if (event.which == 117) {
                    window.location = "../Bookings/Delivery.aspx";
                    return false;
                }
                // open the delivery screen
                if (event.which == 112) {
                    $('#btnNewBooking').click();
                    return false;
                }
                // open the delivery screen
                if (event.which == 115) {
                    document.getElementById('btnEditBooking').click();
                    return false;
                }
                if (event.which == 113) {
                    document.getElementById('btnF2').click();
                    return false;
                }

                var _idToCheck = $(event.target).attr('id');
                if ((event.which == 13 || event.which == 9) && !event.shiftKey && _idToCheck == 'txtLen') {
                    if ($(this).val() == '') {
                        $(this).val('1');
                    }
                    $('#txtBreadth').focus();
                    return false;
                }
                else if ((event.which == 13 || event.which == 9) && !event.shiftKey && _idToCheck == 'txtBreadth') {
                    //savehandlderforlb();
                    if ($(this).val() == '') {
                        $(this).val('1');
                    }
                    $('#pnlLB').dialog('close');
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtBreadth') {
                    $('#txtLen').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtLen') {
                    return false;
                }
                /**********/
                /**** WARNING! REGEX AHEAD ****/
                if ((event.which == 13 || event.which == 9) && !event.shiftKey && _idToCheck == 'txtNumPanels') {
                    /* check for wrong input */
                    if ($('#txtNumPanels').val() == '') {
                        $('#txtNumPanels').val('1');
                    }
                    var regex = /^\d*\.?\d$/;
                    if (!regex.test($('#txtNumPanels').val())) {
                        alert('Invalid Value');
                        $('#txtNumPanels').focus();
                        return false;
                    }
                    $('#pnlPanel').dialog('close');
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtNumPanels') {
                    return false;
                }
                else if ((event.which == 9 || event.which == 13) && (_idToCheck == 'txtPwdForIRD') && event.shiftKey) {
                    return false;
                }
                else if ((event.which == 9 || event.which == 13) && (_idToCheck == 'txtPwdForIRD')) {
                    $('#btnAcceptPwd').click();
                    return;
                }
                /*********/
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    if (_idToCheck == 'txtRate' && event.which == 9) {
                        // change here
                        $('#grdEntry_ctl01_imgBtnGridEntry').focus();
                    }
                    else if (_idToCheck == 'txtEdit' && event.which == 13) {
                        $('#btnEdit').click();
                    }
                    // if id is imgBtnAddMoreProcess focus on add
                    else if (_idToCheck == 'imgBtnAddMoreProcess' && event.which == 9) {
                        $('#grdEntry_ctl01_imgBtnGridEntry').focus();
                    }
                    else if (_idToCheck == 'drpCommLbl') {
                        $('#lblMobileNo').focus();
                    }
                    /*else if (_idToCheck == 'lblMobileNo') {
                    $('#lblEmailId').focus();
                    }*/
                    else if (_idToCheck == 'lblEmailId') {
                        $('#txtQty').focus();
                    }
                    // if tab is pressed on any of the key where we haven't introduced anything, than just return true
                    if (event.which == 9 && (_idToCheck == 'txtDate' || _idToCheck == 'txtDueDate' || _idToCheck == 'txtTime' || _idToCheck == 'rdrPercentage' || _idToCheck == 'rdrAmt' || _idToCheck == 'drpHD'
                            || _idToCheck == 'chkSendsms' || _idToCheck == 'chkEmail' || _idToCheck == 'txtRemarks' || _idToCheck == 'txtWorkshopNotes' || _idToCheck == 'drpCheckedBy' || _idToCheck == 'drpsmstemplate'
                            || _idToCheck == 'drpSearchCustBy'
                            || _idToCheck == 'grdEntry_ctl01_imgBtnGridEntry' || _idToCheck == 'btnSaveBooking' || _idToCheck == 'btnSavePrint'
                            || _idToCheck == 'btnSavePrintBarCode' || _idToCheck == 'btnPrintBarCode')) {
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
                    //$('#imgBtnAddMoreProcess').focus();
                    $('#txtRate').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'imgBtnAddMoreProcess') {
                    $('#txtRate').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtRate') {
                    $('#txtProcess').focus();
                    return false;
                }
                else if (event.which == 9 && !event.shiftKey && _idToCheck == 'txtLen') {
                    $('#txtBreadth').focus();
                    return false;
                }
                else if (event.which == 9 && event.shiftKey && _idToCheck == 'txtProcess') {
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
                    _tempdis = $('#txtDiscount').val();
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
                    return false;
                }
                // Ctrl + H for Home Delivery
                else if ((event.which == 72 || event.which == 104) && event.ctrlKey) {
                    // check if home delivery is already checked
                    $("#drpHD").css("background-color", "yellow");
                    if ($('#drpHD').is(':checked')) {
                        // uncheck it
                        // $('#drpHD').click();
                        $('#drpHD').focus();
                        // $('#txtQty').focus();
                    }
                    else {
                        // $('#drpHD').click();
                        $('#drpHD').focus();
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
                    else if ($('#pnlLB').dialog('isOpen') == true) {
                        $('#pnlLB').dialog('close');
                    }
                    else if ($('#pnlPanel').dialog('isOpen') == true) {
                        $('#pnlPanel').dialog('close');
                    }
                    else if ($('#pnlPwd').dialog('isOpen') == true) {
                        $('#pnlPwd').dialog('close');
                        $('#' + $('#hdnPrvPwdFocus').val()).focus();
                        document.getElementById('lblWrongPwd').textContent = '';
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
            $('#txtDate, #txtDueDate').focus(function (event) {
                //$('#CalendarExtender1_popupDiv').on('show.AttachedEvent', function (event) { $('#CalendarExtender1_popupDiv').css('z-index', 200); });
                //$('#imgBtnAddNewItem').css('z-index', -1);
                //                $('#txtDueDate_CalendarExtender_popupDiv').css('z-index', 200);
            }).blur(function (event) {
                $('#imgBtnAddNewItem').css('z-index', 0);
            });

            $('body').on('dialogclose', '#pnlPwd', function (e) { /* if (e.data != '') */{ $('#btnAcceptPwd').off('click'); document.getElementById('txtPwdForIRD').value = ''; } });

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

            $('#txtDate').on('change', function (e) {
                $('#hdnCustCode').val() && GetPackageDetails(hdnCustCode.value);
            });

            // Step 0.z.1 call the above function
            checkDescAndColorForBinding();

            checkDeliveryDateForConfirmation();

            // Step 0.z.4 call the above func
            CheckIfBindColorToQty();

            checkForEditRemarks();

            LoadDefaultItemProcessAndRate();

            // Call that function to set the default value
            setTheDefaults();
            // set focus to customer name
            $('#txtCustomerName').focus();

            LoadTaxBeforeOrAfter();

            LoadAllThreeTaxes();

            LoadIsTaxExclusive();

            // E. Set a urgent booking rate, default to 0, onCheckChanged set the value, so that on check change 
            // we don't have to call different method for calculation, just using 0% as default is same as
            // no extra charge, when checkchanged, then set the value and calcuation runs in same method fine
            if (isInEditMode.value !== 'true') {
                $('#hdnUrgentRateApplied').val('0');
            }
            // F. Load the priorities in case user adds customers, even if not called here directly, might be called at some other time
            function LoadPriorities() {
            };
            // G. make the list(s) taggalge
            // make remarks taggable
            makeLiTaggable('mytags', 'mySingleField', 'hdnValues', '', $('#hdnBindDesc').val(), true, true, -1);
            // make color taggable
            makeLiTaggable('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', '', $('#hdnBindColor').val(), true, true, $('#hdnBindColorToQty').val());

            LoadIsNetAmountInDecimal();

            // H.3 call these function
            loadAllDis();
            loadAllTax();

            // H.4 this load all the default items
            // so that we don't have to check when user presses enter again
            // H.4.1 this loads all items
            LoadAllItems();


            // H.5 this load all the default processes
            // so that we don't have to check when user presses enter again
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
                recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), false, true, -1);
                // Step NEW 5.A.9 this will recalulate all the grid, if flat discount is on,
                // because we need to find the perctange of flat based on total amt
                if ($('#txtDiscountAmt').is(':visible')) {
                    // recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), true, true);
                    if (parseFloat($('#txtDiscountAmt').val()) > parseFloat($('#txtCurrentDue').val())) {
                        alert('discount amount can\'t be greater than gross amount!');
                        $('#txtDiscountAmt').focus();
                        return false;
                    }
                    // Step 20.A
                    // make the percentage
                    var _disPerc = 100 * parseFloat($('#txtDiscountAmt').val()) / parseFloat($('#txtCurrentDue').val());
                    if (isNaN(_disPerc)) {
                        _disPerc = '0';
                    }
                    _disPerc = _disPerc;
                    // set this perctage to text amount


                    $('#txtDiscount').val(_disPerc);
                    // set the label to current value
                    $('#lblDisAmt').text(parseFloat($('#txtDiscountAmt').val()).toFixed(2));
                    $('#hdnDiscountValue').text($('#txtDiscountAmt').val());
                    // set the hidden recomp value to this
                    $('#hdnDisAmtRecomp').val($('#txtDiscountAmt').val());
                    recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), true, true, -1);
                }
                // findAllQtyOfItems();
                showHideDescColor(1, $('#grdEntry > tbody > tr').size(), true, true);

                GetTheCustDetails(document.getElementById('hdnCustCode').value);

                GetPackageDetails(document.getElementById('hdnCustCode').value);
                AddEditingRemarksNode();
                $('#lblLastBooking').text($('#txtEdit').val().toUpperCase());
                $('#lblLastBooking').parent().prev().text('Invoice #');
            }
            else {
                // nothing just set the default discount type
                // only setting here, because we don't wanna interrupt the uesr with changing the discount when he is editing
                setDefaultDiscountType();
                $('#txtCustomerName').focus();
            }

            // Step 1.V. call the function below
            checkIfDesAndColorEnabled();

            // Step 0x0001
            // This loads the password(s) if any
            LoadThePassWords();

            LoadDefaultSearch();

            // Step 1.V.3. this fucntion returns the values that will be used to resize,
            // not hard coding it, cause vivek sir might ask to change
            function theGridResetValues() {
                var _valToReturn =
                                    {
                                }
            }
            // Step 1.V.4 this is the function that resizes the grid

            // Step 1.W this will change the background color

            // Step 1.W.1 this wil set the default discount type when not in editing mode


            if ($('#isInEditMode').val() != 'true') {
                checkSMS();
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
                        $('#pnlNewCustomer').dialog({ width: 520, height: 450, modal: true });
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

            /*    ADDED CODE    */
            // Step 2. on keydown at txtcustomername
            $('#txtCustomerName').keydown(function (event) {
                // if enter add one 
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {

                    $('#txtCustomerName').one('keyup.AttachedEvent', txtCustomerNameKeyupHandler);
                }
            });

            // Step 2.A.1 add funtion to select customer on mousedown, delegating event because the control doensn't exists the first time
            $('#UpdatePanel3').on('mousedown.AttachedEvent', $('#autoComplete1_completionListElem'), function (e) {

                if (e.which === 1 && (e.target.id !== 'drpSearchCustBy' && e.target.className === 'AutoExtenderHighlight')) {
                    console.log(e);
                    setTimeout(function () { txtCustomerNameKeyupHandler(); }, 10);
                }
            });

            /*    ADDED CODE    */
            // Step 2.A.2 if user clicked the litte "+" icon to add open the pop up of customer panel
            $('#imgBtnCustAdd').click(function (event) {
                $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                $('#txtCAddress').focus();
                $('#pnlNewCustomer').dialog({ width: 520, height: 450, modal: true });
                return false;
            });
            // Step 2.A.2.1 on the keydown, if the key is enter
            $('#imgBtnCustAdd').keydown(function (event) {
                if (event.which == 13) {

                    $('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                    $('#txtCAddress').focus();
                    $('#pnlNewCustomer').dialog({ width: 520, height: 450, modal: true });
                    return false;
                }
            });
            // Step 2.A.3 HEY MAC! U KNOW WHAT? USE CSS FOR THIS YOU PIECE OF LAZY SHIT!!!! 
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
                type: 'GET',
                timeout: 20000,
                datatype: 'JSON',
                contentType: 'appliaction/json; charset=utf8',
                cache: false,
                data: "arg='cust1'",
                success: function (response) {
                    var _tmpResult = response.d;
                    // the values is in form of name, address, mobile, priority and remark, and discount
                    var _tmpResultAry = _tmpResult.split(':');
                },
                error: function (response) {
                }
            });

            // Step 2.B Attach the close event handler
            var pnlNewCustomerKeydownHandler = function (event) {
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
                /*
                if ($('#txtCName').val() == '') {
                $('#txtCName').focus();
                }
                else {
                // set the focus to customer address
                $('#txtCAddress').focus();
                }
                $('#txtCName').click();
                */
                var _itemToSet, _itemToFocus, _itemToSetValue = txtCustomerName.value;
                switch (drpSearchCustBy.value) {
                    case 'All':
                    case 'Name': _itemToSet = 'txtCName';
                        if (_itemToSetValue) _itemToFocus = 'txtCAddress'; else _itemToFocus = 'txtCName';
                        break;
                    case 'Address': _itemToSet = 'txtCAddress';
                        if (_itemToSetValue) _itemToFocus = 'txtCName'; else _itemToFocus = 'txtCAddress';
                        break;
                    case 'Mobile': _itemToSet = 'txtMobile';
                        if (_itemToSetValue) _itemToFocus = 'txtCName'; else _itemToFocus = 'txtMobile';
                        break;
                    case 'Email': _itemToSet = 'txtEmail';
                        if (_itemToSetValue) _itemToFocus = 'txtCName'; else _itemToFocus = 'txtEmail';
                        break;
                    case 'MembershipId': _itemToSet = 'txtCName';
                        if (_itemToSetValue) _itemToFocus = 'txtCName'; else _itemToFocus = 'txtCName';
                        break;
                    case 'CustCode': _itemToSet = 'txtCName';
                        if (_itemToSetValue) _itemToFocus = 'txtCName'; else _itemToFocus = 'txtCName';
                        break;
                }
                $('#' + _itemToSet).val(_itemToSetValue);
                $('#' + _itemToFocus).focus();
            });

            $('#drpSearchCustBy').change(function (e) {
                txtCustomerName.focus();
                if (this.value === 'Mobile') {
                    $('#FTBdrpSearchCustBy').attr('Enable', true);
                    txtCustomerName.value = '';
                    $('#txtCustomerName').on('keypress.Mobile', function (ee) {
                        var test = String.fromCharCode(ee.charCode).replace(/^0+/ig, '');
                        return ((isFinite(Math.log(test))) || (test === '') || (ee.keyCode === 8) || (ee.keyCode === 46) || (ee.keyCode === 35) || (ee.keyCode === 36) || (ee.keyCode === 37) || (ee.keyCode === 39));
                    });
                } else {
                    $('#txtCustomerName').off('keypress.Mobile');
                    $('#FTBdrpSearchCustBy').attr('Enable', false);
                }
            });

            // Call the next function
            findPriorityCode();

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
                    datatype: 'JSON',
                    contentType: 'application/json; charset=utf8',
                    cache: false,
                    data: "{arg: '" + $('#txtNewPriority').val() + "'}",
                    success: function (response) {
                        var result = response.d;
                        if (result == 'Record Saved') {
                            LoadPriorities();
                            $('#txtNewPriority').hide();
                        }
                    },
                    error: function (response) {
                    }
                });
            });

            // Step 2.H.1 this is handler that makes the addcusotmer dialog box work for enter key
            // on all the fields on add customer dialog box, add the keyevent
            $('body').on('keydown.AttachedEvent', '#drpTitle, #txtCName, #txtCAddress, #txtMobile, #txtAreaLocaton, #ddlRateListNewCustomer, #txtPriority, #txtDefaultDiscount, #drpCommunicationMeans, #txtEmail, #txtRemarks1', function (event) {
                // if enter key is pressed
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    if ($(event.target).attr('id') != 'txtRemarks1' && $(event.target).attr('id') != 'drpTitle' && $(event.target).attr('id') != 'txtMobile') {
                        // find the previous tr
                        if ($(event.target).attr('id') == 'txtPriority') {
                            $('#drpCommunicationMeans').focus();
                        }
                        var _nextInput = $(event.target).closest('tr').next('tr').find('td:eq(2) input:eq(0)');
                        // focus on next input
                        if (_nextInput.size() == 0) {
                            $(event.target).closest('tr').next('tr').find('td:eq(2) select').focus();
                        }
                        else {
                            $(_nextInput).focus();
                        }
                    }
                    else if ($(event.target).attr('id') == 'drpTitle') {
                        $('#txtCName').focus();
                        return;
                    }
                    else if ($(event.target).attr('id') == 'txtMobile') {
                        var _mobNo;
                        if ($('#txtMobile').val() == '') {
                            hdnDummyManyMobUnq.value = 'true';
                            $('#txtAreaLocaton').focus();
                            return;
                        }

                        var _unq = $.ajax({
                            url: '../Autocomplete.asmx/IsMobileUnique',
                            type: 'GET',
                            contentType: 'application/json; charset=utf8',
                            dataType: 'JSON',
                            data: "mobileNo='" + $('#txtMobile').val() + "'",
                            async: false,
                            timeout: 2000,
                            success: function (res) { /*if (res.d) return false; else return true; */ },
                            error: function (res) { alert('some error occurred when checking mobile no'); }
                        }), chained = _unq.then(function (data) {
                            if (data.d == false) {
                                alert('Mobile no already in use!'); $('#txtMobile').focus(); $('#btnOkay').attr('disabled', true);
                            }
                            else {
                                hdnDummyManyMobUnq.value = 'true';
                                $('#txtAreaLocaton').focus();
                                $('#btnOkay').attr('disabled', false);
                            }
                            return data.d;
                        });
                    }
                    else {
                        // its the last one, the txtAdate, all the btnOkay
                        // check here for address and remark
                        if (event.which == 9) {
                            event.which = 13;
                        }
                        if (!checkIfCustomerIsValid(event)) {
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

            // Step 3. On selecting process, find discount applicable tax applicable
            // rather find the default rate associated with given process and item
            $('#txtProcess, #txtExtraProcess1, #txtExtraProcess2').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $(event.target).one('keyup.AttachedEvent', txtPrckeyupHandler);
                    if (event.target.id == 'txtProcess') {
                        if ($('#hdnIsTour').val() == "1") { BookingTour(5); }  //for focus on Process
                    }
                }
            });

            // do the same for mouse
            //grdEntry_ctl01_autoComplete1_txtName_completionListElem
            $('#grdEntry_ctl01_upProcess ,#upAddExtraProcess').on('mousedown.AttachedEvent', $('#grdEntry_ctl01_autoComplete1_txtProcess_completionListElem, #AutoCompleteExtender1_completionListElem, #autoComplete1_txtExtraProcess1_completionListElem'), function (e) {
                if (e.which !== 1) {
                    return;
                }
                switch (e.delegateTarget.id) {
                    case 'grdEntry_ctl01_upProcess':
                        e.target = txtProcess;
                        break;
                    case 'upAddExtraProcess':
                        // extraProcess1 or 2
                        // e.target = logic ???
                        return; // return now, will deal later!
                }

                setTimeout(function () {
                    // Yeah bit*h! This is closure! ever used one of these?
                    (function (argEvent) { txtPrckeyupHandler(argEvent) })(e);
                    // using this from instead of (function() { function(arg) }(e)) (as used in checkForPassword!) becuase jQuery uses this form, and feels more natural
                }, 10);

            });

            // Step 3.A This will open up the dialog box for adding extraprocess
            $('body').on('click.AttachedEvent', '#imgBtnAddMoreProcess', function (event) {
                $('#plnAddExtraProcess').dialog({ width: 520, modal: true });
                $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
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
                $('#grdEntry').unblock({ fadeOut: 0 });
            });

            // Step 3.A.3 on keydown on rate move next
            $('body').on('keydown.AttachedEvent', '#txtExtraRate1', function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtExtraProcess2').focus();
                    return false;
                }
            });

            // change handler for drop down of communication means
            drpCommLbl.onchange = function (e) {
                switch (this.value) {
                    case 'NA': { chkSendsms.checked = false; chkEmail.checked = false; } break;
                    case 'SMS': { chkSendsms.checked = true; chkEmail.checked = false; } break;
                    case 'Email': { chkSendsms.checked = false; chkEmail.checked = true; } break;
                    case 'Both': { chkSendsms.checked = true; chkEmail.checked = true; } break;
                    default: { chkSendsms.checked = false; chkEmail.checked = false; } break;
                }
            }

            //mobile change handler
            $('#lblMobileNo').on('keydown', function (e) {
                if ((e.which === $.ui.keyCode.ENTER || e.which === $.ui.keyCode.TAB) && !e.shiftKey) {
                    if (this.value !== '') CheckMobileUnique();
                    else {
                        hdnDummyManyMobUnq.value = 'true';
                        lblEmailId.focus();
                    }
                    return false;
                }
                else if ((e.which === $.ui.keyCode.ENTER || e.which === $.ui.keyCode.TAB) && e.shiftKey) {
                    drpCommLbl.focus();
                    return false;
                }

            });

            // Step 5. For gridEntry add event handler for add
            /***************************************************/
            /********    THE MAJOR METHOD 1  *******************/
            var imgBtnGridEntryClickHandler = function (e) {
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

                //                if (e.isTrigger === undefined) {
                //                    // if the rate is changed
                //                    if (($('#txtRate').val() != $('#hdnPrvRate').val()) || ($('#txtExtraRate1').val() != $('#hdnPrvRate1').val()) || ($('#txtExtraRate2').val() != $('#hdnPrvRate2').val())) {
                //                        if (document.getElementById('txtPwdForIRD').value != $('#hdnPwdItemRateDis').val().split(':')[1] && document.getElementById('hdnPwdItemRateDis').value !== '') {
                //                            checkForPassword('Rate');
                //                            return false;
                //                        }
                //                    }
                //                }

                // disable the button
                $('#grdEntry_ctl01_imgBtnGridEntry').attr('disabled', true);
                $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
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
                    // disable rate list id
                    $('#ddlRateList').attr('disabled', true);
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
                        ComputeRowDisTaxAmt(_insPrc, _insRate, _insPrc1, _insRate1, _insPrc2, _insRate2, _insQty, false, -1, false, false);
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
                        //$('#txtQty').focus();
                        // Step 5.A.8
                        // save the rates to database
                        saveNewRatesToDataBase(_insItemName, _insPrc, _insRate, _insPrc1, _insRate1, _insPrc2, _insRate2);
                        // Step NEW 5.A.9 this will recalulate all the grid, if flat discount is on,
                        // because we need to find the perctange of flat based on total amt
                        if ($('#txtDiscountAmt').is(':visible')) {
                            recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), true, true, parseInt($('#grdEntry > tbody > tr').size()) - (1));
                        }
                        SetUnitAndValue(-1, -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
                        SetUnitAndValue($('#hdnPrevUnit').val(), -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
                        // it item contains panel or has dimenstions, show em
                        var _itmUnit = $('#hdnItemUnit').val().split('_');
                        _itmUnit = _itmUnit[_itmUnit.length - 1];
                        var _itmUnitValue = $('#hdnItemUnitValue').val().split('_');
                        _itmUnitValue = _itmUnitValue[_itmUnitValue.length - 1];
                        if (_itmUnit == 'LB') {
                            $('#grdEntry').find('#ItemName_' + _insCurRowNumber + '').text(_insItemName + ' ' + _itmUnitValue.split(':')[0]);
                        }
                        else if (_itmUnit == 'P') {
                            $('#grdEntry').find('#ItemName_' + _insCurRowNumber + '').text(_insItemName + ' ' + _itmUnitValue.split(':')[1] + 'PL');
                        }
                        setNewTxtValues();
                        setTheDefaults();
                        $('#grdEntry').children().children('tr').length === 2 && txtCustomerName.value !== '' && GetTheCustDetails(hdnCustCode.value);
                    });
                    if ($('#hdnIsTour').val() == "1") { BookingTour(7); } // For focus on Add button
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
                    var _upAllCurDisTax = ComputeRowDisTaxAmt(_upPrc, _upRate, _upPrc1, _upRate1, _upPrc2, _upRate2, _upQty, true, _currentRow, false, false);
                    var _upCDis = _upAllCurDisTax.dis;
                    var _upCTax = _upAllCurDisTax.tax;
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
                    recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), false, false, _currentRow);

                    // Step 5.B.12
                    // save the rates to database
                    saveNewRatesToDataBase(_upItemName, _upPrc, _upRate, _upPrc1, _upRate1, _upPrc2, _upRate2);
                    // Step NEW 5.B.13 this will recalulate all the grid, if flat discount is on,
                    // because we need to find the perctange of flat based on total amt
                    if ($('#txtDiscountAmt').is(':visible')) {
                        recomputeAllGrid(1, $('#grdEntry > tbody > tr').size(), true, true, _currentRow);
                    }
                    SetUnitAndValue(-1, -1, _upSNo, false);
                    SetUnitAndValue($('#hdnPrevUnit').val(), -1, _upSNo, false);
                    // it item contains panel or has dimenstions, show em
                    var _itmUnit = $('#hdnItemUnit').val().split('_');
                    _itmUnit = _itmUnit[_currentRow - 1];
                    var _itmUnitValue = $('#hdnItemUnitValue').val().split('_');
                    _itmUnitValue = _itmUnitValue[_currentRow - 1];
                    if (_itmUnit == 'LB') {
                        $('#grdEntry').find('#ItemName_' + _currentRow + '').text(_upItemName + ' ' + _itmUnitValue.split(':')[0]);
                    }
                    else if (_itmUnit == 'P') {
                        $('#grdEntry').find('#ItemName_' + _currentRow + '').text(_upItemName + ' ' + _itmUnitValue.split(':')[1] + 'PL');
                    }
                    $('#grdEntry > tbody > tr:eq(' + _rowToRemoveHighLight + ')').css('background-color', 'white');
                    setNewTxtValues();
                    // block lower grid
                    var isPackageCdn = isPackageCondition();
                    !isPackageCdn && $('#txtDiscountAmt').attr('disabled', false);
                    !isPackageCdn && $('#txtDiscount').attr('disabled', false);
                    !isPackageCdn && $('#txtAdvance').attr('disabled', false);
                    $('#txtCustomerName').attr('disabled', false);
                    $('#grdEntry').children().children('tr').length === 2 && txtCustomerName.value !== '' && GetTheCustDetails(hdnCustCode.value);
                    // Step 5.B.11. set the defaults
                    setTheDefaults();
                }
                return false;
            };

            // Step 5.C Attach the event handler for add/update button
            $('body').on('click.AttachedEvent', '#grdEntry_ctl01_imgBtnGridEntry', imgBtnGridEntryClickHandler);
            // Step 5.C.1 Attach the event handler for add/update button on keydown enter
            $('body').on('keydown.AttachedEvent', '#grdEntry_ctl01_imgBtnGridEntry',
                function (event) {
                    if (event.which == 13) {
                        $('#grdEntry_ctl01_imgBtnGridEntry').click();
                    }
                    else if (event.which == 9) {
                        return true;
                    }
                    return false;
                });

            $('form').submit(function (e) {
                console.log(e);
                //return false;
            });

            // Step 6. Event Hanlder for edit button
            // using data='value' because the id of edit button will be changed
            // and each button will have a different id
            // so the only option is to use a class or a cutom attrib
            $('body').delegate('[Data="value"]', 'click', function () {
                $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
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
                // if ItemName contains PL (for panel) or X (for len X bth) then trim the name
                /* var _patt1 = / \d+PL$/gm; */
                /**** WARNING! REGEX AHEAD ****/
                var _patt1 = /\s\d*(?:[.]\d{1})?PL$/gm;
                var _patt2 = / \d+(?:\.\d+)?X\d+(?:\.\d+)?$/gm;
                // if it contains any one of them
                if (_patt1.test(_edtItemName)) {
                    // it contains the _PL
                    var _idx = _edtItemName.lastIndexOf(' ');
                    _edtItemName = _edtItemName.substring(0, _idx);
                }
                else if (_patt2.test(_edtItemName)) {
                    var _idx = _edtItemName.lastIndexOf(' ');
                    _edtItemName = _edtItemName.substring(0, _idx);
                }
                console.log(_edtItemName + ' was item and panels are ' + _edtItemName);
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

                ///<urgent case editing>///
                if (($('#chkNextDay').attr('checked') === 'checked') || ($('#chkToday').attr('checked') === 'checked')) {
                    // there is already urgency, set the rates
                    var _allEditedNonUrgentRate = FindNonUrgentRates(_edtPrc, _edtRate, _edtPrc1, _edtRate1, _edtPrc2, _edtRate2, _edtQty, hdnUrgentRateApplied.value, undefined);
                    _edtRate = _allEditedNonUrgentRate.rt;
                    _edtRate1 = _allEditedNonUrgentRate.rt1;
                    _edtRate2 = _allEditedNonUrgentRate.rt2;
                }

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
                // make the percentage
                makeThePercentage();
                // set the amount, previous value - current row value
                $('#txtCurrentDue').val(parseFloat($('#txtCurrentDue').val()) - parseFloat(_edtCurrentEditingAmt).toFixed(2));


                //$('#txtCurrentDue').text(parseFloat($('#txtCurrentDue').val()) - parseFloat(_edtCurrentEditingAmt).toFixed(2));
                // calcuate the discount and tax for this row, subtract it from the previous discount
                var _allCurDisTax = ComputeRowDisTaxAmt(_edtPrc, _edtRate, _edtPrc1, _edtRate1, _edtPrc2, _edtRate2, _edtQty, true, _rowNumberContainingString, false, false);
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
                $('#grdEntry').unblock({ fadeOut: 0 });
                SetUnitAndValue(-1, -1, _rowNumberContainingString, true);
                setNewTxtValues();
                // block lower grid
                $('#txtDiscountAmt').attr('disabled', true);
                $('#txtDiscount').attr('disabled', true);
                $('#txtAdvance').attr('disabled', true);
                $('#txtCustomerName').attr('disabled', true);
                return false;
            });

            // Step 7. Event handler for delete button
            // this is the handler for delete button
            $('body').delegate('[Data="deleteMe"]', 'click', function (event) {
                // check if rows pending to be updated
                if ($('#grdEntry_ctl01_imgBtnGridEntry').text() == 'Update') {
                    alert('There are rows pending to be updated');
                    return false;
                }
                var isPackageCdn = false;
                isPackageCdn = isPackageCondition();
                // just find the row, and delete it
                var _rowId = '' + $(event.target).attr('id') + '';
                _rowId = _rowId.split('_');
                _rowId = _rowId[1];
                var _totalRowCount = $('#grdEntry > tbody > tr').size();
                if (confirm("Are you sure you want to remove this record?") == false) { return false; };
                // Step 7.A set the hidden filed value that will represent the next id
                var _prvID = parseInt($('#hdnCurrentValue').val());
                $('#hdnCurrentValue').val(_prvID - 1);
                // Step 7.A.1 find the itemname and qty of current row,
                // so the update the qty label count
                var _delItemName = $('body').find('#grdEntry > tbody').find('#ItemName_' + _rowId + '').text() + '';
                /* var _patt1 = / \d+PL$/gm; */
                /**** WARNING! REGEX AHEAD ****/
                var _patt1 = /\s\d*(?:[.]\d{1})?PL$/gm;
                var _patt2 = / \d+(?:\.\d+)?X\d+(?:\.\d+)?$/gm;
                // the txtLen, txtBreadth, and txtNumPanels will not be the value what we might except, so let's do it!
                var _l = 1;
                var _b = 1;
                var _pl = 1;
                // if it contains any one of them
                if (_patt1.test(_delItemName)) {
                    // it contains the _PL
                    var _idx = _delItemName.lastIndexOf(' ');
                    // update index
                    _idx = parseInt(_idx) + parseInt(1);
                    // index of pl
                    var _idxPL = _delItemName.lastIndexOf('PL');
                    // the number of panels
                    _pl = _delItemName.substring(_idx, _idxPL);
                    // the item
                    _delItemName = _delItemName.substring(0, _idx - 1);
                }
                else if (_patt2.test(_delItemName)) {
                    var _idx = _delItemName.lastIndexOf(' ');
                    // update index
                    _idx = parseInt(_idx) + parseInt(1);
                    // the LXB string
                    var _idxLB = _delItemName.substring(_idx);
                    // index of 'X'
                    var _idxX = _idxLB.indexOf('X');
                    // update the 'X' (index)
                    _idxX = parseInt(_idxX) + parseInt(1);
                    // the length
                    _l = _idxLB.substring(0, _idxX - 1);
                    // the breadth
                    _b = _idxLB.substring(_idxX);
                    // the item
                    _delItemName = _delItemName.substring(0, _idx - 1);
                }
                console.log(_delItemName + ' was item and panels are ' + _pl);
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
                // set the LBP
                $('#txtLen').val(_l);
                $('#txtBreadth').val(_b);
                $('#txtNumPanels').val(_pl);
                // Step 7.B.4 find all amount
                var _delCurAllAmt = findRowCalc(_rowId);
                // Step 7.B.5 update the gross amount
                var _delAllAmtOfEntireGrid = parseFloat($('#txtCurrentDue').val()) - parseFloat(_delCurAllAmt);
                $('#txtCurrentDue').val(_delAllAmtOfEntireGrid.toFixed(2));
                //$('#txtCurrentDue').text(_delAllAmtOfEntireGrid.toFixed(2));
                // Step 7.B.3 calcuate the discount and tax for this row, subtract it from the previous discount
                var _delAllCurDisTax = ComputeRowDisTaxAmt(_delPrc, _delRate, _delPrc1, _delRate1, _delPrc2, _delRate2, _delQty, true, _rowId, false, true);
                var _delCDis = _delAllCurDisTax.dis;
                var _delCTax = _delAllCurDisTax.tax;
                // check if discount is in flat, and if so, check if its greater then total
                // if it is give a warning and return
                if ($('#txtDiscountAmt').is(':visible')) {
                    if ((parseFloat(_delAllAmtOfEntireGrid) < parseFloat($('#txtDiscountAmt').val())) && ($('#grdEntry > tbody > tr').size() > 2)) {
                        alert('Cannot remove this row, because discount would become greater then total amount.\n Please first change the discount then try to remove the row');
                        $('#hdnCurrentValue').val(_prvID);
                        var _mainAmt = parseFloat(_delAllAmtOfEntireGrid) + parseFloat(_delCurAllAmt);
                        $('#txtCurrentDue').val(_mainAmt.toFixed(2));
                        return false;
                    }
                }
                var _tmpTax = parseFloat($('#txtSrTax').val());
                _tmpTax = _tmpTax - _delCTax;
                // if tax is inclusive, set the tax to 0, cause the tax for current row might be greater (or may be anything) and it will
                // become negative
                // first check if it is package condition
                if (isPackageCdn) {
                    // if the tax type if inclusive, then hide
                    ($('#hdnPackageTaxPerItemPrice').val().split(':')[0] === 'INCLUSIVE') && (_tmpTax = 0);
                    // else if tax type is exclusive, don't do anythign
                }
                // if not pacakge and tax is inclusive
                else if ($('#hdnIsTaxExclusive').val() != 'true') {
                    _tmpTax = '0';
                }

                if (!isPackageCdn) {
                    // check if advance is in given, and if so, check if after remove the advance the net amount would become negative
                    // if it is give a warning and return
                    if ((parseFloat($('#txtCurrentDue').val()) + parseFloat($('#txtDiscountAmt').val()) + parseFloat(_tmpTax) - parseFloat($('#txtAdvance').val()) < 0) && ($('#grdEntry > tbody > tr').size() > 2)) {
                        alert('Cannot remove this row, because advance would become greater then net amount.\n Please first change the discount and/or advance then try to remove the row');
                        $('#hdnCurrentValue').val(_prvID);
                        var _mainAmt = parseFloat(_delAllAmtOfEntireGrid) + parseFloat(_delCurAllAmt);
                        $('#txtCurrentDue').val(_mainAmt.toFixed(2));
                        return false;
                    }
                }
                else {
                    // update the advance by remove that much amount from it as is the currentRowAmt

                }
                // set the amount in grdEntry_ctl01_lblHAmount
                $('#grdEntry_ctl01_lblHAmount').text(_delAllAmtOfEntireGrid.toFixed(2));
                // calculate the delete amount, in case its discount amount, and not percentage
                makeThePercentage();
                // Step 7.B.6 calculate the lower grid details
                calculateLowerGridDetails(_delCDis, _delCTax, false, true, false);
                // Step 7.C reove the row, the id would be
                // row size - _rowId +1
                var _rowToRemove = parseInt(_totalRowCount) - parseInt(_rowId);
                $('#grdEntry > tbody > tr:eq(' + _rowToRemove + ')').remove();
                // Step 7.B.6 reset all grid id(s)
                resetGridId('grdEntry', parseInt(_rowId) + 1, _totalRowCount);
                // calc the qty count
                setQtyInLabel(_delItemName, _delQty, false);
                // now update the advance if it is packageType
                if (isPackageCdn) {
                    $('#txtAdvance').val($('#txtTotal').val());
                }
                // focus in txtqty
                $('#txtQty').focus();
                // if all rows are removed, that is the count is one, set everything in lower grid 0
                if ($('#grdEntry > tbody > tr').size() == 1) {
                    $('#txtCurrentDue').val('0');
                    $('#txtDiscountAmt').val('0');
                    $('#txtDiscount').val('0');
                    $('#lblDisAmt').text('0');
                    $('#txtSrTax').val('0');
                    $('#txtTotal').val('0');
                    $('#txtAdvance').val('0').triggerHandler('change.package');
                    $('#txtBalance').val('0').triggerHandler('change.package');
                }
                SetUnitAndValue(-1, -1, _rowId, true);
                setNewTxtValues();
                // if tax is inclusive, recompute everything, because when checking this previously, also do the same if packageType and discount is inclusive
                // it might have set the taxable amt, that doesn't takes into accnt the removed row
                console.log('the taxable amt ' + $('#hdnTaxableAmt').val());
                if ($('#hdnIsTaxExclusive').val() != 'true' || (isPackageCdn && $('#hdnPackageTaxPerItemPrice').val().split(':')[0] === 'INCLUSIVE')) {
                    recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1);
                    console.log('in loop');
                    console.log('the taxable amt ' + $('#hdnTaxableAmt').val());
                }

                if ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Flat Qty') {
                    SetAdvanceTypeOfPackage();
                }

                return false;
            });


            // Step 7.Z Attacht the event handler for delete button
            // Step 8. Event handler for extra prc button
            // Step 8.Z Attach the event handler for the add extra prc button
            $('#grdEntry_ctl01_btnExtra').click(function (event) {
                $('#plnAddExtraProcess').dialog({ width: 520, modal: true });
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

            // Step 9.B.1 on the click of cancel button, clear the extra process and rate
            $('#RemovePrc2').click(function (event) {
                $('#txtExtraProcess1').val('');
                $('#txtExtraRate1').val('0');
            });

            // Step 9.B.2 on the click of cancel button, clear the extra process and rate
            $('#RemovePrc3').click(function (event) {
                $('#txtExtraProcess2').val('');
                $('#txtExtraRate2').val('0');
            });

            // Step 10.1 On pressing enter on qty, desc, and color, not on rate, because this will be used to add the row
            $('#txtQty, #mytags').on('keydown.AttachedEvent', function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    if ($(event.target).attr('id') == 'txtQty') {
                        if ($(event.target).val() == '0' || $(event.target).val() == '') {
                            $(event.target).val('1');
                        }
                        $('#txtName').focus();
                        var _tmpValAllowed = -1;
                        if ($('#hdnBindColorToQty').val() == 1) {
                            _tmpValAllowed = $('#txtQty').val();
                        }
                        makeLiTaggable('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', '', $('#hdnBindColor').val(), true, true, _tmpValAllowed);
                        if ($('#hdnIsTour').val() == "1") { BookingTour(1); }  //For focus on Item name
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
                            if ($('#hdnIsTour').val() == "1") { BookingTour(3); }  // Focus on color
                        }
                        else {
                            $('#txtProcess').focus();
                            if ($('#hdnIsTour').val() == "1") { BookingTour(4); }  // Focus on process
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
                        if ($('#hdnIsTour').val() == "1") { BookingTour(4); }   // Focus on Process
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
                setNewTxtValues();
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtName').one('keyup.AttachedEvent', txtNameKeyUpHanlder);
                    //itemLenBredth($(event.target).val());
                }
            });

            // do the same for mouse
            //grdEntry_ctl01_autoComplete1_txtName_completionListElem
            $('#grdEntry_ctl01_upItemName').on('mousedown.AttachedEvent', $('#grdEntry_ctl01_autoComplete1_txtName_completionListElem'), function (e) {
                if (e.which !== 1) {
                    return;
                }
                e.target = txtName;
                setTimeout(function () {
                    // Yeah bit*h! This is closure! ever used one of these?
                    (function (argEvent) { txtNameKeyUpHanlder(argEvent) })(e);
                    // using this from instead of (function() { function(arg) }(e)) (as used in checkForPassword!) becuase jQuery uses this form, and feels more natural
                }, 10);
            });

            // Step 10.A.3 open and close handler for lenght and bredth
            $('body').on('dialogopen.AttachedEvent', '#pnlLB', function (event) {
                // if txtName is empty set focus to it
                $('#txtLen').focus();
                //event.stopimmediatepropagation();
                return false;
            });

            // Step 10.A.4 close handler for add new item dialog box
            // on close set focus to prc
            $('body').on('dialogclose.AttachedEvent', '#pnlLB', function (event) {
                // just set the focus on txtItemCode
                if ($('#hdnDescEnabled').val() == 'True') {
                    $('#mytags').find('input').focus();
                }
                else if ($('#hdnColorEnabled').val() == 'True') {
                    $('#mytagsColor').find('input').focus();
                }
                else {
                    $('#txtProcess').focus();
                }
                //SetUnitAndValue(-1, -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
                return false;
            });

            // Step 10.A.3 open and close handler for lenght and bredth
            $('body').on('dialogopen.AttachedEvent', '#pnlPanel', function (event) {
                // if txtName is empty set focus to it
                $('#txtNumPanels').focus();
                //event.stopimmediatepropagationtxt
                return false;
            });
            // Step 10.A.4 close handler for add new item dialog box
            // on close set focus to prc
            $('body').on('dialogclose.AttachedEvent', '#pnlPanel', function (event) {
                // just set the focus on txtItemCode
                if ($('#hdnDescEnabled').val() == 'True') {
                    $('#mytags').find('input').focus();
                }
                else if ($('#hdnColorEnabled').val() == 'True') {
                    $('#mytagsColor').find('input').focus();
                }
                else {
                    $('#txtProcess').focus();
                }
                //SetUnitAndValue(-1, -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
                return false;
            });

            // Step 10.A.5 key up handler that shows the area
            $('#txtLen, #txtBreadth').keyup(function (e) {
                if ($('#txtLen').val() != '' && $('#txtBreadth').val() != '') {
                    var _lb = parseFloat($('#txtLen').val()) * parseFloat($('#txtBreadth').val());
                    $('#lblCalcDim').text(_lb.toFixed(2));
                }
                else {
                    $('#lblCalcDim').text('');
                }
            });

            // Step 10.A.7 fucntion to cancel updating the dims
            $('#btnCancelLB, #btnCancelPanel').click(function (e) {
                $('#pnlLB').dialog('close');
                $('#pnlPanel').dialog('close');
                setNewTxtValues();
            });

            // Step 10.A.8 fucntion to cancel updating the dims
            $('#btnSaveLB, #btnSavePanel').click(function (e) {
                $('#pnlLB').dialog('close');
                $('#pnlPanel').dialog('close');
            });

            // Step 10.B.1 add handler to open of dialog, this will set the appropriate focus
            $('body').on('dialogopen.AttachedEvent', '#pnlItem', function (event) {
                if (
                    (document.getElementById('txtPwdForIRD').value != $('#hdnPwdItemRateDis').val().split(':')[0] && document.getElementById('hdnPwdItemRateDis').value !== '') // &&
                // (document.getElementById('txtPwdForIRD').value !== '' && document.getElementById('hdnPwdItemRateDis').value !== '')
                    ) {
                    $('#pnlItem').dialog('close'); checkForPassword('ItemName'); return false;
                }
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
            // Step 10.B.4 close handler for add new item dialog box
            // on close set focus to prc
            $('body').on('dialogclose.AttachedEvent', '#pnlItem', function (event) {
                // just set the focus on txtItemCode
                // $('#txtProcess').focus();
                document.getElementById('txtPwdForIRD').value = '';
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

            // Step 12.A this handles the enter key and adds the keyup for enter
            $('#txtItemSubQty').keydown(function (event) {
                if ((event.which == 13 || event.which == 9)) {
                    $('#txtItemSubQty').one('keyup.AttachedEvent', txtItemSubQtykeyupHandler);
                }
            });

            // Step 13.A Attach the event handler
            $('#txtNewItemName').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtNewItemName').one('keyup.AttachedEvent', txtNewItemNameKeyUpHandler);
                }
            });

            // Step 14. A Attach the event handler
            $('#txtItemCode').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    var _res = txtItemCodeKeyUpHandler(event);
                    if (_res) {
                        btnItemSaveClickHander(event);
                    }
                }
            });

            // Step 15.A attach the event handler
            $('body').on("click.AttachedEvent", '#btnItemSave', btnItemSaveClickHander);

            // Step 15.A.1 attach the event handler
            $('body').on("keydown.AttachedEvent", '#btnItemSave', function (event) {
                if (event.which == 13) {
                    $('#btnItemSave').click();
                }
            });

            // Step 16.A Attach that event handler
            $('body').on("dialogclose.AttachedEvent", '#pnlItem', pnlItemSavedialogcloseHander);

            // Step 17. Add the event handler for txtAdvance lost amount
            $('#txtAdvance').keydown(function (event) {
                if ((event.which == 13 || event.which == 9) && !event.shiftKey) {
                    $('#txtAdvance').one('keyup.AttachedEvent', txtAdvanceKeyUpHandler);
                }
            });

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

                    if (document.getElementById('txtPwdForIRD').value != $('#hdnPwdItemRateDis').val().split(':')[2] && document.getElementById('hdnPwdItemRateDis').value !== '') {
                        checkForPassword('DiscountAmt');
                        return false;
                    }
                    $('#txtDiscountAmt').one('keyup.AttachedEvent', txtDiscountAmtKeyUpHandler);
                }
            });


            // Step 20.E add this handler to keydown event
            $('#txtDiscount').keydown(function (event) {
                window['lastDiscount'] = true;
                if (event.type = 'keydown' && ((event.which == 13 || event.which == 9) && !event.shiftKey)) {

                    if (document.getElementById('txtPwdForIRD').value != $('#hdnPwdItemRateDis').val().split(':')[3] && document.getElementById('hdnPwdItemRateDis').value !== '') {

                        if ($('#txtDiscount').val() > _tempdis || $('#txtDiscount').val() < _tempdis) {
                            checkForPassword('Discount');
                        }
                        else {
                            $('#txtAdvance').focus();
                        }
                        return false;
                    }

                    $('#txtDiscount').one('keyup.AttachedEvent', txtDiscountkeyUpHandler);
                }
            });

            // Step 22. on keypress at rate, if keypress is enter, call the add button
            $('#txtRate').keydown(function (event) {
                // check for if key is enter
                if (event.which == 13) {
                    // if the rate is changed
                    if (($('#txtRate').val() != $('#hdnPrvRate').val()) || ($('#txtExtraRate1').val() != $('#hdnPrvRate1').val()) || ($('#txtExtraRate2').val() != $('#hdnPrvRate2').val())) {
                        if (document.getElementById('txtPwdForIRD').value != $('#hdnPwdItemRateDis').val().split(':')[1] && document.getElementById('hdnPwdItemRateDis').value !== '') {
                            checkForPassword('Rate');
                            return false;
                        }
                    }

                    $('#grdEntry_ctl01_imgBtnGridEntry').trigger('click.AttachedEvent');
                    if ($('#hdnIsTour').val() == "1") { BookingTour(6); }  //Focus on Rate
                    return false;
                }
            });

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
                // remove the handler
                $('#' + _btnToclick).off('click');
                // remove handler on span
                $('.' + _spanToClick).off('click');
                event.preventDefault();
                // for that error where it clears the txtDiscount
                $('#hdnDiscountPerc').val($('#txtDiscount').val());
                // on the okay button 
                // enable the attrs
                $('#txtCurrentDue').attr('disabled', false);
                $('#txtSrTax').attr('disabled', false);
                $('#txtTotal').attr('disabled', false);
                $('#txtBalance').attr('disabled', false);
                $('#chkToday').attr('disabled', false);
                $('#chkNextDay').attr('disabled', false);
                $('#txtAdvance').attr('disabled', false);
                $('#ddlRateList').attr('disabled', false);
                $('#' + _btnToclick).click();
                return true;
            });

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
                $('#drpMonth').val(_mnth).find("option[value='" + _mnth + "']").attr("selected", true);
                $('#drpYear').val(_yr).find("option[value='" + _yr + "']").attr("selected", true); ;
                $('#drpDate').focus();
                // set the previous prev fields
                $('#hdnPrevDate').val(_datePart);
                $('#hdnPrevMonth').val(_mnth);
                $('#hdnPrevYear').val(_yr);
            });

            // Step 25.A add the previous hander
            $('body').on('change.AttachedEvent', '#chkToday, #chkNextDay', chkUrgentChangeHandler);
            if (isInEditMode.value !== 'true') {
                LoadUrgentRates();
            }

            // Step 26 For package of flat qty, all val in netAmt should come to advance
            $('#txtBalance').on('change.package', function (e) {
                // first check if it is package condition
                if (isPackageCondition()) {
                    // set the value of advance to be the net value
                    $('#txtAdvance').val($('#txtBalance').val());
                    $('#txtBalance').val('0')
                }
            });
            var strBookingNo = $('#hdnCustBooking').val();
            if (strBookingNo != "") {
                GetTheCustDetails(strBookingNo);
                $('#hdnCustBooking').val('');
            }
        });

    </script>
    
    <script type="text/javascript">
        $(document).ajaxStop(function () {
            $('#grdEntry_ctl01_imgBtnGridEntry').attr('disabled', false);
            $('#grdEntry').unblock({ fadeOut: 0 });
            // return false;

            // for the first time load
            $('#hdnPageLoaded').val('true');
            $.unblockUI();

            if (document.getElementById('hdnFireEdit').value == 'true') {
                document.getElementById('hdnFireEdit').value = 'false';
                $('.Container').block();
                if (document.getElementById('btnEdit') === null) return;
                setTimeout(function () { document.getElementById('btnEdit').click(); }, 3000);
            }

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
                           <%=strVersion%>
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
                                    BorderWidth="1px" Font-Names="Arial" Font-Size="15px" VerticalPadding="2px" />
                                <DynamicHoverStyle BackColor="#6086ac" BorderColor="#6086ac" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Arial" Font-Size="Large" ForeColor="Orange" />
                                <DynamicMenuStyle BackColor="#6086ac" />
                                <DynamicSelectedStyle BackColor="#6086ac" />
                                <DynamicMenuItemStyle BackColor="#6086ac" Font-Size="Large" ForeColor="White" VerticalPadding="2px"
                                    BorderColor="White" BorderWidth="1px" Font-Names="Arial" HorizontalPadding="5px" />
                                <Items>
                                </Items>
                                <StaticHoverStyle BorderColor="#6086ac" BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial"
                                    Font-Size="15px" ForeColor="White" BackColor="#6086ac" />
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
                                        <asp:Button ID="btnF8" runat="server" Text="Save (F8)" OnClick="btnF8_Click" Visible="false"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnDelivery" runat="server" Text="Delivery (F6)"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnF2" runat="server" Text="Search Order (F2)"
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
                                        Hi,
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
                                                 &nbsp;
                                                <asp:TextBox ID="txtEdit" runat="server" MaxLength="10" AutoCompleteType="None" AutoComplete="off"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtEdit_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True"  TargetControlID="txtEdit" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
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
                                                                    <asp:DropDownList ID="drpSearchCustBy" runat="server">
                                                                        <asp:ListItem Value="All" Text="All" />
                                                                        <asp:ListItem Value="Name" Text="Name" />
                                                                        <asp:ListItem Value="Address" Text="Address" />
                                                                        <asp:ListItem Value="Mobile" Text="Mobile" />
                                                                        <asp:ListItem Value="MembershipId" Text="Membership Id" />
                                                                        <asp:ListItem Value="CustCode" Text="Customer Code" />
                                                                    </asp:DropDownList>
                                                    <%--                <cc1:FilteredTextBoxExtender ID="FTBdrpSearchCustBy" runat="server" ClientIDMode="Static"
                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerName"></cc1:FilteredTextBoxExtender>--%>
                                                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static"
                                                                        MaxLength="250" onfocus="javascript:select();"
                                                                       ></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName" EnableCaching="false"
                                                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="1" 
                                                                        OnClientPopulating ="setContextKey" OnClientItemSelected="setMe"
                                                                        UseContextKey="true"
                                                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                                                                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new"
                                                                        >
                                                                    </cc1:AutoCompleteExtender>
                                                                    <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtCustomerName" FilterMode="InvalidChars"
                                                                    InvalidChars="`~:;,'"></cc1:FilteredTextBoxExtender>
                                                                    
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
                                                                    Prefrence :                                                                    
                                                                </td>
                                                                <td style="width: 475px" nowrap="nowrap" id="td1" runat="server">
                                                                    <asp:Label ID="lblPriority"  ClientIDMode="Static" runat="server" Font-Bold="True" Width="60px" CssClass="LabelBkgScrn"></asp:Label>
                                                                </td>                                                            
                                                               
                                                                <td>
                                                                    Address :
                                                                </td>
                                                                <td style="width: 260px" nowrap="nowrap" id="td2" runat="server">
                                                                    <asp:Label ID="lblAddress" runat="server" Font-Bold="True" Width="250px" ClientIDMode="Static" CssClass="LabelBkgScrn"></asp:Label>
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
                                                                   <span class="blink">
                                                                    <asp:Label ID="lblRemarks" ClientIDMode="Static" runat="server" style="color:#428BCA;font-weight:bold; font-size:20Px; background-color:Yellow;" ></asp:Label>
                                                                    </span>
                                                                </td>
                                                               
                                                                
                                                                    <td>
                                                                Comm Pref :
                                                                </td>
                                                                <td style="width: 250px" nowrap="nowrap">
                                                                    <asp:DropDownList ID="drpCommLbl" runat="server">
                                                                            <asp:ListItem Text="None" Value="NA" />
                                                                            <asp:ListItem Text="Only SMS" Value="SMS" />
                                                                            <asp:ListItem Text="Only Email" Value="Email" />
                                                                            <asp:ListItem Text="Both SMS and Email" Value="Both" />
                                                                    </asp:DropDownList>
                                                                </td>

                                                                <td style="width: 160px" nowrap="nowrap">
                                                                    <span><asp:CheckBox ID="chkNextDay" runat="server" /></span>
                                                                </td>                                                                
                                                        </tr>
                                                         <tr>
                                                                <td>
                                                                    Mob No. :
                                                                </td>
                                                                <td style="width: 470px" nowrap="nowrap">
                                                                    <asp:TextBox ID="lblMobileNo" ClientIDMode="Static" runat="server" 
                                                                        Font-Bold="true" Width="160px" CssClass="LabelBkgScrn" MaxLength="10"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="lblMobileNo_FilteredTextBoxExtender" 
                                                                        runat="server" Enabled="True" FilterType="Numbers" 
                                                                        TargetControlID="lblMobileNo">
                                                                    </cc1:FilteredTextBoxExtender>&nbsp;&nbsp;
                                                                    <asp:Label ID="Label16" runat="server" Text="RateList"></asp:Label>&nbsp;&nbsp;
                                                    <asp:DropDownList runat="server" ID="ddlRateList"></asp:DropDownList>
                                                                </td>
                                                               
                                                                
                                                                    <td>
                                                                    Email : 
                                                                </td>
                                                                <td style="width: 250px" nowrap="nowrap">
                                                                    <asp:TextBox ID="lblEmailId" ClientIDMode="Static" runat="server" Font-Bold="true" Width="160px" CssClass="LabelBkgScrn"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="lblEmailId" FilterMode="InvalidChars"
                                                                    InvalidChars="`~:;,"></cc1:FilteredTextBoxExtender>
                                                                </td> 
                                                                    
                                                                <td style="width: 160px" nowrap="nowrap">
                                                                    <span><asp:CheckBox ID="chkUrgent" runat="server" Text="Urgent" /></span>
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
                                                                                            <asp:TextBox ID="txtQty" runat="server" Width="25px" Text='<%# Bind("QTY") %>' MaxLength="4" ClientIDMode="Static" AutoComplete="off"
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
                                                                                                    <cc1:FilteredTextBoxExtender runat="server" FilterMode="InvalidChars" InvalidChars="`~:;," TargetControlID="txtName"></cc1:FilteredTextBoxExtender>
                                                                                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtName" TargetControlID="txtName"  EnableCaching="false"
                                                                                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1" OnClientItemSelected="sel_ItemName"
                                                                                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_Process"
                                                                                                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
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
                                                                                             <asp:Label ID="hdnTmpItemName" runat="server" Text='<%# Bind("ITEMNAME") %>' style="display:none" ForeColor="Black"></asp:Label>                                                                                          
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ControlStyle-Width="180px" ItemStyle-Width="180px" HeaderStyle-Width="180px">
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
                                                                                            <ul id="mytags" class="ul.Tagit" style="width:180px">
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
                                                                        <asp:TemplateField ControlStyle-Width="180px" ItemStyle-Width="180px" HeaderStyle-Width="180px">
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
                                                                                            <ul id="mytagsColor" style="width:180px">
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
                                                                        <asp:TemplateField ControlStyle-Width="300px" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                            <HeaderTemplate>
                                                                                <asp:UpdatePanel ID="upProcess" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <table>
                                                                                            <tr class="mGridCustomHeader">
                                                                                                <td colspan="3" align="center" style="color: Orange; border-width: 0px">
                                                                                                    Service and Price
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" style="border-width: 0px">
                                                                                                    <asp:TextBox ID="txtProcess" runat="server" Width="170px" Text='<%# Bind("PROCESS") %>' ClientIDMode="Static"
                                                                                                        CausesValidation="false" MaxLength="15"></asp:TextBox>
                                                                                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtProcess" TargetControlID="txtProcess"  EnableCaching="false"
                                                                                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1" OnClientItemSelected="set_prc"
                                                                                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_Process"
                                                                                                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new" 
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
                                                                    <asp:DropDownList ID="drpHD" runat="server" >
                                                                     <asp:ListItem Text="No" />
                                                                        <asp:ListItem Text="Yes" />                                                                       
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 5px !important;" align="left">
                                                                    &nbsp;&nbsp;&nbsp;SMS :
                                                                </td>
                                                                <td align="left" style="width: 10px;">
                                                                    <asp:CheckBox ID="chkSendsms" runat="server" TextAlign="Left" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="drpsmstemplate" runat="server" Width="80px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 5px !important;" align="left">
                                                                    &nbsp;&nbsp;&nbsp;Email :
                                                                </td>
                                                                <td align="left" style="width: 10px;">
                                                                    <asp:CheckBox ID="chkEmail" runat="server" TextAlign="Left" />
                                                                </td>
                                                                <td></td>
                                                                
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
                                                                   Order Notes :
                                                                </td>
                                                                <td align="left" colspan="5">
                                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtRemarks" FilterMode="InvalidChars" InvalidChars="`~:;,-"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption" style="color: Black">
                                                                   Workshop Notes :
                                                                </td>
                                                                <td align="left" colspan="5">
                                                                    <asp:TextBox ID="txtWorkshopNotes" runat="server" ClientIDMode="Static" Width="250px" MaxLength="100"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtWorkshopNotes" FilterMode="InvalidChars" InvalidChars="`~:;,-"></cc1:FilteredTextBoxExtender>
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
                                            <div id="PkgDates" class="otherClass"  style="display: none;">
                                           
                                                <table>
                                                <tr><td>
                                                <asp:Label ID="Label12" runat="server" CssClass="LabelPending" Text="Package : "></asp:Label></td><td>
                                                <asp:Label ID="LblPkgName" CssClass="LabelPendingText"  runat="server" ClientIDMode="Static"></asp:Label></td></tr>
                                                &nbsp;
                                                <tr><td>
                                                <asp:Label ID="Label14" runat="server" CssClass="LabelPendingRt" Text="Expiry Date : "></asp:Label></td><td>
                                                <asp:Label ID="lblPkgEndDate" CssClass="LabelPendingTextRt" runat="server" ClientIDMode="Static"></asp:Label></td></tr>                                                
                                                </table>
                                            </div>
                                            <div id="lowerRowCenter" class="lowerCenterTd">
                                            <table>
                                            <tr><td>
                                                <asp:Label ID="lblSave" CssClass="SuccessMessage" runat="server" EnableViewState="false" /></td>
                                                <td><asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="ErrorMessage" /></td></tr>
                                                <tr><td><asp:Label ID="Label1" runat="server" CssClass="LabelPending" Text="Pending Clothes : "></asp:Label></td>
                                                <td>
                                                <asp:Label ID="lblPendingCloth" CssClass="LabelPendingText"  runat="server" ClientIDMode="Static"></asp:Label>
                                                </td>
                                                </tr>
                                                <tr>
                                                <td>
                                                <asp:Label ID="Label9" runat="server" CssClass="LabelPendingRt" Text="Pending Amount : "></asp:Label></td>
                                                <td><asp:Label ID="lblPendingAmt" CssClass="LabelPendingTextRt" runat="server" ClientIDMode="Static"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                <td>
                                                <asp:Label ID="Label18" runat="server" CssClass="LabelPendingOrd" Text="Pending Order&nbsp;&nbsp;&nbsp;&nbsp: "></asp:Label>                                                
                                                </td>
                                                <td>
                                                 <asp:Label ID="lblPendinOrder" runat="server" CssClass="LabelPendingText" ></asp:Label>                                                
                                                </td>                                                
                                                </tr>
                                                </table>

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
                                                            <tr style="display:none;">
                                                                <td style="color: Green">
                                                                    Tender Amt :
                                                                </td>
                                                                <td >
                                                                    <asp:TextBox ID="txtTenderAmt" runat="server" Width="167px" Style="text-align: right"
                                                                        MaxLength="6"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                        Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtTenderAmt">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: Green">
                                                                    Advance :
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="drpAdvanceType" runat="server" Width="113px" AppendDataBoundItems="true">
                                                                        <asp:ListItem Text="Cash"></asp:ListItem>
                                                                        <asp:ListItem Text="Credit Card/Debit Card" />
                                                                        <asp:ListItem Text="Cheque/Bank" />
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtAdvance" runat="server" Width="50px" Style="text-align: right"
                                                                        MaxLength="6"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtAdvance_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtAdvance">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <%--<td>
                                                                    <asp:DropDownList ID="drpPaymentType" runat="server">
                                                                        <asp:ListItem Text="Cash"></asp:ListItem>
                                                                        <asp:ListItem Text="Credit Card/Debit Card" />
                                                                        <asp:ListItem Text="Cheque/Bank" />
                                                                    </asp:DropDownList>
                                                                </td>--%>
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
                                                Add Extra Service
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td align="left">
                                                        Second Service
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraProcess1" runat="server" Width="180"
                                                            MaxLength="15"  ClientIDMode="Static"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtExtraProcess1" TargetControlID="txtExtraProcess1"  EnableCaching="false"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1" OnClientItemSelected="set_prc"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_Process"
                                                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        Price
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraRate1" runat="server" ClientIDMode="Static" Width="100" MaxLength="6" Style="text-align: right" CssClass="clsRate">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="txtExtraRate1_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtExtraRate1">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="RemovePrc2" ClientIDMode="Static" Text="X" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Third Service
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraProcess2" runat="server" Width="180" ClientIDMode="Static"
                                                            MaxLength="15" ></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtExtraProcess2"  EnableCaching="false"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1" OnClientItemSelected="set_prc"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_Process"
                                                            CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        Price
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraRate2" runat="server"  ClientIDMode="Static" Width="100" MaxLength="6" Style="text-align: right" CssClass="clsRate">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="txtExtraRate2_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtExtraRate2">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="RemovePrc3" ClientIDMode="Static" Text="X" runat="server" />
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
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterMode="InvalidChars" TargetControlID="txtCName"
                                                            InvalidChars="`~:;,-'"></cc1:FilteredTextBoxExtender>
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
                                                        <asp:TextBox ID="txtCAddress" runat="server" Width="245" TabIndex="3" MaxLength="50"></asp:TextBox>&nbsp;<span
                                                            class="span">*</span>
                                                              <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="InvalidChars" TargetControlID="txtCAddress"
                                                            InvalidChars="`~:;,-'"></cc1:FilteredTextBoxExtender>
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
                                                    <td nowrap="nowrap">
                                                        Area / Location
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAreaLocaton" runat="server" Width="245" TabIndex="5" MaxLength="100"></asp:TextBox>
                                                          <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterMode="InvalidChars" TargetControlID="txtAreaLocaton"
                                                            InvalidChars="`~:;,-'"></cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                <td>RateList</td><td>:</td><td><asp:DropDownList ID="ddlRateListNewCustomer" TabIndex="6" runat="server"></asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Prefrence 
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
                                                            <asp:TextBox ID="txtPriority" runat="server" ClientIDMode="Static" Width="245px"  TabIndex="7"
                                                                MaxLength="300" onfocus="javascript:select();"
                                                                ></asp:TextBox>
                                                              <cc1:FilteredTextBoxExtender ID="txtPrioritysdds" runat="server"
                                                                        Enabled="True" TargetControlID="txtPriority" ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                            <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender2" TargetControlID="txtPriority" EnableCaching="false"
                                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetPriorityList" MinimumPrefixLength="1"
                                                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                >
                                                            </cc1:AutoCompleteExtender>
                                                              <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="InvalidChars" TargetControlID="txtPriority"
                                                            InvalidChars="`~:;,-"></cc1:FilteredTextBoxExtender>
                                                            <asp:HiddenField ID="txtPriorityCode" runat="server" />

                                                    </td>
                                                </tr>                                               
                                                <tr>
                                                    <td nowrap="nowrap">
                                                      <span id="trdiscount" runat="server">  Default Discount</span>
                                                    </td>
                                                    <td>
                                                       <span id="trdot" runat="server">  :</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDefaultDiscount" runat="server" Width="245" TabIndex="8" MaxLength="2"></asp:TextBox><span class="span" id="trpercentage" runat="server">%</span>
                                                        <cc1:FilteredTextBoxExtender runat="server" ID="fteDefaultDiscount" FilterMode="ValidChars"
                                                        TargetControlID="txtDefaultDiscount" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Communication Preference
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drpCommunicationMeans" runat="server" Width="245" TabIndex="9">
                                                            <asp:ListItem Text="None" Value="NA" />
                                                            <asp:ListItem Text="Only SMS" Value="SMS" />
                                                            <asp:ListItem Text="Only Email" Value="Email" />
                                                            <asp:ListItem Text="Both SMS and Email" Value="Both" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap">
                                                        Email
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="245" TabIndex="10" MaxLength="100"></asp:TextBox>
                                                         <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterMode="InvalidChars" TargetControlID="txtEmail"
                                                            InvalidChars="`~:;,-'"></cc1:FilteredTextBoxExtender>
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
                                                        <asp:TextBox ID="txtRemarks1" runat="server" TabIndex="11" Width="245" MaxLength="100"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterMode="InvalidChars" TargetControlID="txtRemarks1"
                                                            InvalidChars="`~:;,-'"></cc1:FilteredTextBoxExtender>
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
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG3%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span></td>
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
            <table class="TDCaption">
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

        <asp:Panel ID="pnlLB" runat="server" CssClass="modalPopup" Style="display: none"
            BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="340px">
                    <div class="popup_Titlebar" id="Div5">
                        <div class="TitlebarLeft">
                            <%=BkScrItmDim%>
                        </div>
                    </div>
                    <div class="popup_Body">
                        <table class="TableData">
                            <tr>
                                <td width="150px" align="left">
                                <%= BkScrItmDimCaption %> :
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>                               
                                
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label2" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                    <asp:Label ID="Label3" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                    <asp:Label ID="Label4" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="Label5" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="TableData">
                                        <tr>
                                            <td class="TDCaption">
                                                 <%=BkScrItmLen%>
                                            </td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <td class="TDDot" nowrap="nowrap">
                                                &nbsp;&nbsp;<asp:TextBox ID="txtLen" runat="server" MaxLength="5" TabIndex="1" Width="30px"  Text="1"
                                                    CssClass="Textbox"></asp:TextBox>
                                                
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                    Enabled="True" TargetControlID="txtLen" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                                <span class="span"></span>
                                            </td>
                                            <td>&nbsp;X&nbsp;</td>
                                            <td class="TDCaption" align="left">
                                                <%=BkScrItmWth%>
                                            </td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <td class="TDDot" nowrap="nowrap">
                                                &nbsp;&nbsp;<asp:TextBox ID="txtBreadth" runat="server" Width="30Px" CssClass="Textbox" Text="1"
                                                    TabIndex="5" MaxLength="5"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                    Enabled="True" TargetControlID="txtBreadth" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                            &nbsp;= Area <asp:Label runat="server" ID="lblCalcDim">1</asp:Label>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                           <td>
                                            
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
                                    <table style="width: 265px;">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnSaveLB" CausesValidation="false" runat="server" Text="Save" style="position:relative; left:30px;"
                                                    TabIndex="6" />
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="btnCancelLB" CausesValidation="false" runat="server" Text="Cancel"
                                                    TabIndex="7" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
        </asp:Panel>

        <asp:Panel ID="pnlPanel" runat="server" CssClass="modalPopup" Style="display: none"
            BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="340px">
                    <div class="popup_Titlebar" id="Div8">
                        <div class="TitlebarLeft">
                            <%=BkScrItmPnl %>
                        </div>
                    </div>
                    <div class="popup_Body">
                        <table class="TableData">
                            <tr>
                                <td width="150px" align="left">
                                <%= BkScrItmPnlCaption%> :
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>                               
                                
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label6" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                    <asp:Label ID="Label7" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                    <asp:Label ID="Label8" runat="server" Visible="False"></asp:Label>
                                    
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="TableData">
                                        <tr>
                                            <td class="TDCaption">
                                                 <%=BkScrItmPnlDesc%>
                                            </td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <td class="TDDot" nowrap="nowrap">
                                                &nbsp;&nbsp;<asp:TextBox ID="txtNumPanels" runat="server" MaxLength="4" TabIndex="1" Width="30px"  Text="1"
                                                    CssClass="Textbox"></asp:TextBox>
                                                
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                    Enabled="True" TargetControlID="txtNumPanels" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                                <span class="span"></span>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                           <td>
                                            
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
                                    <table style="width: 265px;">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnSavePanel" CausesValidation="false" runat="server" Text="Save" style="position:relative; left:30px;"
                                                    TabIndex="6" />
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="btnCancelPanel" CausesValidation="false" runat="server" Text="Cancel"
                                                    TabIndex="7" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
        </asp:Panel>

        <asp:Panel ID="pnlPwd" runat="server"  Style="display: none"
            BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="340px">
                    <div class="popup_Titlebar" id="Div9">
                        <div class="TitlebarLeft">
                            Password
                        </div>
                    </div>
                    <div class="popup_Body">
                        <table class="TableData">
                            <tr>
                                <td width="150px" align="left">
                                Enter the password:
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>                               
                                
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label10" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                    <asp:Label ID="Label11" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                    <asp:Label ID="Label13" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="Label15" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="TableData">
                                        <tr>
                                            <td class="TDCaption">
                                                 Password :
                                            </td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <td class="TDDot" nowrap="nowrap">
                                                &nbsp;&nbsp;<asp:TextBox ID="txtPwdForIRD" runat="server" MaxLength="10" TabIndex="1" Width="80px"  Text="1" TextMode="Password"
                                                    CssClass="Textbox"></asp:TextBox>
                                                <span class="span"></span>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                           <td>
                                            
                                           </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            <asp:Label ID="lblWrongPwd" runat="server"></asp:Label>
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
                                    <table style="width: 265px;">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnAcceptPwd" CausesValidation="false" runat="server" Text="Save" style="position:relative; left:30px;"
                                                    TabIndex="6" />
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="btnCancelPwd" CausesValidation="false" runat="server" Text="Cancel"
                                                    TabIndex="7" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                                <td>
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
    <asp:HiddenField ID="hdnUrgentRateApplied" runat="server" ClientIDMode="Static" />
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
    <asp:HiddenField ID="hdnPrevDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPrevMonth" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPrevYear" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPageLoaded" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnItemUnit" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnItemUnitValue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPrevContainedDim" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPrevUnit" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBindColorToQty" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnFireEdit" runat="server" />
    <asp:HiddenField ID="hdnAssignId" runat="server" />
    <asp:HiddenField ID="hdnPwdItemRateDis" runat="server" />
    <asp:HiddenField ID="hdnPrvRate" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnPrvRate1" runat="server"  Value="0"/>
    <asp:HiddenField ID="hdnPrvRate2" runat="server" Value="0" />
    <asp:HiddenField ID="hdnPrvDis" runat="server" />
    <asp:HiddenField ID="hdnPrvPwdFocus" runat="server" />
    <asp:HiddenField ID="hdnCheckForEditRemarks" runat="server" />
    <asp:HiddenField ID="hdnPackageExpired" runat="server" />
    <asp:HiddenField  ID="hdnDummyManyMobUnq" runat="server" Value="true" />
    <asp:HiddenField ID="hdnPackgeItems" runat="server" />
    <asp:HiddenField ID="hdnPackageTypeRecNo" runat="server" />
    <asp:HiddenField ID="hdnPackageStartEndDate" runat="server" />
    <asp:HiddenField ID="hdnPackageTaxPerItemPrice" runat="server" />
    <asp:HiddenField ID="hdnCheckPkgDiscount" runat="server" />
     <asp:HiddenField ID="hdnCheckDiscountRight" runat="server" />
      <asp:HiddenField ID="hdnTmpPkgType" runat="server" Value="0" />
      <asp:HiddenField ID="hdnTmpAdvType" runat="server" />
      <asp:HiddenField ID="hdnIsPkgBooking" runat="server" />
        <asp:HiddenField ID="hdnIsTour" ClientIDMode="Static" Value="0" runat="server" />

     <asp:PlaceHolder ID="PlaceHolder1" runat="server">
      <asp:HiddenField ID="hdnCustBooking" runat="server" />
        <asp:Label ID="hdnValues" Text="c++,java,php,coldfusion,javascript,asp,ruby,python,c" ClientIDMode="Static"
            runat="server" style="display: none;"/>
            <asp:Label ID="LblValuesColor" runat="server" style="display: none;" ClientIDMode="Static"></asp:Label>
    </asp:PlaceHolder>
    </form>
   
</body>
<script type="text/javascript">
    function ShowReturnMsg() {
        var msg = '<%= Session["ReturnMsg"] %>';
        if (msg.length > 0) alert(msg);
    }
</script>
<script src="../JavaScript/AppTour.js" type="text/javascript"></script>
 <script type="text/javascript">
     $(document).ready(function () {
         //  BookingTour(0); // for focus on Qty
         if (RegExp('IsTour=1', 'gi').test(window.location.search)) {
             BookingTour(0); // for focus on Qty
             $('#hdnIsTour').val('1');
         }        
         $('body').delegate('[data-role="end"]', 'click', function () {
             $('#hdnIsTour').val('0');
         });

     });
      </script>

<% Session["ReturnMsg"] = null; %>
</html>