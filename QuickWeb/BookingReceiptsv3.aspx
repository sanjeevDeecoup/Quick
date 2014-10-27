<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingReceiptsv3.aspx.cs"
    Inherits="promos.BookingReceiptsv3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" href="css/bookingreceiptsv3.css" />
    <script type="text/javascript" src="js/jquery-1.6.2.min.js"></script>
    <script src="js/common.js" type="text/javascript"></script>
    <script src="js/numpad.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/bookingReceiptsv3.js"></script>
    <link href="css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.datepick.js" type="text/javascript"></script>
    <title>Booking Receipts</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainContainer">
        <div id="leftContainer">
            <div id="bookingType">
                <div id="receiptNo">
                    Booking #<span>001</span>
                </div>
                <div id="walkIn" class="button" radiogroup="bookingType">
                    Walkin
                </div>
                <div id="home" class="button" radiogroup="bookingType">
                    Home
                </div>
                <div id="homeDeliveryReceipt">
                    Home Receipt #<span></span>
                </div>
            </div>
            <div id="customerData">
                <div id="customerDetails">
                    <div id="customerSearch" custid="">
                        <ul id="ulcol1">
                            <li>
                                <label for="txtSearchCustomer">
                                    Search Customer</label></li>
                            <li>
                                <input type="text" value="" id="txtSearchCustomer" sourcediv="#allCustomers" rel="autoComplete"
                                    autocomplete="off" oncustomselect="SelectCustomer" /></li>
                        </ul>
                        <ul id="ulcol2">
                            <li>
                                <div id="selectedCustomer">
                                    <div id="customerName">
                                        <div class="nowrap">
                                            <span>Name:</span> <span id="spnCustomerName" class="coloredText"></span>
                                        </div>
                                    </div>
                                    <div id="customerPhone">
                                        <div class="nowrap">
                                            <span>Phone:</span> <span id="spnCustomerPhone" class="coloredText"></span>
                                        </div>
                                    </div>
                                    <div id="customerAddress">
                                        <div class="nowrap">
                                            <span>Address:</span> <span id="spnCustomerAddress" class="coloredText"></span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
                <div id="customerPriorities" class="customerComments">
                    <ul>
                        <li><span>Priorities:</span></li>
                        <li>
                            <input id="txtCustomerPriority" type="text" value="" /></li>
                    </ul>
                </div>
                <div id="customerRemarks" class="customerComments">
                    <ul>
                        <li><span>Remarks:</span></li>
                        <li>
                            <input id="txtCustomerRemarks" type="text" value="" /></li>
                    </ul>
                </div>
                <div id="customerDiscount" class="hidden">
                </div>
                <div id="allCustomers">
                </div>
            </div>
            <div id="bookingTable">
                <table cellpadding="0" cellspacing="0" border="1">
                    <thead>
                        <tr>
                            <th class="col1">
                                #
                            </th>
                            <th class="col2">
                                Qty
                            </th>
                            <th class="col3">
                                Description
                            </th>
                            <th class="col4">
                                Process
                            </th>
                            <th class="col5">
                                Amount
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div id="totals">
                <div id="receiptTotals">
                    <div id="totalLabels">
                        <div>
                            <label for="txtGross">
                                Gross:</label></div>
                        <div>
                            <label for="txtDiscount">
                                Discount:</label></div>
                        <div>
                            <label for="txtTax">
                                Tax:</label></div>
                        <div>
                            <label for="txtBalance">
                                Balance:</label></div>
                        <div>
                            <label for="txtAdvance">
                                Advance:</label></div>
                        <div>
                            <label for="txtNetTotal">
                                Net Total:</label></div>
                    </div>
                    <div id="totalAmount">
                        <input type="text" id="txtGross" value="" readonly="readonly" />
                        <input type="text" id="txtDiscount" value="" readonly="readonly" rel="discountOptions" />
                        <input type="text" id="txtTax" value="" readonly="readonly" />
                        <input type="text" id="txtBalance" value="" />
                        <input type="text" id="txtAdvance" value="" maxlength="5" rel="numPad" shownumpad="top" />
                        <input type="text" id="txtNetTotal" value="" readonly="readonly" />
                    </div>
                </div>
                <div id="deliveryDetails">
                    <div id="dueDetails">
                        <div id="dueDate">
                            <label for="txtDueDate">
                                Due Date:</label>
                            <input type="text" id="txtDueDate" value="" readonly="readonly" />
                        </div>
                        <div id="dueTime">
                            <label for="txtDueTime">
                                Due Time:</label>
                            <input type="text" id="txtDueTime" value="" readonly="readonly" />
                        </div>
                    </div>
                    <div id="dueButtons">
                        <div id="urgent" class="button" radiogroup="urgent" rel="multipleOptions" optionsdiv="#urgentDescriptions"
                            autoselect="div[radioGroup=urgentOptions]">
                            Urgent</div>
                        <div id="sms" class="button" radiogroup="sms">
                            SMS</div>
                        <div id="email" class="button" radiogroup="email">
                            Email</div>
                    </div>
                    <div id="remarks">
                        <div id="remarksLabel">
                            <label for="txtRemarks">
                                Remarks</label></div>
                        <div id="remarksControl">
                            <input type="text" id="txtRemarks" /></div>
                    </div>
                    <div id="bookingPersonDetails">
                        <div id="bokkingSalesman">
                            <label for="drpSalesman">
                                Salesman</label>
                            <select id="drpSalesman">
                                <option selected="selected"></option>
                            </select>
                        </div>
                        <div id="checkedBy">
                            <label for="drpCheckedBy">
                                Checked By</label>
                            <select id="drpCheckedBy">
                                <option selected="selected"></option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="rightContainer">
            <div id="itemsPanel">
                <div id="catItems">
                </div>
            </div>
            <div id="miscPanel">
                <div id="subItemsContainer" class="container3">
                    <div id="subItemsHeader" class="header">
                        <div id="subItemsDetail" class="detail">
                            <span class="watermark">Sub Items</span>
                        </div>
                    </div>
                </div>
                <div id="variationsContainer" class="container4">
                    <div id="variationsHeader" class="header">
                        <div id="variationsDetail" class="detail">
                            <span class="watermark">Variations</span>
                        </div>
                        <div id="allVariations">
                        </div>
                    </div>
                </div>
                <div id="quantityContainer" class="container1">
                    <ul>
                        <li class="quantityLabel">Quantity</li>
                        <li id="quantityText" class="quantityText" maxlength="5" rel="numPad" shownumpad="right"
                            showdecimal="false">1</li>
                    </ul>
                </div>
                <div id="categoriesContainer" class="container2">
                    <div id="categoriesHeader" class="header">
                        <div id="categoriesDetail" class="detail">
                            <span class="watermark">Categories</span>
                        </div>
                        <div id="allCategories">
                        </div>
                    </div>
                </div>
                <div id="colorsContainer" class="container5" title="Click to select colors" flyoutdiv="colorsPanel">
                    <div id="colorsIcon" class="iconContainer">
                        Colors & Patterns
                    </div>
                    <div id="colorsPanel" rel="panel">
                        <div id="colors">
                        </div>
                        <hr />
                        <div id="patterns">
                        </div>
                    </div>
                </div>
                <div id="brandsContainer" class="container5" title="Brands" flyoutdiv="brandsPanel">
                    <div id="brandsIcon" class="iconContainer">
                        Brands</div>
                    <div id="brandsPanel" rel="panel">
                        <div id="brands">
                        </div>
                    </div>
                </div>
                <div id="commentsContainer" class="container5" title="Comments" flyoutdiv="commentsPanel">
                    <%--<div class="flyout"></div>--%>
                    <div id="commentsIcon" class="iconContainer">
                        Remarks</div>
                    <div id="commentsPanel" rel="panel">
                        <div id="comments">
                        </div>
                    </div>
                </div>
            </div>
            <div id="processContainer">
                <div id="processPanel">
                    <div class="noScrollLeft">
                    </div>
                    <div id="processes">
                    </div>
                    <div class="noScrollRight">
                    </div>
                    <div id="processDesc">
                        <div id="processDescItems">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="rightContainerKbd">
            <ul>
                <li>
                    <div class="label">
                        <label for="kbdQuantity">
                            Qty</label></div>
                </li>
                <li>
                    <input type="text" id="kbdQuantity" class="smallText" /></li>
            </ul>
            <ul>
                <li>
                    <div class="label">
                        <label for="kbdItem">
                            Item</label></div>
                </li>
                <li>
                    <input type="text" id="kbdItem" class="largeText" sourcediv="#catItems" rel="autoComplete" /></li>
            </ul>
            <ul class="newLine">
                <li>
                    <div class="label">
                        <label for="kbdCategory">
                            Category</label></div>
                </li>
                <li>
                    <input type="text" id="kbdCategory" sourcediv="#categoriesDetail" rel="autoComplete" /></li>
            </ul>
            <ul>
                <li>
                    <div class="label">
                        <label for="kbdVariations">
                            Variations</label></div>
                </li>
                <li>
                    <input type="text" id="kbdVariations" /></li>
            </ul>
            <ul class="newLine">
                <li>
                    <div class="label">
                        <label for="kbdSubItems">
                            Sub-Items</label></div>
                </li>
                <li>
                    <input type="text" id="kbdSubItems" class="largeText" /></li>
            </ul>
            <ul class="newLine">
                <li>
                    <div class="label">
                        <label for="kbdColors">
                            Colors</label></div>
                </li>
                <li>
                    <input type="text" id="kbdColors" class="largeText" /></li>
            </ul>
            <ul class="newLine">
                <li>
                    <div class="label">
                        <label for="kbdBrands">
                            Brands</label></div>
                </li>
                <li>
                    <input type="text" id="kbdBrands" class="largeText" /></li>
            </ul>
            <ul class="newLine">
                <li>
                    <div class="label">
                        <label for="kbdRemark">
                            Remark</label></div>
                </li>
                <li>
                    <input type="text" id="kbdRemark" class="largeText" /></li>
            </ul>
            <ul class="newLine">
                <li>
                    <div class="label">
                        <label for="kbdProcesses">
                            Processes</label></div>
                </li>
                <li>
                    <input type="text" id="kbdProcesses" class="largeText" /></li>
            </ul>
        </div>
        <div id="list">
        </div>
        <div id="urgentDescriptions">
        </div>
        <div id="discountOptions">
            <table id="discountTable" cellpadding="0" cellspacing="5px">
                <tr>
                    <td colspan="4">
                        <a href="javascript:HideDiscountOptions();">X</a>
                    </td>
                </tr>
                <tr>
                    <td class="discountCol1">
                        <input type="checkbox" id="chkPercent" />
                    </td>
                    <td class="discountCol2">
                        <span>Discount Percent</span>
                    </td>
                    <td class="discountCol3">
                        <input type="text" id="txtPercentDiscount" maxlength="2" rel="numPad" shownumpad="top" />
                    </td>
                    <td class="discountCol4">
                        <span rel="totalPercentDiscount"></span>
                    </td>
                </tr>
                <tr>
                    <td class="discountCol1">
                        <input type="checkbox" id="chkFlat" />
                    </td>
                    <td class="discountCol2">
                        <span>Flat Discount</span>
                    </td>
                    <td class="discountCol3">
                        <input type="text" id="txtFlatDiscount" maxlength="5" rel="numPad" shownumpad="top" />
                    </td>
                    <td class="discountCol4">
                        <span rel="totalFlatDiscount"></span>
                    </td>
                </tr>
            </table>
            <%--<div id="applicableDiscount" class="hidden"></div>
            <div class="button selectedButton" value="1" radioGroup="discountOption" id="discountOption1">
                <span rel="label">#1-Discount Percent</span>
                <input type="text" id="txtPercentDiscount" maxlength="2" rel="numPad" showNumPad="top" />
                <span rel="totalPercentDiscount"></span>
            </div>
            <div class="button" radioGroup="discountOption" id="discountOption2">
                <span rel="label">#2-Discount Amount</span>
                <input type="text" id="txtFlatDiscount" maxlength="5" rel="numPad" showNumPad="top" />
                <span rel="totalFlatDiscount"></span>
            </div>--%>
        </div>
        <div id="commands">
            <div id="rightCommands">
                <div id="new">
                    <input type="button" id="btnNew" value="Add New Item" /></div>
                <div id="delete">
                    <input type="button" id="btnDelete" value="Delete" /></div>
            </div>
            <div id="leftCommands">
                <div id="submit">
                    <input type="button" id="btnSubmit" value="Submit" />
                </div>
                <div id="submitAndPrint">
                    <input type="button" id="btnSubmitPrint" value="Submit &amp; Print" />
                </div>
            </div>
        </div>
        <div id="inputDeviceContainer">
            <div id="mouse" class="inputDevice">
            </div>
            <div class="inputDeviceShortcut">
                (ALT + K)</div>
            <div id="keyboard" class="inputDevice">
            </div>
            <div class="inputDeviceShortcut">
                (ALT + M)</div>
        </div>
        <div id="tooltipDiv">
        </div>
        <div id="numPad">
        </div>
        <div id="mask" class="semiTransparent">
        </div>
        <div id="dialog">
        </div>
        <div id="allErrors" class="hidden">
            <div id="errorContainer">
                <div id="lineItemErrors" class="errorDiv">
                </div>
                <div id="headerErrors" class="errorDiv">
                </div>
            </div>
        </div>
        <div id="editBookingXml" class="hidden">
        </div>
        <div id="errorTooltip">
        </div>
        <div id="priceList" class="hidden">
        </div>
        <div id="lineItemXmls">
            <div id="lineItemTemplate">
            </div>
            <div id="lineItems">
            </div>
            <div id="test">
            </div>
        </div>
        <div id="receiptHeaderTemplate">
        </div>
        <div id="processData" current="1">
        </div>
        <div class="hidden" id="defaults">
        </div>
        <div id="feedbackHeader">
            <span>Feedback...</span>
            <div id="feedbackDetail">
                <label for="txtName">
                    Name(*)</label><br />
                <input type="text" id="txtName" /><br />
                <br />
                <label for="txtFeedback">
                    Feedback(*)</label><br />
                <textarea id="txtFeedback" rows="10" cols="80"></textarea><br />
                <br />
                <input type="button" id="btnFeedback" value="Submit Feedback" />
                <input type="button" id="btnClose" value="Close" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>