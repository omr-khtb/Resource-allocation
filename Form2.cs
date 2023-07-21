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
    public partial class Form2 : Form
    {
        List<Portal> lPortal = new List<Portal>();
        List<Robot> lRobot = new List<Robot>();
        List<Title> lTitle = new List<Title>();
        Bitmap off; bool left, right;
        Timer tt = new Timer();
        bool hobaaa;

        public Form2()
        {
            this.WindowState = FormWindowState.Maximized;
            Load += Form2_Load;
            this.Paint += Form1_Paint;
            tt.Tick += Tt_Tick;
            this.KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            tt.Interval = 1;
            tt.Start();
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            createPortal();
            createRobot();
            createTitle();
            /*
            Form2 f2 = new Form2();
            this.Hide();
            f2.ShowDialog();
            */
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:

                    left = false;

                    break;
                case Keys.Right:

                    right = false;
                    break;

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    lRobot[0].flag = 1;
                    left = true;
                    right = false;
                    
                    break;
                case Keys.Right:
                    lRobot[0].flag = 3;
                    right = true;
                    left = false;
                    break;
                case Keys.Space:
                    IsHit();
                    break;
            }
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            animatePortal();

            lRobot[0].iStatic = (lRobot[0].iStatic + 1) % 29;
            //lTitle[0].iFrame = (lTitle[0].iFrame + 1) % 49;

            if (right && !left)
            {
                lRobot[0].flag = 3;
                lRobot[0].X += 20;
            }
            else if (left && !right)
            {
                lRobot[0].flag = 1;
                lRobot[0].X -= 20;
            }
            else
            {
                lRobot[0].flag = 2;
            }
           
            DrawDubb(this.CreateGraphics());
        }
        void IsHit()
        {
            if (lRobot[0].X >= lPortal[1].X)
            {
                // Create and show Form1
                this.Hide();
                Form1 f1 = new Form1();
                f1.ShowDialog();

                // Close the current form
                this.Close();
            }
            else if (lRobot[0].X <= lPortal[0].X)
            {
                // Create and show Form2
                this.Hide();
                Form4 f4 = new Form4();
                f4.ShowDialog();

                // Close the current form
                this.Close();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void animatePortal()
        {


            for (int i = 0; i < lPortal.Count; i++)
            {
                lPortal[i].iFrame = (lPortal[i].iFrame + 1) % 89;
            }
        }


        void createRobot()
        {
            Robot pnn = new Robot();


            for (int i = 1; i <= 1; i++) // left
            {
                pnn.Left.Add(new Bitmap("L" + i + ".png"));

            }

            for (int i = 1; i <= 1; i++) // right
            {
                pnn.Right.Add(new Bitmap("R" + i + ".png"));

            }

            for (int i = 1; i < 30; i++) // static
            {
                pnn.Static.Add(new Bitmap("RB (" + i + ").png"));

            }
            pnn.flag = 2;
            pnn.iLeft = 0;
            pnn.iRight = 0;
            pnn.iStatic = 0;
            pnn.X = this.ClientSize.Width / 2 - pnn.Left[0].Width / 2;
            pnn.Y = this.ClientSize.Height - pnn.Left[0].Height - 100;
            lRobot.Add(pnn);
        }


        void createPortal()
        {
            int initialX = this.ClientSize.Width / 5 - 150;
            for (int i = 0; i < 2; i++)
            {
                Portal pnn = new Portal();

                pnn.X = initialX;
                pnn.iFrame = 0;
                initialX += (this.ClientSize.Width / 5) * 3;

                for (int j = 1; j < 90; j++)
                {
                    pnn.imgs.Add(new Bitmap("P (" + j + ").jpg"));
                }

                pnn.Y = this.ClientSize.Height - pnn.imgs[0].Height - 100;


                pnn.subTitle = new Title();
                pnn.subTitle.imgs.Add(new Bitmap("PT" + (i + 1) + ".png"));
                pnn.subTitle.rcSrc = new Rectangle(0, 0, pnn.imgs[0].Width * 2, pnn.imgs[0].Height * 2);
                int X = pnn.X + (pnn.imgs[0].Width / 2) - (pnn.subTitle.imgs[0].Width / 4);
                int Y = pnn.Y - 150;
                pnn.subTitle.rcDst = new Rectangle(X, Y, pnn.imgs[0].Width, pnn.imgs[0].Height);

                lPortal.Add(pnn);

            }
        }


        void createTitle()
        {
            int initialX = this.ClientSize.Width / 2;
            for (int i = 0; i < 1; i++)
            {
                Title pnn = new Title();




                pnn.imgs.Add(new Bitmap("T1.png"));


                pnn.iFrame = 0;
                pnn.rcSrc = new Rectangle(0, 0, pnn.imgs[0].Width, pnn.imgs[0].Height);
                int X = this.ClientSize.Width / 2 - pnn.imgs[0].Width / 4;
                int Y = 150;
                pnn.rcDst = new Rectangle(X, Y, pnn.imgs[0].Width / 2, pnn.imgs[0].Height / 2);


                lTitle.Add(pnn);

            }
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



            for (int i = 0; i < lPortal.Count; i++)
            {
                BackColor = lPortal[i].imgs[lPortal[i].iFrame].GetPixel(0, 0);
                lPortal[i].imgs[lPortal[i].iFrame].MakeTransparent(BackColor);
                g2.DrawImage(lPortal[i].imgs[lPortal[i].iFrame], lPortal[i].X, lPortal[i].Y);



                BackColor = lPortal[i].subTitle.imgs[lPortal[i].subTitle.iFrame].GetPixel(0, 0);
                lPortal[i].subTitle.imgs[lPortal[i].subTitle.iFrame].MakeTransparent(BackColor);
                lPortal[i].subTitle.imgs[lPortal[i].subTitle.iFrame].MakeTransparent(Color.Black);
                g2.DrawImage(lPortal[i].subTitle.imgs[lPortal[i].subTitle.iFrame], lPortal[i].subTitle.rcDst, lPortal[i].subTitle.rcSrc, GraphicsUnit.Pixel);

            }

            for (int i = 0; i < lTitle.Count; i++)
            {
                BackColor = lTitle[i].imgs[lTitle[i].iFrame].GetPixel(0, 0);
                lTitle[i].imgs[lTitle[i].iFrame].MakeTransparent(BackColor);
                lTitle[i].imgs[lTitle[i].iFrame].MakeTransparent(Color.Black);
                g2.DrawImage(lTitle[i].imgs[lTitle[i].iFrame], lTitle[i].rcDst, lTitle[i].rcSrc, GraphicsUnit.Pixel);
            }

            for (int i = 0; i < lRobot.Count-1; i++)
            {

                if (lRobot[0].flag == 1)
                {
                    BackColor = lRobot[i].Left[lRobot[i].iLeft].GetPixel(50, 60);
                    lRobot[i].Left[lRobot[i].iLeft].MakeTransparent(BackColor);
                    lRobot[i].Left[lRobot[i].iLeft].MakeTransparent(Color.FromArgb(254, 252, 253));
                    g2.DrawImage(lRobot[i].Left[lRobot[i].iLeft], lRobot[i].X, lRobot[i].Y);
                }
                else if (lRobot[0].flag == 2)
                {
                    BackColor = lRobot[i].Static[lRobot[i].iStatic].GetPixel(0, 0);
                    lRobot[i].Static[lRobot[i].iStatic].MakeTransparent(BackColor);
                    g2.DrawImage(lRobot[i].Static[lRobot[i].iStatic], lRobot[i].X, lRobot[i].Y);
                }
                else if (lRobot[0].flag == 3)
                {
                    BackColor = lRobot[i].Right[lRobot[i].iRight].GetPixel(0, 0);
                    lRobot[i].Right[lRobot[i].iRight].MakeTransparent(BackColor);
                    g2.DrawImage(lRobot[i].Right[lRobot[i].iRight], lRobot[i].X, lRobot[i].Y);
                }
            }


        }
    }


    class Robot
    {
        public List<Bitmap> Left = new List<Bitmap>();
        public List<Bitmap> Right = new List<Bitmap>();
        public List<Bitmap> Static = new List<Bitmap>();
        public int X, Y, iStatic, iLeft, iRight;
        public int flag;
        // 1 for left 
        // 2 for static 
        // 3 for right
    }

    class Title
    {
        public List<Bitmap> imgs = new List<Bitmap>();
        public int iFrame;
        public Rectangle rcDst;
        public Rectangle rcSrc;
    }

    class Portal
    {
        public List<Bitmap> imgs = new List<Bitmap>();
        public Title subTitle;
        public int X, Y, iFrame;
    }
}
