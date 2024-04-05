using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class EmailConfigurationSetting
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public string Host { get; set; }
        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromMail { get; set; }
        public int Port { get; set; }
        public int TeacherId { get; set; }
        public int TeacherId1 { get; set; }
        public string TeacherName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Response { get; set; }
        public string SearchText { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecord { get; set; }
        public decimal? PageCount { get; set; }
        public long? ROWNUMBER { get; set; }
        public int TotalEntries { get; set; }
        public int ShowingEntries { get; set; }
        public int fromEntries { get; set; }
        public Pager Pager { get; set; }
        public List<EmailConfigurationSetting> LSTEmailConfigurationList { get; set; }
        public List<RoleModel> LSTRoleList { get; set; }
         
        public class TeachersList
        {
            public int TeacherId { get; set; }
        }

        public EmailConfigurationSetting addEmailConfiguration(EmailConfigurationSetting cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateEmailConfiguration", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.AddWithValue("@RoleName", cls.Rolename);
                cmd.Parameters.AddWithValue("@UserName", cls.Username);
                cmd.Parameters.AddWithValue("@Password", cls.Password);
                cmd.Parameters.AddWithValue("@FromMail", cls.FromMail);
                cmd.Parameters.AddWithValue("@TeacherId", cls.TeacherId);
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);

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
                        cls.Response = "Success";
                    }
                    else if (intRefId == "2")
                    {
                        cls.Response = "Updated";
                    }
                    else if (intRefId == "-1")
                    {
                        cls.Response = "Exists";
                    }
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return cls;
        }

        public EmailConfigurationSetting GetEmailConfiguration(EmailConfigurationSetting cls, int? Id)
        {
            try
            {
                List<EmailConfigurationSetting> LSTList = new List<EmailConfigurationSetting>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetSingleEmailConfiguration", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Rolename = dt.Rows[i]["Rolename"] == null || dt.Rows[i]["Rolename"].ToString().Trim() == "" ? null : dt.Rows[i]["Rolename"].ToString();
                        obj.Username = dt.Rows[i]["Username"] == null || dt.Rows[i]["Username"].ToString().Trim() == "" ? null : dt.Rows[i]["Username"].ToString();
                        obj.FromMail = dt.Rows[i]["FromMail"] == null || dt.Rows[i]["FromMail"].ToString().Trim() == "" ? null : dt.Rows[i]["FromMail"].ToString();
                        obj.Password = dt.Rows[i]["Password"] == null || dt.Rows[i]["Password"].ToString().Trim() == "" ? null : dt.Rows[i]["Password"].ToString();
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["TeacherId"] == null || dt.Rows[i]["TeacherId"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        LSTList.Add(obj);
                    }
                }
                cls.LSTEmailConfigurationList = LSTList;
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
        public EmailConfigurationSetting FillTeacherList(EmailConfigurationSetting cls)
        {
            try
            {
                List<EmailConfigurationSetting> LSTList = new List<EmailConfigurationSetting>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("FillTeacherDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        EmailConfigurationSetting obj = new EmailConfigurationSetting();
                        obj.TeacherId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTEmailConfigurationList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmailConfigurationSetting deleteEmailConfiguration(EmailConfigurationSetting cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETEEmailConfiguration", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", cls.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows[0][0].ToString() == "1")
                {
                    cls.Response = "Success";
                }
                else if (dt.Rows[0][0].ToString() == "2")
                {
                    cls.Response = "dependency";
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return cls;
        }


        public DataTable ExportEmailConfiguration(EmailConfigurationSetting cls)
        {
            try
            {
                List<EmailConfigurationSetting> LSTList = new List<EmailConfigurationSetting>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 9);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (cls.TeacherId == 0 && cls.StudentId == 0)
                        {// HTML Tags Code Remove.
                            dr["Rolename"] = Regex.Replace(dr["Rolename"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Username"] = Regex.Replace(dr["Username"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["FromMail"] = Regex.Replace(dr["FromMail"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Password"] = Regex.Replace(dr["Password"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Host"] = Regex.Replace(dr["Host"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Port"] = Regex.Replace(dr["Port"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        }
                        else if (cls.TeacherId != 0 && cls.StudentId == 0)
                        {
                            dr["Rolename"] = Regex.Replace(dr["Rolename"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Username"] = Regex.Replace(dr["Username"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["FromMail"] = Regex.Replace(dr["FromMail"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Password"] = Regex.Replace(dr["Password"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Host"] = Regex.Replace(dr["Host"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Port"] = Regex.Replace(dr["Port"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["TeacherName"] = Regex.Replace(dr["TeacherName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        }
                        else
                        {
                            dr["Rolename"] = Regex.Replace(dr["Rolename"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Username"] = Regex.Replace(dr["Username"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["FromMail"] = Regex.Replace(dr["FromMail"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Password"] = Regex.Replace(dr["Password"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Host"] = Regex.Replace(dr["Host"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["Port"] = Regex.Replace(dr["Port"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["StudentName"] = Regex.Replace(dr["StudentName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                            dr["TeacherName"] = Regex.Replace(dr["TeacherName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        }
                    }
                }

                return dt;
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
    }
}