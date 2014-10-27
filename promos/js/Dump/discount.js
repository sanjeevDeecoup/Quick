function showTypeChangeContents(select, suf) {
    var showPro, showClo = null;
    var elePro = '#processText' + suf;
    var eleClo = '#clothesText' + suf;
    $(elePro).hide();
    $(eleClo).hide();
    switch ($(select).val()) {
        case "1":
            showPro = "true";
            showClo = "true";
            break;
        case "2":
            showPro = "true";
            break;
        case "3":
            showClo = "true";
            break;
        default:
            break;
    }
    if (showPro == "true") {
        $(elePro).show();
    }
    if (showClo == "true") {
        $(eleClo).show();
    }
}
var count;
function createWizard(container) {
    //find all fieldsets and enclose them in a div
    var steps = $(container).find('fieldset');
    count = steps.size();

    //$(container).before("<ul id='steps'></ul>");
    //$(container).before("<div id='steps'></div>");

    //$(container).append("<div id='commands'></div>");

    //$('#finish').hide();

    steps.each(function (i) {
        $(this).wrap("<div id='step" + i + "'></div>");

        $('#commands').append("<p id='step" + i + "commands'></p>");

        var name = $(this).find("legend").html();
        $("#steps").append("<span id='stepDesc" + (i + 1) + "'>Step 1</span>");
        //$("#promoStepDesc").html("Step " + (i + 1) + " - " + name);
        //$(this).find("legend").remove();
        //$("#steps").append("<li id='stepDesc" + i + "'>Step " + (i + 1) + "<span>" + name + "</span></li>");
        //$("#steps").append("<div id='stepDesc" + i + "'>Step " + (i + 1) + "</div>");

        if (i == 0) {
            createNextButton(i);
            selectStep(i);
        }
        else if (i == count - 1) {
            $("#step" + i).hide();
            createPrevButton(i);
            //Add finish button
            $("#step" + i + "commands").append('<a href="javascript:showOfferText();" id="finish" class="next">Finish</a>');
        }
        else {
            $("#step" + i).hide();
            createPrevButton(i);
            createNextButton(i);
        }
    })
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

function selectStep(i) {
    $("#steps span").removeClass("current");
    $("#stepDesc" + i).addClass("current");
    $("#commands p").removeClass("currentSel");
    $("#step" + i + "commands").addClass("currentSel");
}

function showOfferText() {
    var txt = null;
    //Create Step 1 text
    txt = "Get " + $("#txtDiscountedValueOn").val() + " " + $("#selectDiscountedUnitOn option:selected").text() + " of ";
    var pro, clo;
    var dcVal = $("#selectDiscountTypeOn").val();
    if (dcVal == "1") {
        pro = "true";
        clo = "true";
    }
    else if (dcVal == "2") {
        pro = "true";
    }
    else if (dcVal == "3") {
        clo = "true";
    }
    if (dcVal != "0") {
        if (pro == "true" && clo == "true") {
            txt += $("#txtProcessOn").val() + " of " + $("#txtClothesOn").val();
        }
        else if (pro == "true") {
            txt += $("#txtProcessOn").val();
        }
        else if (clo == "true") {
            txt += $("#txtClothesOn").val();
        }
        else {
            txt += $("#selectDiscountTypeOn option:selected").text();
        }
    }

    //Create Step 2 text
    pro = null;clo = null;dcVal = null;
    txt += " on " + $("#txtDiscountedValueFor").val() + " " + $("#selectDiscountedUnitFor option:selected").text() + " of ";
    dcVal = $("#selectDiscountTypeFor").val();
    if (dcVal == "1") {
        pro = "true";
        clo = "true";
    }
    else if (dcVal == "2") {
        pro = "true";
    }
    else if (dcVal == "3") {
        clo = "true";
    }
    if (dcVal != "0") {
        if (pro == "true" && clo == "true") {
            txt += $("#txtProcessFor").val() + " of " + $("#txtClothesFor").val();
        }
        else if (pro == "true") {
            txt += $("#txtProcessFor").val();
        }
        else if (clo == "true") {
            txt += $("#txtClothesFor").val();
        }
        else {
            txt += $("#selectDiscountTypeFor option:selected").text();
        }
    }
    $("#txtOffer").val(txt);
}