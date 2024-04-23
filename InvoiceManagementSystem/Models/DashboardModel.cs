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
        public int TotalSchool { get; set; }
        public int TotalAdmin { get; set; }
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
        public string NextHolidayName { get; set; }
        public string NextHolidayDate { get; set; }
        public string NextHolidayDay { get; set; }

        public decimal MonthlyAttendance { get; set; }
        public decimal YearlyAttendance { get; set; }
        public decimal PresentDays { get; set; }
        public decimal AbsentDays { get; set; }
        public string Response { get; set; }
        public string Dob { get; set; }
        public string Name { get; set; }

        public string ClassNo { get; set; }
        public string CurrentClass { get; set; }
        public string CurrentSubject { get; set; }
        public string NextClass { get; set; }
        public string NextSubject { get; set; }
        public int Type { get; set; }

        public string IsMessage { get; set; }
        public List<DashboardModel> LSTDashBoardList { get; set; }
        public List<DashboardModel> LSTTeacherList { get; set; }
        public List<DashboardModel> LSTStudentList { get; set; }


        public DashboardModel GetSAAccountDashboardCount(DashboardModel cls)
        {
            try
            {
                List<DashboardModel> LSTList = new List<DashboardModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetSADashboardCountList", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        DashboardModel obj = new DashboardModel();
                        obj.TotalSchool = Convert.ToInt32(dt.Rows[i]["TotalSchool"] == null || dt.Rows[i]["TotalSchool"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalSchool"].ToString());
                        obj.TotalAdmin = Convert.ToInt32(dt.Rows[i]["TotalAdmin"] == null || dt.Rows[i]["TotalAdmin"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalAdmin"].ToString());
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
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
                cmd.Parameters.AddWithValue("@AcademicYear", objCommon.getAcademicYearFromSession());
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
                        obj.NextHolidayName = dt.Rows[i]["NextHolidayName"] == null || dt.Rows[i]["NextHolidayName"].ToString().Trim() == "" ? null : dt.Rows[i]["NextHolidayName"].ToString();
                        obj.NextHolidayDate = dt.Rows[i]["NextHolidayDate"] == null || dt.Rows[i]["NextHolidayDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["NextHolidayDate"]).ToString("dd/MM/yyyy");
                        obj.NextHolidayDay = dt.Rows[i]["NextHolidayDay"] == null || dt.Rows[i]["NextHolidayDay"].ToString().Trim() == "" ? null : dt.Rows[i]["NextHolidayDay"].ToString();

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
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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
                        obj.MonthlyAttendance = Convert.ToDecimal(dt.Rows[i]["MonthlyAttendance"] == null || dt.Rows[i]["MonthlyAttendance"].ToString().Trim() == "" ? null : dt.Rows[i]["MonthlyAttendance"]);
                        obj.YearlyAttendance = Convert.ToDecimal(dt.Rows[i]["YearlyAttendance"] == null || dt.Rows[i]["YearlyAttendance"].ToString().Trim() == "" ? null : dt.Rows[i]["YearlyAttendance"]);
                        obj.CurrentClass = dt.Rows[i]["CurrentClass"] == null || dt.Rows[i]["CurrentClass"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentClass"].ToString();
                        obj.CurrentSubject = dt.Rows[i]["CurrentSubject"] == null || dt.Rows[i]["CurrentSubject"].ToString().Trim() == "" ? null : dt.Rows[i]["CurrentSubject"].ToString();
                        obj.NextClass = dt.Rows[i]["NextClass"] == null || dt.Rows[i]["NextClass"].ToString().Trim() == "" ? null : dt.Rows[i]["NextClass"].ToString();
                        obj.NextSubject = dt.Rows[i]["NextSubject"] == null || dt.Rows[i]["NextSubject"].ToString().Trim() == "" ? null : dt.Rows[i]["NextSubject"].ToString();
                        obj.NextHolidayName = dt.Rows[i]["NextHolidayName"] == null || dt.Rows[i]["NextHolidayName"].ToString().Trim() == "" ? null : dt.Rows[i]["NextHolidayName"].ToString();
                        obj.NextHolidayDay = dt.Rows[i]["NextHolidayDay"] == null || dt.Rows[i]["NextHolidayDay"].ToString().Trim() == "" ? null : dt.Rows[i]["NextHolidayDay"].ToString();
                        obj.NextHolidayDate = dt.Rows[i]["NextHolidayDate"] == null || dt.Rows[i]["NextHolidayDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["NextHolidayDate"]).ToString("dd/MM/yyyy");
                        obj.ThisMonthCollection = Convert.ToInt32(dt.Rows[i]["ThisMonthCollection"] == null || dt.Rows[i]["ThisMonthCollection"].ToString().Trim() == "" ? null : dt.Rows[i]["ThisMonthCollection"].ToString());
                        obj.ThisMonthPendingCollection = Convert.ToInt32(dt.Rows[i]["ThisMonthPendingCollection"] == null || dt.Rows[i]["ThisMonthPendingCollection"].ToString().Trim() == "" ? null : dt.Rows[i]["ThisMonthPendingCollection"].ToString());
                        obj.TotalAmountForThisMonth = Convert.ToInt32(dt.Rows[i]["TotalAmountForthisMonth"] == null || dt.Rows[i]["TotalAmountForthisMonth"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalAmountForthisMonth"].ToString());
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
            cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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
            cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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