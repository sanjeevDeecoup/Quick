
var promoServiceUrl = "http://localhost/svnappServices/PromoSchemeService.svc";
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
    var extraText = "";
    if (additionalAttr != "" && additionalAttr != null) {
        extraText = additionalAttr;
    }
    $(targetDiv).append('<div class="itemIconContainer" dbid=' + dbid + ' id="' + type + count + '" name="' + type + 'Items" value="0" ' + extraText + '></div>');
    if (imageName != null && imageName != '') {
        $('#' + type + count).append('<img class="itemIconImage" name="' + type + 'Img" src="' + imageName + '"></img>');
    }
    $('#' + type + count).append('<div class="itemIconName" name="' + type + 'Name"><span class="itemIconNameSpan">' + itemName + '</span></div>');
    $('#' + type + count).append('<span class="itemIconCode" name="code">' + itemCode + '</span>');
    //Attach click event on the new DIV created
    $('#' + type + count).click(function (event) {
        //event.stopImmediatePropagation();
        if (!IsDivMarked(this)) {
            MarkDiv(this, type + "Check", type);
        }
        else {
            UnmarkDiv(this, type + "Check", type);
        }
        if (targetText != null) {
            CreateTargetText(targetDiv, targetText, "div[name=" + type + "Items]");
        }
        CreateRowFromLineItem();
        return false;
    })
    .mouseover(function (e) {
        $(this).css("cursor", "pointer");
        var tooltipText = $(this).children("div[name=" + type + "Name]").text();
        $("#tooltipDiv").text(tooltipText);
        $("#tooltipDiv").css("left", e.pageX + 5);
        $("#tooltipDiv").css("top", e.pageY + 5);
        $("#tooltipDiv").show();
        return false;
    })
    .mouseout(function () {
        $("#tooltipDiv").hide();
        return false;
    });
}
function UnmarkDiv(divName, imageNameTag, type, resetFlag) {
    $(divName).each(function () {
        $(this).children('img[name=' + imageNameTag + ']').remove();
        $(this).attr("value", "0");
        if (type == "process" && resetFlag != true) {
            RemoveSelectedProcessDesc("#" + $(this).attr("id") + "Desc");
        }
        if (type == "item") {
            ResetItemDetails();
        }
    });
}
function MarkDiv(divName, imageNameTag, type) {
    $(divName).each(function () {
        if (type == "category") {
            //UnMark all other divs and Mark this div
            UnmarkDiv("#categoriesDetail div[value=1]", type + "Check", type);
        }
        else if (type == "item") {
            //Unmark all other divs
            UnmarkDiv("#catItems div[value=1]", type + "Check", type);
            LoadItemDetails($(this).attr("id"));
        }
        else if (type == "variation") {
            //Unmark all other divs
            UnmarkDiv("#variationsDetail div[value=1]", type + "Check", type);
        }
        $(this).append('<img class="itemIconCheckImage" src="images/checkmark_1.gif" name="' + imageNameTag + '"></img>');
        //var top = $(this).height();
        //$(this).children('img[name=' + imageNameTag + ']').css("top", "-" + top.toString() + "px");
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
                        "allCategory", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
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
                var catids = GetXmlData(null, "item[id=" + tempIds[i] + "]", "attribute", "catids", null, itemXml);
                var varids = GetXmlData(null, "item[id=" + tempIds[i] + "]", "attribute", "varids", null, itemXml);
                var subitemids = GetXmlData(null, "item[id=" + tempIds[i] + "]", "attribute", "subitemids", null, itemXml);
                var itemID = tempIds[i];
                //alert(itemXml.toString().toLowerCase());
                LoadItemsDiv(targetDiv, itemName, itemCode, itemImage, "item", i, null, itemID, "catids='" + catids + "' varids='" + varids + "' subitemids='" + subitemids + "'");
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}
function LoadItemsDiv(targetDiv, itemName, itemCode, itemImage, type, count, targetText, itemID, additional) {
    setTimeout(function () {
        AddItems(targetDiv, itemName, itemCode, itemImage, type, count, targetText, itemID, additional);
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

/////////////////////////////Brands//////////////////////////
function LoadBrands(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllBrands xmlns="http://tempuri.org/" />';
            var sFn = LoadBrands;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllBrands", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllBrandsResponse").find("GetAllBrandsResult").text();
            var tempIds = GetXmlData(null, "brands brand", "attribute", "id", "true", xmlData).split(" ");
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv,
                    GetXmlData(null, "brand[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    '', '', "brand", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}
/////////////////////////////~Brands//////////////////////////

/////////////////////////////Comments//////////////////////////
function LoadComments(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllComments xmlns="http://tempuri.org/" />';
            var sFn = LoadComments;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllComments", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllCommentsResponse").find("GetAllCommentsResult").text();
            var tempIds = GetXmlData(null, "comments comment", "attribute", "id", "true", xmlData).split(" ");
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv,
                    GetXmlData(null, "comment[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    '', '', "comment", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}
/////////////////////////////~Comments//////////////////////////
/////////////////////////////Variations//////////////////////////
function LoadVariations(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllVariations xmlns="http://tempuri.org/" />';
            var sFn = LoadVariations;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllVariations", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetAllVariationsResponse").find("GetAllVariationsResult").text();
            var tempIds = GetXmlData(null, "variations variation", "attribute", "id", "true", xmlData).split(" ");
            var child = null;
            for (var i = 0; i < tempIds.length; i++) {
                AddItems(targetDiv,
                    GetXmlData(null, "variation[id=" + tempIds[i] + "] Name", "nodeText", null, null, xmlData),
                    '', '', "allVariation", i, null, tempIds[i]);
            }
            MarkDivAsLoaded(targetDiv);
        }
    }
}
/////////////////////////////~Variations//////////////////////////

/////////////////////////////AutoComplete//////////////////////////
function AttachEventsForKeyboard() {
    $("input[rel=autoComplete]").focus(function () {
        //$(this).addClass("selectedText");
        //Prepare the list
        var sourceDiv = $(this).attr("sourceDiv");
        if (sourceDiv != null) {
            CreateList(sourceDiv, true, true, this);
        }
        $(this).bind("blur", function () {
            //$(this).removeClass("selectedText");
            HideList();
        });
        $(this).keyup(function (e) {
            //console.log(e.which);
            e.stopImmediatePropagation();
            switch (e.which) {
                case 9: case 38: case 40:
                    break;
                case 13:
                    //Select the selection from list
                    SelectListItem();
                    break;
                case 27:
                    HideList();
                    break;
                default:
                    FilterList($(this).val());
                    ShowList();
            }
            return false;
        });
        $(this).keydown(function (e) {
            //console.log(e.which);
            e.stopImmediatePropagation();
            switch (e.which) {
                case 40: //Down Arrow
                    //Move Next in list
                    MoveSelection("next");
                    break;
                case 38: //Up Arrow
                    MoveSelection("prev");
                    break;
                default:
                    //Do Nothing
            }
            //return false;
        });
    });

}
function SelectListItem() {
    switch ($("#list").attr("curInput")) {
        case "txtSearchCustomer":
            $("#txtSearchCustomer").val('');
            //Add selected values in customerData div
            //Name
            $("#customerName").children().children("span#spnCustomerName").text
                        ($("#list div.selectedListItem").children().children("span[rel=customerName]").text())
            //Phone
            $("#customerPhone").children().children("span#spnCustomerPhone").text
                        ($("#list div.selectedListItem").children().children("span[rel=customerPhone]").text())
            //Address
            $("#customerAddress").children().children("span#spnCustomerAddress").text
                        ($("#list div.selectedListItem").children().children("span[rel=customerAddress]").text())
            //Add Priority and Remarks
            //Priority
            $("#txtCustomerPriority").val
                        ($("#list div.selectedListItem").children().children("span[rel=customerPriority]").text())
            //Remarks
            $("#txtCustomerRemarks").val
                        ($("#list div.selectedListItem").children().children("span[rel=customerRemarks]").text())

            break;
        default:
            var inputid = "#" + $("#list").attr("curInput");
            $(inputid).val($("#list div.selectedListItem span[rel=name]").text());
    }
    HideList();
}
function MoveSelection(newpos) {
    var flag = false;
    if ($("#list").css("display") != "none") {
        //console.log("Scrolling -" + newpos);
        if (newpos == "next") {
            var lastSeq = $("#list div.listItem:visible:last").attr("seq");
            var seq = $("#list div.selectedListItem").attr("seq"); ;
            var curSeq = seq;
            while (flag == false) {
                seq++;
                if ($("#list div.listItem[seq=" + seq + "]").is(":visible")) {
                    $("#list div.listItem[seq=" + curSeq + "]").removeClass("selectedListItem");
                    $("#list div.listItem[seq=" + seq + "]").addClass("selectedListItem");
                    flag = true;
                }
                if (seq >= lastSeq) { flag = true; }
            }
        }
        else if (newpos == "prev") {
            var firstSeq = $("#list div.listItem:visible:first").attr("seq");
            var seq = $("#list div.selectedListItem").attr("seq"); ;
            var curSeq = seq;
            flag = false;
            while (flag == false) {
                seq--;
                if ($("#list div.listItem[seq=" + seq + "]").is(":visible")) {
                    $("#list div.listItem[seq=" + curSeq + "]").removeClass("selectedListItem");
                    $("#list div.listItem[seq=" + seq + "]").addClass("selectedListItem");
                    flag = true;
                }
                if (seq <= firstSeq) { flag = true; }
            }
        }
        AdjustScroll();
    }
}
function AdjustScroll() {
    //Get height of currently selected listItem
    var itemHeight = $("#list div.selectedListItem").outerHeight(true);
    var itemTop = $("#list div.selectedListItem").position().top;
    var listHeight = $("#list").height();
    var scrollPos = $("#list").scrollTop();
    //console.log("ih=" + itemHeight + ";it=" + itemTop + ";listH=" + listHeight + ";sp=" + scrollPos);
    //console.log((itemHeight + itemTop) + "vs" + (listHeight + scrollPos));
    if ((itemHeight + itemTop) > listHeight) {
        $("#list").scrollTop(scrollPos + itemHeight);
    }
    else if (itemTop < itemHeight) {
        $("#list").scrollTop(scrollPos - itemHeight);
    }
}

function ShowList() {
    //Get count of all visible elements
    var count = $("#list div.listItem").filter(function () {
        return ($(this).css("display") != "none");
    }).length;
    //console.log("count =" + count);
    if (count > 0) {
        $("#list").show();
        $("#list div.selectedListItem").removeClass("selectedListItem");
        //Get the first visible listItem and select it
        $("#list div.listItem:visible:first").addClass("selectedListItem");
    }
}

function HideList() {
    $("#list").hide();
}

function FilterList(fstr) {
    $("#list div.listItem").show();
    var curInput = $("#list").attr("curInput");
    var temp = $(this).children("span[rel=name]").text();
    var flag1 = false;
    var flag2 = false;
    var flag3 = false;
    if (curInput == "txtSearchCustomer") {
        $("#list div.listItem").filter(function () {
            temp = $(this).children().children("span[rel=customerName]").text();
            flag1 = false;
            flag2 = false;
            flag3 = false;
            if (temp.toUpperCase().indexOf(fstr.toUpperCase()) > -1) {
                $(this).children().children("span.customerNameText").html(AddHighlighter(temp, fstr));
                flag1 = true;
            }
            temp = $(this).children().children("span[rel=customerPhone]").text();
            if (temp.toUpperCase().indexOf(fstr.toUpperCase()) > -1) {
                $(this).children().children("span.customerPhoneText").html(AddHighlighter(temp, fstr));
                flag2 = true;
            }
            temp = $(this).children().children("span[rel=customerAddress]").text();
            if (temp.toUpperCase().indexOf(fstr.toUpperCase()) > -1) {
                $(this).children().children("span.customerAddressText").html(AddHighlighter(temp, fstr));
                flag3 = true;
            } return !(flag1 || flag2 || flag3);
        }).hide();
    }
    else {
        $("#list div.listItem").filter(function () {
            temp = $(this).children("span[rel=name]").text();
            flag1 = false;
            flag2 = false;
            if (temp.toUpperCase().indexOf(fstr.toUpperCase()) > -1) {
                $(this).children("span.nameText").html(AddHighlighter(temp, fstr));
                flag1 = true;
            }
            temp = $(this).children("span[rel=code]").text();
            if (temp.toUpperCase().indexOf(fstr.toUpperCase()) > -1) {
                $(this).children("span.codeText").html(AddHighlighter(temp, fstr));
                flag2 = true;
            }
            return !(flag1 || flag2);
        }).hide();
    }
    //console.log("filtering items-" + fstr);
}

RegExp.escape = function (str) {
    var specials = /[.*+?|()\[\]{}\\$^]/g; // .*+?|()[]{}\$^
    return str.replace(specials, "\\$&");
}

function AddHighlighter(mainStr, searchStr) {
    var regex = new RegExp("(" + RegExp.escape(searchStr) + ")", "i");
    return mainStr.replace(regex, "<span class='highlighter'>$1</span>");

    //return mainStr.replace(searchStr, "<span class='highlighter'>" + searchStr + "</span>");
}

function CreateList(sourceDiv, hasCode, hasIcon, inputTextControl) {
    //console.log("creating list");
    var count = 0;
    var list = "#list";
    var curInput = $(inputTextControl).attr("id");
    //Add the current input text tag in list div
    $(list).attr("curInput", curInput);
    $(list).children().remove();
    if (curInput == "txtSearchCustomer") {
        var tempids = GetXmlData(sourceDiv, "customers customer", "attribute", "id", "true", null).split(" ");
        for (var i = 0; i < tempids.length; i++) {
            $(list).append("<div id= 'listItem" + count + "' seq='" + count + "' class='listItem' id='" + tempids[i] + "'></div>");
            var name = GetXmlData(sourceDiv, "customer[id=" + tempids[i] + "] name", "nodeText", null, null, null);
            var phone =  GetXmlData(sourceDiv, "customer[id=" + tempids[i] + "] phone", "nodeText", null, null, null);
            var address =  GetXmlData(sourceDiv, "customer[id=" + tempids[i] + "] address", "nodeText", null, null, null);
            var priority =  GetXmlData(sourceDiv, "customer[id=" + tempids[i] + "] priority", "nodeText", null, null, null);
            var remarks =  GetXmlData(sourceDiv, "customer[id=" + tempids[i] + "] remarks", "nodeText", null, null, null);

            //Add Customer Name and Phone
            $("#listItem" + count).append("<div class='line11'>"
                                                + "<span class='customerNameText'>" + name + "</span>"
                                                + "<span rel='customerName' class='hidden'>" + name + "</span>"
                                            + "</div>"
                                            + "<div class='line12'>"
                                                + "<span class='customerPhoneText'>" + phone + "</span>"
                                                + "<span rel='customerPhone' class='hidden'>" + phone + "</span>"
                                            + "</div>"
                                            + "<div class='line2'>"
                                                + "<span class='customerAddressText'>" + address + "</span>"
                                                + "<span rel='customerAddress' class='hidden'>" + address + "</span>"
                                            + "</div>"
                                            + "<div class='line3'>"
                                                + "<span rel='customerPriority' class='hidden'>" + priority + "</span>"
                                                + "<span rel='customerRemarks' class='hidden'>" + remarks + "</span>"
                                            + "</div>");
            count++;
        }
    }
    else {
        $(sourceDiv + " div.itemIconContainer").each(function () {
            $(list).append("<div id='listItem" + count + "' seq='" + count + "' class='listItem' id='" + $(this).attr("id") + "'></div>");
            //Add Code
            if (hasCode == true) {
                var code = $(this).find("span.itemIconCode").text();
                if (code != "") {
                    $("#listItem" + count).append("<span class='codeText'>" + code + "</span>" +
                                      "<span rel='code'>" + code + "</span><span> - </span>");
                }
            }
            //Add Name
            var name = $(this).find("div.itemIconName").text();
            $("#listItem" + count).append("<span class='nameText'>" + name + "</span>" +
                                      "<span rel='name'>" + name + "</span>");
            count++;
        });
    }
    var left = $(inputTextControl).position().left + 10; // + ($(inputTextControl).width() / 2) - ($(list).width() / 2);
    var top = $(inputTextControl).outerHeight(true) + $(inputTextControl).position().top;
    $(list).css("left", left);
    $(list).css("top", top);
    AttachEventsForList();
}

function AttachEventsForList() {
    $("#list").mouseenter(function () {
        var inputid = "#" + $(this).attr("curInput");
        $(inputid).unbind("blur");
    });
    $("#list").mouseleave(function () {
        var inputid = "#" + $(this).attr("curInput");
        $(inputid).bind("blur", function () {
            HideList();
        });
    });
    $("#list div.listItem").click(function () {
        //Continue to show the list 
        ShowList();
        //Select the clicked item
        $("#list div.selectedListItem").removeClass("selectedListItem");
        $(this).addClass("selectedListItem");
        SelectListItem();
        var inputid = "#" + $("#list").attr("curInput");
        $(inputid).focus();
    });
}
/////////////////////////////~AutoComplete//////////////////////////

