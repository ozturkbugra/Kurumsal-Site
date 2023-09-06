using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using webson.Models;

namespace webson.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            ViewBag.HizmetSayisi = db.Hizmets.Count();
            ViewBag.BlogSayisi = db.Blogs.Count();
            ViewBag.OnayliYorumSayisi = db.Yorumlars.Where(x=> x.Onay == true).Count();
            ViewBag.OnaysizYorumSayisi = db.Yorumlars.Where(x => x.Onay == false).Count();

            ViewBag.Listele = db.Yorumlars.Where(x => x.Onay == false).ToList();
            ViewBag.YorumOnay = db.Yorumlars.Where(x => x.Onay == false).Count();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin, string sifre)
        {
            ViewBag.Kimlik = db.Kimliks.SingleOrDefault();
            var s = Crypto.Hash(sifre, "MD5");
            var l = db.Admins.Where(x=> x.Eposta == admin.Eposta).SingleOrDefault();
            if (l.Eposta == admin.Eposta && l.Sifre == s)
            {
                Session["adminid"] = l.AdminId;
                Session["eposta"] = l.Eposta;
                Session["yetki"] = l.Yetki;
                Session["resim"] = l.ResimURL;
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Uyari = "Kullanıcı adı veya şifre yanlış";
            return View(admin);

        }

        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session["yetki"] = null;
            Session.Abandon();
            
            return RedirectToAction("Login","Admin");

        }
    }
}