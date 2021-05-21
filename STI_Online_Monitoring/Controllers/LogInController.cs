using STI_Online_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace STI_Online_Monitoring.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public string status;
        // 3. ************* LOG IN PAGE DETAILS ******************
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(GuestModel e)
        {

            try
            {

                String SqlCon = ConfigurationManager.ConnectionStrings["GuestConn"].ConnectionString;
                SqlConnection con = new SqlConnection(SqlCon);
                string SqlQuery = "select Email,Password from GuestTbl where Email=@Email and Password=@Password";
                con.Open();
                SqlCommand cmd = new SqlCommand(SqlQuery, con); ;
                cmd.Parameters.AddWithValue("@Email", e.Email);
                cmd.Parameters.AddWithValue("@Password", e.Password);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    if (e.Type == "Guest")
                    {
                        Session["Email"] = e.Email.ToString();
                        return RedirectToAction("Welcome", "LogIn");
                    }
                    else if (e.Type == "Guard")
                    {
                        Session["Email"] = e.Email.ToString();
                        return RedirectToAction("Guard", "Home");
                    }
                    else if (e.Type == "Cashier")
                    {
                        Session["Email"] = e.Email.ToString();
                        return RedirectToAction("Cashier", "Monitoring");
                    }
                    else
                    {
                        ViewData["Message"] = "You do not have" + e.Type + "Account";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Email or Password is not Correct.";
                }
                con.Close();
                return View();
            }
            catch (Exception i)
            {

                return View(i);
            }

        }



        // 3. *********** GUEST PAGE: GUEST DETAILS ****************
        [HttpGet]
        public ActionResult Welcome()
        {
            GuestModel user = new GuestModel();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection("Data Source=localhost\\MSSQLSERVER02;Initial Catalog=finfoDB;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("GetGuestDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 30).Value = Session["Email"].ToString();
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    List<GuestModel> userlist = new List<GuestModel>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GuestModel detail = new GuestModel();
                        detail.GuestID = Convert.ToInt32(ds.Tables[0].Rows[i]["GuestID"].ToString());
                        detail.LasttName = ds.Tables[0].Rows[i]["Lastname"].ToString();
                        detail.FirstName = ds.Tables[0].Rows[i]["Firstname"].ToString();
                        detail.MiddleName = ds.Tables[0].Rows[i]["Middlename"].ToString();
                        detail.Suffix = ds.Tables[0].Rows[i]["Suffix"].ToString();
                        detail.Gender = ds.Tables[0].Rows[i]["Gender"].ToString();
                        detail.ContactNumber = ds.Tables[0].Rows[i]["GuestCNumber"].ToString();
                        detail.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                        detail.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        string EncryptionKey = "MAKV2SPBNI99212";
                        string password = ds.Tables[0].Rows[i]["Password"].ToString();
                        byte[] clearBytes = Encoding.Unicode.GetBytes(password);
                        using (Aes encryptor = Aes.Create())
                        {
                            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                            encryptor.Key = pdb.GetBytes(32);
                            encryptor.IV = pdb.GetBytes(16);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    cs.Write(clearBytes, 0, clearBytes.Length);
                                    cs.Close();
                                }
                                password = Convert.ToBase64String(ms.ToArray());
                            }
                        }
                        detail.Password = password;
                        if (string.IsNullOrEmpty(detail.MiddleName) && string.IsNullOrEmpty(detail.Suffix))
                        {
                            detail.Suffix = "N/A";
                            detail.MiddleName = "N/A";
                            userlist.Add(detail);
                        }
                        else if (string.IsNullOrEmpty(detail.Suffix))
                        {
                            detail.Suffix = "N/A";
                            userlist.Add(detail);
                        }
                        else if (string.IsNullOrEmpty(detail.MiddleName))
                        {
                            detail.MiddleName = "N/A";
                            userlist.Add(detail);
                        }
                        else
                        {
                            userlist.Add(detail);
                        }
                    }






                    user.Guestinfo = userlist;
                }
                con.Close();

            }
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home", "Home");


        }
        //  *********** EDIT GUEST DETAILS ****************
        // GET: Student/Edit/5
        public ActionResult EditGuestDetails(int id)
        {
            GuestDBHandle sdb = new GuestDBHandle();
            return View(sdb.GetGuest().Find(smodel => smodel.GuestID == id));
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult EditGuestDetails(GuestModel smodel)
        {
            try
            {
                GuestDBHandle sdb = new GuestDBHandle();
                sdb.UpdateGuestDetails(smodel);
                return RedirectToAction("Welcome");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }


        //============ VALIDATION FOR GUEST VISIT LOG =======

        [HttpPost]
        public ActionResult Welcome(GuestModel e)
        {
            try
            {
                String SqlCon = ConfigurationManager.ConnectionStrings["GuestConn"].ConnectionString;
                SqlConnection con = new SqlConnection(SqlCon);
                string SqlQuery = "select GuestID, Password from GuestTbl where GuestID=@GuestID and Password=@Password";
                con.Open();
                SqlCommand cmd = new SqlCommand(SqlQuery, con); ;
                cmd.Parameters.AddWithValue("@GuestID", e.GuestID);
                cmd.Parameters.AddWithValue("@Password", e.Password);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    Session["GuestID"] = e.GuestID.ToString();
                    return RedirectToAction("GuestRequest", "LogIn");
                }
                else
                {
                    ViewData["Message"] = "Password is not Correct.";
                }


                con.Close();
                return View();


            }
            catch
            {
                ViewData["Message"] = "";
                return View();
            }
        }

        // ========================= TABLE: GUEST REQUESTS ====================
        [HttpGet]
        public ActionResult GuestRequest()
        {
            try
            {
                VisitModel user = new VisitModel();
                DataSet ds = new DataSet();

                using (SqlConnection con = new SqlConnection("Data Source=localhost\\MSSQLSERVER02;Initial Catalog=finfoDB;Integrated Security=True"))
                {

                    using (SqlCommand cmd = new SqlCommand("GetGuestRequestTable", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@GuestID", SqlDbType.Int).Value = Session["GuestID"].ToString();
                        con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);
                        List<VisitModel> userlist = new List<VisitModel>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            VisitModel uobj = new VisitModel();
                            uobj.VisitLogID = Convert.ToInt32(ds.Tables[0].Rows[i]["VisitLogID"].ToString());
                            uobj.GuestID = Convert.ToInt32(ds.Tables[0].Rows[i]["GuestID"].ToString());
                            uobj.DateOfVisit = ds.Tables[0].Rows[i]["DateOfVisit"].ToString();
                            uobj.TimeIn = ds.Tables[0].Rows[i]["TimeIn"].ToString();
                            uobj.TimeOut = ds.Tables[0].Rows[i]["TimeOut"].ToString();
                            uobj.Department = ds.Tables[0].Rows[i]["Department"].ToString();
                            uobj.Transactions = ds.Tables[0].Rows[i]["Transactions"].ToString();
                            uobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                            userlist.Add(uobj);
                        }
                        user.Visitinfo = userlist;
                    }
                    con.Close();

                }

                return View(user);
            }
            catch
            {
                ViewData["Message"] = " ";
            }
            return View();
        }



        // =================================================================
        //==================== GUEST GUEST  TABLE =========================
        //=================================================================
        public JsonResult GetbyID(int ID)
        {
            GuestDBHandle guestDB = new GuestDBHandle();
            var Guest = guestDB.GetGuest().Find(x => x.GuestID.Equals(ID));
            return Json(Guest, JsonRequestBehavior.AllowGet);
        }
        public JsonResult List()
        {
            GuestDBHandle guestDB = new GuestDBHandle();
            return Json(guestDB.GetGuest(), JsonRequestBehavior.AllowGet);
        }


        // =================================================================
        //==================== GUEST REQUEST TABLE =========================
        //=================================================================

        GuestDBHandle GuestDB = new GuestDBHandle();
        public JsonResult AddRequest(VisitModel request)
        {
            return Json(GuestDB.AddRequest(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(GuestModel request)
        {
            return Json(GuestDB.UpdateGuestDetails(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRequest(VisitModel request)
        {
            return Json(GuestDB.UpdateDetails(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRequest(int id)
        {
            try
            {
                GuestDBHandle sdb = new GuestDBHandle();
                if (sdb.DeleteRequest(id))
                {
                    ViewBag.AlertMsg = "Form Deleted Successfully";
                }
                return RedirectToAction("GuestRequest");
            }
            catch
            {
                return View();
            }
        }


        // ============ EDIT ====================


        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            GuestDBHandle sdb = new GuestDBHandle();
            return View(sdb.GetRequest().Find(smodel => smodel.VisitLogID == id));
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, VisitModel smodel)
        {
            try
            {
                GuestDBHandle sdb = new GuestDBHandle();
                sdb.UpdateDetails(smodel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}