<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="promos.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.6.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a id ="a1" href="javascript:alert('hello');">Click to say hello</a>
        <input type="button" value="Disable" onclick = "DisableA();" />
        <script type="text/javascript">
            function DisableA() {
                //$("#a1").attr("href", "#");
                $("#a1").removeAttr("href");
            }
        </script>
    </div>
    </form>
</body>
</html>
