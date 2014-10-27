using System;

namespace DTO
{
    public class CommonFields
    {
        private int _Active;
        private DateTime _DateModified;
        private DateTime _DateCreated;
        private int _CreatedBy;
        private int _ModifiedBy;
        private string _BranchId;

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string BranchId
        {
            get { return this._BranchId; }
            set { this._BranchId = value; }
        }

        public DateTime DateModified
        {
            get { return this._DateModified; }
            set { this._DateModified = value; }
        }

        public DateTime DateCreated
        {
            get { return this._DateCreated; }
            set { this._DateCreated = value; }
        }

        public int CreatedBy
        {
            get { return this._CreatedBy; }
            set { this._CreatedBy = value; }
        }

        public int ModifiedBy
        {
            get { return this._ModifiedBy; }
            set { this._ModifiedBy = value; }
        }
    }
}