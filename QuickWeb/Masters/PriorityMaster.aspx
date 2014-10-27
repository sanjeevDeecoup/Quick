<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    Inherits="PriorityMaster" Title="Priority Master" CodeBehind="PriorityMaster.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkName() {
            clearMsg();
            var strname = document.getElementById("<%=txtPriority.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text("Kindly enter Customer preference");
                document.getElementById("<%=txtPriority.ClientID %>").focus();
                return false;
            }
        }
        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }
        function clearMsg() {
            $('#lblErr').text('');
            $('#lblMsg').text('');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="textmargin">
                    <span style="margin-left: 40%">
                        <asp:Label ID="lblMsg" ClientIDMode="static" runat="server" EnableViewState="False"
                            ForeColor="White" Font-Bold="True" />
                        <asp:Label ID="lblErr" ClientIDMode="static" runat="server" ForeColor="White" EnableViewState="False"
                            Font-Bold="True" />
                    </span><span style="margin-left: -20%"></span>
                </h4>
            </div>
        </div>
    </div>
    <div class="row-fluid div-margin">
        <div class="col-sm-6">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Customer Preference</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-10 Textpadding">
                            <div class="input-group input-sm Textpadding">
                                <span class="input-group-addon IconBkColor">Customer Preference</span>
                                <asp:TextBox ID="txtPriority" runat="server" MaxLength="100" CssClass="form-control"
                                    placeholder="Kindly enter customer preference" />
                                <asp:FilteredTextBoxExtender ID="txtPriority_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtPriority" ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                </asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding">
                            <span class="span textBold">&nbsp;*</span>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnSave" class="btn btn-info" runat="server" OnClick="btnSave_Click"
                        EnableTheming="false" OnClientClick="return checkName();"><i class="fa fa-floppy-o"></i>&nbsp;Save</asp:LinkButton>
                    <asp:LinkButton ID="btnEdit" class="btn btn-info" EnableTheming="false" runat="server"
                        OnClientClick="return checkName();" OnClick="btnEdit_Click" Visible="false"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny" style="overflow: auto; height: 320px;">
            <asp:GridView ID="grdSearchResult" runat="server" DataKeyNames="PriorityID" AutoGenerateColumns="False"
                EmptyDataText="There are no data records to display." CssClass="Table Table-striped Table-bordered Table-hover"
                EnableTheming="false" Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <%-- <asp:BoundField DataField="PriorityID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                    SortExpression="PriorityID" />--%>
                    <asp:BoundField DataField="Priority" HeaderText="Customer Preference" ReadOnly="True"
                        SortExpression="Priority" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblPriority" runat="server" Visible="False"></asp:Label>
</asp:Content>
