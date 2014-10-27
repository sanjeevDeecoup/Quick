<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true" CodeBehind="frmWorkshopDetails.aspx.cs" Inherits="QuickWeb.Factory.frmWorkshopDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
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
            $("#MobileInfo").tooltip({
                title: 'This Mobile no will be used for Administrative Notifications like Access Autenthication ',
                html: true
            });
            $("#EmailInfo").tooltip({
                title: 'This Email will be used for Administrative Notifications like Access Autenthication',
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
                    $('#lblMsg').text('Kindly enter workshop code.');
                    document.getElementById("<%=txtBranchCode.ClientID %>").focus();
                    return false;
                }
                if (brname == "" || brname.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter workshop name.');
                    document.getElementById("<%=txtBranchName.ClientID %>").focus();
                    return false;
                }
                if (bradd == "" || bradd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter workshop address.');
                    document.getElementById("<%=txtBranchAddress.ClientID %>").focus();
                    return false;
                }
                if (brMob == "" || brMob.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter workshop mobile no.');
                    document.getElementById("<%=txtMobileNo.ClientID %>").focus();
                    return false;
                }
                if (emailId == "" || emailId.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter workshop email id.');
                    document.getElementById("<%=txtEmailId.ClientID %>").focus();
                    return false;
                }
                if ($.trim(sEmail).length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter valid email address.');
                    alert('');
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
        }
        function clearMsg() {
            $('#lblErr').text('');
            $('#lblMsg').text('');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                Workshop Details</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid div-margin">
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">Code</span>
                                    <asp:TextBox ID="txtBranchCode" runat="server" CssClass="form-control" placeholder="Kindly enter workshop Code"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <span class="textRed">&nbsp;*</span>
                                </div>
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-building-o fa-lg"></i>
                                    </span>
                                    <asp:TextBox ID="txtBranchName" runat="server" MaxLength="15" CssClass="form-control"
                                        placeholder="Kindly enter workshop Name" />
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
                                        placeholder="Kindly enter workshop address"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <span class="textRed">&nbsp;*</span>
                                </div>
                            </div>
                            <div class="row-fluid div-margin">
                                <div class="col-sm-5 input-group input-group-sm Textpadding">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-phone fa-lg"></i></span>
                                    <asp:TextBox ID="txtMobileNo" placeholder="Kindly enter mobile no" runat="server"  MaxLength="11"
                                        CssClass="form-control" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                        FilterType="Numbers" TargetControlID="txtMobileNo">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <div class="col-sm-1 Textpadding form-inline">
                                    <div class="form-group">
                                        <span class="span textBold  textRed">&nbsp;*</span>
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
                                        <span class="span textBold textRed">&nbsp;*</span>
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
                        <%--<asp:BoundField DataField="Challan" HeaderText="Sent to Workshop" />--%>
                        <asp:BoundField DataField="BranchSlogan" HeaderText="Slogan" Visible="false" />
                        <asp:BoundField DataField="BranchMobile" HeaderText="Mobile" />
                        <asp:BoundField DataField="BranchEmail" HeaderText="Email" />
                    </Columns>
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
            <asp:RadioButton ID="rdrBranch" runat="server" Checked="True" GroupName="A" Text="Store"
                Visible="false" />&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="rdrFactory" runat="server" GroupName="A" Text="Workshop" 
                Visible="false" />
        </div>
    </div>
</asp:Content>
