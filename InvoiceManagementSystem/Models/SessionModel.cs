using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class SessionModel
    {
        public static int RoleId
        {
            get { return HttpContext.Current.Session["RoleId"] == null ? 0 : (int)HttpContext.Current.Session["RoleId"]; }
            set { HttpContext.Current.Session["RoleId"] = value; }
        }
        public static string SchoolPhoto
        {
            get { return HttpContext.Current.Session["SchoolPhoto"] == null ? "" : (string)HttpContext.Current.Session["SchoolPhoto"]; }
            set { HttpContext.Current.Session["v"] = value; }
        }
        public static int SchoolId
        {
            get { return HttpContext.Current.Session["SchoolId"] == null ? 0 : (int)HttpContext.Current.Session["SchoolId"]; }
            set { HttpContext.Current.Session["SchoolId"] = value; }
        }
        public static int TeacherId
        {
            get { return HttpContext.Current.Session["TeacherId"] == null ? 0 : (int)HttpContext.Current.Session["TeacherId"]; }
            set { HttpContext.Current.Session["TeacherId"] = value; }
        }
        public static int UserId
        {
            get { return HttpContext.Current.Session["UserId"] == null ? 0 : (int)HttpContext.Current.Session["UserId"]; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        public static int ClassId
        {
            get { return HttpContext.Current.Session["ClassId"] == null ? 0 : (int)HttpContext.Current.Session["ClassId"]; }
            set { HttpContext.Current.Session["ClassId"] = value; }
        }
        public static string Username
        {
            get { return HttpContext.Current.Session["Username"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["Username"])); }
            set { HttpContext.Current.Session["Username"] = value; }
        }
        public static string Email
        {
            get { return HttpContext.Current.Session["Email"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["Email"])); }
            set { HttpContext.Current.Session["Email"] = value; }
        }
        public static string Fullname
        {
            get { return HttpContext.Current.Session["Fullname"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["Fullname"])); }
            set { HttpContext.Current.Session["Fullname"] = value; }
        }
        public static string RoleName
        {
            get { return HttpContext.Current.Session["RoleName"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["RoleName"])); }
            set { HttpContext.Current.Session["RoleName"] = value; }
        }
        public static string Profile
        {
            get { return HttpContext.Current.Session["Profile"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["Profile"])); }
            set { HttpContext.Current.Session["Profile"] = value; }
        }
        public static string Address
        {
            get { return HttpContext.Current.Session["Address"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["Address"])); }
            set { HttpContext.Current.Session["Address"] = value; }
        }

        public static string Mobile
        {
            get { return HttpContext.Current.Session["Mobile"] == null ? "" : (Convert.ToString(HttpContext.Current.Session["Mobile"])); }
            set { HttpContext.Current.Session["Mobile"] = value; }
        }




        public static void ClearSession()
        {
            System.Web.HttpContext.Current.Session.Remove("ROLE_ID");
            System.Web.HttpContext.Current.Session.Clear();
        }
    }
}