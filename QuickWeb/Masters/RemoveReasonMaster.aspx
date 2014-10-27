<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="RemoveReasonMaster.aspx.cs" Inherits="RemoveReasonMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .tooltip-inner
        {
            max-width: 340px;
            width: 340px;
            background-color: #F1F1F1;
            text-align: left;
            font-size: 14px;
            color: Black;
            border: 1px solid #C0C0A0;
        }
    </style>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function checkName() {
            clearMsg();
            var strname = document.getElementById("<%=txtReason.ClientID %>").value.trim().length;
            if (strname == "" || strname.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblMsg').text("Kindly enter cloth return cause type.");
                document.getElementById("<%=txtReason.ClientID %>").focus();
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
     <script type="text/javascript" language="javascript">
         $(document).ready(function (e) {
             $("#CauseInfo").tooltip({
                 title: 'This will be used as reason when clothes are returned without processing.',
                 html: true
             });
         });
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
                        Cloth Return Cause
                    </h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-10 Textpadding">
                            <div class="col-sm-10 Textpadding">
                                <div class="input-group input-sm Textpadding">
                                    <span class="input-group-addon IconBkColor">
                                        <%=ClothReturnCauses%></span>
                                    <asp:TextBox ID="txtReason" runat="server" MaxLength="100" CssClass="form-control"   placeholder="Kindly enter Cloth Return Cause" />
                                    <asp:FilteredTextBoxExtender ID="Filteredtextboxextender1" runat="server" FilterMode="InvalidChars"
                                        TargetControlID="txtReason" InvalidChars="`~:;,-">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-sm-1 Textpadding form-inline">
                                <div class="form-group">
                                    <span class="span textBold">&nbsp;*</span>
                                </div>
                                <div class="form-group" style="margin-top: -9px">
                                    &nbsp;<i id="CauseInfo" data-placement="right" class="fa fa fa-info-circle fa-lg txtColor"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnSave" class="btn btn-info" runat="server" OnClick="btnSave_Click"
                        EnableTheming="false" OnClientClick="return checkName();"><i class="fa fa-floppy-o"></i>&nbsp;Save</asp:LinkButton>
                    <asp:LinkButton ID="btnEdit" class="btn btn-info" EnableTheming="false" runat="server"
                        OnClientClick="return checkName();" OnClick="btnEdit_Click" Visible="false"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" class="btn btn-info" EnableTheming="false" runat="server"
                        Visible="false" OnClick="btnDelete_Click"><i class="fa fa-trash-o"></i>&nbsp;Delete</asp:LinkButton>
                </div>
            </div>
        </div>
        <asp:Label ID="lblSaveOption" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblUpdateId" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblReason" runat="server" Visible="False"></asp:Label>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny" style="overflow: auto; height: 320px;">
            <asp:GridView ID="grdSearchResult" runat="server" DataKeyNames="RemoveReasonID" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                 <%--   <asp:BoundField DataField="RemoveReasonID" HeaderText="ID" InsertVisible="False"
                        ReadOnly="True" SortExpression="RemoveReasonID" />--%>
                    <asp:BoundField DataField="RemoveReason" HeaderText="Cloth Return Cause" ReadOnly="True"
                        SortExpression="RemoveReason" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
