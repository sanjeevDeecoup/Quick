/// <reference path="jquery-1.6.2.min.js" />
/// <reference path="common.js" />

$(document).ready(function () {
    ResetForm();
    ItemFocus();

    formName = "itemMaster";
    imageFolder = "../images";

    LoadItems(null, null, null, "0", "#hiddensubitem", ItemLoadComplete);

    LoadVariations(null, null, null, "0", "#hiddenvariation");
    LoadCategories(null, null, null, "0", "#hiddencategory");
    AttachEventsForKeyboard();
    AttachEventsForTable();
});

function SelectVariations(inputid, dbid, checked) {
    SelectFromList("#undervariation", "itemvariations", inputid, dbid, "allVariationImg", checked);
    //checktest("#undervariation", dbid);
}

function SelectCategories(inputid, dbid, checked) {
    SelectFromList("#undercategory", "itemcategories", inputid, dbid, "allCategoryImg", checked);
    //checktest("#undercategory", dbid, inputid);
}

function SelectSubItems(inputid, dbid, checked) {
    SelectFromList("#undersubitem", "itemsubitems", inputid, dbid, "itemImg", checked);
    //checktest("#undersubitem", dbid, inputid);
}

function SelectFromList(targetDiv, type, inputid, dbid, imageNameTag, checked) {
    //check if item already exists
    if ($(targetDiv + " div.icol1 div[dbid=" + dbid + "]").length > 0) {
        return;
    }
    var source = $(inputid).attr("sourcediv");
    var name = $(source + " div[dbid=" + dbid + "]").children("div").children("span.itemIconNameSpan").text();

    var code = $(source + " div[dbid=" + dbid + "]").children("span.itemIconCodeSpan").text();

    var imagesrc = $(source + " div[dbid=" + dbid + "]").children("img[name=" + imageNameTag + "]").attr("src");

    if (imagesrc != null && imagesrc != "") {
        imagesrc = imagesrc.replace(imageFolder, "");
    }
    var count = $(targetDiv + " div.details div.addedItem").length;

    if (checked == null && count == 0) {
        checked = "checked";
    }

    var typevalue = "";
    if (source == '#hiddensubitem') {
        typevalue = "<input type='checkbox'" + checked + "></input>";
    }
    else
        if (source == '#hiddenvariation') {
            typevalue = "<input type='radio' name='vgroup'" + checked + "></input>";
        }
        else {
            typevalue = "<input type='radio' name='cgroup'" + checked + "></input>";
        }

    $(targetDiv + " div.details").append("<div class='addedItem' id='addedItem" + type + count + "'>"
                                      + "<div class='icol1' id='col1" + type + count + "' onclick='javascript:SelectOption(\"#addedItem" + type + count + "\");'></div>"
                                      + "<div class='icol2' onclick='javascript:SelectOption(\"#addedItem" + type + count + "\");'>" + name + "</div>"
                                      + "<div class='icol3'>" + typevalue + "</div>"
                                      + "<div class='icol4'><a href='javascript:RemoveItem(\"#addedItem" + type + count + "\");' >X</a></div>"
                                  + "</div>"
                          );

    AddItems("div#col1" + type + count, name, code, imagesrc, type, count, null, dbid, null, "true");
}

function RemoveItem(itemID) {
    $(itemID).remove();
}

function SelectOption(itemID) {
    var checkbox = null;
    if ($(itemID).find("input:checkbox").length > 0) {
        checkbox = $(itemID).find("input:checkbox");
    }
    else {
        checkbox = $(itemID).find("input:radio");
    }

    if (checkbox.is(":checked") == "") {
        checkbox.attr("checked", "true");
    }
    else {
        checkbox.removeAttr('checked')
    }
}

function SaveItem() {
    //Validate Form
    if (ValidateForm() == true) {
        //Create Variation String
        $("#hdnVariations").val(CreateItemString("#undervariation"));
        //Create Sub Item String
        $("#hdnSubItems").val(CreateItemString("#undersubitem"));
        //Create category String
        $("#hdnCategories").val(CreateItemString("#undercategory"));
        __doPostBack("btnSave", "");
    }
}

function ValidateForm() {
    var retVal = false;
    var errMsg = "";
    if ($("#txtName").val() == "") {
        errMsg = "Item Name cannot be blank.\n\r\n";
    }
    if ($("#txtCode").val() == "") {
        errMsg += "Item Code cannot be blank.\n";
    }
    var ret = false;
    $("#hiddensubitem div.itemIconContainer div.itemIconName span.itemIconNameSpan").each(function () {
        if ($(this).text().toUpperCase() == $("#txtName").val().toUpperCase()) {
            ret = true;
        }
    });
    if (ret == true) {
        errMsg += "Duplicate Value";
    }
    if (errMsg == "") {
        retVal = true;
    }
    else {
        alert(errMsg);
    }
    return retVal;
}

