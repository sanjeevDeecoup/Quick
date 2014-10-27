
var promoServiceUrl = "http://localhost/appServices/PromoSchemeService.svc";

$(document).ready(function () {
    //Load Categories
    LoadCategories(null, null, null, "0", "#itemCategories");
    //Load Processes
    LoadProcesses(null, null, null, "0", "#processes");
    //SetProcessPanelHeaderText(1, 1);
    //Load Colors & Patterns
    LoadPatterns(null, null, null, "0", "#patterns");
    LoadColors(null, null, null, "0", "#colors");
    //Attach Numpad
    LoadNumPad("#numPad");
    AttachEventsForNumPad("#numPad");
    //Add New functionality
    LoadLineItemTemplate(null, null, null, "0", "#lineItemTemplate");
    AttachEventsForNew();
    //Add Update functionality
    AttachEventsForUpdate();
    //Add Delete functionality
    AttachEventsForDelete();
    //Add Process Previous functionality
    //AttachEventForPreviousProcess();
    //Add Process Next functionality
    //AttachEventForNextProcess();
    //Attach Table Events
    AttachEventForRowClick();
    //Create the first row
    AddNewRow(true);
    //Attach Feedback Events
    AttachEventsForFeedback();
    //Attach Div Button Events
    AttachEventsForDivButtons();
    //Attach events for Date Picker
    AttachEventsForDatePicker();
    //Load Items
    LoadItems(null, null, null, "0", "#catItems");
});

//////////////////////Common Functions//////////////////////////////////////
function DoAjaxCall(requestBody, successFunction, errorFunction, arg0, arg1, arg2) {
    var request = '<s:Envelope xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:s="http://www.w3.org/2003/05/soap-envelope">' +
                  '<s:Header>' +
                    '<a:Action s:mustUnderstand="1">http://tempuri.org/IPromoSchemeService/' + 
                    arg0 +
                    '</a:Action>' +
                    '<a:MessageID>urn:uuid:903ab3b0-135b-406f-8a88-136f0bedaf3a</a:MessageID>' +
                    '<a:ReplyTo>' +
                      '<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>' +
                    '</a:ReplyTo>' +
                    '<a:To s:mustUnderstand="1">' + promoServiceUrl + '</a:To>' +
                  '</s:Header>' +
                  '<s:Body>' +
                    requestBody +
                  '</s:Body>' +
                '</s:Envelope>';

    jQuery.support.cors = true;
    $.ajax({
        type: "POST",
        url: promoServiceUrl,
        data: request,
        timeout: 10000,
        contentType: "application/soap+xml",
        dataType: "xml",
        async: false,
        success: function (data, status, xhr) {
            successFunction(data, status, xhr, arg1, arg2)
        },
        error: function (xhr, status, error) {
            //AjaxError(xhr, status, error, targetDiv);
            errorFunction(xhr, status, error, arg2);
        }
    });
}

function ShowLoadingDiv(targetDiv, wrapText) {
    //Remove All Children
    $(targetDiv).children().remove();
    //Append Loading image
    $(targetDiv).append('<div name="loading"></div>');
    $(targetDiv + " div[name=loading]").append('<img src="images/loading_3.gif"></img>');
    if (wrapText == "true") {
        $(targetDiv + " div[name=loading]").append('Loading');
    }
    else {
        $(targetDiv + " div[name=loading]").append('<p>Loading</p>');
    }
}

function AjaxError(xhr, status, error, targetDiv) {
    //Remove All Children
    $(targetDiv).children().remove();
    $(targetDiv).append('<div name=error></div>');
    $(targetDiv + " div[name=error]").append(
            '<p>Unable to load contents</p>' +
            '<p>Error Details :' + error + '</p>');
}

function GetXmlData(dataDiv, parentNode, infoType, infoName, getAll, xml) {
    var retVal = null;
    var xmlstring;
    if (xml != null) {
        xmlstring = unescape(xml);
    }
    else {
        xmlstring = unescape($(dataDiv).html());
    }
    xmlDoc = $.parseXML(xmlstring);
    $xmlstring = $(xmlDoc);
    switch (infoType) {
        case "attribute":
            retVal = $xmlstring.find(parentNode).map(function () {
                return ($(this).attr(infoName));
            }).get().join(" ");
            break;
        case "nodeText":
            if (getAll == "true") {
                retVal = $xmlstring.find(parentNode).map(function () {
                    return $(this).text();
                }).get().join("~");
            }
            else {
                retVal = $xmlstring.find(parentNode).text();
            }
            break;
        case "nodeName":
            retVal = $xmlstring.find(parentNode).children().map(function () {
                return this.nodeName;
            }).get().join(" ");
            break;
        case "xml":
            //For IE
            $xmlstring.find(parentNode).each(function () {
                retVal = this.xml;
            });
            //For FF
            if (retVal == null) {
                $xmlstring.find(parentNode).each(function () {
                    $('#test').append($(this));
                    retVal = $('#test').html();
                    $('#test').html('');
                });
            }
            break;
        default:
            break;
    }
    return retVal;
}

