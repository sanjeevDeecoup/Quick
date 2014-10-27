<%@ Page Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    Inherits="MenuRights" Title="Untitled Page" CodeBehind="MenuRights.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/styles.css" rel="stylesheet" type="text/css" />
    <script src="../js/main.js" type="text/javascript"></script>
    <style type="text/css">
        .table
        {
            margin-bottom: 0px;
        }
    </style>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="text-center"> 
                 <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />                 
                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />                        
                </h4>
            </div>
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="col-sm-12">
    <div class="panel panel-primary well-sm-tiny1">
        <div class="panel-heading">
            <h3 class="panel-title">
                Menu Rights Assignment</h3>
        </div>
        <div class="panel-body">           
            <div class="row-fluid">               
                <div class="col-sm-5">
                    <div class="input-group">
                        <span class="input-group-addon">User Type</span>
                        <asp:DropDownList ID="drpUserTypes" runat="server" DataSourceID="SqlDataSource1"
                            Width="99%" CssClass="form-control" DataTextField="UserType" DataValueField="UserTypeID"
                            AutoPostBack="True" OnSelectedIndexChanged="drpUserTypes_SelectedIndexChanged" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <asp:LinkButton ID="btnUpdateRights" runat="server" 
                        EnableTheming="false" CssClass="btn btn-info  btn-block btn-lg" 
                        onclick="btnUpdateRights_Click" ><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                </div>
            </div>
            <div class="row-fluid div-margin">               
                <div id='cssmenu' class="col-sm-11">
                    <ul>
                        <li></li>
                        <li><a class="testMenu" href=''><span><i class="fa fa-user fa-lg"></i>&nbsp;Customer</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdCustomer1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover well padding">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"   HeaderStyle-BackColor="#E4E4E4"/>
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdCustomer2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover well padding">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"  HeaderStyle-BackColor="#E4E4E4"/>
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>
                        <li><a class="testMenu" href=''><span><i class="fa fa-download fa-lg"></i>&nbsp;Drop</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdDrop1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover well padding">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"   HeaderStyle-BackColor="#E4E4E4"/>
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdDrop2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover well padding">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title"  HeaderStyle-BackColor="#E4E4E4"  ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>
                        <li><a class="testMenu" href=''><span><i class="fa fa-bitbucket fa-lg"></i>&nbsp;Process</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdProcess1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover well padding">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"  HeaderStyle-BackColor="#E4E4E4" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate> 
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdProcess2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover ">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"  HeaderStyle-BackColor="#E4E4E4"/>
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>
                        <li><a class="testMenu" href=''><span><i class="fa fa-truck fa-lg"></i>&nbsp;PickUp</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdPickUp1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle"   HeaderStyle-BackColor="#E4E4E4"/>
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdPickUp2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>
                        <li><a class="testMenu" href=''><span><i class="fa fa-usd fa-lg"></i>&nbsp;Account</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdAccount1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdAccount2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>

                         <li><a class="testMenu" href=''><span><i class="fa fa-book fa-lg"></i>&nbsp;Reports</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdReport1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdReport2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>

                         <li><a class="testMenu" href=''><span><i class="fa fa-database fa-lg"></i>&nbsp;Master Data</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdMasterData1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdMasterData2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>


                         <li><a class="testMenu" href=''><span><i class="fa fa-cog fa-lg"></i>&nbsp;Admin</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdAdmin1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdAdmin2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title" ItemStyle-Wrap="false" SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>

                        <li><a class="testMenu" href=''><span><i class="fa fa-th fa-lg"></i>&nbsp;Special Access Right</span></a>
                            <ul>
                                <div class="row-fluid">
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdSpecialPart1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle"  HeaderStyle-BackColor="#E4E4E4" HeaderText="Title"  SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-6 Textpadding">
                                        <asp:GridView ID="grdSpecialPart2" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                                            EnableTheming="false" class="table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:BoundField DataField="PageTitle" HeaderText="Title"   HeaderStyle-BackColor="#E4E4E4"  SortExpression="PageTitle" />
                                                <asp:TemplateField HeaderText="&nbsp;Allow"  HeaderStyle-BackColor="#E4E4E4" ItemStyle-Width="14%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" Text="&nbsp;Allow" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllow" runat="server" Checked='<%# Bind("RightToView") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
           
        </div>
    </div>
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
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('.testMenu').click(function () {
                var myVar1 = setInterval(function () { SetFocusSize() }, 300);
                function SetFocusSize() {
                    var height1 = document.body.clientHeight;
                    height1 = height1 + 'px';
                    $("#tdheight").css("height", height1);
                    $(".nav_new").css("height", height1);
                    clearInterval(myVar1);
                    return false;
                }
            });
        });
    </script>
      <script type="text/javascript">
          function setDivMouseOver(argColorOne, argColorTwo) {
              document.getElementById('divShowMsg').style.display = "inline";
              $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
              setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
          }
    </script>
</asp:Content>
