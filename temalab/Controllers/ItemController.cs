using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using temalab.Models;
using temalab.Services;

namespace temalab.Controllers
{
    public class ItemController : Controller
    {
        private ItemService itemService = new ItemService();
        private CategoryService categoryService = new CategoryService();

        // GET: Item
        public ActionResult Index()
        {
            ViewBag.categories = categoryService.getAll().ToList();
            var items = itemService.getAll();
            return View(items);
        }

        public ActionResult List(int? id) //Category ID
        {
            try
            {
                ViewBag.categories = categoryService.getAll().ToList();
                var items = itemService.getbyCategory(id).ToList();

                return View(items);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (ex is HttpNotFoundException)
                    return HttpNotFound();
                throw ex;
            }
        }
        // GET: Item/Search?CategoryID={id}&SearchString={searchString}
        public ActionResult Search(int? categoryID, string searchString)
        {
            ViewBag.categories = categoryService.getAll().ToList();
            ViewBag.CategoryID = new SelectList(categoryService.getAll(), "CategoryID", "Name");

            var items = itemService.Select(categoryID, searchString);

            return View(items.ToList());

        }


        // GET: Item/Details/{id}
        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.categories = categoryService.getAll().ToList();

                var item = itemService.getOne(id);


                var Owner = item.Owner.UserName;
                if (!User.Identity.IsAuthenticated)
                {
                    ViewBag.User = "Visitor";
                }
                else if (User.Identity.IsAuthenticated && User.Identity.Name == Owner)
                {
                    ViewBag.User = "Owner";
                }
                else
                {
                    ViewBag.User = "Buyer";
                }


                return View(item);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (ex is HttpNotFoundException)
                    return HttpNotFound();
                throw ex;
            }
        }

        [Authorize]
        public ActionResult Sold()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            var items = itemService.getbyOwner(User.Identity.Name);

            return View(items);
        }
        [Authorize]
        public ActionResult Bought()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            var items = itemService.getbyBuyer(User.Identity.Name);

            return View(items);
        }

        // GET: Item/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(categoryService.getAll(), "CategoryID", "Name");
            ViewBag.categories = categoryService.getAll().ToList();

            return View();
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,BiggestTip,End,CategoryID")] Item item)
        {
            if (ModelState.IsValid)
            {
                itemService.add(item, User.Identity.Name);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(categoryService.getAll(), "CategoryID", "Name", item.CategoryID);
            ViewBag.categories = categoryService.getAll().ToList();

            return View(item);
        }

        // GET: Item/Edit/{id}
        [Authorize]
        public ActionResult Edit(int? id)
        {
            try
            {
                var item = itemService.getOne(id, User.Identity.Name);

                ViewBag.CategoryID = new SelectList(categoryService.getAll(), "CategoryID", "Name", item.CategoryID);
                ViewBag.categories = categoryService.getAll().ToList();

                return View(item);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (ex is ForbiddenException)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                if (ex is HttpNotFoundException)
                    return HttpNotFound();
                throw ex;
            }
        }

        // POST: Item/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,BiggestTip,End,CategoryID")] Item item)
        {
            if (ModelState.IsValid)
            {

                itemService.Edit(item);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(categoryService.getAll(), "CategoryID", "Name", item.CategoryID);
            ViewBag.categories = categoryService.getAll().ToList();

            return View(item);
        }

        // GET: Item/Delete/{id}
        [Authorize]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.categories = categoryService.getAll().ToList();

                Item item = itemService.getOne(id, User.Identity.Name);
                return View(item);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (ex is ForbiddenException)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                if (ex is HttpNotFoundException)
                    return HttpNotFound();
                throw ex;
            }
        }

        // POST: Item/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            itemService.remove(id);
            return RedirectToAction("Index");
        }


        //GET: Item/Upload/{id}
        public ActionResult Upload(int? id)
        {
            try
            {
                ViewBag.categories = categoryService.getAll().ToList();

                return View(itemService.getOne(id, User.Identity.Name));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (ex is ForbiddenException)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                if (ex is HttpNotFoundException)
                    return HttpNotFound();
                throw ex;
            }
        }

        //TODO!!!!!!
        [HttpPost]
        public ActionResult Upload([Bind(Include = "ID,Name,Description,BiggestTip,End,CategoryID")] Item item)
        {
            if (Request.Files.Count > 0)
            {
                
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);

                    itemService.uploadPic(item, "~/Images/" + fileName);
                }
                return RedirectToAction("Index");
            }
            ViewBag.categories = categoryService.getAll().ToList();

            return View();
        }
        //{
        //      if (Request.Files.Count > 0)
        //      {
        //          var file = Request.Files[0];

        //          if (file != null && file.ContentLength > 0)
        //          {
        //              var fileName = Path.GetFileName(file.FileName);
        //              var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
        //              file.SaveAs(path);

        //              Picture pic = new Picture();
        //              pic.Path = path;
        //              pic.Item = item;
        ////              db.Pictures.Add(pic);
        // //             db.SaveChanges();
        //          }
        //      }

        //      return RedirectToAction("Index");
        //  }


        [Authorize]
        public ActionResult Bid(int? id)
        {

            ViewBag.categories = categoryService.getAll().ToList();

            Item item = itemService.getOne(id);
            if (item.Owner.UserName == User.Identity.Name)
                return RedirectToAction("Edit");
            return View(item);

        }

        [HttpPost]
        public ActionResult Bid(Item item)
        {
            string currentUserName = User.Identity.Name;
            if (itemService.bid(item, currentUserName))
                return RedirectToAction("Index");
            return View(item);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                itemService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
