<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="frmWorkShopInChallanSummary.aspx.cs" Inherits="QuickWeb.Factory.frmWorkShopInChallanSummary" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <%-- <asp:Button ID="ImgPrintButton" runat="server" Text="Print" ClientIDMode="Static"
                                    OnClick="ImgPrintButton_Click" />--%>
                        </td>
                        <td>
                            <asp:Button ID="imgCancel" runat="server" Text="Cancel" OnClientClick="return window.close();"
                                OnClick="imgCancel_Click" />
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
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Visible="false">
                    <LocalReport ReportPath="RDLC/BranchInvoiceChallan.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
</asp:Content>