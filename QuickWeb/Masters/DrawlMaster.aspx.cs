using System;
using System.Data;
using System.Data.SqlClient;

public partial class DrawlMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnAddNew_Click(null, null);
            drpDrawl.Focus();
            ShowGrid("");
            BindDropDown();
        }
    }

    private void BindDropDown()
    {
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Flag", 7);
        ds = AppClass.GetData(cmd);
        drpDrawl.DataSource = ds;
        drpDrawl.DataTextField = "Drawl";
        drpDrawl.DataValueField = "Id";
        drpDrawl.DataBind();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        EnableModification(true);
        RefreshForm();
        btnSave.Text = "Save";
        btnSave.Enabled = true;
        lblSaveOption.Text = "1";
        btnEdit.Enabled = false;
        drpDrawl.Focus();
        btnDelete.Enabled = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool success = false;
        if (txtParent.Text == "")
        {
            lblErr.Text = "Please enter parent drawl.";
            return;
        }
        try
        {
            string sql = string.Empty, Msg = string.Empty, res = string.Empty;
            if (lblSaveOption.Text == "1")
            {
                if (CheckDuplicateRecord(drpDrawl.SelectedItem.Text, txtParent.Text) != true)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_Dry_DrawlMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", 1);
                    cmd.Parameters.AddWithValue("@DrawlName", drpDrawl.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@ParentDrawl", txtParent.Text);
                    res = AppClass.ExecuteNonQuery(cmd);
                    Msg = "Record Saved.";
                }
                else
                    Msg = "Record already exist.";
            }
            else if (lblSaveOption.Text == "2")
            {
                if (CheckDuplicateRecord(drpDrawl.SelectedItem.Text, txtParent.Text) != true)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_Dry_DrawlMaster";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", hdnSelectedProcessCode.Value);
                    cmd.Parameters.AddWithValue("@DrawlName", drpDrawl.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@ParentDrawl", txtParent.Text);
                    cmd.Parameters.AddWithValue("@Flag", 2);
                    res = AppClass.ExecuteNonQuery(cmd);
                    Msg = "Record Updated.";
                }
                else
                    Msg = "Record already exist.";
            }
            lblMsg.Text = Msg;
            success = true;
        }
        catch (Exception excp)
        {
            lblErr.Text = excp.ToString();
        }
        finally
        {
        }
        if (success)
        {
            RefreshForm();
            if (Request.QueryString["FNB"] != null && Request.QueryString["FNB"] == "1")
            {
                Response.Redirect("~/Bookings/NewBooking.aspx", false);
            }
            else
            {
                btnAddNew_Click(null, null);
                ShowGrid("");
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        EnableModification(true);
        btnSave.Text = "Update";
        lblSaveOption.Text = "2";
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        txtParent.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ShowGrid(drpDrawl.SelectedItem.Text);
    }

    private void ShowGrid(string strProcessNameLike)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DrawlName", strProcessNameLike);
        if (strProcessNameLike == "")
            cmd.Parameters.AddWithValue("@Flag", 3);
        else
            cmd.Parameters.AddWithValue("@Flag", 4);
        ds = AppClass.GetData(cmd);
        grdSearchResult.DataSource = ds;
        grdSearchResult.DataBind();
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        ShowGrid("");
    }

    protected void grdSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnSelectedProcessCode.Value = grdSearchResult.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
        GetProcessDetails(hdnSelectedProcessCode.Value);
    }

    public bool CheckDuplicateRecord(string drawlName, string parentNo)
    {
        bool status = false;
        SqlDataReader sdr = null;
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DrawlName", drawlName);
        cmd.Parameters.AddWithValue("@ParentDrawl", parentNo);
        cmd.Parameters.AddWithValue("@Flag", 6);
        sdr = AppClass.ExecuteReader(cmd);
        if (sdr.Read())
            status = true;
        else
            status = false;
        return status;
    }

    private void GetProcessDetails(string strProcessCode)
    {
        string Drawlname = "";
        SqlConnection sqlcon = new SqlConnection(PrjClass.sqlConStr);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM mstDrwal WHERE Id = '" + strProcessCode + "'";
        SqlDataReader sdr = null;
        try
        {
            sqlcon.Open();
            cmd.Connection = sqlcon;
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                hdnSelectedProcessCode.Value = "" + sdr.GetValue(0);
                Drawlname = "" + sdr.GetValue(1);
                PrjClass.SetItemInDropDown(drpDrawl, Drawlname, true, false);
                txtParent.Text = "" + sdr.GetValue(2);
            }
            sdr.Close();
            sdr = null;
            btnEdit.Enabled = true;
            EnableModification(false);
            btnDelete.Enabled = true;
        }
        catch (Exception excp)
        {
            lblErr.Text = "Error (GetProcessDetails()): " + excp.Message;
        }
        finally
        {
            if (sdr != null) { sdr.Close(); sdr.Dispose(); }
            sqlcon.Close();
            sqlcon.Dispose();
        }
    }

    protected void grdSearchResult_PageIndexChanged(object sender, EventArgs e)
    {
        grdSearchResult.Visible = true;
    }

    protected void grdSearchResult_OnSorted(object sender, EventArgs e)
    {
        grdSearchResult.Visible = true;
    }

    protected void EnableModification(bool NewSet)
    {
        txtParent.Enabled = NewSet;
        drpDrawl.Enabled = NewSet;
    }

    protected void RefreshForm()
    {
        txtParent.Text = "";
        hdnSelectedProcessCode.Value = "";
        ShowGrid("");
    }

    protected void ResetView()
    {
        RefreshForm();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_Dry_DrawlMaster";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ID", grdSearchResult.SelectedRow.Cells[1].Text.Replace("&nbsp;", ""));
        cmd.Parameters.AddWithValue("@Flag", 5);
        string res = AppClass.ExecuteNonQuery(cmd);
        if (res == "Record Saved")
            lblMsg.Text = "Record deleted";
        else
            lblErr.Text = res.ToString();
        ShowGrid("");
    }
}