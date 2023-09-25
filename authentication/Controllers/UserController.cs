using authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace authentication.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //  public string connectionString = "data source=(LocalDb)\\MSSQLLocalDB;initial catalog=auth;persist security info=True;";
        [HttpPost]
        public ActionResult generateotp(string mobilenumber)
        {
            
            try
            {
                UserModel model = new UserModel();
                bool existMobile = model.MobileNumberExists(mobilenumber);
                if (existMobile) {
                    string otp = model.GenerateRandomOTP(6);
                    DateTime expiretime= DateTime.Now.AddMinutes(5);
                    int userid=model.GetUserIdByMobileNumber(mobilenumber);

                    if(userid > 0)
                    {
                        bool otpinserted= model.InsertOtp(userid, otp, expiretime);
                        if (otpinserted)
                        {
                            return Json(new {success="true",message="otp generated and stored." });
                        }
                        else
                        {
                            return Json(new { success = "false", message = "failed" });
                        }
                    }
                    else
                    {
                        return Json(new { success = "false", message = "user with mobile not found." });
                    }
                }
                else {
                    return Json(new { success = "false", message = "user not found" });
                        }

            }
            catch (Exception ex){ 
              return Json(new { success = "false",message="error:"+ex.Message}); 
            }

        }
    }
}