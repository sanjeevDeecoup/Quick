<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="StockReport" Title="Stock Report" Codebehind="StockReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
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
        <table class="TableData" width="100%">
            <tr valign="top">
                <td>
                    <table class="TableData" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="ErrorMessage"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="TableData" width="100%">
                                    <tr>
                                        <td class="TDCaption" style="text-align: left; width: 50%">
                                            <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                                <asp:GridView ID="grdReport" runat="server" Visible="False" AutoGenerateColumns="False"
                                                    OnRowCommand="grdReport_OnRowCommand" ShowFooter="True" EmptyDataText="No record found"
                                                    CssClass="mGrid">
                                                    <Columns>
                                                        <asp:BoundField DataField="Item" HeaderText="Item" />
                                                        <asp:BoundField DataField="StockQty" HeaderText="Stock Qty" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnShowDetails" runat="server" ImageUrl="~/images/ZoomHS.png"
                                                                    CommandArgument='<%# Bind("Item") %>' CommandName="CMDShowDetails" />
                                                            </ItemTemplate>
                                                       </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="TDCaption" style="text-align: left; width: 50%">
                                            <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                                <asp:GridView ID="grdStockDetails" runat="server" Visible="False" AutoGenerateColumns="False"
                                                    EmptyDataText="No record found" Caption="Details" CssClass="mGrid">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                                    Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="BookingNumber" HeaderText="Booking #" SortExpression="BookingNumber" />--%>
                                                        <asp:BoundField DataField="ItemsReceived" HeaderText="Received" />
                                                        <asp:BoundField DataField="Delivered" HeaderText="Delivered" />
                                                        <asp:TemplateField  HeaderText="Delivery Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DueDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export to Excel"
                                    Visible="False" />
                                <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="Sp_Sel_PaymentReport" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="PaymentDate1" DefaultValue="" />
                                        <asp:Parameter Name="PaymentDate2" DefaultValue="" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
