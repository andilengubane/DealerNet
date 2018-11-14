using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using System.Diagnostics;
using DataAccessLayer;
using PublicWorks.Models;


namespace PublicWorks.Controllers
{
    public class AccountController : Controller
    {
		DealerNetEntities context = new DealerNetEntities();
       
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public ActionResult GetUserAccess(UserModel model)
		{
			if (model.UserName != null)
			{
				var data = context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);
				if (data != null)
				{
					this.Session["ID"] = data.ID;
					this.Session["UserName"] = data.UserName;
					this.Session["FirstName"] = data.FirstName;
					this.Session["LastName"] = data.LastName;
					ViewBag.Details = this.Session["FirstName"] + " " + this.Session["LastName"];
					return RedirectToAction("Index", "Orders");
				}
				else
				{
					ViewBag.ErroMessage = "Your logged in details are incorrect.";
				}
			}
			{
				ViewBag.ErroMessage = "Please provide correct login detaials.";
			}
			return View("Login");
		}
		public ActionResult RecoverPassword()
		{
			return View();
		}
		public ActionResult Register()
		{
			return View();
		}
		public ActionResult AddNewUser(UserModel model)
		{
			
			if (ModelState.IsValid)
			{
					context.Users.Add(new User
					{
						UserName = model.UserName,
						Password = model.Password,
						LastName = model.LastName,
						FirstName = model.FirstName,
						EmailAddress = model.EmailAddress,
						DateLogged = DateTime.Now,
						ContactNumber = model.ContactNumber,
						IsActive = true
					});
				context.SaveChanges();
				return this.RedirectToAction("Index", "DashBord");
			}
			return this.View("Register", model);
		}
		public ActionResult ForgotPassword(UserModel model)
		{
			if (model.EmailAddress != null)
			{
				try
				{
					var result = from user in context.Users
								 where user.EmailAddress == model.EmailAddress
								 select user;
					foreach (var item in result)
					{
						var password = item.Password;
						var emailAddres = item.EmailAddress;
						this.sendMail(password, emailAddres);
					}
				}
				catch (Exception ex)
				{
					string error = ex.Message;
				}
				ViewBag.ErrorMessage = "Please check your email Address";
				return this.View("Login");
			}
			return this.View("RecoverPassword", model);
		}
        public void sendMail(string password, string emailAddress)
        {
        	MailMessage msg = new MailMessage();
        	msg.From = new MailAddress("joe@contoso.com");
        	msg.To.Add(new MailAddress(emailAddress));
        	msg.Subject = "Recover Password";
        	msg.Body = " Please make sure you don't you keep your password" + password;
        
        	SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        	System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("joe@contoso.com", "XXXXXX");
        	smtpClient.Credentials = credentials;
        	smtpClient.EnableSsl = true;
        	smtpClient.Send(msg);
        }

	}
}