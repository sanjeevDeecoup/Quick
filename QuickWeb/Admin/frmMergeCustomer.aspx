<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="frmMergeCustomer.aspx.cs" Inherits="QuickWeb.Admin.frmMergeCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function checkName() {
            var strname = document.getElementById("<%=txtCustomerSearch.ClientID %>").value.trim().length;
            var stradd = document.getElementById("<%=txtDuplicateCustomer.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter main Customer.");
                document.getElementById("<%=txtCustomerSearch.ClientID %>").focus();
                return false;
            }
            if (stradd == "" || stradd.length == 0) {
                alert("Please enter duplicate Customer.");
                document.getElementById("<%=txtDuplicateCustomer.ClientID %>").focus();
                return false;
            }
            return confirm('Are you sure you want merge this customer?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableData">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <fieldset class="Fieldset">
                    <table class="TableData">
                        <tr>
                            <td height="30px">
                            </td>
                        </tr>
                        <tr>
                            <td class="Legend">
                                Orginal Customer
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="txtCustomerSearch" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                                    Width="450px" Height="20px" ToolTip="Please enter customer name." OnTextChanged="txtCustomerSearch_TextChanged"
                                    TabIndex="1"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerSearch"
                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" DelimiterCharacters=""
                                    Enabled="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                                <asp:GridView ID="grdCustomerSearch" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
                                    CssClass="mGrid">
                                    <Columns>
                                        <asp:TemplateField HeaderText="LedgerName" SortExpression="LedgerName">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlBtnLedgerName" runat="server" Text='<%#Bind("CustomerName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Address" SortExpression="CustomerAddress">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlBtnCustomerAddress" runat="server" Text='<%#Bind("CustomerAddress") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Due" SortExpression="TotalDue">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlBtnCustomerTotalDue" runat="server" Text='<%#Bind("DuePayment") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td width="2px">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="width: 50Px">
            </td>
            <td>
                <fieldset class="Fieldset">
                    <table class="TableData">
                        <tr>
                            <td height="30px">
                            </td>
                        </tr>
                        <tr>
                            <td class="Legend">
                                Duplicate Customer
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="txtDuplicateCustomer" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                                    Width="450px" Height="20px" ToolTip="Please enter customer name." OnTextChanged="txtDuplicateCustomer_TextChanged"
                                    TabIndex="2"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtDuplicateCustomer"
                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" DelimiterCharacters=""
                                    Enabled="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                                <asp:GridView ID="GridViewDuplicate" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
                                    CssClass="mGrid">
                                    <Columns>
                                        <asp:TemplateField HeaderText="LedgerName" SortExpression="LedgerName">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlBtnLedgerName" runat="server" Text='<%#Bind("CustomerName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Address" SortExpression="CustomerAddress">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlBtnCustomerAddress" runat="server" Text='<%#Bind("CustomerAddress") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Due" SortExpression="TotalDue">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlBtnCustomerTotalDue" runat="server" Text='<%#Bind("DuePayment") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td width="2px">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <tr>
        <td height="50px">
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="ErrorMessage"></asp:Label>
        </td>
    </tr>
    <tr>
        <td height="50px">
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Button ID="btnMerge" runat="server" Text="Merge Customer" OnClientClick="return checkName();"
                OnClick="btnMerge_Click" TabIndex="3" Width="250Px" />
            <asp:HiddenField ID="hdnNewCustomer" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOldCustomer" runat="server" Value="0" />
        </td>
    </tr>
</asp:Content>