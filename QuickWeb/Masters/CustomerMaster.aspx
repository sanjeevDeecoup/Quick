<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    Inherits="CustomerMaster" Title="Customer Master" CodeBehind="CustomerMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-blink.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('.blink').blink();

            $('#chkWebSite').click(function () {
                if ($('#chkWebSite').is(':checked')) {
                    $('#btnReset').removeAttr('disabled');
                }
                else {
                    ('#btnReset').attr('disabled');
                }
            });
            $('#btnReset').click(function (eventParam) {

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


            $('#txtCustomerSearch').live('keydown', function (event) {
                if (event.keyCode == 9) {
                    if ($('#txtCustomerSearch').val() == "") {
                        $('#lblErr').text('Please enter customer name.');
                        return false;
                    }
                    $('#lblMsg').text('');
                    $('#lblErr').text('');
                    var customerdetail = $('#txtCustomerSearch').val();
                    var customercode = customerdetail.split("-");
                    var custcode = customercode[0].trim();
                    if (customercode.length == 1) {
                        setBlank();
                        $('#lblErr').text('Please enter valid customer.');
                        return false;
                    }
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
                            var resultAry = result.split(':');
                            $('#lblErr').text('');
                            $('#txtCustomerSearch').val('');
                            $('#lblCustomerCode').text(custcode);
                            $('#hdnCustomercode').val(custcode);
                            $('#drpCustSalutation').val(resultAry[0]);
                            $('#txtCustomerName').val(resultAry[1].trim());
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
                            document.getElementById('btnSave').style.display = "none";
                            document.getElementById('btnEdit').style.display = "inline";
                            document.getElementById('btnDelete').style.display = "inline";
                            return false;
                        },
                        error: function (response) {
                        }
                    });
                }
            });


            $('#txtCustomerSearch').on('keyup', function (event) {
                if (event.which == '13') {
                    if ($('#txtCustomerSearch').val() == "") {
                        $('#lblErr').text('Please enter customer name.');
                        return false;
                    }
                    $('#lblMsg').text('');
                    $('#lblErr').text('');
                    var customerdetail = $('#txtCustomerSearch').val();
                    var customercode = customerdetail.split("-");
                    var custcode = customercode[0].trim();
                    if (customercode.length == 1) {
                        setBlank();
                        $('#lblErr').text('Please enter valid customer.');
                        return false;
                    }

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
                            var resultAry = result.split(':');
                            $('#lblErr').text('');
                            $('#txtCustomerSearch').val('');
                            $('#lblCustomerCode').text(custcode);
                            $('#hdnCustomercode').val(custcode);
                            $('#drpCustSalutation').val(resultAry[0]);
                            $('#txtCustomerName').val(resultAry[1].trim());
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
                            document.getElementById('btnSave').style.display = "none";
                            document.getElementById('btnEdit').style.display = "inline";
                            document.getElementById('btnDelete').style.display = "inline";
                            return false;
                        },
                        error: function (response) {
                        }
                    });
                }
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {           
            $("#btnEdit").click(function (eventParam) {
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
                    type: 'GET',
                    data: "CustomerCode='" + $('#lblCustomerCode').html() + "'&CustomerSalutation='" + $('#drpCustSalutation').val() + "'&CustomerName='" + $('#txtCustomerName').val().trim() + "'&CustomerAddress='" + $('#txtCustomerAddress').val().trim() + "'&CustomerPhone='" + $('#txtCustomerPhone').val().trim() + "'&CustomerMobile='" + $('#txtCustomerMobile').val().trim() + "'&CustomerEmailId='" + $('#txtCustomerEmailId').val() + "'&CustomerPriority='" + $('#drpPriority').val() + "'&CustomerRefferredBy='" + $('#txtCustomerRefferredBy').val() + "'&DefaultDiscountRate='" + $('#drpDefaultDiscountRate').val() + "'&Remarks='" + $('#txtRemarks').val() + "'&BirthDate='" + $('#txtBirthDay').val() + "'&AnniversaryDate='" + $('#txtAnniversaryDate').val() + "'&AreaLocation='" + $('#txtAreaLocation').val() + "'&CommunicationMeans='" + $('#drpCommunicationMeans').val() + "'&MemberShipId='" + $('#txtMemberShip').val().trim() + "'&BarCode='" + $('#txtBarcode').val().trim() + "'&RateListId='" + $('#ddlRateList').val() + "'&IsWebsite='" + IsWebSite + "' &tempCustMobile='" + $('#hdnMobileNo').val() + "' &tempMemberShipId='" + $('#hdnMembershipId').val() + "' &tempBarCode='" + $('#hdnBarcode').val() + "'&tempWebsite='" + $('#hdnCustWebsite').val() + "'",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var _val = response.d;
                        if (_val == "Record Saved") {
                            $('#lblMsg').text('Record updated sucessfully.');
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
        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnSave").click(function (eventParam) {
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
                    data: "CustomerSalutation='" + $('#drpCustSalutation').val() + "'&CustomerName='" + $('#txtCustomerName').val().trim() + "'&CustomerAddress='" + $('#txtCustomerAddress').val().trim() + "'&CustomerPhone='" + $('#txtCustomerPhone').val().trim() + "'&CustomerMobile='" + $('#txtCustomerMobile').val().trim() + "'&CustomerEmailId='" + $('#txtCustomerEmailId').val() + "'&CustomerPriority='" + $('#drpPriority').val() + "'&CustomerRefferredBy='" + $('#txtCustomerRefferredBy').val() + "'&DefaultDiscountRate='" + $('#drpDefaultDiscountRate').val() + "'&Remarks='" + $('#txtRemarks').val() + "'&BirthDate='" + $('#txtBirthDay').val() + "'&AnniversaryDate='" + $('#txtAnniversaryDate').val() + "'&AreaLocation='" + $('#txtAreaLocation').val() + "'&CommunicationMeans='" + $('#drpCommunicationMeans').val() + "'&MemberShipId='" + $('#txtMemberShip').val().trim() + "'&BarCode='" + $('#txtBarcode').val().trim() + "'&RateListId='" + $('#ddlRateList').val() + "'&IsWebsite='" + IsWebSite + "' ",
                    timeout: 20000,
                    contentType: 'application/json; charset=UTF-8',
                    datatype: 'JSON',
                    cache: true,
                    async: false,
                    success: function (response) {
                        var _val = response.d;
                        if (_val == "Record Saved") {
                            $('#lblMsg').text('Record Saved.');
                            setBlank();
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
            $("#btnDelete").click(function (eventParam) {
                var success = confirm('Are you sure you want to delete this customer?');
                if (success == false) {
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
                            $('#lblMsg').text('Record deleted sucessfully.');
                            setBlank();
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
        });
    </script>
    <script type="text/javascript" language="javascript">
        function checkName() {
            //
            var strname = document.getElementById("<%=txtCustomerName.ClientID %>").value.trim().length;
            var stradd = document.getElementById("<%=txtCustomerAddress.ClientID %>").value.trim().length;
            var strEmail = document.getElementById("<%=txtCustomerEmailId.ClientID %>").value.trim().length;
            var strPhn = document.getElementById("<%=txtCustomerMobile.ClientID %>").value.trim().length;
            var CustomerPreference = document.getElementById("<%=drpCommunicationMeans.ClientID %>");
            var DiscountRate = document.getElementById("<%=drpDefaultDiscountRate.ClientID %>").value;
            var selectedText = CustomerPreference.options[CustomerPreference.selectedIndex].text;
            if (strname == "" || strname.length == 0) {
                alert("Please enter Customer Name");
                document.getElementById("<%=txtCustomerName.ClientID %>").focus();
                return false;
            }
            if (stradd == "" || stradd.length == 0) {
                alert("Please enter Customer Address");
                document.getElementById("<%=txtCustomerAddress.ClientID %>").focus();
                return false;
            }
            if (selectedText == "Only SMS") {
                if (strPhn == "" || strPhn.length == 0) {
                    alert("Please enter Customer Phone no");
                    document.getElementById("<%=txtCustomerMobile.ClientID %>").focus();
                    return false;
                }
            }
            if (selectedText == "Only Email") {
                if (strEmail == "" || strEmail.length == 0) {
                    alert("Please enter Customer Email id");
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            }
            if (selectedText == "Both SMS and Email") {
                if (strPhn == "" || strPhn.length == 0) {
                    alert("Please enter Customer Phone no");
                    document.getElementById("<%=txtCustomerMobile.ClientID %>").focus();
                    return false;
                }
                if (strEmail == "" || strEmail.length == 0) {
                    alert("Please enter Customer Email id");
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            }
            if (DiscountRate != "") {
                if (parseFloat(DiscountRate) < 0 || parseFloat(DiscountRate) > 100) {
                    alert("Discount should be between 0 - 100");
                    document.getElementById("<%=drpDefaultDiscountRate.ClientID %>").focus();
                    return false;
                }
            }
            if ($('#chkWebSite').attr('checked')) {
                if (strEmail == "" || strEmail.length == 0) {
                    alert("Please enter Customer Email id");
                    document.getElementById("<%=txtCustomerEmailId.ClientID %>").focus();
                    return false;
                }
            } 
            if (strEmail != "") {
                var valid = checkEmail(document.getElementById("<%=txtCustomerEmailId.ClientID %>").value);
                if (valid == false) {
                    alert("Not a valid e-mail address");
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
            $('#txtCustomerName').val('');
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
            $('#txtCustomerSearch').focus();
            $('#divCustDetail').hide();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Customer
                    Creation
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td class="H1" style="font-weight: bolder" align="center">
                    <span class="blink">
                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ErrorMessage"
                            ClientIDMode="Static" /></span>
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="SuccessMessage"
                        ClientIDMode="Static" />
                    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableData">
            <tr>
                <td align="left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableData">
                        <tr>
                            <td class="TDCaption">
                                Customer Search :
                            </td>
                            <td align="left" nowrap="nowrap">
                                <asp:TextBox ID="txtCustomerSearch" runat="server" Width="1000px" Height="30Px" ToolTip="Please enter customer name." placeholder="Search Customer here to Edit/Delete a Customer"
                                    ClientIDMode="Static"></asp:TextBox>
                                <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCustomerSearch"
                                    ServicePath="~/AutoComplete.asmx" ServiceMethod="GetFullDetailofCustomer" MinimumPrefixLength="1"
                                    CompletionInterval="10" CompletionSetCount="15" FirstRowSelected="True" CompletionListCssClass="AutoExtender"
                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td align="left" width="50px">
                                &nbsp;
                            </td>
                            <td align="left" style="margin-left: 40px">
                                &nbsp;
                            </td>
                            <td align="left" style="margin-left: 40px">
                                &nbsp;
                            </td>
                            <td align="left" style="margin-left: 40px">
                                &nbsp;
                            </td>
                            <td align="left">
                                &nbsp;
                            </td>
                        </tr>                        
                    </table>
                    <table class="TableData">
                        <tr>
                            <td class="TDCaption">
                                Customer Code :
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCustomerCode" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td align="left" width="50px">
                            </td>
                            <td align="left" style="margin-left: 40px">
                            </td>
                            <td align="left" style="margin-left: 40px">
                            </td>
                            <td align="left" style="margin-left: 40px">
                            </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Customer Name :
                            </td>
                            <td align="left" colspan="4" nowrap="nowrap">
                                <asp:DropDownList ID="drpCustSalutation" runat="server" ClientIDMode="Static">
                                    <asp:ListItem Selected="True" Value=" "></asp:ListItem>
                                    <asp:ListItem>Mr</asp:ListItem>
                                    <asp:ListItem>Mrs</asp:ListItem>
                                    <asp:ListItem>Ms</asp:ListItem>
                                    <asp:ListItem>Dr</asp:ListItem>
                                    <asp:ListItem>M/S</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="30" Width="350px" CssClass="Textbox"
                                    ClientIDMode="Static" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                    TargetControlID="txtCustomerName" InvalidChars="`~:;,-" FilterMode="InvalidChars">
                                </cc1:FilteredTextBoxExtender>
                                <span class="span">*</span>
                            </td>
                            <td align="left" nowrap="nowrap">
                            </td>
                            <td align="left" style="margin-left: 40px">
                            </td>
                        </tr>
                        <tr valign="top">
                            <td rowspan="5" nowrap="nowrap" class="TDCaption">
                                Customer Address :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top" rowspan="5" width="150px">
                                <asp:TextBox ID="txtCustomerAddress" runat="server" TextMode="MultiLine" Rows="4"
                                    MaxLength="100" Width="150px" Height="120px" CssClass="Textbox" ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    FilterMode="InvalidChars" TargetControlID="txtCustomerAddress" InvalidChars="`~:;,-">
                                </cc1:FilteredTextBoxExtender>
                                <span class="span">*</span>
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Phone 1
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top" colspan="2">
                                <asp:TextBox ID="txtCustomerMobile" runat="server" Width="125px" CssClass="Textbox"
                                    ClientIDMode="Static" />
                                <cc1:FilteredTextBoxExtender ID="txtCustomerMobile_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerMobile">
                                </cc1:FilteredTextBoxExtender>
                                <span style="font-size: smaller">&nbsp; (&nbsp; Will be printed on receipt.&nbsp; )</span>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Phone 2
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:TextBox ID="txtCustomerPhone" runat="server" Width="125px" CssClass="Textbox"
                                    ClientIDMode="Static" />
                                <cc1:FilteredTextBoxExtender ID="txtCustomerPhone_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCustomerPhone">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Customer Email-Id
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:TextBox ID="txtCustomerEmailId" runat="server" MaxLength="100" Width="125px"
                                    CssClass="Textbox" ClientIDMode="Static" />                                
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                    TargetControlID="txtCustomerEmailId" InvalidChars="`~:;,-" FilterMode="InvalidChars">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Birth Date
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:TextBox ID="txtBirthDay" runat="server" CssClass="Textbox" Width="125px" onkeydown="return false;"
                                    onpaste="return false;" ClientIDMode="Static"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtBirthDay_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtBirthDay" Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Anniversary Date
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:TextBox ID="txtAnniversaryDate" runat="server" CssClass="Textbox" Width="125px"
                                    onkeydown="return false;" onpaste="return false;" ClientIDMode="Static"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtAnniversaryDate_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtAnniversaryDate" Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Prefrence :
                            </td>
                            <td align="left" colspan="6" nowrap="nowrap">
                                <table class="TableData">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="drpPriority" runat="server" Width="130px" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <input id="btnShowNewPriority" onclick="javascript: var val=document.getElementById('btnShowNewPriority').value;if(val=='+'){document.getElementById('divNewPriority').style.visibility='Visible';document.getElementById('<%= txtNewPriority.ClientID %>').focus(); document.getElementById('btnShowNewPriority').value='-';} else {document.getElementById('divNewPriority').style.visibility='Hidden';document.getElementById('btnShowNewPriority').value='+';document.getElementById('<%= drpPriority.ClientID %>').focus();}"
                                                size="2" type="button" value="+" style="background-color: #CCCCCC; font-weight: bold;
                                                font-family: Tahoma" />
                                        </td>
                                        <td>
                                            <div id="divNewPriority" style="visibility: hidden;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtNewPriority" MaxLength="50" size="37" type="text"
                                                                Style="" ClientIDMode="Static" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtNewPriority"
                                                                FilterMode="ValidChars" ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnAddNewPriority" ImageUrl="~/images/add.png" OnClientClick="return checkNewPriorityBox();"
                                                                runat="server" OnClick="btnAddNewPriority_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td rowspan="5" nowrap="nowrap" class="TDCaption">
                                Area Location :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top" rowspan="5" width="150px">
                                <asp:TextBox ID="txtAreaLocation" runat="server" TextMode="MultiLine" Rows="4" MaxLength="100"
                                    Width="150px" Height="120px" CssClass="Textbox" ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" Enabled="True"
                                    FilterMode="InvalidChars" TargetControlID="txtAreaLocation" InvalidChars="`~:;,-">
                                </cc1:FilteredTextBoxExtender>
                                <span class="span">*</span>
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Referred By
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top" colspan="2">
                                <asp:TextBox ID="txtCustomerRefferredBy" runat="server" MaxLength="50" CssClass="Textbox"
                                    ClientIDMode="Static" />
                                <cc1:FilteredTextBoxExtender ID="txtCustomerRefferredByExt" runat="server" Enabled="True"
                                    TargetControlID="txtCustomerRefferredBy" InvalidChars="`~:;,-" FilterMode="InvalidChars">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Communication Preference
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:DropDownList ID="drpCommunicationMeans" runat="server" ClientIDMode="Static">
                                    <asp:ListItem Text="None" Value="NA" />
                                    <asp:ListItem Text="Only SMS" Value="SMS" />
                                    <asp:ListItem Text="Only Email" Value="Email" />
                                    <asp:ListItem Text="Both SMS and Email" Value="Both" />
                                </asp:DropDownList>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Default Discount (%)
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:TextBox ID="drpDefaultDiscountRate" runat="server" Width="50px" MaxLength="5"
                                    ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="drpDefaultDiscountRate_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="drpDefaultDiscountRate" ValidChars="1234567890.">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                Barcode
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                               <asp:TextBox ID="txtBarcode" runat="server" ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                    TargetControlID="txtBarcode" InvalidChars="`~:;,-" FilterMode="InvalidChars">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                                MemberShip Id
                            </td>
                            <td nowrap="nowrap" valign="top" class="TDDot">
                                :
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                <asp:TextBox ID="txtMemberShip" runat="server" ClientIDMode="Static"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtMemberShip"
                                    FilterMode="InvalidChars" InvalidChars="`~:;,-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                            <td align="left" nowrap="nowrap" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Remarks :
                            </td>
                            <td align="left" colspan="4">
                                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" Width="480px" CssClass="Textbox"
                                    ClientIDMode="Static" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtRemarks"
                                    FilterMode="InvalidChars" InvalidChars="`~:;,-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="left">
                            </td>
                            <td align="left" style="margin-left: 40px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr valign="top">
                        <td style="width:85Px">
                        </td>
                            <td nowrap="nowrap" valign="top" class="TDCaption">
                              Rate List :
                            </td>
                            <td>
                             <asp:DropDownList ID="ddlRateList" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>                                
                            </td>
                             <td style="width:50Px">
                        </td>
                            <td class="TDCaption">
                                Register For Website :
                            </td>
                            <td>
                                <asp:CheckBox ID="chkWebSite" runat="server" ClientIDMode="Static" />
                                <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset Password"
                                    Enabled="false" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table style="width: 400px;">
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" ClientIDMode="Static" />
                            </td>
                            <td>
                                <asp:Button ID="btnEdit" runat="server" Text="Update" ClientIDMode="Static" Style="display: none" />
                            </td>
                            <td>
                                <asp:Button ID="btnShowAll" runat="server" Text="Show All Customers" 
                                    ClientIDMode="Static" onclick="btnShowAll_Click" />
                            </td>
                            <%-- <td>
                                <asp:Button ID="btnAddNew" runat="server" Text="Refresh"  />
                            </td>--%>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" ClientIDMode="Static" Style="display: none" />
                            </td>
                            <td>
                                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export"
                                    Visible="false" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <table width="100%" id="divCustDetail" runat="server">
            <tr>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF9933" Text="DrySoft "></asp:Label>
                    Customer Details
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 700px">
                        <asp:GridView ID="grdSearchResult" runat="server" AutoGenerateColumns="False" EmptyDataText="No record Found.">
                            <Columns>
                                <asp:BoundField DataField="CustomerCode" ReadOnly="true" SortExpression="CustomerCode"
                                    HeaderText="Code" />
                                <asp:BoundField DataField="CustomerName" ReadOnly="true" SortExpression="CustomerName"
                                    HeaderText="Name" />
                                <asp:BoundField DataField="CustomerAddress" ReadOnly="true" SortExpression="CustomerAddress"
                                    HeaderText="Address" />
                                <asp:BoundField DataField="CustomerPhone" ReadOnly="true" SortExpression="CustomerPhone"
                                    HeaderText="Phone" />
                                <asp:BoundField DataField="CustomerMobile" ReadOnly="true" SortExpression="CustomerMobile"
                                    HeaderText="Mobile" />
                                <asp:BoundField DataField="CustomerEmailId" ReadOnly="true" SortExpression="CustomerEmailId"
                                    HeaderText="Email Id" />
                                <asp:BoundField DataField="Priority" ReadOnly="true" SortExpression="Priority" HeaderText="Priority" />
                                <asp:BoundField DataField="CustomerRefferredBy" ReadOnly="true" SortExpression="CustomerRefferredBy"
                                    HeaderText="Refferred By" />
                                <asp:BoundField DataField="DefaultDiscountRate" ReadOnly="true" SortExpression="DefaultDiscountRate"
                                    HeaderText="Discount" />
                                <asp:BoundField DataField="IsWebsite" ReadOnly="true" SortExpression="IsWebsite"
                                    HeaderText="Website" />
                            </Columns>
                        </asp:GridView>
                    </div>                    
                </td>
            </tr>
        </table>

        <asp:HiddenField ID="hdnBarcode" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnMobileNo" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnMembershipId" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnCustomercode" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnCustWebsite" ClientIDMode="Static" runat="server" />
    </fieldset>
</asp:Content>
