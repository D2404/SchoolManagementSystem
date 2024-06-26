﻿using ClosedXML.Excel;
using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Repository;
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
    public class ClassSectionController : Controller
    {
        clsCommon objCommon = new clsCommon();

        private readonly ClassSectionRepository _repository;
        private readonly clsCommon _commonModel;


        public ClassSectionController(ClassSectionRepository repository, clsCommon commonModel)
        {
            _repository = repository;
            _commonModel = commonModel;
        }

        public ActionResult ClassSection()
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

        public ActionResult GetClassSection(ClassSectionModel model)
        {
            try
            {
                model = _repository.GetAllClassSection(model);
                return PartialView("_ClassSectionListPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult InsertClassSection(ClassSectionModel model)
        {
            model = _repository.AddClassSection(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSingleClassSectionData(ClassSectionModel model)
        {
            try
            {
                model = _repository.GetSingleClassSection(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteClassSection(ClassSectionModel model)
        {
            try
            {
                model = _repository.DeleteClassSection(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateStatus(ClassSectionModel model)
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

        public ActionResult ExpotToExcelClassSectionReport(ClassSectionModel model)
        {
            try
            {
                if (objCommon.getUserIdFromSession() != 0)
                {
                    DataTable dt = new DataTable();
                    dt = _repository.ExportClassSection(model);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Session["ExpotToExcelClassSectionReport"] = dt;
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
            DataTable data = (DataTable)Session["ExpotToExcelClassSectionReport"];

            string Filepath = Path.Combine(Server.MapPath("~/Data/Item/"));
            string fileName = "ClassSectionReport" + DateTime.Now.ToString("ddMMyyyymmss");
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
                var ws = wb.Worksheets.Add("ClassSectionReport");

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

                // Add title "Manage ClassSection" above column headers
                var titleCell = ws.Cell(3, 1);
                titleCell.Value = "ClassSection Data";
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