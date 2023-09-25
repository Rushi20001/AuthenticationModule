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

        public ActionResult login(LoginModel loginModel)
        {
            loginModel model = new loginModel();
            bool result = model.authLogin(loginModel);
            if (result)
            {

                if (model.IsAdmin(loginModel.userEmail))
                {
                    return RedirectToAction("Index","Home");
                }
                else { 
                    return RedirectToAction ("About","Home"); 
                }
               // return RedirectToAction("About", "Home");
               // return View(loginModel);
                //return Json(1);
            }
           // return View();
            else { return Json(2); }

        }
        //public ActionResult login(LoginModel loginModel)
        //{
        //    loginModel model = new loginModel();
        //    if (model.authLogin(loginModel))
        //    {
        //        return RedirectToAction("About", "Home"); }


        //    else if (model.IsAdmin(loginModel.userEmail))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // return RedirectToAction("About", "Home");
        //    // return View(loginModel);
        //    //return Json(1);
        //    return Json(2);

        //}
        // return View();



        [HttpPost]
        public ActionResult MatchOtp(string no)
        {
            LoginModel loginModel = new LoginModel();

            UserModel user = new UserModel();
            if(user.GetOtp(no)!=null )
            {
                bool otp = user.verifyOTP("465704",no);
                if(otp)
                {
                    return Json(new{message="login" });
                }
                else
                {
                    return Json(00);
                }
                //return Json(1);
            }
            else
            {
                return Json(0);
            }

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
                    return Json(new { success = "false", message = "mobile not found" });
                        }

            }
            catch (Exception ex){ 
              return Json(new { success = "false",message="error:"+ex.Message}); 
            }

        }
        
    }
}