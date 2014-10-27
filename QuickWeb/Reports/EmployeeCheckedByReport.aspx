<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="EmployeeCheckedByReport" Title="Untitled Page" Codebehind="EmployeeCheckedByReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
function checkEntry()
{    
    var InvoiceNo=document.getElementById("<%=txtReportFrom.ClientID %>").value;   
    
    if(InvoiceNo=="")
    {        
        alert("Please enter invoice no.");
        document.getElementById("<%=txtReportFrom.ClientID %>").focus();
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Checked
                    By Employee Report
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
                    <table class="TableData"  width="100%">
                        <tr>
                            <td>
                                <table class="TableData"  width="100%">
                                    <tr>
                                        <td nowrap="nowrap" align="left">
                                            Invoice No :
                                            <asp:TextBox ID="txtReportFrom" runat="server" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                                                OnClick="btnShowReport_Click" />
                                            &nbsp;
                                            &nbsp;
                                            <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Visible="False"
                                                OnClick="btnExport_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                                <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                                    <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNo"
                                        EmptyDataText="No record found" PageSize="50">
                                        <Columns>
                                            <asp:BoundField DataField="BookingNo" HeaderText="Booking Number" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="BookingNo">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EmployeeName" HeaderText="Checked By" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="EmployeeName">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Item" HeaderText="Item Name" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="Item">
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
                                &nbsp;
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
