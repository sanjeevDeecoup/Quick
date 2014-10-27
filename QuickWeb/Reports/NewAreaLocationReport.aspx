<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="NewAreaLocationReport" Title="Untitled Page" Codebehind="NewAreaLocationReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <legend class="Legend">Booking Report</legend>
        <table class="TableData">
            <tr valign="top">
                <td>
                    <table class="TableData">
                        <tr>
                            <td>
                                <table class="TableData">
                                    <tr>
                                        <td nowrap="nowrap" class="TDCaption">
                                            Area Location
                                        </td>
                                        <td nowrap="nowrap" width="150">
                                            <asp:TextBox ID="txtAreaLocation" runat="server"></asp:TextBox>
                                        </td>
                                        <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
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
                                        </td>
                                        <td>
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
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                    EmptyDataText="No record found" PageSize="50">
                                    <FooterStyle BackColor="#F5DFE4" ForeColor="White" HorizontalAlign="Center" />
                                    <Columns>                                        
                                        <asp:BoundField DataField="Item" HeaderText="Item Name" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="Item" FooterText="Total Qty"></asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="Qty"></asp:BoundField>
                                    </Columns>
                                    <HeaderStyle BackColor="Maroon" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#CCFFFF" />
                                </asp:GridView>
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
