<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true" CodeBehind="frmMergeCustomer.aspx.cs" Inherits="QuickWeb.New_Admin.frmMergeCustomer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
<script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
  <script type="text/javascript" language="javascript">
      function checkName() {
          var strname = document.getElementById("<%=txtCustomerSearch.ClientID %>").value.trim().length;
          var stradd = document.getElementById("<%=txtDuplicateCustomer.ClientID %>").value.trim().length;
          if (strname == "" || strname.length == 0) {
              // alert("Please enter main Customer.");
              $('#<%=lblError.ClientID%>').html("Please enter main Customer.");
              document.getElementById("<%=txtCustomerSearch.ClientID %>").focus();
              return false;
          }
          if (stradd == "" || stradd.length == 0) {
              // alert("Please enter duplicate Customer.");
              $('#<%=lblError.ClientID%>').html("Please enter duplicate Customer.");
              document.getElementById("<%=txtDuplicateCustomer.ClientID %>").focus();
              return false;
          }
          return confirm('Are you sure you want merge this customer?');
      }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title">Merge Customer</h3>
		</div >
		<div class="panel-body">  
		<div>
		<div class="col-md-1"></div>
		
		<div class="col-md-10">

		<div class="panel panel-info">
		<div class="panel-heading">
		  <h3 class="panel-title"> Customer Selection</h3>
		</div>
		<div class="panel-body">
		 
		   <div class="row-fluid">
			 <div class="col-md-6">
			 <div class="well">	         
				<h4 class="Legend" style="margin-top:0px">&nbsp; Orginal Customer</h4>
				 <div class="form-group">
				<label class="sr-only" for="txtCustomerSearch">Enter Orginal Customer</label>					
				<asp:TextBox ID="txtCustomerSearch" runat="server" class="form-control input-lg" AutoPostBack="True" onfocus="javascript:select();"
					placeholder="Enter Orginal Customer"  EnableTheming="false" ToolTip="Please enter customer name." OnTextChanged="txtCustomerSearch_TextChanged"
							TabIndex="1" Font-Size="16px" />
					<cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerSearch"
									ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
									CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" DelimiterCharacters=""
									Enabled="True" CompletionListCssClass="AutoExtender_new" CompletionListItemCssClass="AutoExtenderList_new"
									CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
								</cc1:AutoCompleteExtender>	
			   </div>	
				 <asp:DetailsView ID="grdCustomerSearch" 
					 class="table table-striped table-bordered table-hover" runat="server" 
						 AutoGenerateRows="False"   AllowPaging="True" AllowSorting="True">
					<Fields>
				<asp:TemplateField HeaderText="Ledger Name" SortExpression="LedgerName">
					<ItemTemplate>
						<asp:Label ID="lnlBtnLedgerName" runat="server" Text='<%#Bind("CustomerName") %>' />                        
					</ItemTemplate>
					<HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Customer Address" SortExpression="CustomerAddress">
					<ItemTemplate>
						<asp:Label ID="lnlBtnCustomerAddress" runat="server" Text='<%#Bind("CustomerAddress") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Total Due" SortExpression="TotalDue">
					<ItemTemplate>
						<asp:Label ID="lnlBtnCustomerTotalDue" runat="server" Text='<%#Bind("DuePayment") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
                  <asp:TemplateField HeaderText="Pending Pcs" SortExpression="PendingPcs">
					<ItemTemplate>
						<asp:Label ID="lblPendingPcss" runat="server" Text='<%#Bind("PendingPcs") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
					</Fields>
				</asp:DetailsView>		 
					
			</div>
			</div>
			 <div class="col-md-6">
			 <div class="well">
			 <h4 class="Legend" style="margin-top:0px">&nbsp; Duplicate Customer</h4>		
				<div class="form-group">
				<label class="sr-only" for="txtDuplicateCustomer">Duplicate Customer</label>		
				<asp:TextBox ID="txtDuplicateCustomer" runat="server" class="form-control input-lg" AutoPostBack="True" onfocus="javascript:select();"
					placeholder="Enter Duplicate Customer"  EnableTheming="false" ToolTip="Please enter customer name." OnTextChanged="txtDuplicateCustomer_TextChanged"
				TabIndex="2" Font-Size="16px" />			
			  </div>
			<cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtDuplicateCustomer" 
				ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
				CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" DelimiterCharacters=""
				Enabled="True" CompletionListCssClass="AutoExtender_new" CompletionListItemCssClass="AutoExtenderList_new"
				CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
			</cc1:AutoCompleteExtender>			
			<asp:DetailsView ID="GridViewDuplicate" class="table table-striped table-bordered table-hover" 
				 runat="server" AutoGenerateRows="False"  AllowPaging="True" AllowSorting="True"   CellPadding="4">
					<Fields>
					<asp:TemplateField HeaderText="Ledger Name" SortExpression="LedgerName">
					<ItemTemplate>
						<asp:Label ID="lnlBtnLedgerName" runat="server" Text='<%#Bind("CustomerName") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Customer Address" SortExpression="CustomerAddress">
					<ItemTemplate>
						<asp:Label ID="lnlBtnCustomerAddress" runat="server" Text='<%#Bind("CustomerAddress") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Total Due" SortExpression="TotalDue">
					<ItemTemplate>
						<asp:Label ID="lnlBtnCustomerTotalDue" runat="server" Text='<%#Bind("DuePayment") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
                <asp:TemplateField HeaderText="Pending Pcs" SortExpression="PendingPcs">
					<ItemTemplate>
						<asp:Label ID="lblPendingPcs" runat="server" Text='<%#Bind("PendingPcs") %>' />
					</ItemTemplate>
					 <HeaderStyle Font-Bold="True"   ForeColor="Black" />
							<ItemStyle  />
				</asp:TemplateField>
					</Fields>
				</asp:DetailsView>
				
			</div>
		 </div> 
			</div>           
			<div class="row-fluid" style="text-align:center">  
			  <h3><asp:Label ID="lblMsg" runat="server" EnableTheming="False" EnableViewState="False" ClientIDMode="Static" CssClass="label label-success" /></h3>
			  <h3><asp:Label ID="lblError" EnableTheming="False" runat="server" ClientIDMode="Static" EnableViewState="false" class="label label-warning"></asp:Label></h3>
			 </div>		 
			<div class="row-fluid" >
			<div class="col-md-4">
			</div>			
			<div class="col-md-4">		 
				<div  class="well-sm">
				<asp:Button ID="btnMerge" runat="server" Text="Merge Customer" OnClientClick="return checkName();" 
				OnClick="btnMerge_Click" TabIndex="3" ClientIDMode="Static" EnableTheming="False" CssClass="btn btn-danger btn-lg btn-block"   />
				  </div>
			</div>
			</div>	
		</div>
	  </div>     
	   </div> 	
		<div class="col-md-1"></div>
		</div>
	  </div>      
  <asp:HiddenField ID="hdnNewCustomer" runat="server" Value="0" />
  <asp:HiddenField ID="hdnOldCustomer" runat="server" Value="0" />
  </div>
</asp:Content>
