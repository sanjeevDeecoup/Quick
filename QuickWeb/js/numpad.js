//////////////////////NumPad//////////////////////////////////////
function LoadNumPad(divName) {
    //Append child elements
    var i, j;
    for (i = 0; i <= 11; i++) {
        var temp = "";
        j = (i + 1).toString();
        if (j == '10') { j = 'C'; }
        if (j == '11') { j = '0'; }
        if (j == '12') {
            j = '.';
            temp = "dotkey=true";
        }
        $(divName).append('<div ' + temp + ' class="key activeKey" name="numPadKey" keyVal="' + j.toString() + '">' + j.toString() + '</div>');
        j = '0';
        temp = "";
    }
}
function AttachEventsForNumPad(divName) {
    var closeNumPadFlag = false;
    $("*[rel=numPad]").live("click", function () {
        var showDecimal = true;
        if ($(this).attr("showDecimal") != null && $(this).attr("showDecimal") == "false") {
            showDecimal = false;
        }
        if (showDecimal == false) {
            $(divName + ' div[dotkey=true]').removeClass("activeKey");
            $(divName + ' div[dotkey=true]').addClass("inactiveKey");
        }
        else {
            $(divName + ' div[dotkey=true]').addClass("activeKey");
            $(divName + ' div[dotkey=true]').removeClass("inactiveKey");
        }
        var txtName1 = $(this).attr("id");
        var pos = $(this).attr("showNumPad");
        var x, y;
        if (pos == "right") {
            x = $(this).position().left + $(this).width();
            y = ($(this).position().top + ($(this).height() / 2)) - ($(divName).height() / 2);
        }
        else if (pos == "top") {
            x = ($(this).offset().left + $(this).width() / 2) - ($(divName).width() / 2);
            y = $(this).offset().top - $(divName).height() - 20;
        }
        else if (pos == "bottom") {
            x = ($(this).position().left + $(this).width() / 2) - ($(divName).width() / 2);
            y = $(this).position().top + $(this).height() + 20;
        }

        CloseNumPad();
        $(this).css("background-color", "#ffe6e6");
        $(divName).css("left", x);
        $(divName).css("top", y);
        $(divName).attr("txtName", txtName1);
        $(divName).show();
        closeNumPadFlag = true;
        //Add attribute for first click
        $(divName).attr("firstTime", "1");
    });

    $("*[rel=numPad]").live("change", function () {
        CreateRowFromLineItem();
    });

    //Attach click event on all numpadbuttons
    $(divName + ' div.activeKey').live("click", function (event) {
        var txtName = "#" + $(divName).attr("txtName");

        //Check if this is the first click
        if ($(divName).attr("firstTime") == "1") {
            $(divName).attr("firstTime", "0");
            $(txtName).val('');
            if ($(txtName).val().length == null) {
                $(txtName).text('');
            }
        }
        //event.stopPropagation();
        if ($(this).attr('keyVal') == 'C') {
            $(txtName).val('');
            if ($(txtName).val().length == null) {
                $(txtName).text('');
            }
        }
        else if (($(txtName).val().length + $(this).attr('keyVal').length) <= $(txtName).attr('maxlength')) {
            $(txtName).val($(txtName).val() + $(this).attr('keyVal'));
        }
        else if ($(txtName).val().length == null) {
            if (($(txtName).text().length + $(this).attr('keyVal').length) <= $(txtName).attr('maxlength')) {
                $(txtName).text($(txtName).text() + $(this).attr('keyVal'));
            }
        }
        CreateRowFromLineItem();
        closeNumPadFlag = true;
    });
    $(document).click(function () {
        if (closeNumPadFlag == true) {
            closeNumPadFlag = false;
        }
        else {
            CloseNumPad();
        }
    });
}

function CloseNumPad(flag) {
    $("#numPad").hide();
    $("#" + $("#numPad").attr("txtName")).css("background-color", "#ffffff");
}
//////////////////////~NumPad//////////////////////////////////////