<%@ Page Language="C#" AutoEventWireup="true" Inherits="New_Booking_frm_New_Booking"
    CodeBehind="OldBooking.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="Shortcut Icon" type="image/ico" href="../images/DRY.jpg" />
    <title>
        <%=AppTitle %></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/code.js" type="text/javascript"></script>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript" src="../js/tag-it.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.23.custom.css" />
    <link href="../css/jquery.tagit.css" rel="stylesheet" type="text/css" />
    <link href="../css/tagit.ui-zendesk.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function SetColor(colorName) {
            var count = document.getElementById('grdEntry_ctl01_txtColor').value;
            // alert(document.getElementById('grdEntry_ctl01_txtColor').value);
            if (count != '') {
                document.getElementById('grdEntry_ctl01_txtColor').value = document.getElementById('grdEntry_ctl01_txtColor').value + ', ' + colorName;
                document.getElementById('grdEntry_ctl01_txtColor').select();
            }
            else {
                document.getElementById('grdEntry_ctl01_txtColor').value = colorName;
                document.getElementById('grdEntry_ctl01_txtColor').select();
            }
        }

        function toggleDropDownList(source) {
            document.getElementById('drpsmstemplate').disabled = !source.checked;
        }
        function Select(ControlID) {
            alert("hii");
            var control = document.getElementById(ControlID);
            
            control.select();
        }
        function addAcc(a, b) {
            var btn = document.getElementById(a);
            alert(a);
            alert(b);
            alert(btn);
        };


        //        function SetLabels() {
        //            alert("working");
        //            alert($('tr').size());
        //            alert($(this));
        //            
        //            alert($(this).val());
        //            alert($(this).index());
        //            alert($(this).html());
        //            alert($(this).closest('tr'));
        //            alert($(this).closest('tr').size());
        //            alert($(this).closest('tr').find('td').size());
        //            alert($(this).closest('tr > td').eq(4).text());
        //            alert("done");
        //            
        //            alert("third");
        //        }



    </script>
