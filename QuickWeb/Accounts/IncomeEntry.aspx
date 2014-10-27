<%@ Page Title="" Language="C#" MasterPageFile="~/DryMasterMain.master" AutoEventWireup="true"
    CodeBehind="IncomeEntry.aspx.cs" Inherits="QuickWeb.Accounts.IncomeEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnAddNewExpense,#btnUpdate").click(function () {
                var options = $('#cmbDrExpenses1 > option:selected');
                if (options.length == 0) {
                    alert('Please add ledger name.');
                    $('#btnAddNewLedger').css({ "background-color": "Red" });
                    $('#cmbDrExpenses1').focus();
                    return false;
                }
                var txtPaidAmt = $('#txtPaidAmount').val();
                if (txtPaidAmt == "0") {
                    alert('Please enter valid amount to save.');
                    $('#txtPaidAmount').focus();
                    return false;
                }                
            });
        });
    </script>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#btnSaveRename").click(function () {
                 var txtRename = $('#txtRename').val();
                 if (txtRename == "") {
                     alert('Please enter value to rename this expense.');
                     $('#txtRename').focus();
                     return false;
                 }
             });
         });
    </script>
    <script type="text/javascript" language="javascript">
        function CheckForNewLedger() {
            var txtnewledgerbox = document.getElementById("<%= txtNewLedgerName.ClientID %>");
            var txtnewOpbal = document.getElementById("<%= txtNewLedgerOpenBal.ClientID %>");
            if (txtnewledgerbox.value == "") {
                alert("Enter Ledger Name to create.");
                txtnewledgerbox.focus();
                return false;
            }
            if (txtnewOpbal.value == "" || float.Parse(txtnewOpbal.value) <= 0) {
                alert("Enter correct opening balance");
                txtnewOpbal.focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="Fieldset">
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label1" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Receipt
                    Entry
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
        </table>
        <table class="TableData">
            <tr>
                <td>
                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Font-Bold="True" CssClass="SuccessMessage" />
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableData">
                        <tr>
                            <td class="TDCaption">
                                Date
                            </td>
                            <td width="2px" class="TDDot">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEntryDate" runat="server" MaxLength="10" onkeypress="return false;"
                                    onpaste="return false;" Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtEntryDate_CalendarExtender" runat="server" Enabled="True"
                                    Format="dd MMM yyyy" TargetControlID="txtEntryDate">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDCaption">
                                Received From
                            </td>
                            <td width="2px" class="TDDot">
                                :
                            </td>
                            <td>
                                <div id="divExpenseLedgerSelection" runat="server">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <%--<asp:DropDownList ID="cmbDrExpenses" runat="server" DataSourceID="SDTExpenseLedgers"
                                                    DataTextField="LedgerName" DataValueField="LedgerName">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList ID="cmbDrExpenses1" runat="server" DataSourceID="SDTExpenseLedgers" ClientIDMode="Static"
                                                    DataTextField="LedgerName" DataValueField="LedgerName">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAddNewLedger" runat="server" Text="Add new Ledger" OnClick="btnAddNewLedger_Click" ClientIDMode="Static"/>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnRename" runat="server" Text="Rename" OnClick="btnRename_Click" ClientIDMode="Static"
                                                    Visible="false" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRename" runat="server" Width="200px" Height="20" Visible="false" ClientIDMode="Static"></asp:TextBox>
                                                 <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender3" runat="server" FilterMode="InvalidChars" TargetControlID="txtRename"
                                InvalidChars="`~:;,-"></cc1:filteredtextboxextender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSaveRename" runat="server" Visible="false" Text="Save" OnClick="btnSaveRename_Click" ClientIDMode="Static"/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divNewLedgerCreation" runat="server" visible="false">                                  
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNewLedgerName" runat="server" MaxLength="50" Width="150px" />
                                                 <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender1" runat="server" FilterMode="InvalidChars" TargetControlID="txtNewLedgerName"
                                InvalidChars="`~:;,-"></cc1:filteredtextboxextender>
                                            </td> <td>
                                                <asp:Button ID="btnSaveNewLedger" runat="server" Text="Save" OnClick="btnSaveNewLedger_Click" ClientIDMode="Static"
                                                    OnClientClick="return CheckForNewLedger();" />
                                            </td>
                                            <td class="TDCaption" style="visibility:hidden">
                                                Opening balance
                                            </td>
                                            <td class="TDDot" style="visibility:hidden">
                                                :
                                            </td>
                                            <td style="visibility:hidden">
                                                <asp:TextBox ID="txtNewLedgerOpenBal" runat="server" MaxLength="10" Width="60px"
                                                    dir="rtl" Text="0" onfocus="if(this.value=='0'){ this.value='';}" onblur="if(this.value==''){this.value='0';}" />
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr id="trPaidAmount" runat="server">
                            <td class="TDCaption">
                                Amount
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td class="TDDot">
                                <asp:TextBox ID="txtPaidAmount" runat="server" Width="150px" Text="0" onfocus="if(this.value=='0'){ this.value='';}"
                                    onblur="if(this.value==''){this.value='0';}" MaxLength="10" ClientIDMode="Static" />
                                <cc1:FilteredTextBoxExtender ID="txtPaidAmount_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtPaidAmount" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr id="trRemark" runat="server">
                            <td class="TDCaption">
                                Narration
                            </td>
                            <td class="TDDot">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" Width="150px" MaxLength="100" />
                                  <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender2" runat="server" FilterMode="InvalidChars" TargetControlID="txtRemark"
                                InvalidChars="`~:;,-"></cc1:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>                               
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddNewExpense" runat="server" Text="Save" ClientIDMode="Static"
                                                OnClick="btnAddNewExpense_Click" />
                                        </td>
                                        <td nowrap="nowrap">
                                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Visible="false" ClientIDMode="Static"
                                                 Text="Update" />
                                            <asp:Button ID="btnDelete" runat="server" Visible="false" Text="Delete" OnClick="btnDelete_Click" />
                                            <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete this Transaction ?"
                                                Enabled="True" TargetControlID="btnDelete">
                                            </cc1:ConfirmButtonExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClientClick="window.location='ExpenseEntry.aspx';"
                                                OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 7px">
                </td>
                <td class="H1" style="font-weight: bolder">
                    <asp:Label ID="Label2" runat="server" Text="DrySoft " ForeColor="#FF9933"></asp:Label>Search
                    Receipt Entry
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <hr style="width: 100%; border-bottom: 1px #5081A1 solid;" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td nowrap="nowrap">
                    <asp:RadioButton ID="radReportFrom" runat="server"  GroupName="radReportOptions"
                        Text="From" CssClass="TDCaption" />
                    &nbsp;<asp:TextBox ID="txtReportFrom" runat="server" Width="80px" onkeypress="return false;"
                        onpaste="return false;" onchange="return SetUptoDate();" />
                    <asp:Image ID="imgReportFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender ID="txtReportFrom_CalendarExtender" runat="server" PopupButtonID="imgReportFrom"
                        Format="dd MMM yyyy" TargetControlID="txtReportFrom">
                    </cc1:CalendarExtender>
                    &nbsp; <span class="TDCaption">To</span><asp:TextBox ID="txtReportUpto" runat="server"
                        Width="80px" onkeypress="return false;" onpaste="return false;" />
                    <asp:Image ID="imgReportUpto" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender ID="txtReportUpto_CalendarExtender" runat="server" PopupButtonID="imgReportUpto"
                        Format="dd MMM yyyy" TargetControlID="txtReportUpto">
                    </cc1:CalendarExtender>
                    <asp:RadioButton ID="radReportMonthly" runat="server"  Checked="True" GroupName="radReportOptions"
                        Text="Monthly Report" CssClass="TDCaption" />
                    &nbsp;<asp:DropDownList ID="drpMonthList" runat="server">
                        <asp:ListItem Selected="True" Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February</asp:ListItem>
                        <asp:ListItem Value="3">March</asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">May</asp:ListItem>
                        <asp:ListItem Value="6">June</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="drpYearList" runat="server">
                    </asp:DropDownList>
                    Ledger Name :
                    <asp:DropDownList ID="drpLedger" runat="server" AppendDataBoundItems="true" DataSourceID="SDTExpenseLedgers"
                        DataTextField="LedgerName" DataValueField="LedgerName">
                        <asp:ListItem Text="All"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnShowReport" runat="server" Text="Show" OnClientClick="return checkEntry();"
                        OnClick="btnShowReport_Click" />
                    <asp:Button ID="btnReportRefresh" runat="server" Text="Reset" OnClick="btnReportRefresh_Click" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <div class="DivStyleWithScroll" style="width: 100%; overflow: scroll; height: 250px;">
                        <asp:GridView ID="grdSearchResult" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                            Visible="True" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                            ShowFooter="true" DataSourceID="SqlGridSource" AllowSorting="true" OnSorted="grdSearchResult_OnSorted"
                            OnRowDataBound="grdSearchResult_RowDataBound">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="TransId" HeaderText="Trans Id" InsertVisible="False" ReadOnly="True" />
                                <asp:BoundField DataField="Expense_Date" HeaderText="Date" SortExpression="Expense_Date"
                                    ReadOnly="true" />
                                <asp:BoundField DataField="Particulars" HeaderText="Narration" ReadOnly="True" SortExpression="Particulars" />
                                <asp:TemplateField HeaderText="Received From">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountType" runat="server" Text='<%#Eval("AccountType") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbltxtTotal" runat="server" Text="Total" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnExport" runat="server" Height="20px" Text="Export to Excel" OnClick="btnExport_Click"
                        Visible="False" />
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:SqlDataSource ID="SDTExpenseLedgers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT DISTINCT [LedgerName] FROM [LedgerMaster] WHERE (([LedgerName] NOT LIKE '%' + @LedgerName + '%') AND ([LedgerName] NOT LIKE '%' + @LedgerName2 + '%'))">
        <SelectParameters>
            <asp:Parameter DefaultValue="CASH" Name="LedgerName" Type="String" />
            <asp:Parameter DefaultValue="CUST" Name="LedgerName2" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlGridSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"></asp:SqlDataSource>
    <asp:Label ID="lblUpdateId" runat="server"></asp:Label>
</asp:Content>