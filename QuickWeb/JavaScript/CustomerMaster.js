/// <reference path="jquery-1.6.2.min.js" />
/// <reference path="common.js" />

$(document).ready(function () {
    CreateList(null, null, null, "0", "#hiddencustomer");

    LoadVariations(null, null, null, "0", "#hiddenvariation");
    LoadCategories(null, null, null, "0", "#hiddencategory");
    AttachEventsForKeyboard();
});