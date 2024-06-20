using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IClassSectionRepository
    {
        ClassSectionModel GetAllClassSection(ClassSectionModel cls);
        ClassSectionModel AddClassSection(ClassSectionModel cls);
        ClassSectionModel GetSingleClassSection(ClassSectionModel cls);
        ClassSectionModel DeleteClassSection(ClassSectionModel cls);
        string UpdateStatus(ClassSectionModel cls);
        DataTable ExportClassSection(ClassSectionModel cls);
    }
}
