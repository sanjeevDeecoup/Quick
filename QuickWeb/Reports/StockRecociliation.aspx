<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" CodeBehind="StockRecociliation.aspx.cs" Inherits="QuickWeb.Reports.StockRecociliation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Shortcut Icon" type="image/ico" href="../images/DRY.jpg" />
    <title></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/code.js" type="text/javascript"></script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>
                    Stock Reconciliation Report
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
            <tr valign="top"><td nowrap="nowrap">
                                            Item Name
                                        </td>
                                        <td  width="400px">
                                            <asp:DropDownList ID="drpItemNames" runat="server" Width="100px">
                                            </asp:DropDownList>
                                            &nbsp;
                                            &nbsp;
                                            &nbsp;&nbsp;Customer Name&nbsp;&nbsp;
                                        
                                            <asp:TextBox ID="txtCName" runat="server"  width="150px"
                                                AutoPostBack="True" ontextchanged="txtCName_TextChanged"></asp:TextBox>
                                            <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCName"
                                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="1"
                                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                            </cc1:AutoCompleteExtender>
                                        </td>                                       
                                        <td>
                                            <asp:CheckBox ID="chkInvoice" runat="server" Text="Search By Invoice No." 
                                                AutoPostBack="True" oncheckedchanged="chkInvoice_CheckedChanged"
                                                />
                                        
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" Visible="false"></asp:TextBox>
                                           &nbsp;&nbsp; Bar Code <asp:TextBox ID="txtScanner" runat="server" 
                                                AutoPostBack="true" ontextchanged="txtScanner_TextChanged" ></asp:TextBox>
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show" 
                                                onclick="btnShowReport_Click"  />         
                            </td>
                        </tr>
                        </table>
                        <table>
                        <tr><td >
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" 
                                CssClass="SuccessMessage"  ></asp:Label>
                                 <asp:Label ID="lblReconcile" runat="server" EnableViewState="False" 
                                CssClass="SuccessMessage"  ></asp:Label>
                                
                        </td></tr>
                        <tr align="center" style="font-family: Verdana; font-size: 16px; font-weight: bold">
                        <td>Inventory</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>Cloths in System/Shop</td>                        
                        </tr>                        
                        <tr>                           
                            <td class="TDCaption">
                    <div class="DivStyleWithScroll" style="height:250px;">
                                    
                                    <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                                       EmptyDataText="No record found" PageSize="50" Width="100%" ShowFooter="true">
                                        <Columns>
                                           <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="B. Date"
                                                SortExpression="BookingDate" />
                                            <asp:TemplateField HeaderText="Inv No.">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                        Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="item" HeaderText="Sub Item" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="item">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="Status">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No" ItemStyle-HorizontalAlign="Right"
                                                SortExpression="CustomerMobile">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>                                            
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="Gridexcel" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                                         EmptyDataText="No record found" PageSize="50" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="B. Date"
                                                SortExpression="BookingDate" />                                           
                                            <asp:TemplateField HeaderText="Inv No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatno" runat="server" Text='<%# Bind("BookingNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="item" HeaderText="Sub Item" SortExpression="item"></asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                                            <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No" SortExpression="CustomerMobile">
                                            </asp:BoundField>
                                            </Columns>
                                    </asp:GridView>
                                    <br />
                                
                          </div>  </td>
                            <td></td>
                            <td></td><td style="width:2%;"></td>
                            <td class="TDCaption">
                             <div class="DivStyleWithScroll" style="height:250px;">
                        <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="BookingNumber"
                                     EmptyDataText="No record found" PageSize="50" Width="100%" ShowFooter="true" >
                                        <Columns>
                                            <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="B.Date"
                                                SortExpression="BookingDate" />
                                           <asp:TemplateField HeaderText="Inv No.">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                                        Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DueDate")) %>' />
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Size="Large" Font-Bold="true" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="item" HeaderText="Sub Item" SortExpression="item"></asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                                            <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No" SortExpression="CustomerMobile">
                                            </asp:BoundField>
                                            </Columns>
                                    </asp:GridView></div></td></tr>     
                        <tr>
                       <td>
                                <asp:Button ID="btnExport1" runat="server" Text="Export to Excel" 
                                    Visible="False" onclick="btnExport1_Click" />
                                    <td>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" 
                                    Visible="False" onclick="btnExport_Click1"
                                     />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                
                            </td>
                               
                            </td></tr></table>
                         </fieldSet>
                         <fieldset class="Fieldset">
                            <table>
                            <tr align="center">
                            <td width="75%"></td>
                            <td><asp:Button ID="btnReconcile" runat="server" Text="Reconciliation Done" onclick="btnReconcile_Click" 
                                     /></td></tr>
                        </table>
                
           
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            
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
    <asp:HiddenField ID="hdnCustId" runat="server" />
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />    
    <asp:HiddenField ID="hdnCheckStatus" Value="0" runat="server" />
    <asp:HiddenField ID="hdnInvoiceNo" runat="server" />
     
</asp:Content>
