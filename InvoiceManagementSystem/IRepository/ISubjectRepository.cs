using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface ISubjectRepository
    {
        SubjectModel GetAllSubject(SubjectModel cls);
        SubjectModel AddSubject(SubjectModel cls);
        SubjectModel GetSingleSubject(SubjectModel cls);
        SubjectModel DeleteSubject(SubjectModel cls);
        string UpdateStatus(SubjectModel cls);
        DataTable ExportSubject(SubjectModel cls);
    }
}
