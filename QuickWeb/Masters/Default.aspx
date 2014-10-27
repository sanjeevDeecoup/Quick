<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
	Inherits="_Default" Title="Untitled Page" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../js/bootstrap.js"></script>
	<script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="panel panel-primary well-sm-tiny1" style="background-color: #EFEFEF">
		<div class="panel-body">
			<div class="col-md-8">
				<div class="BorderTopLine">
				</div>
				<div class="row-fluid well well-sm-tiny WhiteColor">
					<div class="H1">						
						<asp:Label ID="LblUBook" runat="server" Text="Today Urgent Order Status"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text=" " ForeColor="#999999"></asp:Label>
					</div>
					<asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" DataKeyNames="BNO"
						OnRowDataBound="grdReport_RowDataBound" EmptyDataText="No record found" PageSize="50"
						ShowFooter="True" EnableTheming="false" CssClass="table table-striped table-bordered table-hover">
						<FooterStyle CssClass="pgr" ForeColor="Black" Font-Bold="true" />
						<Columns>
							<asp:TemplateField HeaderText="Order No." FooterText="Total">
								<ItemTemplate>
									<asp:HyperLink ID="hypBtnShowDetails" runat="server" NavigateUrl='<%# String.Format("~/Bookings/Delivery.aspx?BN={0}",Eval("BNO")) %>' Target="_blank"
										Text='<%# Bind("BNO") %>' />
									<asp:HiddenField ID="hdnDueDate" runat="server" Value='<%# Bind("DELDATE") %>' />
								</ItemTemplate>
							</asp:TemplateField>						
							<asp:BoundField DataField="NAME" HeaderText="Customer Details" ItemStyle-HorizontalAlign="Right"
								SortExpression="NAME">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="Qty" HeaderText="Pcs" ItemStyle-HorizontalAlign="Right"
								SortExpression="Quantity">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="ITEM" HeaderText="Garments" ItemStyle-HorizontalAlign="Right"
								SortExpression="ITEM" ItemStyle-Wrap="true" ItemStyle-Width="100Px">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>   
                             <asp:BoundField DataField="NETAMOUNT" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
								SortExpression="NETAMOUNT">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                             <asp:BoundField DataField="PAID" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
								SortExpression="Paid">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                             <asp:BoundField DataField="BALANCE" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
								SortExpression="BALANCE">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>

						</Columns>
					</asp:GridView>
					<asp:Button ID="btnUrgentBookingShow" runat="server" Text="Export to Excel" OnClick="btnUrgentBookingShow_Click"
						EnableTheming="false" CssClass="btn btn-info" />
				</div>
				<div class="BorderTopLine">
				</div>
				<div class="row-fluid well well-sm-tiny WhiteColor">
					<div class="H1">
						
						<asp:Label ID="lblgg" runat="server" Text="Today Delivery Status"></asp:Label>
                        <asp:Label ID="Label6" runat="server" Text=" " ForeColor="#999999"></asp:Label>
					</div>
					<asp:GridView ID="grdDelivery" runat="server" AutoGenerateColumns="False" DataKeyNames="BNO"
						OnRowDataBound="grdDelivery_RowDataBound" ShowFooter="True" EmptyDataText="No record found"
						PageSize="50" EnableTheming="false" CssClass="table table-striped table-bordered table-hover">
						<FooterStyle CssClass="pgr" ForeColor="Black" Font-Bold="true" />
						<Columns>
							<asp:TemplateField HeaderText="Order No." FooterText="Total">
								<ItemTemplate>
									<asp:HyperLink ID="hypBtnShowDetails" runat="server" NavigateUrl='<%# String.Format("~/Bookings/Delivery.aspx?BN={0}",Eval("BNO")) %>' Target="_blank"
										Text='<%# Bind("BNO") %>' />
									<asp:HiddenField ID="hdnDueDate" runat="server" Value='<%# Bind("DELDATE") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<%--<asp:BoundField DataField="BDATE" DataFormatString="{0:d}" FooterText="Total" HeaderText="Order Date"
								SortExpression="BDATE" />
							<asp:BoundField DataField="DELDATE" DataFormatString="{0:d}" HeaderText="Due Date"
								SortExpression="DELDATE" />--%>
							<asp:BoundField DataField="NAME" HeaderText="Customer Details" ItemStyle-HorizontalAlign="Right"
								SortExpression="NAME">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="Qty" HeaderText="Pcs" ItemStyle-HorizontalAlign="Right"
								SortExpression="Quantity">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="ITEM" HeaderText="Garments" ItemStyle-HorizontalAlign="Right"
								SortExpression="ITEM" ItemStyle-Wrap="true" ItemStyle-Width="100Px">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>   
                             <asp:BoundField DataField="NETAMOUNT" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
								SortExpression="NETAMOUNT">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                             <asp:BoundField DataField="PAID" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
								SortExpression="Paid">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                             <asp:BoundField DataField="BALANCE" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
								SortExpression="BALANCE">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>

						</Columns>
					</asp:GridView>
					<asp:Button ID="btnTodayDelivered" runat="server" EnableTheming="false" CssClass="btn btn-info"
						Text="Export to Excel" OnClick="btnTodayDelivered_Click" />
				</div>
				<div class="BorderTopLine">
				</div>
				<div class="row-fluid well well-sm-tiny WhiteColor">
					<div class="H1">
						
						<asp:Label ID="lblHomDel" runat="server" Text="Today Home Delivery Status"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text=" "  ForeColor="#999999"></asp:Label>
					</div>
					<asp:GridView ID="grdHomeDelivery" runat="server" AutoGenerateColumns="False" DataKeyNames="BNO"
						OnRowDataBound="grdHomeDelivery_RowDataBound" EmptyDataText="No record found"
						EnableTheming="false" CssClass="table table-striped table-bordered table-hover"
						PageSize="50" ShowFooter="True">
						<FooterStyle CssClass="pgr" ForeColor="Black" Font-Bold="true" />
						<Columns>
							<asp:TemplateField HeaderText="Order No." FooterText="Total">
								<ItemTemplate>
									<asp:HyperLink ID="hypBtnShowDetails" runat="server" NavigateUrl='<%# String.Format("~/Bookings/Delivery.aspx?BN={0}",Eval("BNO")) %>' Target="_blank"
										Text='<%# Bind("BNO") %>' />
									<asp:HiddenField ID="hdnDueDate" runat="server" Value='<%# Bind("DELDATE") %>' />
								</ItemTemplate>
							</asp:TemplateField>						
							<asp:BoundField DataField="NAME" HeaderText="Customer Details" ItemStyle-HorizontalAlign="Right"
								SortExpression="NAME">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="Qty" HeaderText="Pcs" ItemStyle-HorizontalAlign="Right"
								SortExpression="Quantity">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="ITEM" HeaderText="Garments" ItemStyle-HorizontalAlign="Right"
								SortExpression="ITEM" ItemStyle-Wrap="true" ItemStyle-Width="100Px">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>   
                             <asp:BoundField DataField="NETAMOUNT" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
								SortExpression="NETAMOUNT">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                             <asp:BoundField DataField="PAID" HeaderText="Paid" ItemStyle-HorizontalAlign="Right"
								SortExpression="Paid">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                             <asp:BoundField DataField="BALANCE" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
								SortExpression="BALANCE">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>

						</Columns>
						<HeaderStyle BackColor="Maroon" ForeColor="Black" />
						<AlternatingRowStyle BackColor="#CCFFFF" />
					</asp:GridView>
					<asp:Button ID="btnHomeDelivery" runat="server" Text="Export to Excel" EnableTheming="false"
						CssClass="btn btn-info" OnClick="btnHomeDelivery_Click" />
				</div>
				<div class="BorderTopLine">
				</div>
				<div class="row-fluid well well-sm-tiny WhiteColor">
					<div class="H1">
						<asp:Label ID="Label3" runat="server" Text=" " ForeColor="#FF9933"></asp:Label>
						<asp:Label ID="lblCustBirt" runat="server" Text="Upcoming Customer's Birthday"></asp:Label>
					</div>
					<asp:GridView ID="grdCustomerBirthDay" runat="server" AutoGenerateColumns="false"
						EnableTheming="false" CssClass="table table-striped table-bordered table-hover">
						<Columns>
							<asp:BoundField DataField="BirthDate" DataFormatString="{0:d}" HeaderText="Birth Date"
								ItemStyle-HorizontalAlign="Right" SortExpression="BirthDate">
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="CustomerName" HeaderText="Name" ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerName">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="CustomerAddress" HeaderText="Address" ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerAddress">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<%--<asp:BoundField DataField="CustomerPhone" HeaderText="Phone No." ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerPhone">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>--%>
							<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No." ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerMobile">
								<FooterStyle HorizontalAlign="Right" ForeColor="Black" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="CustomerEmailId" HeaderText="EMail ID" ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerEmailId">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
						</Columns>
					</asp:GridView>
					<asp:Button ID="btnCustomerBirthday" runat="server" Text="Export to Excel" OnClick="btnCustomerBirthday_Click"
						EnableTheming="false" CssClass="btn btn-info" />
				</div>
				<div class="BorderTopLine">
				</div>
				<div class="row-fluid well well-sm-tiny WhiteColor">
					<div class="H1">
						<asp:Label ID="Label4" runat="server" Text=" " ForeColor="#FF9933"></asp:Label>
						<asp:Label ID="lblCustAni" runat="server" Text="Upcoming Customer's Anniversary"></asp:Label>
					</div>
					<asp:GridView ID="grdCustomerAnniversary" runat="server" AutoGenerateColumns="false"
						EnableTheming="false" CssClass="table table-striped table-bordered table-hover">
						<Columns>
							<asp:BoundField DataField="AnniversaryDate" DataFormatString="{0:d}" HeaderText="Anniversary Date"
								ItemStyle-HorizontalAlign="Right" SortExpression="AnniversaryDate">
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="CustomerName" HeaderText="Name" ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerName">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<asp:BoundField DataField="CustomerAddress" HeaderText="Address" ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerAddress">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
							<%--<asp:BoundField DataField="CustomerPhone" HeaderText="Phone No." ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerPhone">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>--%>
							<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No." ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerMobile">
								<FooterStyle HorizontalAlign="Right" ForeColor="Black" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
                            <asp:BoundField DataField="CustomerEmailId" HeaderText="EMail ID" ItemStyle-HorizontalAlign="Right"
								SortExpression="CustomerEmailId">
								<FooterStyle HorizontalAlign="Right" />
								<ItemStyle HorizontalAlign="Right" />
							</asp:BoundField>
						</Columns>
					</asp:GridView>
					<asp:Button ID="btnAnniversary" runat="server" Text="Export to Excel" OnClick="btnAnniversary_Click"
						EnableTheming="false" CssClass="btn btn-info" />
				</div>
				<div class="BorderTopLine">
				</div>
			</div>
			<div class="col-md-4 nopadding">
				<div class="row-fluid">
					<div id="divWSWorkload" class="well well-sm-tiny" style="background-color: White;
						display: none">
						<p class="text-center no-bottom-margin">
							<span class="textBold">Workshop Workload</span></p>
						<div class="col-sm-12 well well-sm-tiny">
							<div class="row-fluid" id="divWorkshop">
								<div class="col-sm-3">
								</div>
								<div class="form-inline">
									<div class="form-group">
										<a href="" id="achrWorkshopDec"><i class="fa fa-minus-circle icon-white fa-2x"></i>
										</a>
									</div>
									<div class="form-group">
										<div class="col-sm-8 Textpadding">
											<input type="text" id="txtWorkshoploadDate" class="form-control" />
										</div>
										<div class=" ">
											<a href="" id="achrWorkshopInc">&nbsp;<i class="fa fa-plus-circle icon-white fa-2x margintop5">
											</i></a>
										</div>
									</div>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Total
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblTotalPcs"></span>
								</div>
							</div>
							<div id="divProcessAndQtyDtl">
							</div>
						</div>
					</div>
					<div id="columnchart_values" style="width: 390px; height: 250px; display: none">
					</div>
				</div>
				<div class="row-fluid well well-sm-tiny" style="background-color: White; height: 260px">
					<p class="text-center no-bottom-margin">
						<span class="textBold">Ready and Delivery Status</span></p>
					<div class="col-sm-12 well well-sm-tiny">
						<div class="row-fluid">
							<div class="col-sm-3">
							</div>
							<div class="form-inline">
								<div class="form-group">
									<a href="" id="DateDec"><i class="fa fa-minus-circle icon-white fa-2x"></i></a>
								</div>
								<div class="form-group">
									<div class="col-sm-8 Textpadding">
										<input type="text" id="txtRDDate" class="form-control" />
									</div>
									<div class=" ">
										<a href="" id="DateInc">&nbsp;<i class="fa fa-plus-circle icon-white fa-2x margintop5">
										</i></a>
									</div>
								</div>
							</div>
						</div>
						<div class="row-fluid" id="divStageData">
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Total
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblTotal"></span>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Workshop In
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblWIP"></span>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Ready
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblReady"></span>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Delivered
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lbldelivered"></span>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Ready but Pending
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblReadyPen"></span>
								</div>
							</div>
						</div>
					</div>
					<div id="columnchartData" style="width: 380px; height: 100px; margin-left: -5px;">
					</div>
				</div>
				<div class="row-fluid well well-sm-tiny" style="background-color: White">
					<p class="text-center no-bottom-margin">
						<span class="textBold">Pending for Pressing</span></p>
					<div class="col-sm-12 well well-sm-tiny">
						<div class="row-fluid">
							<div class="col-sm-3">
							</div>
							<div class="form-inline">
								<div class="form-group">
									<a href="" id="achrdateDec"><i class="fa fa-minus-circle icon-white fa-2x"></i></a>
								</div>
								<div class="form-group">
									<div class="col-sm-8 Textpadding">
										<input type="text" id="txtPendingDate" class="form-control" />
									</div>
									<div class=" ">
										<a href="" id="achrdateInc">&nbsp;<i class="fa fa-plus-circle icon-white fa-2x margintop5">
										</i></a>
									</div>
								</div>
							</div>
						</div>
						<div class="row-fluid">
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Over Due
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblOverDue"></span>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Today's Due
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblTodayDue"></span>
								</div>
							</div>
							<div class="row-fluid well well-sm-tiny">
								<div class="col-sm-7 Textpadding textBold">
									Future Due
								</div>
								<div class="col-sm-5 Textpadding ">
									<span id="lblFutureDue"></span>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="chart_div">
	</div>
	<div id="chart_div1">
	</div>
	<asp:HiddenField ID="hdnDate" runat="server" />
	<asp:HiddenField ID="hdnTotal" runat="server" />
	<asp:HiddenField ID="hdnQuantity" runat="server" />
	<input type="hidden" value="" id="hdnIsCloud" />
	<script type="text/javascript">
		$(document).ready(function () {
			$.ajax({
				url: '../AutoComplete.asmx/GetCurrentDate',
				type: 'GET',
				data: " ",
				timeout: 20000,
				contentType: 'application/json; charset=UTF-8',
				datatype: 'JSON',
				cache: true,
				async: false,
				success: function (response) {
					var _val = response.d;
					var aryDate = _val.split(':');
					$('#txtRDDate').val(aryDate[0]);
					$('#txtPendingDate').val(aryDate[0]);
					$('#txtWorkshoploadDate').val(aryDate[0]);
					$('#hdnIsCloud').val(aryDate[1]);
				},
				error: function (response) {
					alert(response.toString())
				}
			});
		
			GarmentStatusDetail();
			GetPressingdetail();
			var strCloud2 = $('#hdnIsCloud').val();
			if (strCloud2 == "False") {
				GetWorkshopWorkloadDetails();
			}
			$('#DateDec').click(function (eventParam) {
				var date = new Date($('#txtRDDate').val());
				var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);
				var dt = '';
				dt = SetDate(dd);
				$('#txtRDDate').val(dt);
				if (strCloud2 == "False") {
					GarmentStatusDetail();
				} else {
					GetAllStatusData();
					drawChartForReadyAndDelivery();
				}
				eventParam.preventDefault();
			});
			$('#DateInc').click(function (eventParam) {
				var date = new Date($('#txtRDDate').val());
				var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
				var dt = '';
				dt = SetDate(dd);
				$('#txtRDDate').val(dt);
				if (strCloud2 == "False") {
					GarmentStatusDetail();
				} else {
					GetAllStatusData();
					drawChartForReadyAndDelivery();
				}
				eventParam.preventDefault();
			});
			var dpFrom = $('#txtRDDate,#txtPendingDate,#txtWorkshoploadDate');
			dpFrom.datepicker({
				changeMonth: true,
				changeYear: true,
				format: "dd M yyyy",
				language: "tr"
			}).on('changeDate', function (ev) {
				$(this).blur();
				$(this).datepicker('hide');
			});
			$("#txtRDDate").change(function () {
				if (strCloud2 == "False") {
					GarmentStatusDetail();
				} else {
					GetAllStatusData();
					drawChartForReadyAndDelivery();
				}
			});
			function GarmentStatusDetail() {
				$("#divStageData").css("display", "block");
				$("#columnchartData").css("display", "none");
				$.ajax({
					url: '../AutoComplete.asmx/GetGarmentStatus',
					type: 'GET',
					data: "date='" + $('#txtRDDate').val() + "' ",
					timeout: 20000,
					contentType: 'application/json; charset=UTF-8',
					datatype: 'JSON',
					cache: true,
					async: false,
					success: function (response) {
						var result = response.d;
						var resultAry = result.split(':');
						$('#lblTotal').text(resultAry[0]);
						$('#lblWIP').text(resultAry[1]);
						$('#lblReady').text(resultAry[2]);
						$('#lbldelivered').text(resultAry[3]);
						$('#lblReadyPen').text(resultAry[4]);
						$('#lblUserName').text(resultAry[5]);
					},
					error: function (response) {
						alert(response.toString())
					}
				});
			}

			$('#achrdateDec').click(function (eventParam) {
				var date = new Date($('#txtPendingDate').val());
				var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);
				var dt = '';
				dt = SetDate(dd);
				$('#txtPendingDate').val(dt);
				GetPressingdetail();
				eventParam.preventDefault();
			});

			$('#achrdateInc').click(function (eventParam) {
				var date = new Date($('#txtPendingDate').val());
				var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
				var dt = '';
				dt = SetDate(dd);
				$('#txtPendingDate').val(dt);
				GetPressingdetail();
				eventParam.preventDefault();
			});
			$("#txtPendingDate").change(function () {
				GetPressingdetail();
			});

			function SetDate(dd) {
				var dt = '';
				dt = dd.getDate().toString().length == 2 ? dd.getDate().toString() : '0' + dd.getDate().toString();
				dt += ' ' + Array.prototype.map.call([dd.getMonth()], function (val) {
					switch (val) {
						case 0: return 'Jan'; case 1: return 'Feb'; case 2: return 'Mar'; case 3: return 'Apr'; case 4: return 'May'; case 5: return 'Jun'; case 6: return 'Jul'; case 7: return 'Aug'; case 8: return 'Sep'; case 9: return 'Oct'; case 10: return 'Nov'; case 11: return 'Dec';
					}
				})[0];
				dt += ' ' + dd.getFullYear();
				return dt;

			}

			function GetPressingdetail() {
				$.ajax({
					url: '../AutoComplete.asmx/GetPendingStatus',
					type: 'GET',
					data: "date='" + $('#txtPendingDate').val() + "' ",
					timeout: 20000,
					contentType: 'application/json; charset=UTF-8',
					datatype: 'JSON',
					cache: true,
					async: false,
					success: function (response) {
						var result = response.d;
						var AryPending = result.split(':');
						$('#lblOverDue').text(AryPending[0]);
						$('#lblTodayDue').text(AryPending[1]);
						$('#lblFutureDue').text(AryPending[2]);
					},
					error: function (response) {
						alert(response.toString())
					}
				});
			}


			$('#achrWorkshopInc').click(function (eventParam) {
				var date = new Date($('#txtWorkshoploadDate').val());
				var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
				var dt = '';
				dt = SetDate(dd);
				$('#txtWorkshoploadDate').val(dt);
				GetWorkshopWorkloadDetails();
				eventParam.preventDefault();
			});

			$('#achrWorkshopDec').click(function (eventParam) {
				var date = new Date($('#txtWorkshoploadDate').val());
				var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);
				var dt = '';
				dt = SetDate(dd);
				$('#txtWorkshoploadDate').val(dt);
				GetWorkshopWorkloadDetails();
				eventParam.preventDefault();
			});
			$("#txtWorkshoploadDate").change(function () {
				GetWorkshopWorkloadDetails();
			});
			function GetWorkshopWorkloadDetails() {
				$("#divWSWorkload").css("display", "block");
				$("#columnchart_values").css("display", "none");
				$.ajax({
					url: '../AutoComplete.asmx/GetWorkshopWorkloadData',
					type: 'GET',
					data: "date='" + $('#txtWorkshoploadDate').val() + "' ",
					timeout: 20000,
					contentType: 'application/json; charset=UTF-8',
					datatype: 'JSON',
					cache: true,
					async: false,
					success: function (response) {
						var result = response.d;
						var AryWorkShop = result.split(':');
						if (AryWorkShop[1] != "") {
							$('#lblTotalPcs').text(AryWorkShop[1]);
						} else {
							$('#lblTotalPcs').text("0");
						}
						var strProcessAndQty = AryWorkShop[0];
						if (strProcessAndQty != "") {
							var aryProcessAndQty = strProcessAndQty.split('@');
							var strProcess, strQty, _htmldata = "";
							for (var j = 0; j < aryProcessAndQty.length; j += 1) {
								var strTempdata = aryProcessAndQty[j];
								var arytempData = strTempdata.split(',');
								strProcess = arytempData[0];
								strQty = arytempData[1];
								_htmldata = _htmldata + '<div class="row-fluid well well-sm-tiny"><div class="col-sm-7 Textpadding textBold"> ' + strProcess + '</div><div class="col-sm-5 Textpadding "><span>' + strQty + '</span></div></div>';
							}
							$('#divProcessAndQtyDtl').html(_htmldata);
						}
						else {
							$('#divProcessAndQtyDtl').html("");
						}
					},
					error: function (response) {
						alert(response.toString())
					}
				});
			}
		});
	</script>
	<script type="text/javascript">   
	 $(document).ready(function () {
		var strCloud = $('#hdnIsCloud').val();	
		  var strDueDate, strProcess,StrFromAndToDate,strAllStatus ,strProcess1= "";  
		  if( strCloud == "True")
		  {
		  $.ajax({
					url: '//www.google.com/jsapi',
					dataType: 'script',
					cache: true,
					success: function () { 
					google.load('visualization', '1', {'packages': ['corechart'],'callback': drawChart});
					google.load('visualization', '1', {'packages': ['corechart'],'callback': drawChartForReadyAndDelivery });
					}
				});
		  }
		   if(strCloud=="True")
		   {        
			$("#columnchart_values").css("display", "block");
			$("#divWSWorkload").css("display", "none");     
			 $.ajax({
				url: '../AutoComplete.asmx/GetDetailForGraph',
				type: 'GET',
				data: " ",
				timeout: 20000,
				contentType: 'application/json; charset=UTF-8',
				datatype: 'JSON',
				cache: true,
				async: false,
				success: function (response) {
					var ResultData = response.d;                    
					var Arydata = ResultData.split(':');
					strDueDate = Arydata[0];
					strProcess = Arydata[1]; 
					StrFromAndToDate =Arydata[2];
					aryDuedate = strDueDate.split('@');
					aryProcess = strProcess.split(',');    
					aryFromAndToDate =StrFromAndToDate.split('@');                 
				},
				error: function (response) {
					alert(response.toString())
				}
			});      
			
			GetAllStatusData();     
			}
			 }); 
   
	 function GetAllStatusData()
	 {   
	   $("#divStageData").css("display", "none");
	   $("#columnchartData").css("display", "block");
	  $.ajax({
				url: '../AutoComplete.asmx/GetDataForAllStatus',
				type: 'GET',
				 data: "date='" + $('#txtRDDate').val() + "' ",
				timeout: 20000,
				contentType: 'application/json; charset=UTF-8',
				datatype: 'JSON',
				cache: true,
				async: false,
				success: function (response) {
					var ResultData1 = response.d;                    
					var Arydata1 = ResultData1.split(':');
					strAllStatus = Arydata1[0];
					strProcess1 = Arydata1[1];                  
					aryAllStatus = strAllStatus.split('@');
					aryProcess1 = strProcess1.split(','); 
				},
				error: function (response) {
					alert(response.toString())
				}
			});
	 }

