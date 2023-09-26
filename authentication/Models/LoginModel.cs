using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace authentication.Models
{
    public class LoginModel
    {
        public int userId { get; set; }
       public string firstname{ get; set; }
       public string lastname{ get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
     
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Roles { get; set; }
    }

    public class LoginUser
    {
        public int LoginUserId { get; set; }
        public string LoginOtp { get; set; }

        public DateTime ExpirationLoginTime { get; set; }
        public int userIdByLoginUser { get; set; }
    }

}