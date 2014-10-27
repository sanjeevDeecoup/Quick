<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemNewMaster.aspx.cs"
    Inherits="QuickWeb.Masters.ItemNewMaster" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Item Master</title>
    <link href="../css/ItemMaster.css" rel="stylesheet" type="text/css" />
    <%--<link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />--%>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../JavaScript/ItemMaster.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainContainer">
        <div id="hiddensubitem">
        </div>
        <div id="hiddenvariation">
        </div>
        <div id="hiddencategory">
        </div>
        <div id="NewItemgroup">
            <ul id="unorderlist">
                <li>
                    <label for="txtName">
                        Name :</label>
                    <input type="text" id="txtName" runat="server" maxlength="30" />
                    <%--<asp:RequiredFieldValidator ID="reqItemName" runat="server" Display="None" EnableClientScript="true"
                        ControlToValidate="txtName" ErrorMessage="Item Name is a required field and cannot be left blank"></asp:RequiredFieldValidator>
                    <asp:ValidationSummary ID="validItemName" runat="server" EnableClientScript="true"
                        ShowMessageBox="true" ShowSummary="false" />--%>
                </li>
                <li>
                    <label for="txtCode">
                        Code :</label>
                    <input type="text" id="txtCode" runat="server" maxlength="4" />
                    <%--<asp:RequiredFieldValidator ID="reqItemCode" runat="server" Display="None" EnableClientScript="true"
                        ControlToValidate="txtCode" ErrorMessage="Item Code is a required field and cannot be left blank"></asp:RequiredFieldValidator>--%>
                </li>
                <li>
                    <label for="fileIcon">
                        Icon :</label>
                    <input type="file" id="fileIcon" name="pic" runat="server" accept="image/*" />
                    <%--                    <input id="btnUploadImage" type="button" runat="server" value="Upload Image" />--%>
                    <input type="hidden" id="hdnImageName" runat="server" value="" />
                </li>
                <li>
                    <div id="iconContainer" class="itemIconContainer">
                        <img src="" id="tempImage" runat="server" alt=""></img>
                        <%--<input type="hidden" runat="server" id="hdnImagePath" value="" />--%>
                    </div>
                </li>
            </ul>
            <div id="miscPanel">
                <div class="container3">
                    <ul id="subitemdetail">
                        <li>
                            <label for="txtSubItem">
                                SubItem :</label>
                            <input type="text" id="txtSubItem" sourcediv="#hiddensubitem" rel="autoComplete"
                                autocomplete="off" oncustomselect="SelectSubItems" extrafilter="[subitemids='']" />
                        </li>
                        <li>
                            <input type="image" src="../images/add.png" />
                        </li>
                    </ul>
                    <div id="undersubitem">
                        <div class="header">
                            <div class="icol1">
                                Icon</div>
                            <div class="icol2">
                                Name</div>
                            <div class="icol3">
                                Default</div>
                        </div>
                        <div class="details">
                            <!-- Add selected subitems here -->
                        </div>
                    </div>
                </div>
                <div class="container4">
                    <ul id="variationdetail">
                        <li>
                            <label for="txtVariation">
                                Variation :</label>
                            <input type="text" id="txtVariation" sourcediv="#hiddenvariation" rel="autoComplete"
                                autocomplete="off" oncustomselect="SelectVariations" />
                        </li>
                        <li>
                            <input type="image" src="../images/add.png" />
                        </li>
                    </ul>
                    <div id="undervariation">
                        <div class="header">
                            <div class="icol1">
                                Icon</div>
                            <div class="icol2">
                                Name</div>
                            <div class="icol3">
                                Default</div>
                        </div>
                        <div class="details">
                            <!-- Add selected variations here -->
                        </div>
                    </div>
                </div>
                <div class="container5">
                    <ul id="categorydetail">
                        <li>
                            <label for="txtCategory">
                                Category :</label>
                            <input type="text" id="txtCategory" sourcediv="#hiddencategory" rel="autoComplete"
                                autocomplete="off" oncustomselect="SelectCategories" />
                        </li>
                        <li>
                            <input type="image" src="../images/add.png" />
                        </li>
                    </ul>
                    <div id="undercategory">
                        <div class="header">
                            <div class="icol1">
                                Icon</div>
                            <div class="icol2">
                                Name</div>
                            <div class="icol3">
                                Default</div>
                        </div>
                        <div class="details">
                            <!-- Add selected categories here -->
                        </div>
                    </div>
                </div>
                <div id="list">
                </div>
                <div id="test">
                </div>
                <div id="tooltipDiv">
                </div>
            </div>
            <input type="hidden" id="hdnCategories" name="hdnCategories" value="" runat="server" />
            <input type="hidden" id="hdnSubItems" name="hdnSubItems" value="" runat="server" />
            <input type="hidden" id="hdnVariations" name="hdnVariations" value="" runat="server" />
            <label id="lblError" runat="server" class="ErrorMessage">
            </label>
            <div id="rightCommands">
                <input type="button" id="btnNew" value="New" onclick="ResetForm();" />
                <input type="button" id="btnSave" value="Save" onclick="SaveItem();" runat="server" />
                <input type="button" id="btnDelete" value="Delete" runat="server" onserverclick="btnDelete_ServerClick" />
                <asp:HiddenField ID="hdnItemID" runat="server" Value="0" />
            </div>
            <div id="itemsGrid">
                <table id="itemsTable" cellpadding="0" cellspacing="0" border="1">
                    <thead>
                        <tr>
                            <th class="col1">
                                Item ID
                            </th>
                            <th class="col2">
                                Item Name
                            </th>
                            <th class="col3">
                                Item Code
                            </th>
                            <th class="col4">
                                Sub Items
                            </th>
                            <th class="col5">
                                Variations
                            </th>
                            <th class="col6">
                                Categories
                            </th>
                            <th class="col7">
                                Image
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <%--<div id="griddiv">
                <asp:GridView ID="grdview" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    EmptyDataText="There are No Items" ShowFooter="True" OnSelectedIndexChanged="grdview_SelectedIndexChanged">
                    <Columns>
                        <asp:CommandField ShowSelectButton="true" />
                        <asp:TemplateField Visible="true" HeaderText="Item Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode")  %>'></asp:Label>
                                <asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("ItemId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:BoundField DataField="ItemCode" HeaderText="Item Code" ReadOnly="true"></asp:BoundField>
                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" ReadOnly="true"></asp:BoundField>
                        <asp:BoundField DataField="ItemImage" HeaderText="Image" ReadOnly="true"></asp:BoundField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    Text='<%# Eval("ItemId")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>--%>
            <div id="lbldiv">
            </div>
        </div>
    </div>
    </form>
</body>
</html>