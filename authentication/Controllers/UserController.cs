using authentication.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Vonage;
using Vonage.Common;
using Vonage.Request;

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
        public ActionResult MatchOtp(int userotp)
        {

            //LoginModel loginModel = new LoginModel();

            DataAccess user = new DataAccess();

            int userid = user.getidfromOtp(userotp,out string msg);

            if (userid > 0)
            {   //string stored = user.GetOtp(userid);

                int stored = user.GetOtp(userid,out string message);
                //bool otp = user.verifyOTP(userotp,out stored);
                if (userotp == stored)
                {

                    return Json(new { message = "login" });
                }
                else
                {
                    ViewBag.userid = message;
                    return Json(00);
                }
                //return Json(1);}
                



            }else
            {
                ViewBag.msg = msg;
                return Json(2);
            }
        }

            public ActionResult generateotp()
        {
            return View();
        }
        //  public string connectionString = "data source=(LocalDb)\\MSSQLLocalDB;initial catalog=auth;persist security info=True;";
        [HttpPost]
        //public ActionResult generateotp(string MobileNo)
        //{
            
        //    try
        //    {
        //        DataAccess model = new DataAccess();
        //        bool existMobile = model.MobileNumberExists(MobileNo);
        //        if (existMobile) {
        //            string otp = model.GenerateRandomOTP(6);
        //            DateTime expiretime= DateTime.Now.AddMinutes(5);
        //            int userid=model.GetUserIdByMobileNumber(MobileNo);

        //            if(userid > 0)
        //            {
        //                bool otpinserted= model.InsertOtp(userid, otp, expiretime);
        //                if (otpinserted)
        //                {
        //                    return Json(new {success="true",message="otp generated and stored." });
        //                }
        //                else
        //                {
        //                    return Json(new { success = "false", message = "failed" });
        //                }
        //            }
        //            else
        //            {
        //                return Json(new { success = "false", message = "user with mobile not found." });
        //            }
        //        }
        //        else {
        //            return Json(new { success = "false", message = "mobile not found" });
        //                }

        //    }
        //    catch (Exception ex){ 
        //      return Json(new { success = "false",message="error:"+ex.Message}); 
        //    }

        
        
        //}


   //     Replace these values with your Vonage credentials
   //string apiKey = "e3cb936f";
   //     string apiSecret = "CYknodsz77JxHXx8";
   //     string vonagePhoneNumber = "918237382320";


        public ActionResult SendOtpViaSms()
    {
            DataAccess userModel = new DataAccess();  
        var otp = userModel.GenerateRandomOTP(6);
        string userPhoneNumber = "918401095050"; 


            //var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);
            //var client = new VonageClient(credentials);
            var credentials = Credentials.FromApiKeyAndSecret(
    "e3cb936f",
    "CYknodsz77JxHXx8"
    );

            var VonageClient = new VonageClient(credentials);

            var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
            {
                To = "918237382320",
                From = "Vonage APIs",
                Text = $"A text message sent using the Vonage SMS API:{otp}"
            });
            Session["UserOTP"] = otp;

        return View();
    }

       
        public ActionResult verify()
        {
            return View();
        }
        [HttpPost]
        public ActionResult verify(string userotp)
        {
            DataAccess dataAccess = new DataAccess();
           
                if (userotp == Session["otp"].ToString())
                {
                    return RedirectToAction("About", "Home");
                }
                else
                {
                    return Json(0);
                }
           
            
            
         
        }

        public ActionResult sendotp()
        {
            return View();
        }
        [HttpPost]
    public ActionResult sendotp(string Mobileno)
        {
            DataAccess dataAccess = new DataAccess();
            bool ismobile=dataAccess.MobileNumberExists(Mobileno);
          if (!ismobile)
            {
                return Json(new { success = false, message = "Invalid phone number format." });
            }
            Random rand = new Random();
            int value=rand.Next(100001,999999);
            //string address = "+918237382320";
            string otp = $"Your otp is:{value}";

            
            int userid=dataAccess.GetUserIdByMobileNumber(Mobileno);
            DateTime expiretime = DateTime.Now.AddMinutes(5);
            dataAccess.InsertOtp(userid,value,expiretime);

            using (var wb=new WebClient())
            { 
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                    {"apikey","NTA2ODRhNGU0ZDUyNmY0MzQ2NmQ1YTczNjQ1YTM2N2E=" },
                    {"numbers","91"+Mobileno },
                    {"sender","TXTLCL" }
                });
                string result=System.Text.Encoding.UTF8.GetString(response);
                Session["otp"] = value;
                
            }
            return RedirectToAction("verify","User");
            return Json(new { success = true, message = "OTP sent successfully." });
           // return View();
        }
    //private string accountsid = "AC12b92edc7fc54296db745d34dc4288df";
    //string accounttoken = "e69dd6eafe6a5a81bf973f88b8ffbc87";
    //string phonenumber = "+918237382320";
    //public ActionResult SendOtpViaSms()
    //{
    //    UserModel userModel = new UserModel();  

    //    var otp = userModel.GenerateRandomOTP(6); // Implement this method to generate a random OTP
    //    string userPhoneNumber ="+918830761630"; // Get the user's phone number from your database

    //    TwilioClient.Init(accountsid, accounttoken);

    //    var message = MessageResource.Create(
    //        body: $"Your OTP is: {otp}",
    //        from: new PhoneNumber(phonenumber),
    //        to: new PhoneNumber(userPhoneNumber)
    //    );

    //    // Store the OTP on the server temporarily, associated with the user's session or request context
    //    Session["UserOTP"] = otp;

    //    return View();
    //}
}
    

    
}
