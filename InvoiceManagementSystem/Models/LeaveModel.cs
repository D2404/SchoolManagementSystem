using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class LeaveModel
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public decimal NoOfDays { get; set; }
        public int LeaveType { get; set; }
        public string LeaveTypeName { get; set; }
        public int LeaveSubType { get; set; }
        public string LeaveSubTypeName { get; set; }
        public string Reason { get; set; }
        public string Time { get; set; }
        public string ClassNo { get; set; }
        public string TeacherName { get; set; }
        public string StudentName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RollNo { get; set; }
        public string Profile { get; set; }
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public int intActive { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Response { get; set; }
        public string SearchText { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecord { get; set; }
        public decimal? PageCount { get; set; }
        public long? ROWNUMBER { get; set; }
        public int TotalEntries { get; set; }
        public int ShowingEntries { get; set; }
        public int fromEntries { get; set; }
        public Pager Pager { get; set; }

        public List<LeaveModel> LSTLeaveList { get; set; }



        public LeaveModel GetClassRoom(LeaveModel cls)
        {
            try
            {
                List<LeaveModel> LSTList = new List<LeaveModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetSingleLeave", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        LeaveModel obj = new LeaveModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Status = Convert.ToInt32(dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString());
                        obj.NoOfDays = Convert.ToDecimal(dt.Rows[i]["NoOfDays"] == null || dt.Rows[i]["NoOfDays"].ToString().Trim() == "" ? null : dt.Rows[i]["NoOfDays"].ToString());
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        obj.FromDate = dt.Rows[i]["FromDate"] == null || dt.Rows[i]["FromDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("dd/MM/yyyy");
                        obj.ToDate = dt.Rows[i]["ToDate"] == null || dt.Rows[i]["ToDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("dd/MM/yyyy");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTLeaveList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw ex;
            }
        }
        public LeaveModel addLeave(LeaveModel cls)
        {
            int count = 0;
            try
            {
                var StudentId = objCommon.getStudentIdFromSession();
                conn.Open();
                if (StudentId == 0)
                {
                    SqlCommand cmd = new SqlCommand("AddUpdateTeacherLeave", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = cls.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = cls.ToDate;
                    cmd.Parameters.Add("@NoOfDays", SqlDbType.Decimal).Value = cls.NoOfDays;
                    cmd.Parameters.Add("@LeaveType", SqlDbType.NVarChar).Value = cls.LeaveType;
                    cmd.Parameters.Add("@LeaveSubType", SqlDbType.NVarChar).Value = cls.LeaveSubType;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = cls.Reason;
                    cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                    cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 0;
                    da.ReturnProviderSpecificTypes = true;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        string intRefId = dt.Rows[0][0].ToString();
                        if (intRefId == "1")
                        {
                            cls.Response = "Success";
                        }
                        else if (intRefId == "2")
                        {
                            cls.Response = "Updated";
                        }
                        else if (intRefId == "-1")
                        {
                            cls.Response = "Exists";
                        }
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("AddUpdateStudentLeave", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = cls.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = cls.ToDate;
                    cmd.Parameters.Add("@NoOfDays", SqlDbType.Decimal).Value = cls.NoOfDays;
                    cmd.Parameters.Add("@LeaveType", SqlDbType.NVarChar).Value = cls.LeaveType;
                    cmd.Parameters.Add("@LeaveSubType", SqlDbType.NVarChar).Value = cls.LeaveSubType;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = cls.Reason;
                    cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                    cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                    cmd.Parameters.AddWithValue("@StudentId", objCommon.getStudentIdFromSession());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 0;
                    da.ReturnProviderSpecificTypes = true;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        string intRefId = dt.Rows[0][0].ToString();
                        if (intRefId == "1")
                        {
                            cls.Response = "Success";
                        }
                        else if (intRefId == "2")
                        {
                            cls.Response = "Updated";
                        }
                        else if (intRefId == "-1")
                        {
                            cls.Response = "Exists";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return cls;
        }
        public LeaveModel deleteLeave(LeaveModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteLeave", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", cls.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows[0][0].ToString() == "1")
                {
                    cls.Response = "Success";
                }
                else if (dt.Rows[0][0].ToString() == "2")
                {
                    cls.Response = "dependency";
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return cls;
        }

        public string ApproveStatus(LeaveModel cls)
        {
            var Status = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_ApproveLeaveStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                //cmd.Parameters.Add("@intLoginUser", SqlDbType.Int).Value = LoginUser;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                conn.Close();
                Status = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
                Status = "error";
            }
            return Status;
        }

        public string RejectStatus(LeaveModel cls)
        {
            var Status = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_RejectLeaveStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                //cmd.Parameters.Add("@intLoginUser", SqlDbType.Int).Value = LoginUser;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                conn.Close();
                Status = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
                Status = "error";
            }
            return Status;
        }

        public LeaveModel LeaveData(LeaveModel cls)
        {
            try
            {
                List<LeaveModel> LSTList = new List<LeaveModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetLeaveData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        LeaveModel obj = new LeaveModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.NoOfDays = Convert.ToInt32(dt.Rows[i]["NoOfDays"] == null || dt.Rows[i]["NoOfDays"].ToString().Trim() == "" ? null : dt.Rows[i]["NoOfDays"].ToString());
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        obj.Profile = dt.Rows[i]["Profile"] == null || dt.Rows[i]["Profile"].ToString().Trim() == "" ? null : dt.Rows[i]["Profile"].ToString();
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTLeaveList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw ex;
            }
        }

        public LeaveModel LeaveMail(LeaveModel cls)
        {
            try
            {
                List<LeaveModel> lstStudentList = new List<LeaveModel>();
                SqlCommand cmd = new SqlCommand("LeaveMail", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", cls.Email);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                LeaveModel obj = new LeaveModel();
                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Id = Convert.ToInt32(dt.Rows[0]["Id"] == null || dt.Rows[0]["Id"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Id"].ToString());
                    obj.UserName = dt.Rows[0]["FullName"] == null || dt.Rows[0]["FullName"].ToString().Trim() == "" ? "" : dt.Rows[0]["FullName"].ToString();
                    obj.LeaveTypeName = dt.Rows[0]["LeaveTypeName"] == null || dt.Rows[0]["LeaveTypeName"].ToString().Trim() == "" ? "" : dt.Rows[0]["LeaveTypeName"].ToString();
                    obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                    obj.LeaveType = Convert.ToInt32(dt.Rows[0]["LeaveType"] == null || dt.Rows[0]["LeaveType"].ToString().Trim() == "" ? "" : dt.Rows[0]["LeaveType"].ToString());
                    obj.FromDate = dt.Rows[0]["FromDate"] == null || dt.Rows[0]["FromDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[0]["FromDate"]).ToString("yyyy/MM/dd");
                    obj.ToDate = dt.Rows[0]["ToDate"] == null || dt.Rows[0]["ToDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[0]["ToDate"]).ToString("yyyy/MM/dd");
                    obj.Reason = dt.Rows[0]["Reason"] == null || dt.Rows[0]["Reason"].ToString().Trim() == "" ? "" : dt.Rows[0]["Reason"].ToString();
                    obj.Response = "Success";
                    lstStudentList.Add(obj);
                }
                else
                {
                    obj.Response = "Error";
                }
                cls.LSTLeaveList = lstStudentList;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return cls;
        }
    }
}