using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface ITeacherRepository
    {
        TeacherModel GetAllTeacher(TeacherModel cls);
        TeacherModel AddTeacher(TeacherModel cls);
        TeacherModel GetSingleTeacher(TeacherModel cls, int? Id);
        TeacherModel DeleteTeacher(TeacherModel cls);
        string UpdateStatus(TeacherModel cls);
        DataTable ExportTeacher(TeacherModel cls);
        TeacherModel FillSingleClassRoom(TeacherModel cls);
        TeacherModel FillClassRoomList(TeacherModel cls);
        bool CheckEmailInBulkUpdate(string strEmail);
        TeacherModel WelcomeMail(TeacherModel cls);
    }
}
