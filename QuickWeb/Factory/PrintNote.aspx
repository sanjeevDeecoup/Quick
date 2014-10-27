<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="PrintNote.aspx.cs" Inherits="QuickWeb.Factory.PrintNote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <div class="container-fluid margin-left-right">
        
        <div class="row-fluid">
            <div class="span4 well well-small">
                <div class="row-fluid">
                    <div class="span7">
                      <div class="span4">
                        <asp:RadioButton ID="radReportFrom" runat="server" Checked="True" GroupName="radReportOptions" />
                        <big>From</big>
                     </div>
                        <asp:TextBox ID="txtReportFrom" runat="server" onkeypress="return false;" onpaste="return false;"
                            CssClass="span6 form-control" />
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtReportFrom"
                            Format="dd MMM yyyy">
                        </cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="datecal1" runat="server" PopupButtonID="datepicker" Format="dd MMM yyyy"
                            TargetControlID="txtReportFrom">
                        </cc1:CalendarExtender>
                        <i class="fa fa-calendar fa-lg" runat="server"  id="datepicker" style="margin-left:5px"></i>
                    </div>
                    <div class="span5">
                     <div class="span2">
                        <big>To </big>
                       </div>
                        <asp:TextBox ID="txtReportUpto" runat="server" onkeypress="return false;" onpaste="return false;"
                            CssClass="span8 form-control" />
                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtReportUpto"
                            Format="dd MMM yyyy">
                        </cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="datepicker1"
                            Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                        </cc1:CalendarExtender>
                        <i class="fa fa-calendar fa-lg" runat="server" id="datepicker1" style="margin-left:5px"></i>
                    </div>
                </div>
            </div>
            <div class="span4 well well-small">
                <div class="row-fluid">
                    <div class="span3">
                    <asp:RadioButton ID="radReportMonthly" runat="server" GroupName="radReportOptions" />
                    <big>Monthly</big>
                    </div>
                    <asp:DropDownList ID="drpMonthList" runat="server" CssClass="span5 form-control">
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
                    <asp:DropDownList ID="drpYearList" runat="server" CssClass="span4 form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="prnButton" class="span4" >
                <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                    CssClass="span3 btn  btn-info btn-lg" EnableTheming="False" OnClick="btnShowReport_Click" />
                <asp:Button ID="btnallPrintSticker1" runat="server" CssClass="span4 btn  btn-info btn-lg"
                    Text="Print Sticker" EnableTheming="False" 
                    OnClick="BtnPrintSticker"  Visible="false"/>
                    <asp:Button ID="btnallPrintChallan" runat="server" Text="Print Delivery Note" CssClass="span5 btn  btn-info btn-lg" 
                    EnableTheming="false" OnClick="btnallPrintChallan_Click" ClientIDMode="Static" Visible="false" />
            </div>
        </div>
        <div class="row-fluid ">
            <div class="span12">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="label-important"></asp:Label>
                <div class="row-fluid form-signin4 no-bottom-margin">
                    <div class="span12 well well-small no-bottom-margin">
                        <div class="gridhight">
                            <asp:GridView ID="grdReport" runat="server" Visible="False" ShowFooter="False" EmptyDataText="No Record Found"
                                ClientIDMode="Static" EnableTheming="false" PageSize="50" CssClass="mgrid" 
                                AutoGenerateColumns="false" >
                               
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ChallanDate" HeaderText="Date" SortExpression="ChallanDate" />
                                    <asp:BoundField DataField="ChallanNumber" HeaderText="Delivery Note No" SortExpression="ChallanNumber" />
                                    <asp:TemplateField HeaderText="Print Out Format">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpOption" runat="server" CssClass="form-control well-sm-tiny">
                                                <asp:ListItem Text="Invoice With Item Detailed"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workshop Remark">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblWorkshopRemark" Text='<%# Bind("WorkshopRemark") %>'>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="challanUserName" HeaderText="Created By"  />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnPrintSticker" runat="server" Text="Print Sticker" CssClass="btn btn-info"
                                                OnClick="BtnPrintSticker" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnShowChallan" runat="server" Text="Preview" CssClass="btn btn-info"
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
    </div>
    <asp:HiddenField runat="server" ID="hdnStartValue" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnChallanNum" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnAllChallanNum" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdncount" ClientIDMode="Static" />
    <asp:Panel ID="pnlPanel" runat="server" CssClass="modalPopup" Style="display: none"
        ClientIDMode="Static" BackImageUrl="~/App_Themes/Default/Images/Stage_BG_btm.png"
        Width="340px">
        <div class="popup_Titlebar" id="Div8">
            <div class="TitlebarLeft">
                Print Packing Sticker
            </div>
        </div>
        <div class="popup_Body">
            <table class="TableData">
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        <asp:Label ID="LabelLbl" Text="Start Printing from Sticker Number " runat="server"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="drpPrintStartFrom" runat="server" ClientIDMode="Static" Width="100px">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSaveAndPrintNew" Text="Print" runat="server" OnClick="BtnPrintSticker"
                            ClientIDMode="Static" />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <script type="text/javascript">


        $(function () {


            $('#chkSelectAll').on('change', function (e) {
                var allCheckBoxes = $(this).closest('table').find('[type="checkbox"]').not(':first');
                if (e.target.checked) {
                    allCheckBoxes.attr('checked', true);

                }
                else {
                    allCheckBoxes.attr('checked', false);

                }
            });

            $('#grdReport').on('change', '[type="checkbox"] :not(:first)', function (e) {


                var checkedLength = $(e.delegateTarget).find(':checked').length;
                if (checkedLength) {

                    chkSelectAll.checked = $('#grdReport').find(':checkbox').not($('#chkSelectAll')).length === $('#grdReport').find(':checked').not($('#chkSelectAll')).length;
                }
                else {

                    chkSelectAll.checked = false;
                }
            });

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
            $('#grdReport').on('click', '.btn-info', function (e) {
                if ($(e.target).val() !== 'Print Sticker') return;
                $('#pnlPanel').dialog({ width: 420, modal: true });
                $('#hdnChallanNum').val($(e.target).closest('tr').find('td').eq(2).text());
                var t3 = 0;
                $('#hdncount').val(t3);
               
                return false;
            });

            $('#prnButton').click(function (e) {
                if ($(e.target).val() !== 'Print Sticker') return;
                var count = gridviewCheck();
                if (count == 0) {
                    alert('Please select at least one workshop note');
                    return false;
                }
                $('#pnlPanel').dialog({ width: 420, modal: true });
                var t3 = 1;
                $('#hdncount').val(t3);                
                return false;
            });


            $('#btnSaveAndPrintNew').click(function (e) {
                //e.preventDefault();
                $('#pnlPanel').dialog('close');
                var t2 = $('#drpPrintStartFrom').val();
                $('#hdnStartValue').val(t2);
                __doPostBack('ctl00$ContentPlaceHolder1$btnSaveAndPrintNew', null);
                return;
            });

        });
    </script>
    <script type="text/javascript">
        $(function () {

            $('#grdReport').on('click', ':checkbox', function (e) {
                if (e.target.id === 'chkSelectAll') {
                    if (e.target.checked) {
                        $('#grdReport').find(':checkbox').attr('checked', true);
                    }
                    else {
                        $('#grdReport').find(':checkbox').attr('checked', false);
                    }
                }
            });
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
            $('#ctl00_ContentPlaceHolder1_btnallPrintSticker1').on('click', function (e) {
                var count = gridviewCheck();
                if (count == 0) {
                    alert('Please select at least one workshop note');
                    return false;
                }
            });

            $('#btnallPrintChallan').on('click', function () {

                var count = gridviewCheck();
                if (count == 0) {
                    alert('Please select at least one workshop note');
                    return false;
                }
            });

        });
    </script>
</asp:Content>
