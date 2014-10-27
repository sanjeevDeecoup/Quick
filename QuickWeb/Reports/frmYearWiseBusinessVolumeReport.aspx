<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmYearWiseBusinessVolumeReport.aspx.cs" Inherits="QuickWeb.Reports.frmYearWiseBusinessVolumeReport"
    EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>      
     <script type="text/javascript">
         $(document).ready(function (e) {
             document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').onclick = function () { window.location = window.location.href.substr(0, window.location.href.indexOf('?')); return false; };
         });
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="Label2" runat="server" EnableViewState="False" ForeColor="#CC0000"></asp:Label>
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Business Summary              
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="form-group col-sm-2">
                    <div class="input-group">
                     <span class="input-group-addon IconBkColor">
                                Duration</span>
                        <asp:DropDownList ID="drpDefView" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Yearly"></asp:ListItem>
                                                <asp:ListItem Text="Monthly"></asp:ListItem>
                                            </asp:DropDownList>                         
                    </div>
                </div>
                <div class="col-sm-2">                  
                    <asp:LinkButton ID="btnSaveViewSettings" runat="server" class="btn btn-primary" EnableTheming="false" OnClick="btnSaveViewSettings_click" ClientIDMode="Static"><i class="fa fa-floppy-o"></i>&nbsp;Save</asp:LinkButton>                      
                     <asp:LinkButton ID="btnRefresh" runat="server" class="btn btn-primary" EnableTheming="false" ClientIDMode="Static"><i class="fa fa-refresh"></i>&nbsp;Refresh</asp:LinkButton>                      
                </div>                
            </div>
            <div id="divGrid" class="row-fluid" style="height: 400px;  background-color: White; overflow: auto; white-space:nowrap">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="Duration" EnableTheming="false"
                                        ShowFooter="True" EmptyDataText="No record found" PageSize="50" CssClass="Table Table-striped Table-bordered Table-hover"
                                        OnRowDataBound="grdReport_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Duration" FooterStyle-BackColor="#E4E4E4">
                                                <ItemTemplate>
                                                    <asp:HyperLink Target="_self" Text='<%# Eval("Duration") %>' runat="server" ID="hplNavigate">                                                 
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" />   --%>
                                            <asp:BoundField DataField="TotalBookings" HeaderText="Total Orders" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="TotalBookings">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BkdQty" HeaderText="Booked Pcs" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="BkdQty">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dlvrdqty" HeaderText="Delivered Pcs" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="dlvrdqty">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                           <%-- <asp:BoundField DataField="rmngQty" HeaderText="Remaining Qty" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="rmngQty" Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>--%>
                                          <%--  <asp:BoundField DataField="bkdAmt" HeaderText="Booked Amt" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="BkdAmt" Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>--%>
                                            <asp:BoundField DataField="rcvdAmt" HeaderText="Received Amt" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="rcvdAmt">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                           <%-- <asp:BoundField DataField="balAmt" HeaderText="Pending Amt" HeaderStyle-HorizontalAlign="Right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="Right" SortExpression="balAmt" Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>--%>
                                            <asp:BoundField DataField="OtherExpenses" HeaderText="Other Income" HeaderStyle-HorizontalAlign="right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="right" SortExpression="OtherExpenses">
                                                <FooterStyle HorizontalAlign="right" />
                                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Expenses" HeaderText="Payment" HeaderStyle-HorizontalAlign="right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="right" SortExpression="Expenses"> 
                                                <FooterStyle HorizontalAlign="right" />
                                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bal" HeaderText="Bal Amt" HeaderStyle-HorizontalAlign="right" FooterStyle-BackColor="#E4E4E4"
                                                ItemStyle-HorizontalAlign="right" SortExpression="bal">
                                                <FooterStyle HorizontalAlign="right" />
                                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                                            </asp:BoundField>
                                        </Columns>
                                         <FooterStyle Font-Bold="true" />
                                          <HeaderStyle Font-Size="12px" />                                       
                                    </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" runat="server" class="btn btn-primary" EnableTheming="false"
                OnClick="btnExport_Click" Visible="False"><i class="fa fa-file-excel-o"></i>&nbsp;Export to Excel</asp:LinkButton>            
        </div>
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustId" Value="" runat="server" />
        <asp:HiddenField ID="hdntmpColCount" Value="" runat="server" />
         <asp:HiddenField ID="hdnYear" runat="server" />
    <asp:HiddenField ID="hdnRefresh" ClientIDMode="Static" Value="false" runat="server" />
     <asp:Label ID="lblStatus" runat="server"></asp:Label>
      <asp:Label ID="lblDefaultView" runat="server"></asp:Label>
    </div>
       
</asp:Content>
