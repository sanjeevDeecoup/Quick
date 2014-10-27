<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="QuickWeb.WebsiteUsers.UserLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Registration Form</title>
    <link href="../css/template.css" rel="stylesheet" type="text/css" />
    <link href="../css/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").validationEngine();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <td colspan="2">
                    <div style="border: 1px solid #CCCCCC; padding: 10px;">
                        <table cellpadding="0" cellspacing="20" style="background-color: White">
                            <tr>
                                <td valign="top" colspan="2" style="text-align: center">
                                    <h1>
                                        Login Form</h1>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Select Branch :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpBranchName" runat="server" CssClass="validate[required] radio">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                           <tr>
                                <td>
                                    User Name :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="12" CssClass="validate[required]"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Password :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPwd" runat="server" MaxLength="15" CssClass="validate[required]"
                                        TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                          <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSubmit" runat="server" Text="Login" onclick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                            <td></td>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

