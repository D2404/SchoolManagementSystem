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
    public class LeaveController : Controller
    {
        clsCommon objCommon = new clsCommon();

        // GET: Leave
        public ActionResult Leave()
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

        public ActionResult InsertLeave(LeaveModel model)
        {
            model = model.addLeave(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLeave(LeaveModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                var TeacherId = objCommon.getTeacherIdFromSession();
                List<LeaveModel> lstLeaveList = new List<LeaveModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetLeaveList", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@TeacherId", TeacherId);
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
                        LeaveModel obj = new LeaveModel();
                        TeacherId = TeacherId;
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Status = Convert.ToInt32(dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString());
                        obj.NoOfDays = Convert.ToDecimal(dt.Rows[i]["NoOfDays"] == null || dt.Rows[i]["NoOfDays"].ToString().Trim() == "" ? null : dt.Rows[i]["NoOfDays"].ToString());
                        if(TeacherId == 0)
                        {
                            obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        }
                        else
                        {
                            obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                            obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        }
                        obj.TeacherId = (int)TeacherId;
                        obj.FromDate = dt.Rows[i]["FromDate"] == null || dt.Rows[i]["FromDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("dd/MM/yyyy");
                        obj.ToDate = dt.Rows[i]["ToDate"] == null || dt.Rows[i]["ToDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("dd/MM/yyyy");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstLeaveList.Add(obj);
                    }
                }
                cls.LSTLeaveList = lstLeaveList;
                if (cls.LSTLeaveList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTLeaveList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTLeaveList = lstLeaveList;

                return PartialView("_LeaveListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult GetSingleLeave(LeaveModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<LeaveModel> lstLeaveList = new List<LeaveModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetSingleLeave", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
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
                        obj.FromDate = dt.Rows[i]["FromDate"] == null || dt.Rows[i]["FromDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("yyyy/MM/dd");
                        obj.ToDate = dt.Rows[i]["ToDate"] == null || dt.Rows[i]["ToDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("yyyy/MM/dd");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                       
                        lstLeaveList.Add(obj);
                    }
                }
                cls.LSTLeaveList = lstLeaveList;
                //if (cls.LSTLeaveList.Count > 0)
                //{
                //    var pager = new Models.Pager((int)cls.LSTLeaveList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                //    cls.Pager = pager;
                //}
                //cls.TotalEntries = TotalEntries;
                //cls.ShowingEntries = showingEntries;
                //cls.fromEntries = startentries;
                //cls.LSTLeaveList = lstLeaveList;

                return PartialView("_LeaveListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteLeave(LeaveModel cls)
        {
            try
            {
                cls = cls.deleteLeave(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ApproveStatus(LeaveModel cls)
        {
            try
            {
                var Status = cls.ApproveStatus(cls);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult RejectStatus(LeaveModel cls)
        {
            try
            {
                var Status = cls.RejectStatus(cls);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}