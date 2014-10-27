﻿<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="frmColors.aspx.cs" Inherits="QuickWeb.Masters.frmColors" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function Checkfiles() {
            var fup = document.getElementById('ctl00_ContentPlaceHolder1_fltImage').value;
            var strname = document.getElementById("<%=txtColor.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter color.");
                document.getElementById("<%=txtColor.ClientID %>").text == "";
                document.getElementById("<%=txtColor.ClientID %>").focus();
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
            var strname = document.getElementById("<%=txtColor.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                alert("Please enter pattern.");
                document.getElementById("<%=txtColor.ClientID %>").text == "";
                document.getElementById("<%=txtColor.ClientID %>").focus();
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Color
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
                                Color Name
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <asp:TextBox ID="txtColor" runat="server" MaxLength="30" Width="200px" />
                                <span class="span">*</span>
                            </td>
                            <td>
                            </td>
                            <td rowspan="3">
                                <asp:Image ID="imgStudentPhoto" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    Height="80px" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Color Image
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td align="left" style="margin-left: 40px">
                                <input id="fltImage" runat="server" class="textbox" onchange="return fupStudentPhoto_onchange();"
                                    size="10" tabindex="18" type="file" />
                            </td>
                            <td>
                                <asp:Button ID="btnupload" runat="server" Text="Upload" Visible="false" OnClick="btnupload_Click" />
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
                                <asp:Button ID="btnEdit" runat="server" Text="Update" Visible="False" OnClientClick="return Checkfiles();"
                                    OnClick="btnEdit_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return CheckBlank();"
                                    OnClick="btnSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                                    Visible="false" />
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
                <td class="H1" style="font-weight: bolder" colspan="4">
                    <asp:Label ID="Label2" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Color
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
                        <asp:GridView ID="grdColor" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                            Visible="True" OnSelectedIndexChanged="grdComment_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="ColorName" HeaderText="Color" ReadOnly="True" SortExpression="ColorName" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text='<%# Eval("ColorID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Icon">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" Height="30Px" Width="30Px" ImageUrl='<%# Bind("colorimage") %>'
                                            runat="server" />
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
                <asp:HiddenField ID="upimg" runat="server" />
            </tr>
        </table>
    </fieldset>
</asp:Content>