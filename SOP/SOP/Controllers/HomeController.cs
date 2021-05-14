using PagedList;
using SOP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using SOP.ViewModels;
using System.Data.Entity;

namespace SOP.Controllers
{
    public class HomeController : Controller
    {
        SopSistemiEntities1 db = new SopSistemiEntities1();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(sopUser user)
        {
            try
            {
                using (SopSistemiEntities1 db = new SopSistemiEntities1())
                {
                    //login control
                    var usr = db.sopUser.Where(u => u.kullaniciAdi == user.kullaniciAdi && u.sifre == user.sifre).FirstOrDefault();

                    if (usr.yetkiID == 1)//user is not administrator
                    {
                        Session["UyeId"] = usr.user_id.ToString();
                        Session["KullaniciAdi"] = usr.kullaniciAdi.ToString();
                        return RedirectToAction("kullaniciAnasayfa");
                    }
                    else if (usr.yetkiID == 2)//user is administrator
                    {
                        Session["UyeId"] = usr.user_id.ToString();
                        Session["KullaniciAdi"] = usr.kullaniciAdi.ToString();
                        return RedirectToAction("yoneticiAnasayfa");
                    }
                    else//incorrect user login
                    {
                        TempData["uyarıMesaj"] = "Kullanıcı Adı veya Şifre Hatalı!";
                    }
                }
                return View();
            }
            catch
            {
                TempData["uyarıMesaj"] = "Kullanıcı Adı veya Şifre Hatalı!";
                return View();
            }
        }


        public ActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KayitOl(sopUser model)//user registration
        {
            if (ModelState.IsValid)
            {
                sopUser kayit = new sopUser();
                var deneme = db.sopUser.Where(a => model.kullaniciAdi == a.kullaniciAdi);
                if (deneme.Count() <= 0)
                {
                    kayit.kullaniciAdi = model.kullaniciAdi;
                    kayit.sifre = model.sifre;
                    kayit.tcKimlik = model.tcKimlik;
                    kayit.adSoyad = model.adSoyad;
                    kayit.sicilNo = model.sicilNo;
                    kayit.unvan = model.unvan;
                    kayit.email = model.email;
                    kayit.yetkiID = 1;
                    db.sopUser.Add(kayit);
                    db.SaveChanges();
                    TempData["KullaniciMesaji2"] = "Başarılı bir şekilde kaydedilmiştir.";
                }
                else
                {
                    TempData["KullaniciMesaji"] = "Kullanıcı adı önceden alınmış!";
                }
            }
            return View();
        }
        //admin homepage
        public ActionResult yoneticiAnasayfa(int Page = 1)
        {
            var sop = db.sopOnay.Where(a => a.durum == false).OrderByDescending(m => m.sopID).ToPagedList(Page, 10);
            return View(sop);
        }
        //admin display page
        public ActionResult yoneticiGoruntuleme(int? id)
        {
            Listeleme sdmodel = new Listeleme();

            sdmodel.sopKayit = db.sopKayit.Where(m => m.sopID == id).SingleOrDefault();
            sdmodel.sopOnay = db.sopOnay.Where(m => m.sopID == id).ToList();
            ViewBag.ID = id;
            if (sdmodel.sopKayit == null)
            {
                return HttpNotFound();
            }
            return View(sdmodel);
        }

        public FileResult GetReport(int id)
        {
            var prd = db.sopKayit.Where(m => m.sopID == id).SingleOrDefault();

            string filepath = Server.MapPath(prd.dosya);

            return File(filepath, "application/pdf");
        }

        public ActionResult kullaniciAnasayfa(int Page = 1)
        {
            var id = Convert.ToInt32(Session["UyeID"]);
            var sopKullanici = db.sopOnay.Where(m => m.durum == false).Where(a => a.user_id == id).OrderByDescending(m => m.sopID).ToPagedList(Page, 10);
            return View(sopKullanici);
        }
        public ActionResult kullaniciGoruntuleme(int id)
        {
            Listeleme sdmodel = new Listeleme();

            sdmodel.sopKayit = db.sopKayit.Where(m => m.sopID == id).SingleOrDefault();
            sdmodel.sopOnay = db.sopOnay.Where(m => m.sopID == id).ToList();
            ViewBag.ID = id;
            if (sdmodel.sopKayit == null)
            {
                return HttpNotFound();
            }
            return View(sdmodel);
        }
        [HttpPost]
        public ActionResult kullaniciGoruntuleme(int? id, sopKayit kayit, sopOnay onay)
        {
            var kullaniciID = Convert.ToInt32(Session["UyeID"]);
            var onaylar = db.sopOnay.Where(a => a.user_id == kullaniciID).Where(m => m.sopID == id).SingleOrDefault();
            var kayitlar = db.sopKayit.Where(a => a.user_id == kullaniciID).Where(m => m.sopID == id).SingleOrDefault();
            sopOnay k = new sopOnay();

            onaylar.durum = true;
            DateTime zaman = DateTime.Now;
            onaylar.tarih = zaman;
            onaylar.user_id = kullaniciID;
            onaylar.sopID = id;
            db.Entry(onaylar).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("onaylanmisSOP");
        }
        public ActionResult onaylanmisSOP(int Page = 1)
        {
            var kullanici = Convert.ToInt32(Session["UyeID"]);
            var sopKullanici = db.sopOnay.Where(a => a.user_id == kullanici).Where(m => m.durum == true).OrderByDescending(m => m.sopID).ToPagedList(Page, 10);
            return View(sopKullanici);
        }
        public ActionResult onayGoruntuleme(int id)
        {
            Listeleme sdmodel = new Listeleme();

            sdmodel.sopKayit = db.sopKayit.Where(m => m.sopID == id).SingleOrDefault();
            sdmodel.sopOnay = db.sopOnay.Where(m => m.sopID == id).ToList();
            ViewBag.ID = id;
            if (sdmodel.sopKayit == null)
            {
                return HttpNotFound();
            }
            return View(sdmodel);
        }
        public ActionResult adminOnaylanmis(int Page = 1)
        {
            var sopKullanici = db.sopOnay.Where(m => m.durum == true).OrderByDescending(m => m.sopID).ToPagedList(Page, 10);
            return View(sopKullanici);
        }
        public ActionResult adminGoruntule(int id)
        {
            Listeleme sdmodel = new Listeleme();

            sdmodel.sopKayit = db.sopKayit.Where(m => m.sopID == id).SingleOrDefault();
            sdmodel.sopOnay = db.sopOnay.Where(m => m.sopID == id).ToList();
            ViewBag.ID = id;
            if (sdmodel.sopKayit == null)
            {
                return HttpNotFound();
            }
            return View(sdmodel);
        }

    }
}