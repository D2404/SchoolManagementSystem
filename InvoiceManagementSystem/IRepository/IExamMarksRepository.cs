using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IExamMarksRepository
    {
        ExamModel GetAllExamMarks(ExamModel cls);
        ExamModel AddExamMarks(ExamModel cls);
        ExamModel GetSingleExamMarks(ExamModel cls);
        ExamModel DeleteExamMarks(ExamModel cls);
        string UpdateStatus(ExamModel cls);
        DataTable ExportExamMarks(ExamModel cls);
    }
}
