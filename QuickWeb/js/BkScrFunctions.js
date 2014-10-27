// Step 0.z this will check if desc and color are to be bind to master or not
function checkDescAndColorForBinding() {
    $.ajax({
        url: '../AutoComplete.asmx/checkDescAndColorForBinding',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        prcoessData: false,
        async: false,
        success: function (response) {
            $('#hdnBindDesc').val(response.d.split(':')[0].toLowerCase());
            $('#hdnBindColor').val(response.d.split(':')[1].toLowerCase());
        },
        error: function (response) {
        }
    });
};

// Step 0.z.2 check if confirmation for delivery date

function checkDeliveryDateForConfirmation() {
    $.ajax({
        url: '../AutoComplete.asmx/ConfirmDelivery',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: true,
        success: function (response) {
            $('#hdnConfirmDelivery').val(response.d);
        },
        error: function (response) {
        }
    });
}

// Step 0.z.3 check if bind color to qty
function CheckIfBindColorToQty() {
    $.ajax({
        url: '../Autocomplete.asmx/CheckIfBindColorToQty',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: true,
        async: false,
        success: function (response) {
            var _val = response.d;
            if (_val == true) {
                $('#hdnBindColorToQty').val('1');
            }
            else {
                $('#hdnBindColorToQty').val('-1');
            }
        },
        error: function (response) {
        }
    });
}

// Step 1.
// On page load, load:
// A. Default Item
// B. Default Process
// C. Default Rate For Default Item And Process.
// D. Also, this loads the default qty, weather 1 or blank
function LoadDefaultItemProcessAndRate() {
    $.ajax({
        url: '../AutoComplete.asmx/LoadDefaultItemProcessAndRate?rateListId=' + $('#ddlRateList').val(),
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        success: function (response) {
            // Set default Item
            $('#hdnDefaultItem').val(response.d[0].toUpperCase());
            // Set default Process
            $('#hdnDefaultProcess').val(response.d[1]);
            // Set default Rate
            $('#hdnDefaultItemProcessRate').val(response.d[2]);
            $('#hdnPrvRate').val(response.d[2]);
            // Set default qty
            $('#hdnDefaultQty').val(response.d[3]);
        },
        error: function (response) {
        }
    });
}

function LoadTaxBeforeOrAfter() {
    // D. If tax is calculated before or after discount, if method returns ture, then before else if false then after
    $.ajax({
        url: '../AutoComplete.asmx/TaxBeforeOrAfter',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (response) {
            $('#hdnTaxBefore').val(response.d);
        },
        error: function (response) {
        }
    });
}

function LoadAllThreeTaxes() {
    // D.1 this loads all 3 tax rates
    // D. If tax is calculated before or after discount, if method returns ture, then before else if false then after
    $.ajax({
        url: '../AutoComplete.asmx/LoadAllThreeTaxes',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (response) {
            $('#hdnThreeTaxRates').val(response.d);
        },
        error: function (response) {
        }
    });
}

function LoadIsTaxExclusive() {
    // D.2 this loads all 3 tax rates
    // D. If tax is calculated before or after discount, if method returns ture, then before else if false then after
    $.ajax({
        url: '../AutoComplete.asmx/IsTaxExclusive',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (response) {
            $('#hdnIsTaxExclusive').val(response.d);
        },
        error: function (response) {
        }
    });
}

// H.1 this will load all the tax and discount
function loadAllTax() {
    $.ajax({
        url: '../AutoComplete.asmx/FindTaxActive',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (response) {
            $('#totalTaxWithActive').val(response.d);
        },
        error: function (response) {
        }
    });
}
// H.2 this will load all the tax and discount
function loadAllDis() {
    $.ajax({
        url: '../AutoComplete.asmx/FindDisActive',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            $('#totalDisWithActive').val(response.d);
        },
        error: function (response) {
        }
    });
}

// H.this will tell weather to show the net amount in round or decimal
function LoadIsNetAmountInDecimal() {
    $.ajax({
        url: '../AutoComplete.asmx/isNetAmountInDecimal',
        type: 'GET',
        cache: false,
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            $('#isNetAmountInDecimal').val(response.d);
        },
        error: function (response) {
        }
    });
}

function LoadAllItems() {
    $.ajax({
        url: '../AutoComplete.asmx/LoadAllItems',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            $('#hdnAllItems').val(response.d);
        },
        error: function (response) {
        }
    });
}

function LoadAllProcesses() {
    $.ajax({
        url: '../AutoComplete.asmx/LoadAllProcesses',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            $('#hdnAllPrcs').val(response.d);
        },
        error: function (response) {
        }
    });
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
        $('#txtBalance').val('0').triggerHandler('change.package');
    }
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
        $('#grdEntry').find('#grdEntry_ctl' + _placeHolderForID + '_hdnTmpItemName').attr('id', 'TmpItemName_' + _idToSet + '');
        // now update the coutner that is updating the required id(s)
        _j++;
    }
}

// Step 1.V.1 this will check if desc is enabled, and color is enabled
function checkIfDesAndColorEnabled() {
    $.ajax({
        url: '../AutoComplete.asmx/checkIfDesAndColorEnabled',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            $('#hdnDescEnabled').val(response.d.split(':')[0]);
            $('#hdnColorEnabled').val(response.d.split(':')[1]);
            // this function will hide the color or desc box, depending on the value returned from server
            showHideDescColor(-1, -1, false, true);
        },
        error: function (response) {
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

function resizeTheGridForDiabled() {
    $('#grdEntry > tbody > tr:eq(0) > th:eq(0)').width(60);
    $('#grdEntry > tbody > tr:eq(0) > th:eq(1)').width(60);
    $('#txtQty').width(60);
    $('#grdEntry > tbody > tr:eq(0) > th:eq(2)').width(400);
    $('#txtName').width(400);
    $('#grdEntry > tbody > tr:eq(0) > th:eq(6)').width(200);
}

function changeBackGroundColor() {
    $('#grdEntry > tbody > tr:not(:eq(0))').css('background-color', 'white');
}

// Step 1.X this padds the reuired
function pad(str, max) {
    return str.length < max ? pad("0" + str, max) : str;
}

function setDefaultDiscountType() {
    $.ajax({
        url: '../Autocomplete.asmx/FindDefaultDiscountType',
        type: 'GET',
        cache: false,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        timeout: 20000,
        success: function (response) {
            var _result = response.d;
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
        }
    });
};

// Step 1.W.2 checks the sms box based on configuration
function checkSMS() {
    $.ajax({
        url: '../Autocomplete.asmx/checkDefaultSMS',
        type: 'GET',
        cache: false,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        timeout: 20000,
        success: function (response) {
            var _result = response.d;
            if (_result == true) {
                $('#chkSendsms').attr('checked', true);
            }
            else {
                $('#chkSendsms').attr('checked', false);
            }
        },
        error: function (response) {
        }
    });
}

// Step 2.A.1 This is the event handler for customer name event handler
var txtCustomerNameKeyupHandler = function (event) {
    var custArray = $('#txtCustomerName').val().split('-')[0];
    var custCode = custArray.substring(0, custArray.length - 1).trim();
    if ($('#txtCustomerName').val() == '') { $('#txtQty').focus(); return; }
    if ($('#txtCustomerName').val().indexOf('-') < 0) {
        // Disable first row and lower grid
        $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
        // check if this customer exists, if no show the add customer dialog
        $.ajax({
            url: '../AutoComplete.asmx/CheckIfCustomerExists',
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            data: "argCustName='" + $('#txtCustomerName').val().trim().toUpperCase() + ":" + drpSearchCustBy.value + "'",
            success: function (response) {
                var _res = response.d;
                if (_res == false) {
                    //$('#txtCName').val($('#txtCustomerName').val().trim().toUpperCase());
                    $('#pnlNewCustomer').dialog({ width: 520, height: 450, modal: true });
                }
                else {
                    $('#grdEntry').unblock({ fadeOut: 0 });
                    $('#txtQty').focus();
                }
            },
            error: function (response) { }
        });
    }
    else {
        $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
        GetTheCustDetails(custCode);
        /// <Obsolete> ///
        $.ajax({
            url: '../AutoComplete.asmx/CheckMeansOfCommunication',
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            data: "customerCode='" + custCode + "'",
            contentType: 'application/json; charset=utf8',
            cache: false,
            success: function (result) {
                var _resComm = result.d;
                setCommunicationCheckBox(_resComm)
            },
            error: function () { /* alert('something went terribly wrong, the planet is about to explode, take your belongings and leave your home as soon as you can'); */ }
        });
    }
};

// This gets the cust details, separated because we need to call it even on update
function GetTheCustDetails(custCode) {
    $.ajax({
        url: '../AutoComplete.asmx/GetPriorityAndRemarks',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        data: "arg='" + custCode + "'",
        success: function (response) {
            var result = response.d;
            // the values is in form of name, address, mobile, priority and remark, and discount
            var resultAry = result.split(':');
            $('#txtCustomerName').val(resultAry[0].trim());
            $('#lblAddress').text(resultAry[1]);
            $('#lblMobileNo').val(resultAry[2]);
            $('#lblPriority').text(resultAry[3]);
            $('#lblRemarks').text(resultAry[4]);
            // if discount is not null or 0
            if (resultAry[5] != '' && resultAry[5] != '0') {
                if (document.getElementById('isInEditMode').value != 'true') {
                    $('#rdrPercentage').click();
                    $('#txtDiscount').val(resultAry[5]);
                    addDiscountFromAddCustomer(resultAry[5]);
                }
            }
            else if (resultAry[5] == '') {

                setTimeout(function () {
                    PendingClothesForPackage(resultAry, 5, []);
                }, 10);

            }
            $('#lblPendingCloth').text(resultAry[8]);
            $('#lblPendingAmt').text(resultAry[9]);
            $('#lblPendinOrder').text(resultAry[11]);
            setCommunicationCheckBox(resultAry[6], true);
            $("#lblEmailId").val(resultAry[7]);
            $('#txtQty').focus();
            $('#txtQty').select();
            $('#hdnCustCode').val(custCode);
            //  fire the focusout handler
            $('#txtDiscount').focusout();
            // set the rate list id
            isInEditMode.value !== 'true' && $('#grdEntry').children().children('tr').length === 1 && $('#ddlRateList').val(resultAry[10])
            GetPackageDetails(custCode);

            $('#grdEntry').unblock({ fadeOut: 0 });
            return false;
        },
        error: function (response) {
        }
    });
}

// This gets the cust details, separated because we need to call it even on update
function GetPackageDetails(custCode) {
    $.ajax({
        url: '../AutoComplete.asmx/CheckDetailsOfPackage',
        type: 'GET',
        timeout: 2000,
        data: "customerCode='" + custCode + "'&bookingDate='" + $('#txtDate').val() + "'",
        processData: false,
        datatype: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var _pkg = response.d;
            console.log('done');
            console.log(_pkg);
            if (_pkg == '') {
                $('#lowerRowCenter').find('span').not($('#tblBtnSavings').find('span')).not('#lblError, #lblSave').css('display', 'inline')
                $('#lowerRowCenter').find('span.LabelPending').text('Pending Cloth :');
                $('#lowerRowCenter').find('span.LabelPendingRt').text('Pending Amount :');
                document.getElementById('hdnPackageExpired').value = 'false';
                document.getElementById('txtTenderAmt').removeAttribute('disabled');
                document.getElementById('txtAdvance').removeAttribute('disabled');
                document.getElementById('txtDiscount').removeAttribute('disabled');
                document.getElementById('txtDiscountAmt').removeAttribute('disabled');
                document.getElementById('rdrAmt').disabled = false;
                document.getElementById('rdrPercentage').disabled = false;
                document.getElementById('drpAdvanceType').disabled = false;
                $('#drpAdvanceType option').eq(3).remove();
                $('#drpAdvanceType').off('change.AttachedEvent');
                document.getElementById('hdnAssignId').value = '';
                document.getElementById('hdnPackageTypeRecNo').value = ':';
                $('#PkgDates').hide();
                if ($('#grdEntry').children().children().length <= 1) {
                    $('#chkToday, #chkNextDay').attr('disabled', false);
                }
                $('#dynamicPendingPkg').hide();

                //set discount according to right
                if ($('#hdnCheckDiscountRight').val() === 'True') {
                    $('#txtDiscount').attr('disabled', false);
                    $('#rdrPercentage').attr('disabled', false);
                    $('#rdrAmt').attr('disabled', false);
                }
                else {
                    $('#txtDiscount').attr('disabled', true);
                    $('#rdrPercentage').attr('disabled', true);
                    $('#rdrAmt').attr('disabled', true);
                }

                return;
            };
            setPackage(_pkg);
        },
        error: function (response) {
            console.log(response);
        }
    });
}

/// <param name="pkgDetails" type="String">A string that contains all packageDetails</param>
/// <summary>This sets the pkg details</summary>
function setPackage(pkgDetails) {
    // if package type if "Discount", don't do nothing
    var pkgObj = {};
    var compareDate; // the date that will be used to compare, Very last one if available, else last one
    pkgObj = setPackageObj(pkgObj, pkgDetails);
    $('#hdnPackageStartEndDate').val(pkgObj.startDate + ':' + pkgObj.lastEndDate);
    compareDate = pkgObj.lastEndDate;
    if (pkgObj.lastEndDate === 'None') {
        $('#hdnPackageStartEndDate').val(pkgObj.startDate + ':' + pkgObj.endDate);
        compareDate = pkgObj.endDate;
    }
    // if ((pkgObj.pkgType === 'Qty / Item' && pkgObj.recurrence === '0') || (pkgObj.pkgType !== 'Qty / Item' && compareDates(pkgObj.endDate) == false)) {
    // if pkgType is qty or flat (3 or 4)
    if ((pkgObj.pkgType === 'Qty / Item' || pkgObj.pkgType === 'Flat Qty')) {
        // and recurrence is 0, or current date is greater then lastLastDate, then package is expired
        if (pkgObj.recurrence === '0' || compareDates(compareDate) == false) {
            document.getElementById('hdnPackageExpired').value = 'true';
            //var id = setInterval(function () { alert('This package is expired. Please renew or mark complete'); }, 200);
            setTimeout(function () { alert('This package is expired. Please renew or mark complete') }, 500);
        }
    }
    // if package if of type 2, or 1, and date has expired
    else if (compareDates(compareDate, document.getElementById('txtDate').value) == false) {
        setTimeout(function () { alert('This package is expired. Please renew or mark complete') }, 500);
    }
    else
        document.getElementById('hdnPackageExpired').value = 'false';

    $('#hdnPackageTaxPerItemPrice').val(pkgObj.taxType + ':' + pkgObj.perItemPrice);

    if (pkgObj.pkgType === 'Value / Benefit') {
        $('#Label9').html('Balance Amount :');
    }
    else {
        $('#Label9').html('Pending Amount :');
    }
    /*
    if (pkgObj.pkgType === 'Discount')
    return; */
    if (pkgObj.pkgType === 'Flat Qty') {
        $('#chkToday, #chkNextDay').attr('disabled', true);
    }

    $('#lowerRowCenter').children('span').not('#lblError, #lblSave').css('display', 'inline')
    if (pkgObj.pkgType === 'Qty / Item' || pkgObj.pkgType === 'Flat Qty') { // we are here so we must have set cust code accordingly
        GetQtyndItemsForPackage($('#hdnCustCode').val(), pkgObj.assignId, pkgObj.recurrence);
        $('#lowerRowCenter').find('span').not($('#tblBtnSavings').find('span')).not('#lblError, #lblSave').css('display', 'none');
    }

    document.getElementById('hdnPackageTypeRecNo').value = pkgObj.pkgType + ':' + pkgObj.recurrence;
    $('#LblPkgName').text(pkgObj.pkgName);
    pkgObj.lastEndDate === 'None' ? $('#lblPkgEndDate').text(pkgObj.endDate) : $('#lblPkgEndDate').text(pkgObj.lastEndDate);
    //(pkgObj.pkgType !== 'Discount') && $('#lblPendingCloth').text(pkgObj.totalValue);
    if ((pkgObj.pkgType !== 'Discount') && (pkgObj.pkgType !== 'Value / Benefit')) {
        $('#lblPendingCloth').text(pkgObj.totalValue);
    }
    (pkgObj.pkgType !== 'Discount') && $('#lblPendingAmt').text(pkgObj.remainingValue);  

    $('#PkgDates').show();

    if (parseInt(pkgObj.assignId) != 0) {
        document.getElementById('hdnAssignId').value = pkgObj.assignId;
    }
    if (pkgObj.pkgType !== 'Discount') {
        document.getElementById('txtTenderAmt').value = 0;
        document.getElementById('txtAdvance').value = 0;
        document.getElementById('txtTenderAmt').setAttribute('disabled', 'disabled');
        document.getElementById('txtAdvance').setAttribute('disabled', 'disabled');
        /* document.getElementById('txtDiscount').value = 0;
        document.getElementById('txtDiscountAmt').value = 0; */
        document.getElementById('rdrPercentage').click();
        document.getElementById('txtDiscount').setAttribute('disabled', 'disabled');
        document.getElementById('txtDiscountAmt').setAttribute('disabled', 'disabled');
        document.getElementById('rdrAmt').disabled = true;
        document.getElementById('rdrPercentage').disabled = true;
        $('#drpAdvanceType option').length === 3 && $('#drpAdvanceType').append('<option value="Package">Package</option>');
        $('#hdnTmpAdvType').val($('#drpAdvanceType').val());
        $('#drpAdvanceType').val('Package');
        var o = { contorlToEnable: 'txtAdvance:txtDiscount:txtDiscountAmt:rdrAmt:rdrPercentage', enableValue: '', disableValue: 'Package' };
        //document.getElementById('drpAdvanceType').disabled = true
        if ($._data(drpAdvanceType, 'events')["change"] === undefined) {
            $('#drpAdvanceType').on('change.AttachedEvent', o, enableContolOnValue);
        }
        else if ($._data(drpAdvanceType, 'events')["change"][0]["namespace"] !== 'AttachedEvent') {
            $('#drpAdvanceType').on('change.AttachedEvent', o, enableContolOnValue);
        }

        if (pkgObj.pkgType === 'Flat Qty' && $('#hdnUpdate').val() === "1" && $('#hdnIsPkgBooking').val() === "False") {
            $('#drpAdvanceType').val('Cash');
        }
   
        if (pkgObj.pkgType === 'Flat Qty') {
            if ($('#grdEntry > tbody > tr').size() >= 2) {              
                $('#drpAdvanceType').attr('disabled', true);
            }
            else {
                $('#drpAdvanceType').attr('disabled', false);
            }
            if ($('#hdnTmpPkgType').val() == "1") {
                $('#drpAdvanceType').val($('#hdnTmpAdvType').val());
                $('#hdnTmpPkgType').val('0');  
            }
        }
        else {
            $('#drpAdvanceType').attr('disabled', false);
         }     
           
    }
    else {
        //document.getElementById('lowerRowCenter').children[0].children[0].children.textContent = 'Pending Cloth :';
        /* var me2 = document.getElementById('lowerRowCenter').children[0].children[0].children[2].children[0].textContent;
        var me3 = document.getElementById('lowerRowCenter').children[0].children[0].children[1].children[0].textContent;
        document.getElementById('lowerRowCenter').children[0].children[0].children[2].children[0].textContent = me2.replace(/.*:/g, 'Pending Cloth :');
        document.getElementById('lowerRowCenter').children[0].children[0].children[2].children[0].textContent = me3.replace(/.*:/g, 'Pending Amount :');
        */
        //document.getElementById('lowerRowCenter').children[4].textContent = 'Pending Amount :';
        $('#drpAdvanceType option').eq(3).remove();
        $('#drpAdvanceType').off('change.AttachedEvent');
        document.getElementById('txtTenderAmt').removeAttribute('disabled');
        document.getElementById('txtAdvance').removeAttribute('disabled');
        document.getElementById('txtDiscount').removeAttribute('disabled');
        document.getElementById('txtDiscountAmt').removeAttribute('disabled');
        document.getElementById('rdrAmt').disabled = false;
        document.getElementById('rdrPercentage').disabled = false;
        document.getElementById('drpAdvanceType').disabled = false;
    }
    if (document.getElementById('txtDiscount').style['display'] !== 'none')
        txtDiscountkeyUpHandler();
    else
        txtDiscountAmtKeyUpHandler();

    $('#txtQty').focus();
}

function compareDates(date1, date2) {
    var dFst = new Date(date1);
    var dScnd;
    if (!date2)
        dScnd = new Date();
    else
        dScnd = new Date(date2);

    if (dScnd.getFullYear() < dFst.getFullYear())
        return true;
    else if (dScnd.getFullYear() === dFst.getFullYear()) {
        if (dScnd.getMonth() < dFst.getMonth())
            return true;
        else if (dScnd.getMonth() === dFst.getMonth()) {
            if (dScnd.getDate() <= dFst.getDate())
                return true;
            else
                return false;
        }
        else
            return false;
    }
    else
        return false;
}

function enableContolOnValue(e, contorlToEnable, enableValue, disableValue) {
    var _val = $(e.target).val();
    var allItemsTxt = e.data.contorlToEnable;
    var allItems = $();
    allItemsTxt.split(':').forEach(function (v, i) { var it = $('#' + v); allItems = allItems.add(it); });
    if (e.data.enableValue) {
        if (e.data.enableValue === _val) {
            $.prototype.removeAttr.call(allItems, 'disabled');
        }
        else {
            $.prototype.attr.call(allItems, 'disabled', true);
        }
    }
    else if (e.data.disableValue) {
        if (e.data.disableValue === _val) {
            console.info('in here');
            // if disabling, set the value in net amt to be the value in package
            $.prototype.attr.call(allItems, 'disabled', true);
            recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1);
            //$('#txtAdvance').val($('#txtBalance').val()); // swap advance and bal
            $('#txtBalance').val('0'); // swap advance and bal
            /* On the keydown for rate */
            $('#txtRate, #txtExtraRate1, #txtExtraRate2').on('keydown.package', function (e) {
                if (e.which !== 13 && e.which !== 9 && e.which !== 61 && e.which !== 107) {
                    return false;
                }
            });
            /* set def rate as per package */
            $('#txtRate').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
            var tmpTax = calcJustTaxFromHidden();
            // now the booking is done in package, set the tax as package
            if ($('#hdnPackageTaxPerItemPrice').val().split(':')[0] === 'EXCLUSIVE') {
                recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1);
                $('#txtSrTax').val(tmpTax);
            }
            else {
                $('#txtSrTax').val(0);
                //$('#txtTotal').val(parseFloat($('#txtTotal').val()) - tmpTax);
                //$('#txtAdvance').val($('#txtTotal').val());
            }
        }
        else {
            $.prototype.removeAttr.call(allItems, 'disabled');
            if ($('#txtAdvance').val() !== '0') {
                recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1);
                //$('#txtBalance').val($('#txtAdvance').val()); // swap advance and bal
                $('#txtAdvance').val('0'); // swap advance and bal
                // the bal would be negative, if the tax calc in bk is inclusive, while on customer is exclusive, and drop down is changed frm package to something else
                if (Number(txtBalance.value) < 0) {
                    txtBalance.value = 0;
                }
                $('#txtBalance').val($('#txtTotal').val());
                $('#txtRate, #txtExtraRate1, #txtExtraRate2').off('keydown.package'); // off the keydown for package
                $('#txtRate').val('' + $('#hdnDefaultItemProcessRate').val() + ''); // set def rate
                var tmpTaxRemove = calcJustTaxFromHidden();
                // if booking is not being done if package and tax is exclusive, set it
                if (hdnIsTaxExclusive.value === 'true') {
                    recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1);
                    $('#txtSrTax').val(tmpTaxRemove);
                }
                else {
                    $('#txtSrTax').val(0);
                    // $('#txtTotal').val(parseFloat($('#txtTotal').val()) - tmpTaxRemove);
                    // $('#txtAdvance').val($('#txtTotal').val());
                }
            }
            $('#hdnTmpPkgType').val('1');
        }
    }
}

