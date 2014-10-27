/// <reference path="jquery-1.6.2.min.js" />
/// <reference path="common.js" />

var sep1 = "~";
var sep2 = ",";
$(document).ready(function () {
    AttachEventsForInputDevices("#leftContainer");
    AttachEventsForKeyboard();
    //Switch to default input mode
    SelectDefaultInputMode();
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
    //Attach Events for Flyouts
    AttachEventsForFlyout();
    //Submit
    AttachEventsForSubmit();
    //Load Items
    LoadItems(null, null, null, "0", "#catItems");
});



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
                $(sourceDiv).children("img[name=" + masterType + "Img]").attr("src"),
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
        if (isNaN($(this).children("input[type=text]").val()) == false && isNaN($("#quantityContainer li.quantityText").text()) == false) {
            amount = parseInt($(this).children("input[type=text]").val())
                        * parseInt($("#quantityContainer li.quantityText").text());
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
    //Select the First line item
    if ($("#lineItems div").length > 0) {
        ResetAll();
        SelectRow("1");
        LoadLineItem($("#bookingTable tbody tr[sequence=1]").attr("sequence"));
    }
}
function SaveLineItem(sequence) {
    if (sequence > 0) {
        var xmlItemNode = SaveItems();
        var xmlPatternsNode = SavePatterns();
        var xmlColorsNode = SaveColors();
        var xmlProcessesNode = SaveProcesses();
        var xmlCategoriesNode = SaveCategories();
        var xmlBrandsNode = SaveBrands();
        var xmlCommentsNode = SaveComments();
        var xmlSubItemsNode = SaveSubItems();
        var xmlVariationsNode = SaveVariations();
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
        mainStr=mainStr.replace(searchStr, replaceStr);
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
        //SetProcessDataAttributes("#processData div[sequence=" + seq + "]", tempids[i], rate, amount, seq);
        //Set Rate
        $("#processDescItems div[processdbid=" + tempids[i] + "] input:text").val(rate);
        //seq++;
    }
}
function ResetAll() {
    ResetItems();
    ResetItemDetails();
    ResetColorsPatterns();
    ResetProcesses();
    ResetBrands();
    ResetComments();
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
    ShowTotals();
}
function ShowTotals() {
    var total = 0;
    $("#bookingTable table tbody tr td:nth-child(5)").each(function () {
        if (!isNaN($(this).children("span").text())) {
            total += parseInt($(this).children("span").text());
        }
    });
    $("#txtGross").val(total);
    if ($("#txtDiscount").val() != "" && !isNaN($("#txtDiscount").val())) {
        total -= parseInt($("#txtDiscount").val());
    }
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
                $(this).attr("value", "1");
            }
        });
    });
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
        $("#inputDeviceContainer").show();
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

/////////////////////////////SaveEntireRecord//////////////////////////
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
function SubmitBooking() {
    //Create XML
    //Get all lineitems
    var lineitemsXml = escape("<lineitems>");
    $("#lineItems").children("div[rel=lineItem]").each(function () {
        lineitemsXml += $(this).html();
    });
    lineitemsXml += unescape("</lineitems>");
    var bookingXml = escape("<booking>") + escape(SaveReceiptHeader()) + lineitemsXml + escape("</booking>");
    alert(unescape(bookingXml));
}
/////////////////////////////~SaveEntireRecord//////////////////////////

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
/////////////////////////////~Customer//////////////////////////

/////////////////////////////Load Defaults//////////////////////////

/////////////////////////////~Load Defaults//////////////////////////

