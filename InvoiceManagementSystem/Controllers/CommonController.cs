using ClosedXML.Excel;
using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class CommonController : Controller
    {
        clsCommon objCommon = new clsCommon();

        public ActionResult GetSchoolDetails(SchoolModel cls)
        {
            try
            {
                List<SchoolModel> lstSchoolList = new List<SchoolModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSchoolList", conn);
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
                        SchoolModel obj = new SchoolModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.PhotoImg = dt.Rows[i]["Photo"] == null ? "" : dt.Rows[i]["Photo"].ToString();
                        obj.SchoolName = dt.Rows[i]["SchoolName"] == null || dt.Rows[i]["SchoolName"].ToString().Trim() == "" ? null : dt.Rows[i]["SchoolName"].ToString();
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.Address = dt.Rows[i]["Address"] == null || dt.Rows[i]["Address"].ToString().Trim() == "" ? null : dt.Rows[i]["Address"].ToString();
                        obj.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.Since = dt.Rows[i]["Since"] == null || dt.Rows[i]["Since"].ToString().Trim() == "" ? null : dt.Rows[i]["Since"].ToString();

                        lstSchoolList.Add(obj);
                    }
                }
                cls.LSTSchoolList = lstSchoolList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult GetSchool(SchoolModel cls)
        {
            try
            {
                List<SchoolModel> lstClientList = new List<SchoolModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSchoolList", conn);
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
                        SchoolModel obj = new SchoolModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.SchoolName = dt.Rows[i]["SchoolName"] == null || dt.Rows[i]["SchoolName"].ToString().Trim() == "" ? null : dt.Rows[i]["SchoolName"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTSchoolList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public ActionResult GetRole(RoleModel cls)
        {
            try
            {
                List<RoleModel> lstClientList = new List<RoleModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetRoleList", conn);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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
                        RoleModel obj = new RoleModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.RoleName = dt.Rows[i]["RoleName"] == null || dt.Rows[i]["RoleName"].ToString().Trim() == "" ? null : dt.Rows[i]["RoleName"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTRoleList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult GetClassRoom(ClassRoomModel cls)
        {
            try
            {
                List<ClassRoomModel> lstClientList = new List<ClassRoomModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetClassRoomList", conn);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@intActive", 1);
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
                        ClassRoomModel obj = new ClassRoomModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTClassRoomList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult LoadSubject(int ClassId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon objCommon = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadSubjectDropDown", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.Add("@ClassId", ClassId);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<ExamModel> clsLst = new List<ExamModel>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ExamModel obj = new ExamModel();
                        obj.SubjectId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.SubjectName = dt.Rows[i]["SubjectName"] == null || dt.Rows[i]["SubjectName"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectName"].ToString();
                        clsLst.Add(obj);
                    }
                }
                return Json(clsLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAdmin(AdminModel cls)
        {
            try
            {
                List<AdminModel> lstClientList = new List<AdminModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("LoadAdminDropdown", conn);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                //cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
                        AdminModel obj = new AdminModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTAdminList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult GetTeacher(TeacherModel cls)
        {
            try
            {
                List<TeacherModel> lstClientList = new List<TeacherModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("LoadTeacherDropdown", conn);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                //cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
                        TeacherModel obj = new TeacherModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTTeacherList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult LoadClassWiseStudent(int ClassId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon objCommon = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadClasswiseStudentDropDown", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ClassId", ClassId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<ExamModel> clsLst = new List<ExamModel>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ExamModel obj = new ExamModel();
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        clsLst.Add(obj);
                    }
                }
                return Json(clsLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadUser(int AdminId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon objCommon = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadUserDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.Add("@UserId", AdminId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<EmailConfigurationSetting> clsLst = new List<EmailConfigurationSetting>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.Username = dt.Rows[i]["UserName"] == null || dt.Rows[i]["UserName"].ToString().Trim() == "" ? null : dt.Rows[i]["UserName"].ToString();
                        obj.FromMail = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        clsLst.Add(obj);
                    }
                }
                return Json(clsLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadStudent(int TeacherId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon objCommon = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadStudentDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.Add("@TeacherId", TeacherId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<EmailConfigurationSetting> clsLst = new List<EmailConfigurationSetting>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.StudentName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        clsLst.Add(obj);
                    }
                }
                return Json(clsLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadTeacher(int TeacherId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon objCommon = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadTeacherDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.Add("@TeacherId", TeacherId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<EmailConfigurationSetting> clsLst = new List<EmailConfigurationSetting>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.Username = dt.Rows[i]["UserName"] == null || dt.Rows[i]["UserName"].ToString().Trim() == "" ? null : dt.Rows[i]["UserName"].ToString();
                        obj.FromMail = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        clsLst.Add(obj);
                    }
                }
                return Json(clsLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LoadStudentDetails(int StudentId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon objCommon = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadStudentDetailById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.Add("@StudentId", StudentId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<EmailConfigurationSetting> clsLst = new List<EmailConfigurationSetting>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.Username = dt.Rows[i]["UserName"] == null || dt.Rows[i]["UserName"].ToString().Trim() == "" ? null : dt.Rows[i]["UserName"].ToString();
                        obj.FromMail = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        clsLst.Add(obj);
                    }
                }
                return Json(clsLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}