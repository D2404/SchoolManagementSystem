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
    public class StudentModel
    {
        clsCommon objCommon = new clsCommon();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RollNo { get; set; }
        public string FullName { get; set; }
        public string TeacherName { get; set; }
        public string UserName { get; set; }
        public string ClassNo { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
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
        public string Dob { get; set; }
        public bool Gender { get; set; }
        public bool IsActive { get; set; }
        public Pager Pager { get; set; }
        public HttpPostedFileBase[] Profile { get; set; }
        public string ProfileImg { get; set; }

        public List<StudentModel> LSTStudentList { get; set; }

        public StudentModel addStudent(StudentModel cls)
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
                SqlCommand cmd = new SqlCommand("AddUpdateStudent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.Add("@FullName", SqlDbType.VarChar).Value = cls.FullName;
                cmd.Parameters.Add("@ClassId", SqlDbType.Int).Value = cls.ClassId;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = cls.UserName;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = cls.Email;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = cls.Password;
                cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = cls.MobileNo;
                cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = cls.Address;
                cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = cls.Dob;
                cmd.Parameters.Add("@RollNo", SqlDbType.VarChar).Value = cls.RollNo;
                cmd.Parameters.Add("@Gender", SqlDbType.Bit).Value = cls.Gender;
                cmd.Parameters.AddWithValue("@Profile", SqlDbType.VarChar).Value = cls.ProfileImg;
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                cmd.Parameters.AddWithValue("@RoleId", cls.RoleId);

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

        public StudentModel GetStudent(StudentModel cls)
        {
            try
            {
                List<StudentModel> LSTList = new List<StudentModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleStudent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        StudentModel obj = new StudentModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.RollNo = dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString();
                        obj.UserName = dt.Rows[i]["UserName"] == null || dt.Rows[i]["UserName"].ToString().Trim() == "" ? null : dt.Rows[i]["UserName"].ToString();
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.Password = dt.Rows[i]["Password"] == null || dt.Rows[i]["Password"].ToString().Trim() == "" ? null : dt.Rows[i]["Password"].ToString();
                        obj.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.Address = dt.Rows[i]["Address"] == null || dt.Rows[i]["Address"].ToString().Trim() == "" ? null : dt.Rows[i]["Address"].ToString();
                        obj.Dob = dt.Rows[i]["Dob"] == null || dt.Rows[i]["Dob"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Dob"]).ToString("yyyy/MM/dd");
                        obj.Gender = Convert.ToBoolean(dt.Rows[i]["Gender"] == null || dt.Rows[i]["Gender"].ToString().Trim() == "" ? null : dt.Rows[i]["Gender"].ToString());
                        cls.ProfileImg = dt.Rows[i]["Profile"] == null || dt.Rows[i]["Profile"].ToString().Trim() == "" ? null : dt.Rows[i]["Profile"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTStudentList = LSTList;
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



        public StudentModel GetCategory(StudentModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", cls.Id);


                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                List<StudentModel> list = new List<StudentModel>();
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    cls.Id = Convert.ToInt32(dt.Rows[i]["CategoryId"] == null || dt.Rows[i]["CategoryId"].ToString().Trim() == "" ? null : dt.Rows[i]["CategoryId"].ToString());
                    cls.FullName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                    cls.UserName = dt.Rows[i]["UserName"] == null || dt.Rows[i]["UserName"].ToString().Trim() == "" ? null : dt.Rows[i]["UserName"].ToString();
                    cls.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                    cls.Password = dt.Rows[i]["Password"] == null || dt.Rows[i]["Password"].ToString().Trim() == "" ? null : dt.Rows[i]["Password"].ToString();
                    cls.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                    cls.Address = dt.Rows[i]["Address"] == null || dt.Rows[i]["Address"].ToString().Trim() == "" ? null : dt.Rows[i]["Address"].ToString();
                    cls.Dob = dt.Rows[i]["Dob"] == null || dt.Rows[i]["Dob"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Dob"]).ToString("dd/MM/yyyy");
                    cls.Gender = Convert.ToBoolean(dt.Rows[i]["Gender"] == null || dt.Rows[i]["Gender"].ToString().Trim() == "" ? null : dt.Rows[i]["Gender"].ToString());
                    cls.ProfileImg = dt.Rows[i]["Profile"] == null || dt.Rows[i]["Profile"].ToString().Trim() == "" ? null : dt.Rows[i]["Profile"].ToString();

                    //obj.bisIsNew = row["bisNew"] == null || row["bisNew"].ToString() == "" ? 0 : Convert.ToBoolean(row["bisNew"].ToString());
                    //cls.intId = dr["intId"] == null ? 0 : Convert.ToInt32(dr["intId"].ToString());
                    list.Add(cls);
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw ex;
            }
            return cls;
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
    }
}