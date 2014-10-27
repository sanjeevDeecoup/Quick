<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="frmWorkshopMenuRights.aspx.cs" Inherits="QuickWeb.Factory.frmWorkshopMenuRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../JavaScript/javascript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid textCenter">
        <asp:Label ID="lblErr" runat="server" EnableViewState="false" CssClass="textRed"
            Font-Size="20px" />
        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="textGreen"
            Font-Size="20px" />
    </div>
    <div class="row-fluid">
        <div class="col-sm-1">
        </div>
        <div class="col-sm-4">
            <div class="input-group input-group-sm">
                <span class="input-group-addon">User Type</span>
                <asp:DropDownList ID="drpUserTypes" runat="server" DataSourceID="SqlDataSource1"
                    Width="99%" CssClass="form-control" DataTextField="UserType" DataValueField="UserTypeID"
                    AutoPostBack="True" OnSelectedIndexChanged="drpUserTypes_SelectedIndexChanged" />
            </div>
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="col-sm-1">
        </div>
        <div class="col-sm-4">
            <asp:GridView ID="grdMenuRights" runat="server" AutoGenerateColumns="False" OnRowCommand="grdMenuRights_OnRowCommand"
                class="table table-striped table-bordered table-hover well" EnableTheming="false"
                EmptyDataText="There are no data records to display." ShowFooter="True">
                <Columns>
                    <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"
                        HeaderStyle-BackColor="#E4E4E4" />
                    <asp:TemplateField HeaderText="Allow" HeaderStyle-BackColor="#E4E4E4">
                     <HeaderTemplate>
                            <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnUpdateRights" runat="server" Text="Update" CommandName="UPDATERIGHTS" CssClass="btn btn-primary btn-block" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:SqlDataSource ID="SqlSourceMenuRights" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT PageTitle, RightToView FROM EntMenuRights WHERE (UserTypeId = @UserTypeId) ORDER BY MenuItemLevel, PageTitle">
        <SelectParameters>
            <asp:Parameter Name="UserTypeId" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [UserTypeID], [UserType] FROM [UserTypeMaster]  where UserTypeID<>4 ORDER BY UserType">
    </asp:SqlDataSource>
    <asp:HiddenField ID="hdnSelectedUserTypeId" runat="server" />
</asp:Content>
