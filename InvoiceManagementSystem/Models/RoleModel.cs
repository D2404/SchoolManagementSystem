using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<RoleModel> LSTRoleList { get; set; }
    }
}