<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="smsconfig.aspx.cs" Inherits="QuickWeb.Config_Setting.smsconfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function fetchdropvalue() {
            var tex = document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabPanel_Select1').value;
            if (tex != "Select") {
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabPanel_txtmessage').value = document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabPanel_txtmessage').value + tex + " ";
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabPanel_txtmessage').focus();
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabPanel_txtmessage').focus();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <legend class="Legend">Dry Soft SMS Configuration</legend>
        <table class="TableData">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnsmstemplate" runat="server" Text="SMS Setup" Width="150" Style="text-align: left"
                                    OnClick="btnsmstemplate_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnsmsserversetting" runat="server" Text="Gateway Setup" Width="150"
                                    Style="text-align: left" OnClick="btnsmsserversetting_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btndefaultsms" runat="server" Text="SMS Preview" Width="150" Visible="false"
                                    Style="text-align: left" OnClick="btndefaultsms_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="tdSetsmstemplate" runat="server">
                    <cc1:TabContainer ID="TabContainer1" runat="server">
                        <cc1:TabPanel ID="TabPanel" runat="server" HeaderText="SMS Template Setting">
                            <HeaderTemplate>
                                SMS Setup
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblSmstemplateerror" runat="server" CssClass="ErrorMessage" EnableViewState="False" />
                                                <asp:Label ID="lblsmstemplateSuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <table class="TableData">
                                                        <tr>
                                                            <td colspan="4">
                                                                <table>
                                                                    <tr>
                                                                        <td class="TDCaption">
                                                                            Template Name
                                                                        </td>
                                                                        <td class="TDDot">
                                                                            :
                                                                        </td>
                                                                        <td align="left" style="margin-left: 40px">
                                                                            <asp:TextBox ID="txttemplate" runat="server" Width="200px" CssClass="Textbox" />
                                                                            <span class="span">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDCaption">
                                                                            Placeholder
                                                                        </td>
                                                                        <td class="TDDot">
                                                                            :
                                                                        </td>
                                                                        <td align="left" style="margin-left: 40px">
                                                                            <select id="Select1" onchange="Javascript:fetchdropvalue()" runat="server">
                                                                                <option value="Select"></option>
                                                                                <option value="[Customer Name]"></option>
                                                                                <option value="[Booking No]"></option>
                                                                                 <option value="[Booking Date]"></option>
                                                                                <option value="[Delivery Date]"></option>
                                                                                <option value="[Amount]"></option>
                                                                                <option value="[Quantity]"></option>
                                                                                <option value="[User Name]"></option>
                                                                                <option value="[Password]"></option>
                                                                                <option value="[SenderId]"></option>
                                                                                <option value="[Mobile No]"></option>
                                                                                <option value="[Package Name]"></option>
                                                                                <option value="[Expiry Date]"></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDCaption">
                                                                            <%=Defaultsms%>
                                                                        </td>
                                                                        <td class="TDDot">
                                                                            :
                                                                        </td>
                                                                        <td align="left" style="margin-left: 40px">
                                                                            <asp:DropDownList ID="drpDefaultMsg" runat="server" AppendDataBoundItems="true">
                                                                                <asp:ListItem Text="None"></asp:ListItem>
                                                                                <asp:ListItem Text="Booking"></asp:ListItem>
                                                                                <asp:ListItem Text="Cloth Ready"></asp:ListItem>
                                                                                <asp:ListItem Text="Delivery"></asp:ListItem>
                                                                                 <asp:ListItem Text="Pending Stock"></asp:ListItem>
                                                                                  <asp:ListItem Text="Package Creation"></asp:ListItem>
                                                                                   <asp:ListItem Text="Package Expire/Mark Complete"></asp:ListItem>
                                                                                    <asp:ListItem Text="Package to be Renew"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDCaption">
                                                                            Message
                                                                        </td>
                                                                        <td class="TDDot">
                                                                            :
                                                                        </td>
                                                                        <td align="left" style="margin-left: 40px">
                                                                            <asp:TextBox ID="txtmessage" runat="server" Height="80" Width="600px" TextMode="MultiLine" />
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
                                                                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return checkName('',document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabPanel_txttemplate') ,'', 'Please Enter Template Name ' )"
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
                                                                SMS Configurator Details
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
                                                                    <asp:GridView ID="grdsms" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                                                        Visible="True" OnSelectedIndexChanged="grdsms_SelectedIndexChanged">
                                                                        <Columns>
                                                                            <asp:CommandField ShowSelectButton="True" />
                                                                            <asp:BoundField DataField="template" HeaderText="Template" ReadOnly="True" SortExpression="template" />
                                                                            <asp:TemplateField HeaderText="Screen">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDefaultMsg" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                        Text='<%# Eval("MsgScreen") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="massage" HeaderText="Message" ReadOnly="True" SortExpression="massage" />
                                                                            <asp:TemplateField Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblId" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                        Text='<%# Eval("SmsID") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tdapi" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer2" runat="server">
                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="SMS API Setting">
                            <HeaderTemplate>
                                Gateway Setup
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblapiSuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False" />
                                                <asp:Label ID="lblapiError" runat="server" CssClass="ErrorMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr style="visibility: hidden;">
                                                        <td class="TDCaption">
                                                            API URL
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtapi" runat="server" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td colspan="4">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Sender Id
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Txtsenderidpreview" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Txtsenderid" runat="server" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td style="visibility: hidden;">
                                                            <asp:DropDownList ID="Drpsender" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            User Name
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtusernamepreview" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtuserName" runat="server" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td style="visibility: hidden;">
                                                            <asp:DropDownList ID="Drpusername" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Password
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpasswordpreview" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td style="visibility: hidden;">
                                                            <asp:DropDownList ID="Drppassword" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden;">
                                                        <td class="TDCaption">
                                                            Mobile No
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Drpmobile" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden;">
                                                        <td class="TDCaption">
                                                            Message
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Txtmassagepreview" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Drpmassage" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnapisettting" runat="server" Text="Save" CausesValidation="False"
                                                                            OnClick="btnapisettting_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
                <td id="tddefaultsms" runat="server" visible="false">
                    <cc1:TabContainer ID="TabContainer3" runat="server">
                        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Default SMS Setting">
                            <HeaderTemplate>
                                SMS Preview
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                    <table class="TableData">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Lbdefaultsmssuccess" runat="server" CssClass="SuccessMessage" EnableViewState="False" />
                                                <asp:Label ID="Lbdefaultsmserror" runat="server" CssClass="ErrorMessage" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="TableData">
                                                    <tr>
                                                        <td class="TDCaption">
                                                            New Booking
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Drpnewbooking" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbbooking" runat="server" CssClass="Legend"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Cloth Ready
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Drpclothready" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbclothready" runat="server" ForeColor="#24000E"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TDCaption">
                                                            Delivery
                                                        </td>
                                                        <td class="TDDot">
                                                            :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpdelivery" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbdelivery" runat="server" ForeColor="#5E1331"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td width="2px">
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td colspan="2" nowrap="nowrap">
                                                                        <asp:Button ID="btpreview" runat="server" Text="Preview" CausesValidation="False"
                                                                            OnClick="btpreview_Click" />&nbsp;&nbsp;&nbsp;
                                                                        <asp:Button ID="btdefaultsmsupdate" runat="server" Text="Save" CausesValidation="False"
                                                                            OnClick="btdefaultsmsupdate_Click" Visible="false" />
                                                                    </td>
                                                                    <td style="width: 50Px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>