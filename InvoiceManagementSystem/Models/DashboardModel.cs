using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class DashboardModel
    {
        clsCommon objCommon = new clsCommon();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int Id { get; set; }
        public int TotalClassRoom { get; set; }
        public int TotalStudent { get; set; }
        public int TotalTeacher { get; set; }
        public int ThisMonthCollection { get; set; }
        public int ThisMonthPendingCollection { get; set; }
        public int TotalAmountForThisMonth { get; set; }
        public int ThisYearCollection { get; set; }
        public int TotalPendingThisYear { get; set; }
        public int TotalAmountForThisYear { get; set; }
        public int TotalTeacherSubject { get; set; }
       
        public int TotalSubject { get; set; }
        
        public decimal TotalAttendance { get; set; }
        public decimal PresentDays { get; set; }
        public decimal AbsentDays { get; set; }
        public string Response { get; set; }
        public string Dob { get; set; }
        public string Name { get; set; }
        
        public string ClassNo { get; set; }
        public int Type { get; set; }
        public List<DashboardModel> LSTDashBoardList { get; set; }
        public List<DashboardModel> LSTTeacherList { get; set; }
        public List<DashboardModel> LSTStudentList { get; set; }

        public DashboardModel GetUserAccountDashboardCount(DashboardModel cls)
        {
            try
            {
                List<DashboardModel> LSTList = new List<DashboardModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetLatestDashboardCountList", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@UserId", objCommon.getUserIdFromSession());
                //cmd.Parameters.AddWithValue("@intUserType", objCommon.getUserTypeFromSession());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        DashboardModel obj = new DashboardModel();
                        obj.TotalClassRoom = Convert.ToInt32(dt.Rows[i]["TotalClassRoom"] == null || dt.Rows[i]["TotalClassRoom"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalClassRoom"].ToString());
                        obj.TotalStudent = Convert.ToInt32(dt.Rows[i]["TotalStudent"] == null || dt.Rows[i]["TotalStudent"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalStudent"].ToString());
                        obj.TotalTeacher = Convert.ToInt32(dt.Rows[i]["TotalTeacher"] == null || dt.Rows[i]["TotalTeacher"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalTeacher"].ToString());
                        obj.TotalSubject = Convert.ToInt32(dt.Rows[i]["TotalSubject"] == null || dt.Rows[i]["TotalSubject"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalSubject"].ToString());
                        obj.ThisMonthCollection = Convert.ToInt32(dt.Rows[i]["ThisMonthCollection"] == null || dt.Rows[i]["ThisMonthCollection"].ToString().Trim() == "" ? null : dt.Rows[i]["ThisMonthCollection"].ToString());
                        obj.ThisMonthPendingCollection = Convert.ToInt32(dt.Rows[i]["ThisMonthPendingCollection"] == null || dt.Rows[i]["ThisMonthPendingCollection"].ToString().Trim() == "" ? null : dt.Rows[i]["ThisMonthPendingCollection"].ToString());
                        obj.TotalAmountForThisMonth = Convert.ToInt32(dt.Rows[i]["TotalAmountForthisMonth"] == null || dt.Rows[i]["TotalAmountForthisMonth"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalAmountForthisMonth"].ToString());
                        obj.ThisYearCollection = Convert.ToInt32(dt.Rows[i]["ThisYearCollection"] == null || dt.Rows[i]["ThisYearCollection"].ToString().Trim() == "" ? null : dt.Rows[i]["ThisYearCollection"].ToString());
                        obj.TotalPendingThisYear = Convert.ToInt32(dt.Rows[i]["TotalPendingThisYear"] == null || dt.Rows[i]["TotalPendingThisYear"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalPendingThisYear"].ToString());
                        obj.TotalAmountForThisYear = Convert.ToInt32(dt.Rows[i]["TotalAmountForthisYear"] == null || dt.Rows[i]["TotalAmountForthisYear"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalAmountForthisYear"].ToString());

                        LSTList.Add(obj);
                    }
                }
                cls.LSTDashBoardList = LSTList;
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

        public DashboardModel GetTeacherDashboardCount(DashboardModel cls)
        {
            try
            {
                List<DashboardModel> LSTList = new List<DashboardModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetTeacherDashboardCountList", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeacherId", objCommon.getTeacherIdFromSession());
                //cmd.Parameters.AddWithValue("@intUserType", objCommon.getUserTypeFromSession());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        DashboardModel obj = new DashboardModel();
                        obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                        obj.TotalStudent = Convert.ToInt32(dt.Rows[i]["TotalStudent"] == null || dt.Rows[i]["TotalStudent"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalStudent"].ToString());
                        obj.TotalTeacherSubject = Convert.ToInt32(dt.Rows[i]["TotalTeacherSubject"] == null || dt.Rows[i]["TotalTeacherSubject"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalTeacherSubject"].ToString());
                        //obj.TotalAttendance = Convert.ToDecimal(dt.Rows[i]["PresentDays"] == null || dt.Rows[i]["PresentDays"].ToString().Trim() == "" ? null : dt.Rows[i]["PresentDays"]);

                        LSTList.Add(obj);
                    }
                }
                cls.LSTDashBoardList = LSTList;
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

        public List<DashboardModel> GetTeacherDetailsList(DashboardModel cls)
        {
            List<DashboardModel> lstEmpDetail = new List<DashboardModel>();

            conn.Open();
            SqlCommand cmd = new SqlCommand("sp_GetTeacherDetailDashboard", conn);
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
                    DashboardModel obj = new DashboardModel();
                    obj.Dob = Convert.ToDateTime(dt.Rows[i]["dob"] == null || dt.Rows[i]["dob"].ToString().Trim() == "" ? null : dt.Rows[i]["dob"].ToString()).ToString("dd-MMM-yyyy");
                    obj.Name = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                    obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                    obj.Type = Convert.ToInt32(dt.Rows[i]["Type"] == null || dt.Rows[i]["Type"].ToString().Trim() == "" ? null : dt.Rows[i]["Type"].ToString());
                    lstEmpDetail.Add(obj);
                }
            }
            return lstEmpDetail;
        }
        public List<DashboardModel> GetStudentDetailsList(DashboardModel cls)
        {
            List<DashboardModel> lstEmpDetail = new List<DashboardModel>();

            conn.Open();
            SqlCommand cmd = new SqlCommand("sp_GetStudentDetailDashboard", conn);
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
                    DashboardModel obj = new DashboardModel();
                    obj.Dob = Convert.ToDateTime(dt.Rows[i]["dob"] == null || dt.Rows[i]["dob"].ToString().Trim() == "" ? null : dt.Rows[i]["dob"].ToString()).ToString("dd-MMM-yyyy");
                    obj.Name = dt.Rows[i]["FullName"] == null || dt.Rows[i]["FullName"].ToString().Trim() == "" ? null : dt.Rows[i]["FullName"].ToString();
                    obj.ClassNo = dt.Rows[i]["ClassNo"] == null || dt.Rows[i]["ClassNo"].ToString().Trim() == "" ? null : dt.Rows[i]["ClassNo"].ToString();
                    obj.Type = Convert.ToInt32(dt.Rows[i]["Type"] == null || dt.Rows[i]["Type"].ToString().Trim() == "" ? null : dt.Rows[i]["Type"].ToString());
                    lstEmpDetail.Add(obj);
                }
            }
            return lstEmpDetail;
        }
    }
}