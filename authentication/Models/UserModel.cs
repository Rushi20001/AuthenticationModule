using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace authentication.Models
{
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



        public bool MobileNumberExists(string mobileNumber)
        {
            using (SqlConnection connection=new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
               connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE mobile = @mobile";
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
                string q = "insert into logUser(id,loginOtp,expiretime)values(@id,@otp,@expiretime)";
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

                    string query = "SELECT id FROM Users WHERE mobile = @mobile";
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

