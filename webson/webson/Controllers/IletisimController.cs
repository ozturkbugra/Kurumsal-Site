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
    public class IletisimController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Iletisim
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View(db.Iletisims.ToList());
        }

        // GET: Iletisim/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iletisim iletisim = db.Iletisims.Find(id);
            if (iletisim == null)
            {
                return HttpNotFound();
            }
            return View(iletisim);
        }

        // GET: Iletisim/Create
        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View();
        }

        // POST: Iletisim/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IletisimId,Adres,Tel,Fax,Whatsapp,Facebook,Twitter,Instagram")] Iletisim iletisim)
        {
            if (ModelState.IsValid)
            {
                db.Iletisims.Add(iletisim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iletisim);
        }

        // GET: Iletisim/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iletisim iletisim = db.Iletisims.Find(id);
            if (iletisim == null)
            {
                return HttpNotFound();
            }
            return View(iletisim);
        }

        // POST: Iletisim/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IletisimId,Adres,Tel,Fax,Whatsapp,Facebook,Twitter,Instagram")] Iletisim iletisim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iletisim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iletisim);
        }

        // GET: Iletisim/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iletisim iletisim = db.Iletisims.Find(id);
            if (iletisim == null)
            {
                return HttpNotFound();
            }
            return View(iletisim);
        }

        // POST: Iletisim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Iletisim iletisim = db.Iletisims.Find(id);
            db.Iletisims.Remove(iletisim);
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
