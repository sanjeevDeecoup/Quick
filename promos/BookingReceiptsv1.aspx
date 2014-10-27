<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingReceiptsv1.aspx.cs" Inherits="promos.BookingReceiptsv1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" href="css/bookingreceiptsv1.css" />
    <script type="text/javascript" src="js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript" src="js/bookingReceiptsv1.js"></script>
    <link href="css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.datepick.js" type="text/javascript"></script>
    <title>Book Receipts</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainContainer">
        <div id="leftContainer">
            <div id="bookingType">
                <div id="receiptNo">
                    Booking #<span>001</span>
                </div>
                <div id="walkIn" class="button" radioGroup="bookingType">
                    Walkin
                </div>
                <div id="home" class="button" radioGroup="bookingType">
                    Home
                </div>
                <div id="homeDeliveryReceipt">
                    Home Recipt #
                </div>
                <%--<span>Booking Type:</span>
                <input type="radio" id="rdoWalkin" name="bookingtype" />
                <label for="rdoWalkin">Walk-in</label>
                <input type="radio" id="rdoHome" name="bookingtype" />
                <label for="rdoHome">Home</label>
                <input type="radio" id="rdoHomeInvoice" name="bookingtype" />
                <label for="rdoHomeInvoice">Home Invoice</label>--%>
            </div>
            <div id="customerData">
                <div id="customerDetails">
                    <span>Customer Name:</span>
                    <input type="text" value="Jeevan Lal" />
                </div>
                <div id="customerPriorities">
                    <span>Priorities:</span>
                    <input type="text" value="" />
                </div>
                <div id="customerRemarks">
                    <span>Remarks:</span>
                    <input type="text" value="" />
                </div>

            </div>
<%--            <div id="rowCommands">
                <div id="delete"><input type="button" id="btnDelete" value="Delete" /></div>
                <div id="new"><input type="button" id="btnNew" value="New" /></div>
            </div>--%>
            <div id="bookingTable">
                <table cellpadding="0" cellspacing="0" border="1">
                    <thead>
                        <tr>
                            <th class="col1">#</th>
                            <th class="col2">Qty</th>
                            <th class="col3">Description</th>
                            <th class="col4">Process</th>
                            <th class="col5">Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div id="totals">
            <div id="receiptTotals">
                <div id="totalLabels">
                    <div><label for="txtGross">Gross:</label></div>
                    <div><label for="txtDiscount">Discount:</label></div>
                    <div><label for="txtTax">Tax:</label></div>
                    <div><label for="txtBalance">Balance:</label></div>
                    <div><label for="txtAdvance">Advance:</label></div>
                    <div><label for="txtNetTotal">Net Total:</label></div>
                </div>
                <div id="totalAmount">
                    <input type="text" id="txtGross" value="" />
                    <input type="text" id="txtDiscount" value="" maxlength="5" rel="numPad" showNumPad="right" />
                    <input type="text" id="txtTax" value="" />
                    <input type="text" id="txtBalance" value="" />
                    <input type="text" id="txtAdvance" value="" maxlength="5"  rel="numPad" showNumPad="right" />
                    <input type="text" id="txtNetTotal" value="" />
                </div>
            </div>
            <div id="deliveryDetails">
                    <div id="dueDetails">
                        <div id="dueDate">
                            <label for="txtDueDate">Due Date:</label>
                            <input type="text" id="txtDueDate" value="" />
                        </div>
                        <div id="dueTime">
                            <label for="txtDueTime">Due Time:</label>
                            <input type="text" id="txtDueTime" value="" />
                        </div>
                    </div>
                    <div id="dueButtons">
                        <div id="urgent" class="button">Urgent</div>
                        <div id="sms" class="button">SMS</div>
                        <div id="email" class="button">Email</div>
                    </div>
                    <div id="remarks">
                        <div id="remarksLabel"><label for="txtRemarks">Remarks</label></div>
                        <div id="remarksControl"><input type="text" id="Text1" /></div>
                    </div>
                    <div id="bookingPersonDetails">
                        <div id="bokkingSalesman">
                            <label for="drpSalesman">Salesman</label>
                            <select id="drpSalesman">
                                <option selected="selected">Rajbir</option>
                            </select>
                        </div>
                        <div id="checkedBy">
                            <label for="drpCheckedBy">Checked By</label>
                            <select id="drpCheckedBy">
                                <option selected="selected">Rajbir</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
                <%--<div id="dueDateLabels">
                    <div><label for="txtDueDate">Due Date:</label></div>
                    <div><label for="chkUrgent">Urgent:</label></div>
                    <div><label>Send SMS:</label></div>
                    <div><label>Send Email:</label></div>
                </div>
                <div id="dueDateControls">
                    <div><input type="text" id="txtDueDate" value="" /></div>
                    <div><input type="checkbox" id="chkUrgent" /></div>
                    <div><input type="button" id="btnSendSMS" value="Send SMS" /></div>
                    <div><input type="button" id="btnSendEmail" value="Send Email" /></div>
                </div>--%>
                
            
            <%--<div id="remarks">
                <div id="remarksLabel"><label for="txtRemarks">Remarks</label></div>
                <div id="remarksControl"><input type="text" id="txtRemarks" /></div>
            </div>--%>
            <div id="leftCommands">
                <div id="submit">
                    <input type="button" id="btnSubmit" value="Submit" />
                </div>
                <div id="submitAndPrint">
                    <input type="button" id="btnSubmitPrint" value="Submit &amp; Print" />
                </div>
            </div>
        </div>
        <div id="rightContainer">
            <div id="itemsPanel">
                <%--<div id="itemsPanelHeader"><span>Items</span></div>--%>
                <div id="itemCategories">
                    <div><span>Men</span></div>
                    <div><span>Women</span></div>
                    <div><span>Kids</span></div>
                    <div><span>Household</span></div>
                    <div><span>Others</span></div>
                </div>
                <div id="catItems"></div>
                <div id="itemDesc">
                    <div id="qty">
                        <label for="txtQuantity">Qty</label>
                        <input type="text" id="txtQuantity"  maxlength="5" value="1" rel="numPad" showNumPad="bottom" createRow="true" />
                    </div>
                    <div id="brand">
                        <label for="txtBrand">Brand</label><input type="text" id="txtBrand" />
                    </div>
                    <div id="itemRemarks">
                        <label for="txtItemRemarks">Remarks</label><input type="text" id="txtItemRemarks" />
                    </div>
                </div>
            </div>
