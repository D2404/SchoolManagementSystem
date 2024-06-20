using InvoiceManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

namespace InvoiceManagementSystem.Models
{
    public static class UnityConfig
    {
        public static void RegisterComponents(IUnityContainer container)
        {
            // Register all your components with the container here
            // This is an example registration
            container.RegisterType<ClassRoomRepository>();
            container.RegisterType<clsCommon>();

            // Register your controllers as well if needed
            // container.RegisterType<ClassRoomController>();
        }
    }
}