function calcJustTaxFromHidden() {
    var _tmpAllLoopingTax = 0;
    var tt = $('#hdnAllTax').val().split('`')
    for (var i = 0; i < tt.length; i++) {
        var tmp = tt[i].split('_');
        for (var j = 0; j < tmp.length; j++) {
            var ll = tmp[j].split(':');
            for (var k = 0; k < ll.length; k++) {
                //console.log(' i : j : k : and tax => ' + i.toString() + ' <=> ' + j.toString() + ' <=> ' + k.toLocaleString() + ' <=> ' + _tmpAllLoopingTax);
                _tmpAllLoopingTax = parseFloat(_tmpAllLoopingTax) + parseFloat(ll[k]);
                //console.log(' i : j : k : and tax => ' + i.toString() + ' <=> ' + j.toString() + ' <=> ' + k.toLocaleString() + ' <=> ' + _tmpAllLoopingTax);
            }
        };
    };
    return _tmpAllLoopingTax.toFixed(2);
}

function setPackageObj(pkgObject, pkgStringToSplit) {
    pkgObject.pkgName = pkgStringToSplit.split(':')[0];
    pkgObject.totalValue = pkgStringToSplit.split(':')[1];
    pkgObject.startDate = pkgStringToSplit.split(':')[2];
    pkgObject.endDate = pkgStringToSplit.split(':')[3];
    pkgObject.assignId = pkgStringToSplit.split(':')[4];
    pkgObject.consumedValue = pkgStringToSplit.split(':')[5];
    pkgObject.remainingValue = pkgStringToSplit.split(':')[6];
    pkgObject.pkgType = pkgStringToSplit.split(':')[7];
    pkgObject.recurrence = pkgStringToSplit.split(':')[8];
    pkgObject.lastEndDate = pkgStringToSplit.split(':')[9];
    pkgObject.taxType = pkgStringToSplit.split(':')[10];
    pkgObject.perItemPrice = pkgStringToSplit.split(':')[11];
    return pkgObject;
}

function PendingClothesForPackage(CustDetailsArray, maxLimit, timerIdsToClear, pkgBalQty) {
    // if called from pkg one and pkgBalQty is not null, just set and return
    if (pkgBalQty !== undefined) {
        $('#LblPkgBal').text(pkgBalQty);
        // check if in edit mode
        if (isInEditMode.value === 'true' && hdnPackageTypeRecNo.value.split(':')[0] === 'Flat Qty') {
            UpdateRemainingInCaseOfEdit();
        }
        return;
    }

    // only do this if the package dates is visible, else in other case, when there is no pakcage, pending clothes and what not show by default
    // but instead of directy calling quitzies, we should rather wait, as the ajax to pull package might still be running
    // the max limit is the time we check for the waitout!
    if ($('#PkgDates').is(':visible') === false) {
        var id = setTimeout(function () {
            timerIdsToClear.push(id);
            PendingClothesForPackage(CustDetailsArray, maxLimit--, timerIdsToClear);
        }, 40);
        return;
    }
    else {
        // first clear prev timeouts
        for (var k = 0; k < timerIdsToClear.length; k++) {
            // (function () { console.log('clearing id for interval ' + timerIdsToClear[k] + ' '); clearTimeout(timerIdsToClear[k]); })();
            console.info('clearing id for interval ' + timerIdsToClear[k]);
            clearTimeout(timerIdsToClear[k]);
        }
        // now we are here, then either the package is fetched, if there is one
        // or the timeout has expired, in which case we are quite safe to assume there is none
        var htm = $('#PkgDates').html().trim();
        // if there is pending id, that was added before, then just change else add
        var pendingDivPkg = $('#dynamicPendingPkg').length === 1 ? $('#dynamicPendingPkg') : $('<div id="dynamicPendingPkg" class="otherClass" style="displaly:none;">' + htm + '</div>')

        pendingDivPkg.find('span').eq(0).text('Pending Garments :');
        pendingDivPkg.find('span').eq(1).text(CustDetailsArray[8]);
        pendingDivPkg.find('span').eq(2).text('Balance Garments :');
        // <<< DONT DO THIS >>> pendingDivPkg.find('span').eq(3).text(CustDetailsArray[8]);
        // if not added then add
        if (!$('#dynamicPendingPkg').is(':visible')) {
            $('#PkgDates').after(pendingDivPkg);
            if ($('#PkgDates').next().attr('id') === 'dynamicPendingPkg') {
                $('#dynamicPendingPkg').show();
            }
        }
        $('#dynamicPendingPkg #LblPkgName').attr('id', 'LblPkgCloth');
        $('#dynamicPendingPkg #lblPkgEndDate').attr('id', 'LblPkgBal');
    }
}

// Step 2.B.D. this is the method that checks or uncheck the sms and/or email checkboxes
function setCommunicationCheckBox(communicationMeans, fromCustomerSearch) {
    switch (communicationMeans) {
        case 'NA':
            {
                $('#chkSendsms').attr('checked', false);
                $('#chkEmail').attr('checked', false);
                if (fromCustomerSearch) {
                    $('#drpCommLbl').val('NA');
                }
            }
            break;
        case 'SMS':
            {
                $('#chkSendsms').attr('checked', true);
                $('#chkEmail').attr('checked', false);
                if (fromCustomerSearch) {
                    $('#drpCommLbl').val('SMS');
                }
            }
            break;
        case 'Email':
            {
                $('#chkSendsms').attr('checked', false);
                $('#chkEmail').attr('checked', true);
                if (fromCustomerSearch) {
                    $('#drpCommLbl').val('Email');
                }
            }
            break;
        case 'Both':
            {
                $('#chkSendsms').attr('checked', true);
                $('#chkEmail').attr('checked', true);
                if (fromCustomerSearch) {
                    $('#drpCommLbl').val('Both');
                }
            }
            break;
        default:
            {
                $('#chkSendsms').attr('checked', false);
                $('#chkEmail').attr('checked', false);
                if (fromCustomerSearch) {
                    $('#drpCommLbl').val('NA');
                }
            }
            break;
    }
}

// Step 2.F.Pre this function will add the discount if any, to the discount option and recalc the grid
function addDiscountFromAddCustomer(argDiscount) {
    $('#txtDiscount').val(argDiscount);
    if (parseFloat(argDiscount) > 0) {
        var keyup = jQuery.Event('keyup');
        keyup.which = 13;
        txtDiscountkeyUpHandler(keyup);
    }
}

