using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Globals file.
/// <remarks>Updated by Mac to allow multiple user and session, without changing a single line of code anywhere else in the project!</remarks>
/// </summary>
public static class Globals
{
    private static string _branchID;
    private static string _UserId;
    private static string _UserType;
    private static string _UserBranch;
    private static string _UserName;
    private static string _CustCode;
    private static string _StorePrint;
    private static ArrayList _date = new ArrayList();
    private static int _startCount = 0;
    private static Dictionary<string, int> _sessionWiseUserNumber = new Dictionary<string, int>();
    private static Dictionary<int, Dictionary<string, string>> _userProperties = new Dictionary<int, Dictionary<string, string>>();

    public static ArrayList date
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            var prop = _userProperties[userNumber]["date"];      // return her id
            var ary = prop.Split('$').ToArray();
            return (new ArrayList(ary));
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("StorePrint"))
                _userProperties[userNumber].Add("date", string.Join("$", value.ToArray()));
            else
                _userProperties[userNumber]["date"] = string.Join("$", value.ToArray());
        }
    }

    public static string StorePrint
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties[userNumber].Keys.Contains("StorePrint")) return "";
            return _userProperties[userNumber]["StorePrint"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("StorePrint"))
                _userProperties[userNumber].Add("StorePrint", value);
            else
                _userProperties[userNumber]["StorePrint"] = value;
        }
    }


    public static string CustCode
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties[userNumber].Keys.Contains("CustCode")) return "";
            return _userProperties[userNumber]["CustCode"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("CustCode"))
                _userProperties[userNumber].Add("CustCode", value);
            else
                _userProperties[userNumber]["CustCode"] = value;
        }
    }

    public static string BranchID
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["BranchID"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("BranchID"))
                _userProperties[userNumber].Add("BranchID", value);
            else
                _userProperties[userNumber]["BranchID"] = value;
        }
    }

    public static string UserId
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["UserId"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("UserId"))
                _userProperties[userNumber].Add("UserId", value);
            else
                _userProperties[userNumber]["UserId"] = value;
        }
    }

    public static string UserType
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["UserType"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("UserType"))
                _userProperties[userNumber].Add("UserType", value);
            else
                _userProperties[userNumber]["UserType"] = value;
        }
    }

    public static string WorkshopUserType
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["WorkshopUserType"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("WorkshopUserType"))
                _userProperties[userNumber].Add("WorkshopUserType", value);
            else
                _userProperties[userNumber]["WorkshopUserType"] = value;
        }
    }
    public static string StoreName
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["StoreName"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("StoreName"))
                _userProperties[userNumber].Add("StoreName", value);
            else
                _userProperties[userNumber]["StoreName"] = value;
        }
    }
    public static string UserBranch
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["UserBranch"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("UserBranch"))
                _userProperties[userNumber].Add("UserBranch", value);
            else
                _userProperties[userNumber]["UserBranch"] = value;
        }
    }

    public static string UserName
    {
        get
        {
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            return _userProperties[userNumber]["UserName"];     // return her id
        }
        set
        {
            // check to see, it this user (i.e. session exists in the current context)
            if (!SessionWiseUserNumber.Keys.Contains(HttpContext.Current.Session["UniqueIDPBU"].ToString()))
            {
                var dict = new System.Collections.Generic.Dictionary<string, int>();
                dict.Add(value.ToString(), 0);
                SessionWiseUserNumber = dict;
            }
            // now we have this session in the list, either added now, or before, doesn't matter
            var userNumber = _sessionWiseUserNumber[HttpContext.Current.Session["UniqueIDPBU"].ToString()];     // find the user number;
            if (!_userProperties.ContainsKey(userNumber)) _userProperties.Add(userNumber, new Dictionary<string, string>());
            if (!_userProperties[userNumber].Keys.Contains("UserName"))
                _userProperties[userNumber].Add("UserName", value);
            else
                _userProperties[userNumber]["UserName"] = value;
        }
    }

    public static string UniqueIDPBU
    {
        get;
        set;
    }

    public static Dictionary<string, int> SessionWiseUserNumber
    {
        get
        {
            return _sessionWiseUserNumber;
        }
        set
        {
            if (!_sessionWiseUserNumber.ContainsKey(value.Keys.FirstOrDefault()))
            {
                _sessionWiseUserNumber.Add(value.Keys.FirstOrDefault(), ++_startCount);
            }
        }
    }

    public static int StartCount
    {
        get { return _startCount; }
    }
}