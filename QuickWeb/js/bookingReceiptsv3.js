/// <reference path="jquery-1.6.2.min.js" />
/// <reference path="common.js" />

$(document).ready(function () {
    ShowProcessingDialog("Initializing Booking Screen. Please wait...");
    AttachEventsForInputDevices("#leftContainer");
    AttachEventsForKeyboard();
    //Switch to default input mode
    SelectDefaultInputMode();
    AttachEventsForInputMultipleOptions();
    //Load Customers
    LoadCustomers(null, null, null, "0", "#allCustomers");
    //Load Categories
    LoadCategories(null, null, null, "0", "#allCategories");
    //Load Processes
    LoadProcesses(null, null, null, "0", "#processes");
    //SetProcessPanelHeaderText(1, 1);
    //Load Colors & Patterns
    LoadPatterns(null, null, null, "0", "#patterns");
    LoadColors(null, null, null, "0", "#colors");
    //Brands
    LoadBrands(null, null, null, "0", "#brands");
    LoadComments(null, null, null, "0", "#comments");
    LoadVariations(null, null, null, "0", "#allVariations");
    //LoadPriceList
    LoadPriceList(null, null, null, "0", "#priceList");
    //Attach Numpad
    LoadNumPad("#numPad");
    AttachEventsForNumPad("#numPad");
    //Load Recipt Header XML
    LoadReceiptHeaderTemplate(null, null, null, "0", "#receiptHeaderTemplate");
    //Load Line Item XML
    LoadLineItemTemplate(null, null, null, "0", "#lineItemTemplate");
    //Add New functionality
    AttachEventsForNew();
    //Add Update functionality
    AttachEventsForUpdate();
    //Add Delete functionality
    AttachEventsForDelete();
    //Attach Discount Events
    AttachEventsForDiscount();
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
    //Attach Events for Flyouts
    AttachEventsForFlyout();
    //Submit
    AttachEventsForSubmit();
    //Load Defaults
    LoadDefaults(null, null, null, "0", "#defaults");
    //Load Items
    LoadItems(null, null, null, "0", "#catItems");
    //Apply Defaults after all items have loaded -- Refer to method ItemLoadComplete
});

function ItemLoadComplete() {
    if (IsLoadPreviousBooking() == true) {
        var bookingNumber = GetBookingNumber();
        LoadBooking(null, null, null, "0", "#editBookingXml", bookingNumber);
    }
    else {
        ApplyDefaults();
    }
    AttacheErrorEvents();
}

function IsLoadPreviousBooking() {
    if (GetBookingNumber() != null) {
        return true;
    }
    else {
        return false;
    }
}

function GetBookingNumber() {
    var retVal = null;
    var url = window.location.href;
    var start = url.indexOf("bookN=", 0);
    if (start > -1) {
        retVal = url.substring(start + 6);
    }
    return retVal;
}

//////////////////////Items//////////////////////////////////////

function LoadItemDetails(itemid) {
    ResetItemDetails();
    var tempSel = "#catItems div[id=" + itemid + "]";
    //Get Categories
    AddItemDetails(tempSel, "catids", "#allCategories", "#categoriesDetail", "category", "allCategory");
    //Get Variations
    AddItemDetails(tempSel, "varids", "#allVariations", "#variationsDetail", "variation", "allVariation");
    //Get Sub Items
    AddItemDetails(tempSel, "subitemids", "#catItems", "#subItemsDetail", "subItem", "item");
}

function AddItemDetails(itemSel, attrName, masterDataDiv, targetDiv, type, masterType) {
    var count = 0;
    if ($(itemSel).attr(attrName) != "") {
        $(targetDiv).children().remove();
        var tempids = $(itemSel).attr(attrName).split(sep2);
        for (var i = 0; i < tempids.length; i++) {
            if (tempids[i] != "") {
                //Split using sep1
                var ids = tempids[i].split(sep1);
                if (ids[0] != "") {
                    var sourceDiv = masterDataDiv + " div[dbid=" + ids[0] + "]";
                    var newTargetDiv = targetDiv;
                    if (type != "category") {
                        if (count % 2 == 0) {
                            //$(targetDiv).append("<ul id='ul" + type+ count + "'></ul>");
                            $(targetDiv).append("<ul></ul>");
                        }
                        $(targetDiv + " ul:last-child").append("<li></li>");
                        newTargetDiv = targetDiv + " ul:last-child li:last-child";
                    }
                    AddItems(newTargetDiv, $(sourceDiv).children("div[name=" + masterType + "Name]").text(),
                $(sourceDiv).children("span[name=code]").text(),
                $(sourceDiv).children("img[name=" + masterType + "Img]").attr("origImg"),
                type, count, null, $(sourceDiv).attr("dbid"));
                    var newDiv = targetDiv + " div[dbid=" + ids[0] + "]";
                    if (ids[1] != "") {
                        if (ids[1] == "1") {
                            MarkDiv(newDiv, type + "Check", type);
                        }
                    }
                    count++;
                }
            }
        }
    }
}
//////////////////////~Items//////////////////////////////////////

//////////////////////Processes//////////////////////////////////////
function RemoveSelectedProcessDesc(divName) {
    $(divName).remove();
    CreateRowFromLineItem();
}
function AddSelectedProcessDesc(processDiv) {
    var id = $(processDiv).attr("id");
    var code = $(processDiv).children("span[name=code]").text();
    var isDiscount = $(processDiv).attr("isdiscount");
    var tax = $(processDiv).attr("tax");
    $("#processDescItems").append('<div id="' + id + 'Desc" rel="processDescItem" code="' + code + '" processdbid="' + GetDBID("#" + id) + '" isdiscount="' + isDiscount + '" tax="' + tax + '">'
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
        if (isNaN($(this).children("input[type=text]").val()) == false && isNaN($("#quantityContainer li.quantityText").text()) == false) {
            amount = parseFloat($(this).children("input[type=text]").val())
                        * parseFloat($("#quantityContainer li.quantityText").text());
        }
        $("#processData").append('<div rel="processData" dbid="" rate="" amount="" sequence="' + seq + '"></div>');
        var divName = "#processData div[sequence=" + seq + "]";
        SetProcessDataAttributes(divName, $(this).attr("processdbid"),
                                    $(this).children("input[type=text]").val(),
                                    amount, seq, $(this).attr("isdiscount"),
                                    $(this).attr("tax"));
    });
}
function SetProcessDataAttributes(divName, dbid, rate, amount, sequence, isdiscount, tax) {
    if (dbid == null) {
        dbid = "";
    }
    $(divName).attr("dbid", dbid);
    $(divName).attr("rate", rate);
    $(divName).attr("amount", amount);
    $(divName).attr("sequence", sequence);
    $(divName).attr("isdiscount", isdiscount);
    $(divName).attr("tax", tax);
}

