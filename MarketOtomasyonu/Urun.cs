//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MarketOtomasyonu
{
    using System;
    using System.Collections.Generic;
    
    public partial class Urun
    {
        public int UrunID { get; set; }
        public string UrunBarkod { get; set; }
        public string UrunAd { get; set; }
        public string UrunAciklama { get; set; }
        public string UrunGrup { get; set; }
        public Nullable<double> AlisFiyati { get; set; }
        public Nullable<double> SatisFiyati { get; set; }
        public Nullable<int> KdvOrani { get; set; }
        public Nullable<double> KdvTutari { get; set; }
        public string UrunBirim { get; set; }
        public Nullable<double> Miktar { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string Kullanici { get; set; }
    }
}