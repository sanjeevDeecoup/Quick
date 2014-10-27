using System;
using System.Configuration;
using System.IO;

namespace QuickWeb.Masters
{
    public partial class ItemNewMaster : System.Web.UI.Page
    {
        private DTO.Common Common = new DTO.Common();
        private DTO.Item Item = new DTO.Item();
        private int row;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Request.Form["__EVENTTARGET"] == "btnSave")
                {
                    btnSave_ServerClick(null, null);
                }
            }
        }

        public DTO.Item SaveValue()
        {
            //Get item strings
            Item.ID = hdnItemID.Value;
            Item.VariationId = hdnVariations.Value;
            Item.SubItemRefID = hdnSubItems.Value;
            Item.CategoryID = hdnCategories.Value;
            Item.BranchId = "1";
            Item.ItemCode = txtCode.Value;
            Item.ItemName = txtName.Value;
            string imageName = UploadImage();
            if (imageName == null)
            {
                imageName = hdnImageName.Value;
                imageName = imageName.Substring(imageName.LastIndexOf("/") + 1);
            }
            Item.ItemImage = imageName;
            return Item;
        }

        protected string UploadImage()
        {
            string fileName = null;
            //Save the file in the Images/Item/Temp folder
            string iconName = fileIcon.Value;
            if (iconName != null && iconName != "")
            {
                string itemImagesFolder = ConfigurationManager.AppSettings["itemImageFolder"].ToString();
                fileName = iconName.Substring(iconName.LastIndexOf("\\") + 1);
                if (File.Exists(itemImagesFolder + fileName))
                {
                    File.Delete(itemImagesFolder + fileName);
                }
                fileIcon.PostedFile.SaveAs(itemImagesFolder + fileName);
            }
            return fileName;
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            bool success = false;
            Item = SaveValue();
            if (Item.ID == "0" || Item.ID == "")
            {
                Common.Result = BAL.BALFactory.Instance.BAL_Item.SaveNewItem(Item);
            }
            else
            {
                Common.Result = BAL.BALFactory.Instance.BAL_Item.UpdateNewItem(Item);
            }
            success = true;
            if (success)
            {
                lblError.InnerText = "Item has been saved successfully.";
            }
        }

        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            bool success = false;
            Item = SaveValue();
            Common.Result = BAL.BALFactory.Instance.BAL_Item.DeleteNewItem(Item);
            success = true;
            if (success)
            {
                lblError.InnerText = "Item has been deleted.";
            }
        }
    }
}