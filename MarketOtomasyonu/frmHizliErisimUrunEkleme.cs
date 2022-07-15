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
    public partial class frmHizliErisimUrunEkleme : Form
    {
        public frmHizliErisimUrunEkleme()
        {
            InitializeComponent();
        }
        MarketOtomasyonuDbEntities db = new MarketOtomasyonuDbEntities();
        private void txtUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (txtUrunAra.Text != "")
            {
                string urunad = txtUrunAra.Text;
                var urunler = db.Urun.Where(x=>x.UrunAd.Contains(urunad)).ToList();
                dataGridView2.DataSource = urunler;
            }
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count>0)
            {
                string barkod = dataGridView2.CurrentRow.Cells["UrunBarkod"].Value.ToString();
                string urunad = dataGridView2.CurrentRow.Cells["UrunAd"].Value.ToString();
                double fiyat = Convert.ToDouble( dataGridView2.CurrentRow.Cells["SatisFiyati"].Value.ToString());

                int id = Convert.ToInt16(lblButonId.Text);

                var guncellenecek = db.HizliUrun.Find(id);
                guncellenecek.Barkod = barkod;
                guncellenecek.UrunAd = urunad;
                guncellenecek.Fiyat = fiyat;
                db.SaveChanges();
                MessageBox.Show("Güncellendi");
                frmSatis frm = (frmSatis)Application.OpenForms["frmSatis"];
                if (frm != null)
                {
                    Button b = frm.Controls.Find("btnHizli" + id, true).FirstOrDefault() as Button;
                    b.Text = urunad + "\n" + fiyat.ToString("C2");
                }
            }
        }

        private void chbTumunuGoster_CheckedChanged(object sender, EventArgs e)
        {
            var urunler = db.Urun.ToList();
            if (chbTumunuGoster.Checked)
            {
                dataGridView2.DataSource = urunler;
            }
            else
            {
                dataGridView2.DataSource = null;
            }
            
        }
    }
}
