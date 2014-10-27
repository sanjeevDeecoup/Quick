<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true" CodeBehind="frmSearchInvoiceBarcode.aspx.cs" Inherits="QuickWeb.Factory.frmSearchInvoiceBarcode"   EnableEventValidation="false" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="panel panel-primary">
		<div class="panel-heading">
		  <h3 class="panel-title">Search Invoice / Garment</h3>
		</div >
		<div class="panel-body">  

		<div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2"></div>
             <div class="col-sm-3">
				<asp:DropDownList ID="drpBranch" runat="server" DataSourceID="SDTBranch" DataTextField="BranchName"
					CssClass="form-control" AppendDataBoundItems="true" DataValueField="BranchId" ClientIDMode="Static"
					EnableTheming="False">                  
				</asp:DropDownList>
				</div>
			  <div class="col-sm-3">
			   <asp:TextBox ID="txtBarcode" runat="server" placeholder="Enter Invoice No / Barcode" EnableTheming="False"
							ClientIDMode="Static" AutoComplete="off"  AutoPostBack="true" EnableViewState="false"
							CssClass="form-control" MaxLength="20" ontextchanged="txtBarcode_TextChanged"></asp:TextBox>
						<cc1:FilteredTextBoxExtender ID="txtBarcode_FilteredTextBoxExtender" runat="server"
							Enabled="True" TargetControlID="txtBarcode" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
						</cc1:FilteredTextBoxExtender>   
				</div>
			 
				<div class="col-sm-2">
				<asp:Button ID="btnSearchInvoice" runat="server" Text="Search"  ClientIDMode="Static" 
			   CssClass="btn btn-info btn-lg btn-block" EnableTheming="false" onclick="btnSearchInvoice_Click"  />
			   </div>
		</div>   
		<div class="row-fluid" style="text-align:center">
		<h3 class="well-sm-tiny"><asp:Label ID="lblMsg" runat="server" Text=""  CssClass="label label-warning" ClientIDMode="Static" EnableViewState="false"></asp:Label></h3>
		</div>

		<div class="row-fluid" id="divInvoiceDetail" runat="server" visible="false" clientidmode="Static">

		<div class="row-fluid well well-sm well-sm-tiny1">
		
		<div class="col-sm-2">
		<div class="well well-sm well-sm-tiny text-center">		
		<a class="btn btn-default btn-block active" id="achrOrder" runat="server"   EnableTheming="false" clientidmode="Static">       
		 <b>Order Details&nbsp;&nbsp;</b><asp:Label ID="lblTotalQty" runat="server" Text="" CssClass="badge1" ClientIDMode="Static"></asp:Label>            
		</a>
		<b><asp:Label ID="lblBookingDate" runat="server" Text="" CssClass="legend"  ClientIDMode="Static"></asp:Label></b>
		
		</div>
		</div>

		 <div class="col-sm-2" id="divInprocess" runat="server" clientidmode="Static" visible="false"  >
		 <div class="well well-sm well-sm-tiny text-center">
		<a class="btn btn-default btn-block active" id="achrInProcess" runat="server"   EnableTheming="false" clientidmode="Static">         
		<b>IN Process&nbsp;&nbsp;</b><asp:Label ID="lblInProcQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
	   </a>
		<b><asp:Label ID="lblInProcessDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>
		  </div>
		</div>


		 <div class="col-sm-2 " id="divWorkshopIn" runat="server" clientidmode="Static" visible="false">
		 <div class="well well-sm well-sm-tiny text-center">
		<a class="btn btn-default btn-block active" id="achrWorkshopIn" runat="server"   EnableTheming="false" clientidmode="Static">         
		<b>Workshop In&nbsp;&nbsp;</b><asp:Label ID="lblWorkshopQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
	 
		</a>
		<b> <asp:Label ID="lblWorkshopDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>		
		 </div>
		</div>

		<div class="col-sm-2 " id="divWorkshopOut" runat="server" clientidmode="Static" visible="false">
		<div class="well well-sm well-sm-tiny text-center">
		<a class="btn btn-default btn-block active" id="achrWorkshopOut" runat="server"   EnableTheming="false" clientidmode="Static">         
	  <b>Workshop Out&nbsp;&nbsp;</b><asp:Label ID="lblWorkshopOutQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
	
		</a>
		 <b><asp:Label ID="lblWorkshopOutDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>
		
		</div>
		</div>

		<div class="col-sm-3 well well-sm well-sm-tiny text-center"  id="divRecvFromWorkshop" runat="server" clientidmode="Static" visible="false">      
		<a class="btn btn-default btn-block active" id="achrRecvFromWorkshop" runat="server"   EnableTheming="false" clientidmode="Static">         
		<b>Receive From Workshop&nbsp;&nbsp;</b><asp:Label ID="lblRecvFromWorkshopQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
		</a>
		 <b><asp:Label ID="lblRecvFromWorkshopDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>
				
		</div>


		<div class="col-sm-2 well well-sm well-sm-tiny text-center" style="margin:0px 0px 0px 0px;" id="divMarkForDel" runat="server" clientidmode="Static" visible="false">      
		<a class="btn btn-default btn-block active" id="achrMarkForDel" runat="server"   EnableTheming="false" clientidmode="Static">         
		<b>Mark For Delivery&nbsp;</b><asp:Label ID="lblMarkForDelQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
		</a>
		 <b><asp:Label ID="lblMarkForDelDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>	
		</div>

		
		<div class="col-sm-2" id="divReady" runat="server" clientidmode="Static" visible="false">
		 <div class="well well-sm well-sm-tiny text-center">
		<a class="btn btn-default btn-block active" id="achrReady" runat="server"   EnableTheming="false" clientidmode="Static">         
		<b>Ready&nbsp;&nbsp;</b><asp:Label ID="lblReadyQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
		</a>
		 <b> <asp:Label ID="lblReadyDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>	
		</div>
		</div>

		 <div class="col-sm-2" id="divDelivery" runat="server" clientidmode="Static" visible="false">
		  <div class="well well-sm well-sm-tiny text-center">
		<a class="btn btn-default btn-block active" id="achrDelivery" runat="server"   EnableTheming="false" clientidmode="Static">         
		<b>Delivered&nbsp;&nbsp;</b><asp:Label ID="lblDeliveryQty" runat="server" Text="" ClientIDMode="Static" CssClass="badge1"></asp:Label>
		<asp:HiddenField ID="hdnDeliveryQty" runat="server" ClientIDMode="Static" />
		</a>
		  <b><asp:Label ID="lblDeliveryDate" runat="server" Text="" ClientIDMode="Static"></asp:Label></b>	
		</div>
		</div>

		
		</div>

		  <div class="row-fluid ">
			<div class="col-sm-7 well well-sm">
			<div class="well well-small no-bottom-margin grid-hight">                   
						<asp:GridView ID="grdInvoice" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false" EmptyDataText="No Recotd Found."
							EnableTheming="false"  >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblItemProcessType" runat="server" Text='<%#Bind("ItemProcessType") %>' />&nbsp;&nbsp;
									 <asp:Label  ID="lblItemExtProc1" runat="server" Text='<%#Bind("ItemExtraprocessType1") %>' />&nbsp;&nbsp;
									  <asp:Label  ID="lblItemExtProc2" runat="server" Text='<%#Bind("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblBarcode" runat="server" Text='<%#Bind("barcode") %>' style="visibility:hidden"/>
										  <asp:Label  ID="lblStatusID" runat="server" Text='<%#Bind("StatusID") %>' style="visibility:hidden"/>
										<asp:Label  ID="LblISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField>
                                <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />
								<asp:BoundField DataField="ItemRemarks" HeaderText="Description" ReadOnly="True"
									SortExpression="ItemRemarks"></asp:BoundField>
								<asp:BoundField DataField="Colour" HeaderText="Colour" ReadOnly="True"></asp:BoundField>
							 <asp:BoundField DataField="BookingDateTime" HeaderText="Time Stamp" ReadOnly="True"></asp:BoundField>
							<asp:BoundField DataField="UserName" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                          
													   
							</Columns>
						</asp:GridView>  



						 <asp:GridView ID="grdInprocess" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false" 
						 EmptyDataText="No Recotd Found." 	EnableTheming="false" OnRowDataBound="grdInprocess_RowDataBound"  AllowSorting="true"  >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" >
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblInprocItemProcessType" runat="server" Text='<%#Eval("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblInprocItemExtProc1" runat="server" Text='<%#Eval("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblInprocItemExtProc2" runat="server" Text='<%#Eval("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblInprocBarcode" runat="server" Text='<%#Eval("barcode") %>' style="visibility:hidden"/>
										  <asp:Label  ID="LbInproclISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField>  
								 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />
								 <asp:BoundField DataField="BookingDateTime" HeaderText="Time In"  ReadOnly="True"  ></asp:BoundField>   
                                 <asp:BoundField DataField="DynamicDateInproc"  HeaderText="Time Out" ReadOnly="True"   ></asp:BoundField> 
                                  <asp:BoundField DataField="DynamicUserInproc" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                    							
							</Columns>
						</asp:GridView> 
						 <asp:GridView ID="grdWorkshopIn" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false"
						 EmptyDataText="No Recotd Found."  SortExpression="LastDateTime30"	EnableTheming="false" OnRowDataBound="grdWorkshopIn_RowDataBound" >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblInprocItemProcessType" runat="server" Text='<%#Eval("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblInprocItemExtProc1" runat="server" Text='<%#Eval("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblInprocItemExtProc2" runat="server" Text='<%#Eval("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblInprocBarcode" runat="server" Text='<%#Eval("barcode") %>' style="visibility:hidden"/>
										   <asp:Label  ID="LblWShInISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField> 
										 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />									
							 <asp:BoundField DataField="LastDateTime20" HeaderText="Sent from Store"  ReadOnly="True"></asp:BoundField>                          							
							 <asp:BoundField DataField="LastDateTime30"  HeaderText="Received in Workshop" ReadOnly="True"></asp:BoundField>							
							<asp:BoundField DataField="UserStatus30" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                            
															   
							</Columns>
						</asp:GridView>    


						 <asp:GridView ID="grdWorkShopOut" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false"
						 EmptyDataText="No Recotd Found."	EnableTheming="false" OnRowDataBound="grdWorkShopOut_RowDataBound" >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblInprocItemProcessType" runat="server" Text='<%#Eval("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblInprocItemExtProc1" runat="server" Text='<%#Eval("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblInprocItemExtProc2" runat="server" Text='<%#Eval("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblInprocBarcode" runat="server" Text='<%#Eval("barcode") %>' style="visibility:hidden"/>
										   <asp:Label  ID="LblWShOutISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField> 
								 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />											
							 <asp:BoundField DataField="LastDateTime30" HeaderText="Received in Workshop"  ReadOnly="True"></asp:BoundField>                          							
							 <asp:BoundField DataField="LastDateTime2"  HeaderText="Sent to Store" ReadOnly="True"></asp:BoundField>							
							<asp:BoundField DataField="UserStatus2" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                            
															   
							</Columns>
						</asp:GridView>    
				 

				  <asp:GridView ID="grdRecvFromWorkShop" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false"
						 EmptyDataText="No Recotd Found."	EnableTheming="false" OnRowDataBound="grdRecvFromWorkShop_RowDataBound" >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblInprocItemProcessType" runat="server" Text='<%#Eval("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblInprocItemExtProc1" runat="server" Text='<%#Eval("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblInprocItemExtProc2" runat="server" Text='<%#Eval("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblInprocBarcode" runat="server" Text='<%#Eval("barcode") %>' style="visibility:hidden"/>
										   <asp:Label  ID="lblRcfromWSISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField> 
												 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />							
							 <asp:BoundField DataField="LastDateTime2" HeaderText="Sent from Workshop"  ReadOnly="True"></asp:BoundField>
                             <asp:BoundField DataField="DynamicDate"  HeaderText="Received in Store" ReadOnly="True"></asp:BoundField>                          							
                              <asp:BoundField DataField="DynamicUser" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>
							</Columns>
						</asp:GridView>  
						
						<asp:GridView ID="grdMarkForDel" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false"
						 EmptyDataText="No Recotd Found."	EnableTheming="false" OnRowDataBound="grdMarkForDel_RowDataBound" >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblInprocItemProcessType" runat="server" Text='<%#Eval("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblInprocItemExtProc1" runat="server" Text='<%#Eval("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblInprocItemExtProc2" runat="server" Text='<%#Eval("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblInprocBarcode" runat="server" Text='<%#Eval("barcode") %>' style="visibility:hidden"/>
											<asp:Label  ID="lblMarkForDelISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField> 
													 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />						
							 <asp:BoundField DataField="LastDateTime50" HeaderText="Received for Finishing"  ReadOnly="True"></asp:BoundField>                          							
							 <asp:BoundField DataField="LastDateTime3"  HeaderText="Ready On" ReadOnly="True"></asp:BoundField>							
							<asp:BoundField DataField="UserStatus3" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                            
															   
							</Columns>
						</asp:GridView>  
						
						 <asp:GridView ID="grdReady" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false"
						 EmptyDataText="No Recotd Found."	EnableTheming="false" OnRowDataBound="grdReady_RowDataBound" >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblInprocItemProcessType" runat="server" Text='<%#Eval("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblInprocItemExtProc1" runat="server" Text='<%#Eval("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblInprocItemExtProc2" runat="server" Text='<%#Eval("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblInprocBarcode" runat="server" Text='<%#Eval("barcode") %>' style="visibility:hidden"/>
										 <asp:Label  ID="lblReadyISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField> 
														 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />					
							 <asp:BoundField DataField="LastDateTime3" HeaderText="Sent from Workshop"  ReadOnly="True"></asp:BoundField>                          							
							 <asp:BoundField DataField="LastDateTime3"  HeaderText="Ready" ReadOnly="True"></asp:BoundField>							
							<asp:BoundField DataField="UserStatus3" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                            
															   
							</Columns>
						</asp:GridView>  
						

							 
					 
					  <asp:GridView ID="grdDelivery" runat="server" CssClass="mgrid" AutoGenerateColumns="False"  Visible="false" EnableViewState="false" EmptyDataText="No Recotd Found."
							EnableTheming="false" OnRowDataBound="grdDelivery_RowDataBound"  >
							<Columns>
								<asp:BoundField DataField="SubItemName" HeaderText="Garment" ReadOnly="True" SortExpression="SubItemName">
								</asp:BoundField>
								  <asp:TemplateField HeaderText="Services">
									<ItemTemplate >
									<asp:Label  ID="lblDelItemProcessType" runat="server" Text='<%#Bind("ItemProcessType") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									 <asp:Label  ID="lblDelItemExtProc1" runat="server" Text='<%#Bind("ItemExtraprocessType1") %>' />&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Label  ID="lblDelItemExtProc2" runat="server" Text='<%#Bind("ItemExtraprocessType2") %>' />                                                                            
										 <asp:Label  ID="lblDelBarcode" runat="server" Text='<%#Bind("barcode") %>' style="visibility:hidden"/>
										 <asp:Label  ID="lblDelISN" runat="server" Text='<%#Bind("ISN") %>' style="visibility:hidden"/>
									</ItemTemplate>
								</asp:TemplateField>
                                 <asp:BoundField DataField="barcode" HeaderText="Barcode" ReadOnly="True" />
                                 <asp:TemplateField HeaderText="Description">
									<ItemTemplate >
                                    <asp:Label  ID="lblDelDesc" runat="server" Text='<%#Bind("ItemRemarks") %>' /><br />
                                    <asp:Label  ID="lblCauseRet" runat="server" ForeColor="Red" Text='<%#Bind("ReturnCause") %>' />
                                    </ItemTemplate>
                                 </asp:TemplateField>							                    						
									<asp:BoundField DataField="ReadyOn" HeaderText="Ready On" ReadOnly="True"></asp:BoundField>	
									<asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" ReadOnly="True"></asp:BoundField>								
								<asp:BoundField DataField="UserName" HeaderText="Accepted By" ReadOnly="True"></asp:BoundField>                            
															   
							</Columns>
						</asp:GridView> 
				</div>
			</div>
			<div class="col-sm-5 well well-sm">

			 <div class="row-fluid">

			<div class="col-sm-12" style="padding:2px">
		  <div class="panel panel-info well-sm-tiny1">
				 
			<div class="panel-heading">Order Details of <span class="textBold fa-lg" >&nbsp;#&nbsp;<%=_OrderNo %></span> 
             &nbsp;&nbsp;Store&nbsp;&nbsp;<asp:Label ID="lblWorkShopName" runat="server"  clientidmode="Static" CssClass="textBold fa-lg"  EnableTheming="false"></asp:Label>
            </div>
			<div class="panel-body well-sm-tiny">
			<div class="row-fluid well well-sm-tiny">
			<div class="col-sm-3 Textpadding">
			 <span  class="TextGray">Home Delivery</span>     
			</div>
			<div class="col-sm-3 Textpadding">
            	<div class="fa fa-check fa-lg" id="HomeDelYes" visible="false"   style="color:green"  runat="server"  clientidmode="Static"></div> 
			<div class="fa fa-times fa-lg" id="HomeDelNo"  visible="false"    runat="server"  clientidmode="Static"> </div>			
			</div>
			<div class="col-sm-3 Textpadding">
			 <span class="TextGray">Order Date </span>       
			</div>
			<div class="col-sm-3 Textpadding" style="font-size:15px">
			 <asp:Label ID="lblBookDate" runat="server" Text=""></asp:Label>
			</div>
			</div>

			  <div class="row-fluid  well well-sm-tiny">
			<div class="col-sm-3 Textpadding">
			 <span class="TextGray">Due Date</span>      
			</div>
			<div class="col-sm-3 Textpadding ">
			  <asp:Label ID="lblDelDate" runat="server" Text=""></asp:Label>
			</div>          
			<div class="col-sm-3 Textpadding">
			 <span class="TextGray">Delivery Date</span>
			</div>
			<div class="col-sm-3 Textpadding">
			  <asp:Label ID="lblDeliverDate" runat ="server"  clientidmode="Static"  EnableTheming="false"></asp:Label>
			  
			</div>
			</div>
		  
		   <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-3 Textpadding">
			<span class="TextGray" >WorkShop Note </span>
			</div>
			<div class="col-sm-9 Textpadding">
		   <%=_WorkShopNote %>

			</div>
			</div>
			 <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-3 Textpadding">
			<span class="TextGray"  >Order Note </span>
			</div>
			<div class="col-sm-9 Textpadding ">
			 <%=_OrderNote %>

			</div>
			</div>
				  </div>
				</div>

			</div>

			</div>

			<div class="row-fluid">

			 <div class="panel panel-info well-sm-tiny1">
				 
			<div class="panel-heading">Customer Detail</div>
			<div class="panel-body well-sm-tiny">


            <div class="col-sm-8 Textpadding">


			 <div class="row-fluid well well-sm-tiny" >
			<div class="col-sm-1 Textpadding">
			 <i class="fa fa-user fa-lg"></i>
			</div>
			<div class="col-sm-11 Textpadding ">
			 <asp:Label ID="lblCustDetail" runat="server" Text=""></asp:Label>
			</div>
			</div>

			 <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-1 Textpadding">
			<i class="fa fa-building-o fa-lg"></i>
			</div>
			<div class="col-sm-11 Textpadding ">
			  <%=_custAddress %><br /> 
			</div>
			</div>

			<div class="row-fluid well well-sm-tiny" style="height:30px">
			<div class="col-sm-1 Textpadding">
			<i class="fa fa-mobile fa-2x"></i> 
			</div>
			<div class="col-sm-11 Textpadding ">
		  <%=_custMob %><br /> 
			</div>
			</div>
           
			 <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-1 Textpadding ">
		   <i class="fa fa-envelope-o fa-lg"></i>
			</div>
			<div class="col-sm-11 Textpadding ">
			 &nbsp;<%=_custEmail%>
			</div>
			</div>

            </div>

            <div class="col-sm-4 well well-sm well-sm-tiny1" style="min-height:135px">
         
             <div class="textCenter" style="margin-bottom:3px;margin-top:10px">
             <asp:Label ID="lblPendingAmt" runat="server" Text="" CssClass="textBold textRed fa-lg" ClientIDMode="Static"></asp:Label><br />
             <span class="">Pending Amount</span>
           </div>
            <div class="textCenter" style="border-top-style: solid; border-width: 2px;padding-top:5px">
              <asp:Label ID="lblPendingCloth" runat="server" Text="" CssClass="textBold textRed  fa-lg" ClientIDMode="Static"></asp:Label><br />
               <span class="">Pending Cloths</span>
            </div>
            
            </div>


			</div>
			</div>

			</div> 
				  

   <div class="row-fluid">

	<div class="panel panel-info">
				 
			<div class="panel-heading">Payment Details</div>
			<div class="panel-body">

			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			<span class="TextGray">Gross Amount</span>
			</div>
			<div class="col-sm-3 Textpadding">            
			</div>
          
			<div class="col-sm-4 Textpadding ">
				&nbsp;&nbsp;&nbsp;<%=_GrossAmt%>
			</div>
			</div>

			
			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
		  <span  class="textRed">Discount</span>&nbsp;<asp:Label ID="lblDiscountOption" runat ="server"  CssClass="textRed"  clientidmode="Static"  EnableTheming="false"></asp:Label>
			</div>
			<div class="col-sm-2 Textpadding textBold">
			 -
			</div>
			<div class="col-sm-5 Textpadding">
			<%=_Discount%> 
			</div>
			</div>

			
			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			<span  class="TextGray" style="color:Blue">Tax</span><br />
			</div>
			<div class="col-sm-2 Textpadding textBold">
			 +
			</div>
			<div class="col-sm-5 Textpadding">
			<%=_Tax%>
			</div>
			</div>

			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			 <span class="TextGray" >Net Amount</span>
			</div>
			<div class="col-sm-3 Textpadding textBold">
			=
			</div>
			<div class="col-sm-4 Textpadding">
			   <span  class="textBold">&nbsp;&nbsp;&nbsp;<%=_NetAmount%> </span>
			</div>
			</div>

			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			 <span  class="textGreen">Advance</span>
			</div>
			<div class="col-sm-2 Textpadding textBold">
			 -
			</div>
			<div class="col-sm-5 Textpadding">
			 <%=_Advance%>
			</div>
			</div>


			
			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			  <span  class="textGreen" >Paid </span>
			</div>
			<div class="col-sm-2 Textpadding textBold">
			 -
			</div>
			<div class="col-sm-5 Textpadding">
			 <%=_paid%>
			</div>
			</div>

			
			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			<span  class="textRed" >Delivery Discount </span>
			</div>
			<div class="col-sm-2 Textpadding textBold">
			 -
			</div>
			<div class="col-sm-5 Textpadding ">
			<%=_DelDiscount%>
			</div>
			</div>
			
			<div class="row-fluid ">
			<div class="col-sm-5 Textpadding">
			  <span  class="textRed textBold">Balance</span><br />
			</div>
			<div class="col-sm-3 Textpadding textBold ">
			  =
			</div>
			<div class="col-sm-4 Textpadding">
		  <span class="textRed textBold">&nbsp;&nbsp;&nbsp;<%=_Balance%> </span>
		 
			</div>
			</div>
			</div>
			</div>


			 <asp:GridView ID="grdPayment" runat="server" CssClass="mgrid" AutoGenerateColumns="true" 
							EnableTheming="false"  >
			</asp:GridView>
			
			</div>

		 

		  </div>
		  </div>

		</div>
		</div>
		<asp:SqlDataSource ID="SDTBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
				SelectCommand="SELECT [BranchId], [BranchName] FROM [BranchMaster] where IsFactory='False'">
			</asp:SqlDataSource>
		 
 </div>

  <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
 <script type="text/javascript" language="javascript">
     $(document).ready(function () {

         $("#btnSearchInvoice").click(function (e) {
             var CustomerName = $("#txtBarcode").val().trim();
             if (CustomerName == "") {
                 alert("Please Enter Invoice No.");
                 $("#txtBarcode").focus();
                 return false;
             }
         });

         $("#drpBranch").change(function (e) {

             $("#txtBarcode").val("");
             $("#txtBarcode").focus();
             $("#lblMsg").html('');
             $("#divInvoiceDetail").hide();

         });

         $('#achrOrder,#achrInProcess,#achrWorkshopIn,#achrWorkshopOut,#achrMarkForDel,#achrReady,#achrDelivery,#achrRecvFromWorkshop').click(function (e) {
             if (e.clientX == 0 || e.clientY == 0) {
                 return false;
             }
         });

         $('#achrOrder,#achrInProcess,#achrWorkshopIn,#achrWorkshopOut,#achrMarkForDel,#achrReady,#achrDelivery,#achrRecvFromWorkshop').click(function (e) {
             var clickedId = $(this).attr("id");

             if (e.clientX == 0 || e.clientY == 0) {
                 return false;
             }
             if (clickedId == 'achrOrder') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrOrder', null);
             }
             else if (clickedId == 'achrInProcess') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrInProcess', null);
             }
             else if (clickedId == 'achrWorkshopIn') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrWorkshopIn', null);
             }
             else if (clickedId == 'achrWorkshopOut') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrWorkshopOut', null);
             }
             else if (clickedId == 'achrMarkForDel') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrMarkForDel', null);
             }
             else if (clickedId == 'achrReady') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrReady', null);
             }
             else if (clickedId == 'achrDelivery') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrDelivery', null);
             }
             else if (clickedId == 'achrRecvFromWorkshop') {
                 __doPostBack('ctl00$ContentPlaceHolder1$achrRecvFromWorkshop', null);
             }
         });

         var totQty = $('#lblTotalQty').html();
         $('#lblTotalQty').addClass("btn btn-success");

         var totInProcQty;
         try {
             totInProcQty = $('#lblInProcQty').html().trim();
             totInProcQty = totInProcQty.substring(0, 1);
         }
         catch (err) {
             totInProcQty = 0;
         }
         if (totInProcQty != '0') {

             $('#lblInProcQty').addClass("btn btn-danger");

         }
         else {
             $('#lblInProcQty').addClass("btn btn-success");
         }



         var totWorkShopInQty;
         try {
             totWorkShopInQty = $('#lblWorkshopQty').html().trim();
             totWorkShopInQty = totWorkShopInQty.substring(0, 1);
         }
         catch (err) {
             totWorkShopInQty = 0;
         }
         if (totWorkShopInQty != '0') {
             $('#lblWorkshopQty').addClass("btn btn-danger");
         }
         else {

             $('#lblWorkshopQty').addClass("btn btn-success");
         }


         var totWorkShopOutQty;
         try {
             totWorkShopOutQty = $('#lblWorkshopOutQty').html().trim();
             totWorkShopOutQty = totWorkShopOutQty.substring(0, 1);
         }
         catch (err) {
             totWorkShopOutQty = 0;
         }
         if (totWorkShopOutQty != '0') {
             $('#lblWorkshopOutQty').addClass("btn btn-danger");
         }
         else {

             $('#lblWorkshopOutQty').addClass("btn btn-success");
         }

         var totRecvFromWorkShopQty;
         try {

             totRecvFromWorkShopQty = $('#lblRecvFromWorkshopQty').html().trim();
             totRecvFromWorkShopQty = totRecvFromWorkShopQty.substring(0, 1);
         }
         catch (err) {
             totRecvFromWorkShopQty = 0;
         }
         if (totRecvFromWorkShopQty != '0') {
             $('#lblRecvFromWorkshopQty').addClass("btn btn-danger");
         }
         else {

             $('#lblRecvFromWorkshopQty').addClass("btn btn-success");
         }


         var totMarkForDelQty;
         try {
             totMarkForDelQty = $('#lblMarkForDelQty').html().trim();
             totMarkForDelQty = totMarkForDelQty.substring(0, 1);
         }
         catch (err) {
             totMarkForDelQty = 0;
         }
         if (totMarkForDelQty != '0') {
             $('#lblMarkForDelQty').addClass("btn btn-danger");
         }
         else {
             $('#lblMarkForDelQty').addClass("btn btn-success");
         }



         var totReadyQty;
         try {
             totReadyQty = $('#lblReadyQty').html().trim();
             totReadyQty = totReadyQty.substring(0, 1);
         }
         catch (err) {
             totMarkForDelQty = 0;
         }
         if (totReadyQty != '0') {
             $('#lblReadyQty').addClass("btn btn-danger");
         }
         else {
             $('#lblReadyQty').addClass("btn btn-success");

         }

         var totDelQty = $('#hdnDeliveryQty').val().trim();
         if (totQty == totDelQty) {
             $('#lblDeliveryQty').addClass("btn btn-success");
         }
         else {
             $('#lblDeliveryQty').addClass("btn btn-danger");

         }

     });
   
	</script>
</asp:Content>
