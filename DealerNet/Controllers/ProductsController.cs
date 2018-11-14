using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;

namespace DealerNet.Controllers
{
    public class ProductsController : Controller
    {
        private DealerNetEntities context = new DealerNetEntities();

        public ActionResult Index()
        {
            return View(context.Products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
			if (product != null)
			{
				context.Products.Add(new DataAccessLayer.Product
				{
					Name = product.Name,
					IsActive = product.IsActive.HasValue,
					DateLooged = DateTime.Now
				});
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			return PartialView(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,DateLooged,IsActive")] Product product)
        {
			if (product.ID != 0)
			{
				var data = context.Products.FirstOrDefault(m => m.ID == product.ID);

				data.Name = product.Name;
				data.IsActive = product.IsActive.HasValue;

				context.Entry(data).State = EntityState.Modified;
				context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			Product product = context.Products.Find(id);
			context.Products.Remove(product);
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
