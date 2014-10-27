<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="GetChallanByNumbers" Title="Untitled Page" Codebehind="GetChallanByNumbers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
function CheckBoxValues()
{
    var startvaluebox=document.getElementById("<%= txtStartChallanNumber.ClientID %>");
    var endvaluebox=document.getElementById("<%= txtEndChallanNumber.ClientID %>");
    if(parseInt(startvaluebox)==0 || parseInt(endvaluebox)==0)
    {
        alert("Invalid start and/or upto challan number(s)");
        startvaluebox.focus();
        return false;
    }
    if(parseInt(startvaluebox) >= parseInt(endvaluebox))
    {
        alert("Invalid challan search number range");
        startvaluebox.focus();
        return false;
    }
}

function PrintDiv()
{    
	var win4Print = window.open('','Win4Print');
	var msg=document.getElementById("Div4Print").innerHTML;
	win4Print.document.write(msg);
	win4Print.document.close();
	win4Print.focus();
	win4Print.print();
	win4Print.close();
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Print
                    Workshop\Delivery Note By Numbers
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
            <tr>
                <td>
                    <table class="TableData">
                        <tr>
                            <td width="125px" class="TDCaption">
                                Start Receipt No
                            </td>
                            <td width="100" style="width: 0">
                                <asp:TextBox ID="txtStartChallanNumber" runat="server" Width="50px"></asp:TextBox>
                            </td>
                            <td width="100" style="width: 50px" class="TDCaption">
                                Upto
                            </td>
                            <td width="20">
                                <asp:TextBox ID="txtEndChallanNumber" runat="server" Width="50px"></asp:TextBox>
                            </td>
                            <td class="TDCaption">
                                Select Process
                            </td>
                            <td>
                                <asp:DropDownList ID="drpProcessNames" DataTextFormatString="{0:d}" runat="server"
                                    Width="150px" AppendDataBoundItems="true" DataSourceID="SqlSourceProcesses" DataTextField="ProcessName"
                                    DataValueField="ProcessName">
                                    <asp:ListItem Selected="True" Text="All Processes" Value="0" />
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlSourceProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="SELECT [ProcessName] FROM [ProcessMaster]"></asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:Button ID="btnShowChallan" runat="server" OnClientClick="return CheckBoxValues();"
                                    OnClick="btnShowChallan_Click" Text="Show Challan" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="3">
                    <asp:Label ID="lblMsg" runat="server" CssClass="WarningMessage" EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="3">
                    <div id="Div4Print">
                        <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                            <asp:GridView ID="grdChallan" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                EmptyDataText="There is no Item to show." ShowFooter="True" Visible="False" BorderWidth="2px">
                                <FooterStyle Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="ChallanNumber" HeaderText="S.No" />
                                    <asp:BoundField DataField="BookingNumber" HeaderText="Booking No." SortExpression="BookingNumber" />
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="200px"
                                        SortExpression="ItemName">
                                        <ItemStyle Width="200px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemProcessType" HeaderText="Process" SortExpression="ItemProcessType" />
                                    <asp:BoundField DataField="ItemExtraProcessType1" HeaderText="Extra Process" SortExpression="ItemExtraProcessType1" />
                                    <asp:TemplateField HeaderText="Urgent" SortExpression="Urgent" ItemStyle-ForeColor="Red">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUrgent" runat="server" Text='<%# Eval("Urgent").ToString() == "True" ? "Yes": "" %>' />
                                        </ItemTemplate>
                                        <ItemStyle ForeColor="Red"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TotalQtySent" HeaderText="Total Qty" SortExpression="TotalQtySent" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    <asp:Button ID="btnPrint" runat="server" OnClientClick="PrintDiv();return false;"
                        Visible="false" Text="Print" />
                    &nbsp;
                    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"
                        Visible="false" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:SqlDataSource ID="SqlSourceChallan" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [EntChallan] Order By BookingNumber">
                    </asp:SqlDataSource>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
