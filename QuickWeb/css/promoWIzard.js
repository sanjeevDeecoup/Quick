

var stepIndex = 0;
function InitializeSteps(stepids, container, templateContainer, schemeXml) {
    if (container == null) { container = "#mainContainer"; }
    if (templateContainer == null) { templateContainer = "#templateContainer"; }
    //Get All Steps
    var i;
    var nextprev = "nextprev";
    var seqNum = 0;
    var last = false;
    if (stepids != null) {
        var steps = stepids.split(" ");
        if (steps.length > 0) {
            for (i = 0; i < steps.length; i++) {
                if ($(templateContainer + ' fieldset[name=' + steps[i] + ']') != null) {
                    if (stepIndex == 0) {
                        nextprev = "next";
                    }
                    else if (stepIndex > 2 && i == steps.length - 1) {
                        nextprev = "prev"; last = true;
                    }
                    seqNum++;
                    InitializeStep(steps[i], container, stepIndex, templateContainer, nextprev, schemeXml, seqNum, last);
                    stepIndex++;
                }
                else { alert(steps[i] + " not found"); }
            }
        }
        else {

        }
    }
}

function InitializeStep(stepid, container, index, templateContainer, nextprev, schemeXml, seqNum, lastStep) {
    
    var stepName = "step" + index;
    $(container).append('<div id ="' + stepName + '"></div>');
    $("#" + stepName).append($(templateContainer + ' fieldset[name=' + stepid + ']').clone());
    $('#commands').append('<p id="stepCommands' + index + '"></p>');
    $('#' + stepName).hide();
    //Add command buttons
    if (nextprev == "next" || nextprev == "nextprev") {
        createNextButton(stepIndex, container, templateContainer);
    }
    if (nextprev == "prev" || nextprev == "nextprev") {
        createPrevButton(index);
    }
    if (lastStep == true) {
        createFinishButton(index);
    }
    if (stepIndex == 0) { ShowStep(index, null); }
    
    //Load HelpText, Legend, ErrorText if schemeXml is not null
    if (schemeXml != null) {
        InitializeStepHeaders(index, stepid, schemeXml, seqNum);
    }
}

function InitializeStepHeaders(index, stepid, schemeXml, seqNum) {
    var tempDiv = "#step" + index + " fieldset[name=" + stepid + "] div";
    //Add Sequence Number
    if ($("#step" + index + " fieldset[name=" + stepid + "]").attr("sequence") == "0") {
        $("#step" + index + " fieldset[name=" + stepid + "]").attr("sequence", seqNum);
    }
    //Add Validation attribute
    var validation = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "]", "attribute", "validation", null, schemeXml);
    if (validation == "0") {
        $("#step" + index + " fieldset[name=" + stepid + "]").attr("validation", validation);
    }
    //Load Legend
    $(tempDiv + ".formLegend span").html(
        GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] legend" , "nodeText", null, null, schemeXml)
    );
    //Load HelpText
    $(tempDiv + ".formHelpText p").html(
        GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] help", "nodeText", null, null, schemeXml)
    );
    //Load ErrorText
    $(tempDiv + ".formError").html(
        GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] error", "nodeText", null, null, schemeXml)
    );
    //Load Contents
    //captions
    LoadContents(tempDiv, index, stepid, schemeXml, seqNum, "captions");
    //showHide
    LoadContents(tempDiv, index, stepid, schemeXml, seqNum, "showHide");
    //f2 - Suffix the group name of radio buttons with the sequence numbers
    if (stepid == "f2") {
        var newName = $(tempDiv + " input:radio").attr("name") + seqNum.toString();
        $(tempDiv + " input:radio").attr("name", newName);
    }
}

function LoadContents(tempdiv, index, stepid, schemeXml, seqNum, operationType) {
    var i;
    var tempSel = "steps " + stepid + "[sequence=" + seqNum + "] content " + operationType + " control";
    var tempids = GetXmlData(null, tempSel, "attribute", "seqid", "true", schemeXml);
    if (tempids != "") {
        tempids = tempids.split(" ");
        for (i = 0; i < tempids.length; i++) {
            var sel = GetXmlData(null, tempSel + "[seqid=" + tempids[i] + "] controlSelector", "nodeText", null, null, schemeXml);
            var selVal = GetXmlData(null, tempSel + "[seqid=" + tempids[i] + "] controlValue", "nodeText", null, null, schemeXml);
            switch (operationType) {
                case "captions":
                    $("#step" + index + " " + sel).text(selVal);
                    break;
                case "showHide":
                    if (selVal == "show") { //Do Nothing 
                    }
                    else if (selVal == "hide") {
                        $("#step" + index + " " + sel).hide();
                    }
                    break;
                default:
                    break;
            }
        } 
    }
}