function SetXmlData(xml, selectorString, txtValue, attributeName) {
    var xmlstring = unescape(xml);
    xmlDoc = $.parseXML(xmlstring);
    $xmlstring = $(xmlDoc);
    //if attribute is not null, then set attribute value with txtValue
    if (attributeName != null) {
        $xmlstring.find(selectorString).attr(attributeName, txtValue);
    }
    else {
        //replace existing contents with the new text
        $xmlstring.find(selectorString).text(txtValue);
    }
    var retVal = xmlDoc.xml;
    if (retVal == null) {
        $('#test').append($(xmlDoc).contents());
        retVal = $('#test').html();
        $('#test').html('');
    }
    return retVal;

}

function IsDivLoaded(targetDiv) {
    if ($(targetDiv).attr("loaded") == "1")
        return true;
    else
        return false;
}

function MarkDivAsLoaded(targetDiv) {
    $(targetDiv).attr("loaded", "1");
}

function AddItems(targetDiv, itemName, itemCode, imageName, type, count, targetText, dbid, additionalAttr) {
    //if (type == "item") { //debugger; }
    $(targetDiv).append('<div dbid=' + dbid + ' id="' + type + count + '" name="' + type + 'Items" value="0" ' + additionalAttr + '></div>');
    $('#' + type + count).append('<img name="' + type + 'Img" src="' + imageName + '"></img>');
    $('#' + type + count).append('<span>' + itemName + '</span>');
    $('#' + type + count).append('<span name="code">' + itemCode + '</span>');
    //Attach click event on the new DIV created
    $('#' + type + count).click(function () {
        if (!IsDivMarked(this)) {
            if (type == "category") {
                //UnMark all other divs and Mark this div
                UnmarkDiv("#itemCategories div[name=categoryItems]", type + "Check", type);
            }
            else if (type == "item") {
                //Unmark all other divs
                UnmarkDiv("#catItems div[name=itemItems]", type + "Check", type);
            }
            else if (type == "process") {
                //Unmark all other divs
                //UnmarkDiv("#processes div[name=processItems]", type + "Check", type);
                //Allow Multiple selections
            }
            MarkDiv(this, type + "Check", type);
        }
        else {
            if (type == "category") {
                //Do Nothing
            }
            else {
                UnmarkDiv(this, type + "Check", type);
            }
        }
        if (targetText != null) {
            CreateTargetText(targetDiv, targetText, "div[name=" + type + "Items]");
        }
        CreateRowFromLineItem();
    })
    .mouseover(function (e) {
        var tooltipText = $(this).children("span[name!=code]").text();
        $("#tooltipDiv").text(tooltipText);
        $("#tooltipDiv").css("left", e.pageX + 5);
        $("#tooltipDiv").css("top", e.pageY + 5);
        $("#tooltipDiv").show();
    })
    .mouseout(function () {
        $("#tooltipDiv").hide();
    });
}
function UnmarkDiv(divName, imageNameTag, type, resetFlag) {
    $(divName).each(function () {
        if (type == "category") {
            $(divName).removeClass("selectedCategory");
        }
        else {
            $(this).children('img[name=' + imageNameTag + ']').remove();
        }
        $(this).attr("value", "0");
        if (type == "process" && resetFlag != true) {
            RemoveSelectedProcessDesc("#" + $(this).attr("id") + "Desc");
        }
    });
}
function MarkDiv(divName, imageNameTag, type) {
    $(divName).each(function () {
        if (type == "category") {
            $(divName).addClass("selectedCategory");
            LoadItemsForSelectedCategory(this);
        }
        else {

            $(this).append('<img src="images/checkmark_1.gif" name="' + imageNameTag + '"></img>');
            var top = $(this).height();
            $(this).children('img[name=' + imageNameTag + ']').css("top", "-" + top.toString() + "px");
        }
        $(this).attr("value", "1");
        if (type == "process") {
            //Create ProcessDescItems
            AddSelectedProcessDesc(divName);
        }
    });
}
function IsDivMarked(divName) {
    return $(divName).attr("value") == "1";
}

