using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsTakipSistemi.Models;

namespace IsTakipSistemi.Controllers
{
    public class LoginController : Controller
    {
        IsTakipDbEntities entity = new IsTakipDbEntities();
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.mesaj = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string kullaniciAd, string parola)
        {
            var personel =(from p in entity.Personeller where p.personelKullaniciAd==kullaniciAd && p.personelParola == parola select p).FirstOrDefault();
            if (personel != null)
            {
                Session["PersonelAdSoyad"] = personel.personelAdSoyad;
                Session["PersonelId"] = personel.personelid;
                Session["PersonelBirimId"] = personel.personelBirimId;
                Session["PersonelYetkiTurId"] = personel.personelYetkiTurId;

                switch(personel.personelYetkiTurId)
                {
                    case 1:
                        return RedirectToAction("Index", "Yonetici");
                    case 2:
                        return RedirectToAction("Index", "Calisan");
                    default:
                        return View();
                }
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Adı ya da Parola Yanlıs";
                return View();
            }


            
        }
    }
}