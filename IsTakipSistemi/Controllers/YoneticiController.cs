using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsTakipSistemi.Models;

namespace IsTakipSistemi.Controllers
{
    public class YoneticiController : Controller
    {
        IsTakipDbEntities entity = new IsTakipDbEntities();
        // GET: Yonetici
        public ActionResult Index()
        {
            int yetkiTurId = Convert.ToInt32(Session["PersonelYetkiTurId"]);
            if (yetkiTurId == 1)
            {
                int birimId = Convert.ToInt32(Session["PersonelBirimId"]);
                var birim = (from b in entity.Birimler where b.birimId == birimId select b).FirstOrDefault();
                ViewBag.birimAd = birim.birimAd;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult Ata()
        {
            int yetkiTurId = Convert.ToInt32(Session["PersonelYetkiTurId"]);
            if (yetkiTurId == 1)
            {
                int birimId = Convert.ToInt32(Session["PersonelBirimId"]);
                var calisanlar = (from p in entity.Personeller where p.personelBirimId == birimId && p.personelYetkiTurId == 2 select p).ToList();
                ViewBag.personeller = calisanlar;
                var birim = (from b in entity.Birimler where b.birimId == birimId select b).FirstOrDefault();
                ViewBag.birimAd = birim.birimAd;

                return View();
            }
            else
            { 
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpPost]
        public ActionResult Ata(string isBaslik, string isAciklama, int selectPer)
        {
            Isler yeniIs = new Isler();
            yeniIs.isBaslik = isBaslik;
            yeniIs.isAciklama = isAciklama;
            yeniIs.isPersonelId = selectPer;
            yeniIs.iletilenTarih = DateTime.Now;
            yeniIs.isDurumId = 1;

            entity.Isler.Add(yeniIs);
            entity.SaveChanges();

            return RedirectToAction("Takip","Yonetici");          
        } 
        public ActionResult Takip()
        {
            int yetkiTurId = Convert.ToInt32(Session["PersonelYetkiTurId"]);
            if (yetkiTurId == 1)
            {
                int birimId = Convert.ToInt32(Session["PersonelBirimId"]);
                var calisanlar = (from p in entity.Personeller where p.personelBirimId == birimId && p.personelYetkiTurId == 2 select p).ToList();
                ViewBag.personeller = calisanlar;
                var birim = (from b in entity.Birimler where b.birimId == birimId select b).FirstOrDefault();
                ViewBag.birimAd = birim.birimAd;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult Takip(int selectPer)
        {
            var secilenPersonel = (from p in entity.Personeller where p.personelid == selectPer select p).FirstOrDefault();
            TempData["secilen"]=secilenPersonel;

            return RedirectToAction("Listele", "Yonetici");
        }
        public ActionResult Listele()
        {
            int yetkiTurId = Convert.ToInt32(Session["PersonelYetkiTurId"]);
            if (yetkiTurId == 1)
            {
                Personeller secilenPersonel = (Personeller)TempData["secilen"];

                try
                {
                    var isler = (from i in entity.Isler where i.isPersonelId == secilenPersonel.personelid select i).ToList().OrderByDescending(i=>i.iletilenTarih);

                    ViewBag.isler = isler;
                    ViewBag.personel = secilenPersonel;

                    return View();
                }
                catch (Exception ex) 
                {
                    return RedirectToAction("Takip", "Yonetici");        
                }
                
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
    }
}