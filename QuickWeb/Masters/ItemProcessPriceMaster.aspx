<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="ItemProcessPriceMaster" Title="Item Master" CodeBehind="ItemProcessPriceMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .tooltip-inner
        {
            max-width: 340px;
            width: 340px;
            background-color: #F1F1F1;
            text-align: left;
            font-size: 14px;
            color: Black;
            border: 1px solid #C0C0A0;
        }               
    </style>
    <script type="text/javascript">
        $(document).keypress(function (event) {
            var textval = $('#ctl00_ContentPlaceHolder1_txtItemNameSearch').val();
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                if (textval == "") {
                    return false;
                }
                else {
                    if ($(event.target).attr('id') == 'txtProcessRate') {
                        return false;
                    }
                    document.getElementById("ctl00_ContentPlaceHolder1_btnTemp").click();
                }
            }
        });
        $(document).keydown(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '119') {
                document.getElementById("ctl00_ContentPlaceHolder1_btnTempSave").click();
            }
        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (e) {
            $("#EditInfo").tooltip({
                title: 'Select a rate list from drop down above and use this to change prices for garments.',
                html: true
            });

            $("#PrintInfo").tooltip({
                title: 'Click to view price or export it to excel.',
                html: true
            });

            $("#CopyInfo").tooltip({
                title: 'Click here if you want to create new price list by copying a predefined price list.',
                html: true
            });

            $("#BlankInfo").tooltip({
                title: 'Click here to create blank Price list that you can modify.',
                html: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row-fluid div-margin">
        <div class="col-sm-7">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Price List</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid">
                        <div class="col-sm-5  nopadding">
                            <div class="input-group">
                                <span class="input-group-addon">Price List</span>
                                <asp:DropDownList ID="ddlRateList" runat="server" ClientIDMode="Static" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton runat="server" ID="btnEdit" ClientIDMode="Static" OnClick="BtnEditClick"
                        EnableTheming="false" CssClass="btn btn-primary "><i class="fa fa-pencil-square-o"></i>&nbsp;Edit</asp:LinkButton>
                    <i id="EditInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor">
                    </i>&nbsp; &nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="btnPrint" ClientIDMode="Static" OnClick="BtnPrintClick"
                        EnableTheming="false" CssClass="btn btn-primary"><i class="fa fa-print"></i>&nbsp;View Prices</asp:LinkButton>
                    <i id="PrintInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor">
                    </i>&nbsp; &nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="btnCopy" ClientIDMode="Static" OnClick="BtnCopyClick"
                        EnableTheming="false" CssClass="btn btn-primary"><i class="fa fa-files-o"></i>&nbsp;Copy and Create</asp:LinkButton>
                    <i id="CopyInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor">
                    </i>&nbsp; &nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="btnBlankList" ClientIDMode="Static" EnableTheming="false"
                        CssClass="btn btn-primary"><i class="fa fa-file-o"></i>&nbsp;Create Blank Rate List</asp:LinkButton>
                    <i id="BlankInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor">
                    </i>
                </div>
            </div>
        </div>
    </div>
    <fieldset class="Fieldset">
        <table class="TableData" style="display: none;">
            <tr>
                <td>
                    <table class="Table">
                        <tr>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Item Name :&nbsp;
                            </td>
                            <td align="left" nowrap="nowrap">
                                <asp:TextBox ID="txtItemNameSearch" runat="server" Width="350px" Height="20Px"></asp:TextBox><span
                                    class="span"> Search your item here.</span>
                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtItemNameSearch"
                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                <%=stylebutton %>
                            </td>
                            <td align="left" nowrap="nowrap">
                                <asp:DropDownList ID="drpItemNames" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="drpItemNames_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Button ID="btnShowDetails" runat="server" OnClick="btnShowDetails_Click" Text="Show" />
                                <cc1:ListSearchExtender ID="drpItemNames_ListSearchExtender" runat="server" Enabled="True"
                                    TargetControlID="drpItemNames">
                                </cc1:ListSearchExtender>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="TDCaption">
                                Import Rate List&nbsp; :
                            </td>
                            <td align="left" nowrap="nowrap">
                                <asp:FileUpload ID="flpUpload" runat="server" />
                                <asp:Button ID="btnImportRateList" runat="server" Text="Import Rate List" TabIndex="14"
                                    OnClick="btnImportRateList_Click" />
                                <cc1:ConfirmButtonExtender ID="btnImportRateList_ConfirmButtonExtender" runat="server"
                                    ConfirmText="Do you want to import this file ?" Enabled="True" TargetControlID="btnImportRateList">
                                </cc1:ConfirmButtonExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save (F8)"
                                    Visible="false" />
                                <asp:Button ID="btnRateList" runat="server" Text="Print Rate List" TabIndex="13"
                                    OnClick="btnRateList_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%" style="display: none;">
            <tr>
                <td align="center">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Rate List Details
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdTable" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                            <Columns>
                                <asp:TemplateField HeaderText="Process">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnProcessCode" runat="server" Value='<%# Bind("ProcessCode") %>' />
                                        <asp:Label ID="lblProcessName" runat="server" Text='<%# Bind("ProcessName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtProcessRate" runat="server" MaxLength="6" Width="50px" ClientIDMode="Static">0</asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr style="display: none;">
                <td align="left">
                    <asp:Button ID="btnTemp" runat="server" OnClick="btnTemp_Click" />
                    <asp:Button ID="btnTempSave" runat="server" OnClick="btnTempSave_Click" />
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdnIsBlankList" ClientIDMode="Static" />
    </fieldset>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-dialog-center">
            <div class="modal-content">
                <div class="panel panel-primary popup2">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            New Price List</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row-fluid">
                            <div class="col-sm-7  nopadding">
                                <div class="input-group">
                                    <span class="input-group-addon">Price List</span>
                                    <asp:TextBox ID="txtNewListName" CssClass="form-control" placeholder="Kindly enter new Price list label" MaxLength="50"
                                        runat="server" ClientIDMode="Static" autocomplete="off"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNewListName"
                                        FilterMode="InvalidChars" InvalidChars="`~;:,-">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <asp:LinkButton ID="btnCreateNewList" CssClass="btn btn-primary" runat="server" ClientIDMode="Static"
                            EnableTheming="false" OnClick="BtnCreateNewListClick"><i class="fa fa-floppy-o"></i>&nbsp;Create & Save</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function (e) {

            $('#btnCopy, #btnBlankList').click(function (e) {
                // $('#pnlCopyRateList').dialog({ width: 400, modal: true }).parent().appendTo($('form:first'));
                $('#myModal').modal('toggle');
                var myVar = setInterval(function () { SetFocusControl() }, 500);
                function SetFocusControl() {                  
                    txtNewListName.focus();   
                    clearInterval(myVar);
                    return false;
                }
                if (e.target.id === 'btnBlankList') {
                    hdnIsBlankList.value = 'true';
                }
                else {
                    hdnIsBlankList.value = 'false';
                }
                return false;
            });


            $('#btnCreateNewList').click(function (e) {
                /* To do : verify name not already exist and is valid, like stopping special character etc.  */
                var unique = true;

                if (!txtNewListName.value) {
                    txtNewListName.focus();
                    return false;
                }

                $('#ddlRateList option').each(function (i, v) {
                    if (unique) {
                        unique = v.textContent.toLowerCase() != txtNewListName.value.toLowerCase();
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
</asp:Content>
