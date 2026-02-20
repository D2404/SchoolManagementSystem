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
    public class ExamModel
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Performance { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Grade { get; set; }
        public int RollNo { get; set; }
        public int TotalMarks { get; set; }
        public int OutOfMarks { get; set; }
        public string ClassNo { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public int intActive { get; set; }
        public bool IsActive { get; set; }
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
        public List<ExamModel> LSTExamList { get; set; }
    }
}