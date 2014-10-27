<%@ Page Language="C#" MasterPageFile="~/StoreMain.master" AutoEventWireup="true"
    ClientIDMode="Static" Inherits="Reports_InvoiceNoPrint" CodeBehind="InvoiceNoPrint.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-primary well-sm-tiny1">
        <div class="panel-heading">
            <h3 class="panel-title">
                Print Barcode</h3>
        </div>
        <div class="panel-body">
            <div class="row-fluid  well well-sm-tiny">
                <div class="col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon IconBkColor">Invoice No</span>
                        <asp:DropDownList ID="ddlinvoice" runat="server" BackColor="White" CssClass="form-control"
                            EnableTheming="false" OnSelectedIndexChanged="ddlinvoice_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlno" runat="server" BackColor="White" CssClass="form-control"
                        EnableTheming="false">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnsubmit" EnableTheming="false" CssClass="btn btn-primary btn-block"
                        Text="Preview" runat="server" OnClick="btnsubmit_Click" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnPrint" EnableTheming="false" CssClass="btn btn-primary btn-block"
                        Text="Print" runat="server" OnClick="btnPrint_Click" ClientIDMode="Static" />
                </div>
            </div>
            <div class="BorderTopLine div-margin2">
            </div>
            <div class="row-fluid div-margin">
                <div style="width: 100%; overflow: auto; height: 300px;">
                    <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="false" EnableTheming="false"
                        CssClass="table table-striped table-bordered table-hover" EmptyDataText="No record found"
                        Font-Size="13px">
                        <Columns>
                            <asp:TemplateField HeaderStyle-BackColor="#E4E4E4">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#E4E4E4">
                                <HeaderTemplate>
                                    <asp:Label ID="Label1" runat="server" Text="S No"></asp:Label></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("RowIndex") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BarCode" HeaderText="BarCode" HeaderStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="Item" HeaderText="Garment" HeaderStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="ItemRemarks" HeaderText="Remark" HeaderStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="Colour" HeaderText="Color" HeaderStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="Process" HeaderText="Service" HeaderStyle-BackColor="#E4E4E4" />
                            <asp:BoundField DataField="CustomerDetails" HeaderText="Customer" HeaderStyle-BackColor="#E4E4E4" />
                        </Columns>
                        <HeaderStyle Font-Size="12px" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnPrintingFromGrid" runat="server" Value="false" />
    <script type="text/javascript">

        $(function () {

            $('#chkSelectAll').on('change', function (e) {
                var allCheckBoxes = $(this).closest('table').find('[type="checkbox"]').not(':first');
                if (e.target.checked) {
                    allCheckBoxes.attr('checked', true);
                    setValueAndAttribute('ddlno', 0, 'disabled', true);
                }
                else {
                    allCheckBoxes.attr('checked', false);
                    setValueAndAttribute('ddlno', 0, 'disabled', false);
                }
            });

            $('#grdReport').on('change', '[type="checkbox"] :not(:first)', function (e) {
                var checkedLength = $(e.delegateTarget).find(':checked').length;
                if (checkedLength) {
                    setValueAndAttribute('ddlno', 0, 'disabled', true);
                    chkSelectAll.checked = $('#grdReport').find(':checkbox').not($('#chkSelectAll')).length === $('#grdReport').find(':checked').not($('#chkSelectAll')).length;
                }
                else {
                    setValueAndAttribute('ddlno', 0, 'disabled', false);
                    chkSelectAll.checked = false;
                }
            });

            function setValueAndAttribute(elementId, elementValue, attribute, attributeValue) {
                $('#' + elementId).val(elementValue).attr(attribute, attributeValue);
            }

            $('#btnsubmit, #btnPrint').click(function (e) {
                if ($('#ddlno').attr('disabled')) {
                    $('#hdnPrintingFromGrid').val('true');
                }
                else {
                    $('#hdnPrintingFromGrid').val('false');
                }
            });

        });
    
    </script>
</asp:Content>
