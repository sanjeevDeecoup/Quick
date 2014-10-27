///////////////////////////////////Common Functions/////////////////////////////////
var promoServiceUrl = "http://localhost/appServices/PromoSchemeService.svc";

function SetXmlData(xml, selectorString, txtValue) {
    var xmlstring = unescape(xml);
    xmlDoc = $.parseXML(xmlstring);
    $xmlstring = $(xmlDoc);

    //replace existing contents with the new text
    $xmlstring.find(selectorString).text(txtValue);
    var retVal = xmlDoc.xml;
    if (retVal == null) {
        $('#test').append($(xmlDoc).contents());
        retVal = $('#test').html();
        $('#test').html('');
    }
    return retVal;
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

function ShowLoadingDiv(index, targetDiv) {
    var target = "div#step" + index + " div[" + targetDiv + "]";
    //Remove All Children
    $(target).children().remove();
    $(target).append('<div name="loading"></div>');
    $(target + " div[name=loading]").append('<img src="images/loading_3.gif"></img>');
    $(target + " div[name=loading]").append('<p>Please wait</p>');
}

function AjaxError(xhr, status, error, index, targetDiv) {
    if (index != null) {
        var target = "div#step" + index + " div[" + targetDiv + "]";
        //Remove All Children
        $(target).children().remove();
        $(target).append('<div name=error></div>');
        $(target + " div[name=error]").append(
                '<p>Unable to load contents</p>' +
                '<p>Error Details :' + error + '</p>');
    }
    else {
        alert("Unable to save record.\nError Details:" + error);
    }
}

function AddItems(targetDiv, itemName, itemCode, imageName, type, count, targetText, dbid) {
    $(targetDiv).append('<div dbid=' + dbid + ' id="' + type + count + '" name="' + type + 'Items" value="0"></div>');
    $('#' + type + count).append('<img name="' + type + 'Img" src="' + imageName + '"></img>');
    $('#' + type + count).append('<span>' + itemName + '</span>');
    $('#' + type + count).append('<span name="code">' + itemCode + '</span>');
    //Attach click event on the new DIV created
    //if (type == 'process') {
    $('#' + type + count).click(function () {
        if (!IsDivMarked(this)) {
            MarkDiv(this, type + "Check");
        }
        else {
            UnmarkDiv(this, type + "Check");
        }
        if (targetText != null) {
            CreateTargetText(targetDiv, targetText, "div[name=" + type + "Items]");
        }
    });
}
function UnmarkDiv(divName, imageNameTag) {
    $(divName).each(function () {
        $(this).children('img[name=' + imageNameTag + ']').remove();
        $(this).attr("value", "0");
    });
}
function MarkDiv(divName, imageNameTag) {
    $(divName).each(function () {
        var top = 0;
        $(this).children().each(function () {
            top += $(this).height();
        });
        $(this).append('<img src="images/checkmark_1.gif" name="' + imageNameTag + '"></img>');
        $(this).attr("value", "1");
        $(this).children('img[name=' + imageNameTag + ']').css("top", "-" + top.toString() + "px");
    });
}
function IsDivMarked(divName) {
    return $(divName).attr("value") == "1";
}
///////////////////////////////////Common Functions/////////////////////////////////

///////////////////////////////////f1/////////////////////////////////
function Loadf1(data, status, xhr, initFlag, index, targetDiv) {
    //Check if this div has already been loaded or not
    var divName = "div#step" + index + " div[" + targetDiv + "]";
    if ($(divName).attr("loaded") != "0") { return; }

    if (initFlag == "0") { //Call Ajax Method
        ShowLoadingDiv(index, targetDiv);
        setTimeout(function () { GetAllTemplatesData(index, targetDiv) }, 500);
    }
    else { //Load Result
        var allSchemeXMLs = $(data).find("GetAllTemplatesDataResponse").find("GetAllTemplatesDataResult").text();
        $("div#step" + index + " div.formContent").append('<div name="data">' + escape(allSchemeXMLs) + '</div>');
        var dataDiv = "div#step" + index + " div.formContent" + " div[name=data]";
        //var dataDiv = divName + " div[name=data]";
        var tempids = GetXmlData(dataDiv, "schemeTemplates schemeTemplate", "attribute", "id").split(" ");
        var promoDiv = "";
        for (i = 0; i < tempids.length; i++) {
            promoDiv +=
            '<li><div dbid=' + tempids[i] + ' class="smallButton" next="' +
                GetXmlData(dataDiv, "schemeTemplate[id=" + tempids[i] + "] steps", "nodeName", null, null)
                + '" value="0">' +
                GetXmlData(dataDiv, "schemeTemplate[id=" + tempids[i] + "] schemeName", "nodeText", null, null)
                + '</div><p>' +
                GetXmlData(dataDiv, "schemeTemplate[id=" + tempids[i] + "] examples example", "nodeText", null, "true")
                + '</p><div name="data">' +
                escape(GetXmlData(dataDiv, "schemeTemplate[id=" + tempids[i] + "] steps", "xml", null, null))
                + '</div></li>';
        }
        promoDiv = '<ul>' + promoDiv + '</ul>';
        //Clear all children
        $(divName).children().remove();
        $(divName).append(promoDiv);
        AttachMouseEvents(divName + " div.smallButton");
        $(divName).attr("loaded", "1");
    }
}

function GetAllTemplatesData(index, targetDiv) {
    var request = '<s:Envelope xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:s="http://www.w3.org/2003/05/soap-envelope">' +
                  '<s:Header>' +
                    '<a:Action s:mustUnderstand="1">http://tempuri.org/IPromoSchemeService/GetAllTemplatesData</a:Action>' +
                    '<a:MessageID>urn:uuid:a75f37e2-50ea-47eb-b6e1-eb776acdbbfa</a:MessageID>' +
                    '<a:ReplyTo>' +
                      '<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>' +
                    '</a:ReplyTo>' +
                    '<a:To s:mustUnderstand="1">' + promoServiceUrl + '</a:To>' +
                  '</s:Header>' +
                  '<s:Body>' +
                    '<GetAllTemplatesData xmlns="http://tempuri.org/" />' +
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
            Loadf1(data, status, xhr, "1", index, targetDiv);
        },
        error: function (xhr, status, error) {
            AjaxError(xhr, status, error, index, targetDiv);
        }
    });
}
///////////////////////////////////f1/////////////////////////////////

