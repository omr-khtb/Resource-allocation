using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceAllocation
{
    public partial class Form4 : Form
    {
        Bitmap off;
        Timer tt = new Timer();
        List<BG> lBG = new List<BG>();
        List<btn> lbtn = new List<btn>();

        public Form4()
        {
            InitializeComponent();
            this.Load += Form4_Load;
            this.Paint += Form2_Paint;
            this.KeyDown += Form2_KeyDown;
            tt.Interval = 1;
            tt.Tick += Tt_Tick; tt.Start();
            this.Size = new Size(1800, 1200);

        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    e.Handled = true; // Prevents further processing of the key event
                    this.Hide();
                    Form2 f2 = new Form2();
                    f2.ShowDialog();
                    this.Close();
                    break;
            }

        }

        private void Tt_Tick(object sender, EventArgs e)
        {

            lBG[0].iFrame = (lBG[0].iFrame + 1) % 87;
            DrawDubb(this.CreateGraphics());
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            createBG();
            createbtn();
        }

        void createbtn()
        {
            btn pnn = new btn();
            pnn.img = new Bitmap("explaintxt.png");
            pnn.X = 220;
            pnn.Y = 230;
            lbtn.Add(pnn);
        }

        void createBG()
        {
            BG pnn = new BG();
            pnn.X = 0;
            pnn.Y = 0;

            for (int i = 1; i < 88; i++)
            {
                pnn.imgs.Add(new Bitmap("F (" + i + ").jpg"));
            }
            pnn.iFrame = 0;

            lBG.Add(pnn);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void DrawScene(Graphics g2)
        {
            Color BackColor;
            g2.Clear(Color.Black);

            for (int i = 0; i < lBG.Count - 1; i++) 
            {
                BackColor = lBG[i].imgs[lBG[i].iFrame].GetPixel(0, 0);
                lBG[i].imgs[lBG[i].iFrame].MakeTransparent(BackColor);
                g2.DrawImage(lBG[i].imgs[lBG[i].iFrame], lBG[i].X, lBG[i].Y);
            }

            for (int i = 0; i < lbtn.Count; i++)
            {
                BackColor = lbtn[i].img.GetPixel(0, 0);
                lbtn[i].img.MakeTransparent(BackColor);
                g2.DrawImage(lbtn[i].img, lbtn[i].X, lbtn[i].Y);
            }



        }


        class btn
        {
            public int X, Y;
            public Bitmap img;
        }


        class BG
        {
            public List<Bitmap> imgs = new List<Bitmap>();
            public int iFrame, X, Y;
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {

        }
    }
}

