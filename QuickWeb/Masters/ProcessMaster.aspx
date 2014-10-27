<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="ProcessMaster.aspx.cs" Inherits="QuickWeb.ProcessMaster" EnableEventValidation="false"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkEntry() {
            var strcode = document.getElementById("<%=txtProcessCode.ClientID %>").value;
            var strname = document.getElementById("<%=txtProcessName.ClientID %>").value;
            if (strname == "") {
                setDivMouseOver('Red', '#999999')
                $('#lblErr').text('Kindly enter service name.');
                document.getElementById("<%=txtProcessName.ClientID %>").focus();
                return false;
            }
            if (strcode == "") {
                setDivMouseOver('Red', '#999999')
                $('#lblErr').text('Kindly enter service short code.');
                document.getElementById("<%=txtProcessCode.ClientID %>").focus();
                return false;
            }            
        }

        function SetImage(ImageName) {
            var image = ImageName.title;
            var ImageUrl = document.getElementById('imgcheck').src = "AllImages/" + image;
            document.getElementById('ctl00_ContentPlaceHolder1_hdnpath').value = ImageUrl;
            return false;
        }
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }

        function setCheckBoxesText() {
            if ($('#ChkReady').is(':checked')) {
                $('#readyTxt').text("Mark Clothes ready automatically. I don't want to create workshop note. ");
                $('#readyTxt').removeClass('txtColor');
            }
            else {
                $('#readyTxt').text("Don’t mark clothes ready automatically. I want to create workshop note.");               
                $('#chkChallan').attr('disabled', false);
                $('#readyTxt').addClass('txtColor');
            }
            if ($('#chkChallan').is(':checked')) {
                $('#ChallanTxt').text("Factory/vendor process applicable, this Service requires garment to be sent to a Factory or Vendor.");
                $('#ChallanTxt').removeClass('txtColor');
            }
            else {
                $('#ChallanTxt').text("Factory/vendor process is not applicable on this service.");
                $('#ChallanTxt').addClass('txtColor');
            }
            if ($('#chkDiscount1').is(':checked')) {
                $('#DisTxt').text("Discount will be applicable.");
                $('#DisTxt').removeClass('txtColor');
            }
            else {
                $('#DisTxt').text("Discount will not be applicable on this service.");
                $('#DisTxt').addClass('txtColor');
            }
            if ($('#chkServiceTax').is(':checked')) {
                $('#ServiceTxt').text("Taxes are applicable on this service and i want to configure tax rates.");
                $('#ServiceTxt').removeClass('txtColor');
            }
            else {
                $('#ServiceTxt').text("I don't collect taxes on this service.");
                $('#ServiceTxt').addClass('txtColor');
            }
        }

        function clearMsg() {

            $('#lblErr').text('');
            $('#lblMsg').text('');
            $('#lblSaveOption').text('');
            $('#lblUpdateId').text('');
        }
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            $("#<%=grdSearchResult.ClientID%> tr:nth-child(odd)").css("background-color", "white")
            $("#<%=grdSearchResult.ClientID%> tr:nth-child(even)").css("background-color", "#F0F4FB");
            setCheckBoxesText();
            $('#ChkReady').click(function (e) {
                if ($('#ChkReady').prop('checked')) {
                    $("#chkChallan").prop("checked", false);
                    $('#chkChallan').attr('disabled', true);
                    $('#readyTxt').text("Mark Clothes ready automatically. I don't want to create workshop note.");
                    $('#readyTxt').removeClass('txtColor');
                    $('#ChallanTxt').addClass('txtColor');

                } else {
                    $('#readyTxt').text("Don’t mark clothes ready automatically. I want to create workshop note.");                   
                    $('#readyTxt').addClass('txtColor');
                    $('#chkChallan').attr('disabled', false);

                }
            });

            $('#chkChallan').click(function (e) {
                if ($('#chkChallan').prop('checked')) {
                    $('#ChallanTxt').text("Factory/vendor process applicable, this Service requires garment to be sent to a Factory or Vendor.");
                    $('#ChallanTxt').removeClass('txtColor');
                } else {
                    $('#ChallanTxt').text("Factory/vendor process is not applicable on this service.");
                    $('#ChallanTxt').addClass('txtColor');
                }
            });


            $('#chkDiscount1').click(function (e) {
                if ($('#chkDiscount1').prop('checked')) {
                    $('#DisTxt').text("Discount will be applicable.");
                    $('#DisTxt').removeClass('txtColor');
                } else {
                    $('#DisTxt').text("Discount will not be applicable on this service.");
                    $('#DisTxt').addClass('txtColor');
                }
            });

            $('#chkServiceTax').click(function (e) {
                if ($('#chkServiceTax').prop('checked')) {
                    $('#ServiceTxt').text("Taxes are applicable on this service and i want to configure tax rates.");
                    $('#ServiceTxt').removeClass('txtColor');
                } else {
                    $('#ServiceTxt').text("I don't collect taxes on this service.");
                    $('#ServiceTxt').addClass('txtColor');
                }
                if ($(this).is(':checked')) {
                    $('#divServiceTax').show();
                }
                else {
                    $('#divServiceTax').hide();
                }
            });


            $('#btnSave,#btnEdit,#btnDelete').click(function (e) {
                var clickedId = $(this).attr("id");
                clearMsg();
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                if (clickedId == 'btnSave') {
                    var status = checkEntry();
                    if (status == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnSave', null);
                }
                else if (clickedId == 'btnEdit') {
                    var status3 = checkEntry();
                    if (status3 == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnUpdate', null);
                }
                else if (clickedId == 'btnDelete') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnDelete', null);
                }
            });

        });
    </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus" class="well well-sm-tiny">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="textmargin">
                    <span style="margin-left: 40%">
                        <asp:Label ID="lblErr" ClientIDMode="static" runat="server" EnableViewState="False"
                            ForeColor="White" Font-Bold="True" />
                        <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                            Font-Bold="True" />
                        <asp:Label ID="lblSaveOption" ClientIDMode="static" runat="server" Visible="False"
                            ForeColor="White"></asp:Label>
                        <asp:Label ID="lblUpdateId" ClientIDMode="static" runat="server" Visible="False"
                            ForeColor="White"></asp:Label>
                    </span><span style="margin-left: -20%"></span>
                </h4>
            </div>
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="col-sm-10">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Service Creation</h3>
                </div>
                <div class="panel-body">                   
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 Textpadding">
                            <div class="input-group input-sm Textpadding">
                                <span class="input-group-addon IconBkColor">&nbsp;&nbsp;&nbsp;&nbsp;Service Name&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                <asp:TextBox ID="txtProcessName" runat="server" MaxLength="30" CssClass="form-control"
                                    placeholder="Create Services like Dry Cleaning, Laundry etc." />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                    TargetControlID="txtProcessName" ValidChars="abcdefghijklmnopqrstuvwzxyABCDEFGHIJKLMNOPQRSTUVWZXY 1234567890.">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                     <div class="row-fluid div-margin">
                        <div class="row-fluid">
                            <div class="col-sm-11 Textpadding">
                                <div class="input-group input-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">Service Short Code</span>
                                    <asp:TextBox ID="txtProcessCode" runat="server" MaxLength="15" CssClass="form-control"
                                        placeholder="Define a short name of the service to make order generation faster. Like DC for Dry Cleaning" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                        TargetControlID="txtProcessCode" ValidChars="abcdefghijklmnopqrstuvwzxyABCDEFGHIJKLMNOPQRSTUVWZXY 1234567890.">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-sm-1 Textpadding">
                                <span class="span textBold">&nbsp;*</span>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-1 Textpadding">
                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="" runat="server" id="ChkReady" />
                            <span class="lbl"></span>
                        </div>
                        <div class="col-sm-12 Textpadding">
                            <asp:Label ID="readyTxt" runat="server" ClientIDMode="Static">
                                </asp:Label>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-1 Textpadding">
                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="" runat="server" id="chkChallan" />
                            <span class="lbl"></span>
                        </div>
                        <div class="col-sm-12 Textpadding">
                           <asp:Label ID="ChallanTxt" runat="server" ClientIDMode="Static"></asp:Label>                                
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-1 Textpadding">
                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="" runat="server" id="chkDiscount1" />
                            <span class="lbl"></span>
                        </div>
                        <div class="col-sm-12 Textpadding">
                            <asp:Label ID="DisTxt" runat="server" ClientIDMode="Static"></asp:Label>                               
                        </div>
                    </div>
                    <div class="row-fluid div-margin" id="trOutSourced" runat="server" visible="false">
                        <div class="col-sm-1 Textpadding">
                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="" runat="server" id="chkDiscount" />
                            <span class="lbl"></span>
                        </div>
                        <div class="col-sm-12 Textpadding">
                            <p>
                                &nbsp;Out Sourced</p>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-1 Textpadding">
                            <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="" runat="server" id="chkServiceTax" />
                            <span class="lbl"></span>
                        </div>
                        <div class="col-sm-12 Textpadding">
                            <asp:Label ID="ServiceTxt" runat="server" ClientIDMode="Static"></asp:Label>
                        </div>
                    </div>
                    <div class="row-fluid div-margin" id="divServiceTax" runat="server" clientidmode="Static" style="display:none">
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">
                                    <asp:Label ID="lblFirstTax" runat="server" EnableViewState="false"></asp:Label></span>
                                <div class="row-fluid">
                                    <div class="col-sm-10 Textpadding">
                                        <asp:TextBox ID="txtServiceTax" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="txtServiceTax_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" TargetControlID="txtServiceTax" ValidChars="1234567890.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-sm-2 Textpadding">
                                        <span class="span fa-lg paddingtop10">&nbsp;%</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">
                                    <asp:Label ID="lblSecondTax" runat="server" EnableViewState="false"></asp:Label></span>
                                <div class="row-fluid">
                                    <div class="col-sm-10 Textpadding">
                                        <asp:TextBox ID="txtCSS" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="txtCSS_FilteredTextBoxExtender" runat="server" Enabled="True"
                                            TargetControlID="txtCSS" ValidChars="1234567890.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-sm-2 Textpadding">
                                        <span class="span fa-lg paddingtop10">&nbsp;%</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">
                                    <asp:Label ID="lblThird" runat="server" EnableViewState="false"></asp:Label></span>
                                <div class="row-fluid">
                                    <div class="col-sm-10 Textpadding">
                                        <asp:TextBox ID="txtECESJ" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="txtECESJ_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" TargetControlID="txtECESJ" ValidChars="1234567890.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-sm-2 Textpadding">
                                        <span class="span fa-lg paddingtop10">&nbsp;%</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:CheckBox ID="chkUseForVendorReport" runat="server" Text="Use In Vendor Report"
                    Checked="true" CssClass="TDCaption" Visible="False" />
                <div class="panel-footer">
                    <a class="btn btn-info" id="btnSave" runat="server" clientidmode="Static"><i class="fa fa-floppy-o">
                    </i>&nbsp;&nbsp;Save</a> <a class="btn btn-info" id="btnEdit" runat="server" visible="False"
                        clientidmode="Static"><i class="fa fa fa-pencil-square-o"></i>&nbsp;&nbsp;Update</a>
                    <a class="btn btn-info" id="btnDelete" runat="server" visible="False" clientidmode="Static">
                        <i class="fa fa-trash-o"></i>&nbsp;&nbsp;Delete</a>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny">
            <asp:GridView ID="grdSearchResult" runat="server" EnableTheming="false" CssClass="table table-striped table-bordered table-hover"
                AllowPaging="true" AutoGenerateColumns="False" EmptyDataText="There are no data records to display." OnSorting="grdSearchResult_Sorting" OnSorted="grdSearchResult_Sorted"
                PageSize="50"  AllowSorting="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged" OnPageIndexChanging="grdSearchResult_PageIndexChanging">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" HeaderStyle-BackColor="#808080" />
                     <asp:BoundField DataField="ProcessName" HeaderText="Name" ReadOnly="True" SortExpression="ProcessName"
                        HeaderStyle-BackColor="#808080" />
                    <asp:BoundField DataField="ProcessCode" HeaderText="Code" ReadOnly="True" SortExpression="ProcessCode"
                        HeaderStyle-BackColor="#808080" />                   
                         <asp:BoundField DataField="IsChallanByPass" HeaderText="Mark Ready Automatically"
                        ReadOnly="True" HeaderStyle-BackColor="#808080" />
                         <asp:BoundField DataField="IsChallan" HeaderText="Factory/Vendor Process Applicable"
                        ReadOnly="True" HeaderStyle-BackColor="#808080" />
                         <asp:BoundField DataField="IsDiscount" HeaderText="Discount" ReadOnly="True"
                        HeaderStyle-BackColor="#808080" />
                    <asp:TemplateField HeaderText="Out Sourced" Visible="false" HeaderStyle-BackColor="#808080">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkForDiscount" runat="server" Checked='<%# Bind("Discount") %>'
                                Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="IsActiveServiceTax" HeaderText="Tax" ReadOnly="True"
                        HeaderStyle-BackColor="#808080" />
                    <asp:BoundField DataField="ServiceTax" ReadOnly="True" SortExpression="ServiceTax"
                        HeaderStyle-BackColor="#808080" />
                    <asp:BoundField DataField="CssTax" ReadOnly="True" SortExpression="CssTax" HeaderStyle-BackColor="#808080" />
                    <asp:BoundField DataField="EcesjTax" ReadOnly="True" SortExpression="EcesjTax" HeaderStyle-BackColor="#808080" />                   
                </Columns>
                <HeaderStyle Font-Size="12px" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="hdnSelectedProcessCode" runat="server" />
    <asp:HiddenField ID="hdnStorePathTemp" runat="server" />
    <asp:HiddenField ID="hdnSaveImage" runat="server" />
    <asp:HiddenField ID="hdntest" runat="server" Value="0" />
    <asp:SqlDataSource ID="SqlGridSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"></asp:SqlDataSource>
</asp:Content>