///////////////////////////////////f2/////////////////////////////////
function Loadf2(divName, txtName, index) {
    if ($(divName).attr("loaded") == "0") {
        LoadNumPad(divName, txtName);
        $(divName).attr("loaded", "1");
        LoadSavedData(index);
    }
}
function LoadNumPad(divName, txtName) {
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
    //Attach click event on all numpadbuttons
    $(divName + ' div[name="numPadKey"]').click(function (event) {
        if ($(this).attr('keyVal') == 'C') {
            $(txtName).val('');
        }
        else if ($(txtName).val().length < $(txtName).attr('maxlength')) {
            $(txtName).val($(txtName).val() + $(this).attr('keyVal'));
        }
    });
}
///////////////////////////////////f2/////////////////////////////////

///////////////////////////////////f3/////////////////////////////////

function Loadf3(data, status, xhr, initFlag, index, targetDiv) {
    //Check if this div has already been loaded or not
    var divName = "div#step" + index + " div[" + targetDiv + "]";
    if ($(divName).attr("loaded") != "0") { return; }

    if (initFlag == "0") { //Call Ajax Method
        ShowLoadingDiv(index, targetDiv);
        setTimeout(function () { GetAllProcesses(index, targetDiv) }, 500);
    }
    else { //Load Result
        var allProcesses = $(data).find("GetAllProcessesResponse").find("GetAllProcessesResult").text();
        $("div#step" + index + " div.formContent").append('<div name="data">' + escape(allProcesses) + '</div>');
        var dataDiv = "div#step" + index + " div.formContent" + " div[name=data]";
        //Add all proceses
        var count = 0;
        var count1 = "";
        var seq = $("div#step" + index + " fieldset").attr("sequence");
        //Get All Process
        var allP = GetXmlData(dataDiv, "processes process", "attribute", "id").split(" ");
        $(divName).children().remove();
        for (i = 0; i < allP.length; i++) {
            count++;
            count1 = count.toString() + seq.toString();
            AddItems(divName,
            GetXmlData(dataDiv, "process[id=" + allP[i] + "] Name", "nodeText", null),
            GetXmlData(dataDiv, "process[id=" + allP[i] + "] Code", "nodeText", null),
            GetXmlData(dataDiv, "process[id=" + allP[i] + "] Image", "nodeText", null),
            "process", count1, "div#step" + index + " input[name=txtSelectedProcess]", allP[i]);
        }
        $(divName).attr("loaded", "1");
        //AttacheMouseEvents for Select All CheckBox
        $("div#step" + index + " input[name=chkSelectAllProcess]").click(function () {
            if ($(this).is(":checked")) {
                //Unmark first
                UnmarkDiv(divName + " div[name=processItems]", "processCheck");
                MarkDiv(divName + " div[name=processItems]", "processCheck");
            }
            else {
                UnmarkDiv(divName + " div[name=processItems]", "processCheck");
            }
            CreateTargetText(divName, "div#step" + index + " input[name=txtSelectedProcess]", "div[name=processItems]");
        });
        LoadSavedData(index);
    }
}

