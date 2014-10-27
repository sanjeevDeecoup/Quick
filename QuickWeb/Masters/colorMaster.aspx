<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeBehind="colorMaster.aspx.cs"
    Inherits="QuickWeb.Masters.colorMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkName() {
            clearMsg();
            var strname = document.getElementById("<%=txtJobType.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text("Kindly enter color name.");
                document.getElementById("<%=txtJobType.ClientID %>").focus();
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                        Color</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-10 Textpadding">
                            <div class="input-group input-sm Textpadding">
                                <span class="input-group-addon IconBkColor">Color Name</span>
                                <asp:TextBox ID="txtJobType" runat="server" MaxLength="100" placeholder="Kindly Enter color name"
                                    CssClass="form-control" />
                                <asp:FilteredTextBoxExtender ID="Filteredtextboxextender1" runat="server" FilterMode="InvalidChars"
                                    TargetControlID="txtJobType" InvalidChars="`~:;,-">
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
                    <asp:LinkButton ID="btnUpdate" class="btn btn-info" EnableTheming="false" runat="server"
                        OnClientClick="return checkName();" OnClick="btnUpdate_Click" Visible="false"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" class="btn btn-info" EnableTheming="false" runat="server"
                        Visible="false" OnClick="btnDelete_Click"><i class="fa fa-trash-o"></i>&nbsp;Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny" style="overflow: auto; height: 320px;">
            <asp:GridView ID="grdSearchResult" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                EmptyDataText="There are no color data records to display." CssClass="Table Table-striped Table-bordered Table-hover"
                Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                EnableTheming="false">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <%-- <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                        SortExpression="ID" />--%>
                    <asp:BoundField DataField="ColorName" HeaderText="Color Name" ReadOnly="True" SortExpression="Remarks" />
                </Columns>
                <HeaderStyle Font-Size="12px" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>
    <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblShift" runat="server" Visible="False"></asp:Label>
</asp:Content>