// Step 2.C.2 Find All Priority and Code
function findPriorityCode() {
    $.ajax({
        url: '../AutoComplete.asmx/FindAllPriority',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            var _result = response.d;
            $('#hdnAllPriorities').val(_result);
        },
        error: function (response) {
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
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            async: false,
            data: "{'arg' :'" + argPriority + "'}",
            success: function (response) {
                _priorityCode = response.d;
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
    var _argList = "{ args: ['" + $('#txtPriority').val();
    $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
    $('#pnlNewCustomer').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
    var _prcCode = findThePriorityCode($('#txtPriority').val());
    if (_prcCode == '') {
        _prcCode = 0;
    }
    _argList = _argList + "', '" + parseInt(_prcCode);
    _argList = _argList + "', '" + $('#txtCAddress').val().toUpperCase();
    _argList = _argList + "', '" + $('#txtAreaLocaton').val().toUpperCase();
    _argList = _argList + "', '" + $('#txtMobile').val().toUpperCase();
    _argList = _argList + "', '" + $('#txtCName').val().toUpperCase();
    _argList = _argList + "', '" + $('#txtRemarks1').val().toUpperCase();
    _argList = _argList + "', '" + $('#drpTitle').val();
    _argList = _argList + "', '" + $('#txtDefaultDiscount').val();
    _argList = _argList + "', '" + $('#drpCommunicationMeans').val();
    _argList = _argList + "', '" + $('#ddlRateListNewCustomer').val();
    _argList = _argList + "', '" + $('#txtEmail').val() + "'] }";
    //                _argList = _argList + '&args=' + $('#txtBDate').val().toUpperCase();
    //                _argList = _argList + '&args=' + $('#txtADate').val().toUpperCase();
    // remove the focus out handler from txtCutomerName, so that when losing focus this time
    // doesn't causes a infinite loop
    // Step 2.E remove handler, attach at txtQty focus
    // $('body').off('keydown.AttachedEvent', '#txtCustomerName', txtCustomerNamekeydown);
    $('#btnOkay').attr('disabled', true);
    // Step 2.F Add the customer
    $.ajax({
        url: '../AutoComplete.asmx/AddCustomer',
        type: 'POST',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        data: _argList,
        success: function (response) {
            var result = response.d;
            // the values is in form of name, address, mobile, priority and remark
            if (result.split(':')[0] == 'Added') {
                $('#txtCustomerName').val($('#txtCName').val().trim().toUpperCase());
                $('#lblAddress').text($('#txtCAddress').val().toUpperCase());
                $('#lblMobileNo').val($('#txtMobile').val().toUpperCase());
                // get the priority for this drop down value
                // ERROR
                $('#lblPriority').text($('#txtPriority').val().toUpperCase());
                $('#lblRemarks').text($('#txtRemarks1').val().toUpperCase());
                $('#ddlRateList').val($('#ddlRateListNewCustomer').val());
                addDiscountFromAddCustomer($('#txtDefaultDiscount').val());
                // check the sms and/or email box, depending on the set value(s)
                setCommunicationCheckBox($('#drpCommunicationMeans').val(), true);
                $("#lblEmailId").val($('#txtEmail').val());
                $('#pnlNewCustomer').dialog('close');
                clearDataFromAddCustDialog();
                $('#btnOkay').attr('disabled', false);
                $('#txtQty').focus();
                $('#hdnCustCode').val(result.split(':')[1]);
                $('#grdEntry').unblock();
                GetPackageDetails($('#hdnCustCode').val());
            }
            else if (result == 'Exists!') {
                alert('A customer with given details already exists!');
                clearDataFromAddCustDialog();
                $('#btnOkay').attr('disabled', false);
                $('#txtCName').focus();
            }
        },
        error: function (response) {
        }
    });
};

// Step 2.H this fucntion will clear the previous data that was enterd in the add customer dialog box
function clearDataFromAddCustDialog() {
    $('#drpPriority').val('');
    $('#txtCAddress').val('');
    $('#txtAreaLocaton').val('');
    $('#txtMobile').val('');
    $('#txtPriority').val('');
    $('#txtCName').val('');
    $('#txtRemarks1').val('');
    $('#txtDefaultDiscount').val('');
    $('#drpTitle').val('');
    $('#drpCommunicationMeans').val('');
    $('#txtEmail').val('');
    $('#txtCustomerName').focus();
    $('#grdEntry').unblock({ fadeOut: 0 });
    $('#pnlNewCustomer').unblock({ fadeOut: 0 });
};

// Step 2.I. this function checks if there is valid entry in customer adding dialog box
function checkIfCustomerIsValid(event) {
    var _commMeans = $('#drpCommunicationMeans option:selected').text() + '';
    if ($('#txtCName').val() == '') {
        $('#btnOkay').attr('disabled', true);
        alert('Please enter a valid value for customer name!');
        $('#btnOkay').attr('disabled', false);
        $('#txtCName').focus();
        return false;
    }
    if ($('#txtCAddress').val() == '') {
        $('#btnOkay').attr('disabled', true);
        alert('Please enter a valid value for customer address!');
        $('#btnOkay').attr('disabled', false);
        $('#txtCAddress').focus();
        return false;
    }
    if (parseFloat($('#txtDefaultDiscount').val()) > 100) {
        $('#btnOkay').attr('disabled', true);
        alert('Please enter a valid value for discount!');
        $('#btnOkay').attr('disabled', false);
        $('#txtDefaultDiscount').focus();
        return false;
    }
    if ((_commMeans.indexOf('Email') > -1) && ($('#txtEmail').val() == '')) {
        $('#btnOkay').attr('disabled', true);
        alert('Please enter email!');
        $('#btnOkay').attr('disabled', false);
        $('#txtEmail').focus();
        return false;
    }
    if ((_commMeans.indexOf('SMS') > -1) && ($('#txtMobile').val() == '')) {
        $('#btnOkay').attr('disabled', true);
        alert('Please enter mobile no!');
        $('#btnOkay').attr('disabled', false);
        $('#txtMobile').focus();
        return false;
    }

    return true;
}

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
        $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
        findItemAndProcessRate($('#txtName').val().trim().toUpperCase(), $(event.target).val(), $(event.target).closest('tr').find('.clsRate'));
        $('#grdEntry').unblock({ fadeOut: 0 });
        $('#txtRate').focus();
        return;
    }
    else {
        $('#txtProcess').focus();
        alert('The entered value: "' + $(event.target).val() + '" doesn\'t exists in the database! Please enter a valid value!');
        $(event.target).focus();
        return false;
    }
    $('#grdEntry').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0} });
    // 3.A Find if process exists
    /*
    $.ajax({
    url: '../AutoComplete.asmx/CheckIfProcessExists',
    type: 'GET',
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
    $('#grdEntry').unblock({ fadeOut: 0 });
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
    $('#grdEntry').unblock({ fadeOut: 0 });
    }
    },
    error: function (response) {
    }
    });
    */
};

// Step 10.C function that finds the rate for given process and item
// the last field is used to find which field will be set by the text
// not used till now
function findItemAndProcessRate(argItem, argPrc, argFieldToSet) {
    if (argItem == '' || argPrc == '') { return; }
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
        // make 3 calls
        $.ajax({
            url: '../AutoComplete.asmx/GetItemRateForProcess',
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            data: "arg='" + _mainData + ":true'&rateListId=" + $('#ddlRateList').val() + "",
            success: function (response) {
                var result = response.d;
                $('#txtProcess').val(result.split(':')[0]);
                // if not the packageCondition
                if (isPackageCondition()) {
                    $('#txtRate').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                    $('#hdnPrvRate').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                }
                else {
                    $('#txtRate').val(result.split(':')[1]);
                    $('#hdnPrvRate').val(result.split(':')[1]);
                }
            },
            error: function (response) {
            }
        });
        if (_prc1 == '') { return; };
        $.ajax({
            url: '../AutoComplete.asmx/GetItemRateForProcess',
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            data: "arg='" + _mainData1 + "'&rateListId=" + $('#ddlRateList').val() + "",
            success: function (response) {
                var result = response.d;
                if (isPackageCondition()) {
                    $('#txtExtraRate1').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                    $('#hdnPrvRate1').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                }
                else {
                    $('#txtExtraRate1').val(result);
                    $('#hdnPrvRate1').val(result);
                }
            },
            error: function (response) {
            }
        });
        if (_prc2 == '') { return; };
        $.ajax({
            url: '../AutoComplete.asmx/GetItemRateForProcess',
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            data: "arg='" + _mainData2 + "'&rateListId=" + $('#ddlRateList').val() + "",
            success: function (response) {
                var result = response.d;
                if (isPackageCondition()) {
                    $('#txtExtraRate2').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                    $('#hdnPrvRate2').val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                }
                else {
                    $('#txtExtraRate2').val(result);
                    $('#hdnPrvRate2').val(result);
                }
            },
            error: function (response) {
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
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            data: "arg='" + _newData + "'&rateListId=" + $('#ddlRateList').val() + "",
            success: function (response) {
                var result = response.d;
                if (argFieldToSet != '') {
                    if (isPackageCondition()) {
                        $(argFieldToSet).val($('#hdnPackageTaxPerItemPrice').val().split(':')[1]);
                    }
                    else {
                        $(argFieldToSet).val(result);
                    }
                    if (argFieldToSet.attr('id') === 'txtExtraRate1') $('#hdnPrvRate1').val(result);
                    else if (argFieldToSet.attr('id') === 'txtExtraRate2') $('#hdnPrvRate2').val(result);
                    else if (argFieldToSet.attr('id') === 'txtRate') $('#hdnPrvRate').val(result);
                    $(argFieldToSet).focus();
                    $(argFieldToSet).select();
                }
            },
            error: function (response) {
            }
        });
        return false;
    }
};

// Step 3.B.Pre
// This is the function that finds the length X breadth X panel (only LB or PL will be active at a time)
// we could then multiply by it, in findAmtForProcess, findTaxForProcess, findDiscountForProcess, and also FindRowCalc
function findLBP() {
    var _len = $('#txtLen').val();
    var _bth = $('#txtBreadth').val();
    var _numCur = $('#txtNumPanels').val();
    var _lb = parseFloat(_len) * parseFloat(_bth) * parseFloat(_numCur);
    return _lb;
}

// Step 3.B Find row calc
// arg is the row S.No, not using this right now, because add or update, both are happening at the grdHeader
// if arg == -1, then calc for grid header, else calc for the given row
function findRowCalc(argSNo) {
    // the lenght and breadth in case of pieces and panel
    // moved to LBP
    var _lbp = findLBP();
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
                                            + parseFloat($('#txtExtraRate1').val())
                                            + (parseFloat($('#hdnUrgentRateApplied').val()) * parseFloat($('#txtExtraRate1').val()) / 100)
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
    return parseFloat(_returnAmtForGivenRow).toFixed(2) * parseFloat(_lbp).toFixed(2);
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
    var _lbp = findLBP();
    return parseFloat(_prcAmt) * parseFloat(_lbp);
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
function findTaxForProcess(argPrc, argPrcAmount, argPrcDiscount, argIsDeleting) {
    var _valToReturn;
    if (argPrc == '') {
        _valToReturn =
                                    {
                                        'tax': 0,
                                        'tax1': 0,
                                        'tax2': 0,
                                        'totalTax': 0
                                    }
        setTaxableAmt(0, argIsDeleting);
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
            setTaxableAmt(0, argIsDeleting);
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
    // temp variables to hold the tax, tax1, and tax2 values
    // we'll add them to make the total tax
    var _t1, _t2, _t3, _x1, _x2, _x3, _ttt;
    // first set the pacakge tax to the default, in case of no pacakge or other types f package where tax is not applicable
    var _pkgTax = $('#hdnIsTaxExclusive').val();
    if (isPackageCondition()) {
        // now modify the tax
        _pkgTax = $('#hdnPackageTaxPerItemPrice').val().split(':')[0];
    }
    if ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Value / Benefit') {
        _pkgTax = 'false';
    }
    // if tax is exclusive
    //    if ($('#hdnIsTaxExclusive').val() == 'true') {
    // now instead of checking the "hdnIsTaxExclusive", check for package tax, if there is no pacakge, it is set to default, if there is then it reflects the packge type
    // true in case of default, EXCLUSIVE in case of package
    if (_pkgTax == 'true' || _pkgTax == 'EXCLUSIVE') {
        // if before
        if ($('#hdnTaxBefore').val() == "true") {
            // just calc here
            _stMain = ((parseFloat(argPrcAmount) * parseFloat(_taxRate)) / 100);
            _stCess = ((_stMain * _taxRate1) / 100);
            _stLast = ((_stMain * _taxRate2) / 100);
            _t1 = parseFloat(_stMain);
            _t2 = parseFloat(_stCess);
            _t3 = parseFloat(_stLast);
            _x1 = _t1.toFixed(3);
            _x2 = _t2.toFixed(3);
            _x3 = _t3.toFixed(3);
            _ttt = parseFloat(_x1) + parseFloat(_x2) + parseFloat(_x3);
            //_stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
            _valToReturn =
                                    {
                                        'tax': _stMain.toFixed(3),
                                        'tax1': _stCess.toFixed(3),
                                        'tax2': _stLast.toFixed(3),
                                        'totalTax': _ttt
                                    };
            setTaxableAmt(argPrcAmount, argIsDeleting);
        }
        else {
            //var _disAmt = (argPrcDiscount * argPrcAmount) / 100;
            var _disAmt = argPrcDiscount;
            var _newAmtForPrc = parseFloat(argPrcAmount) - parseFloat(_disAmt);
            _stMain = ((parseFloat(_newAmtForPrc) * parseFloat(_taxRate)) / 100);
            _stCess = ((_stMain * _taxRate1) / 100);
            _stLast = ((_stMain * _taxRate2) / 100);
            _t1 = parseFloat(_stMain);
            _t2 = parseFloat(_stCess);
            _t3 = parseFloat(_stLast);
            _x1 = _t1.toFixed(3);
            _x2 = _t2.toFixed(3);
            _x3 = _t3.toFixed(3);
            _ttt = parseFloat(_x1) + parseFloat(_x2) + parseFloat(_x3);
            //_stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
            _valToReturn =
                                    {
                                        'tax': _stMain.toFixed(3),
                                        'tax1': _stCess.toFixed(3),
                                        'tax2': _stLast.toFixed(3),
                                        'totalTax': _ttt
                                    };
            setTaxableAmt(_newAmtForPrc, argIsDeleting);
        }
    }
    // else tax is inclusive
    else {
        if ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Value / Benefit') {
            $('#hdnTaxBefore').val('false');
        }
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
            _t1 = parseFloat(_stMain);
            _t2 = parseFloat(_stCess);
            _t3 = parseFloat(_stLast);
            _x1 = _t1.toFixed(3);
            _x2 = _t2.toFixed(3);
            _x3 = _t3.toFixed(3);
            _ttt = parseFloat(_x1) + parseFloat(_x2) + parseFloat(_x3);
            //_stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
            _valToReturn =
                                    {
                                        'tax': _stMain.toFixed(3),
                                        'tax1': _stCess.toFixed(3),
                                        'tax2': _stLast.toFixed(3),
                                        'totalTax': _ttt
                                    };
            setTaxableAmt(_bookingAmt, argIsDeleting);
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
            _t1 = parseFloat(_stMain);
            _t2 = parseFloat(_stCess);
            _t3 = parseFloat(_stLast);
            _x1 = _t1.toFixed(3);
            _x2 = _t2.toFixed(3);
            _x3 = _t3.toFixed(3);
            _ttt = parseFloat(_x1) + parseFloat(_x2) + parseFloat(_x3);
            //_stTotal = parseFloat(_stMain) + parseFloat(_stCess) + parseFloat(_stLast);
            _valToReturn =
                                    {
                                        'tax': _stMain.toFixed(3),
                                        'tax1': _stCess.toFixed(3),
                                        'tax2': _stLast.toFixed(3),
                                        'totalTax': _ttt
                                    };
            setTaxableAmt(_bookingAmt, argIsDeleting);
        }
    }
    return _valToReturn;
};

