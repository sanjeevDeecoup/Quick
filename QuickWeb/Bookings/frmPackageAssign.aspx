<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="frmPackageAssign.aspx.cs" Inherits="QuickWeb.Bookings.frmPackageAssign"
    EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>
     <style type="text/css">       
        .tooltip-inner {
            max-width: 300px;   
            width: 300px; 
            background-color:#F1F1F1;
            text-align:left;
            font-size:14px;
            color:Black;
            border: 1px solid #C0C0A0;
            }              
            .ajax__tab_tab
            {
            height:21px !important;
            font-weight:bold;
            }
    </style>
    <script type="text/javascript" language="javascript">
        function checkEntry() {
            //var StartValue = document.getElementById("<%=txtStartValue.ClientID %>").value.trim().length;
            var CurName = document.getElementById("<%=txtCustomerSearch.ClientID %>").value.trim().length;
            var StartDate = document.getElementById("<%=txtStartDate.ClientID %>").value.trim().length;
            var EndDate = document.getElementById("<%=txtEndDate.ClientID %>").innerHTML;
            var memberShip = document.getElementById("<%=txtMemberShip.ClientID %>").value.trim().length;
            try {
                var Recurrence = document.getElementById("<%=txtRecurrence.ClientID %>").value;
                var input = parseInt(Recurrence);
                if (input < 1 || input > 100) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Recurrence should be between 1 - 100.');
                    document.getElementById("<%=txtRecurrence.ClientID %>").focus();
                    return false;
                }
                if (Recurrence == "" || Recurrence.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter Recurrence.');
                    document.getElementById("<%=txtRecurrence.ClientID %>").focus();
                    return false;
                }
            } catch (ex) {

            }
            finally {

            }
            if (CurName == "" || CurName.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Please enter customer name.');
                document.getElementById("<%=txtCustomerSearch.ClientID %>").focus();
                return false;
            }
            if (memberShip == "" || memberShip.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Please enter membership id that you want to assign to the customer.');
                document.getElementById("<%=txtMemberShip.ClientID %>").focus();
                return false;
            }
            if (StartDate == "" || StartDate.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Kindly enter start date.');
                document.getElementById("<%=txtStartDate.ClientID %>").focus();
                return false;
            }
            if (EndDate == "" || EndDate.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Kindly enter store end date.');
                document.getElementById("<%=txtEndDate.ClientID %>").focus();
                return false;
            }
            var objFromDate = document.getElementById("<%=txtStartDate.ClientID %>").value;
            var objToDate = document.getElementById("<%=txtEndDate.ClientID %>").innerHTML;
            var date1 = new Date(objFromDate);
            var date2 = new Date(objToDate);

            var date3 = new Date();
            var date4 = date3.getMonth() + "/" + date3.getDay() + "/" + date3.getYear();
            var currentDate = new Date(date4);

            if (date1 > date2) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('End date should be less than start date.');
                document.getElementById("<%=txtEndDate.ClientID %>").focus();
                return false;
            }
           
            if ($('#chkSMS').is(':checked')) {
            var strMob = $('#txtCustomerMobile').val().trim();
            if (strMob == "") {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Please enter mobile number.');
                document.getElementById("<%=txtCustomerMobile.ClientID %>").focus();
                return false;
            }
          }

        if ($('#ChkEmail').is(':checked')) {
            var strEmail = $('#txtCustomerEmailId').val().trim();
            if (strEmail == "") {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text('Please enter email id.');
                document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                return false;
            }

            var valid = checkEmail(document.getElementById("<%=txtCustomerEmailId.ClientID %>").value);
            if (valid == false) {            
                setDivMouseOver('#FA8602', '#999999')
                $('#lblMsg').text('Not a valid e-mail address');
                document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                return false;
            }
        }           
          
        }
        function CheckCorrectDate() {

            //txtRecurrence
        }

        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 5000);
        }
        function clearMsg() {

            $('#lblErr').text('');
            $('#lblMsg').text('');
        }

        var checkEmail = function (value) {
            var valid = true;
            if (value.indexOf('@') == -1) {
                valid = false;
            } else {
                var parts = value.split('@');
                var domain = parts[1];
                if (domain.indexOf('.') == -1) {
                    valid = false;
                } else {
                    var domainParts = domain.split('.');
                    var ext = domainParts[1];
                    if (ext.length > 4 || ext.length < 2) {
                        valid = false;
                    }
                }
            }
            return valid;
        };

    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            var strTmpVar = $('#hdntmp').val();
            if (strTmpVar == "1") {
                $('#chkSMS').attr("disabled", "disabled");
                $('#ChkEmail').attr("disabled", "disabled");

//                if ($('#ChkEmail').is(':checked')) {
//                    $('#txtCustomerEmailId').removeAttr('disabled');
//                }
//                else {
//                    $('#txtCustomerEmailId').attr("disabled", "disabled");
//                }
//                if ($('#chkSMS').is(':checked')) {
//                    $('#txtCustomerMobile').removeAttr('disabled');
//                }
//                else {
//                    $('#txtCustomerMobile').attr("disabled", "disabled");
         //       }

                $('#hdntmp').val('0')
            }
            $("#txtStartDate").change(function () {
                var dateDiffer = $('#hdnDateDifference').val();
                var startDate = $('#txtStartDate').val();
                var endDate = $('#txtEndDate').text();
                var date = new Date(startDate);
                var dd = new Date(date.getFullYear(), date.getMonth(), date.getDate() + Number(dateDiffer));

                var dt = '';
                dt = dd.getDate().toString().length == 2 ? dd.getDate().toString() : '0' + dd.getDate().toString();
                dt += ' ' + Array.prototype.map.call([dd.getMonth()], function (val) {
                    switch (val) {
                        case 0: return 'Jan'; case 1: return 'Feb'; case 2: return 'Mar'; case 3: return 'Apr'; case 4: return 'May'; case 5: return 'Jun'; case 6: return 'Jul'; case 7: return 'Aug'; case 8: return 'Sep'; case 9: return 'Oct'; case 10: return 'Nov'; case 11: return 'Dec';
                    }
                })[0];
                dt += ' ' + dd.getFullYear();
                $('#txtEndDate').text(dt);
                //Store Hidden value in EndDate
                $('#hdnEndDate').val(dt);
            });


            $('#ctl00_ContentPlaceHolder1_btnSaveOnly,#ctl00_ContentPlaceHolder1_btnSave,#ctl00_ContentPlaceHolder1_btnUpdate,#ctl00_ContentPlaceHolder1_btnDelete,#ctl00_ContentPlaceHolder1_btnMarkComplete,#ctl00_ContentPlaceHolder1_btnPrintSlip').click(function (e) {
                clearMsg();
                var clickedId = $(this).attr("id");
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                if (clickedId == 'ctl00_ContentPlaceHolder1_btnSaveOnly') {
                    var status = checkEntry();
                    if (status == false) {
                        return false;
                    }
                    var mobstatus = checkmobileNo();
                    if (mobstatus == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnSaveOnly', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnSave') {
                    var status1 = checkEntry();
                    if (status1 == false) {
                        return false;
                    }
                    var mobstatus1 = checkmobileNo();
                    if (mobstatus1 == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnSave', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnUpdate') {
                    var status3 = checkEntry();
                    if (status3 == false) {
                        return false;
                    }
                    var mobstatus2 = checkmobileNo();
                    if (mobstatus2 == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnUpdate', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnDelete') {
                    //                    var status3 = checkEntry();
                    //                    if (status3 == false) {
                    //                        return false;
                    //                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnDelete', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnMarkComplete') {
                    //                    var status3 = checkEntry();
                    //                    if (status3 == false) {
                    //                        return false;
                    //                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnMarkComplete', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnPrintSlip') {
//                    var status3 = checkEntry();
//                    if (status3 == false) {
//                        return false;
//                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnPrintSlip', null);
                }

            });

            $('#chkSMS').click(function (e) {
                if ($('#chkSMS').is(':checked')) {
                    $('#txtCustomerMobile').removeAttr('disabled');
                    $('#txtCustomerMobile').focus();
                }
                else {
                    $('#txtCustomerMobile').attr("disabled", "disabled");
                }
            });

            $('#ChkEmail').click(function (e) {
                if ($('#ChkEmail').is(':checked')) {
                    $('#txtCustomerEmailId').removeAttr('disabled');
                    $('#txtCustomerEmailId').focus();
                }
                else {
                    $('#txtCustomerEmailId').attr("disabled", "disabled");
                }
            });


            $("#SmsInfo").tooltip({
                title: 'Select  “Yes” to send a confirmation email of the package purchase transaction to the customer’s registered  mobile number.'
            });
            $("#EmailInfo").tooltip({
                title: 'Select  “Yes” to send a confirmation email of the package purchase transaction to the customer’s registered email id.'
            });
        });
    </script>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {

        var Date = $('#txtStartDate');
        Date.datepicker({
            changeMonth: true,
            changeYear: true,
            format: "dd M yyyy",
            language: "tr",
            orientation: 'top right'
        }).on('changeDate', function (ev) {
            $(this).blur();
            $(this).datepicker('hide');
        });
    });



    function checkmobileNo() {
        var unique = true;
        var tempphoneno = $('#hdntempNo').val();
        if (tempphoneno != txtCustomerMobile.value.toLowerCase().trim()) {
            $('#ddlRateList1 option').each(function (i, v) {
                if (unique) {
                    unique = v.textContent.toLowerCase() != txtCustomerMobile.value.toLowerCase().trim();
                }
            });
        }
        if (!unique) {
            setDivMouseOver('Red', '#999999');
            $('#lblMsg').text('Mobile number already in use.');
            txtCustomerMobile.focus();
            txtCustomerMobile.select();
            return false;
        }
    
    }
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="DivContainerStatus">
                <div id="DivContainerInnerStatus" class="span label label-default">
                    <h4 class="text-center">
                         <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" EnableViewState="False"
                            ForeColor="White" Font-Bold="True" />
                             <asp:Label ID="lblErr" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                            Font-Bold="True" />
                              <asp:Label ID="lblAssignId" ClientIDMode="static" runat="server" Visible="False"
                            ForeColor="White"></asp:Label>
                    </h4>
                </div>
            </div>
        </div> 
    <div class="row-fluid div-margin">
        <div class="col-sm-7">
            <div class="panel panel-primary well-sm-tiny1">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Assign Package to customer</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group input-sm Textpadding">
                                <span class="input-group-addon IconBkColor">Package Name</span>
                                <asp:DropDownList ID="drpPackageName" CssClass="form-control" runat="server" AutoPostBack="true"
                              TabIndex="1"       OnSelectedIndexChanged="drpPackageName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group  labelBorder divheight backcolor2" id="divcost">
                                <span class="input-group-addon IconBkColor">Cost</span>
                                <p class="textmargin5">
                                    &nbsp;<asp:Label ID="lblCost" runat="server" CssClass="fontsize" ClientIDMode="Static"></asp:Label></p>
                            </div>
                        </div>
                        <div class="col-sm-3 Textpadding">
                            <div class="input-group  Textpadding labelBorder divheight backcolor2" id="divDiscount">
                                <span class="input-group-addon IconBkColor">
                                    <asp:Label ID="lblValueName" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblTotalAmount" runat="server" Text="Total Cost"></asp:Label>
                                </span>
                                <p class="textmargin5 paddingleft2">                                  
                                    <asp:Label ID="lblValue" runat="server" CssClass="fontsize">
                                    </asp:Label>
                                    <asp:Label ID="lblTotalAmountValue" runat="server" CssClass="fontsize" ClientIDMode="Static"></asp:Label>
                                    <asp:Label ID="lblstar" runat="server" CssClass="fontsize"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin" id="divRecurrence" runat="server" visible="false">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group  labelBorder divheight backcolor2" id='divCycle'>
                                <span class="input-group-addon IconBkColor">Cycle</span>
                                <p class="textmargin5">
                                    &nbsp;
                                    <asp:Label ID="txtRecurrence" runat="server" MaxLength="3" CssClass="fontsize" ClientIDMode="Static"
                                       ></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="col-sm-6 paddingRight">
                            <div class="input-group  labelBorder divheight backcolor2" id='divQtyCycle'>
                                <span class="input-group-addon IconBkColor">
                                    <asp:Label ID="lblTotalQtyText" runat="server" Text="Quantity / Cycle"></asp:Label></span>
                                <p class="textmargin5">
                                    &nbsp;
                                    <asp:Label ID="lblTotalQtyValue" runat="server" CssClass="fontsize"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 input-group input-group-sm  Textpadding">
                            <span class="input-group-addon IconBkColor"><i class="fa fa-search fa-lg"></i></span>
                            <asp:TextBox ID="txtCustomerSearch" TabIndex="2"  runat="server" AutoPostBack="True" onfocus="javascript:select();"
                                ToolTip="Select Customer" placeholder="Select Customer"
                                CssClass="form-control" OnTextChanged="txtCustomerSearch_TextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerSearch"
                                ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                                CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                            </cc1:AutoCompleteExtender>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="textRed">&nbsp;*</span>
                        </div>
                    </div>


                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-phone fa-lg"></i>
                                <input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="checked" runat="server" id="chkSMS" />
                            <span class="lbl"></span></span>
                                <asp:TextBox ID="txtCustomerMobile" ClientIDMode="Static" TabIndex="3"  CssClass="form-control" placeholder="Mobile no" Height="36px"
                                    runat="server" MaxLength="20"></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="txtCustomerMobile_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerMobile">
                                                </cc1:FilteredTextBoxExtender>
                              
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            &nbsp;<i id="SmsInfo"  data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                        </div>
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-envelope-o fa-lg"></i><input type="checkbox" class="ace ace-switch ace-switch-5" clientidmode="Static"
                                checked="checked" runat="server" id="ChkEmail" />
                            <span class="lbl"></span></span>
                                <asp:TextBox ID="txtCustomerEmailId" TabIndex="4"  ClientIDMode="Static" Height="36px" runat="server" placeholder="Email ID" MaxLength="50"
                                    CssClass="form-control"></asp:TextBox>
                             
                            </div>
                        </div>
                       
                        <div class="col-sm-1 Textpadding">
                            &nbsp;<i id="EmailInfo"  data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-users fa-lg"></i></span>
                                <asp:TextBox ID="txtMemberShip" CssClass="form-control" placeholder="Membership ID" TabIndex="5" 
                                    runat="server" MaxLength="50" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtMemberShip_FilteredTextBoxExtender" InvalidChars="`~:;,-"
                                    runat="server" Enabled="True" FilterMode="InvalidChars" TargetControlID="txtMemberShip">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="textRed">&nbsp;*</span></div>
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor"><i class="fa fa-barcode fa-lg"></i></span>
                                <asp:TextBox ID="txtBarCode" runat="server" placeholder="Barcode" MaxLength="50" TabIndex="6" 
                                    CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" InvalidChars="`~:;,-"
                                    runat="server" Enabled="True" FilterMode="InvalidChars" TargetControlID="txtBarCode">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                       
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">Start</span>
                                <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" CssClass="form-control" TabIndex="7" 
                                    placeholder="Start Date" onpaste="return false;" ClientIDMode="Static"></asp:TextBox>
                                    <span class="input-group-addon IconBkColor textBold"><i class="fa fa-calendar fa-lg">
                                </i></span>
                                <%--<cc1:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Format="dd MMM yyyy"
                        TargetControlID="txtStartDate">
                    </cc1:CalendarExtender>--%>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group labelBorder divheight backcolor2" id="divExpDate">
                                <span class="input-group-addon IconBkColor">End</span>
                                <p class="textmargin5">
                                    &nbsp;
                                    <asp:Label ID="txtEndDate" CssClass="fontsize" runat="server" MaxLength="11" onkeypress="return false;"
                                        onpaste="return false;" ClientIDMode="Static"></asp:Label>
                                </p>
                                <span class="input-group-addon IconBkColor textBold"><i class="fa fa-calendar fa-lg">
                                </i></span>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group input-sm Textpadding">
                                <span class="input-group-addon IconBkColor">Payment Type</span>
                                <asp:DropDownList ID="drpPaymentType" runat="server" ClientIDMode="Static" TabIndex="8" 
                                    CssClass="form-control" onchange="someFunction()">
                                    <asp:ListItem Text="None" />
                                    <asp:ListItem Text="Cash" />
                                    <asp:ListItem Text="Credit Card/Debit Card" />
                                    <asp:ListItem Text="Cheque/Bank" />
                                </asp:DropDownList>
                            </div>
                        </div>

                          <div class="col-sm-1">                          
                          </div>

                            <div class="col-sm-5 Textpadding" id="divPaymentDetails" style="display:none" runat="server" clientidmode="Static">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor">  <asp:Label ID="lblPackageDetails" runat="server" ClientIDMode="Static" Text="Details"
        Style="display: none;" ></asp:Label></span>
                               <asp:TextBox ID="txtPaymentDetails" TabIndex="9"  runat="server" MaxLength="250" ClientIDMode="Static" CssClass="form-control"
                               Style="display: none;"></asp:TextBox>
                            </div>
                          </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <a class="btn btn-info" id="btnSaveOnly" runat="server"><i class="fa fa-floppy-o"></i>
                        &nbsp;&nbsp;Assign Package</a> <a class="btn btn-info" id="btnSave" runat="server"><i class="fa fa-print fa-lg">
                        </i>&nbsp;&nbsp;Assign Package and Print Receipt</a> <a class="btn btn-info" id="btnUpdate" runat="server"
                            visible="false"><i class="fa fa-check-square-o"></i>&nbsp;&nbsp;Update</a> <a class="btn btn-info"
                                id="btnDelete" runat="server" visible="false"><i class="fa fa-trash-o"></i>&nbsp;&nbsp;Delete</a>
                    <a class="btn btn-info" id="btnMarkComplete" runat="server" visible="false"><i class="fa fa-shield">
                    </i>&nbsp;&nbsp;Mark Complete</a> <a class="btn btn-info" id="btnPrintSlip" runat="server"
                        visible="false"><i class="fa fa-print"></i>&nbsp;&nbsp;Print Receipt</a>                  
                </div>
            </div>
        </div>
        <div class="col-sm-5">
            <div class="panel panel-primary well-sm-tiny1" id="pnlGrid" runat="server" clientidmode="Static">
                <div class="panel-heading">
                    <h6 class="panel-title">
                        <span>Selected Garments in Package</span>
                    </h6>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="grdQtyDetails" runat="server" CssClass="table table-striped table-bordered table-hover"
                        EnableTheming="false" AutoGenerateColumns="false" Visible="false">
                        <Columns>
                            <asp:BoundField DataField="S_No" HeaderText="S.No." SortExpression="S_No" HeaderStyle-BackColor="#72AAD8" HeaderStyle-Width="44px" />
                            <asp:BoundField DataField="ItemName" HeaderText="Garment" SortExpression="ItemName"
                                HeaderStyle-BackColor="#72AAD8" />
                            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" HeaderStyle-BackColor="#72AAD8" />
                        </Columns>
                        <HeaderStyle Font-Size="12px" ForeColor="white" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny">
        <cc1:TabContainer ID="TabContainer2" runat="server">
                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="In Use" >
                            <HeaderTemplate>
                              <span>Active</span>
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                   <asp:GridView ID="grdEntry" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                OnSelectedIndexChanged="grdEntry_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />              
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblAssignId" runat="server" Text='<%# Bind("AssignId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PackageName" HeaderText="Package" ReadOnly="True" />
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStartValue" runat="server" Text='<%# Bind("StartValue") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" />  
                    <asp:BoundField DataField="StartDate" HeaderText="Start" ReadOnly="True" />
                    <asp:BoundField DataField="EndDate" HeaderText="End" ReadOnly="True" />
                    <asp:BoundField DataField="PackageComplete" HeaderText="Status" ReadOnly="True" />
                </Columns>
                <HeaderStyle Font-Size="12px" />
            </asp:GridView> 
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>

                          <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Completed">
                            <HeaderTemplate>
                             Completed
                            </HeaderTemplate>
                            <ContentTemplate>
                                <fieldset class="Fieldset">
                                   <asp:GridView ID="grdPkgCompleted" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                OnSelectedIndexChanged="grdPkgCompleted_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />              
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblAssignId" runat="server" Text='<%# Bind("AssignId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PackageName" HeaderText="Package" ReadOnly="True" />
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStartValue" runat="server" Text='<%# Bind("StartValue") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" />  
                    <asp:BoundField DataField="StartDate" HeaderText="Start" ReadOnly="True" />
                    <asp:BoundField DataField="EndDate" HeaderText="End" ReadOnly="True" />
                    <asp:BoundField DataField="PackageComplete" HeaderText="Status" ReadOnly="True" />
                </Columns>
                <HeaderStyle Font-Size="12px" />
            </asp:GridView> 
                                </fieldset>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
        </div>
    </div>
    <div style="display:none">
    <asp:Label ID="lblStartValue" runat="server" CssClass="TDCaption" Text="Start Value :"></asp:Label>
    <asp:TextBox ID="txtStartValue" runat="server" MaxLength="7" Enabled="false"></asp:TextBox><span
        class="span"></span>        
    <cc1:FilteredTextBoxExtender ID="txtStartValue_FilteredTextBoxExtender" runat="server"
        Enabled="True" TargetControlID="txtStartValue" ValidChars="1234567890.">
    </cc1:FilteredTextBoxExtender>
    </div>

     <asp:DropDownList ID="drpsmstemplate" runat="server" EnableTheming="false" ClientIDMode="Static"
        Style="display: none">
    </asp:DropDownList>
    <asp:DropDownList ID="drpsmstemplateMarkcomplete" runat="server" EnableTheming="false" ClientIDMode="Static"
        Style="display: none">
    </asp:DropDownList>
    <asp:Label ID="lblCustomerCode" runat="server" Style="display: none"></asp:Label>
    <asp:Label ID="lblDuplicateCustomer" runat="server" Style="display: none"></asp:Label>
    <asp:HiddenField ID="hdnDateDifference" Value="0" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEndDate" Value="0" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdntempNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdntempEmailid" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdntmp" runat="server" ClientIDMode="Static" />
      <asp:DropDownList ID="ddlRateList1" runat="server" ClientIDMode="Static" Style="visibility: hidden">
        </asp:DropDownList>
    <script type="text/javascript">
        document.getElementById('drpPaymentType').onchange = function (e) {
            if (this.value === 'None' || this.value === 'Cash') {
                document.getElementById('txtPaymentDetails').style.display = 'none';
                document.getElementById('lblPackageDetails').style.display = 'none';
                document.getElementById('divPaymentDetails').style.display = 'none';
                document.getElementById('txtPaymentDetails').focus();
            }
            else {
                
                document.getElementById('txtPaymentDetails').style.display = 'block';
                document.getElementById('lblPackageDetails').style.display = 'block';
                document.getElementById('divPaymentDetails').style.display = 'block';
                $('#txtPaymentDetails').focus();
            }
        }
    </script>
</asp:Content>
