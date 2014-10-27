<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true" CodeBehind="frmFactoryBoundToMachine.aspx.cs" Inherits="QuickWeb.Factory.frmFactoryBoundToMachine" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divShowMsg" class="row-fluid col-sm-12 Textpadding" style="display: none;">
        <div id="DivContainerStatus">
            <div id="DivContainerInnerStatus" class="span label label-default">
                <h4 class="textmargin">
                    <span style="margin-left: 40%">
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="White"
                            ClientIDMode="Static" />
                        <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" ClientIDMode="Static"
                            ForeColor="White" /></span> <span style="margin-left: -20%"></span>
                </h4>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-sm-6 div-margin">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        How to use?</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid" style="text-align: justify;">
                        <p class="textBold">
                            Secure your information with increased security measures. Authenticate the devices
                            to ensure that your information is being accessed from a trusted device. By enabling
                            this feature, Quick Dry Cleaning Software will only be accessed through devices
                            which are permitted by the administrator .<br />
                            It is fast easy and reliable. Follow the below mentioned steps to enable this feature
                            for your software.
                        </p>
                        <div>
                            <ol style="margin-left: 20px">
                                <li>Kindly make sure you have configured email and mobile no in workshop configuration,
                                    click<a class="link2" href="../Factory/frmWorkshopDetails.aspx"> here to Verify / Change</a> workshop
                                        details. </li>
                                <li>Click Yes to Enable Authenticated Access Control. Please note, all the devices,
                                    tablets and phones, from where you access this software, will need to be authenticated.
                                </li>
                                <li>Click Yes to Enable Authenticated Access Control. Please note, all the devices,
                                    tablets and phones, from where you access this software, will need to be authenticated.
                                </li>
                                <li>Login to software application again. </li>
                                <li>Click to generate code for this device.</li>
                                <li>Enter the device name. Give a pet name to this device so that you can identify
                                    all the devices you have given access to.</li>
                                <li>Enter the verification code sent to your email.</li>
                                <li>Login and you are all set.</li>
                                <li>Register all other devices from where you wish to access the software application.</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 div-margin">
            <div class="panel panel-primary ">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Authenticated Access Control</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid">
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-8">
                            <p class="textBold">
                                Do you want to enable Authenticated Access Control</p>
                        </div>
                        <div class="">
                            <input type="checkbox" class="ace ace-switch ace-switch-5" checked="False" clientidmode="Static"
                                runat="server" id="rdrBoundToMachine" />
                            <span class="lbl"></span>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div id="divBoudToMachine" visible="false" runat="server" clientidmode="Static">
                            <asp:GridView ID="grdBoudToMachine" runat="server" AutoGenerateColumns="false" EnableTheming="false"
                                class="Table Table-striped Table-bordered Table-hover" ShowFooter="false" EmptyDataText="No devices  for  Authenticated Access Control"
                                OnRowCommand="grdSearchResult_OnRowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DeviceName" ReadOnly="true" HeaderText="Device" />
                                    <asp:BoundField DataField="CreationDate" ReadOnly="true" HeaderText="Authenticated" />
                                    <asp:BoundField DataField="LastLoginDate" ReadOnly="true" HeaderText="Last Login" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("Id") %>' />
                                            <asp:LinkButton ID="lnkBtnInvoke" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ClientIDMode="Static" CssClass="btn btn-danger btn-block"><i class="fa fa-trash-o"></i>&nbsp;Revoke Access</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnMobileNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEmailID" runat="server" ClientIDMode="Static" />
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            $('#rdrBoundToMachine').click(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
            });
            $('#rdrBoundToMachine').click(function (e) {
                if (e.clientX == 0 || e.clientY == 0) {
                    return false;
                }
                if ($('#rdrBoundToMachine').is(':checked')) {
                    var mobile = $('#hdnMobileNo').val();
                    var EmalID = $('#hdnEmailID').val();
                    if ((mobile == '') || (EmalID == '')) {
                        setDivMouseOver('#FA8602', '#999999');
                        $('#lblMsg').text('Kindly make sure you have configured email and mobile no in workshop Details');
                        return false;
                    }
                    __doPostBack('ctl00$ContentPlaceHolder1$rdrBoundToMachine', null);
                }
                else {
                    __doPostBack('ctl00$ContentPlaceHolder1$rdrBoundToMachine', null);
                }

            });
        });

        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
        }
    </script>
</asp:Content>