//////////////////////~Processes//////////////////////////////////////

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
function AttachEventsForSubmit() {
    $("#btnSubmit").click(function () {
        SubmitBooking();
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
    var retVal = "0";
    $("#bookingTable tbody tr").each(function () {
        if ($(this).hasClass("selectedRow")) {
            retVal = $(this).attr("sequence");
        }
    });
    return retVal;
    //return $("#bookingTable tbody tr[class=selectedRow]").attr("sequence");
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
        if (confirm("Do you really want to delete the selected line item?")) {
            DeleteRowItem();
        }
    });
}
function DeleteRowItem() {
    var seq = GetSelectedRow();
    $("#bookingTable tbody tr[sequence=" + seq + "]").remove();
    $("#lineItems div[sequence=" + seq + "]").remove();
    //Re-Sequence line items
    $("#lineItems div[rel=lineItem]").each(function (i) {
        $(this).attr("sequence", i + 1);
    });
    //Re-Sequence table rows
    $("#bookingTable tbody tr").each(function (i) {
        $(this).attr("sequence", i + 1);
        $(this).children("td.col1").children("span").text(i + 1);
    });
    ResetAll();
    //Select the First line item
    if ($("#lineItems div").length > 0) {
        SelectRow("1");
        LoadLineItem($("#bookingTable tbody tr[sequence=1]").attr("sequence"));
    }
    else {
        //Add a new blank row
        AddNewRow()
    }
    ResetTotals();
}
function SaveLineItem(sequence) {
    if (sequence > 0) {
        var xmlItemNode = SaveItems();
        xmlItemNode = UpdateAllIdentityAttributes(xmlItemNode, "item", sequence);

        var xmlPatternsNode = SavePatterns();
        xmlPatternsNode = UpdateAllIdentityAttributes(xmlPatternsNode, "patterns pattern", sequence);

        var xmlColorsNode = SaveColors();
        xmlColorsNode = UpdateAllIdentityAttributes(xmlColorsNode, "colors color", sequence);

        var xmlProcessesNode = SaveProcesses();
        xmlProcessesNode = UpdateAllIdentityAttributes(xmlProcessesNode, "processes process", sequence);

        var xmlCategoriesNode = SaveCategories();
        xmlCategoriesNode = UpdateAllIdentityAttributes(xmlCategoriesNode, "categories category", sequence);

        var xmlBrandsNode = SaveBrands();
        xmlBrandsNode = UpdateAllIdentityAttributes(xmlBrandsNode, "brands brand", sequence);

        var xmlCommentsNode = SaveComments();
        xmlCommentsNode = UpdateAllIdentityAttributes(xmlCommentsNode, "comments comment", sequence);

        var xmlSubItemsNode = SaveSubItems();
        xmlSubItemsNode = UpdateAllIdentityAttributes(xmlSubItemsNode, "subItems subItem", sequence);

        var xmlVariationsNode = SaveVariations();
        xmlVariationsNode = UpdateAllIdentityAttributes(xmlVariationsNode, "variations variation", sequence);

        var xmlLineItem = "<lineItem>" + xmlItemNode + xmlPatternsNode + xmlColorsNode
                            + xmlSubItemsNode
                            + xmlCategoriesNode
                            + xmlBrandsNode
                            + xmlVariationsNode
                            + xmlCommentsNode
                            + xmlProcessesNode
                            + "</lineItem>";

        //Add this xml in the LineItems Div
        //debugger;
        $("#lineItems").children("div[sequence=" + sequence + "]").html('');
        $("#lineItems").children("div[sequence=" + sequence + "]").html(escape(xmlLineItem));
    }
}

function UpdateAllIdentityAttributes(xml, searchText, sequence) {
    var tempids = GetXmlData(null, searchText, "attribute", "id", "true", xml);
    if (tempids.indexOf(" ", 0) == -1) {
        xml = UpdateIdentityAttribute(xml, searchText, sequence, tempids);
    }
    else {
        tempids = tempids.split(" ");
        for (var i = 0; i < tempids.length; i++) {
            if (tempids[i] != "") {
                xml = UpdateIdentityAttribute(xml, searchText, sequence, tempids[i]);
            }
        }
    }
    return xml;
}

function UpdateIdentityAttribute(xml, itemName, sequence, id) {
    var mainXml = unescape($("#lineItems").children("div[sequence=" + sequence + "]").html());
    if (mainXml != null && mainXml != "") {
        var identity = GetXmlData(null, itemName + "[id=" + id + "]", "attribute", "identity", "false", mainXml);
        if (identity != null && identity != "" && identity != "0") {
            xml = SetXmlData(xml, itemName + "[id=" + id + "]", identity, "identity");
        }
    }
    return xml;
}
function SaveItems() {
    //Get itemNode from Template
    var itemNodeXml = GetXmlData("#lineItemTemplate", "lineItem item", "xml", null, null, null);
    //ItemID
    itemNodeXml = SetXmlData(itemNodeXml, "item", GetDBID("#catItems div[value=1]"), "id");
    //Quantity
    itemNodeXml = SetXmlData(itemNodeXml, "item quantity", $("#quantityContainer li.quantityText").text());
    //Length
    //itemNodeXml = SetXmlData(itemNodeXml, "item length", $("#txtLength").val());
    //Breadth
    //itemNodeXml = SetXmlData(itemNodeXml, "item breadth", $("#txtBreadth").val());
    //Area
    //itemNodeXml = SetXmlData(itemNodeXml, "item area1", $("#txtArea").val());
    //Remarks
    //itemNodeXml = SetXmlData(itemNodeXml, "item remarks", $("#txtItemRemarks").val());
    //Brand
    //itemNodeXml = SetXmlData(itemNodeXml, "item brand", $("#txtBrand").val());
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
            //isdiscount
            tempNode = SetXmlData(tempNode, "process isdiscount", $("#processData div[dbid=" + tempids[i] + "]").attr("isdiscount"));
            //tax
            tempNode = SetXmlData(tempNode, "process tax", $("#processData div[dbid=" + tempids[i] + "]").attr("tax"));
            allProcesses += tempNode;
        }
    }
    processesNodeXml = ReplaceNode(processesNodeXml, "<process ", "</process>", allProcesses);
    return processesNodeXml;
}

