using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DealerNet.Models
{
	public class OrderModel
	{
		public int OrderNumber { get; set; }
		public int CustomerID { get; set; }
		[Required(ErrorMessage = "Order Date is a required")]
		[Display(Name = "Order Date")]
		public System.DateTime OrderDate { get; set; }
		[Required(ErrorMessage = "Status is a required")]
		public string Status { get; set; }
		[Required(ErrorMessage = "Description is a required")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Item Category is a required")]
		[Display(Name = "Item Category")]
		public int ItemCategory { get; set; }
		[Required(ErrorMessage = "Quantity is a required")]
		public int Quantity { get; set; }
		[Required(ErrorMessage = "Total is a required")]
		public decimal Total { get; set; }
		public System.DateTime Datelogged { get; set; }
		public IEnumerable<SelectListItem> Product { get; set; }
		public IEnumerable<SelectListItem> StatusList { get; set; }
	}
}