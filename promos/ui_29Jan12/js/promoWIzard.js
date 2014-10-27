function CreateNumPad(id, target) {
    var divName = '#' + id;
    //Append child elements
    var i, j;
    for (i = 0; i <= 11; i++) {
        j = (i + 1).toString();
        if (j == '10') { j = 'C'; }
        if (j == '11') { j = '0'; }
        if (j == '12') { j = '00'; }
        $(id).append('<div name="numPadKey" keyVal="' + j.toString() + '">' + j.toString() + '</div>');
        j = '0';
    }
    //Attach click event on all numpadbuttons
    $('div[name="numPadKey"]').click(function (event) {
        if ($(this).attr('keyVal') == 'C') {
            $(target).val('');
        }
        else if ($(target).val().length < $(target).attr('maxlength')) {
            $(target).val($(target).val() + $(this).attr('keyVal'));
        }
    });
}

function PopulateProcessData(id) {
    //Get Ajax Method Name to be called for getting process data
    //the returned data structure will have Image URL, Item/Process Name and Keywords for optimizing the search @ client
    AppendItemContents(id, "Alteration", "images/process/Alteration.png", "process");
    AppendItemContents(id, "Bleach", "images/process/Bleach.png", "process");
    AppendItemContents(id, "Calender", "images/process/Calender.png", "process");
    AppendItemContents(id, "Carpet Cleaning", "images/process/Carpet Cleaning.png", "process");
    AppendItemContents(id, "Chemical Wash", "images/process/Chemical Wash.png", "process");
    AppendItemContents(id, "Darning", "images/process/Darning.png", "process");
    AppendItemContents(id, "Dry Cleaning", "images/process/Dry Cleaning.png", "process");
    AppendItemContents(id, "Dye", "images/process/Dye.png", "process");
    AppendItemContents(id, "HandWash", "images/process/HandWash.png", "process");
    AppendItemContents(id, "Iron", "images/process/Iron.png", "process");

}
var count = 0;
function AppendItemContents(container, itemName, imageName, type) {
    count++;
    $(container).append('<div id="' + type + count + '" name="' + type + 'Items"></div>');
    $('#' + type + count).append('<img name="' + type + 'Img" src="' + imageName + '"></img>');
    $('#' + type + count).append('<span>' + itemName + '</span>');
    //Attach click event on the new DIV created
    if (type == 'process') {
        $('#process' + count).click(function () {
            if ($(this).children('img[name=processCheck]').length == 0) {
                $(this).append('<img src="images/checkmark_1.gif" name="processCheck"></img>');
            }
            else {
                $(this).children('img[name=processCheck]').remove();
            }
        });
    }
    else if (type == 'item') {
        $('#item' + count).click(function () {
            if ($(this).children('img[name=itemCheck]').length == 0) {
                $(this).append('<img src="images/checkmark_1.gif" name="itemCheck"></img>');
            }
            else {
                $(this).children('img[name=itemCheck]').remove();
            }
        });
    }
}
function PopulateItemsData(id) {
    count = 0;
    AppendItemContents(id, "Apron", "images/item/Apron.png", "item");
    AppendItemContents(id, "Bag", "images/item/Bag.png", "item");
    AppendItemContents(id, "Bath Mat2", "images/item/Bath Mat2.png", "item");
    AppendItemContents(id, "Bath Robe", "images/item/Bath Robe.png", "item");
    AppendItemContents(id, "Bedsheet (Single)", "images/item/Bedsheet (Single).png", "item");
    AppendItemContents(id, "Blazer (ladies)", "images/item/Blazer (ladies)2.png", "item");
    AppendItemContents(id, "Blazer Gents", "images/item/Blazer Gents.png", "item");
    AppendItemContents(id, "Blouse (NORMAL)", "images/item/Blouse (NORMAL).png", "item");
    AppendItemContents(id, "Cardigan", "images/item/Cardigan.png", "item");
    AppendItemContents(id, "Cushion Cover", "images/item/Cushion Cover.png", "item");
    AppendItemContents(id, "Coat Gents", "images/item/Coat Gents.png", "item");
    AppendItemContents(id, "Dressing Gown", "images/item/Dressing Gown.png", "item");
    AppendItemContents(id, "Frock Kids", "images/item/Frock Kids.png", "item");

    AppendItemContents(id, "Apron", "images/item/Apron.png", "item");
    AppendItemContents(id, "Bag", "images/item/Bag.png", "item");
    AppendItemContents(id, "Bath Mat2", "images/item/Bath Mat2.png", "item");
    AppendItemContents(id, "Bath Robe", "images/item/Bath Robe.png", "item");
    AppendItemContents(id, "Bedsheet (Single)", "images/item/Bedsheet (Single).png", "item");
    AppendItemContents(id, "Blazer (ladies)", "images/item/Blazer (ladies)2.png", "item");
    AppendItemContents(id, "Blazer Gents", "images/item/Blazer Gents.png", "item");
    AppendItemContents(id, "Blouse (NORMAL)", "images/item/Blouse (NORMAL).png", "item");
    AppendItemContents(id, "Cardigan", "images/item/Cardigan.png", "item");
    AppendItemContents(id, "Cushion Cover", "images/item/Cushion Cover.png", "item");
    AppendItemContents(id, "Coat Gents", "images/item/Coat Gents.png", "item");
    AppendItemContents(id, "Dressing Gown", "images/item/Dressing Gown.png", "item");
    AppendItemContents(id, "Frock Kids", "images/item/Frock Kids.png", "item");
}
function FilterAllItems(id, e) {
    value = $(id).val();
    if (e.keyCode == 13) {
        if (value == "") {
            $('div[name=itemItems] span').each(function () {
                $(this).parent().show();
            });
        }
        else {
            $('div[name=itemItems] span').each(function () {
                $(this).filter(function () {
                    return !($(this).html().toString().toUpperCase().match(value.toString().toUpperCase()))
                }).parent().hide();
            });
        }
    }
}
function CreateWizard(container) {
    var steps = $(container).find('fieldset');
    var stepCount = steps.length;
    steps.each(function (i) {
        $(this).wrap('<div id="step' + i + '"></div>');
        $('#commands').append("<p id='step" + i + "commands'></p>");

        if (i == 0) {
            createNextButton(i);
            selectStep(i);
        }
        else if (i == stepCount - 1) {
            $("#step" + i).hide();
            createPrevButton(i);
//            //Add finish button
//            $("#step" + i + "commands").append('<a href="javascript:showOfferText();" id="finish" class="next">Finish</a>');
        }
        else {
            $("#step" + i).hide();
            createPrevButton(i);
            createNextButton(i);
        }


    });
}
function createPrevButton(i) {
    var stepName = "step" + i;
    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Prev' class='prev'>< Back</a>");

    $("#" + stepName + "Prev").bind("click", function (e) {
        $("#" + stepName).hide();
        $("#step" + (i - 1)).show();
        selectStep(i - 1);
    });
}

function createNextButton(i) {
    var stepName = "step" + i;
    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Next' class='next'>Next ></a>");

    $("#" + stepName + "Next").bind("click", function (e) {
        $("#" + stepName).hide();
        $("#step" + (i + 1)).show();
        //if (i + 2 == count)
            //$('#finish').show();
        selectStep(i + 1);
    });
}
var loaded1 = '0';
var loaded2 = '0';
var loaded3 = '0';
function selectStep(i) {
    $("#commands p").removeClass("currentSel");
    $("#step" + i + "commands").addClass("currentSel");
    if (i == 2 && loaded1 == '0') { loaded1 = '1'; CreateNumPad('#numPad', '#txtClothQty'); }
    if (i == 3 && loaded2 == '0') { loaded2 = '1'; PopulateProcessData('#process'); }
    if (i == 4 && loaded3 == '0') { loaded3 = '1'; PopulateItemsData('#items'); }
}

