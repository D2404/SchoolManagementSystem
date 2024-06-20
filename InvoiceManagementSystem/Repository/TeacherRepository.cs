using InvoiceManagementSystem.IRepository;
using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public TeacherModel GetAllTeacher(TeacherModel model)
         {
            try
            {

                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<TeacherModel> lstTeacherList = new List<TeacherModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetTeacherList", conn);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", model.PageIndex);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@FromDate", model.Date);
                cmd.Parameters.AddWithValue("@ToDate", model.Date);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@intActive", model.intActive);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                conn.Close();


                if (dt != null && dt.Rows.Count > 0)
                {

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        TeacherModel obj = new TeacherModel();
                        obj.ProfileImg = dt.Rows[i]["Profile"] == null ? "" : dt.Rows[i]["Profile"].ToString();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"] == null || dt.Rows[i]["IsActive"].ToString().Trim() == "" ? null : dt.Rows[i]["IsActive"].ToString());
                        obj.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.CurrentAddress = dt.Rows[i]["CurrentAddress"] == null || dt.Rows[i]["CurrentAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentAddress"].ToString();
                        obj.Dob = dt.Rows[i]["Dob"] == null || dt.Rows[i]["Dob"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Dob"]).ToString("dd/MM/yyyy");
                        obj.Education = dt.Rows[i]["Education"] == null || dt.Rows[i]["Education"].ToString().Trim() == "" ? null : dt.Rows[i]["Education"].ToString();
                        obj.Gender = dt.Rows[i]["Gender"] == null || dt.Rows[i]["Gender"].ToString().Trim() == "" ? null : dt.Rows[i]["Gender"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstTeacherList.Add(obj);
                    }
                }
                model.LSTTeacherList = lstTeacherList;
                if (model.LSTTeacherList.Count > 0)
                {
                    var pager = new Models.Pager((int)model.LSTTeacherList[0].TotalRecord, model.PageIndex, (int)model.PageSize);

                    model.Pager = pager;
                }
                model.TotalEntries = TotalEntries;
                model.ShowingEntries = showingEntries;
                model.fromEntries = startentries;
                model.LSTTeacherList = lstTeacherList;

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TeacherModel AddTeacher(TeacherModel cls)
        {
            try
            {
                if (cls.Profile != null && cls.Profile.Length > 0)
                {
                    string Profile = ("TeacherProfile_" + cls.Id + "_" + DateTime.Now.Ticks).ToString();
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
                var ddd = clsCommon.DecryptString("QU734hNlS/9lJ6Eof1tOcg==");
                cls.Password = clsCommon.EncryptString(cls.Password);
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateTeacher", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = cls.Title;
                cmd.Parameters.Add("@TeacherName", SqlDbType.VarChar).Value = cls.TeacherName;
                cmd.Parameters.Add("@FatherName", SqlDbType.VarChar).Value = cls.FatherName;
                cmd.Parameters.Add("@Surname", SqlDbType.VarChar).Value = cls.Surname;
                cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = cls.Dob;
                cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = cls.Gender;
                cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = cls.BloodGroup;
                cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = cls.MobileNo;
                cmd.Parameters.Add("@AlternateMobileNo", SqlDbType.VarChar).Value = cls.AlternateMobileNo;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = cls.Email;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = cls.Password;
                cmd.Parameters.Add("@Education", SqlDbType.VarChar).Value = cls.Education;
                cmd.Parameters.Add("@DateOfJoining", SqlDbType.DateTime).Value = cls.DateOfJoining;
                cmd.Parameters.Add("@DateOfLeaving", SqlDbType.DateTime).Value = cls.DateOfLeaving;
                cmd.Parameters.Add("@MaritalStatus", SqlDbType.VarChar).Value = cls.MaritalStatus;
                cmd.Parameters.Add("@AnniversaryDate", SqlDbType.DateTime).Value = cls.AnniversaryDate;
                cmd.Parameters.Add("@Experience", SqlDbType.VarChar).Value = cls.Experience;
                cmd.Parameters.Add("@CurrentAddress", SqlDbType.VarChar).Value = cls.CurrentAddress;
                cmd.Parameters.Add("@CurrentPincode", SqlDbType.VarChar).Value = cls.CurrentPincode;
                cmd.Parameters.Add("@CurrentCity", SqlDbType.VarChar).Value = cls.CurrentCity;
                cmd.Parameters.Add("@CurrentState", SqlDbType.VarChar).Value = cls.CurrentState;
                cmd.Parameters.Add("@PermenantAddress", SqlDbType.VarChar).Value = cls.PermenantAddress;
                cmd.Parameters.Add("@PermenantPincode", SqlDbType.VarChar).Value = cls.PermenantPincode;
                cmd.Parameters.Add("@PermenantCity", SqlDbType.VarChar).Value = cls.PermenantCity;
                cmd.Parameters.Add("@PermenantState", SqlDbType.VarChar).Value = cls.PermenantState;
                cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = cls.BankName;
                cmd.Parameters.Add("@BankBranch", SqlDbType.VarChar).Value = cls.BankBranch;
                cmd.Parameters.Add("@AccountNo", SqlDbType.VarChar).Value = cls.AccountNo;
                cmd.Parameters.Add("@IFSCCode", SqlDbType.VarChar).Value = cls.IFSCCode;
                cmd.Parameters.AddWithValue("@Profile", SqlDbType.VarChar).Value = cls.ProfileImg;
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@RoleId", cls.RoleId);
                cmd.Parameters.AddWithValue("@ClassId", cls.ClassId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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
        public TeacherModel GetSingleTeacher(TeacherModel cls, int? Id)
        {
            try
            {
                conn.Open();
                List<TeacherModel> lst = new List<TeacherModel>();
                SqlCommand cmd = new SqlCommand("sp_GetSingleTeacher", conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da1.Fill(dt);
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        TeacherModel obj = new TeacherModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Title = dt.Rows[i]["Title"] == null || dt.Rows[i]["Title"].ToString().Trim() == "" ? null : dt.Rows[i]["Title"].ToString();
                        obj.ProfileImg = dt.Rows[i]["Profile"] == null || dt.Rows[i]["Profile"].ToString().Trim() == "" ? null : dt.Rows[i]["Profile"].ToString();
                        obj.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        obj.FatherName = dt.Rows[i]["FatherName"] == null || dt.Rows[i]["FatherName"].ToString().Trim() == "" ? null : dt.Rows[i]["FatherName"].ToString();
                        obj.Surname = dt.Rows[i]["Surname"] == null || dt.Rows[i]["Surname"].ToString().Trim() == "" ? null : dt.Rows[i]["Surname"].ToString();
                        obj.Gender = dt.Rows[i]["Gender"] == null || dt.Rows[i]["Gender"].ToString().Trim() == "" ? null : dt.Rows[i]["Gender"].ToString();
                        obj.BloodGroup = dt.Rows[i]["BloodGroup"] == null || dt.Rows[i]["BloodGroup"].ToString().Trim() == "" ? null : dt.Rows[i]["BloodGroup"].ToString();
                        obj.Dob = dt.Rows[i]["Dob"] == null || dt.Rows[i]["Dob"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Dob"]).ToString("yyyy-MM-dd");
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.AlternateMobileNo = dt.Rows[i]["AlternateMobileNo"] == null || dt.Rows[i]["AlternateMobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["AlternateMobileNo"].ToString();
                        obj.DateOfJoining = dt.Rows[i]["DateOfJoining"] == null || dt.Rows[i]["DateOfJoining"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["DateOfJoining"]).ToString("yyyy-MM-dd");
                        obj.DateOfLeaving = dt.Rows[i]["DateOfLeaving"] == null || dt.Rows[i]["DateOfLeaving"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["DateOfLeaving"]).ToString("yyyy-MM-dd");
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.Education = dt.Rows[i]["Education"] == null || dt.Rows[i]["Education"].ToString().Trim() == "" ? null : dt.Rows[i]["Education"].ToString();
                        obj.MaritalStatus = dt.Rows[i]["MaritalStatus"] == null || dt.Rows[i]["MaritalStatus"].ToString().Trim() == "" ? null : dt.Rows[i]["MaritalStatus"].ToString();
                        obj.AnniversaryDate = dt.Rows[i]["AnniversaryDate"] == null || dt.Rows[i]["AnniversaryDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["AnniversaryDate"]).ToString("yyyy-MM-dd");
                        obj.Experience = dt.Rows[i]["Experience"] == null || dt.Rows[i]["Experience"].ToString().Trim() == "" ? null : dt.Rows[i]["Experience"].ToString();
                        obj.CurrentAddress = dt.Rows[i]["CurrentAddress"] == null || dt.Rows[i]["CurrentAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentAddress"].ToString();
                        obj.CurrentPincode = dt.Rows[i]["CurrentPincode"] == null || dt.Rows[i]["CurrentPincode"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentPincode"].ToString();
                        obj.CurrentCity = dt.Rows[i]["CurrentCity"] == null || dt.Rows[i]["CurrentCity"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentCity"].ToString();
                        obj.CurrentState = dt.Rows[i]["CurrentState"] == null || dt.Rows[i]["CurrentState"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentState"].ToString();
                        obj.PermenantAddress = dt.Rows[i]["PermenantAddress"] == null || dt.Rows[i]["PermenantAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["PermenantAddress"].ToString();
                        obj.PermenantPincode = dt.Rows[i]["CurrentAddress"] == null || dt.Rows[i]["CurrentAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentAddress"].ToString();
                        obj.PermenantCity = dt.Rows[i]["PermenantCity"] == null || dt.Rows[i]["PermenantCity"].ToString().Trim() == "" ? null : dt.Rows[i]["PermenantCity"].ToString();
                        obj.PermenantState = dt.Rows[i]["PermenantState"] == null || dt.Rows[i]["PermenantState"].ToString().Trim() == "" ? null : dt.Rows[i]["PermenantState"].ToString();
                        obj.BankName = dt.Rows[i]["BankName"] == null || dt.Rows[i]["BankName"].ToString().Trim() == "" ? null : dt.Rows[i]["BankName"].ToString();
                        obj.AccountNo = dt.Rows[i]["AccountNo"] == null || dt.Rows[i]["AccountNo"].ToString().Trim() == "" ? null : dt.Rows[i]["AccountNo"].ToString();
                        obj.BankBranch = dt.Rows[i]["BankBranch"] == null || dt.Rows[i]["BankBranch"].ToString().Trim() == "" ? null : dt.Rows[i]["BankBranch"].ToString();
                        obj.IFSCCode = dt.Rows[i]["IFSCCode"] == null || dt.Rows[i]["IFSCCode"].ToString().Trim() == "" ? null : dt.Rows[i]["IFSCCode"].ToString();



                        lst.Add(obj);
                    }
                }
                cls.LSTTeacherList = lst;

                return cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TeacherModel FillSingleClassRoom(TeacherModel cls)
        {
            try
            {
                List<TeacherModel> LSTList = new List<TeacherModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleClassRoom", conn);
                cmd.Parameters.AddWithValue("@Id", cls.ClassId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        TeacherModel obj = new TeacherModel();
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTTeacherList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TeacherModel FillClassRoomList(TeacherModel cls)
        {
            try
            {
                List<TeacherModel> LSTList = new List<TeacherModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetClassRoomByUser", conn);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        TeacherModel obj = new TeacherModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTTeacherList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TeacherModel DeleteTeacher(TeacherModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteTeacher", conn);
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
        public string UpdateStatus(TeacherModel cls)
        {
            var Status = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateTeacherStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                //cmd.Parameters.Add("@intLoginUser", SqlDbType.Int).Value = LoginUser;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                conn.Close();
                Status = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
                Status = "error";
            }
            return Status;
        }
        public DataTable ExportTeacher(TeacherModel cls)
        {
            try
            {
                List<TeacherModel> LSTList = new List<TeacherModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 5);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@intActive", cls.intActive);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
                        // HTML Tags Code Remove.
                        dr["FullName"] = Regex.Replace(dr["FullName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["TeacherName"] = Regex.Replace(dr["TeacherName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Email"] = Regex.Replace(dr["Email"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["MobileNo"] = Regex.Replace(dr["MobileNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Dob"] = Regex.Replace(dr["Dob"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Gender"] = Regex.Replace(dr["Gender"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Status"] = Regex.Replace(dr["Status"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
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
        public bool CheckEmailInBulkUpdate(string strEmail)
        {
            bool Status = true;
            TeacherModel cls = new TeacherModel();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_CheckEmailForUserMst", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = strEmail;
                //cmd.Parameters.Add("@intCompanyId", SqlDbType.Int).Value = objCommon.getCompanyIdFromSession();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    cls.Id = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                if (cls.Id == 1)
                {
                    Status = false;
                }
                else
                {
                    Status = true;
                }
                return Status;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TeacherModel WelcomeMail(TeacherModel cls)
        {
            try
            {
                List<TeacherModel> lstTeacherList = new List<TeacherModel>();
                SqlCommand cmd = new SqlCommand("WelcomeMail", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Remove commas from the email addresses
                string[] TempEmail = cls.Email.Split(',');

                for (int i = 0; i < TempEmail.Length; i++)
                {
                    string Email = TempEmail[i];
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Emails", Email.Trim());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 0;
                    da.ReturnProviderSpecificTypes = true;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conn.Close();
                    TeacherModel obj = new TeacherModel();
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        obj.Id = Convert.ToInt32(dt.Rows[0]["Id"] == null || dt.Rows[0]["Id"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Id"].ToString());
                        obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                        obj.Surname = dt.Rows[0]["SurName"] == null || dt.Rows[0]["SurName"].ToString().Trim() == "" ? "" : dt.Rows[0]["SurName"].ToString();
                        obj.TeacherName = dt.Rows[0]["UserName"] == null || dt.Rows[0]["UserName"].ToString().Trim() == "" ? "" : dt.Rows[0]["UserName"].ToString();
                        obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                        obj.MobileNo = dt.Rows[0]["MobileNo"] == null || dt.Rows[0]["MobileNo"].ToString().Trim() == "" ? "" : dt.Rows[0]["MobileNo"].ToString();
                        obj.ProfileImg = dt.Rows[0]["Profile"] == null || dt.Rows[0]["Profile"].ToString().Trim() == "" ? "" : dt.Rows[0]["Profile"].ToString();
                        obj.RoleName = dt.Rows[0]["RoleName"] == null || dt.Rows[0]["RoleName"].ToString().Trim() == "" ? "" : dt.Rows[0]["RoleName"].ToString();
                        obj.Password = dt.Rows[0]["Password"] == null || dt.Rows[0]["Password"].ToString().Trim() == "" ? "" : dt.Rows[0]["Password"].ToString();
                        obj.ClassId = Convert.ToInt32(dt.Rows[0]["ClassId"] == null || dt.Rows[0]["ClassId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ClassId"].ToString());
                        obj.Response = "Success";
                        lstTeacherList.Add(obj);
                    }
                    else
                    {
                        obj.Response = "Error";
                    }
                }
                cls.LSTTeacherList = lstTeacherList;
            }
            catch (Exception ex)
            {
                // Handle exception
                // Log the exception
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return cls;
        }

    }
}