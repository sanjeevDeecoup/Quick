<%@ Page Title="" Language="C#" MasterPageFile="~/WebsiteUser/WebsiteUserMain.Master"
    AutoEventWireup="true" CodeBehind="frmGarmentAndPaymentDetail.aspx.cs" Inherits="QuickWeb.WebsiteUser.frmGarmentAndPaymentDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="row-fluid well well-sm-tiny">
<div class="col-sm-4"></div>
<div class="col-sm-2 ">
<span > Order Number :</span>&nbsp;&nbsp
  <asp:Label ID="lblInvoiceNumber" runat="server" EnableViewState="false" CssClass="TextGray textBold"></asp:Label>
</div>
<div class="col-sm-3">
<span> Delivery Date :</span>&nbsp;&nbsp
  <asp:Label ID="lblDeliveryDate" runat="server" EnableViewState="false" CssClass="TextGray textBold"></asp:Label>
</div>
</div>
<div class="row-fluid">
<div class="col-sm-6">
<asp:GridView ID="grdClothDetails" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover well" EnableTheming="false" >
                <Columns>                
                <asp:BoundField DataField="ItemName" HeaderText="Garment" InsertVisible="False" ReadOnly="True"
                    SortExpression="ItemName" />
                <asp:BoundField DataField="ItemProcessType" HeaderText="Services" InsertVisible="False" ReadOnly="True"
                    SortExpression="ItemProcessType" />
                <asp:BoundField DataField="DeliverItemStaus" HeaderText="Status" InsertVisible="False" ReadOnly="True"
                    SortExpression="DeliverItemStaus" />
                <asp:BoundField DataField="ReadyOn" HeaderText="Ready on" InsertVisible="False" ReadOnly="True"
                    SortExpression="ReadyOn" />
                     <asp:BoundField DataField="Date" HeaderText="Deliver On" InsertVisible="False" ReadOnly="True"
                    SortExpression="Date" />
                </Columns>
                </asp:GridView>

</div>
<div class="col-sm-6">
 <asp:GridView ID="grdPaymentDetails" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover well" EnableTheming="false" >
                <Columns>
                 <asp:BoundField DataField="PaidOn" HeaderText="Paid On" InsertVisible="False"
                    ReadOnly="True" SortExpression="PaidOn" />
                <asp:BoundField DataField="Payment" HeaderText="Payment" InsertVisible="False" ReadOnly="True"
                    SortExpression="Payment" />
                <asp:BoundField DataField="PaymentType" HeaderText="Payment Mode" InsertVisible="False" ReadOnly="True"
                    SortExpression="PaymentType" />
                <asp:BoundField DataField="PaymentDetails" HeaderText="Payment Details" InsertVisible="False" ReadOnly="True"
                    SortExpression="PaymentDetails" />               
                </Columns>
  </asp:GridView>
</div>
</div>
</asp:Content>
