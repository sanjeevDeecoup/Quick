<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUserAndGarmentDetail.aspx.cs"
    MasterPageFile="~/WebsiteUser/WebsiteUserMain.Master" Inherits="QuickWeb.Website_User.frmUserAndGarmentDetail" %>

<%@ Register TagPrefix="uc" TagName="Duration" Src="~/Controls/Bootstrap_DurationControlDateWise.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (event) {
            $('#chkBookingNo').click(function () {
                if ($('#chkBookingNo').is(":checked")) {
                    $("#divBookingNo").show();
                    $("#txtBookingNo").focus();
                }
                else {
                    $("#divBookingNo").hide();
                }
            });
            var Date = $('#txtBirthDaate,#txtAnniversaryDate,#ctl00_ContentPlaceHolder1_uc1_txtReportFrom,#ctl00_ContentPlaceHolder1_uc1_txtReportUpto');
            Date.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd M yyyy",
                language: "tr"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });


            $('#txtPhone1').change(function () {
                $("#btnUpdateInformation").removeAttr("disabled").removeClass('btn btn-default2 btn-block btn-lg').addClass('btn btn-primary btn-block btn-lg');
            });
            $('#txtPhone2').change(function () {
                $("#btnUpdateInformation").removeAttr("disabled").removeClass('btn btn-default2 btn-block btn-lg').addClass('btn btn-primary btn-block btn-lg');
            });
            $('#txtEmailID').change(function () {
                $("#btnUpdateInformation").removeAttr("disabled").removeClass('btn btn-default2 btn-block btn-lg').addClass('btn btn-primary btn-block btn-lg');
            });
            $('#txtBirthDaate').change(function () {
                $("#btnUpdateInformation").removeAttr("disabled").removeClass('btn btn-default2 btn-block btn-lg').addClass('btn btn-primary btn-block btn-lg');
            });
            $('#txtAnniversaryDate').change(function () {
                $("#btnUpdateInformation").removeAttr("disabled").removeClass('btn btn-default2 btn-block btn-lg').addClass('btn btn-primary btn-block btn-lg');
            });
            $('#drpCommPre').change(function () {
                $("#btnUpdateInformation").removeAttr("disabled").removeClass('btn btn-default2 btn-block btn-lg').addClass('btn btn-primary btn-block btn-lg');
            });

        });
    </script>
    <script type="text/javascript">
        $(function (e) {
            $('#btnUpdateInformation').click(function (e) {
                /* To do : verify name not already exist and is valid, like stopping special character etc.  */

                var status = checkName();
                if (status == false) {
                    return false;
                }
                var unique = true;

                if ($('#chkBookingNo').is(":checked")) {
                    var bookingno = $('#txtBooking').val.length();
                    if (bookingno == "" || bookingno.length == 0) {
                        alert("Please enter booking no.");
                        $('#txtBooking').focus();
                        return false;
                    }
                }

                var tempphoneno = $('#hdntempNo').val();
                if (tempphoneno != txtPhone1.value.toLowerCase().trim()) {
                    $('#ddlRateList1 option').each(function (i, v) {
                        if (unique) {
                            unique = v.textContent.toLowerCase() != txtPhone1.value.toLowerCase().trim();
                        }
                    });
                }
                if (!unique) {
                    alert('This phone no already exists, please enter a different no');
                    txtPhone1.focus();
                    txtPhone1.select();
                    return false;
                }
            });
            $('#txtBookingNo').keypress(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                }
            });

            $("#btnShow").click(function () {
                if ($('#chkBookingNo').is(":checked")) {
                    var bookingno = $('#txtBookingNo').val();
                    if (bookingno == "") {
                        alert("Please enter Invoice number.");
                        $('#txtBookingNo').focus();
                        return false;
                    }
                }
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
        function checkName() {

            var strEmail = document.getElementById("<%=txtEmailID.ClientID %>").value.trim().length;
            var strPhn = document.getElementById("<%=txtPhone1.ClientID %>").value.trim().length;
            var CustomerPreference = document.getElementById("<%=drpCommPre.ClientID %>");
            var selectedText = CustomerPreference.options[CustomerPreference.selectedIndex].text;
            if (selectedText == "Only SMS") {
                if (strPhn == "" || strPhn.length == 0) {
                    $('#lblErr').text('Please enter Customer Phone no');
                    $("#txtPhone1").focus();
                    return false;
                }
            }
            if (selectedText == "Only Email") {
                if (strEmail == "" || strEmail.length == 0) {
                    $('#lblErr').text('Please enter Customer Email id');
                    $("#txtEmailID").focus();
                    return false;
                }
            }
            if (selectedText == "Both SMS and Email") {
                if (strPhn == "" || strPhn.length == 0) {
                    $('#lblErr').text('Please enter Customer Phone no');
                    $("#txtPhone1").focus();
                    return false;
                }
                if (strEmail == "" || strEmail.length == 0) {
                    $('#lblErr').text('Please enter Customer Email id');
                    $("#txtEmailID").focus();
                    return false;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblStoreName" runat="server" EnableTheming="false" />
    <asp:ScriptManager ID="script1" runat="server">
    </asp:ScriptManager>
    <div class="row-fluid ">
        <div class="row-fluid">
            <div class="row-fluid text-center" style="margin-bottom:5px">
            <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True"  CssClass="fa-lg"  ClientIDMode="Static" ForeColor="#009900" />
                <asp:Label ID="lblErr" runat="server" EnableTheming="false" CssClass="textRed" ClientIDMode="Static"
                    Font-Bold="True" />
            </div>

            <div class="row-fluid">
            <div class="col-sm-4">
                <div class="row-fluid" style="display: none">
                    <div class="col-sm-6  nopadding">
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="txtCode" runat="server" ClientIDMode="Static" CssClass="form-control"
                                disabled="disabled"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6  nopadding">
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="txtMemID" runat="server" ClientIDMode="Static" CssClass="form-control"
                                disabled="disabled"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-user fa-lg"></i></span>
                        <asp:TextBox ID="txtCustName" runat="server" ClientIDMode="Static" CssClass="form-control"
                            disabled="disabled"></asp:TextBox>
                    </div>
                </div>
                <div class="row-fluid div-margin2">
                    <div class="">
                        <asp:TextBox ID="txtCustAddress" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                            CssClass="form-control" disabled="disabled"></asp:TextBox>
                    </div>
                </div>
                <div class="row-fluid div-margin2">
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-chain-broken fa-lg"></i></span>
                        <asp:DropDownList ID="drpCommPre" runat="server" ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Text="None" Value="NA" />
                            <asp:ListItem Text="Only SMS" Value="SMS" />
                            <asp:ListItem Text="Only Email" Value="Email" />
                            <asp:ListItem Text="Both SMS and Email" Value="Both" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row-fluid">
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-phone fa-lg"></i></span>
                        <asp:TextBox ID="txtPhone1" placeholder="Enter Mobile no." runat="server" ClientIDMode="Static" MaxLength="20"
                            CssClass="form-control"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtPhone1_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Numbers" TargetControlID="txtPhone1">
                        </asp:FilteredTextBoxExtender>
                    </div>
                </div>
                <div class="row-fluid div-margin2">
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">
                            <img src="../images/landline.png" width="15px" height="18px" class="" /></span>
                        <asp:TextBox ID="txtPhone2" runat="server" placeholder="Enter Phone no." ClientIDMode="Static" MaxLength="20"
                            CssClass="form-control"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtPhone2_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Numbers" TargetControlID="txtPhone2">
                        </asp:FilteredTextBoxExtender>
                    </div>
                </div>
                <div class="row-fluid div-margin2">
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-envelope-o fa-lg"></i></span>
                        <asp:TextBox ID="txtEmailID" runat="server" placeholder="Enter Email ID" ClientIDMode="Static"
                            CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row-fluid div-margin2">
                    <div class="col-sm-6 nopadding">
                        <div class="input-group input-group-sm">
                            <span class="input-group-addon">
                                <img src="../images/Birthday.png" width="15px" height="18px" /></span>
                            <asp:TextBox ID="txtBirthDaate" runat="server" ClientIDMode="Static" onkeydown="return false;"
                                onpaste="return false;" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6 nopadding">
                        <div class="input-group input-group-sm">
                            &nbsp;<span class="input-group-addon" style="padding: 0px"><img src="../img/anniversary.png"
                                width="30px" height="28px" /></span>
                            <asp:TextBox ID="txtAnniversaryDate" runat="server" onkeydown="return false;" onpaste="return false;"
                                ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <span class="textBold">Pending</span>
                <div class="row-fluid well well-sm-tiny">
                    <div class="col-sm-3 text-center" style="padding-top: 5px; padding-left: 0px">
                        <%--  <span class="fa-2x" ><i class="fa fa-usd"></i></span>   --%>
                        <img src="../img/Currrency.png" width="40px" height="35px" />
                    </div>
                    <div class="col-sm-9 nomargin text-center" style="background-color: #9ABC32; color: White;
                        height: 40px;">
                        <span class="fa-2x textBold" id="btnAmount" runat="server"></span>
                    </div>
                </div>
                <div class="row-fluid well well-sm-tiny">
                    <div class="col-sm-3 text-center" style="padding-top: 2px; padding-left: 0px">
                        <img src="../images/Suit.png" width="40px" height="35px" />
                    </div>
                    <div class="col-sm-9 nomargin text-center" style="background-color: #6FB3E0; color: White;
                        height: 40px;">
                        <span id="btnCloth" runat="server" class="fa-2x textBold"></span>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <br />
                <br />
                <asp:Button ID="btnUpdateInformation" runat="server" Text="Update Information" EnableTheming="false"
                    disabled="disabled" CssClass="btn btn-default2 btn-block btn-lg" ClientIDMode="Static"
                    OnClick="btnUpdateInformation_Click" />
            </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="BorderTopLine">
        </div>
        <div class="row-fluid">
            <div class="form-inline">
                <div class="form-group">
                    <uc:Duration ID="uc1" runat="server"></uc:Duration>
                </div>
                <div class="form-group">
                    &nbsp;&nbsp;<asp:CheckBox ID="chkBookingNo" runat="server" Text="&nbsp;Booking No"
                        ClientIDMode="Static" />
                </div>
                <div class="form-group input-group-sm" id="divBookingNo" style="display: none">
                    <asp:TextBox ID="txtBookingNo" runat="server" ClientIDMode="Static" MaxLength="10"
                        CssClass="form-control" placeholder="Enter Booking No."></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtBookingNo_FilteredTextBoxExtender" runat="server"
                        Enabled="True"  TargetControlID="txtBookingNo" ValidChars="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-">
                    </asp:FilteredTextBoxExtender>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnShow" runat="server" Text="Show Order" EnableTheming="false" CssClass="btn btn-info textBold"
                        ClientIDMode="Static" OnClientClick="return checkName();" OnClick="btnShow_Click" />
                </div>
            </div>
        </div>
        <div class="BorderTopLine">
        </div>
        <div class="row-fluid">
            <asp:GridView ID="grdOrderDetails" runat="server" class="table table-striped table-bordered table-hover well div-margin"
                AllowPaging="true" EnableTheming="false" AutoGenerateColumns="false" EmptyDataText="No Record Found.."
                PageSize="8" OnRowCommand="grdOrderDetails_RowCommand" OnPageIndexChanging="grdOrderDetails_PageIndexChanging">
                <pagerstyle cssclass="gridview" HorizontalAlign="Center">
                 </pagerstyle>
                <Columns>
                    <asp:TemplateField HeaderText="Order No.">
                        <ItemTemplate>
                            <asp:LinkButton ID="hypBtnShowDetails" runat="server" Text='<%# Bind("BookingNumber") %>'
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ClientIDMode="Static"
                                CommandName="LinkButton" />
                            <asp:HiddenField ID="hdnDueDate" runat="server" Value='<%# Eval("DueDate") %>' />
                            <asp:HiddenField ID="hdnpcs" runat="server" Value='<%# Eval("Pcs") %>' />
                            <asp:HiddenField ID="hdnAmount" runat="server" Value='<%# Eval("Amount") %>' />
                        </ItemTemplate>
                        <HeaderStyle BackColor="#E4E4E4" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BookingDate" HeaderText="Order Date" HeaderStyle-BackColor="#E4E4E4" InsertVisible="False"
                        ReadOnly="True" SortExpression="BookingDate" />
                    <asp:BoundField DataField="DueDate" HeaderText="DueDate" InsertVisible="False" ReadOnly="True" HeaderStyle-BackColor="#E4E4E4"
                        SortExpression="DueDate" />
                    <asp:BoundField DataField="ReadyOn" HeaderText="Ready on" InsertVisible="False" ReadOnly="True" HeaderStyle-BackColor="#E4E4E4"
                        SortExpression="ReadyOn" />
                    <asp:BoundField DataField="Pcs" HeaderText="Pcs" InsertVisible="False" ReadOnly="True" HeaderStyle-BackColor="#E4E4E4"
                        SortExpression="Pcs" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" InsertVisible="False" ReadOnly="True" HeaderStyle-BackColor="#E4E4E4"
                        SortExpression="Amount" />
                    <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" InsertVisible="False" HeaderStyle-BackColor="#E4E4E4"
                        ReadOnly="True" SortExpression="DeliveryStatus" />
                    <asp:BoundField DataField="DuePcs" HeaderText="Due Pcs" InsertVisible="False" ReadOnly="True" HeaderStyle-BackColor="#E4E4E4"
                        SortExpression="DuePcs" />
                    <asp:BoundField DataField="DueAmount" HeaderText="Due Amount" InsertVisible="False" HeaderStyle-BackColor="#E4E4E4"
                        ReadOnly="True" SortExpression="DueAmount" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="row-fluid" id="divDetails" runat="server" visible="false" clientidmode="Static">
            <div class="row-fluid well well-sm-tiny" style="background-color: #DDDDDD">
                <div class="col-sm-2">
                </div>
                <div class="col-sm-2 ">
                    <span class="textBold">Order Number :</span>&nbsp;&nbsp
                    <asp:Label ID="lblInvoiceNumber" runat="server" EnableViewState="false" CssClass="TextGray textBold"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <span class="textBold">Pcs :</span>&nbsp;&nbsp
                    <asp:Label ID="lblPcs" runat="server" EnableViewState="false" CssClass="TextGray textBold"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <span class="textBold">Amount :</span>&nbsp;&nbsp
                    <asp:Label ID="lblAmount" runat="server" EnableViewState="false" CssClass="TextGray textBold"></asp:Label>
                </div>
                <div class="col-sm-3">
                    <span class="textBold">Delivery Date :</span>&nbsp;&nbsp
                    <asp:Label ID="lblDeliveryDate" runat="server" EnableViewState="false" CssClass="TextGray textBold"></asp:Label>
                </div>
            </div>
            <div class="row-fluid">
                <div class="col-sm-6 nopadding">
                    <asp:GridView ID="grdClothDetails" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover well"
                        EnableTheming="false">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Garment" InsertVisible="False" ReadOnly="True"
                                SortExpression="ItemName" />
                            <asp:BoundField DataField="ItemProcessType" HeaderText="Services" InsertVisible="False"
                                ReadOnly="True" SortExpression="ItemProcessType" />
                            <asp:BoundField DataField="DeliverItemStaus" HeaderText="Status" InsertVisible="False"
                                ReadOnly="True" SortExpression="DeliverItemStaus" />
                            <asp:BoundField DataField="ReadyOn" HeaderText="Ready on" InsertVisible="False" ReadOnly="True"
                                SortExpression="ReadyOn" />
                            <asp:BoundField DataField="Date" HeaderText="Deliver On" InsertVisible="False" ReadOnly="True"
                                SortExpression="Date" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-sm-6">
                    <asp:GridView ID="grdPaymentDetails" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover well"
                        EnableTheming="false">
                        <Columns>
                            <asp:BoundField DataField="PaidOn" HeaderText="Paid On" InsertVisible="False" ReadOnly="True"
                                SortExpression="PaidOn" />
                            <asp:BoundField DataField="Payment" HeaderText="Payment" InsertVisible="False" ReadOnly="True"
                                SortExpression="Payment" />
                            <asp:BoundField DataField="PaymentType" HeaderText="Payment Mode" InsertVisible="False"
                                ReadOnly="True" SortExpression="PaymentType" />
                            <asp:BoundField DataField="PaymentDetails" HeaderText="Payment Details" InsertVisible="False"
                                ReadOnly="True" SortExpression="PaymentDetails" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <asp:DropDownList ID="ddlRateList1" runat="server" ClientIDMode="Static" Style="visibility: hidden">
        </asp:DropDownList>
        <asp:HiddenField ID="hdntempNo" runat="server" ClientIDMode="Static" />
    </div>
</asp:Content>