function GetDBID(divName) {
    return $(divName).attr("dbid");
}
function GetDBIDs(divName) {
    return $(divName).map(function () {
        return GetDBID(this);
    }).get().join(" ");
}

//////////////////////~Common Functions//////////////////////////////////////

//////////////////////Categories//////////////////////////////////////
function LoadCategories(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllCategories xmlns="http://tempuri.org/" />';
            var sFn = LoadCategories;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllCategories", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllCategoriesResponse").find("GetAllCategoriesResult").text();
            var tempIds = GetXmlData(null, "categories category", "attribute", "id", "true", xmlData).split(" ");
            //var sampleDiv = '<div dbid="@CategoryID"><span>@CategoryName</span></div>';
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv, 
                    GetXmlData(null, "category[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    GetXmlData(null, "category[id=" + tempIds[i] + "] Code", "nodeText", null, null, xmlData),
                    GetXmlData(null, "category[id=" + tempIds[i] + "] Image", "nodeText", null, null, xmlData),
                        "category",i,null,tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}


function LoadItemsForSelectedCategory(divName) {
    var catid = GetDBID(divName);
    //Hide all Items that do not belong to this category
    $("#catItems div[catid!=" + catid + "]").hide();
    $("#catItems div[catid=" + catid + "]").show();
}

//////////////////////~Categories//////////////////////////////////////

//////////////////////Items//////////////////////////////////////
var lastItemNum = null;
function LoadItems(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv);
            var requestBody = '<GetAllItems xmlns="http://tempuri.org/" />';
            var sFn = LoadItems;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllItems", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllItemsResponse").find("GetAllItemsResult").text();
            var tempIds = GetXmlData(null, "items item", "attribute", "id", "true", xmlData).split(" ");
            lastItemNum = tempIds.length - 1;
            for (var i = 0; i < tempIds.length; i++) {
                var itemXml = GetXmlData(null, "item[id=" + tempIds[i] + "]", "xml", null, null, xmlData);
                var itemName = GetXmlData(null, "item[id=" + tempIds[i] + "] name", "nodeText", null, null, itemXml);
                if (itemName == "" || itemName == null) {
                    itemName = GetXmlData(null, "item[id=" + tempIds[i] + "] Name", "nodeText", null, null, itemXml);
                }
                var itemCode = GetXmlData(null, "item[id=" + tempIds[i] + "] Code", "nodeText", null, null, itemXml);
                if (itemCode == "" || itemCode == null) {
                    itemCode = GetXmlData(null, "item[id=" + tempIds[i] + "] code", "nodeText", null, null, itemXml);
                }
                var itemImage = GetXmlData(null, "item[id=" + tempIds[i] + "] Image", "nodeText", null, null, itemXml);
                if (itemImage == "" || itemImage == null) {
                    itemImage = GetXmlData(null, "item[id=" + tempIds[i] + "] image", "nodeText", null, null, itemXml);
                }
                var catid = GetXmlData(null, "item[id=" + tempIds[i] + "]", "attribute", "catid", null, itemXml);
                var itemID = tempIds[i];
                //alert(itemXml.toString().toLowerCase());
                LoadItemsDiv(targetDiv, itemName, itemCode, itemImage, "item", i, null, itemID, "catid=" + catid);
            }
            MarkDivAsLoaded(targetDiv);
            //Hide all items by default
            //$(targetDiv).children().hide();
            //Select the first Category by default
            //MarkDiv("#itemCategories div[name=categoryItems]:nth-child(1)", "", "category");
        }
    }
}
function LoadItemsDiv(targetDiv, itemName, itemCode, itemImage, type, count, targetText, itemID, additional) {
    setTimeout(function () {
        AddItems(targetDiv, itemName, itemCode, itemImage, type, count, targetText, itemID, additional);
        if (count == lastItemNum) {
            //Select the first Category
            MarkDiv("#itemCategories div[name=categoryItems]:nth-child(1)", "", "category");
        }
    }, 10);
}
//////////////////////~Items//////////////////////////////////////