function SaveCategories() {
    var categoriesNodeXml = GetXmlData("#lineItemTemplate", "lineItem categories", "xml", null, null, null);
    //Get Allselected patternsIDS
    var tempids = GetDBIDs("#categoriesDetail div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "categories category", "xml", null, null, categoriesNodeXml);
    var tempNode = null;
    var allCategories = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get Color node
        tempNode = tempNodeXml;
        //ColorID
        tempNode = SetXmlData(tempNode, "category", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "category text", $("#categoriesDetail div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allCategories += tempNode;
    }
    categoriesNodeXml = ReplaceNode(categoriesNodeXml, "<category ", "</category>", allCategories);
    return categoriesNodeXml;
}

function SaveVariations() {
    var variationsNodeXml = GetXmlData("#lineItemTemplate", "lineItem variations", "xml", null, null, null);

    var tempids = GetDBIDs("#variationsDetail div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "variations variation", "xml", null, null, variationsNodeXml);
    var tempNode = null;
    var allVariations = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get Color node
        tempNode = tempNodeXml;
        //ColorID
        tempNode = SetXmlData(tempNode, "variation", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "variation text", $("#variationsDetail div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allVariations += tempNode;
    }
    variationsNodeXml = ReplaceNode(variationsNodeXml, "<variation ", "</variation>", allVariations);
    return variationsNodeXml;
}
function ReplaceAll(mainStr, searchStr, replaceStr) {
    while (mainStr.indexOf(searchStr) > -1) {
        mainStr = mainStr.replace(searchStr, replaceStr);
    }
    return mainStr;
}
function SaveSubItems() {
    var subItemsNodeXml = GetXmlData("#lineItemTemplate", "lineItem subItems", "xml", null, null, null);
    //For IE9
    subItemsNodeXml = ReplaceAll(subItemsNodeXml, "subitem", "subItem");
    var tempids = GetDBIDs("#subItemsDetail div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "subItems subItem", "xml", null, null, subItemsNodeXml);
    //for IE9
    tempNodeXml = ReplaceAll(tempNodeXml, "subitem", "subItem");
    var tempNode = null;
    var allSubItems = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get Color node
        tempNode = tempNodeXml;
        //ColorID
        tempNode = SetXmlData(tempNode, "subItem", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "subItem text", $("#subItemsDetail div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allSubItems += tempNode;
    }
    subItemsNodeXml = ReplaceNode(subItemsNodeXml, "<subItem ", "</subItem>", allSubItems);
    //for IE 9
    subItemsNodeXml = ReplaceAll(subItemsNodeXml, "subitem", "subItem");
    return subItemsNodeXml;
}
function SaveBrands() {
    var brandsNodeXml = GetXmlData("#lineItemTemplate", "lineItem brands", "xml", null, null, null);

    var tempids = GetDBIDs("#brands div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "brands brand", "xml", null, null, brandsNodeXml);
    var tempNode = null;
    var allBrands = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get Color node
        tempNode = tempNodeXml;
        //ColorID
        tempNode = SetXmlData(tempNode, "brand", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "brand text", $("#brands div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allBrands += tempNode;
    }
    brandsNodeXml = ReplaceNode(brandsNodeXml, "<brand ", "</brand>", allBrands);
    return brandsNodeXml;
}
function SaveComments() {
    var commentsNodeXml = GetXmlData("#lineItemTemplate", "lineItem comments", "xml", null, null, null);

    var tempids = GetDBIDs("#comments div[value=1]").split(" ");
    var tempNodeXml = GetXmlData(null, "comments comment", "xml", null, null, commentsNodeXml);
    var tempNode = null;
    var allComments = "";
    for (var i = 0; i < tempids.length; i++) {
        //Get Color node
        tempNode = tempNodeXml;
        //ColorID
        tempNode = SetXmlData(tempNode, "comment", tempids[i], "id");
        //text
        tempNode = SetXmlData(tempNode, "comment text", $("#comments div[dbid=" + tempids[i] + "] span[name!=code]").text());
        allComments += tempNode;
    }
    commentsNodeXml = ReplaceNode(commentsNodeXml, "<comment ", "</comment>", allComments);
    return commentsNodeXml;
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
        LoadLineItemBrands(lineItemXml);
        LoadLineItemComments(lineItemXml);
        LoadLineItemCategories(lineItemXml);
        LoadLineItemSubItems(lineItemXml);
        LoadLineItemVariations(lineItemXml);
    }
}

function LoadLineItemItems(lineItemXml) {
    //Get itemNode from Template
    var itemNodeXml = GetXmlData(null, "lineItem item", "xml", null, null, lineItemXml);
    //Set ItemID
    var controlValue = GetXmlData(null, "item", "attribute", "id", null, lineItemXml);
    //var tempid = $("#catItems div[dbid=" + controlValue + "]").attr("catid");
    //MarkDiv("#categoriesDetail div[dbid=" + tempid + "]", "categoryCheck", "category");
    MarkDiv("#catItems div[dbid=" + controlValue + "]", "itemCheck", "item");
    //Quantity
    $("#quantityContainer li.quantityText").text(GetXmlData(null, "item quantity", "nodeText", null, null, itemNodeXml));
    //Length
    //$("#txtLength").val(GetXmlData(null, "item length", "nodeText", null, null, itemNodeXml));
    //Breadth
    //$("#txtBreadth").val(GetXmlData(null, "item breadth", "nodeText", null, null, itemNodeXml));
    //Area
    //$("#txtArea").val(GetXmlData(null, "item area1", "nodeText", null, null, itemNodeXml));
    //Remarks
    //$("#txtItemRemarks").val(GetXmlData(null, "item remarks", "nodeText", null, null, itemNodeXml));
    //Brand
    //$("#txtBrand").val(GetXmlData(null, "item brand", "nodeText", null, null, itemNodeXml));
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
function LoadLineItemBrands(lineItemXml) {
    var brandsNodeXml = GetXmlData(null, "lineItem brands", "xml", null, null, lineItemXml);
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "brand", "attribute", "id", "true", brandsNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //ColorID
        MarkDiv("#brands div[dbid=" + tempids[i] + "]", "brandCheck", "brand");
    }
}
function LoadLineItemComments(lineItemXml) {
    var commentsNodeXml = GetXmlData(null, "lineItem comments", "xml", null, null, lineItemXml);
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "comment", "attribute", "id", "true", commentsNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //ColorID
        MarkDiv("#comments div[dbid=" + tempids[i] + "]", "commentCheck", "comment");
    }
}
function LoadLineItemCategories(lineItemXml) {
    //Unmark default selctions
    UnmarkDiv("#categoriesDetail div[value=1]", "categoryCheck", "category");
    var categoriesNodeXml = GetXmlData(null, "lineItem categories", "xml", null, null, lineItemXml);
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "category", "attribute", "id", "true", categoriesNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //ColorID
        MarkDiv("#categoriesDetail div[dbid=" + tempids[i] + "]", "categoryCheck", "category");
    }
}
function LoadLineItemSubItems(lineItemXml) {
    //Unmark default selctions
    UnmarkDiv("#subItemsDetail div[value=1]", "subItemCheck", "subItem");
    var subItemsNodeXml = GetXmlData(null, "lineItem subItems", "xml", null, null, lineItemXml);
    //For IE9
    subItemsNodeXml = ReplaceAll(subItemsNodeXml, "subitem", "subItem");
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "subItem", "attribute", "id", "true", subItemsNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //ColorID
        MarkDiv("#subItemsDetail div[dbid=" + tempids[i] + "]", "subItemCheck", "subItem");
    }
}
function LoadLineItemVariations(lineItemXml) {
    //Unmark default selctions
    UnmarkDiv("#variationsDetail div[value=1]", "variationCheck", "variation");
    var variationsNodeXml = GetXmlData(null, "lineItem variations", "xml", null, null, lineItemXml);
    //Get All selected patternsIDS
    var tempids = GetXmlData(null, "variation", "attribute", "id", "true", variationsNodeXml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //ColorID
        MarkDiv("#variationsDetail div[dbid=" + tempids[i] + "]", "variationCheck", "variation");
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
        //Set Rate
        $("#processDescItems div[processdbid=" + tempids[i] + "] input:text").val(rate);
        //seq++;
    }
}
//Reset LineItem
function ResetAll() {
    ResetItems();
    ResetItemDetails();
    ResetColorsPatterns();
    ResetProcesses();
    ResetBrands();
    ResetComments();
}
function ResetReceiptHeader() {
    //Reset Customer
    ResetCustomer();
    //Reset Discount
    ResetDiscountOptions();
    //Reset urgent options
    $("#urgentDescriptions").children().remove();
    //Reset all text boxes
    $("#leftContainer input:text").val("");
    //Reset Div buttons
    ResetDivButtons();
}
function ResetDiscountOptions() {
    //Uncheck both check boxes
    $("#discountOptions input:checkbox").attr("checked", false);
    //Remove values from text baxes
    $("#discountOptions input:text").val("");
}
function ResetTotals() {
    $("#receiptTotals input:text").val('');
}
function ResetItems() {
    //Unmark all category Divs
    //UnmarkDiv("#categoriesDetail div[value=1]", "categoryCheck", "category");
    //Unmark all Item Divs
    UnmarkDiv("#catItems div[value=1]", "itemCheck", "item");
    //Reset all text baxes
    //$("#itemDesc1 input:text").val('');
    //$("#itemDesc2 input:text").val('');
    $("#quantityContainer li.quantityText").text("1");
}
function ResetBrands() {
    UnmarkDiv("#brands div[value=1]", "brandCheck", "brand");
}
function ResetComments() {
    UnmarkDiv("#comments div[value=1]", "commentCheck", "comment");
}
function ResetItemDetails() {
    ResetCategories();
    ResetVariations();
    ResetSubItems();
}
function ResetCategories() {
    $("#categoriesDetail").children().remove();
    AddWatermark_Category();
}
function ResetVariations() {
    $("#variationsDetail").children().remove();
    AddWatermark_Variation()
}
function ResetSubItems() {
    $("#subItemsDetail").children().remove();
    AddWatermark_SubItem();
}
function AddWatermark_Category() {
    $("#categoriesDetail").append('<span class="watermark">Categories</span>');
}
function AddWatermark_Variation() {
    $("#variationsDetail").append('<span class="watermark">Variations</span>');
}
function AddWatermark_SubItem() {
    $("#subItemsDetail").append('<span class="watermark">Sub Items</span>');
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
function CreateRowFromLineItem(saveItemFlag) {
    var seq = GetSelectedRow();
    var tdSel = "#bookingTable table tbody tr[sequence=" + seq + "]";
    if (saveItemFlag == null || saveItemFlag == true) {
        UpdateProcess();
        SaveLineItem(seq);
    }
    var xml = GetLineItemXml(seq);
    //Col 2 - Qty
    $(tdSel + " td:nth-child(2)").children().remove();
    $(tdSel + " td:nth-child(2)").append("<span>" + GetItemQuantity(xml) + "</span>");
    //Col 3 - Desc
    var desc = "<span class='descCategory'></span><span class='descItem'></span>"
                + "<span class='descVariations'></span><span class='descSubItems'></span>"
                + "<br><span class='descColors'></span><span class='descPatterns'></span>"
                + "<span class='descComments'></span><span class='descBrands'></span>";
    var descTD = tdSel + " td:nth-child(3)";
    $(descTD).children().remove();
    $(descTD).append(desc);
    //descCategory
    var tempText = GetItemDesc(xml, "category");
    if (tempText != "") { tempText += " "; }
    $(descTD).children("span.descCategory").text(tempText);
    //descItem
    tempText = GetItemDesc(xml, "item");
    if (tempText != "") { tempText += " "; }
    $(descTD).children("span.descItem").text(tempText);
    //descSubItems
    tempText = GetItemDesc(xml, "subItem");
    if (tempText != "") { tempText = "(" + tempText.replace(/~/g, ", ") + ")"; }
    $(descTD).children("span.descSubItems").text(tempText);
    //descVariations
    tempText = GetItemDesc(xml, "variation");
    if (tempText != "") { tempText += " "; }
    $(descTD).children("span.descVariations").text(tempText);
    //descColors
    tempText = GetItemDesc(xml, "color");
    if (tempText != "") { tempText = tempText.replace(/~/g, ", ") + "/ "; }
    $(descTD).children("span.descColors").text(tempText);
    //descPatterns
    tempText = GetItemDesc(xml, "pattern");
    if (tempText != "") { tempText = tempText.replace(/~/g, ", ") + "/ "; }
    $(descTD).children("span.descPatterns").text(tempText);
    //descComments
    tempText = GetItemDesc(xml, "comment");
    if (tempText != "") { tempText = tempText.replace(/~/g, ", ") + "/ "; }
    $(descTD).children("span.descComments").text(tempText);
    //descBrands
    tempText = GetItemDesc(xml, "brand");
    if (tempText != "") { tempText = tempText.replace(/~/g, ", ") + " "; }
    $(descTD).children("span.descBrands").text(tempText);
    //    tempText = GetPatternsText(xml);
    //
    //    tempText = tempText.replace(/~/g, " ~ ");
    //    var tempText1 = GetColorsText(xml);
    //    tempText1 = tempText1.replace(/~/g, " ~ ");
    //    $(tdSel + " td:nth-child(3)").append("<span>" +
    //                GetItemText(xml) + " - " + tempText + " - " + tempText1
    //             + "</span");
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
    UpdateTotalsInLineItem(seq, xml);
    ShowTotals();
}

function UpdateTotalsInLineItem(seq, xml) {
    var tdSel = "#bookingTable table tbody tr[sequence=" + seq + "]";
    $(tdSel + " td:nth-child(5)").children().remove();
    var amounts = GetProcessesAmountText(xml).split("~");
    var amount = 0;
    for (var i = 0; i < amounts.length; i++) {
        if (amounts[i] != "") {
            if (isNaN(amounts[i]) == false) {
                var tempAmount = parseFloat(amounts[i]);
                //Add urgent amount in the process if any urgent option is selected
                var urgentAmount = GetUrgentAmount(tempAmount);
                //amount = urgentAmount;
                amount += urgentAmount;
            }
        }
    }
    $(tdSel + " td:nth-child(5)").append("<span>" + amount + "</span>");
}

function ApplyUrgentAmount() {
    $("#bookingTable table tbody tr").each(function () {
        var seq = $(this).attr("sequence");
        var xml = GetLineItemXml(seq);
        UpdateTotalsInLineItem(seq, xml);
    });
    ShowTotals();
}

function ApplyUrgentDate() {
    var flag = false;
    $("div[radioGroup=urgentOptions]").each(function () {
        if ($(this).attr("value") == "1") {
            $("#txtDueDate").val($(this).children("span[rel=urgentdate]").text());
            flag = true;
        }
    });
    if (flag == false) {
        $("#txtDueDate").val($("#txtDueDate").attr("default"));
    }
}
function GetUrgentAmount(amount) {
    var retAmount = amount;
    $("div[radioGroup=urgentOptions]").each(function () {
        if ($(this).attr("value") == "1") {
            var temp = $(this).children("span[rel=urgentrate]").text();
            retAmount = retAmount + ((parseFloat(temp) * retAmount) / 100);
        }
    });
    return retAmount;
}

function ShowTotals() {
    var total = 0;
    $("#bookingTable table tbody tr td:nth-child(5)").each(function () {
        if ($(this).children("span").text() != "" && !isNaN($(this).children("span").text())) {
            total += parseFloat($(this).children("span").text());
        }
    });
    CalculateDiscount();
    CalculateTax();
    var dc = $("#txtDiscount").val();
    var tax = $("#txtTax").val();
    var adv = $("#txtAdvance").val();
    if (isNaN(adv)) {
        adv = 0;
        $("#txtAdvance").val(0);
    }
    else if (adv == "") {
        adv = 0;
        $("#txtAdvance").val(0);
    }
    $("#txtGross").val(total);
    total = parseFloat(total) - parseFloat(dc) + parseFloat(tax) - parseFloat(adv);
    $("#txtNetTotal").val(total);
}
function GetItemDesc(lineItemXml, descName) {
    return GetXmlData(null, descName + " text", "nodeText", null, "true", lineItemXml);
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
    var divButton = "div.button";
    //$("div.button").each(function () {
    $(divButton).live("mouseenter", function () {
        //add selected class
        if ($(this).hasClass("selectedButton") == false) {
            $(this).addClass("selectedButton");
        }
        //if the div has multipleoptions, then show them
        var options = $(this).attr("rel");
        if (options != null || options != '') {
            var optionsdiv = $(this).attr("optionsDiv");
            $(optionsdiv).show();
        }
    });
    $(divButton).live("mouseleave", function () {
        //If the div is selected, then dont remove selected class else remove it
        if ($(this).attr("value") == null || $(this).attr("value") == "0") {
            $(this).removeClass("selectedButton");
        }
        //if the div has multipleoptions, then show them
        var options = $(this).attr("rel");
        if (options != null && options != '') {
            var optionsdiv = $(this).attr("optionsDiv");
            $(optionsdiv).hide();
        }
    });
    $(divButton).live("click", function (e) {
        //Uncheck all other with same radioGroup name
        var radioGroup = $(this).attr("radioGroup");
        //If the div is already selected, then unselect it else select it
        if ($(this).attr("value") == "1") {
            $(this).removeClass("selectedButton");
            $(this).attr("value", "0");
            var autoSelect = $(this).attr("autoSelect");
            if (autoSelect != null && autoSelect != '') {
                $(autoSelect).removeClass("selectedButton");
                $(autoSelect).attr("value", "0");
            }
        }
        else {
            var options = $(this).attr("rel");
            if (options != null && options != '') { return; }
            //Remove the selected class from every div and select the current div
            $("div.button[radioGroup=" + radioGroup + "]").removeClass("selectedButton");
            $("div.button[radioGroup=" + radioGroup + "]").attr("value", "0")
            $(this).addClass("selectedButton");
            $(this).attr("value", "1");
            //e.stopImmediatePropagation();
            //return false;
            var autoSelect = $(this).attr("autoSelect");
            if (autoSelect != null && autoSelect != '') {
                $(autoSelect).addClass("selectedButton");
                $(autoSelect).attr("value", "1");
            }
        }
        if (radioGroup == 'urgentOptions' || radioGroup == "urgent") {
            ApplyUrgentAmount();
            ApplyUrgentDate();
        }
    });
}
function ResetDivButtons() {
    $("div.button").attr("value", "0");
    $("div.button").removeClass("selectedButton");
}
/////////////////////////////~Div Buttons////////////////////
function AttachEventsForDatePicker() {
    $("#txtDueDate").datepick({ dateFormat: 'D dd-mm-yyyy' });
}

/////////////////////////////FLYOUT//////////////////////////
function AttachEventsForFlyout() {
    var flag = false;
    $("div.container5").mouseenter(function (event) {
        var panel = "#" + $(this).attr("flyoutDiv");
        $(panel).addClass("semiTransparent");
        var left = $(this).offset().left - $(panel).width();
        var top = $(this).offset().top - $(panel).height() / 2;
        $(this).children().addClass("selectedFlyout");
        $(panel).css("left", left);
        $(panel).css("top", top)
        $(panel).show();
        return false;
    })
    .mouseleave(function (event) {
        var panel = "#" + $(this).attr("flyoutDiv");
        HidePanel(panel);
        return false;
    })
    .click(function (event) {
        //event.stopImmediatePropagation();
        var panel = "#" + $(this).attr("flyoutDiv");
        ShowPanel(panel);
        return false;
    });

    $("div[rel=panel]").mouseenter(function () {
        ShowPanel(this);
    });
}

function ShowPanel(panel) {
    $(panel).removeClass("semiTransparent");
    //$(panel).addClass("fullOpaque");
    $(panel).parent().children("div.iconContainer").addClass("selectedFlyout");
    $(panel).show();
}

function HidePanel(panel) {
    $(panel).removeClass("semiTransparent");
    //$(panel).removeClass("fullOpaque");
    $(panel).parent().children("div.iconContainer").removeClass("selectedFlyout");
    $(panel).hide();
}
/////////////////////////////~FLYOUT//////////////////////////

/////////////////////////////Input Devices//////////////////////////
function AttachEventsForInputDevices(targetDiv) {
    var temp1 = null;
    $(targetDiv).mouseenter(function () {
        //Attach the input device on the top right hand side
        var left = $(targetDiv).position().left + $(targetDiv).width();
        var top = $(targetDiv).position().top;
        $("#inputDeviceContainer").css("left", left);
        $("#inputDeviceContainer").css("top", top);
        $("#inputDeviceContainer").addClass("semiTransparent");
        //$("#inputDeviceContainer").show();
    })
    .mouseleave(function () {
        //temp1 = setTimeout(HideInputDevices, 500);
        HideInputDevices();
    });

    $("#inputDeviceContainer").mouseenter(function () {
        if (temp1 != null) {
            clearTimeout(temp1);
        }
        $(this).removeClass("semiTransparent");
        $(this).show();
    })
    .mouseleave(function () {
        HideInputDevices();
    });

    $("#keyboard").click(function () {
        HideInputDevices();
        $("#rightContainer").hide();
        $("#rightContainerKbd").show();
    });

    $("#mouse").click(function () {
        HideInputDevices();
        $("#rightContainerKbd").hide();
        $("#rightContainer").show();
    });

    $(document).keydown(function (e) {
        //alert(e.which);
        if (e.altKey && e.which == 75) { $("#keyboard").trigger("click"); }
        if (e.altKey && e.which == 77) { $("#mouse").trigger("click"); }
    });
}
function HideInputDevices() {
    $("#inputDeviceContainer").removeClass("semiTransparent");
    $("#inputDeviceContainer").hide();
}
function SelectDefaultInputMode() {
    if (window.location.href.indexOf("kbd", 0) > -1) {
        $("#keyboard").trigger("click");
    }
    else {
        $("#mouse").trigger("click");
    }
}
/////////////////////////////~Input Devices//////////////////////////
/////////////////////////////Keyboard//////////////////////////

/////////////////////////////~Keyboard//////////////////////////

/////////////////////////////ReciptHeader//////////////////////////
function LoadReceiptHeaderTemplate(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            var requestBody = '<GetReceiptHeaderTemplate xmlns="http://tempuri.org/" />';
            var sFn = LoadReceiptHeaderTemplate;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetReceiptHeaderTemplate", "1", targetDiv);
            }, 500);
        }
        else {
            //Load Template
            $(targetDiv).text(escape($(data).find("GetReceiptHeaderTemplateResponse").find("GetReceiptHeaderTemplateResult").text()));
            MarkDivAsLoaded(targetDiv);
        }
    }
}
/////////////////////////////~ReciptHeader//////////////////////////

/////////////////////////////Customer//////////////////////////
function LoadCustomers(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            var requestBody = '<GetAllCustomers xmlns="http://tempuri.org/" />';
            var sFn = LoadCustomers;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllCustomers", "1", targetDiv);
            }, 500);
        }
        else {
            //Load Template
            $(targetDiv).text(escape($(data).find("GetAllCustomersResponse").find("GetAllCustomersResult").text()));
            MarkDivAsLoaded(targetDiv);
        }
    }
}

