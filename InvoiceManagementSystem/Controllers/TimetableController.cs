using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class TimetableController : Controller
    {
        clsCommon objCommon = new clsCommon();
        // GET: Timetable
        public ActionResult Timetable()
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

        public ActionResult InsertTimetable(TimetableModel model)
        {
            model = model.addTimetable(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTimetable(TimetableModel cls)
        
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<TimetableModel> lstTimetableList = new List<TimetableModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetTimetableList", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@Days", cls.Days);
                if (objCommon.getUserIdFromSession() == 1 && objCommon.getTeacherIdFromSession() == 0 ||  objCommon.getTeacherIdFromSession() == null)
                {
                    cmd.Parameters.AddWithValue("@TeacherId", cls.TeacherId);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                }
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
                        TimetableModel obj = new TimetableModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.SubjectName = dt.Rows[i]["SubjectName"] == null || dt.Rows[i]["SubjectName"].ToString().Trim() == "" ? null : dt.Rows[i]["SubjectName"].ToString();
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        obj.Days = dt.Rows[i]["Days"] == null || dt.Rows[i]["Days"].ToString().Trim() == "" ? null : dt.Rows[i]["Days"].ToString();
                        obj.StartTime = dt.Rows[i]["StartTime"] == null || dt.Rows[i]["StartTime"].ToString().Trim() == "" ? null : dt.Rows[i]["StartTime"].ToString();
                        obj.EndTime = dt.Rows[i]["EndTime"] == null || dt.Rows[i]["EndTime"].ToString().Trim() == "" ? null : dt.Rows[i]["EndTime"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());

                        lstTimetableList.Add(obj);
                    }
                }
                cls.LSTTimetableList = lstTimetableList;
                if (cls.LSTTimetableList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTTimetableList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTTimetableList = lstTimetableList;

                return PartialView("_TimetableListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleTimetableData(TimetableModel cls)
        {
            try
            {
                cls = cls.GetTimetable(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteTimetable(TimetableModel cls)
        {
            try
            {
                cls = cls.deleteTimetable(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}