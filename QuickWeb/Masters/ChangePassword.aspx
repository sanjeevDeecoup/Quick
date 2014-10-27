<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="ChangePassword" Title="Process Master" CodeBehind="ChangePassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnChangePassword").click(function (event) {
                clearMsg();
                var strPwd = document.getElementById("<%=txtPwd.ClientID %>").value.trim().length;
                var strNewPwd = document.getElementById("<%=txtPasswordNew.ClientID %>").value.trim().length;
                var strConfirmPwd = document.getElementById("<%=txtPasswordConfirm.ClientID %>").value.trim().length;

                if (strPwd == "" || strPwd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Kindly enter current password.");
                    document.getElementById("<%=txtPwd.ClientID %>").focus();
                    return false;
                }
                if (strNewPwd == "" || strNewPwd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Kindly enter new password.");
                    document.getElementById("<%=txtPasswordNew.ClientID %>").focus();
                    return false;
                }
                if (strConfirmPwd == "" || strConfirmPwd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Kindly enter Confirm password.");
                    document.getElementById("<%=txtPasswordConfirm.ClientID %>").focus();
                    return false;
                }

                var NewPwd = document.getElementById("<%=txtPasswordNew.ClientID %>").value.trim();
                var ConfirmPwd = document.getElementById("<%=txtPasswordConfirm.ClientID %>").value.trim();
                if (NewPwd != ConfirmPwd) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Confirm password does not match.");
                    document.getElementById("<%=txtPasswordConfirm.ClientID %>").focus();
                    return false;
                }

                if (strPwd != '') {
                    $.ajax({
                        url: '../AutoComplete.asmx/GetStoreUserPassword',
                        type: 'GET',
                        data: "Password='" + $('#txtPwd').val().trim() + "'",
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
                                setDivMouseOver('#FA8602', '#999999');
                                $('#lblErr').text('Current Password does not match.');
                                $('#txtPwd').focus();
                                event.preventDefault();
                            }
                        },
                        error: function (response) {
                            alert(response.toString())
                        }
                    });

                }
            });
        });
    </script>
    <script type="text/javascript" language="javascript">

        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
        function clearMsg() {
            $('#lblErr').text('');
            $('#lblMsg').text('');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="textmargin">
                    <span style="margin-left: 40%">
                        <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" EnableViewState="False"
                            ForeColor="White" Font-Bold="True" />
                        <asp:Label ID="lblErr" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                            Font-Bold="True" />
                    </span><span style="margin-left: -20%"></span>
                </h4>
            </div>
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="col-sm-4">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Change Password - <span class="fa-lg">
                            <%Response.Write(Session["UserName"].ToString()); %></span></h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 Textpadding">
                            <asp:TextBox ID="txtPwd" runat="server" MaxLength="15" ClientIDMode="Static" CssClass="form-control validate[required]"
                                placeholder="Current Password" TextMode="Password"></asp:TextBox>
                        </div>
                        <div class="col-sm-1 Textpadding form-inline">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 Textpadding">
                            <asp:TextBox ID="txtPasswordNew" placeholder="New Password" runat="server" MaxLength="30"
                                TextMode="Password" CssClass="form-control"></asp:TextBox>
                            <%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPasswordNew"
                                Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-sm-1 Textpadding form-inline">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 Textpadding">
                            <asp:TextBox ID="txtPasswordConfirm" placeholder="Confirm Password " runat="server"
                                MaxLength="30" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPasswordConfirm"
                                Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                            <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPasswordNew"
                                ControlToValidate="txtPasswordConfirm" Display="Dynamic" ErrorMessage="Confirm password does not match."></asp:CompareValidator>--%>
                        </div>
                        <div class="col-sm-1 Textpadding form-inline">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnChangePassword" class="btn btn-info" runat="server" OnClick="btnChangePassword_Click"
                        EnableTheming="false"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
