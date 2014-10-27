//////////////////////NumPad//////////////////////////////////////
function LoadNumPad(divName) {
    //Append child elements
    var i, j;
    for (i = 0; i <= 11; i++) {
        j = (i + 1).toString();
        if (j == '10') { j = 'C'; }
        if (j == '11') { j = '0'; }
        if (j == '12') { j = '00'; }
        $(divName).append('<div name="numPadKey" keyVal="' + j.toString() + '">' + j.toString() + '</div>');
        j = '0';
    }
}
function AttachEventsForNumPad(divName) {
    var closeNumPadFlag = false;
    $("*[rel=numPad]").live("click", function () {
        var txtName1 = $(this).attr("id");
        var pos = $(this).attr("showNumPad");
        var x, y;
        if (pos == "right") {
            x = $(this).position().left + $(this).width();
            y = ($(this).position().top + ($(this).height() / 2)) - ($(divName).height() / 2);
        }
        else if (pos == "top") {
            x = ($(this).position().left + $(this).width() / 2) - ($(divName).width() / 2);
            y = $(this).position().top - $(divName).height() - 20;
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
    });
    //Attach click event on all numpadbuttons
    $(divName + ' div[name=numPadKey]').live("click", function (event) {
        //event.stopPropagation();
        var txtName = "#" + $(divName).attr("txtName");

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