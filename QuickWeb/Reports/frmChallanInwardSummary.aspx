<%@ Page Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Codebehind="frmChallanInwardSummary.aspx" Inherits="QuickWeb.Reports.frmChallanInwardSummary"   %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Challan Inward Summary 
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
        </table>
        <table class="TableData" width="100%">
            <tr valign="top">
                <td>              
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" 
                            ForeColor="#CC0000" CssClass="SuccessMessage"></asp:Label>
                        <asp:Label ID="lblQuantity" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblStoreName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lbl" runat="server"></asp:Label>

                    </td> 
                </tr>           
                        <tr>
                        <td>
                         <rsweb:ReportViewer ID="RptChallan" runat="server" Width="700px" Height="500px">
                         <LocalReport ReportPath="RDLC/ChallanInward.rdlc"></LocalReport>
                            </rsweb:ReportViewer></td></tr>
       
                    </table>
                    <asp:SqlDataSource ID="SqlSourceReportBooking" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT * FROM [ViewBookingReport]"></asp:SqlDataSource>
             
            <table>
            <tr valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
        </table>

</asp:Content>
