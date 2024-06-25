using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class TeacherAttandenceModel
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public int MonthId { get; set; }

        public string strerrorMessage { get; set; }
        public HttpPostedFileBase[] file { get; set; }
        public int YearId { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public string TotalPresentDays { get; set; }
        public string TotalAbsentDays { get; set; }
        public string FatherName { get; set; }
        public string SurName { get; set; }
        public int LeaveType { get; set; }
        public string Reason { get; set; }
        public string TeacherId { get; set; }
        public string Status { get; set; }
        public int intActive { get; set; }
        
        public string Date { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
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
        public Pager Pager { get; set; }

        public List<TeacherAttandenceModel> LSTTeacherAttandenceList { get; set; }
    }

}