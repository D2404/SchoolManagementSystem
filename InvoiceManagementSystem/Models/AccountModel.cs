using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class AccountModel
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        clsCommon objCommon = new clsCommon();
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string FatherName { get; set; }
        public string SurName { get; set; }
        public HttpPostedFileBase[] Profile { get; set; }
        public HttpPostedFileBase[] PhotoImg { get; set; }


        public HttpPostedFileBase[] hdnProfile { get; set; }
        public string ProfileImg { get; set; }
        public HttpPostedFileBase[] hdnSchoolPhoto { get; set; }

        public string SchoolPhoto { get; set; }

        public string SchoolEmail { get; set; }
        public string SchoolMobile { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string Since { get; set; }
        public string SchoolAddress { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }

        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int LeaveCount { get; set; }
        public string ClassNo { get; set; }
        public HttpPostedFileBase[] strFile { get; set; }
        public string Response { get; set; }
        public List<AccountModel> LSTAccountList { get; set; }
    }
}