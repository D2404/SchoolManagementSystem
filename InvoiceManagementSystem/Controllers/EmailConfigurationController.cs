using ClosedXML.Excel;
using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class EmailConfigurationController : Controller
    {
        clsCommon objCommon = new clsCommon();
        public ActionResult EmailConfigurationList()
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

        public ActionResult EmailConfiguration(int? id)
        {
            if (objCommon.getUserIdFromSession() != 0)
            {
                EmailConfigurationSetting cls = new EmailConfigurationSetting();
                if (id != null || id > 0)
                {
                    cls = cls.GetEmailConfiguration(cls, id);
                }
                else
                {
                    cls = cls.FillTeacherList(cls);
                }
                return View(cls);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult InsertEmailConfiguration(EmailConfigurationSetting model)
        {
            model = model.addEmailConfiguration(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmailConfiguration(EmailConfigurationSetting cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<EmailConfigurationSetting> lstEmailConfigurationList = new List<EmailConfigurationSetting>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetEmailConfiguration", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
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
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Rolename = dt.Rows[i]["Rolename"] == null || dt.Rows[i]["Rolename"].ToString().Trim() == "" ? null : dt.Rows[i]["Rolename"].ToString();
                        obj.Username = dt.Rows[i]["Username"] == null || dt.Rows[i]["Username"].ToString().Trim() == "" ? null : dt.Rows[i]["Username"].ToString();
                        obj.FromMail = dt.Rows[i]["FromMail"] == null || dt.Rows[i]["FromMail"].ToString().Trim() == "" ? null : dt.Rows[i]["FromMail"].ToString();
                        obj.Password = dt.Rows[i]["Password"] == null || dt.Rows[i]["Password"].ToString().Trim() == "" ? null : dt.Rows[i]["Password"].ToString();
                        obj.Host = dt.Rows[i]["Host"] == null || dt.Rows[i]["Host"].ToString().Trim() == "" ? null : dt.Rows[i]["Host"].ToString();
                        obj.Port = Convert.ToInt32(dt.Rows[i]["Port"] == null || dt.Rows[i]["Port"].ToString().Trim() == "" ? null : dt.Rows[i]["Port"].ToString());
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstEmailConfigurationList.Add(obj);
                    }
                }
                cls.LSTEmailConfigurationList = lstEmailConfigurationList;
                if (cls.LSTEmailConfigurationList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTEmailConfigurationList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTEmailConfigurationList = lstEmailConfigurationList;

                return PartialView("_EmailConfigurationListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult GetSingleEmailConfigurationData(EmailConfigurationSetting cls)
        //{
        //    try
        //    {
        //        cls = cls.GetEmailConfiguration(cls);
        //        return Json(cls, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult deleteEmailConfiguration(EmailConfigurationSetting cls)
        {
            try
            {
                cls = cls.deleteEmailConfiguration(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExpotToExcelEmailConfigurationReport(EmailConfigurationSetting cls)
        {
            try
            {
                if (objCommon.getUserIdFromSession() != 0)
                {
                    DataTable dt = new DataTable();
                    dt = cls.ExportEmailConfiguration(cls);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Session["ExpotToExcelEmailConfigurationReport"] = dt;
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("error", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Redirect(objCommon.RedirectToLogin(2));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExportToExcel()
        {
            DataTable data = (DataTable)Session["ExpotToExcelEmailConfigurationReport"];

            string Filepath = Path.Combine(Server.MapPath("~/Data/Item/"));
            string fileName = "EmailConfigurationReport" + DateTime.Now.ToString("ddMMyyyymmss");
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
                var ws = wb.Worksheets.Add("EmailConfigurationReport");

                 //          var schoolLogo = ws.AddPicture(Server.MapPath("~/Data/assets/img/muktajivan-school-logo.png"))
                 //.MoveTo(ws.Cell(1, 1))
                 //.WithSize(50, 50);

                  // Add school name to cell (1, 2)
                  var schoolNameCell = ws.Cell(1, 1);
                  schoolNameCell.Value = "Shree Mukta Jivan School";
                  schoolNameCell.Style.Font.Bold = true;
                  schoolNameCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                  schoolNameCell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                  schoolNameCell.Style.Font.FontSize = 16;
                ws.Range(schoolNameCell, ws.Cell(1, data.Columns.Count + 4)).Merge();

                // Add title "Manage EmailConfiguration" above column headers
                var titleCell = ws.Cell(3, 1);
                titleCell.Value = "EmailConfiguration Data";
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