<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="BranchMaster" Title="Branch Master" CodeBehind="BranchMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/bootstrap-timepicker.min.js" type="text/javascript"></script>
    <style type="text/css">
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
    <script type="text/javascript" language="javascript">
        $(document).ready(function (e) {
            ShowText();
            $("#MobileInfo").tooltip({
                title: 'This Mobile no will be used for Administrative Notifications like Access Autenthication and Daily Status. ',
                html: true
            });
            $("#EmailInfo").tooltip({
                title: 'This Email will be used for Administrative Notifications like Access Autenthication and Daily Status.',
                html: true
            });
            $("#Workshopinfo").tooltip({
                title: 'It will work only if you have Workshop Module.',
                html: true
            });
            $('#btnSave,#btnUpdate').click(function () {
                clearMsg();
                var sEmail = $('#txtEmailId').val();
                var brcode = document.getElementById("<%=txtBranchCode.ClientID %>").value.trim().length;
                var brname = document.getElementById("<%=txtBranchName.ClientID %>").value.trim().length;
                var bradd = document.getElementById("<%=txtBranchAddress.ClientID %>").value.trim().length;
                var brMob = document.getElementById("<%=txtMobileNo.ClientID %>").value.trim().length;
                var emailId = document.getElementById("<%=txtEmailId.ClientID %>").value.trim().length;
                if (brcode == "" || brcode.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter store code.');
                    document.getElementById("<%=txtBranchCode.ClientID %>").focus();
                    return false;
                }
                if (brname == "" || brname.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter store name.');
                    document.getElementById("<%=txtBranchName.ClientID %>").focus();
                    return false;
                }
                if (bradd == "" || bradd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter store address.');
                    document.getElementById("<%=txtBranchAddress.ClientID %>").focus();
                    return false;
                }
                if (brMob == "" || brMob.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter store mobile no.');
                    document.getElementById("<%=txtMobileNo.ClientID %>").focus();
                    return false;
                }
                if (emailId == "" || emailId.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter store email id.');
                    document.getElementById("<%=txtEmailId.ClientID %>").focus();
                    return false;
                }
                if ($.trim(sEmail).length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter valid email address.');
                    $('#txtEmailId').focus();
                    return false;
                }
                if (validateEmail(sEmail)) {
                }
                else {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter valid email id.');
                    $('#txtEmailId').focus();
                    return false;
                }

                var clickedId = $(this).attr("id");
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                if (clickedId == 'btnUpdate') {

                }
                if ($('#chkOperatingTime').is(':checked')) {
                    var Istime2 = CheckTime();
                    if (Istime2 == "False") {
                        $('#txtStartTime').focus();
                        return false;
                    }
                }
            });


            $('#ctl00_ContentPlaceHolder1_chkChallan').click(function (e) {
                ShowText();
                if ($('#ctl00_ContentPlaceHolder1_chkChallan').is(':checked')) {
                    $.ajax({
                        url: '../AutoComplete.asmx/CheckFectory',
                        type: 'GET',
                        timeout: 20000,
                        contentType: 'application/json; charset=UTF-8',
                        datatype: 'JSON',
                        cache: true,
                        async: false,
                        success: function (response) {
                            var strData = response.d;
                            if (strData != "True") {
                                setDivMouseOver('#FA8602', '#999999');
                                $('#lblMsg').text("You don't have Workshop Module, this functionality can't be used.");
                                $('#ctl00_ContentPlaceHolder1_chkChallan').attr('checked', false)
                                return false;
                            }
                        },
                        error: function (response) {
                        }
                    });
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

        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }

        function clearMsg() {
            $('#lblErr').text('');
            $('#lblMsg').text('');
        }

        function ShowText() {
            if ($('#ctl00_ContentPlaceHolder1_chkChallan').is(':checked')) {
                $('#WorkShopTxt').text("I will send garments for processing to Workshop.");
                $('#WorkShopTxt').removeClass('txtColor');

            } else {
                $('#WorkShopTxt').text("I will process garments in this store only.");
                $('#WorkShopTxt').addClass('txtColor');
            }
        }

        function ShowTimeControl() {
            if ($('#chkOperatingTime').is(':checked')) {
                $('#divTiming').show();
            } else {
                $('#divTiming').hide();
            }
        }

        function CheckTime(strCheck) {
            var fromTime = $('#txtStartTime').val().trim();
            var ToTime = $('#txtEndTime').val().trim();
            $.ajax({
                url: '../AutoComplete.asmx/CheckCompareTime',
                type: 'GET',
                data: "startTime='" + fromTime + "'&EndTime='" + ToTime + "'",
                timeout: 20000,
                contentType: 'application/json; charset=UTF-8',
                datatype: 'JSON',
                cache: true,
                async: false,
                success: function (response) {
                    var _val = response.d;
                    if (_val == "False") {
                        clearMsg();
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text("End date can't smaller or equal than start date");
                        strCheck ="False";
                    }
                    else {
                        document.getElementById('divShowMsg').style.display = "none";
                    }
                },
                error: function (response) {
                    alert(response.toString())
                }
            });
            return strCheck;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function (e) {


            $('#chkOperatingTime').click(function (e) {
                ShowTimeControl();
                if ($('#chkOperatingTime').is(':checked')) {
                    $('#txtStartTime').focus();
                }
            });

            $('#txtStartTime,#txtEndTime').timepicker({
                minuteStep: 1,
                showInputs: false,
                disableFocus: false
                //showSeconds: true
            });

            $('#txtEndTime,#txtStartTime').change(function (e) {  
                var Istime = CheckTime();
                if (Istime == "False") {
                    return false;
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="textmargin">
                    <span style="margin-left: 40%">
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                            ForeColor="White" /></span> <span style="margin-left: -20%"></span>
                </h4>
            </div>
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="row-fluid">
            <div class="row-fluid">
                <div class="col-sm-7">
                    <div class="panel panel-primary ">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Store Details</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid div-margin">
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">Code</span>
                                    <asp:TextBox ID="txtBranchCode" runat="server" CssClass="form-control" placeholder="Kindly enter Store Code"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <span class="textRed">&nbsp;*</span>
                                </div>
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-building-o fa-lg"></i>
                                    </span>
                                    <asp:TextBox ID="txtBranchName" runat="server" MaxLength="15" CssClass="form-control"
                                        placeholder="Kindly enter Store Name" />
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <span class="textRed">&nbsp;*</span>
                                </div>
                            </div>
                            <div class="row-fluid div-margin">
                                <div class="col-sm-11 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">Business Name</span>
                                    <asp:TextBox ID="txtBusinessName" runat="server" MaxLength="50" CssClass="form-control"
                                        placeholder="Kindly enter Business name" />
                                </div>
                            </div>
                            <div class="row-fluid div-margin">
                                <div class="col-sm-11 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-home fa-lg"></i></span>
                                    <asp:TextBox ID="txtBranchAddress" runat="server" MaxLength="100" CssClass="form-control"
                                        placeholder="Kindly enter store address"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <span class="textRed">&nbsp;*</span>
                                </div>
                            </div>
                            <div class="row-fluid div-margin">
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-phone fa-lg"></i></span>
                                    <asp:TextBox ID="txtMobileNo" placeholder="Kindly enter mobile no" runat="server"
                                        CssClass="form-control" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                        FilterType="Numbers" TargetControlID="txtMobileNo">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <div class="col-sm-1 Textpadding form-inline">
                                    <div class="form-group">
                                        <span class="span textBold">&nbsp;*</span>
                                    </div>
                                    <div class="form-group" style="margin-top: -9px">
                                        &nbsp;<i id="MobileInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                    </div>
                                </div>
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-envelope-o fa-lg"></i>
                                    </span>
                                    <asp:TextBox ID="txtEmailId" runat="server" placeholder="Kindly enter email id" CssClass="form-control"
                                        ClientIDMode="Static" />
                                </div>
                                <div class="col-sm-1 Textpadding form-inline">
                                    <div class="form-group">
                                        <span class="span textBold">&nbsp;*</span>
                                    </div>
                                    <div class="form-group" style="margin-top: -9px">
                                        &nbsp;<i id="EmailInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid div-margin" style="display: none">
                                <div class="col-sm-11 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">Slogan</span>
                                    <asp:TextBox ID="txtBranchSlogan" placeholder="Kindly enter slogan" runat="server"
                                        MaxLength="50" CssClass="form-control" />
                                </div>
                            </div>                           
                            <div class="row-fluid div-margin">
                                <div class="col-sm-1 Textpadding">
                                    <input type="checkbox" class="ace ace-switch ace-switch-5" checked="checked" runat="server"
                                        id="chkChallan" />
                                    <span class="lbl"></span>
                                </div>
                                <div class="col-sm-11 Textpadding ">
                                    <div class="form-inline">
                                        <div class="form-group">
                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="WorkShopTxt" runat="server" ClientIDMode="Static">
                                            </asp:Label>
                                        </div>
                                        <div class="form-group">
                                            &nbsp;<i id="Workshopinfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false"
                                ClientIDMode="Static" />
                            <asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" class="btn btn-info"
                                EnableTheming="false" ClientIDMode="Static"><i class="fa fa-check-square-o"> </i>&nbsp;Update</asp:LinkButton>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return checkEntry();"
                                OnClick="btnDelete_Click" Visible="false" />
                            <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure delete this branch ?"
                                Enabled="True" TargetControlID="btnDelete">
                            </cc1:ConfirmButtonExtender>
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="panel panel-primary ">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Store Operation Time</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid div-margin">                               
                                <div class="col-sm-9 Textpadding">
                                    <span>Do you want to enable store operation timing feature</span>
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <input type="checkbox" class="ace ace-switch ace-switch-5" checked="checked" runat="server"
                                        clientidmode="Static" id="chkOperatingTime" />
                                    <span class="lbl"></span>
                                </div>
                            </div>
                            <div class="row-fluid div-margin" id="divTiming" runat="server" clientidmode="Static">
                                <div class="row-fluid">
                                    <div class="col-sm-5 input-group  Textpadding">
                                        <span class="input-group-addon IconBkColor">Opening</span>
                                        <asp:TextBox ID="txtStartTime" runat="server" MaxLength="50" CssClass="form-control"
                                            ClientIDMode="Static" placeholder="Opening time" onpaste="return false;" onkeypress="return false;" />
                                        <span class="input-group-addon IconBkColor"><i class="fa fa-calendar fa-lg"></i>
                                        </span>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-5 input-group Textpadding">
                                        <span class="input-group-addon IconBkColor">Closing</span>
                                        <asp:TextBox ID="txtEndTime" onpaste="return false;" onkeypress="return false;" runat="server"
                                            MaxLength="50" CssClass="form-control" ClientIDMode="Static" placeholder="Closing time" />
                                        <span class="input-group-addon IconBkColor"><i class="fa fa-calendar fa-lg"></i>
                                        </span>
                                    </div>
                                </div>
                                <div class="row-fluid div-margin">
                                <div class="col-sm-11 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">Weekly Off</span>
                                    <asp:DropDownList ID="drpWeekend" runat="server" EnableTheming="false" CssClass="form-control">
                                        <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Sun" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Mon" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Tue" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Wed" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Thu" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="Fri" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="Sat" Value="7"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                                <div class="row-fluid">
                                    <br />
                                    <span>This will make software accessible only at the time specified above, also it will
                                        count Weekly Off and Holiday specified. </span>
                                        <span>
                                        <br />
                                      Please Note : Admin will always have unrestricted access.
                                        </span>
                                </div>
                                  
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny">
            <div style="overflow: auto; max-height: 350px">
                <asp:GridView ID="grdSearchResult" runat="server" DataKeyNames="BranchId" AutoGenerateColumns="False"
                    EmptyDataText="There are no data records to display." OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                    EnableTheming="false" CssClass="Table Table-striped Table-bordered Table-hover">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <%--<asp:BoundField DataField="BranchId" HeaderText="ID" />--%>
                        <%--<asp:BoundField DataField="BranchCode" HeaderText="Code" />--%>
                        <%--<asp:BoundField DataField="Type" HeaderText="Type" />--%>
                        <asp:BoundField DataField="BranchName" HeaderText="Name" />
                        <asp:BoundField DataField="BusinessName" HeaderText="Business Name" />
                        <asp:BoundField DataField="BranchAddress" HeaderText="Address" />
                        <asp:BoundField DataField="Challan" HeaderText="Sent to Workshop" />
                        <asp:BoundField DataField="BranchSlogan" HeaderText="Slogan" Visible="false" />
                        <asp:BoundField DataField="BranchMobile" HeaderText="Mobile" />
                        <asp:BoundField DataField="BranchEmail" HeaderText="Email" />
                        <asp:BoundField DataField="IsLoginTime" HeaderText="Operating Timing" />
                    </Columns>
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
            <asp:RadioButton ID="rdrBranch" runat="server" Checked="True" GroupName="A" Text="Store"
                OnCheckedChanged="rdrBranch_CheckedChanged" AutoPostBack="true" Visible="false" />&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="rdrFactory" runat="server" GroupName="A" Text="Workshop" OnCheckedChanged="rdrFactory_CheckedChanged"
                AutoPostBack="true" Visible="false" />
        </div>
    </div>
</asp:Content>
