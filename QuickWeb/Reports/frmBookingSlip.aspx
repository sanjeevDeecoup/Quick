<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="Reports_frmBookingSlip" Title="Untitled Page" Codebehind="frmBookingSlip.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="Javascript">
function SetPrintOption()
{
	var win4Print = window.open('','Win4Print');
	var msg=document.getElementById("divPrint").innerHTML;	
	win4Print.document.write(msg);	
	win4Print.document.close();
	win4Print.focus();
	win4Print.print();
	win4Print.close();
}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div id="divPrint" class="paper" style="page-break-after: always;">
        <center>
 <% if (strPreview.Length > 0)
   {
       Response.Write(strPreview);
   }
%>
</center></div>
</asp:Content>

