<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    CodeBehind="frmHolidayMaster.aspx.cs" Inherits="QuickWeb.Masters.frmHolidayMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function checkEntry() {
            var date = document.getElementById("<%=txtHolidayDate.ClientID %>").value.trim().length;
            if (date == "" || date.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblHolidaySucess').text("Kindly enter date.");
                document.getElementById("<%=txtHolidayDate.ClientID %>").focus();
                return false;
            }
            var descrption = document.getElementById("<%=txtHolidayDescription.ClientID %>").value.trim().length;
            if (descrption == "" || descrption.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblHolidaySucess').text("Kindly enter description.");
                document.getElementById("<%=txtHolidayDescription.ClientID %>").focus();
                return false;
            }
        }
        function CheckSearch() {
            var descrption = document.getElementById("<%=txtHolidayDescription.ClientID %>").value.trim().length;
            if (descrption == "" || descrption.length == 0) {
                setDivMouseOver('#FA8602', '#999999');
                $('#lblHolidaySucess').text("Kindly enter description.");
                document.getElementById("<%=txtHolidayDescription.ClientID %>").focus();
                return false;
            }
        }

        function clearMsg() {
            $('#lblHolidaySucess').text('');
            $('#lblHolidayErr').text('');
        }

        function setDivMouseOver(argColorOne, argColorTwo) {
            document.getElementById('divShowMsg').style.display = "inline";
            $('#DivContainerInnerStatus').css('backgroundColor', argColorOne);
            setTimeout(function () { $('#divShowMsg').fadeOut(3000); }, 4000);
        }

    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (e) {

            var Date = $('#ctl00_ContentPlaceHolder1_txtHolidayDate');
            Date.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-M-yyyy",
                language: "tr"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
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
                        <asp:Label ID="lblHolidaySucess" ClientIDMode="static" runat="server" EnableViewState="False"
                            ForeColor="White" Font-Bold="True" />
                        <asp:Label ID="lblHolidayErr" ClientIDMode="static" runat="server" ForeColor="White"
                            EnableViewState="False" Font-Bold="True" />
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
                        Holiday Events</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-7 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor">Date</span>
                                <asp:TextBox ID="txtHolidayDate" runat="server" MaxLength="11" onkeypress="return false;" placeholder="Kindly enter date"
                                    onpaste="return false;" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon IconBkColor"><i class="fa fa-calendar fa-lg"></i></span>
                                <%--<cc1:CalendarExtender ID="txtHolidayDate_CalendarExtender" runat="server" Enabled="True"
                                                Format="dd-MMM-yyyy" TargetControlID="txtHolidayDate">
                                            </cc1:CalendarExtender>--%>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding form-inline">
                            <div class="form-group">
                                <span class="span textBold">&nbsp;*</span>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-7 Textpadding">
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon IconBkColor">Label</span>
                                <asp:TextBox ID="txtHolidayDescription" runat="server" CssClass="form-control" placeholder="Kindly enter label" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1 Textpadding form-inline">
                            <div class="form-group">
                                <span class="span textBold">&nbsp;*</span>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid div-margin">
                        <div class="col-sm-12 Textpadding">
                            <asp:Label ID="Label1" runat="server" Text="Weekly off and other holidays will not be offered as Delivery Date (Delivery Date  will automatically get pushed to next working day)"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnHolidaySave" runat="server" CausesValidation="False" class="btn btn-info"
                        EnableTheming="false" OnClick="btnHolidaySave_Click" OnClientClick="return checkEntry();"><i class="fa fa-floppy-o"></i>&nbsp;Save</asp:LinkButton>
                    <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="False" class="btn btn-info"
                        EnableTheming="false" OnClick="btnUpdate_Click" Visible="False" OnClientClick="return checkEntry();"><i class="fa fa-check-square-o"></i>&nbsp;Update</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" Visible="False" CausesValidation="False"
                        class="btn btn-info" EnableTheming="false" OnClick="btnDelete_Click"><i class="fa fa-trash-o"></i>&nbsp;Delete</asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        Weekly OFF</h3>
                </div>
                <div class="panel-body">
                    <div class="row-fluid div-margin">
                        <div class="col-sm-8 Textpadding">
                            <div class="input-group input-group-sm">
                                &nbsp;<span class="input-group-addon IconBkColor">Weekly Off</span>
                                <asp:DropDownList ID="drpWeekend" runat="server" EnableTheming="false" CssClass="form-control">
                                    <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Sun" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Mon" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Tue" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Wed" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Thu" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Fri" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Sat" Value="7"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:LinkButton ID="btnWeekendSave" class="btn btn-info" runat="server" OnClick="btnWeekendSave_Click"><i class="fa fa-floppy-o"></i>&nbsp;Save </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="well well-sm-tiny" style="overflow: auto; max-height: 320px;">
            <asp:GridView ID="grdHoliday" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                CssClass="Table Table-striped Table-bordered Table-hover" EnableTheming="false"
                OnSelectedIndexChanged="grdHoliday_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblWeek" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text='<%# Eval("weeklyoff") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" CommandName="SelectCustomer" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text='<%# Eval("holidayid") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="description" HeaderText="Description" ReadOnly="True" />
                    <asp:BoundField DataField="date" HeaderText="Date" ReadOnly="True" />
                    <asp:BoundField DataField="DateDay" HeaderText="Day" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hdnId" runat="server" Value="0" />
        </div>
    </div>
</asp:Content>