function SelectCustomer(inputid, dbid) {
    var sourceDiv = $(inputid).attr("sourceDiv");
    var name = GetXmlData(sourceDiv, "customer[id=" + dbid + "] name", "nodeText", null, null, null);
    var phone = GetXmlData(sourceDiv, "customer[id=" + dbid + "] phone", "nodeText", null, null, null);
    var address = GetXmlData(sourceDiv, "customer[id=" + dbid + "] address", "nodeText", null, null, null);
    var priority = GetXmlData(sourceDiv, "customer[id=" + dbid + "] priority", "nodeText", null, null, null);
    var remarks = GetXmlData(sourceDiv, "customer[id=" + dbid + "] remarks", "nodeText", null, null, null);
    var discount = GetXmlData(sourceDiv, "customer[id=" + dbid + "] discount", "nodeText", null, null, null);
    //ID
    $("#customerSearch").attr("custid", dbid);
    //Name
    $("span#spnCustomerName").text(name);
    //Phone
    $("span#spnCustomerPhone").text(phone);
    //Address
    $("span#spnCustomerAddress").text(address);
    //Add Priority and Remarks
    //Priority
    $("#txtCustomerPriority").val(priority);
    //Remarks
    $("#txtCustomerRemarks").val(remarks);
    //Discount
    $("#customerDiscount").text(discount);
    //Add discount % in the % discount text box
    $("#txtPercentDiscount").val(discount);
    ShowTotals();
}

