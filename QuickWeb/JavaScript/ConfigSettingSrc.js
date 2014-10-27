
function ShowAllLabelText() {
    ShowActDescText();
    ShowAllowDescText();
    ShowActColorText();
    ShowAllowColorText();
    ShowremarkText();
    ShowDeliveryText();
    ShowcappingColorText();
    ShowHeaderText();
    ShowPrintStoreCopyText();
    ShowRecptLogoText();
    ShowHeaderSloganText();
    ShowBarcodeText();
    ShowPhoneText();
    ShowDueDateText();
    ShowServiceTaxText();
    ShowTaxBifurcationText();
    ShowBookingTimeText();
    ShowpreviousDueText();
    ShowPriceText();
    ShowProcessText();
    ShowReceiptBorderText();
    ShowSubItemsText();
    ShowCustomerSignText();
    ShowTermText();
    ShowStoreTermText();
    ShowAndroidText();
}

// show label message
function ShowActDescText() {
    if ($('#rdbEnterRemarkTrue').is(':checked')) {
        $('#ActDescTxt').text("Activate description field, I would like to manage it.");
        $('#ActDescTxt').removeClass('txtColor');

    } else {
        $('#ActDescTxt').text("I don't use Description/ Defects.");
        $('#ActDescTxt').addClass('txtColor');
    }
}

function ShowAllowDescText() {
    if ($('#rdbBindToDescriptionTrue').is(':checked')) {
        $('#ActAllowDescTxt').text("Allow description only from master.");
        $('#ActAllowDescTxt').removeClass('txtColor');

    } else {
        $('#ActAllowDescTxt').text("let it be free form, we will type it on order screen.");
        $('#ActAllowDescTxt').addClass('txtColor');
    }
}

function ShowActColorText() {
    if ($('#rdbEnterColorTrue').is(':checked')) {
        $('#ActColorText').text("I would like to mention color of the garment in Invoice.");
        $('#ActColorText').removeClass('txtColor');

    } else {
        $('#ActColorText').text("No, I don't maintain colors.");
        $('#ActColorText').addClass('txtColor');
    }
}

function ShowAllowColorText() {
    if ($('#rdbBindColorToMasterTrue').is(':checked')) {
        $('#AllowColorText').text("Allow colors only from master.");
        $('#AllowColorText').removeClass('txtColor');

    } else {
        $('#AllowColorText').text("Don't allow color from master leave it free form.");
        $('#AllowColorText').addClass('txtColor');
    }
}

function ShowremarkText() {
    if ($('#rdbSaveEditRemarksTrue').is(':checked')) {
        $('#RemarkTxt').text("Remarks are mandatory, I want any one who does editing should mention a reason.");
        $('#RemarkTxt').removeClass('txtColor');

    } else {
        $('#RemarkTxt').text("Leave editing remarks as blank.");
        $('#RemarkTxt').addClass('txtColor');
    }
}

function ShowDeliveryText() {
    if ($('#rdbConfirmDateTrue').is(':checked')) {
        $('#DelDate').text("I would like system to confirm due date before finalizing order.");
        $('#DelDate').removeClass('txtColor');

    } else {
        $('#DelDate').text("I will select if from order screen, No confirmation required.");
        $('#DelDate').addClass('txtColor');
    }
}
function ShowcappingColorText() {
    if ($('#rdbBindColorQtyTrue').is(':checked')) {
        $('#cappingColorText').text("Put capping on No of Colors i can type based on qty of garments.");
        $('#cappingColorText').removeClass('txtColor');
    } else {
        $('#cappingColorText').text("I would like to use as many colors as i want.");
        $('#cappingColorText').addClass('txtColor');
    }
}
function ShowHeaderText() {
    if ($('#rdrLogoAndTest').is(':checked')) {
        $('#StoreCopyInfo').text("I would like to use blank paper as receipt.");
        $('#StoreCopyInfo').removeClass('txtColor');

    } else {
        $('#StoreCopyInfo').text("I would use pre printed receipt formate.");
        $('#StoreCopyInfo').addClass('txtColor');
    }
}

function ShowPrintStoreCopyText() {
    if ($('#chkStoreCopy').is(':checked')) {
        $('#PrintStoreCopyInfo').text("I want to print additional store copy of invoice.");
        $('#PrintStoreCopyInfo').removeClass('txtColor');

    } else {
        $('#PrintStoreCopyInfo').text("I don't want to print additional store copy of invoice.");
        $('#PrintStoreCopyInfo').addClass('txtColor');
    }
}

