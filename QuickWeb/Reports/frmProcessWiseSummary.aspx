<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmProcessWiseSummary.aspx.cs" Inherits="QuickWeb.Reports.frmProcessWiseSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
       <script src="../js/bootstrap.min.js" type="text/javascript"></script> 
  
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
<div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
               Service Wise Order Summary Report -    <asp:Label ID="lblDescription" runat="server" Text="Label" Font-Bold="true"></asp:Label>                
            </h3>
        </div>
        <div class="panel-body">           
            <div class="row-fluid" style="height: 400px; background-color: White; overflow: auto">

            <asp:GridView ID="grdReport" runat="server"  ShowFooter="True" CssClass="Table Table-striped Table-bordered Table-hover"
                          EnableTheming="false"  AutoGenerateColumns="false" EmptyDataText="No Service Wise Summary record found." PageSize="50" OnRowDataBound="grdReport_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Order Number" FooterStyle-BackColor="#E4E4E4">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                            Target="_blank" NavigateUrl='<%# String.Format("~/Bookings/BookingSlip.aspx?BN={0}{1}{2}",Eval("BookingNumber"),-1,Eval("DeliveryDate")) %>' />
                                             <asp:HiddenField ID="hdnDeliveryDate" runat="server" Value='<%# Bind("DeliveryDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Customer Details" DataField="Customer" ItemStyle-HorizontalAlign="Left" FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Order Date" DataField="BookingDate" ItemStyle-HorizontalAlign="Left" FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Due Date" DataField="DeliveryDate" ItemStyle-HorizontalAlign="Left"  FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Garment Details Qty Wise" DataField="ItemDetails" ItemStyle-HorizontalAlign="Left" FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Qty" DataField="Qty" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="Pcs" DataField="Pcs" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Amt without Tax & Dis" DataField="Amount" ItemStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                    FooterStyle-HorizontalAlign="Right" />
                            </Columns>
                             <FooterStyle Font-Bold="true" />
                    <HeaderStyle Font-Size="12px" />
                        </asp:GridView>

            </div>
        </div>
        <div class="panel-footer">
        

                  <asp:LinkButton ID="btnExportExcel" runat="server" class="btn btn-primary"  EnableTheming="false"  OnClick="btnExportExcel_Click"
                        Visible="False" ><i class="fa fa-file-excel-o"></i>&nbsp;Export To Excel</asp:LinkButton>
        </div>
        
      
    </div>

     <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT BookingDate, SUM(NetAmount) FROM [ViewBookingReport]">
                    </asp:SqlDataSource>

 <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] ORDER BY [ProcessName]">
                    </asp:SqlDataSource>
                      <asp:HiddenField ID="hdnFromDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnToDate" runat="server" ClientIDMode="Static" />

  
</asp:Content>
