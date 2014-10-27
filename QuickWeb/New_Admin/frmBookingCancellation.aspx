<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true" CodeBehind="frmBookingCancellation.aspx.cs" Inherits="QuickWeb.New_Admin.frmBookingCancellation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
<script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
	function CheckBookingNumber() {
		var bn = document.getElementById("<%= txtReceiptNumber.ClientID %>").value;
		if (bn == "") {
			//alert("Please enter receipt number");
			$('#<%=lblError.ClientID%>').html("Please enter receipt number");
			document.getElementById("<%= txtReceiptNumber.ClientID %>").focus();
			return false;
		}
	}

	function ConfirmCancel() {
		var bn = document.getElementById("<%= txtRemarks.ClientID %>").value;
		if (bn == "") {
			//			alert("Please enter reason for cancellation");
			$('#<%=lblError.ClientID%>').html("Please enter reason for cancellation");
			document.getElementById("<%=txtRemarks.ClientID %>").focus();
			return false;
		}
		else {
			return confirm("Are you sure you want to cancel this Order?");
		}
	}
	function setPrefix() {
		document.getElementById("<%= txtReceiptNumber.ClientID %>").value = document.getElementById("<%= hdnPrefixForCurrentYear.ClientID %>").value;

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
					document.getElementById("ctl00_ContentPlaceHolder1_btnShowDetails").click();
				}
			}
		});

	});		 
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title">Order Cancellation</h3>
		</div>
		<div class="panel-body">            
			<div>			
			<asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage" />             
			</div>						
			<div class="form-inline" style="text-align:center">
			 <div class="form-group">				
				<div class="span3">
				<asp:TextBox ID="txtReceiptNumber" runat="server" class="form-control input-lg" onfocus="return setPrefix();" 
					placeholder="Order No."  EnableTheming="false" TabIndex="1" Font-Size="18px" />
                      <cc1:FilteredTextBoxExtender ID="txtReceiptNumber_FilteredTextBoxExtender" 
					runat="server" Enabled="True"   ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-"
					TargetControlID="txtReceiptNumber">
				</cc1:FilteredTextBoxExtender>
				</div>
			</div>               				
			<asp:Button ID="btnShowDetails"  EnableTheming="False" runat="server"   CssClass="btn btn-info btn-lg"  OnClick="btnShowDetails_Click" Text="Show Details" OnClientClick="return CheckBookingNumber()" />
			 </div> 

			  <div class="row" style="text-align:center"> 
			  <h3><asp:Label ID="lblMsg" runat="server" EnableTheming="False" EnableViewState="False" ClientIDMode="Static" CssClass="label label-success" /></h3>
			  <h3><asp:Label ID="lblError" EnableTheming="False" runat="server" ClientIDMode="Static" EnableViewState="false" class="label label-warning"></asp:Label></h3>
			 </div>

				 <div class="row">
				   <div class="col-md-1"></div>
				  <div class="col-md-10">                  
					  <div class="panel panel-info" id="idRemakrs" runat="server" visible="false">
					<div class="panel-heading">
					  <h3 class="panel-title">Order Information</h3>
					</div>
	<div class="panel-body">
	<div class="well-sm">
		  <div class="col-md-8">   					
		<asp:DetailsView ID="dtvBookingDetails" class="table table-striped table-bordered table-hover"  runat="server" AutoGenerateRows="False" EmptyDataText="Incorrect receipt number"
					Visible="False" >
					<Fields>
						<asp:BoundField DataField="BookingNumber" HeaderText="Order Number" SortExpression="BookingNumber">
							<HeaderStyle  Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>
						<asp:TemplateField HeaderText="Customer Name" SortExpression="CustName">
							<ItemTemplate>
								(<asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CustomerCode") %>' />)
								<asp:Label	ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>' />
							</ItemTemplate>
							<HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
						</asp:TemplateField>
						<asp:BoundField DataField="CustomerAddress" HeaderText="Customer Address" SortExpression="CustomerAddress" >
							<HeaderStyle  Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
						</asp:BoundField>
						<asp:BoundField DataField="BookingDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Booking Date"
							SortExpression="BookingDate">
							<HeaderStyle  Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
						</asp:BoundField>
						<asp:BoundField DataField="NetAmount" HeaderText="Net Amount" SortExpression="NetAmount">
							<HeaderStyle  Font-Bold="True"   ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>
						<asp:BoundField DataField="PaymentMade" HeaderText="Payment Made" SortExpression="PaymentMade">
							<HeaderStyle Font-Bold="True"  ForeColor="Black" />
							<ItemStyle   />
						</asp:BoundField>						
					</Fields>
				</asp:DetailsView>
		</div>					 
		  <div class="col-md-4">  					
					<div class="well" style="min-height:258px" >
					 <br /><br />
					 <div class="form-group">
					<label class="sr-only" for="txtRemarks">Please Enter cancellation Reason</label>				  
					<asp:TextBox ID="txtRemarks"  runat="server" class="form-control" onfocus="return setPrefix();"   MaxLength="250"
					 placeholder="Please Enter cancellation Reason"  TextMode="MultiLine" />
					   <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtRemarks"
									ServicePath="~/AutoComplete.asmx" ServiceMethod="GetDetailRemoveReasonMaster"
									MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True"
									CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
									CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
								</cc1:AutoCompleteExtender> 
					<asp:SqlDataSource ID="SqlSourceBookingDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>">
					</asp:SqlDataSource>				  
					 </div>				
					 <asp:Button ID="btnCancel"   runat="server" ClientIDMode="Static" EnableTheming="False" CssClass="btn btn-danger btn-lg btn-block"  Text="Cancel this booking" OnClick="btnCancel_Click" OnClientClick="return ConfirmCancel()" Visible="False"  />
					</div>
		</div>							 
	</div>	
	<asp:HiddenField ID="hdnPrefixForCurrentYear" runat="server" /> 		   
	 </div>
	 </div>                                 
	 </div>                
	 <div class="col-md-1"></div>
  </div>      
  </div>
  </div>
</asp:Content>
