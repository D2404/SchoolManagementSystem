using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IExamDetailsRepository
    {
        ExamModel GetAllExamDetails(ExamModel cls);
        ExamModel AddExamDetails(ExamModel cls);
        ExamModel GetSingleExamDetails(ExamModel cls);
        ExamModel DeleteExamDetails(ExamModel cls);
        string UpdateStatus(ExamModel cls);
        DataTable ExportExamDetails(ExamModel cls);
    }
}
