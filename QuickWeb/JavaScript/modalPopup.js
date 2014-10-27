function pageLoad(sender, args) {
    if (!args.get_isPartialLoad()) {
        $addHandler(document, "keydown", onKeyDown);
    }
}
function onKeyDown(e) {
    if (e && e.keyCode == Sys.UI.Key.esc) {
        if (window.parent.document.getElementById('hdnOption').value == 0) {
            window.parent.document.getElementById('grdEntry_ctl01_txtRate').focus();
            window.parent.document.getElementById('grdEntry_ctl01_txtRate').select();
        }
        if (window.parent.document.getElementById('hdnOption').value == 2) {
            window.parent.document.getElementById('grdEntry_ctl01_txtName').focus();
            window.parent.document.getElementById('grdEntry_ctl01_txtName').select();
        }
        if (window.parent.document.getElementById('hdnOption').value == 3) {
            window.parent.document.getElementById('grdEntry_ctl01_txtRate').focus();
            window.parent.document.getElementById('grdEntry_ctl01_txtRate').select();
        }
        if (window.parent.document.getElementById('hdnOption').value == 4) {
            window.parent.document.getElementById('txtCustomerName').focus();
            window.parent.document.getElementById('txtCustomerName').select();
        }
        if (window.parent.document.getElementById('hdnOption').value == 5) {
            window.parent.document.getElementById('txtCustomerName').focus();
            window.parent.document.getElementById('txtCustomerName').select();
        }
        if (window.parent.document.getElementById('hdnOption').value == 6) {
            window.parent.document.getElementById('grdEntry_ctl01_txtQty').focus();
            window.parent.document.getElementById('grdEntry_ctl01_txtQty').select();
        }
        if (window.parent.document.getElementById('check').value == 1) {
            window.parent.document.getElementById('grdEntry_ctl01_btnColors').focus();
            window.parent.document.getElementById('grdEntry_ctl01_txtProcess').select();
        }

        $find('ModalPopupExtender4').hide();
        $find('ModalPopupExtender5').hide();
        $find('ModalPopupExtender7').hide();
        $find('ModalPopupExtender8').hide();
        $find('ModalPopupExtender1').hide();
        $find('Remarks_ModalPopup').hide();
    }
}                