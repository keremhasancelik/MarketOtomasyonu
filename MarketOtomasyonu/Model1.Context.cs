﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MarketOtomasyonuDbEntities : DbContext
    {
        public MarketOtomasyonuDbEntities()
            : base("name=MarketOtomasyonuDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Urun> Urun { get; set; }
        public virtual DbSet<Terazi> Terazi { get; set; }
        public virtual DbSet<HizliUrun> HizliUrun { get; set; }
        public virtual DbSet<Islem3> Islem3 { get; set; }
        public virtual DbSet<Satiss> Satiss { get; set; }
        public virtual DbSet<ozet> ozet { get; set; }
    }
}