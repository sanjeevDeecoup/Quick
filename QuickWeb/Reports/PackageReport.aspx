<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="PackageReport.aspx.cs" Inherits="QuickWeb.Reports.PackageReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <link href="../css/loader.css" rel="stylesheet" type="text/css" />    
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">       
        .tooltip-inner {
            max-width: 120px;   
            width: 120px; 
            background-color:#F1F1F1;
            text-align:left;
            font-size:14px;
            color:Black;
            border: 1px solid #C0C0A0;
            }
    </style>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (e) {
            $("#PkgTypeInfo").tooltip({
                title: 'Package Type',
                html: true
            });   
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding div-margin" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="textmargin">
                    <span style="margin-left: 40%">
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                    </span><span style="margin-left: -20%"></span>
                </h4>
            </div>
        </div>
    </div>
    <div class="panel panel-primary ">
        <div class="panel-heading">
            <h3 class="panel-title">
                Package Report</h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid">
                <div class="col-sm-2  input-group">
                   <span class="input-group-addon IconBkColor"><i id="PkgTypeInfo"  data-placement="right" class="fa fa fa-info-circle fa-lg"></i></span>
                    <asp:DropDownList ID="drpPackageType" runat="server" OnSelectedIndexChanged="drpPackageType_SelectedIndexChanged"
                        CssClass="form-control" AutoPostBack="true">
                        <asp:ListItem Text="Discount" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Value / Benefit" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Flat Qty" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                    <asp:TextBox ID="txtCustName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                        CssClass="form-control" placeholder="Filter records by Customer" OnTextChanged="txtCustName_TextChanged"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustName"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="1"
                        UseContextKey="true" ContextKey="All" CompletionInterval="10" CompletionSetCount="15"
                        FirstRowSelected="True" CompletionListCssClass="AutoExtender_new" CompletionListItemCssClass="AutoExtenderList_new"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-sm-3 input-group">
                    <span class="input-group-addon IconBkColor"><i class="fa fa-suitcase fa-lg"></i></span>
                    <asp:TextBox ID="txtPackageName" runat="server" AutoPostBack="True" onfocus="javascript:select();"
                        CssClass="form-control" placeholder="Filter records by Package" OnTextChanged="txtPackageName_TextChanged"></asp:TextBox>
                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete2" TargetControlID="txtPackageName"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetPackageName" MinimumPrefixLength="1"
                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-sm-2 input-group">
                    <span class="input-group-addon IconBkColor">Active</span>
                    <asp:DropDownList ID="drpActive" runat="server" OnSelectedIndexChanged="drpActive_SelectedIndexChanged"
                        CssClass="form-control" AutoPostBack="true">
                        <asp:ListItem Text="Yes" />
                        <asp:ListItem Text="No" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:LinkButton ID="lnkSendSMS" class="btn btn-primary" EnableTheming="false" runat="server"
                        ClientIDMode="Static" Visible="true" OnClick="lnkSendSMS_Click"><i class="fa fa-envelope-o fa-lg"></i>&nbsp;Send Reminder</asp:LinkButton>
                </div>
            </div>
            <br />
            <div class="row-fluid" style="height: 355px; background-color: White; overflow: auto">
                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="true" ShowFooter="true"
                    EmptyDataText="No package record found." PageSize="50" CssClass="Table Table-striped Table-bordered Table-hover"
                    OnRowDataBound="grdReport_RowDataBound" EnableTheming="false" OnDataBound="grdReport_OnDataBound"
                    OnDataBinding="grdReport_OnDataBinding">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckAll" runat="server" onclick="checkAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer">
                           <%-- <HeaderTemplate>
                                <asp:Label ID="lblHeader" runat="server" Text="Customer"></asp:Label>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:HyperLink ID="hplNav" runat="server" Target="_blank"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#F0F0F0" Font-Bold="true" />
                    <HeaderStyle Font-Size="12px" />
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">
            <asp:LinkButton ID="btnExport" class="btn btn-primary" EnableTheming="false" runat="server"
                ClientIDMode="Static" Visible="true" OnClick="btnExport_Click"><i class="fa fa-th fa-lg"></i> Export to Excel</asp:LinkButton>
        </div>
        <asp:Panel ID="pnlPackageDetails" runat="server" ClientIDMode="Static" Style="display: none"
            Width="340px">
            <div class="popup_Body">
                <%--<table class="TableData">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" class="TDCaption">Item</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" class="TDCaption">Qty</asp:Label>
                        </td>
                    </tr>
                </table>--%>
            </div>
        </asp:Panel>
    </div>
    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlSourceItems" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [ItemID], [ItemName] FROM [ItemMaster] ORDER BY ItemName">
    </asp:SqlDataSource>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
    <asp:DropDownList ID="drpsmstemplateRenewal" runat="server" EnableTheming="false"
        ClientIDMode="Static" Style="display: none">
    </asp:DropDownList>
    <asp:DropDownList ID="drpsmstemplateMarkcomplete" runat="server" EnableTheming="false"
        ClientIDMode="Static" Style="display: none">
    </asp:DropDownList>
    <asp:Panel ID="pnlMsg" runat="server" Style="display: none" ClientIDMode="Static">
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div style="margin-top: -10px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="fa  textBold"> Please Wait..</span><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-spinner fa-spin fa-3x"></i>
                    <%--<img  src="../images/ajax-loader.gif" style="margin-top:5px;margin-left:30%" />--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div class="modal fade" id="modelpopRemname" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-dialog-center">
            <div class="modal-content">
                <div class="panel panel-primary popup2">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            Package Details
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true"><i class="fa fa-times-circle"></i></span><span class="sr-only">
                                    Close</span></button>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <div class="row-fluid">
                            <div class="col-sm-8  nopadding">
                                <table class="TableData">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" class="TDCaption">Item</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" class="TDCaption">Qty</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(function (e) {
            /*
            $('#pnlPackageDetails').dialog({
            autoOpen: false,
            height: 300,
            width: 350,
            modal: true
            });
            */

            $('#ctl00_ContentPlaceHolder1_grdReport').on('click', 'td', loadPackageDetails);

            function loadPackageDetails(e) {
                if ($('#ctl00_ContentPlaceHolder1_drpPackageType').val() !== '3' && $('#ctl00_ContentPlaceHolder1_drpPackageType').val() !== '4')
                    return;
                var assId = 0;
                if ($(e.target).index() === 2 /* && e.type == 'mouseenter' */) {
                    // find the packageid
                    assId = $(e.target).parent().find('a').attr('href');
                    assId = assId.slice(assId.lastIndexOf('PkgId') + 5 + 1, assId.lastIndexOf('&Type'));
                    // call json method
                    $.ajax({
                        url: '../Autocomplete.asmx/GetQtyPackageDetails',
                        type: 'GET',
                        contentType: 'application/json; charset=utf8',
                        data: "assignId='" + assId + "'",
                        dataType: 'JSON',
                        success: function (data, status, jqXHR) { openDialog(data.d, false); },
                        error: function (jqXHR, status, error) { openDialog(data, true); }
                    });
                }
            }

            function openDialog(data, isError) {
                var _html = '';
                var _header = '';
                if (isError) {
                    _html = 'Some error occured while fetching data. Please contact administrator';
                } // if not error
                else {
                    if ($('#ctl00_ContentPlaceHolder1_drpPackageType').val() === '4') { // if Flat Qty
                        _header = 'This Package has ' + data.split('_')[0].split(':')[1] + ' Garments allowed';
                        if (data.split('_')[0].split(':')[0] !== 'All') { // if All
                            _html += "<tr><td><span class=\"TDCaption\">" + '' + "</span></td></tr>"; // dummy row to pretty format
                            data.split('_').forEach(function (v, i) {
                                _html += "<tr><td><span class=\"TDCaption\">" + v.split(':')[0] + "</span></td></tr>";
                            });
                        }
                    }
                    else { // if not Flat Qty
                        data.split('_').forEach(function (v, i) {
                            _html += "<tr><td><span class=\"TDCaption\">" + v.split(':')[0] + "</span></td><td><span class=\"TDCaption\">" + v.split(':')[1] + "</span></td></tr>"
                        });
                    }
                }

                $('#modelpopRemname').find('table').prev().remove(); // remove previous span(s)
                $('#modelpopRemname').find('tbody').html(_html); // update html
                (_header !== '') && $('#modelpopRemname').find('table').before('<span  class=\"TDCaption\">' + _header + '</test>') // if _header is not null, then prepend it before

                $('#modelpopRemname').modal('toggle');
                //                $('#pnlPackageDetails').dialog({
                //                    title: "Package Details",
                //                    width: 360,
                //                    modal: true
                //                });
            }

            function gridviewCheck() {
                var grid = document.getElementById("<%= grdReport.ClientID %>");

                //variable to contain the cell of the grid

                var cell;
                var count = 0;


                if (grid.rows.length > 0) {

                    //loop starts from 1. rows[0] points to the header.

                    for (i = 1; i < grid.rows.length; i++) {

                        //get the reference of first column

                        cell = grid.rows[i].cells[0];



                        //loop according to the number of childNodes in the cell

                        for (j = 0; j < cell.childNodes.length; j++) {

                            //if childNode type is CheckBox                 

                            if (cell.childNodes[j].type == "checkbox") {

                                //assign the status of the Select All checkbox to the cell checkbox within the grid

                                if (cell.childNodes[j].checked == true) {
                                    count = +1;

                                }

                            }

                        }

                    }

                }
                return count;
            }

            $('#<% =lnkSendSMS.ClientID%>').click(function () {
                clearMsg();
                var count = gridviewCheck();
                if (count == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly select at least one customer.');
                    return false;
                }
                else {
                    $('#pnlMsg').dialog({ width: 100, height: 120, modal: true });
                }
            });

        });
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
        function clearMsg() {
            $('#lblMsg').text('');
        }
    </script>
</asp:Content>
