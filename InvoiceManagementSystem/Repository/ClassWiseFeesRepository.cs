using InvoiceManagementSystem.IRepository;
using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace InvoiceManagementSystem.Repository
{
    public class ClassWiseFeesRepository : IClassWiseFeesRepository
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public FeesModel GetAllClassWiseFees(FeesModel model)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<FeesModel> lstFeesList = new List<FeesModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetClassRoomWiseFeesList", conn);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", model.PageIndex);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@intActive", model.intActive);
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
                        obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"] == null || dt.Rows[i]["IsActive"].ToString().Trim() == "" ? null : dt.Rows[i]["IsActive"].ToString());
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.Monthly = Convert.ToInt32(dt.Rows[i]["Monthly"] == null || dt.Rows[i]["Monthly"].ToString().Trim() == "" ? null : dt.Rows[i]["Monthly"].ToString());
                        obj.Yearly = Convert.ToInt32(dt.Rows[i]["Yearly"] == null || dt.Rows[i]["Yearly"].ToString().Trim() == "" ? null : dt.Rows[i]["Yearly"].ToString());
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstFeesList.Add(obj);
                    }
                }
                model.LSTFeesList = lstFeesList;
                if (model.LSTFeesList.Count > 0)
                {
                    var pager = new Models.Pager((int)model.LSTFeesList[0].TotalRecord, model.PageIndex, (int)model.PageSize);

                    model.Pager = pager;
                }
                model.TotalEntries = TotalEntries;
                model.ShowingEntries = showingEntries;
                model.fromEntries = startentries;
                model.LSTFeesList = lstFeesList;

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FeesModel AddClassWiseFees(FeesModel model)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddUpdateClassRoomWiseFees", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@Monthly", model.Monthly);
                cmd.Parameters.AddWithValue("@Yearly", model.Yearly);
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
                        model.Response = "Success";
                    }
                    else if (intRefId == "2")
                    {
                        model.Response = "Updated";
                    }
                    else if (intRefId == "-1")
                    {
                        model.Response = "Exists";
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

            return model;
        }

        public FeesModel GetSingleClassWiseFees(FeesModel model)
        {
            try
            {
                List<FeesModel> LSTList = new List<FeesModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSingleClassRoomWiseFees", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
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
                model.LSTFeesList = LSTList;
                return model;
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


        public FeesModel DeleteClassWiseFees(FeesModel model)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteRecord", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Mode", 1);
                cmd.Parameters.Add("@Id", model.Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows[0][0].ToString() == "1")
                {
                    model.Response = "Success";
                }
                else if (dt.Rows[0][0].ToString() == "2")
                {
                    model.Response = "dependency";
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return model;
        }

        public string UpdateStatus(FeesModel model)
        {
            var Status = "";
            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateClassWiseFeesStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
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

        public DataTable ExportClassWiseFees(FeesModel model)
        {
            try
            {
                List<FeesModel> LSTList = new List<FeesModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("ExportToExcel", conn);
                cmd.Parameters.AddWithValue("@Mode", 1);
                cmd.Parameters.AddWithValue("@Search", model.SearchText);
                cmd.Parameters.AddWithValue("@intActive", model.intActive);
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
                        dr["ClassNo"] = Regex.Replace(dr["ClassNo"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
                        dr["AcademicYear"] = Regex.Replace(dr["AcademicYear"].ToString(), @"<[^>]+>| ", " ").Replace("&nbsp;", " ").Replace("&amp;", " ").Trim();
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