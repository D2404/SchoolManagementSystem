﻿using InvoiceManagementSystem.IRepository;
using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace InvoiceManagementSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private readonly clsCommon _commonModel;


        public AccountRepository(clsCommon commonModel)
        {
            _commonModel = commonModel;
        }

        public AccountModel ChangePassword(AccountModel cls)
        {
            AccountModel res = new AccountModel();
            try
            {
                var ddd = clsCommon.DecryptString("QU734hNlS/9lJ6Eof1tOcg==");
                cls.Password = clsCommon.EncryptString(cls.Password);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_ChangePassword", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", _commonModel.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@Password", cls.Password);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    string intRefId = dt.Rows[0][0].ToString();
                    if (intRefId == "1")
                    {
                        res.Response = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                cls.Response = "error";
            }
            return res;
        }

        public AccountModel ForgotPassword(AccountModel cls)
        {
            AccountModel obj = new AccountModel();
            try
            {
                SqlCommand cmd = new SqlCommand("ForgotPassword", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", cls.Email);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Id = Convert.ToInt32(dt.Rows[0]["Id"] == null || dt.Rows[0]["Id"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Id"].ToString());
                    obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                    obj.SurName = dt.Rows[0]["SurName"] == null || dt.Rows[0]["SurName"].ToString().Trim() == "" ? "" : dt.Rows[0]["SurName"].ToString();
                    obj.UserName = dt.Rows[0]["UserName"] == null || dt.Rows[0]["UserName"].ToString().Trim() == "" ? "" : dt.Rows[0]["UserName"].ToString();
                    obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                    obj.Mobile = dt.Rows[0]["Mobile"] == null || dt.Rows[0]["Mobile"].ToString().Trim() == "" ? "" : dt.Rows[0]["Mobile"].ToString();
                    obj.ProfileImg = dt.Rows[0]["Profile"] == null || dt.Rows[0]["Profile"].ToString().Trim() == "" ? "" : dt.Rows[0]["Profile"].ToString();
                    obj.RoleName = dt.Rows[0]["RoleName"] == null || dt.Rows[0]["RoleName"].ToString().Trim() == "" ? "" : dt.Rows[0]["RoleName"].ToString();
                    obj.Password = dt.Rows[0]["Password"] == null || dt.Rows[0]["Password"].ToString().Trim() == "" ? "" : dt.Rows[0]["Password"].ToString();
                    obj.RoleId = Convert.ToInt32(dt.Rows[0]["RoleId"] == null || dt.Rows[0]["RoleId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["RoleId"].ToString());
                    obj.TeacherId = Convert.ToInt32(dt.Rows[0]["TeacherId"] == null || dt.Rows[0]["TeacherId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["TeacherId"].ToString());
                    obj.ClassId = Convert.ToInt32(dt.Rows[0]["ClassId"] == null || dt.Rows[0]["ClassId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ClassId"].ToString());
                    obj.StudentId = Convert.ToInt32(dt.Rows[0]["StudentId"] == null || dt.Rows[0]["StudentId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["StudentId"].ToString());
                    obj.Response = "Success";
                }
                else
                {
                    obj.Response = "Error";
                }

            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return obj;
        }

        public AccountModel Login(AccountModel cls)
        {
            AccountModel obj = new AccountModel();
            // clsCommon objcommon = new clsCommon();
            try
            {
                var ddd = clsCommon.DecryptString("bdJLYJZfIjbrN6NrQxS0ZA==");
                cls.Password = clsCommon.EncryptString(cls.Password);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand("Login", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", cls.UserName);
                cmd.Parameters.AddWithValue("@Password", cls.Password);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Id = Convert.ToInt32(dt.Rows[0]["Id"] == null || dt.Rows[0]["Id"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Id"].ToString());
                    obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                    obj.SurName = dt.Rows[0]["SurName"] == null || dt.Rows[0]["SurName"].ToString().Trim() == "" ? "" : dt.Rows[0]["SurName"].ToString();
                    obj.UserName = dt.Rows[0]["UserName"] == null || dt.Rows[0]["UserName"].ToString().Trim() == "" ? "" : dt.Rows[0]["UserName"].ToString();
                    obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                    obj.Mobile = dt.Rows[0]["Mobile"] == null || dt.Rows[0]["Mobile"].ToString().Trim() == "" ? "" : dt.Rows[0]["Mobile"].ToString();
                    obj.ProfileImg = dt.Rows[0]["Profile"] == null || dt.Rows[0]["Profile"].ToString().Trim() == "" ? "" : dt.Rows[0]["Profile"].ToString();
                    obj.RoleName = dt.Rows[0]["RoleName"] == null || dt.Rows[0]["RoleName"].ToString().Trim() == "" ? "" : dt.Rows[0]["RoleName"].ToString();
                    obj.Password = dt.Rows[0]["Password"] == null || dt.Rows[0]["Password"].ToString().Trim() == "" ? "" : dt.Rows[0]["Password"].ToString();
                    obj.SchoolId = Convert.ToInt32(dt.Rows[0]["SchoolId"] == null || dt.Rows[0]["SchoolId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["SchoolId"].ToString());
                    obj.SchoolName = dt.Rows[0]["SchoolName"] == null || dt.Rows[0]["SchoolName"].ToString().Trim() == "" ? "" : dt.Rows[0]["SchoolName"].ToString();
                    obj.SchoolEmail = dt.Rows[0]["SchoolEmail"] == null || dt.Rows[0]["SchoolEmail"].ToString().Trim() == "" ? "" : dt.Rows[0]["SchoolEmail"].ToString();
                    obj.SchoolMobile = dt.Rows[0]["SchoolMobile"] == null || dt.Rows[0]["SchoolMobile"].ToString().Trim() == "" ? "" : dt.Rows[0]["SchoolMobile"].ToString();
                    obj.SchoolPhoto = dt.Rows[0]["SchoolPhoto"] == null || dt.Rows[0]["SchoolPhoto"].ToString().Trim() == "" ? "" : dt.Rows[0]["SchoolPhoto"].ToString();
                    obj.SchoolAddress = dt.Rows[0]["SchoolAddress"] == null || dt.Rows[0]["SchoolAddress"].ToString().Trim() == "" ? "" : dt.Rows[0]["SchoolAddress"].ToString();
                    obj.Since = dt.Rows[0]["Since"] == null || dt.Rows[0]["Since"].ToString().Trim() == "" ? "" : dt.Rows[0]["Since"].ToString();
                    obj.RoleId = Convert.ToInt32(dt.Rows[0]["RoleId"] == null || dt.Rows[0]["RoleId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["RoleId"].ToString());
                    obj.TeacherId = Convert.ToInt32(dt.Rows[0]["TeacherId"] == null || dt.Rows[0]["TeacherId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["TeacherId"].ToString());
                    obj.ClassId = Convert.ToInt32(dt.Rows[0]["ClassId"] == null || dt.Rows[0]["ClassId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ClassId"].ToString());
                    obj.StudentId = Convert.ToInt32(dt.Rows[0]["StudentId"] == null || dt.Rows[0]["StudentId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["StudentId"].ToString());
                    obj.Response = "Success";
                }
                else
                {
                    obj.Response = "Error";
                }

            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return obj;
        }

        public AccountModel MyProfile(AccountModel cls)
        {
            try
            {
                if (cls.Profile != null && cls.Profile.Length > 0)
                {
                    string Profile = ("Profile_" + cls.Id + "_" + DateTime.Now.Ticks).ToString();
                    string strOriginalFile = cls.Profile[0].FileName;
                    string ext = System.IO.Path.GetExtension(cls.Profile[0].FileName).ToLower();
                    string fileLocation = HttpContext.Current.Server.MapPath("/Data/Profile/");
                    if (!Directory.Exists(fileLocation))
                    {
                        Directory.CreateDirectory(fileLocation);
                    }
                    if (ext == ".jpeg" || ext == ".jpg" || ext == ".png")
                    {
                        Profile = Profile + ext;
                        cls.Profile[0].SaveAs(fileLocation + Profile);
                    }
                    var strPath = fileLocation + cls.Profile;
                    FileInfo file = new FileInfo(strPath);
                    if (file.Exists)//check file exsit or not
                    {
                        file.Delete();
                    }
                    cls.ProfileImg = Profile;
                }
                List<AccountModel> LSTList = new List<AccountModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetMyProfile", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeacherId", cls.TeacherId);
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);
                cmd.Parameters.AddWithValue("@SchoolId", cls.SchoolId);
                cmd.Parameters.AddWithValue("@UserId", cls.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        AccountModel obj = new AccountModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        obj.RoleId = Convert.ToInt32(dt.Rows[i]["RoleId"] == null || dt.Rows[i]["RoleId"].ToString().Trim() == "" ? null : dt.Rows[i]["RoleId"].ToString());
                        obj.UserName = dt.Rows[i]["UserName"] == null || dt.Rows[i]["UserName"].ToString().Trim() == "" ? null : dt.Rows[i]["UserName"].ToString();
                        obj.FatherName = dt.Rows[i]["FatherName"] == null || dt.Rows[i]["FatherName"].ToString().Trim() == "" ? null : dt.Rows[i]["FatherName"].ToString();
                        obj.SurName = dt.Rows[i]["SurName"] == null || dt.Rows[i]["SurName"].ToString().Trim() == "" ? null : dt.Rows[i]["SurName"].ToString();
                        obj.ProfileImg = dt.Rows[i]["Profile"] == null || dt.Rows[i]["Profile"].ToString().Trim() == "" ? null : dt.Rows[i]["Profile"].ToString();
                        obj.Mobile = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.Address = dt.Rows[i]["Address"] == null || dt.Rows[i]["Address"].ToString().Trim() == "" ? null : dt.Rows[i]["Address"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTAccountList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw ex;
            }
        }

        public void sendEmail(string toEmail, string subject, string body, string imagePath)
        {
            try
            {
                string to = toEmail;

                var EmailConfigaration = _commonModel.EmailConfigaration();
                string host = EmailConfigaration.Host;
                string username = EmailConfigaration.Username;
                string FromEmail = EmailConfigaration.FromMail;
                string password = EmailConfigaration.Password;
                int port = EmailConfigaration.Port;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(FromEmail);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                LinkedResource logo = new LinkedResource(imagePath, "image/jpg");
                logo.ContentId = "logoImage";

                av.LinkedResources.Add(logo);

                mail.AlternateViews.Add(av);

                SmtpClient smtp = new SmtpClient();

                smtp.Host = host;
                smtp.Credentials = new System.Net.NetworkCredential
                     (FromEmail, password);
                smtp.Port = port;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {

            }

        }

        public AccountModel UpdateProfile(AccountModel cls)
        {
            AccountModel res = new AccountModel();
            try
            {
                if (cls.Profile != null && cls.Profile.Length > 0)
                {
                    string Profile = ("Profile_" + cls.Id + "_" + DateTime.Now.Ticks).ToString();
                    string strOriginalFile = cls.Profile[0].FileName;
                    string ext = System.IO.Path.GetExtension(cls.Profile[0].FileName).ToLower();
                    string fileLocation = HttpContext.Current.Server.MapPath("/Data/Profile/");
                    if (!Directory.Exists(fileLocation))
                    {
                        Directory.CreateDirectory(fileLocation);
                    }
                    if (ext == ".jpeg" || ext == ".jpg" || ext == ".png")
                    {
                        Profile = Profile + ext;
                        cls.Profile[0].SaveAs(fileLocation + Profile);
                    }
                    var strPath = fileLocation + cls.Profile;
                    FileInfo file = new FileInfo(strPath);
                    if (file.Exists)//check file exsit or not
                    {
                        file.Delete();
                    }
                    cls.ProfileImg = Profile;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_UpdateProfile", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", _commonModel.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@UserName", cls.UserName);
                cmd.Parameters.AddWithValue("@FatherName", cls.FatherName);
                cmd.Parameters.AddWithValue("@SurName", cls.SurName);
                cmd.Parameters.AddWithValue("@MobileNo", cls.Mobile);
                cmd.Parameters.AddWithValue("@Email", cls.Email);
                cmd.Parameters.AddWithValue("@Address", cls.Address);
                cmd.Parameters.AddWithValue("@Profile", cls.ProfileImg);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    string intRefId = dt.Rows[0][0].ToString();
                    if (intRefId == "1")
                    {
                        res.Response = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                cls.Response = "error";
            }
            return res;
        }
    }
}