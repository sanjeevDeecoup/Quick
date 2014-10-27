<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="DrawlMaster" Title="Drawl Master" CodeBehind="DrawlMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">

        function checkName() {
            var strname = document.getElementById("<%=txtParent.ClientID %>").value;
            if (strname == "") {
                alert("Please enter parent rack.");
                document.getElementById("<%=txtParent.ClientID %>").focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Stock
                    Location Creation
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
                <td>
                    <table class="TableData">
                        <tr>
                            <td class="TDCaption">
                                Rack Name
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDrawl" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Parent Rack
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <asp:TextBox ID="txtParent" runat="server"></asp:TextBox>
                                <span class="span">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="right">
                            </td>
                            <td align="left" style="margin-left: 40px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="left" colspan="2">
                    <table style="width: 400px;">
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return checkName();"
                                    Enabled="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" Enabled="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return checkName();"
                                    OnClick="btnSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure, you want to delete this process?');"
                                    Text="Delete" Enabled="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnAddNew" runat="server" Text="Refresh" OnClick="btnAddNew_Click" />
                            </td>
                            <td>
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
                <td align="center">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Stock Location Details
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
                        <asp:GridView ID="grdSearchResult" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                            OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged" OnPageIndexChanged="grdSearchResult_PageIndexChanged"
                            OnSorted="grdSearchResult_OnSorted">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="Id" HeaderText="SrNo" ReadOnly="True" SortExpression="Id" />
                                <asp:BoundField DataField="DrawlName" HeaderText="Rack" ReadOnly="True" SortExpression="DrawlName" />
                                <asp:BoundField DataField="ParentDrawl" HeaderText="Parent" ReadOnly="True" SortExpression="ParentDrawl" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnSelectedProcessCode" runat="server" />
</asp:Content>