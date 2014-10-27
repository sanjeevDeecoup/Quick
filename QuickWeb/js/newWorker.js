onmessage = function (e) {
    try {
        var dataLength = e.data.length;
        switch (e.data[0]) {
            case 'NewChallanSave': MakeTheArrayToSave(e.data[1]);
                break;
            default: postMessage('could not identify mesaage ' + e.data);
                break;
        }
    }
    catch (exception) {
        postMessage(exception);
        postMessage('Some error ccured during processing!');
    }
};


function MakeTheArrayToSave(dump) {
    /**** WARNING! REGEX AHEAD ****/
    /**** DON'T F*CK WITH THIS UNLESS YOU REALLY KNOW WHAT YOU ARE DOING! ****/
    var allRowsString = dump.replace(/\n/gi, '').replace(/<table>/gi, '').replace(/<\/table>/gi, '').replace(/<tbody>/gi, '').replace(/<\/tbody>/gi, '').replace(/<\/tr>/gi, '');
    var allRowsArray = allRowsString.split(/<tr>/);
    var allRowsLength = allRowsArray.length;
    var _rowData = '', _curRow = '';
    var _curRowAry;
    //postMessage(allRowsString);
    //postMessage(allRowsArray);
    //postMessage(allRowsLength);
    for (var i = 0; i< allRowsLength; i++) {
        if (i === 0) {
            continue;
        }
        _curRow = allRowsArray[i].replace(/<\/td>/gi, '');
        _curRowAry = _curRow.split(/<td.*?>/);
        // booking number
        _rowData += _curRowAry[3] + ':';
        // barcode
        _rowData += _curRowAry[5].split('-')[1] + ':';
        // Item
        var startIdx = _curRowAry[7].lastIndexOf('">');
        var lastIx = _curRowAry[7].lastIndexOf('</span>');
        _rowData += _curRowAry[7].substr(startIdx + 2, lastIx - startIdx - 2) + ':';
        // qty
        _rowData += '1' + ':';
        //urgent
        startIdx = _curRowAry[8].lastIndexOf('">');
        lastIx = _curRowAry[8].lastIndexOf('</span>');
        _rowData += _curRowAry[8].substr(startIdx + 2, lastIx - startIdx - 2) + ':';
        _rowData += '_';
        //postMessage('row data at iteration ' + i);
        //postMessage('cur row is');
        //postMessage(_curRowAry);
    };
    _rowData = _rowData.substr(0, _rowData.length - 1);
    //postMessage('final row data');
    postMessage(_rowData);

    /*
    var _rowData = '';
    var _grdSize = $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr').size();
    for (var _i = 1; _i < _grdSize; _i++) {
        // first the booking number
        _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(2)').text() + ':';
        // now the item serial number
        _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(4)').text().split('-')[1] + ':';
        // subItem name
        _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(6) > span').text() + ':';
        // now the qty, hard coding 1
        _rowData += '1' + ':';
        // now the urgent
        _rowData += $('#ctl00_ContentPlaceHolder1_grdSelectedCloth > tbody > tr:eq(' + _i + ') > td:eq(7) > span').text() + ':';
        // now add a '_' to seperate rows
        _rowData += '_';
    }
    _rowData = _rowData.substr(0, _rowData.length - 1);
    $('#hdnAllData').val(_rowData);
    return true;
    */
    //postMessage(dump);
}


