<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmItemWiseRateList.aspx.cs" Inherits="QuickWeb.Bookings_New.frmItemWiseRateList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>
        <%Response.Write(ConfigurationManager.AppSettings["AppTitle"]); %></title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />    
       <link href="../css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
     <link href="../css/bootstrap-extend.css" rel="stylesheet" type="text/css" />    
     <link href="../css/jqx.base.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqx.classic.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqx.energyblue.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../js/jqxcore.js" type="text/javascript"></script>
    <script src="../js/jqxbuttons.js" type="text/javascript"></script>
    <script src="../js/jqxdata.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.selection.js" type="text/javascript"></script>
    <script src="../js/jqxmenu.js" type="text/javascript"></script>
    <script src="../js/jqxscrollbar.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.edit.js" type="text/javascript"></script>
    <script src="../js/jqxnumberinput.js" type="text/javascript"></script>
    <script src="../js/jqxgrid.columnsresize.js" type="text/javascript"></script>   
      <style type="text/css">          
         .jqx-grid-column-header-energyblue
         {
             background-color:#428BCA;
             font-weight:bold;
             color:White;
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

</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:ScriptManager ID="ScriptManager1" runat="server" />

              <div class="panel panel-primary marginleftRight div-margin">
        <div class="panel-heading">
            <h3 class="panel-title">
               <asp:Label ID="lblRateListName" runat="server" Font-Bold="true" Font-Size="20px" ClientIDMode="Static"></asp:Label> 
               <span style="float:right;margin-top:-7px">
              <asp:LinkButton ID="btnExport" runat="server"   OnClick="btnExport_Click" EnableTheming="false" CssClass="btn btn-info" ><i class="fa fa-th"></i>&nbsp;Export To Excel</asp:LinkButton>
            <asp:LinkButton ID="btncancel" runat="server"  OnClick="btncancel_Click" EnableTheming="false" CssClass="btn btn-info" Visible="false"><i class="fa fa-times-circle-o"></i>&nbsp;Close</asp:LinkButton>
             <asp:LinkButton ID="btnSave" runat="server"    OnClick="BtnSaveClick"   ClientIDMode="Static" EnableTheming="false" CssClass="btn btn-info" ><i class="fa fa-floppy-o"></i>&nbsp;Save ( F8 )</asp:LinkButton> 

                        </span>
                        </h3>
        </div>
        <div class="panel-body">
             <div id="jqxgrid" style="margin-left:10px;" runat="server" clientidmode="Static"></div> 
        </div>
    </div>



                 
  
       <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnProcesses" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnItems" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnRates" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnForEdit" />
    <span id="spnColName" style="display:none" ><%=col%></span>        
    </div>
    </form>
 <script type="text/javascript">
     $(document).ready(function () {
         //Getting the source data with ajax GET request
         var Items = [];
         var cellclassname = function (row, column, value, data) {
             if (column == 'ITEMNAME' || column == 'ITEMCODE') {
                 return "colBackcolor";
             };
         };
         var beginedit = function (row, datafield) {
             if (datafield == 'ITEMNAME' || datafield == 'ITEMCODE') {
                 return false;
             };
         };

         var ValidateNo = function (cell, value) {
             if ((isNaN(Number(value))) == true || value < 0 || value === "") {
                 return { result: false, message: "Invalid Price Press ESC to Continue." };
             }
             return true;
         };

         var strColName = $('#spnColName').text();
         strColName = strColName.substring(1, strColName.length);
         var AllData = strColName.split(":");
         var dataFields = [];
         var dataColumns = [];
         for (var j = 0; j < AllData.length; j += 1) {
             var strColumn = AllData[j];
             var res = strColumn.substring(2, strColumn.length);
             res = res.replace("-", ".");
             dataFields.push({ name: AllData[j] });
             if (AllData[j] === 'ITEMCODE') {
                 dataColumns.push({ text: res, columntype: 'textbox', dataField: AllData[j], cellclassname: cellclassname, width: '8%', validation: ValidateNo, cellbeginedit: beginedit });
                // dataColumns.push({ text: AllData[j], columntype: 'textbox', dataField: AllData[j], cellclassname: cellclassname, width: '8%', validation: ValidateNo, cellbeginedit: beginedit });
             } else {
                // dataColumns.push({ text: AllData[j], columntype: 'textbox', dataField: AllData[j], cellclassname: cellclassname, validation: ValidateNo, cellbeginedit: beginedit });
                 dataColumns.push({ text: res, columntype: 'textbox', dataField: AllData[j], cellclassname: cellclassname, validation: ValidateNo, cellbeginedit: beginedit });
             }
         }

         source = {
             datatype: "xml",
             datafields: dataFields,
             async: false,
             record: 'Table',
             url: 'frmItemWiseRateList.aspx/GetRateListData'
         };
         var dataAdapter = new $.jqx.dataAdapter(source,
                { contentType: 'application/json; charset=utf-8' }
            );

         var columns = dataColumns;
         var IsEdit = $('#hdnForEdit').val();
         if (IsEdit == "false") {
             IsEdit = false;
         }
         $("#jqxgrid").jqxGrid({
             width: '98%',
             height: 527,
             source: dataAdapter,
             columnsresize: true,
             editable: IsEdit,
             altrows: true,
             theme: 'energyblue',
             selectionmode: 'multiplecellsadvanced',
             columns: columns
         });




     });
    </script>
    <script type="text/javascript">
        $(function () {
            $('#btnSave, #btncancel').click(function (e) {
                if (e.target.id === 'btnSave') {
                    makeRates();
                }
            });
            $('body').on('keydown.Attached', function (e) {
                if (e.which !== 119) return;
                $('#btnSave').click();
            });
            function checkInput(e) {
                if (isNaN(Number(e.target.value)) || e.target.value === '') {
                    $(e.target).css('background-color', 'yellow');
                    setTimeout(function () {
                        e.target.focus();
                    }, 10);
                }
                else {
                    $(this).css('background-color', '');
                }
                return false;
            }
        });
        function makeRates() {         
            var itemRates = [];

            var strColName1 = $('#spnColName').text();
            strColName1 = strColName1.substring(1, strColName1.length);
            var AllColName = strColName1.split(":");


            var AllRowdata = $('#jqxgrid').jqxGrid('getdatainformation');
            var RowCount = AllRowdata.rowscount;

            for (var row = 0; row < RowCount; row += 1) {
                items = '';
                for (var j = 0; j < AllColName.length; j += 1) {
                    if (j === 0) {
                        items += $("#jqxgrid").jqxGrid('getcellvalue', row, AllColName[j]);
                    }
                    else if (j === 1) {
                    }
                    else {
                        items += ':' + $("#jqxgrid").jqxGrid('getcellvalue', row, AllColName[j]);
                    }
                }
                itemRates.push(items);
            }
            hdnRates.value = itemRates;
        }         
    </script>
</body>
</html>