//       google.load("visualization", "1", {packages:["corechart"]});
//       google.setOnLoadCallback(drawChart); 
//       google.setOnLoadCallback(drawChartForReadyAndDelivery); 
	  
	   function drawChart() {
	   var data = new google.visualization.DataTable();
		 data.addColumn('string', 'Year');
		  for (var i = 0 ; i < aryProcess.length; i += 1) {
		  data.addColumn('number',aryProcess[i]);
		  }
		var columCounter =0;
		for (var j = 0 ; j < aryDuedate.length; j += 1) {
		var aryDueAndQty =[];
		var count = 0;
		var strDueAndQty = aryDuedate[j];
	   aryDueAndQty = strDueAndQty.split(',');
	   data.addRows(1);
	   for (var i = 0 ; i < aryDueAndQty.length; i += 1) {        
		if(i==0)
		{
		  data.setValue(columCounter, count, aryDueAndQty[i]);
		  count = count+1;
		}else{        
		var number = parseInt(aryDueAndQty[i]);
		 data.setValue(columCounter,count ,number);
		 count = count+1;
		}
	   }
	   columCounter=columCounter+1;
	} 
	 var  strTitle ='';
	 var aryLength= aryFromAndToDate.length;
	 if(aryLength==1)
	 {     
	 strTitle= 'Workshop Workload';
	 }
	 else{
	  strTitle= 'Workshop Workload  ('+aryFromAndToDate[0]+' - '+aryFromAndToDate[1] +')';
	 }

	  var options = {
	  chartArea:{left:50,width:"80%"},
		title:strTitle,
		titleTextStyle: { 
		fontSize: 14        
	},
		width: 390,
		height: 250,
		 vAxis: {title: "Qty"},
		 hAxis: {title: "Due Date"},
		legend: { position: 'top', maxLines: 3 },
	 bar: { groupWidth: '40%' },
		isStacked: true,
	  };
	  var chart = new google.visualization.ColumnChart(document.getElementById("columnchart_values"));
	  chart.draw(data, options);
  }
   function drawChartForReadyAndDelivery() {
	   var dataAll = new google.visualization.DataTable();
		 dataAll.addColumn('string', 'Year');
		  for (var i = 0 ; i < aryProcess1.length; i += 1) {
		  dataAll.addColumn('number',aryProcess1[i]);
		  }
		var columCounter1 =0;
		for (var j = 0 ; j < aryAllStatus.length; j += 1) {
		var aryStatusAndQty =[];
		var counter = 0;
		var strAllStatusTmp = aryAllStatus[j];
	   aryStatusAndQty = strAllStatusTmp.split(',');
	   dataAll.addRows(1);
	   for (var i = 0 ; i < aryStatusAndQty.length; i += 1) {        
		if(i==0)
		{
		  dataAll.setValue(columCounter1, counter, aryStatusAndQty[i]);
		  counter = counter+1;
		}else{        
		var number1 = aryStatusAndQty[i];
		 dataAll.setValue(columCounter1,counter ,number1);
		 counter = counter+1;
		}
	   }
	   columCounter1=columCounter1+1;
	}   

	  var options1 = {
	  chartArea:{left:60,width:"76%"},                
		width: 380,
		height: 175,
		 //vAxis: {title: "Status"},
		 hAxis: {title: "Qty"},
		legend: { position: 'top', maxLines: 3 },
	 bar: { groupWidth: '60%' },
		isStacked: true,
	  };
	  var chart = new google.visualization.BarChart(document.getElementById("columnchartData"));
	  chart.draw(dataAll, options1);
  } 
	</script>
</asp:Content>
