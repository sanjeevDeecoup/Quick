<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="Masters_NewItemMaster" Title="New Item Master" CodeBehind="NewItemMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkEntry() {
            var brcode = document.getElementById("<%=txtItemName.ClientID %>").value.trim().length;
            var brname = document.getElementById("<%=txtItemCode.ClientID %>").value.trim().length;
            if (brcode == "" || brcode.length == 0) {
                alert("Please enter Item Name.");
                document.getElementById("<%=txtItemName.ClientID %>").focus();
                return false;
            }
            if (brname == "" || brname.length == 0) {
                alert("Please enter Item code.");
                document.getElementById("<%=txtItemCode.ClientID %>").focus();
                return false;
            }
        }

        $(document).ready(function (event) {


//            $(document).keypress(function (event) {

//                var textval = $('#ctl00_ContentPlaceHolder1_txtItemNameSearch').val();
//                var keycode = (event.keyCode ? event.keyCode : event.which);
//                if (keycode == '13') {
//                    if (($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtProcess') || ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_txtDefaultRate'))
//                        return false;
//                    if (textval == "") {
//                        return false;
//                    }
//                    else {
//                        alert("hii");
//                        document.getElementById("ctl00_ContentPlaceHolder1_btnTemp").click();

//                    }
//                }

//            });



            $('#ctl00_ContentPlaceHolder1_rdbinch, #ctl00_ContentPlaceHolder1_rdbpanel').click(function (e) {
                if ($(this).is(':checked')) {
                    $('#ctl00_ContentPlaceHolder1_txtSubItem1').attr('disabled', true);
                    $('#ctl00_ContentPlaceHolder1_txtItemSubQty').attr('disabled', true);
                    $('#ctl00_ContentPlaceHolder1_lstSubItem').attr('disabled', true);
                }
                else {
                    $('#ctl00_ContentPlaceHolder1_txtSubItem1').attr('disabled', true);
                    $('#ctl00_ContentPlaceHolder1_txtItemSubQty').attr('disabled', true);
                    $('#ctl00_ContentPlaceHolder1_lstSubItem').attr('disabled', true);
                }
            });

            $('#ctl00_ContentPlaceHolder1_rdbPcs').click(function (e) {
                if ($(this).is(':checked')) {
                    $('#ctl00_ContentPlaceHolder1_txtSubItem1').attr('disabled', false);
                    $('#ctl00_ContentPlaceHolder1_txtItemSubQty').attr('disabled', false);
                    $('#ctl00_ContentPlaceHolder1_lstSubItem').attr('disabled', false);
                }
                //                else {
                //                    $('#ctl00_ContentPlaceHolder1_txtSubItem1').attr('disabled', true);
                //                    $('#ctl00_ContentPlaceHolder1_lstSubItem').attr('disabled', true);
                //                }
            });


        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#txtItemNameSearch').on('keyup', function (event) {
                if (event.which == '13') {
                    if ($('#txtItemNameSearch').val() == "") {
                        $('#lblErr').text('Please enter item name.');
                        return false;
                    }
                    else {
                        var itemdetail = $('#txtItemNameSearch').val();
                        var Fullitemdetail = itemdetail.split("-");                       
                        if (Fullitemdetail.length == 1) {                           
                            $('#lblErr').text('Please enter valid item name.');
                            return false;
                        }
                        document.getElementById("ctl00_ContentPlaceHolder1_btnTemp").click();                       
                    }
                }
                
            });
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="Create Item " ForeColor="#FF9933"></asp:Label>
                    <span class="" style="font-size: 12Px">Create and Maintain a List of all Clothes
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="TDCaption">
                                Search Item
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                <asp:TextBox ID="txtItemNameSearch" runat="server" Width="350px" Height="20Px" TabIndex="1" ClientIDMode="Static"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtItemNameSearch"
                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="5">
                                <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" class="H1" style="font-weight: bolder">
                                <asp:Label ID="lblnan" runat="server" Text="Add Item" ForeColor="Black"></asp:Label>
                                <span class="" style="font-size: 12Px">Please Note: Item can’t be deleted
                                    once the first order for particular item is booked. </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                &nbsp;
                            </td>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="TDCaption">
                                Item Name
                            </td>
                            <td class="TDCaption" nowrap="nowrap">
                                <asp:TextBox ID="txtItemName" runat="server" MaxLength="50" TabIndex="2" Width="200px"
                                    ClientIDMode="Static" CssClass="Textbox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtItemName_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtItemName" ValidChars=" abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890">
                                </cc1:FilteredTextBoxExtender>
                                <span class="span">*</span> Item Code
                                <asp:TextBox ID="txtItemCode" runat="server" Width="50Px" CssClass="Textbox" ToolTip="Please enter item code"
                                    ClientIDMode="Static" TabIndex="3" MaxLength="4"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                    TargetControlID="txtItemCode" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                </cc1:FilteredTextBoxExtender>
                                 &nbsp;&nbsp;<asp:Label ID="Label2" Text="Rate List" runat="server" AssociatedControlID="ddlRateList" ></asp:Label>&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlRateList" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlRateListChange" TabIndex="4">
                                </asp:DropDownList>
                                 
                            </td>
                            <td class="TDCaption">
                                Default Process
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                <asp:TextBox ID="txtProcess" runat="server" AutoComplete="off" TabIndex="5" AutoPostBack="True"
                                    ClientIDMode="Static" OnTextChanged="txtProcess_TextChanged"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1_txtProcess" TargetControlID="txtProcess"
                                    EnableCaching="false" ServicePath="~/AutoComplete.asmx" ServiceMethod="GetProcessList"
                                    MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True"
                                    CompletionListCssClass="AutoExtenderProcessList" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td class="TDCaption">
                                Default Rate
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                <asp:TextBox ID="txtDefaultRate" runat="server" TabIndex="6" ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftd" runat="server" TargetControlID="txtDefaultRate"
                                    FilterMode="ValidChars" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>
                               
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" class="TDCaption">
                                Measurement Unit
                            </td>
                            <td class="TDDot" nowrap="nowrap">
                                <asp:RadioButton ID="rdbPcs" Checked="true" Text="Pcs" CssClass="Legend" runat="server"
                                    GroupName="a" TabIndex="7" />
                                <asp:RadioButton ID="rdbinch" Text="Len x Breadth" CssClass="Legend" runat="server"
                                    GroupName="a" TabIndex="8" />
                                <asp:RadioButton ID="rdbpanel" Text="Panel" CssClass="Legend" runat="server" GroupName="a"
                                    TabIndex="9" />
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Sub Items
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <asp:TextBox ID="txtItemSubQty" runat="server" MaxLength="2" TabIndex="10" Width="50px"
                                    Text="1" ToolTip="No. of sub items for Item" CssClass="Textbox" AutoPostBack="true"
                                    OnTextChanged="txtItemSubQty_TextChanged"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtItemSubQty_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtItemSubQty" ValidChars="1234567890">
                                </cc1:FilteredTextBoxExtender>
                                <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="False"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" class="TDCaption">
                                <asp:Label ID="lblItem1" runat="server" Visible="False" CssClass="TDCaption">Sub Items</asp:Label>
                                &nbsp;
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtSubItem1" runat="server" Width="150px" TabIndex="11" MaxLength="30"
                                    CssClass="Textbox" AutoPostBack="true" OnTextChanged="txtSubItem1_TextChanged" />
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:ListBox ID="lstSubItem" runat="server" Width="150px" TabIndex="5" CssClass="Textbox"
                                    AutoPostBack="true" OnSelectedIndexChanged="lstSubItem_OnSelectedIndexChanged">
                                </asp:ListBox>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td nowrap="nowrap" class="TDCaption">
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return checkEntry();"
                                    ClientIDMode="Static" OnClick="btnSave_Click" TabIndex="12" />
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClientClick="return checkEntry();"
                                    ClientIDMode="Static" TabIndex="13" OnClick="btnUpdate_Click" Visible="False" />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return checkEntry();"
                                    OnClick="btnSearch_Click" TabIndex="14" />
                                <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click"
                                    TabIndex="15" />
                                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure, you want to delete this item?');"
                                    Text="Delete" TabIndex="16" />
                                <asp:Button ID="btnAddNew" runat="server" Text="Refresh" OnClick="btnAddNew_Click"
                                    TabIndex="17" />
                                    <asp:Button runat="server" ID="btnCopy" Text="Rename" ClientIDMode="Static" OnClick="BtnCopyClick" Visible="False"/> 

                                                                  
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:HiddenField ID="hdntemp" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnItemCode" runat="server" Value="0" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 7px">
                    &nbsp;
                </td>
                <td class="H1" style="font-weight: bolder" align="center">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" ClientIDMode="Static" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="SuccessMessage" />
                    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Item Details
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 700px;">
                        <asp:GridView ID="grdSearchResult" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" DataSourceID="SqlGridSource" EmptyDataText="There are no data records to display."
                            Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                            PageSize="50" OnPageIndexChanging="grdSearchResult_PageIndexChanging" OnSorted="grdSearchResult_OnSorted">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" ReadOnly="True" SortExpression="ItemName" />
                                <asp:BoundField DataField="NumberOfSubItems" HeaderText="Sub Items" SortExpression="NumberOfSubItems" />
                                <asp:BoundField DataField="ItemID" HeaderText="ItemID" InsertVisible="False" ReadOnly="True"
                                    SortExpression="ItemID"></asp:BoundField>
                                <asp:BoundField DataField="ItemCode" HeaderText="Item Code" InsertVisible="False"
                                    ReadOnly="True" SortExpression="ItemCode" HeaderStyle-Width="70Px"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:SqlDataSource ID="SqlGridSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"></asp:SqlDataSource>
                    <asp:Button ID="btnTemp" runat="server" OnClick="btnTemp_Click" />
                </td>
            </tr>
        </table>  
         <asp:Panel runat="server" ID="pnlCopyRateList" CssClass="modalPopup" Style="display: none;"
            ClientIDMode="Static" BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png"
            Width="340px">
            <div class="popup_Titlebar" id="Div2">
                <div class="TitlebarLeft">
                    Item Rename
                </div>
            </div>
            <div class="popup_Body">
                <table class="TableData">
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                            <asp:Label ID="Label3" Text="Please enter item name" runat="server"></asp:Label>&nbsp;&nbsp;
                            <asp:TextBox ID="txtNewListName" runat="server" ClientIDMode="Static" autocomplete="off" onkeyPress="return checkKey(event);"></asp:TextBox><br />
                            <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtNewListName" FilterMode="ValidChars"
                             ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ "></cc1:FilteredTextBoxExtender>
                            <asp:Button ID="btnCreateNewList" runat="server" ClientIDMode="Static" Text="Rename" OnClick="btnCreateNewListClick"  
                                 />
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel> 
        <asp:DropDownList ID="ddlRateList1" runat="server" ClientIDMode="Static" style="visibility:hidden">
                    </asp:DropDownList>     
    </fieldset>   
     <script type="text/javascript">
         $(function (e) {

             $('#btnCopy, #btnBlankList').click(function (e) {

                 $('#pnlCopyRateList').dialog({ width: 400, modal: true }).parent().appendTo($('form:first'));
                 txtNewListName.focus();               

                 return false;
             });


             $('#btnCreateNewList').click(function (e) {
                 /* To do : verify name not already exist and is valid, like stopping special character etc.  */
                 var unique = true;

                 if (!txtNewListName.value.trim()) {
                     txtNewListName.focus();
                     return false;
                 }

                 $('#ddlRateList1 option').each(function (i, v) {
                     if (unique) {
                         unique = v.textContent.toLowerCase() != txtNewListName.value.toLowerCase().trim();
                     }
                 });

                 if (!unique) {
                     alert('This name already exists, please enter a different name');
                     txtNewListName.focus();
                     txtNewListName.select();
                     return false;
                 }

             });

         });
    </script>   
        <script language="javascript" type="text/javascript">
            function checkKey(e) {
                var targ;
                var code;
                if (!e) var e = window.event;
                if (e.target) targ = e.target;
                if (e.keyCode) code = e.keyCode;
                else if (e.which) code = e.which;
                else if (e.srcElement) targ = e.srcElement;
                if (code == 13) {
                    document.getElementById("btnCreateNewList").click();
                }
            }
    </script>
      <script type="text/javascript">

          $(function () {

              $('#btnSave, #btnUpdate').on('click.QDC', verifyProcessAndRate);

              function verifyProcessAndRate() {
                  var 
                    rate = $('#txtDefaultRate').val().trim(),
                    process = $('#txtProcess').val().trim();

                  if ($('#txtItemName').val().trim() === '' || $('#txtItemCode').val().trim() === '')
                      return false;

                  // if rate is specified and process is not, force the user to specify the process too!
                  if (rate !== '' && process === '') {
                      alert('Please enter a default process');
                      $('#txtProcess').focus();
                      return false;
                  }

                  if (process !== '' && rate === '') {
                      $('#txtDefaultRate').val('0');
                  }
              }

          });
    
    </script>
</asp:Content>