// Step 3.E.1 this function set the taxable amt
// argVal : the value to set/add to previous value
function setTaxableAmt(argVal, isDeleting) {
    var _prvTaxable = $('#hdnTaxableAmt').val();
    if (_prvTaxable == '') {
        _prvTaxable = '0';
    }
    var _amt = parseFloat(_prvTaxable) + parseFloat(argVal);
    if (isDeleting) {
        $('#hdnTaxableAmt').val(_prvTaxable - argVal);
    }
    else {
        $('#hdnTaxableAmt').val(_amt);
    }
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
        _rate = (parseFloat(rate) * 10 + parseFloat(_urgentRateAmt) * 10) / 10;
        _allPrcArray = '(' + prc + '@' + _rate + ')';
        if (prc1 != '') {
            _rate1 = (parseFloat(rate1) * 10 + parseFloat(_urgentRateAmt1) * 10) / 10;
            _allPrcArray = _allPrcArray + ',(' + prc1 + '@' + _rate1 + ')';
        }
        if (prc2 != '') {
            _rate2 = (parseFloat(rate2) * 10 + parseFloat(_urgentRateAmt2) * 10) / 10;
            _allPrcArray = _allPrcArray + ',(' + prc2 + '@' + _rate2 + ')';
        }
    }
    return _allPrcArray;
}

// Step 3.G
// this will call findAmt, findDis, and findTax 3 times for each prc and rate
// and will add the values in hidden field, or update them based on parameter passed
// params are just default as can be seen from their args
function ComputeRowDisTaxAmt(argPrc, argRate, argPrc1, argRate1, argPrc2, argRate2, argQty, argIsUpdating, argRowNumber, argIsRecomputing, argIsDeleting) {
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
    _curPrcTaxAry = findTaxForProcess(argPrc, _curPrcAmount, _curPrcDis, argIsDeleting);
    _curPrcTax = _curPrcTaxAry.totalTax;
    _curDisStr = _curPrcDis + '';
    _curTaxStr = _curPrcTaxAry.tax + ':' + _curPrcTaxAry.tax1 + ':' + _curPrcTaxAry.tax2 + '';
    // second time
    _curPrcDis1 = findDiscountForProcess(argPrc1, argRate1, false, argRowNumber);
    _curPrcAmount1 = findAmountForProcess(argRate1, argQty, false);
    _curPrcTax1Ary = findTaxForProcess(argPrc1, _curPrcAmount1, _curPrcDis1, argIsDeleting);
    _curPrcTax1 = _curPrcTax1Ary.totalTax;
    _curDisStr = _curDisStr + ':' + _curPrcDis1;
    _curTaxStr = _curTaxStr + '_' + _curPrcTax1Ary.tax + ':' + _curPrcTax1Ary.tax1 + ':' + _curPrcTax1Ary.tax2 + '';
    // third time
    _curPrcDis2 = findDiscountForProcess(argPrc2, argRate2, false, argRowNumber);
    _curPrcAmount2 = findAmountForProcess(argRate2, argQty, false);
    _curPrcTax2Ary = findTaxForProcess(argPrc2, _curPrcAmount2, _curPrcDis2, argIsDeleting);
    _curPrcTax2 = _curPrcTax2Ary.totalTax;
    _curDisStr = _curDisStr + ':' + _curPrcDis2;
    _curTaxStr = _curTaxStr + '_' + _curPrcTax2Ary.tax + ':' + _curPrcTax2Ary.tax1 + ':' + _curPrcTax2Ary.tax2 + '';
    // add discount of row
    _totalDiscOfRow = parseFloat(_curPrcDis) + parseFloat(_curPrcDis1) + parseFloat(_curPrcDis2);
    _totalTaxOfRow = parseFloat(_curPrcTax) + parseFloat(_curPrcTax1) + parseFloat(_curPrcTax2);
    // store these values in previous values of hidden fields
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
        // find the row index of current hdnAllTax and CurrentAllDis
        // that is in the form of dis:dis1:dis2_rdis:rdis1:rdis2_r1dis:r1dis1:r1dis2_r2dis:r2dis1:r2:dis2... and so on
        setAllTaxDisOnUpdating(argRowNumber, _curDisStr, _curTaxStr, '_', '`', argIsDeleting);
    }
    else { // same as (argIsRecomputing == false && argIsUpdating == true)
        // the user is editing
        // the differences would be, the discount and tax would be RETURNED
        // instead of being performed calculations upon
        // also, the row index would be found and hdnAllTax,
        // and hdnAllDiscount would be updated and not addended
        // find the row index of current hdnAllTax and CurrentAllDis
        // that is in the form of dis:dis1:dis2_rdis:rdis1:rdis2_r1dis:r1dis1:r1dis2_r2dis:r2dis1:r2:dis2... and so on
        setAllTaxDisOnUpdating(argRowNumber, _curDisStr, _curTaxStr, '_', '`', argIsDeleting);
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
    // and that would be equal to previous discount + current discount and previous tax + current tax
    calculateLowerGridDetails(_totalDiscOfRow, _totalTaxOfRow, true, true, argIsRecomputing);
}

// Step 3. H
// calculate the lower grid details
function calculateLowerGridDetails(argCurRowDiscount, argCurRowTax, argAddDis, argAddTax, argIsRecomputing) {
    var _prvDis, _prvTax;
    var _isPkg = isPackageCondition();
    var _pkgTaxInclusive = $('#hdnPackageTaxPerItemPrice').val().split(':')[0] === 'INCLUSIVE';

    if ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Value / Benefit') {
        _isPkg = true;
        _pkgTaxInclusive = true;
    }

    if (argIsRecomputing) {
        _prvDis = $('#hdnDisAmtRecomp').val();
        /* SHAKTIMAAN
        _prvTax = $('#hdnTaxAmtRecomp').val();
        */
    }
    else {
        _prvDis = $('#lblDisAmt').text();
        /* SHAKTIMAAN
        _prvTax = $('#txtSrTax').val();
        */
    }
    var _tmpAllLoopingTax = 0;
    _tmpAllLoopingTax = calcJustTaxFromHidden();
    _prvTax = parseFloat(_tmpAllLoopingTax).toFixed(2);
    if (_prvTax == '' || _prvTax == null) {
        _prvTax = 0;
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
            $('#hdnDisAmtRecomp').val(_disVal);
            $('#hdnDisAmtRecomp').text(_disVal);
        }
    }
    else {
        // also, only set if discountamt is NOT visible, if it is,
        // then discount will not be touched
        if ($('#txtDiscountAmt').is(':visible') == false) {
            $('#lblDisAmt').text(_disVal.toFixed(2));
            $('#hdnDiscountValue').val(_disVal);
        }
    }
    // Step 3.H.2
    // set the tax value
    if (argAddTax) {
        /* SHAKTIMAAN
        _taxValue = parseFloat(_prvTax) + parseFloat(argCurRowTax);
        */
        _taxValue = parseFloat(_prvTax);
    }
    else {
        _taxValue = parseFloat(_prvTax) - parseFloat(argCurRowTax);
    }
    // if recomputing
    if (argIsRecomputing) {
        // then don't set the discount here, but set in a different field,
        // so that at the end of loop it can be accessed and updated as required
        //inclusive case
        // also do the same if package and in inclusive
        // if package and is inlclusive, its priority takes over
        if (_isPkg && _pkgTaxInclusive) {
            $('#hdnTaxAmtRecomp').val('0');
        }
        // else if pacakge and is NOT inclusive, i.e. is exclusive then
        else if (_isPkg && !_pkgTaxInclusive) {
            $('#hdnTaxAmtRecomp').val(_taxValue.toFixed(2));
        }
        // else if not package then check what is the value from default
        else if ($('#hdnIsTaxExclusive').val() == 'false') {
            $('#hdnTaxAmtRecomp').val('0');
        }
        else { // not package and not inclusive (i.e. exclusive!)
            $('#hdnTaxAmtRecomp').val(_taxValue.toFixed(2));
        }
    }
    else {
        // same as above, but for txtSrTax
        if (_isPkg && _pkgTaxInclusive) {
            $('#txtSrTax').val('0');
        }
        // else if pacakge and is NOT inclusive, i.e. is exclusive then
        else if (_isPkg && !_pkgTaxInclusive) {
            $('#txtSrTax').val(_taxValue.toFixed(2));
        }
        // else if not package then check what is the value from default
        else if ($('#hdnIsTaxExclusive').val() == 'false') {
            $('#txtSrTax').val('0');
        }
        else { // not package and not inclusive (i.e. exclusive!)
            $('#txtSrTax').val(_taxValue.toFixed(2));
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
    // if package condition, then remove 0, as the advance now contains the actaul amt and not the one that entered by user
    _NetAmt = parseFloat(_TotalAmt) - (isPackageCondition() ? 0 : parseFloat($('#txtAdvance').val()));
    // $('#txtBalance').val(parseFloat(NetAmt).toFixed(2));
    // Step NEW: 3.H.3
    // when the row is added set the percentage of discount in discount percentage box
    //setDiscountPerc();
    setNetBal(_NetAmt, argIsRecomputing);
}

// Step NEW 3.H.4
// this will set the percentage in discount box
function setDiscountPerc() {
    // if the discount amt is visible
    if ($('#txtDiscountAmt').is(':visible')) {
        // this means the discount is flat and we need to calc the perc
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
        // if there are just 2 rows, the header and the current, then
        if ($('#grdEntry > tbody > tr').size() == 2) {
            recomputeAllGrid(1, 2, false, true, -1);
            //$('#txtAdvance').focus();
            return false;
        }
        else {
            recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1);
            //$('#txtAdvance').focus();
            return false;
        }
    }
}

// Step 3.I this will set the current dis and tax to allTax and allDiscount
// jaadui method
function setAllTaxDisOnUpdating(argRow, argNewDis, argNewTax, argDelimeter, argDelimeterTax, argIsDeleting) {
    // set the discount
    var _prvAllDis = $('#hdnAllDiscount').val();
    var _prvAllDisAry = _prvAllDis.split('' + argDelimeter + '');
    if (!argIsDeleting) {
        _prvAllDisAry[argRow - 1] = argNewDis;
    }
    else {
        _prvAllDisAry.splice(argRow - 1, 1);
    }
    var _newDis = _prvAllDisAry.join('' + argDelimeter + '');
    $('#hdnAllDiscount').val(_newDis);
    // set the tax
    var _prvAllTax = $('#hdnAllTax').val();
    var _prvAllTaxAry = _prvAllTax.split('' + argDelimeterTax + '');
    if (!argIsDeleting) {
        _prvAllTaxAry[argRow - 1] = argNewTax;
    }
    else {
        _prvAllTaxAry.splice(argRow - 1, 1);
    }
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
            //                        $('#hdnBalance').val(_amtToSet.toFixed(2));
            //                        $('#hdnBalance').text(_amtToSet.toFixed(2));
            $('#hdnBalance').val(_amtToSet);
            $('#hdnBalance').text(_amtToSet);
        }
        else {
            // added because of the error I was getting in my browser, when entering 0, it was not adding up proper row
            // and firebug said toFixed is not a function
            if ($.isFunction(_amtToSet.toFixed)) {
                $('#txtBalance').val(_amtToSet.toFixed(2)).triggerHandler('change.package');
                //$('#txtBalance').text(_amtToSet.toFixed(2));
            }
            else {
                $('#txtBalance').val(_amtToSet).triggerHandler('change.package');
            }
        }
    }
    else {
        if (argIsRecomputing) {
            //                        $('#hdnBalance').val(Math.round(_amtToSet));
            //                        $('#hdnBalance').text(Math.round(_amtToSet));
            $('#hdnBalance').val(_amtToSet);
            $('#hdnBalance').text(_amtToSet);
        }
        else {
            $('#txtBalance').val(Math.ceil(_amtToSet)).triggerHandler('change.package');
            //$('#txtBalance').text(Math.round(_amtToSet));
        }
    }
}

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

