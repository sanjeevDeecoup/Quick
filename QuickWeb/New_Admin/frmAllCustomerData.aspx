<%@ Page Title="" Language="C#" MasterPageFile="~/StoreMain.Master" AutoEventWireup="true"
    CodeBehind="frmAllCustomerData.aspx.cs" Inherits="QuickWeb.New_Admin.frmAllCustomerData"
    EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/jqx.base.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqx.classic.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqx.energyblue.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jqx-grid-column-header-energyblue
        {
            background-color: gray;
            color: White;
            font-weight: bold;
        }
        .jqx-grid-cell-alt
        {
            background-color: #F0F4FB;
        }
        .colBackcolor
        {
            background-color: #F2F2F2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-primary marginleftRight">
        <div class="panel-heading">
            <h3 class="panel-title">
               All Customer Information<a class="btn btn-info margin4 " runat="server" style="float:right;margin-top:-7px" clientidmode="Static" id="btnExportExcel">
                        <i class="fa fa-th"></i>&nbsp;&nbsp;Export to excel </a>
                        </h3>
        </div>
        <div class="panel-body">
            <asp:Label ID="lblErr" runat="server" EnableViewState="False" Font-Bold="True" CssClass="ErrorMessage"
                ForeColor="White" ClientIDMode="Static" />           
            <div class="row-fluid div-margin">
                <div id="jqxgrid" runat="server" clientidmode="Static">
                </div>
            </div>
        </div>
    </div>
    <script src="../JavaScript/javascript.js" type="text/javascript"></script>
    <script src="../JavaScript/code.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/jqxcore.js" type="text/javascript"></script>
    <script src="../js/jqxbuttons.js" type="text/javascript"></script>
    <script src="../js/jqxdata.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.selection.js" type="text/javascript"></script>
    <script src="../js/jqxmenu.js" type="text/javascript"></script>
    <script src="../js/jqxscrollbar.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.columnsresize.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {


            source = {
                datatype: "xml",
                datafields: [
                { name: 'Code' },
                { name: 'Name' },
                { name: 'Address' },
                { name: 'AreaLocation' },
                { name: 'Mobile' },
                { name: 'EmailId' },
                 { name: 'CommunicationPrefrence' },
                 { name: 'Discount' },
                 { name: 'Ratelist' }
                ],
                async: false,
                record: 'Table',
                url: 'frmAllCustomerData.aspx/GetAllCustomer'
            };
            var dataAdapter = new $.jqx.dataAdapter(source,
                { contentType: 'application/json; charset=utf-8' }
            );

            var columns = [{ text: 'Code', columntype: 'textbox', dataField: 'Code', width: 80 },
                    { text: 'Name', columntype: 'textbox', dataField: 'Name', width: 150 },
                    { text: 'Address', columntype: 'textbox', dataField: 'Address', width: 180 },
                     { text: 'Area Location', columntype: 'numberinput', dataField: 'AreaLocation', width: 150 },
                     { text: 'Mobile', columntype: 'textbox', dataField: 'Mobile', width: 120 },
                     { text: 'Email', columntype: 'textbox', dataField: 'EmailId', width: 150 },
                     { text: 'Communication Prefrence', columntype: 'textbox', dataField: 'CommunicationPrefrence', width: 200 },
                     { text: 'Discount', columntype: 'textbox', dataField: 'Discount', width: 70 },
                     { text: 'Rate list', columntype: 'textbox', dataField: 'Ratelist', width: 100 }
                                      ];

            $("#jqxgrid").jqxGrid({
                width: '100%',
                height: 430,
                source: dataAdapter,
                columnsresize: true,
                altrows: true,
                theme: 'energyblue',
                selectionmode: 'singlecell',
                columns: columns
            });


            $("#btnExportExcel").click(function (e) {
                var data1 = $('#hdnAllCustData').val();
                var clickedId = $(this).attr("id");
                if (clickedId == 'btnExportExcel') {
                    __doPostBack('ctl00$ContentPlaceHolder1$btnExportExcel', null);
                }
                e.preventDefault();
            });

        });
        
    </script>
</asp:Content>
