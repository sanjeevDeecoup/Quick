<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="frmWorkshopUser.aspx.cs" Inherits="QuickWeb.Factory.frmWorkshopUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #passstrength
        {
            color: red;
            font-family: verdana;
            font-size: 10px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid textCenter">
        <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="16px"
            CssClass="textRed" />
        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="16px"
            CssClass="textGreen" />
        <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
    </div>
    <div class="row-fluid ">
        <div class="col-sm-4">
            <div class="row-fluid">
                <div class="col-sm-11 input-group input-group-sm Textpadding">
                    <span class="input-group-addon width2" >User Id</span>
                    <asp:TextBox ID="txtUserId" runat="server" MaxLength="20" placeholder="User ID" CssClass="form-control" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="InvalidChars"
                        TargetControlID="txtUserId" InvalidChars="`~:;,-">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-1 Textpadding">
                    <span class="textRed">&nbsp;*</span></div>
            </div>
            <div class="row-fluid div-margin2">
                <div class="col-sm-11 input-group input-group-sm Textpadding">
                 <span class="input-group-addon width2">Password</span>
                    <asp:TextBox ID="txtUserPassword" Width="33%" runat="server" MaxLength="30" TextMode="Password" EnableViewState="false"
                        placeholder="Password" ClientIDMode="Static" CssClass="form-control" />
                       &nbsp;<asp:CheckBox ID="checkUpdatePassword" runat="server" EnableViewState="False" Text="&nbsp;Also Update Password."
                    Visible="False"  />
                </div>
                <div class="col-sm-1 Textpadding">
                    <span class="textRed">&nbsp;*</span></div>
               
            </div>
             <div class="row-fluid">
            <span id="passstrength"></span>
            </div>
             <div class="row-fluid div-margin2">
                <div class="col-sm-6 input-group input-group-sm PaddingLeft">
                    <span class="input-group-addon">User Status</span>
                    <asp:CheckBox ID="chkActive" runat="server" Text="&nbsp;Active" Checked="True" BorderStyle="None"
                        CssClass="form-control" />
                </div>
            </div>
            
            <div class="row-fluid div-margin2" style="display:none">
                <div class="col-sm-11 input-group input-group-sm Textpadding">
                <span class="input-group-addon width2" >User Pin</span>
                    <asp:TextBox ID="txtUserPin" runat="server" MaxLength="200" placeholder="User Pin"
                        CssClass="form-control" />
                </div>
            </div>          
        </div>
        <div class="col-sm-4 PaddingLeft">

           <div class="row-fluid div-margin2">
                <div class="col-sm-11 input-group input-group-sm Textpadding">
                  <span class="input-group-addon width2" >Name</span>
                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" CssClass="form-control"
                        placeholder="User Name" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="InvalidChars"
                        TargetControlID="txtUserName" InvalidChars="`~:;,-">
                    </cc1:FilteredTextBoxExtender>
                </div>
                <div class="col-sm-1 Textpadding">
                    <span class="textRed">&nbsp;*</span></div>
            </div>

            <div class="row-fluid div-margin2">
                <div class="input-group input-group-sm">
                    <span class="input-group-addon">User Type&nbsp;</span>
                    <asp:DropDownList ID="drpUserType" runat="server" DataSourceID="SqlDataUserTypeMaster"
                        Width="90%" DataTextField="UserType" DataValueField="UserTypeID" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataUserTypeMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                        SelectCommand="SELECT [UserTypeID], [UserType] FROM [UserTypeMaster] where UserTypeID<>4" ProviderName="System.Data.SqlClient">
                    </asp:SqlDataSource>
                </div>
            </div>


            <div class="row-fluid" style="display:none">
                <div class="">
                    <asp:TextBox ID="txtUserAddress" runat="server" TextMode="MultiLine" MaxLength="50"
                        placeholder="User Address" CssClass="form-control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="InvalidChars"
                        TargetControlID="txtUserAddress" InvalidChars="`~:;,-">
                    </cc1:FilteredTextBoxExtender>
                </div>
            </div>
              <div class="row-fluid div-margin2" style="display:none">
                <div class="col-sm-12 input-group input-group-sm Textpadding">
                <span class="input-group-addon">Bar Code</span>
                    <asp:TextBox ID="txtUserbarcode" runat="server" placeholder="Barcode" MaxLength="200"
                        CssClass="form-control" />
                </div>
            </div>
            <div class="row-fluid div-margin2" style="display:none">
                <div class="input-group input-group-sm">
                    <span class="input-group-addon">
                        <img src="../images/landline.png" width="15px" height="18px" class="" /></span>
                    <asp:TextBox ID="txtUserPhone" runat="server" MaxLength="30" CssClass="form-control"
                        placeholder="User Phone"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                        TargetControlID="txtUserPhone" >
                    </cc1:FilteredTextBoxExtender>
                </div>
            </div>
            <div class="row-fluid" style="display:none">
                <div class="input-group input-group-sm">
                    <span class="input-group-addon"><i class="fa fa-phone fa-lg"></i></span>
                    <asp:TextBox ID="txtUserMobile" runat="server" placeholder="Mobile" MaxLength="30"
                        CssClass="form-control" />
                         <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                        TargetControlID="txtUserMobile" >
                    </cc1:FilteredTextBoxExtender>
                </div>
            </div>
            <div class="row-fluid div-margin2" style="display:none">
                <div class="input-group input-group-sm">
                    <span class="input-group-addon"><i class="fa fa-envelope-o fa-lg"></i></span>
                    <asp:TextBox ID="txtUserEmailId" runat="server" placeholder="User Email Id" MaxLength="100"
                        CssClass="form-control" />
                </div>
            </div>
           
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="col-sm-2">
        </div>
        <div class="col-sm-1">
            <asp:Button ID="btnEdit" runat="server" EnableTheming="false" CssClass="btn btn-primary btn-block"
                Text="Update" OnClick="btnEdit_Click" OnClientClick="return checkEntry();" />       
            <asp:Button ID="btnSave" CssClass="btn btn-primary btn-block" runat="server" Text="Save"
                EnableTheming="false" OnClick="btnSave_Click" OnClientClick="return checkEntry();" />
        </div>
         <div class="col-sm-1">
            <asp:Button ID="btnAddNew" runat="server" Text="Refresh" OnClick="btnAddNew_Click"
                EnableTheming="false" CssClass="btn btn-primary btn-block" />
        </div>
        <div class="col-sm-1">
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" Visible="false"
                EnableTheming="false" CssClass="btn btn-primary btn-block" OnClientClick="return checkEntry();" />
        </div>
        <div class="col-sm-1">
            <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click" Visible="false"
                EnableTheming="false" CssClass="btn btn-primary btn-block" />
        </div>
       
    </div>
    <div class="BorderTopLine div-margin2">
    </div>   
    <div class="row-fluid ">
        <div class="margin-left-right" style="overflow: auto; height: 260px;">
            <asp:GridView ID="grdSearchResult" runat="server" EnableTheming="false" class="table table-striped table-bordered table-hover well div-margin"
                AllowPaging="True" AutoGenerateColumns="False" PageSize="10" EmptyDataText="There are no data records to display."
                CellPadding="2" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                OnPageIndexChanging="grdSearchResult_PageIndexChanging">
                   <pagerstyle cssclass="gridview" HorizontalAlign="Center">
                         </pagerstyle>
                <Columns>
                    <asp:CommandField ShowSelectButton="True" HeaderStyle-BackColor="#E4E4E4" />
                    <asp:BoundField DataField="UserId" HeaderText="User Id" ReadOnly="True" HeaderStyle-BackColor="#E4E4E4" />
                    <asp:BoundField DataField="UserType" HeaderText="User Type" HeaderStyle-BackColor="#E4E4E4" />
                    <asp:BoundField DataField="UserName" HeaderText="Name" HeaderStyle-BackColor="#E4E4E4" /> 
                    <asp:TemplateField HeaderText="UserActive" HeaderStyle-BackColor="#E4E4E4">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("UserActive") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("UserActive") %>' Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUserPin" runat="server" Text='<%# Bind("UserPin") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUserBarcode" runat="server" Text='<%# Bind("Userbarcode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUserPassword" runat="server" Text='<%# Bind("UserPassword") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
    <!-- Start of js -->
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkEntry() {
            var brcode = document.getElementById("<%=txtUserId.ClientID %>").value.trim().length;
            var brname = document.getElementById("<%=txtUserName.ClientID %>").value.trim().length;
            var pwd = document.getElementById("<%=txtUserPassword.ClientID %>").value.trim().length;
            if (brcode == "" || brcode.length == 0) {
                alert("Please enter User Id.");
                document.getElementById("<%=txtUserId.ClientID %>").focus();
                return false;
            }
            if (brname == "" || brname.length == 0) {
                alert("Please enter User name.");
                document.getElementById("<%=txtUserName.ClientID %>").focus();
                return false;
            }
            if (pwd == "" || pwd.length == 0) {
                alert("Please enter password.");
                document.getElementById("<%=txtUserPassword.ClientID %>").focus();
                return false;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#txtUserPassword').keyup(function (e) {
                var strongRegex = new RegExp("^(?=.{6,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
                var mediumRegex = new RegExp("^(?=.{5,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
                var enoughRegex = new RegExp("(?=.{3,}).*", "g");
                if (false == enoughRegex.test($(this).val())) {
                    $('#passstrength').html('More Characters');
                } else if (strongRegex.test($(this).val())) {
                    $('#passstrength').className = 'ok';
                    $('#passstrength').html('Strong!');
                } else if (mediumRegex.test($(this).val())) {
                    $('#passstrength').className = 'alert';
                    $('#passstrength').html('Medium!');
                } else {
                    $('#passstrength').className = 'error';
                    $('#passstrength').html('Weak!');
                }
                return true;
            });

        });
    </script>
</asp:Content>
