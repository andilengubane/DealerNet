using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using DealerNet.Models;

namespace DealerNet.Controllers
{
    public class OrdersController : Controller
    {
        private DealerNetEntities context = new DealerNetEntities();
    
        public ActionResult Index()
        {
			var orders = context.Orders.Include(o => o.User).Include(o => o.Product);
			return View(orders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = context.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return PartialView(order);
        }

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(context.Users, "ID", "FirstName");
            ViewBag.ItemCategory = new SelectList(context.Products.Where(x => x.IsActive == true), "ID", "Name");
			var model = new OrderModel()
			{
				StatusList = context.DataLooKUps.Select(c => new SelectListItem
				{
					Value = c.Name,
					Text = c.Name
				})
			};
			return View(model);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderModel orderModel)
        {
			if (ModelState.IsValid)
			{
				var userID = Session["ID"].ToString();
				if (userID != null)
				{
					var totalAmount = orderModel.Total * orderModel.Quantity;
					context.Orders.Add(new DataAccessLayer.Order
					{
						CustomerID = Convert.ToInt32(userID),
						OrderDate = orderModel.OrderDate,
						Status = orderModel.Status,
						Description = orderModel.Description,
						ItemCategory = orderModel.ItemCategory,
						Datelogged = DateTime.Now,
						Quantity = orderModel.Quantity,
						Total = totalAmount
					});
					context.SaveChanges();
					return RedirectToAction("Index");
				}
			}
		    ViewBag.CustomerID = new SelectList(context.Users, "ID", "FirstName", orderModel.CustomerID);
            ViewBag.ItemCategory = new SelectList(context.Products.Where(x => x.IsActive == true), "ID", "Name", orderModel.ItemCategory);
            return View(orderModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = context.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(context.Users, "ID", "FirstName", order.CustomerID);
            ViewBag.ItemCategory = new SelectList(context.Products.Where(x => x.IsActive == true), "ID", "Name", order.ItemCategory);
			ViewBag.Status = new SelectList(context.DataLooKUps, "Name", "Name", order.Status);
			return PartialView(order);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
			var userID = Session["ID"].ToString();
			var totalAmount = order.Total * order.Quantity;
			if (order.OrderNumber != 0) {
				var data = context.Orders.FirstOrDefault(m => m.OrderNumber == order.OrderNumber);

				data.CustomerID = Convert.ToInt32(userID);
				data.Description = order.Description;
				data.ItemCategory = order.ItemCategory;
				data.Status = order.Status;
				data.Quantity = order.Quantity;
				data.Total = totalAmount;

				context.Entry(data).State = EntityState.Modified;
				context.SaveChanges();
			}
			ViewBag.CustomerID = new SelectList(context.Users, "ID", "FirstName", order.CustomerID);
            ViewBag.ItemCategory = new SelectList(context.Products.Where(x => x.IsActive == true), "ID", "Name", order.ItemCategory);
			ViewBag.Status = new SelectList(context.DataLooKUps, "Name", "Name", order.Status);
			return RedirectToAction("Index");
		}

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = context.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return PartialView(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = context.Orders.Find(id);
			context.Orders.Remove(order);
			context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
