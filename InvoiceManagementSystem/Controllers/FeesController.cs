using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Repository;
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
    public class FeesController : Controller
    {
        private readonly ClassWiseFeesRepository _repository;
        private readonly clsCommon _commonModel;


        public FeesController(ClassWiseFeesRepository repository, clsCommon commonModel)
        {
            _repository = repository;
            _commonModel = commonModel;
        }
        // GET: Exam

        #region FeesMaster
        public ActionResult ClassWiseFees()
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

        public ActionResult InsertClassWiseFees(FeesModel model)
        {
            model = _repository.AddClassWiseFees(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassWiseFees(FeesModel model)
        {
            try
            {
                model = _repository.GetAllClassWiseFees(model);
                return PartialView("_ClassWiseFeesListPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleClassWiseFeesData(FeesModel model)
        {
            try
            {
                model = _repository.GetSingleClassWiseFees(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteClassWiseFees(FeesModel model)
        {
            try
            {
                model = _repository.DeleteClassWiseFees(model);
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
                cmd.Parameters.AddWithValue("@SchoolId", _commonModel.getSchoolIdFromSession());
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

        public ActionResult UpdateStatus(SubjectModel model)
        {
            try
            {
                var Status = model.UpdateStatus(model);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region FeesCollection
        public ActionResult Fees()
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

        public ActionResult FeesHistory()
        {
            int studentId = Convert.ToInt32(Request.QueryString["studentid"]); // Retrieve the studentid query string parameter
            Session["ValueToPass"] = studentId;
            return View();
        }


        [HttpPost]

        public ActionResult InsertFees(FeesModel model)
        {
            model = model.addFees(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertandPayFees(FeesModel model)
        {
            model = model.addandPayFees(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFees(FeesModel model)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<FeesModel> lstFeesList = new List<FeesModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetFeesList", conn);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", model.PageIndex);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@SchoolId", _commonModel.getSchoolIdFromSession());
                //cmd.Parameters.AddWithValue("@UserId", _commonModel.getUserIdFromSession());
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
                        FeesModel obj = new FeesModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Status = Convert.ToInt32(dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        //obj.MonthId = Convert.ToInt32(dt.Rows[i]["MonthId"] == null || dt.Rows[i]["MonthId"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthId"].ToString());
                        //obj.YearId = Convert.ToInt32(dt.Rows[i]["YearId"] == null || dt.Rows[i]["YearId"].ToString().Trim() == "" ? null : dt.Rows[i]["YearId"].ToString());
                        //obj.FeesAmount = Convert.ToInt32(dt.Rows[i]["FeesAmount"] == null || dt.Rows[i]["FeesAmount"].ToString().Trim() == "" ? null : dt.Rows[i]["FeesAmount"].ToString());
                        obj.TotalPay = Convert.ToInt32(dt.Rows[i]["TotalPay"] == null || dt.Rows[i]["TotalPay"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPay"].ToString());
                        obj.TotalPending = Convert.ToInt32(dt.Rows[i]["TotalPending"] == null || dt.Rows[i]["TotalPending"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPending"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["YearlyFees"] == null || dt.Rows[i]["YearlyFees"].ToString().Trim() == "" ? null : dt.Rows[i]["YearlyFees"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["YearlyFees"] == null || dt.Rows[i]["YearlyFees"].ToString().Trim() == "" ? null : dt.Rows[i]["YearlyFees"].ToString());
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        //obj.MonthName = dt.Rows[i]["MonthName"] == null || dt.Rows[i]["MonthName"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthName"].ToString();
                        //obj.YearName = dt.Rows[i]["Year"] == null || dt.Rows[i]["Year"].ToString().Trim() == "" ? null : dt.Rows[i]["Year"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstFeesList.Add(obj);
                    }
                }
                model.LSTFeesList = lstFeesList;
                if (model.LSTFeesList.Count > 0)
                {
                    var pager = new Models.Pager((int)model.LSTFeesList[0].TotalRecord, model.PageIndex, (int)model.PageSize);

                    model.Pager = pager;
                }
                model.TotalEntries = TotalEntries;
                model.ShowingEntries = showingEntries;
                model.fromEntries = startentries;
                model.LSTFeesList = lstFeesList;

                return PartialView("_FeesListPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult GetFeesHistory(FeesModel model)
        {
            try
            {
                int parameterValue = (int)Session["ValueToPass"];
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<FeesModel> lstFeesList = new List<FeesModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetFeesCollectionHistoryList", conn);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@PageIndex", model.PageIndex);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                //cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@StudentId", parameterValue);
                cmd.Parameters.AddWithValue("@SchoolId", _commonModel.getSchoolIdFromSession());
                //cmd.Parameters.AddWithValue("@UserId", _commonModel.getUserIdFromSession());
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
                        FeesModel obj = new FeesModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.MonthId = Convert.ToInt32(dt.Rows[i]["MonthId"] == null || dt.Rows[i]["MonthId"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthId"].ToString());
                        obj.FeesAmount = Convert.ToInt32(dt.Rows[i]["FeesAmount"] == null || dt.Rows[i]["FeesAmount"].ToString().Trim() == "" ? null : dt.Rows[i]["FeesAmount"].ToString());
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        obj.MonthName = dt.Rows[i]["MonthName"] == null || dt.Rows[i]["MonthName"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthName"].ToString();
                        obj.AcademicYearId = Convert.ToInt32(dt.Rows[i]["Year"] == null || dt.Rows[i]["Year"].ToString().Trim() == "" ? null : dt.Rows[i]["Year"].ToString());
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstFeesList.Add(obj);
                    }
                }
                model.LSTFeesList = lstFeesList;
                if (model.LSTFeesList.Count > 0)
                {
                    var pager = new Models.Pager((int)model.LSTFeesList[0].TotalRecord, model.PageIndex, (int)model.PageSize);

                    model.Pager = pager;
                }
                model.TotalEntries = TotalEntries;
                model.ShowingEntries = showingEntries;
                model.fromEntries = startentries;
                model.LSTFeesList = lstFeesList;


                return PartialView("_FeesHistoryListPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetSingleFeesData(FeesModel model)
        {
            try
            {
                model = model.GetFees(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteFees(FeesModel model)
        {
            try
            {
                model = model.deleteFees(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteFeesHistory(FeesModel model)
        {
            try
            {
                model = model.deleteFeesHistory(model);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult FeesInvoice(int StudentId)
        {
            try
            {
                if (_commonModel.getUserIdFromSession() != 0)
                {
                    FeesModel model = new FeesModel();
                    if (StudentId.ToString() != null && StudentId > 0)
                    {
                        model.StudentId = StudentId;

                        model = model.GetFeesByStudentId(model);
                        model = model.GetSchoolDetails(model);

                        if (model.LSTFeesList != null && model.LSTFeesList.Count > 0)
                        {
                            model.StudentId = model.LSTFeesList[0].StudentId;
                            model = model.GetFeesHistoryByStudentId(model);
                        }
                    }
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
                //}
                //else
                //{
                //    return RedirectToPage();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult DownloadFees(int StudentId)
        {
            string body = "";
            FeesModel model = new FeesModel();

            if (StudentId.ToString() != null && StudentId > 0)
            {
                model.StudentId = StudentId;

                model = model.GetFeesByStudentId(model);
                model = model.GetSchoolDetails(model);

                if (model.LSTFeesList != null && model.LSTFeesList.Count > 0)
                {
                    model.StudentId = model.LSTFeesList[0].StudentId;
                    model = model.GetFeesHistoryByStudentId(model);
                }
            }

            using (StreamReader reader = new StreamReader(Server.MapPath("/Data/PDFFormat/FeesInvoice.html")))
            {
                body = reader.ReadToEnd();
            }
            if (model.LSTSchoolList != null && model.LSTSchoolList.Count > 0)
            {
                var item = model.LSTSchoolList[0];

                body = body.Replace("[[SchoolName]]", item.SchoolName);
                body = body.Replace("[[Address]]", item.Address);
                body = body.Replace("[[Email]]", "School Email.: " + item.Email); 
                body = body.Replace("[[MobileNo]]", "School Contact.: <span>" + item.MobileNo + "</span>");
                string dynamicImagePath = "https://localhost:44349" + "/Data/SchoolPhoto/" + item.PhotoImg; // Assuming item.PhotoImg is a property or variable containing the image path
                body = body.Replace("[[schoolLogo]]", $"<img style=\"height:50px;width:50px;\" src=\"{dynamicImagePath}\" />");

            }
            if (model.LSTFeesList != null && model.LSTFeesList.Count > 0)
            {
                var item1 = model.LSTFeesList[0];

                body = body.Replace("[[StudentName]]", item1.StudentName);
                body = body.Replace("[[StudentEmail]]", item1.Email);
                body = body.Replace("[[StudentMobileNo]]", item1.MobileNo);
                body = body.Replace("[[ClassNo]]", item1.ClassNo);
                body = body.Replace("[[RollNo]]", item1.RollNo.ToString());
                body = body.Replace("[[Date]]", item1.Date);  

                string InvoiceDetails = "";
                for (int i = 0; i < model.LSTFeesHistoryList.Count; i++)
                {
                    var detail = model.LSTFeesHistoryList[i];
                    InvoiceDetails = InvoiceDetails + "<tr>";
                    InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;text-align:center'>" + (i + 1) + "</td>";
                    InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6; border-right: 1px solid  black;text-align:center'>" + detail.StudentName + "</td>";
                    InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6; border-right: 1px solid  black;text-align:center'>" + detail.Date + "</td>";
                    InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;text-align:center'>" + detail.MonthName + "</td>";
                    InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6; border-right: 1px solid  black;text-align:center'>" + detail.AcademicYearId + "</td>";
                    InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6; border-right: 1px solid  black;text-align:center'>" + detail.FeesAmount + "</td>";
                    InvoiceDetails = InvoiceDetails + "</tr>";


                }


                if (model.LSTFeesHistoryList.Count < 15)

                {


                    //for (int j = model.LSTFeesHistoryList.Count; j == model.LSTFeesHistoryList.Count; j++)
                    for (int j = model.LSTFeesHistoryList.Count; j < 10; j++)
                    {
                        InvoiceDetails = InvoiceDetails + "<tr>";

                        InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;'>&nbsp</td>";
                        InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;'></td>";
                        InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;'></td>";
                        InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6; border-right: 1px solid  black;'></td>";
                        InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;'></td>";
                        InvoiceDetails = InvoiceDetails + "<td style='border-top: 1px solid #dee2e6;  border-right: 1px solid  black;'></td>";
                        InvoiceDetails = InvoiceDetails + "</tr>";
                    }

                }

                body = body.Replace("[[InvoiceDetails]]", InvoiceDetails);
                body = body.Replace("[[FeesAmount]]", "" + item1.FeesAmount);
            }

            string dateTimeForName = "FeesInvoice_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");

            string Filepath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Data/FeesInvoice/"));
            if (!(Directory.Exists(Filepath)))
            {
                Directory.CreateDirectory(Filepath);
            }
            System.IO.TextWriter w = new System.IO.StreamWriter(Server.MapPath("~/Data/FeesInvoice/") + dateTimeForName + ".html");
            w.Write(body.ToString());
            w.Flush();
            w.Close();

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var margins = new PageMargins();

            margins.Top = 0;
            margins.Bottom = 0;
            margins.Left = 0;
            margins.Right = 0;
            margins.Right = 0;

            htmlToPdf.Orientation = NReco.PdfGenerator.PageOrientation.Portrait;
            htmlToPdf.Zoom = 1.58f;
            htmlToPdf.Size = NReco.PdfGenerator.PageSize.A4;
            htmlToPdf.Margins = margins;
            htmlToPdf.GeneratePdfFromFile(Filepath + dateTimeForName + ".html", null, Filepath + dateTimeForName + ".pdf");
            string strFilePath = Filepath + dateTimeForName + ".pdf";
            var fileName = Path.GetFileName(strFilePath);
            return Json(fileName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadStudent(int ClassId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon _commonModel = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadStudentDropDown", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@UserId", _commonModel.getUserIdFromSession());
                cmd.Parameters.Add("@ClassId", ClassId);
                cmd.Parameters.AddWithValue("@SchoolId", _commonModel.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<FeesModel> modelLst = new List<FeesModel>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        FeesModel obj = new FeesModel();
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.StudentName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.Monthly = Convert.ToInt32(dt.Rows[i]["Monthly"] == null || dt.Rows[i]["Monthly"].ToString().Trim() == "" ? null : dt.Rows[i]["Monthly"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["Yearly"] == null || dt.Rows[i]["Yearly"].ToString().Trim() == "" ? null : dt.Rows[i]["Yearly"].ToString());
                        modelLst.Add(obj);
                    }
                }
                return Json(modelLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LoadDDlStudent(int ClassId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon _commonModel = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadDdlStudentDropDown", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", _commonModel.getUserIdFromSession());
                cmd.Parameters.Add("@ClassId", ClassId);
                cmd.Parameters.AddWithValue("@SchoolId", _commonModel.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<FeesModel> modelLst = new List<FeesModel>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        FeesModel obj = new FeesModel();
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.StudentName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();

                        modelLst.Add(obj);
                    }
                }
                return Json(modelLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult LoadRollNo(int StudentId)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                clsCommon _commonModel = new clsCommon();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_LoadRollNo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StudentId", StudentId);
                cmd.Parameters.AddWithValue("@SchoolId", _commonModel.getSchoolIdFromSession());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                List<FeesModel> modelLst = new List<FeesModel>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        FeesModel obj = new FeesModel();
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        modelLst.Add(obj);
                    }
                }
                return Json(modelLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetMonth(MonthModel model)
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
                model.LSTMonthList = lstClientList;

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public ActionResult GetYear(YearModel model)
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
                model.LSTYearList = lstClientList;

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