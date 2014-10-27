<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true" CodeBehind="BackupwithEmail.aspx.cs" Inherits="QuickWeb.BackUp.BackupwithEmail" EnableEventValidation="false" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="../css/loader.css" rel="stylesheet" type="text/css" />
	<script src="../JavaScript/jquery-1.6.2.min.js" type="text/javascript"></script>
	<script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
	<script src="../js/loader.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {

        $('#btnBackup').click(function (e) {
            if (e.clientX == 0 || e.clientY == 0) {
                return false;
            }
        });
        $('#btnBackup').click(function (e) {
            if (e.clientX == 0 || e.clientY == 0) {
                return false;
            }

            clearMsg();
            if ($('#chkEmail').is(":checked")) {
                if ($("#txtEmailID").val().trim() == "") {
                    setDivMouseOver('#FA8602', '#999999');
                    $("#lblErr").text("Kindly enter Email ID.");
                    $("#txtEmailID").focus();
                    return false;
                }
                var sEmail = $('#txtEmailID').val();
                if (validateEmail(sEmail)) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblErr').text("");
                }
                else {
                    $("#txtEmailID").focus();
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblErr').text('Kindly enter valid Email ID.');
                    return false;
                }

            }
            __doPostBack('ctl00$ContentPlaceHolder1$btnBackup', null);
            $('#pnlBkp').dialog({ width: 100, height: 120, modal: true });
        });

        $('#chkEmail').click(function () {
            if ($('#chkEmail').is(":checked")) {
                $("#txtEmailID").removeAttr("disabled");
                $("#txtEmailID").focus();
            }
            else {
                var Email = $("#txtEmailID").val();
                $('#hdnEmailID').val(Email);
                $("#txtEmailID").attr("disabled", "disabled");

            }
        });

    });
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) {
            return true;
        }
        else {
            return false;
        }
    }
    function clearMsg() {
        $('#lblErr').text('');
        $('#lblMsg').text('');
    }
    function setDivMouseOver(argColorOne, argColorTwo) {
        document.getElementById('divShowMsg').style.display = "inline";
        $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
        setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center">                  
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                            ForeColor="White" />
                </h4>
            </div>
        </div>
    </div>
<div class="panel panel-primary well-sm-tiny1">
		<div class="panel-heading">
		  <h3 class="panel-title">Backup</h3>
		</div>
		<div class="panel-body">

                <div id="divBkpMsg" runat="server" clientidmode="Static" visible="false">
                <div class="row-fluid text-center well well-sm-tiny" style="background-color: #FFDDDD">         
             <p style="font-size: medium">
             Back up for all the information including business and customer records is being automatically taken every night and is safe on the web server. We take due care to preserve it  so as to use it in case of any eventuality. Feel free to contact the support team for more information around this.
            <a href="mailto:info@quickdrycleaning.com">info@quickdrycleaning.com</a>
             </p>
             </div>
                </div>

        <div id="divBackUp" runat="server" clientidmode="Static" visible="false">
         <div class="row-fluid text-center well well-sm-tiny" style="background-color: #FFDDDD">         
         <p style="font-size: medium">You can now take the back up for all the information including business and customer records. Kindly specify the location on your hard drive and also the email id, in case you wish to get the back up on your email. Please ensure this computer is connected with an active internet connection to be able to send the email.</p>
         </div>        
		<div class="row-fluid div-margin">
        <div class="row-fluid">
        <div class="col-sm-2"></div>
         <div class="col-sm-2">
         <span class="textBold">Back Up on Hard Drive</span>         
         </div>
         <div class="col-sm-1">
		  <input type="checkbox" class="ace ace-switch ace-switch-5" checked=""  clientidmode="Static" runat="server" id="chkBackUp" disabled="disabled" />
						   <span class="lbl"></span>  </div>
           <div class="col-sm-2">
		    <span class="textBold"> Database Backup Path</span>
		    </div>
		    <div class="col-sm-2">
            <div class="input-group">
              <span class="input-group-addon"><i class="fa fa-download fa-lg"></i></span>
                 <asp:DropDownList ID="Drpdrive" runat="server" CssClass="form-control" ClientIDMode="Static" >
		    </asp:DropDownList>
            </div>
		    </div>
          </div>
          <div class="row-fluid div-margin">
        <div class="col-sm-2"></div>
         <div class="col-sm-2">
         <span class="textBold">Back Up on Email</span>         
         </div>
         <div class="col-sm-1">
		  <input type="checkbox" class="ace ace-switch ace-switch-5" checked="" clientidmode="Static" runat="server" id="chkEmail" />
		 <span class="lbl"></span> 
         </div>        
		    <div class="col-sm-4">
            <div class="input-group">
              <span class="input-group-addon"><i class="fa fa-envelope-o fa-lg"></i></span>              
               <asp:TextBox ID="txtEmailID" runat="server"  ClientIDMode="Static"  MaxLength="50"  CssClass="form-control" placeholder="Enter Email ID"></asp:TextBox>
            </div>
		    </div>
          </div>
		</div>
		<div class="row-fluid div-margin">				
		<div class="col-sm-4"></div>
		<div class="col-sm-4"> 
		<div class="col-sm-1"></div>  
		<div class="col-sm-10">
		<%--<asp:Button ID="btnBackup" runat="server" Text="Take Back Up" EnableTheming="false"  CssClass="btn btn-info btn-block btn-lg" 
						onclick="btnBackup_Click" ClientIDMode="Static" />--%>
        <a class="btn btn-primary btn-lg btn-block" id="btnBackup" runat="server" clientidmode="static">
           <i class="fa fa-download fa-lg"></i>&nbsp;&nbsp;Back up Now</a>
		</div>   
		</div>		
		</div>
		<%--<div class="row-fluid">
		<div id="loadimage" class="loading">
				<span class="disp"> Please Wait Backup is in Progress....</span>
					<img  src="../images/ajax-loaderBig.gif"  />
				</div>		
		</div>--%>
		<div class="row-fluid">
		<asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hdnEmailID" runat="server" ClientIDMode="Static" />
	<asp:Label ID="Label1" runat="server"></asp:Label>
		</div>
        </div>
	<asp:Panel ID="pnlBkp" runat="server" Style="display:none" ClientIDMode="Static" >
			<asp:UpdatePanel ID="UpdatePanel8" runat="server">			
                   <ContentTemplate>
                <div style="margin-top: -10px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="fa  textBold"> Please Wait..</span><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-spinner fa-spin fa-3x"></i>                  
                </div>
            </ContentTemplate>
		  </asp:UpdatePanel>
	</asp:Panel>
		</div>
 </div>
</asp:Content>