// Step 5.E this function makes the percentage, if the discount is net
// and user edited or deleted the row
function makeThePercentage() {
    // first check if discount amount is visible,
    // if its not, the the rate is already in discount percentage
    // and will be calculated according to current amount
    if ($('#txtDiscountAmt').is(':visible')) {
        //$('#txtDiscountAmt').focusout();
        var _qty = $('#txtQty').val();
        // txtDiscountAmtKeyUpHandler.call(txtDiscountAmt);
        //$('#txtQty').val()
        var _disPerc = 100 * parseFloat($('#txtDiscountAmt').val()) / parseFloat($('#txtCurrentDue').val());
        _disPerc = _disPerc;
        if (isNaN(_disPerc)) {
            _disPerc = '0';
        }
        // set this perctage to text amount
        $('#txtDiscount').val(_disPerc);
        // set the label to current value
        $('#lblDisAmt').text(parseFloat($('#txtDiscountAmt').val()).toFixed(2));
        $('#hdnDiscountValue').text($('#txtDiscountAmt').val());
        // set the hidden recomp value to this
        $('#hdnDisAmtRecomp').val($('#txtDiscountAmt').val());
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
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            processData: false,
            data: "{argRemarks: '" + argRemark + "'}",
            success: function (response) {
                // don't do anything, just added and we are good
                // rather update the field that is source of remarks
                updateRemarksSource();
            },
            error: function (response) {
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
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            processData: false,
            data: "{argColors: '" + argColor + "'}", // not using encodeURIComponent(argColor) because it was used when datatype was not json
            success: function (response) {
                // don't do anything, just added and we are good
                // rather update the field that is source of remarks
                updateColorSource();
            },
            error: function (response) {
            }
        });
    }
};
// Step 5.H this updates the remarks source
function updateRemarksSource() {
    $.ajax({
        url: '../AutoComplete.asmx/FindRemarksSource',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            $('#hdnValues').val(response.d);
            $('#hdnValues').text(response.d);
            makeLiTaggable('mytags', 'mySingleField', 'hdnValues', '', $('#hdnBindDesc').val(), true, true, -1);
        },
        error: function (response) {
        }
    });
}
// Step 5.H.1 this updates the color source
function updateColorSource() {
    $.ajax({
        url: '../AutoComplete.asmx/FindColorsSource',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        success: function (response) {
            //$('#hdnValues').val($(response).find('string').text());
            $('#LblValuesColor').text(response.d);
            //makeLiTaggable('mytags', 'mySingleField', 'hdnValues', '', $('#hdnBindDesc').val(), true, true, -1);
            var _qty = $('#txtQty').val();
            if (_qty == '') {
                _qty = 1;
            }
            makeLiTaggable('mytagsColor', 'mySingleFieldColor', 'LblValuesColor', '', $('#hdnBindColor').val(), true, true, $('#hdnBindColorToQty').val());
        },
        error: function (response) {
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
    if (isPackageCondition()) {
        // now modify the tax
        $('#txtRate').val('' + $('#hdnPackageTaxPerItemPrice').val().split(':')[1] + '');
        $('#txtRate, #txtExtraRate1, #txtExtraRate2').on('keydown.package', function (e) {
            if (e.which !== 13 && e.which !== 9 && e.which !== 61 && e.which !== 107) {
                return false;
            }
        });
    }
    else {
        $('#txtRate, #txtExtraRate1, #txtExtraRate2').off('keydown.package');
    }
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

// Step 10.A.7 function that sets the unit and unit value for items
function SetUnitAndValue(argUnit, argValue, argRowNumber, argBoolDelete) {
    // if argValue is 1 we are just setting the unit
    var _unit = $('#hdnItemUnit').val();
    var _unitValue = $('#hdnItemUnitValue').val();
    var _unitValToSet = $('#txtLen').val() + 'X' + $('#txtBreadth').val() + ':' + $('#txtNumPanels').val();
    if (_unit == '' && !argBoolDelete && argUnit != -1) {
        $('#hdnItemUnit').val(argUnit);
        return;
    }
    if (_unitValue == '' && !argBoolDelete) {
        $('#hdnItemUnitValue').val(_unitValToSet);
        return;
    }
    // set the Unit
    if (argUnit != -1) {
        if (_unit == '') {
            $('#hdnItemUnit').val(argUnit);
            return;
        }
        else {
            var _rwCount = $('#grdEntry > tbody > tr').size();
            var _size = _unit.split('_');
            var _allRows = $('#grdEntry > tbody > tr');
            var _itemEdit;
            $.each(_allRows, function (index) {
                if ($(this).css('background-color') == 'rgb(255, 255, 0)') {
                    // we are in edit, do the jaadui metod
                    _itemEdit = true;
                }
            });
            if (_itemEdit) {
                // this is the case where a row is in edit mode
                // remove the last one
                var _unitLastAry = _unit.split('_');
                _unitLastAry.splice(argRowNumber - 1, 0, argUnit);
                //_unitLastAry[argRowNumber - 1] = argUnit;
                var _newUtLast = _unitLastAry.join('_');
                $('#hdnItemUnit').val(_newUtLast);
            }
            else {
                // this is case when user is changing without anything
                if (parseInt(_size.length) + parseInt(1) > _rwCount) {
                    var _unitLastAry = _unit.split('_');
                    _unitLastAry.splice(_unitLastAry.length - 1, 1);
                    _unitLastAry.push(argUnit);
                    var _newUtLast = _unitLastAry.join('_');
                    $('#hdnItemUnit').val(_newUtLast);
                }
                else {
                    $('#hdnItemUnit').val(_unit + '_' + argUnit);
                }
            }
            return;
        }
    }
    // set the Unit Value
    if (argUnit == -1 && !argBoolDelete) {
        // this is the case, when user selects a item with length and breadth,
        // and then changes the item, and then changes the item again back to one having length and breadth
        // we will find that if (_unitValue.split(_).length +1 > grdEntry.rows.count)
        // it will only be the case when _unitValue != ''
        if (_unitValue == '') {
            $('#hdnItemUnitValue').val(_unitValToSet);
        }
        else {
            var _rwCount = $('#grdEntry > tbody > tr').size();
            var _size = _unitValue.split('_');
            var _allRows = $('#grdEntry > tbody > tr');
            var _itemEdit = false;
            $.each(_allRows, function (index) {
                if ($(this).css('background-color') == 'rgb(255, 255, 0)') {
                    // we are in edit, do the jaadui metod
                    _itemEdit = true;
                }
            });
            if (_itemEdit) {
                // this is the case where a row is in edit mode
                // remove the last one
                var _unitValueLastAry = _unitValue.split('_');
                _unitValueLastAry.splice(argRowNumber - 1, 0, _unitValToSet);
                //_unitValueLastAry[argRowNumber - 1] = _unitValToSet;
                var _newUtVLast = _unitValueLastAry.join('_');
                $('#hdnItemUnitValue').val(_newUtVLast);
            }
            else {
                // this is case when user is changing without anything
                if (parseInt(_size.length) + parseInt(1) > _rwCount) {
                    var _unitValueLastAry = _unitValue.split('_');
                    _unitValueLastAry.splice(_unitValueLastAry.length - 1, 1);
                    _unitValueLastAry.push(_unitValToSet);
                    var _newUtvLast = _unitValueLastAry.join('_');
                    $('#hdnItemUnitValue').val(_newUtvLast);
                }
                else {
                    $('#hdnItemUnitValue').val(_unitValue + '_' + _unitValToSet);
                }
            }
            return;
            /************/
            /*
            var _rwCount = $('#grdEntry > tbody > tr').size();
            var _size = _unit.split('_');
            if (_size + 1 > _rwCount) {
            // remove the last one
            var _unitValueLastAry = _unitValue.split('_');
            _unitValueLastAry.splice(_unitValueLastAry.length - 1, 1);
            _unitValueLastAry.push(_unitValToSet);
            var _newUtVLast = _unitValueLastAry.join('_');
            $('#hdnItemUnitValue').val(_newUtVLast);
            }
            else {
            $('#hdnItemUnitValue').val(_unitValue + '_' + _unitValToSet);
            }
            */
            /*************************/
        }
        return;
    }
    if (argBoolDelete) {
        // find the row to delete and delete both unit and value
        var _unitAry = _unit.split('_');
        _unitAry.splice(argRowNumber - 1, 1);
        var _newUt = _unitAry.join('_');
        $('#hdnItemUnit').val(_newUt);
        var _unitValueAry = _unitValue.split('_');
        _unitValueAry.splice(argRowNumber - 1, 1);
        var _newUtV = _unitValueAry.join('_');
        $('#hdnItemUnitValue').val(_newUtV);
        return;
    }
    /********** Here ************/
}

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
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        cache: false,
        data: "{ItemName: '" + argItemName + "', ProcessName: '" + argProcess + "', argExtraProcess: '" + argExtraProcess1 + "', argExtraProcess2: '" + argExtraProcess2
                            + "', rate: '" + argRate + "', rate1: '" + argRate1 + "', rate2: '" + argRate2 + "'}",
        success: function (response) {
            // don't do anything, just added and we are good
            // rather update the field that is source of remarks
            // updateRemarksSource(); I DON'T THINK SO
            if (response.d == true) { // naaw! fuck it! I could update the def rate, but not doing this as this is confusing // doing it now, if item and prc are same as def, then update the rate
                if ($('#hdnDefaultItem').val().toUpperCase() == argItemName.toUpperCase() && $('#hdnDefaultProcess').val().toUpperCase() == argProcess.toUpperCase()) {
                    // Set the rate
                    $('#hdnDefaultItemProcessRate').val(argRate);
                    // set the rate to process, in case the setdefaults finishes first, this will do, in case this, then well, the setdefault will do the same
                    $('#txtRate').val(argRate);
                }
            }
        },
        error: function (response) {
        }
    });
}

// Step 10.A.6 function that set the default value in LB and Panels
function setNewTxtValues() {
    $('#txtLen').val('1');
    $('#txtBreadth').val('1');
    $('#txtNumPanels').val('1');
    $('#lblCalcDim').text('1');
}

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
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        data: "argItemName='" + argItemName + "'",
        cache: false,
        async: false,
        success: function (response) {
            // Set items qty
            _numSubItems = response.d;
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
        }
    });
};

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
        $('#' + argGrdId).find('#TmpItemName_' + _iCounter).attr('id', 'TmpItemName_' + _newID);
    };
    // set the new S.No. to appropriate value
    $('#grdEntry_ctl01_lblHSNo').text($('#hdnCurrentValue').val());
};

// Step 10.B.2 this is the event handler for the keyup event of itemName
var txtNameKeyUpHanlder = function (event) {
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
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            data: "itemName='" + newData + "'",
            success: function (response) {
                // if the value is false, the item don't exists
                // show the pop up to add new item, and add the item
                if (response.d == "false") {
                    // first remove the focusout handler from this(txtName) because that might cause an infinite recursion
                    // $('body').off("focusout.AttachedEvent", '#txtName', txtNameKeydownHandler);
                    $('#txtItemName').val(newData);
                    $('#txtItemSubQty').focus();
                    //if (checkForPassword('ItemName')) {
                    $('#pnlItem').dialog({ width: 500, modal: true });
                    //}
                    $(event.target).val('');
                    $(event.target).focus();
                    // $('#hdnItemNameChanged').val('dummy');
                }
                else {
                    // if item exists, then just set the text to trimmed upper text
                    $(event.target).val(newData);
                    itemLenBredth($('#txtName').val().toUpperCase());
                    //SetUnitAndValue(-1, -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
                    // save in box
                    // find the rate of current item and current prc(s)
                    if (isPackageCondition()) {
                        $('#txtRate, #txtExtraRate1, #txtExtraRate2').on('keydown.package', function (e) {
                            if (e.which !== 13 && e.which !== 9 && e.which !== 61 && e.which !== 107) {
                                return false;
                            }
                        });
                    }
                    else {
                        $('#txtRate, #txtExtraRate1, #txtExtraRate2').off('keydown.package');
                    }
                    findItemAndProcessRate(newData, -1, '');
                    // set the focus
                    if ($('#hdnDescEnabled').val() == 'True') {
                        $('#mytags').find('input').focus();
                        if ($('#hdnIsTour').val() == "1") { BookingTour(2); }  // Focus on Description
                    }
                    else if ($('#hdnColorEnabled').val() == 'True') {
                        $('#mytagsColor').find('input').focus();
                    }
                    else {
                        $('#txtProcess').focus();
                    }
                    if ($('#pnlLB').dialog('isOpen') == true) {
                        $('#txtLen').focus();
                    }
                    if ($('#pnlPanel').dialog('isOpen') == true) {
                        $('#txtNumPanels').focus();
                    }
                }
            },
            error: function (response) {
            }
        });
    }
    else {
        // check if in array
        // new
        // if the discount is applicable
        //var _allItemLst = $('#hdnAllItems').val() + '';
        var _allItems = $('#hdnAllItems').val().split(':');
        if ($.inArray($('#txtName').val().toUpperCase(), _allItems) != -1) {
            // check if item is in length breadth or panel form
            itemLenBredth($('#txtName').val().toUpperCase());
            //SetUnitAndValue(-1, -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
            if (isPackageCondition()) {
                $('#txtRate, #txtExtraRate1, #txtExtraRate2').on('keydown.package', function (e) {
                    if (e.which !== 13 && e.which !== 9 && e.which !== 61 && e.which !== 107) {
                        return false;
                    }
                });
            }
            else {
                $('#txtRate, #txtExtraRate1, #txtExtraRate2').off('keydown.package');
            }
            findItemAndProcessRate(newData, -1, '');
            // set the focus
            if ($('#hdnDescEnabled').val() == 'True') {
                $('#mytags').find('input').focus();
                if ($('#hdnIsTour').val() == "1") { BookingTour(2); }  // Focus on Description
            }
            else if ($('#hdnColorEnabled').val() == 'True') {
                $('#mytagsColor').find('input').focus();
                if ($('#hdnIsTour').val() == "1") { BookingTour(3); }  //Focus on color
            }
            else {
                $('#txtProcess').focus();
                if ($('#hdnIsTour').val() == "1") { BookingTour(4); } // Focus on Process
            }
            if ($('#pnlLB').dialog('isOpen') == true) {
                $('#txtLen').focus();
            }
            if ($('#pnlPanel').dialog('isOpen') == true) {
                $('#txtNumPanels').focus();
            }
        }
        else {
            $('#txtItemName').val(newData);
            $('#txtItemCode').focus();
            //if (checkForPassword('ItemName')) {
            $('#pnlItem').dialog({ width: 500, modal: true });
            //}
        }
    }
};

// Step 10.A.2 this is the function for checking the length and bredth
var itemLenBredth = function (itemName) {
    $.ajax({
        url: '../AutoComplete.asmx/CheckLenBredth',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        async: false,
        cache: false,
        data: "argItemName='" + itemName + "'",
        success: function (response) {
            var _dim = response.d;
            if (_dim == 'LB') {
                // set the text here,
                $('#pnlLB').dialog({ width: 400, height: 280, modal: true });
                $('#txtLen').focus();
                $('#hdnPrevUnit').val('LB');
                //SetUnitAndValue('LB', -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
            }
            else if (_dim == 'P') {
                $('#pnlPanel').dialog({ width: 340, height: 250, modal: true });
                $('#txtNumPanels').focus();
                $('#hdnPrevUnit').val('P');
                //SetUnitAndValue('P', -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
            }
            else if (_dim = 'Pcs') {
                $('#hdnPrevUnit').val('Pcs');
                //SetUnitAndValue('Pcs', -1, $('#grdEntry_ctl01_lblHSNo').text(), false);
            }
        },
        error: function () {
        }
    })
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
            return false;
        }
        else {
            $(event.target).val($(event.target).val().toUpperCase());
            return true;
        }
        // check if given item exits in the list
    }
};

// Step 12. event handler for subqty focus out
// what it does is, shows/hides the listbox and subitemtextbox depending on the qty of subitems
var txtItemSubQtykeyupHandler = function (event) {
    // if just one qty, set focus to next field
    if ($(this).val() == '1' && !$('#lstSubItem').is(':visible')) {
        $('#txtItemCode').focus();
        return;
    }
    if ($(this).val() == '') {
        $(this).val('1');
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

// Step 14. Item code focus out handler
var txtItemCodeKeyUpHandler = function (event) {
    // caps the value
    //$('#upMakeingItem').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0, border: 'none'} })
    var _retValue = '';
    var _argData;
    if ($(event.target).val() == null) {
        _argData = $('#txtItemCode').val().toUpperCase();
    }
    else {
        _argData = $(event.target).val().toUpperCase();
    }
    if ($(event.target).val() != '') {
        // $(event.target).val($(event.target).val().toUpperCase());
        $('#upMakeingItem').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0, border: 'none'} })
        // check if this item code exits
        $.ajax({
            url: '../AutoComplete.asmx/checkIfItemCodeExits',
            type: 'GET',
            timeout: 20000,
            datatype: 'JSON',
            contentType: 'application/json; charset=utf8',
            cache: false,
            async: false,
            data: "argItemCode='" + _argData + "'",
            success: function (response) {
                var _result = response.d;
                // if result is true, the given item code is present
                // else add to database
                if (_result == true) {
                    alert('Supplied code already exists, please enter a new code!');
                    $('#upMakeingItem').unblock({ fadeOut: 0 });
                    $(event.target).val('');
                    $(event.target).focus();
                    _retValue = false;
                }
                else {
                    $('#upMakeingItem').unblock({ fadeOut: 0 });
                    _retValue = true;
                }
            },
            error: function (response) {
            }
        });
    }
    return _retValue;
};

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
        var orgTarget = event.target;
        event.target = 'txtItemCode';
        var _res = txtItemCodeKeyUpHandler(event);
        if (!_res) {
            // $('#btnItemSave').click();
            return false;
        }
        event.target = orgTarget;
        var itemnamechk = $('#txtItemName').val().toUpperCase();
        var _allItems = $('#hdnAllItems').val().split(':');
        if ($.inArray(itemnamechk, _allItems) != -1) {
            alert('Item Already Exist, Please Enter A New Name.');
            $('#txtItemName').focus();
            return false;
        }
        $('#upMakeingItem').block({ fadeIn: 0, message: null, overlayCSS: { backgroundColor: '#fff', opacity: 0, border: 'none'} })
        var _subItemList = ", subItems: ['";
        var _MainSubItemList;
        // for all items in listbox, add them to the subitem list
        $('#lstSubItem option').each(function (index) {
            _subItemList = _subItemList + $('#lstSubItem option:eq(' + index + ')').val() + "', '";
        });
        _subItemList = _subItemList + "']";
        _MainSubItemList = _subItemList; //.substring(0, _subItemList.length - 12);
        // formulate the data to be passed to server
        var _dataString = "{itemName: '" + $('#txtItemName').val() + "', itemCode:'" + $('#txtItemCode').val() +
                                            "', qtyOfSubItems:'" + $('#txtItemSubQty').val() + "'" + _MainSubItemList;
        // in case there is no subitem, we need to add this item as subitem
        if (_dataString.indexOf('subItems') < 0) {
            _dataString = _dataString + ", subItems: [\'\']";
        }
        _dataString = _dataString + "}",
        // add item to master
                    $.ajax({
                        url: '../AutoComplete.asmx/AddItemsToMaster',
                        type: 'POST',
                        timeout: 20000,
                        datatype: 'JSON',
                        contentType: 'application/json; charset=utf8',
                        cache: false,
                        data: _dataString,
                        success: function (response) {
                            if (response.d == "Record Saved") {
                                // set the name in main name
                                $('#txtName').val($('#txtItemName').val());
                                $('#pnlItem').dialog("close");
                                $('#mytags').find('input').focus();
                                // add the item to the list of already added items
                                var _val = $('#hdnAllItems').val();
                                _val = _val + ':' + $('#txtName').val().toUpperCase();
                                $('#hdnAllItems').val(_val);
                                $('#hdnPrevUnit').val('Pcs');
                                $('#upMakeingItem').unblock({ fadeOut: 0 })
                            }
                            else {
                                alert('Error : ' + response.d);
                                $('#upMakeingItem').unblock({ fadeOut: 0 })
                            }
                        },
                        error: function (response) {
                        }
                    });
    }
};

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

