using DAW_Lab_4.Models;
using DAW_Lab_8.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_Lab_4.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        // POST: New
        [HttpPost]
        public ActionResult New(Comment comment)
        {
            comment.Date = DateTime.Now;
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost adaugat cu succes!";
                return Redirect("/Articles/Show/" + comment.ArticleId);
            }
            catch (Exception)
            {
                return Redirect("/Articles/Show/" + comment.ArticleId);
            }
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            var comment = db.Comments.Find(id);
            return View(comment);
        }

        // PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                var comment = db.Comments.Find(id);
                if (TryUpdateModel(comment))
                {
                    comment.Content = requestComment.Content;
                    TempData["message"] = "Comentariul a fost editat cu succes!";
                    db.SaveChanges();
                    return Redirect("/Articles/Show/" + comment.ArticleId);
                }
                return View(requestComment);
            }
            catch (Exception)
            {
                return View(requestComment);
            }
        }

        // DELETE: Delete
        [HttpDelete]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);

            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost sters cu succes!";
                return Redirect("/Articles/Show/" + comment.ArticleId);
            }
            TempData["message"] = "Nu aveti dreptul sa stergeti acest comentariu!";
            return RedirectToAction("Index", "Articles");
        }
    }
}