using DAW_Lab_4.Models;
using DAW_Lab_8.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Security.Application;
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
        private ApplicationDbContext db = new ApplicationDbContext();

        private int _perPage = 3;


        // GET: Articles
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            var articles = db.Articles.Include("Category").Include("User").OrderBy(a => a.Date);
            var totalItems = articles.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedArticles = articles.Skip(offset).Take(this._perPage);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Articles = paginatedArticles;
            return View();
        }


        // GET: New
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New()
        {
            Article article = new Article();
            article.Categ = GetAllCategories();
            article.UserId = User.Identity.GetUserId();
            return View(article);
        }

        // POST: New
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New(Article article)
        {
            article.Date = DateTime.Now;
            article.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    article.Content = Sanitizer.GetSafeHtmlFragment(article.Content);

                    db.Articles.Add(article);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat cu succes!";
                    return RedirectToAction("Index");
                }
                article.Categ = GetAllCategories();
                return View(article);
            }
            catch (Exception)
            {
                article.Categ = GetAllCategories();
                return View(article);
            }
        }


        // GET: Show
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            var article = db.Articles.Find(id);

            SetAccesRights();

            return View(article);
        }

        // POST: Show
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(Comment comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost adaugat cu succes!";
                    return Redirect("/Articles/Show/" + comment.ArticleId);
                }
                Article article = db.Articles.Find(comment.ArticleId);
                SetAccesRights();
                return View(article);
            }
            catch (Exception)
            {
                Article article = db.Articles.Find(comment.ArticleId);
                SetAccesRights();
                return View(article);
                //return Redirect("/Articles/Show/" + comment.ArticleId);
            }
        }



        // GET: Edit
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Article article = db.Articles.Find(id);
            article.Categ = GetAllCategories();

            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(article);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati acest articol!";
                return RedirectToAction("Index");
            }
        }

        // PUT: Edit
        [HttpPut]
        [ValidateInput(false)]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id, Article requestArticle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Article article = db.Articles.Find(id);
                    if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(article))
                        {
                            requestArticle.Content = Sanitizer.GetSafeHtmlFragment(requestArticle.Content);

                            article.Title = requestArticle.Title;
                            article.Content = requestArticle.Content;
                            article.CategoryId = requestArticle.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Articolul a fost editat!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa editati acest articol!";
                        return RedirectToAction("Index");
                    }

                }
                requestArticle.Categ = GetAllCategories();
                return View(requestArticle);
            }
            catch (Exception)
            {
                requestArticle.Categ = GetAllCategories();
                return View(requestArticle);
            }
        }


        // DELETE: Delete
        [HttpDelete]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Articles.Remove(article);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters cu succes!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti acest articol!";
                return RedirectToAction("Index");
            }
        }

        #region Helpers
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

        private void SetAccesRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }
        #endregion
    }
}
