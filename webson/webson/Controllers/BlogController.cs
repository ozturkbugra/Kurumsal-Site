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
    public class BlogController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Blog
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View(db.Blogs.ToList()) ;

        }

        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAd");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, HttpPostedFileBase ResimURL)
        {
            
            if (ResimURL != null) // buradan logonun dolu olup olmadığını kontrol ediyoruz
            {

                WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme
                FileInfo imginfo = new FileInfo(ResimURL.FileName);

                string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma
                img.Resize(600, 400);  // resim boyutu
                img.Save("~/Uploads/Blog/" + logoname);
                blog.ResimURL = "/Uploads/Blog/" + logoname;


            }
            db.Blogs.Add(blog);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        
        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            if (id == null)
            {
                return HttpNotFound();
            }
            var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();
            if (b == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategoris,"KategoriId","KategoriAd",b.KategoriId);
            return View(b);
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit (int? id, Blog blog, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();
                if (ResimURL != null)
                {
                   if (System.IO.File.Exists(Server.MapPath(b.ResimURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
                    {
                        System.IO.File.Delete(Server.MapPath(b.ResimURL));
                    }


                    WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma
                    img.Resize(500, 500);  // resim boyutu
                    img.Save("~/Uploads/Blog/" + logoname);
                    b.ResimURL = "/Uploads/Blog/" + logoname;
                }
                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.KategoriId = blog.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);

        }
           
        
        public ActionResult Delete(int id)
        {
            var d = db.Blogs.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(d.ResimURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
            {
                System.IO.File.Delete(Server.MapPath(d.ResimURL));
            }
            db.Blogs.Remove(d);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



    }
 }
