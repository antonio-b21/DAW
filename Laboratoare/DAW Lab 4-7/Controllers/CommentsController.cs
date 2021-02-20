using DAW_Lab_4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_Lab_4.Controllers
{
    public class CommentsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
            
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
        public ActionResult Delete(int id)
        {
            var comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            TempData["message"] = "Comentariul a fost sters cu succes!";
            return Redirect("/Articles/Show/" + comment.ArticleId);
        }
    }
}