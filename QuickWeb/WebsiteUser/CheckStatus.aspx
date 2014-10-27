<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckStatus.aspx.cs" Inherits="QuickWeb.Website_User.CheckStatus" %>

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
			var rowscount = $("#grdCustomerWiseReport >tbody > tr").length;
			var count = rowscount - 1;
			$('#grdCustomerWiseReport> tbody > tr:eq('+count+') >td').css('background-color', 'lime');
		});        
		</script>
</head>
<body>
	<form id="form1" runat="server">

	<div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title textBold">Check Status</h3>
		</div>
		<div class="panel-body">           
		  <div class="col-sm-12 nopadding">

          <div class="row-fluid">
           <div class="col-sm-11"></div>
          <div class="col-sm-1 nopadding">
           <asp:Button ID="Button1" runat="server"  Text="Close" OnClick="btnClose_Click" EnableTheming="false"
			CssClass="btn btn-info  btn-block active"  />
          </div>
          </div>


		  <div class="row-fluid  well well-sm-tiny">
		  <div style="overflow: auto; height: 460px;">
		<asp:GridView ID="grdCustomerWiseReport" runat="server" AutoGenerateColumns="False"
			EmptyDataText="No Records found." Visible="True" PageSize="10" ShowFooter="True" EnableTheming="false"
			CssClass="mgrid">
			<Columns>
				<asp:BoundField DataField="BookingDate" DataFormatString="{0:d}" HeaderText="Order Date / Time"
					SortExpression="BookingDate" FooterText="Total" />
				<asp:TemplateField HeaderText="Order No.">
					<ItemTemplate>
						<asp:HyperLink ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
							Target="_blank" NavigateUrl='<%# String.Format("~/Website user/DeliveryStatus.aspx?BN={0}{1}{2}",Eval("BookingNumber"),"-",Eval("BranchId")) %>' />
						<asp:HiddenField ID="hidBookingStatus" runat="server" Value='<%# Bind("BookingStatus") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Customer Detail" SortExpression="CustomerName">
					<ItemTemplate>
						<asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
						<br></br>
						<asp:Label ID="lblCustomerAddress" runat="server" Text='<%# Bind("CustomerAdress") %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
					Visible="false"></asp:BoundField>
				<asp:TemplateField HeaderText="Due Date">
					<ItemTemplate>
						<asp:Label ID="lblDate" runat="server" Text='<%# Bind("DueDate") %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px">
					<ItemTemplate>
						<asp:Label ID="lblDeliverDate" runat="server" Text='<%# Bind("DeliverDate") %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField DataField="Qty" HeaderText="Pcs." ItemStyle-HorizontalAlign="Right"
					SortExpression="Quantity">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="TotalCost" HeaderText="Gross Amount" ItemStyle-HorizontalAlign="Right"
					SortExpression="TotalCost">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="DiscountOnPayment" HeaderText="Discount" ItemStyle-HorizontalAlign="Right"
					SortExpression="DiscountOnPayment">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="ST" HeaderText="Tax" ItemStyle-HorizontalAlign="Right"
					SortExpression="ST">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="NetAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
					SortExpression="NetAmount">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="PaymentMade" HeaderText="Advance" ItemStyle-HorizontalAlign="Right"
					SortExpression="PaymentMade">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="Paid" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
					SortExpression="Paid">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="DeliveryDiscount" HeaderText="D. Discount" ItemStyle-HorizontalAlign="Right"
					SortExpression="DeliveryDiscount">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="DuePayment" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
					SortExpression="DuePayment">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="BookingAcceptedByUserId" HeaderText="Booked By" ItemStyle-HorizontalAlign="Right"
					SortExpression="BookingAcceptedByUserId">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="WorkShopNote" HeaderText="WorkShop Note" ItemStyle-HorizontalAlign="Right"
					SortExpression="WorkShopNote">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundField>
				<asp:BoundField DataField="OrderNote" HeaderText="Order Note" ItemStyle-HorizontalAlign="Right"
					SortExpression="OrderNote">
					<FooterStyle HorizontalAlign="Right" />
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				  </asp:BoundField>
				  <asp:BoundField DataField="BranchId" HeaderText="Branch Id" ItemStyle-HorizontalAlign="Right" SortExpression="Branchid" Visible="false" />
			</Columns>
		</asp:GridView>
		</div>          
		  </div>          
		 
		  </div>  
  </div>
  </div>
	<div>
	   
	</div>
	</form>
</body>
</html>
