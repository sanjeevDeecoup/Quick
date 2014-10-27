<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmReceipt.aspx.cs" Inherits="QuickWeb.New_Admin.frmReceipt" EnableEventValidation="false"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tooltip-inner
        {
            max-width: 300px;
            width: 300px;
            background-color: #F1F1F1;
            text-align: left;
            font-size: 14px;
            color: Black;
            border: 1px solid #C0C0A0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <div id="divReceiptMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="divReceiptInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                        ClientIDMode="Static" />
                    <asp:Label ID="lblReceiptErr" runat="server" EnableViewState="False" Font-Bold="True"
                        ClientIDMode="Static" ForeColor="White" />
                </h4>
            </div>
        </div>
        <div id="divDefaultSetting" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="divDefaultSettingInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblDefaultSucess" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="White" ClientIDMode="Static" />
                    <asp:Label ID="lblDefaultErr" runat="server" EnableViewState="False" Font-Bold="True"
                        ClientIDMode="Static" ForeColor="White" />
                </h4>
            </div>
        </div>
        <div id="divServiceTaxMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="divServiceTaxInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblErrServiceTax" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="White" ClientIDMode="Static" />
                    <asp:Label ID="lblSucessServiceTax" runat="server" EnableViewState="False" Font-Bold="True"
                        ClientIDMode="Static" ForeColor="White" />
                </h4>
            </div>
        </div>
        <div id="divEmailMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="divEmailInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblEmailSuccess" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="White" ClientIDMode="Static" />
                    <asp:Label ID="lblEmailError" runat="server" EnableViewState="False" Font-Bold="True"
                        ClientIDMode="Static" ForeColor="White" />
                </h4>
            </div>
        </div>
        <div id="divTimeZoneMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="divTimeZoneMsgInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblSetTimeZoneErr" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="White" ClientIDMode="Static" />
                    <asp:Label ID="lblSetTimeZoneSuccess" runat="server" EnableViewState="False" Font-Bold="True"
                        ClientIDMode="Static" ForeColor="White" />
                </h4>
            </div>
        </div>
        <div id="divStoreMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="divStoreInnerStatus" class="span label label-default">
                <h4 class="text-center">
                    <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="White" ClientIDMode="Static" />
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                        ForeColor="White" />
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary well-sm-tiny1">
        <div class="panel-heading">
            <h3 class="panel-title">
                Configuration setting&nbsp;-&nbsp; <span style="font-size: 15px">
                    <asp:Label ID="lblHeaderTextMsg" runat="server" ClientIDMode="Static" />
                </span>
            </h3>
        </div>
        <div class="panel-body well-sm" style="min-height: 500px">
            <div class="col-sm-12 Textpadding">
                <fieldset>
                    <table>
                        <tr>
                            <td>
                                <div class="col-sm-12">
                                    <div class="row-fluid">
                                        <div style="width: 180px">
                                            <a class="btn btn-primary  btn-block" id="btnSetTimeZone" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp<i class="fa fa-adjust fa-lg"></i>&nbsp;&nbsp;&nbsp;Set Time Zone</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnDefaultSetting" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp<i class="fa fa-list-ol fa-lg"></i>&nbsp;&nbsp;&nbsp;Pick Up/New Order</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnDispaly" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp<i class="fa fa-building-o fa-lg"></i>&nbsp;&nbsp;&nbsp;Store Information</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnReceipt" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp<i class="fa fa-print fa-lg"></i>&nbsp;&nbsp;&nbsp;Receipt</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin" style="display: none">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnGeneralSetting" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp;<i class="fa fa-th fa-lg"></i>&nbsp;&nbsp;&nbsp;Miscellaneous</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnSetEmail" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp;<i class="fa fa-envelope-o fa-lg"></i>&nbsp;&nbsp;&nbsp;EMail</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin" style="display: none">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnBackUp" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp;<i class="fa fa-download fa-lg"></i>&nbsp;&nbsp;&nbsp;Backup Path</div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div>
                                            <a class="btn btn-primary  btn-block" id="btnServiceTac" runat="server" enabletheming="false">
                                                <div style="text-align: left">
                                                    &nbsp;&nbsp;<i class="fa fa-usd fa-lg"></i>&nbsp;&nbsp;&nbsp;&nbsp;Default Tax Rates</div>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td id="tdReceipt" runat="server" style="width: 100%">
                                <cc1:TabContainer ID="tbReceipt" runat="server" ActiveTabIndex="0" CssClass="">
                                    <cc1:TabPanel ID="TabPanel1" runat="server">
                                        <ContentTemplate>
                                            <fieldset class="Fieldset">
                                                <div class="row-fluid">
                                                    <div class="row-fluid">
                                                        <div class="col-sm-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Printer configuration - Order <i id="OrderInfo" style="float: right" data-placement="right"
                                                                            class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </h3>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row-fluid">
                                                                        <div class="input-group  labelBorder">
                                                                            <span class="input-group-addon">Receipt</span>
                                                                            <div class="div-margin3">
                                                                                &nbsp;&nbsp;<label>
                                                                                    <input name="p" id="rdrLaser" type="radio" runat="server" checked="True" class="ace" />
                                                                                    <span class="lbl" style="font-size: 13px">&nbsp;Laser (8.5 * 5.5 Inches)</span>
                                                                                </label>
                                                                                <asp:Button ID="btntmpLaser" runat="server" OnClick="rdrLaser_CheckedChanged" ClientIDMode="Static"
                                                                                    Style="display: none" />
                                                                                <%--  <asp:RadioButton ID="rdrLaser" runat="server" Checked="True" GroupName="p" Text="&nbsp;Laser (8.5 * 5.5 Inches)"
                                                                                CssClass="TDCaption" AutoPostBack="True" OnCheckedChanged="rdrLaser_CheckedChanged" />--%>
                                                                                <%--  &nbsp;&nbsp;<asp:RadioButton ID="rdrDotMatrix" runat="server" GroupName="p" Text="&nbsp;Thermal (80 mm Roll Paper)"
                                                                                CssClass="TDCaption" AutoPostBack="True" OnCheckedChanged="rdrDotMatrix_CheckedChanged" />--%>
                                                                                &nbsp;&nbsp;<label>
                                                                                    <input name="p" id="rdrDotMatrix" type="radio" runat="server" class="ace" />
                                                                                    <span class="lbl" style="font-size: 13px">&nbsp;Thermal (80 mm Roll Paper)</span>
                                                                                </label>
                                                                                <asp:Button ID="btnTmpDotMatrix" runat="server" OnClick="rdrDotMatrix_CheckedChanged"
                                                                                    ClientIDMode="Static" Style="display: none" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin4">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Default printer</span>
                                                                            <asp:DropDownList ID="drpDefaultPrinter" runat="server" CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid">
                                                                        <div class="div-margin3">
                                                                            <div id="trStoreCopy" runat="server">
                                                                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                    checked="" runat="server" id="chkStoreCopy" />
                                                                                <span class="lbl"></span>
                                                                                <asp:Label ID="PrintStoreCopyInfo" runat="server" ClientIDMode="Static"></asp:Label>
                                                                            </div>
                                                                            <%--<asp:CheckBox ID="chkStoreCopy" runat="server" CssClass="Legend" Text="&nbsp;Select to Print additional Store Copy of Invoice" />--%>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Printer Configuration - Delivery <i id="DeliveryInfo" style="float: right" data-placement="left"
                                                                            class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </h3>
                                                                    </h3>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row-fluid">
                                                                        <div class="input-group labelBorder">
                                                                            <span class="input-group-addon">Delivery</span>
                                                                            <div class="div-margin3">
                                                                                &nbsp;&nbsp;<label>
                                                                                    <input name="DelPrint" id="rdrDeliveryLaser" type="radio" runat="server" checked="True"
                                                                                        class="ace" />
                                                                                    <span style="font-size: 13px" class="lbl">&nbsp;Laser (8.5 * 5.5 Inches)</span>
                                                                                </label>
                                                                                &nbsp;&nbsp;<label>
                                                                                    <input name="DelPrint" id="rdrDeliveryDotMatrix" type="radio" runat="server" class="ace" />
                                                                                    <span style="font-size: 13px" class="lbl">&nbsp;Thermal (80 mm Roll Paper)</span>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin4">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Default printer</span>
                                                                            <asp:DropDownList ID="drpDeliveryPrinter" runat="server" CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <asp:Label ID="lblPrinter" runat="server" CssClass="TDDot" Visible="False"></asp:Label>
                                                        <asp:RadioButton ID="rdbDotMatrix40" runat="server" Text=" Dotmatrix(80 column)"
                                                            CssClass="Legend" Visible="False" GroupName="fgh" Checked="True" />
                                                        <asp:RadioButton ID="rdb3Inches" runat="server" Text=" Dotmatrix (3 Inches)" CssClass="Legend"
                                                            Visible="False" GroupName="fgh" />
                                                        <div>
                                                            <asp:RadioButton ID="rdrThermal" AutoPostBack="True" GroupName="p" Text=" DotMatrix (80 mm Roll Paper)"
                                                                CssClass="TDCaption" runat="server" OnCheckedChanged="rdrThermal_CheckedChanged"
                                                                Visible="false" />
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="col-sm-12">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Header Configuration</h3>
                                                                </div>
                                                                <div class="panel-body well-sm-tiny">
                                                                    <div class="row-fluid">
                                                                        <div class="col-sm-12">
                                                                            <div class="div-margin3">
                                                                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                    checked="" runat="server" id="rdrLogoAndTest" />
                                                                                <span class="lbl"></span>
                                                                                <asp:Label ID="StoreCopyInfo" runat="server" ClientIDMode="Static"></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                        <%--   <asp:RadioButton ID="rdrLogoAndTest" runat="server" AutoPostBack="True" Checked="True"
                                                                        CssClass="Legend" GroupName="a" OnCheckedChanged="rdrLogoAndTest_CheckedChanged"
                                                                        Text="&nbsp;Create Your Header" />--%>
                                                                        <%--<asp:RadioButton ID="rdrBanner" runat="server" CssClass="Legend" GroupName="a" OnCheckedChanged="rdrBanner_CheckedChanged"
                                                                        Text="&nbsp;Pre Printed" AutoPostBack="True" />--%>
                                                                    </div>
                                                                    <div class="row-fluid">
                                                                        <div id="tblLogo" runat="server" class="div-margin" style="margin-top: 15px">
                                                                            <div class="row-fluid" id="Tr2" runat="server">
                                                                                <div class="col-sm-4">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">Store Name</span>
                                                                                        <asp:TextBox ID="txtName" runat="server" Height="35px" CssClass="form-control" MaxLength="100"
                                                                                            placeholder="Store name" TextMode="MultiLine"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName" visible="false">
                                                                                                Font
                                                                                            </div>
                                                                                            <div runat="server" id="tdName1" visible="false">
                                                                                            </div>
                                                                                        </span>
                                                                                        <div runat="server" id="tdName2" visible="false">
                                                                                            <asp:DropDownList ID="drpFontName" runat="server" CssClass="form-control">
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-2">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName3" visible="false">
                                                                                                Size</div>
                                                                                            <div runat="server" id="tdName4" visible="false">
                                                                                            </div>
                                                                                        </span>
                                                                                        <div runat="server" id="tdName5" visible="false">
                                                                                            <asp:TextBox ID="TxtHdrFontsize" runat="server" CssClass="input-mini" ClientIDMode="Static"
                                                                                                onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                                                                            <%-- <asp:DropDownList ID="drpFontsize" runat="server" CssClass="form-control" >
                                                                                                <asp:ListItem Selected="True" Text="8" />
                                                                                                <asp:ListItem Text="9" />
                                                                                                <asp:ListItem Text="10" />
                                                                                                <asp:ListItem Text="11" />
                                                                                                <asp:ListItem Text="12" />
                                                                                                <asp:ListItem Text="13" />
                                                                                                <asp:ListItem Text="14" />
                                                                                                <asp:ListItem Text="15" />
                                                                                                <asp:ListItem Text="16" />
                                                                                                <asp:ListItem Text="17" />
                                                                                                <asp:ListItem Text="18" />
                                                                                                <asp:ListItem Text="19" />
                                                                                                <asp:ListItem Text="20" />
                                                                                                <asp:ListItem Text="21" />
                                                                                                <asp:ListItem Text="22" />
                                                                                                <asp:ListItem Text="23" />
                                                                                                <asp:ListItem Text="24" />
                                                                                                <asp:ListItem Text="25" />
                                                                                                <asp:ListItem Text="26" />
                                                                                                <asp:ListItem Text="27" />
                                                                                                <asp:ListItem Text="28" />
                                                                                                <asp:ListItem Text="29" />
                                                                                                <asp:ListItem Text="30" />
                                                                                            </asp:DropDownList>--%>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-3">
                                                                                    <div class="input-group labelBorder" style="height: 34px">
                                                                                        <span class="input-group-addon">
                                                                                            <div nowrap="nowrap" runat="server" id="tdName6" visible="false">
                                                                                                Style
                                                                                            </div>
                                                                                            <div runat="server" id="tdName7" visible="false">
                                                                                            </div>
                                                                                        </span>
                                                                                        <div class="div-margin" runat="server" id="tdName8" visible="false">
                                                                                            &nbsp;<asp:CheckBox ID="chkNameBold" Text="&nbsp;B" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                            <asp:CheckBox ID="chkNameItalic" Text="&nbsp;I" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                            <asp:CheckBox ID="chkNameUL" Text="&nbsp;U" runat="server" CssClass="TDCaption" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row-fluid div-margin" id="Tr3" runat="server">
                                                                                <div class="col-sm-4">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">
                                                                                            <div id="Td10" runat="server">
                                                                                                Store Address
                                                                                            </div>
                                                                                        </span>
                                                                                        <div id="Tr1" runat="server">
                                                                                            <asp:TextBox ID="txtLogoAddress" runat="server" CssClass="form-control" Height="35px"
                                                                                                placeholder="Store address" MaxLength="100" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName9" visible="false">
                                                                                                Font
                                                                                            </div>
                                                                                        </span>
                                                                                        <div id="tdName10" visible="false" runat="server">
                                                                                        </div>
                                                                                        <div runat="server" id="tdName11" visible="false">
                                                                                            <asp:DropDownList ID="drpAFontName" runat="server" CssClass="form-control">
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-2">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName12" visible="false">
                                                                                                Size
                                                                                            </div>
                                                                                        </span>
                                                                                        <div runat="server" id="tdName13" visible="false">
                                                                                        </div>
                                                                                        <div runat="server" id="tdName14" visible="false">
                                                                                            <asp:TextBox ID="txtAddFontSize" runat="server" CssClass="input-mini" ClientIDMode="Static"
                                                                                                onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                                                                            <%--   <asp:DropDownList ID="drpAFontSize" runat="server" CssClass="form-control">
                                                                                                <asp:ListItem Selected="True" Text="8" />
                                                                                                <asp:ListItem Text="9" />
                                                                                                <asp:ListItem Text="10" />
                                                                                                <asp:ListItem Text="11" />
                                                                                                <asp:ListItem Text="12" />
                                                                                                <asp:ListItem Text="13" />
                                                                                                <asp:ListItem Text="14" />
                                                                                                <asp:ListItem Text="15" />
                                                                                                <asp:ListItem Text="16" />
                                                                                            </asp:DropDownList>--%>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-3">
                                                                                    <div class="input-group labelBorder" style="height: 34px">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName15" visible="false">
                                                                                                Style</div>
                                                                                            <div runat="server" id="tdName16" visible="false">
                                                                                            </div>
                                                                                        </span>
                                                                                        <div class="div-margin" runat="server" id="tdName17" visible="false">
                                                                                            &nbsp;<asp:CheckBox ID="chkAddressBold" Text="&nbsp;B" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                            <asp:CheckBox ID="chkAddressItalic" Text="&nbsp;I" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                            <asp:CheckBox ID="chkAddressUL" Text="&nbsp;U" runat="server" CssClass="TDCaption" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row-fluid " runat="server" id="tdName42" visible="false">
                                                                            </div>
                                                                            <div class="row-fluid div-margin">
                                                                                <div class="col-sm-4">
                                                                                    <div class="div-margin3">
                                                                                        <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                            checked="" runat="server" id="rdbShowOnReceiptTrue" />
                                                                                        <span class="lbl"></span>
                                                                                        <asp:Label ID="lblLogoText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                                    </div>
                                                                                    <%-- <div class="input-group labelBorder" style="height: 34px">
                                                                                <span class="input-group-addon IconBkColor">Print Logo</span>
                                                                                <div class="div-margin3">
                                                                                    &nbsp;<asp:RadioButton ID="rdbShowOnReceiptTrue" runat="server" GroupName="afghj"
                                                                                        Text="&nbsp;Yes" Checked="True" CssClass="TDCaption" />&nbsp;&nbsp;&nbsp;
                                                                                    <asp:RadioButton ID="rdbShowOnReceiptFalse" runat="server" GroupName="afghj" Text="&nbsp;No"
                                                                                        CssClass="TDCaption" /></div>
                                                                            </div>--%>
                                                                                </div>
                                                                                <div class="col-sm-4">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName36" visible="false">
                                                                                                Logo</div>
                                                                                            <div runat="server" id="tdName37" visible="false">
                                                                                            </div>
                                                                                        </span>
                                                                                        <div runat="server" id="tdName38" visible="false">
                                                                                            <input id="fupLogo" runat="server" onchange="return fupLogo_onchange();" size="10"
                                                                                                type="file" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div runat="server" id="tdName40" visible="false">
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-1 Textpadding">
                                                                                    &nbsp;<i id="StoreLogoInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                                </div>
                                                                                <div class="col-sm-3">
                                                                                    <div class="input-group labelBorder" style="height: 34px">
                                                                                        <span class="input-group-addon">
                                                                                            <div runat="server" id="tdName39" visible="false">
                                                                                                Align
                                                                                            </div>
                                                                                        </span>
                                                                                        <div class="div-margin3" runat="server" id="tdName41" visible="false">
                                                                                            <div class="div-margin3">
                                                                                                &nbsp;&nbsp;<label>
                                                                                                    <input name="b" id="rdrLAlign" type="radio" runat="server" checked="True" class="ace" />
                                                                                                    <span class="lbl">&nbsp;Left</span>
                                                                                                </label>
                                                                                                &nbsp;&nbsp;<label>
                                                                                                    <input name="b" id="rdrRAlign" type="radio" runat="server" class="ace" />
                                                                                                    <span class="lbl">&nbsp;Right</span>
                                                                                                </label>
                                                                                            </div>
                                                                                            <%--<asp:RadioButton ID="rdrLAlign" runat="server" Checked="True" CssClass="Legend" GroupName="b"   Text="&nbsp;Left" />--%>
                                                                                            <%--<asp:RadioButton ID="rdrRAlign" runat="server" CssClass="Legend" GroupName="b" Text="&nbsp;Right" />--%>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row-fluid div-margin" id="Tr5" runat="server">
                                                                                <div class="col-sm-2">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">Currency</span>
                                                                                        <asp:TextBox ID="txtCurrencyType" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                                                        <asp:DropDownList ID="drpLogoHeight" runat="server" Visible="False">
                                                                                            <asp:ListItem Text="1" />
                                                                                            <asp:ListItem Text="2" />
                                                                                            <asp:ListItem Text="3" />
                                                                                        </asp:DropDownList>
                                                                                        <asp:DropDownList ID="txtLogoWeirht" runat="server" Visible="False">
                                                                                            <asp:ListItem Text="1" />
                                                                                            <asp:ListItem Text="2" />
                                                                                            <asp:ListItem Text="3" />
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                                <div runat="server" id="tdName43" visible="false">
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="col-sm-12">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Header Slogan</h3>
                                                                </div>
                                                                <div class="panel-body well-sm-tiny">
                                                                    <div class="row-fluid">
                                                                        <div class="col-sm-12">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbHeaderSloganTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblHeaderText" runat="server" ClientIDMode="Static">Print Header Slogan</asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3">
                                                                        <div class="col-sm-4">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon ">Receipt Slogan</span>
                                                                                <asp:TextBox ID="txtSloganName" runat="server" MaxLength="100" Rows="2" Height="35px"
                                                                                    CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3" runat="server" visible="false" id="tdName19">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">
                                                                                    <div runat="server" visible="false" id="tdName18">
                                                                                        Font
                                                                                    </div>
                                                                                </span>
                                                                                <div visible="false" id="tdName20" runat="server">
                                                                                    <asp:DropDownList ID="drpHeaderFontName" runat="server" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2" runat="server" visible="false" id="tdName22">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">
                                                                                    <div runat="server" visible="false" id="tdName21">
                                                                                        Size</div>
                                                                                </span>
                                                                                <div runat="server" visible="false" id="tdName35">
                                                                                    <asp:TextBox ID="TxtHeaderFontSize" runat="server" CssClass="input-mini" ClientIDMode="Static"
                                                                                        onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                                                                    <%--  <asp:DropDownList ID="drpHeaderFontSize" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Selected="True" Text="8" />
                                                                                        <asp:ListItem Text="9" />
                                                                                        <asp:ListItem Text="10" />
                                                                                        <asp:ListItem Text="11" />
                                                                                        <asp:ListItem Text="12" />
                                                                                        <asp:ListItem Text="13" />
                                                                                        <asp:ListItem Text="14" />
                                                                                    </asp:DropDownList>--%>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3" runat="server" visible="false" id="tdName24">
                                                                            <div class="input-group labelBorder" style="height: 34px">
                                                                                <span class="input-group-addon">
                                                                                    <div runat="server" visible="false" id="tdName23">
                                                                                        Style</div>
                                                                                </span>
                                                                                <div runat="server" class="div-margin" visible="false" id="tdName25">
                                                                                    &nbsp;<asp:CheckBox ID="chkHeaderBold" Text="&nbsp;B" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkHeaderItalic" Text="&nbsp;I" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkHeaderUL" Text="&nbsp;U" runat="server" CssClass="TDCaption" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin">
                                                                        <div class="col-sm-4">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">Deliver Slogan</span>
                                                                                <asp:TextBox ID="txtFooterSloganName" runat="server" MaxLength="100" Height="35px"
                                                                                    CssClass="form-control" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3" runat="server" visible="false" id="tdName27">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">
                                                                                    <div runat="server" visible="false" id="tdName26">
                                                                                        Font
                                                                                    </div>
                                                                                    <%--<div runat="server" visible="false" id="tdName27"></div>--%>
                                                                                </span>
                                                                                <div runat="server" visible="false" id="tdName28">
                                                                                    <asp:DropDownList ID="drpFooterFontName" runat="server" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2" runat="server" visible="false" id="tdName30">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">
                                                                                    <div runat="server" visible="false" id="tdName29">
                                                                                        Size</div>
                                                                                    <%--<div runat="server" visible="false" id="tdName30"></div>--%>
                                                                                </span>
                                                                                <div runat="server" visible="false" id="tdName31">
                                                                                    <asp:TextBox ID="TxtFooterFontSize" runat="server" CssClass="input-mini" ClientIDMode="Static"
                                                                                        onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                                                                    <%--<asp:DropDownList ID="drpFooterFontSize" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Selected="True" Text="8" />
                                                                                        <asp:ListItem Text="9" />
                                                                                        <asp:ListItem Text="10" />
                                                                                        <asp:ListItem Text="11" />
                                                                                        <asp:ListItem Text="12" />
                                                                                        <asp:ListItem Text="13" />
                                                                                        <asp:ListItem Text="14" />
                                                                                    </asp:DropDownList>--%>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3" runat="server" visible="false" id="tdName33">
                                                                            <div class="input-group labelBorder" style="height: 34px">
                                                                                <span class="input-group-addon">
                                                                                    <div runat="server" visible="false" id="tdName32">
                                                                                        Style</div>
                                                                                    <%--<div runat="server" visible="false" id="tdName33"></div>--%>
                                                                                </span>
                                                                                <div class="div-margin" runat="server" visible="false" id="tdName34">
                                                                                    &nbsp;<asp:CheckBox ID="chkFooterBold" Text="&nbsp;B" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkFooterItalic" Text="&nbsp;I" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkFooterUL" Text="&nbsp;U" runat="server" CssClass="TDCaption" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <span runat="server" class="Legend" visible="false" id="tdName100"></span><span runat="server"
                                                                        class="Legend" visible="false" id="tdName102"></span>
                                                                    <div class="row-fluid div-margin">
                                                                        <div class="col-sm-6" runat="server" visible="false" id="tdName101">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon ">Left header label</span>
                                                                                <asp:TextBox ID="txtleftMsg" runat="server" MaxLength="100" Rows="2" Height="35px"
                                                                                    placeholder="Left header label message" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-6" runat="server" visible="false" id="tdName103">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon ">Right header label</span>
                                                                                <asp:TextBox ID="txtRightMsg" runat="server" MaxLength="100" Rows="2" Height="35px"
                                                                                    placeholder="Right header label message" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="col-sm-12">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Order no / Customer information style</h3>
                                                                </div>
                                                                <div class="panel-body well-sm-tiny">
                                                                    <div class="row-fluid div-margin">
                                                                        <div class="col-sm-4">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon ">Order No/Date</span>
                                                                                <asp:DropDownList ID="drpBookingNoFontStyle" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon ">Size</span>
                                                                                <asp:TextBox ID="txtFontBookingNoSize" runat="server" CssClass="input-mini" ClientIDMode="Static"
                                                                                    onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                                                                <%--  <asp:DropDownList ID="drpFontBookingNoSize" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="9" />
                                                                                    <asp:ListItem Text="10" />
                                                                                    <asp:ListItem Text="11" />
                                                                                    <asp:ListItem Text="12" />
                                                                                    <asp:ListItem Text="13" />
                                                                                    <asp:ListItem Text="14" />
                                                                                    <asp:ListItem Text="15" />
                                                                                    <asp:ListItem Text="16" />
                                                                                    <asp:ListItem Text="17" />
                                                                                    <asp:ListItem Text="18" />
                                                                                    <asp:ListItem Text="19" />
                                                                                    <asp:ListItem Text="20" />
                                                                                    <asp:ListItem Text="21" />
                                                                                    <asp:ListItem Text="22" />
                                                                                    <asp:ListItem Text="23" />
                                                                                    <asp:ListItem Text="24" />
                                                                                </asp:DropDownList>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="input-group labelBorder" style="height: 34px">
                                                                                <span class="input-group-addon">Style </span>
                                                                                <div class="div-margin">
                                                                                    &nbsp;<asp:CheckBox ID="chkBookingB" Text="&nbsp;B" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkBookingI" Text="&nbsp;I" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkBookingU" Text="&nbsp;U" runat="server" CssClass="TDCaption" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="div-margin3">
                                                                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                    checked="" runat="server" id="rdrBarcodeTrue" />
                                                                                <span class="lbl"></span>
                                                                                <asp:Label ID="lblBarcodeText" runat="server" ClientIDMode="Static">Print Barcode</asp:Label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin">
                                                                        <div class="col-sm-4">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">Name/Address</span>
                                                                                <asp:DropDownList ID="drpNameFontStyle" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">Size</span>
                                                                                <asp:TextBox ID="txtNameFontSize" runat="server" CssClass="input-mini" ClientIDMode="Static"
                                                                                    onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                                                                <%-- <asp:DropDownList ID="drpNameFontSize" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="9" />
                                                                                    <asp:ListItem Text="10" />
                                                                                    <asp:ListItem Text="11" />
                                                                                    <asp:ListItem Text="12" />
                                                                                </asp:DropDownList>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="input-group labelBorder" style="height: 34px">
                                                                                <span class="input-group-addon">Style</span>
                                                                                <div class="div-margin">
                                                                                    &nbsp;<asp:CheckBox ID="chkNameB" Text="&nbsp;B" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkNameI" Text="&nbsp;I" runat="server" CssClass="TDCaption" />&nbsp;
                                                                                    <asp:CheckBox ID="chkNameU" Text="&nbsp;U" runat="server" CssClass="TDCaption" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <div class="div-margin3">
                                                                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                    checked="" runat="server" id="rdbPhoneNoTrue" />
                                                                                <span class="lbl"></span>
                                                                                <asp:Label ID="lblPhoneText" runat="server" ClientIDMode="Static">Print Phone No</asp:Label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="col-sm-12">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Receipt detail configuration</h3>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row-fluid">
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbPrintDueDateTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblDueDtText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbServicetaxTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblTax" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid  div-margin3">
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbTaxDetailTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblTaxDtlText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbBookingTimeTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblBooingText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid  div-margin3">
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbPreviousTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblPreDueText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbRateTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblrateText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid  div-margin3">
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbProcessTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblprocessText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbTableBorderTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblRectBorderText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                        <asp:RadioButton ID="rdbFooterSloganTrue" runat="server" GroupName="asdfgh" Text=" Yes"
                                                                            Checked="True" CssClass="TDCaption" Visible="False" />
                                                                        <asp:RadioButton ID="rdbFooterSloganFalse" runat="server" GroupName="asdfgh" Text=" No"
                                                                            CssClass="TDCaption" Visible="False" />
                                                                    </div>
                                                                    <div class="row-fluid  div-margin3">
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbSubItemTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblSubItemText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                        <div class="col-sm-6">
                                                                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                checked="" runat="server" id="rdbCustomerSignatureTrue" />
                                                                            <span class="lbl"></span>
                                                                            <asp:Label ID="lblCustSignText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="col-sm-12">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading">
                                                                    <h3 class="panel-title">
                                                                        Term & Condition</h3>
                                                                </div>
                                                                <div class="panel-body well-sm-tiny">
                                                                    <div class="row-fluid">
                                                                        <div class="col-sm-2">
                                                                        </div>
                                                                        <div class="col-sm-4">
                                                                            <div class="div-margin3">
                                                                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                    checked="" runat="server" id="rdbTermConditionTrue" />
                                                                                <span class="lbl"></span>
                                                                                <asp:Label ID="lblTermText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-5">
                                                                            <div class="div-margin3">
                                                                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                                    checked="" runat="server" id="rdbPrintTermsConditonTrue" />
                                                                                <span class="lbl"></span>
                                                                                <asp:Label ID="lblStoreTermText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Condition #1</span>
                                                                            <asp:TextBox ID="txtTerms1" runat="server" CssClass="form-control" MaxLength="115"
                                                                                placeholder="Condition #1"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Condition #2</span>
                                                                            <asp:TextBox ID="txtTerms2" runat="server" CssClass="form-control" MaxLength="115"
                                                                                placeholder="Condition #2"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Condition #3</span>
                                                                            <asp:TextBox ID="txtTerms3" runat="server" CssClass="form-control" MaxLength="115"
                                                                                placeholder="Condition #3"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms4" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Condition #4</span>
                                                                            <asp:TextBox ID="txtTerms4" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #4"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms5" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Condition #5</span>
                                                                            <asp:TextBox ID="txtTerms5" runat="server" CssClass=" form-control" MaxLength="100"
                                                                                placeholder="Condition #5"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms6" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Condition #6</span>
                                                                            <asp:TextBox ID="txtTerms6" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #6"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms7" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #7</span>
                                                                            <asp:TextBox ID="txtTerms7" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #7"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms8" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #8</span>
                                                                            <asp:TextBox ID="txtTerms8" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #8"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms9" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #9</span>
                                                                            <asp:TextBox ID="txtTerms9" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #9"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms10" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #10</span>
                                                                            <asp:TextBox ID="txtTerms10" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #10"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms11" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #11</span>
                                                                            <asp:TextBox ID="txtTerms11" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #11"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms12" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #12</span>
                                                                            <asp:TextBox ID="txtTerms12" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #12"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms13" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #13</span>
                                                                            <asp:TextBox ID="txtTerms13" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #13"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms14" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #14</span>
                                                                            <asp:TextBox ID="txtTerms14" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #14"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row-fluid div-margin3" runat="server" id="terms15" visible="false">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Condition #15</span>
                                                                            <asp:TextBox ID="txtTerms15" runat="server" CssClass="form-control" MaxLength="100"
                                                                                placeholder="Condition #15"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                                <div class="text-center">
                                    <a class="btn btn-primary" id="btnSave" runat="server" clientidmode="static" enabletheming="false"
                                        causesvalidation="False"><i class="fa fa-pencil-square-o"></i>&nbsp;Update Configuration</a>
                                </div>
                            </td>
                            <td id="tdDisplay" runat="server" visible="false" style="width: 100%">
                                <div class="panel panel-primary">
                                    <div class="panel-body">
                                        <div class="row-fluid">
                                            <div class="row-fluid" style="display: none;">
                                                <div class="col-sm-3">
                                                    Website Address
                                                </div>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtWebsiteName" runat="server" MaxLength="100" TabIndex="1"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="col-sm-5 Textpadding">
                                                    <div class="input-group">
                                                        <span class="input-group-addon IconBkColor"><i class="fa fa-building-o fa-lg"></i>
                                                        </span>
                                                        <asp:TextBox ID="txtStoreName" runat="server" MaxLength="100" placeholder="Kindly enter store name"
                                                            TabIndex="2" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1 Textpadding">
                                                </div>
                                                <div class="col-sm-5 Textpadding">
                                                    <div class="input-group">
                                                        <span class="input-group-addon IconBkColor"><i class="fa fa-home fa-lg"></i></span>
                                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="100" TabIndex="3" placeholder="Kindly enter store address"
                                                            CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1 Textpadding">
                                                </div>
                                            </div>
                                            <div class="row-fluid  div-margin">
                                                <div class="col-sm-5 Textpadding">
                                                    <div class="input-group">
                                                        <span class="input-group-addon IconBkColor"><i class="fa fa-clock-o fa-lg"></i></span>
                                                        <asp:TextBox ID="txtTiming" runat="server" MaxLength="100" TabIndex="4" placeholder="Kindly enter store timing"
                                                            CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1 Textpadding">
                                                    &nbsp;<i id="TimeInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                </div>
                                                <div class="col-sm-5 Textpadding">
                                                    <div class="input-group">
                                                        <span class="input-group-addon IconBkColor">Business slogan</span>
                                                        <asp:TextBox ID="txtFooterName" runat="server" MaxLength="100" TabIndex="6" placeholder="Kindly enter business slogan"
                                                            CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1 Textpadding">
                                                    &nbsp;<i id="BusinessInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                </div>
                                            </div>
                                            <div class="row-fluid div-margin">
                                                <div class="col-sm-5 Textpadding">
                                                    <div class="input-group">
                                                        <span class="input-group-addon IconBkColor">Website Url</span>
                                                        <asp:TextBox ID="txtWebsiteLink" runat="server" placeholder="Kindly enter webSite link"
                                                            MaxLength="200" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1 Textpadding">
                                                    &nbsp;<i id="WebInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                </div>
                                            </div>
                                            <div class="row-fluid div-margin" id="trSlip" runat="server" visible="False">
                                                <div class="col-sm-3">
                                                    Set&#160;Slip &#160;Inches
                                                </div>
                                                <div class="col-sm-6" id="Td41" runat="server">
                                                    <asp:TextBox ID="txtSSlipInch" runat="server" MaxLength="2" TabIndex="7"></asp:TextBox>
                                                    <asp:RangeValidator ID="rv" runat="server" ControlToValidate="txtSSlipInch" ErrorMessage="Please Select Inches Between 0-12"
                                                        Type="Integer" MaximumValue="12" MinimumValue="0"></asp:RangeValidator>
                                                </div>
                                            </div>
                                            <div class="row-fluid div-margin">
                                                <div class="col-sm-5 Textpadding">
                                                    <div class="input-group">
                                                        <span class="input-group-addon IconBkColor">Screen and Tags logo</span>
                                                        <input id="fupStudentPhoto" runat="server" onchange="return fupStudentPhoto_onchange();"
                                                            size="10" tabindex="8" type="file" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-1 Textpadding">
                                                    &nbsp;<i id="LogoInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                </div>
                                                <asp:CheckBox ID="chkImage" runat="server" Text=" Print Logo On Receipt" TabIndex="7"
                                                    Visible="False" />
                                                <div class="col-sm-4">
                                                    <img id="abc" runat="server" style="width: 60px; height: 60px;" />
                                                </div>
                                            </div>
                                            <%-- <div class="row-fluid div-margin">
                                                    <div class="col-sm-1">
                                                    </div>
                                                    <div class="col-sm-11">
                                                        Please upload a 60 X 60 pixels image. Allowed file type are Jpg, Jpeg, Gif and Png.
                                                    </div>
                                                </div>--%>
                                            <div class="row-fluid div-margin">
                                                <div class="col-sm-6">
                                                    <asp:CheckBox ID="chkSetting" runat="server" Text=" Display this screen on start up. "
                                                        TabIndex="9" Visible="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-footer text-center">
                                        <a class="btn btn-primary" id="Button1" tabindex="10" runat="server" clientidmode="static"
                                            causesvalidation="False" enabletheming="False"><i class="fa fa-pencil-square-o">
                                            </i>&nbsp;Update Store Information</a> <a class="btn btn-primary" id="BtnCancel"
                                                runat="server" style="display: none" causesvalidation="False" tabindex="11" enabletheming="False"
                                                clientidmode="static"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Cancel</a>
                                    </div>
                                </div>
                            </td>
                            <td id="tdDefaultSetting" runat="server" visible="false" style="width: 100%">
                                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="">
                                    <cc1:TabPanel ID="TabPanel2" runat="server">
                                        <ContentTemplate>
                                            <div class="row-fluid">
                                                <div class="row-fluid">
                                                    <div class="col-sm-5">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h3 class="panel-title">
                                                                    Order screen configuration</h3>
                                                            </div>
                                                            <div class="panel-body">
                                                                <div class="row-fluid">
                                                                    <div class="input-group input-group-sm">
                                                                        <span class="input-group-addon">Start Order From</span>
                                                                        <div class="row-fluid">
                                                                            <div class="col-sm-4 Textpadding">
                                                                                <asp:DropDownList ID="drpBookingPreFix" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                                                    <asp:ListItem Selected="True" Text=" " Value=" " />
                                                                                    <asp:ListItem Text="A" Value="A" />
                                                                                    <asp:ListItem Text="B" Value="B" />
                                                                                    <asp:ListItem Text="C" Value="C" />
                                                                                    <asp:ListItem Text="D" Value="D" />
                                                                                    <asp:ListItem Text="E" Value="E" />
                                                                                    <asp:ListItem Text="F" Value="F" />
                                                                                    <asp:ListItem Text="G" Value="G" />
                                                                                    <asp:ListItem Text="H" Value="H" />
                                                                                    <asp:ListItem Text="I" Value="I" />
                                                                                    <asp:ListItem Text="J" Value="J" />
                                                                                    <asp:ListItem Text="K" Value="K" />
                                                                                    <asp:ListItem Text="L" Value="L" />
                                                                                    <asp:ListItem Text="M" Value="M" />
                                                                                    <asp:ListItem Text="N" Value="N" />
                                                                                    <asp:ListItem Text="O" Value="O" />
                                                                                    <asp:ListItem Text="P" Value="P" />
                                                                                    <asp:ListItem Text="Q" Value="Q" />
                                                                                    <asp:ListItem Text="R" Value="R" />
                                                                                    <asp:ListItem Text="S" Value="S" />
                                                                                    <asp:ListItem Text="T" Value="T" />
                                                                                    <asp:ListItem Text="U" Value="U" />
                                                                                    <asp:ListItem Text="V" Value="V" />
                                                                                    <asp:ListItem Text="W" Value="W" />
                                                                                    <asp:ListItem Text="X" Value="X" />
                                                                                    <asp:ListItem Text="Y" Value="Y" />
                                                                                    <asp:ListItem Text="Z" Value="Z" />
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-sm-7 Textpadding">
                                                                                <asp:TextBox ID="txtStartBookingNo" runat="server" CssClass="form-control" MaxLength="6"
                                                                                    placeholder="Order no" TabIndex="7" ClientIDMode="Static"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                                        ID="txtStartBookingNo_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                        FilterType="Numbers" TargetControlID="txtStartBookingNo">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                            </div>
                                                                            <div class="col-sm-1 Textpadding">
                                                                                &nbsp;<i id="BookingNoInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-7 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Delivery Date Offset</span>
                                                                            <asp:TextBox ID="txtDefaultDateSet" runat="server" MaxLength="2" placeholder="Delivery date offset"
                                                                                TabIndex="6" CssClass="form-control"></asp:TextBox><cc1:FilteredTextBoxExtender ID="txtDefaultDateSet_FilteredTextBoxExtender"
                                                                                    runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtDefaultDateSet">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="DelSetInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="input-group input-group-sm">
                                                                        <span class="input-group-addon">Delivery Time</span>
                                                                        <div class="row-fluid">
                                                                            <div class="col-sm-5 Textpadding">
                                                                                <asp:DropDownList ID="txtDefaultTime" runat="server" TabIndex="4" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-sm-6 Textpadding">
                                                                                <asp:DropDownList ID="drpAMPM" runat="server" AppendDataBoundItems="True" TabIndex="5"
                                                                                    CssClass="form-control">
                                                                                    <asp:ListItem Text="AM"></asp:ListItem>
                                                                                    <asp:ListItem Text="PM"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="input-group input-group-sm">
                                                                        <span class="input-group-addon">&nbsp;&nbsp;<i class="fa fa-user fa-lg">&nbsp;&nbsp;</i></span>
                                                                        <div class="row-fluid">
                                                                            <div class="col-sm-11 Textpadding">
                                                                                <asp:DropDownList ID="drpDefaultCustomerSearch" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                                    <asp:ListItem Text="Name" Value="Name"></asp:ListItem>
                                                                                    <asp:ListItem Text="Address" Value="Address"></asp:ListItem>
                                                                                    <asp:ListItem Text="Mobile" Value="Mobile"></asp:ListItem>
                                                                                    <asp:ListItem Text="Membership Id" Value="MembershipId"></asp:ListItem>
                                                                                    <asp:ListItem Text="Customer Code" Value="CustCode"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-sm-1 Textpadding">
                                                                                &nbsp;<i id="CustSearchInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-11 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Workshop note type</span>
                                                                            <asp:DropDownList ID="drpChallanType" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="Invoice Based Detailed"></asp:ListItem>
                                                                                <asp:ListItem Text="Invoice Based"></asp:ListItem>
                                                                                <asp:ListItem Text="Invoice With Item Detailed"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="WSNoteInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-1">
                                                    </div>
                                                    <div class="col-sm-5">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h3 class="panel-title">
                                                                    Order screen configuration</h3>
                                                            </div>
                                                            <div class="panel-body">
                                                                <div class="row-fluid">
                                                                    <div class="col-sm-7 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Default Quantity</span>
                                                                            <asp:DropDownList ID="drpSetQty" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="QtyInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-11 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="fa fa-shopping-cart fa-lg"></i></span>
                                                                            <asp:DropDownList ID="drpMainProcesses" CssClass=" form-control" runat="server" TabIndex="1">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="processInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-11 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="fa fa-bitbucket fa-lg"></i></span>
                                                                            <asp:DropDownList ID="drpItems" runat="server" TabIndex="2" CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="GarmentInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-11 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Discount Type</span>
                                                                            <asp:DropDownList ID="drpDefaultDiscountType" CssClass="form-control" runat="server">
                                                                                <asp:ListItem Text="Percentage"></asp:ListItem>
                                                                                <asp:ListItem Text="Flat"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="DiscountInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-10 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Net Amount</span>
                                                                            <div class="div-margin3">
                                                                                &nbsp;&nbsp;<label>
                                                                                    <input name="NetAmount" id="rdbFlat" type="radio" runat="server" checked="True" class="ace" />
                                                                                    <span class="lbl">&nbsp;Rounded</span>
                                                                                </label>
                                                                                <%--  <asp:RadioButton ID="rdbFlat" runat="server" Text="&nbsp;Rounded" GroupName="NetAmount" Checked="True" CssClass="Legend"   ToolTip="Amount display like e.g(100)" />--%>
                                                                                &nbsp;&nbsp;<label>
                                                                                    <input name="NetAmount" id="rdbFloat" type="radio" runat="server" class="ace" />
                                                                                    <span class="lbl">&nbsp;In Decimal</span>
                                                                                </label>
                                                                                <%--<asp:RadioButton ID="rdbFloat" runat="server" Text="&nbsp;In Decimal" GroupName="NetAmount"   CssClass="Legend" ToolTip="Amount display like e.g(100.00)" />--%>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="NetAmountInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid div-margin" id="Tr7" runat="server" visible="false">
                                                        <div class="col-sm-3 ">
                                                            Color
                                                        </div>
                                                        <div class="col-sm-3 ">
                                                            <asp:DropDownList ID="drpColorName" runat="server" TabIndex="3" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid div-margin" id="Tr8" runat="server" visible="false">
                                                        <div class="col-sm-3 ">
                                                            Default Customer Search Criteria
                                                        </div>
                                                        <div class="col-sm-3 ">
                                                            <asp:RadioButton ID="rdbName" runat="server" Text="Name" CssClass="Legend" GroupName="default"
                                                                Checked="True" TabIndex="8" /><asp:RadioButton ID="rdbAddress" runat="server" Text="Address"
                                                                    CssClass="Legend" GroupName="default" TabIndex="9" /><asp:RadioButton ID="rdbMobileNo"
                                                                        runat="server" Text="Mobile No" CssClass="Legend" GroupName="default" TabIndex="10" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row-fluid">
                                                    <div class="col-sm-11">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h3 class="panel-title">
                                                                    Urgent configuration</h3>
                                                            </div>
                                                            <div class="panel-body">
                                                                <div class="row-fluid">
                                                                    <div class="col-sm-5 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon textRed">Urgent # 1</span>
                                                                            <asp:TextBox ID="txtUrgunt1" runat="server" EnableTheming="false" CssClass="form-control"
                                                                                MaxLength="50" TabIndex="7"></asp:TextBox>
                                                                            <cc1:TextBoxWatermarkExtender ID="txtUrgunt1_TextBoxWatermarkExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtUrgunt1" WatermarkText="Create express service here">
                                                                            </cc1:TextBoxWatermarkExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="Urgent1Info" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                    <div class="col-sm-2 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Rate</span>
                                                                            <asp:TextBox ID="txtUrgunt1Rate" CssClass="form-control" runat="server" MaxLength="3"
                                                                                placeholder="Rate" TabIndex="7"></asp:TextBox>
                                                                            <span class="input-group-addon" style="color: Red">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt1Rate_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtUrgunt1Rate" ValidChars="1234567890">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="Urgent1RateInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                    <div class="col-sm-2 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">Day Offset</span>
                                                                            <asp:TextBox ID="txtUrgunt1Day" CssClass="form-control" runat="server" MaxLength="2"
                                                                                placeholder="Day offset" TabIndex="7"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt1Day_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtUrgunt1Day">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                        &nbsp;<i id="Urgent1OffsetInfo" data-placement="left" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-5 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon textRed">Urgent # 2</span>
                                                                            <asp:TextBox ID="txtUrgunt2" EnableTheming="false" CssClass="form-control" runat="server"
                                                                                MaxLength="50" TabIndex="7"></asp:TextBox>
                                                                            <cc1:TextBoxWatermarkExtender ID="txtUrgunt2_TextBoxWatermarkExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtUrgunt2" WatermarkText="Create express service here">
                                                                            </cc1:TextBoxWatermarkExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                    </div>
                                                                    <div class="col-sm-2 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Rate</span>
                                                                            <asp:TextBox ID="txtUrgunt2Rate" CssClass="form-control" runat="server" MaxLength="3"
                                                                                placeholder="Rate" TabIndex="7"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt2Rate_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtUrgunt2Rate" ValidChars="1234567890">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <span class="input-group-addon " style="color: Red">%</span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                    </div>
                                                                    <div class="col-sm-2 Textpadding">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon ">Day Offset</span>
                                                                            <asp:TextBox ID="txtUrgunt2Day" CssClass="form-control" runat="server" MaxLength="2"
                                                                                placeholder="Day offset" TabIndex="7"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="txtUrgunt2Day_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtUrgunt2Day">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1 Textpadding">
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin4">
                                                                    <div class="col-sm-6 Textpadding">
                                                                        <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                            checked="" runat="server" id="rdbEnterRemarkTrue" />
                                                                        <span class="lbl"></span>&nbsp;<asp:Label ID="ActDescTxt" runat="server" ClientIDMode="Static"> </asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-6 Textpadding">
                                                                        <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                            checked="" runat="server" id="rdbBindToDescriptionTrue" />
                                                                        <span class="lbl"></span>&nbsp;<asp:Label ID="ActAllowDescTxt" runat="server" ClientIDMode="Static"> </asp:Label>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin3">
                                                                    <div class="col-sm-6 Textpadding">
                                                                        <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                            checked="" runat="server" id="rdbEnterColorTrue" />
                                                                        <span class="lbl"></span>&nbsp;<asp:Label ID="ActColorText" runat="server" ClientIDMode="Static"> </asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-6 Textpadding">
                                                                        <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                            checked="" runat="server" id="rdbBindColorToMasterTrue" />
                                                                        <span class="lbl"></span>&nbsp;<asp:Label ID="AllowColorText" runat="server" ClientIDMode="Static"> </asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row-fluid">
                                                    <div class="col-sm-11">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h3 class="panel-title">
                                                                    Other configuration</h3>
                                                            </div>
                                                            <div class="panel-body">
                                                                <div class="row-fluid">
                                                                    <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                        checked="" runat="server" id="rdbSaveEditRemarksTrue" />
                                                                    <span class="lbl"></span>&nbsp;<asp:Label ID="RemarkTxt" runat="server" ClientIDMode="Static"> </asp:Label>
                                                                </div>
                                                                <div class="row-fluid div-margin3">
                                                                    <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                        checked="" runat="server" id="rdbConfirmDateTrue" />
                                                                    <span class="lbl"></span>&nbsp;<asp:Label ID="DelDate" runat="server" ClientIDMode="Static"></asp:Label>
                                                                </div>
                                                                <div class="row-fluid div-margin3">
                                                                    <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                        checked="" runat="server" id="rdbBindColorQtyTrue" />
                                                                    <span class="lbl"></span>&nbsp;<asp:Label ID="cappingColorText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                </div>


                                                                 <div class="row-fluid div-margin3">
                                                                    <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                                                        checked="" runat="server" id="chkAndroid" />
                                                                    <span class="lbl"></span>&nbsp;<asp:Label ID="lblAndroidText" runat="server" ClientIDMode="Static"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row-fluid div-margin" style="display: none">
                                                    <div class="col-sm-7 ">
                                                        Save/Update Rate in Price List (for next time use)
                                                    </div>
                                                    <div class="col-sm-2 ">
                                                        <asp:RadioButton ID="rdbSaveRateInItemMasterTrue" runat="server" Text=" Yes" GroupName="CFC"
                                                            Checked="True" CssClass="Legend" />
                                                        <asp:RadioButton ID="rdbSaveRateInItemMasterFalse" runat="server" Text=" No" GroupName="CFC"
                                                            CssClass="Legend" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                                <div class="text-center">
                                    <a class="btn btn-primary" id="btnDefault" runat="server" onclientclick="return checkName();"
                                        clientidmode="static" causesvalidation="False" tabindex="11"><i class="fa fa-pencil-square-o">
                                        </i>&nbsp;Update Default Configuration</a> <a class="btn btn-primary" id="btnReset"
                                            runat="server" enabletheming="false" style="display: none" clientidmode="static"
                                            tabindex="12"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Reset</a>
                                </div>
                            </td>
                            <td id="tdSetTimeZone" runat="server" visible="false" style="width: 100%">
                                <div class="panel panel-primary">
                                    <div class="panel-body">
                                        <cc1:TabContainer ID="TabContainer2" CssClass="" runat="server">
                                            <cc1:TabPanel ID="TabPanel3" runat="server">
                                                <ContentTemplate>
                                                    <fieldset class="Fieldset">
                                                        <div class="row-fluid div-margin">
                                                            <div class="col-sm-6 Textpadding">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor"><i class="fa fa-globe fa-lg"></i></span>
                                                                    <asp:DropDownList ID="drpZoneList" CssClass="form-control" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-1 Textpadding">
                                                                &nbsp;<i id="timeZoneInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid div-margin">
                                                            &nbsp;<asp:Label ID="lblSetTimeZoneSucess" runat="server" Font-Size="15px" Font-Bold="true"
                                                                ForeColor="Gray" EnableViewState="False" />
                                                        </div>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </div>
                                    <div class="panel-footer text-center">
                                        <a class="btn btn-primary" id="btnZoneSave" runat="server" clientidmode="static"
                                            causesvalidation="False"><i class="fa fa-pencil-square-o"></i>&nbsp;Update Time
                                            Zone</a>
                                    </div>
                                </div>
                            </td>
                            <td id="tdGeneralSetting" runat="server" visible="false" style="width: 100%">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">
                                            Miscellaneous Setting</h3>
                                    </div>
                                    <div class="panel-body">
                                        <cc1:TabContainer ID="TabContainer3" runat="server" CssClass="">
                                            <cc1:TabPanel ID="TabPanel4" runat="server">
                                                <ContentTemplate>
                                                    <div class="row-fluid" style="text-align: center">
                                                        <h3>
                                                            <asp:Label ID="generalError" runat="server" CssClass="label label-warning" EnableViewState="False" />
                                                            <asp:Label ID="generalSuccess" runat="server" CssClass="label label-success" EnableViewState="False"></asp:Label></h3>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="col-sm-12 ">
                                                            <div class="well">
                                                            </div>
                                                            <div class="well">
                                                                <div class="row-fluid div-margin">
                                                                    <div class="col-sm-12 ">
                                                                        Please Choose Options below, if you want to <span style="font-size: 14px; font-weight: bold">
                                                                            Disable Move All </span>and <span style="font-size: 14px; font-weight: bold">Force use
                                                                                of Bar Code</span> on Following Screens.
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin">
                                                                    <div class="col-sm-6 ">
                                                                        Send To WorkShop
                                                                    </div>
                                                                    <div class="col-sm-4 ">
                                                                        <asp:RadioButton ID="rdbSendToWorkShopTrue" runat="server" Text=" Yes" GroupName="SST"
                                                                            Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rdbSendToWorkShopFalse" runat="server" Text=" No" GroupName="SST"
                                                                            CssClass="Legend" />
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin">
                                                                    <div class="col-sm-6 ">
                                                                        Receive From WorkShop
                                                                    </div>
                                                                    <div class="col-sm-4 ">
                                                                        <asp:RadioButton ID="rdbReceiveTrue" runat="server" Text=" Yes" GroupName="RFT" Checked="True"
                                                                            CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rdbReceiveFalse" runat="server" Text=" No" GroupName="RFT" CssClass="Legend" />
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin">
                                                                    <div class="col-sm-6 ">
                                                                        Mark For Delivery
                                                                    </div>
                                                                    <div class="col-sm-4 ">
                                                                        <asp:RadioButton ID="rdbMarkDeliveryTrue" runat="server" Text=" Yes" GroupName="MDT"
                                                                            Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rdbMarkDeliveryFalse" runat="server" Text=" No" GroupName="MDT"
                                                                            CssClass="Legend" />
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid div-margin">
                                                                    <div class="col-sm-6 ">
                                                                        Force User Pin Barcode On Mark Ready Screen
                                                                    </div>
                                                                    <div class="col-sm-4 ">
                                                                        <asp:RadioButton ID="rdbWorkRecieveTrue" runat="server" Text=" Yes" GroupName="WRT"
                                                                            Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rdbWorkRecieveFalse" runat="server" Text=" No" GroupName="WRT"
                                                                            CssClass="Legend" />
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid" style="visibility: hidden">
                                                                    <div class="col-sm-6 ">
                                                                        WorkShop Ready
                                                                    </div>
                                                                    <div class="col-sm-4 ">
                                                                        <asp:RadioButton ID="rdbWorkReadyTrue" runat="server" Text=" Yes" GroupName="WWRT"
                                                                            Checked="True" CssClass="Legend" />
                                                                        <asp:RadioButton ID="rdbWorkReadyFalse" runat="server" Text=" No" GroupName="WWRT"
                                                                            CssClass="Legend" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="well">
                                                                <div class="row-fluid  well well-sm-tiny" style="height: 43px">
                                                                    <div class="col-sm-9">
                                                                        Do you want Item Creation from Booking Screen to be controlled by Password <span
                                                                            style="float: right">
                                                                            <asp:RadioButton ID="rdbItemCreationTrue" runat="server" Text=" Yes" GroupName="PWD"
                                                                                Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <asp:RadioButton ID="rdbItemCreationFalse" runat="server" Text=" No" GroupName="PWD"
                                                                                CssClass="Legend" />
                                                                        </span>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <asp:TextBox ID="txtPasswordItemCreation" runat="server" TextMode="Password" MaxLength="50"
                                                                            CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid well well-sm-tiny" style="height: 43px">
                                                                    <div class="col-sm-9">
                                                                        Do you want Price Change on Booking Screen to be controlled by Password <span style="float: right">
                                                                            <asp:RadioButton ID="rdbRateChangeTrue" runat="server" Text=" Yes" GroupName="Rate"
                                                                                Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <asp:RadioButton ID="rdbRateChangeFalse" runat="server" Text=" No" GroupName="Rate"
                                                                                CssClass="Legend" />
                                                                        </span>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <asp:TextBox ID="txtRateChange" runat="server" TextMode="Password" MaxLength="50"
                                                                            CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid well well-sm-tiny" style="height: 43px">
                                                                    <div class="col-sm-9">
                                                                        Do you want Discount on Booking to be controlled by Password <span style="float: right">
                                                                            <asp:RadioButton ID="rdbDiscountChangeTrue" runat="server" Text=" Yes" GroupName="Discount"
                                                                                Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <asp:RadioButton ID="rdbDiscountChangeFalse" runat="server" Text=" No" GroupName="Discount"
                                                                                CssClass="Legend" />
                                                                        </span>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <asp:TextBox ID="txtDiscountChange" runat="server" TextMode="Password" MaxLength="50"
                                                                            CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid  well well-sm-tiny" style="height: 43px">
                                                                    <div class="col-sm-9">
                                                                        Do you want Discount on Delivery to be controlled by Password <span style="float: right">
                                                                            <asp:RadioButton ID="rdbDiscountDelChangeTrue" runat="server" Text=" Yes" GroupName="DiscountDel"
                                                                                Checked="True" CssClass="Legend" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <asp:RadioButton ID="rdbDiscountDelChangeFalse" runat="server" Text=" No" GroupName="DiscountDel"
                                                                                CssClass="Legend" />
                                                                        </span>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <asp:TextBox ID="txtDiscountDelChange" runat="server" TextMode="Password" MaxLength="50"
                                                                            CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <%--  <div class="row-fluid well" style="text-align:center" >      

		                  <asp:Button ID="btnPasswordSave" runat="server" Text="Save" CausesValidation="False" CssClass="btn btn-info btn-lg"	 EnableTheming="false"	OnClick="btnPasswordSave_Click" />
		
			                </div>--%>
                                                            </div>
                                                            <div class="row-fluid">
                                                                <div class="col-sm-5">
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <div>
                                                                        <a class="btn btn-primary btn-lg btn-block" id="btnGeneralSetting1" enabletheming="false"
                                                                            runat="server" causesvalidation="False"><i class="fa fa-floppy-o"></i>&nbsp;&nbsp;Save</a>
                                                                    </div>
                                                                    <%--  <asp:Button ID="btnGeneralSetting1" runat="server" Text="Save" CausesValidation="False"
						CssClass="btn btn-info btn-lg btn-block icon-save-image"  EnableTheming="false"		OnClick="btnGeneralSetting1_Click" />--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    </div>
                                                    <div>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </div>
                                </div>
                            </td>
                            <td id="tdEmail" runat="server" visible="false" style="width: 100%">
                                <div class="panel panel-primary">
                                    <div class="panel-body">
                                        <cc1:TabContainer ID="TabContainer4" CssClass="" runat="server">
                                            <cc1:TabPanel ID="TabPanel5" runat="server">
                                                <ContentTemplate>
                                                    <fieldset class="Fieldset">
                                                        <div class="row-fluid">
                                                            <div class="row-fluid ">
                                                                <div class="col-sm-6">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon IconBkColor">Mail Server</span>
                                                                        <asp:TextBox ID="txtHostName" placeholder="Enter mail server" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row-fluid div-margin">
                                                                <div class="col-sm-6">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon IconBkColor"><i class="fa fa-envelope-o fa-lg"></i>
                                                                        </span>
                                                                        <asp:TextBox ID="txtBranchEmail" placeholder="Enter Email ID" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row-fluid div-margin">
                                                                <div class="col-sm-6">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon IconBkColor">Password</span>
                                                                        <asp:TextBox ID="txtBranchPassword" placeholder="Enter password" runat="server" TextMode="Password"
                                                                            CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row-fluid div-margin">
                                                                <div class="col-sm-3">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon IconBkColor">Port</span>
                                                                        <asp:DropDownList ID="drpPort" runat="server" CssClass="form-control">
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>587</asp:ListItem>
                                                                            <asp:ListItem>465</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <div class="div-margin">
                                                                        <asp:CheckBox ID="chkSSL" runat="server" CssClass="TDCaption" Text="&nbsp;SSL Enabled" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div style="height: 182px; display: none">
                                                                <asp:CheckBox ID="chkmailActive" runat="server" CssClass="TDCaption" />&nbsp; <span
                                                                    class="textBold">Email ID for Daily Status Mail </span>
                                                                <asp:TextBox ID="txtStatusEmailID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </div>
                                    <div class="panel-footer text-center">
                                        <a class="btn btn-primary" id="btnEmail" runat="server" clientidmode="Static" enabletheming="false"
                                            causesvalidation="False"><i class="fa fa-pencil-square-o"></i>&nbsp;Update Email</a>
                                    </div>
                                </div>
                            </td>
                            <td id="tdbackup" runat="server" visible="false" style="width: 100%">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">
                                            Database Backup Path Setting</h3>
                                    </div>
                                    <div class="panel-body">
                                        <cc1:TabContainer ID="TabContainer7" runat="server" CssClass="">
                                            <cc1:TabPanel ID="TabPanel8" runat="server">
                                                <ContentTemplate>
                                                    <fieldset class="Fieldset">
                                                        <div class="row-fluid" style="text-align: center">
                                                            <h3>
                                                                <asp:Label ID="lbpatherror" runat="server" EnableTheming="false" CssClass="label label-warning"
                                                                    EnableViewState="False" /></h3>
                                                            <h3>
                                                                <asp:Label ID="lbpathsucess" runat="server" EnableTheming="false" CssClass="label label-success"
                                                                    EnableViewState="False"></asp:Label></h3>
                                                        </div>
                                                        <div class="col-sm-12">
                                                            <div class="row-fluid well">
                                                                <div class="col-sm-4">
                                                                    Choose Database Backup Path
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:DropDownList ID="Drpdrive" runat="server" CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="row-fluid">
                                                                <div class="col-sm-5">
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <div>
                                                                        <a class="btn btn-primary btn-lg btn-block" id="btbackupsave" runat="server" clientidmode="static">
                                                                            <i class="fa fa-floppy-o"></i>&nbsp;&nbsp;Save</a>
                                                                    </div>
                                                                    <%-- <asp:Button ID="btbackupsave" runat="server" Text="Save" EnableTheming="false" CausesValidation="False" CssClass="btn btn-info btn-lg btn-block "	OnClick="btbackupsave_Click" />--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </div>
                                </div>
                            </td>
                            <td id="tdServiceTax" runat="server" visible="false" style="width: 100%">
                                <div class="panel panel-primary">
                                    <div class="panel-body well-sm-tiny">
                                        <cc1:TabContainer ID="TabContainer5" runat="server" CssClass="">
                                            <cc1:TabPanel ID="TabPanel6" runat="server">
                                                <ContentTemplate>
                                                    <fieldset class="Fieldset">
                                                        <span style="color: #6F6F6F"><span class="Header-size" style="margin-left: 5px; font-size: 14px">
                                                            Default Tax : </span><span class="Header-size" style="font-size: 13px">Default tax on
                                                                Invoice Amount for applicable Services</span> <span style="font-size: 13px"><i>(Different
                                                                    Services can have different tax rates <a href="../Masters/ProcessMaster.aspx">Click
                                                                        Here</a> to update Tax Amount for each Service)</i></span> </span>
                                                        <div class="row-fluid div-margin">
                                                            <div class="col-sm-5 ">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Label # 1</span>
                                                                    <asp:TextBox ID="txtServiceTaxText1" runat="server" MaxLength="50" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Value</span>
                                                                    <asp:TextBox ID="txtServiceTaxRate1" runat="server" MaxLength="5" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                        TargetControlID="txtServiceTaxRate1" ValidChars="1234567890.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <span class="input-group-addon IconBkColor textRed">%</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="div-margin" style="color: #6F6F6F">
                                                            <span class="Header-size" style="margin-left: 5px; font-size: 14px">Default Tax on Tax:
                                                            </span><span class="Header-size" style="font-size: 13px">On tax amount calculated on
                                                                invoiced amount </span><span style="font-size: 13px"><i>(Different services can have
                                                                    different tax on tax rates <a href="../Masters/ProcessMaster.aspx">Click Here</a>
                                                                    to update tax on tax amount for each service) </i></span>
                                                        </div>
                                                        <div class="row-fluid div-margin ">
                                                            <div class="col-sm-5 ">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Label # 2</span>
                                                                    <asp:TextBox ID="txtServiceText2" runat="server" MaxLength="50" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Value</span>
                                                                    <asp:TextBox ID="txtServiceTaxRate2" runat="server" MaxLength="5" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                        TargetControlID="txtServiceTaxRate2" ValidChars="1234567890.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <span class="input-group-addon IconBkColor textRed">%</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid div-margin">
                                                            <div class="col-sm-5 ">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Label # 3</span>
                                                                    <asp:TextBox ID="txtServiceText3" runat="server" MaxLength="50" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Value</span>
                                                                    <asp:TextBox ID="txtServiceTaxRate3" runat="server" MaxLength="5" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                        TargetControlID="txtServiceTaxRate3" ValidChars="1234567890.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <span class="input-group-addon IconBkColor textRed">%</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid div-margin ">
                                                            <div class="col-sm-5">
                                                                <div class="input-group labelBorder">
                                                                    <span class="input-group-addon IconBkColor">Tax Applicable</span>
                                                                    <div class="div-margin3">
                                                                        &nbsp;&nbsp;<label>
                                                                            <input name="Tax" id="rdbTaxBefore" type="radio" runat="server" checked="True" class="ace" />
                                                                            <span class="lbl">&nbsp;Before Discount</span>
                                                                        </label>
                                                                        &nbsp;&nbsp;<label>
                                                                            <input name="Tax" id="rdbTaxAfter" type="radio" runat="server" class="ace" clientidmode="Static" />
                                                                            <span class="lbl">&nbsp;After Discount</span>
                                                                        </label>
                                                                    </div>
                                                                    <%-- <asp:RadioButton ID="rdbTaxBefore" runat="server" Text="&nbsp;Before Discount" GroupName="Tax"
                                                                        Checked="True" CssClass="Legend" />
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rdbTaxAfter" runat="server" Text="&nbsp;After Discount" GroupName="Tax"
                                                                        CssClass="Legend" ClientIDMode="Static" />--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon IconBkColor">Tax Calculation</span>
                                                                    <asp:DropDownList runat="server" AppendDataBoundItems="True" ID="drpServiceTaxType"
                                                                        CssClass="form-control" ClientIDMode="Static">
                                                                        <asp:ListItem Text="Inclusive"></asp:ListItem>
                                                                        <asp:ListItem Text="Exclusive"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-1 Textpadding">
                                                                &nbsp;<i id="TaxInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                                            </div>
                                                        </div>
                                                        </div>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                        <div class="panel-footer text-center">
                                            <a class="btn btn-primary" id="btnServiceTaxSave" enabletheming="False" runat="server"
                                                clientidmode="static" causesvalidation="False" tabindex="11"><i class="fa fa-pencil-square-o">
                                                </i>&nbsp;Update Tax Rate</a>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnPassword" runat="server" />
    <asp:SqlDataSource ID="SDTItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [ItemName], [ItemID] FROM [ItemMaster] order by ItemName asc">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SDTColors" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT ID, ColorName FROM mstcolor where colorName!='/' order by colorName asc">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SDTProcesses" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [ProcessCode], [ProcessName] FROM [ProcessMaster] order by ProcessName asc">
    </asp:SqlDataSource>
    <asp:HiddenField ID="hdnPrint" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnValue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDeliveryPrinter" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDelPrintValue" runat="server" ClientIDMode="Static" />
    <!-- Start of js -->
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JavaScript/ace-elements.min.js" type="text/javascript"></script>
    <script src="../JavaScript/ace.min.js" type="text/javascript"></script>
    <script src="../JavaScript/fuelux.spinner.min.js" type="text/javascript"></script>
    <script src="../JavaScript/ConfigSettingSrc.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('#<%=txtStartBookingNo.ClientID%>').keyup(function () { //Event that will fire on each change of textbox value.
            var myLength = $("#<%=txtStartBookingNo.ClientID%>").val().length;
            if (myLength == 1) //To check only when entering first character.
            {
                clearReceiptMsg();
                if ($(this).val() === '0') {
                    //alert('0 is not allowed as first character');
                    ShowReceiptMsg('#FA8602', '#999999');
                    $('#lblReceiptErr').html('0 is not allowed as first character.');
                    $(this).val('');
                }
            }
        });


        $('#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_rdrLaser,#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_rdrDeliveryLaser').click(function () {
            document.getElementById("<%=btntmpLaser.ClientID %>").click();
        });
        $('#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_rdrDotMatrix,#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_rdrDeliveryDotMatrix').click(function () {
            document.getElementById("<%=btnTmpDotMatrix.ClientID %>").click();
        });


        $('#rdrLogoAndTest').click(function () {
            if ($('#rdrLogoAndTest').is(':checked')) {
                $('#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_tblLogo').show();
                $('#StoreCopyInfo').text("I would like to use blank paper as receipt.");
                $('#StoreCopyInfo').removeClass('txtColor');

            } else {
                $('#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_tblLogo').hide();
                $('#StoreCopyInfo').text("I would use pre printed receipt formate.");
                $('#StoreCopyInfo').addClass('txtColor');
            }
        });

        function ShowMsgDefaultSetting(argColorOne, argColorTwo) {
            document.getElementById('divDefaultSetting').style.display = "inline";
            $('#divDefaultSettingInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divDefaultSetting').fadeOut(3000); }, 4000);
        }

        function ShowTimeZoneMsg(argColorOne, argColorTwo) {
            document.getElementById('divTimeZoneMsg').style.display = "inline";
            $('#divTimeZoneMsgInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divTimeZoneMsg').fadeOut(3000); }, 4000);
        }

        function ShowEmailMsg(argColorOne, argColorTwo) {
            document.getElementById('divEmailMsg').style.display = "inline";
            $('#divEmailInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divEmailMsg').fadeOut(3000); }, 4000);
        }

        function ShowStoreMsg(argColorOne, argColorTwo) {
            document.getElementById('divStoreMsg').style.display = "inline";
            $('#divStoreInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divStoreMsg').fadeOut(3000); }, 4000);
        }


        function ShowServiceTaxMsg(argColorOne, argColorTwo) {
            document.getElementById('divServiceTaxMsg').style.display = "inline";
            $('#divServiceTaxInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divServiceTaxMsg').fadeOut(3000); }, 4000);
        }

        function ShowReceiptMsg(argColorOne, argColorTwo) {
            document.getElementById('divReceiptMsg').style.display = "inline";
            $('#divReceiptInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divReceiptMsg').fadeOut(3000); }, 4000);
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            try {


                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbItemCreationFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbItemCreationTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbRateChangeFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbRateChangeTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbDiscountChangeFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbDiscountChangeTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbDiscountDelChangeFalse').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').hidden = true;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').style["display"] = "none"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').value = "";
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbDiscountDelChangeTrue').onclick = function (e) {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').hidden = false;
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').style["display"] = "block"
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').focus();
                }

                document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_btnGeneralSetting1').onclick = function (e) {


                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbItemCreationTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtPasswordItemCreation').focus();
                        return false;
                    }
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbRateChangeTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtRateChange').focus();
                        return false;
                    }
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbDiscountChangeTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountChange').focus();
                        return false;
                    }
                    if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').value == '' && document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_rdbDiscountDelChangeTrue').checked == true) {
                        alert('Please enter the password!');
                        document.getElementById('ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_txtDiscountDelChange').focus();
                        return false;
                    }

                    if (e.clientX == 0 || e.clientY == 0) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnGeneralSetting1', null);
                }

            }

            catch (e) {
                // whatever
            }


            $('#btnZoneSave,#btnDefault,#btbackupsave,#btnSave,#btnReset,#Button1,#BtnCancel,#btnEmail,#ctl00_ContentPlaceHolder1_TabContainer3_TabPanel4_btnGeneralSetting1,#btnServiceTaxSave').click(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
            });

            $('#ctl00_ContentPlaceHolder1_btnSetTimeZone,#ctl00_ContentPlaceHolder1_btnDefaultSetting,#ctl00_ContentPlaceHolder1_btnDispaly,#ctl00_ContentPlaceHolder1_btnGeneralSetting,#ctl00_ContentPlaceHolder1_btnSetEmail,#ctl00_ContentPlaceHolder1_btnBackUp,#ctl00_ContentPlaceHolder1_btnServiceTac,#ctl00_ContentPlaceHolder1_btnReceipt').click(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
            });

            $('#btnZoneSave,#btnDefault,#btnReset,#Button1,#BtnCancel,#btnEmail,#btbackupsave,#btnSave,#btnServiceTaxSave').click(function (e) {
                var clickedId = $(this).attr("id");

                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                if (clickedId == 'btnZoneSave') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnZoneSave', null);
                }
                else if (clickedId == 'btnDefault') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnDefault', null);
                }
                else if (clickedId == 'btnReset') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnReset', null);
                }
                else if (clickedId == 'Button1') {
                    __doPostBack('ctl00$ContentPlaceHolder1$Button1', null);
                }
                else if (clickedId == 'BtnCancel') {
                    __doPostBack('ctl00$ContentPlaceHolder1$BtnCancel', null);
                }
                else if (clickedId == 'btnSave') {

                    __doPostBack('ctl00$ContentPlaceHolder1$btnSave', null);
                }
                else if (clickedId == 'btnEmail') {
                    clearEmailMsg();
                    if ($('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_chkmailActive').is(":checked")) {
                        if ($('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtStatusEmailID').val().trim() == '') {
                            $('#lblEmailSuccess').html('');
                            ShowEmailMsg('#FA8602', '#999999');
                            $('#lblEmailError').html('Please enter daily status EMail ID.');
                            $('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtStatusEmailID').focus();
                            return false;
                        }
                    }
                    if ($('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtHostName').val().trim() == '') {
                        ShowEmailMsg('#FA8602', '#999999');
                        $('#lblEmailError').html('Please enter mail server.');
                        $('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtHostName').focus();
                        return false;
                    }
                    if ($('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtBranchEmail').val().trim() == '') {
                        ShowEmailMsg('#FA8602', '#999999');
                        $('#lblEmailError').html('Please enter EMail ID.');
                        $('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtBranchEmail').focus();
                        return false;
                    }
                    if ($('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtBranchPassword').val().trim() == '') {
                        $('#lblEmailError').html('Please enter password.');
                        $('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtBranchPassword').focus();
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnEmail', null);
                }
                else if (clickedId == 'btbackupsave') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btbackupsave', null);
                }
                else if (clickedId == 'btnServiceTaxSave') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnServiceTaxSave', null);
                }

            });


            $('#ctl00_ContentPlaceHolder1_btnSetTimeZone,#ctl00_ContentPlaceHolder1_btnDefaultSetting,#ctl00_ContentPlaceHolder1_btnDispaly,#ctl00_ContentPlaceHolder1_btnGeneralSetting,#ctl00_ContentPlaceHolder1_btnSetEmail,#ctl00_ContentPlaceHolder1_btnBackUp,#ctl00_ContentPlaceHolder1_btnServiceTac,#ctl00_ContentPlaceHolder1_btnReceipt').click(function (e) {
                var cId = $(this).attr("id");
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }

                if (cId == 'ctl00_ContentPlaceHolder1_btnSetTimeZone') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnSetTimeZone', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnDefaultSetting') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnDefaultSetting', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnDispaly') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnDispaly', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnGeneralSetting') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnGeneralSetting', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnSetEmail') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnSetEmail', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnBackUp') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnBackUp', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnServiceTac') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnServiceTac', null);
                }
                else if (cId == 'ctl00_ContentPlaceHolder1_btnReceipt') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnReceipt', null);
                }

            });

            $('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_chkmailActive').click(function (event) {
                if ($('#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_chkmailActive').is(":checked")) {
                    $("#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtStatusEmailID").removeAttr("disabled");
                    $("#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtStatusEmailID").focus();
                } else {
                    $("#ctl00_ContentPlaceHolder1_TabContainer4_TabPanel5_txtStatusEmailID").attr("disabled", "disabled");
                }
            });

            // if inclusive, disallow after
            $('#btnServiceTaxSave').click(function (event) {

                // $('#rdbTaxAfter').is(':checked')
                if ($('#drpServiceTaxType').val() == 'Inclusive' && $('#rdbTaxAfter').is(':checked')) {
                    // disabled for now
                    //alert('can\'t have tax after discount, if tax calculation is inclusive');
                    //return false;
                }
            });

            var prt = '';
            try {
                var printers = jsPrintSetup.getPrintersList();
                prt = printers.split(',');
            }
            catch (err) {
            }
            var printValue = $('#hdnValue').val();
            $('#<%=drpDefaultPrinter.ClientID %>').append($("<option value='" + prt[0] + "'" + ">" + printValue + "</option>"));
            for (var i = 0; i < prt.length; i++) {

                if (prt[i] == printValue)
                    continue;

                $('#<%=drpDefaultPrinter.ClientID %>').append($("<option value='" + prt[i] + "'" + ">" + prt[i] + "</option>"));

            }

            $('#<%=drpDefaultPrinter.ClientID %>').change(function () {

                $('#<%=hdnPrint.ClientID %>').val($('#<%=drpDefaultPrinter.ClientID %>').val())
            });



            var printDelValue = $('#hdnDelPrintValue').val();
            $('#<%=drpDeliveryPrinter.ClientID %>').append($("<option value='" + prt[0] + "'" + ">" + printDelValue + "</option>"));
            for (var i = 0; i < prt.length; i++) {
                if (prt[i] == printDelValue)
                    continue;
                $('#<%=drpDeliveryPrinter.ClientID %>').append($("<option value='" + prt[i] + "'" + ">" + prt[i] + "</option>"));
            }

            $('#<%=hdnDeliveryPrinter.ClientID %>').val(printDelValue);
            $('#<%=drpDeliveryPrinter.ClientID %>').change(function () {
                $('#<%=hdnDeliveryPrinter.ClientID %>').val($('#<%=drpDeliveryPrinter.ClientID %>').val())
            });

        });
	</script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (e) {

            ShowAllLabelText();
            $('#rdbEnterRemarkTrue').click(function (e) { ShowActDescText(); });
            $('#rdbBindToDescriptionTrue').click(function (e) { ShowAllowDescText(); });
            $('#rdbEnterColorTrue').click(function (e) { ShowActColorText(); });
            $('#rdbBindColorToMasterTrue').click(function (e) { ShowAllowColorText(); });
            $('#rdbSaveEditRemarksTrue').click(function (e) { ShowremarkText(); });
            $('#rdbConfirmDateTrue').click(function (e) { ShowDeliveryText(); });
            $('#rdbBindColorQtyTrue').click(function (e) { ShowcappingColorText(); });
            $('#chkStoreCopy').click(function (e) { ShowPrintStoreCopyText(); });
            $('#rdbShowOnReceiptTrue').click(function (e) { ShowRecptLogoText(); });
            $('#rdbHeaderSloganTrue').click(function (e) { ShowHeaderSloganText(); });
            $('#rdrBarcodeTrue').click(function (e) { ShowBarcodeText(); });
            $('#rdbPhoneNoTrue').click(function (e) { ShowPhoneText(); });
            $('#rdbPrintDueDateTrue').click(function (e) { ShowDueDateText(); });
            $('#rdbServicetaxTrue').click(function (e) { ShowServiceTaxText(); });
            $('#rdbTaxDetailTrue').click(function (e) { ShowTaxBifurcationText(); });
            $('#rdbBookingTimeTrue').click(function (e) { ShowBookingTimeText(); });
            $('#rdbPreviousTrue').click(function (e) { ShowpreviousDueText(); });
            $('#rdbRateTrue').click(function (e) { ShowPriceText(); });
            $('#rdbProcessTrue').click(function (e) { ShowProcessText(); });
            $('#rdbTableBorderTrue').click(function (e) { ShowReceiptBorderText(); });
            $('#rdbSubItemTrue').click(function (e) { ShowSubItemsText(); });
            $('#rdbCustomerSignatureTrue').click(function (e) { ShowCustomerSignText(); });
            $('#rdbTermConditionTrue').click(function (e) { ShowTermText(); });
            $('#rdbPrintTermsConditonTrue').click(function (e) { ShowStoreTermText(); });
            $('#chkAndroid').click(function (e) { ShowAndroidText(); });

        });

        function clearEmailMsg() {
            $('#lblEmailSuccess').text('');
            $('#lblEmailError').text('');
        }
        function clearStoreMsg() {
            $('#lblSuccess').text('');
            $('#lblErr').text('');
        }
        function clearReceiptMsg() {
            $('#lblMsg').text('');
            $('#lblReceiptErr').text('');
        }


        $('#TxtHdrFontsize').ace_spinner({ value: 8, min: 8, max: 30, step: 1, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });
        $('#txtAddFontSize').ace_spinner({ value: 8, min: 8, max: 16, step: 1, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });
        $('#TxtHeaderFontSize').ace_spinner({ value: 8, min: 8, max: 14, step: 1, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });
        $('#TxtFooterFontSize').ace_spinner({ value: 8, min: 8, max: 14, step: 1, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });
        $('#txtFontBookingNoSize').ace_spinner({ value: 9, min: 9, max: 24, step: 1, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });
        $('#txtNameFontSize').ace_spinner({ value: 9, min: 9, max: 12, step: 1, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });



        showAlllTooltipText();   
    </script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            $('#ctl00_ContentPlaceHolder1_fupStudentPhoto,#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_fupLogo').ace_file_input({
                no_file: 'No File ...',
                btn_choose: 'Choose',
                btn_change: 'Change',
                droppable: false,
                onchange: null,
                thumbnail: false,
                allowExt: ["jpeg", "jpg", "png", "gif"]
            });

            $('#ctl00_ContentPlaceHolder1_fupStudentPhoto').click(function () {
                clearStoreMsg();
            });

            $('#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_fupLogo').click(function () {
                clearReceiptMsg();
            });

            $("#ctl00_ContentPlaceHolder1_fupStudentPhoto").on('file.error.ace', function (ev, info) {
                if (info.error_count['ext'] || info.error_count['mime']) {
                    ShowStoreMsg('#FA8602', '#999999');
                    $('#lblSuccess').html('Invalid file type! Please select an image!');
                    $("#ctl00_ContentPlaceHolder1_fupStudentPhoto").focus();
                }
            });


            $("#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_fupLogo").on('file.error.ace', function (ev, info) {
                if (info.error_count['ext'] || info.error_count['mime']) {
                    ShowReceiptMsg('#FA8602', '#999999');
                    $('#lblReceiptErr').html('Invalid file type! Please select an image!');
                    $("#ctl00_ContentPlaceHolder1_tbReceipt_TabPanel1_fupLogo").focus();
                }
            });
        });
    </script>
</asp:Content>
