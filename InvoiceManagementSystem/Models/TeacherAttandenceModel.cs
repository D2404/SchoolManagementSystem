using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class TeacherAttandenceModel
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public int MonthId { get; set; }

        public string strerrorMessage { get; set; }
        public HttpPostedFileBase[] file { get; set; }
        public int YearId { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public string TotalPresentDays { get; set; }
        public string TotalAbsentDays { get; set; }
        public string FatherName { get; set; }
        public string SurName { get; set; }
        public int LeaveType { get; set; }
        public string Reason { get; set; }
        public string TeacherId { get; set; }
        public string Status { get; set; }
        public int intActive { get; set; }
        
        public string Date { get; set; }
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
        public string CreatedDate { get; set; }
        public int ShowingEntries { get; set; }
        public int fromEntries { get; set; }
        public Pager Pager { get; set; }

        public List<TeacherAttandenceModel> LSTTeacherAttandenceList { get; set; }
        public TeacherAttandenceModel addTeacherAttandence(TeacherAttandenceModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateTeacherAttandence", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeacherId", cls.TeacherId);
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = cls.Date;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = cls.Status;
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());

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
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return cls;
        }
        public TeacherAttandenceModel GetTeacherAttandence(TeacherAttandenceModel cls)
        {
            try
            {
                List<TeacherAttandenceModel> LSTList = new List<TeacherAttandenceModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleTeacherAttandence", conn);
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
                        TeacherAttandenceModel obj = new TeacherAttandenceModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.TeacherId = dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString();
                        obj.Status = dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString();
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("yyyy/MM/dd");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTTeacherAttandenceList = LSTList;
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
        public TeacherAttandenceModel deleteTeacherAttandence(TeacherAttandenceModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteTeacherAttandence", conn);
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
        public DataTable ExportTeacherAttendance(TeacherAttandenceModel cls)
        {
            try
            {
                List<TeacherAttandenceModel> LSTList = new List<TeacherAttandenceModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 3);
                //cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                //cmd.Parameters.AddWithValue("@intActive", cls.intActive);
                cmd.Parameters.AddWithValue("@Date", cls.Date);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@TeacherId", cls.TeacherId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        // HTML Tags Code Remove.
                        dr["TeacherName"] = Regex.Replace(dr["TeacherName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["ClassNo"] = Regex.Replace(dr["ClassNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Status"] = Regex.Replace(dr["Status"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Date"] = Regex.Replace(dr["Date"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        //  dr["Date"] = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                    }
                }

                return dt;
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
    }

}