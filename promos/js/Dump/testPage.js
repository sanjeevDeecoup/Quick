$(document).ready(function () {
    $("#btnLoadContents").click(function () {
        var container = '#itemContainer';
        $(container).append('<span id="spnTemp" name="loading"><img src="images/loading_3.gif"><p>Loading</p></img></span>');
        $(container).addClass("centerLoading");
        $.ajax({
            type: "POST",
            url: "TestPage.aspx/GetItems",
            data: "{}",
            beforeSend: function (xhr) {
                //xhr.setRequestHeader("Content-length", params.length);
                xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $(container + ' #spnTemp').remove();
                $(container).removeClass("centerLoading");
                for (var name in msg.d)
                { AppendItemContents(msg.d[name]); }
            },
            error: function (msg) {
                alert("error:" + msg.responseText);
                for (var name in msg)
                { alert(name) }
                //document.write(msg); 
            }
        });
        //alert("calling ajax methods end");
    });
});

function delay(time) {
    var d1 = new Date();
    var d2 = new Date();
    while (d2.valueOf() < d1.valueOf() + time) {
        d2 = new Date();
    }
}

var count=0;
function AppendItemContents(name) {
    //create an itemcontent div
    var container = '#itemContainer';
    count++;
    $(container).append('<div id="itemContent' + count + '" class="normalDiv"></div>');
    $('#itemContent' + count).append('<img name="content" src="images/' + name + '.jpg"></img>');
    $('#itemContent' + count).append('<span>' + name + '</span>');
    $('#itemContent' + count).mouseover(function () {
        $(this).removeClass("normalDiv");
        $(this).addClass("highlightDiv");
    });
    $('#itemContent' + count).mouseout(function () {
        $(this).removeClass("highlightDiv");
        $(this).addClass("normalDiv");
    });
}
