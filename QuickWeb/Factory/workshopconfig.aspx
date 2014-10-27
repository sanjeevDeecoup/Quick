<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="workshopconfig.aspx.cs" Inherits="QuickWeb.Factory.workshopconfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <h3>
                    Workshop Configuration</h3>
            </div>
        </div>
        <div class="well well-small form-signin2">
            <h5>
                Select yes if you want to force Use of Barcode and disable Move All Buttons on following
                screens.</h5>
            <div class="row-fluid">
                <div class="span2">
                    Recieve in Workshop</div>
                <div class="span10">
                    <asp:DropDownList ID="drpOption" runat="server">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span2">
                    Sent to Store</div>
                <div class="span10">
                    <asp:DropDownList ID="drpOption1" runat="server">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
</asp:Content>