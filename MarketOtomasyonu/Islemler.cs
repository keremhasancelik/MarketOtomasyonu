using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOtomasyonu
{
    static class Islemler
    {
        public static double DoubleYap(string deger)
        {
            double sonuc;
            double.TryParse(deger, NumberStyles.Currency, CultureInfo.CurrentUICulture.NumberFormat, out sonuc);
            return Math.Round(sonuc,2);
        }


        public static void StokAzalt(string barkod,double miktar)
        {
            if (barkod != "1111111111116")
            {
                using (var db = new MarketOtomasyonuDbEntities())
                {
                    var urunbilgi = db.Urun.SingleOrDefault(x => x.UrunBarkod == barkod);
                    urunbilgi.Miktar -= miktar;
                    db.SaveChanges();
                }
            }
            
        }

        public static void StokArttir(string barkod, double miktar)
        {
            if (barkod != "1111111111116")
            {
                using (var db = new MarketOtomasyonuDbEntities())
                {
                    var urunbilgi = db.Urun.SingleOrDefault(x => x.UrunBarkod == barkod);
                    urunbilgi.Miktar += miktar;
                    db.SaveChanges();
                }
            }
        }
    }
}