function ResetCustomer() {
    //ID
    $("#customerSearch").attr("custid", "");
    //Name
    $("span#spnCustomerName").text("");
    //Phone
    $("span#spnCustomerPhone").text("");
    //Address
    $("span#spnCustomerAddress").text("");
    //Add Priority and Remarks
    //Priority
    $("#txtCustomerPriority").val("");
    //Remarks
    $("#txtCustomerRemarks").val("");
    //Discount
    $("#customerDiscount").text("");
    //Add discount % in the % discount text box
    $("#txtPercentDiscount").val("");
}
/////////////////////////////~Customer//////////////////////////

/////////////////////////////Load Defaults//////////////////////////
function LoadDefaults(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetAllDefaults xmlns="http://tempuri.org/" />';
            var sFn = LoadDefaults;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetAllDefaults", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).text(escape($(data).find("GetAllDefaultsResponse").find("GetAllDefaultsResult").text()));
            MarkDivAsLoaded(targetDiv);
        }
    }
}

function ApplyDefaults() {
    //Default Item
    var temp = GetXmlData("#defaults", "defaults item", "nodeText", null, null, null);
    //Find item with the required db id
    $("#catItems div[dbid=" + temp + "]").trigger("click");
    //Default Process
    temp = GetXmlData("#defaults", "defaults process", "nodeText", null, null, null);
    //Find item with the required db id
    $("#processes div[dbid=" + temp + "]").trigger("click");
    //Booking Number
    temp = GetXmlData("#defaults", "defaults bookingnumber", "nodeText", null, null, null);
    //Find item with the required db id
    $("#receiptNo span").text(temp);
    //Default Date Offset
    temp = GetXmlData("#defaults", "defaults duedate", "nodeText", null, null, null);
    var d = new Date()
    d = new Date(d.setDate(d.getDate() + parseInt(temp)));
    $("#txtDueDate").val(ConvertDate(d));
    $("#txtDueDate").attr("default", ConvertDate(d));
    //Due time
    temp = GetXmlData("#defaults", "defaults duetime", "nodeText", null, null, null);
    $("#txtDueTime").val(temp);
    $("#txtDueTime").attr("default", temp);
    //Add urgent divs
    var tempids = GetXmlData("#defaults", "defaults urgentdescriptions urgent", "attribute", "seq", null, null).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        //Get Offset
        temp = GetXmlData("#defaults", "defaults urgentdescriptions urgent[seq=" + tempids[i] + "] dateoffset", "nodeText", null, null, null);
        d = new Date()
        d = new Date(d.setDate(d.getDate() + parseInt(temp)));
        var temp1 = ConvertDate(d);
        //Get rate
        temp = GetXmlData("#defaults", "defaults urgentdescriptions urgent[seq=" + tempids[i] + "] rate", "nodeText", null, null, null);
        var desc = "Deliver on " + temp1 + " @ " + temp + "% extra";
        $("#urgentDescriptions").children().remove();
        $("#urgentDescriptions").append("<div id='urgentseq" + i + "' class='button' radioGroup='urgentOptions' autoSelect='#urgent'>"
                                            + "<span>" + desc + "</span>"
                                            + "<span rel='urgentdate' class='hidden'>" + temp1 + "</span>"
                                            + "<span rel='urgentrate' class='hidden'>" + temp + "</span>"
                                        + "</div>");
    }
    //Adjust left and top of the urgentDesciptions div
    var left = $("#urgent").position().left;
    var top = $("#urgent").position().top - $("#urgentDescriptions").height() + 5;
    $("#urgentDescriptions").css("left", left);
    $("#urgentDescriptions").css("top", top);
    $("#urgentDescriptions")
    .mouseenter(function () {
        $(this).show();
    })
    .mouseleave(function () {
        $(this).hide();
    });
    HideProcessingDialog();
}

