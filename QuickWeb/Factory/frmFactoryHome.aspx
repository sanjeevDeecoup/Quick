<%@ Page Title="" Language="C#" MasterPageFile="~/Factory/Factory.Master" AutoEventWireup="true"
    CodeBehind="frmFactoryHome.aspx.cs" Inherits="QuickWeb.Factory.frmFactoryHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid"> 
        <div class="row-fluid">
            <div class="span3 offset1 form-signin2">
                <a href="WorkShopIn.aspx" class="btn btn-block btn-primary btn-lg active"  onclick="return checkAccessRights('Receive from Store')"><i class="fa fa-arrow-right icon-white">
                </i><b>&nbsp;Receive from Store (F1)</b></a>
            </div>
            <div class="span3 form-signin2">
                <a href="WorkshopOut.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Send to Store')"><i class="fa fa-arrow-left icon-white">
                </i><b>&nbsp;Send to Store (F2)</b></a>
            </div>
            <div class="span3  form-signin2">
                <a href="PrintNote.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Delivery Note')"><i class="fa fa-list icon-white">
                </i><b>&nbsp; Delivery Notes</b> </a>
            </div>
        </div>
        <div class="row-fluid">            
            <div class="span3 offset1 form-signin2">
                <a href="frmPackingSticker.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Packing Stickers')"><i class="fa fa-tags icon-white">
                </i><b>&nbsp;Packing Stickers</b></a>
            </div>
             <div class="span3  form-signin2">
                <a href="frmWorkshopMenuRights.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Menu Rights')" ><i class="fa fa-desktop  icon-white">
                </i><b>&nbsp;Menu Rights</b> </a>
            </div>

            <div class="span3  form-signin2">
                <a href="frmWorkshopUser.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('User Creation')" ><i class="fa fa-user  icon-white">
                </i><b>&nbsp;User Creation</b> </a>
            </div>
        </div>

         <div class="row-fluid">
             <div class="span3 offset1 form-signin2">
                <a href="frmSearchInvoiceBarcode.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Search Invoice')"><i class="fa fa-search  icon-white">
                </i><b>&nbsp;Search Invoice</b> </a>
            </div>
               <div class="span3  form-signin2">
                <a href="frmFactoryBoundToMachine.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Workshop Access Authentication')"><i class="fa fa-tags icon-white">
                </i><b>&nbsp;Access Authentication</b></a>
            </div>
             <div class="span3  form-signin2">
                <a href="frmWorkshopDetails.aspx" class="btn btn-block btn-primary btn-lg active" onclick="return checkAccessRights('Workshop Details')"><i class="fa fa-tags icon-white">
                </i><b>&nbsp;Workshop Details</b></a>
            </div>            
        </div>
    </div>
    <!-- Main Container ENDS  -->
    <!-- Le javascript
================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <!-- Right now it is referring to JQuery from Net I have to do it Conditionally so local can also work-->
    <script src="../JavaScript/jquery-latest.js" type="text/javascript"></script>
    <script src="js/bootstrap.js"></script>
    <script src="../js/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            $('body').keydown(function (e) {
                var _idx = window.location.href.lastIndexOf('/');
                var _newLoc = window.location.href.substr(0, _idx);
                if (e.which == 112) {
                    if (checkAccessRights('Receive from Store')) {
                        var _href = _newLoc + '/WorkshopIn.aspx';
                        window.location.href = _href;
                    }
                }
                else if (e.which == 117) {

                    if (checkAccessRights('Send to Store')) {
                        var _href = _newLoc + '/WorkshopOut.aspx';
                        window.location.href = _href;
                    }
                }
            });
        });
    </script>       
</asp:Content>