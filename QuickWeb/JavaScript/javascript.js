function checkAll(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");

    for (var i = 0; i < inputList.length; i++) {
        //Get the Cell To find out ColumnIndex
        var row = inputList[i].parentNode.parentNode;

        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                //If the header checkbox is checked
                //check all checkboxes
                //and highlight all rows
                inputList[i].checked = true;
            }
            else {
                //If the header checkbox is checked
                //uncheck all checkboxes
                //and change rowcolor back to original
                inputList[i].checked = false;
            }
        }
    }
}
function Check_Click(objRef) {
    //Get the Row based on checkbox
    var row = objRef.parentNode.parentNode;

    //Get the reference of GridView
    var GridView = row.parentNode;
    //Get all input elements in Gridview
    var inputList = GridView.getElementsByTagName("input");

    for (var i = 0; i < inputList.length; i++) {
        //The First element is the Header Checkbox
        var headerCheckBox = inputList[0];
        //Based on all or none checkboxes
        //are checked check/uncheck Header Checkbox
        var checked = true;

        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
            if (!inputList[i].checked) {
                checked = false;
                break;
            }
        }
    }
    headerCheckBox.checked = checked;
}
function checkName(Image, Text, msg, msg1) {
    try {
        if (Image.value.length > 0) {
            if (Image.value != '') {
                var split = Image.value.split('.');
                if (split[1] != "png" || split[1] != "PNG")
                    alert(msg);
            }
            Image.focus();
            return false;
        }
    }
    catch (Error) {
    }
    if (Text.value == "" || Text.value.trim().length == 0) {
        alert(msg1);
        Text.focus();
        return false;
    }
}