function createFinishButton(index) {
    $('#stepCommands' + index).append('<a href="#" id="stepCommands' + index + 'Finish" class="next">Finish</a>');

    //Attach Handler
    $("#stepCommands" + index + "Finish").bind("click", function (e) {
        if (ValidateStep(index)) {
            $(this).unbind("click");
            SaveData(index);
            SubmitData();
        }
    });
}

function createPrevButton(index) {
    $('#stepCommands' + index).append('<a href="#" id="stepCommands' + index + 'Prev" class="prev">< Back</a>');

    //Attach Handler
    $("#stepCommands" + index + "Prev").bind("click", function (e) {
        ShowStep(index - 1, index);
    });
}
function createNextButton(index, container, templateContainer) {
    $('#stepCommands' + index).append("<a href='#' id='stepCommands" + index + "Next' class='next' nextStepsLoaded='0'>Next ></a>");

    //Attach handler
    $("#stepCommands" + index + "Next").bind("click", function (e) {
        if (ValidateStep(index) == true) {
            SaveData(index);
            var formContent = "#step" + index + " div.formContent";
            var nextFrom = $(formContent).attr('nextFrom');
            if (nextFrom != null && nextFrom != "") {
                NextClickDynamic(index, container, templateContainer);
            }
            else {
                NextClickGeneric(index, container, templateContainer);
            }
        }
    });
}

function NextClickGeneric(index, container, templateContainer) {
    ShowStep(index + 1, index);
}

function NextClickDynamic(index, container, templateContainer) {
    var formContent = "#step" + index + " div.formContent";
    var nextFrom = $(formContent).attr('nextFrom');
    var nextFromAttr = $(formContent).attr('nextFromAttr');
    if (nextFromAttr == null && nextFromAttr == "") {
        nextFromAttr = "next"; 
    }
    var selectedValue = $(formContent).attr('selectedValue');
    if (selectedValue == null && selectedValue == "") {
        selectedValue = "1"; 
    }
    var selectedValueAttr = $(formContent).attr('selectedValueAttr');
    if (selectedValueAttr == null && selectedValueAttr == "") {
        selectedValueAttr = "value"; 
    }
    if ((nextFrom != "" || nextFrom != null) && (nextFromAttr != "" || nextFromAttr != null)) {
        //Get ID of next step(s)
        var selDiv = "'" + nextFrom + "[" + selectedValueAttr + "=" + selectedValue + "]'";
        var nextids = $(formContent).find(selDiv).attr(nextFromAttr);
        var contents = $(formContent).find(selDiv).html();
        var schemeXml = $(formContent).find(selDiv).parent().children("div[name=data]").html();
        if (nextids != null) {
            var nextStepsLoaded = $("#stepCommands" + index + "Next").attr('nextStepsLoaded');
            if (nextStepsLoaded != "0" && nextStepsLoaded != contents) {
                //Remove all next steps
                var k = 0;
                for (j = index + 1; j <= stepIndex; j++) {
                    $("#step" + j).remove();
                    $("#stepCommands" + j).remove();
                    k++;
                }
                stepIndex = stepIndex - k + 1;
            }
            if (nextStepsLoaded != contents) {
                InitializeSteps(nextids, container, templateContainer, schemeXml);
                $("#stepCommands" + index + "Next").attr('nextStepsLoaded', contents);
            }
        }
    }
    ShowStep(index + 1, index);
}

function ShowStep(showIndex, hideIndex) {
    //setTimeout(function () { LoadStep(showIndex); }, 100);

    if (hideIndex != null) {
        $("#step" + hideIndex).hide();
    }
    $("#commands p").removeClass('currentSel');
    $("#step" + showIndex).fadeIn(500);
    $("#stepCommands" + showIndex).addClass('currentSel');
    LoadStep(showIndex);
}

function LoadStep(index) {
    var stepid = $("#step" + index + " fieldset").attr('name');
    switch (stepid) {
        case 'f0':
            AttachMouseEvents("div#step" + index + " div[rel=promoIntro] div.largeButton");
            break;
        case 'f1':
            Loadf1(null, null, null, "0", index, "rel=schemeTemplates");
            break;
        case "f2":
            Loadf2("div#step" + index + " div[rel=quantity] div[rel=numPad]", "div#step" + index + " input[name=txtClothQty]", index);
            break;
        case "f3":
            Loadf3(null, null, null, "0", index, "rel=process");
            break;
        case "f4":
            Loadf4(null, null, null, "0", index, "rel=allItemsMain");
            break;
        case "f5":
            Loadf5(GetStepsXml(), index);
            break;
        case "f6":
            Loadf6(index);
            break;
        case "f7":
            Loadf7(null, null, null, "0", index, "rel=promos");
            break;
        default:
            break;
    }

}

