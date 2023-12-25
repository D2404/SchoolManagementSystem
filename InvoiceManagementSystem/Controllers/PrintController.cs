using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class PrintController : Controller
    {
        clsCommon objCommon = new clsCommon();
        // GET: ClassRoom

        public ActionResult PrintData()
        {
            List<ClassRoomModel> data = new List<ClassRoomModel>();

            // Call the stored procedure with the provided parameter to retrieve the dynamic data
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("ClassRoomExportData", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add the parameter to the command

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create an instance of YourModel and populate it with the data from the reader
                            ClassRoomModel item = new ClassRoomModel
                            {
                                ClassNo = reader["ClassNo"].ToString()
                               
                            };

                            data.Add(item);
                        }
                    }
                }
            }

            return PartialView("_PrintData", data);
        }
    }
}