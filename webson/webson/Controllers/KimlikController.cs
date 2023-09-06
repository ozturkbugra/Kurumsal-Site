using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using webson.Models;

namespace webson.Controllers
{
    public class KimlikController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Kimlik
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View(db.Kimliks.ToList());
        }

       
        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            var kimlik = db.Kimliks.Where(x => x.KimlikId == id).SingleOrDefault();
            return View(kimlik);
        }

        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Kimlik kimlik, HttpPostedFileBase LogoURL)
        {
            if (ModelState.IsValid)
            {
                var k = db.Kimliks.Where(x => x.KimlikId == id).SingleOrDefault(); //buradan değer geldi mi diye kontrol ediyoruz
                if (LogoURL!= null) // buradan logonun dolu olup olmadığını kontrol ediyoruz
                {
                    if (System.IO.File.Exists(Server.MapPath(k.LogoURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
                    {
                        System.IO.File.Delete(Server.MapPath(k.LogoURL));
                    }

                    WebImage img = new WebImage(LogoURL.InputStream); //bu ikisi resim ekleme
                    FileInfo imginfo = new FileInfo(LogoURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma
                    img.Resize(300, 300);  // resim boyutu
                    img.Save("~/Uploads/Kimlik/" + logoname);
                    k.LogoURL = "/Uploads/Kimlik/" + logoname;


                }
                k.Title = kimlik.Title;
                k.Keywords = kimlik.Keywords;
                k.Description = kimlik.Description;
                k.Unvan = kimlik.Unvan;

                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(kimlik);
        }

       
    }
}