function GetAllProcesses(index, targetDiv) {
    var request = '<s:Envelope xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:s="http://www.w3.org/2003/05/soap-envelope">' +
                    '<s:Header>' +
                    '<a:Action s:mustUnderstand="1">http://tempuri.org/IPromoSchemeService/GetAllProcesses</a:Action>' +
                    '<a:MessageID>urn:uuid:9c1f2a50-84f7-4cf8-8722-c80b1caf4234</a:MessageID>' +
                    '<a:ReplyTo>' +
                        '<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>' +
                    '</a:ReplyTo>' +
                    '<a:To s:mustUnderstand="1">' + promoServiceUrl + '</a:To>' +
                    '</s:Header>' +
                    '<s:Body>' +
                    '<GetAllProcesses xmlns="http://tempuri.org/" />' +
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
            //alert($(data).find("GetAllProcessesResponse").find("GetAllProcessesResult").text());
            Loadf3(data, status, xhr, "1", index, targetDiv)
        },
        error: function (xhr, status, error) {
            AjaxError(xhr, status, error, index, targetDiv);
        }
    });
}

function CreateTargetText(container, targetText, itemsDiv) {
    var allItems = container + " " + itemsDiv;
    var text = $(allItems).filter(function () {
        return $(this).attr("value") == "1";
    })
    .map(function () {
        return $(this).find("span[name=code]").map(function () {
            return $(this).html();
        }).get().join("-");
    }).get().join(";");
    $(targetText).val(text);
}
///////////////////////////////////f3/////////////////////////////////

///////////////////////////////////f4/////////////////////////////////
function Loadf4(data, status, xhr, initFlag, index, targetDiv) {
    //Check if this div has already been loaded or not
    var divName = "div#step" + index + " div[" + targetDiv + "]";
    if ($(divName).attr("loaded") != "0") { return; }

    if (initFlag == "0") { //Call Ajax Method
        ShowLoadingDiv(index, targetDiv);
        setTimeout(function () { GetAllItems(index, targetDiv) }, 500);
    }
    else { //Load Result
        var allItems = $(data).find("GetAllItemsResponse").find("GetAllItemsResult").text();
        $("div#step" + index + " div.formContent").append('<div name="data">' + escape(allItems) + '</div>');
        var dataDiv = "div#step" + index + " div.formContent" + " div[name=data]";
        //Add all proceses
        var count = 0;
        //Get All Process
        var allI = GetXmlData(dataDiv, "items item", "attribute", "id").split(" ");
        var count1 = "";
        var seq = $("div#step" + index + " fieldset").attr("sequence");
        $(divName).children().remove();
        for (i = 0; i < allI.length; i++) {
            count++;
            count1 = count.toString() + seq.toString();
            AddItems(divName,
            GetXmlData(dataDiv, "item[id=" + allI[i] + "] Name", "nodeText", null),
            GetXmlData(dataDiv, "item[id=" + allI[i] + "] Code", "nodeText", null),
            GetXmlData(dataDiv, "item[id=" + allI[i] + "] Image", "nodeText", null),
            "item", count1, null, allI[i]);
        }
        $(divName).attr("loaded", "1");
        //AttacheMouseEvents - txtAllItems keypress
        AttacheMouseEvent_FilterText(index, "input[name=txtAllItems]", "div[rel=allItemsMain] div[name=itemItems] span[name!=code]"); // -
        //Attach Button Events - index, btnName, fromDiv, toDiv, all
        //Select
        AttacheMouseEvent_SelectButtons(index, "input[name=btnSelect]", "div[rel=allItemsMain]", "div[rel=selectedItemsMain]", null);
        //Select All
        AttacheMouseEvent_SelectButtons(index, "input[name=btnSelectAll]", "div[rel=allItemsMain]", "div[rel=selectedItemsMain]", "true");
        //Unselect
        AttacheMouseEvent_SelectButtons(index, "input[name=btnUnselect]", "div[rel=selectedItemsMain]", "div[rel=allItemsMain]", null);
        //Unselect All
        AttacheMouseEvent_SelectButtons(index, "input[name=btnUnselectAll]", "div[rel=selectedItemsMain]", "div[rel=allItemsMain]", "true");
        LoadSavedData(index);
    }
}

