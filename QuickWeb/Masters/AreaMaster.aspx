<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="AreaMaster.aspx.cs" Inherits="QuickWeb.Masters.AreaMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Area
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
                <td class="TDCaption">
                    Area Name
                </td>
                <td>
                    <asp:TextBox ID="txtArea" runat="server" MaxLength="100" CssClass="Textbox" />
                    <span class="span">*</span>
                </td>
            </tr>
            <tr>
                <td class="TDCaption">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="TDCaption">
                </td>
                <td>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td class="TDCaption">
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return checkName();" />
                    <asp:Button ID="btnEdit" runat="server" Text="Update" OnClick="btnEdit_Click" OnClientClick="return checkName();"
                        Visible="false" />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return checkName();"
                        OnClick="btnSearch_Click" />
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
                    <asp:Label ID="lblArea" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Area Details
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
                                <asp:BoundField DataField="AreaID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                    SortExpression="AreaID" />
                                <asp:BoundField DataField="Area" HeaderText="Area" ReadOnly="True" SortExpression="Area" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td align="left">
                    <asp:SqlDataSource ID="SqlGridSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [AreaID], [Area] FROM [AreaMaster] WHERE Area LIKE @AreaLike">
                        <SelectParameters>
                            <asp:Parameter Name="AreaLike" DefaultValue="%" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>--%>
        </table>
    </fieldset>
</asp:Content>