using ClosedXML.Excel;
using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly TeacherRepository _repository;
        private readonly clsCommon _commonModel;


        public TeacherController(TeacherRepository repository, clsCommon commonModel)
        {
            _repository = repository;
            _commonModel = commonModel;
        }

        public ActionResult TeacherList()
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
        public ActionResult Teacher(int? id)
        {
            if (_commonModel.getUserIdFromSession() != 0)
            {
                TeacherModel cls = new TeacherModel();
                if (id != null || id > 0)
                {
                    cls = _repository.GetSingleTeacher(cls, id);
                }
                else
                {
                    cls = _repository.FillClassRoomList(cls);
                }
                return View(cls);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult ViewTeacher(int? id)
        {
            if (_commonModel.getUserIdFromSession() != 0)
            {
                TeacherModel cls = new TeacherModel();
                cls = _repository.GetSingleTeacher(cls, id);
                return View(cls);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }



        [HttpPost]
        public ActionResult InsertTeacher(TeacherModel model)
        {
            model = _repository.AddTeacher(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTeacher(TeacherModel model)
        {
            try
            {
                model = _repository.GetAllTeacher(model);
                return PartialView("_TeacherListPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetTeacherGrid(TeacherModel model)
        {
            try
            {
                model = _repository.GetAllTeacher(model);
                return PartialView("_TeacherGridPartial", model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteTeacher(TeacherModel cls)
        {
            try
            {
                cls = _repository.DeleteTeacher(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateStatus(TeacherModel cls)
        {
            try
            {
                var Status = _repository.UpdateStatus(cls);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExpotToExcelTeacherReport(TeacherModel cls)
        {
            try
            {
                if (_commonModel.getUserIdFromSession() != 0)
                {
                    DataTable dt = new DataTable();
                    dt = _repository.ExportTeacher(cls);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Session["ExpotToExcelTeacherReport"] = dt;
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
            DataTable data = (DataTable)Session["ExpotToExcelTeacherReport"];

            string Filepath = Path.Combine(Server.MapPath("~/Data/Item/"));
            string fileName = "TeacherReport" + DateTime.Now.ToString("ddMMyyyymmss");
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
                var ws = wb.Worksheets.Add("TeacherReport");

                //          var schoolLogo = ws.AddPicture(Server.MapPath("~/Data/assets/img/muktajivan-school-logo.png"))
                //.MoveTo(ws.Cell(1, 1))
                //.WithSize(50, 50);

                // Add school name to cell (1, 2)
                var schoolNameCell = ws.Cell(1, 1);
                schoolNameCell.Value = Session["SchoolName"] as string;
                schoolNameCell.Style.Font.Bold = true;
                schoolNameCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                schoolNameCell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                schoolNameCell.Style.Font.FontSize = 16;
                ws.Range(schoolNameCell, ws.Cell(1, data.Columns.Count + 4)).Merge();

                // Add title "Manage Teacher" above column headers
                var titleCell = ws.Cell(3, 1);
                titleCell.Value = "Teacher Data";
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


        [HttpPost, ValidateInput(false)]
        public ActionResult TeacherBulkUpload(TeacherModel cls)
        {

            try
            {

                if (_commonModel.getUserIdFromSession() != 0)
                {
                    string filePath = string.Empty;
                    //clsClient cls = new clsClient();
                    if (cls.file != null)
                    {
                        string path = Server.MapPath("ManageBulkTeacher/Data/BulkTeacher/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(cls.file[0].FileName);
                        string extension = Path.GetExtension(cls.file[0].FileName);
                        cls.file[0].SaveAs(filePath);

                        string conString = string.Empty;
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                                break;

                        }

                        DataTable dt = new DataTable();
                        conString = string.Format(conString, filePath, "NO");


                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    cls.LSTTeacherList = (List<TeacherModel>)validateData(dt);
                                    if (cls.LSTTeacherList[0].ErrorMessage == "")
                                    {
                                        cls.LSTTeacherList[0].ErrorMessage = "Teacher Uploaded Successfully";
                                        InsertBulkIntoDatabase(dt);
                                    }
                                    else
                                    {

                                    }
                                    connExcel.Close();
                                }
                            }
                        }
                    }
                }
                else
                {
                    return Redirect(_commonModel.RedirectToLogin(2));
                }
                var jsonResult = Json(cls.LSTTeacherList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        int rowNo = 0;
        string errorMessage = "";

        public IList<TeacherModel> validateData(DataTable dt)
        {
            bool? status = true;
            TeacherModel obj = new TeacherModel();
            List<TeacherModel> lst = new List<TeacherModel>();
            try
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    TeacherModel cls = new TeacherModel();
                    errorMessage = "";
                    rowNo = i + 1;

                    cls.Title = dt.Rows[i][0].ToString();
                    if (cls.Title == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Title should not be blank.";
                    }

                    cls.TeacherName = dt.Rows[i][1].ToString();

                    if (cls.TeacherName == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Teacher name should not be blank.";
                    }

                    cls.FatherName = dt.Rows[i][2].ToString();
                    if (cls.FatherName == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Father name should not be blank.";
                    }

                    cls.Surname = dt.Rows[i][3].ToString();
                    if (cls.Surname == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Surname should not be blank.";
                    }

                    cls.Gender = dt.Rows[i][4].ToString();
                    if (cls.Gender == "0")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Gender should not be blank.";
                    }
                    cls.BloodGroup = dt.Rows[i][5].ToString();
                    if (cls.BloodGroup == "0")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>BloodGroup should not be blank.";
                    }
                    cls.Dob = dt.Rows[i][6].ToString();
                    if (cls.Dob == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Dob should not be blank.";
                    }
                    cls.Email = dt.Rows[i][7].ToString();
                    if (cls.Email == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Email should not be blank.";
                    }
                    else if (cls.Email != "")
                    {
                        //status = cls.CheckEmailInBulkUpdate(cls.Email);
                        status = true;
                        if (status == false)
                        {
                            errorMessage = errorMessage + "<br>Email already exists.";
                        }
                    }

                    cls.Password = dt.Rows[i][8].ToString();
                    if (cls.Password == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Password should not be blank.";
                    }

                    cls.MobileNo = dt.Rows[i][9].ToString();
                    if (cls.MobileNo == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>MobileNo should not be blank.";
                    }
                    else if (cls.MobileNo.Length != 10)
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>MobileNo should be in 10 Digit.";
                    }

                    cls.AlternateMobileNo = dt.Rows[i][10].ToString();
                    cls.DateOfJoining = dt.Rows[i][11].ToString();
                    if (cls.DateOfJoining == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>DateOfJoining should not be blank.";
                    }
                    cls.ClassNo = dt.Rows[i][12].ToString();
                    if (cls.ClassNo == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Class should not be blank.";
                    }

                    cls.Education = dt.Rows[i][13].ToString();
                    if (cls.Education == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Qualification should not be blank.";
                    }

                    cls.MaritalStatus = dt.Rows[i][14].ToString();
                    if (cls.MaritalStatus == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>MaritalStatus should not be blank.";
                    }

                    cls.AnniversaryDate = dt.Rows[i][15].ToString();
                    //if (cls.AnniversaryDate == "")
                    //{
                    //    status = false;
                    //    errorMessage = errorMessage + "<br>Anniversary Date should not be blank.";
                    //}

                    cls.Experience = dt.Rows[i][16].ToString();
                    if (cls.Experience == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Experience should not be blank.";
                    }

                    cls.CurrentAddress = dt.Rows[i][17].ToString();
                    if (cls.CurrentAddress == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Current Address should not be blank.";
                    }

                    cls.CurrentPincode = dt.Rows[i][18].ToString();
                    if (cls.CurrentPincode == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Current Pincode should not be blank.";
                    }

                    cls.CurrentCity = dt.Rows[i][19].ToString();
                    if (cls.CurrentCity == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Current City should not be blank.";
                    }

                    cls.CurrentState = dt.Rows[i][20].ToString();
                    if (cls.CurrentState == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Current State should not be blank.";
                    }

                    cls.PermenantAddress = dt.Rows[i][21].ToString();
                    if (cls.PermenantAddress == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Permenant Address should not be blank.";
                    }

                    cls.PermenantPincode = dt.Rows[i][22].ToString();
                    if (cls.PermenantPincode == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Permenant Pincode should not be blank.";
                    }

                    cls.PermenantCity = dt.Rows[i][23].ToString();
                    if (cls.PermenantCity == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Permenant City should not be blank.";
                    }

                    cls.PermenantState = dt.Rows[i][24].ToString();
                    if (cls.PermenantState == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Permenant State should not be blank.";
                    }

                    cls.BankName = dt.Rows[i][25].ToString();
                    if (cls.BankName == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Bank Name should not be blank.";
                    }

                    cls.BankBranch = dt.Rows[i][26].ToString();
                    if (cls.BankName == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Bank Branch should not be blank.";
                    }

                    cls.AccountNo = dt.Rows[i][27].ToString();
                    if (cls.AccountNo == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>Account No should not be blank.";
                    }
                    cls.IFSCCode = dt.Rows[i][28].ToString();
                    if (cls.IFSCCode == "")
                    {
                        status = false;
                        errorMessage = errorMessage + "<br>IFSC Code should not be blank.";
                    }

                    cls.ErrorMessage = errorMessage;


                    lst.Add(cls);
                }
                //string Email = string.Empty;
                //foreach (var item in lst)
                //{
                //    cls.TempEmail = Email + item.Email + ",";
                //}
                string TempEmail = string.Empty;
                TempEmail = string.Join(",", lst.Select(w => w.Email));

                foreach (var item in lst)
                {
                    item.TempEmail = TempEmail;
                }
                obj.LSTTeacherList = lst;
            }
            catch (Exception ex)
            {
                throw ex;
                errorMessage = errorMessage + "|" + rowNo;
                status = false;
                return obj.LSTTeacherList;
            }


            return obj.LSTTeacherList;
        }

        public void InsertBulkIntoDatabase(DataTable dt)
        {
            try
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    TeacherModel cls = new TeacherModel();
                    Random rnd = new Random();
                    int RandomNo = rnd.Next(1000, 10000);
                    cls.TeacherUniqueId = "Teacher_" + RandomNo.ToString();//Client_1123

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertBulkTeacherData", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = dt.Rows[i][0].ToString();
                    cmd.Parameters.Add("@TeacherName", SqlDbType.VarChar).Value = dt.Rows[i][1].ToString();
                    cmd.Parameters.Add("@FatherName", SqlDbType.VarChar).Value = dt.Rows[i][2].ToString();
                    cmd.Parameters.Add("@Surname", SqlDbType.VarChar).Value = dt.Rows[i][3].ToString();
                    cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = dt.Rows[i][4].ToString();
                    cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = dt.Rows[i][5].ToString();
                    cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows[i][6].ToString());
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = dt.Rows[i][7].ToString();
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = clsCommon.EncryptString(dt.Rows[i][8].ToString());
                    cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = dt.Rows[i][9].ToString();
                    cmd.Parameters.Add("@AlternateMobileNo", SqlDbType.VarChar).Value = dt.Rows[i][10].ToString();
                    cmd.Parameters.Add("@DateOfJoining", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows[i][11]);
                    cmd.Parameters.Add("@ClassId", SqlDbType.Int).Value = dt.Rows[i][12].ToString();
                    cmd.Parameters.Add("@Education", SqlDbType.VarChar).Value = dt.Rows[i][13].ToString();
                    cmd.Parameters.Add("@MaritalStatus", SqlDbType.VarChar).Value = dt.Rows[i][14].ToString();
                    cmd.Parameters.Add("@AnnivarsaryDate", SqlDbType.VarChar).Value = dt.Rows[i][15].ToString();
                    cmd.Parameters.Add("@Experience", SqlDbType.VarChar).Value = dt.Rows[i][16].ToString();
                    cmd.Parameters.Add("@CurrentAddress", SqlDbType.VarChar).Value = dt.Rows[i][17].ToString();
                    cmd.Parameters.Add("@CurrentPincode", SqlDbType.VarChar).Value = dt.Rows[i][18].ToString();
                    cmd.Parameters.Add("@CurrentCity", SqlDbType.VarChar).Value = dt.Rows[i][19].ToString();
                    cmd.Parameters.Add("@CurrentState", SqlDbType.VarChar).Value = dt.Rows[i][20].ToString();
                    cmd.Parameters.Add("@PermenantAddress", SqlDbType.VarChar).Value = dt.Rows[i][21].ToString();
                    cmd.Parameters.Add("@PermenantPincode", SqlDbType.VarChar).Value = dt.Rows[i][22].ToString();
                    cmd.Parameters.Add("@PermenantCity", SqlDbType.VarChar).Value = dt.Rows[i][23].ToString();
                    cmd.Parameters.Add("@PermenantState", SqlDbType.VarChar).Value = dt.Rows[i][24].ToString();
                    cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = dt.Rows[i][25].ToString();
                    cmd.Parameters.Add("@BankBranch", SqlDbType.VarChar).Value = dt.Rows[i][26].ToString();
                    cmd.Parameters.Add("@AccountNo", SqlDbType.VarChar).Value = dt.Rows[i][27].ToString();
                    cmd.Parameters.Add("@IFSCCode", SqlDbType.VarChar).Value = dt.Rows[i][28].ToString();



                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = _commonModel.getUserIdFromSession();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 0;
                    da.ReturnProviderSpecificTypes = true;
                    DataTable dt1 = new DataTable();
                    da.Fill(dt1);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WelcomeMail(TeacherModel cls)
        {
            clsCommon commonobj = new clsCommon();
            var data = _repository.WelcomeMail(cls);
            if (data.LSTTeacherList[0].Response == "Success")
            {
                string toEmail = cls.Email;
                string[] TempEmail = toEmail.Split(',');

                for (int i = 0; i < TempEmail.Length; i++)
                {
                    toEmail = TempEmail[i];
                    var Role = commonobj.getRoleNameFromSession();
                    string RoleName = null;
                    if (Role == "SuperAdmin")
                    {
                        RoleName = "Admin";
                    }
                    else if (Role == "Admin")
                    {
                        RoleName = "Teacher";
                    }
                    else if (Role == "Teacher")
                    {
                        RoleName = "Student";
                    }
                    var Password = clsCommon.DecryptString(data.LSTTeacherList[i].Password);
                    string Teacher = "Registration Successfully.";
                    string body = "";
                    using (StreamReader reader = new StreamReader(Server.MapPath("/Data/MailTemplate/WelcomeMail.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    string imageBase64 = string.Empty;
                    string imagePath = string.Empty;

                    // Set imagePath and imageBase64 based on the current email being processed
                    if (data.LSTTeacherList[i].ProfileImg == null || data.LSTTeacherList[i].ProfileImg == "Null" || data.LSTTeacherList[i].ProfileImg == "undefined")
                    {
                        imageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/Data/Profile/dummy.jpg")));
                        imagePath = Server.MapPath("~/Data/Profile/dummy.jpg");
                    }
                    else
                    {
                        imageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("/Data/Profile/" + data.LSTTeacherList[i].ProfileImg)));
                        imagePath = Server.MapPath("/Data/Profile/" + data.LSTTeacherList[i].ProfileImg);
                    }

                    body = body.Replace("[[Profile]]", $"cid:logoImage");
                    body = body.Replace("[[UserName]]", data.LSTTeacherList[i].TeacherName);
                    body = body.Replace("[[EmailId]]", data.LSTTeacherList[i].Email);
                    body = body.Replace("[[Password]]", Password);
                    body = body.Replace("[[RoleName]]", RoleName);

                    sendEmail(toEmail, Teacher, body, imagePath); // Pass Email instead of toEmail
                    cls.Response = "Success";
                }

            }
            else
            {
                cls.Response = "Error";
            }
            return Json(cls.Response, JsonRequestBehavior.AllowGet);
        }


        public void sendEmail(string toEmail, string Subject, string body, string imagePath)
        {
            try
            {
                clsCommon obj = new clsCommon();
                string[] TempEmail = toEmail.Split(',');

                for (int i = 0; i < TempEmail.Length; i++)
                {
                    string to = TempEmail[i];
                    //string to = toEmail;

                    var EmailConfigaration = obj.EmailConfigaration();
                    string host = EmailConfigaration.Host;
                    string username = EmailConfigaration.Username;
                    string FromEmail = EmailConfigaration.FromMail;
                    string password = EmailConfigaration.Password;
                    int port = EmailConfigaration.Port;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(FromEmail);
                    mail.To.Add(to);
                    mail.Subject = Subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                        LinkedResource logo = new LinkedResource(imagePath, "image/jpg");
                        logo.ContentId = "logoImage";

                        av.LinkedResources.Add(logo);

                        mail.AlternateViews.Add(av);
                    }
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = host;
                    smtp.Credentials = new System.Net.NetworkCredential
                         (FromEmail, password);
                    smtp.Port = port;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

            }
            catch (Exception ex)
            {

            }

        }
    }
}