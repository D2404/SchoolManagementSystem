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
    public class ClassRoomController : Controller
    {
        clsCommon objCommon = new clsCommon();

        public ActionResult ClassRoom()
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
        public ActionResult InsertClassRoom(ClassRoomModel model)
        {
            model = model.addClassRoom(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassRoom(ClassRoomModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<ClassRoomModel> lstClassRoomList = new List<ClassRoomModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetClassRoomList", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@intActive", cls.intActive);
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
                        ClassRoomModel obj = new ClassRoomModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"] == null || dt.Rows[i]["IsActive"].ToString().Trim() == "" ? null : dt.Rows[i]["IsActive"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstClassRoomList.Add(obj);
                    }
                }
                cls.LSTClassRoomList = lstClassRoomList;
                if (cls.LSTClassRoomList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTClassRoomList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTClassRoomList = lstClassRoomList;

                return PartialView("_ClassRoomListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleClassRoomData(ClassRoomModel cls)
        {
            try
            {
                cls = cls.GetClassRoom(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteClassRoom(ClassRoomModel cls)
        {
            try
            {
                cls = cls.deleteClassRoom(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateStatus(ClassRoomModel cls)
        {
            try
            {
                var Status = cls.UpdateStatus(cls);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExportToExcel(ClassRoomModel cls)
        {
            try
            {
                // ... your data retrieval logic ...
                DataTable dt = GetDataFromDatabase(cls);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.TableName = "ClassRoomMaster";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt);

                        // Apply styles if needed
                        ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Style.Font.Bold = true;

                        // Save the workbook to a MemoryStream
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            wb.SaveAs(memoryStream);

                            // Get the file bytes
                            byte[] fileBytes = memoryStream.ToArray();

                            // Return the file download URL in the JSON response
                            string fileBase64 = Convert.ToBase64String(fileBytes);
                            string fileUrl = "data:application/vnd.openxml/formats-officedocument.spreadsheetml.sheet;base64," + fileBase64;

                            return Json(new { downloadUrl = fileUrl }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error = "No data available for export." }, JsonRequestBehavior.AllowGet);
        }

        private DataTable GetDataFromDatabase(ClassRoomModel cls)
        {
            // Implement your data retrieval logic here
            // Example: Fetch data from a SQL Server database using ADO.NET

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetClassRoomList", conn);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@intActive", cls.intActive);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}