function AttachMouseEvents(div) {
    $(div).each(function (i) {
        $(this).click(function () {
            //Init All
            $(div).attr('value', '0');
            $(div).removeClass('buttonClicked');
            //change cur divs value attribute's to 1 
            $(this).attr('value', '1');
            $(this).addClass('buttonClicked');
        });
    });
}

function ValidateStep(index) {
    var stepid = $("#step" + index + " fieldset").attr('name');
    var validation = $("#step" + index + " fieldset").attr('validation');
    var showError = false;
    if (validation == "1") {
        switch (stepid) {
            case 'f0':
                if ($("#step" + index + " div[rel=promoIntro] div[value=1]").length == 0) {
                    showError = true;
                }
                break;
            case 'f1':
                if ($("#step" + index + " div[rel=schemeTemplates] div[value=1]").length == 0) {
                    showError = true;
                }
                break;
            case "f2":
                var temp = $("#step" + index + " input[name=txtClothQty]").val();
                if (temp == null || temp == "") {
                    showError = true;
                }
                break;
            case "f3":
                if ($("#step" + index + " div[rel=process] div[value=1]").length == 0) {
                    showError = true;
                }
                break;
            case "f4":
                if ($("#step" + index + " div[rel=selectedItemsMain] div").length == 0) {
                    showError = true;
                }
                break;
            case "f5":
                var temp = $("#step" + index + " input[name=txtSchemeName]").val();
                if (temp == null || temp == "") {
                    showError = true;
                }
                break;
            case "f6":
                var temp = $("#step" + index + " input:radio[name=validity]:checked").val();
                if (temp != "1" && temp != "2") {
                    showError = true;
                }
                break;
            case "f7":
                var divName = "#step" + index + " div[rel=promos] div[value=1]";
                if ($(divName).length != 1) {
                    showError = true;
                }
                break;
            default:
                break;
        }
        if (showError) {
            alert($("div#step" + index + " div.formError").text());
        }
    }
    return !showError;

}

