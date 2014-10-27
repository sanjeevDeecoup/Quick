<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" Inherits="Reports_CustomerPayment" Codebehind="CustomerPaymentReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <fieldset>
    <legend>Customer Payment Report</legend>
    <table style="width: 100%;">
        <tr>
            <td width="100">
                &nbsp;</td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;Customer Code</td>
            <td width="100">
                <asp:TextBox ID="txtCustomerCode" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
            </td>
            <td>
                &nbsp;Start Date
                <asp:TextBox ID="txtStartDate" runat="server" Width="80px"></asp:TextBox>
                <asp:Image ID="imgStartDate" runat="server" />
                
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</fieldset>
</asp:Content>

