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
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" , ErrorMessage ="Please enter a valid email Address")]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		[Required(ErrorMessage ="Username is a required")]
		[Display(Name = "Username")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Password is a required")]
		[Display(Name = "Password")]
		[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{8,15}" , ErrorMessage ="Please your password should contain a upper and lower case , digit also special characters.") ]
		public string Password { get; set; }
		[RegularExpression(@"(?<!\d)\d{10}(?!\d)", ErrorMessage = "Please enter a valid phone number")]
		[Display(Name = "Cellphone Number")]
		public string ContactNumber { get; set; }
		public bool IsActive { get; set; }
	}
}