<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmPackageMaster.aspx.cs" Inherits="QuickWeb.Masters.frmPackageMaster" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.9.2.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>   
    <style type="text/css">       
        .tooltip-inner {
            max-width: 410px;   
            width: 410px; 
            background-color:#F1F1F1;
            text-align:left;
            font-size:14px;
            color:Black;
            border: 1px solid #C0C0A0;
            }
    </style>
    <script type="text/javascript" language="javascript">

        function checkName() {
            var strname = document.getElementById("<%=txtTitle.ClientID %>").value.trim().length;
            var stradd = document.getElementById("<%=txtCost.ClientID %>").value.trim();
            var amnt1 = document.getElementById("<%=txtAmount.ClientID %>").value.trim();
            var amnt2 = document.getElementById("<%=txtDiscount.ClientID %>").value;
            var PackageType = document.getElementById("<%=drpPackageType.ClientID %>").value;
            var input = parseInt(amnt2);
            var InputAmount = parseInt(amnt1);
            var InputCost = parseInt(stradd);

            if (PackageType == '1') {
                if ($('#txtStartDate').val() === '') {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter start date.');                     
                    $('#txtStartDate').focus();
                    return false;
                }
                if (PackageType == '1') {
                    if (input < 0 || input > 100) {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text("Discount should be between 0 - 100");
                        document.getElementById("txtDiscount.ClientID ").focus();
                        return false;
                    }
                }
                else {
                    if (input < 0 || input > 100) {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text("Discount should be between 0 - 100");
                        document.getElementById("<%=txtDiscount.ClientID %>").focus();
                        return false;
                    }
                }
                if ($('#txtEndDate').val() === '') {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter end date.');
                    document.getElementById("<%=txtEndDate.ClientID %>").focus();                  
                    return false;
                }
                if (IsFirstDateBigger($('#txtStartDate').val(), $('#txtEndDate').val())) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Start date can't be after end date.");                  
                    return false;
                }

            }
            if (PackageType != '3' && PackageType != '4') {
                if (strname == "" || strname.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Please enter the package name.');
                    document.getElementById("<%=txtTitle.ClientID %>").focus();
                    return false;
                }
                if (stradd == "" || stradd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Package cost is required.');
                    document.getElementById("<%=txtCost.ClientID %>").focus();
                    return false;
                }

                if (PackageType == '2') {
                    if (amnt1 == "" || amnt1.length == 0) {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text('Please enter the benefit value.');
                        document.getElementById("<%=txtAmount.ClientID %>").focus();
                        return false;
                    }
                    var TextType = document.getElementById("<%=drpServiceTaxType.ClientID %>").value;
                    if (TextType == 'Yes') {
                         CheckAmountWithTax();
                        var strAmtWithText = document.getElementById("<%=hdnComputedCost.ClientID %>").value;                       
                        var AmtwithTax = parseFloat(strAmtWithText);
                        InputCost = AmtwithTax;
                    }
                    var InputAmount1 = parseFloat(InputAmount);
                    var InputCost1 = parseFloat(InputCost);
                    if (InputAmount1 < InputCost1) {
                        setDivMouseOver('#FA8602', '#999999');
                        if (TextType == 'Yes') {
                            $('#lblMsg').text("Value must be more than Cost + Tax.");
                        } else {
                            $('#lblMsg').text("Value must be more than cost.");
                        }
                        document.getElementById("<%=txtAmount.ClientID %>").focus();
                        return false;
                    }

                } else if (PackageType == '1') {
                    if (amnt2 == "" || amnt2.length == 0) {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text('Please enter the discount percentage.');
                        document.getElementById("<%=txtDiscount.ClientID %>").focus();
                        return false;
                    }
                }
              
                //                }
            }
            else {
                var strname = document.getElementById("<%=txtTitle.ClientID %>").value.trim().length;
                var stradd = document.getElementById("<%=txtCost.ClientID %>").value.trim().length;
                var strTotalQty = document.getElementById("<%=txtFlatQty.ClientID %>").value.trim().length;
                var GridId = "<%=grdBind.ClientID %>";

              //var GridId = $('#ctl00_ContentPlaceHolder1_grdBind');

                
                var grid = document.getElementById(GridId);
                rowscount = grid.rows.length;
                if (strname == "" || strname.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter package name.'); 
                    document.getElementById("<%=txtTitle.ClientID %>").focus();
                    return false;
                }
                if (stradd == "" || stradd.length == 0) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Kindly enter package cost.');                    
                    document.getElementById("<%=txtCost.ClientID %>").focus();
                    return false;
                }
                if (!(parseInt($('#txtRecurrence').val()) >= 1)) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Package cycle must be defined as greater than 0.');                   
                    $('#txtRecurrence').focus();
                    return false;
                }

                if (!(parseInt($('#ctl00_ContentPlaceHolder1_txtFlatQty').val()) >= 1)) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('Package quantity must be defined.');
                    $('#ctl00_ContentPlaceHolder1_txtFlatQty').focus();
                    return false;
                }

                if ($('#txtStartDate').val() === '') {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('kindly enter start date.');                
                    $('#txtStartDate').focus();
                    return false;
                }
                if ($('#txtEndDate').val() === '') {

                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text('kindly enter end date.');
                    document.getElementById("<%=txtEndDate.ClientID %>").focus();               
                  
                    return false;
                }
                if (IsFirstDateBigger($('#txtStartDate').val(), $('#txtEndDate').val())) {
                    setDivMouseOver('#FA8602', '#999999');
                    $('#lblMsg').text("Start date can't be after end date.");                 
                    return false;
                }
                if (PackageType == '4') {
                    if (strTotalQty == "" || strTotalQty.length == 0) {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text('kindly enter total qty.'); 
                        document.getElementById("<%=txtFlatQty.ClientID %>").focus();
                        return false;
                    }
                }
                if (PackageType == '3') {
                    if (rowscount == "2") {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text('kindly enter garment detail.');                       
                        return false;
                    }
                }                
            }
        }
        function CheckAmountWithTax() {
            $.ajax({
                url: '../AutoComplete.asmx/GetAmoutWithTax',
                type: 'GET',
                data: "Cost='" + $('#ctl00_ContentPlaceHolder1_txtCost').val() + "'",
                timeout: 20000,
                contentType: 'application/json; charset=UTF-8',
                datatype: 'JSON',
                cache: true,
                async: false,
                success: function (response) {
                    var _val = response.d;
                    document.getElementById("<%=hdnComputedCost.ClientID %>").value = '';
                    document.getElementById("<%=hdnComputedCost.ClientID %>").value = _val;
                },
                error: function (response) {
                    alert(response.toString())
                }
            }); 
        }

        function checkRec() {
            var rtr = false;
            $.ajax({
                url: '../AutoComplete.asmx/ValidateRecurrenceCount',
                type: 'POST',
                async: false,
                data: "{ startDate :'" + $('#txtStartDate').val() + "', endDate: '" + $('#txtEndDate').val() + "', recurrenceCount: '" + $('#txtRecurrence').val() + "' }",
                datatype: 'JSON',
                contentType: 'application/json; charset=utf8',
                succcess: function (data) {
                    if (data.d !== 'true') {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text("No of cycle can't be more then no of days b/w start date and end date, Change b/w to Between.");                      
                        rtr = false;
                    }
                    else {
                        rtr = true;
                    }
                },
                error: function () {
                    rtr = false;
                }
            });
            return rtr;
        }

        function ConfirmCancel() {
            return confirm("Are you sure you want to delete this package?");
        }

        

        $(document).ready(function (event) {

            $(document).keypress(function (event) {
                var textval = $('#ctl00_ContentPlaceHolder1_txtTitle').val();
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    if (textval == "") {
                        return false;
                    }
                    else {
                    }
                }
            });

//            $('#ctl00_ContentPlaceHolder1_rdbAmount').click(function (e) {
//                if ($(this).is(':checked')) {
//                    $('#ctl00_ContentPlaceHolder1_txtDiscount').attr('disabled', true);
//                    $('#ctl00_ContentPlaceHolder1_txtAmount').attr('disabled', false);
//                    $('#ctl00_ContentPlaceHolder1_txtAmount').focus();
//                }
//            });

            //            $('#ctl00_ContentPlaceHolder1_rdbDiscount').click(function (e) {
            //                if ($(this).is(':checked')) {
            //                    $('#ctl00_ContentPlaceHolder1_txtAmount').attr('disabled', true);
            //                    $('#ctl00_ContentPlaceHolder1_txtDiscount').attr('disabled', false);
            //                    $('#ctl00_ContentPlaceHolder1_txtDiscount').focus();
            //                }
            //            });

        });
    </script>
    <script type="text/javascript" language="javascript">
        function testCustom(selObj) {


            try {
                // alert(selObj.options[selObj.selectedIndex].value);
                if (selObj.options[selObj.selectedIndex].value == '1') {
                    //Show the textbox                   
                    $("#PkgTypeInfo").tooltip('destroy');
                    $("#PkgTypeInfo").tooltip({
                        title: 'Creates a membership plan by which any customer can pay a fixed amount and enjoy certain discount for a selected duration of time. This amount is non refundable, not adjustable and not transfearable. </br>  For Example: </br><b>  Membership Cost: </b> 2000 </br><b> Benefit:</b> 20% discount on every order till the package expires</br><b>Validity:</b> One Year',
                        html:true
                    });

                    //$('#txtRecurrence').closest('tr').hide();                   
                    $('#divRecu').hide();
                    //                    $('#txtStartDate').closest('tr').hide();
                    document.getElementById("<%=trTaxtype.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trTaxtype.ClientID %>").style.display = 'none';
                    document.getElementById("<%=trFlatQty.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trFlatQty.ClientID %>").style.display = 'none';

                    document.getElementById("<%=tblQty.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=tblQty.ClientID %>").style.display = 'none';
                    document.getElementById("<%=trDiscount.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=trDiscount.ClientID %>").style.display = '';
                    document.getElementById("<%=trAmount.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trAmount.ClientID %>").style.display = 'none';

                    document.getElementById("<%=txtDiscount.ClientID %>").attributes('disabled', true);


                }
                else if (selObj.options[selObj.selectedIndex].value == '2') {
                    //Hide the textbox                  
                    //   $('#txtRecurrence').closest('tr').hide();
                    $('#divRecu').hide();
                  //  $('#txtStartDate').closest('tr').hide();
                    document.getElementById("<%=trFlatQty.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trFlatQty.ClientID %>").style.display = 'none';
                    document.getElementById("<%=tblQty.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=tblQty.ClientID %>").style.display = 'none';

                    document.getElementById("<%=trDiscount.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trDiscount.ClientID %>").style.display = 'none';
                    document.getElementById("<%=trTaxtype.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=trTaxtype.ClientID %>").style.display = '';
                    // document.getElementById("<%=txtAmount.ClientID %>").attributes('disabled', true);
                    document.getElementById("<%=trAmount.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=trAmount.ClientID %>").style.display = '';
                    $("#PkgTypeInfo").tooltip('destroy');                  
                    $("#PkgTypeInfo").tooltip({
                        title: 'Creats a membership plan by which any customer can pay a fixed amount as advance and can get orders done for more than what he paid. This amount is non refundable, not adjustable and not transferable. </br>  For Example: </br><b>  Membership Cost: </b> 10000 </br><b> Order Amount that can be booked:</b> 12000</br><b>Validity:</b> One Year',
                        html: true
                    });

                }
                else if (selObj.options[selObj.selectedIndex].value == '3') {
                    //Hide the textbox                   
                    //   $('#txtRecurrence').closest('tr').show();
                    $('#divRecu').show();
                    $('#txtStartDate').closest('tr').show();
                    document.getElementById("<%=trFlatQty.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trFlatQty.ClientID %>").style.display = 'none';

                    document.getElementById("<%=trDiscount.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trDiscount.ClientID %>").style.display = 'none';
                    document.getElementById("<%=tblQty.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=tblQty.ClientID %>").style.display = '';
                    document.getElementById("<%=trTaxtype.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=trTaxtype.ClientID %>").style.display = '';
                    document.getElementById("<%=trAmount.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trAmount.ClientID %>").style.display = 'none';

                }
                else {
                    //Hide the textbox
                    //  $('#txtRecurrence').closest('tr').show();
                    $('#divRecu').show();
                    $('#txtStartDate').closest('tr').show();
                   
                    document.getElementById("<%=trFlatQty.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=trFlatQty.ClientID %>").style.display = '';

                    document.getElementById("<%=trDiscount.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trDiscount.ClientID %>").style.display = 'none';
                    document.getElementById("<%=trAmount.ClientID %>").style.visibility = 'hidden';
                    document.getElementById("<%=trAmount.ClientID %>").style.display = 'none';
                    document.getElementById("<%=tblQty.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=tblQty.ClientID %>").style.display = '';

                    document.getElementById("<%=grdBind.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=grdBind.ClientID %>").style.display = '';

                    document.getElementById("<%=trTaxtype.ClientID %>").style.visibility = 'visible';
                    document.getElementById("<%=trTaxtype.ClientID %>").style.display = '';                   
                    $("#PkgTypeInfo").tooltip('destroy');
                    $("#PkgTypeInfo").tooltip({
                        title: 'Creates a membership plan by which a customer can pre pay for a certain number of garments that he/she may get processes for a selected duration of time. This amount is non refundable, not adjustable and not transfearable.</br>For example:</br><b>Membership Cost:</b> 10,000 </br><b> No of cloths that can be processes:</b> 1,000</br><b> Validity:</b> 3 months ',
                        html:true
                    }); 

                }                             

                if (selObj.options[selObj.selectedIndex].value == '4') {
                    $('th:nth-child(3), td:nth-child(3)', '#ctl00_ContentPlaceHolder1_grdBind').hide();
                }
                else {
                    $('th:nth-child(3), td:nth-child(3)', '#ctl00_ContentPlaceHolder1_grdBind').show();
                }
            }
            catch (ex) {

            }
            finally {                
              
            }
        }
    </script>
    <script type="text/javascript">
        $(function (e) {

            $('#hdnFireRowCommand').val('');
            testCustom(ctl00_ContentPlaceHolder1_drpPackageType);

            hideExtender = function () {
                if ($('#ctl00_ContentPlaceHolder1_grdBind_ctl03_autoComplete1_completionListElem li').length == 0) {
                    $('#ctl00_ContentPlaceHolder1_grdBind_ctl03_autoComplete1_completionListElem').css('display', 'none');
                }
            };

            $('#ctl00_ContentPlaceHolder1_grdBind').on('keydown', function (e) {
                if (e.which !== 13) return;
                if (e.target.type !== 'text') return;
                if ((e.target.id.indexOf('Qty') === -1) && ($('#ctl00_ContentPlaceHolder1_grdBind td:nth-child(3)').is(':visible'))) return;
                if ($(e.target).closest('tr').find('input').eq(0).val() === '') return;
                if ($('#ctl00_ContentPlaceHolder1_grdBind td:nth-child(3)').is(':hidden')) {
                    $(e.target).closest('tr').closest('tr').find('input').eq(1).val(0);
                    $('#hdnFireRowCommand').val('FireAdd');
                }
                else {
                    $(e.target).closest('tr').find('input').eq(2).trigger('click');
                }
            });

            $('#ctl00_ContentPlaceHolder1_drpPackageType').on('change', hideExtender);
        });
    </script>
    <script type="text/javascript" language="javascript">

        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 5000);
        }
        function clearMsg() {

            $('#lblErr').text('');
            $('#lblMsg').text('');
        }

        $(document).ready(function () {
            $("#CycleInfo").tooltip({
                title: 'Number of times a same package is repeated. For ex, a Flat Qty package is created which allows processing of 30 cloths for a pre paid amount of 1,000 valid for 1 month. This package can be extended to 3 months just by mentioning Cycle as "3". This implies that the customer has paid for 3 such monthly packages with one packages being active at a time and after the expiry of first package second package will be become active automatically. and so on.'
            });
            $("#QtyInfo").tooltip({
                title: 'This number defines the maximum number of cloths allowed in each cycle. For example 30 cloths for a pre-paid amount of 1,000 valid for 1 month. Here Quantity/Cycle is 30.'
            });
            $("#TaxInfo").tooltip({
                title: 'Choose Yes if package cost needs to be topped up with state/government Taxes.'
            });

            var Date = $('#txtStartDate,#txtEndDate');
            Date.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd M yyyy",
                language: "tr"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });

            $("#<%=grdEntry.ClientID%> tr:nth-child(odd)").css("background-color", "white")
            $("#<%=grdEntry.ClientID%> tr:nth-child(even)").css("background-color", "#F0F4FB");
            $("#txtRecurrence,#ctl00_ContentPlaceHolder1_txtFlatQty").spinner(
               {
                   min: 1
               }
               );

            $('#ctl00_ContentPlaceHolder1_btnSave,#ctl00_ContentPlaceHolder1_btnUpdate,#ctl00_ContentPlaceHolder1_btnDelete').click(function (e) {
                clearMsg();
                var clickedId = $(this).attr("id");
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                if (clickedId == 'ctl00_ContentPlaceHolder1_btnSave') {
                    var status = checkName();
                    if (status == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnSave', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnUpdate') {
                    var status1 = checkName();
                    if (status1 == false) {
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$btnUpdate', null);
                }
                else if (clickedId == 'ctl00_ContentPlaceHolder1_btnDelete') {                   
                    __doPostBack('ctl00$ContentPlaceHolder1$btnDelete', null);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="DivContainerStatus">
                <div id="DivContainerInnerStatus" class="span label label-default">
                    <h4 class="text-center">
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" 
                            ForeColor="White" ClientIDMode="Static" />
                               <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                            ForeColor="White" />                            
                    </h4>
                </div>
            </div>
        </div>  
    <div class="row-fluid div-margin">
        <div class="col-sm-8">
            <div class="panel panel-primary well-sm-tiny1">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Create Your Package</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">Package Type&nbsp;&nbsp;</span>
                                <asp:DropDownList ID="drpPackageType" BackColor="White" runat="server" CssClass="form-control" TabIndex="1"
                                    onChange="javascript:testCustom(this);">
                                    <asp:ListItem Text="Discount" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Value / Benefit" Value="2"></asp:ListItem>
                                    <%--<asp:ListItem Text="Qty / Item" Value="3"></asp:ListItem>--%>
                                    <asp:ListItem Text="Flat Qty" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            &nbsp;<i id="PkgTypeInfo"  data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-11 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">Package Name</span>
                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="250" CssClass="form-control" TabIndex="2"
                                    placeholder="Package Name"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtTitle_FilteredTextBoxExtender" FilterMode="ValidChars"
                                    ValidChars=" 0123456789abcdedfghijklmnopqurstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                    runat="server" Enabled="True" TargetControlID="txtTitle">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor textBold">&nbsp;<i class="fa fa-usd fa-lg"></i>&nbsp;</span>
                                <asp:TextBox ID="txtCost" runat="server" MaxLength="8" CssClass="form-control" placeholder="Cost" TabIndex="3"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtCost_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtCost" ValidChars="1234567890">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                        <div class="col-sm-6 Textpadding" id="trDiscount" runat="server">
                            <div class="col-sm-10 Textpadding">
                                <div class="input-group">
                                    <span class="input-group-addon IconBkColor">Discount&nbsp;(%)&nbsp;</span>
                                    <asp:TextBox ID="txtDiscount" CssClass="form-control" runat="server" MaxLength="3" TabIndex="4"
                                        placeholder="Discount"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        TargetControlID="txtDiscount" ValidChars="1234567890">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-sm-1 Textpadding">
                                <span class="span textBold">&nbsp;*</span>
                            </div>
                        </div>

                        <div class="col-sm-6 Textpadding" id="trAmount" runat="server" style="display: none">
                        <div class="col-sm-10 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">Value</span>                              
                                        <%--<asp:RadioButton ID="rdbAmount" Text="&nbsp;Amount" GroupName="a" runat="server" />--%>                                    
                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="8" CssClass="form-control" TabIndex="4" placeholder="Value"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                            TargetControlID="txtAmount" ValidChars="1234567890.">
                                        </cc1:FilteredTextBoxExtender>                                  
                                </div>
                            </div>                       
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>


                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor">Start</span>
                                <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" CssClass="form-control" TabIndex="5"
                                    placeholder="Start Date" ClientIDMode="Static" onpaste="return false;"></asp:TextBox>
                                    <span class="input-group-addon IconBkColor textBold"><i class="fa fa-calendar fa-lg">
                                </i></span>
                                <%--   <cc1:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Format="dd MMM yyyy"
                                    TargetControlID="txtStartDate">
                                </cc1:CalendarExtender>--%>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                        <div class="col-sm-5 Textpadding">
                            <div class="input-group">
                                <span class="input-group-addon IconBkColor ">End</span>
                                <asp:TextBox ID="txtEndDate" runat="server" MaxLength="11" CssClass="form-control" TabIndex="6"
                                    placeholder="Expiry Date" ClientIDMode="Static" ></asp:TextBox>
                                     <span class="input-group-addon IconBkColor textBold"><i class="fa fa-calendar fa-lg">
                                </i></span>
                                <%--<cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd MMM yyyy"
                                    TargetControlID="txtEndDate">
                                </cc1:CalendarExtender>--%>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>                    
                    <div class="row-fluid div-margin">
                        <div class="col-sm-4 Textpadding" id="divRecu">
                            <div class="col-sm-8 Textpadding">
                                <div class="input-group" style="height: 30px">
                                    <span class="input-group-addon IconBkColor">Cycle</span>
                                    <asp:TextBox ID="txtRecurrence" runat="server" MaxLength="3" value="1" ClientIDMode="Static" TabIndex="7"
                                        CssClass="form-control "></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtRecurrence_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="txtRecurrence">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-sm-2 Textpadding form-inline">
                                <div class="form-group">
                                    <span class="span textBold">&nbsp;*</span>
                                </div>
                                <div class="form-group" style="margin-top:-9px">
                                    &nbsp;<i id="CycleInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4  Textpadding" id="trFlatQty" runat="server" style="display: none">
                            <div class="col-sm-9 Textpadding">
                                <div class="input-group">
                                    <span class="input-group-addon IconBkColor">Quantity / Cycle</span>
                                    <asp:TextBox ID="txtFlatQty" runat="server" value="1" CssClass="form-control" MaxLength="4" TabIndex="8"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtFlatQty_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="txtFlatQty">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-sm-2 Textpadding form-inline">
                                <div class="form-group">
                                    <span class="span textBold">&nbsp;*</span>
                                </div>
                                <div class="form-group" style="margin-top:-9px">
                                    &nbsp;<i id="QtyInfo"  data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4  Textpadding" id="trTaxtype" runat="server" style="display: none">
                            <div class="col-sm-9 Textpadding">
                                <div class="input-group">
                                    <span class="input-group-addon IconBkColor">Taxes Extra</span>
                                    <asp:DropDownList runat="server" AppendDataBoundItems="True" ID="drpServiceTaxType" TabIndex="9"
                                        CssClass="form-control" BackColor="White" ClientIDMode="Static">
                                        <asp:ListItem Text="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            &nbsp;<i id="TaxInfo"  data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                        </div>
                    </div>
            </div>
            <div class="panel-footer">
                <a class="btn btn-info" id="btnSave" runat="server"><i class="fa fa-floppy-o"></i>&nbsp;&nbsp;Save</a>
                <a class="btn btn-info" id="btnUpdate" runat="server"><i class="fa fa-check-square-o">
                </i>&nbsp;&nbsp;Update</a> <a class="btn btn-info" id="btnDelete" visible="False"
                    runat="server"><i class="fa fa-trash-o"></i>&nbsp;&nbsp;Delete</a>              
            </div>
        </div>
    </div>
    <div class="col-sm-4 ">
        <div class="row-fluid" id="tblQty" runat="server">
            <div class="panel panel-primary well-sm-tiny1">
                <div class="panel-heading">
                    <h6 class="panel-title">
                      <span style="font-size:15px">  Please select Garment to be included in package</span></h6>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="grdBind" runat="server" CssClass="table table-striped table-bordered table-hover"
                        EnableTheming="false" DataKeyNames="S_No" AutoGenerateColumns="False" ShowFooter="True"
                        OnRowCommand="grdBind_RowCommand" OnRowCancelingEdit="grdBind_RowCancelingEdit"
                        Font-Size="15px" OnRowEditing="grdBind_RowEditing" OnRowUpdating="grdBind_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="S. No" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#72AAD8  ">
                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%# Bind("S_No") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblNewSno" runat="server" />
                                   <%-- <br />
                                    <span style="color: White">*</span>--%>
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblESrNo" runat="server" Text='<%# Bind("S_No") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Garment" SortExpression="DescriptionOfMaterial" HeaderStyle-BackColor="#72AAD8">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFDescription" runat="server" CssClass="form-control" onblur="this.className='form-control'" Height="30px"
                                        onfocus="this.className='form-control'" AutoPostBack="True" OnTextChanged="txtFDescription_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtFDescription"
                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                                    </cc1:AutoCompleteExtender>
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEDescription" runat="server" Text='<%# Bind("ItemName") %>' CssClass="form-control" Height="30px"
                                        AutoPostBack="True" OnTextChanged="txtEDescription_TextChanged" onblur="this.className='form-control'"
                                        onfocus="this.className='form-control'"></asp:TextBox>
                                    <cc1:AutoCompleteExtender runat="server" ID="autoComplete2" TargetControlID="txtEDescription"
                                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetItemNameList" MinimumPrefixLength="1"
                                        CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender_new"
                                        CompletionListItemCssClass="AutoExtenderList_new" CompletionListHighlightedItemCssClass="AutoExtenderHighlight_new">
                                    </cc1:AutoCompleteExtender>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty" HeaderStyle-BackColor="#72AAD8">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFQty" MaxLength="10" onkeyPress="return checkKey(event);" runat="server"
                                        CssClass="textbox" Width="50px" Text="1" onfocus="javascript:select();"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtFQty_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="txtFQty">
                                    </cc1:FilteredTextBoxExtender>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEQty" MaxLength="10" onkeyPress="return checkKey(event);" runat="server"
                                        CssClass="textbox" Width="50px" Text='<%# Bind("Qty") %>' onfocus="javascript:select();"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtEQty_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="txtEQty">
                                    </cc1:FilteredTextBoxExtender>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" HeaderStyle-BackColor="#72AAD8">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/img/Edit.png" Width="20px"
                                        Height="20px" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit garment" />
                                    <asp:ImageButton ID="btnDeleteRecord" runat="server" ImageUrl="~/img/delete.png"
                                        Width="20px" Height="20px" ToolTip="Remove garment from the list" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                        CommandName="DeleteRecord" Text="Delete" ValidationGroup="Save" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="BtnUpdate" runat="server" CausesValidation="True" ImageUrl="~/img/Update.png"
                                        Height="20px" Width="20px" ToolTip="Update garment" CommandName="Update" Text="Update" />
                                    <asp:ImageButton ID="BtnCancel" runat="server" CausesValidation="False" ImageUrl="~/img/Undo.png"
                                        Height="20px" ToolTip="Reset garment" Width="20px" Text="Cancel" OnClick="BtnCancel_Click" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="btnAddNewRecord" runat="server" ToolTip="Add new garment" CommandName="AddNewRecord"
                                        ImageUrl="~/img/Add.png" Text="Add" Width="20px" Height="20px" />
                                    <span style="color: White">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Size="12px" ForeColor="white" />
                    </asp:GridView>
                    <asp:HiddenField ID="hdnRowInsex" Value="0" runat="server" />
                </div>
            </div>
        </div>
    </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny">
            <asp:GridView ID="grdEntry" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                CssClass="table table-striped table-bordered table-hover" EnableTheming="false"
                OnSelectedIndexChanged="grdEntry_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" HeaderStyle-BackColor="#808080  " />
                    <%--<asp:BoundField DataField="PackageId" HeaderText="Package Id" ReadOnly="True" />--%>
                    <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblPackageId" runat="server" Text='<%# Bind("PackageId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="PackageType" HeaderText="Type" ReadOnly="True"
                        HeaderStyle-BackColor="#808080  " />
                    <asp:BoundField DataField="PackageName" HeaderText="Name" ReadOnly="True"
                        HeaderStyle-BackColor="#808080  " />                   
                    <asp:BoundField DataField="PackageCost" HeaderText="Cost" ReadOnly="True"
                        HeaderStyle-BackColor="#808080  " />
                    <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBenefitType" runat="server" Text='<%# Bind("BenefitType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBenefitValue" runat="server" Text='<%# Bind("BenefitValue") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Active" HeaderText="Active" ReadOnly="True" HeaderStyle-BackColor="#808080  " Visible="false" />
                    <asp:BoundField DataField="CreateDate" HeaderText="Creation Date" ReadOnly="True"
                        HeaderStyle-BackColor="#808080  " />
                    <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblTaxType" runat="server" Text='<%# Bind("TaxType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delivered On" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalQty" runat="server" Text='<%# Bind("TotalQty") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recurrence" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblRecurrence" runat="server" Text='<%# Bind("Recurrence") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Start Date" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End Date" ItemStyle-ForeColor="Green" ItemStyle-Width="5px"
                        HeaderStyle-BackColor="#808080  " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Font-Size="12px" ForeColor="White" />
            </asp:GridView>
            <asp:HiddenField ID="hdnId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnFireRowCommand" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnIsRecurrenceValid" runat="server" ClientIDMode="Static" />
            <asp:Label ID="lblId" runat="server" Style="display: none;"></asp:Label>
            <asp:Label ID="lblDuplicateItem" runat="server" Style="display: none;"></asp:Label>
             <asp:HiddenField ID="hdnComputedCost" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <script type="text/javascript">
        window.onload = BindHandlers();

        function BindHandlers() {

            // on change of start
            document.getElementById('txtStartDate').onchange = function (e) {
                var dd = new Date('' + this.value + '');
                var dt = '';
                dd.setFullYear(dd.getFullYear() + 1);
                dt = dd.getDate().toString().length == 2 ? dd.getDate().toString() : '0' + dd.getDate().toString();
                dt += ' ' + Array.prototype.map.call([dd.getMonth()], function (val) {
                    switch (val) {
                        case 0: return 'Jan'; case 1: return 'Feb'; case 2: return 'Mar'; case 3: return 'Apr'; case 4: return 'May'; case 5: return 'Jun'; case 6: return 'Jul'; case 7: return 'Aug'; case 8: return 'Sep'; case 9: return 'Oct'; case 10: return 'Nov'; case 11: return 'Dec';
                    }
                })[0];
                dt += ' ' + dd.getFullYear();
                document.getElementById('txtEndDate').value = dt;
            }

            // on save
            /*
            $('#ctl00_ContentPlaceHolder1_btnSave, #ctl00_ContentPlaceHolder1_btnUpdate').on('click', function (e) {
            if (!$('#txtRecurrence').is(':visible')) {
            return; // could have checked for anyone, the start date or even the end date
            }

            checkRec();

            });
            */

            $('#txtEndDate, #txtRecurrence').on('change.AttachedEvent', function (e) {
                if ($('#ctl00_ContentPlaceHolder1_drpPackageType').val() !== '4') return;
                if ($('#txtStartDate').val() === '' || $('#txtEndDate').val() === '' || $('#txtRecurrence').val() === '') return;

                checkRec();
            });

            function checkRec() {
                var rtr = false;
                $.ajax({
                    url: '../AutoComplete.asmx/ValidateRecurrenceCount',
                    type: 'POST',
                    async: false,
                    data: "{ startDate :'" + $('#txtStartDate').val() + "', endDate: '" + $('#txtEndDate').val() + "', recurrenceCount: '" + $('#txtRecurrence').val() + "' }",
                    datatype: 'JSON',
                    contentType: 'application/json; charset=utf8',
                    success: function (data) {
                        if (data.d !== true) {
                            setDivMouseOver('#FA8602', '#999999');
                            $('#lblMsg').text('No. of recurrence can\'t be more then no of days btw start date and end date');                           
                            $('#hdnIsRecurrenceValid').val('false');
                            $('#txtRecurrence').focus();
                            rtr = false;
                        }
                        else {
                            rtr = true;
                            $('#hdnIsRecurrenceValid').val('true');
                        }
                    },
                    error: function () {
                        rtr = false;
                    }
                });
                return rtr;
            }

        }

        function IsFirstDateBigger(date1, date2) {
            var diff = 0;
            var dFst = new Date(date1);
            var dScnd;
            if (!date2)
                dScnd = new Date();
            else
                dScnd = new Date(date2);

            diff = (dFst.getFullYear() - dScnd.getFullYear()) * 365 + (dFst.getMonth() - dScnd.getMonth())

            var dFst = new Date(date1);
            var dScnd;
            if (!date2)
                dScnd = new Date();
            else
                dScnd = new Date(date2);

            if (dScnd.getFullYear() < dFst.getFullYear())
                return true;
            else if (dScnd.getFullYear() === dFst.getFullYear()) {
                if (dScnd.getMonth() < dFst.getMonth())
                    return true;
                else if (dScnd.getMonth() === dFst.getMonth()) {
                    if (dScnd.getDate() < dFst.getDate())
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        function findDiff(dateFirst, dateSecond) {
            var diff = 0;
            var localDtStrt = new Date(dateFirst);
            var localDtEnd = new Date(dateSecond);

            // check if leap year
            // if year is different
            // then
            var isLeapYear = (localDtEnd.getFullYear() === localDtStrt.getFullYear() ? (new Date(localDtEnd.getFullYear(), 01, 29).getMonth() === 1) : findLeapForDiffYear());

            // the leap year
            var findLeapForDiffYear = function () {
                // we are here as the year is diff
                // find the year of start date and end date
                var startDtMnth = localDtStrt.getFullYear();
                var endDtMnth = localDtEnd.getFullYear();

                // if start dt month is greater then 2
                if (startDtMnth > 1) {
                    // if end date month is less then 2
                    if (endDtMnth < 1) {
                        // the month is before feb, and no leap
                        return false;
                    }
                    else {
                        // find if the year of end date is leap
                        return (new Date(localDtEnd.getFullYear(), 01, 29).getMonth() === 1);
                    }
                }
                // else check this year
                else {
                    return (new Date(localDtStrt.getFullYear(), 01, 29).getMonth() === 1);
                }
            }
        }
    </script>
</asp:Content>