function SaveData(index) {
    if (index > 1) {
        var stepsxml = GetStepsXml();
        var fName = "#step" + index + " fieldset";
        var stepid = $(fName).attr('name');
        var seqNum = $(fName).attr('sequence');
        var dynamicData = null;
        switch (stepid) {
            case "f2": case "f6":case "f5":
                //Get selector to set
                var sel; var controlValue;
                var tempstr = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata", "attribute", "seqid", "true", stepsxml).split(" ");
                for (i = 0; i < tempstr.length; i++) {
                    sel = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata[seqid=" + tempstr[i] + "] uiselector", "nodeText", null, null, stepsxml);
                    controlValue = $(fName + " " + sel).val();
                    stepsxml = SetXmlData(stepsxml, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata[seqid=" + tempstr[i] + "] uivalue", controlValue);
                }
                //Create Summary Dynamic Data
                dynamicData = GetDynamicData(index, stepid);
                if (dynamicData != null) {
                    stepsxml = SetXmlData(stepsxml, "steps " + stepid + "[sequence=" + seqNum + "] summarydata dynamicdata", dynamicData);
                }
                SetStepsXml(stepsxml);
                break;
            case "f3":case "f4":
                var sel = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata uiselector", "nodeText", null, null, stepsxml);
                //for ui data
                var controlValue = $(fName + " " + sel).map(function () {
                    return $(this).attr("dbid");
                }).get().join(" ");
                stepsxml = SetXmlData(stepsxml, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata uivalue", controlValue);

                //for dynamic data
                dynamicData = GetDynamicData(index, stepid, sel);
                stepsxml = SetXmlData(stepsxml, "steps " + stepid + "[sequence=" + seqNum + "] summarydata dynamicdata", dynamicData);
                SetStepsXml(stepsxml);
                break;
            default:
                break;
        }
    } 
}

function GetDynamicData(index, stepid, selText) {
    var retVal = null;
    switch (stepid) {
        case "f2":
            retVal = GetDynamicDataForf2(index);
            break;
        case "f6":
            retVal = GetDynamicDataForf6(index);
            break;
        case "f3":
            retVal = GetDynamicDataForf3f4(index, stepid, selText, "", "all processes");
            break;
        case "f4":
            retVal = GetDynamicDataForf3f4(index, stepid, selText, "", "item(s) ");
            break;
        default:
            break;
    }
    return retVal;
}

function GetDynamicDataForf3f4(index, stepid, selText, anyText, allText) {
    var fName = "#step" + index + " fieldset";
    var controlValue = $(fName + " " + selText + " span[name!=code]").map(function () {
        return $(this).html();
    }).get().join(";");
    //Check if none selected
    if ((controlValue == "") && ($(fName).attr("validation") == "0")) {
        controlValue = anyText;
    }
    else if (controlValue != "") {
        controlValue = "{" + controlValue + "}";
    }
    //Check if all selected
    if (stepid == "f3") {
        var sel = selText.split(" ");
        if ($(sel[0] + " div").length == $(selText).length) {
            controlValue = "{" + allText + "}";
        }
    }
    else if (stepid == "f4") {
        if ($(fName + " div[rel=allItemsMain]").children().length == 0) {
            controlValue = allText;
        }
    }
    return controlValue;
}

function GetDynamicDataForf2(index) {
    var retVal = null;
    var div = "#step" + index;
    var amount = $(div + " input:text[name=txtClothQty]").val();
    var seq = $(div + " fieldset").attr("sequence");
    if ($(div + " input:radio[name=unit" + seq + "]:checked").val() == "1") {
        retVal = "Rs. " + amount + " ";
    }
    else if ($(div + " input:radio[name=unit" + seq + "]:checked").val() == "2") {
        retVal = amount + "% ";
    }
    else {
        retVal = amount;
    }
    return retVal;
}

function GetDynamicDataForf6(index) {
    var retVal = null;
    var div = "#step" + index;
    if ($(div + " input:radio[name=validity]:checked").val() == "1") {
        retVal = "for " + $(div + " input:text[name=txtApplicableFor]").val() + " ";
        retVal += $(div + " select[name=selectApplicableForTenure] option:selected").text() + " ";
        retVal += "from date of enrollment.";
    }
    else if ($(div + " input:radio[name=validity]:checked").val() == "2") {
        retVal = "between " + $(div + " input:text[name=txtDateFrom]").val();
        retVal += " and " + $(div + " input:text[name=txtDateTo]").val();
    }
    return retVal;
}

function GetSelectorDivForStepsXml(fName) {
    if ($(fName) != null) {
        var formContent = fName + " div.formContent";
        var nextFrom = $(formContent).attr('nextFrom');
        var selectedValue = $(formContent).attr('selectedValue');
        var selectedValueAttr = $(formContent).attr('selectedValueAttr');

        var selDiv = "'" + nextFrom + "[" + selectedValueAttr + "=" + selectedValue + "]'";
        return selDiv;
    }
}

function GetStepsXml() {
    var fName = "div#step1 fieldset";
    if ($(fName) != null) {
        var formContent = fName + " div.formContent";
        var selDiv = GetSelectorDivForStepsXml(fName);
        var schemeXml = $(formContent).find(selDiv).parent().children("div[name=data]").html();

        return schemeXml;
    }
}

function SetStepsXml(xml) {
    var fName = "div#step1 fieldset";
    if ($(fName) != null) {
        var formContent = fName + " div.formContent";
        var selDiv = GetSelectorDivForStepsXml(fName);
        $(formContent).find(selDiv).parent().children("div[name=data]").html('');
        $(formContent).find(selDiv).parent().children("div[name=data]").html(escape(xml));
        
    }
}

function LoadSavedData(index) {
    if (index > 1) {
        var stepsxml = GetStepsXml();
        var fName = "#step" + index + " fieldset";
        var stepid = $(fName).attr('name');
        var seqNum = $(fName).attr('sequence');
        switch (stepid) {
            case "f2": case "f6":case "f5":
                //Get selector value to load
                var sel; var controlValue;
                var tempstr = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata", "attribute", "seqid", "true", stepsxml).split(" ");
                for (var i = 0; i < tempstr.length; i++) {
                    sel = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata[seqid=" + tempstr[i] + "] uiselector", "nodeText", null, null, stepsxml);
                    controlValue = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata[seqid=" + tempstr[i] + "] uivalue", "nodeText", null, null, stepsxml);
                    if (sel.indexOf(":radio") >= 0) {
                        sel = sel.replace(":checked", "");
                        //radio
                        $(fName + " " + sel).filter("[value=" + controlValue + "]").attr("checked", true);
                    }
                    else {
                        $(fName + " " + sel).val(controlValue);
                    }
                }
                break;
            case "f3":case "f4":
                var sel = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata uiselector", "nodeText", null, null, stepsxml).split(" ");
                var controlValue = GetXmlData(null, "steps " + stepid + "[sequence=" + seqNum + "] filldata uidata uivalue", "nodeText", null, null, stepsxml);
                var tempstr = controlValue.split(" ");
                var type = "process";
                var selDiv = sel[0];
                if (stepid == "f4") {
                    type = "item";
                    selDiv = "div[rel=allItemsMain]";
                }
                for (var i = 0; i < tempstr.length; i++) {
                    $(fName + " " + selDiv + " div[dbid=" + tempstr[i] + "]").trigger("click");
                }
                if (stepid == "f4") {
                    $(fName + " input:button[name=btnSelect]").trigger("click");
                }
                break;
            default:
                break;
        }
    }
}