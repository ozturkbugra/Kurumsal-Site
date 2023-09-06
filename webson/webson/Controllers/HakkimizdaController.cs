using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webson.Models;

namespace webson.Controllers
{
    public class HakkimizdaController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Hakkimizda
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            var h = db.Hakkimizdas.ToList();
            return View(h);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            var h = db.Hakkimizdas.Where(x => x.HakkimizdaId == id).FirstOrDefault();
            return View(h);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Hakkimizda h)
        {
            if (ModelState.IsValid)
            {
                var hakkimizda = db.Hakkimizdas.Where(x => x.HakkimizdaId == id).SingleOrDefault();

                hakkimizda.Aciklama = h.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(h);
        }
    }
}