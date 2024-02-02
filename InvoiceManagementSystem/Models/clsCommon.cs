using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Models
{
    public class clsCommon
    {
        private static string key = "z20ds5898a4e5523bbce3ea1025a1916";
        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            cipherText = cipherText.Replace(" ", "+");
            byte[] buffer = Convert.FromBase64String(cipherText);
            cipherText = cipherText.Replace("+", " ");
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string RedirectToLogin(int type = 0)
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                string url = request.Url.ToString();
                string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                string ReturnUrl = "";
                if (type == 1)
                {
                    ReturnUrl = domainName + "/Home/Index/index?returnUrl=" + url;
                }
                //else if (type == 2)
                //{
                //    ReturnUrl = domainName + "/Company/index?returnUrl=" + url;
                //}
                //else
                //{
                //    ReturnUrl = domainName + "/admin/home/index?returnUrl=" + url;
                //}
                return ReturnUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int? getUserIdFromSession()
        {
            int? Id = 0;
            if (HttpContext.Current.Session["Id"] != null && HttpContext.Current.Session["Id"] != "")
            {
                Id = Convert.ToInt32(HttpContext.Current.Session["Id"]);
            }
            return Id;
        }
        public int? getRoleIdFromSession()
        {
            int? Id = 0;
            if (HttpContext.Current.Session["RoleId"] != null && HttpContext.Current.Session["RoleId"] != "")
            {
                Id = Convert.ToInt32(HttpContext.Current.Session["RoleId"]);
            }
            return Id;
        }
        public int? getTeacherIdFromSession()
        {
            int? Id = 0;
            if (HttpContext.Current.Session["TeacherId"] != null && HttpContext.Current.Session["TeacherId"] != "")
            {
                Id = Convert.ToInt32(HttpContext.Current.Session["TeacherId"]);
            }
            return Id;
        }
        public int? getStudentIdFromSession()
        {
            int? Id = 0;
            if (HttpContext.Current.Session["StudentId"] != null && HttpContext.Current.Session["StudentId"] != "")
            {
                Id = Convert.ToInt32(HttpContext.Current.Session["StudentId"]);
            }
            return Id;
        }
        public int? getClassIdFromSession()
        {
            int? Id = 0;
            if (HttpContext.Current.Session["ClassId"] != null && HttpContext.Current.Session["ClassId"] != "")
            {
                Id = Convert.ToInt32(HttpContext.Current.Session["ClassId"]);
            }
            return Id;
        }
        public int? getComapanyUserIdFromSession()
        {
            int? intid = 0;
            if (HttpContext.Current.Session["CompanyUserId"] != null && HttpContext.Current.Session["CompanyUserId"] != "")
            {
                intid = Convert.ToInt32(HttpContext.Current.Session["CompanyUserId"]);
            }

            return intid;
        }
        public int? getLeaveCountFromSession()
        {
            int? intid = 0;
            if (HttpContext.Current.Session["LeaveCount"] != null && HttpContext.Current.Session["LeaveCount"] != "")
            {
                intid = Convert.ToInt32(HttpContext.Current.Session["LeaveCount"]);
            }

            return intid;
        }


        public string convertMMDDYYYY(string date)
        {
            if (date != null)
            {
                if (date.Contains('/'))
                {
                    var nDate = date.Split('/');
                    date = nDate[1] + '/' + nDate[0] + '/' + nDate[2];
                }
                if (date.Contains('-'))
                {
                    var nDate = date.Split('-');
                    date = nDate[1] + '-' + nDate[0] + '-' + nDate[2];
                }
            }
            return date;
        }


        public IEnumerable<SelectListItem> FillClassRoom()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<ClassRoomModel> objDistrictModel = new List<ClassRoomModel>();

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                             CommandType.StoredProcedure, "Sp_GetClassRoomList"
                            );

            //objDistrictModel = ManageCollection.ToCollection<DistrictModel>(ds.Tables[0]);

            var cat = objDistrictModel.ToArray();


            if (cat.Length >= 1 || cat.Length == 0)
                list.Add(new SelectListItem { Text = "Select ClassRoom", Value = "-1" });

            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].ClassNo,
                    Value = cat[i].Id.ToString()
                });
            }
            return list;
        }

        public IEnumerable<SelectListItem> Fill_Subject(int? ClassId = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<SubjectModel> objDistrictModel = new List<SubjectModel>();

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                             CommandType.StoredProcedure, "Sp_GetSubjectModelList",
                             new SqlParameter("@ClassId", ClassId));

            //objDistrictModel = ManageCollection.ToCollection<ClassRoomModel>(ds.Tables[0]);

            var cat = objDistrictModel.ToArray();


            if (cat.Length >= 1 || cat.Length == 0)
                list.Add(new SelectListItem { Text = "Select Block", Value = "-1" });

            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].SubjectName,
                    Value = cat[i].Id.ToString()
                });
            }
            return list;
        }
    }
}