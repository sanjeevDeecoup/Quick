namespace DTO
{
    public class Comment
    {
        private string _CommentID;
        private string _CommentName;
        private int _Active;
        private string _DateCreated;
        private string _DateModified;
        private string _BranchId;

        public string CommentID
        {
            get { return this._CommentID; }
            set { this._CommentID = value; }
        }

        public string CommentName
        {
            get { return this._CommentName; }
            set { this._CommentName = value; }
        }

        public int Active
        {
            get { return this._Active; }
            set { this._Active = value; }
        }

        public string DateCreated
        {
            get { return this._DateCreated; }
            set { this._DateCreated = value; }
        }

        public string DateModified
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