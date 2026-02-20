using ClosedXML.Excel;
using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Repository;
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
    public class TeacherSubjectController : Controller
    {
        private readonly TeacherSubjectRepository _repository;
        private readonly clsCommon _commonModel;


        public TeacherSubjectController(TeacherSubjectRepository repository, clsCommon commonModel)
        {
            _repository = repository;
            _commonModel = commonModel;
        }

        // GET: Subject
        public ActionResult TeacherSubject()
        {
            if (_commonModel.getUserIdFromSession() != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]

        public ActionResult InsertTeacherSubject(TeacherSubjectModel model)
        {
            model = _repository.AddTeacherSubject(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTeacherSubject(TeacherSubjectModel model)
        {
            try
            {
                model = _repository.GetAllTeacherSubject(model);
                return PartialView("_TeacherSubjectListPartial", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleTeacherSubjectData(TeacherSubjectModel model)
        {
            try
            {
                model = _repository.GetSingleTeacherSubject(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteTeacherSubject(TeacherSubjectModel model)
        {
            try
            {
                model = _repository.DeleteTeacherSubject(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetClassRoom(ClassRoomModel model)
        {
            try
            {
                List<ClassRoomModel> lstClientList = new List<ClassRoomModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetClassRoomList", conn);
                cmd.Parameters.AddWithValue("@UserId", _commonModel.getUserIdFromSession());
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
                model.LSTClassRoomList = lstClientList;

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public ActionResult GetTeacher(TeacherModel model)
        {
            try
            {
                List<TeacherModel> lstClientList = new List<TeacherModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetTeacherList", conn);
                cmd.Parameters.AddWithValue("@UserId", _commonModel.getUserIdFromSession());
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
                        TeacherModel obj = new TeacherModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();

                        lstClientList.Add(obj);
                    }
                }
                model.LSTTeacherList = lstClientList;

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult UpdateStatus(TeacherSubjectModel model)
        {
            try
            {
                var Status = _repository.UpdateStatus(model);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExpotToExcelTeacherSubjectReport(TeacherSubjectModel model)
        {
            try
            {
                if (_commonModel.getUserIdFromSession() != 0)
                {
                    DataTable dt = new DataTable();
                    dt = _repository.ExportTeacherSubject(model);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Session["ExpotToExcelTeacherSubjectReport"] = dt;
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("error", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Redirect(_commonModel.RedirectToLogin(2));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExportToExcel()
        {
            DataTable data = (DataTable)Session["ExpotToExcelTeacherSubjectReport"];

            string Filepath = Path.Combine(Server.MapPath("~/Data/Item/"));
            string fileName = "TeacherSubjectReport" + DateTime.Now.ToString("ddMMyyyymmss");
            string file = Filepath + fileName;

            DataTable dtcolumn = new DataTable();
            for (int i = 0; i < data.Columns.Count; i++)
            {
                dtcolumn.Columns.Add(data.Columns[i].ToString());
            }

            dtcolumn.Rows.Add();
            for (int i = 0; i < data.Columns.Count; i++)
            {
                dtcolumn.Rows[0][i] = (data.Columns[i].ToString());
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("TeacherSubjectReport");

                //          var schoolLogo = ws.AddPicture(Server.MapPath("~/Data/assets/img/muktajivan-school-logo.png"))
                //.MoveTo(ws.Cell(1, 1))
                //.WithSize(50, 50);

                // Add school name to cell (1, 2)
                var schoolNameCell = ws.Cell(1, 1);
                schoolNameCell.Value = "SHREE MUKTA JIVAN SCHOOL";
                schoolNameCell.Style.Font.Bold = true;
                schoolNameCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                schoolNameCell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                schoolNameCell.Style.Font.FontSize = 16;
                ws.Range(schoolNameCell, ws.Cell(1, data.Columns.Count + 4)).Merge();

                // Add title "Manage Classroom" above column headers
                var titleCell = ws.Cell(3, 1);
                titleCell.Value = "Teacher Subject Data";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleCell.Style.Font.FontSize = 14;
                ws.Range(titleCell, ws.Cell(3, data.Columns.Count + 3)).Merge();

                // Add header row and make it bold with borders
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    var cell = ws.Cell(5, i + 1);
                    cell.Value = data.Columns[i].ToString();
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // Add borders to header row
                    cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.BottomBorderColor = XLColor.Black;
                }

                // Add data rows with borders
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        var cell = ws.Cell(i + 6, j + 1);
                        cell.Value = data.Rows[i][j].ToString();
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorderColor = XLColor.Black;
                    }
                }

                // Add data rows
                ws.Cell(6, 1).InsertData(data.Rows);

                // Auto-adjust column widths
                ws.Columns().AdjustToContents();

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}