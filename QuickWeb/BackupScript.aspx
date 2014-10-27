<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true" CodeBehind="BackupScript.aspx.cs" Inherits="QuickWeb.BackupScript" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


       <asp:Button ID="btnScript1" runat="server" Text="Generate" 
        onclick="btnScript1_Click" />
        <asp:Label ID="lblsummarySucess" runat="server"></asp:Label>
        <asp:Label ID="lblsummaryErr" runat="server"></asp:Label>
</asp:Content>
