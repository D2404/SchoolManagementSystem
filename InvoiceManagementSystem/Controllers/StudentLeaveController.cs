using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManagementSystem.Controllers
{
    public class StudentLeaveController : Controller
    {
        clsCommon objCommon = new clsCommon();

        // GET: StudentLeave
        public ActionResult StudentLeave()
        {
            if (objCommon.getUserIdFromSession() != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]

        public ActionResult InsertStudentLeave(LeaveModel model)
        {
            model = model.addLeave(model);
            return Json(model.Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStudentLeave(LeaveModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                var TeacherId = objCommon.getTeacherIdFromSession();
                var StudentId = objCommon.getStudentIdFromSession();
                List<LeaveModel> lstStudentLeaveList = new List<LeaveModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetStudentLeaveList", conn);
                cmd.Parameters.AddWithValue("@PageSize", cls.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", cls.PageIndex);
                cmd.Parameters.AddWithValue("@Search", cls.SearchText);
                cmd.Parameters.AddWithValue("@TeacherId", TeacherId);
                cmd.Parameters.AddWithValue("@StudentId", StudentId);
                cmd.Parameters.AddWithValue("@SchoolId", objCommon.getSchoolIdFromSession());
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
                        LeaveModel obj = new LeaveModel();

                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Status = Convert.ToInt32(dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString());
                        obj.NoOfDays = Convert.ToDecimal(dt.Rows[i]["NoOfDays"] == null || dt.Rows[i]["NoOfDays"].ToString().Trim() == "" ? null : dt.Rows[i]["NoOfDays"].ToString());
                        obj.StudentName = dt.Rows[i]["StudentName"] == null || dt.Rows[i]["StudentName"].ToString().Trim() == "" ? null : dt.Rows[i]["StudentName"].ToString();
                        obj.RollNo = Convert.ToInt32(dt.Rows[i]["RollNo"] == null || dt.Rows[i]["RollNo"].ToString().Trim() == "" ? null : dt.Rows[i]["RollNo"].ToString());
                        obj.TeacherId = (int)TeacherId;
                        obj.FromDate = dt.Rows[i]["FromDate"] == null || dt.Rows[i]["FromDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("dd/MM/yyyy");
                        obj.ToDate = dt.Rows[i]["ToDate"] == null || dt.Rows[i]["ToDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("dd/MM/yyyy");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();
                        obj.ROWNUMBER = Convert.ToInt32(dt.Rows[i]["ROWNUMBER"] == null || dt.Rows[i]["ROWNUMBER"].ToString().Trim() == "" ? null : dt.Rows[i]["ROWNUMBER"].ToString());
                        obj.PageCount = Convert.ToInt32(dt.Rows[i]["PageCount"] == null || dt.Rows[i]["PageCount"].ToString().Trim() == "" ? null : dt.Rows[i]["PageCount"].ToString());
                        obj.PageSize = Convert.ToInt32(dt.Rows[i]["PageSize"] == null || dt.Rows[i]["PageSize"].ToString().Trim() == "" ? null : dt.Rows[i]["PageSize"].ToString());
                        obj.PageIndex = Convert.ToInt32(dt.Rows[i]["PageIndex"] == null || dt.Rows[i]["PageIndex"].ToString().Trim() == "" ? null : dt.Rows[i]["PageIndex"].ToString());
                        obj.TotalRecord = Convert.ToInt32(dt.Rows[i]["TotalRecord"] == null || dt.Rows[i]["TotalRecord"].ToString().Trim() == "" ? null : dt.Rows[i]["TotalRecord"].ToString());
                        lstStudentLeaveList.Add(obj);
                    }
                }
                cls.LSTLeaveList = lstStudentLeaveList;
                if (cls.LSTLeaveList.Count > 0)
                {
                    var pager = new Models.Pager((int)cls.LSTLeaveList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                    cls.Pager = pager;
                }
                cls.TotalEntries = TotalEntries;
                cls.ShowingEntries = showingEntries;
                cls.fromEntries = startentries;
                cls.LSTLeaveList = lstStudentLeaveList;

                return PartialView("_StudentLeaveListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult GetSingleStudentLeave(LeaveModel cls)
        {
            try
            {
                int TotalEntries = 0;
                int showingEntries = 0;
                int startentries = 0;
                List<LeaveModel> lstStudentLeaveList = new List<LeaveModel>();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetSingleStudentLeave", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cls.Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                conn.Close();


                if (dt != null && dt.Rows.Count > 0)
                {

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        LeaveModel obj = new LeaveModel();
                        obj.Id = Convert.ToInt32(dt.Rows[i]["Id"] == null || dt.Rows[i]["Id"].ToString().Trim() == "" ? null : dt.Rows[i]["Id"].ToString());
                        obj.Status = Convert.ToInt32(dt.Rows[i]["Status"] == null || dt.Rows[i]["Status"].ToString().Trim() == "" ? null : dt.Rows[i]["Status"].ToString());
                        obj.NoOfDays = Convert.ToDecimal(dt.Rows[i]["NoOfDays"] == null || dt.Rows[i]["NoOfDays"].ToString().Trim() == "" ? null : dt.Rows[i]["NoOfDays"].ToString());
                        obj.TeacherName = dt.Rows[i]["TeacherName"] == null || dt.Rows[i]["TeacherName"].ToString().Trim() == "" ? null : dt.Rows[i]["TeacherName"].ToString();
                        obj.FromDate = dt.Rows[i]["FromDate"] == null || dt.Rows[i]["FromDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("yyyy/MM/dd");
                        obj.ToDate = dt.Rows[i]["ToDate"] == null || dt.Rows[i]["ToDate"].ToString().Trim() == "" ? null : Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("yyyy/MM/dd");
                        obj.LeaveType = Convert.ToInt32(dt.Rows[i]["LeaveType"] == null || dt.Rows[i]["LeaveType"].ToString().Trim() == "" ? null : dt.Rows[i]["LeaveType"].ToString());
                        obj.Reason = dt.Rows[i]["Reason"] == null || dt.Rows[i]["Reason"].ToString().Trim() == "" ? null : dt.Rows[i]["Reason"].ToString();

                        lstStudentLeaveList.Add(obj);
                    }
                }
                cls.LSTLeaveList = lstStudentLeaveList;
                //if (cls.LSTStudentLeaveList.Count > 0)
                //{
                //    var pager = new Models.Pager((int)cls.LSTStudentLeaveList[0].TotalRecord, cls.PageIndex, (int)cls.PageSize);

                //    cls.Pager = pager;
                //}
                //cls.TotalEntries = TotalEntries;
                //cls.ShowingEntries = showingEntries;
                //cls.fromEntries = startentries;
                //cls.LSTStudentLeaveList = lstStudentLeaveList;

                return PartialView("_StudentLeaveListPartial", cls);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deleteStudentLeave(LeaveModel cls)
        {
            try
            {
                cls = cls.deleteLeave(cls);
                return Json(cls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ApproveStatus(LeaveModel cls)
        {
            try
            {
                var Status = cls.ApproveStatus(cls);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult RejectStatus(LeaveModel cls)
        {
            try
            {
                var Status = cls.RejectStatus(cls);
                return Json(Status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LeaveMail(LeaveModel cls)
        {
            clsCommon commonobj = new clsCommon();
            var data = cls.StudentLeaveMail(cls);
            if (data.LSTLeaveList[0].Response == "Success")
            {
                string toEmail = cls.Email;
                string fromEmail = cls.LSTLeaveList[0].FromMail;
                //var Role = commonobj.getRoleNameFromSession();
                // var Password = clsCommon.DecryptString(data.LSTLeaveList[i].Password);
                string subject = "Leave Application";
                string body = cls.Reason;
                using (StreamReader reader = new StreamReader(Server.MapPath("/Data/MailTemplate/LeaveMail.html")))
                {
                    body = reader.ReadToEnd();
                }
                string imageBase64 = string.Empty;
                string imagePath = string.Empty;

                //body = body.Replace("[[Profile]]", $"cid:logoImage");
                body = body.Replace("[[UserName]]", data.LSTLeaveList[0].UserName);
                //body = body.Replace("[[EmailId]]", data.LSTLeaveList[i].Email);
                //body = body.Replace("[[Password]]", Password);
                body = body.Replace("[[FromDate]]", data.LSTLeaveList[0].FromDate);
                body = body.Replace("[[ToDate]]", data.LSTLeaveList[0].ToDate);
                body = body.Replace("[[Reason]]", data.LSTLeaveList[0].Reason);
                body = body.Replace("[[LeaveTypeName]]", data.LSTLeaveList[0].LeaveTypeName);
                if (cls.LeaveType == 1)
                {
                    body = body.Replace("[[LeaveSubTypeName]]", data.LSTLeaveList[0].LeaveSubTypeName);
                }
                sendEmail(fromEmail, toEmail, subject, body, imagePath); // Pass Email instead of toEmail
                cls.Response = "Success";


            }
            else
            {
                cls.Response = "Error";
            }
            return Json(cls.Response, JsonRequestBehavior.AllowGet);
        }


        public void sendEmail(string fromEmail, string toEmail, string subject, string body, string imagePath)
        {
            try
            {
                clsCommon obj = new clsCommon();
                //string[] TempEmail = toEmail.Split(',');

                //for (int i = 0; i < TempEmail.Length; i++)
                //{
                string to = toEmail;
                //string to = toEmail;

                var EmailConfigaration = obj.EmailConfigaration();
                string host = EmailConfigaration.Host;
                string username = EmailConfigaration.Username;
                string FromEmail = fromEmail;
                string password = EmailConfigaration.Password;
                int port = EmailConfigaration.Port;

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(FromEmail);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                    LinkedResource logo = new LinkedResource(imagePath, "image/jpg");
                    logo.ContentId = "logoImage";

                    av.LinkedResources.Add(logo);

                    mail.AlternateViews.Add(av);
                }
                SmtpClient smtp = new SmtpClient();

                smtp.Host = host;
                smtp.Credentials = new System.Net.NetworkCredential
                     (FromEmail, password);
                smtp.Port = port;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                //}

            }
            catch (Exception ex)
            {

            }

        }
    }
}