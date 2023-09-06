using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using webson.Models;
using PagedList.Mvc;
using PagedList;

namespace webson.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();
        public ActionResult Index()
        {

            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Sliders.ToList().OrderByDescending(x => x.SliderId));
        }

        public ActionResult HizmetPartial()
        {
            return View(db.Hizmets.ToList().OrderByDescending(x => x.HizmetId));
        }

        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            return View(db.Hakkimizdas.SingleOrDefault());

        }

        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            return View(db.Hizmets.ToList().OrderByDescending(x=> x.HizmetId));
        }

        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            return View(db.Iletisims.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult Iletisim(string AdSoyad, string Email, string Konu, string Mesaj)
        {
            if (!string.IsNullOrEmpty(AdSoyad) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Konu) && !string.IsNullOrEmpty(Mesaj))
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ztrk5050@gmail.com", "Samsunga31"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ztrk5050@gmail.com"),
                    Subject = Konu,
                    Body = Email + "<br />" + Mesaj,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(new MailAddress("ztrk5050@gmail.com"));

                try
                {
                    smtpClient.Send(mailMessage);
                    ViewBag.uyari = "Mesajınız Başarıyla gönderildi";
                }
                catch (Exception ex)
                {
                    ViewBag.uyari = "Mesajınız gönderilemedi: " + ex.Message;
                }
            }
            else
            {
                ViewBag.uyari = "Lütfen tüm alanları doldurun.";
            }

            return View("Iletisim","Home");
        }

        public ActionResult Blog(int sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            return View(db.Blogs.Include("Kategori").OrderByDescending(x => x.BlogId).ToPagedList(sayfa,5));
        }

        public JsonResult YorumYap(string AdSoyad, string Eposta, string Icerik, int BlogId)
        {
            if (Icerik == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorumlars.Add(new Yorumlar { AdSoyad = AdSoyad, Eposta = Eposta, Icerik = Icerik, BlogId = BlogId, Onay = false});
            db.SaveChanges();
            return Json(false,JsonRequestBehavior.AllowGet);
        }

        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();
            return View(b);
        }
        
        
        public ActionResult KategoriBlog(int id, int Sayfa=1)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            var b = db.Blogs.OrderByDescending(x=> x.KategoriId).Where(x => x.Kategori.KategoriId == id).ToPagedList(Sayfa,5);
            return View(b);
        }

        public ActionResult BlogKategoriPartial()
        {
            return PartialView(db.Kategoris.ToList().OrderBy(x=> x.KategoriAd));
        }

        public ActionResult BlogKayitPartial()
        {
            return PartialView(db.Blogs.OrderByDescending(x => x.BlogId).ToList());
        }

        public ActionResult FooterPartial()
        {
            
            ViewBag.Hizmetler = db.Hizmets.ToList().OrderByDescending(x => x.HizmetId);
            ViewBag.Iletisim = db.Iletisims.SingleOrDefault();
            ViewBag.Blog = db.Blogs.OrderByDescending(x => x.BlogId).Take(5).ToList();
            return PartialView();

        }
    }
}