namespace DTO
{
    public class Item
    {
        private string _ItemName;
        private string _NoOfItem;
        private string _ItemCode;
        private string _BranchId;
        private string _ExternalBranchId;
        private string _SubItemName;
        private string _SubItemOrder;
        private string _ID;
        private string _OldItemName;
        private string _ItemImage;
        private int _Active;
        private string _VariationId;
        private string _SubItemRefID;
        private string _CategoryID;

        public string VariationId
        {
            get { return this._VariationId; }
            set { this._VariationId = value; }
        }

        public string SubItemRefID
        {
            get { return this._SubItemRefID; }
            set { this._SubItemRefID = value; }
        }

        public string CategoryID
        {
            get { return this._CategoryID; }
            set { this._CategoryID = value; }
        }

        public string ItemImage
        {
            get { return this._ItemImage; }
            set { this._ItemImage = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string ID
        {
            get { return this._ID; }
            set { this._ID = value; }
        }

        public string OldItemName
        {
            get { return this._OldItemName; }
            set { this._OldItemName = value; }
        }

        public string SubItemOrder
        {
            get { return this._SubItemOrder; }
            set { this._SubItemOrder = value; }
        }

        public string ItemName
        {
            get { return this._ItemName; }
            set { this._ItemName = value; }
        }

        public string NoOfItem
        {
            get { return this._NoOfItem; }
            set { this._NoOfItem = value; }
        }

        public string ItemCode
        {
            get { return this._ItemCode; }
            set { this._ItemCode = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public string ExternalBranchId
        {
            get { return this._ExternalBranchId; }
            set { this._ExternalBranchId = value; }
        }

        public string SubItemName
        {
            get { return this._SubItemName; }
            set { this._SubItemName = value; }
        }

        public string DefaultRate { get; set; }

        public string DefaultPrc { get; set; }
    }
}