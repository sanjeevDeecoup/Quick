<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="ItemwiseReport" Title="Untitled Page" Codebehind="ItemwiseReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script type="text/javascript" language="javascript">
function checkEntry()
{    
    var AreaLocation=document.getElementById("<%=txtAreaLocation.ClientID %>").value;
    
        if(AreaLocation=="")
        {
            alert("Please select area/location for search.");
            document.getElementById("<%=txtAreaLocation.ClientID %>").focus();
            return false;
        }
    
}
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Item
                    Wise
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
                                <table width="100%">
                                    <tr>
                                        <td nowrap="nowrap" align="left">
                                            Item Name
                                            <asp:DropDownList ID="drpItemNames" runat="server" Width="150px" DataSourceID="SqlSourceItems"
                                                DataTextField="ItemName" DataValueField="ItemID" MaxLength="0">
                                            </asp:DropDownList>
                                            <asp:RadioButton ID="radReportMonthly" runat="server" Checked="true" GroupName="radReportOptions"
                                                Text="Monthly Report" CssClass="TDCaption" />
                                            &nbsp;<asp:DropDownList ID="drpMonthList" runat="server">
                                                <asp:ListItem Selected="True" Value="1">January</asp:ListItem>
                                                <asp:ListItem Value="2">February</asp:ListItem>
                                                <asp:ListItem Value="3">March</asp:ListItem>
                                                <asp:ListItem Value="4">April</asp:ListItem>
                                                <asp:ListItem Value="5">May</asp:ListItem>
                                                <asp:ListItem Value="6">June</asp:ListItem>
                                                <asp:ListItem Value="7">July</asp:ListItem>
                                                <asp:ListItem Value="8">August</asp:ListItem>
                                                <asp:ListItem Value="9">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;<asp:DropDownList ID="drpYearList" runat="server">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption" style="text-align: left">
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                    <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNO"
                                        ShowFooter="True" EmptyDataText="No record found" PageSize="50" CssClass="mGrid">
                                        <Columns>
                                            <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Booking Date"
                                                SortExpression="BookingDate" FooterText="Total" />
                                            <asp:TemplateField HeaderText="Booking Number">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNO") %>'
                                                        Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNO"),-1,Eval("Deliverydate")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Deliverydate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="QTY" HeaderText="QTY" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="QTY">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RATE" HeaderText="RATE" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="RATE">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Visible="False"
                                    OnClick="btnExport_Click" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlSourceItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT [ItemID], [ItemName] FROM [ItemMaster] ORDER BY ItemName">
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
</asp:Content>
