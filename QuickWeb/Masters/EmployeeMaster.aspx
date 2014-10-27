<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="EmployeeMaster" Title="Employee Master" CodeBehind="EmployeeMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/MaxLength.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function checkName() {
            clearMsg();
            var strname = document.getElementById("<%=txtCustomerName.ClientID %>").value.trim().length;
            var stradd = document.getElementById("<%=txtCustomerAddress.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Kindly enter Employee Name');
                document.getElementById("<%=txtCustomerName.ClientID %>").focus();
                return false;
            }
            if (stradd == "" || stradd.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Kindly enter Employee Address');
                document.getElementById("<%=txtCustomerAddress.ClientID %>").focus();
                return false;
            }
            var sEmail = $('#ctl00_ContentPlaceHolder1_txtCustomerEmailId').val().trim();
            if (sEmail != "") {
                if (validateEmail(sEmail)) {
                }
                else {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter valid email id.');
                    $('#ctl00_ContentPlaceHolder1_txtCustomerEmailId').focus();
                    return false;
                }
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

        function validateEmail(sEmail) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(sEmail)) {
                return true;
            }
            else {
                return false;
            }
        }

    </script>
    <script type="text/javascript">
        $(function () {

            //Disable Character Count
            $("[id*=txtCustomerAddress]").MaxLength(
            {
                MaxLength: 100,
                DisplayCharacterCount: false
            });
        });
    </script>
    <style type="text/css">
        .style1
        {
            color: #5E6265;
            font-family: verdana,Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: right;
            height: 27px;
            font-weight: bolder;
            white-space: nowrap;
        }
        .style2
        {
            color: #5E6265;
            font-family: verdana,Arial, Helvetica, sans-serif;
            font-size: 14;
            text-align: left;
            height: 27px;
            font-weight: bolder;
        }
        .style3
        {
            height: 27px;
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
        <div class="col-sm-6">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Employee</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="input-group input-group-sm">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                            <div class="col-sm-12 Textpadding">
                                <div class="col-sm-2 Textpadding">
                                    <asp:DropDownList ID="drpCustSalutation" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="Mr"></asp:ListItem>
                                        <asp:ListItem>Mrs</asp:ListItem>
                                        <asp:ListItem>Ms</asp:ListItem>
                                        <asp:ListItem>Dr</asp:ListItem>
                                        <asp:ListItem>M/S</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-9 Textpadding">
                                    <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="30" placeholder="Kindly enter employee name"
                                        CssClass="form-control" />
                                </div>
                                <div class="col-sm-1 Textpadding">
                                    <span class="textRed">&nbsp;*</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-building-o fa-lg"></i>
                                </span>
                                <asp:TextBox ID="txtCustomerAddress" runat="server" TextMode="MultiLine" placeholder="Kindly enter address"
                                    MaxLength="100" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding form-inline">
                            <div class="form-group">
                                <span class="span textBold">&nbsp;&nbsp;*</span>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding padding4">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-phone fa-lg"></i></span>
                                <asp:TextBox ID="txtCustomerMobile" runat="server" MaxLength="10" CssClass="form-control"
                                    placeholder="Kindly enter mobile no" />
                                <cc1:FilteredTextBoxExtender ID="txtCustomerMobile_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerMobile">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-6 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-envelope-o fa-lg"></i>
                                </span>
                                <asp:TextBox ID="txtCustomerEmailId" runat="server" MaxLength="100" placeholder="Kindly enter Email id"
                                    CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnSave" class="btn btn-info" runat="server" OnClick="btnSave_Click"
                        EnableTheming="false" OnClientClick="return checkName();"><i class="fa fa-floppy-o"></i>&nbsp;Save</asp:LinkButton>
                    <asp:LinkButton ID="btnUpdate" class="btn btn-info" EnableTheming="false" runat="server"
                        OnClientClick="return checkName();" OnClick="btnUpdate_Click" Visible="false"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" class="btn btn-info" EnableTheming="false" runat="server"
                        Visible="false" OnClick="btnDeleteCustomer_Click"><i class="fa fa-trash-o"></i>&nbsp;Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:TextBox ID="txtCustomerPhone" runat="server" MaxLength="10" Visible="false" />
    <cc1:FilteredTextBoxExtender ID="txtCustomerPhone_FilteredTextBoxExtender" runat="server"
        Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerPhone">
    </cc1:FilteredTextBoxExtender>
    <div class="row-fluid">
        <div class="well well-sm-tiny" style="overflow: auto; height: 320px;">
            <asp:GridView ID="grdSearchResult" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                DataKeyNames="ID,EmployeeCode,EmployeeName" OnRowCommand="grdSearchResult_OnRowCommand"
                OnPageIndexChanging="grdSearchResult_PageIndexChanging">
                <Columns>
                    <%--<asp:TemplateField HeaderText="Code" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnCode" runat="server" CausesValidation="false" CommandName="SelectCustomer"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("EmployeeCode") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Name" ShowHeader="False">
                        <ItemTemplate>
                          <asp:HiddenField ID="lnkbtnCode" runat="server" Value='<%# Bind("EmployeeCode") %>' />
                            <asp:LinkButton ID="lnkbtnName" runat="server" CausesValidation="false" CommandName="SelectCustomer"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("EmployeeName") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnAddress" runat="server" CausesValidation="false" CommandName="SelectCustomer"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("EmployeeAddress") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="Phone" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnPhone" runat="server" CausesValidation="false" CommandName="SelectCustomer"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("EmployeePhone") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Mobile" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnMobile" runat="server" CausesValidation="false" CommandName="SelectCustomer"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("EmployeeMobile") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnEmailId" runat="server" CausesValidation="false" CommandName="SelectCustomer"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("EmployeeEmailId") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
    <asp:Label runat="server" ID="lblCustomerCode" Style="display: none"></asp:Label>
</asp:Content>
