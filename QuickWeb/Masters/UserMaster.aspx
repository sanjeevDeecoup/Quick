<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="UserMaster" Title="Untitled Page" CodeBehind="UserMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #passstrength
        {
            color: red;
            font-family: verdana;
            font-size: 10px;
            font-weight: bold;
        }
        .tooltip-inner
        {
            max-width: 340px;
            width: 340px;
            background-color: #F1F1F1;
            text-align: left;
            font-size: 14px;
            color: Black;
            border: 1px solid #C0C0A0;
        }
    </style>
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
        <div class="col-sm-8">
            <div class="panel panel-primary well-sm-tiny1">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Users</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid textCenter">
                        <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                    </div>
                    <div class="row-fluid ">
                        <div class="row-fluid">
                            <div class="col-sm-11 input-group Textpadding">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-users"></i></span>
                                <asp:DropDownList ID="drpUserType" runat="server" DataSourceID="SqlDataUserTypeMaster"
                                    TabIndex="1" DataTextField="UserType" DataValueField="UserTypeID" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataUserTypeMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                    SelectCommand="SELECT [UserTypeID], [UserType] FROM [UserTypeMaster] where UserTypeID<>4" ProviderName="System.Data.SqlClient">
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="row-fluid div-margin">
                            <div class="col-sm-5 input-group  Textpadding">
                                <span class="input-group-addon IconBkColor">ID</span>
                                <asp:TextBox ID="txtUserId" runat="server" MaxLength="20" placeholder="kindly enter user ID"
                                    TabIndex="2" CssClass="form-control" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="InvalidChars"
                                    TargetControlID="txtUserId" InvalidChars="`~:;,-">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="col-sm-1 Textpadding form-inline">
                                <div class="form-group">
                                    <span class="span textBold">&nbsp;*</span>
                                </div>
                                <div style="margin-top: -9px" class="form-group">
                                    &nbsp;<i class="fa fa fa-info-circle fa-lg txtColor" data-placement="right" id="IdInfo"
                                        data-original-title="" title=""></i>
                                </div>
                            </div>
                            <div class="col-sm-5 input-group  Textpadding">
                                <span class="input-group-addon IconBkColor">Password</span>
                                <asp:TextBox ID="txtUserPassword" runat="server" MaxLength="30" TextMode="Password"
                                    TabIndex="3" EnableViewState="false" placeholder="Password" ClientIDMode="Static"
                                    CssClass="form-control" />
                               <%-- <span id="passstrength"></span>--%>
                            </div>
                            <div class="col-sm-1 Textpadding">
                                <span class="textRed">&nbsp;*</span></div>
                        </div>
                       <%-- <div class="row-fluid">
                            <div class="col-sm-6">
                            </div>
                            <div class="col-sm-6 Textpadding">
                                <asp:CheckBox ID="checkUpdatePassword" runat="server" EnableViewState="False" Text="&nbsp;Update Password to."
                                    Visible="False" CssClass="TDCaption" />
                            </div>
                        </div>--%>
                        <div class="row-fluid div-margin">
                            <div class="col-sm-11 input-group  Textpadding">
                                <span class="input-group-addon">Display Label</span>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" CssClass="form-control"
                                    TabIndex="4" placeholder="Kindly enter user name" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="InvalidChars"
                                    TargetControlID="txtUserName" InvalidChars="`~:;,-">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="col-sm-1 Textpadding form-inline">
                                <div class="form-group">
                                    <span class="span textBold">&nbsp;*</span>
                                </div>
                                <div style="margin-top: -9px" class="form-group">
                                    &nbsp;<i class="fa fa fa-info-circle fa-lg txtColor" data-placement="right" id="DisplayInfo"
                                        data-original-title="" title=""></i>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid div-margin">
                            <div class="col-sm-5 input-group  Textpadding">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-thumb-tack"></i></span>
                                <asp:TextBox ID="txtUserPin" runat="server" MaxLength="200" placeholder="Kindly enter user Pin"
                                    TabIndex="6" CssClass="form-control" />
                            </div>
                            <div class="col-sm-1 Textpadding">
                                &nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa fa-info-circle fa-lg txtColor" data-placement="right"
                                    id="PinInfo" data-original-title="" title=""></i>
                            </div>
                            <div class="col-sm-5 input-group  Textpadding">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-barcode fa-lg"></i></span>
                                <asp:TextBox ID="txtUserbarcode" runat="server" placeholder="Kindly enter barcode"
                                    TabIndex="7" MaxLength="200" CssClass="form-control" />
                            </div>
                            <div class="col-sm-1 Textpadding">
                                &nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa fa-info-circle fa-lg txtColor" data-placement="right"
                                    id="Barcodeinfo" data-original-title="" title=""></i>
                            </div>
                        </div>
                        <div class="row-fluid div-margin">
                            <div class="col-sm-5 input-group  Textpadding">
                                <div class="input-group">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-envelope-o fa-lg"></i>
                                    </span>
                                    <asp:TextBox ID="txtUserEmailId" runat="server" placeholder="Kindly enter user Email"
                                        TabIndex="5" MaxLength="100" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-sm-1 Textpadding">
                            </div>
                            <div class="col-sm-5 input-group  Textpadding">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-phone fa-lg"></i></span>
                                <asp:TextBox ID="txtUserMobile" runat="server" placeholder="Kindly enter mobile no"
                                    TabIndex="9" MaxLength="11" CssClass="form-control" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                                    TargetControlID="txtUserMobile">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="col-sm-1 Textpadding ">
                            </div>
                        </div>
                        <div class="row-fluid div-margin" runat="server" clientidmode="Static" id="divActive">
                            <div class="col-sm-11 input-group  Textpadding">
                                <div class="col-sm-1 Textpadding">
                                    <input type="checkbox" class="ace ace-switch ace-switch-5" checked="checked" runat="server"
                                        id="chkActive" />
                                    <span class="lbl"></span>
                                </div>
                                <div class="col-sm-11 Textpadding">
                                    &nbsp;&nbsp;<asp:Label ID="ActiveTxt" runat="server" ClientIDMode="Static">
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnSave" class="btn btn-info" runat="server" OnClick="btnSave_Click"
                        EnableTheming="false"><i class="fa fa-floppy-o"></i>&nbsp;Save</asp:LinkButton>
                    <asp:LinkButton ID="btnEdit" runat="server" EnableTheming="false" CssClass="btn btn-info"
                        OnClick="btnEdit_Click"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                    <asp:Button ID="btnAddNew" runat="server" Text="Refresh" OnClick="btnAddNew_Click"
                        EnableTheming="false" CssClass="btn btn-primary" Visible="false" />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        Visible="false" EnableTheming="false" CssClass="btn btn-primary" OnClientClick="return checkEntry();" />
                    <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click"
                        Visible="false" EnableTheming="false" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny" style="overflow: auto; height: 190px;">
            <asp:GridView ID="grdSearchResult" runat="server" EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover"
                AllowPaging="True" AutoGenerateColumns="False" PageSize="10" EmptyDataText="There are no data records to display."
                CellPadding="2" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                OnPageIndexChanging="grdSearchResult_PageIndexChanging">
                <PagerStyle CssClass="gridview" HorizontalAlign="Center"></PagerStyle>
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="UserId" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="UserType" HeaderText="Type" />
                    <asp:BoundField DataField="UserName" HeaderText="Name" />
                    <asp:BoundField DataField="UserMobileNumber" HeaderText="Mobile" />
                    <asp:BoundField DataField="UserEmailId" HeaderText="Email" />
                    <asp:BoundField DataField="UserActive" HeaderText="Active" />
                    <%-- <asp:TemplateField HeaderText="Active">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("UserActive") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("UserActive") %>' Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUserPin" runat="server" Text='<%# Bind("UserPin") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUserBarcode" runat="server" Text='<%# Bind("Userbarcode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUserPassword" runat="server" Text='<%# Bind("UserPassword") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:DropDownList ID="ddlUsername" runat="server" ClientIDMode="Static" Style="visibility: hidden">
        </asp:DropDownList>
        <asp:HiddenField ID="hdntempUserName" runat="server" ClientIDMode="Static" />
    </div>
    <!-- Start of js -->
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            $("#ctl00_ContentPlaceHolder1_btnSave,#ctl00_ContentPlaceHolder1_btnEdit").click(function (event) {
                clearMsg();
                var brcode = document.getElementById("<%=txtUserId.ClientID %>").value.trim().length;
                var brname = document.getElementById("<%=txtUserName.ClientID %>").value.trim().length;
                var pwd = document.getElementById("<%=txtUserPassword.ClientID %>").value.trim().length;
                if (brcode == "" || brcode.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Kindly enter user Id.");
                    document.getElementById("<%=txtUserId.ClientID %>").focus();
                    return false;
                }
                if (brname == "" || brname.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Kindly enter user name.");
                    document.getElementById("<%=txtUserName.ClientID %>").focus();
                    return false;
                }
                if (pwd == "" || pwd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Kindly enter password.");
                    document.getElementById("<%=txtUserPassword.ClientID %>").focus();
                    return false;
                }

                var unique = true;
                var tempUserName = $('#hdntempUserName').val().toLowerCase();
                var name = $('#ctl00_ContentPlaceHolder1_txtUserName').val().toLowerCase();
                if (tempUserName != name) {
                    $('#ddlUsername option').each(function (i, v) {
                        if (unique) {
                            unique = v.textContent.toLowerCase() != name;
                        }
                    });
                }
                if (!unique) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("This Display label  already exists, please enter a different no.");
                    $('#ctl00_ContentPlaceHolder1_txtUserName').focus();
                    event.preventDefault();
                }
            });
        });
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
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
//            $('#txtUserPassword').keyup(function (e) {
//                var strongRegex = new RegExp("^(?=.{6,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
//                var mediumRegex = new RegExp("^(?=.{5,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
//                var enoughRegex = new RegExp("(?=.{3,}).*", "g");
//                if (false == enoughRegex.test($(this).val())) {
//                    $('#passstrength').html('More Characters');
//                } else if (strongRegex.test($(this).val())) {
//                    $('#passstrength').className = 'ok';
//                    $('#passstrength').html('Strong!');
//                } else if (mediumRegex.test($(this).val())) {
//                    $('#passstrength').className = 'alert';
//                    $('#passstrength').html('Medium!');
//                } else {
//                    $('#passstrength').className = 'error';
//                    $('#passstrength').html('Weak!');
//                }
//                return true;
//            });

            ShowText();

            $('#ctl00_ContentPlaceHolder1_chkActive').click(function (e) {
                ShowText();
            });
            $("#IdInfo").tooltip({
                title: 'This will be used for login purpose,Keep it small.',
                html: true
            });

            $("#DisplayInfo").tooltip({
                title: 'This will be used in Reporting. Type your name here.',
                html: true
            });

            $("#PinInfo").tooltip({
                title: 'This will be used in while making cloth ready , it help to trace how many cloths are marked ready by a user.',
                html: true
            });

            $("#Barcodeinfo").tooltip({
                title: 'This can be used for employee ID if you are using Barcode for employee code.',
                html: true
            });

        });
        function ShowText() {
            if ($('#ctl00_ContentPlaceHolder1_chkActive').is(':checked')) {
                $('#ActiveTxt').text("This user is active, allow him to login.");
                $('#ActiveTxt').removeClass('txtColor');

            } else {
                $('#ActiveTxt').text("User is temporary disabled.");
                $('#ActiveTxt').addClass('txtColor');
            }
        }


    </script>
</asp:Content>