function AttacheMouseEvent_FilterText(index, txtBox, itemSelector) {
    var txt = "div#step" + index + " " + txtBox;
    var sel = "div#step" + index + " " + itemSelector;
    $(txt).keydown(function (event) {
        if (event.keyCode == 13) {
            if ($(txt).val() == "") {
                $(sel).each(function () {
                    $(this).parent().show();
                });
            }
            else {
                $(sel).each(function () {
                    $(this).filter(function () {
                        return !($(this).html().toString().toUpperCase().match($(txt).val().toString().toUpperCase()))
                    }).parent().hide();
                });
            }
            return false;
        }
    });
}

function AttacheMouseEvent_SelectButtons(index, btnName, fromDiv, toDiv, allButton) {
    var btn = "div#step" + index + " " + btnName;
    var sourceItems = "div#step" + index + " " + fromDiv;
    var destItems = "div#step" + index + " " + toDiv;
    var searchCondition = "div[value = 1]";
    if (allButton == "true") { searchCondition = "*"; }
    $(btn).click(function () {
        //Load All selected Divs in the destination
        $(destItems).append($(sourceItems).children(searchCondition));
        $(sourceItems).children(searchCondition).remove();
        $(destItems).children().each(function () {
            UnmarkDiv(this, "itemCheck");
        });
    });
}

function GetAllItems(index, targetDiv) {
    var request = '<s:Envelope xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:s="http://www.w3.org/2003/05/soap-envelope">' +
                  '<s:Header>' +
                    '<a:Action s:mustUnderstand="1">http://tempuri.org/IPromoSchemeService/GetAllItems</a:Action>' +
                    '<a:MessageID>urn:uuid:903ab3b0-135b-406f-8a88-136f0bedaf3a</a:MessageID>' +
                    '<a:ReplyTo>' +
                      '<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>' +
                    '</a:ReplyTo>' +
                    '<a:To s:mustUnderstand="1">' + promoServiceUrl + '</a:To>' +
                  '</s:Header>' +
                  '<s:Body>' +
                    '<GetAllItems xmlns="http://tempuri.org/" />' +
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
            //alert($(data).find("GetAllItemsResponse").find("GetAllItemsResult").text());
            Loadf4(data, status, xhr, "1", index, targetDiv)
        },
        error: function (xhr, status, error) {
            AjaxError(xhr, status, error, index, targetDiv);
        }
    });
}
///////////////////////////////////f4/////////////////////////////////

///////////////////////////////////f5/////////////////////////////////
function Loadf5(stepsxml, index) {
    var divName = "div#step" + index + " div[rel=schemeCaption]";
    if ($(divName).attr("loaded") == "0") {
        LoadSavedData(index);
        $(divName).attr("loaded", "1")
    }

    //Loop through all summarydata nodes and concatenate text
    var xmlstring = unescape(stepsxml);
    xmlDoc = $.parseXML(xmlstring);
    $xmlstring = $(xmlDoc);
    var promoDesc = $xmlstring.find("summarydata").map(function () {
        var staticDataSel = $(this).attr("staticDataSelector");
        var sdata; var ddata;
        var sdatakey = "staticdata";

        if (staticDataSel != "" && staticDataSel != null) {
            var staticValAttr = $xmlstring.find(staticDataSel).text();
            sdatakey = "staticdata[value=" + staticValAttr + "]";
            sdata = $(this).find(sdatakey).text();
        }
        else {
            sdata = $(this).find("staticdata").text();
        }
        ddata = $(this).find("dynamicdata").text();
        if (ddata == "") { sdata = ""; }
        if ($(this).find(sdatakey).attr("beforeValue") == "true") {
            return sdata + ddata;
        }
        else {
            return ddata + sdata;
        }
    }).get().join(" ");
    promoDesc = promoDesc.replace("  ", " ");
    promoDesc = promoDesc.replace("  .", ".");
    $("div#step" + index + " textarea[name=txtSchemeDesc]").val(promoDesc);
}
///////////////////////////////////f5/////////////////////////////////