function CreateItemString(divName) {
    var retVal = $(divName + " div.details div.addedItem div.icol1 div")

    .map(function () {
        var id = $(this).attr("dbid");

        if (id != null && id != "") {
            if ($(this).parent().parent().find("div.icol3 input").is(":checked")) {
                return id + "~1";
            }
            else {
                return id + "~0";
            }
        }
    }).get().join(",");
    return retVal;
}

function ItemLoadComplete() {
    //Load Items Table
    var itemsTable = "#itemsTable";
    $(itemsTable + " tbody").children().remove();
    $("#hiddensubitem div.itemIconContainer").each(function () {
        var itemID = $(this).attr("dbid");
        var itemImage = $(this).children("img.itemIconImage").attr("origImg");
        var itemName = $(this).children("div.itemIconName").children("span.itemIconNameSpan").text();
        var itemCode = $(this).children("span.itemIconCode").text();
        var subItemList = $(this).attr("subitemids");

        var subItemNames = GetNames(subItemList, "#hiddensubitem");

        var variationList = $(this).attr("varids");
        var variationNames = GetNames(variationList, "#hiddenvariation");
        var categoryList = $(this).attr("catids");
        var categoryNames = GetNames(categoryList, "#hiddencategory");
        //Append rows in itemsTable
        $(itemsTable + " tbody").append("<tr>"
                                        + "<td class='col1'><span>" + itemID + "</span>"
                                        + "<td class='col2'><span>" + itemName + "</span>"
                                        + "<td class='col3'><span>" + itemCode + "</span>"
                                        + "<td class='col4'><span>" + subItemNames + "</span><span class='hidden'>" + subItemList + "</span>"
                                        + "<td class='col5'><span>" + variationNames + "</span><span class='hidden'>" + variationList + "</span>"
                                        + "<td class='col6'><span>" + categoryNames + "</span><span class='hidden'>" + categoryList + "</span>"
                                        + "<td class='col7'><span>" + itemImage + "</span>"
                                      + "</tr>");
    });
}

function GetNames(dbidslist, sourceDiv) {
    var retList = "";
    var temp = dbidslist.split(sep2);

    for (var i = 0; i < temp.length; i++) {
        if (temp[i] != null && temp[i] != "") {
            var tempid = temp[i].split(sep1);
            if (tempid != null && tempid != "") {
                retList += GetNameFromDBID(tempid[0], sourceDiv) + " ";
            }
        }
    }

    return retList;
}
function GetNameFromDBID(dbid, sourceDiv) {
    return $(sourceDiv + " div[dbid=" + dbid + "]").children("div.itemIconName").children("span.itemIconNameSpan").text();
}

function AttachEventsForTable() {
    var itemsTable = "#itemsTable";
    $(itemsTable + " tbody tr").live("click", function () {
        $(itemsTable + " tbody tr").removeClass("selectedRow");
        $(this).addClass("selectedRow");
        SelectItem();
    });
}
function SelectItem() {
    ResetForm();
    var row = "#itemsTable tbody tr.selectedRow";
    $("#hdnItemID").val($(row + " td.col1 span").text());
    $("#txtName").val($(row + " td.col2 span").text());
    $("#txtCode").val($(row + " td.col3 span").text());
    $("#tempImage").attr("src", imageFolder + $(row + " td.col7 span").text());
    $("#hdnImageName").val($(row + " td.col7 span").text());
    LoadItemDetails("subitems", $(row + " td.col4 span.hidden").text());
    LoadItemDetails("variations", $(row + " td.col5 span.hidden").text());
    LoadItemDetails("categories", $(row + " td.col6 span.hidden").text());
}

function LoadItemDetails(detailType, dbidslist) {
    var temp = dbidslist.split(sep2);
    for (var i = 0; i < temp.length; i++) {
        if (temp[i] != null && temp[i] != "") {
            var tempid = temp[i].split(sep1);
            if (tempid != null && tempid != "") {
                var checked = "unchecked";
                if (tempid[1] == "1") {
                    checked = "checked";
                }
                switch (detailType) {
                    case "variations":

                        SelectVariations("#txtVariation", tempid[0], checked);
                        break;
                    case "categories":
                        SelectCategories("#txtCategory", tempid[0], checked);
                        break;
                    case "subitems":
                        SelectSubItems("#txtSubItem", tempid[0], checked);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

function ResetForm() {
    $("#hdnItemID").val("0");
    $("input[type=text]").val('');
    $("#tempImage").attr("src", "");
    $("input[type=hidden]").val('');
    $("#undervariation div.details").children().remove();
    $("#undercategory div.details").children().remove();
    $("#undersubitem div.details").children().remove();
}

function ItemFocus() {
    $('#mainContainer #NewItemgroup #unorderlist input:eq(0)').focus();
}