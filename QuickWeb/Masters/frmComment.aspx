<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="frmComment.aspx.cs" Inherits="QuickWeb.Masters.frmComment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Comment
                    Creation
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
        <table class="TableData">
            <tr>
                <td colspan="4">
                    <table>
                        <tr>
                            <td class="TDCaption">
                                Comment Name
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <asp:TextBox ID="txtComment" runat="server" MaxLength="30" Width="200px" />
                                <span class="span">*</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="left" colspan="2">
                    <table>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return checkName('',document.getElementById('ctl00_ContentPlaceHolder1_txtComment') ,'', 'Please Enter Comment ' )"
                                    OnClick="btnSave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnEdit" runat="server" Text="Update" Visible="False" OnClientClick="return checkName();"
                                    OnClick="btnEdit_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return checkName();"
                                    OnClick="btnSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return checkName();"
                                    OnClick="btnDelete_Click" Visible="false" />
                                <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to delete this record. ?"
                                    Enabled="True" TargetControlID="btnDelete">
                                </cc1:ConfirmButtonExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnAddNew" runat="server" Text="Refresh" OnClick="btnAddNew_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    &nbsp;
                </td>
                <td align="left" style="margin-left: 40px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4" align="center">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder" colspan="3">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Comment Details
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
                <td align="left" colspan="4">
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdComment" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                            Visible="True" OnSelectedIndexChanged="grdComment_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="CommentName" HeaderText="Comment" ReadOnly="True" SortExpression="CommentName" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text='<%# Eval("CommentID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>