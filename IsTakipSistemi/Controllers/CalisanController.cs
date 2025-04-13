using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsTakipSistemi.Models;

namespace IsTakipSistemi.Controllers
{
    
    public class CalisanController : Controller
    {
        IsTakipDbEntities entity = new IsTakipDbEntities();
        // GET: Calisan
        public ActionResult Index()
        {
            int YetkiId = Convert.ToInt32(Session["PersonelYetkiTurId"]);
            if (YetkiId == 2)
            {
                int birimId = Convert.ToInt32(Session["PersonelBirimId"]);
                var birimAdi = (from b in entity.Birimler where b.birimId == birimId select b).FirstOrDefault();
                ViewBag.birimAd = birimAdi.birimAd;
                return View();
            }
            else 
            { 
                return RedirectToAction("Index","Login");
            }
        }
        public ActionResult Yap()
        {
            int YetkiId = Convert.ToInt32(Session["PersonelYetkiTurId"]);

            if (YetkiId == 2)
            {
                int personelId = Convert.ToInt32(Session["PersonelId"]);

                var yapılacakIs=(from i in entity.Isler where i.isPersonelId == personelId && i.isDurumId==1 select i ).ToList().OrderByDescending(i=>i.iletilenTarih);

                ViewBag.isler = yapılacakIs;

                return View();
            }
            else 
            {
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpPost]
        public ActionResult Yap(int isId)
        {
            var tamamlananIs = (from i in entity.Isler where i.isId == isId select i).FirstOrDefault();

            tamamlananIs.isDurumId = 2;
            tamamlananIs.yapilanTarih = DateTime.Now;

            entity.SaveChanges();


            return RedirectToAction("Index", "Calisan");
        }

        public ActionResult Takip()
        {

            int YetkiId = Convert.ToInt32(Session["PersonelYetkiTurId"]);

            if (YetkiId == 2)
            {
                int personelId = Convert.ToInt32(Session["PersonelId"]);

                var isler = (from i in entity.Isler where i.isPersonelId == personelId select i).ToList();

                

                return View(isler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}