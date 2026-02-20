using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface ITeacherSubjectRepository
    {
        TeacherSubjectModel GetAllTeacherSubject(TeacherSubjectModel cls);
        TeacherSubjectModel AddTeacherSubject(TeacherSubjectModel cls);
        TeacherSubjectModel GetSingleTeacherSubject(TeacherSubjectModel cls);
        TeacherSubjectModel DeleteTeacherSubject(TeacherSubjectModel cls);
        string UpdateStatus(TeacherSubjectModel cls);
        DataTable ExportTeacherSubject(TeacherSubjectModel cls);
    }
}