</head>
<body onkeydown="if(event.keyCode==119){document.getElementById('btnSaveBooking').click();}else if(event.keyCode==120){document.getElementById('btnSavePrint').click();}else if(event.keyCode==121){document.getElementById('btnSavePrintBarCode').click();}else if(event.keyCode==112){document.getElementById('btnNewBooking').click();}else if(event.keyCode==123){document.getElementById('btnReset').click();}else{if(event.keyCode==115){document.getElementById('btnEditBooking').click();}}">
    <form id="form1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var newList = $('#<%=hdnValues.ClientID %>').text().split(',');
            var newList2 = $.makeArray(newList);

            if ($('#hdnItems').val() != "") {
                $('#mySingleField').val($('#hdnItems').val());
            }

            var theTags = $('#mytags');
            //debugger;
            $('#mytags').tagit({
                availableTags: newList2,
                singleField: true,
                singleFieldNode: $('#mySingleField'),
                allowSpaces: true,
                allowCustom: true
            });


            $('#grdEntry_ctl01_imgBtnGridEntry').click(function () {
                $('#grdEntry_ctl01_txtRemarks').val('' + $('#mySingleField').val() + '')
            });

            $('#imgBtnEdit').click(function () {
                //alert("hey");
                alert($(this).closest('tr > td').eq(4).text());
                alert($(this).closest('tr > td').eq(4).val());

            });

            $('body').delegate('[Data="value"]', 'click', function () {
                //alert("hey");
                var list = '' + $(this).attr("id") + '';
                var item = list.split('_');
                var itemMain = '' + item[1] + '';
                var LastItem = itemMain.substring(itemMain.length - 1, itemMain);
                var otherItem = itemMain.substr(itemMain.length - 1, itemMain.length)
                var valueToSet = $('body').find('#grdEntry > tbody > tr:eq(' + (otherItem - 1) + ') > td:eq(4) > table > tbody > tr:eq(0) > td  > span').text();
                // alert(valueToSet);
                $('#mySingleField').text(valueToSet);
                $('#mySingleField').val(valueToSet);
                //hdnItems
                $('#hdnItems').text(valueToSet);
                $('#hdnItems').val(valueToSet);
                //                $('#mySingleField').html($('#grdEntry_ctl01_txtRemarks'));
            });


            //            // previous code
            //            $('.btnsavebookingspan').click(function (event) {
            //                var _btnToClick = $(this).closest('td').find('input').attr('id');
            //                $('#' + _btnToClick + '').click();
            //            });

            // this will open up the delivery screen
            $('body').keydown(function (event) {
                if (event.which == 117) {
                    window.location = "../Bookings/Delivery.aspx";
                }
            });

            // on the click too
            $('#btnDelivery').click(function (event) {
                window.location = "../Bookings/Delivery.aspx";
                return false;
            });

            // on ctrl + f12
            $('body').keydown(function (event) {
                if (event.which == 123) {
                    $('#btnPrintBarCode').click();
                }
            });

            //1. on click of the image and what not click the button
            $('#save').one('click.AttachedEvent', function (event) {
                $('#btnSaveBooking').click();
            });

            //            $('#save').click(function (event) {
            //                $('#btnSaveBooking').click();
            //                event.preventDefault();
            //            });

            //2. on click of the image and what not click the button
            $('#print').one('click.AttachedEvent', function (event) {
                $('#btnSavePrint').click();
            });

            //3. on click of the image and what not click the button
            $('#printtag').one('click.AttachedEvent', function (event) {
                $('#btnPrintBarCode').click();
            });

            //4. on click of the image and what not click the button
            $('#saveprinttag').one('click.AttachedEvent', function (event) {
                $('#btnSavePrintBarCode').click();
            });

            //5. on click of the image and what not click the button
            $('#reset').one('click.AttachedEvent', function (event) {
                $('#btnReset').click();
            });

        });
    
    </script>
    <script src="../JavaScript/modalPopup.js" type="text/javascript"></script>
    <asp:HiddenField ID="check" runat="server" Value="0" />
    <table class="BaseTableStyle" border="2" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table class="TableForHeader" border="0" cellpadding="0" cellspacing="0">
                    <tr valign="top">
                        <td width="100px">
                        </td>
                        <td align="center" valign="top">
                            <div style="font-family: 'Bauhaus 93'; font-size: 20px;">
                                <asp:Label ID="lblStoreName" runat="server" ForeColor="#6086ac"></asp:Label></div>
                        </td>
                        <td align="right" width="75px" nowrap="nowrap" height="10" valign="top" style="color: Black;
                            font-weight: bold">
                            Version : 1.4.6
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top" style="background-color: White; color: White; font-weight: bold;">
            <td align="left" style="background-color: White">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:Menu ID="MainMenu" runat="server" StaticEnableDefaultPopOutImage="False" Font-Size="1.4em"
                                ForeColor="Black" Orientation="Horizontal" Font-Names="Arial Black" BackColor="White"
                                DynamicHorizontalOffset="2" StaticSubMenuIndent="10px">
                                <StaticMenuStyle BorderColor="#6086ac" BorderStyle="Solid" BorderWidth="0px" />
                                <StaticSelectedStyle BackColor="#6086ac" />
                                <StaticMenuItemStyle HorizontalPadding="5px" BorderColor="White" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Arial" Font-Size="Large" VerticalPadding="2px" />
                                <DynamicHoverStyle BackColor="#6086ac" BorderColor="#6086ac" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Arial" Font-Size="Large" ForeColor="Orange" />
                                <DynamicMenuStyle BackColor="#6086ac" />
                                <DynamicSelectedStyle BackColor="#6086ac" />
                                <DynamicMenuItemStyle BackColor="#6086ac" Font-Size="Large" ForeColor="White" VerticalPadding="2px"
                                    BorderColor="White" BorderWidth="1px" Font-Names="Arial" HorizontalPadding="5px" />
                                <Items>
                                </Items>
                                <StaticHoverStyle BorderColor="#6086ac" BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial"
                                    Font-Size="Large" ForeColor="White" BackColor="#6086ac" />
                            </asp:Menu>
                        </td>
                        <td align="right">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="font-weight: bolder; border-style: solid; border-width: 0px; border-color: #FFFFFF #006600 #006600 #006600;
                                        background-color: White; color: Black; font-family: Arial; font-size: small"
                                        align="right" nowrap="nowrap">
                                        <a href="../Help.html" target="_blank">Help</a>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnNewBooking" runat="server" AccessKey="n" Text="New (F1)"
                                            CausesValidation="false" OnClick="btnNewBooking_Click" />
                                        <asp:Button ID="btnEditBooking" runat="server" Text="Edit (F4)" OnClick="btnEditBooking_Click"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnF8" runat="server" Text="Save (F8)" OnClick="btnF8_Click"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnDelivery" runat="server" Text="Delivery (F6)"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnF2" runat="server" Text="F2-Advance Search" Style="display: none"
                                            OnClick="btnF2_Click" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="font-weight: bolder; border-style: solid; border-width: 0px; border-color: #FFFFFF #006600 #006600 #006600;
                                        background-color: White; color: Black; font-family: Arial; font-size: small"
                                        align="right" nowrap="nowrap">
                                        Welcome
                                        <%=CurrentUserName %>,&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" Text="Log Out"
                                            ForeColor="Red" Font-Bold="true" runat="server" OnClick="lnkLogOut_Click" CausesValidation="False" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="Container">
                                <%--div class="ToolBar">--%>
                                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                                <table width="100%">
                                    <tr>
                                        <td align="center" colspan="15">
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" visible="false" id="tblSearch" runat="server">
                                    <tr>
                                        <td align="center" colspan="15" class="TableData">
                                            Booking Number :
                                            <asp:TextBox ID="txtEdit" runat="server" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtEdit_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" FilterType="Numbers" TargetControlID="txtEdit">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Style="display: none"
                                                CausesValidation="false" Text="Edit booking" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="15">
                                            <asp:Label ID="lblSearchError" runat="server" EnableViewState="false" CssClass="ErrorMessage"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" id="tblEntry" runat="server">
                                    <tr>
                                        <td nowrap="nowrap" class="TableData">
                                            Last Booking
                                        </td>
                                        <td nowrap="nowrap">
                                            <%--<asp:Timer runat="server" id="UpdateTimer" interval="5000" ontick="UpdateTimer_Tick"  />
                                                <asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional" >
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger controlid="UpdateTimer" eventname="Tick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                          <asp:Label ID="lblLastBooking" runat="server" Font-Bold="True" Font-Size="Small"
                                                    Font-Underline="True" ForeColor="Blue"></asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                            <asp:Label ID="lblLastBooking" runat="server" Font-Bold="True" Font-Size="Small"
                                                ForeColor="Blue"></asp:Label>
                                        </td>
                                        <td class="TableData">
                                            Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDate" runat="server" Width="80px" MaxLength="11" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td nowrap="nowrap" class="TableData">
                                            Due Date
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtDueDate" runat="server" Width="80px" MaxLength="11" onkeyPress="return checkKey(event);"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtDueDate" Format="dd MMM yyyy">
                                                    </cc1:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="TableData">
                                            Time
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTime" runat="server" Width="49px" MaxLength="10" onkeyPress="return checkKey(event);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtTime_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="txtTime" ValidChars="0123456789AMPamp: ">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td class="TableData">
                                            Customer
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtCustomerName" runat="server" onKeyPerss="EnterKeyPerss" AutoPostBack="True"
                                                                    MaxLength="300" onfocus="javascript:select();" OnTextChanged="txtCustomerName_TextChanged"
                                                                    Width="420px"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerName"
                                                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="1"
                                                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                </cc1:AutoCompleteExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <cc1:HoverMenuExtender ID="CustHover" runat="server" TargetControlID="imgBtnCustomerAdd"
                                                                    PopupControlID="CustPanal1" PopupPosition="Bottom" OffsetX="6" PopDelay="25"
                                                                    HoverCssClass="popupHover">
                                                                </cc1:HoverMenuExtender>
                                                                <asp:Panel ID="CustPanal1" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                    <table style="font-size: smaller; color: Black">
                                                                        <tr>
                                                                            <td align="left" style="color: Black">
                                                                                Add new customer.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                                <asp:Button ID="imgBtnCustomerAdd" runat="server" Style="border-radius: 7px;" Text="Add"
                                                                    onfocus="CtlOnFocus(this);" onblur="CtlOnBlur(this,'btn');" Enabled="True" onmouseout="CtlOnBlur(this,'btn');"
                                                                    onmouseover="CtlOnFocus(this);" OnClick="imgBtnCustomerAdd_Click" CausesValidation="False" />
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" id="tblEntry1" runat="server">
                                    <tr>
                                        <td colspan="10">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <table class="TableData">
                                                        <tr>
                                                            <td>
                                                                Address :
                                                            </td>
                                                            <td style="width: 265px" nowrap="nowrap" id="td1" runat="server">
                                                                <asp:Label ID="lblAddress" runat="server" Font-Bold="True" Width="250px" CssClass="Legend"></asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 225px" nowrap="nowrap">
                                                            </td>
                                                            <td>
                                                                Mob. No. :
                                                            </td>
                                                            <td style="width: 160px" nowrap="nowrap">
                                                                <asp:Label ID="lblMobileNo" runat="server" Font-Bold="True" Width="160px" CssClass="Legend"></asp:Label>
                                                            </td>
                                                            <td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="15">
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <table class="TableData">
                                                        <tr>
                                                            <td>
                                                                Priority :
                                                            </td>
                                                            <td style="width: 265px" nowrap="nowrap" id="td2" runat="server">
                                                                <asp:Label ID="lblPriority" runat="server" Font-Bold="True" Width="60px" CssClass="Legend"></asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 230px" nowrap="nowrap">
                                                            </td>
                                                            <td>
                                                                Remarks :
                                                            </td>
                                                            <td style="width: 160px" nowrap="nowrap">
                                                                <asp:Label ID="lblRemarks" runat="server" Font-Bold="True" Width="160px" CssClass="Legend"></asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 160px" nowrap="nowrap">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                &nbsp;<asp:CheckBox ID="chkToday" runat="server" AutoPostBack="True" OnCheckedChanged="chkToday_CheckedChanged1" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:CheckBox ID="chkNextDay" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextDay_CheckedChanged1" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:CheckBox ID="chkOldBooking" runat="server" OnCheckedChanged="chkNextDay_CheckedChanged1"
                                                                    Text="Old Booking" Visible="False" />
                                                            </td>
                                                            <td nowrap="nowrap" style="width: 160px">
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" id="tblEntry2" runat="server">
                                    <tr>
                                        <td colspan="15">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlEntry" runat="server" ScrollBars="Vertical" Height="225px">
                                                            <asp:GridView ID="grdEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                CssClass="mGrid" ForeColor="#333333" GridLines="Both" OnRowCommand="grdEntry_RowCommand"
                                                                OnRowDataBound="grdEntry_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="color: orange; border-width: 0px">
                                                                                        S.No.
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:Label ID="lblHSNo" runat="server" Text='<%# Bind("SNO") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:Label ID="lblSNO" runat="server" Text='<%# Bind("SNO") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="color: Orange; border-width: 0px">
                                                                                        Qty
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:TextBox ID="txtQty" runat="server" Width="50px" Text='<%# Bind("QTY") %>' MaxLength="4"
                                                                                            onkeyPress="return checkKey(event);" onfocus="javascript:select();" onblur="javascript:select();"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="txtQty_FilteredTextBoxExtender" runat="server" FilterType="Numbers"
                                                                                            Enabled="True" TargetControlID="txtQty">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="left" style="border-width: 0px">
                                                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QTY") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:UpdatePanel ID="upItemName" runat="server">
                                                                                <ContentTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td colspan="2" align="center" style="color: Orange; border-width: 0px">
                                                                                                Item Name
                                                                                            </td>
                                                                                            <td colspan="2" align="center" style="color: Orange; border-width: 0px">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" style="border-width: 0px">
                                                                                                <asp:TextBox ID="txtName" runat="server" Width="200px" Text='<%# Bind("ITEMNAME") %>'
                                                                                                    CausesValidation="false" MaxLength="300" AutoPostBack="True" OnTextChanged="txtName_TextChanged" onfocus="javascript:select();"  onblur="javascript:select();"></asp:TextBox>
                                                                                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtName" TargetControlID="txtName"
                                                                                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                                                                                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                            </td>
                                                                                            <td style="border-width: 0px">
                                                                                                <cc1:HoverMenuExtender ID="ItemHover" runat="server" TargetControlID="imgBtnItemAdd"
                                                                                                    PopupControlID="ItemPanal" PopupPosition="Bottom" OffsetX="6" PopDelay="25" HoverCssClass="popupHover">
                                                                                                </cc1:HoverMenuExtender>
                                                                                                <asp:Panel ID="ItemPanal" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                                                    <table style="font-size: smaller; color: Black">
                                                                                                        <tr>
                                                                                                            <td align="left" style="color: Black">
                                                                                                                Add new item When you are not found in the list.
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>
                                                                                                <asp:Button ID="imgBtnItemAdd" runat="server" Text="Add" CausesValidation="false"
                                                                                                    Style="border-radius: 7px;" onfocus="CtlOnFocus(this);" onblur="CtlOnBlur(this,'btn');"
                                                                                                    Enabled="True" onmouseout="CtlOnBlur(this,'btn');" onmouseover="CtlOnFocus(this);"
                                                                                                    OnClick="imgBtnItemAdd_Click" />
                                                                                            </td>
                                                                                            <td style="border-width: 0px">
                                                                                                <cc1:HoverMenuExtender ID="RemarkHoverPopup" runat="server" TargetControlID="btnAddRAndC"
                                                                                                    PopupControlID="RemarkPopUp" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                                    HoverCssClass="popupHover">
                                                                                                </cc1:HoverMenuExtender>
                                                                                                <asp:Panel ID="RemarkPopUp" runat="server" Height="50px" Width="125px" CssClass="popupMenu">
                                                                                                    <table style="font-size: smaller;">
                                                                                                        <tr>
                                                                                                            <td align="left" style="color: Black">
                                                                                                                There you can add remarks.
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>
                                                                                                <asp:Button ID="btnAddRAndC" runat="server" CausesValidation="false" OnClick="btnAddRAndC_Click"
                                                                                                    Text="Remarks" ToolTip="" Style="border-radius: 7px; display: none" onfocus="CtlOnFocus(this);"
                                                                                                    onblur="CtlOnBlur(this,'btn');" Enabled="True" onmouseout="CtlOnBlur(this,'btn');"
                                                                                                    onmouseover="CtlOnFocus(this);" />
                                                                                            </td>
                                                                                            <td style="border-width: 0px">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="left" style="border-width: 0px">
                                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ITEMNAME") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:UpdatePanel ID="upProcess" runat="server">
                                                                                <ContentTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td colspan="3" align="center" style="color: Orange; border-width: 0px">
                                                                                                Process and Rate
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" style="border-width: 0px">
                                                                                                <asp:TextBox ID="txtProcess" runat="server" Width="50px" Text='<%# Bind("PROCESS") %>'
                                                                                                    CausesValidation="false" MaxLength="3" AutoPostBack="True" OnTextChanged="txtProcess_TextChanged" onfocus="javascript:select();" onblur="javascript:select();"></asp:TextBox>
                                                                                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtProcess" TargetControlID="txtProcess"
                                                                                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1"
                                                                                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                            </td>
                                                                                            <td style="color: Black; border-width: 0px">
                                                                                                @
                                                                                            </td>
                                                                                            <td style="border-width: 0px">
                                                                                                <asp:TextBox ID="txtRate" runat="server" Width="60px" Text='<%# Bind("RATE") %>'
                                                                                                    Style="text-align: right" MaxLength="6" onfocus="javascript:select();" onblur="javascript:select();">0</asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="txtRate_FilteredTextBoxExtender" runat="server"
                                                                                                    Enabled="True" TargetControlID="txtRate" FilterType="Custom" ValidChars=".0123456789">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                            <%-- <td>
                                                                                                    <cc1:HoverMenuExtender ID="ProcessHover" runat="server" TargetControlID="imgBtnProcessAdd"
                                                                                                        PopupControlID="HoverPanal" PopupPosition="Bottom" OffsetX="6" PopDelay="25"
                                                                                                        HoverCssClass="popupHover">
                                                                                                    </cc1:HoverMenuExtender>
                                                                                                    <asp:Panel ID="HoverPanal" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                                                        <table style="font-size: smaller; color: Black">
                                                                                                            <tr>
                                                                                                                <td align="left" style="color: Black">
                                                                                                                    Add new process When you are not found in the list.
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </asp:Panel>
                                                                                                    <asp:Button ID="imgBtnProcessAdd" runat="server" Text="Add" CausesValidation="false"
                                                                                                        OnClick="imgBtnProcessAdd_Click" Style="border-radius: 7px;" onfocus="CtlOnFocus(this);"
                                                                                                        onblur="CtlOnBlur(this,'btn');" Enabled="True" onmouseout="CtlOnBlur(this,'btn');"
                                                                                                        onmouseover="CtlOnFocus(this);" />
                                                                                                </td>--%>
                                                                                            <td style="border-width: 0px">
                                                                                                <cc1:HoverMenuExtender ID="ExtraHoverPopup" runat="server" TargetControlID="btnExtra"
                                                                                                    PopupControlID="PanelPopUp" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                                    HoverCssClass="popupHover">
                                                                                                </cc1:HoverMenuExtender>
                                                                                                <asp:Panel ID="PanelPopUp" runat="server" Height="50px" Width="125px" CssClass="popupMenu">
                                                                                                    <table style="font-size: smaller; color: Black">
                                                                                                        <tr>
                                                                                                            <td align="left" style="color: Black">
                                                                                                                Extra process add extra processing for individual clothes.
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>
                                                                                                <asp:Button ID="btnExtra" runat="server" Text=" Ext Pr " CausesValidation="false"
                                                                                                    ToolTip="" Style="border-radius: 7px;" OnClick="btnExtra_Click" onfocus="CtlOnFocus(this);"
                                                                                                    onblur="CtlOnBlur(this,'btn');" Enabled="True" onmouseout="CtlOnBlur(this,'btn');"
                                                                                                    onmouseover="CtlOnFocus(this);" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="left" style="border-width: 0px">
                                                                                        <asp:Label ID="lblProcess" runat="server" Text='<%# Bind("PROCESS") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="color: Orange; border-width: 0px">
                                                                                        Description
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border-width: 0px; text-align: left">
                                                                                        <asp:TextBox ID="txtRemarks" runat="server" Width="120px" onkeyPress="return checkKey(event);"
                                                                                            AutoPostBack="true" OnTextChanged="txtRemarks_TextChanged" Style="display: none;"></asp:TextBox>
                                                                                        <input name="tags" id="mySingleField" style="display: none;" />
                                                                                        <ul id="mytags">
                                                                                        </ul>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="left" style="border-width: 0px">
                                                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>' ForeColor="Black"
                                                                                            CssClass="grdCustName"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="color: Orange; border-width: 0px">
                                                                                        Color
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border-width: 0px; text-align: left">
                                                                                        <asp:TextBox ID="txtColor" runat="server" Width="120px" onkeyPress="return checkKey(event);"></asp:TextBox>
                                                                                        <cc1:HoverMenuExtender ID="ColorHoverPopup" runat="server" TargetControlID="txtColor"
                                                                                            PopupControlID="PanelPopUpColor" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                            HoverCssClass="popupHover">
                                                                                        </cc1:HoverMenuExtender>
                                                                                        <asp:Panel ID="PanelPopUpColor" runat="server" Width="370px" CssClass="popupMenu">
                                                                                            <table style="font-size: 12px; font-family: 'Courier New', Courier, monospace; cursor: pointer"
                                                                                                cellpadding="2" cellspacing="2">
                                                                                                <tr>
                                                                                                    <td style='background-color: White; color: Black' title="White" nowrap="nowrap" onclick="SetColor('White');">
                                                                                                        White
                                                                                                    </td>
                                                                                                    <td style='background-color: Gainsboro; color: Black' title="Light Gray" nowrap="nowrap"
                                                                                                        onclick="SetColor('Light Gray');">
                                                                                                        Light Gray
                                                                                                    </td>
                                                                                                    <td style='background-color: Gray; color: Black' title="Gray" nowrap="nowrap" onclick="SetColor('Dark Gray');">
                                                                                                        Dark Gray
                                                                                                    </td>
                                                                                                    <td style='background-color: Cornsilk; color: Black' title="Cornsilk" nowrap="nowrap"
                                                                                                        onclick="SetColor('Cream');">
                                                                                                        Cream
                                                                                                    </td>
                                                                                                    <td style='background-color: Wheat; color: Black' title="Wheat" nowrap="nowrap" onclick="SetColor('Light Brown');">
                                                                                                        Light Brown
                                                                                                    </td>
                                                                                                    <td style='background-color: Bisque; color: Black' title="Beige" nowrap="nowrap"
                                                                                                        onclick="SetColor('Beige');">
                                                                                                        Beige
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style='background-color: Khaki; color: Black' title="Khaki" nowrap="nowrap" onclick="SetColor('Khaki');">
                                                                                                        Khaki
                                                                                                    </td>
                                                                                                    <td style='background-color: Gold; color: Black' title="Gold" nowrap="nowrap" onclick="SetColor('Gold');">
                                                                                                        Gold
                                                                                                    </td>
                                                                                                    <td style='background-color: Orange; color: Black' title="Orange" nowrap="nowrap"
                                                                                                        onclick="SetColor('Orange');">
                                                                                                        Orange
                                                                                                    </td>
                                                                                                    <td style='background-color: Chocolate; color: Black' title="Chocolate" nowrap="nowrap"
                                                                                                        onclick="SetColor('Chocolate');">
                                                                                                        Chocolate
                                                                                                    </td>
                                                                                                    <td style='background-color: Brown; color: White' title="Brown" nowrap="nowrap" onclick="SetColor('Brown');">
                                                                                                        Brown
                                                                                                    </td>
                                                                                                    <td style='background-color: Maroon; color: White' title="Maroon" nowrap="nowrap"
                                                                                                        onclick="SetColor('Maroon');">
                                                                                                        Maroon
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style='background-color: Pink; color: Black' title="Pink" nowrap="nowrap" onclick="SetColor('Pink');">
                                                                                                        Pink
                                                                                                    </td>
                                                                                                    <td style='background-color: Violet; color: Black' title="Violet" nowrap="nowrap"
                                                                                                        onclick="SetColor('Violet');">
                                                                                                        Violet
                                                                                                    </td>
                                                                                                    <td style='background-color: Magenta; color: Black' title="Magenta" nowrap="nowrap"
                                                                                                        onclick="SetColor('Magenta');">
                                                                                                        Magenta
                                                                                                    </td>
                                                                                                    <td style='background-color: BlueViolet; color: white' title="BlueViolet" nowrap="nowrap"
                                                                                                        onclick="SetColor('BlueViolet');">
                                                                                                        BlueViolet
                                                                                                    </td>
                                                                                                    <td style='background-color: Purple; color: white' title="Purple" nowrap="nowrap"
                                                                                                        onclick="SetColor('Purple');">
                                                                                                        Purple
                                                                                                    </td>
                                                                                                    <td style='background-color: SkyBlue; color: Black' title="SkyBlue" nowrap="nowrap"
                                                                                                        onclick="SetColor('SkyBlue');">
                                                                                                        SkyBlue
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style='background-color: Turquoise; color: Black' title="Turquoise" nowrap="nowrap"
                                                                                                        onclick="SetColor('Aqua');">
                                                                                                        Aqua
                                                                                                    </td>
                                                                                                    <td style='background-color: Blue; color: White' title="Blue" nowrap="nowrap" onclick="SetColor('Blue');">
                                                                                                        Blue
                                                                                                    </td>
                                                                                                    <td style='background-color: Navy; color: White' title="Navy" nowrap="nowrap" onclick="SetColor('Navy');">
                                                                                                        Navy
                                                                                                    </td>
                                                                                                    <td style='background-color: Lime; color: Black' title="Lime" nowrap="nowrap" onclick="SetColor('Lime');">
                                                                                                        Lime
                                                                                                    </td>
                                                                                                    <td style='background-color: Teal; color: Black' title="Teal" nowrap="nowrap" onclick="SetColor('Teal');">
                                                                                                        Teal
                                                                                                    </td>
                                                                                                    <td style='background-color: Green; color: Black' title="Green" nowrap="nowrap" onclick="SetColor('Green');">
                                                                                                        Green
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style='background-color: Olive; color: Black' title="Olive" nowrap="nowrap" onclick="SetColor('Olive');">
                                                                                                        Olive
                                                                                                    </td>
                                                                                                    <td style='background-color: Yellow; color: Black' title="Yellow" nowrap="nowrap"
                                                                                                        onclick="SetColor('Yellow');">
                                                                                                        Yellow
                                                                                                    </td>
                                                                                                    <td style='background-color: Tomato; color: Black' title="Tomato" nowrap="nowrap"
                                                                                                        onclick="SetColor('Tomato');">
                                                                                                        Tomato
                                                                                                    </td>
                                                                                                    <td style='background-color: Red; color: Black' title="Red" nowrap="nowrap" onclick="SetColor('Red');">
                                                                                                        Red
                                                                                                    </td>
                                                                                                    <td style='background-color: Crimson; color: White' title="Crimson" nowrap="nowrap"
                                                                                                        onclick="SetColor('Crimson');">
                                                                                                        Crimson
                                                                                                    </td>
                                                                                                    <td style='background-color: Black; color: White' title="Black" nowrap="nowrap" onclick="SetColor('Black');">
                                                                                                        Black
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="left" style="border-width: 0px">
                                                                                        <asp:Label ID="lblColor" runat="server" Text='<%# Bind("COLOR") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="color: Orange; border-width: 0px">
                                                                                        Amount
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:Label ID="lblHAmount" runat="server" Text='<%# Bind("AMOUNT") %>' Style="text-align: right"
                                                                                            ForeColor="Black">0</asp:Label>
                                                                                    </td>
                                                                                    <td style="border-width: 0px">
                                                                                        <cc1:HoverMenuExtender ID="AddPopup" runat="server" TargetControlID="imgBtnGridEntry"
                                                                                            PopupControlID="AddButtonPopUp" PopupPosition="bottom" OffsetX="6" PopDelay="25"
                                                                                            HoverCssClass="popupHover">
                                                                                        </cc1:HoverMenuExtender>
                                                                                        <asp:Panel ID="AddButtonPopUp" runat="server" Height="50px" Width="100px" CssClass="popupMenu">
                                                                                            <table style="font-size: smaller; color: Black">
                                                                                                <tr>
                                                                                                    <td align="left" style="color: Black">
                                                                                                        To add or update record click here.
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                        <asp:Button ID="imgBtnGridEntry" runat="server" CausesValidation="false" ToolTip=""
                                                                                            Text="Add" Style="border-radius: 7px;" onfocus="CtlOnFocus(this);" onblur="CtlOnBlur(this,'btn');"
                                                                                            Enabled="True" onmouseout="CtlOnBlur(this,'btn');" onmouseover="CtlOnFocus(this);"
                                                                                            OnClick="imgBtnGridEntry_Click" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("AMOUNT") %>' ForeColor="Black"></asp:Label>
                                                                                    </td>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:ImageButton ID="imgbtnEdit" CommandName="EDITItemDetails" runat="server" ImageUrl="~/images/EditInformationHS.png"
                                                                                            CausesValidation="false" onfocus="CtlOnFocus(this);" onblur="CtlOnBlur(this,'btn');"
                                                                                            OnClientClick="addAcc(this.id,<%# CType(Container, GridViewRow).RowIndex %>)"
                                                                                            TabIndex="22" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Click to edit the item details in list."
                                                                                            Data="value" />
                                                                                    </td>
                                                                                    <td style="border-width: 0px">
                                                                                        <asp:ImageButton ID="imgbtnDeleteItemDetails" CommandName="DeleteItemDetails" runat="server"
                                                                                            CausesValidation="false" ImageUrl="~/images/Delete.gif" Text="Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                            OnClientClick="return confirm('Are you sure? you want to delete this item details.');"
                                                                                            ToolTip="Click to delete the item details in list." />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" colspan="7" style="font-size: large; color: Black">
                                            Qty :&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCount" runat="server" Text="0" ForeColor="Black"
                                                Font-Size="Large"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" class="TableData" colspan="7">
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <table class="TableData tblOldBookingLeftGrid" style="border-style: solid;">
                                                        <%--<tr>
                                                                <td style="color: Black">
                                                                    <asp:Label runat="server" ID="lblToday"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkToday" runat="server" TextAlign="Left" AutoPostBack="True" OnCheckedChanged="chkToday_CheckedChanged" />
                                                                </td>
                                                                <td style="color: Black">
                                                                    <asp:Label runat="server" ID="lblNextDay"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkNextDay" runat="server" TextAlign="Left" AutoPostBack="True"
                                                                        OnCheckedChanged="chkNextDay_CheckedChanged" />
                                                                </td>
                                                            </tr>--%>
                                                        <tr>
                                                            <td style="color: Black">
                                                                Home Delivery :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="chkHD" runat="server" TextAlign="Left" />
                                                            </td>
                                                            <td style="color: Black">
                                                                Send SMS :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="chkSendsms" runat="server" TextAlign="Left" onclick="toggleDropDownList(this);" />
                                                            </td>
                                                            <td style="color: Black">
                                                                Sms Template :
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpsmstemplate" runat="server" Enabled="false">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDCaption" style="color: Black">
                                                                Notes :
                                                            </td>
                                                            <td align="left" colspan="3">
                                                                <asp:TextBox ID="txtRemarks" runat="server" Width="170px" MaxLength="100" onkeyPress="return checkKey(event);"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDCaption" style="color: Black">
                                                                Checked By :
                                                            </td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="drpCheckedBy" runat="server" Width="170px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2" class="BookingBtns">
                                            <div id="lowerRowCenter" class="lowerCenterTd">
                                                <asp:Label ID="lblSave" CssClass="SuccessMessage" runat="server" EnableViewState="false" />
                                                <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                                <br />
                                                <br />
                                                <table id="tblBtnSavings">
                                                    <tr>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="save">
                                                                    <span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnSaveBooking" runat="server" OnClick="btnSaveBooking_Click" CausesValidation="false"
                                                                            Enabled="True" />
                                                                        (F8)</span></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="print">
                                                                    <span class="btnImage"></span><span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnSavePrint" runat="server" CausesValidation="false" Enabled="True"
                                                                            OnClick="btnSavePrint_Click" ClientIDMode="Static" />
                                                                        (F9)</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="saveprinttag">
                                                                    <span class="btnImage"></span><span class="btnImage"></span><span class="btnImage">
                                                                    </span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnSavePrintBarCode" runat="server" OnClick="btnSavePrintBarCode_Click"
                                                                            CausesValidation="false" Enabled="True" />
                                                                        (F10)</span></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="btnSaveBooking">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="printtag">
                                                                    <span class="btnImage"></span><span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnPrintBarCode" runat="server" OnClick="btnPrintBarCode_Click" CausesValidation="false"
                                                                            Enabled="True" />
                                                                        (F12)</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <%--       <tr>
                                                        <td class="btnSaveBooking" colspan="2" align="center">
                                                            <div class="btnSaveBookingContainder">
                                                                <div id="reset">
                                                                    <span class="btnImage"></span><span class="btnsavebookingspan">
                                                                        <asp:Button ID="btnReset" runat="server" CausesValidation="false" Enabled="True"
                                                                            OnClick="btnReset_Click" />
                                                                        (F12)</span></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </div>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="updcheckservice" runat="server">
                                                <ContentTemplate>
                                                    <table class="TDCaption calcGrid" style="border-style: solid;">
                                                        <tr>
                                                            <td style="color: Black">
                                                                G. Total :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCurrentDue" runat="server" Width="167px" Style="text-align: right"
                                                                    ReadOnly="true">0</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: Red">
                                                                Discount :
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdrPercentage" runat="server" AutoPostBack="True" Checked="True"
                                                                    GroupName="a" OnCheckedChanged="rdrPercentage_CheckedChanged" Text="  %" />
                                                                <asp:RadioButton ID="rdrAmt" runat="server" AutoPostBack="True" GroupName="a" OnCheckedChanged="rdrAmt_CheckedChanged"
                                                                    Text=" Flat" />
                                                                <asp:DropDownList ID="drpDiscountOption" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDiscountOption_SelectedIndexChanged"
                                                                    Visible="False">
                                                                    <asp:ListItem Selected="True" Text="In Percentage" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="In Amount" Value="1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtDiscount" runat="server" Width="35px" Style="text-align: right"
                                                                    AutoPostBack="True" OnTextChanged="txtDiscount_TextChanged" MaxLength="2">0</asp:TextBox>
                                                                <cc1:TextBoxWatermarkExtender ID="txtDiscount_TextBoxWatermarkExtender" runat="server"
                                                                    Enabled="True" TargetControlID="txtDiscount" WatermarkText="0">
                                                                </cc1:TextBoxWatermarkExtender>
                                                                <asp:Label ID="lblDisAmt" runat="server" ForeColor="Red"></asp:Label>
                                                                <cc1:FilteredTextBoxExtender ID="txtDiscount_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" TargetControlID="txtDiscount" ValidChars=".0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:TextBox ID="txtDiscountAmt" runat="server" Width="38px" MaxLength="6" Style="text-align: right"
                                                                    AutoPostBack="True" OnTextChanged="txtDiscountAmt_TextChanged" Visible="False">0</asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="txtDiscountAmt_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" TargetControlID="txtDiscountAmt" FilterType="Custom" ValidChars=".0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: Black">
                                                                Tax :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSrTax" runat="server" Width="167px" Style="text-align: right"
                                                                    ReadOnly="true">0</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: Black">
                                                                Total :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTotal" runat="server" Width="167px" Style="text-align: right"
                                                                    ReadOnly="true">0</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: Green">
                                                                Advance :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAdvance" runat="server" Width="167px" Style="text-align: right"
                                                                    Text="0" AutoPostBack="True" OnTextChanged="txtAdvance_TextChanged" MaxLength="6">0</asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="txtAdvance_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtAdvance">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: Black">
                                                                Balance :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBalance" runat="server" Width="167px" Style="text-align: right"
                                                                    ReadOnly="true">0</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--</div>--%>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender7" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="plnAddProcess" Drag="true" TargetControlID="btnPopUpProcess">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnPopUpProcess" runat="server" Style="display: none" Text="a" CausesValidation="false"
                                OnClick="btnPopUpProcess_Click" />
                            <asp:Panel ID="plnAddProcess" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="450PX">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="PopupHeader">
                                            <div class="TitlebarLeft">
                                                Add Process
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td colspan="3" align="left">
                                                        <asp:Label ID="lblPrcErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                                        <asp:Label ID="lblPrcSucess" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Process Code &nbsp;:
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtProcessCode" runat="server" MaxLength="4" CssClass="Textbox"
                                                            TabIndex="1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 5Px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Process Name :
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox TabIndex="2" ID="txtProcessName" MaxLength="20" runat="server" CssClass="Textbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 10Px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnProcessSave" runat="server" Text="Save" OnClick="btnProcessSave_Click"
                                                            CausesValidation="false" TabIndex="3" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <asp:Button ID="BtnItem" runat="server" Text="Button" Style="display: none" OnClick="btnOption_Click1" />
                            <asp:Panel ID="pnlItem" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="500PX">
                                <asp:UpdatePanel ID="upMakeingItem" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div3">
                                            <div class="TitlebarLeft">
                                                Add Items
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TableData">
                                                <tr>
                                                    <td width="150px" align="right">
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />
                                                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                                                        <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table class="TableData">
                                                            <tr>
                                                                <td class="TDCaption">
                                                                    Item Name :
                                                                </td>
                                                                <td class="TDDot" nowrap="nowrap">
                                                                    <asp:TextBox ID="txtItemName" runat="server" MaxLength="50" TabIndex="1" Width="200px"
                                                                        CssClass="Textbox"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtItemName_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtItemName" ValidChars=" abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <span class="span">*</span>
                                                                </td>
                                                                <td width="100%">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption">
                                                                    Sub Items :
                                                                </td>
                                                                <td align="left" style="margin-left: 160px">
                                                                    <asp:TextBox ID="txtItemSubQty" runat="server" MaxLength="2" TabIndex="2" Width="50px"
                                                                        ToolTip="No. of sub items for Item" CssClass="Textbox" AutoPostBack="True" OnTextChanged="txtItemSubQty_TextChanged">1</asp:TextBox>
                                                                    <asp:TextBox ID="txtNewItemName" runat="server" AutoPostBack="true" TabIndex="3"
                                                                        OnTextChanged="txtNewItemName_TextChanged" Visible="False"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtItemSubQty_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtItemSubQty" ValidChars="1234567890">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td rowspan="3">
                                                                    <asp:ListBox ID="lstSubItem" runat="server" AutoPostBack="true" CssClass="Textbox"
                                                                        OnSelectedIndexChanged="lstSubItem_OnSelectedIndexChanged" TabIndex="4" Visible="False"
                                                                        Width="150px"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TDCaption">
                                                                    Item Code :
                                                                </td>
                                                                <td class="TDDot" nowrap="nowrap">
                                                                    <asp:TextBox ID="txtItemCode" runat="server" Width="50Px" CssClass="Textbox" ToolTip="Please enter item code"
                                                                        TabIndex="5" MaxLength="4"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtItemCode_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtItemCode" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <table style="width: 400px;">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnItemSave" CausesValidation="false" runat="server" Text="Save"
                                                                        OnClick="btnItemSave_Click" OnClientClick="return checkName();" TabIndex="6" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span><asp:HiddenField
                                                                ID="hdntemp" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnItemCode" runat="server" Value="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender4" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="pnlItem" Drag="true" TargetControlID="btnAddItem">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnAddItem" runat="server" Text="ReloadGrid" Style="display: none"
                                OnClick="btnAddItem_Click" />
                            <asp:SqlDataSource ID="SqlDataProcessTypes" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                SelectCommand="SELECT ProcessCode, ProcessName FROM ProcessMaster Order By ProcessName">
                            </asp:SqlDataSource>
                            <asp:Button ID="btnSearch" runat="server" Text="ReloadGrid" Style="display: none"
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="btnCustomerSearch" runat="server" Text="ReloadGrid" Style="display: none"
                                CausesValidation="false" />
                            <cc1:ModalPopupExtender ID="ModalPopupExtender5" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="pnlCustomerSearch" Drag="true"
                                TargetControlID="btnCustomerSearch">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pnlCustomerSearch" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="750PX">
                                <div class="popup_Titlebar" id="Div4">
                                    <div class="TitlebarLeft">
                                        Search Customer
                                    </div>
                                </div>
                                <div class="popup_Body">
                                    <table class="TDCaption">
                                        <tr>
                                            <td align="center">
                                                Customer Name
                                            </td>
                                            <td align="center">
                                                Address
                                            </td>
                                            <td align="center">
                                                Mobile No
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtCNameSearch" runat="server" Width="150" onfocus="javascript:select();"
                                                            TabIndex="1" Text=" " OnTextChanged="txtCNameSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%--onfocus="javascript:select();" ontextchanged="txtCustomerName_TextChanged"--%>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Width="220" onfocus="javascript:select();"
                                                            TabIndex="2" OnTextChanged="txtAddress_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtPhoneNo" runat="server" Width="80" OnTextChanged="txtPhoneNo_TextChanged"
                                                            TabIndex="3" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:UpdatePanel ID="upCustSearch" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="250" Width="730">
                                                            <%-- <asp:GridView ID="grdCustomerSearch" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                                                ToolTip="List of found customers. Click on the link to select the customer."
                                                                Width="710" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                                                                CellPadding="4" ForeColor="Black" GridLines="Vertical" OnRowCommand="grdCustomerSearch_OnRowCommand">
                                                                <RowStyle BackColor="#F7F7DE" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Name" ShowHeader="False" HeaderStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerName" runat="server" CausesValidation="false" TabIndex="4"
                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustName") %>'></asp:LinkButton>
                                                                            <asp:HiddenField ID="lnkBtnCustomerCode" runat="server" Value='<%# Eval("CustomerCode") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Address" ShowHeader="False" HeaderStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerAddress" runat="server" CausesValidation="false"
                                                                                TabIndex="5" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustAddress") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Mobile" ShowHeader="False" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerMobile" runat="server" CausesValidation="false"
                                                                                TabIndex="6" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustMobile") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks" ShowHeader="False" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerRemark" runat="server" CausesValidation="false"
                                                                                TabIndex="7" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustRemarks") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Priority" ShowHeader="False" HeaderStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCustomerPriority" runat="server" CausesValidation="false"
                                                                                TabIndex="8" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SelectCustomer"
                                                                                Text='<%# Eval("CustPriority") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle VerticalAlign="Top" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#CCCC99" />
                                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                                                <AlternatingRowStyle BackColor="White" />
                                                            </asp:GridView>
                                                            --%>
                                                        </asp:Panel>
                                                        <asp:Button ID="btnHideCustSearch" CausesValidation="false" runat="server" Text="Cust Search"
                                                            Style="display: none" OnClick="btnHideCustSearch_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender8" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="plnAddExtraProcess" Drag="true"
                                TargetControlID="btnAddExtraProcess">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnAddExtraProcess" runat="server" Text="ReloadGrid" CausesValidation="false"
                                Style="display: none" />
                            <asp:Panel ID="plnAddExtraProcess" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="450PX">
                                <asp:UpdatePanel ID="upAddExtraProcess" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div6">
                                            <div class="TitlebarLeft">
                                                Add Extra Process
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td align="left">
                                                        Second Process
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraProcess1" runat="server" Width="100" AutoPostBack="True"
                                                            onkeyPress="return checkKey(event);" MaxLength="3" OnTextChanged="txtExtraProcess1_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtExtraProcess1" TargetControlID="txtExtraProcess1"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        Rate
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraRate1" runat="server" Width="100" MaxLength="6" Style="text-align: right">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="txtExtraRate1_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtExtraRate1">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Third Process
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraProcess2" runat="server" Width="100" AutoPostBack="True"
                                                            onkeyPress="return checkKey(event);" MaxLength="3" OnTextChanged="txtExtraProcess2_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtExtraProcess2"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList" MinimumPrefixLength="1"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        Rate
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtExtraRate2" runat="server" Width="100" MaxLength="6" Style="text-align: right">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="txtExtraRate2_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom" ValidChars=".0123456789" TargetControlID="txtExtraRate2">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnExtraProcSave" Style="border-radius: 7px;" Text="Save" onfocus="CtlOnFocus(this);"
                                                            onblur="CtlOnBlur(this,'btn');" onmouseout="CtlOnBlur(this,'btn');" onmouseover="CtlOnFocus(this);"
                                                            CausesValidation="false" OnClientClick="return checkNewPriorityBox();" runat="server"
                                                            OnClick="btnExtraProcSave_Click" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="center">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="Remarks_ModalPopup" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="plnRemarks" Drag="true" TargetControlID="btnAddRemarks">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnAddRemarks" runat="server" Text="ReloadGrid" CausesValidation="false"
                                Style="display: none" OnClick="btnAddRemarks_Click" />
                            <asp:Panel ID="plnRemarks" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="450PX">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div1">
                                            <div class="TitlebarLeft">
                                                Add Remarks
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table class="TDCaption">
                                                <tr>
                                                    <td align="left">
                                                        Remarks :&nbsp;
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtWholeRemark" runat="server" Width="200" MaxLength="200"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender runat="server" ID="autoRemarks" TargetControlID="txtWholeRemark"
                                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemRemarks" MinimumPrefixLength="1"
                                                            CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="ModalPopupBG"
                                DropShadow="true" runat="server" PopupControlID="Panel2" Drag="true" TargetControlID="btnCustAdd">
                            </cc1:ModalPopupExtender>
                            <asp:Button ID="btnCustAdd" runat="server" Text="a" Style="display: none" OnClick="btnCustAdd_Click" />
                            <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none"
                                BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png" Width="500PX">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <div class="popup_Titlebar" id="Div7">
                                            <div class="TitlebarLeft">
                                                Add Customer
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Name
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drpTitle" runat="server" TabIndex="1">
                                                            <asp:ListItem Value=" " Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="Mr"></asp:ListItem>
                                                            <asp:ListItem Value="Mrs"></asp:ListItem>
                                                            <asp:ListItem Value="Ms"></asp:ListItem>
                                                            <asp:ListItem Value="Dr"></asp:ListItem>
                                                            <asp:ListItem Value="M/S"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtCName" runat="server" Width="200px" TabIndex="2" MaxLength="50"></asp:TextBox>&nbsp;<span
                                                            class="span">*</span>
                                                        <asp:RequiredFieldValidator ID="RQName" runat="server" ControlToValidate="txtCName"
                                                            Display="None" ErrorMessage="Please enter customer name." SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Address
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCAddress" runat="server" Width="245" TabIndex="3" MaxLength="100"></asp:TextBox>&nbsp;<span
                                                            class="span">*</span>
                                                        <asp:RequiredFieldValidator ID="RQAddress" runat="server" ControlToValidate="txtCAddress"
                                                            Display="None" ErrorMessage="Please enter address." SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Mobile
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMobile" runat="server" Width="245" TabIndex="4"></asp:TextBox>&nbsp;
                                                        <cc1:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtMobile" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Priority
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:DropDownList ID="drpPriority" runat="server" Width="250" TabIndex="5">
                                                        </asp:DropDownList>
                                                        <input id="btnShowNewPriority" onclick="javascript: var val=document.getElementById('btnShowNewPriority').value;if(val=='Add'){document.getElementById('divNewPriority').style.visibility='Visible';document.getElementById('<%= txtNewPriority.ClientID %>').focus(); document.getElementById('btnShowNewPriority').value='Close';} else {document.getElementById('divNewPriority').style.visibility='Hidden';document.getElementById('btnShowNewPriority').value='Add';document.getElementById('<%= drpPriority.ClientID %>').focus();}"
                                                            tabindex="6" size="2" type="button" value="Add" style="background-color: #CCCCCC;
                                                            font-weight: bold; font-family: Tahoma" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <div id="divNewPriority" style="visibility: hidden;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <input id="txtNewPriority" maxlength="50" runat="server" size="37" type="text" style=""
                                                                            tabindex="7" />
                                                                    </td>
                                                                    <td>
                                                                        <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" TargetControlID="btnAddNewPriority"
                                                                            PopupControlID="Panel3" PopupPosition="Bottom" OffsetX="6" PopDelay="25" HoverCssClass="popupHover">
                                                                        </cc1:HoverMenuExtender>
                                                                        <asp:Panel ID="Panel3" runat="server" Height="50px" Width="80px" CssClass="popupMenu">
                                                                            <table style="font-size: smaller; color: Black">
                                                                                <tr>
                                                                                    <td align="left" style="color: Black">
                                                                                        To add priority
                                                                                        <br />
                                                                                        click here.
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Button ID="btnAddNewPriority" Style="border-radius: 7px;" Text="Add" onfocus="CtlOnFocus(this);"
                                                                            onblur="CtlOnBlur(this,'btn');" Enabled="True" onmouseout="CtlOnBlur(this,'btn');"
                                                                            onmouseover="CtlOnFocus(this);" CausesValidation="false" OnClientClick="return checkNewPriorityBox();"
                                                                            TabIndex="11" runat="server" OnClick="btnAddNewPriority_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap">
                                                        Area / Location
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAreaLocaton" runat="server" Width="245" TabIndex="8" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Remarks
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemarks1" runat="server" TabIndex="9" Width="245" MaxLength="100"></asp:TextBox>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                            ShowSummary="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Birth Day
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBDate" runat="server" TabIndex="10" Width="245" MaxLength="100"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtBDate_CalendarExtender" runat="server" TargetControlID="txtBDate"
                                                            Format="dd MMM yyyy">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap">
                                                        Anniversary Date
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtADate" runat="server" TabIndex="11" Width="245" MaxLength="100"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtADate_CalendarExtender" runat="server" TargetControlID="txtADate"
                                                            Format="dd MMM yyyy">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblCustSave" CssClass="SuccessMessage" runat="server" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:Button ID="btnOkay" Text="Save" runat="server" OnClick="btnOkay_Click" TabIndex="12" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <span style="color: Red; font-weight: bold; font-size: medium">
                                                            <%=MSG2 %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MSG1 %></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <asp:HiddenField ID="hdnMAC" runat="server" />
                    <asp:HiddenField ID="hdnEditItemId" runat="server" Value="-1" />
                    <asp:HiddenField ID="hdnOption" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnUpdate" runat="server" Value="0" />
                    <asp:HiddenField ID="BranchId" runat="server" Value="0" />
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnItems" runat="server" ClientIDMode="Static" />
    </form>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <asp:Label ID="hdnValues" Text="c++,java,php,coldfusion,javascript,asp,ruby,python,c"
            runat="server" />
    </asp:PlaceHolder>
</body>
</html>