//////////////////////Processes//////////////////////////////////////
function LoadProcesses(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllProcesses xmlns="http://tempuri.org/" />';
            var sFn = LoadProcesses;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllProcesses", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllProcessesResponse").find("GetAllProcessesResult").text();
            var tempIds = GetXmlData(null, "processes process", "attribute", "id", "true", xmlData).split(" ");
            //var sampleDiv = '<div dbid="@CategoryID"><span>@CategoryName</span></div>';
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv,
                    GetXmlData(null, "process[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    GetXmlData(null, "process[id=" + tempIds[i] + "] Code", "nodeText", null, null, xmlData),
                    GetXmlData(null, "process[id=" + tempIds[i] + "] Image", "nodeText", null, null, xmlData),
                        "process", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}
function RemoveSelectedProcessDesc(divName) {
    $(divName).remove();
    CreateRowFromLineItem();
}
function AddSelectedProcessDesc(processDiv) {
    var id = $(processDiv).attr("id");
    var code = $(processDiv).children("span[name=code]").text();
    $("#processDescItems").append('<div id="' + id + 'Desc" rel="processDescItem" code="'+ code +'" processdbid="' + GetDBID("#" + id) +'">'
                                    + '<span name="code">' + code + ' @</span>'
                                    + '<input id="' + $(processDiv).attr('id') + 'text" maxlength="5" type="text" rel="numPad" showNumPad="top" createRow="true" />'
                                    + '<a href=\'javascript:UnmarkDiv("#' + id + '", "processCheck", "process");\'>x</a>'
                                + '</div>');
}
function UpdateProcess() {
    $("#processData").children("div[rel=processData]").remove();
    var seq = 0;
    $("#processDescItems").children("div[rel=processDescItem]").each(function () {
        seq++;
        var amount = 0;
        if (isNaN($(this).children("input[type=text]").val()) == false && isNaN($("#txtQuantity").val()) == false) {
            amount = parseInt($(this).children("input[type=text]").val())
                        * parseInt($("#txtQuantity").val());
        }
        $("#processData").append('<div rel="processData" dbid="" rate="" amount="" sequence="' + seq + '"></div>');
        var divName = "#processData div[sequence=" + seq + "]";
        SetProcessDataAttributes(divName, $(this).attr("processdbid"),
                                    $(this).children("input[type=text]").val(),
                                    amount, seq);
    });
}
function SetProcessDataAttributes(divName, dbid, rate, amount, sequence) {
    if (dbid == null) {
        dbid = "";
    }
    $(divName).attr("dbid", dbid);
    $(divName).attr("rate", rate);
    $(divName).attr("amount", amount);
    $(divName).attr("sequence", sequence);
}

//////////////////////~Processes//////////////////////////////////////

//////////////////////Patterns//////////////////////////////////////
function LoadPatterns(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllPatterns xmlns="http://tempuri.org/" />';
            var sFn = LoadPatterns;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllPatterns", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllPatternsResponse").find("GetAllPatternsResult").text();
            var tempIds = GetXmlData(null, "patterns pattern", "attribute", "id", "true", xmlData).split(" ");
            //var sampleDiv = '<div dbid="@CategoryID"><span>@CategoryName</span></div>';
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv,
                    GetXmlData(null, "pattern[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    GetXmlData(null, "pattern[id=" + tempIds[i] + "] Code", "nodeText", null, null, xmlData),
                    GetXmlData(null, "pattern[id=" + tempIds[i] + "] Image", "nodeText", null, null, xmlData),
                        "pattern", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}

//////////////////////~Patterns//////////////////////////////////////

//////////////////////Colors//////////////////////////////////////
function LoadColors(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllColors xmlns="http://tempuri.org/" />';
            var sFn = LoadColors;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllColors", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllColorsResponse").find("GetAllColorsResult").text();
            var tempIds = GetXmlData(null, "colors color", "attribute", "id", "true", xmlData).split(" ");
            //var sampleDiv = '<div dbid="@CategoryID"><span>@CategoryName</span></div>';
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv,
                    GetXmlData(null, "color[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    GetXmlData(null, "color[id=" + tempIds[i] + "] Code", "nodeText", null, null, xmlData),
                    GetXmlData(null, "color[id=" + tempIds[i] + "] Image", "nodeText", null, null, xmlData),
                        "color", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}

