<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryStatus.aspx.cs" Inherits="QuickWeb.Website_User.DeliveryStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="Shortcut Icon" type="image/ico" href="../images/favicon.ico" />
	 <title>
		<%=AppTitle %></title>
     <link href="../css/CheckStatus.css" rel="stylesheet" type="text/css" />
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
      <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             var rowscount = $("#grdItemDetails >tbody > tr").length;
             var count = rowscount - 1;
             $('#grdItemDetails> tbody > tr:eq(' + count + ') >td').css('background-color', 'lime');
         });        
        </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title textBold">Booking Details</h3>
		</div>
		<div class="panel-body">           
          <div class="col-sm-12 nopadding">
           <div class="row-fluid">
           <div class="col-sm-11"></div>
          <div class="col-sm-1 nopadding">
            <asp:Button ID="btnClose" runat="server"  Text="Close" OnClick="btnClose_Click"  EnableTheming="false"
            CssClass="btn btn-info btn-block active" />
          </div>
          </div>

          <div class="row-fluid  well well-sm-tiny">
          <div style="overflow: auto; height: 460px;">
           <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="mgrid" EmptyDataText="No Record Found" EnableTheming="false" >
            <Columns>
                <asp:TemplateField  HeaderText="Item">
                    <ItemTemplate>
                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' />
                        <asp:HiddenField ID="hdnItemSN" runat="server" Value='<%# Bind("ISN") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemTotalQuantity" HeaderText="Total Qty" SortExpression="ItemTotalQuantity" />
                <asp:BoundField DataField="BarCode" HeaderText="BarCode" SortExpression="BarCode" />
                <asp:BoundField DataField="ItemProcessType" HeaderText="Process" SortExpression="ItemProcessType" />
                <%--<asp:TemplateField HeaderText="Select All">--%>
                    <%--<HeaderTemplate>
                        <asp:CheckBox ID="CheckAll" Text=" Select All" runat="server" onclick="checkAll(this);" />
                    </HeaderTemplate>--%>
                   <%-- <ItemTemplate>
                        <asp:CheckBox ID="txtItemDelivered" runat="server" Checked='<%# Bind("DeliveredQty") %>' />
                    </ItemTemplate>--%>
                    <%--<FooterTemplate>
                        <asp:Button ID="btnUpdateItemDelivery" runat="server" CommandName="UPDATEITEMDELIVERY"
                            Text="Deliver" CausesValidation="false" />
                    </FooterTemplate>--%>
                   <%-- <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                <asp:BoundField DataField="ISN" HeaderText="SNo." SortExpression="ISN" />
                <%-- <asp:BoundField DataField="AllottedDrawl" HeaderText="Rack" SortExpression="AllottedDrawl" />--%>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("DeliverItemStaus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("ItemRemarks") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ready On">
                    <ItemTemplate>
                        <asp:Label ID="lblReadyOn" runat="server" Text='<%# Bind("ReadyOn") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delivery Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AcceptedUser" HeaderText="Accepted By" SortExpression="AcceptedUser" />
            </Columns>
        </asp:GridView>
        </div>          
          </div> 
          </div>  
  </div>
  </div>
    </form>
</body>
</html>

