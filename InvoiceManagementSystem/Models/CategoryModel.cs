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
    public class CategoryModel
    {

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int CategoryId { get; set; }
        public string Name { get; set; }
        
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
        public Pager Pager { get; set; }
        public HttpPostedFileBase[] Image { get; set; }
        public string CatImg { get; set; }

        public List<CategoryModel> LSTCategoryList { get; set; }

        public CategoryModel addCategory(CategoryModel cls)
        {
            try
            {
                if (cls.Image != null && cls.Image.Length > 0)
                {
                    string Image = ("Category_" + cls.CategoryId + "_" + DateTime.Now.Ticks).ToString();
                    string strOriginalFile = cls.Image[0].FileName;
                    string ext = System.IO.Path.GetExtension(cls.Image[0].FileName).ToLower();
                    string fileLocation = HttpContext.Current.Server.MapPath("/Data/Item/");
                    if (!Directory.Exists(fileLocation))
                    {
                        Directory.CreateDirectory(fileLocation);
                    }
                    if (ext == ".jpeg" || ext == ".jpg" || ext == ".png")
                    {
                        Image = Image + ext;
                        cls.Image[0].SaveAs(fileLocation + Image);
                    }
                    var strPath = fileLocation + cls.Image;
                    FileInfo file = new FileInfo(strPath);
                    if (file.Exists)//check file exsit or not
                    {
                        file.Delete();
                    }
                    cls.CatImg = Image;
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand("InsertCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.CategoryId;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = cls.Name;
                cmd.Parameters.AddWithValue("@Image", SqlDbType.VarChar).Value = cls.CatImg;

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

        public CategoryModel GetCategory(CategoryModel cls)
        {
            try
            {
                List<CategoryModel> LSTList = new List<CategoryModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.CategoryId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        CategoryModel obj = new CategoryModel();
                        obj.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"] == null || dt.Rows[i]["CategoryId"].ToString().Trim() == "" ? null : dt.Rows[i]["CategoryId"].ToString());
                        obj.Name = dt.Rows[i]["Name"] == null || dt.Rows[i]["Name"].ToString().Trim() == "" ? null : dt.Rows[i]["Name"].ToString();
                        cls.CatImg = dt.Rows[i]["ImageFile"] == null || dt.Rows[i]["ImageFile"].ToString().Trim() == "" ? null : dt.Rows[i]["ImageFile"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTCategoryList = LSTList;
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

     
        public CategoryModel deleteCategory(CategoryModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", cls.CategoryId);
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
    }
}