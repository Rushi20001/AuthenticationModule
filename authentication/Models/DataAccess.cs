using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Vonage.Users;

namespace authentication.Models
{

    public class loginModel
    {
        public bool authLogin(LoginModel model)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string q = "select count(*) from Users where userEmail=@userEmail and userPassword=@userPassword";
            SqlCommand cmd = new SqlCommand(q,conn);
            cmd.Parameters.AddWithValue("@userEmail", model.userEmail);
            cmd.Parameters.AddWithValue("@userPassword", model.userPassword);
            conn.Open();
             int count=(int)cmd.ExecuteScalar();
            conn.Close();
            if (count > 0)
            {
                bool isadmin = IsAdmin(model.userEmail);
                if (isadmin)
                {
                    return true;
                }


            }
            //return false;
            return count > 0;
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //DataTable dataTable = new DataTable();

        }
        public bool IsAdmin(string userEmail)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string query = "select roles from users where userEmail=@userEmail and Roles='Admin'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userEmail", userEmail);
            conn.Open();
            string role = (string)cmd.ExecuteScalar();
            conn.Close();
            return role == "Admin";
        }
    }

  

    public class DataAccess
    {
        //public SqlConnection _connection;
        //public UserModel()
        //{
        //    _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        //}
        //private SqlConnection connectionString;

        //public UserModel()
        //{
        //   connectionString = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

        //}

        //public int GetOtp( int userid,out int storeotp)
        //  {
        //      SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        //      string query = "select lu.LoginOtp from Users u join LoginUser lu on u.userId=lu.userIdByLoginUser where lu.LoginUserId=@id";

        //      SqlCommand cmd=new SqlCommand(query,connection);
        //      cmd.Parameters.AddWithValue("@id",userid);
        //      connection.Open();
        //      int count = (int)cmd.ExecuteScalar() ;
        //      connection.Close();
        //      if (count>0)
        //      {
        //          storeotp = count;
        //          return storeotp;
        //      }



        //  }
        public int GetOtp( int userId,out string message )
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string query = "SELECT lu.LoginOtp FROM Users u JOIN LoginUser lu ON u.userId = lu.userIdByLoginUser WHERE lu.LoginUserId = @id and lu.ExpirationLoginTime>GETDATE()";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", userId);
            connection.Open();
            int count = (int)(cmd.ExecuteScalar() ?? 0); // Use null coalescing to handle null results
            connection.Close();
            if (count == 0)
            {
                message = "Time Expired";
                return 0;
            }
            else
            {
                message=string.Empty;

                return count;
            }
             // Set the out parameter here
 
        }
        public int getidfromOtp(int userotp,out string msg)
        {
           
            
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                string query = "SELECT LoginUserId FROM LoginUser where LoginOtp=@otp";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@otp", userotp);
                connection.Open();
                object c = cmd.ExecuteScalar();
                connection.Close();
                if (c == null)
                 {
                msg = "Otp not matched";
                    return 0;
                
                }

            msg = null;
            //else { msg = "user found"; return (int)c; }
            return (int)c;
            }

        //public bool verifyOTP(string uotp,int userid )
        //{
        //    // string uotp = "465704";
                             
        //    string count=GetOtp(userid);
        //    if (count == uotp)
        //    {
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        public bool MobileNumberExists(string MobileNo)
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
               connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE MobileNo = @mobile";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@mobile", MobileNo);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool InsertOtp(int userid,int otp,DateTime expiretime)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                string q = "insert into LoginUser(userIdByLoginUser,LoginOtp,ExpirationLoginTime)values(@id,@otp,@expiretime)";
                using (SqlCommand cmd=new SqlCommand(q,connection))
                {
                    cmd.Parameters.AddWithValue("@id",userid);
                    cmd.Parameters.AddWithValue("@otp",otp);
                    cmd.Parameters.AddWithValue("@expiretime",expiretime);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }           
            }
        }

        public string GenerateRandomOTP(int length)
        {
            const string validChars = "0123456789";
            Random random = new Random();
            char[] otpChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                otpChars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(otpChars);
        }

        public int GetUserIdByMobileNumber(string mobileNumber)
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();

                    string query = "SELECT userId FROM Users WHERE MobileNo = @mobile";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@mobile", mobileNumber);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return (int)result;
                    }
                    else
                    {
                        return 0; 
                    }

                }
            }

        //internal static object GenerateRandomOtp(int v)
        //{
        //    throw new NotImplementedException();
        //}

        public bool checktime(LoginUser user)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string q = "select ExpirationLoginTime,userIdByLoginUser from LoginUser where ExpirationLoginTime>GETDATE() and" +
                " userIdByLoginUser=@userIdByLoginUser";
             SqlCommand cmd=new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue ("@userIdByLoginUser", user.LoginUserId);
            conn.Open();
            int row=(int)cmd.ExecuteScalar();
            conn.Close();
            return true;
        }
    }





    }

