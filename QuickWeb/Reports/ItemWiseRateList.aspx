<%@ Page Language="C#" AutoEventWireup="true" Inherits="Reports_ItemWiseRateList"
    CodeBehind="ItemWiseRateList.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%Response.Write(ConfigurationManager.AppSettings["AppTitle"]); %></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />
                    <asp:Button ID="btncancel" runat="server" Text="Close" OnClick="btncancel_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save ( F8 )" OnClick="BtnSaveClick"
                        ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRateListName" runat="server" CssClass="Title2" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grdDetails" runat="server" CssClass="mGrid mGirdAlternate" OnRowEditing="GrdDetailsRowEdit" EnableTheming="false"
                        DataKeyNames="ItemName, ItemCode" ClientIDMode="Static">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnProcesses" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnItems" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnRates" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnForEdit" />
    </form>
    <script type="text/javascript">

        $(function () {

            setInputs('grdDetails', 1);

            $('[type="text"]').focus(function (e) {
                $(this).select();
                return false;
            }).focusout(checkInput);

            $('#btnSave, #btncancel').click(function (e) {

                $('[type="text"]').off('focusout');

                var 
                    isAnyInputInvalid,
                    invalidInput;

                isAnyInputInvalid = $('[type="text"]').get().some(function (v, i) {
                    return (invalidInput = v) && ((v.value.trim() === '') || (isNaN(Number(v.value))))
                });

                if (isAnyInputInvalid) {
                    $(invalidInput).css('background-color', 'yellow');
                    invalidInput.focus();
                    $('[type="text"]').on('focusout', checkInput);
                    return false;
                }

                if (e.target.id === 'btnSave') {
                    makeRates();
                }

            });

            $('body').on('keydown.Attached', function (e) {
                if (e.which !== 119) return;
                $('#btnSave').click();
            });

            function checkInput(e) {
                if (isNaN(Number(e.target.value)) || e.target.value === '') {
                    $(e.target).css('background-color', 'yellow');
                    setTimeout(function () {
                        e.target.focus();
                    }, 10);
                }
                else {
                    $(this).css('background-color', '');
                }
                return false;
            }

        });

        function setInputs(gridId, columnToStartFrom) {

            if (!$('#hdnForEdit').val()) {
                return;
            }

            var input = $('<input type="text" maxlength="6" />');
            $('#' + gridId + ' tr').not(':eq(0)').find('td:gt(' + columnToStartFrom + ')').each(function (i, v) {
                input.val(v.textContent);
                v.textContent = '';
                $(v).append(input.clone());
            });
        }

        function makeRates() {
            var items = '';
            var itemRates = [];

            $('#grdDetails tr').not(':eq(0)').each(function (i, v) {
                items = '';
                $(v).find('td').each(function (ii, vv) {
                    if (ii === 0) {
                        items += $(vv).text();
                    } else if (ii === 1) {
                        return;
                    } else {
                        items += ':' + $(vv).find('input').val();
                    }
                });
                itemRates.push(items);
            });

            hdnRates.value = itemRates;
        }

    </script>
</body>
</html>
