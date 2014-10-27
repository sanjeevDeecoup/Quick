<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="frmPatterns.aspx.cs" Inherits="QuickWeb.Masters.frmPatterns" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function Checkfiles() {
            var fup = document.getElementById('ctl00_ContentPlaceHolder1_fltImage').value;
            var strname = document.getElementById("<%=txtPatterns.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter pattern.");
                document.getElementById("<%=txtPatterns.ClientID %>").text == "";
                document.getElementById("<%=txtPatterns.ClientID %>").focus();
                return false;
            }
            if (fup != "") {
                var split = fup.split('.');
                if (split[1] == "png" || split[1] == "PNG" || split[1] == "jpg" || split[1] == "JPG") {
                    return true;
                }
                else {
                    alert("Upload Png or Jpg images only");
                    document.getElementById('ctl00_ContentPlaceHolder1_fltImage').focus();
                    return false;
                }
            }
        }
        function CheckBlank() {
            var strname = document.getElementById("<%=txtPatterns.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter pattern.");
                document.getElementById("<%=txtPatterns.ClientID %>").text == "";
                document.getElementById("<%=txtPatterns.ClientID %>").focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Patterns
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
                                Patterns Name
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <asp:TextBox ID="txtPatterns" runat="server" MaxLength="30" Width="200px" />
                                <span class="span">*</span>
                            </td>
                            <td rowspan="3">
                            </td>
                            <td rowspan="3">
                                <asp:Image ID="img" runat="server" BorderColor="Black" BorderStyle="Solid" Height="80px"
                                    Width="100px" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Patterns Image
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <input id="fltImage" runat="server" class="textbox" size="10" tabindex="18" type="file" />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" Width="100px" OnClick="btnUpload_Click"
                                    Visible="False" />
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
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return Checkfiles();"
                                    OnClick="btnSave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnEdit" runat="server" Text="Update" Visible="False" OnClick="btnEdit_Click"
                                    OnClientClick="return Checkfiles();" />
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return CheckBlank();"
                                    OnClick="btnSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return CheckBlank();"
                                    OnClick="btnDelete_Click" Visible="false" />
                                <asp:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to delete this record. ?"
                                    Enabled="True" TargetControlID="btnDelete">
                                </asp:ConfirmButtonExtender>
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
                    <asp:HiddenField ID="hdnImageName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="H1" style="font-weight: bolder" colspan="4">
                    <asp:Label ID="Label2" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Patterns
                    Details
                </td>
            </tr>
            <tr>
                <td colspan="4">
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
                                <asp:BoundField DataField="PatternName" HeaderText="Pattern" ReadOnly="True" SortExpression="PatternName" />
                                <asp:TemplateField HeaderText="Icon">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" Height="30Px" Width="30Px" ImageUrl='<%# Bind("PatternImage") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text='<%# Eval("PatternID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblImage" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text='<%# Eval("ImageName") %>'></asp:Label>
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