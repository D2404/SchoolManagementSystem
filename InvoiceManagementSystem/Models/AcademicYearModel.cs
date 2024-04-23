using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class AcademicYearModel
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        clsCommon objCommon = new clsCommon();
        public int AcademicYearId { get; set; }
        public string AcademicYear { get; set; }
        public string Response { get; set; }
        public bool IsActive { get; set; }
        public int intActive { get; set; }

        public List<AcademicYearModel> LSTAcademicYearList { get; set; }


        public AcademicYearModel LoadAcademicYear(string AcademicYear)
        {
            AcademicYearModel cls = new AcademicYearModel();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_LoadAcademicYearDetail", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AcademicYearId", AcademicYear);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.ReturnProviderSpecificTypes = true;
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    cls.AcademicYearId = Convert.ToInt32(dt.Rows[0]["AcademicYearId"] == null || dt.Rows[0]["AcademicYearId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["AcademicYearId"].ToString());
                    cls.AcademicYear = dt.Rows[0]["AcademicYear"] == null || dt.Rows[0]["AcademicYear"].ToString().Trim() == "" ? "" : dt.Rows[0]["AcademicYear"].ToString();
                    cls.Response = "Success";
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