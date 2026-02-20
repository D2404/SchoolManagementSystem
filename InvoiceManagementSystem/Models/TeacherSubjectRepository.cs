using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class TeacherSubjectRepository
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public TeacherSubjectModel GetAllTeacherSubject(TeacherSubjectModel model)
        {
            try
            {

                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<TeacherSubjectModel> lstTeacherSubjectList = new List<TeacherSubjectModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetTeacherSubjectList", conn);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", model.PageIndex);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@SubjectId", model.SubjectId);
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@intActive", model.intActive);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@AcademicYear", objCommon.getAcademicYearFromSession());
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
                        TeacherSubjectModel obj = new TeacherSubjectModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"] == null || dt.Rows[i]["IsActive"].ToString().Trim() == "" ? null : dt.Rows[i]["IsActive"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.SubjectName = dt.Rows[i]["SubjectName"] == null || dt.Rows[i]["SubjectName"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectName"].ToString();
                        obj.TeacherName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstTeacherSubjectList.Add(obj);
                    }
                }
                model.LSTTeacherSubjectList = lstTeacherSubjectList;
                if (model.LSTTeacherSubjectList.Count > 0)
                {
                    var pager = new Models.Pager((int)model.LSTTeacherSubjectList[0].TotalRecord, model.PageIndex, (int)model.PageSize);

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
        public TeacherSubjectModel AddTeacherSubject(TeacherSubjectModel model)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateTeacherSubject", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id); 
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@TeacherId", model.TeacherId);
                cmd.Parameters.AddWithValue("@SubjectId", model.SubjectId);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@AcademicYear", objCommon.getAcademicYearFromSession());


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
                        model.Response = "Success";
                    }
                    else if (intRefId == "2")
                    {
                        model.Response = "Updated";
                    }
                    else if (intRefId == "-1")
                    {
                        model.Response = "Exists";
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

            return model;
        }

        public TeacherSubjectModel GetSingleTeacherSubject(TeacherSubjectModel model)
        {
            try
            {
                List<TeacherSubjectModel> LSTList = new List<TeacherSubjectModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleTeacherSubject", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        TeacherSubjectModel obj = new TeacherSubjectModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());

                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.SubjectId = Convert.ToInt32(dt.Rows[i]["SubjectId"] == null || dt.Rows[i]["SubjectId"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectId"].ToString());
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        LSTList.Add(obj);
                    }
                }
                model.LSTTeacherSubjectList = LSTList;
                return model;
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


        public TeacherSubjectModel DeleteTeacherSubject(TeacherSubjectModel model)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteRecord", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Mode", 5);
                cmd.Parameters.Add("@Id", model.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows[0][0].ToString() == "1")
                {
                    model.Response = "Success";
                }
                else if (dt.Rows[0][0].ToString() == "2")
                {
                    model.Response = "dependency";
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return model;
        }

        public string UpdateStatus(TeacherSubjectModel model)
        {
            var Status = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateTeacherSubjectStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
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

        public DataTable ExportTeacherSubject(TeacherSubjectModel model)
        {
            try
            {
                List<TeacherSubjectModel> LSTList = new List<TeacherSubjectModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 5);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@intActive", model.intActive);
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
                        dr["TeacherName"] = Regex.Replace(dr["TeacherName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["ClassNo"] = Regex.Replace(dr["ClassNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["SubjectName"] = Regex.Replace(dr["SubjectName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
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