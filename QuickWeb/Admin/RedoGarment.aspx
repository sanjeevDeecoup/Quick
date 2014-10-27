<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="RedoGarment.aspx.cs" Inherits="QuickWeb.Admin.RedoGarment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jsapi.js" type="text/javascript"></script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $("body").delegate(":checkbox", "change", function (event) {
                // if the id is 'chkAll' then select all the ids
                if ($(event.target).attr('id') == 'ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll') {
                    // alert('Its all');
                    list3 = $(':checkbox:checked').not(document.getElementById('ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll')).map(function () {
                        return $(this).closest('tr').find('td').eq(1).text().substring(0, 11) + ':' + $(this).closest('tr').find('a').text();
                    }).get();
                    // alert(list3);
                }
                else if ($(event.target).attr('id') != 'ctl00_ContentPlaceHolder1_chkInvoice') {
                    list3 = $(':checkbox:checked').not(document.getElementById('ctl00_ContentPlaceHolder1_grdReport_ctl01_CheckAll')).map(function () {
                        return $(this).closest('tr').find('td').eq(1).text().substring(0, 11) + ':' + $(this).closest('tr').find('a').text();
                    }).get();
                    //  alert(list3);
                }
                //alert(list3);
                $('#<%=hdnSelectedList.ClientID %>').text(list3);
                $('#<%=hdnSelectedList.ClientID %>').val(list3);
            });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="Redo Garment " ForeColor="#FF9933"></asp:Label>
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
                                <table class="TableData" width="100%">
                                    <tr>
                                        <td height="10Px">
                                         <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Order No</span>
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="7" Visible="true" Width="100px"></asp:TextBox>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClick="btnShowReport_Click" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustName" runat="server" Text="Customer Name :" Visible="false"></asp:Label>
                                            <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOrder" runat="server" Text="Order Number :" Visible="false"></asp:Label>
                                            <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOrderda" runat="server" Text="Order Date :" Visible="false"></asp:Label>
                                            <asp:Label ID="lblOrderdate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="TDCaption" style="text-align: left">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                    <div class="DivStyleWithScroll" style="width: 1224Px; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNo"
                            ShowFooter="True" EmptyDataText="No record found" PageSize="50" CssClass="mGrid">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:BoundField DataField="BookingNo" HeaderText="Order No" SortExpression="BookingNo" />
                                <asp:BoundField DataField="BookingDate" HeaderText="Order Date" SortExpression="BookingDate" />--%>
                                <%--<asp:BoundField DataField="CustomerName" HeaderText="Customer Details" SortExpression="CustomerName" />--%>
                                <asp:BoundField DataField="ISN" HeaderText="S.No." SortExpression="ISN" />
                                <asp:BoundField DataField="ItemName" HeaderText="Item" SortExpression="ItemName" />
                                <asp:BoundField DataField="ItemTotalQuantity" HeaderText="Total Qty" SortExpression="ItemTotalQuantity" />
                                <asp:BoundField DataField="ItemProcessType" HeaderText="Process" SortExpression="ItemProcessType" />
                                <asp:BoundField DataField="ItemStatus" HeaderText="Status" SortExpression="ItemStatus" />
                                <asp:BoundField DataField="CountRedo" HeaderText="Redo Count" SortExpression="CountRedo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td class="TDCaption2">
                    <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Redo" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnPrintValue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
    <asp:HiddenField ID="hdnSelectedList" runat="server" />
    <asp:HiddenField ID="hdnDTOReportsBFlag" Value="false" runat="server" />
    <asp:HiddenField ID="hdnFirstSel" Value="No" runat="server" />
    <asp:HiddenField ID="hdnPassValue" runat="server" />
    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
    <asp:Label ID="lblCustomerCode" runat="server" Style="visibility: hidden"></asp:Label>
</asp:Content>
