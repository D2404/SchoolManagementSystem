using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class StudentModel
    {
        clsCommon objCommon = new clsCommon();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public string StudentUniqueId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int RollNo { get; set; }
        public string Title { get; set; }
        public string TeacherName { get; set; }
        public string FullName { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string Surname { get; set; }
        public string Dob { get; set; }
        public string MobileNo { get; set; }
        public string AlternateMobileNo { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
        public string TempEmail { get; set; }
        public string Password { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfLeaving { get; set; }

        public string MaritalStatus { get; set; }
        public string AnniversaryDate { get; set; }
        public string Experience { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentPincode { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string PermenantAddress { get; set; }
        public string PermenantPincode { get; set; }
        public string PermenantCity { get; set; }
        public string PermenantState { get; set; }
        public string ParentType { get; set; }
        public string ParentName { get; set; }
        public string ParentFatherName { get; set; }
        public string ParentGender { get; set; }
        public string ParentEmail { get; set; }
        public string ParentMobileNo { get; set; }
        public string Qualification { get; set; }
        public string Profession { get; set; }
        public int ClassId { get; set; }
        public string ClassNo { get; set; }
        public int SectionId { get; set; }
        public string SectionNo { get; set; }
        public int TeacherId { get; set; }
        public string Date { get; set; }
        public string Response { get; set; }
        public string SearchText { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecord { get; set; }
        public decimal? PageCount { get; set; }
        public long? ROWNUMBER { get; set; }
        public int TotalEntries { get; set; }
        public string CreatedDate { get; set; }
        public int ShowingEntries { get; set; }
        public int fromEntries { get; set; }
        public int intActive { get; set; }
        public bool IsActive { get; set; }
        public Pager Pager { get; set; }
        public HttpPostedFileBase[] Profile { get; set; }
        public string ProfileImg { get; set; }
        public HttpPostedFileBase[] file { get; set; }
        public string HiddenfileForImage { get; set; }
        public string ErrorMessage { get; set; }

        public List<StudentModel> LSTStudentList { get; set; }

        //public StudentModel addStudent(StudentModel cls)
        //{
        //    try
        //    {
        //        if (cls.Profile != null && cls.Profile.Length > 0)
        //        {
        //            string Profile = ("Profile_" + cls.Id + "_" + DateTime.Now.Ticks).ToString();
        //            string strOriginalFile = cls.Profile[0].FileName;
        //            string ext = System.IO.Path.GetExtension(cls.Profile[0].FileName).ToLower();
        //            string fileLocation = HttpContext.Current.Server.MapPath("/Data/Profile/");
        //            if (!Directory.Exists(fileLocation))
        //            {
        //                Directory.CreateDirectory(fileLocation);
        //            }
        //            if (ext == ".jpeg" || ext == ".jpg" || ext == ".png")
        //            {
        //                Profile = Profile + ext;
        //                cls.Profile[0].SaveAs(fileLocation + Profile);
        //            }
        //            var strPath = fileLocation + cls.Profile;
        //            FileInfo file = new FileInfo(strPath);
        //            if (file.Exists)//check file exsit or not
        //            {
        //                file.Delete();
        //            }
        //            cls.ProfileImg = Profile;
        //        }
        //        var ddd = clsCommon.DecryptString("QU734hNlS/9lJ6Eof1tOcg==");
        //        cls.Password = clsCommon.EncryptString(cls.Password);
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand("AddUpdateStudent", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
        //        cmd.Parameters.Add("@FullName", SqlDbType.VarChar).Value = cls.FullName;
        //        cmd.Parameters.Add("@ClassId", SqlDbType.Int).Value = cls.ClassId;
        //        cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = cls.UserName;
        //        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = cls.Email;
        //        cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = cls.Password;
        //        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = cls.MobileNo;
        //        cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = cls.Address;
        //        cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = cls.Dob;
        //        cmd.Parameters.Add("@RollNo", SqlDbType.VarChar).Value = cls.RollNo;
        //        cmd.Parameters.Add("@Gender", SqlDbType.Bit).Value = cls.Gender;
        //        cmd.Parameters.AddWithValue("@Profile", SqlDbType.VarChar).Value = cls.ProfileImg;
        //        cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
        //        cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
        //        cmd.Parameters.AddWithValue("@RoleId", cls.RoleId);

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        cmd.CommandTimeout = 0;
        //        da.ReturnProviderSpecificTypes = true;
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        conn.Close();
        //        if (dt.Rows.Count > 0)
        //        {
        //            string intRefId = dt.Rows[0][0].ToString();
        //            if (intRefId == "1")
        //            {
        //                cls.Response = "Success";
        //            }
        //            else if (intRefId == "2")
        //            {
        //                cls.Response = "Updated";
        //            }
        //            else if (intRefId == "-1")
        //            {
        //                cls.Response = "Exists";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    return cls;
        //}

        public StudentModel addStudent(StudentModel cls)
        {
            try
            {
                if (cls.Profile != null && cls.Profile.Length > 0)
                {
                    string Profile = ("StudentProfile_" + cls.Id + "_" + DateTime.Now.Ticks).ToString();
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
                SqlCommand cmd = new SqlCommand("AddUpdateStudent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = cls.Title;
                cmd.Parameters.Add("@StudentName", SqlDbType.VarChar).Value = cls.StudentName;
                cmd.Parameters.Add("@FatherName", SqlDbType.VarChar).Value = cls.FatherName;
                cmd.Parameters.Add("@Surname", SqlDbType.VarChar).Value = cls.Surname;
                cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = cls.Dob;
                cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = cls.Gender;
                cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = cls.BloodGroup;
                cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = cls.MobileNo;
                cmd.Parameters.Add("@AlternateMobileNo", SqlDbType.VarChar).Value = cls.AlternateMobileNo;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = cls.Email;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = cls.Password;
                cmd.Parameters.Add("@DateOfJoining", SqlDbType.DateTime).Value = cls.DateOfJoining;
                //cmd.Parameters.Add("@DateOfLeaving", SqlDbType.DateTime).Value = cls.DateOfLeaving;
                cmd.Parameters.Add("@CurrentAddress", SqlDbType.VarChar).Value = cls.CurrentAddress;
                cmd.Parameters.Add("@CurrentPincode", SqlDbType.VarChar).Value = cls.CurrentPincode;
                cmd.Parameters.Add("@CurrentCity", SqlDbType.VarChar).Value = cls.CurrentCity;
                cmd.Parameters.Add("@CurrentState", SqlDbType.VarChar).Value = cls.CurrentState;
                cmd.Parameters.Add("@PermenantAddress", SqlDbType.VarChar).Value = cls.PermenantAddress;
                cmd.Parameters.Add("@PermenantPincode", SqlDbType.VarChar).Value = cls.PermenantPincode;
                cmd.Parameters.Add("@PermenantCity", SqlDbType.VarChar).Value = cls.PermenantCity;
                cmd.Parameters.Add("@PermenantState", SqlDbType.VarChar).Value = cls.PermenantState;
                cmd.Parameters.Add("@ParentType", SqlDbType.VarChar).Value = cls.ParentType;
                cmd.Parameters.Add("@ParentName", SqlDbType.VarChar).Value = cls.ParentName;
                cmd.Parameters.Add("@ParentFatherName", SqlDbType.VarChar).Value = cls.ParentFatherName;
                cmd.Parameters.Add("@ParentGender", SqlDbType.VarChar).Value = cls.ParentGender;
                cmd.Parameters.Add("@ParentEmail", SqlDbType.VarChar).Value = cls.ParentEmail;
                cmd.Parameters.Add("@ParentMobileNo", SqlDbType.VarChar).Value = cls.ParentMobileNo;
                cmd.Parameters.Add("@AnniversaryDate", SqlDbType.DateTime).Value = cls.AnniversaryDate;
                cmd.Parameters.Add("@Profession", SqlDbType.VarChar).Value = cls.Profession;
                cmd.Parameters.Add("@Qualification", SqlDbType.VarChar).Value = cls.Qualification;
                cmd.Parameters.Add("@ClassId", SqlDbType.VarChar).Value = cls.ClassId;
                cmd.Parameters.Add("@SectionId", SqlDbType.VarChar).Value = cls.SectionId;
                cmd.Parameters.Add("@RollNo", SqlDbType.Int).Value = cls.RollNo;
                cmd.Parameters.AddWithValue("@Profile", SqlDbType.VarChar).Value = cls.ProfileImg;
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@RoleId", cls.RoleId);
                //cmd.Parameters.AddWithValue("@ClassId", cls.ClassId);
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

     
        public StudentModel GetSingleStudent(StudentModel cls, int? Id)
        {
            try
            {
                conn.Open();
                List<StudentModel> lst = new List<StudentModel>();
                SqlCommand cmd = new SqlCommand("sp_GetSingleStudent", conn);
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
                        StudentModel obj = new StudentModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Title = dt.Rows[i]["Title"] == null || dt.Rows[i]["Title"].ToString().Trim() == "" ? null : dt.Rows[i]["Title"].ToString();
                        obj.ProfileImg = dt.Rows[i]["Profile"] == null || dt.Rows[i]["Profile"].ToString().Trim() == "" ? null : dt.Rows[i]["Profile"].ToString();
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.SectionId = Convert.ToInt32(dt.Rows[i]["SectionId"] == null || dt.Rows[i]["SectionId"].ToString().Trim() == "" ? null : dt.Rows[i]["SectionId"].ToString());
                        obj.SectionNo = dt.Rows[i]["SectionNo"] == null || dt.Rows[i]["SectionNo"].ToString().Trim() == "" ? null : dt.Rows[i]["SectionNo"].ToString();
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
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
                        obj.Qualification = dt.Rows[i]["Qualification"] == null || dt.Rows[i]["Qualification"].ToString().Trim() == "" ? null : dt.Rows[i]["Qualification"].ToString();
                        obj.AnniversaryDate = dt.Rows[i]["AnniversaryDate"] == null || dt.Rows[i]["AnniversaryDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["AnniversaryDate"]).ToString("yyyy-MM-dd");
                        obj.CurrentAddress = dt.Rows[i]["CurrentAddress"] == null || dt.Rows[i]["CurrentAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentAddress"].ToString();
                        obj.CurrentPincode = dt.Rows[i]["CurrentPincode"] == null || dt.Rows[i]["CurrentPincode"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentPincode"].ToString();
                        obj.CurrentCity = dt.Rows[i]["CurrentCity"] == null || dt.Rows[i]["CurrentCity"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentCity"].ToString();
                        obj.CurrentState = dt.Rows[i]["CurrentState"] == null || dt.Rows[i]["CurrentState"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentState"].ToString();
                        obj.PermenantAddress = dt.Rows[i]["PermenantAddress"] == null || dt.Rows[i]["PermenantAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["PermenantAddress"].ToString();
                        obj.PermenantPincode = dt.Rows[i]["CurrentAddress"] == null || dt.Rows[i]["CurrentAddress"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentAddress"].ToString();
                        obj.PermenantCity = dt.Rows[i]["PermenantCity"] == null || dt.Rows[i]["PermenantCity"].ToString().Trim() == "" ? null : dt.Rows[i]["PermenantCity"].ToString();
                        obj.PermenantState = dt.Rows[i]["PermenantState"] == null || dt.Rows[i]["PermenantState"].ToString().Trim() == "" ? null : dt.Rows[i]["PermenantState"].ToString();
                        obj.ParentType = dt.Rows[i]["ParentType"] == null || dt.Rows[i]["ParentType"].ToString().Trim() == "" ? null : dt.Rows[i]["ParentType"].ToString();
                        obj.ParentName = dt.Rows[i]["ParentName"] == null || dt.Rows[i]["ParentName"].ToString().Trim() == "" ? null : dt.Rows[i]["ParentName"].ToString();
                        obj.ParentFatherName = dt.Rows[i]["ParentFatherName"] == null || dt.Rows[i]["ParentFatherName"].ToString().Trim() == "" ? null : dt.Rows[i]["ParentFatherName"].ToString();
                        obj.ParentGender = dt.Rows[i]["ParentGender"] == null || dt.Rows[i]["ParentGender"].ToString().Trim() == "" ? null : dt.Rows[i]["ParentGender"].ToString();
                        obj.ParentEmail = dt.Rows[i]["ParentEmail"] == null || dt.Rows[i]["ParentEmail"].ToString().Trim() == "" ? null : dt.Rows[i]["ParentEmail"].ToString();
                        obj.ParentMobileNo = dt.Rows[i]["ParentMobileNo"] == null || dt.Rows[i]["ParentMobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ParentMobileNo"].ToString();
                        obj.Profession = dt.Rows[i]["Profession"] == null || dt.Rows[i]["Profession"].ToString().Trim() == "" ? null : dt.Rows[i]["Profession"].ToString();
                        lst.Add(obj);
                    }
                }
                cls.LSTStudentList = lst;

                return cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StudentModel FillClassRoomList(StudentModel cls)
        {
            try
            {
                List<StudentModel> LSTList = new List<StudentModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetClassRoomByTeacher", conn);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getTeacherIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        StudentModel obj = new StudentModel();
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTStudentList = LSTList;
                return cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public StudentModel deleteStudent(StudentModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteStudent", conn);
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

        public string UpdateStatus(StudentModel cls)
        {
            var Status = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateStudentStatus", conn);
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

        public DataTable ExportStudent(StudentModel cls)
        {
            try
            {
                List<StudentModel> LSTList = new List<StudentModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 7);
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
                        dr["StudentName"] = Regex.Replace(dr["StudentName"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Email"] = Regex.Replace(dr["Email"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["MobileNo"] = Regex.Replace(dr["MobileNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Dob"] = Regex.Replace(dr["Dob"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Gender"] = Regex.Replace(dr["Gender"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["Status"] = Regex.Replace(dr["Status"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["ClassNo"] = Regex.Replace(dr["ClassNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["SectionNo"] = Regex.Replace(dr["SectionNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["RollNo"] = Regex.Replace(dr["RollNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
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

        public StudentModel WelcomeMail(StudentModel cls)
        {
            try
            {
                List<StudentModel> lstStudentList = new List<StudentModel>();
                SqlCommand cmd = new SqlCommand("WelcomeMail", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

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
                    StudentModel obj = new StudentModel();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        obj.Id = Convert.ToInt32(dt.Rows[0]["Id"] == null || dt.Rows[0]["Id"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Id"].ToString());
                        obj.Surname = dt.Rows[0]["SurName"] == null || dt.Rows[0]["SurName"].ToString().Trim() == "" ? "" : dt.Rows[0]["SurName"].ToString();
                        obj.StudentName = dt.Rows[0]["UserName"] == null || dt.Rows[0]["UserName"].ToString().Trim() == "" ? "" : dt.Rows[0]["UserName"].ToString();
                        obj.Email = dt.Rows[0]["Email"] == null || dt.Rows[0]["Email"].ToString().Trim() == "" ? "" : dt.Rows[0]["Email"].ToString();
                        obj.MobileNo = dt.Rows[0]["MobileNo"] == null || dt.Rows[0]["MobileNo"].ToString().Trim() == "" ? "" : dt.Rows[0]["MobileNo"].ToString();
                        obj.ProfileImg = dt.Rows[0]["Profile"] == null || dt.Rows[0]["Profile"].ToString().Trim() == "" ? "" : dt.Rows[0]["Profile"].ToString();
                        obj.RoleName = "Student";
                        obj.Password = dt.Rows[0]["Password"] == null || dt.Rows[0]["Password"].ToString().Trim() == "" ? "" : dt.Rows[0]["Password"].ToString();
                        obj.ClassId = Convert.ToInt32(dt.Rows[0]["ClassId"] == null || dt.Rows[0]["ClassId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ClassId"].ToString());
                        obj.Response = "Success";
                        lstStudentList.Add(obj);
                    }
                    else
                    {
                        obj.Response = "Error";
                    }
                }
                cls.LSTStudentList = lstStudentList;
            }
            catch (Exception ex)
            {
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