var txtAdvanceKeyUpHandler = function (event) {
    if ($(this).val() == '') { return; };
    var _allTotal = parseFloat($('#txtTotal').val());
    if (_allTotal == '') {
        _allTotal = 0;
    }
    else {
        _allTotal = Math.ceil(_allTotal);
    }
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

txtDiscountAmtKeyUpHandler = function () {
    if ($('#txtDiscountAmt').val() == '') {
        $('#txtDiscountAmt').val('0');
    }
    // check if the amount entered is not greater than gross amt
    // make sure that this is not null, and is a number
    // check for parseFloat
    if (parseFloat($('#txtDiscountAmt').val()) > parseFloat($('#txtCurrentDue').val())) {
        alert('discount amount can\'t be greater than gross amount!');
        $('#txtDiscountAmt').focus();
        return false;
    }

    //checkForPassword('Discount');

    // Step 20.A
    // make the percentage
    var _disPerc = 100 * parseFloat($('#txtDiscountAmt').val()) / parseFloat($('#txtCurrentDue').val());
    _disPerc = _disPerc;
    if (isNaN(_disPerc)) {
        _disPerc = '0';
    }
    // set this perctage to text amount
    $('#txtDiscount').val(_disPerc);
    // set the label to current value
    $('#lblDisAmt').text(parseFloat($('#txtDiscountAmt').val()).toFixed(2));
    $('#hdnDiscountValue').text($('#txtDiscountAmt').val());
    // set the hidden recomp value to this
    $('#hdnDisAmtRecomp').val($('#txtDiscountAmt').val());
    // if there are just 2 rows, the header and the current, then
    if ($('#grdEntry > tbody > tr').size() == 2) {
        recomputeAllGrid(1, 2, false, true, -1, true);
        $('#txtAdvance').focus();
        return false;
    }
    else {
        recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1, true);
        $('#txtAdvance').focus();
        return false;
    }
};
// Step 20.B on changing discount
txtDiscountkeyUpHandler = function (event) {
    if ($('#txtDiscount').val() == '') {
        $('#txtDiscount').val('0');
    }

    //checkForPassword('Discount');

    // find the amount and save it in txtDiscountAmt
    recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1, true);
    // focus on advance
    $('#txtAdvance').focus();
    return false;
};

// Step 21. this will recalculate all the data of lower grid
// argStart = the starting number to being counting from
// argEnd = the row to stop count at
// argCallEachRowAsUpdate = call row as updadte or not
// argAddDisAndTax = should dis and tax be added or subtracted?
// argRowNumberForWhichToTakeLBPFromTexts = the row number (in case of update or add) for which we would not split the
// itemname to find panel or LB, but rather we take the value from texts
// isFromDiscount => tells if coming from discount (or advance), if it is, then if urgent is checked,
// we wont' supply the rates as seen, but reverse calc them to find the original rates before the urgent rate was applied
function recomputeAllGrid(argStartRow, argEndRow, argCallEachRowAsUpdate, argAddDisAndTax, argRowNumberForWhichToTakeLBPFromTexts, isFromDiscount) {
    var _iCounter;
    var _isPkg = isPackageCondition();
    var _pkgTaxInclusive = $('#hdnPackageTaxPerItemPrice').val().split(':')[0] === 'INCLUSIVE';
    // remove the event handler
    // $('body').off("focusout.AttachedEvent", '#txtDiscount', txtDiscountFocusOutHandler);
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
    var _lTmp = $('#txtLen').val();
    var _bTmp = $('#txtBreadth').val();
    var _plTmp = $('#txtNumPanels').val();

    // find the checks for urgent
    var _rtUrg;
    if (($('#chkToday').is(':checked') || $('#chkNextDay').is(':checked')) /* && (isFromDiscount) */) {
        _rtUrg = $('#hdnUrgentRateApplied').val();
    }
    else {
        _rtUrg = 0;
    }
    // now what we want is the rate, that's like x + ( x * rateUrgent ) / 100 = rate =>  x =  ( rate * 100 ) / ( 100 + rateUrgent )
    _rtUrg = parseFloat(_rtUrg) + parseFloat(100);

    for (_iCounter = argStartRow; _iCounter < argEndRow; _iCounter++) {
        // find qty
        _reCompQty = $('body').find('#grdEntry > tbody').find('#Qty_' + _iCounter + '').text() + '';
        // find the process array for each
        var _reCompProcessArray = $('body').find('#grdEntry > tbody').find('#Prc_' + _iCounter + '').text() + '';
        _reCompAllSplittedPrcRate = splitPrcRateFromArray(_reCompProcessArray);
        // set the variables
        _reCompPrc = _reCompAllSplittedPrcRate.prc;
        _reCompPrc1 = _reCompAllSplittedPrcRate.prc1;
        _reCompPrc2 = _reCompAllSplittedPrcRate.prc2;
        _reCompRate = (_reCompAllSplittedPrcRate.rate * 100) / (_rtUrg);
        _reCompRate1 = (_reCompAllSplittedPrcRate.rate1 * 100) / (_rtUrg);
        _reCompRate2 = (_reCompAllSplittedPrcRate.rate2 * 100) / (_rtUrg);
        // find the disAndTax
        /* For the tax new, the panel and length breadth */
        /* var _patt1 = / \d+PL$/gm; */
        var _patt1 = /\s\d*(?:[.]\d{1})?PL$/gm;
        var _patt2 = / \d+(?:\.\d+)?X\d+(?:\.\d+)?$/gm;
        var _itemRecomp = $('body').find('#grdEntry > tbody').find('#ItemName_' + _iCounter + '').text() + '';
        var _l = 1;
        var _b = 1;
        var _pl = 1;
        if (argRowNumberForWhichToTakeLBPFromTexts != _iCounter) {
            // if it contains any one of them
            if (_patt1.test(_itemRecomp)) {
                // it contains the _PL
                var _idx = _itemRecomp.lastIndexOf(' ');
                _idx = parseInt(_idx) + parseInt(1);
                var _idxPL = _itemRecomp.lastIndexOf('PL');
                _pl = _itemRecomp.substring(_idx, _idxPL);
            }
            else if (_patt2.test(_itemRecomp)) {
                var _idx = _itemRecomp.lastIndexOf(' ');
                _idx = parseInt(_idx) + parseInt(1);
                var _idxLB = _itemRecomp.substring(_idx);
                var _idxX = _idxLB.indexOf('X');
                _idxX = parseInt(_idxX) + parseInt(1);
                _l = _idxLB.substring(0, _idxX - 1);
                _b = _idxLB.substring(_idxX);
            }
            else {
                _l = 1;
                _b = 1;
                _pl = 1;
            }
        }
        else if (argRowNumberForWhichToTakeLBPFromTexts == _iCounter) {
            // take the values from the texts
            /*_l = $('#txtLen').val();
            _b = $('#txtBreadth').val();
            _pl = $('#txtNumPanels').val();*/
            _l = _lTmp;
            _b = _bTmp;
            _pl = _plTmp;
        }
        console.log(_itemRecomp + ' was item and panels are ' + _pl);
        $('#txtLen').val(_l);
        $('#txtBreadth').val(_b);
        $('#txtNumPanels').val(_pl);
        ComputeRowDisTaxAmt(_reCompPrc, _reCompRate, _reCompPrc1, _reCompRate1, _reCompPrc2, _reCompRate2, _reCompQty, false, _iCounter, true, false);
    }
    /*************************************/
    // set the values from hidden field
    // inclusive case
    // do the same for package
    var _tmp = parseFloat($('#hdnTaxAmtRecomp').val());

    if (_isPkg && _pkgTaxInclusive) {
        $('#txtSrTax').val('0');
    }
    // else if pacakge and is NOT inclusive, i.e. is exclusive then
    else if (_isPkg && !_pkgTaxInclusive) {
        $('#txtSrTax').val(_tmp.toFixed(2));
    }
    // else if not package then check what is the value from default
    else if ($('#hdnIsTaxExclusive').val() == 'false') {
        $('#txtSrTax').val('0');
    }
    else { // not package and not inclusive (i.e. exclusive!)
        $('#txtSrTax').val(_tmp.toFixed(2));
    }
    /****** DONE TAX *********/

    $('#lblDisAmt').text(parseFloat($('#hdnDisAmtRecomp').val()).toFixed(2));
    $('#hdnDiscountValue').val($('#hdnDisAmtRecomp').val());
    $('#txtTotal').val($('#hdnTotal').val());
    var _nb = parseFloat($('#hdnBalance').val());
    setNetBal(_nb, false);
    // set here
    $('#txtLen').val(_lTmp);
    $('#txtBreadth').val(_bTmp);
    $('#txtNumPanels').val(_plTmp);
    // reset all values
    setTheDefaults();
    $('#grdEntry_ctl01_lblHAmount').text($('#txtCurrentDue').val());
    // attach the event handler again
    // $('body').on("focusout.AttachedEvent", '#txtDiscount', txtDiscountFocusOutHandler);
};