<%--            <div id="itemDesc1">
                <label for="txtQuantity">Quantity</label>
                <input type="text" id="txtQuantity"  maxlength="5" value="1" rel="numPad" showNumPad="bottom" createRow="true" />
                <label for="txtLength">Length</label><input type="text" id="txtLength" />
                <label for="txtBreadth">Breadth</label><input type="text" id="txtBreadth" />
                <label for="txtArea">Area</label><input type="text" id="txtArea" />
            </div>
            <div id="itemDesc2">
                <label for="txtItemRemarks">Remarks&nbsp;</label><input type="text" id="txtItemRemarks" />
                <label for="txtBrand">Brand</label><input type="text" id="txtBrand" />
            </div>--%>
            <div id="colorsPanel">
                <%--<div id="colorsPanelHeader"><span>Colors and Patterns</span></div>--%>
                <div id="colors">
                </div>
                <hr />
                <div id="patterns">
                </div>
            </div>
            <div id="processContainer">
                <%--<div id="leftNav"><input type="button" value="&lt;" id="btnPrevious" /></div>--%>
                <div id="processPanel">
                    <%--<div id="processPanelHeader">
                        <span>Process 1 of 1</span>
                        <div id="processCommands">
                            <div id="processAdd"><a href="javascript:AddProcess();">Add</a></div>
                            <div id="processUpdate"><a href="javascript:UpdateProcess();">Update</a></div>
                            <div id="processDelete"><a href="javascript:DeleteProcess();">Delete</a></div>
                        </div>
                    </div>--%>
                    <div id="processes">
                    </div>
                    <div id="processDesc">
                        <div id="processDescItems">
<%--                            <div rel="processDescItem">
                                <span name="code">DC @</span>
                                <input type="text" rel="numPad" showNumPad="top" createRow="true" />
                                <a href=javascript:CloseProcessDescItem();">x</a>
                            </div>--%>
                        </div>
                        <%--<label for="txtProcessRate">Rate</label>
                        <input type="text" id="txtProcessRate" maxlength="5" rel="numPad" showNumPad="top" createRow="true" />
                        <label for="txtProcessAmount">Amount</label>
                        <input type="text" id="txtProcessAmount" maxlength="5" rel="numPad" showNumPad="top" createRow="true" />--%>
                    </div>
                </div>
                <%--<div id="rightNav"><input type="button" value="&gt;" id="btnNext" /></div>--%>
            </div>
            <div id="rightCommands">
                <div id="new"><input type="button" id="btnNew" value="Add New Item" /></div>
                <div id="delete"><input type="button" id="btnDelete" value="Delete" /></div>
            </div>        
        </div>
        <div id="tooltipDiv">
        </div>
        <div id="numPad">
            <%--<div id="closeNumPad"><a href="javascript:CloseNumPad('a');">Close</a></div>--%>
        </div>
        <div id="lineItemXmls">
            <div id="lineItemTemplate"></div>
            <div id="lineItems"></div>
            <div id="test"></div>
        </div>
        <div id="processData" current="1">
            
        </div>
        <div id="feedbackHeader">
            <span>Feedback...</span>
            <div id="feedbackDetail">
                <label for="txtName">Name(*)</label><br />
                <input type="text" id="txtName" /><br /><br />
                <label for="txtFeedback">Feedback(*)</label><br />
                <textarea id="txtFeedback" rows="10" cols="80"></textarea><br /> <br />
                <input type="button" id="btnFeedback" value="Submit Feedback" />
                <input type="button" id="btnClose" value="Close" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
