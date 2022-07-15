using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketOtomasyonu
{
    public partial class frmSatis : Form
    {
        public frmSatis()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        MarketOtomasyonuDbEntities db = new MarketOtomasyonuDbEntities();
        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barkod = txtBarkod.Text.Trim();

                if (barkod.Length <= 2)
                {
                    txtMiktar.Text = barkod;
                    txtBarkod.Clear();
                    txtBarkod.Focus();
                }
                else
                {
                    if (db.Urun.Any(x => x.UrunBarkod == barkod))
                    {
                        var urun = db.Urun.Where(x => x.UrunBarkod == barkod).FirstOrDefault();
                        UrunGetir(urun, barkod, Convert.ToDouble(txtMiktar.Text));
                    }
                    else
                    {
                        int onek = Convert.ToInt16(barkod.Substring(0, 2));
                        if (db.Terazi.Any(x => x.TeraziOnEk == onek))
                        {
                            string teraziUrunNo = barkod.Substring(2, 5);
                            if (db.Urun.Any(x => x.UrunBarkod == teraziUrunNo))
                            {
                                var urunTerazi = db.Urun.Where(x => x.UrunBarkod == teraziUrunNo).FirstOrDefault();
                                double miktarKg = Convert.ToDouble(barkod.Substring(7, 5)) / 1000;
                                UrunGetir(urunTerazi, barkod, miktarKg);
                            }
                            else
                            {
                                Console.Beep(900, 800);
                                MessageBox.Show("Kg Ürün Ekleme Sayfası");
                            }
                        }
                        else
                        {
                            Console.Beep(900, 800);
                            MessageBox.Show("Normal Ürün Ekleme Sayfası");
                        }
                    }
                }

                dataGridView1.ClearSelection();
                GenelToplam();

            }
        }


        private void UrunGetir(Urun urun, string barkod, double miktar)
        {
            int satirsayisi = dataGridView1.Rows.Count;
            //double miktar = Convert.ToDouble(txtMiktar.Text);
            bool eklenmismi = false;
            if (satirsayisi > 0)
            {
                for (int i = 0; i < satirsayisi; i++)
                {
                    if (dataGridView1.Rows[i].Cells["Barkod"].Value.ToString() == barkod)
                    {
                        dataGridView1.Rows[i].Cells["Miktar"].Value = miktar + Convert.ToDouble(dataGridView1.Rows[i].Cells["Miktar"].Value);
                        dataGridView1.Rows[i].Cells["Toplam"].Value = Math.Round(Convert.ToDouble(dataGridView1.Rows[i].Cells["Miktar"].Value) * Convert.ToDouble(dataGridView1.Rows[i].Cells["Fiyat"].Value), 2);
                        eklenmismi = true;
                    }
                }
            }

            if (!eklenmismi)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[satirsayisi].Cells["Barkod"].Value = barkod;
                dataGridView1.Rows[satirsayisi].Cells["UrunAdi"].Value = urun.UrunAd;
                dataGridView1.Rows[satirsayisi].Cells["UrunGrup"].Value = urun.UrunGrup;
                dataGridView1.Rows[satirsayisi].Cells["Birim"].Value = urun.UrunBirim;
                dataGridView1.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyati;
                dataGridView1.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                dataGridView1.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyati, 2);
                dataGridView1.Rows[satirsayisi].Cells["AlisFiyati"].Value = urun.AlisFiyati;
                dataGridView1.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KdvTutari;

            }
        }
        private void GenelToplam()
        {
            double toplam = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                toplam += Convert.ToDouble(dataGridView1.Rows[i].Cells["Toplam"].Value);
            }

            txtGenelToplam.Text = toplam.ToString("C2");
            txtMiktar.Text = 1.ToString();
            txtBarkod.Clear();
            txtBarkod.Focus();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                dataGridView1.ClearSelection();
                GenelToplam();
                txtBarkod.Focus();
            }
        }

        private void frmSatis_Load(object sender, EventArgs e)
        {
            HizliErisimDoldur();
            btn5.Text = 5.ToString("C2");
            btn10.Text = 10.ToString("C2");
            btn20.Text = 20.ToString("C2");
            btn50.Text = 50.ToString("C2");
            btn100.Text = 100.ToString("C2");
            btn200.Text = 200.ToString("C2");
        }

        private void HizliErisimDoldur()
        {
            var hizliurun = db.HizliUrun.ToList();
            foreach (var x in hizliurun)
            {
                Button btnHizli = this.Controls.Find("btnHizli" + x.Id,true).FirstOrDefault() as Button;
                if (btnHizli!=null)
                {
                    double fiyat = Islemler.DoubleYap(x.Fiyat.ToString());
                    btnHizli.Text = x.UrunAd + "\n" + fiyat.ToString("C2");
                }
            }
        }

        private void HizliErisimClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int butonid = Convert.ToUInt16(b.Name.ToString().Substring(8, b.Name.Length - 8));
            if (b.Text.ToString().StartsWith("-"))
            {
                frmHizliErisimUrunEkleme frm = new frmHizliErisimUrunEkleme();
                frm.lblButonId.Text = butonid.ToString();
                frm.ShowDialog();
            }
            else
            {
                
                var urunBarkod = db.HizliUrun.Where(x => x.Id == butonid).Select(y => y.Barkod).FirstOrDefault();
                var urun = db.Urun.Where(x => x.UrunBarkod == urunBarkod).FirstOrDefault();
                UrunGetir(urun, urunBarkod, Convert.ToDouble(txtMiktar.Text));
                GenelToplam();
            }
            
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Button b = (Button)sender;

                if (!b.Text.StartsWith("-"))
                {
                    int butonid = Convert.ToUInt16(b.Name.ToString().Substring(8, b.Name.Length - 8));
                    ContextMenuStrip s = new ContextMenuStrip();
                    ToolStripMenuItem sil = new ToolStripMenuItem();
                    sil.Text = "Temizle - Buton No:" +butonid.ToString();
                    sil.Click += Sil_Click;
                    s.Items.Add(sil);
                    this.ContextMenuStrip = s;

                }

            }
        }

        private void Sil_Click(object sender, EventArgs e)
        {
            int butonid = Convert.ToUInt16(sender.ToString().Substring(19, sender.ToString().Length - 19));
            var guncellenecek = db.HizliUrun.Find(butonid);
            guncellenecek.Barkod = "-";
            guncellenecek.UrunAd = "-";
            guncellenecek.Fiyat = 0;
            db.SaveChanges();
            Button b = this.Controls.Find("btnHizli" + butonid, true).FirstOrDefault() as Button;
            b.Text = "-" + "\n" + "₺0,00";
            MessageBox.Show("Güncellendi");
        }

        private void bNx_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Text==",")
            {
                int virgul = txtNumarator.Text.Count(x => x == ',');
                if (virgul<1)
                {
                    txtNumarator.Text += b.Text;
                }
            }
            else if (b.Text == "<")
            {
                if (txtNumarator.Text.Length>0)
                {
                    txtNumarator.Text = txtNumarator.Text.Substring(0, txtNumarator.Text.Length - 1);
                }
            }
            else
            {
                txtNumarator.Text += b.Text;
            }
        }

        private void btnAdet_Click(object sender, EventArgs e)
        {
            if (txtNumarator.Text != "")
            {
                txtMiktar.Text = txtNumarator.Text;
                txtBarkod.Clear();
                txtNumarator.Clear();
                txtBarkod.Focus();
            }
        }

        private void btnOdenen_Click(object sender, EventArgs e)
        {
            if (txtNumarator.Text != "")
            {
                double sonuc = Islemler.DoubleYap(txtNumarator.Text) - Islemler.DoubleYap(txtGenelToplam.Text);
                txtParaUstu.Text = sonuc.ToString("C2");
                txtOdenen.Text = Islemler.DoubleYap(txtNumarator.Text).ToString("C2");
                txtNumarator.Clear();
                txtBarkod.Focus();
            }
        }

        private void btnBarkod_Click(object sender, EventArgs e)
        {
            if (txtNumarator.Text != "")
            {
                if (db.Urun.Any(x=>x.UrunBarkod == txtNumarator.Text))
                {
                    var urun = db.Urun.Where(x => x.UrunBarkod == txtNumarator.Text).FirstOrDefault();
                    UrunGetir(urun, txtNumarator.Text, Convert.ToDouble(txtMiktar.Text));
                    txtNumarator.Clear();
                    txtBarkod.Focus();
                }
                else
                {
                    MessageBox.Show("Ürün Ekleme Sayfasına Git");
                }
            }
        }

        private void ParaButonlari_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            double sonuc = Islemler.DoubleYap(b.Text) - Islemler.DoubleYap(txtGenelToplam.Text);
            txtOdenen.Text = Islemler.DoubleYap(b.Text).ToString("C2");
            txtParaUstu.Text = sonuc.ToString("C2");
        }

        private void btnDigerUrun_Click(object sender, EventArgs e)
        {
            if (txtNumarator.Text != "")
            {
                int satirsayisi = dataGridView1.Rows.Count;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[satirsayisi].Cells["Barkod"].Value = "1111111111116";
                dataGridView1.Rows[satirsayisi].Cells["UrunAdi"].Value = "Barkodsuz Ürün";
                dataGridView1.Rows[satirsayisi].Cells["UrunGrup"].Value = "Barkodsuz Ürün";
                dataGridView1.Rows[satirsayisi].Cells["Birim"].Value = "Adet";
                dataGridView1.Rows[satirsayisi].Cells["Miktar"].Value = 1;
                dataGridView1.Rows[satirsayisi].Cells["AlisFiyati"].Value = 0;
                dataGridView1.Rows[satirsayisi].Cells["Fiyat"].Value = Convert.ToDouble(txtNumarator.Text);
                dataGridView1.Rows[satirsayisi].Cells["KdvTutari"].Value = 0;
                dataGridView1.Rows[satirsayisi].Cells["Toplam"].Value = Convert.ToDouble(txtNumarator.Text);
                txtNumarator.Text = "";
                GenelToplam();
                txtBarkod.Focus();
            }
        }

        private void btnIade_Click(object sender, EventArgs e)
        {
            if (chbSatisIadeIslemi.Checked)
            {
                chbSatisIadeIslemi.Checked = false;
                chbSatisIadeIslemi.Text = "Satış Yapılıyor";
            }
            else
            {
                chbSatisIadeIslemi.Checked = true;
                chbSatisIadeIslemi.Text = "İade İşlemi";
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        public void Temizle()
        {
            txtMiktar.Text = Convert.ToDouble(1).ToString();
            txtBarkod.Clear();
            txtOdenen.Clear();
            txtParaUstu.Clear();
            txtGenelToplam.Text = 0.ToString("C2");
            chbSatisIadeIslemi.Checked = false;
            txtNumarator.Clear();
            dataGridView1.Rows.Clear();
            txtBarkod.Clear();
            txtBarkod.Focus();
        }


        public void SatisYap(string odemeturu)
        {
            int satirsayisi = dataGridView1.Rows.Count;
            bool satisIade = chbSatisIadeIslemi.Checked;
            double alisFiyatToplam = 0;
            if (satirsayisi>0)
            {
                int? islemNo = db.Islem3.First().islemNumarasi;
                Satiss satiss = new Satiss();

                for (int i = 0; i < satirsayisi; i++)
                {
                    satiss.islemNumarasi = islemNo;
                    satiss.urunAdi = dataGridView1.Rows[i].Cells["UrunAdi"].Value.ToString();
                    satiss.urunGrubu = dataGridView1.Rows[i].Cells["UrunGrup"].Value.ToString();
                    satiss.urunBarkodu = dataGridView1.Rows[i].Cells["Barkod"].Value.ToString();
                    satiss.urunBirim = dataGridView1.Rows[i].Cells["Birim"].Value.ToString();
                    satiss.alisFiyati = Islemler.DoubleYap( dataGridView1.Rows[i].Cells["AlisFiyati"].Value.ToString());
                    satiss.satisFiyati = Islemler.DoubleYap( dataGridView1.Rows[i].Cells["Fiyat"].Value.ToString());
                    satiss.miktari = Islemler.DoubleYap( dataGridView1.Rows[i].Cells["Miktar"].Value.ToString());
                    satiss.toplami = Islemler.DoubleYap( dataGridView1.Rows[i].Cells["Toplam"].Value.ToString());
                    satiss.kdvTutar = Islemler.DoubleYap( dataGridView1.Rows[i].Cells["KdvTutari"].Value.ToString()) * Islemler.DoubleYap(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString());
                    satiss.odemeSekli = odemeturu;
                    satiss.iadeIslem = satisIade;
                    satiss.tarihi = DateTime.Now;
                    satiss.kullaniciAdi = lblKasiyer.Text;
                    db.Satiss.Add(satiss);
                    db.SaveChanges();
                    
                    if (!satisIade)
                    {
                        Islemler.StokAzalt(dataGridView1.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    else
                    {
                        Islemler.StokArttir(dataGridView1.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString()));

                    }
                    alisFiyatToplam += Islemler.DoubleYap(dataGridView1.Rows[i].Cells["AlisFiyati"].Value.ToString());
                }

                ozet ozet = new ozet();
                ozet.OzetIslemNumarasi = islemNo;
                ozet.OzetIade = satisIade;
                ozet.OzetFiyatToplam = alisFiyatToplam;
                ozet.OzetGelir = false;
                ozet.OzetGider = false;
                if (!satisIade)
                {
                    ozet.OzetAciklama = odemeturu + " Satış";
                }
                else
                {
                    ozet.OzetAciklama = "İade işlemi (" + odemeturu + ")";
                }
                ozet.OzetOdemeSekli = odemeturu;
                ozet.OzetKullanici = lblKasiyer.Text;
                ozet.OzetTarih = DateTime.Now;
                switch (odemeturu)
                {
                    case "Nakit":
                        ozet.OzetNakit = Islemler.DoubleYap(txtGenelToplam.Text);
                        ozet.OzetKart = 0;
                        break;
                    case "Kart":
                        ozet.OzetKart = Islemler.DoubleYap(txtGenelToplam.Text);
                        ozet.OzetNakit = 0;
                        break;
                    case "Kart-Nakit":
                        ozet.OzetNakit = Islemler.DoubleYap(lblNakit.Text);
                        ozet.OzetKart = Islemler.DoubleYap(lblKart.Text);
                        break;
                }
                db.ozet.Add(ozet);
                db.SaveChanges();
                var islemNoArttir = db.Islem3.First();
                islemNoArttir.islemNumarasi += 1;
                db.SaveChanges();
                MessageBox.Show("Yazdırma İşlemi Yap");
                Temizle();
            }
        }

        private void btnNakit_Click(object sender, EventArgs e)
        {
            SatisYap("Nakit");
        }

        private void btnKart_Click(object sender, EventArgs e)
        {
            SatisYap("Kart");
        }

        private void btnNakitKart_Click(object sender, EventArgs e)
        {
            frmNakitKart frm = new frmNakitKart();
            frm.ShowDialog();
        }

        private void txtBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void txtNumarator_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void txtMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void frmSatis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                SatisYap("Nakit");
            }
            if (e.KeyCode == Keys.F2)
            {
                SatisYap("Kart");
            }
            if (e.KeyCode == Keys.F3)
            {
                frmNakitKart frm = new frmNakitKart();
                frm.ShowDialog();
            }
            
        }

        private void btnIslemBeklet_Click(object sender, EventArgs e)
        {
            if (btnIslemBeklet.Text == "İşlem Beklet")
            {
                IslemBeklet();
                btnIslemBeklet.BackColor = System.Drawing.Color.OrangeRed;
                btnIslemBeklet.Text = "İşlem Bekliyor";
                dataGridView1.Rows.Clear();
            }
            else
            {
                BeklemeIptal();
                btnIslemBeklet.BackColor = System.Drawing.Color.DimGray;
                btnIslemBeklet.Text = "İşlem Beklet";
                dataGridView1Kopya.Rows.Clear();
            }
            
        }

        private void IslemBeklet()
        {
            int satir = dataGridView1.Rows.Count;
            int sutun = dataGridView1.Columns.Count;
            if (satir > 0)
            {
                for (int i = 0; i < satir; i++)
                {
                    dataGridView1Kopya.Rows.Add();
                    for (int j = 0; j < sutun-1; j++)
                    {
                        dataGridView1Kopya.Rows[i].Cells[j].Value = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }
            }
        }
        private void BeklemeIptal()
        {
            int satir = dataGridView1Kopya.Rows.Count;
            int sutun = dataGridView1Kopya.Columns.Count;
            if (satir > 0)
            {
                for (int i = 0; i < satir; i++)
                {
                    dataGridView1.Rows.Add();
                    for (int j = 0; j < sutun-1; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = dataGridView1Kopya.Rows[i].Cells[j].Value;
                    }
                }
            }
        }


    }
}
