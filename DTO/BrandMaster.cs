using System;

namespace DTO
{
    public class BrandMaster
    {
        private int _BrandID;
        private string _BrandName;
        private int _Active;
        private DateTime _DateCreated;
        private DateTime _DateModified;
        private string _BranchId;

        public int BrandID
        {
            get { return this._BrandID; }
            set { this._BrandID = value; }
        }

        public string BrandName
        {
            get { return this._BrandName; }
            set { this._BrandName = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public DateTime DateCreated
        {
            get { return this._DateCreated; }
            set { this._DateCreated = value; }
        }

        public DateTime DateModified
        {
            get { return this._DateModified; }
            set { this._DateModified = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }
    }
}