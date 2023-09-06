using System;
using System.Collections.Generic;
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
    public class HizmetController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Hizmet
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View(db.Hizmets.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null) // buradan logonun dolu olup olmadığını kontrol ediyoruz
                {
                   
                    WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma
                    img.Resize(500, 500);  // resim boyutu
                    img.Save("~/Uploads/Hizmet/" + logoname);
                    hizmet.ResimURL = "/Uploads/Hizmet/" + logoname;


                }
                db.Hizmets.Add(hizmet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hizmet);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                ViewBag.Uyari = "Güncellenecek hizmet bulunamadı";
            }

            var hizmet = db.Hizmets.Find(id);

            if (hizmet == null)
            {
                return HttpNotFound();
            }
            return View(hizmet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var h = db.Hizmets.Where(x => x.HizmetId == id).SingleOrDefault();
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
                    img.Save("~/Uploads/Hizmet/" + logoname);
                    h.ResimURL = "/Uploads/Hizmet/" + logoname;
                }
                h.Baslik = hizmet.Baslik;
                h.Aciklama = hizmet.Aciklama;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();

            }
            var h = db.Hizmets.Find(id);
            if (h == null)
            {
                return HttpNotFound();
            }
            db.Hizmets.Remove(h); 
            db.SaveChanges();
            System.IO.File.Delete(Server.MapPath(h.ResimURL));
            return RedirectToAction("Index");

           
        }
    }
}