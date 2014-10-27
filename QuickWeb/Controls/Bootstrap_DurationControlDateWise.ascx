<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Bootstrap_DurationControlDateWise.ascx.cs"
    Inherits="QuickWeb.Controls.Bootstrap_DurationControlDateWise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div>
    <div class="form-inline">
        <div class="form-group">
            <div class="input-group input-group-sm">
                <span class="input-group-addon IconBkColor">
                    <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions" />
                    &nbsp;From</span>
                <asp:TextBox ID="txtReportFrom" CssClass="form-control hasDatepicker" Height="33px"
                    onpaste="return false;" onkeypress="return false;" EnableTheming="false" runat="server"></asp:TextBox>
                <span class="input-group-addon IconBkColor textBold"><i class="fa fa-calendar fa-lg">
                </i></span>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group input-group-sm">
                <span class="input-group-addon IconBkColor">&nbsp;To</span>
                <asp:TextBox ID="txtReportUpto" CssClass="form-control hasDatepicker" onpaste="return false;"
                    Height="33px" onkeypress="return false;" EnableTheming="false" runat="server"></asp:TextBox>
                <span class="input-group-addon IconBkColor textBold"><i class="fa fa-calendar fa-lg">
                </i></span>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group input-group-sm">
                <span class="input-group-addon IconBkColor">
                    <asp:RadioButton ID="radReportMonthly" runat="server" GroupName="radReportOptions" />&nbsp;Monthly</span>
                <div class="row-fluid">
                    <div class="col-sm-7 Textpadding">
                        <asp:DropDownList ID="drpMonthList" runat="server" CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-5 Textpadding">
                        <asp:DropDownList ID="drpYearList" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
