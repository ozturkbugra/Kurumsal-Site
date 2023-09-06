using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using webson.Models;

namespace webson.Controllers
{
    public class YetkiliController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Yetkili
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View(db.Admins.ToList());
        }

        // GET: Yetkili/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Yetkili/Create
        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View();
        }

        // POST: Yetkili/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin, HttpPostedFileBase ResimURL, string Sifre, string Eposta, string Yetki)
        {
            if (ModelState.IsValid)
            {

                if (ResimURL != null) // buradan logonun dolu olup olmadığını kontrol ediyoruz
                {

                    WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma
                    img.Resize(383, 383);  // resim boyutu
                    img.Save("~/Uploads/Yetki/" + logoname);
                    admin.ResimURL = "/Uploads/Yetki/" + logoname;


                }
                admin.Sifre = Crypto.Hash(Sifre, "MD5");
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Yetkili/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Yetkili/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin, HttpPostedFileBase ResimURL,int? id, string Sifre, string Eposta, string Yetki)
        {
            if (ModelState.IsValid)
            {
                var h = db.Admins.Where(x => x.AdminId == id).SingleOrDefault();
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }


                    WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma
                    img.Resize(500, 500);  // resim boyutu
                    img.Save("~/Uploads/Yetki/" + logoname);
                    h.ResimURL = "/Uploads/Yetki/" + logoname;
                }

                h.Eposta = admin.Eposta;
                h.Sifre = Crypto.Hash(Sifre, "MD5");
                h.Yetki = admin.Yetki;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Yetkili/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Yetkili/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            if (System.IO.File.Exists(Server.MapPath(admin.ResimURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
            {
                System.IO.File.Delete(Server.MapPath(admin.ResimURL));
            }
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
