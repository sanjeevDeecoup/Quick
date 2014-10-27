<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="SearchByInvoice.aspx.cs" Inherits="QuickWeb.Reports.SearchByInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/moment.js" type="text/javascript"></script>
    <script src="../JavaScript/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkName() {           
            var strname = document.getElementById("<%=txtInvoiceNo.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                setDivMouseOver('Red', '#999999');
                $('#lblMsg').text('Please enter a valid order no.');               
                document.getElementById("<%=txtInvoiceNo.ClientID %>").focus();
                return false;
            }
        }
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
        }

        $(document).ready(function (event) {

            $(document).keypress(function (event) {
                var textval = $('#txtInvoiceNo').val();
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    if (textval == "") {
                        return false;
                    }
                    else {
                        document.getElementById("btnShowReport").click();
                    }
                }
            });

        });
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ClientIDMode="Static"></asp:Label>
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                View the Details of the Seleceted Order               
            </h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="form-group col-sm-2">
                    <div class="input-group">
                         <span class="input-group-addon IconBkColor"><i class="fa fa-download fa-lg"></i>
                        </span>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" CssClass="form-control" placeholder="Search Order" ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtInvoiceNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtInvoiceNo" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                                </cc1:FilteredTextBoxExtender>
                    </div>                     
                </div>
                <div class="col-sm-2">                    
                                     <asp:LinkButton ID="btnShowReport" EnableTheming="False" runat="server" ClientIDMode="Static" 
                        CssClass="btn btn-primary" OnClick="btnShowReport_Click" OnClientClick="return checkName();"><i class="fa fa fa-list-alt fa-lg"></i>&nbsp;Show</asp:LinkButton>
                     </div>
            </div>
            <div id="divGrid" class="row-fluid" style="height: 400px; width:1200Px; background-color: White; overflow: auto; white-space:nowrap">
                <asp:DetailsView ID="DetailsViewDeliverSlip" runat="server" AutoGenerateRows="False"
                        Height="50px" Visible="False" CssClass="Table Table-striped Table-bordered Table-hover">
                        <Fields>
                            <asp:BoundField DataField="BookingNumber" HeaderText="Invoice No." SortExpression="BookingNumber">                               
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Customer Name" SortExpression="CustomerName">
                                <ItemTemplate>
                                    (<asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerCode") %>'></asp:Label>)
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                               
                            </asp:TemplateField>
                            <asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date"
                                SortExpression="BookingDate">
                               
                            </asp:BoundField>
                             <asp:BoundField DataField="DeliveryDate" HeaderText="Due Date" SortExpression="DeliveryDate"
                                            DataFormatString="{0:d}">
                                            
                                        </asp:BoundField>
                            <asp:BoundField DataField="ClothDeliverDate" HeaderText="Delivered On" SortExpression="ClothDeliverDate"
                                DataFormatString="{0:d}">
                               
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                              
                            </asp:BoundField>
                        </Fields>
                    </asp:DetailsView>
            </div>
        </div>       
        <asp:HiddenField ID="hdnDateFromAndTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustId" Value="" runat="server" />
    </div>    
</asp:Content>
