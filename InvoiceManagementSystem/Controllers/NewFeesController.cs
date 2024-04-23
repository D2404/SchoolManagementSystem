using InvoiceManagementSystem.Models;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class NewFeesController : Controller
    {
        clsCommon objCommon = new clsCommon();

        // GET: Exam

        public ActionResult NewFeesDetails(int? Id)
        {
            try
            {
                if (objCommon.getUserIdFromSession() != 0)
                {
                    FeesModel cls = new FeesModel();

                    if (Id == null || Id == 0)
                    {
                        ViewBag.PageTitle = "Add Fees";
                    }
                    else
                    {
                        ViewBag.PageTitle = "Edit Fees";
                        //cls = cls.GetSingleSupplierDebitNoteInvoiceData(cls, Id);
                    }
                    cls = cls.FillClassRoomList(cls);
                    return View(cls);
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
    }
}