/////////////////////////////~Load Defaults//////////////////////////

/////////////////////////////Discount//////////////////////////
function AttachEventsForDiscount() {
    $("input[rel=discountOptions]").click(function () {
        var discountDiv = "#discountOptions";
        var left = $(this).position().left;
        var top = $(this).position().top - $(discountDiv).height();
        $(discountDiv).css("left", left);
        $(discountDiv).css("top", top);
        $(discountDiv).show();
    });
    $("#chkPercent").click(function () {
        if ($(this).is(":checked")) {
            //Uncheck other checkbox
            $("#chkFlat").attr("checked", false);
        }
        CalculateDiscount();
    });
    $("#chkFlat").click(function () {
        if ($(this).is(":checked")) {
            //Uncheck other checkbox
            $("#chkPercent").attr("checked", false);
        }
        CalculateDiscount();
    });
}
function HideDiscountOptions() {
    $("#discountOptions").hide();
}
function AttachEventsForInputMultipleOptions() {
    $("input[rel=multipleOptions]").focus(function () {
        var optionsDiv = $(this).attr("optionsDiv");
        var left = $(this).position().left;
        var top = $(this).position().top - $(optionsDiv).height();
        $(optionsDiv).css("left", left);
        $(optionsDiv).css("top", top);
        $(optionsDiv).show();
    });
}
function GetDiscountAmount() {
    //Get total process amount for which discount is applicable
    var discountAmount = 0;
    $("#lineItems").children("div[rel=lineItem]").each(function () {
        var lineitemsXml = $(this).html();
        if (lineitemsXml != "") {
            var tempids = GetXmlData(null, "lineItem processes process", "attribute", "id", "true", lineitemsXml).split(" ");
            var isdiscount = "False";
            for (var i = 0; i < tempids.length; i++) {
                isdiscount = GetXmlData(null, "lineItem processes process[id=" + tempids[i] + "] isdiscount", "nodeText", null, null, lineitemsXml);
                if (isdiscount == "True") {
                    var temp = GetXmlData(null, "lineItem processes process[id=" + tempids[i] + "] amount", "nodeText", "amount", null, lineitemsXml);
                    if (temp != "" && temp != "0" && isNaN(temp) == false) {
                        discountAmount += parseFloat(temp);
                    }
                }
            }
        }
    });
    return discountAmount;
}
function GetTaxAmount() {
    //Get total process amount for which tax is applicable
    var taxAmount = 0;
    $("#lineItems").children("div[rel=lineItem]").each(function () {
        var lineitemsXml = $(this).html();
        if (lineitemsXml != "") {
            var tempids = GetXmlData(null, "lineItem processes process", "attribute", "id", "true", lineitemsXml).split(" ");
            var tax = 0;
            for (var i = 0; i < tempids.length; i++) {
                tax = GetXmlData(null, "lineItem processes process[id=" + tempids[i] + "] tax", "nodeText", null, null, lineitemsXml);
                if (tax != "0" && tax != "" && isNaN(tax) == false) {
                    var temp = GetXmlData(null, "lineItem processes process[id=" + tempids[i] + "] amount", "nodeText", null, null, lineitemsXml);
                    if (temp != "" && temp != "0" && isNaN(temp) == false) {
                        var temp1 = parseFloat(temp) * parseFloat(tax) / 100;
                        taxAmount += temp1;
                    }
                }
            }
        }
    });
    return taxAmount;
}
function CalculateDiscount() {
    var discount = GetDiscountAmount();
    var netdc = parseFloat(0);
    $("#discountOptions span[rel=totalPercentDiscount]").text('');
    $("#discountOptions span[rel=totalFlatDiscount]").text('');
    //$("#applicableDiscount").text(discount);
    if ($("#chkPercent").is(":checked")) {
        var dcp = $("#txtPercentDiscount").val();
        if (dcp != "" && dcp != "0" && isNaN(dcp) == false) {
            netdc = parseFloat(dcp) * parseFloat(discount) / 100;
            $("#discountOptions span[rel=totalPercentDiscount]").text(netdc);
        }
    }
    if ($("#chkFlat").is(":checked")) {
        var dcp = $("#txtFlatDiscount").val();
        if (dcp != "" && dcp != "0" && isNaN(dcp) == false) {
            netdc = dcp;
            $("#discountOptions span[rel=totalFlatDiscount]").text(netdc);
        }
    }
    $("#txtDiscount").val(netdc.toFixed(2));
}
function CalculateTax() {
    var tax = GetTaxAmount();
    $("#txtTax").val(tax.toFixed(2));
}

/////////////////////////////~Discount//////////////////////////

/////////////////////////////PriceList//////////////////////////
function LoadPriceList(data, status, xhr, initFlag, targetDiv) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowLoadingDiv(targetDiv, "true");
            var requestBody = '<GetPriceList xmlns="http://tempuri.org/" />';
            var sFn = LoadPriceList;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetPriceList", "1", targetDiv);
            }, 500);
        }
        else {
            //Load PriceList
            $(targetDiv).text(escape($(data).find("GetPriceListResponse").find("GetPriceListResult").text()));
            MarkDivAsLoaded(targetDiv);
        }
    }
}
function ShowAllItemPrice() {
    $("#processes div[value=1]").each(function () {
        ShowItemPrice($(this).attr("id"));
    });
}
function ShowItemPrice(processid) {
    var itemid = GetDBID($("#catItems div[value=1]"));
    if (itemid == null) { itemid = "0"; }

    var categoryid = GetDBID("#categoriesDetail div[value=1]");
    if (categoryid == null) { categoryid = "0"; }

    var variationid = GetDBID("#variationsDetail div[value=1]");
    if (variationid == null) { variationid = "0"; }

    var processdbid = GetDBID("#" + processid);
    var customid = itemid + "_" + categoryid + "_" + variationid;
    var price = 0;
    var tempcustomid = "";
    if ($("#subItemsDetail div[value=1]").length == 0) {
        customid = customid + "_0_" + processdbid;
        var temp1 = GetXmlData("#priceList", "pricelist price[customid=" + customid + "]", "nodeText", null, null, null);
        if (temp1 != "" && temp1 != null && !isNaN(temp1)) {
            price = parseFloat(temp1);
        }
    }
    else {
        $("#subItemsDetail div[value=1]").each(function () {
            var subitemid = GetDBID(this);
            tempcustomid = customid + "_" + subitemid + "_" + processdbid;
            var temp = GetXmlData("#priceList", "pricelist price[customid=" + tempcustomid + "]", "nodeText", null, null, null);
            if (temp != "" && temp != null && !isNaN(temp)) {
                price += parseFloat(temp);
            }
            tempcustomid = "";
        });
    }
    if (price == 0) {
        price = "";
    }
    //Display this price in the amount text box
    $("#processDescItems input[id=" + processid + "text]").val(price);
}
/////////////////////////////~PriceList//////////////////////////

/////////////////////////////SaveEntireRecord//////////////////////////
function LoadNewBookingReceipt() {
    //Clear all LineItem controls
    ResetAll();
    //Reset ReceiptHedaer
    ResetReceiptHeader();
    //Reset All Totals
    ResetTotals();
    //Remove all table rows
    $("#bookingTable table tbody tr").remove();
    //Remove all saved line item xmls
    ResetLineItems();

    //Add a new row
    AddNewRow();
    //Apply Defaults
    ApplyDefaults();
}

