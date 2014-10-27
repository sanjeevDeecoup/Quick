<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="ShiftMaster" Title="Shift Master" CodeBehind="ShiftMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/MainStyleSheet.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript" language="javascript">

        function checkName() {
            var strname = document.getElementById("<%=txtShift.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter shift");
                document.getElementById("<%=txtShift.ClientID %>").focus();
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Shift
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
                                Shift Name
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <asp:TextBox ID="txtShift" runat="server" MaxLength="100" />
                                <span class="span">*</span>
                            </td>
                        </tr>
                    </table>
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
        </table>
        <table width="100%">
            <tr>
                <td align="left" colspan="2">
                    <table style="width: 400px;">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return checkName();" />
                            </td>
                            <td>
                                <asp:Button ID="btnEdit" runat="server" Text="Update" OnClick="btnEdit_Click" Visible="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return checkName();"
                                    OnClick="btnSearch_Click" />
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
                    <asp:Label ID="lblShift" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder" colspan="3">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Shift Details
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
                            Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                            OnPageIndexChanged="grdSearchResult_PageIndexChanged" OnSorted="grdSearchResult_OnSorted">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="ShiftID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                    SortExpression="ShiftID" />
                                <asp:BoundField DataField="ShiftName" HeaderText="Shift" ReadOnly="True" SortExpression="ShiftName" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>