<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmVerification.aspx.cs"
    Inherits="QuickWeb.frmVerification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-extend.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        //function to disable browser back button
        function DisableBackButton() {
            window.history.forward();
        }
        setTimeout("DisableBackButton()", 0);
        window.onunload = function () { null };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container">
        <div class="row-fluid">
            <div class="span12">
                <br />
                <div class="row-fluid">
                    <div class="span12 WhiteColor header">
                        <h2 class="text-center header2">
                            <small style="color: #787878"><span style="font-size: 18px"><b>Welcome to Quick Dry
                                Cleaning Software</b></span></small>
                        </h2>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span1 offset11">
                    <h4 class="btn btn-block disabled navbar-inverse" style="color: White">
                        <b>
                            <%=strVersion%></b></h4>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="well well-sm-tiny">
                <p class="textBold" style="text-align: center;">
                    A code will be sent to your registered email and an SMS will be sent to the registered
                    mobile no.
                </p>
            </div>
        </div>
        <br />
        <div class="row-fluid">
            <div id="divShowMsg" class="row-fluid Textpadding" style="display: none;">
                <div id="DivContainerStatus" class="div-margin">
                    <div id="DivContainerInnerStatus" class="span label label-default">
                        <h4 class="textmargin">
                            <span style="margin-left: 40%">
                                <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" EnableViewState="False"
                                    ForeColor="White" Font-Bold="True" />
                            </span><span style="margin-left: -20%"></span>
                        </h4>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="col-sm-1">
            </div>
            <div class="col-sm-2">
                <img src="img/LogoBIG.png" class="img-rounded" />
            </div>
            <div class="col-sm-1">
            </div>
            <div class="col-sm-4">
                <br />
                <div class="row-fluid well well-sm" visible="false" id="divLogIn" runat="server"
                    clientidmode="Static">
                    <div class="row-fluid">
                        <p>
                            Verification code has been sent to :<br />
                            <i class="fa fa-envelope-o fa-lg"></i>
                            <asp:Label ID="lblEmail" runat="server" EnableTheming="false" Text="" ClientIDMode="Static"
                                Font-Bold="True"></asp:Label>, <i class="fa fa-mobile fa-lg"></i><span>&nbsp;*** **
                                    ****</span>
                            <asp:Label ID="lblMobile" runat="server" EnableTheming="false" Text="" ClientIDMode="Static"
                                Font-Bold="True"></asp:Label>
                        </p>
                    </div>
                    <asp:TextBox ID="txtCode" placeholder="Kindly enter verification Code" class="input-block-level form-control"
                        runat="server" ClientIDMode="Static" MaxLength="6" TextMode="Password"> </asp:TextBox>
                    <asp:LinkButton ID="btnVerify1" runat="server" class="btn btn-primary btn-block div-margin textBold"
                        OnClick="btnVerify_Click" ClientIDMode="Static" EnableTheming="False"><i class="fa fa-external-link"></i>&nbsp;Verify</asp:LinkButton>
                    <div class="div-margin">
                        <asp:CheckBox ID="chkremember" Text="&nbsp;Remember me" Checked="true" runat="server" />
                        &nbsp; <a class="link2" id="achrForGetPwd" runat="server" clientidmode="Static" style="float: right;
                            display: none">I didn't receive a code</a>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="panel panel-primary " id="divPasswordGen" style="display: none" runat="server"
                        clientidmode="Static">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Generate Code for this Device
                            </h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid" id="divCodeCreate">
                                <div class="row-fluid">
                                    <span class="textBold"></span>
                                </div>
                                <asp:TextBox ID="txtDeviceName" placeholder="kindly enter Device Name" class="input-block-level form-control"
                                    runat="server" ClientIDMode="Static"> </asp:TextBox>
                                <asp:LinkButton ID="btnGenCode" runat="server" OnClick="btnGenCode_Click" class="btn btn-primary  btn-block div-margin"
                                    ClientIDMode="Static" EnableTheming="False"><i class="fa fa-align-justify"></i>&nbsp;Generate Code for this Device</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
    <div class="row-fluid container">
        <div class="span12 WhiteColor footer2">
            <h2 class="text-center footer5">
                <small style="color: #B7B7B7"><em><span style="font-size: 25px">The best way to manage
                    your Dry Cleaning and Laundry business.</span> </em></small>
            </h2>
        </div>
    </div>
    <asp:HiddenField ID="hdnMsg" runat="server" ClientIDMode="Static" />
    </form>
</body>
<script language="javascript" type="text/javascript">
    $(document).ready(function (e) {

        var strMsg = $('#hdnMsg').val();
        if (strMsg == '1') {
            setDivMouseOver('#FA8602', '#999999');
            $('#hdnMsg').val('0');
        }

        $('#btnGenCode').click(function (e) {
            if ($('#txtDeviceName').val().trim() == '') {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text("Kindly enter device name.");
                $('#txtDeviceName').focus();
                return false;
            }
            //    e.preventDefault();
        });
        $('#btnVerify1').click(function (e) {

            if ($('#txtCode').val().trim() == '') {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text("Kindly enter verification code.");
                $('#txtCode').focus();
                return false;
            }
        });

        $('#achrGenerateCode').click(function () {
            $('#txtDeviceName').val('');
            setTimeout(function () {
                $('#txtDeviceName').focus();
            }, 80);
        });
        $("#txtCode").keypress(function (e) {
            if (e.which == 13) {
                var btn = document.getElementById('<%=btnVerify1.ClientID%>');
                btn.click();
                return false;
            }
            //if the letter is not digit then display error and don't type anything
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //display error message
                setDivMouseOver('#FA8602', '#999999');
                $("#lblMsg").html("Kindly enter digits Only").show().fadeOut("slow");
                return false;
            }
        });

        $('#achrForGetPwd').click(function (e) {
            $("#divPasswordGen").show();
            $("#divLogIn").hide();
            $("#txtDeviceName").val('');
            $("#txtDeviceName").focus();
        });

        $("#txtDeviceName").keypress(function (e) {
            if (e.which == 13) {
                var btn1 = document.getElementById('<%=btnGenCode.ClientID%>');
                btn1.click();
                return false;
            }
        });
    });

    function setDivMouseOver(argColorOne, argColorTwo) {
        document.getElementById('divShowMsg').style.display = "inline";
        $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
    }
</script>
</html>
