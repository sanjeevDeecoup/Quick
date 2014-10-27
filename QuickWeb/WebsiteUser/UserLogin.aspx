<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="QuickWeb.WebsiteUsers.UserLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
<link rel="Shortcut Icon" type="image/ico" href="../images/favicon.ico" />
	 <title>
		<%=AppTitle %></title>
	<link href="../css/template.css" rel="stylesheet" type="text/css" />
	<link href="../css/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
	<link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />    
	<link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    	
	<link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
	<link href="../css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
	<script src="../JavaScript/jquery-1.6.2.min.js" type="text/javascript"></script> 
	<script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
	 <script src="../javascript/jquery.validationEngine-en.js" type="text/javascript"></script>
	<script src="../javascript/jquery.validationEngine.js" type="text/javascript"></script>    
    <script language="javascript" type="text/javascript">
        //function to disable browser back button
        function DisableBackButton() {
            window.history.forward();
        }
        setTimeout("DisableBackButton()", 0);
        window.onunload = function () { null };
	</script>   
    <script language="Javascript" type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var s = String.fromCharCode(e.which);
                if ((s.toUpperCase() === s && s.toLowerCase() !== s && !e.shiftKey) ||
                   (s.toUpperCase() !== s && s.toLowerCase() === s && e.shiftKey)) {
                    $('#lblResult').html('Caps Lock is on.');
                } else {
                    $('#lblResult').html('');
                }
            });
        });
    </script>
	<script type="text/javascript">
	    jQuery(document).ready(function () {
	        jQuery("#form1").validationEngine();
	        $('#txtUserName').focus();
	        $('#chkforget').click(function () {
	            if ($('#chkforget').is(':checked'))
	                alert('Please Contact your Shop Owner');
	        });

	        $('#drpBranchName').change(function () {
	            //  $('#lblStoreName').html($("#drpBranchName option:selected").text());                 

	            $.ajax({
	                url: '../AutoComplete.asmx/GetStoreData',
	                type: 'GET',
	                data: "BID='" + $('#drpBranchName').val() + "'",
	                timeout: 20000,
	                contentType: 'application/json; charset=UTF-8',
	                datatype: 'JSON',
	                cache: true,
	                async: false,
	                success: function (response) {
	                    var _val = response.d;
	                    var Arydata = _val.split(':');
	                    $('#lblStoreName').text(Arydata[0]);
	                    $('#lblStoreAddress').text(Arydata[1]);
	                },
	                error: function (response) {
	                    alert(response.toString())
	                }
	            });
	            var LOGOPath = "../ReceiptLogo/DRY" + $('#drpBranchName').val() + ".jpg";
	            $('#StoreLOGO').attr('src', LOGOPath);
	        });

	    });
	</script>
</head>
<body class="backgroundImage">
	<form id="form1" runat="server">
	<div class="panel panel-primary well-sm-tiny1 panel-borderColor" style="background-color:transparent">
		<div class="panel-heading">
		  <h3 class="panel-title textBold"><asp:Label ID="lblStoreName" runat="server" EnableTheming="false" />   - Customer Login 
          <span style="float:right"><asp:Label ID="lblStoreAddress" runat="server" EnableTheming="false" /></span>
           </h3>
		</div>
		<div class="panel-body"> 

        <div class="row-fluid">
		   <div class="col-sm-4">
           <div class="col-sm-6"></div>
           <div class="col-sm-5"><br /><br />
           <img id="StoreLOGO"  runat="server"  clientidmode="Static"/>           
           </div>
           </div>
		   <div class="col-sm-4">
			 <div class="col-sm-1"></div>
			 <div class="col-sm-10  well well-sm WhiteColor">
			<div class="row-fluid text-center">
			<h4 class="textBold"> Login Form</h4> 
			</div>
			 <div class="row-fluid div-margin textpadding20">			 
			<asp:TextBox ID="txtUserName" runat="server" placeholder="User Name"  MaxLength="12" CssClass="form-control validate[required]"></asp:TextBox>
			</div>
			 <div class="row-fluid div-margin textpadding20">
			  <asp:TextBox ID="txtPwd" runat="server" MaxLength="15" CssClass="form-control validate[required]"
				placeholder="Password"	TextMode="Password"></asp:TextBox>
			</div>

			 <div class="row-fluid textpadding20 div-margin">
			 <asp:DropDownList ID="drpBranchName" runat="server" CssClass="form-control validate[required]">
									</asp:DropDownList>  
			</div>
			<div class="row-fluid textpadding20 div-margin">
			<div class="col-sm-4 Textpadding">
			<asp:Button ID="btnSubmit" runat="server" Text="Login" onclick="btnSubmit_Click" EnableTheming="false"  CssClass="btn btn-primary btn-block btn-lg"/>
			</div>
			</div>
			<div class="row-fluid div-margin textpadding20">
			 <asp:CheckBox ID="chkforget" runat="server" Text="&nbsp;Forgot Password" ClientIDMode="Static" />
			</div>
			<div class="row-fluid div-margin">
			<asp:Label ID="lblResult" runat="server" Font-Bold="true" ClientIDMode="Static" EnableTheming="false" CssClass="label label-danger"/>            
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
           </div>
  </div>
  </div>
	</form>
</body>
</html>