function ShowRecptLogoText() {
    if ($('#rdbShowOnReceiptTrue').is(':checked')) {
        $('#lblLogoText').text("I would like to print logo.");
        $('#lblLogoText').removeClass('txtColor');
    } else {
        $('#lblLogoText').text("I would not like to print logo.");
        $('#lblLogoText').addClass('txtColor');
    }
}

function ShowHeaderSloganText() {
    if ($('#rdbHeaderSloganTrue').is(':checked')) {
        $('#lblHeaderText').text("Print header slogan");
        $('#lblHeaderText').removeClass('txtColor');
    } else {
        $('#lblHeaderText').text("Don't print header slogan");
        $('#lblHeaderText').addClass('txtColor');
    }
}

function ShowBarcodeText() {
    if ($('#rdrBarcodeTrue').is(':checked')) {
        $('#lblBarcodeText').text("Print barcode");
        $('#lblBarcodeText').removeClass('txtColor');
    } else {
        $('#lblBarcodeText').text("Don't print barcode");
        $('#lblBarcodeText').addClass('txtColor');
    }
}

function ShowPhoneText() {
    if ($('#rdbPhoneNoTrue').is(':checked')) {
        $('#lblPhoneText').text("Print phone no");
        $('#lblPhoneText').removeClass('txtColor');
    } else {
        $('#lblPhoneText').text("Don't print phone no");
        $('#lblPhoneText').addClass('txtColor');
    }
}


function ShowDueDateText() {
    if ($('#rdbPrintDueDateTrue').is(':checked')) {
        $('#lblDueDtText').text("I would like Due date to be printed.");
        $('#lblDueDtText').removeClass('txtColor');
    } else {
        $('#lblDueDtText').text("Please hide due date.");
        $('#lblDueDtText').addClass('txtColor');
    }
}

function ShowServiceTaxText() {
    if ($('#rdbServicetaxTrue').is(':checked')) {
        $('#lblTax').text("Print Tax amount on Invoice.");
        $('#lblTax').removeClass('txtColor');
    } else {
        $('#lblTax').text("Don't Print Tax amount on Invoice.");
        $('#lblTax').addClass('txtColor');
    }
}

function ShowTaxBifurcationText() {
    if ($('#rdbTaxDetailTrue').is(':checked')) {
        $('#lblTaxDtlText').text("Print tax bifurcation details on invoice.");
        $('#lblTaxDtlText').removeClass('txtColor');
    } else {
        $('#lblTaxDtlText').text("Don't print tax bifurcation details on invoice.");
        $('#lblTaxDtlText').addClass('txtColor');
    }
}

function ShowBookingTimeText() {
    if ($('#rdbBookingTimeTrue').is(':checked')) {
        $('#lblBooingText').text("Print order creation time on slip.");
        $('#lblBooingText').removeClass('txtColor');
    } else {
        $('#lblBooingText').text("Don't print order creation time on slip.");
        $('#lblBooingText').addClass('txtColor');
    }
}


function ShowpreviousDueText() {
    if ($('#rdbPreviousTrue').is(':checked')) {
        $('#lblPreDueText').text("Print previous due on invoice.");
        $('#lblPreDueText').removeClass('txtColor');
    } else {
        $('#lblPreDueText').text("Don't print previous due.");
        $('#lblPreDueText').addClass('txtColor');
    }
}

function ShowPriceText() {
    if ($('#rdbRateTrue').is(':checked')) {
        $('#lblrateText').text("Show price details for every garment on invoice print out.");
        $('#lblrateText').removeClass('txtColor');
    } else {
        $('#lblrateText').text("Don't show price details on invoice.");
        $('#lblrateText').addClass('txtColor');
    }
}


function ShowProcessText() {
    if ($('#rdbProcessTrue').is(':checked')) {
        $('#lblprocessText').text("Show services on invoice print out.").removeClass('txtColor');
    } else {
        $('#lblprocessText').text("Don't show services on invoice.").addClass('txtColor');
    }
}

function ShowReceiptBorderText() {
    if ($('#rdbTableBorderTrue').is(':checked')) {
        $('#lblRectBorderText').text("Print outline border on invoice.").removeClass('txtColor');
    } else {
        $('#lblRectBorderText').text("I don't want outline on invoice.").addClass('txtColor');
    }
}

