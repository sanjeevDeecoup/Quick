function RefreshDataGrid() {
    $get('btnNewBooking').click();
}
function NewExpanseOkay() {
    $get('btnNewBooking').click();
}
function clickButton(e, buttonid) {
    var evt = e ? e : window.event;
    var bt = document.getElementById(buttonid);
    if (bt) {
        if (evt.keyCode == 13) {
            bt.click();
            return false;
        }
    }
}
function checkKey(e) {
    var targ;
    var code;
    if (!e) var e = window.event;
    if (e.target) targ = e.target;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    else if (e.srcElement) targ = e.srcElement;
    if (code == 13) {
        return false;
    }
}
function CtlOnFocus(ctl) {
    ctl.style.backgroundColor = '#ff7777';
}
function CtlOnBlur(ctl, type) {
    if (ctl.type == 'text' || ctl.type == 'image' || ctl.type == 'image') {
        ctl.style.backgroundColor = '#6086ac';
    }
    else if (ctl.type == 'submit') {
        ctl.style.backgroundColor = '#6086ac';
    }
}