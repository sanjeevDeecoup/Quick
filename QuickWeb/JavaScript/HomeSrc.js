
function showAllCustomer(ControlID) {
    $.ajax({
        url: '../AutoComplete.asmx/GetDetailofCustomer',
        type: 'GET',
        timeout: 20000,
        contentType: 'application/json; charset=UTF-8',
        datatype: 'JSON',
        cache: true,
        async: false,
        success: function (response) {
            var strData = response.d;         
            showCustomer(strData, ControlID);
        },
        error: function (response) {

        }
    });
}
function showCustomer(strData, ControlID) {
  
    var stsCustAllData = strData.split(":");
    var Alldata = [];
    for (var j = 0; j < stsCustAllData.length; j += 1) {
        Alldata.push(stsCustAllData[j]);
    }
    $(ControlID).typeahead({
        source: Alldata,
        highlighter: function (item) {
            var parts = item.split('#'),
                        html = '<div class="typeahead suggestion">';
            html += '<div class="pull-left">';
            html += '<div class="text-left"><table style="width:' + $('#hdnControlWidth').val() + 'px"><tr><td><strong>' + parts[1] + '</strong></td><td class="text-right d1">' + parts[3] + '</td></tr></table></div>';
            html += '<div class="text-left"><table style="width:' + $('#hdnControlWidth').val() + 'px"><tr><td class="d1">' + parts[2] + '</td><td class="text-right d1">' + parts[4] + '</td></tr></table></div>';         
            html += '</div>';
            html += '<div class="clearfix"></div>';
            html += '</div>';
            return html;
        },
        updater: function (item) {
            var parts = item.split('#');
            return (parts[0] +"-"+ parts[1]);
        }
    });
}