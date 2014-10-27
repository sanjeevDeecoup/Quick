<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="ErrorPage.aspx.cs" Inherits="QuickWeb.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Label ID="LblError" runat="server" Text="oops, Something has gone wrong. Contact Support Team to get this resolved."></asp:Label>
    </h1>
    <br />
    <br />
    <hr />
</asp:Content>