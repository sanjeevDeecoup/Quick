<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="StatusBookingByCustomer" Title="Untitled Page" Codebehind="StatusBookingByCustomer.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/MainStyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
function CheckBookNumber()
{
var val=document.getElementById("<%=txtBookingNumberFrom.ClientID %>").value;
if(val=="")
{
    alert("Please enter booking number.");
    return false;
}
}

function CheckMaxEntry(txt)
{
    var txtID=txt.id;
    var CtlID=txtID.substring(0,txtID.length-16);
    var lblTotalQtyID=CtlID+"lblTotalQtySent";
    var btnId=CtlID+"btnAddOneItem";
    var totalqty=document.getElementById(lblTotalQtyID).innerHTML;
    var qtyreceived=document.getElementById(txtID).value;
    if(qtyreceived=="")qtyreceived="0";
    if(parseInt(totalqty)>parseInt(qtyreceived))
    {
        document.getElementById(txtID).value=qtyreceived;
        document.getElementById(btnId).style.visibility='visible';
    }
    else
    {
        document.getElementById(txtID).value=totalqty;
        document.getElementById(btnId).style.visibility='hidden';
    }   
    CalculateTotalReceivedItems();
    return false;
}
function AddOneToTotal(btn)
{
    var btnId=btn.id;
    var CtlID=btnId.substring(0,btnId.length-13);
    txtId=CtlID+"txtItemsReceived";
    lblTotalQtyID=CtlID+"lblTotalQtySent";
    var totalqty=document.getElementById(lblTotalQtyID).innerHTML;
    var qtyreceived=document.getElementById(txtId).value-0;
    if(parseInt(totalqty)<parseInt(qtyreceived))
    {
        qtyreceived=qtyreceived+1;
        document.getElementById(txtId).value=qtyreceived;
    }
    
    if(parseInt(totalqty)<=parseInt(qtyreceived))
    {
        document.getElementById(btnId).style.visibility='hidden';
    }
    CalculateTotalReceivedItems();
    return false;
    
}
function AddOneToTotal2(btn)
{
    var btnId=btn.id;
    var CtlID=btnId.substring(0,btnId.length-14);
    txtId=CtlID+"txtItemsReceived2";
    lblTotalQtyID=CtlID+"lblTotalQtySent2";
    var totalqty=document.getElementById(lblTotalQtyID).innerHTML;
    var qtyreceived=document.getElementById(txtId).value-0;
    if(parseInt(totalqty)<parseInt(qtyreceived))
    {
        qtyreceived=qtyreceived+1;
        document.getElementById(txtId).value=qtyreceived;
    }
    
    if(parseInt(totalqty)<=parseInt(qtyreceived))
    {
        document.getElementById(btnId).style.visibility='hidden';
    }
    CalculateTotalReceivedItems();
    return false;
    
}
<% if(grdChallan.Rows.Count>0) { %>
function CalculateTotalReceivedItems()
{
    var grdId=document.getElementById("<%= grdChallan.ClientID %>").id;
    var grdCtlId=grdId+"_ctl";
    var TotalRowsInGrid=parseInt("<%= grdChallan.Rows.Count %>");
    var PadLength=TotalReceivedLabelId.length-grdCtlId.length-22; 
    var TotalReceived1=0; TotalReceived2=0;    
    for(var r=0; r<TotalRowsInGrid; r++)
    {
        if(r<100)PadLength=2;
        else if(r<1000)PadLength=3;
        var boxId=grdCtlId + PadLeft(r+2,PadLength,'0')+"_txtItemsReceived";
        var boxvalue=document.getElementById(boxId).value;
        TotalReceived1 = TotalReceived1 + parseInt(boxvalue)-0;        
    }    
    
    for(var r=0; r<TotalRowsInGrid; r++)
    {
        if(r<100)PadLength=2;
        else if(r<1000)PadLength=3;
        var bknum=grdCtlId + PadLeft(r+2,PadLength,'0')+"_lblBookingNumber2";
        if(document.getElementById(bknum).innerHTML !="")
        {
            var boxId=grdCtlId + PadLeft(r+2,PadLength,'0')+"_txtItemsReceived2";
            var boxvalue=document.getElementById(boxId).value;
            TotalReceived2 = TotalReceived2 + parseInt(boxvalue)-0;
        }       
    }
//    
}
<% } %>

