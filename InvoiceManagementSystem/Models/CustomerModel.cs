using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InvoiceManagementSystem.Models
{
    public class CustomerModel
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
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
        public Pager Pager { get; set; }
        public List<CustomerModel> LSTCustomerList { get; set; }
        public CustomerModel addCustomer(CustomerModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Sp_AddCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = cls.CustomerId;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = cls.Name;
                cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = cls.Address;
                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = cls.City;
                
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

        public CustomerModel GetCustomer(CustomerModel cls)
        {
            try
            {
                List<CustomerModel> LSTList = new List<CustomerModel>();
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetSingleCustomerData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerId", cls.CustomerId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        CustomerModel obj = new CustomerModel();
                        obj.CustomerId = Convert.ToInt32(dt.Rows[i]["CustomerId"] == null || dt.Rows[i]["CustomerId"].ToString().Trim() == "" ? null : dt.Rows[i]["CustomerId"].ToString());
                        obj.Name = dt.Rows[i]["Name"] == null || dt.Rows[i]["Name"].ToString().Trim() == "" ? null : dt.Rows[i]["Name"].ToString();
                        obj.Address = dt.Rows[i]["Address"] == null || dt.Rows[i]["Address"].ToString().Trim() == "" ? null : dt.Rows[i]["Address"].ToString();
                        obj.City = dt.Rows[i]["City"] == null || dt.Rows[i]["City"].ToString().Trim() == "" ? null : dt.Rows[i]["City"].ToString();
                        LSTList.Add(obj);
                    }
                }
                cls.LSTCustomerList = LSTList;
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

        public CustomerModel deleteCustomer(CustomerModel cls)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CustomerId", cls.CustomerId);
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