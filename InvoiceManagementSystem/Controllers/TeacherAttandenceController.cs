using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace InvoiceManagementSystem.Controllers
{
    public class TeacherAttandenceController : Controller
    {
        clsCommon objCommon = new clsCommon();

        // GET: TeacherAttandence
        public ActionResult TeacherAttandence()
        {
            if (objCommon.getUserIdFromSession() != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult InsertTeacherAttandence(TeacherAttandenceModel model)
        {
            model = model.addTeacherAttandence(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTeacherAttandence(TeacherAttandenceModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<TeacherAttandenceModel> lstTeacherAttandenceList = new List<TeacherAttandenceModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetTeacherAttandenceList", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@intActive", cls.intActive); 
                cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
                        TeacherAttandenceModel obj = new TeacherAttandenceModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        obj.TeacherName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.Status = dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString();
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstTeacherAttandenceList.Add(obj);
                    }
                }
                cls.LSTTeacherAttandenceList = lstTeacherAttandenceList;
                if (cls.LSTTeacherAttandenceList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTTeacherAttandenceList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTTeacherAttandenceList = lstTeacherAttandenceList;

                return PartialView("_TeacherAttandenceListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetTeacherAttandenceee(TeacherAttandenceModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<TeacherAttandenceModel> lstTeacherAttandenceList = new List<TeacherAttandenceModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_FilterGetTeacherAttandenceList", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@Date",cls.Date);
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
                        TeacherAttandenceModel obj = new TeacherAttandenceModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        obj.TeacherName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.Status = dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString();
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstTeacherAttandenceList.Add(obj);
                    }
                }
                cls.LSTTeacherAttandenceList = lstTeacherAttandenceList;
                if (cls.LSTTeacherAttandenceList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTTeacherAttandenceList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTTeacherAttandenceList = lstTeacherAttandenceList;

                return PartialView("_FilterTeacherAttandenceListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSingleTeacherAttandenceData(TeacherAttandenceModel cls)
        {
            try
            {
                cls = cls.GetTeacherAttandence(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteTeacherAttandence(TeacherAttandenceModel cls)
        {
            try
            {
                cls = cls.deleteTeacherAttandence(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public ActionResult GetMonth(MonthModel cls)
        {
            try
            {
                List<MonthModel> lstClientList = new List<MonthModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetMonthList", conn);
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
                        MonthModel obj = new MonthModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.MonthName = dt.Rows[i]["MonthName"] == null || dt.Rows[i]["MonthName"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthName"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTMonthList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public ActionResult GetYear(YearModel cls)
        {
            try
            {
                List<YearModel> lstClientList = new List<YearModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetYearList", conn);
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
                        YearModel obj = new YearModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Year = dt.Rows[i]["Year"] == null || dt.Rows[i]["Year"].ToString().Trim() == "" ? null : dt.Rows[i]["Year"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                cls.LSTYearList = lstClientList;

                return Json(cls, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

    }
}