function ResetLineItems() {
    $("#lineItems").children().remove();
}
function SaveReceiptHeader() {
    //Get template XML
    var headerXml = unescape($("#receiptHeaderTemplate").html());
    //Set Walkin Receipt #
    headerXml = SetXmlData(headerXml, "receiptheader iswalkin bookingnumber", $("#receiptNo span").text(), null);
    //Set Home Receipt #
    headerXml = SetXmlData(headerXml, "receiptheader ishomebooking homeeceiptnumber", $("#homeDeliveryReceipt span").text(), null);
    //Set Customer ID
    headerXml = SetXmlData(headerXml, "receiptheader customerid", $("#customerSearch").attr("custid"), null);
    //Set Duedate
    headerXml = SetXmlData(headerXml, "receiptheader duedate", $("#txtDueDate").val(), null);
    //Set Due time
    headerXml = SetXmlData(headerXml, "receiptheader duetime", $("#txtDueTime").val(), null);
    //Set Urgent
    headerXml = SetXmlData(headerXml, "receiptheader isurgent", $("#urgent").attr("value"), null);
    //Set SMS
    headerXml = SetXmlData(headerXml, "receiptheader issms", $("#sms").attr("value"), null);
    //Set Email
    headerXml = SetXmlData(headerXml, "receiptheader isemail", $("#email").attr("value"), null);
    //Set remarks
    headerXml = SetXmlData(headerXml, "receiptheader remarks", $("#txtRemarks").val(), null);
    //Set salesman
    headerXml = SetXmlData(headerXml, "receiptheader salesman", $("#drpSalesman").val(), null);
    //Set checkedby
    headerXml = SetXmlData(headerXml, "receiptheader checkedby", $("#drpCheckedBy").val(), null);
    //Set quantity
    headerXml = SetXmlData(headerXml, "receiptheader quantity", $("#quantityText").text(), null);
    //Set totalgrossamount
    headerXml = SetXmlData(headerXml, "receiptheader totalgrossamount", $("#txtGross").val(), null);
    //Set totaldiscount
    headerXml = SetXmlData(headerXml, "receiptheader totaldiscount", $("#txtDiscount").val(), null);
    //Set totaltax
    headerXml = SetXmlData(headerXml, "receiptheader totaltax", $("#txtTax").val(), null);
    //Set totaladvance
    headerXml = SetXmlData(headerXml, "receiptheader totaladvance", $("#txtAdvance").val(), null);
    return headerXml;
}

