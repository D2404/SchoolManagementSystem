using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
   public interface ISectionRepository
    {
        SectionModel GetAllSection(SectionModel cls);
        SectionModel AddSection(SectionModel cls);
        SectionModel GetSingleSection(SectionModel cls);
        SectionModel DeleteSection(SectionModel cls);
        string UpdateStatus(SectionModel cls);
        DataTable ExportSection(SectionModel cls);
    }
}
