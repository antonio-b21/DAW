using BiganAntonioM41.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiganAntonioM41.Controllers
{
    public class SubscriptionsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();


        // GET: Index
        public ActionResult Index()
        {
            var subscriptions = db.Subscriptions.Include("Client");
            ViewBag.Subscriptions = subscriptions;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }


        // GET: New
        public ActionResult New()
        {
            Subscription subscription = new Subscription();
            subscription.Cls = GetAllCls();
            return View(subscription);
        }

        // POST: New
        [HttpPost]
        public ActionResult New(Subscription subscription)
        {
            subscription.Cls = GetAllCls();
            var numere = db.Subscriptions.Select(s => s.Numar).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (subscription.DataEmitere < DateTime.Now.Date)
                    {
                        ViewBag.Message = "Data trebuie sa fie una trecuta!";
                        return View(subscription);
                    }
                    if (numere.Contains(subscription.Numar))
                    {
                        ViewBag.Message = "Numarul trebuie sa fie unic!";
                        return View(subscription);
                    }

                    db.Subscriptions.Add(subscription);
                    db.SaveChanges();
                    TempData["message"] = "Abonamentul a fost adaugat cu succes!";
                    return RedirectToAction("Index");
                }
                return View(subscription);
            }
            catch (Exception)
            {
                return View(subscription);
            }
        }


        // GET: Show
        public ActionResult Show(int id)
        {
            var subscription = db.Subscriptions.Find(id);
            return View(subscription);
        }


        // GET: Edit
        public ActionResult Edit(int id)
        {
            var subscription = db.Subscriptions.Find(id);
            subscription.Cls = GetAllCls();
            return View(subscription);
        }

        // PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Subscription requestSubscription)
        {
            requestSubscription.Cls = GetAllCls();
            var numere = db.Subscriptions.Where(s => s.Id != requestSubscription.Id).Select(s => s.Numar).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    var article = db.Subscriptions.Find(id);
                    if (TryUpdateModel(article))
                    {
                        if (requestSubscription.DataEmitere < DateTime.Now.Date)
                        {
                            ViewBag.Message = "Data trebuie sa fie una trecuta!";
                            return View(requestSubscription);
                        }
                        if (numere.Contains(requestSubscription.Numar))
                        {
                            ViewBag.Message = "Numarul trebuie sa fie unic!";
                            return View(requestSubscription);
                        }

                        article.Descriere = requestSubscription.Descriere;
                        article.Numar = requestSubscription.Numar;
                        article.DataEmitere = requestSubscription.DataEmitere;
                        article.ClientId = requestSubscription.ClientId;
                        db.SaveChanges();
                        TempData["message"] = "Abonamentul a fost editat!";
                        return RedirectToAction("Index");
                    }
                }
                return View(requestSubscription);
            }
            catch (Exception)
            {
                return View(requestSubscription);
            }
        }


        // DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var subscription = db.Subscriptions.Find(id);
            db.Subscriptions.Remove(subscription);
            db.SaveChanges();
            TempData["message"] = "Abonamentul a fost sters cu succes!";
            return RedirectToAction("Index");
        }

        //GET: Search
        public ActionResult Search(string token, string sort)
        {
            var subscriptions = db.Subscriptions.Include("Client").ToArray();

            List<Subscription> subscriptionsList = new List<Subscription>();

            if (!(token is null))
            {
                foreach (var subscription in subscriptions)
                {
                    if (subscription.Descriere.Contains(token) || subscription.DataEmitere.ToString("dd/MM/yyyy") == token)
                    {
                        subscriptionsList.Add(subscription);
                    }
                }
            }
            

            subscriptions = subscriptionsList.ToArray();


            switch (sort)
            {
                case "dateAsc":
                    subscriptions = subscriptions.OrderBy(s => s.DataEmitere).ToArray();
                    break;
                default:
                    subscriptions = subscriptions.OrderByDescending(s => s.DataEmitere).ToArray();
                    break;
            }

            ViewBag.Subscriptions = subscriptions;
            ViewBag.Search = token;
            return View();
        }

        #region Helpers
        [NonAction]
        private IEnumerable<SelectListItem> GetAllCls()
        {
            var selectList = new List<SelectListItem>();
            var clients = db.Clients;

            foreach (var client in clients)
            {
                selectList.Add(new SelectListItem
                {
                    Value = client.Id.ToString(),
                    Text = client.Nume.ToString()
                });
            }
            
            return selectList;
        }
        #endregion
    }
}