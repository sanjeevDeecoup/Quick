<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true" CodeBehind="frmDeleteBooking.aspx.cs" Inherits="QuickWeb.New_Admin.frmDeleteBooking" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
<script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
 <script language="javascript" type="text/javascript">
	 function CheckBookingNumber() {
		 var bn = document.getElementById("<%= txtReceiptNumber.ClientID %>").value;
		 if (bn == "") {			 
			 $('#<%=lblError.ClientID%>').html("Please enter Order number");
			 document.getElementById("<%= txtReceiptNumber.ClientID %>").focus();
			 return false;
		 }
	 }
	 function ConfirmDelete() {

		 return confirm("Are you sure you want to Delete this Order?");

	 }

	 $(document).ready(function (event) {

		 $(document).keypress(function (event) {
			 var textval = $('#ctl00_ContentPlaceHolder1_txtReceiptNumber').val();
			 var keycode = (event.keyCode ? event.keyCode : event.which);
			 if (keycode == '13') {

				 if (textval == "") {
					 return false;
				 }
				 else {
					 document.getElementById("ctl00_ContentPlaceHolder1_btnSDetails").click();
				 }
			 }
		 });

	 });
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title">Delete Order</h3>
		</div>
		<div class="panel-body">            
			<div>			
			<asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />             
			</div>						
			<div class="form-inline" style="text-align:center">
			 <div class="form-group">						
				<div class="span3">
				<asp:TextBox ID="txtReceiptNumber" runat="server" class="form-control input-lg"    MaxLength="15"
					placeholder="Enter Order No."  EnableTheming="false" TabIndex="1" Font-Size="18px" />
		     <cc1:FilteredTextBoxExtender ID="txtReceiptNumber_FilteredTextBoxExtender" 
					runat="server" Enabled="True"  ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-"
					TargetControlID="txtReceiptNumber">
				</cc1:FilteredTextBoxExtender>
				</div>  
			</div>               				
			<asp:Button ID="btnSDetails"  EnableTheming="False" runat="server"  CssClass="btn btn-info btn-lg" 
			OnClick="btnSDetails_Click" Text="Show Details"   TabIndex="2" OnClientClick="return CheckBookingNumber()" />
			 </div>
			 <div class="row" style="text-align:center"> 
			   <h3><asp:Label ID="lblMsg" runat="server"  EnableTheming="False"  EnableViewState="False"  class="label label-success" /></h3>			
			  <h3><asp:Label ID="lblError" EnableTheming="False" runat="server" ClientIDMode="Static" EnableViewState="false" class="label label-warning"></asp:Label></h3>
			 </div>

				 <div class="row">
				   <div class="col-md-1"></div>
				  <div class="col-md-10">                  
		<div class="panel panel-danger" id="pnlShowDetails" runat="server" visible="false">
	<div class="panel-heading">
		<h3 class="panel-title">Order No. Details</h3>
	</div>
	<div class="panel-body">
	<div class="well-sm">
		  <div class="col-md-8"> 	
		  
			  <asp:DetailsView ID="dtvBookingDetails" runat="server" AutoGenerateRows="False" EmptyDataText="Incorrect receipt number"
					Visible="False" class="table table-striped table-bordered table-hover">
					<Fields>
						<asp:BoundField DataField="BookingNumber" HeaderText="Order Number" SortExpression="BookingNumber">
						  <HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>
						<asp:TemplateField HeaderText="Customer Name" SortExpression="CustName">
							<ItemTemplate>
								(<asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerCode") %>' />)<asp:Label
									ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>' />
							</ItemTemplate>
							<HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:TemplateField>
						<asp:BoundField DataField="CustomerAddress" HeaderText="Customer Address" SortExpression="CustomerAddress">
						<HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						 </asp:BoundField>

						<asp:BoundField DataField="BookingDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Booking Date"
							SortExpression="BookingDate">
						  <HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>
						<asp:BoundField DataField="NetAmount" HeaderText="Net Amount" SortExpression="NetAmount">
						   <HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>
						<asp:BoundField DataField="PaymentMade" HeaderText="Payment Made" SortExpression="PaymentMade">
						  <HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>						
					</Fields>
				</asp:DetailsView>				
	

		</div>					 
		  <div class="col-md-4">  					
					<div class="well" style="min-height:258px" >
					 <br /><br /><br /><br />					         		
					 <asp:Button ID="btnShowDetails"   runat="server" ClientIDMode="Static" EnableTheming="False" CssClass="btn btn-danger btn-lg btn-block" 
					  Text="Delete Order" TabIndex="3" OnClick="btnShowDetails_Click" Visible="False"   OnClientClick="return ConfirmDelete()"   />
					</div>
		</div>							 
	</div>			   
	 </div>
	 </div>                                 
	 </div>                
	 <div class="col-md-1"></div>
  </div>      
  </div>
  </div>
</asp:Content>