///////////////////////////////////f6/////////////////////////////////
function Loadf6(index) {
    var divName = "div#step" + index + " div[rel=schemeValidity]";
    if ($(divName).attr("loaded") == "0") {
        LoadSavedData(index);
        $(divName).attr("loaded", "1")
    }
}
///////////////////////////////////f6/////////////////////////////////

///////////////////////////////////f7/////////////////////////////////
function Loadf7(data, status, xhr, initFlag, index, targetDiv) {
    //Check if this div has already been loaded or not
    var divName = "div#step" + index + " div[" + targetDiv + "]";
    if ($(divName).attr("loaded") != "0") { return; }

    if (initFlag == "0") { //Call Ajax Method
        ShowLoadingDiv(index, targetDiv);
        setTimeout(function () { GetAllPromos(index, targetDiv) }, 500);
    }
    else { //Load Result
        var allPromos = $(data).find("GetAllPromosResponse").find("GetAllPromosResult").text();
        $("div#step" + index + " div.formContent").append('<div name="data">' + escape(allPromos) + '</div>');
        var dataDiv = "div#step" + index + " div.formContent" + " div[name=data]";
        //Add all promos
        var count = 0;
        //                    <table cellpadding="0" cellspacing="0">
        //                        <tr><td name="selCol">Select</td><td name="nameCol">Name</td><td name ="descCol">Description</td></tr>
        //                        <tr><td name="selCol"><div name="selDiv" value="0" next="f2">1</div></td><td name="nameCol">Monthly#1</td><td name ="descCol">Get 10 Drycleanings free on monthly pay of Rs. 9000. This is valid for 1 month from enrollment</td></tr>
        //                        <tr><td name="selCol"><div name="selDiv" value="0" next="f2"></div></td><td name="nameCol">Monthly#1</td><td name ="descCol">Get 30 Dye free on monthly pay of Rs. 5000. This is valid for 1 month from enrollment</td></tr>
        //                        <tr><td name="selCol"><div name="selDiv" value="0" next="f2">3</div></td><td name="nameCol">Advacne based Scheme</td><td name ="descCol">Pay Rs. 10000 and get Rs. 15000 of DryCleaning free. This is valid between 02-Feb-2012 and 01-Feb-2012. . This is valid between 02-Feb-2012 and 01-Feb-2012.</td></tr>
        //                    </table>
        var promoDiv = '<tr><td name="selCol">Select</td><td name="nameCol">Name</td><td name ="descCol">Description</td></tr>';
        var allPromos = GetXmlData(dataDiv, "schemeTemplates schemeTemplate", "attribute", "promoid").split(" ");
        var i;
        for (i = 0; i < allPromos.length; i++) {
            promoDiv += '<tr><td name="selCol"><div dbid="' + allPromos[i] + '" class="selColDiv" name="selDiv" value="0" next="' +
                        GetXmlData(dataDiv, "schemeTemplate[promoid=" + allPromos[i] + "] steps", "nodeName", null, "true", null)
                        + '">' + allPromos[i] + '</div><div name="data">' + escape(GetXmlData(dataDiv, "schemeTemplate[promoid=" + allPromos[i] + "] steps", "xml", null, null, null)) +
                        '</div></td><td name="nameCol">' +
                        GetXmlData(dataDiv, "schemeTemplate[promoid=" + allPromos[i] + "] steps f5 filldata uidata[seqid=1] uivalue", "nodeText", null, null, null)
                        + '</td><td name ="descCol">' +
                        GetXmlData(dataDiv, "schemeTemplate[promoid=" + allPromos[i] + "] steps f5 filldata uidata[seqid=2] uivalue", "nodeText", null, null, null) +
                        '</td></tr>';
        }
        promoDiv = '<table cellpadding="0" cellspacing="0">' + promoDiv + '</table>';
        //Clear all children
        $(divName).children().remove();
        $(divName).append(promoDiv);
        $(divName).attr("loaded", "1");

        //Attach Mouse Events and initialize newly added controls
        $(divName + " table tr td[name=selCol]").each(function () {
            $(this).children("div[name=selDiv]").height($(this).height());
        });
        $(divName + " table tr").click(function () {
            var addClass = true;
            if ($(this).hasClass("selectedRow")) { addClass = false; }
            //Unmark all rows
            $(this).parent().children("tr").removeClass("selectedRow");
            $(this).parent().children("tr").children("td").children("div[name=selDiv]").attr("value", "0");
            //Select current row
            if (addClass == true) {
                $(this).addClass("selectedRow");
            }
            var val = $(this).children("td").children("div[name=selDiv]").attr("value");
            if (val == "1") { val = "0"; }
            else { val = "1"; }
            $(this).children("td").children("div[name=selDiv]").attr("value", val);
        });
    }
}
function GetAllPromos(index, targetDiv) {
    var request = '<s:Envelope xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:s="http://www.w3.org/2003/05/soap-envelope">' +
                        '<s:Header>' +
                        '<a:Action s:mustUnderstand="1">http://tempuri.org/IPromoSchemeService/GetAllPromos</a:Action>' +
                        '<a:MessageID>urn:uuid:90578d80-3d2e-4b7f-8a58-3c15a4b3494f</a:MessageID>' +
                        '<a:ReplyTo>' +
                            '<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>' +
                        '</a:ReplyTo>' +
                        '<a:To s:mustUnderstand="1">' + promoServiceUrl + '</a:To>' +
                        '</s:Header>' +
                        '<s:Body>' +
                        '<GetAllPromos xmlns="http://tempuri.org/" />' +
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
            //alert($(data).find("GetAllItemsResponse").find("GetAllItemsResult").text());
            Loadf7(data, status, xhr, "1", index, targetDiv)
        },
        error: function (xhr, status, error) {
            AjaxError(xhr, status, error, index, targetDiv);
        }
    });
}
///////////////////////////////////f7/////////////////////////////////

