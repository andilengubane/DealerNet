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
			var userID = Session["ID"].ToString();
			var orders = context.Orders.Include(o => o.User).Include(o => o.Product);//.Where(o=> o.CustomerID == Convert.ToInt32(userID));
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
            return View();
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
            return PartialView(order);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderNumber,CustomerID,OrderDate,Status,Description,ItemCategory,Quantity,Total,Datelogged")] Order order)
        {
            if (ModelState.IsValid)
            {
				context.Entry(order).State = EntityState.Modified;
				context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(context.Users, "ID", "FirstName", order.CustomerID);
            ViewBag.ItemCategory = new SelectList(context.Products.Where(x => x.IsActive == true), "ID", "Name", order.ItemCategory);
            return PartialView(order);
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
            return View(order);
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
