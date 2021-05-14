using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SOP.Models;

namespace SOP.Controllers
{
    public class SOPController : Controller
    {

        SopSistemiEntities1 db = new SopSistemiEntities1();

        // GET: SOP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateSOP()
        {
            ViewBag.user_id = new SelectList(db.sopUser, "user_id", "adSoyad");
            return View();
        }

        [HttpPost]
        public ActionResult CreateSOP(HttpPostedFileBase dosya, sopKayit model, string[] user_id)
        {

            if (ModelState.IsValid)
            {

                if (dosya != null)
                {
                    var fileName = Path.GetFileName(dosya.FileName);
                    var path = Path.Combine(Server.MapPath("~/Dosyalar/"), fileName);
                    model.dosya = "/Dosyalar/" + fileName;
                    dosya.SaveAs(path);
                }

                var uid = Convert.ToInt32(Session["UyeId"]);
                DateTime zaman = DateTime.Now;
                model.user_id = uid;
                model.tarih = zaman;
                db.sopKayit.Add(model);
                db.SaveChanges();
                if (user_id != null)
                {
                    foreach (var i in user_id)
                    {
                        var yenietiket = new sopOnay { user_id = Int32.Parse(i), durum = false, sopID = model.sopID };

                        db.sopOnay.Add(yenietiket);
                        db.SaveChanges();

                    }

                    var useremail = db.sopOnay.Where(m => m.sopID == model.sopID).ToList();

                    //Mail sending...
                    String baslik = DateTime.Now.ToString("dd/MM/yyyy") + " LOGO - SOP Onay Kabul";//title
                    var fromAddress = new MailAddress("L@LOGO-tr.com", "LOGO - SOP Kabul Paneli");//company mail address
                    string fromPassword = "123123";
                    foreach (var j in useremail)
                    {
                        var personel = db.sopUser.Where(m => m.user_id == j.user_id).ToList();
                        foreach (var k in personel)
                        {
                            var smtp = new SmtpClient
                            {
                                EnableSsl = true,
                                Host = "smtp.yandex.com", //mail serverının host bilgisi 
                                Port = 587,//mail serverının portu 
                                UseDefaultCredentials = false,
                                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,

                                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                                Timeout = 200000
                            };

                            var toAddress = new MailAddress(k.email);

                            var msg = new MailMessage(fromAddress, toAddress)
                            {
                                Subject = baslik,
                                Body = "Merhaba &nbsp;" + k.adSoyad + "<br /> Adınıza tanımlanan<br/> SOP İsmi: " + j.sopKayit.baslik + "<br/> Link: --SOP Link-- linkinden kullanıcı adı ve şifreniz ile giriş yaparak onaylayınız."
                            };

                            msg.IsBodyHtml = true;

                            smtp.Send(msg);
                        }
                    }
                }
                return RedirectToAction("../Home/yoneticiAnasayfa");
            }
            return View();
        }
    }
}