function GetSchemeXml() {
    //return $("div#step1 div.formContent div + div[name=data]").html();
    return $("div#step1 div.formContent").children(":last").html();
}
function GetIDAttributeName() {
    var retVal = null;
    retVal = "promoid";
    if ($("div#step1 fieldset").attr("name") == "f1") {
        retVal = "id";
    }
    return retVal;
}

function SubmitData() {
    //Get Steps XML and merge it with the template xml
    var stepsxml = unescape(GetStepsXml());
    var schemexml = unescape(GetSchemeXml());
    var fName = "div#step1 fieldset";
    var selDiv = GetSelectorDivForStepsXml(fName);

    //var dbid = $("div#step1 fieldset[name=f1] div.formContent div.smallButton[value=1]").attr("dbid");
    var dbid = $(fName + " " + selDiv).attr("dbid");
    var id = GetIDAttributeName();
    var schemeTemplateXml = GetXmlData(null, "schemeTemplates schemeTemplate[" + id + "=" + dbid + "]", "xml", null, null, schemexml);

    //get position of <steps> in templatexml
    var startIndex = schemeTemplateXml.indexOf("<steps>", 0);
    var endIndex = schemeTemplateXml.indexOf("</steps>", startIndex);

    var stepsText = schemeTemplateXml.substring(startIndex, endIndex + 8);

    schemeTemplateXml = schemeTemplateXml.replace(stepsText, stepsxml);

    //Make Ajax call fro saving data
    setTimeout(function () { SavePromoData(schemeTemplateXml) }, 500);
}

function SavePromoData(promoxml) {
    var request = '<s:Envelope xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:s="http://www.w3.org/2003/05/soap-envelope">' +
                  '<s:Header>' +
                    '<a:Action s:mustUnderstand="1">http://tempuri.org/IPromoSchemeService/SavePromoData</a:Action>' +
                    '<a:MessageID>urn:uuid:ed5a5d97-ca03-4837-9094-213c073484fa</a:MessageID>' +
                    '<a:ReplyTo>' +
                      '<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>' +
                    '</a:ReplyTo>' +
                    '<a:To s:mustUnderstand="1">' + promoServiceUrl + '</a:To>' +
                  '</s:Header>' +
                  '<s:Body>' +
                    '<SavePromoData xmlns="http://tempuri.org/">' +
                      '<promoXml>' + escape(promoxml) + '</promoXml>' +
                    '</SavePromoData>' +
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
            alert("Promotional scheme saved successfully");
            //Reset all steps
            ResetWizard();
        },
        error: function (xhr, status, error) {
            AjaxError(xhr, status, error);
        }
    });
}

function ResetWizard() {
    $("#mainContainer").children().remove();
    $("#commands").children().remove();
    stepIndex = 0;
    InitializeSteps("f0");
}