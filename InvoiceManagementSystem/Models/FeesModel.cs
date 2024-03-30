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
    public class FeesModel
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
       
        public int Monthly { get; set; }
        public int Yearly { get; set; }
        public string Date { get; set; }
        public string Profile { get; set; }
        public int FeesAmount { get; set; }
        public int RollNo { get; set; }
        public string ClassNo { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
       
        public int ClassId { get; set; }
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public int YearId { get; set; }
        public string YearName { get; set; }
        public int TotalPay { get; set; }
        public int TotalPending { get; set; }
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public int intActive { get; set; }
        public bool IsActive { get; set; }
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
        public int Status { get; set; }

        public Pager Pager { get; set; }
        public List<SchoolModel> LSTSchoolList { get; set; }
        public List<FeesModel> LSTFeesList { get; set; }
        public List<FeesModel> LSTFeesHistoryList { get; set; }
        public FeesModel addClassWiseFees(FeesModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateClassRoomWiseFees", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.AddWithValue("@ClassId", cls.ClassId);
                cmd.Parameters.AddWithValue("@Monthly", cls.Monthly);
                cmd.Parameters.AddWithValue("@Yearly", cls.Yearly);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
        public FeesModel GetClassWiseFees(FeesModel cls)
        {
            try
            {
                List<FeesModel> LSTList = new List<FeesModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleClassRoomWiseFees", conn);
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
                        FeesModel obj = new FeesModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.Monthly = Convert.ToInt32(dt.Rows[i]["Monthly"] == null || dt.Rows[i]["Monthly"].ToString().Trim() == "" ? null : dt.Rows[i]["Monthly"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["Yearly"] == null || dt.Rows[i]["Yearly"].ToString().Trim() == "" ? null : dt.Rows[i]["Yearly"].ToString());
                        LSTList.Add(obj);
                    }
                }
                cls.LSTFeesList = LSTList;
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
        public FeesModel deleteClassWiseFees(FeesModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteClassRoomWiseFees", conn);
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

        public string UpdateStatus(FeesModel cls)
        {
            var Status = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateClassWiseFeesStatus", conn);
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


        public FeesModel addFees(FeesModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateFees", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);
                cmd.Parameters.AddWithValue("@ClassId", cls.ClassId);
                cmd.Parameters.AddWithValue("@RollNo", cls.RollNo);
                cmd.Parameters.AddWithValue("@FeesAmount", cls.FeesAmount);
                cmd.Parameters.AddWithValue("@MonthId", cls.MonthId);
                cmd.Parameters.AddWithValue("@Year", cls.YearId);
                cmd.Parameters.AddWithValue("@Date", cls.Date);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
                    else if (intRefId == "-2")
                    {
                        cls.Response = "LastMonth";
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

        public FeesModel addandPayFees(FeesModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateandPayFees", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cls.Id;
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);
                cmd.Parameters.AddWithValue("@ClassId", cls.ClassId);
                cmd.Parameters.AddWithValue("@RollNo", cls.RollNo);
                cmd.Parameters.AddWithValue("@FeesAmount", cls.FeesAmount);
                cmd.Parameters.AddWithValue("@MonthId", cls.MonthId);
                cmd.Parameters.AddWithValue("@Year", cls.YearId);
                cmd.Parameters.AddWithValue("@Date", cls.Date);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
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
                    else if (intRefId == "-2")
                    {
                        cls.Response = "LastMonth";
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

        public FeesModel GetFees(FeesModel cls)
        {
            try
            {
                List<FeesModel> LSTList = new List<FeesModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleFees", conn);
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
                        FeesModel obj = new FeesModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.MonthId = Convert.ToInt32(dt.Rows[i]["MonthId"] == null || dt.Rows[i]["MonthId"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthId"].ToString());
                        obj.YearId = Convert.ToInt32(dt.Rows[i]["YearId"] == null || dt.Rows[i]["YearId"].ToString().Trim() == "" ? null : dt.Rows[i]["YearId"].ToString());
                        obj.FeesAmount = Convert.ToInt32(dt.Rows[i]["FeesAmount"] == null || dt.Rows[i]["FeesAmount"].ToString().Trim() == "" ? null : dt.Rows[i]["FeesAmount"].ToString());
                        obj.TotalPay = Convert.ToInt32(dt.Rows[i]["TotalPay"] == null || dt.Rows[i]["TotalPay"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPay"].ToString());
                        obj.TotalPending = Convert.ToInt32(dt.Rows[i]["TotalPending"] == null || dt.Rows[i]["TotalPending"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPending"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["YearlyFees"] == null || dt.Rows[i]["YearlyFees"].ToString().Trim() == "" ? null : dt.Rows[i]["YearlyFees"].ToString());
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("yyyy/MM/dd");
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.StudentName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.MonthName = dt.Rows[i]["MonthName"] == null || dt.Rows[i]["MonthName"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthName"].ToString();
                        obj.YearName = dt.Rows[i]["Year"] == null || dt.Rows[i]["Year"].ToString().Trim() == "" ? null : dt.Rows[i]["Year"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTFeesList = LSTList;
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
        public FeesModel GetFeesByStudentId(FeesModel cls)
        {
            try
            {
                List<FeesModel> LSTList = new List<FeesModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleFeesByStudentId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        FeesModel obj = new FeesModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.MonthId = Convert.ToInt32(dt.Rows[i]["MonthId"] == null || dt.Rows[i]["MonthId"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthId"].ToString());
                        obj.YearId = Convert.ToInt32(dt.Rows[i]["YearId"] == null || dt.Rows[i]["YearId"].ToString().Trim() == "" ? null : dt.Rows[i]["YearId"].ToString());
                        obj.FeesAmount = Convert.ToInt32(dt.Rows[i]["FeesAmount"] == null || dt.Rows[i]["FeesAmount"].ToString().Trim() == "" ? null : dt.Rows[i]["FeesAmount"].ToString());
                        obj.TotalPay = Convert.ToInt32(dt.Rows[i]["TotalPay"] == null || dt.Rows[i]["TotalPay"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPay"].ToString());
                        obj.TotalPending = Convert.ToInt32(dt.Rows[i]["TotalPending"] == null || dt.Rows[i]["TotalPending"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPending"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["YearlyFees"] == null || dt.Rows[i]["YearlyFees"].ToString().Trim() == "" ? null : dt.Rows[i]["YearlyFees"].ToString());
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("yyyy/MM/dd");
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.StudentName = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                        obj.MonthName = dt.Rows[i]["MonthName"] == null || dt.Rows[i]["MonthName"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthName"].ToString();
                        obj.YearName = dt.Rows[i]["Year"] == null || dt.Rows[i]["Year"].ToString().Trim() == "" ? null : dt.Rows[i]["Year"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTFeesList = LSTList;
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
     
        public FeesModel GetFeesHistoryByStudentId(FeesModel cls)
        {
            try
            {
                List<FeesModel> lstFeesList = new List<FeesModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleFeesHistoryByStudentId", conn);
                cmd.Parameters.AddWithValue("@StudentId", cls.StudentId);
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
                        FeesModel obj = new FeesModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.ClassId = Convert.ToInt32(dt.Rows[i]["ClassId"] == null || dt.Rows[i]["ClassId"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassId"].ToString());
                        obj.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"] == null || dt.Rows[i]["StudentId"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentId"].ToString());
                        obj.MonthId = Convert.ToInt32(dt.Rows[i]["MonthId"] == null || dt.Rows[i]["MonthId"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthId"].ToString());
                        obj.FeesAmount = Convert.ToInt32(dt.Rows[i]["FeesAmount"] == null || dt.Rows[i]["FeesAmount"].ToString().Trim() == "" ? null : dt.Rows[i]["FeesAmount"].ToString());
                        obj.Date = dt.Rows[i]["Date"] == null || dt.Rows[i]["Date"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        obj.MonthName = dt.Rows[i]["MonthName"] == null || dt.Rows[i]["MonthName"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthName"].ToString();
                        obj.YearId = Convert.ToInt32(dt.Rows[i]["Year"] == null || dt.Rows[i]["Year"].ToString().Trim() == "" ? null : dt.Rows[i]["Year"].ToString());
                        lstFeesList.Add(obj);
                    }
                }
                cls.LSTFeesHistoryList = lstFeesList;


                return cls;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FeesModel GetSchoolDetails(FeesModel cls)
        {
            try
            {
                List<SchoolModel> lstSchoolList = new List<SchoolModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSchoolList", conn);
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
                        SchoolModel obj = new SchoolModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.PhotoImg = dt.Rows[i]["Photo"] == null ? "" : dt.Rows[i]["Photo"].ToString();
                        obj.SchoolName = dt.Rows[i]["SchoolName"] == null || dt.Rows[i]["SchoolName"].ToString().Trim() == "" ? null : dt.Rows[i]["SchoolName"].ToString();
                        obj.Email = dt.Rows[i]["Email"] == null || dt.Rows[i]["Email"].ToString().Trim() == "" ? null : dt.Rows[i]["Email"].ToString();
                        obj.Address = dt.Rows[i]["Address"] == null || dt.Rows[i]["Address"].ToString().Trim() == "" ? null : dt.Rows[i]["Address"].ToString();
                        obj.MobileNo = dt.Rows[i]["MobileNo"] == null || dt.Rows[i]["MobileNo"].ToString().Trim() == "" ? null : dt.Rows[i]["MobileNo"].ToString();
                        obj.Since = dt.Rows[i]["Since"] == null || dt.Rows[i]["Since"].ToString().Trim() == "" ? null : dt.Rows[i]["Since"].ToString();

                        lstSchoolList.Add(obj);
                    }
                }
                cls.LSTSchoolList = lstSchoolList;

                return cls;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public FeesModel deleteFees(FeesModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteFees", conn);
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

        public FeesModel deleteFeesHistory(FeesModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteFeesCollectionHistoryy", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", cls.Id);
                cmd.Parameters.Add("@StudentId", cls.StudentId);
                cmd.Parameters.Add("@MonthId", cls.MonthId);
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