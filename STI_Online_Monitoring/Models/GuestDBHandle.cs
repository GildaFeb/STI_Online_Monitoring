
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



namespace STI_Online_Monitoring.Models
{
    public class GuestDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["guestconn"].ToString();
            con = new SqlConnection(constring);
        }

        // **************** ADD NEW GUEST  *********************
        public int AddRequest(VisitModel log)
        {
            string constring = ConfigurationManager.ConnectionStrings["guestconn"].ToString();
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                var timeEntered = log.TimeIn.ToString();
                var timeExited= log.TimeOut.ToString();
                var date = log.DateOfVisit.ToString();
                var status = "Pending";
                SqlCommand com = new SqlCommand("AddNewRequest", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@GuestID", log.GuestID);
                com.Parameters.AddWithValue("@DateOfVisit", date);
                com.Parameters.AddWithValue("@TimeIn", timeEntered);
                com.Parameters.AddWithValue("@TimeOut", timeExited);
                com.Parameters.AddWithValue("@Transactionss", log.Transactions);
                com.Parameters.AddWithValue("@Department", log.Department);
                com.Parameters.AddWithValue("@Status", status);
                
                i = com.ExecuteNonQuery();
            }
            con.Close();
            return i;
           
        }

        // ********** VIEW GUEST  DETAILS ********************
        public List<GuestModel> GetGuest()
        {
            string cs = ConfigurationManager.ConnectionStrings["GuestConn"].ConnectionString;

            List<GuestModel> lst = new List<GuestModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetRequestInfo", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    var nullMiddleName = rdr["Middlename"].ToString();
                    var nullSuffix = rdr["Suffix"].ToString();
                    if (string.IsNullOrEmpty(nullSuffix) && string.IsNullOrEmpty(nullMiddleName))
                    {
                        nullSuffix = "N/A";
                        nullMiddleName = "N/A";
                        lst.Add(new GuestModel
                        {

                            GuestID = Convert.ToInt32(rdr["GuestID"]),
                            ContactNumber = rdr["GuestCNumber"].ToString(),
                            Address = rdr["Address"].ToString(),
                            Password = rdr["password"].ToString(),
                            FirstName = rdr["Firstname"].ToString(),
                            LasttName = rdr["Lastname"].ToString(),
                            MiddleName = nullMiddleName,
                            Suffix = null,
                            Gender = rdr["Gender"].ToString(),
                            Email = rdr["Email"].ToString()
                        });
                    }
                    else if (string.IsNullOrEmpty(nullSuffix))
                    {
                        nullSuffix = "N/A";
                        lst.Add(new GuestModel
                        {

                            GuestID = Convert.ToInt32(rdr["GuestID"]),
                            ContactNumber = rdr["GuestCNumber"].ToString(),
                            Address = rdr["Address"].ToString(),
                            Password = rdr["password"].ToString(),
                            FirstName = rdr["Firstname"].ToString(),
                            LasttName = rdr["Lastname"].ToString(),
                            MiddleName = rdr["Middlename"].ToString(),
                            Suffix = nullSuffix,
                            Gender = rdr["Gender"].ToString(),
                            Email = rdr["Email"].ToString()
                        });
                    }
                    else if (string.IsNullOrEmpty(nullMiddleName))
                    {
                        nullMiddleName  = "N/A";
                        lst.Add(new GuestModel
                        {

                            GuestID = Convert.ToInt32(rdr["GuestID"]),
                            ContactNumber = rdr["GuestCNumber"].ToString(),
                            Address = rdr["Address"].ToString(),
                            Password = rdr["password"].ToString(),
                            FirstName = rdr["Firstname"].ToString(),
                            LasttName = rdr["Lastname"].ToString(),
                            MiddleName = nullMiddleName,
                            Suffix = rdr["Suffix"].ToString(),
                            Gender = rdr["Gender"].ToString(),
                            Email = rdr["Email"].ToString()
                        });
                    }
                    else
                    {
                        lst.Add(new GuestModel
                        {

                            GuestID = Convert.ToInt32(rdr["GuestID"]),
                            ContactNumber = rdr["GuestCNumber"].ToString(),
                            Address = rdr["Address"].ToString(),
                            Password = rdr["password"].ToString(),
                            FirstName = rdr["Firstname"].ToString(),
                            LasttName = rdr["Lastname"].ToString(),
                            MiddleName = rdr["Middlename"].ToString(),
                            Suffix = rdr["Suffix"].ToString(),
                            Gender = rdr["Gender"].ToString(),
                            Email = rdr["Email"].ToString()
                        });
                    }
                    
                }
                return lst;
            }


        }

        // ***************** UPDATE GUEST  DETAILS *********************
        public bool UpdateDetails(VisitModel smodel)
        {
            connection();
            var timeExited = smodel.TimeOut;
            SqlCommand cmd = new SqlCommand("UpdateRequestDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (string.IsNullOrEmpty(timeExited))
            {
                
                timeExited = "Pending".ToString();
                cmd.Parameters.AddWithValue("@TimeOut", timeExited);
                cmd.Parameters.AddWithValue("@GstID", smodel.VisitLogID);
                cmd.Parameters.AddWithValue("@GuestID", smodel.GuestID);
                cmd.Parameters.AddWithValue("@DateOfVisit", smodel.DateOfVisit);
                var timeEntered = smodel.TimeIn.ToString();
                cmd.Parameters.AddWithValue("@TimeIn", timeEntered);
                cmd.Parameters.AddWithValue("@Department", smodel.Department);
                cmd.Parameters.AddWithValue("@Transactions", smodel.Transactions);
                cmd.Parameters.AddWithValue("@Status", smodel.Status);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            else
            {
                cmd.Parameters.AddWithValue("@TimeOut", timeExited);
                cmd.Parameters.AddWithValue("@GstID", smodel.VisitLogID);
                cmd.Parameters.AddWithValue("@GuestID", smodel.GuestID);
                cmd.Parameters.AddWithValue("@DateOfVisit", smodel.DateOfVisit);
                var timeEntered = smodel.TimeIn.ToString();
                cmd.Parameters.AddWithValue("@TimeIn", timeEntered);
                cmd.Parameters.AddWithValue("@Department", smodel.Department);
                cmd.Parameters.AddWithValue("@Transactions", smodel.Transactions);
                cmd.Parameters.AddWithValue("@Status", smodel.Status);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }


          
        }

        // ********************** DELETE REQUEST  DETAILS *******************
        public bool DeleteRequest(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@GstID", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }





        // ********************** UPDATE GUEST  DETAILS *******************
        // ********************** GET GUEST  DETAILS *******************


        public bool UpdateGuestDetails(GuestModel smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateGuestDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@GuestID", smodel.GuestID);
            cmd.Parameters.AddWithValue("@GuestCNumber", smodel.ContactNumber);
            cmd.Parameters.AddWithValue("@Address", smodel.Address);
            cmd.Parameters.AddWithValue("@Password", smodel.Password);
            cmd.Parameters.AddWithValue("@FirstName", smodel.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", smodel.MiddleName);
            cmd.Parameters.AddWithValue("@Suffix", smodel.Suffix);
            cmd.Parameters.AddWithValue("@Email", smodel.Email);
            cmd.Parameters.AddWithValue("@Gender", smodel.Gender);
            cmd.Parameters.AddWithValue("@LastName", smodel.LasttName);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }



        // ********** GET GUEST  VISIT LOG DETAILS ********************
        public List<VisitModel> GetGuestVisitlLog()
        {
            connection();
            List<VisitModel> guestlist = new List<VisitModel>();

            SqlCommand cmd = new SqlCommand("GetRequestDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                guestlist.Add(
                    new VisitModel
                    {
                        VisitLogID = Convert.ToInt32(dr["VisitLogID"]),
                        GuestID = Convert.ToInt32(dr["GuestID"]),
                        DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                        TimeIn = Convert.ToString(dr["TimeIn"]),
                        TimeOut = Convert.ToString(dr["TimeOut"]),
                        Department = Convert.ToString(dr["Department"]),
                        Transactions = Convert.ToString(dr["Transactions"])

                    });
            }
            return guestlist;
        }

        //************************** LIST ********************
        // ********** VIEW REQUEST DETAILS ********************
        public List<VisitModel> GetRequest()
        {
            connection();
            List<VisitModel> studentlist = new List<VisitModel>();

            SqlCommand cmd = new SqlCommand("SelectRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                studentlist.Add(
                    new VisitModel
                    {
                        VisitLogID= Convert.ToInt32(dr["VisitLogID"]),
                        GuestID = Convert.ToInt32(dr["GuestID"]),
                        DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                        TimeIn = Convert.ToString(dr["TimeOut"]),
                        TimeOut = Convert.ToString(dr["TimeIn"]),
                        Department = Convert.ToString(dr["Department"]),
                        Transactions = Convert.ToString(dr["Transactions"]),
                        Status = Convert.ToString(dr["Status"]),
                    });
            }
            return studentlist;
        }

        //************************** LIST ********************
        // ********** VIEW REQUEST DETAILS ********************
        public List<VisitModel> GetCashier()
        {
            connection();
            List<VisitModel> studentlist = new List<VisitModel>();

            SqlCommand cmd = new SqlCommand("GetCashierRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                studentlist.Add(
                    new VisitModel
                    {
                        VisitLogID = Convert.ToInt32(dr["VisitLogID"]),
                        GuestID = Convert.ToInt32(dr["GuestID"]),
                        DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                        Department = Convert.ToString(dr["Department"]),
                        Transactions = Convert.ToString(dr["Transactions"]),
                        Status = Convert.ToString(dr["Status"]),
                    });
            }
            return studentlist;
        }



    }
}