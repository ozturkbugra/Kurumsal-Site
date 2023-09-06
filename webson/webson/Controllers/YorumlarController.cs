using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webson.Models;

namespace webson.Controllers
{
    public class YorumlarController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Yorumlar
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            var yorumlars = db.Yorumlars.Include(y => y.Blog);
            return View(yorumlars.ToList());
        }

        // GET: Yorumlar/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorumlar yorumlar = db.Yorumlars.Find(id);
            if (yorumlar == null)
            {
                return HttpNotFound();
            }
            return View(yorumlar);
        }

        // GET: Yorumlar/Create
        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Baslik");
            return View();
        }

        // POST: Yorumlar/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YorumId,AdSoyad,Eposta,Icerik,BlogId,Onay")] Yorumlar yorumlar)
        {
            if (ModelState.IsValid)
            {
                db.Yorumlars.Add(yorumlar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Baslik", yorumlar.BlogId);
            return View(yorumlar);
        }

        // GET: Yorumlar/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorumlar yorumlar = db.Yorumlars.Find(id);
            if (yorumlar == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Baslik", yorumlar.BlogId);
            return View(yorumlar);
        }

        // POST: Yorumlar/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YorumId,AdSoyad,Eposta,Icerik,BlogId,Onay")] Yorumlar yorumlar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yorumlar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Baslik", yorumlar.BlogId);
            return View(yorumlar);
        }

        // GET: Yorumlar/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorumlar yorumlar = db.Yorumlars.Find(id);
            if (yorumlar == null)
            {
                return HttpNotFound();
            }
            return View(yorumlar);
        }

        // POST: Yorumlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yorumlar yorumlar = db.Yorumlars.Find(id);
            db.Yorumlars.Remove(yorumlar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
