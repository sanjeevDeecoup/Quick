<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
	MasterPageFile="~/WebsiteUser/WebsiteUserMain.Master" Inherits="QuickWeb.Website_User.ChangePassword" %>
		<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="../css/template.css" rel="stylesheet" type="text/css" />
	<link href="../css/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
	<link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />    
	<link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    	
	<link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
	<link href="../css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
	<script src="../JavaScript/jquery-1.6.2.min.js" type="text/javascript"></script> 
	<script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
	 <script src="../javascript/jquery.validationEngine-en.js" type="text/javascript"></script>
	<script src="../javascript/jquery.validationEngine.js" type="text/javascript"></script>        
	<script type="text/javascript">
	    $(document).ready(function () {
	        $('#txtPwd').focus();
	        $("#btnSubmit").click(function (event) {
	            $.ajax({
	                url: '../AutoComplete.asmx/GetWebUserPassword',
	                type: 'GET',
	                data: "BID='" + $('#hdnBranchID').val() + "'&UserName='" + $('#lblCustCode').text() + "'&Password='" + $('#txtPwd').val().trim() + "'",
	                timeout: 20000,
	                contentType: 'application/json; charset=UTF-8',
	                datatype: 'JSON',
	                cache: true,
	                async: false,
	                success: function (response) {
	                    var _val = response.d;
	                    if (_val == "True") {
	                    }
	                    else {
	                        $('#lblErr').text('Current Password does not match.');
	                        $('#txtPwd').focus();
	                        event.preventDefault();
	                    }
	                },
	                error: function (response) {
	                    alert(response.toString())
	                }
	            });
	            if ($('#txtNewPassword').val().trim() != $('#txtConfirmPassword').val().trim()) {
	                // alert("Password and Confirm Password don't match");
	                $('#lblErr').text("Password and Confirm Password don't match");
	                event.preventDefault();
	            }

	            if ($('#txtNewPassword').val().trim() === "") {
	                $('#lblErr').text("New Password con't blank.");
	                $('#txtNewPassword').focus();
	                event.preventDefault();
	            }
	            if ($('#txtConfirmPassword').val().trim() === "") {
	                $('#lblErr').text("Confirm Password con't blank.");
	                $('#txtConfirmPassword').focus();
	                event.preventDefault();
	            }
	        });
	    });
	</script>
	
	<script type="text/javascript">
		jQuery(document).ready(function () {
			jQuery("#form1").validationEngine();    
		});
	</script>
	</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="backgroundImage" id="form1">
      <div  class="row-fluid text-center">
	 <span class="textBold fa-lg">Change Password</span>
		 </div>
		  <div class="row-fluid div-margin">
		   <div class="col-sm-4">
		   <div class="col-sm-6"></div>
		   <div class="col-sm-5"><br /><br />
		   <img  id="StoreLOGO"  runat="server"  clientidmode="Static" />           
		   </div>
		   </div>
		   <div class="col-sm-4">
			  <div class="col-sm-1"></div>
			 <div class="col-sm-10  well well-sm WhiteColor">
		  <div class="row-fluid text-center">
			<h4 class="textBold"> User Name - 
			<asp:Label ID="lblCustCode" runat="server" EnableTheming="false" ClientIDMode="Static"  CssClass="textgray" /></h4> 
			</div>
			 <div class="row-fluid textpadding20">
			  <asp:TextBox ID="txtPwd" runat="server" MaxLength="15" ClientIDMode="Static" CssClass="form-control validate[required]" placeholder="Current Password"
			   TextMode="Password"></asp:TextBox> 
			</div>
			 <div class="row-fluid div-margin textpadding20">
			   <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="15" ClientIDMode="Static" CssClass=" form-control validate[required]" placeholder="New Password"
										TextMode="Password"></asp:TextBox>
			</div>
			 <div class="row-fluid div-margin textpadding20">              
			  <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="15" CssClass="form-control validate[required]" placeholder="Confirm Password "
									ClientIDMode="Static"	TextMode="Password"></asp:TextBox>             
			</div>    
			<div class="row-fluid div-margin textpadding20">           
			 <div class="col-sm-8 nopadding">
			 <asp:Button ID="btnSubmit" runat="server" Text="Change Password" onclick="btnSubmit_Click" EnableTheming="false"  CssClass="btn btn-primary btn-block btn-lg" ClientIDMode="Static"/>            
			</div>
			</div>
			  <div class="row-fluid">
		  <asp:Label ID="lblErr" runat="server" EnableTheming="false"  ClientIDMode="Static" CssClass="textRed"  Font-Bold="True" />
		  </div>
			<div class="row-fluid">
		   <h4><asp:Label ID="lblResult" runat="server" Font-Bold="true" CssClass="label label-success" ClientIDMode="Static" EnableTheming="false"/></h4>
		  </div>
			</div>
		  </div>
		  </div>
		  <div class="row-fluid container">
			<div class="span12 WhiteColor footer2">
				<h2 class="text-center" style="margin:0px">
					<img src="../img/LogoBIG.png" width="60px" height="50px" /><small><em ><span style="font-size:25px"> best way to manage your Dry Cleaning and Laundry business.</span> </em></small>
				</h2>
			</div>
		   </div><br />&nbsp;
		 </div> 
		  <asp:HiddenField ID="hdnBranchID"  runat="server" ClientIDMode="Static"/>		
		</asp:Content>

	




