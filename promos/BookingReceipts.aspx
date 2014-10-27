<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingReceipts.aspx.cs" Inherits="promos.BookingReceipts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" href="css/bookingreceipts.css" />
    <script type="text/javascript" src="js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript" src="js/bookingReceipts.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainContainer">
        <div id="leftContainer">
            <div id="bookingType">
                <span>Booking Type:</span>
                <input type="radio" id="rdoWalkin" name="bookingtype" />
                <label for="rdoWalkin">Walk-in</label>
                <input type="radio" id="rdoHome" name="bookingtype" />
                <label for="rdoHome">Home</label>
                <input type="radio" id="rdoHomeInvoice" name="bookingtype" />
                <label for="rdoHomeInvoice">Home Invoice</label>
            </div>
            <div id="rowCommands">
                <div id="delete"><input type="button" id="btnDelete" value="Delete" /></div>
                <div id="new"><input type="button" id="btnNew" value="New" /></div>
            </div>
            <div id="bookingTable">
                <table cellpadding="0" cellspacing="0" border="1">
                    <thead>
                        <tr>
                            <th id="col1">S. No.</th>
                            <th id="col2">Qty</th>
                            <th id="col3">Description</th>
                            <th id="col4">Process @ Rate</th>
                            <th id="col5">Amount</th>
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

            <div id="dueDate">
                <div id="dueDateLabels">
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
                </div>
            </div>
                        </div>
            <div id="remarks">
                <div id="remarksLabel"><label for="txtRemarks">Remarks</label></div>
                <div id="remarksControl"><input type="text" id="txtRemarks" /></div>
            </div>
            <div id="leftCommands">
                <div id="submit">
                    <input type="image" id="btnSubmit" src="images/other/Save.png"  onclick="javascript:return false;" />
                </div>
                <div id="submitAndPrint">
                    <input type="image" id="btnSubmitPrint" src="images/other/Print.png" onclick="javascript:return false;" />
                </div>
            </div>
        </div>
        <div id="rightContainer">
            <div id="itemsPanel">
                <div id="itemsPanelHeader"><span>Items</span></div>
                <div id="itemCategories">
                    <div><span>Men</span></div>
                    <div><span>Women</span></div>
                    <div><span>Kids</span></div>
                    <div><span>Household</span></div>
                    <div><span>Others</span></div>
                </div>
                <div id="catItems"></div>
            </div>
            <div id="itemDesc1">
                <label for="txtQuantity">Quantity</label>
                <input type="text" id="txtQuantity"  maxlength="5" rel="numPad" showNumPad="bottom" createRow="true" />
                <label for="txtLength">Length</label><input type="text" id="txtLength" />
                <label for="txtBreadth">Breadth</label><input type="text" id="txtBreadth" />
                <label for="txtArea">Area</label><input type="text" id="txtArea" />
            </div>
            <div id="itemDesc2">
                <label for="txtItemRemarks">Remarks&nbsp;</label><input type="text" id="txtItemRemarks" />
                <label for="txtBrand">Brand</label><input type="text" id="txtBrand" />
            </div>
            <div id="colorsPanel">
                <div id="colorsPanelHeader"><span>Colors and Patterns</span></div>
                <div id="patterns">
                </div>
                <hr />
                <div id="colors">
                </div>
            </div>
            <div id="processContainer">
                <div id="leftNav"><input type="button" value="&lt;" id="btnPrevious" /></div>
                <div id="processPanel">
                    <div id="processPanelHeader">
                        <span>Process 1 of 1</span>
                        <div id="processCommands">
                            <div id="processAdd"><a href="javascript:AddProcess();">Add</a></div>
                            <div id="processUpdate"><a href="javascript:UpdateProcess();">Update</a></div>
                            <div id="processDelete"><a href="javascript:DeleteProcess();">Delete</a></div>
                        </div>
                    </div>
                    <div id="processes">
                    </div>
                    <div id="processDesc">
                        <label for="txtProcessRate">Rate</label>
                        <input type="text" id="txtProcessRate" maxlength="5" rel="numPad" showNumPad="top" createRow="true" />
                        <label for="txtProcessAmount">Amount</label>
                        <input type="text" id="txtProcessAmount" maxlength="5" rel="numPad" showNumPad="top" createRow="true" />
                    </div>
                </div>
                <div id="rightNav"><input type="button" value="&gt;" id="btnNext" /></div>
<%--                <div id="rightCommands">
                    <div id="new"><input type="button" id="btnNew" value="New" /></div>
                    <div id="save"><input type="button" id="btnUpdate" value="Update" /></div>
                    <div id="delete"><input type="button" id="btnDelete" value="Delete" /></div>
                    </div>--%>
            </div>
        </div>
        <div id="tooltipDiv">
        </div>
        <div id="numPad">
            <div id="closeNumPad"><a href="javascript:CloseNumPad('a');">Close</a></div>
        </div>
        <div id="lineItemXmls">
            <div id="lineItemTemplate"></div>
            <div id="lineItems"></div>
            <div id="test"></div>
        </div>
        <div id="processData" current="1">
            <div rel="processData" dbid="" rate="" amount="" sequence="1"></div>
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
