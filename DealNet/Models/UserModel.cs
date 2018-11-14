using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PublicWorks.Models
{
	public class UserModel
	{
		public int ID { get; set; }
		[Required(ErrorMessage = "First Name is a required")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Last Name is a required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Email Address is a required")]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		[Required(ErrorMessage ="Username is a required")]
		[Display(Name = "Username")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Password is a required")]
		[Display(Name = "Password")]
		public string Password { get; set; }
		public string ContactNumber { get; set; }
		public bool IsActive { get; set; }
	}
}