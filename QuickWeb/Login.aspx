<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="Shortcut Icon" type="image/ico" href="images/favicon.ico" />
    <title>
        <% Response.Write(ConfigurationManager.AppSettings["AppTitle"]); %>
    </title>
    <link rel="Stylesheet" type="text/css" href="css/login.css" />
    <link href="css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="JavaScript/bowser.js" type="text/javascript"></script>
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
                 clearMsg();
                 var s = String.fromCharCode(e.which);
                 if ((s.toUpperCase() === s && s.toLowerCase() !== s && !e.shiftKey) ||
                   (s.toUpperCase() !== s && s.toLowerCase() === s && e.shiftKey)) {
                     setDivMouseOver('#FA8602', '#999999');
                     $('#lblCaps').html('Caps Lock is on.');
                 } else {
                     $('#lblCaps').html('');
                     document.getElementById('divShowMsg').style.display = "none";
                 }
             });
         });
    </script>
     <script type="text/javascript">

         $(function () {
             // detect browser properties
             $("#btnLogin").click(function () {
                 clearMsg();
                 var Allbrowsername = JSON.stringify(bowser, null, '    ');
                 var SplitBrowserName = Allbrowsername.split(':');
                 var SplitMainBrowser = SplitBrowserName[1].split(',');
                 var res = SplitMainBrowser[0];
                 var isMobile = {
                     Android: function () {
                         return navigator.userAgent.match(/Android/i);
                     },
                     BlackBerry: function () {
                         return navigator.userAgent.match(/BlackBerry/i);
                     },
                     iOS: function () {
                         return navigator.userAgent.match(/iPhone|iPad|iPod/i);
                     },
                     Opera: function () {
                         return navigator.userAgent.match(/Opera Mini/i);
                     },
                     Windows: function () {
                         return navigator.userAgent.match(/IEMobile/i);
                     },
                     any: function () {
                         return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
                     }
                 };
                 if (isMobile.any()) {
                 }
                 else {
                     if (res != ' "Firefox"') {
                         setDivMouseOver('#FA8602', '#999999');
                         $('#lblCaps').html('Your browser does not support this software.');
                         return false;
                     }
                 }

             });

         });
 
</script>
<script type="text/javascript">
    $(document).ready(function () {
        var Allbrowsername = JSON.stringify(bowser, null, '    ');
        var SplitBrowserName = Allbrowsername.split(':');
        var SplitMainBrowser = SplitBrowserName[1].split(',');
        var res = SplitMainBrowser[0];
        var isMobile = {
            Android: function () {
                return navigator.userAgent.match(/Android/i);
            },
            BlackBerry: function () {
                return navigator.userAgent.match(/BlackBerry/i);
            },
            iOS: function () {
                return navigator.userAgent.match(/iPhone|iPad|iPod/i);
            },
            Opera: function () {
                return navigator.userAgent.match(/Opera Mini/i);
            },
            Windows: function () {
                return navigator.userAgent.match(/IEMobile/i);
            },
            any: function () {
                return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
            }
        };
        if (isMobile.any()) {           
        }
        else {
            if (res != ' "Firefox"') {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblCaps').html('Your browser does not support this software.');
                return false;
            }
        }
    });

    function clearMsg() {
        $('#lblCaps').text('');
        $('#lblMsg').text('');
    }

    function setDivMouseOver(argColorOne, argColorTwo) {
        document.getElementById('divShowMsg').style.display = "inline";
        $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
        setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
    }
    </script>
</head>
<body onload="ShowReturnMsg()">
    <div class="container">
        <div class="row-fluid">
            <div class="span12">
                <h1 class="btn btn-default btn-block disabled btn-lg"><b>
                    Welcome to <strong>Quick Dry Cleaning Software</strong></b></h1>
            </div>
            <div class="row-fluid">
                <div class="span1 offset11">
                    <h4 class="btn btn-block disabled navbar-inverse" style="color:White">
                       <b> <%=strVersion%></b></h4>
                </div>
            </div>
        </div>
         <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">                  
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                        <asp:Label ID="lblCaps" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                            ForeColor="White" />
                </h4>
            </div>
        </div>
    </div>
        <div class="row-fluid">
            <div class="span2 offset1">
                <img src="img/LogoBIG.png" class="img-rounded" />
            </div>
            <div class="span6">
                <form id="form1" runat="server" class="form-signin">
                <h2 class="form-signin-heading">
                    <b>Please sign in</b></h2>
                <asp:TextBox ID="txtUserId" class="input-block-level form-control" placeholder="User Name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqUserId" runat="server" Display="None" EnableClientScript="true"
                    ControlToValidate="txtUserId" ErrorMessage="User Id is a required field and cannot be left blank"></asp:RequiredFieldValidator>
                <asp:ValidationSummary ID="validUserId" runat="server" EnableClientScript="true"
                    ShowMessageBox="true" ShowSummary="false" />
                <asp:TextBox ID="txtPassword" placeholder="Password" class="input-block-level form-control" runat="server"
                    TextMode="Password"> </asp:TextBox>
                <asp:RequiredFieldValidator ID="reqPassword" runat="server" Display="None" EnableClientScript="true"
                    ControlToValidate="txtPassword" ErrorMessage="Password is a required field and cannot be left blank"></asp:RequiredFieldValidator>
                <asp:DropDownList ID="drpBranch" class="input-block-level form-control" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnLogin" runat="server" class="btn btn-primary btn-lg div-margin active" Text="Sign in"
                    OnClick="btnLogin_Click" EnableTheming="False" ClientIDMode="Static" />
              
                <asp:HiddenField ID="hdnMAC" runat="server" />
                  <asp:HiddenField ID="hdnWeekly" runat="server" />
                </form>
            </div>
            <div class="span3">
                &nbsp;</div>
        </div>
    </div>
    <br />
    <div class="container">
        <div class="row-fluid">
            <div class="span12">
                <h1 class="btn btn-default btn-block disabled btn-lg">
                    <small class="fa-lg"><em>The best way to manage your Dry Cleaning and Laundry business. </em></small>
                </h1>
            </div>
        </div>
    </div>   
</body>
<script type="text/javascript">
    function ShowReturnMsg() {
        var msg = '<%= Session["ReturnMsg"] %>';
        if (msg.length > 0) alert(msg);
    }
</script>
<% Session["ReturnMsg"] = null; %>
</html>