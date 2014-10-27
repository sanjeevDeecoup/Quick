<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="frmWorkShopNotePrint.aspx.cs" Inherits="QuickWeb.Factory.frmWorkShopNotePrint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <h3>
                    View/Print Workshop Notes
                </h3>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span4">
                <div class="input-prepend input-append span6">
                    <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" CssClass="span2 aligncenter"
                        GroupName="radReportOptions" />
                    <span class="add-on">From</span>
                    <asp:TextBox ID="txtReportFrom" runat="server" onkeypress="return false;" onpaste="return false;"
                        onchange="return SetUptoDate();" CssClass="span6" />
                    <span class="add-on">
                        <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" /></span><cc1:CalendarExtender
                            ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                            Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                        </cc1:CalendarExtender>
                </div>
                <div class="input-prepend input-append span6">
                    <span class="add-on">To</span>
                    <asp:TextBox ID="txtReportUpto" runat="server" onkeypress="return false;" onpaste="return false;"
                        CssClass="span6" />
                    <span class="add-on">
                        <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" /></span>
                    <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                        Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                    </cc1:CalendarExtender>
                </div>
            </div>
            <div class="span4">
                <div class="input-prepend input-append span6">
                    <asp:RadioButton ID="radReportMonthly" runat="server" GroupName="radReportOptions"
                        CssClass="span2 aligncenter" />
                    <span class="add-on">Monthly Report</span>
                    <asp:DropDownList ID="drpMonthList" runat="server" CssClass="span6">
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
                    <asp:DropDownList ID="drpYearList" runat="server" CssClass="span4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="span2">
                <asp:DropDownList ID="drpShopName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpShopName_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="span1">
                <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                    CssClass="btn btn-block btn-info" EnableTheming="False" OnClick="btnShowReport_Click" />
            </div>
        </div>
        <div class="row-fluid ">
            <div class="span12">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="label-important"></asp:Label>
                <div class="row-fluid form-signin4 no-bottom-margin">
                    <div class="span12 well well-small no-bottom-margin">
                        <div class="gridhight">
                            <asp:GridView ID="grdReport" runat="server" Visible="False" ShowFooter="False" EmptyDataText="No Record Found"
                                EnableTheming="false" PageSize="50" CssClass="mgrid" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="ChallanDate" HeaderText="Date" SortExpression="ChallanDate" />
                                    <asp:BoundField DataField="ChallanNumber" HeaderText="Delivery Note No" SortExpression="ChallanNumber" />
                                    <asp:TemplateField HeaderText="Print Out Format">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpOption" runat="server">
                                                <asp:ListItem Text="Invoice Based Detailed"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnShowChallan" runat="server" Text="Preview" CssClass="btn-info"
                                                OnClick="btnShowChallan_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <asp:Button ID="btnExport" runat="server" Visible="false" CssClass="btn-danger" Text="Export to Excel"
                    EnableTheming="false" />
            </div>
        </div>
    </div>
    <div class="container-fluid">
        This is dummy Content?
    </div>
</asp:Content>