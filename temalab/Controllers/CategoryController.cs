using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using temalab.Models;
using temalab.Services;

namespace temalab.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        CategoryService categoryService = new CategoryService();

        // GET: Categories
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            return View(categoryService.getAll());
        }


        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryService.add(category);
                
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            try
            {
                ViewBag.categories = categoryService.getAll().ToList();

                Category category = categoryService.getOne(id);

                return View(category);
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

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryService.edit(category);
                
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.categories = categoryService.getAll().ToList();

                Category category = categoryService.getOne(id);

                return View(category);
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

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Category category = categoryService.getOne(id);
            categoryService.delete(id);
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                categoryService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
