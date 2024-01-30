using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class SchoolModel
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Since { get; set; }
        public List<SchoolModel> LSTSchoolList { get; set; }


        public SchoolModel GetSchoolDetails(SchoolModel cls)
        {
            try
            {
                List<SchoolModel> lstSchoolList = new List<SchoolModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_GetClassRoomList", conn);
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
    }
}