using InvoiceManagementSystem.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        clsCommon objCommon = new clsCommon();
        // GET: Account

        public ActionResult MyProfile(AccountModel cls)
        {

            int? TeacherId = objCommon.getTeacherIdFromSession();
            int? UserId = objCommon.getUserIdFromSession();
            int? StudentId = objCommon.getStudentIdFromSession();
            if (TeacherId > 0 || UserId > 0 || StudentId > 0)
            {
                cls.TeacherId = TeacherId.Value;
                cls.Id = UserId.Value;
                cls.StudentId = StudentId.Value;
                cls = cls.MyProfile(cls);
                return View(cls);
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }

        }




        public ActionResult GetMyProfile(AccountModel cls)
        {
            cls.Id = objCommon.getUserIdFromSession();
            cls = cls.MyProfile(cls);
            return Json(cls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateProfile(AccountModel cls)
        {
            //Parameter param = new Parameter();
            cls = cls.UpdateProfile(cls);
            return Json(cls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePassword(AccountModel cls)
        {
            //Parameter param = new Parameter();
            cls = cls.ChangePassword(cls);
            return Json(cls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ForgotPassword(AccountModel cls)
        {
            try
            {
                cls = cls.ForgotPassword(cls);
                var Password = clsCommon.DecryptString(cls.Password);
                string toEmail = cls.Email;
                string mailHeading = "Forgot password";
                string subject = "Recover your password";
                string message = "your password is =" + Password;
                SendMail(mailHeading, subject, message, toEmail);
                return Json(cls.Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SendMail(string mailHeading, string subject, string message, string toEmail)
        {
            //try
            //{
            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            //mail.From = new MailAddress(mailHeading + " <panchaldhruv1424@gmail.com>");
            //mail.To.Add(toEmail);
            //mail.Subject = subject;
            //mail.Body = message;
            //mail.IsBodyHtml = true;

            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("panchaldhruv1424@gmail.com", "Your-Zoho-App-Password");
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);

            var fromAddress = new MailAddress("mmchauhan1906@gmail.com");
            var fromPassword = "9737954396";
            var toAddress = new MailAddress("panchaldhruv1424@gmail.com");

            string xsubject = "subject";
            string body = "body";

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var xmessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = xsubject,
                Body = body
            })

                smtp.Send(xmessage);

            //}
            //catch (Exception ex)
            //{
            //    // Handle the exception, log it, or rethrow if needed
            //    throw ex;
            //}
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Login(AccountModel cls)
        {
            try
            {
                cls = cls.Login(cls);
                if (cls.Id > 0)
                {
                    Session["Id"] = cls.Id;
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
            //  objCommon.InsertActivityLog(clsConstant.ActivityLog, "Logout", "Logout", "Logout", "");
            Session["Id"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["Mobile"] = null;
            Response.Cookies["userInfo"].Expires = DateTime.Now;
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
                cls = cls.Login(cls);
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
                        userInfo["Password"] = clsCommon.DecryptString(cls.Password);
                        Response.Cookies.Add(userInfo);
                        Response.Cookies["userInfo"].Expires = DateTime.Now.AddMonths(2);
                    }

                }
            }

            return Json(cls, JsonRequestBehavior.AllowGet);
        }
    }
}