using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using temalab.Services;

namespace temalab.Controllers
{
    public class HomeController : Controller
    {
        ItemService itemService = new ItemService();
        CategoryService categoryService = new CategoryService();

        public ActionResult Index()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            return View(itemService.getAll());
        }

        public ActionResult About()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.categories = categoryService.getAll().ToList();

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}