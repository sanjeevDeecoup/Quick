<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerNewMaster.aspx.cs"
    Inherits="QuickWeb.Masters.CustomerNewMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Customer Master</title>
    <link href="../css/Customer.css" rel="stylesheet" type="text/css" />
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <script src="../JavaScript/CustomerMaster.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainContainer" align="center" style="border-style: outset">
        <div id="leftContainer" class="leftContainer">
            <div id="Button">
                <input type="button" id="btnSearchByCustomer" value="Search By Customer" runat="server"
                    class="button" onserverclick="btnSearchByCustomer_ServerClick" />
                <input type="button" id="btnSearchByInvoice" value="Search By Invoice" runat="server"
                    class="button" onserverclick="btnSearchByInvoice_ServerClick" />
                <input type="button" id="btnCreateNewCustomer" value="Create New Customer" runat="server"
                    class="button" onserverclick="btnCreateNewCustomer_ServerClick" onclick="return btnCreateNewCustomer_onclick()" /></div>
            <div id="Name">
                <input type="text" id="txtCustomerName" runat="server" class="text" />
                <input type="text" id="txtBookingNumber" runat="server" class="text" visible="false" />
                <input type="button" id="btnEnter" value="Enter" runat="server" class="button1" onserverclick="btnEnter_ServerClick" /></div>
            <div id="CustDetails">
                <label for="txtCustID" class="label">
                    Cust Id:
                </label>
                <input type="text" id="txtCustID" runat="server" class="text1" />
                <label for="txtName" class="label">
                    Name:</label>
                <input type="text" id="txtCustName" runat="server" class="text1" />
                <label for="txtPhone" class="label">
                    Phone:</label>
                <input type="text" id="txtPhone" runat="server" class="text1" />
                <label for="txtAddress" class="label">
                    Address:</label>
                <input type="text" id="txtAddress" runat="server" class="text1" style="width: 86%" />
                <label for="txtEmail" class="label">
                    EMail:</label>
                <input type="text" id="txtEmail" runat="server" class="text1" />
                <label for="txtBirDate" class="label">
                    B.Date:</label>
                <input type="text" id="txtBirDate" runat="server" class="text1" />
                <label for="txtAnniversery" class="label">
                    Anniv.Date:</label>
                <input type="text" id="txtAnniversery" runat="server" class="text1" style="width: 21%" />
                <label for="txtRefBy" class="label">
                    Ref.By:</label>
                <input type="text" id="txtRefBy" runat="server" class="text1" style="width: 37%" />
                <label for="txtDiscount" class="label">
                    Discount:</label>
                <input type="text" id="txtDiscount" runat="server" class="text1" style="width: 38%" />
                <label for="txtPriority" class="label">
                    Priority:</label>
                <input type="text" id="txtPriority" runat="server" class="text1" style="width: 87%" />
                <label for="txtRemarks" class="label">
                    Remarks:</label>
                <input type="text" id="txtRemarks" runat="server" class="text1" style="width: 87%" />
            </div>
            <div id="Button" class="container4">
                <div id="ButtonDetails1">
                    <input id="btnBooking" type="button" runat="server" value="Booking" class="button2" />
                    <input id="btnDelivery" type="button" runat="server" value="Delivery" class="button2" />
                </div>
                <div id="Button2">
                    <input id="btnBHistory" type="button" runat="server" value="Booking History" class="button2" />
                    <input id="btnPHistory" type="button" runat="server" value="Payment History" class="button2" />
                </div>
                <div id="ButtonDetails2">
                    <input id="btnDHistory" type="button" runat="server" value="Delivery History" class="button2" />
                    <input id="btnAccountStaement" type="button" runat="server" value="Account Statement"
                        class="button2" />
                </div>
            </div>
        </div>
        <div id="rightContainer" class="rightContainer">
            <div id="PendingDetails" align="center" style="text-align: center">
                <label id="lblheader" class="label1Right" style="border-style: solid; border-width: thin">
                    Pending Tickets
                </label>
            </div>
            <div id="blank">
            </div>
            <div id="bookingTable">
                <table cellpadding="0" cellspacing="0" border="1" class="container3">
                    <thead>
                        <tr>
                            <th>
                                Invoice No
                            </th>
                            <th>
                                Due Date
                            </th>
                            <th>
                                Del Date
                            </th>
                            <th>
                                Amount
                            </th>
                            <th>
                                Qty
                            </th>
                            <th>
                                Status
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div id="MainLabels" class="container41">
                <div id="Label1" align="center">
                    <label class="label4">
                        Rating</label>
                    <label class="label4">
                        Revenue Till Date</label></div>
                <div id="label2" align="center">
                    <label class="label4">
                        First Visit</label>
                    <label class="label4">
                        Last Visit</label></div>
                <div id="label3">
                    <label class="label5">
                        Advance / Balance Due</label></div>
            </div>
            <asp:HiddenField ID="hdnId" runat="server" />
            <asp:HiddenField ID="hdnStatus" runat="server" />
            <asp:HiddenField ID="hdnName" runat="server" />
        </div>
    </div>
    </form>
</body>