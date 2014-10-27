<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="promos.aspx.cs" Inherits="promos.promos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title></title>
    <script type="text/javascript" src="js/jquery-1.6.2.min.js"></script>
    <script src="js/promoAjaxCalls.js" type="text/javascript"></script>
    <script src="js/promoWIzard.js" type="text/javascript"></script>
    <link href="css/promoWizard.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="templateContainer">
        <fieldset name="f0" sequence="0" validation="1">
            <div class="formLegend">
                <span>Welcome to the Promotional Screen Wizard</span></div>
            <div class="formError">Please select an option</div>
            <div class="formHelpText">
                <p>
                    Welcome to the Promotional Scheme Wizard. This wizard is going to help you in creating
                    promotional schemes.</p>
                <p>
                    <b>Examples 1:</b> Get 1 shirt drycleaned and get another free on the same bill</p>
                <p>
                    <b>Examples 2:</b> Pay Rs. 10000 advance and get services worth Rs. 15000</p>
                <p>
                    <b>Examples 3:</b> Get 20% discount on bill receipt</p>
            </div>
            <div class="formContent" nextFrom="div.largeButton" nextFromAttr="next" selectedValue="1" selectedValueAttr="value">
                <div rel="promoIntro">
                    <div class="largeButton" next="f1" value="0">
                        Create New Promotional Scheme</div>
                    <div class="largeButton" next="f7" value="0">
                        Modify Existing Scheme</div>
                </div>
            </div>
        </fieldset>
        <fieldset name="f1" sequence="0" validation="1">
            <div class="formLegend">
                <span>Type of Promotional Scheme</span></div>
            <div class="formError">Please select atleast 1 scheme to proceed</div>
            <div class="formHelpText">
                <p>
                    Please select type of promotional scheme that you want to create hello this is 123</p>
            </div>
            <div class="formContent" nextFrom="div.smallButton" nextFromAttr="next" selectedValue="1" selectedValueAttr="value">
                <div rel="schemeTemplates" loaded="0">
                </div>
            </div>
        </fieldset>
        <fieldset name="f2" sequence="0" validation="1">
            <div class="formLegend">
                <span>Select Quantity</span></div>
            <div class="formError"></div>
            <div class="formHelpText">
                <p>Please enter the quantity</p>
            </div>
            <div class="formContent">
                <div rel="quantity">
                <div rel="quantityDesc">
                    <div name="quantityText">
                        <label for="txtClothQty">Enter Quantity</label>
                        <input type="text" maxlength="5" name="txtClothQty" />
                    </div>
                    <div name="quantityUnit">
                        <input type="radio" name="unit" value="1" />
                        <label>Rs.</label>
                        <input type="radio" name="unit" value="2" />
                        <label>%</label>
                        <input type="radio" name="unit" value="3" />
                        <label>unit</label>
                    </div>
                </div>
                    <div rel="numPad" loaded="0">
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset name="f3" sequence="0" validation="1">
            <div class="formLegend">
                <span>Select Process</span></div>
            <div class="formError"></div>
            <div class="formHelpText">
                <p>
                    Please select the process</p>
            </div>
            <div class="formContent">
                <div rel="allProcesses">
                    <div rel="selectedProcess">
                        <label for="f3txtSelectedProcess">
                            You selected:
                        </label>
                        <input type="text" name="txtSelectedProcess" />
                        <input type="checkbox" name="chkSelectAllProcess" />
                        <label for="f3chkSelectAllProcess">
                            Select All</label>
                    </div>
                    <div rel="process" loaded="0">
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset name="f4" sequence="0" validation="1">
            <div class="formLegend">
                <span>Select Clothes</span></div>
            <div class="formError"></div>
            <div class="formHelpText">
                <p>
                    Please select the clothes</p>
            </div>
            <div class="formContent">
                <div rel="items">
                    <div rel="allItems">
                        <div rel="filterItems1">
                            <label for="f4txtAllItems">
                                Filter Items</label>
                            <input type="text" name="txtAllItems" />
                            <input type="image" name="imgFilterItems" />
                        </div>
                        <div rel="allItemsMain" loaded="0"></div>
                    </div>
                    <div rel="moveItems">
                        <input type="button" name="btnSelectAll" value="&gt;&gt;" />
                        <input type="button" name="btnSelect" value="&gt;" />
                        <input type="button" name="btnUnselect" value="&lt;" />
                        <input type="button" name="btnUnselectAll" value="&lt;&lt;" />
                    </div>
                    <div rel="selectedItems">
                        <div rel="filterItems2">
                            <label>Selected Items</label>
                        </div>
                        <div rel="selectedItemsMain"></div>
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset name="f5" sequence="0" validation="1">
            <div class="formLegend">
                <span>Scheme Name</span></div>
            <div class="formError"></div>
            <div class="formHelpText">
                <p>
                    Please enter the Scheme Name</p>
            </div>
            <div class="formContent">
                <div rel="schemeCaption" loaded="0">
                    <div rel="schemeName">
                        <p>
                            <label for="f5txtSchemeName">
                                Promotional Scheme Name</label></p>
                        <p>
                            <input type="text" name="txtSchemeName" value="" /></p>
                    </div>
                    <div rel="schemeDesc">
                        <p>
                            <label for="f5txtSchemeName">
                                Promotional Scheme Description</label></p>
                        <p>
                            <textarea name="txtSchemeDesc" rows="5" cols="60"></textarea></p>
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset name="f6" sequence="0" validation="1">
            <div class="formLegend">
                <span>Select Validity</span></div>
            <div class="formError"></div>
            <div class="formHelpText">
                <p>
                    Please select the validity</p>
            </div>
            <div class="formContent">
                <div rel="schemeValidity" loaded="0">
                    <ul>
                        <li>
                            <input type="radio" name="validity" value="1" />
                            <label for="f6rdoNTenure">
                                <span>Valid for
                                    <input type="text" name="txtApplicableFor" maxlength="1" value="1" class="shortText" />
                                    <select name="selectApplicableForTenure">
                                        <option value="1" selected="selected">Month</option>
                                        <option value="2">Year</option>
                                    </select>
                                    from date of enrollment </span>
                            </label>
                        </li>
                        <li>
                            <input type="radio" name="validity" value="2" />
                            <label for="rdoDates">
                                <span>Valid from
                                    <input type="text" name="txtDateFrom" class="longText" />
                                    to
                                    <input type="text" name="txtDateTo" class="longText" />
                                </span>
                            </label>
                        </li>
                    </ul>
                </div>
            </div>
        </fieldset>
        <fieldset name="f7" sequence="0" validation="1">
            <div class="formLegend">
                <span>Edit Promotional Schemes</span></div>
            <div class="formError">Please select atleast 1 scheme from the list to edit</div>
            <div class="formHelpText">
                <p>
                    Please select type of promotional scheme that you want to edit</p>
            </div>
            <div class="formContent" nextFrom="div.selColDiv"  nextFromAttr="next" selectedValue="1" selectedValueAttr="value">
                <div rel="promos" loaded="0">
                    <%--<table cellpadding="0" cellspacing="0">
                        <tr><td name="selCol">Select</td><td name="nameCol">Name</td><td name ="descCol">Description</td></tr>
                        <tr><td name="selCol"><div name="selDiv" value="0" next="f2">1</div></td><td name="nameCol">Monthly#1</td><td name ="descCol">Get 10 Drycleanings free on monthly pay of Rs. 9000. This is valid for 1 month from enrollment</td></tr>
                        <tr><td name="selCol"><div name="selDiv" value="0" next="f2"></div></td><td name="nameCol">Monthly#1</td><td name ="descCol">Get 30 Dye free on monthly pay of Rs. 5000. This is valid for 1 month from enrollment</td></tr>
                        <tr><td name="selCol"><div name="selDiv" value="0" next="f2">3</div></td><td name="nameCol">Advacne based Scheme</td><td name ="descCol">Pay Rs. 10000 and get Rs. 15000 of DryCleaning free. This is valid between 02-Feb-2012 and 01-Feb-2012. . This is valid between 02-Feb-2012 and 01-Feb-2012.</td></tr>
                    </table>--%>
                </div>
            </div>
        </fieldset>
    </div>

    <div id="mainContainer"></div>
    <div id="commands"></div>
    <div id="test"></div>

    </form>
</body>
</html>


