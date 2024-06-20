using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IClassRoomRepository
    {
        ClassRoomModel GetAllClassRoom(ClassRoomModel cls);
        ClassRoomModel AddClassRoom(ClassRoomModel cls);
        ClassRoomModel GetSingleClassRoom(ClassRoomModel cls);
        ClassRoomModel DeleteClassRoom(ClassRoomModel cls);
        string UpdateStatus(ClassRoomModel cls);
        DataTable ExportClassRoom(ClassRoomModel cls);
    }
}
