<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="FrmVariation.aspx.cs" Inherits="QuickWeb.Masters.FrmVariation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                    <td class="H1" style="font-weight: bold">
                        <asp:Label ID="lblHeading" runat="server" Text="DrySoft" ForeColor="#FF9933"></asp:Label>
                        Variation Master
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
                <td class="TDCaption">
                    VariationName
                </td>
                <td>
                    <asp:TextBox ID="txtVariName" runat="server">
                    </asp:TextBox>
                    <span class="span">*</span>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td class="TDCaption">
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return checkName('',document.getElementById('ctl00_ContentPlaceHolder1_txtVariName') ,'', 'Please Enter Variation ' )" />
                    <asp:Button ID="btnEdit" runat="server" Text="Update" Visible="false" OnClientClick="return checkName('',document.getElementById('ctl00_ContentPlaceHolder1_txtVariName') ,'', 'Please Enter Variation ' )"
                        OnClick="btnEdit_Click1" />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return checkName('',document.getElementById('ctl00_ContentPlaceHolder1_txtVariName') ,'', 'Please Enter Variation ' )"
                        OnClick="btnSearch_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return checkName('',document.getElementById('ctl00_ContentPlaceHolder1_txtVariName') ,'', 'Please Enter Variation ' )"
                        OnClick="btnDelete_Click" Visible="false" />
                    <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to delete this record. ?"
                        Enabled="True" TargetControlID="btnDelete">
                    </cc1:ConfirmButtonExtender>
                    <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click" />
                    <asp:Button ID="btnAddNew" runat="server" Text="Refresh" OnClick="btnAddNew_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblvariation" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Variation Details
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
                <td class="TDCaption">
                </td>
                <td>
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdSearchResult" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                            Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text='<%# Eval("VariationId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VariationName" HeaderText="Variation" ReadOnly="True"
                                    SortExpression="Variation" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>