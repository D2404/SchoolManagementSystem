using InvoiceManagementSystem.IRepository;
using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Repository
{
    public class ExamMarksRepository : IExamMarksRepository
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ExamModel GetAllExamMarks(ExamModel model)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<ExamModel> lstExamMarksList = new List<ExamModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetExamMarksList", conn);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", model.PageIndex);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@intActive", model.intActive);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        ExamModel obj = new ExamModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"] == null || dt.Rows[i]["IsActive"].ToString().Trim() == "" ? null : dt.Rows[i]["IsActive"].ToString());
                        obj.SubjectId = Convert.ToInt32(dt.Rows[i]["SubjectId"] == null || dt.Rows[i]["SubjectId"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        obj.SubjectName = dt.Rows[i]["SubjectName"] == null || dt.Rows[i]["SubjectName"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectName"].ToString();
                        obj.Grade = dt.Rows[i]["Grade"] == null || dt.Rows[i]["Grade"].ToString().Trim() == "" ? null : dt.Rows[i]["Grade"].ToString();
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.TotalMarks = Convert.ToInt32(dt.Rows[i]["TotalMarks"] == null || dt.Rows[i]["TotalMarks"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalMarks"].ToString());
                        obj.OutOfMarks = Convert.ToInt32(dt.Rows[i]["OutOfMarks"] == null || dt.Rows[i]["OutOfMarks"].ToString().Trim() == "" ? null : dt.Rows[i]["OutOfMarks"].ToString());
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstExamMarksList.Add(obj);
                    }
                }
                model.LSTExamList = lstExamMarksList;
                if (model.LSTExamList.Count > 0)
                {
                    var pager = new Models.Pager((int)model.LSTExamList[0].TotalRecord, model.PageIndex, (int)model.PageSize);

                    model.Pager = pager;
                }
                model.TotalEntries = TotalEntries;
                model.ShowingEntries = showingEntries;
                model.fromEntries = startentries;

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ExamModel AddExamMarks(ExamModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateExamMarks", conn);
                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.AddWithValue("@ClassId", cls.ClassId);
                cmd.Parameters.AddWithValue("@SubjectId", cls.SubjectId);
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);
                cmd.Parameters.AddWithValue("@TotalMarks", cls.TotalMarks);
                cmd.Parameters.AddWithValue("@OutOfMarks", cls.OutOfMarks);
                cmd.Parameters.AddWithValue("@RollNo", cls.RollNo);
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

        public ExamModel GetSingleExamMarks(ExamModel cls)
        {
            try
            {
                List<ExamModel> LSTList = new List<ExamModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleExamMarks", conn);
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
                        ExamModel obj = new ExamModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"] == null || dt.Rows[i]["IsActive"].ToString().Trim() == "" ? null : dt.Rows[i]["IsActive"].ToString());
                        obj.SubjectId = Convert.ToInt32(dt.Rows[i]["SubjectId"] == null || dt.Rows[i]["SubjectId"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        obj.SubjectName = dt.Rows[i]["SubjectName"] == null || dt.Rows[i]["SubjectName"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectName"].ToString();
                        obj.Grade = dt.Rows[i]["Grade"] == null || dt.Rows[i]["Grade"].ToString().Trim() == "" ? null : dt.Rows[i]["Grade"].ToString();
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.TotalMarks = Convert.ToInt32(dt.Rows[i]["TotalMarks"] == null || dt.Rows[i]["TotalMarks"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalMarks"].ToString());
                        obj.OutOfMarks = Convert.ToInt32(dt.Rows[i]["OutOfMarks"] == null || dt.Rows[i]["OutOfMarks"].ToString().Trim() == "" ? null : dt.Rows[i]["OutOfMarks"].ToString());
                        LSTList.Add(obj);
                    }
                }
                cls.LSTExamList = LSTList;
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


        public ExamModel DeleteExamMarks(ExamModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteRecord", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Mode", 10);
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

        public string UpdateStatus(ExamModel cls)
        {
            var Status = "";
            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateExamMarksStatus", conn);
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

        public DataTable ExportExamMarks(ExamModel cls)
        {
            try
            {
                List<ExamModel> LSTList = new List<ExamModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 1);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@intActive", cls.intActive);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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
                        dr["ClassNo"] = Regex.Replace(dr["ClassNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["AcademicYear"] = Regex.Replace(dr["AcademicYear"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
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