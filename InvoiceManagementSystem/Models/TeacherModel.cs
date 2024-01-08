using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class TeacherModel
    {
        clsCommon objCommon = new clsCommon();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public string ClassNo { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string TeacherName { get; set; }
        public string FatherName { get; set; }
        public string Surname { get; set; }
        public string Dob { get; set; }
        public string MobileNo { get; set; }
        public string AlternateMobileNo { get; set; }
        public bool Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Education { get; set; }
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
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
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
        public string Date { get; set; }
        
        public int intActive { get; set; }
        public bool IsActive { get; set; }
       
        public Pager Pager { get; set; }
        public HttpPostedFileBase[] Profile { get; set; }
        public string ProfileImg { get; set; }

        public List<TeacherModel> LSTTeacherList { get; set; }
        public List<ClassRoomModel> LSTClassRoomList { get; set; }

        public TeacherModel addTeacher(TeacherModel cls)
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
                var ddd = clsCommon.DecryptString("QU734hNlS/9lJ6Eof1tOcg==");
                cls.Password = clsCommon.EncryptString(cls.Password);
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateTeacher", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = cls.Title;
                cmd.Parameters.Add("@TeacherName", SqlDbType.VarChar).Value = cls.TeacherName;
                cmd.Parameters.Add("@FatherName", SqlDbType.VarChar).Value = cls.FatherName;
                cmd.Parameters.Add("@Surname", SqlDbType.VarChar).Value = cls.Surname;
                cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = cls.Dob;
                cmd.Parameters.Add("@Gender", SqlDbType.Bit).Value = cls.Gender;
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
                        obj.Gender = Convert.ToBoolean(dt.Rows[i]["Gender"] == null || dt.Rows[i]["Gender"].ToString().Trim() == "" ? null : dt.Rows[i]["Gender"].ToString());
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
        public TeacherModel GetTeacher(TeacherModel teacher, int? id)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleTeacher", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    teacher.Id = Convert.ToInt32(row["Id"]);
                    teacher.ClassId = Convert.ToInt32(row["ClassId"]);
                    teacher.ClassNo = row["ClassNo"].ToString();
                    teacher.FullName = row["FullName"].ToString();
                    teacher.TeacherName = row["TeacherName"].ToString();
                    teacher.Email = row["Email"].ToString();
                    teacher.Password = row["Password"].ToString();
                    teacher.MobileNo = row["MobileNo"].ToString();
                    teacher.CurrentAddress = row["CurrentAddress"].ToString();
                    teacher.Dob = (row["Dob"] == DBNull.Value) ? null : Convert.ToDateTime(row["Dob"]).ToString("yyyy/MM/dd");
                    teacher.Education = row["Education"].ToString();
                    teacher.Gender = Convert.ToBoolean(row["Gender"].ToString());
                    teacher.ProfileImg = row["Profile"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return teacher;
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
                SqlCommand cmd = new SqlCommand("Sp_GetClassRoomList", conn);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@intActive", 1);
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
        public TeacherModel deleteTeacher(TeacherModel cls)
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
    }
}