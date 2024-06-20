using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class TeacherModel
    {
        clsCommon objCommon = new clsCommon();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public string TeacherUniqueId { get; set; }
        public int RoleId { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public string ClassNo { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string TeacherName { get; set; }
        public string FatherName { get; set; }
        public string Surname { get; set; }
        public string Dob { get; set; }
        public string MobileNo { get; set; }
        public string AlternateMobileNo { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string Education { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfLeaving { get; set; }

        public string MaritalStatus { get; set; }
        public string AnniversaryDate { get; set; }
        public string Experience { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentPincode { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string PermenantAddress { get; set; }
        public string PermenantPincode { get; set; }
        public string PermenantCity { get; set; }
        public string PermenantState { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string Response { get; set; }
        public string SearchText { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecord { get; set; }
        public decimal? PageCount { get; set; }
        public long? ROWNUMBER { get; set; }
        public int TotalEntries { get; set; }
        public string CreatedDate { get; set; }
        public int ShowingEntries { get; set; }
        public int fromEntries { get; set; }
        public string Date { get; set; }
        public string TempEmail { get; set; }
        public int intActive { get; set; }
        public bool IsActive { get; set; }
       
        public Pager Pager { get; set; }
        public HttpPostedFileBase[] Profile { get; set; }
        public string ProfileImg { get; set; }

        public string HiddenfileForImage { get; set; }
        public string ErrorMessage { get; set; }
        public HttpPostedFileBase[] file { get; set; }

        public List<TeacherModel> LSTTeacherList { get; set; }
        public List<ClassRoomModel> LSTClassRoomList { get; set; }

    }
}