using DAW_Lab_4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_Lab_4.Controllers
{
    public class CategoriesController : Controller
    {
        private Models.AppContext db = new Models.AppContext();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = db.Categories;
            ViewBag.Categories = categories;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        //GET: New
        public ActionResult New()
        {
            Category category = new Category();
            return View(category);
        }

        //POST: New
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata cu succes!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(category);
            }
        }

        //GET: Show
        public ActionResult Show(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }

        //GET: Edit
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }

        //PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                var category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost editata cu succes!";
                    return RedirectToAction("Index");
                }
                return View(requestCategory);
            }
            catch (Exception)
            {
                return View(requestCategory);
            }
        }

        //DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa cu succes!";
            return RedirectToAction("Index");
        }
    }
}