// Step 20.F this will remove the event handler, on the
// keypress in txtBalance, will fire that event manually at the end of recomputeAllGrid
var txtBalanceKeyDownHandler = function (event) {
    // remove the handler here
    $('body').off("focusout.AttachedEvent", '#txtDiscount', txtDiscountKeyDownHandler);
};

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
    // check if rows pending to be updated
    if ($('#grdEntry_ctl01_imgBtnGridEntry').text() == 'Update') {
        alert('There are rows pending to be updated');
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

    // set if package is  Value / Benefit
    if ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Value / Benefit') {
        $('#hdnTaxType').val('INCLUSIVE');
    }

    // verify the date
    if (!validateDeliveryDate(false, 'txtDueDate')) {
        alert('Due date can\'t be before the booking date');
        return false;
    }

    var _commMeans = $('#drpCommLbl option:selected').text() + '';
    if ((_commMeans.indexOf('Email') > -1) && ($('#lblEmailId').val() == '')) {
        alert('Please enter email!');
        $('#lblEmailId').focus();
        return false;
    }
    if ((_commMeans.indexOf('SMS') > -1) && ($('#lblMobileNo').val() == '')) {
        alert('Please enter mobile no!');
        $('#lblMobileNo').focus();
        return false;
    }

    if (hdnDummyManyMobUnq.value === 'false') {
        alert('Mobile no already in use!');
        lblMobileNo.focus();
        return false;
    }

    /* skip this check as it is already being done in next clause
    if ($('#hdnPackageExpired').val() === 'true') {
    alert('The package has expired, please renew or mark complete');
    return false;
    }*/

    // check if booking date is in the bounds of packge
    if (((compareDates($('#txtDate').val(), $('#hdnPackageStartEndDate').val().split(':')[0]) === true) && (compareDates($('#hdnPackageStartEndDate').val().split(':')[1], $('#txtDate').val()) === true))) {
        var packageVal, msg, allMsg;
        if (($('#drpAdvanceType').val() === 'Package') && ($('#lowerRowCenter').children('span').not('#lblError, #lblSave').css('display') === 'none') && $('#hdnPackageTypeRecNo').val().split(':')[0] !== 'Flat Qty') {
            packageVal = checkForPackage();
            // vivek sir might ask to show, TOTAL QTY allowed for each item, but we don't return that from procedure, so its not a point
            if (!packageVal.IsValid) {
                msg = 'Order not generated, kindly rectify the following as per package limits:\n\n';
                if (packageVal.Invalid_Items.length !== 0) {
                    msg += packageVal.Invalid_Items.reduce(function (prev, curr, index, ary) { return prev + "\n" + curr; }, "These garments are invalid: ");
                    alert(msg.substr(0, msg.length - 1));
                    return false;
                }
                else {
                    allMsg = packageVal.Invalid_Qty.Item[0] + " Allowed: " + packageVal.Invalid_Qty.Allowed[0] + ", In current order: " + packageVal.Invalid_Qty.Current[0];
                    //allMsg = "The following are invalid:";
                    allMsg = msg + packageVal.Invalid_Qty.Item.reduce(function (p, c, i, a) { return p + '\n' + c + ' Allowed: ' + packageVal.Invalid_Qty.Allowed[i] + ', In current order: ' + packageVal.Invalid_Qty.Current[i]; }, allMsg);
                    alert(allMsg);
                    return false;
                }
            }
            // now the package is valid, so this is a package booking, set the tax type accordingly
            else {
                if (hdnPackageTaxPerItemPrice.value !== '') { // if the pkgTaxType if not null
                    $('#hdnTaxType').val($('#hdnPackageTaxPerItemPrice').val().split(':')[0]);
                }
            }
        }

        var CurrBalanceAmt = 0;
        if ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Value / Benefit') {
            CurrBalanceAmt = $('#txtCurrentDue').val();
        }
        else {
            CurrBalanceAmt = $('#txtBalance').val();
        }
       // if (($('#drpAdvanceType').val() === "Package") && ($('#hdnPackageTypeRecNo').val().split(':')[0] !== 'Flat Qty') && $('#hdnPackageTypeRecNo').val().split(':')[0] !== 'Qty / Item' && (parseFloat($('#lblPendingAmt').text()) < parseFloat($('#txtBalance').val()))) {
        if (($('#drpAdvanceType').val() === "Package") && ($('#hdnPackageTypeRecNo').val().split(':')[0] !== 'Flat Qty') && $('#hdnPackageTypeRecNo').val().split(':')[0] !== 'Qty / Item' && (parseFloat($('#lblPendingAmt').text()) < parseFloat(CurrBalanceAmt))) {
            alert('The booking can\'t be greater then the package amt left.');
            return false;
        }

        if (($('#drpAdvanceType').val() === "Package") && $('#hdnPackageTypeRecNo').val().split(':')[0] === 'Flat Qty') {
            packageVal = checkForPackage(true);
            msg = 'Order not generated, kindly rectify the following as per package limits:\n\n';
            if (!packageVal.IsValid) {
                if (packageVal.Invalid_Items.length !== 0) {
                    msg += packageVal.Invalid_Items.reduce(function (prev, curr, index, ary) { return prev + "\n" + curr; }, "These garments are invalid: ");
                    alert(msg.substr(0, msg.length - 1));
                    return false;
                }
                else /* if (hdnPackgeItems.value.split('_')[0].split(':')[0] === 'All') */{
                    msg += 'Total garments allowed: ' + packageVal.MaxQtyAll + '\nIn current order: ' + packageVal.CurrentQtyAll;
                    alert(msg);
                    return false;
                } /*
                else {
                    msg = packageVal.Invalid_Qty.Item[0] + " has allowed qty of " + packageVal.Invalid_Qty.Allowed[0] + " but is given " + packageVal.Invalid_Qty.Current[0];
                    allMsg = "The following are invalid:";
                    allMsg = packageVal.Invalid_Qty.Item.reduce(function (p, c, i, a) { return p + '\n' + c + ' allowed => ' + packageVal.Invalid_Qty.Allowed[i] + ' current => ' + packageVal.Invalid_Qty.Current[i]; }, allMsg);
                    alert(allMsg);
                    return false;
                }*/
            }
            // now the package is valid, so this is a package booking, set the tax type accordingly
            else {
                if (hdnPackageTaxPerItemPrice.value !== '') { // if the pkgTaxType if not null
                    $('#hdnTaxType').val($('#hdnPackageTaxPerItemPrice').val().split(':')[0]);
                }
            }
        }
    }
    // if not, then
    else {
        // check for if it is package (currently only discont, but it will still apply to other types) and bkDate < pkgStart date then there is error
        if (($('#hdnAssignId').val() !== '0') && $('#hdnAssignId').val() !== '') {
            // alert('This booking can\'t be done in package, this will be a normal booking as Customer does not have active package on ' + txtDate.value);
            alert('This order cannot be generated under the package as the package start date is in future.');
            $('#hdnAssignId').val('0');
            document.getElementById('hdnCheckPkgDiscount').value = "1";
            $('#drpAdvanceType option').eq(3).remove();
            // DONT RETURN FALSE!!!
        }
    }

    if (($('#isInEditMode').val() == 'true') && ($('#txtEditRemarks').val() == '') && ($('#hdnCheckForEditRemarks').val() == 'true')) {
        alert('Please enter the edit remarks');
        $('#txtEditRemarks').focus();
        return false;
    }
    else if (($('#isInEditMode').val() == 'true') && ($('#txtEditRemarks').val() == '') && ($('#hdnCheckForEditRemarks').val() == 'false')) {
        $('#isInEditMode').val('');
        return false;
    }
    else if (($('#isInEditMode').val() == 'true') && ($('#txtEditRemarks').val() != '')) {
        $('#isInEditMode').val($('#txtEditRemarks').val());
        $('#txtEditRemarks').val('');
        // remove the elem cause it causes the "," to be appened to txtWorkShopNote
        var elem = document.getElementById('txtEditRemarks');
        elem.parentNode.removeChild(elem);
    }

    // check if asked for confirmation
    if ($('#hdnConfirmDelivery').val() == 'true') {
        var strdiscount = document.getElementById('hdnCheckPkgDiscount').value;
        if (strdiscount == "1") {
            $('#pnlConfirmDate').dialog('close');
            document.getElementById('txtDiscount').value = 0;
            document.getElementById('hdnCheckPkgDiscount').value = 0;
            recomputeAllGrid(1, ($('#grdEntry > tbody > tr').size()), false, true, -1, true);
            return false;
        }
        $('#pnlConfirmDate').dialog({ width: 250, modal: true, closeOnEscape: false });
        return false;
    }

    // set the discount if not set
    $('#hdnDiscountPerc').val($('#txtDiscount').val());
    // enable the attrs
    $('#txtCurrentDue').attr('disabled', false);
    $('#txtSrTax').attr('disabled', false);
    $('#txtTotal').attr('disabled', false);
    $('#txtBalance').attr('disabled', false);
    $('#chkToday').attr('disabled', false);
    $('#chkNextDay').attr('disabled', false);
    $('#txtAdvance').attr('disabled', false);
    $('#ddlRateList').attr('disabled', false);
    return true;
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
    if (Math.ceil($('#txtAdvance').val()) > Math.ceil($('#txtTotal').val())) {
        alert('Advance can\'t be greater than balance');
        return false;
    }
    var _bal = $('#txtBalance').val();
    if (parseFloat(_bal) < 0) {
        alert('Invalid value in balance');
        return false;
    }
    // check if gross total - discount + tax == total
    if (parseFloat(_CurrentDue) - parseFloat(_dis) + parseFloat(_tax) != parseFloat(_total)) {
        //return false;
    }
    // check if total - advance == net
    if (parseFloat(_total) - parseFloat(_advance) != parseFloat(_bal)) {
        //return false;
    }
    // all validation complete, return true
    return true;
}

