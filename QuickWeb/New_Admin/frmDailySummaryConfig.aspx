<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true" CodeBehind="frmDailySummaryConfig.aspx.cs" Inherits="QuickWeb.New_Admin.frmDailySummaryConfig" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Duration" Src="~/Controls/Bootstrap_DurationControlDateWise.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
	
	<script type="text/javascript" language="javascript">
	    $(document).ready(function (event) {

	        var dpFrom = $('#ctl00_ContentPlaceHolder1_uc7_txtReportFrom,#ctl00_ContentPlaceHolder1_uc1_txtReportFrom,#ctl00_ContentPlaceHolder1_uc2_txtReportFrom,#ctl00_ContentPlaceHolder1_uc3_txtReportFrom,#ctl00_ContentPlaceHolder1_uc4_txtReportFrom,#ctl00_ContentPlaceHolder1_uc5_txtReportFrom,#ctl00_ContentPlaceHolder1_uc6_txtReportFrom');
	              dpFrom.datepicker({
	                changeMonth: true,
	                changeYear: true,
	                format: "dd M yyyy",
	                language: "tr"
	            }).on('changeDate', function (ev) {
	                $(this).blur();
	                $(this).datepicker('hide');
	            });

	            var dpTo = $('#ctl00_ContentPlaceHolder1_uc7_txtReportUpto,#ctl00_ContentPlaceHolder1_uc1_txtReportUpto,#ctl00_ContentPlaceHolder1_uc2_txtReportUpto,#ctl00_ContentPlaceHolder1_uc3_txtReportUpto,#ctl00_ContentPlaceHolder1_uc4_txtReportUpto,#ctl00_ContentPlaceHolder1_uc5_txtReportUpto,#ctl00_ContentPlaceHolder1_uc6_txtReportUpto');
	        	        dpTo.datepicker({
	        	            changeMonth: true,
	        	            changeYear: true,
	        	            format: "dd M yyyy",
	        	            language: "tr"
	        	        }).on('changeDate', function (ev) {
	        	            $(this).blur();
	        	            $(this).datepicker('hide');
	        	        });                     
	       
	        $('#ctl00_ContentPlaceHolder1_uc7_radReportFrom').click(function (e) {
	            if ($(this).is(':checked')) {
	                $('#ctl00_ContentPlaceHolder1_uc1_radReportFrom').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc2_radReportFrom').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc3_radReportFrom').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc4_radReportFrom').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc5_radReportFrom').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc6_radReportFrom').prop('checked', true);
	            }
	        });

	        $('#ctl00_ContentPlaceHolder1_uc7_radReportMonthly').click(function (e) {
	            if ($(this).is(':checked')) {
	                $('#ctl00_ContentPlaceHolder1_uc1_radReportMonthly').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc2_radReportMonthly').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc3_radReportMonthly').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc4_radReportMonthly').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc5_radReportMonthly').prop('checked', true);
	                $('#ctl00_ContentPlaceHolder1_uc6_radReportMonthly').prop('checked', true);
	            }
	        });

	        $('#chkBookingReport').click(function (e) {
	            if ($('#chkBookingReport').is(':checked')) {
	                $('#booking').show();
	                $('#booking').style["display"] = "block"
	            }
	            else {
	                $('#booking').hide();
	                $('#booking').style["display"] = "none"
	            }
	        });

	        $('#chkSalesReport').click(function (e) {
	            if ($('#chkSalesReport').is(':checked')) {
	                $('#Sales').show();
	                $('#Sales').style["display"] = "block"
	            }
	            else {
	                $('#Sales').hide();
	                $('#Sales').style["display"] = "none"
	            }
	        });

	        $('#chkDeliveryReport').click(function (e) {
	            if ($('#chkDeliveryReport').is(':checked')) {
	                $('#Delivery').show();
	                $('#Delivery').style["display"] = "block"
	            }
	            else {
	                $('#Delivery').hide();
	                $('#Delivery').style["display"] = "none"
	            }
	        });

	        $('#chkPaymentTypeReport').click(function (e) {
	            if ($('#chkPaymentTypeReport').is(':checked')) {
	                $('#Payment').show();
	                $('#Payment').style["display"] = "block"
	            }
	            else {
	                $('#Payment').hide();
	                $('#Payment').style["display"] = "none"
	            }
	        });

	        $('#chkDailyCustAdd').click(function (e) {
	            if ($('#chkDailyCustAdd').is(':checked')) {
	                $('#DailyCustomer').show();
	                $('#DailyCustomer').style["display"] = "block"
	            }
	            else {
	                $('#DailyCustomer').hide();
	                $('#DailyCustomer').style["display"] = "none"
	            }
	        });

	        $('#chkDetailCashbook').click(function (e) {
	            if ($('#chkDetailCashbook').is(':checked')) {
	                $('#DetailCashbook').show();
	                $('#DetailCashbook').style["display"] = "block"
	            }
	            else {
	                $('#DetailCashbook').hide();
	                $('#DetailCashbook').style["display"] = "none"
	            }
	        });

	        $("#ctl00_ContentPlaceHolder1_uc7_txtReportFrom").change(function () {
	            var contents = $("#ctl00_ContentPlaceHolder1_uc7_txtReportFrom").val();
	            $("#ctl00_ContentPlaceHolder1_uc1_txtReportFrom").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc2_txtReportFrom").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc3_txtReportFrom").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc4_txtReportFrom").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc5_txtReportFrom").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc6_txtReportFrom").val(contents);
	        });

	        $("#ctl00_ContentPlaceHolder1_uc7_txtReportUpto").change(function () {
	            var contents = $("#ctl00_ContentPlaceHolder1_uc7_txtReportUpto").val();
	            $("#ctl00_ContentPlaceHolder1_uc1_txtReportUpto").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc2_txtReportUpto").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc3_txtReportUpto").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc4_txtReportUpto").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc5_txtReportUpto").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc6_txtReportUpto").val(contents);
	        });

	        $("#ctl00_ContentPlaceHolder1_uc7_drpMonthList").change(function () {
	            var contents = $("#ctl00_ContentPlaceHolder1_uc7_drpMonthList").val();
	            $("#ctl00_ContentPlaceHolder1_uc1_drpMonthList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc2_drpMonthList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc3_drpMonthList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc4_drpMonthList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc5_drpMonthList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc6_drpMonthList").val(contents);
	        });

	        $("#ctl00_ContentPlaceHolder1_uc7_drpYearList").change(function () {
	            var contents = $("#ctl00_ContentPlaceHolder1_uc7_drpYearList").val();
	            $("#ctl00_ContentPlaceHolder1_uc1_drpYearList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc2_drpYearList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc3_drpYearList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc4_drpYearList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc5_drpYearList").val(contents);
	            $("#ctl00_ContentPlaceHolder1_uc6_drpYearList").val(contents);
	        });

	        $('#btnSendStatus').click(function (e) {
	            if (($('#chkBookingReport').is(':checked') == false) && ($('#chkSalesReport').is(':checked') == false) && ($('#chkDeliveryReport').is(':checked') == false) && ($('#chkPaymentTypeReport').is(':checked') == false) && ($('#chkDailyCustAdd').is(':checked') == false) && ($('#chkDetailCashbook').is(':checked') == false)) {
	                alert('Please select atleast one report!');
	                return false;
	            }
	        });
	    });

		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title">Daily Summary</h3>
		</div>
		<div class="panel-body">	
		 <div class="row-fluid" style="text-align:center"> 		 			  
			 <h3 > <asp:Label ID="lblsummaryErr" runat="server" EnableTheming="False"  class="label label-warning" ClientIDMode="Static" EnableViewState="False" /></h3>
			  <h3><asp:Label ID="lblsummarySucess" runat="server" EnableTheming="false" ClientIDMode="Static" CssClass="label label-success" EnableViewState="False"></asp:Label></h3>
			  <asp:Label ID="lblBranchMailId" runat="server" ClientIDMode="Static" EnableTheming="false"  Style="visibility: hidden"></asp:Label>			
		  </div> 
	<div class="row-fluid breadcrumb"> 	
			<div class="col-md-3"> 
				<span class="TDCaption1">Select Date Range</span> 
				</div>				 
				<div class="col-md-9">	
				<uc:Duration ID="uc7"  runat="server"></uc:Duration>  				
			</div>			
	 </div> 
