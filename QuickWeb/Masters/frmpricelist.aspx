<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmpricelist.aspx.cs" Inherits="QuickWeb.Masters.frmpricelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Price List</title>
    <link href="../css/pricelist.css" rel="stylesheet" type="text/css" />
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
          function isNumberKey(evt) {
              var charCode = (evt.which) ? evt.which : event.keyCode

              if (charCode > 31 && (charCode < 45 || charCode > 57)) { return false; }
              return true;

          }
          function isNumberKey1(evt) {
              var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) { return false; }
            return true;
          }

    function InitXmlHttp() {
        // Attempt to initialize xmlhttp object
        try {
            xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            // Try to use different activex object
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (E) {
                xmlhttp = false;
            }
        }

        // If not initialized, create XMLHttpRequest object
        if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
            xmlhttp = new XMLHttpRequest();
        }
        // Define function call for when Request obj state has changed
        xmlhttp.onreadystatechange = XMLHttpRequestCompleted;
    }

    function InvokeASHX() {
        try {

            InitXmlHttp();
            var Gr = document.getElementById('<%= GrdDynamic.ClientID %>');
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainContainer">
        <div id="Div1" style="height: 35px;">
            <div class="txt">
                -<br />
                &nbsp; Item Price List Details</div>
        </div>
        <div id="itemsGrid">
            <asp:GridView ID="GrdDynamic" runat="server" AutoGenerateColumns="False" CssClass="mGrid">
                <Columns>
                </Columns>
            </asp:GridView>
            &nbsp;</div>
        <div id="griddiv">
        </div>
        <div id="rightCommands">
            <input type="button" id="btnSave" value="Save" runat="server" onclick="InvokeASHX();"
                onserverclick="btnSave_ServerClick" />
            <input type="button" id="btnNew" value="Import" />
            <input type="button" id="btnDelete" value="Export" runat="server" />
        </div>
        <div id="lbldiv">
            <label id="lblError" runat="server" class="ErrorMessage">
            </label>
        </div>
    </div>
    </form>
</body>
</html>