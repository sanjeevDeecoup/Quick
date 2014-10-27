<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="BookingHistoryList.aspx.cs" Inherits="QuickWeb.Reports.BookingHistoryList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="Booking Hisory List" ForeColor="#FF9933"></asp:Label>
                    List of bookings that were edited in the selected duration. Note
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
        <table width="100%">
            <tr>
                <td>
                    <asp:GridView ID="grdHistory" runat="server" OnRowDataBound="grdHistory_RowDataBound"
                        OnDataBinding="grdHistory_DataBinding" OnDataBound="grdHistory_DataBound" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label2" runat="server" Text="Revision"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRevisionNo" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingBackUpId" SortExpression="BookingBackUpId" HeaderText="Backup Booking Id"
                                Visible="true" />
                            <asp:BoundField DataField="BookingNumber" SortExpression="BookingNumber" HeaderText="Order No." />
                            <asp:BoundField DataField="BookingAcceptedByUserId" SortExpression="BookingAcceptedByUserId"
                                HeaderText="Booked/Edited By." />
                            <asp:BoundField DataField="BookingDate" SortExpression="BookingDate" HeaderText="Booked/Edited On" />
                            <asp:BoundField DataField="EditBookingRemarks" SortExpression="EditBookingRemarks"
                                HeaderText="Editing Remarks" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeader" runat="server">Compare</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hplNavigate" Target="_blank" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Visible="false"
                        Text="Export to Excel" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
