/// <reference path="jquery-1.6.2.min.js" />

(function ($) {
    $.fn.autoComplete = function (listContents) {
    };
})(jQuery);

//Add a list div if not exists
if ($("body div#list") == null) {
    $("body").append("<div id='list'></div>");
}

var list = "#list";