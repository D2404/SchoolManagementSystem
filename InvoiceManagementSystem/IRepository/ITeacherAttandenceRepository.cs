using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface ITeacherAttandenceRepository
    {
        TeacherAttandenceModel GetTeacherAttandence(TeacherAttandenceModel model);
        TeacherAttandenceModel AddTeacherAttandence(TeacherAttandenceModel model);
        TeacherAttandenceModel GetSingleTeacherAttandence(TeacherAttandenceModel model);
        TeacherAttandenceModel deleteTeacherAttandence(TeacherAttandenceModel model);
        DataTable ExportTeacherAttendance(TeacherAttandenceModel model);

    }
}