// Step 23.E.
// this the the handler for pnlConfirmDate
var handlerDrpList = function (event) {
    // if key is enter, press the confirm button and save it
    if (event.which == 13) {
        // first set the date in delivery text box
        var _delDate = $('#drpDate').val();
        _delDate += ' ' + $('#drpMonth').val();
        _delDate += ' ' + $('#drpYear').val();
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
        if ($(event.target).attr('id') == 'drpYear') {
            //$('#drpDate, #drpMonth, #drpYear').off('keydown');
            var tmpYear = $('#drpYear').val();
            $(this).blur();
            setTimeout(function () { $('#drpMonth').focus().select(); $('#drpYear').val(tmpYear); }, 10);
            return false;
        }
        else if ($(event.target).attr('id') == 'drpMonth') {
            ///$('#drpDate, #drpMonth, #drpYear').off('keydown');
            var tmpMonth1 = $('#drpMonth').val();
            $(this).blur();
            setTimeout(function () { $('#drpDate').focus().select(); $('#drpMonth').val(tmpMonth1); }, 10);
            return false;
        }
        else {
            //$('#drpDate, #drpMonth, #drpYear').off('keydown');
            var _prv = $('#hdnPrevDate').val();
            var _idx = $('#drpDate option[value=' + _prv + ']').index();
            setTimeout(function () { $('#drpDate option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true); $('#hdnPrevDate').val($('#drpDate').val()); }, 0);
            return false;
        }
        return false;
    }
    // right arrow
    else if (event.which == 39) {
        event.preventDefault();
        //                    $('#hdnPrevYear').val($('#drpYear').val());
        //                    $('#hdnPrevYear').val($('#drpYear').attr('SelectedIndex'));
        if ($(event.target).attr('id') == 'drpDate') {
            //$('#drpDate, #drpMonth, #drpYear').off('keydown');
            var tmpDay = $('#drpDate').val();
            $(this).blur();
            setTimeout(function () { $('#drpMonth').focus().select(); $('#drpDate').val(tmpDay); }, 10);
            return false;
        }
        else if ($(event.target).attr('id') == 'drpMonth') {
            //$('#drpDate, #drpMonth, #drpYear').off('keydown');
            var tmpMonth = $('#drpMonth').val();
            $(this).blur();
            setTimeout(function () { $('#drpYear').focus().select(); $('#drpMonth').val(tmpMonth); }, 10);
            return false;
        }
        else {
            //$('#drpDate, #drpMonth, #drpYear').off('keydown');
            _prv = $('#hdnPrevYear').val();
            _idx = $('#drpYear option[value=' + _prv + ']').index();
            setTimeout(function () { $('#drpYear option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true); $('#hdnPrevYear').val($('#drpYear').val()); }, 0);
            return false;
        }
        return false;
    }
    // up arrow
    // the index is zero based
    else if (event.which == 38) {
        var _prv;
        var _idx;
        event.preventDefault();
        if ($(event.target).attr('id') == 'drpDate') {
            // find which one was previously selected
            _prv = $('#hdnPrevDate').val();
            _idx = $('#drpDate option[value=' + _prv + ']').index();
            //setTimeout(function () { $('#drpDate option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true); $('#hdnPrevDate').val($('#drpDate').val()); }, 0);
            setTimeout(function () {
                if ($('#drpDate option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').attr('value') == null) {
                    // set the selected value to same
                    $('#drpDate option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true);
                    return false;
                }
                else {
                    $('#drpDate option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true);
                    $('#hdnPrevDate').val($('#drpDate').val());
                }
            }, 0);
            //$('#drpDate option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true);
            //$('#hdnPrevDate').val($('#drpDate').val());
        }
        else if ($(event.target).attr('id') == 'drpMonth') {
            // find which one was previously selected
            _prv = $('#hdnPrevMonth').val();
            _idx = $('#drpMonth option[value=' + _prv + ']').index();
            //setTimeout(function () { $('#drpMonth option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true); $('#hdnPrevMonth').val($('#drpMonth').val()); }, 0);
            setTimeout(function () {
                if ($('#drpMonth option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').attr('value') == null) {
                    // set the selected value to same
                    $('#drpMonth option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true);
                    return false;
                }
                else {
                    $('#drpMonth option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true);
                    $('#hdnPrevMonth').val($('#drpMonth').val());
                }
            }, 0);
            //$('#hdnPrevMonth').val($('#drpMonth').val());
        }
        else {
            // find which one was previously selected
            _prv = $('#hdnPrevYear').val();
            _idx = $('#drpYear option[value=' + _prv + ']').index();
            //setTimeout(function () { $('#drpYear option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true); $('#hdnPrevYear').val($('#drpYear').val()); }, 0);
            setTimeout(function () {
                if ($('#drpYear option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').attr('value') == null) {
                    // set the selected value to same
                    $('#drpYear option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true);
                    return false;
                }
                else {
                    $('#drpYear option:eq(' + ((parseInt(_idx) + 0) - 1) + ')').prop('selected', true);
                    $('#hdnPrevYear').val($('#drpYear').val());
                }
            }, 0);
        }
        return false;
    }
    // down arrow
    // the index is zero based
    else if (event.which == 40) {
        var _prv;
        var _idx;
        event.preventDefault();
        if ($(event.target).attr('id') == 'drpDate') {
            // find which one was previously selected
            _prv = $('#hdnPrevDate').val();
            _idx = $('#drpDate option[value=' + _prv + ']').index();
            setTimeout(function () {
                if ($('#drpDate option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').attr('value') == null) {
                    // set the selected value to same
                    $('#drpDate option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true);
                    return false;
                }
                else {
                    $('#drpDate option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').prop('selected', true);
                    $('#hdnPrevDate').val($('#drpDate').val());
                }
            }, 0);
            //$('#drpDate option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').prop('selected', true);
            //$('#hdnPrevDate').val($('#drpDate').val());
            event.preventDefault();
        }
        else if ($(event.target).attr('id') == 'drpMonth') {
            // find which one was previously selected
            _prv = $('#hdnPrevMonth').val();
            _idx = $('#drpMonth option[value=' + _prv + ']').index();
            setTimeout(function () {
                if ($('#drpMonth option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').attr('value') == null) {
                    // set the selected value to same
                    $('#drpMonth option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true);
                    return false;
                }
                else {
                    $('#drpMonth option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').prop('selected', true);
                    $('#hdnPrevMonth').val($('#drpMonth').val());
                }
            }, 0);
        }
        else {
            // find which one was previously selected
            _prv = $('#hdnPrevYear').val();
            _idx = $('#drpYear option[value=' + _prv + ']').index();
            //setTimeout(function () { $('#drpYear option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').prop('selected', true); $('#hdnPrevYear').val($('#drpYear').val()); }, 0);
            setTimeout(function () {
                if ($('#drpYear option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').attr('value') == null) {
                    // set the selected value to same
                    $('#drpYear option:eq(' + ((parseInt(_idx) + 0)) + ')').prop('selected', true);
                    return false;
                }
                else {
                    $('#drpYear option:eq(' + ((parseInt(_idx) + 0) + 1) + ')').prop('selected', true);
                    $('#hdnPrevYear').val($('#drpYear').val());
                }
            }, 0);
            //$('#hdnPrevYear').val($('#drpYear').val());
        }
        return false;
    }
};

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
                             + $('#grdEntry').find('#Amount_' + _i + '').text() + ':'
                             + $('#grdEntry').find('#TmpItemName_' + _i + '').text();
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

function LoadUrgentRates() {
    // Step 25.B.1 this will load the data for urgent rate and date offset
    // this will be called at load, so that it doesn't affect the check change perf
    // call once for 1st data, second for 2nd data
    $.ajax({
        url: '../AutoComplete.asmx/LoadUrgentRate',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        data: "argFlag=37",
        cache: false,
        success: function (response) {
            // don't set in the check field, set in first urgentRateAndDateOffset1
            $('#urgentRateAndDateOffset1').val(response.d);
        },
        error: function (response) {
        }
    });
    // Step 25.B.2 this will load the data for urgent rate and date offset
    // this will be called at load, so that it doesn't affect the check change perf
    // call once for 1st data, second for 2nd data
    $.ajax({
        url: '../AutoComplete.asmx/LoadUrgentRate',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: 'application/json; charset=utf8',
        data: "argFlag='38'",
        cache: false,
        success: function (response) {
            // don't set in the check field, set in first urgentRateAndDateOffset2
            $('#urgentRateAndDateOffset2').val(response.d);
        },
        error: function (response) {
        }
    });
}

// can later implement the other 2 args,
// but for now, only the first one is getting implemented
function checkForPassword(fieldToCheck, passwordAppliedField, passwordValueField) {
    if (document.getElementById('hdnPwdItemRateDis').value === '') return true;
    switch (fieldToCheck) {
        case 'ItemName': (function () {
            if ($('#hdnPwdItemRateDis').val().split(':')[0] == '') return true;
            $('#pnlPwd').dialog({ width: 340, height: 280, modal: true, closeOnEscape: false });
            $('#hdnPrvPwdFocus').val('txtName');
            $('#btnCancelPwd').on('click.AttachedEvent', function (e) {
                $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; $('#txtName').focus(); return false;
            });
            $('#btnAcceptPwd').on('click.AttachedEvent', function (e) {
                if (document.getElementById('txtPwdForIRD').value == $('#hdnPwdItemRateDis').val().split(':')[0]) {
                    $('#btnAcceptPwd').off('click'); $('#pnlPwd').dialog('destroy'); document.getElementById('lblWrongPwd').textContent = ''; $('#pnlItem').dialog({ width: 500, modal: true }); return true;
                }
                else {
                    document.getElementById('lblWrongPwd').textContent = 'Wrong Password';
                    $('#txtPwdForIRD').focus();
                    return false;
                }
            });
        } ());
            break;
        case 'Rate': (function () {
            if ($('#hdnPwdItemRateDis').val().split(':')[1] == '') return true;
            if (isPackageCondition()) return true;
            $('#pnlPwd').dialog({ width: 340, height: 280, modal: true, closeOnEscape: false });
            $('#hdnPrvPwdFocus').val('txtRate');
            $('#btnCancelPwd').on('click.AttachedEvent', function (e) {
                $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; $('#txtRate').focus(); return false;
            });
            $('#btnAcceptPwd').on('click.AttachedEvent', function (e) {
                if (document.getElementById('txtPwdForIRD').value == $('#hdnPwdItemRateDis').val().split(':')[1]) {
                    $('#btnAcceptPwd').off('click'); $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; $('#grdEntry_ctl01_imgBtnGridEntry').trigger('click.AttachedEvent'); return true;
                }
                else {
                    document.getElementById('lblWrongPwd').textContent = 'Wrong Password';
                    $('#txtPwdForIRD').focus();
                    return false;
                }
            });
        } ());
            break;
        case 'Discount': (function () {
            if ($('#hdnPwdItemRateDis').val().split(':')[2] == '') return true;
            if (isPackageCondition()) return true;
            $('#pnlPwd').dialog({ width: 340, height: 280, modal: true, closeOnEscape: false });
            $('#hdnPrvPwdFocus').val('txtDiscount');
            $('#btnCancelPwd').on('click.AttachedEvent', function (e) {
                $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; $('#txtDiscount').focus(); return false;
            });
            $('#btnAcceptPwd').on('click.AttachedEvent', function (e) {
                if (document.getElementById('txtPwdForIRD').value == $('#hdnPwdItemRateDis').val().split(':')[2]) {
                    $('#btnAcceptPwd').off('click'); $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; txtDiscountkeyUpHandler(); return true;
                }
                else {
                    document.getElementById('lblWrongPwd').textContent = 'Wrong Password';
                    $('#txtPwdForIRD').focus();
                    return false;
                }
            });
        } ());
            break;
        case 'DiscountAmt': (function () {
            if ($('#hdnPwdItemRateDis').val().split(':')[3] == '') return true;
            if (isPackageCondition()) return true;
            $('#pnlPwd').dialog({ width: 340, height: 280, modal: true, closeOnEscape: false });
            $('#hdnPrvPwdFocus').val('txtDiscountAmt');
            $('#btnCancelPwd').on('click.AttachedEvent', function (e) {
                $('#btnAcceptPwd').off('click'); $('#pnlPwd').dialog('close'); document.getElementById('txtPwdForIRD').value = ''; $('#txtDiscountAmt').focus(); return false;
            });
            $('#btnAcceptPwd').on('click.AttachedEvent', function (e) {
                if (document.getElementById('txtPwdForIRD').value == $('#hdnPwdItemRateDis').val().split(':')[3]) {
                    $('#btnAcceptPwd').off('click'); $('#pnlPwd').dialog('close'); document.getElementById('lblWrongPwd').textContent = ''; document.getElementById('txtPwdForIRD').value = ''; txtDiscountAmtKeyUpHandler(); return true;
                }
                else {
                    document.getElementById('lblWrongPwd').textContent = 'Wrong Password';
                    $('#txtPwdForIRD').focus();
                    return false;
                }
            });
        } ());
            break;
        default:
            return true;
    }
    //    document.getElementById('hdnPwdItemRateDis').value
}

// Gets the password
function LoadThePassWords() {
    $.ajax({
        url: '../Autocomplete.asmx/LoadThePasswords',
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf8',
        timeout: 20000,
        success: function (response) { document.getElementById('hdnPwdItemRateDis').value = response.d; },
        error: function (response) { document.getElementById('hdnPwdItemRateDis').value = ''; }
    });
};

// default serach in the drop down
function LoadDefaultSearch() {
    $.ajax({
        url: '../Autocomplete.asmx/LoadDefaultSearchCriteriaForCustomer',
        type: 'GET',
        contentType: 'application/json; charset=utf8',
        dataType: 'JSON',
        timeout: 20000,
        success: function (response) {
            /*if (response.d === 'MembershipId') {
            document.getElementById('drpSearchCustBy').value = 'Membership Id';
            }
            else if (response.d === ) {
            }
            else {*/
            document.getElementById('drpSearchCustBy').value = response.d;
            $('#drpSearchCustBy').trigger('change');
            //}
        },
        error: function (response) { if (console) { console.warn('error in loading default search'); } }
    });
}

// add the editing remarking editing node
function AddEditingRemarksNode() {
    var node = document.getElementById('UpdatePanel10').children[1].children[0].children[4].cloneNode(true);
    document.getElementById('UpdatePanel10').children[1].appendChild(node);
    node.children[0].textContent = "Editing Remarks";
    node.children[1].children[0].id = "txtEditRemarks";
    document.getElementById('txtEditRemarks').value = '';
}

function checkForEditRemarks() {
    var _resultEdit = false;
    $.ajax({
        url: '../Autocomplete.asmx/checkForEditRemarks',
        type: 'GET',
        contentType: 'application/json; charset=utf8',
        dataType: 'JSON',
        timeout: 1000,
        async: false,
        success: function (response) {
            $('#hdnCheckForEditRemarks').val(response.d);
            return false;
        },
        error: function (response) { }
    });
}

// this finds the NON-URGENT rate, in case of EDITING, UPDATING, OR DELETING
// if any of the urgent check box is checkined then in all of the above cases the rate was ALREADY urgent-ified
// and when the calc when the cals was being done, it was RE-URGENTING  them, so we need tno NO-URGNET them
function FindNonUrgentRates(prc, rate, prc1, rate1, prc2, rate2, qty, urgentRate, other) {
    var _rt, _rt1, _rt2;
    var _returnValue;
    _rt = (100 * rate) / (100 + Number(urgentRate));
    _rt1 = (100 * rate1) / (100 + Number(urgentRate));
    _rt2 = (100 * rate2) / (100 + Number(urgentRate));
    _returnValue =
                {
                    'rt': _rt,
                    'rt1': _rt1,
                    'rt2': _rt2
                };
    return _returnValue;
}

function CheckMobileUnique(elment) {
    var _mobNo;
    if (lblMobileNo.value === '') {
        hdnDummyManyMobUnq.value = 'true';
        return;
    }

    var _unq = $.ajax({
        url: '../Autocomplete.asmx/IsMobileUnique',
        type: 'GET',
        contentType: 'application/json; charset=utf8',
        dataType: 'JSON',
        data: "mobileNo='" + $('#lblMobileNo').val() + "'",
        async: false,
        timeout: 2000,
        success: function (res) { /*if (res.d) return false; else return true; */ },
        error: function (res) { alert('some error occurred when checking mobile no'); }
    }), chained = _unq.then(function (data) {
        if (data.d == false) {
            alert('Mobile no already in use!');
            hdnDummyManyMobUnq.value = 'false';
            $('#lblMobileNo').focus();
            lblMobileNo.focus();
            if ($(':focus').id !== 'lblMobileNo') {
                console.log('can\'t focus');
                $('#lblMobileNo').focus();
                console.log($(':focus').id);
            }
            return false;
        }
        else {
            hdnDummyManyMobUnq.value = 'true';
            lblEmailId.focus();
            $('#lblEmailId').focus();
            if ($(':focus').id !== 'lblEmailId') {
                console.log('can\'t focus');
                $('#lblEmailId').focus();
                console.log($(':focus').id);
            }
            return false;
        }
    });
}

// this is for the third case of package, if there is qty and items
// hdnPackgeItems contains items in the form of item1:item2:item3.....itemn_rate1:rate2:rate3.....raten
// the checkForFlatQty tells weather to check for flat qty
function checkForPackage(checkForFlatQty) {
    var allRows = $('#grdEntry').children().children();
    var items = $('#hdnPackgeItems').val().trim();
    var isAllFlat = false;
    isAllFlat = hdnPackgeItems.value.split('_')[0].split(':')[0] === 'ALL';

    if (checkForFlatQty && isAllFlat)
        items = $('#hdnAllItems').val();

    var totalQtyForFlat = 0;
    var allowedQtyForFlat = 0;
    var rtr = true;
    var allItemsInGrid = [], allItemsTotalSize = [], innerIdx, idxObj;
    var returnValue = {
        'IsValid': true,
        'Invalid_Items': [],
        'Invalid_Qty': {
            'Item': [],
            'Allowed': [],
            'Current': []
        },
        'MaxQtyAll': 0,
        'CurrentQtyAll': 0
    };
    allRows.not(':first').each(function () {
        var idx = items.split('_')[0].split(':').indexOf($(this).find('td').eq(5).text().trim()); // no harm in splitting even all clothes (in case of flat, cause of index 0)
        //var idx = $.inArray($(this).find('td').eq(5).text().trim(), items.split('_')[0].split(':'));
        // if any item is not in package return false;
        if (idx === -1) {
            rtr = false;
            returnValue.Invalid_Items.push($(this).find('td').eq(5).text().trim()); // add current item to list of invalid items
        }
        else {
            // if qty is greater then allowed return false
            //if (
            if (checkForFlatQty)
                totalQtyForFlat += Number($(this).find('td').eq(3).text().trim());
            else {
                if (parseFloat($(this).find('td').eq(3).text().trim()) > parseFloat(items.split('_')[1].split(':')[idx])) {
                    rtr = false;
                    returnValue.Invalid_Qty.Item.push($(this).find('td').eq(5).text().trim());
                    returnValue.Invalid_Qty.Allowed.push(items.split('_')[1].split(':')[idx]);
                    returnValue.Invalid_Qty.Current.push($(this).find('td').eq(3).text().trim());
                }
            }
        }
        // if rtr is not already false, we need to add items to array to check for duplicate items in rows and sum their count
        if (rtr && !isAllFlat) {
            innerIdx = allItemsInGrid.indexOf($(this).find('td').eq(5).text().trim());
            if (innerIdx === -1) {
                // add the item and qty to corresponding array
                allItemsInGrid.push($(this).find('td').eq(5).text().trim());
                allItemsTotalSize.push($(this).find('td').eq(3).text().trim());
            }
            else {
                // check if prev qty + curnt qty is greater then allowed
                if (Number(allItemsTotalSize[innerIdx]) + Number($(this).find('td').eq(3).text().trim()) > Number(items.split('_')[1].split(':')[idx])) {
                    // if its is greater, just set to false, else update the current index of allItemsTotalSize
                    rtr = false;
                    idxObj = returnValue.Invalid_Qty.Item.indexOf($(this).find('td').eq(5).text().trim());
                    if (idxObj === -1) {
                        returnValue.Invalid_Qty.Item.push($(this).find('td').eq(5).text().trim());
                        returnValue.Invalid_Qty.Allowed.push(items.split('_')[1].split(':')[idx]);
                        returnValue.Invalid_Qty.Current.push(Number(allItemsTotalSize[innerIdx]) + Number($(this).find('td').eq(3).text().trim()));
                    }
                    else {
                        returnValue.Invalid_Qty.Current[idxObj] = Number(allItemsTotalSize[innerIdx]) + Number($(this).find('td').eq(3).text().trim());
                    }
                }
                else {
                    allItemsTotalSize[innerIdx] = Number(allItemsTotalSize[innerIdx]) + Number($(this).find('td').eq(3).text().trim());
                }
            }
        }
    });

    returnValue.IsValid = rtr;
    // set the max allowed and current
    returnValue.MaxQtyAll = Number($('#hdnPackgeItems').val().split('_')[1].split(':')[0]);
    returnValue.CurrentQtyAll = totalQtyForFlat;
    if (checkForFlatQty) {
        if (Number(returnValue.CurrentQtyAll) > Number(returnValue.MaxQtyAll))
            returnValue.IsValid = false;
    }
    return returnValue;
}

// returns true if the pacakge type if 4th one and date if btw start and end date, and dropdown says "pakcage"
function isPackageCondition() {
    var returnValue = true;
    // if bkDate is more then startDate
    returnValue = returnValue && compareDates($('#txtDate').val(), $('#hdnPackageStartEndDate').val().split(':')[0]);
    // if bkDate is less then endDate
    returnValue = returnValue && compareDates($('#hdnPackageStartEndDate').val().split(':')[1], $('#txtDate').val());
    // if pakcageType if 4th one
    returnValue = returnValue && ($('#hdnPackageTypeRecNo').val().split(':')[0] === 'Flat Qty' || $('#hdnPackageTypeRecNo').val().split(':')[0] === 'Qty / Item');
    // if dropDown says pacakge
    returnValue = returnValue && $('#drpAdvanceType').val() === 'Package';
    return returnValue;
}

function GetQtyndItemsForPackage(custCode, assignId, recurrenceId) {
    $.ajax({
        url: '../AutoComplete.asmx/GetQtyndItemsForPackage',
        type: 'GET',
        timeout: 20000,
        datatype: 'JSON',
        contentType: "application/json; charset=utf-8",
        data: "custCode='" + custCode + "'&assignId='" + assignId + "'&recurrenceId='" + recurrenceId + "'",
        success: function (response) {
            $('#hdnPackgeItems').val(response.d);
            if ($('#LblPkgBal').length === 0) {
                PendingClothesForPackage(null, 0, [], hdnPackgeItems.value.split('_')[1].split(':')[0]);
            }
            else {
                $('#LblPkgBal').text(hdnPackgeItems.value.split('_')[1].split(':')[0]);

                if (isInEditMode.value === 'true' && hdnPackageTypeRecNo.value.split(':')[0] === 'Flat Qty') {
                    // update remaining count in case of edit booking
                    UpdateRemainingInCaseOfEdit();
                }
            }
        },
        error: function (response) {
        }
    });
}

// explanation
// the value would be in form "Item:Item2:Item3...:ItemN_Bal:Bal2:Bal3:....BalN'
// what we want is to replace all values with respective Bal + cuurent bookings garment (ony if pkg type if flat qty though)
function UpdateRemainingInCaseOfEdit() {
    var hdnPackgeItemsBalNewValue = $('#hdnPackgeItems').val().split('_')[1].split(':').map(function (v, i, a) {
        return Number(v) + Number($('#lblCount').text());
    }).join(':');
    $('#hdnPackgeItems').val($('#hdnPackgeItems').val().split('_')[0] + '_' + hdnPackgeItemsBalNewValue);
    $('#LblPkgBal').text($('#hdnPackgeItems').val().split('_')[1].split(':')[0]);
}
//postMessage('before');

onmessage = function (msg) {
    postMessage('yo dawg ! ' + msg.data);
    try {
        msg.data();
    }
    catch (ex) {
        console.info(ex);
    }
    if (msg.data == 'deep') {
        //postMessage('starting shit...');
        //nowRunThis();
        //postMessage('outta here');
    }
}

function nowRunThis() {
    while (1) {
    }
}

// var _allObj = _allObj || (window._allObj = []);
function printerMe(test) {
    /*
    console.log('Started logging object :=>');
    console.log(test);

    for (var v in test) {
    if (!test.hasOwnProperty(v))
    continue;

    console.log('all obj is');
    console.log(_allObj);
    console.log('prop =>' + v);
    console.log('value is => ' + test[v]);

    if (test[v] instanceof Array && test[v].length === 1) {
    if ($.inArray(test[v][0], _allObj) === -1) {
    _allObj.push(test[v][0]);
    //setTimeout(printerMe(test[v][0]), 1);
    printerMe(test[v][0]);
    }
    }
    else {
    if (typeof test[v] === 'object' && $.inArray(test[v], _allObj) === -1) {
    _allObj.push(test[v]);
    //setTimeout(printerMe(test[v]), 1);
    printerMe(test[v]);
    } else if (typeof test[v] === 'function') {
    console.log(test[v].toString());
    }
    else {
    console.log(test[v]);
    }
    }
    }
    */
}

function SetAdvanceTypeOfPackage() {
    if ($('#grdEntry > tbody > tr').size() >= 2) {
        // alert($('#grdEntry > tbody > tr').size());
        $('#drpAdvanceType').attr('disabled', true);
    }
    else {
        $('#drpAdvanceType').attr('disabled', false);
    }
}