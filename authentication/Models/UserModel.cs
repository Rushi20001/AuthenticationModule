using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

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

  

    public class UserModel
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

      public string GetOtp(string number)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string query = "select lu.LoginOtp from Users u join LoginUser lu on u.userId=lu.userIdByLoginUser where u.MobileNo=@number";

            SqlCommand cmd=new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@number",number);
            connection.Open();
            string count = cmd.ExecuteScalar() as string;
            connection.Close();
            return count;
            //int otp;
            //if (int.TryParse(cmd.ExecuteScalar()?.ToString(), out otp))
            //{
            //    connection.Close();
            //    return otp;
            //}
            //else
            //{
            //    // Handle the case where the conversion fails (e.g., the result is not an integer)
            //    connection.Close();
            //    // You might want to return a default value or handle the error differently here
            //    throw new Exception("Unable to retrieve OTP.");
            //}
        }

        public bool verifyOTP(string uotp,string rajesh )
        {
            // string uotp = "465704";
        
            string count=GetOtp(rajesh);
            if (count == uotp)
            {
                return true;
            }
            else
                return false;
        }

        public bool MobileNumberExists(string mobileNumber)
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
               connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE MobileNo = @mobile";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@mobile", mobileNumber);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool InsertOtp(int userid,string otp,DateTime expiretime)
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
                        return 0; // Return 0 to indicate that the user was not found
                    }

                }
            } 
        }





    }

