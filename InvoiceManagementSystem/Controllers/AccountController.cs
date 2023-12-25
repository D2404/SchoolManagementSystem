using InvoiceManagementSystem.Models;
using System;
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
            if (objCommon.getUserIdFromSession() != 0)
            {
                cls.Id = objCommon.getUserIdFromSession();
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
                    Session["FullName"] = cls.FullName;
                    Session["RoleId"] = cls.RoleId;
                    Session["RoleName"] = cls.RoleName;
                    Session["Mobile"] = cls.Mobile;
                    Session["Address"] = cls.Address;
                    Session["Profile"] = cls.ProfileImg;
                    Session["TeacherId"] = cls.TeacherId;
                    Session["StudentId"] = cls.StudentId;
                    Session["ClassId"] = cls.ClassId;
                    
                    //  Session["strPassword"] = res.strPassword;
                    //Session["UserId"] = .intId;
                    //Session["ProfilePic"] = "/Data/images/Profilepic/" + cls.strFile;
                    //int id = cls.insertTodayAttendance();
                    //Session["intDailyEntryId"] = id;
                    Session.Timeout = 22500;
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    if (userInfo.Value == null)
                    {
                        userInfo["UserName"] = cls.Email;
                        //userInfo["Password"] = clsCommon.DecryptString(res.strPassword);
                        Response.Cookies.Add(userInfo);
                        Response.Cookies["userInfo"].Expires = DateTime.Now.AddMonths(2);
                    }

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