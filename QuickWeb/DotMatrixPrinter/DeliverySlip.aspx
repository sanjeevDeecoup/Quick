<%@ Page Language="C#" AutoEventWireup="true" Inherits="Dot_DeliverySlip" CodeBehind="DeliverySlip.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <meta http-equiv="Expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="Pragma" content="no-cache" />
    <title>
        <%=AppTitle %></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="Javascript">
        function SetPrintOption() {
            document.getElementById("divButtons").style.visibility = "hidden";
            window.print();
            //document.getElementById("divButtons").style.visibility="visible";
        }
        function SetPrintOption() {
            var win4Print = window.open('', 'Win4Print');
            var msg = document.getElementById("divPrint").innerHTML;
            //	msg.MarginLeft = 0;
            //    msg.MarginTop = 0;
            //    msg.MarginRight = 0;
            //    msg.MarginBottom = 0;
            //    msg.PageHeight = 0;
            //    msg.PageWidth =0;
            //    msg.UseEmfPlus = true;
            win4Print.document.write(msg);
            win4Print.document.close();
            win4Print.focus();
            win4Print.print();
            win4Print.close();
        }
    </script>
</head>
<body onkeydown="if(event.keyCode==115){window.location='../New_Booking/frm_New_Booking.aspx?option=Edit';}else if(event.keyCode==112){window.location='../New_Booking/frm_New_Booking.aspx';}">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divButtons">
        <input type="button" id="btnPrint" value="Print" onclick="SetPrintOption()" style="border-left: 2px solid White;
            color: White; font-weight: bold; font-size: 12px; background-color: #6086ac;
            font-family: Verdana; border-right-color: White; border-right-width: 2px; border-top-color: White;
            border-top-width: 2px; border-bottom-color: White; border-bottom-width: 2px;"
            tabindex="1" />
        <input type="button" id="btnGoHome" value="Go Home" style="border-left: 2px solid White;
            color: White; font-weight: bold; font-size: 12px; background-color: #6086ac;
            font-family: Verdana; border-right-color: White; border-right-width: 2px; border-top-color: White;
            border-top-width: 2px; border-bottom-color: White; border-bottom-width: 2px;"
            onclick="window.location='../Masters/Default.aspx';" tabindex="2" />
        <asp:Button ID="btnGoForNewOrder" runat="server" OnClick="btnGoForNewOrder_Click"
            Text="New Booking (F1)" TabIndex="3" />
        &nbsp;<asp:Button ID="btnf2" runat="server" Text="Edit Booking (F4)" TabIndex="4"
            PostBackUrl="~/New_Booking/frm_New_Booking.aspx?option=Edit';" />
        &nbsp;<asp:Button ID="btnPreviousSlip" runat="server" OnClick="btnPreviousSlip_Click"
            Text="Previous" TabIndex="4" Visible="False" />
        &nbsp;<asp:Button ID="btnNextSlip" runat="server" OnClick="btnNextSlip_Click" Text="Next"
            TabIndex="5" Visible="False" />
        &nbsp;</div>
    <table>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div id="divPrint" class="paper" style="page-break-after: always;">
        <% if (strPreview.Length > 0)
           {
               Response.Write(strPreview);
           }
        %>
    </div>
    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="ErrorMessage" />
    <asp:HiddenField ID="hdnId" runat="server" />
    </form>
</body>
</html>