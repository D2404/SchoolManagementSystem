using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Repository;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository _repository;
        private readonly clsCommon _commonModel;


        public AccountController(AccountRepository repository, clsCommon commonModel)
        {
            _repository = repository;
            _commonModel = commonModel;
        }

        // GET: Account

        public ActionResult MyProfile(AccountModel cls)
        {

            int? TeacherId = _commonModel.getTeacherIdFromSession();
            int? UserId = _commonModel.getUserIdFromSession();
            int? StudentId = _commonModel.getStudentIdFromSession();
            int? SchoolId = _commonModel.getSchoolIdFromSession();
            if (TeacherId > 0 || UserId > 0 || StudentId > 0)
            {
                cls.TeacherId = TeacherId.Value;
                cls.Id = UserId.Value;
                cls.StudentId = StudentId.Value;
                cls.SchoolId = SchoolId.Value;
                cls = _repository.MyProfile(cls);
                return View(cls);
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult GetMyProfile(AccountModel cls)
        {
            cls.Id = _commonModel.getUserIdFromSession();
            cls = _repository.MyProfile(cls);
            return Json(cls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateProfile(AccountModel cls)
        {
            //Parameter param = new Parameter();
            cls = _repository.UpdateProfile(cls);
            return Json(cls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePassword(AccountModel cls)
        {
            //Parameter param = new Parameter();
            cls = _repository.ChangePassword(cls);
            return Json(cls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ForgotPassword1()
        {
            return View();
        }
        public ActionResult Login1()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Login(AccountModel cls)
        {
            try
            {
                cls = _repository.Login(cls);
                if (cls.Id > 0)
                {
                    Session["Id"] = cls.Id;
                    Session["AcademicYear"] = "2024-2025";
                    Session["SchoolName"] = cls.SchoolName.ToUpper();
                    Session["SchoolPhoto"] = cls.SchoolPhoto;
                    Session["SchoolMobileNo"] = cls.SchoolMobile;
                    Session["SchoolEmail"] = cls.SchoolEmail;
                    Session["SchoolAddress"] = cls.SchoolAddress;
                    Session["SchoolId"] = cls.SchoolId;
                    Session["Since"] = cls.Since;
                    Session["UserName"] = cls.UserName;
                    Session["FatherName"] = cls.FatherName;
                    Session["SurName"] = cls.SurName;
                    Session["RoleId"] = cls.RoleId;
                    Session["RoleName"] = cls.RoleName;
                    Session["Mobile"] = cls.Mobile;
                    Session["Address"] = cls.Address;
                    Session["Profile"] = cls.ProfileImg;
                    Session["TeacherId"] = cls.TeacherId;
                    Session["StudentId"] = cls.StudentId;
                    Session["ClassId"] = cls.ClassId;
                    Session["Email"] = cls.Email;

                    Session.Timeout = 22500;
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    if (userInfo.Value == null)
                    {
                        userInfo["UserName"] = cls.Email;
                        Response.Cookies.Add(userInfo);
                        Response.Cookies["userInfo"].Expires = DateTime.Now.AddMonths(2);
                    }
                }
                if (cls.Response == "Success")
                {
                    // Set a success message in TempData to be accessed in the redirected action
                    TempData["SuccessMessage"] = "Logged in successfully.";
                }
                return Json(cls.Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Logout()
        {
            //   _commonModel.InsertActivityLog(clsConstant.ActivityLog, "Logout", "Logout", "Logout", "");
            Session["Id"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["Mobile"] = null;
            Response.Cookies["userInfo"].Expires = DateTime.Now;
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ForgotPassword(AccountModel cls)
        {
            var data = _repository.ForgotPassword(cls);
            if (data.Response == "Success")
            {
                string toEmail = cls.Email;
                var Password = clsCommon.DecryptString(data.Password);
                string subject = "Forgot Password.";
                string body = "";
                using (StreamReader reader = new StreamReader(Server.MapPath("/Data/MailTemplate/ForgotPassword.html")))
                {
                    body = reader.ReadToEnd();
                }
                string imageBase64 = string.Empty;
                string imagePath = string.Empty;

                // Set imagePath and imageBase64 based on the current email being processed
                if (data.ProfileImg == null || data.ProfileImg == "Null" || data.ProfileImg == "undefined")
                {
                    imageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/Data/Profile/dummy.jpg")));
                    imagePath = Server.MapPath("~/Data/Profile/dummy.jpg");
                }
                else
                {
                    imageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("/Data/Profile/" + data.ProfileImg)));
                    imagePath = Server.MapPath("/Data/Profile/" + data.ProfileImg);
                }
                body = body.Replace("[[Profile]]", $"cid:logoImage");
                body = body.Replace("[[UserName]]", data.UserName);
                body = body.Replace("[[EmailId]]", data.Email);
                body = body.Replace("[[Password]]", Password);

                sendEmail(toEmail, subject, body, imagePath);
                cls.Response = "Success";
            }
            else
            {
                cls.Response = "Error";
            }
            return Json(cls.Response, JsonRequestBehavior.AllowGet);
        }
        public void sendEmail(string toEmail, string subject, string body, string imagePath)
        {
            try
            {
                string to = toEmail;

                var EmailConfigaration = _commonModel.EmailConfigaration();
                string host = EmailConfigaration.Host;
                string username = EmailConfigaration.Username;
                string FromEmail = EmailConfigaration.FromMail;
                string password = EmailConfigaration.Password;
                int port = EmailConfigaration.Port;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(FromEmail);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                LinkedResource logo = new LinkedResource(imagePath, "image/jpg");
                logo.ContentId = "logoImage";

                av.LinkedResources.Add(logo);

                mail.AlternateViews.Add(av);

                SmtpClient smtp = new SmtpClient();

                smtp.Host = host;
                smtp.Credentials = new System.Net.NetworkCredential
                     (FromEmail, password);
                smtp.Port = port;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        public ActionResult checkUserSession()
        {
            AccountModel cls = new AccountModel();
            if (Request.Cookies["userinfo"] != null)
            {
                var UserName = Request.Cookies["userinfo"]["UserName"];
                var Password = Request.Cookies["userinfo"]["Password"];
                cls.Email = UserName;
                cls.Password = Password;
                //  cls = cls.CustomerLogin(cls);
                //  cls = CheckLoginCookies(cls);
                cls = _repository.Login(cls);
                if (cls.Id > 0)
                {

                    Session["intId"] = cls.Id;
                    Session["UserName"] = cls.UserName;
                    Session["Email"] = cls.Email;
                    Session["Mobile"] = cls.Mobile;
                    Session["strPassword"] = cls.Password;
                    //Session["ProfilePic"] = "/Data/images/Profilepic/" + cls.strFile;
                    //int id = cls.insertTodayAttendance();
                    //Session["intDailyEntryId"] = id;
                    Session.Timeout = 22500;
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    if (userInfo.Value == null)
                    {
                        userInfo["UserName"] = cls.Email;
                        userInfo["Password"] = clsCommon   .DecryptString(cls.Password);
                        Response.Cookies.Add(userInfo);
                        Response.Cookies["userInfo"].Expires = DateTime.Now.AddMonths(2);
                    }

                }
            }

            return Json(cls, JsonRequestBehavior.AllowGet);
        }
    }
}