//////////////////////~Colors//////////////////////////////////////

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
    $("input:text[rel=numPad]").live("click", function () {
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
        }
        else if (($(txtName).val().length + $(this).attr('keyVal').length) <= $(txtName).attr('maxlength')) {
            $(txtName).val($(txtName).val() + $(this).attr('keyVal'));
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

//////////////////////Right Commands//////////////////////////////////////
function LoadLineItemTemplate(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            var requestBody = '<GetLineItemTemplate xmlns="http://tempuri.org/" />';
            var sFn = LoadLineItemTemplate;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetLineItemTemplate", "1", targetDiv);
            }, 500);
        }
        else {
            //Load Template
            $(targetDiv).text(escape($(data).find("GetLineItemTemplateResponse").find("GetLineItemTemplateResult").text()));
            MarkDivAsLoaded(targetDiv);
        }
    }
}

function AttachEventsForNew() {
    $("#btnNew").click(function () {
        UpdateProcess();
        var seq = GetSelectedRow();
        SaveLineItem(seq);
        //Load contents of the new Row
        ResetAll();
        AddNewRow();
    });
}
function AddNewRow(firstLoad) {
    var seq = $("#lineItems div[rel=lineItem]").length + 1;
    //Add New Row in Table
    $("#bookingTable tbody").append("<tr sequence=" + seq + "><td class='col1'><span>" + seq + "</span></td><td class='col2'></td><td class='col3'></td><td class='col4'></td><td class='col5'></td></tr>");
    //Add Blank Line Item Div
    $("#lineItems").append('<div rel="lineItem" sequence="' + seq + '"></div>');
    //Select New Row
    SelectRow(seq);
}

function SelectRow(sequence) {
    if (sequence > 0) {
        //Select row in the table
        $("#bookingTable tbody tr").removeClass("selectedRow");
        $("#bookingTable tbody tr[sequence=" + sequence + "]").addClass("selectedRow");
    }
}

function GetSelectedRow() {
    return $("#bookingTable tbody tr[class=selectedRow]").attr("sequence");
}
function AttachEventsForUpdate() {
    $("#btnUpdate").click(function () {
        //Validate data

        //Update Line Item XML
        //SaveLineItem();
    });
}
function AttachEventsForDelete() {
    $("#btnDelete").click(function () {
        if (confirm("do you really want to delete the selected line item?")) {
            DeleteRowItem();
        }
    });
}
function DeleteRowItem() {
    var seq = GetSelectedRow();
    $("#bookingTable tbody tr[sequence=" + seq + "]").remove();
    $("#lineItems div[sequence=" + seq + "]").remove();
}
function SaveLineItem(sequence) {
    if (sequence > 0) {
        var xmlItemNode = SaveItems();
        var xmlPatternsNode = SavePatterns();
        var xmlColorsNode = SaveColors();
        var xmlProcessesNode = SaveProcesses();
        var xmlLineItem = "<lineItem>" + xmlItemNode + xmlPatternsNode + xmlColorsNode + xmlProcessesNode + "</lineItem>";

        //Add this xml in the LineItems Div
        //debugger;
        $("#lineItems").children("div[sequence=" + sequence + "]").html('');
        $("#lineItems").children("div[sequence=" + sequence + "]").html(escape(xmlLineItem));
    }
}

