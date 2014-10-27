<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FactoryChallanSummary.aspx.cs"
    Inherits="QuickWeb.Reports.FactoryChallanSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%Response.Write(ConfigurationManager.AppSettings["AppTitle"]); %></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table>
            <center>
                <div id="divPrintOptions">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" ForeColor="#CC0000"
                                    CssClass="SuccessMessage"></asp:Label>
                                <asp:Label ID="lblQuantity" runat="server" Visible="False"></asp:Label>
                                <asp:Label ID="lblStoreName" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ImgPrintButton" runat="server" Text="Print" OnClick="ImgPrintButton_Click" />
                            </td>
                            <td>
                                <asp:Button ID="imgCancel" runat="server" Text="Cancel" OnClientClick="return window.close();" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <tr>
                <td>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="856px" Height="660px">
                        <LocalReport ReportPath="RDLC/ChallanSFReport.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="ChallanDataSet_sp_Challan" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="Copy" TypeName="ChallanDataSet">
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