function PadLeft(strTarg, padByLength, padByChar)
{
    var strDest=strTarg;    
    while(strDest.toString().length<padByLength)
    {
        strDest=padByChar+strDest;
    }
    return strDest;    
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
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Customer
                    Receipt No. Status
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
        <table style="width: 100%;">
            <tr>
                <td>
                    <table class="TableData">
                        <tr>
                            <td class="TDCaption">
                                Select Customer
                            </td>
                            <td>
                                <asp:DropDownList ID="drpVendor" runat="server" DataSourceID="SqlSourceChallanShifts"
                                    DataTextField="CustomerName" DataValueField="CustomerCode">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td nowrap="nowrap">
                                <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions"
                                    Text="From" />
                                &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" onchange="return SetUptoDate();" />
                                <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                                    Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                                </cc1:CalendarExtender>
                            </td>
                            <td nowrap="nowrap" width="150">
                                &nbsp;<span class="TDCaption">To</span>
                                <asp:TextBox ID="txtReportUpto" runat="server" Width="80px" onkeypress="return false;"
                                    onpaste="return false;" />
                                <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                                    Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                                </cc1:CalendarExtender>
                            </td>
                            <td nowrap="nowrap" id="tdForMonthSelection" runat="server">
                                <asp:RadioButton ID="radReportMonthly" runat="server" GroupName="radReportOptions"
                                    Text="Monthly Report" />
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
                <td colspan="4">
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                </td>
            </tr>
            <tr valign="top">
                <td colspan="4" class="TDCaption" style="text-align: left">
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdChallan" runat="server" AutoGenerateColumns="False" OnRowCommand="grdChallan_OnRowCommand"
                            ShowFooter="true" EmptyDataText="No Records Found."  CssClass="mGrid" 
                          >
                            <Columns>
                                <asp:TemplateField HeaderText="Receipt No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingNumber" runat="server" Text='<%# Bind("BookingNumber")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChallanDate" runat="server" Text='<%# Bind("ChallanDate")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnItemSNo" runat="server" Value='<%# Bind("ISN") %>' />
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("SubItemName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item Qty.">
                                    <ItemTemplate>                                        
                                        <asp:Label ID="lblItemQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Urgent" ItemStyle-ForeColor="Red">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUrgent" runat="server" Text='<%# Eval("Urgent").ToString() == "True" ? "Yes": "" %>' />
                                    </ItemTemplate>
                                    <ItemStyle ForeColor="Red"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proc.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMainProcess" runat="server" Text='<%# Eval("ItemProcessType").ToString() == "None" ? "": Eval("ItemProcessType").ToString()  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Clothes Received">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalQtySent" runat="server" Text='<%# Bind("ItemTotalQuantitySent") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Clothes Sent">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalQtyReceived" runat="server" Text='<%# Bind("ItemsReceivedFromVendor") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pending" ItemStyle-Wrap="false" SortExpression="ItemsPending">
                                    <ItemTemplate>
                                        <asp:Label ID="txtItemsReceived" runat="server" Width="40" onblur="return CheckMaxEntry(this);"
                                            onfocus="javascript:if(this.value=='0')this.value='';return false;" Text='<%# Bind("ItemsPending") %>'
                                            BorderStyle="None"></asp:Label>
                                        <asp:Button ID="btnAddOneItem" CommandName="AddOneItem" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            OnClientClick="return AddOneToTotal(this)" runat="server" Text="+" Visible="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalReceivedItems" runat="server" /><br />
                                        <%--<asp:Button ID="btnSaveChallanReturn" runat="server" CommandName="UpdateChallan" Text="Update" />--%>
                                    </FooterTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <%--Second part--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="4">
                    <asp:SqlDataSource ID="SqlSourceChallan" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="Sp_Sel_VendorChallanReturnDetails"
                        SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="BookNumberFrom" DefaultValue="" />
                            <asp:Parameter Name="BookNumberUpto" DefaultValue="" />
                            <asp:Parameter Name="ChallanShift" DefaultValue="" />
                            <asp:Parameter Name="VendorId" DefaultValue="" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:Button ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click"
                        Text="Export to Excel" Visible="false" Width="125px" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnSPBookingFrom" runat="server" />
    <asp:HiddenField ID="hdnSPBookingUpto" runat="server" />
    <asp:HiddenField ID="hdnSPShiftVal" runat="server" />
    <asp:TextBox ID="txtBookingNumberUpto" runat="server" Width="50px" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtBookingNumberFrom" runat="server" Width="50px" Visible="False"></asp:TextBox>
    <asp:HiddenField ID="hdnSelectedProcessType" runat="server" />
    <asp:HiddenField ID="hdnPrefixForCurrentYear" runat="server" />
    <asp:SqlDataSource ID="SqlSourceChallanShifts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT CustomerCode,CustomerName FROM CustomerMaster order by CustomerName ASC"></asp:SqlDataSource>
</asp:Content>