function LoadReceiptHeader(headerXml) {
    //Set Walkin Receipt #
    var temp = GetXmlData(null, "receiptheader iswalkin bookingnumber", "nodeText", null, null, headerXml);
    $("#receiptNo span").text(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader iswalkin bookingnumber", $("#receiptNo span").text(), null);
    //Set Home Receipt #
    temp = GetXmlData(null, "receiptheader ishomebooking homeeceiptnumber", "nodeText", null, null, headerXml);
    $("#homeDeliveryReceipt span").text(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader ishomebooking homeeceiptnumber", $("#homeDeliveryReceipt span").text(), null);
    //Set Customer ID
    temp = GetXmlData(null, "receiptheader customerid", "nodeText", null, null, headerXml);
    SelectCustomer("#txtSearchCustomer", temp);
    //headerXml = SetXmlData(headerXml, "receiptheader customerid", $("#customerSearch").attr("custid"), null);
    //Set Duedate
    temp = GetXmlData(null, "receiptheader duedate", "nodeText", null, null, headerXml);
    $("#txtDueDate").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader duedate", $("#txtDueDate").val(), null);
    //Set Due time
    temp = GetXmlData(null, "receiptheader duetime", "nodeText", null, null, headerXml);
    $("#txtDueTime").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader duetime", $("#txtDueTime").val(), null);
    //Set Urgent
    temp = GetXmlData(null, "receiptheader isurgent", "nodeText", null, null, headerXml);
    if (temp == "1") {
        $("#urgent").trigger("click");
    }
    //headerXml = SetXmlData(headerXml, "receiptheader isurgent", $("#urgent").attr("value"), null);
    //Set SMS
    temp = GetXmlData(null, "receiptheader issms", "nodeText", null, null, headerXml);
    if (temp == "1") {
        $("#sms").trigger("click");
    }
    //headerXml = SetXmlData(headerXml, "receiptheader issms", $("#sms").attr("value"), null);
    //Set Email
    temp = GetXmlData(null, "receiptheader isemail", "nodeText", null, null, headerXml);
    if (temp == "1") {
        $("#email").trigger("click");
    }
    //headerXml = SetXmlData(headerXml, "receiptheader isemail", $("#email").attr("value"), null);
    //Set remarks
    temp = GetXmlData(null, "receiptheader remarks", "nodeText", null, null, headerXml);
    $("#txtRemarks").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader remarks", $("#txtRemarks").val(), null);
    //Set salesman
    temp = GetXmlData(null, "receiptheader salesman", "nodeText", null, null, headerXml);
    $("#drpSalesman").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader salesman", $("#drpSalesman").val(), null);
    //Set checkedby
    temp = GetXmlData(null, "receiptheader checkedby", "nodeText", null, null, headerXml);
    $("#drpCheckedBy").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader checkedby", $("#drpCheckedBy").val(), null);
    //Set quantity
    temp = GetXmlData(null, "receiptheader quantity", "nodeText", null, null, headerXml);
    $("#quantityText").text(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader quantity", $("#quantityText").text(), null);
    //Set totalgrossamount
    temp = GetXmlData(null, "receiptheader totalgrossamount", "nodeText", null, null, headerXml);
    $("#txtGross").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader totalgrossamount", $("#txtGross").val(), null);
    //Set totaldiscount
    temp = GetXmlData(null, "receiptheader totaldiscount", "nodeText", null, null, headerXml);
    $("#txtDiscount").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader totaldiscount", $("#txtDiscount").val(), null);
    //Set totaltax
    temp = GetXmlData(null, "receiptheader totaltax", "nodeText", null, null, headerXml);
    $("#txtTax").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader totaltax", $("#txtTax").val(), null);
    //Set totaladvance
    temp = GetXmlData(null, "receiptheader totaladvance", "nodeText", null, null, headerXml);
    $("#txtAdvance").val(temp);
    //headerXml = SetXmlData(headerXml, "receiptheader totaladvance", $("#txtAdvance").val(), null);
    $("#receiptHeaderTemplate").html(escape(headerXml));
}

function CreateBookingXml() {
    //Create XML
    //Get all lineitems
    var lineitemsXml = escape("<lineitems>");
    $("#lineItems").children("div[rel=lineItem]").each(function () {
        var temp = $(this).html();
        if (temp != "") {
            temp = SetXmlData(temp, "lineItem", $(this).attr("sequence"), "sequence");
            lineitemsXml += escape(temp);
        }
    });
    lineitemsXml += escape("</lineitems>");
    var bookingXml = escape("<booking>") + escape(SaveReceiptHeader()) + lineitemsXml + escape("</booking>");
    return bookingXml;
}

function SubmitBooking() {
    ShowProcessingDialog("Validating data...");
    CleanupErroredControls();
    var xml = CreateBookingXml();
    if (ValidateBooking(xml) == true) {
        SaveBookingData(null, null, null, "0", "#dialog", xml);
    }
    else {
        ShowErrorMessages();
    }
    //Attach Error Events
    AttacheErrorEvents();
}

function SaveBookingData(data, status, xhr, initFlag, targetDiv, xml) {
    if (initFlag == "0") {
        //var targetDiv = "#dialog";
        var sFn = SaveBookingData;
        var eFn = AjaxError;
        ShowLoadingDiv(targetDiv, null, "Submitting data...");
        //Save Booking Xml
        var requestBody = '<SaveBooking xmlns="http://tempuri.org/">' +
                              '<bookingXml>' + xml + '</bookingXml>' +
                          '</SaveBooking>';
        DoAjaxCall(requestBody, sFn, eFn, "SaveBooking", "1", targetDiv);
    }
    else {
        var result = escape($(data).find("SaveBookingResponse").find("SaveBookingResult").text());
        if (result != "-1") {
            $(targetDiv).children().remove();
            $(targetDiv).append("<span>Record saved successfully.</span>");
            //Load bookingnumber in defaults xml
            var defaultsXml = SetXmlData($("#defaults").text(), "defaults bookingnumber", result, null);
            $("#defaults").text(escape(defaultsXml));
            LoadNewBookingReceipt();
        }
        else {
            //Remove All Children
            $(targetDiv).children().remove();
            $(targetDiv).append('<div name=error></div>');
            $(targetDiv + " div[name=error]").append('<p>Unable to save booking</p>');
        }
    }
}

function ValidateBooking(xml) {
    var retVal = true;
    var temp = true;
    //Validate LineItems
    var tempids = GetXmlData(null, "booking lineitems lineItem", "attribute", "sequence", "true", xml).split(" ");
    for (var i = 0; i < tempids.length; i++) {
        var lineItem = GetXmlData(null, "booking lineitems lineItem[sequence=" + tempids[i] + "]", "xml", null, null, xml);
        temp = ValidateLineItem(lineItem);
        if (temp == false) {
            retVal = temp;
        }
    }
    //Validate Header
    var headerXml = GetXmlData(null, "booking receiptheader", "xml", null, null, xml);
    temp = ValidateReceiptHeader(headerXml);
    if (temp == false) {
        retVal = temp;
    }
    return retVal;
}

//TODO - CHange the function to read values from XML
function ValidateReceiptHeader(headerXml) {
    var errorControl = "#headerErrors";
    if ($(errorControl + " ul") != null) {
        $(errorControl + " ul").remove();
    }
    $(errorControl).append("<ul></ul>");
    errorControl += " ul";
    var retVal = true;
    var customerid = $("#customerSearch").attr("custid");
    if (customerid == "" || customerid == "0" || customerid == null) {
        AddError(errorControl, "Please select a customer.");
        retVal = false;
    }
    if (retVal == true) {
        $(errorControl).remove();
    }
    return retVal;
}
function ValidateLineItem(lineItem) {
    var errorControl = "#lineItemErrors";
    var seq = GetXmlData(null, "lineItem", "attribute", "sequence", null, lineItem);
    if ($(errorControl + " div[sequence=" + seq + "]") != null) {
        $(errorControl + " div[sequence=" + seq + "]").remove();
    }
    $(errorControl).append("<div class='lineItemErrorHeader' sequence='" + seq + "'>Row #" + seq + "</div>");
    errorControl += " div[sequence=" + seq + "]";
    $(errorControl).append("<ul sequence='" + seq + "'></ul>");
    errorControl += " ul[sequence=" + seq + "]";
    var retVal = true;
    //Validate Itemid
    var itemid = GetXmlData(null, "lineItem item", "attribute", "id", null, lineItem);
    if (itemid == "" || itemid == "0") {
        AddError(errorControl, "Please select an Item.");
        retVal = false;
    }
    //Validate Quantity
    var qty = GetXmlData(null, "lineItem item quantity", "nodeText", null, null, lineItem);
    if (qty == null || qty == "") {
        AddError(errorControl, "Please select quantity (greater than 0).");
        retVal = false;
    }
    //Validate Processes
    var tempids = GetXmlData(null, "lineItem processes process", "attribute", "id", "true", lineItem);
    if (tempids == null || tempids == "") {
        //No Process Selected
        AddError(errorControl, "Please select a process.");
        retVal = false;
    }
    else {
        tempids = tempids.split(" ");
        for (var i = 0; i < tempids.length; i++) {
            var rate = GetXmlData(null, "lineItem processes process[id=" + tempids[i] + "] rate", "nodeText", null, null, lineItem);
            //var processText = GetXmlData(null, "lineItem processes process[id=" + tempids[i] + "] text", "nodeText", null, null, lineItem);
            if (rate == "" || rate == null) {
                AddError(errorControl, "Please select rate for the process.");
                retVal = false;
            }
        }
    }
    //CleanupErrorControl
    if (retVal == true) {
        $("#lineItemErrors div[sequence=" + seq + "]").remove();
    }
    return retVal;
}
/////////////////////////////~SaveEntireRecord//////////////////////////

/////////////////////////////Error//////////////////////////
function ShowErrorMessages() {
    var dialog = "#dialog";
    $(dialog).children().remove();
    $(dialog).append("<div class='errorHeader'>Please correct the following errors</div>");
    $(dialog).append($("#allErrors").html());
    //Add tooltip events in the respective controls
    $("#lineItemErrors div").each(function () {
        var seq = $(this).attr("sequence");
        if (seq != "" && seq != null) {
            var errControl = "#bookingTable table tbody tr[sequence=" + seq + "]";
            var errTipSource = this;
            AttachErrorTooltipEvents(errControl, errTipSource);
        }
    });
    if ($("#headerErrors ul").length > 0) {
        //Attach Tooltip event for Customer
        AttachErrorTooltipEvents("label[for=txtSearchCustomer]", "#headerErrors");
    }
}

function AttachErrorTooltipEvents(errControl, errTipSource) {
    var errTooltip = "#errorTooltip";
    $(errControl).addClass("error");
    $(errControl).bind("mouseenter", function (e) {
        $(this).css("cursor", "pointer");
        var tooltipText = $(errTipSource).html();
        $(errTooltip).html(tooltipText);
        $(errTooltip).css("left", e.pageX + 5);
        $(errTooltip).css("top", e.pageY + 5);
        $(errTooltip).show();
        return false;
    });
    $(errControl).bind("mouseleave", function () {
        $(errTooltip).hide();
        return false;
    });
}

function AttacheErrorEvents() {
    $("#mask").bind("click", function () {
        $("#mask").hide();
        $("#dialog").hide();
        $("#mask").unbind("click");
    });
}

function RemoveErrorEvents() {
    $("#mask").unbind("click");
}

function AddError(errorControl, errorMessage) {
    $(errorControl).append("<li>" + errorMessage + "</li>");
}

function ShowProcessingDialog(message) {
    var mask = "#mask";
    //Set mask height/width
    $(mask).width($(document).width());
    $(mask).height($(document).height());
    $(mask).css("left", 0);
    $(mask).css("top", 0);
    //Set Dialog left/top
    var dialog = "#dialog";
    $(dialog).css("left", (($(mask).width() / 2) - $(dialog).width() / 2));
    $(dialog).css("top", (($(mask).height() / 2) - $(dialog).height() / 2));

    $(mask).show();
    ShowLoadingDiv(dialog, null, message);
    $(dialog).show();
}

function HideProcessingDialog() {
    $("#mask").hide();
    $("#dialog").hide();
}

function CleanupErroredControls() {
    //Cleanup error controls
    $("#lineItemErrors").html('');
    $("#headerErrors").html('');
    $("#dialog").html('');
    $("#bookingTable table tbody tr").removeClass("error");
    $("#bookingTable table tbody tr").unbind("mouseenter");
    $("#bookingTable table tbody tr").unbind("mouseleave");

    $("label[for=txtSearchCustomer]").removeClass("error");
    $("label[for=txtSearchCustomer]").unbind("mouseenter");
    $("label[for=txtSearchCustomer]").unbind("mouseleave");
}
/////////////////////////////~Error//////////////////////////

/////////////////////////////EditBooking//////////////////////////
function LoadBooking(data, status, xhr, initFlag, targetDiv, bookingNumber) {
    //check if targetDiv has been already loaded
    if (IsDivLoaded(targetDiv) == false) {
        if (initFlag == "0") {
            ShowProcessingDialog("Loading Booking #- " + bookingNumber);
            var requestBody = '<GetBookingXml xmlns="http://tempuri.org/">' +
                                  '<bookingNumber>' + bookingNumber + '</bookingNumber>' +
                              '</GetBookingXml>';
            var sFn = LoadBooking;
            var eFn = AjaxError;
            setTimeout(function () {
                DoAjaxCall(requestBody, sFn, eFn, "GetBookingXml", "1", targetDiv);
            }, 500);
        }
        else {
            $(targetDiv).children().remove();
            var xmlData = $(data).find("GetBookingXmlResponse").find("GetBookingXmlResult").text();
            //Get Receipt Header
            var headerXml = GetXmlData(null, "booking receiptheader", "xml", null, null, xmlData);
            //Get LineItems
            var lineItemXml = GetXmlData(null, "booking lineitems", "xml", null, null, xmlData);
            //Load LineItems
            var tempids = GetXmlData(null, "booking lineitems lineItem", "attribute", "sequence", "true", xmlData).split(" ");
            for (var i = 0; i < tempids.length; i++) {
                var seq = tempids[i];
                if ($("#lineItems div[sequence=" + seq + "]").length == 0) {
                    //Add New Row Item
                    AddNewRow();
                }
                //Append lineitemXml in the new Div
                $("#lineItems div[sequence=" + seq + "]").html("");
                var lineXml = GetXmlData(null, "booking lineitems lineItem[sequence=" + seq + "]", "xml", null, null, xmlData);
                $("#lineItems div[sequence=" + seq + "]").html(escape(lineXml));
                //Create Table row
                CreateRowFromLineItem(false);
            }
            //Select the first row
            SelectRow(1);
            LoadLineItem(1);
            //Load Header
            LoadReceiptHeader(headerXml);

            MarkDivAsLoaded(targetDiv);
            HideProcessingDialog();
        }
    }
}
/////////////////////////////~EditBooking//////////////////////////