<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="PackageReportDetails.aspx.cs" Inherits="QuickWeb.Reports.PackageReportDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script src="../js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Package Details -
                <asp:Label ID="lblCustName" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label>
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid" style="height: 355px; background-color: White;overflow:auto">
                <asp:GridView ID="grdReport" runat="server" Caption="" ShowFooter="True" AutoGenerateColumns="false"
                    CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                    EmptyDataText="No record found" PageSize="50" OnRowDataBound="grdReport_RowDataBound">
                    <Columns>
                      <asp:BoundField HeaderText="Custome  Name" DataField="Customername" ItemStyle-HorizontalAlign="Left" 
                            FooterStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Order Number">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                    Target="_blank" NavigateUrl='' />
                                     <asp:HiddenField ID="hdnDate" runat="server" Value='<%# Bind("PaymentDate") %>' /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Date" DataField="PaymentDate" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Available Balance" DataField="OpeningBalance" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="Consumed" DataField="PaymentMade" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" />
                       <%-- <asp:BoundField HeaderText="Total Amt. paid" DataField="runningTotal" ItemStyle-HorizontalAlign="Right"
                            Visible="false" FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="Balance" DataField="Bal" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" />--%>
                    </Columns>
                    <FooterStyle BackColor="#F0F0F0" Font-Bold="true" />
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" class="btn btn-primary" EnableTheming="false" runat="server"
                ClientIDMode="Static" Visible="true" OnClick="btnExport_Click"><i class="fa fa-th fa-lg"></i> Export to Excel</asp:LinkButton>
        </div>
        <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
    </div>
    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT BookingDate, SUM(NetAmount) FROM [ViewBookingReport]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT ProcessCode, [ProcessName] FROM [ProcessMaster] ORDER BY [ProcessName]">
    </asp:SqlDataSource>
</asp:Content>
