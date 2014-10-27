<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="QuickWeb.Website_User.MainForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <link rel="Shortcut Icon" type="image/ico" href="../images/favicon.ico" />
	 <title>
		<%=AppTitle %></title>
	<link href="../css/main.css" rel="stylesheet" type="text/css" />
		<link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    	
	<link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
	<link href="../css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
	<link href="../css/font-awesome.min.css" rel="stylesheet" type="text/css" />
	<script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
   <script type="text/javascript">
	   $(document).ready(function () {
		   $('#lblStoreName').html($("#lblBranch").html());
	   });
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title textBold">Login Detail - <asp:Label ID="lblStoreName" runat="server" EnableTheming="false" /></h3>
		</div>
		<div class="panel-body">        
		     
        
           
		<div class="row-fluid div-margin">        
		 <div class="col-sm-4 rightborder">	
         
         	
            <div class="well well-sm-tiny1" style="background-color:White">

		  <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2 Textpadding textBold">
               <%--<asp:Label ID="lblCName" runat="server" Text="User Name"></asp:Label>--%>	
               <i class="fa fa-users fa-lg margintop5"></i>		
			</div>
			<div class="col-sm-10 Textpadding ">
			<asp:Label ID="lblCustomer" runat="server"></asp:Label>
			</div>
			</div>
            <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2 Textpadding textBold">
               <%--<asp:Label ID="lblBName" runat="server" Text="Branch Name"></asp:Label>--%>
               <i class="fa fa-sitemap fa-lg margintop5"></i> 
			</div>
			<div class="col-sm-10 Textpadding ">
			<asp:Label ID="lblBranch" runat="server"></asp:Label> 			</div>
			</div>
             <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2 Textpadding textBold">
               <%--<asp:Label ID="lblCustName" runat="server" Text="Customer Name" Visible="false"></asp:Label>--%>
               <i class="fa fa-user fa-lg  margintop5"></i>
			</div>
			<div class="col-sm-10 Textpadding ">
			<asp:Label ID="lblCustValue" runat="server"></asp:Label>
			</div>
			</div>
             <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2 Textpadding textBold">
              <i class="fa fa-building-o fa-lg margintop5"></i>
			</div>
			<div class="col-sm-10 Textpadding ">
			<asp:Label ID="lblAddress" runat="server"></asp:Label>
			</div>
			</div>
             <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2 Textpadding textBold">
               <i class="fa fa-mobile fa-2x margintop5"></i>
			</div>
			<div class="col-sm-10 Textpadding ">
			<asp:Label ID="lblMobile" runat="server"></asp:Label>
			</div>
			</div>
             <div class="row-fluid well well-sm-tiny">
			<div class="col-sm-2 Textpadding textBold">
              <i class="fa fa-envelope-o fa-lg margintop5"></i>
			</div>
			<div class="col-sm-10 Textpadding ">
			<asp:Label ID="lblEmail" runat="server"></asp:Label>
			</div>
			</div>
            </div>
		 </div>
         <div class="col-sm-6">
         <div class="row-fluid">
		<div class="col-sm-6">
		<asp:Button ID="btnChange" runat="server" CssClass="btn btn-primary btn-block btn-lg"  Text="Change Password"  EnableTheming="false" 
			OnClick="btnChange_Click" />
	
		  
		  </div>

          <div class="col-sm-6"> 
          	<asp:Button ID="CheckStatus" runat="server"  CssClass="btn btn-primary btn-block btn-lg" EnableTheming="false"
			Text="Check Status" OnClick="CheckStatus_Click" />
          </div>
        </div>
        <div class="row-fluid div-margin">
        <div class="col-sm-3"></div>
        <div class="col-sm-6">
          <asp:LinkButton ID="linkLabel" runat="server" Text="Log Out"  EnableTheming="false" CssClass="btn btn-danger btn-block btn-lg"
				onclick="linkLabel_Click"></asp:LinkButton>
        </div>
        </div>


          </div>
		</div>
  </div>
  </div>
	</form>
</body>
</html>
