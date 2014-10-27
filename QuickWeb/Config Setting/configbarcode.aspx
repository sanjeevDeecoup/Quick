<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="configbarcode.aspx.cs" Inherits="QuickWeb.Reports.configbarcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/barcode.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="mainContainer" align="center" style="border-style: outset">
        <fieldset class="Fieldset">
            <legend class="Legend">Barcode Setting</legend>
            <div id="leftContainer" class="leftContainer">
                <asp:Panel ID="panelwidth" runat="server">
                    <div>
                        <div class="barcode1">
                            Width
                        </div>
                        <div class="barcode">
                            <asp:DropDownList ID="barcodewirth" runat="server">
                                <asp:ListItem Value="0">1.4in</asp:ListItem>
                                <asp:ListItem Value="1">1.5in</asp:ListItem>
                                <asp:ListItem Value="2">1.6in</asp:ListItem>
                                <asp:ListItem Value="3">1.7in</asp:ListItem>
                                <asp:ListItem Value="4">1.8in</asp:ListItem>
                                <asp:ListItem Value="5">1.9in</asp:ListItem>
                                <asp:ListItem Value="6">2in</asp:ListItem>
                                <asp:ListItem Value="7">2.1in</asp:ListItem>
                                <asp:ListItem Value="8">2.2in</asp:ListItem>
                                <asp:ListItem Value="9">2.3in</asp:ListItem>
                                <asp:ListItem Value="10">2.4in</asp:ListItem>
                                <asp:ListItem Value="11">2.5in</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="barcode">
                            Height
                        </div>
                        <div class="barcode2">
                            <asp:DropDownList ID="barcodehight" runat="server">
                                <asp:ListItem Value="0">.8in</asp:ListItem>
                                <asp:ListItem Value="1">.9in</asp:ListItem>
                                <asp:ListItem Value="2">1in</asp:ListItem>
                                <asp:ListItem Value="3">1.1in</asp:ListItem>
                                <asp:ListItem Value="4">1.2in</asp:ListItem>
                                <asp:ListItem Value="5">1.3in</asp:ListItem>
                                <asp:ListItem Value="6">1.4in</asp:ListItem>
                                <asp:ListItem Value="7">1.5in</asp:ListItem>
                                <asp:ListItem Value="8">1.6in</asp:ListItem>
                                <asp:ListItem Value="9">1.7in</asp:ListItem>
                                <asp:ListItem Value="10">1.8in</asp:ListItem>
                                <asp:ListItem Value="11">1.9in</asp:ListItem>
                                <asp:ListItem Value="12">2in</asp:ListItem>
                                <asp:ListItem Value="13">2.1in</asp:ListItem>
                                <asp:ListItem Value="14">2.2in</asp:ListItem>
                                <asp:ListItem Value="15">2.3in</asp:ListItem>
                                <asp:ListItem Value="16">2.4in</asp:ListItem>
                                <asp:ListItem Value="17">2.5in</asp:ListItem>
                                <asp:ListItem Value="18">2.6in</asp:ListItem>
                                <asp:ListItem Value="19">2.7in</asp:ListItem>
                                <asp:ListItem Value="20">2.8in</asp:ListItem>
                                <asp:ListItem Value="21">2.9in</asp:ListItem>
                                <asp:ListItem Value="22">3in</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="barcode3">
                            <asp:Button ID="btset" runat="server" Text="Set Size" OnClick="btset_Click" />
                        </div>
                        <div class="barcode4">
                        </div>
                    </div>
                    <div style="height: 37px;">
                    </div>
                    <div style="background-color: #5F9EA0; height: 3px;">
                    </div>
                </asp:Panel>
                <asp:Panel ID="barcodepanel" runat="server" Visible="false">
                    <div id="Button">
                        <asp:Label ID="lblMsgbarcode" runat="server" CssClass="SuccessMessage" EnableViewState="False"></asp:Label>
                    </div>
                    <div>
                        <div class="barcode5">
                        </div>
                        <div class="barcode6">
                            Print
                        </div>
                        <div class="barcode7">
                            Position
                        </div>
                        <div class="barcode8">
                            Font
                        </div>
                        <div class="barcode8">
                            Align
                        </div>
                        <div class="barcode8">
                            Size
                        </div>
                        <div class="barcode9">
                            Style
                        </div>
                    </div>
                    <div style="height: 37px;">
                    </div>
                    <div style="background-color: #5F9EA0; height: 3px;">
                    </div>
                    <div class="booking50">
                        <div class="barcode10">
                            Barcode
                        </div>
                        <div class="barcode11">
                            <asp:RadioButton ID="RadioBarcodeyes" runat="server" GroupName="Barcode" Text=" Yes"
                                Checked="True" />
                            <asp:RadioButton ID="RadioBarcodeno" runat="server" GroupName="Barcode" Text=" No" />
                        </div>
                        <div class="barcode12">
                            <asp:DropDownList ID="drpbarcodeposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode12">
                        </div>
                        <div class="barcode12">
                            <asp:DropDownList ID="Dropbarcodealign" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode12">
                            <%--  <asp:DropDownList ID="Dropbarcodesize" runat="server" Width="60px" AutoPostBack="true"
                            OnSelectedIndexChanged="Dropbarcodesize_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Text="12" />
                        </asp:DropDownList>--%>
                        </div>
                        <div class="barcode14">
                        </div>
                    </div>
                    <div style="height: 37px;">
                    </div>
                    <div style="background-color: #5F9EA0; height: 3px;">
                    </div>
                    <div class="booking50">
                        <div class="barcode15">
                            Booking No
                        </div>
                        <div class="barcode16">
                            <asp:RadioButton ID="RadioBookingNoyes" runat="server" GroupName="BookingNo" Text=" Yes"
                                Checked="True" />
                            <asp:RadioButton ID="RadioBookingNorno" runat="server" GroupName="BookingNo" Text=" No" />
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpbookinfposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode18">
                            <asp:DropDownList ID="DropBookingNofont" runat="server" Width="90px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="DropBookingNoalign" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="drpBookingNosize" runat="server" Width="60px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode19">
                            <asp:CheckBox ID="CheckBookingNobould" Text=" B" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="CheckBookingNoitilice" Text=" I" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="CheckBookingNounderline" Text=" U" runat="server" CssClass="TDCaption" />
                        </div>
                    </div>
                    <div style="height: 37px;">
                    </div>
                    <div style="background-color: #5F9EA0; height: 3px;">
                    </div>
                    <div class="booking50">
                        <div class="barcode15">
                            Customer Name
                        </div>
                        <div class="barcode16">
                            <asp:RadioButton ID="radiocusyes" runat="server" GroupName="name" Text=" Yes" Checked="True" />
                            <asp:RadioButton ID="Radiocusno" runat="server" GroupName="name" Text=" No" />
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpcusposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode18">
                            <asp:DropDownList ID="drpfontname" runat="server" Width="90px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpcusalign" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpnamesize" runat="server" Width="60px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode19">
                            <asp:CheckBox ID="Checknamebold" Text=" B" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checknameitalac" Text=" I" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checknameunderline" Text=" U" runat="server" CssClass="TDCaption" />
                        </div>
                    </div>
                    <div style="height: 37px;">
                    </div>
                    <div style="background-color: #5F9EA0; height: 3px;">
                    </div>
                    <div class="booking50">
                        <div class="barcode15">
                            Address
                        </div>
                        <div class="barcode16">
                            <asp:RadioButton ID="Radioaddressyes" runat="server" GroupName="address" Checked="True"
                                Text=" Yes" />
                            <asp:RadioButton ID="Radioaddressno" runat="server" GroupName="address" Text=" No" />
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="drpaddressposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode18">
                            <asp:DropDownList ID="Drpaddressfont" runat="server" Width="90px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpaddressalign" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpaddresssize" runat="server" Width="60px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode19">
                            <asp:CheckBox ID="Checkaddressbold" Text=" B" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkaddressitalic" Text=" I" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkaddressunderline" Text=" U" runat="server" CssClass="TDCaption" />
                        </div>
                    </div>
                    <div style="height: 37px;">
                    </div>
                    <div style="background-color: #5F9EA0; height: 3px;">
                    </div>
                    <div class="booking50">
                        <div class="barcode20">
                            Process<br />
                            ItemTotal
                        </div>
                        <div class="barcode16">
                            <asp:RadioButton ID="RadioProcessyes" runat="server" GroupName="Process" Checked="True"
                                Text=" Yes" />
                            <asp:RadioButton ID="RadioProcessno" runat="server" GroupName="Process" Text=" No" /><br />
                            <asp:RadioButton ID="RadioItemTotalandSubTotalyes" runat="server" GroupName="ItemTotalandSubTotal"
                                Text=" Yes" Checked="True" />
                            <asp:RadioButton ID="RadioItemTotalandSubTotalno" runat="server" GroupName="ItemTotalandSubTotal"
                                Text=" No" />
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpprocessposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode18">
                            <asp:DropDownList ID="drpcusfont" runat="server" Width="90px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Dropcuscolour" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="drpcussize" runat="server" Width="60px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode19">
                            <asp:CheckBox ID="Checkbould" Text=" B" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkitilace" Text=" I" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkunderline" Text=" U" runat="server" CssClass="TDCaption" />
                        </div>
                        <div style="height: 57px;">
                        </div>
                        <div style="background-color: #5F9EA0; height: 3px;">
                        </div>
                    </div>
                    <div class="booking50">
                        <div class="barcode20">
                            Remark<br />
                            Colour
                        </div>
                        <div class="barcode16">
                            <asp:RadioButton ID="Radioremarkyes" runat="server" GroupName="remark" Text=" Yes"
                                Checked="True" />
                            <asp:RadioButton ID="Radioremarkno" runat="server" GroupName="remark" Text=" No" />
                            <br />
                            <asp:RadioButton ID="Radiocoloursyes" runat="server" GroupName="colour" Text=" Yes"
                                Checked="True" />
                            <asp:RadioButton ID="Radiocoloursno" runat="server" GroupName="colour" Text=" No" />
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="drpremarkposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode18">
                            <asp:DropDownList ID="Dropremarkfont" runat="server" Width="90px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Dropremarkalign" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Dropremarksize" runat="server" Width="60px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode19">
                            <asp:CheckBox ID="Checkremarkbould" Text=" B" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkremarkitilice" Text=" I" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkremarkunderline" Text=" U" runat="server" CssClass="TDCaption" />
                        </div>
                        <div style="height: 57px;">
                        </div>
                        <div style="background-color: #5F9EA0; height: 3px;">
                        </div>
                    </div>
                    <div class="booking50">
                        <div class="barcode21">
                            Due Date<br />
                            DueTime
                        </div>
                        <div class="barcode22">
                            <asp:RadioButton ID="RadioDateyes" runat="server" GroupName="Date" Text=" Yes" Checked="True" />
                            <asp:RadioButton ID="RadioDateno" runat="server" GroupName="Date" Text=" No" /><br />
                            <asp:RadioButton ID="Radiotimeyes" runat="server" GroupName="time" Text=" Yes" Checked="True" />
                            <asp:RadioButton ID="Radiotimeno" runat="server" GroupName="time" Text=" No" />
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Drpitemposition" runat="server" Width="70px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode18">
                            <asp:DropDownList ID="Dropitemfont" runat="server" Width="90px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Dropitemalign" runat="server" Width="70px">
                                <asp:ListItem Selected="True" Text="Center" />
                                <asp:ListItem Text="left" />
                                <asp:ListItem Text="Right" />
                            </asp:DropDownList>
                        </div>
                        <div class="barcode17">
                            <asp:DropDownList ID="Dropitemsize" runat="server" Width="60px">
                            </asp:DropDownList>
                        </div>
                        <div class="barcode19">
                            <asp:CheckBox ID="Checkitembould" Text=" B" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkitemitilice" Text=" I" runat="server" CssClass="TDCaption" />
                            <asp:CheckBox ID="Checkitemunderline" Text=" U" runat="server" CssClass="TDCaption" />
                        </div>
                        <div style="height: 97px;">
                        </div>
                        <div style="background-color: #5F9EA0; height: 3px;">
                        </div>
                        <div class="booking50">
                            <div class="barcode15">
                                Item Name
                            </div>
                            <div class="barcode16">
                                <asp:RadioButton ID="RadioItemyes" runat="server" GroupName="Divider" Text=" Yes"
                                    Checked="True" />
                                <asp:RadioButton ID="RadioItemno" runat="server" GroupName="divider" Text=" No" />
                            </div>
                            <div class="barcode17">
                                <asp:DropDownList ID="Drpitemposition1" runat="server" Width="70px">
                                </asp:DropDownList>
                            </div>
                            <div class="barcode18">
                                <asp:DropDownList ID="Dropitemfont1" runat="server" Width="90px">
                                </asp:DropDownList>
                            </div>
                            <div class="barcode17">
                                <asp:DropDownList ID="Dropitemalign1" runat="server" Width="70px">
                                    <asp:ListItem Selected="True" Text="Center" />
                                    <asp:ListItem Text="left" />
                                    <asp:ListItem Text="Right" />
                                </asp:DropDownList>
                            </div>
                            <div class="barcode17">
                                <asp:DropDownList ID="Dropitemsize1" runat="server" Width="60px">
                                </asp:DropDownList>
                            </div>
                            <div class="barcode19">
                                <asp:CheckBox ID="Checkitembould1" runat="server" CssClass="TDCaption" Text=" B" />
                                <asp:CheckBox ID="Checkitemitilice1" runat="server" CssClass="TDCaption" Text=" I" />
                                <asp:CheckBox ID="Checkitemunderline1" runat="server" CssClass="TDCaption" Text=" U" />
                            </div>
                        </div>
                        <div style="height: 37px;">
                        </div>
                        <div style="background-color: #5F9EA0; height: 3px;">
                        </div>
                        <div class="booking50">
                            <div class="barcode15">
                            </div>
                            <div class="barcode16">
                            </div>
                            <div class="barcode17">
                                <asp:Button ID="btpreview" runat="server" Text="Preview" OnClick="btpreview_Click" />
                            </div>
                            <div class="barcode23" style="text-align: left;">
                                <asp:Button ID="btbarcodesave" runat="server" Text="Save" OnClick="btbarcodesave_Click" />
                            </div>
                            <div class="barcode17">
                            </div>
                            <div class="barcode17">
                            </div>
                            <div class="barcode19">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="rightContainer">
                <div style="height: 60px;">
                </div>
                <asp:Panel ID="barcodepaneldisplay" runat="server">
                    <div style="width: 5px; float: left; margin-left: 5px; text-align: center;">
                        <div style="width: .5in; margin-top: 17px; text-align: right; line-height: <%=linehight %>in;
                            height: 2.2in;">
                            1<br />
                            2<br />
                            3<br />
                            4<br />
                            5<br />
                            6<br />
                            7<br />
                            8
                        </div>
                    </div>
                    <div class="barcodeprint">
                        <span style="width: 20%; color: #00649e;">New Barcode </span>
                        <div style="border: 2px solid; font-size: large; width: <%=barcodewidth %>; height: <%=barcodeheight %>;">
                            <% if (strPreview.Length > 0)
                               {
                                   Response.Write(strPreview);

                               }
                            %>
                        </div>
                    </div>
                </asp:Panel>
                <div class="style1">
                </div>
                <div class="barcodeprint">
                    <span style="width: 20%; color: #00649e;">Old Barcode</span>
                    <div style="border: 2px solid; width: <%=oldbarcodewidth %>; font-size: large; height: <%=oldbarcodehight %>;">
                        <% if (strPreviewbarcode.Length > 0)
                           {
                               Response.Write(strPreviewbarcode);

                           }
                        %>
                    </div>
                </div>
            </div>
            </span>
        </fieldset>
    </div>
</asp:Content>