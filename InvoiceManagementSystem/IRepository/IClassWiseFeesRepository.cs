using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IClassWiseFeesRepository
    {
        FeesModel GetAllClassWiseFees(FeesModel model);
        FeesModel AddClassWiseFees(FeesModel model);
        FeesModel GetSingleClassWiseFees(FeesModel model);
        FeesModel DeleteClassWiseFees(FeesModel model);
        string UpdateStatus(FeesModel model);
        DataTable ExportClassWiseFees(FeesModel model);
    }
}
