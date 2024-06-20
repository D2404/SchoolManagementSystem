using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public static class ConstantModel
    {
        public static int ADMIN_TYPE = 0;
        public static int CUSTOMER_TYPE = 2;
        public static int CUSTOMER_Client_TYPE = 3;
        public static int CUSTOMER_USER_TYPE = 4;
        public static int Client_USER_TYPE = 5;

        public static DateTime CURRENT_DATE = DateTime.Now;

        public static string MESSAGE_SUCCESS = "success";
        public static string MESSAGE_OK = "ok";
        public static string MESSAGE_ERROR = "error";
        public static string MESSAGE_FAIL = "fail";
        public static string MESSAGE_DEPENDENCY = "dependency";
        public static string MESSAGE_EXISTS = "exists";
        public static string MESSAGE_UPDATE = "update"; // by ekta
        public static string PROJECT_SUPPORT_SYSTEM = "supportsystem";
        public static string MODULE_COMPANY = "company";
        public static string MODULE_CLIENT = "client";
        public static string MODULE_ADMIN = "admin";

        public static int intProductId = 1;

        public static int Login = 1;
        public static int ActivityLog = 2;

        public static int InvoicePaidStatus = 1; //add by Priyanka used when invoice paid
    }
}