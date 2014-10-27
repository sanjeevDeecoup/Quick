<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmNewCustomer.aspx.cs" Inherits="QuickWeb.New_Admin.frmNewCustomer"
    EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link href="../css/introjs.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../JavaScript/code.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script src="../js/jquery-blink.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="../JavaScript/MaxLength.min.js" type="text/javascript"></script>
    <script src="../JavaScript/HomeSrc.js" type="text/javascript"></script>
    <script src="../js/typeahead-bs2.js" type="text/javascript"></script>
    
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
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('.blink').blink();

            var IsCustCode = $('#hdnIsCustCode').val();
            if (IsCustCode == 1) {
                var strCustCode = $('#hdncustcode').val();
                fillCustomerRecord(strCustCode);
                $('#hdnIsCustCode').val('0');
                $('#txtCustomerSearch').focus();
            }

            var IsNewCust1 = $('#hdnIsNew').val();
            if (IsNewCust1 == 1) {
                document.getElementById('btnSMS').style.display = "none";
                document.getElementById('btnReset').style.display = "none";
                $('#hdnIsNew').val('0');
            }


            var IsNewCust = $('#hdnIsNewCustomer').val();
            if (IsNewCust == 1) {
                setBlank();
                var strNewCustName = $('#hdnNewCustName').val();
                $('#txtCustName').val(strNewCustName);
                $('#hdnIsNewCustomer').val('0');
                $('#txtCustName').focus();
            }

            $('#chkWebSite').click(function () {
                if ($('#chkWebSite').is(':checked')) {
                    $('#btnReset').removeAttr('disabled');
                }
                else {
                    ('#btnReset').attr('disabled');
                }
            });
            $('#btnReset').click(function (eventParam) {
                clearMsg();
                $.ajax({
                    url: '../AutoComplete.asmx/ResetWebsitePassword',
                    type: 'GET',
                    data: "arg='" + $('#lblCustomerCode').html() + "'",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var _val = response.d;
                        if (_val == "Record Saved") {
                            $('#lblMsg').text('Password updated sucessfully.');
                            setBlank();
                            //BindGrid();
                            eventParam.preventDefault();
                            //return false;
                        }
                        else {
                            $('#lblErr').text(_val);
                            eventParam.preventDefault();
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            });
            
            $('#txtCustomerSearch').on('keyup', function (event) {
                if (event.which == '13') {
                    if ($('#txtCustomerSearch').val() == "") {
                        stateOfColor = true;
                        setDivMouseOver('Red', '#999999', stateOfColor);
                        $('#lblErr').text('Please enter customer name.');
                        return false;
                    }
                    $('#lblMsg').text('');
                    $('#lblErr').text('');

                    var myVar = setInterval(function () { customerCheck() }, 10);
                    function customerCheck() {
                        var customerdetail = $('#txtCustomerSearch').val();
                        var customercode = customerdetail.split("-");
                        var custcode = customercode[0].trim();
                        if (customercode.length == 1) {
                            setBlank();
                            stateOfColor = true;
                            setDivMouseOver('Red', '#999999', stateOfColor);
                            $('#lblErr').text('Please enter valid customer.');
                            clearInterval(myVar);
                            return false;
                        }
                        fillCustomerRecord(custcode);
                        clearInterval(myVar);
                    }
                }
            });

            $('#txtCustomerSearch').blur(function (event) {
                clearMsg();
                document.getElementById('divShowMsg').style.display = "none";
                if ($('#txtCustomerSearch').val() == "") {
                    //   $('#lblErr').text('Please enter customer name.');
                    return false;
                }
                var myVar = setInterval(function () { customerCheck() }, 400);
                function customerCheck() {
                    var customerdetail = $('#txtCustomerSearch').val();
                    var customercode = customerdetail.split("-");
                    var custcode = customercode[0].trim();
                    if (customercode.length == 1) {
                        setBlank();
                        stateOfColor = true;
                        setDivMouseOver('Red', '#999999', stateOfColor);
                        $('#lblErr').text('Please enter valid customer.');
                        clearInterval(myVar);
                        return false;
                    }
                    fillCustomerRecord(custcode);
                    clearInterval(myVar);
                }
            });

            $('#achrAmount,#achrCloth').click(function (e) {
                var PendingAmt = $('#btnAmount').text();
                var PendingCloth = $('#btnCloth').text();
                if ((PendingAmt) == 0 && (PendingCloth) == 0) {
                    return false;
                }
                window.open('../Bookings/Delivery.aspx?CustCode=' + $('#hdnCustomercode').val() + '', '_self');
                return false;
            });
            function fillCustomerRecord(custcode) {
                $.ajax({
                    url: '../AutoComplete.asmx/SetCustomerDetailInCustomerScreen',
                    type: 'GET',
                    data: "arg='" + custcode + "'",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var result = response.d;
                        // the values is in form of name, address, mobile, priority and remark, and discount    
                        $('#hdnAllCustData').val(result);
                        var resultAry = result.split(':');
                        $('#lblErr').text('');
                        $('#txtCustomerSearch').val('');
                        $('#lblCustomerCode').text(custcode);
                        $('#hdnCustomercode').val(custcode);
                        $('#hdncustcode').val(custcode);
                        $('#drpCustSalutation').val(resultAry[0]);
                        $('#txtCustName').val(resultAry[1].trim());
                        $('#txtCustomerAddress').val(resultAry[2].trim());
                        $('#txtCustomerMobile').val(resultAry[3].trim());
                        $('#txtCustomerPhone').val(resultAry[4].trim());
                        $('#txtCustomerEmailId').val(resultAry[5].trim());
                        $('#txtBirthDay').val(resultAry[6].trim());
                        $('#txtAnniversaryDate').val(resultAry[7].trim());
                        $('#drpPriority').val(resultAry[8].trim());
                        $('#txtAreaLocation').val(resultAry[9].trim());
                        $('#drpCommunicationMeans').val(resultAry[10].trim());
                        $('#txtCustomerRefferredBy').val(resultAry[11].trim());
                        $('#txtRemarks').val(resultAry[12].trim());
                        $('#drpDefaultDiscountRate').val(resultAry[13].trim());
                        $('#txtMemberShip').val(resultAry[14].trim());
                        $('#txtBarcode').val(resultAry[15].trim());
                        $('#ddlRateList').val(resultAry[16].trim());
                        $('#hdnBarcode').val(resultAry[15].trim());
                        $('#hdnMobileNo').val(resultAry[3].trim());
                        $('#hdnMembershipId').val(resultAry[14].trim());
                        var iswebsite = resultAry[17];
                        if (iswebsite === "1")
                            $('#btnReset').removeAttr('disabled');
                        else
                            $('#btnReset').attr('disabled');

                        var membership = resultAry[14].trim();
                        if (membership == "") {
                            $('#txtMemberShip').attr('disabled', false)
                        }
                        else {
                            $('#txtMemberShip').attr('disabled', true)
                        }
                        if (iswebsite == "1") {
                            $('#chkWebSite').attr('checked', true)
                            $('#hdnCustWebsite').val("True");
                        }
                        else {
                            $('#chkWebSite').attr('checked', false)
                            $('#hdnCustWebsite').val("False");
                        }

                        $('#btnCloth').text(resultAry[18]);
                        $('#btnAmount').text(resultAry[19]);
                        $('#spnLastVisitdate').text(resultAry[20]);
                        var strday = resultAry[21] + ' days';
                        $('#spnlastSince').text(strday);
                        $('#hdnCheckPackage').val(resultAry[22]);
                        document.getElementById('btnSave').style.display = "none";
                        document.getElementById('btnEdit').style.display = "none";
                        document.getElementById('btnDelete').style.display = "inline";
                        document.getElementById('btnCustEdit').style.display = "inline";
                        document.getElementById('btnSMS').style.display = "inline";
                        document.getElementById('btnReset').style.display = "inline";
                        document.getElementById('divShowMsg').style.display = "none";

                        AddControlAttribute();
                        return false;
                    },
                    error: function (response) {
                    }
                });
            }
            $('#achrNewBooking').click(function (e) {
                window.open('../New_Booking/frm_New_Booking.aspx?CustBN=' + $('#hdnCustomercode').val() + '&IsTour=' + $('#hdnIsTour').val()+'', '_self');
                return false;
            });
            $('#achrDel').click(function (e) {
                window.open('../Bookings/Delivery.aspx?CustCode=' + $('#hdnCustomercode').val() + '', '_self');               
                return false;
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnEdit").click(function (eventParam) {
                clearMsg();
                var status = checkName();
                if (status == false) {
                    return false;
                }

                var isAllowed = "";
                var IsWebSite = false;
                $('#lblErr').text('');
                if ($('#chkWebSite').attr('checked')) {
                    IsWebSite = true;
                } else {
                    IsWebSite = false;
                }

                $.ajax({
                    url: '../AutoComplete.asmx/UpdateCustomerJquery',
                    type: 'POST',
                    data: "{CustomerCode: '" + $('#lblCustomerCode').html() + "', CustomerSalutation: '" + $('#drpCustSalutation').val() + "', CustomerName: '" + $('#txtCustName').val().trim() + "', CustomerAddress: '" + $('#txtCustomerAddress').val().trim() + "', CustomerPhone: '" + $('#txtCustomerPhone').val().trim() + "', CustomerMobile: '" + $('#txtCustomerMobile').val().trim() + "', CustomerEmailId: '" + $('#txtCustomerEmailId').val() + "', CustomerPriority: '" + $('#drpPriority').val() + "', CustomerRefferredBy: '" + $('#txtCustomerRefferredBy').val() + "', DefaultDiscountRate: '" + $('#drpDefaultDiscountRate').val() + "', Remarks: '" + $('#txtRemarks').val() + "', BirthDate: '" + $('#txtBirthDay').val() + "', AnniversaryDate: '" + $('#txtAnniversaryDate').val() + "', AreaLocation: '" + $('#txtAreaLocation').val() + "', CommunicationMeans: '" + $('#drpCommunicationMeans').val() + "', MemberShipId: '" + $('#txtMemberShip').val().trim() + "', BarCode: '" + $('#txtBarcode').val().trim() + "', RateListId: '" + $('#ddlRateList').val() + "', IsWebsite: '" + IsWebSite + "', tempCustMobile: '" + $('#hdnMobileNo').val() + "', tempMemberShipId: '" + $('#hdnMembershipId').val() + "', tempBarCode: '" + $('#hdnBarcode').val() + "', tempWebsite: '" + $('#hdnCustWebsite').val() + "'}",                 
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: false,                               
                    success: function (response) {
                        var _val = response.d;
                        if (_val == "Record Saved") {
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#999999', stateOfColor)
                            $('#lblMsg').text('Customer information updated sucessfully.');
                            document.getElementById('btnSMS').style.display = "inline";
                            document.getElementById('btnCustEdit').style.display = "inline";
                            document.getElementById('btnEdit').style.display = "none";
                            AddControlAttribute();
                            eventParam.preventDefault();
                        }
                        else {
                            stateOfColor = true;
                            setDivMouseOver('Red', '#999999', stateOfColor)
                            $('#lblErr').text(_val);
                            eventParam.preventDefault();
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var stateOfColor = true;
            $("#btnSave").click(function (eventParam) {

                $('#lblErr').text('');
                $('#lblMsg').text('');
                var status = checkName();
                if (status == false) {
                    return false;
                }
                var isAllowed = "";
                var IsWebSite = false;
                $('#lblErr').text('');
                $('#lblMsg').text('');
                if ($('#chkWebSite').attr('checked')) {
                    IsWebSite = true;
                } else {
                    IsWebSite = false;
                }
                $.ajax({
                    url: '../AutoComplete.asmx/SaveNewCustomerJquery',
                    type: 'GET',
                    data: "CustomerSalutation='" + $('#drpCustSalutation').val() + "'&CustomerName='" + $('#txtCustName').val().trim() + "'&CustomerAddress='" + $('#txtCustomerAddress').val().trim() + "'&CustomerPhone='" + $('#txtCustomerPhone').val().trim() + "'&CustomerMobile='" + $('#txtCustomerMobile').val().trim() + "'&CustomerEmailId='" + $('#txtCustomerEmailId').val() + "'&CustomerPriority='" + $('#drpPriority').val() + "'&CustomerRefferredBy='" + $('#txtCustomerRefferredBy').val() + "'&DefaultDiscountRate='" + $('#drpDefaultDiscountRate').val() + "'&Remarks='" + $('#txtRemarks').val() + "'&BirthDate='" + $('#txtBirthDay').val() + "'&AnniversaryDate='" + $('#txtAnniversaryDate').val() + "'&AreaLocation='" + $('#txtAreaLocation').val() + "'&CommunicationMeans='" + $('#drpCommunicationMeans').val() + "'&MemberShipId='" + $('#txtMemberShip').val().trim() + "'&BarCode='" + $('#txtBarcode').val().trim() + "'&RateListId='" + $('#ddlRateList').val() + "'&IsWebsite='" + IsWebSite + "' ",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var _val = response.d;
                        if (_val.indexOf('Cust') != -1) {
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#999999', stateOfColor)
                            $('#lblMsg').text('New Customer created sucessfully');
                            $('#lblCustomerCode').text(_val);
                            $('#hdnCustomercode').val(_val);
                            AddControlAttribute();
                            document.getElementById('btnCustEdit').style.display = "inline";
                            document.getElementById('btnReset').style.display = "inline";
                            document.getElementById('btnDelete').style.display = "inline";
                            document.getElementById('btnSave').style.display = "none";
                            if ($('#hdnIsTour').val() == "1"){ NewCustomerTour(2); }
                            eventParam.preventDefault();
                            //return false;
                        }
                        else {
                            stateOfColor = true;
                            setDivMouseOver('Red', '#999999', stateOfColor)
                            $('#lblErr').text(_val);
                            eventParam.preventDefault();
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            });
            $("#btnDelete").click(function (eventParam) {
                clearMsg();

                if ($('#hdnCheckPackage').val().trim() == "True") {
                    setDivMouseOver('Red', '#999999', stateOfColor);
                    //$('#lblMsg').text("Customer name has a Package Assigned, so it can't be deleted.");
                    $('#lblMsg').text('Customer ' + $("#txtCustName").val() + ' cannot be deleted as there is an active package assigned to this customer. You can delete the customer after concluding the package.');
                    return false;
                }

                $.ajax({
                    url: '../AutoComplete.asmx/DeleteCustomerJquery',
                    type: 'GET',
                    data: "CustomerCode='" + $('#lblCustomerCode').html() + "'",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var _val = response.d;
                        if (_val == "Record Saved") {
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#999999', stateOfColor)
                            $('#lblMsg').text('Customer deleted sucessfully.');
                            setBlank();
                            document.getElementById('btnSMS').style.display = "none";
                            document.getElementById('btnSave').style.display = "none";
                            document.getElementById('btnReset').style.display = "none";
                            eventParam.preventDefault();
                            //return false;
                        }
                        else {
                            stateOfColor = true;
                            setDivMouseOver('Red', '#999999', stateOfColor)
                            $('#lblErr').text(_val);
                            eventParam.preventDefault();
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            });

            $("#btnAddNew").click(function (eventParam) {
                setBlank();
                RemoveControlAttribute();
                $('#txtCustName').focus();

                document.getElementById('btnSMS').style.display = "none";
                document.getElementById('divShowMsg').style.display = "none";
                document.getElementById('btnReset').style.display = "none";
                clearMsg();
                eventParam.preventDefault();
            });

            $("#btnCustEdit").click(function (eventParam) {
                clearMsg();
                RemoveControlAttribute();
                document.getElementById('btnEdit').style.display = "inline";
                document.getElementById('btnReset').style.display = "inline";
                document.getElementById('btnCustEdit').style.display = "none";

                document.getElementById('btnSMS').style.display = "none";
                document.getElementById('divShowMsg').style.display = "none";
                CheckPackageAssignToCustomer();
                $('#txtCustName').focus();
                eventParam.preventDefault();
            });

            function CheckPackageAssignToCustomer() {
                $.ajax({
                    url: '../AutoComplete.asmx/IsPackaAssignToCustomer',
                    type: 'GET',
                    timeout: 2000,
                    data: "CustCode='" + $('#lblCustomerCode').text() + "'",
                    processData: false,
                    datatype: 'json',
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        var _pkg = response.d;
                        if (_pkg == true) {
                            $('#drpDefaultDiscountRate').attr('disabled', true)
                        }
                        else {
                            $('#drpDefaultDiscountRate').attr('disabled', false)
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }

            var Date = $('#txtBirthDay,#txtAnniversaryDate');
            Date.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd M yyyy",
                language: "tr"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });


            $('#btnSMS').click(function (eventParam) {
                clearMsg();
                var mobileno = $('#txtCustomerMobile').val();
                if (mobileno == "") {
                    stateOfColor = true;
                    setDivMouseOver('Red', '#999999', stateOfColor)
                    $('#lblErr').text('Mobile number not found kindly update mobile number to send reminder');
                    return false;
                }
                $.ajax({
                    url: 'frmNewCustomer.aspx/SendPendingSMS',
                    type: 'GET',
                    data: "CustCode='" + $('#hdncustcode').val() + "'&smsValue='" + $('#drpsmstemplate').val() + "'",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var _val = response.d;
                        if (_val == "True") {
                            stateOfColor = true;
                            setDivMouseOver('#00aa00', '#999999', stateOfColor)
                            $('#lblMsg').text('Reminder SMS sent sucessfully');
                            eventParam.preventDefault();

                        }
                        else {
                            stateOfColor = true;
                            setDivMouseOver('Red', '#999999', stateOfColor)
                            $('#lblErr').text('No pending amount and clothes found for this customer.');
                            eventParam.preventDefault();
                        }
                    },
                    error: function (response) {
                        alert(response.toString())
                    }
                });
            });

        });
    </script>
    <script type="text/javascript" language="javascript">
        function setDivMouseOver(argColorOne, argColorTwo, stateOfColor) {
            document.getElementById('divShowMsg').style.display = "inline";

            if (stateOfColor) {
                //  $('#DivContainerInnerStatus').animate({ backgroundColor: argColorOne }, 200).delay(1000).animate({ backgroundColor: argColorTwo }, 100);
                $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            }
            else {
                $('#DivContainerInnerStatus').animate({ backgroundColor: argColorTwo }, 1000);
                $('#DivContainerInnerStatus').animate({ backgroundColor: argColorOne }, 100);
            }
            stateOfColor = !stateOfColor;
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
        function clearMsg() {

            $('#lblErr').text('');
            $('#lblMsg').text('');
        }


        function checkName() {
            //
            var strname = document.getElementById("<%=txtCustName.ClientID %>").value.trim().length;
            var stradd = document.getElementById("<%=txtCustomerAddress.ClientID %>").value.trim().length;
            var strEmail = document.getElementById("<%=txtCustomerEmailId.ClientID %>").value.trim().length;
            var strPhn = document.getElementById("<%=txtCustomerMobile.ClientID %>").value.trim().length;
            var CustomerPreference = document.getElementById("<%=drpCommunicationMeans.ClientID %>");
            var DiscountRate = document.getElementById("<%=drpDefaultDiscountRate.ClientID %>").value;
            var selectedText = CustomerPreference.options[CustomerPreference.selectedIndex].text;
            if (strname == "" || strname.length == 0) {
                document.getElementById('divShowMsg').style.display = "inline";
                stateOfColor = true;
                setDivMouseOver('#FA8602', '#999999', stateOfColor)
                $('#lblMsg').text('Please enter Customer Name');
                document.getElementById("<%=txtCustName.ClientID %>").focus();
                return false;
            }
            if (stradd == "" || stradd.length == 0) {
                document.getElementById('divShowMsg').style.display = "inline";
                stateOfColor = true;
                setDivMouseOver('#FA8602', '#999999', stateOfColor)
                $('#lblMsg').text('Please enter Customer Address');
                document.getElementById("<%=txtCustomerAddress.ClientID %>").focus();
                return false;
            }
            if (selectedText == "Only SMS") {
                if (strPhn == "" || strPhn.length == 0) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Please enter Customer Phone no');
                    document.getElementById("<%=txtCustomerMobile.ClientID %>").focus();
                    return false;
                }
            }
            if (selectedText == "Only Email") {
                if (strEmail == "" || strEmail.length == 0) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Please enter Customer Email id');
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            }
            if (selectedText == "Both SMS and Email") {
                if (strPhn == "" || strPhn.length == 0) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Please enter Customer Phone no');
                    document.getElementById("<%=txtCustomerMobile.ClientID %>").focus();
                    return false;
                }
                if (strEmail == "" || strEmail.length == 0) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Please enter Customer Email id');
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            }
            if (DiscountRate != "") {
                if (parseFloat(DiscountRate) < 0 || parseFloat(DiscountRate) > 100) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Discount should be between 0 - 100');
                    document.getElementById("<%=drpDefaultDiscountRate.ClientID %>").focus();
                    return false;
                }
            }
            if ($('#chkWebSite').attr('checked')) {
                if (strEmail == "" || strEmail.length == 0) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Please enter Customer Email id');
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            }
            if (strEmail != "") {
                var valid = checkEmail(document.getElementById("<%=txtCustomerEmailId.ClientID %>").value);
                if (valid == false) {
                    document.getElementById('divShowMsg').style.display = "inline";
                    stateOfColor = true;
                    setDivMouseOver('#FA8602', '#999999', stateOfColor)
                    $('#lblMsg').text('Not a valid e-mail address');
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            }
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

        function setBlank() {
            $('#txtCustomerSearch').val('');
            $('#lblCustomerCode').text('');
            $('#drpCustSalutation').val('');
            $('#txtCustName').val('');
            $('#txtCustomerAddress').val('');
            $('#txtCustomerMobile').val('');
            $('#txtCustomerPhone').val('');
            $('#txtCustomerEmailId').val('');
            $('#txtBirthDay').val('');
            $('#txtAnniversaryDate').val('');
            $('#drpPriority').val('');
            $('#txtAreaLocation').val('');
            $('#drpCommunicationMeans').val('');
            $('#txtCustomerRefferredBy').val('');
            $('#txtRemarks').val('');
            $('#drpDefaultDiscountRate').val('');
            $('#txtMemberShip').val('');
            $('#txtBarcode').val('');
            $('#ddlRateList').val('');
            $('#hdnBarcode').val('');
            $('#hdnMobileNo').val('');
            $('#hdnCustomercode').val('');
            $('#hdnMembershipId').val('');
            $('#chkWebSite').attr('checked', false)
            $('#txtMemberShip').attr('disabled', false)
            document.getElementById('btnSave').style.display = "inline";
            document.getElementById('btnEdit').style.display = "none";
            document.getElementById('btnDelete').style.display = "none";
            document.getElementById('btnCustEdit').style.display = "none";
            $('#divCustDetail').hide();
            $('#btnAmount').text('');
            $('#btnCloth').text('');
            $('#spnLastVisitdate').text('');
            $('#spnlastSince').text('');
        }
        function BindGrid() {
            $.ajax({
                url: '../AutoComplete.asmx/GetCustomersData',
                type: 'POST',
                data: '{}',
                timeout: 20000,
                contentType: 'application/json; charset=UTF-8',
                cache: true,
                async: false,
                datatype: 'JSON',
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }
        function OnSuccess(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var customers = xml.find("Table");
            var row = $("[id*=grdSearchResult] tr:last-child").clone(true);
            $("[id*=grdSearchResult] tr").not($("[id*=grdSearchResult] tr:first-child")).remove();
            $.each(customers, function () {
                var customer = $(this);
                $("td", row).eq(0).html($(this).find("CustomerCode").text());
                $("td", row).eq(1).html($(this).find("CustomerName").text());
                $("td", row).eq(2).html($(this).find("CustomerAddress").text());
                $("td", row).eq(3).html($(this).find("CustomerPhone").text());
                $("td", row).eq(4).html($(this).find("CustomerMobile").text());
                $("td", row).eq(5).html($(this).find("CustomerEmailId").text());
                $("td", row).eq(6).html($(this).find("Priority").text());
                $("td", row).eq(7).html($(this).find("CustomerRefferredBy").text());
                $("td", row).eq(8).html($(this).find("DefaultDiscountRate").text());
                $("td", row).eq(9).html($(this).find("IsWebsite").text());
                $("[id*=grdSearchResult]").append(row);
                row = $("[id*=grdSearchResult] tr:last-child").clone(true);
            });
        }
        function RemoveControlAttribute() {

            $('#drpCustSalutation').removeAttr('disabled');
            $('#txtCustName').removeAttr('disabled');
            $('#txtCustomerAddress').removeAttr('disabled');
            $('#txtCustomerMobile').removeAttr('disabled');
            $('#txtCustomerEmailId').removeAttr('disabled');
            $('#txtMemberShip').removeAttr('disabled');
            $('#txtBarcode').removeAttr('disabled');
            $('#chkWebSite').removeAttr('disabled');
            $('#txtAreaLocation').removeAttr('disabled');
            $('#drpPriority').removeAttr('disabled');
            $('#drpDefaultDiscountRate').removeAttr('disabled');
            $('#drpCommunicationMeans').removeAttr('disabled');
            $('#ddlRateList').removeAttr('disabled');
            $('#txtBirthDay').removeAttr('disabled');
            $('#txtAnniversaryDate').removeAttr('disabled');
            $('#txtRemarks').removeAttr('disabled');
            $('#txtCustomerRefferredBy').removeAttr('disabled');
            $('#txtCustomerPhone').removeAttr('disabled');
        }
        function AddControlAttribute() {
            $('#drpCustSalutation').attr("disabled", "disabled");
            $('#txtCustName').attr("disabled", "disabled");
            $('#txtCustomerAddress').attr("disabled", "disabled");
            $('#txtCustomerMobile').attr("disabled", "disabled");
            $('#txtCustomerEmailId').attr("disabled", "disabled");
            $('#txtMemberShip').attr("disabled", "disabled");
            $('#txtBarcode').attr("disabled", "disabled");
            $('#chkWebSite').attr("disabled", "disabled");
            $('#txtAreaLocation').attr("disabled", "disabled");
            $('#drpPriority').attr("disabled", "disabled");
            $('#drpDefaultDiscountRate').attr("disabled", "disabled");
            $('#drpCommunicationMeans').attr("disabled", "disabled");
            $('#ddlRateList').attr("disabled", "disabled");
            $('#txtBirthDay').attr("disabled", "disabled");
            $('#txtAnniversaryDate').attr("disabled", "disabled");
            $('#txtRemarks').attr("disabled", "disabled");
            $('#txtCustomerRefferredBy').attr("disabled", "disabled");
            $('#txtCustomerPhone').attr("disabled", "disabled");
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#hdnControlWidth').val($('#txtCustomerSearch').width()-17); 
            showAllCustomer('#txtCustomerSearch');
        });       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
            <div id="DivContainerStatus">
                <div id="DivContainerInnerStatus" class="span label label-default">
                    <h4 class="text-center">
                         <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                            ForeColor="White" />
                          <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage"
                            ForeColor="White" ClientIDMode="Static" />                             
                    </h4>
                </div>
            </div>
        </div>
    <div class="row-fluid div-margin">
        <div class="row-fluid">
            <div class="row-fluid">
                <div class="col-sm-8">
                    <div class="panel panel-primary " id="pnlNewCustomer">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Customer Detail</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid div-margin">
                                <div class="col-sm-12 input-group input-group-sm">
                                    <span class="input-group-addon IconBkColor"><i class="fa fa-search fa-lg"></i></span>
                                    <asp:TextBox ID="txtCustomerSearch" runat="server" ToolTip="Please enter customer name."
                                        placeholder="Search Customer here to Edit/Delete a Customer" CssClass="form-control"
                                        ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="col-sm-6">
                                    <div class="row-fluid">
                                        <div class="col-sm-8 div-margin Textpadding">
                                            <div class="input-group input-group-sm ">
                                                <span style="color: Gray" class="textBold">Customer Code</span>&nbsp;
                                                <asp:Label ID="lblCustomerCode" runat="server" ClientIDMode="Static" ForeColor="#666666"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon IconBkColor"><i class="fa fa-user fa-lg"></i></span>
                                            <div class="row-fluid">
                                                <div class="col-sm-3 Textpadding">
                                                    <asp:DropDownList ID="drpCustSalutation" runat="server" EnableTheming="false" ClientIDMode="Static"
                                                        CssClass="form-control" BackColor="White">
                                                        <asp:ListItem Selected="True" Value=" "></asp:ListItem>
                                                        <asp:ListItem>Mr</asp:ListItem>
                                                        <asp:ListItem>Mrs</asp:ListItem>
                                                        <asp:ListItem>Ms</asp:ListItem>
                                                        <asp:ListItem>Dr</asp:ListItem>
                                                        <asp:ListItem>M/S</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-9 Textpadding">
                                                    <div class="col-sm-11 Textpadding">
                                                        <asp:TextBox ID="txtCustName" runat="server" Width="105%" ClientIDMode="Static" CssClass="form-control"
                                                            placeholder="Name"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtCustName" InvalidChars="`~:;,-.#&'" FilterMode="InvalidChars">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </div>
                                                    <div class="col-sm-1 Textpadding">
                                                        <span class="textRed" style="float: right">*</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-11 Textpadding">
                                            <asp:TextBox ID="txtCustomerAddress" runat="server" Rows="2" ClientIDMode="Static"
                                                MaxLength="100" Width="106%" TextMode="MultiLine" placeholder="Address" CssClass="form-control"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterMode="InvalidChars" TargetControlID="txtCustomerAddress" InvalidChars="`~:;,-.#&'">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                        <div class="col-sm-1 Textpadding">
                                            <span class="textRed" style="float: right">*</span>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-5 Textpadding padding4">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor"><i class="fa fa-phone fa-lg"></i></span>
                                                <asp:TextBox ID="txtCustomerMobile" placeholder="Mobile No" runat="server" ClientIDMode="Static"
                                                    MaxLength="20" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtCustomerMobile_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerMobile">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="col-sm-7 Textpadding">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor"><i class="fa fa-envelope-o fa-lg"></i>
                                                </span>
                                                <asp:TextBox ID="txtCustomerEmailId" runat="server" placeholder="Email ID" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-6 Textpadding padding4">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor textBold">Prefrence</span>
                                                <asp:DropDownList ID="drpPriority" runat="server" EnableTheming="false" CssClass="form-control"
                                                    BackColor="White" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 Textpadding"  id="divDefaultDiscount" runat="server" clientidmode="Static">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor textBold">Discount(%)</span>
                                                <asp:TextBox ID="drpDefaultDiscountRate" runat="server" MaxLength="5" CssClass="form-control"
                                                    placeholder="Default Discount" ClientIDMode="Static"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="drpDefaultDiscountRate_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" TargetControlID="drpDefaultDiscountRate" ValidChars="1234567890.">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-6 Textpadding padding4">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor"><i class="fa fa-bullhorn fa-lg"></i>
                                                </span>
                                                <asp:DropDownList ID="drpCommunicationMeans" runat="server" ClientIDMode="Static"
                                                    BackColor="White" EnableTheming="false" CssClass="form-control">
                                                    <asp:ListItem Text="None" Value="NA" />
                                                    <asp:ListItem Text="Only SMS" Value="SMS" />
                                                    <asp:ListItem Text="Only Email" Value="Email" />
                                                    <asp:ListItem Text="Both SMS and Email" Value="Both" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 Textpadding">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor textBold">Rate List</span>
                                                <asp:DropDownList ID="ddlRateList" runat="server" EnableTheming="false" ClientIDMode="Static"
                                                    BackColor="White" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="row-fluid div-margin">
                                        <asp:TextBox ID="txtAreaLocation" runat="server" TextMode="MultiLine" MaxLength="100"
                                            placeholder="Area Location" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" Enabled="True"
                                            FilterMode="InvalidChars" TargetControlID="txtAreaLocation" InvalidChars="`~:;,-.#&'">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-6 Textpadding padding4">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor"><i class="fa fa-users fa-lg"></i></span>
                                                <asp:TextBox ID="txtMemberShip" runat="server" placeholder="Membership ID" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtMemberShip"
                                                    FilterMode="InvalidChars" InvalidChars="`~:;,-.#&'">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 Textpadding">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor"><i class="fa fa-barcode fa-lg"></i></span>
                                                <asp:TextBox ID="txtBarcode" CssClass="form-control" runat="server" ClientIDMode="Static"
                                                    placeholder="Barcode"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                    TargetControlID="txtBarcode" InvalidChars="`~:;,-.#&'" FilterMode="InvalidChars">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-6 Textpadding">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor">
                                                    <img src="../images/Birthday.png" width="15px" height="18px" /></span>
                                                <asp:TextBox ID="txtBirthDay" runat="server" ClientIDMode="Static" placeholder="Birthday"
                                                    onpaste="return false;" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 Textpadding">
                                            <div class="input-group input-group-sm">
                                                &nbsp;<span class="input-group-addon IconBkColor" style="padding: 0px"><img src="../img/anniversary.png"
                                                    width="30px" height="28px" /></span>
                                                <asp:TextBox ID="txtAnniversaryDate" runat="server" onpaste="return false;" placeholder="Anniversary"
                                                    ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon IconBkColor"><i class="fa fa-bell-o fa-lg"></i></span>
                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" CssClass="form-control"
                                                placeholder="Remarks" ClientIDMode="Static" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtRemarks"
                                                FilterMode="InvalidChars" InvalidChars="`~:;,-.#&">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-6 Textpadding padding4">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor textBold">Refered by</span>
                                                <asp:TextBox ID="txtCustomerRefferredBy" runat="server" MaxLength="50" CssClass="form-control"
                                                    placeholder="Name" ClientIDMode="Static" />
                                                <cc1:FilteredTextBoxExtender ID="txtCustomerRefferredByExt" runat="server" Enabled="True"
                                                    TargetControlID="txtCustomerRefferredBy" InvalidChars="`~:;,-.#&" FilterMode="InvalidChars">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 Textpadding">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor">
                                                    <img width="15px" height="18px" class="" src="../images/landline.png"></span>
                                                <asp:TextBox ID="txtCustomerPhone" runat="server" placeholder="Phone" ClientIDMode="Static"
                                                    MaxLength="20" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtCustomerPhone_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerPhone">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid div-margin">
                                        <div class="col-sm-5 Textpadding">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon IconBkColor textBold">Register For Website</span>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div style="margin-top: 9px">
                                                <asp:CheckBox ID="chkWebSite" CssClass="checkboxCss" runat="server" ClientIDMode="Static"
                                                    ForeColor="Gray" EnableTheming="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <a class="btn btn-info margin4" id="btnAddNew"><i class="fa fa-user"></i>&nbsp;&nbsp;Add
                                New Customer </a><a class="btn btn-info margin4" id="btnEdit" style="display: none">
                                    <i class="fa fa-user"></i>&nbsp;&nbsp;Update </a><a class="btn btn-info margin4"
                                        id="btnSave"><i class="fa fa-floppy-o"></i>&nbsp;&nbsp;Save </a><a class="btn btn-info margin4 "
                                            id="btnCustEdit" style="display: none"><i class="fa fa-pencil-square-o"></i>&nbsp;&nbsp;Edit
                                        </a><a class="btn btn-info margin4 " id="btnSMS" style="display: none"><i class="fa  fa-mobile">
                                        </i>&nbsp;&nbsp;Send Reminder SMS </a>
                            <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset website credentials"
                                CssClass="btn btn-info margin4" Enabled="false" EnableTheming="false" Style="display: none" />
                            <a class="btn btn-info margin4" id="btnDelete" style="display: none"><i class="fa fa-trash-o">
                            </i>&nbsp;&nbsp;Delete </a>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3" style="margin-left: -10px">
                    <div class="panel panel-primary ">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Pending</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid ">
                                <div id="achrAmount" class="col-sm-5 img-circle circlebox mousePointer">
                                    <br />
                                    <div>
                                        <img src="../img/Money2.png" width="25px" height="30px" /></div>
                                    <div class="div-margin">
                                        <span class="img-responsive fa-lg textBold txtWhite" id="btnAmount" runat="server"
                                            clientidmode="Static">&nbsp;</span></div>
                                </div>
                                <div class="col-sm-1">
                                </div>
                                <div id="achrCloth" class="col-sm-5 img-circle circlebox mousePointer ">
                                    <br />
                                    <div>
                                        <img src="../img/cloth.png" width="25px" height="25px" /></div>
                                    <div class="div-margin">
                                        <span id="btnCloth" runat="server" class="img-responsive fa-lg textBold txtWhite"
                                            clientidmode="Static">&nbsp;</span></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-primary ">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                Last Visit</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row-fluid">
                                <div class="col-sm-5 img-circle circlebox ">
                                    <br />
                                    <div>
                                        <i class="fa fa-calendar fa-lg txtWhite"></i>
                                    </div>
                                    <div class="div-margin">
                                        <span id="spnLastVisitdate" runat="server" clientidmode="Static" class=" img-responsive textBold txtWhite">
                                        </span>
                                    </div>
                                </div>
                                <div class="col-sm-1">
                                </div>
                                <div class="col-sm-5 img-circle  circlebox">
                                    <br />
                                    <div>
                                        <i class="fa fa-clock-o fa-2x txtWhite"></i>
                                    </div>
                                    <div class="div-margin">
                                        <span id="spnlastSince" clientidmode="Static" class="img-responsive fa-lg textBold txtWhite">
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1 Textpadding">
                    <br />
                    <br />
                    <br />                  
                    <a id="achrNewBooking" class="btn btn-primary active btn-lg btn-block ">
                        <br />
                        &nbsp;New
                        <br />
                        Order<br />
                        &nbsp;</a>

                        <a id="achrDel" class="btn btn-primary active btn-lg btn-block ">
                        <br />
                        &nbsp;Pick
                        <br />
                        Up<br />
                        &nbsp;</a>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdntempNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBarcode" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnMobileNo" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnMembershipId" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnCustomercode" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnCustWebsite" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnIsCustCode" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdncustcode" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnAllCustData" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnIsNewCustomer" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnNewCustName" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnIsNew" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnCheckPackage" ClientIDMode="Static" runat="server" /> 
       <asp:HiddenField ID="hdnControlWidth" ClientIDMode="Static" runat="server" />
      <asp:HiddenField ID="hdnIsTour" ClientIDMode="Static" Value="0" runat="server" />
      <asp:HiddenField ID="hdnChkLastControlTour" ClientIDMode="Static" Value="0" runat="server" />
    <asp:DropDownList ID="drpsmstemplate" runat="server" EnableTheming="false" ClientIDMode="Static"
        Style="display: none">
    </asp:DropDownList>
     <script src="../JavaScript/intro.js" type="text/javascript"></script>   
    <script src="../JavaScript/AppTour.js" type="text/javascript"></script>
      <script type="text/javascript">
          $(document).ready(function () {
              if (RegExp('IsTour=1', 'gi').test(window.location.search)) {
                  NewCustomerTour(1);
                  $('#hdnIsTour').val('1');
              }
              $('.introjs-skipbutton').click(function () {
                  $('#txtCustName').focus();
                  $('#hdnIsTour').val('0');
                  if ($('#hdnChkLastControlTour').val() == "1") {
                      $('#hdnIsTour').val('1');
                      window.open('../New_Booking/frm_New_Booking.aspx?CustBN=' + $('#hdnCustomercode').val() + '&IsTour=' + $('#hdnIsTour').val() + '', '_self');
                  }
              });
          });
      </script>
</asp:Content>