<div class="row-fluid">   
		 <div class="panel panel-info" >
					<div class="panel-heading">
					  <h4 class="panel-title">Select Individual Reports</h4>
					</div>
			   <div class="panel-body">				
			
				   <div class="row-fluid well well-sm-tiny well-sm-height" >
					  <div class="col-md-2 "> <span class="TDCaption1">Booking </span> </div>					 
					  <div class="col-md-1 " style="text-align:right"> 
						  <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static" checked="" runat="server" id="chkBookingReport" />
						   <span class="lbl"></span>                     
					   </div>					 
					 <div class="col-md-9"  id="booking"  >	
					
					   <uc:Duration ID="uc1"  runat="server" ></uc:Duration>  				
					  
					</div>	   
				  </div>	
				
				   <div class="row-fluid well well-sm-tiny well-sm-height" >
					  <div class="col-md-2"> <span class="TDCaption1">Sales  </span> </div>					 
					  <div class="col-md-1 " style="text-align:right"> 
						  <input type="checkbox" class="ace ace-switch ace-switch-5" checked="" clientidmode="Static" runat="server" id="chkSalesReport" />
						   <span class="lbl"></span>                     
					   </div>					 
					 <div class="col-md-9" id="Sales"  >	
					   <uc:Duration ID="uc2"  runat="server" ></uc:Duration>  				
					</div>	   
				  </div>

					 <div class="row-fluid well well-sm-tiny well-sm-height" >
					  <div class="col-md-2 "> <span class="TDCaption1">Delivery </span> </div>					 
					  <div class="col-md-1 " style="text-align:right"> 
						  <input type="checkbox" class="ace ace-switch ace-switch-5" checked="" clientidmode="Static" runat="server" id="chkDeliveryReport" />
						   <span class="lbl"></span>                     
					   </div>					 
					 <div class="col-md-9"  id="Delivery" >	
					   <uc:Duration ID="uc3" runat="server" ></uc:Duration>  				
					</div>	   
				  </div>
					 <div class="row-fluid well well-sm-tiny well-sm-height" >
					  <div class="col-md-2 "> <span class="TDCaption1">Payment Type </span> </div>					 
					  <div class="col-md-1 " style="text-align:right"> 
						  <input type="checkbox" class="ace ace-switch ace-switch-5" checked="" clientidmode="Static" runat="server" id="chkPaymentTypeReport" />
						   <span class="lbl"></span>                     
					   </div>					 
					 <div class="col-md-9" id="Payment" >	
					   <uc:Duration ID="uc4" runat="server" ></uc:Duration>  				
					</div>	   
				  </div>


					 <div class="row-fluid well well-sm-tiny well-sm-height" >
					  <div class="col-md-2"> <span class="TDCaption1">Daily Customer Addition </span> </div>					 
					  <div class="col-md-1 " style="text-align:right"> 
						  <input type="checkbox" class="ace ace-switch ace-switch-5" checked=""  clientidmode="Static" runat="server" id="chkDailyCustAdd" />
						   <span class="lbl"></span>                     
					   </div>					 
					 <div class="col-md-9" id="DailyCustomer" >	
					   <uc:Duration ID="uc5" runat="server" ></uc:Duration>  				
					</div>	   
				  </div>
					 <div class="row-fluid well well-sm-tiny well-sm-height" >
					  <div class="col-md-2"> <span class="TDCaption1">Details CashBook </span> </div>					 
					  <div class="col-md-1 " style="text-align:right"> 
						  <input type="checkbox" class="ace ace-switch ace-switch-5" checked=""  clientidmode="Static"  runat="server" id="chkDetailCashbook" />
						   <span class="lbl"></span>                     
					   </div>					 
					 <div class="col-md-9" id="DetailCashbook"  >	
					   <uc:Duration ID="uc6" runat="server" ></uc:Duration>  				
					</div>	   
				  </div> 
			 </div>
			 </div> 
 </div>  
 <div class="row-fluid" >
			<div class="col-md-4">
			</div>			
			<div class="col-md-3">
				<asp:Button ID="btnSendStatus" runat="server" Text="Send Status" CausesValidation="False"  OnClick="btnSendStatus_Click"
				  ClientIDMode="Static" EnableTheming="False" CssClass="btn btn-info btn-lg btn-block"   />
				   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
					InteractiveDeviceInfos="(Collection)" Visible="False" WaitMessageFont-Names="Verdana"
						WaitMessageFont-Size="14pt"></rsweb:ReportViewer>		
			</div>
			</div>	 
  </div>
  </div>
	
<!-- Start of js -->   
<script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
<script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
<script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
<script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>
</asp:Content>