function ShowSubItemsText() {
    if ($('#rdbSubItemTrue').is(':checked')) {
        $('#lblSubItemText').text("Display pieces count in a seperate row.").removeClass('txtColor');
    } else {
        $('#lblSubItemText').text("I don't want to print pieces.").addClass('txtColor');
    }
}

function ShowCustomerSignText() {
    if ($('#rdbCustomerSignatureTrue').is(':checked')) {
        $('#lblCustSignText').text("Print customer signature box.").removeClass('txtColor');
    } else {
        $('#lblCustSignText').text("Don't print customer signature box.").addClass('txtColor');
    }
}

function ShowTermText() {
    if ($('#rdbTermConditionTrue').is(':checked')) {
        $('#lblTermText').text("Term and condition.").removeClass('txtColor');
    } else {
        $('#lblTermText').text("Don't print term and Condition.").addClass('txtColor');
    }
}
function ShowStoreTermText() {
    if ($('#rdbPrintTermsConditonTrue').is(':checked')) {
        $('#lblStoreTermText').text("Term and condition.").removeClass('txtColor');
    } else {
        $('#lblStoreTermText').text("Don't print term and Condition.").addClass('txtColor');
    }
}

function ShowAndroidText() {
    if ($('#chkAndroid').is(':checked')) {
        $('#lblAndroidText').text("Prices can be change from android application.").removeClass('txtColor');
    } else {
        $('#lblAndroidText').text("Don't change prices from android application.").addClass('txtColor');
    }
}

//  Show all tooltip text
function showAlllTooltipText() {
    $("#timeZoneInfo").tooltip({ title: 'Please select your Time Zone this will be used to sync to your computer time.' });       
    $("#TimeInfo").tooltip({ title: 'This will be display only on thermal slip.' });
    $("#BusinessInfo").tooltip({ title: 'This will be display only on thermal slip.' });
    $("#WebInfo").tooltip({ title: 'this will be sent in Emails when user register for website status check module.' });
    $("#LogoInfo").tooltip({ title: 'Upload a 60 X 60 pixels image. Allowed file type are Jpg, Jpeg, Gif and Png.' });
    $("#TaxInfo").tooltip({ title: 'Note: If you are not sure of tax rates consult your Accountant / Tax attorney.' });
    $("#StoreLogoInfo").tooltip({ title: 'Upload a 100 X 100 pixels image. Allowed file type are Jpg, Jpeg, Gif and Png.' });

    $("#BookingNoInfo").tooltip({ title: 'You can select order no series along with a prefix.' });
    $("#DelSetInfo").tooltip({ title: 'Select how due date will be calculated, System will automatically add predefined no of days in order date to calculate delivery date.' });
    $("#CustSearchInfo").tooltip({ title: 'Select how do you search a customer on order screen, following options are available <br> Search by Name only <br>Search by Address only <br>Search by Mobile No only<br>Search by Membership ID only <br>Search by Customer Code only<br>or "All" combination of above five methods', html: true });
    $("#WSNoteInfo").tooltip({ title: 'Please select level you details you need in workshop note format.' });
    $("#QtyInfo").tooltip({ title: 'Do you want quantity as "one" by default or should we leave it blank.' });
    $("#processInfo").tooltip({ title: 'Pre-selected services, select which is used most.' });
    $("#GarmentInfo").tooltip({ title: 'Pre-selected garment, select for which you receive maximum orders.' });
    $("#DiscountInfo").tooltip({ title: 'Select how you maintain discount ( Percentage wise or Flat Amount ).' });
    $("#NetAmountInfo").tooltip({ title: 'Should grand total be a Integer or Decimal.' });
    $("#Urgent1Info").tooltip({ title: 'Select label for urgent services if any, like same day or 24 hours services.' });
    $("#Urgent1RateInfo").tooltip({ title: 'Select how much price will be increased automatically if urgent services is selected.' });
    $("#Urgent1OffsetInfo").tooltip({ title: 'Select how would due date will be handled.' });
    $("#OrderInfo").tooltip({ title: 'Please select order receipt format, Printer to print on and if you want to print duplicate store copy.' });
    $("#DeliveryInfo").tooltip({ title: 'Please select delivery receipt format and printer to print on.' });
}