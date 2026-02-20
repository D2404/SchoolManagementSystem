using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Repository;
using Microsoft.ApplicationBlocks.Data;
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
    public class ExamController : Controller
    {
        private readonly ExamDetailsRepository _repository;
        private readonly ExamMarksRepository _marksrepository;
        private readonly clsCommon _commonModel;


        public ExamController(ExamDetailsRepository repository, ExamMarksRepository marksrepository , clsCommon commonModel)
        {
            _repository = repository;
            _marksrepository = marksrepository;
            _commonModel = commonModel;
        }

        // GET: Exam

        #region ExamMarks
        public ActionResult ExamMarks()
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

        public ActionResult InsertExamMarks(ExamModel model)
        {
            model = _marksrepository.AddExamMarks(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExamMarks(ExamModel model)
        {
            try
            {
                model = _marksrepository.GetAllExamMarks(model);
                return PartialView("_ExamMarksListPartial", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleExamMarksData(ExamModel model)
        {
            try
            {
                model = _marksrepository.GetSingleExamMarks(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteExamMarks(ExamModel model)
        {
            try
            {
                model = _marksrepository.DeleteExamMarks(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetSubjectList(int? ClassId = 0)
        {
            try
            {
                var lstUser = _commonModel.Fill_Subject(Convert.ToInt32(ClassId));
                return Json(lstUser, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        

        

        #endregion


        #region ExamDetails

        public ActionResult ExamDetails()
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

        public ActionResult InsertExamDetails(ExamModel model)
        {
            model = _repository.AddExamDetails(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExamDetails(ExamModel model)
        {
            try
            {
                model = _repository.GetAllExamDetails(model);
                return PartialView("_ExamDetailsListPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleExamDetailsData(ExamModel model)
        {
            try
            {
                model = _repository.GetSingleExamDetails(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteExamDetails(ExamModel model)
        {
            try
            {
                model = _repository.DeleteExamDetails(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}