function SaveItems() {
    //Get itemNode from Template
    var itemNodeXml = GetXmlData("#lineItemTemplate", "lineItem item", "xml", null, null, null);
    //ItemID
    itemNodeXml = SetXmlData(itemNodeXml, "item", GetDBID("#catItems div[value=1]"), "id");
    //Quantity
    itemNodeXml = SetXmlData(itemNodeXml, "item quantity", $("#txtQuantity").val());
    //Length
    itemNodeXml = SetXmlData(itemNodeXml, "item length", $("#txtLength").val());
    //Breadth
    itemNodeXml = SetXmlData(itemNodeXml, "item breadth", $("#txtBreadth").val());
    //Area
    itemNodeXml = SetXmlData(itemNodeXml, "item area1", $("#txtArea").val());
    //Remarks
    itemNodeXml = SetXmlData(itemNodeXml, "item remarks", $("#txtItemRemarks").val());
    //Brand
    itemNodeXml = SetXmlData(itemNodeXml, "item brand", $("#txtBrand").val());
    //Text
    itemNodeXml = SetXmlData(itemNodeXml, "item text", $("#catItems div[value=1] span[name!=code]").text());

    return itemNodeXml;
}
function SavePatterns() {
    var patternsNodeXml = GetXmlData("#lineItemTemplate", "lineItem patterns", "xml", null, null, null);
    //Get Allselected patternsIDS
    var tempids = GetDBIDs("#patterns div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "patterns pattern", "xml", null, null, patternsNodeXml);
    var tempNode = null;
    var allPatterns = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get pattern node
        tempNode = tempNodeXml;
        //PatternID
        tempNode = SetXmlData(tempNode, "pattern", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "pattern text", $("#patterns div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allPatterns += tempNode;
    }
    patternsNodeXml = ReplaceNode(patternsNodeXml, "<pattern ", "</pattern>", allPatterns);
    return patternsNodeXml;
}

function ReplaceNode(sourceString, startNode, endNode, replaceWith) {
    var start = sourceString.indexOf(startNode, 0);
    var end = sourceString.indexOf(endNode, start);
    var replaceText = sourceString.substring(start, end + endNode.length);
    sourceString = sourceString.replace(replaceText, replaceWith);
    return sourceString;
}
function SaveColors() {
    var colorsNodeXml = GetXmlData("#lineItemTemplate", "lineItem colors", "xml", null, null, null);
    //Get Allselected patternsIDS
    var tempids = GetDBIDs("#colors div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "colors color", "xml", null, null, colorsNodeXml);
    var tempNode = null;
    var allColors = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get Color node
        tempNode = tempNodeXml;
        //ColorID
        tempNode = SetXmlData(tempNode, "color", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "color text", $("#colors div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allColors += tempNode;
    }
    colorsNodeXml = ReplaceNode(colorsNodeXml, "<color ", "</color>", allColors);
    return colorsNodeXml;
}
function SaveProcesses() {
    var processesNodeXml = GetXmlData("#lineItemTemplate", "lineItem processes", "xml", null, null, null);
    //Get Allselected patternsIDS
    var tempids = GetDBIDs("#processData div[rel=processData]").split(" ");
    var tempNodeXml = GetXmlData(null, "processes process", "xml", null, null, processesNodeXml);
    var tempNode = null;
    var allProcesses = "";
    for (var i = 0; i < tempids.length; i++) {
        if (tempids[i] != "") {
            //Get Process node
            tempNode = tempNodeXml;
            //ProcessID
            tempNode = SetXmlData(tempNode, "process", tempids[i], "id");
            //Rate
            tempNode = SetXmlData(tempNode, "process rate", $("#processData div[dbid=" + tempids[i] + "]").attr("rate"));
            //Amount
            tempNode = SetXmlData(tempNode, "process amount", $("#processData div[dbid=" + tempids[i] + "]").attr("amount"));
            //text
            tempNode = SetXmlData(tempNode, "process text", $("#processes div[dbid=" + tempids[i] + "] span[name=code]").text() + "@" + $("#processData div[dbid=" + tempids[i] + "]").attr("rate"));
            allProcesses += tempNode;
        }
    }
    processesNodeXml = ReplaceNode(processesNodeXml, "<process ", "</process>", allProcesses);
    return processesNodeXml;
}

function LoadLineItem(sequence) {
    if (sequence > 0) {
        //ResetAll();
        //Get line item XML
        var lineItemXml = unescape($("#lineItems div[sequence=" + sequence + "]").html());
        LoadLineItemItems(lineItemXml);
        LoadLineItemPatterns(lineItemXml);
        LoadLineItemColors(lineItemXml);
        LoadLineItemProcesses(lineItemXml);
    }
}

function LoadLineItemItems(lineItemXml) {
    //Get itemNode from Template
    var itemNodeXml = GetXmlData(null, "lineItem item", "xml", null, null, lineItemXml);
    //Set ItemID
    var controlValue = GetXmlData(null, "item", "attribute", "id", null, lineItemXml);
    var tempid = $("#catItems div[dbid=" + controlValue + "]").attr("catid");
    MarkDiv("#itemCategories div[dbid=" + tempid + "]", "categoryCheck", "category");
    MarkDiv("#catItems div[dbid=" + controlValue + "]", "itemCheck", "item");
    //Quantity
    $("#txtQuantity").val(GetXmlData(null, "item quantity", "nodeText", null, null, itemNodeXml));
    //Length
    $("#txtLength").val(GetXmlData(null, "item length", "nodeText", null, null, itemNodeXml));
    //Breadth
    $("#txtBreadth").val(GetXmlData(null, "item breadth", "nodeText", null, null, itemNodeXml));
    //Area
    $("#txtArea").val(GetXmlData(null, "item area1", "nodeText", null, null, itemNodeXml));
    //Remarks
    $("#txtItemRemarks").val(GetXmlData(null, "item remarks", "nodeText", null, null, itemNodeXml));
    //Brand
    $("#txtBrand").val(GetXmlData(null, "item brand", "nodeText", null, null, itemNodeXml));
    //Text
}

function LoadLineItemPatterns(lineItemXml) {
    var patternsNodeXml = GetXmlData(null, "lineItem patterns", "xml", null, null, lineItemXml);
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "pattern", "attribute", "id", "true", patternsNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //PatternID
        MarkDiv("#patterns div[dbid=" + tempids[i] + "]", "patternCheck", "pattern");
    }
}
function LoadLineItemColors(lineItemXml) {
    var colorsNodeXml = GetXmlData(null, "lineItem colors", "xml", null, null, lineItemXml);
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "color", "attribute", "id", "true", colorsNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //ColorID
        MarkDiv("#colors div[dbid=" + tempids[i] + "]", "colorCheck", "color");
    }
}
function LoadLineItemProcesses(lineItemXml) {
    var processesNodeXml = GetXmlData(null, "lineItem processes", "xml", null, null, lineItemXml);
    //Get All selected processIDS
    var tempids = GetXmlData(null, "process", "attribute", "id", "true", processesNodeXml).split(" ");
    //var seq = 1;
    for (var i = 0; i < tempids.length; i++) {
        //ProcessID
        var rate = GetXmlData(null, "process[id=" + tempids[i] + "] rate", "nodeText", null, null, processesNodeXml);
        var amount = GetXmlData(null, "process[id=" + tempids[i] + "] amount", "nodeText", null, null, processesNodeXml);
        MarkDiv("#processes div[dbid=" + tempids[i] + "]", "processCheck", "process");
        //SetProcessDataAttributes("#processData div[sequence=" + seq + "]", tempids[i], rate, amount, seq);
        //Set Rate
        $("#processDescItems div[processdbid=" + tempids[i] + "] input:text").val(rate);
        //seq++;
    }
}
function ResetAll() {
    ResetItems();
    ResetColorsPatterns();
    ResetProcesses();
}
function ResetItems() {
    //Unmark all category Divs
    UnmarkDiv("#itemCategories div[name=categoryItems]", "categoryCheck", "category");
    //Unmark all Item Divs
    UnmarkDiv("#catItems div[name=itemItems]", "itemCheck", "item");
    //Reset all text baxes
    $("#itemDesc1 input:text").val('');
    $("#itemDesc2 input:text").val('');
    $("#txtQuantity").val("1");
}
function ResetColorsPatterns() {
    //Unmark all patterns
    UnmarkDiv("#patterns div[name=patternItems]", "patternCheck", "pattern");
    //Unmark all colors
    UnmarkDiv("#colors div[name=colorItems]", "colorCheck", "color");
}
function ResetProcesses() {
    ResetProcessControls();
    //Reset Process Data
    $("#processData div").remove();

}
function ResetProcessControls() {
    //Unmark all processes
    UnmarkDiv("#processes div[name=processItems]", "processCheck", "process", true);
    //Reset all text
    $("#processDescItems div").remove();
}
//////////////////////~Right Commands//////////////////////////////////////
////////////////////////////Table Events//////////////////////////////////////
function AttachEventForRowClick() {
    $("#bookingTable tbody tr").live("click", function () {
        //Get current Selected Row
        var seq = GetSelectedRow();
        //Save Current Data
        UpdateProcess();
        SaveLineItem(seq);
        //Select the new Row
        SelectRow($(this).attr("sequence"));
        ResetAll();
        LoadLineItem($(this).attr("sequence"));
    });
}
function CreateRowFromLineItem() {
    var seq = GetSelectedRow();
    var tdSel = "#bookingTable table tbody tr[sequence=" + seq + "]";
    UpdateProcess();
    SaveLineItem(seq);
    var xml = GetLineItemXml(seq);
    //Col 2 - Qty
    $(tdSel + " td:nth-child(2)").children().remove();
    $(tdSel + " td:nth-child(2)").append("<span>" + GetItemQuantity(xml) + "</span>");
    //Col 3 - Desc
    $(tdSel + " td:nth-child(3)").children().remove();
    var tempText = GetPatternsText(xml);
    var str = "";
    tempText = tempText.replace(/~/g, " ~ ");
    var tempText1 = GetColorsText(xml);
    tempText1 = tempText1.replace(/~/g, " ~ ");
    $(tdSel + " td:nth-child(3)").append("<span>" +
                GetItemText(xml) + " - " + tempText + " - " + tempText1
             + "</span");
    //Col 4
    $(tdSel + " td:nth-child(4)").children().remove();
    var tempids = GetProcessesText(xml).split("~");
    var temp = "";
    for (var i = 0; i < tempids.length; i++) {
        if (tempids[i] != "") {
            if (temp != "") {
                temp += "<span>, </span>";
            }
            temp += "<span>" + tempids[i] + "</span>";
        }
    }
    $(tdSel + " td:nth-child(4)").append(temp);
    //Col 5 - Amount
    $(tdSel + " td:nth-child(5)").children().remove();
    var amounts = GetProcessesAmountText(xml).split("~");
    var amount = 0;
    for (var i = 0; i < amounts.length; i++) {
        if (amounts[i] != "") {
            if (isNaN(amounts[i]) == false) {
                amount += parseInt(amounts[i]);
            } 
        }
    }
    $(tdSel + " td:nth-child(5)").append("<span>" + amount + "</span>");
}
function GetLineItemXml(sequence) {
    return unescape($("#lineItems div[sequence=" + sequence + "]").html());
}
function GetItemQuantity(lineItemXml) {
    return GetXmlData(null, "item quantity", "nodeText", null, null, lineItemXml);
}
function GetItemText(lineItemXml) {
    return GetXmlData(null, "item text", "nodeText", null, null, lineItemXml);
}
function GetPatternsText(lineItemXml) {
    return GetXmlData(null, "pattern text", "nodeText", null, "true", lineItemXml);
}
function GetColorsText(lineItemXml) {
    return GetXmlData(null, "color text", "nodeText", null, "true", lineItemXml);
}
function GetProcessesText(lineItemXml) {
    return GetXmlData(null, "process text", "nodeText", null, "true", lineItemXml);
}
function GetProcessesAmountText(lineItemXml) {
    return GetXmlData(null, "process amount", "nodeText", null, "true", lineItemXml);
}
////////////////////////////~Table Events//////////////////////////////////////

function AttachEventsForFeedback() {
//    $("#feedbackHeader").fadeIn(7000, function () {
//        $("#feedbackHeader span").text("Feedback ...");
//    });
//    $("#feedbackHeader").animate({
//        width: '+=50px'
//    }, 5000, function () {
//        $("#feedbackHeader span").text("Feedback ...");
//    });
//    $("#feedbackHeader").slideDown(3000, function () {
//            $("#feedbackHeader span").text("Feedback ...");
//    });
    $("#feedbackHeader").click(function () {
        $(this).children("span").text("Please enter comments and Submit");
        $("#feedbackDetail").show();
    });
    $("#btnClose").click(function () {
        $("#feedbackHeader").hide();
    });
    $("#btnFeedback").click(function () {
        if ($("#txtName").val() == "" || $("#txtFeedback").val() == "") {
            alert("Name and Feedback comments cannot be left blank");
            return;
        }
        var fb = $("#txtName").val() + "~~||~~" + $("#txtFeedback").val();
        var requestBody = '<SaveFeedback xmlns="http://tempuri.org/">' +
                              '<feedbackXml>' + fb + '</feedbackXml>' +
                          '</SaveFeedback>';
        DoAjaxCall(requestBody, null, null, "SaveFeedback", null, null);
        Thanks();
        $("#feedbackHeader").hide();
    });
}
function Thanks() {
    alert("Thanks for your feedback");
}

/////////////////////////////Div Buttons////////////////////
function AttachEventsForDivButtons() {
    $("div.button").each(function () {
        $(this).click(function () {
            //Uncheck all other with same radioGroup name
            var add = true;
            var radioGroup = $(this).attr("radioGroup");
            if ($(this).hasClass("selectedButton")) {
                add = false;
            }
            if (radioGroup != null) {
                $("div.button[radioGroup=" + radioGroup + "]").removeClass("selectedButton");
            }
            if (add == true) {
                $(this).addClass("selectedButton");
            }
        });
    });
}
/////////////////////////////~Div Buttons////////////////////
function AttachEventsForDatePicker() {
    $("#txtDueDate").datepick({ dateFormat: 'D dd-mm-yyyy' });
}