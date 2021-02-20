using DAW_Lab_4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DAW_Lab_4.Controllers
{
    public class ArticlesController : Controller
    {
        private Models.AppContext db = new Models.AppContext();


        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include("Category");
            ViewBag.Articles = articles;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }


        // GET: New
        public ActionResult New()
        {
            Article article = new Article();
            article.Categ = GetAllCategories();
            return View(article);
        }

        // POST: New
        [HttpPost]
        public ActionResult New(Article article)
        {
            article.Categ = GetAllCategories();
            article.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Articles.Add(article);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat cu succes!";
                    return RedirectToAction("Index");
                }
                return View(article);
            }
            catch (Exception)
            {
                return View(article);
            }
        }


        // GET: Show
        public ActionResult Show(int id)
        {
            var article = db.Articles.Find(id);
            return View(article);
        }


        // GET: Edit
        public ActionResult Edit(int id)
        {
            var article = db.Articles.Find(id);
            article.Categ = GetAllCategories();
            return View(article);
        }

        // PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Article requestArticle)
        {
            requestArticle.Categ = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    var article = db.Articles.Find(id);
                    if (TryUpdateModel(article))
                    {
                        article.Title = requestArticle.Title;
                        article.Content = requestArticle.Content;
                        article.CategoryId = requestArticle.CategoryId;
                        db.SaveChanges();
                        TempData["message"] = "Articolul a fost editat!";
                        return RedirectToAction("Index");
                    }
                }
                return View(requestArticle);
            }
            catch (Exception)
            {
                return View(requestArticle);
            }
        }


        // DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            TempData["message"] = "Articolul a fost sters cu succes!";
            return RedirectToAction("Index");
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }
    }
}
