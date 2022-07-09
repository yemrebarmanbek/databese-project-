using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace veri_tabani_kayit_ekleme_silme
{
    public partial class Form1 : Form
    {
        SqlConnection baglan = new SqlConnection(@"server=(localdb)\MSSQLLocalDB; initial catalog=kutuphane; integrated security=true;");

        private void verilerigoster()
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("Select*from kitaplar", baglan); //seçme komutu

            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read()) //bağlantı okuduğu sürece listviewe ekle anlamı taşır
            {
                ListViewItem ekle = new ListViewItem();  //listview ekleme komutları

                ekle.Text = oku["id"].ToString();  //bu okuma işlemler databasedeki tablodaki değerler
                ekle.SubItems.Add(oku["kitapad"].ToString());
                ekle.SubItems.Add(oku["yazar"].ToString());
                ekle.SubItems.Add(oku["yayinevi"].ToString());
                ekle.SubItems.Add(oku["sayfa"].ToString());

                listView1.Items.Add(ekle); //listviewe ekleme komutu

            }
            baglan.Close();
        }


            public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verilerigoster();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglan.Open();
            //burada textbox'lardan alınan bilgilerin veri tabanına kayıt etme işlemi gösterilmiştir.  sql komutundaki parantez içi databesedeki tablodaki değerler
            SqlCommand komut2 = new SqlCommand("insert into kitaplar (id,kitapad,yazar,yayinevi,sayfa) Values ('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + textBox4.Text.ToString() + "','" + textBox5.Text.ToString() + "')", baglan);
            komut2.ExecuteNonQuery();
            baglan.Close();

            verilerigoster();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        int id = 0;
        private void button3_Click(object sender, EventArgs e) //silme butonu komutları
        {
            baglan.Open();
            SqlCommand komut3 = new SqlCommand("Delete From kitaplar where id=(" + id + ") ", baglan);
            komut3.ExecuteNonQuery();
            baglan.Close();   
            verilerigoster();
        }

        //silme komutunun çalışması için id işleminin seçilmesi gerek bu yüzden listviewden mouse double click özelliğini açmamız gerek.
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            id = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text); //her bir değer için bir id sırası tanımlandı
            
            //listviewde tıklanılan değerin tek tek textboxtaki değerleri alında buton3'e tıklanınca bu değerler alınsın diye.
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text; 
            textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox1.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBox1.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBox1.Text = listView1.SelectedItems[0].SubItems[4].Text;
        }


        //yanlış kaydedilen bir kitabı bu buton ile düzeltme işlemi yaparız
        private void button4_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand(" Update kitaplar set id='" + textBox1.Text.ToString() + "',kitapad='" + textBox2.Text.ToString() + "',yazar='" + textBox3.Text.ToString() + "',yayınevi='" + textBox4.Text.ToString() + "',sayfa='" + textBox5.Text.ToString() + "' where id=(" + id + ")", baglan);
            komut.ExecuteNonQuery();
            baglan.Close();
            verilerigoster();
        }
    }
}
