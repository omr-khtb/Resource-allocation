using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceAllocation.Properties
{
    public partial class Form5 : Form
    {
        public int row, col;
        public int[,] matrixx;
        public Form5()
        {
            InitializeComponent();
            Load += Form5_Load;
            FormClosing += Form5_FormClosing;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            int xPos = 70;
            int textBoxSize = 200, yPos = 70;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Location = new System.Drawing.Point(xPos + j * textBoxSize, yPos + i * textBoxSize);
                    textBox.Size = new System.Drawing.Size(textBoxSize, 200);
                    textBox.TextAlign = HorizontalAlignment.Center;
                    this.Controls.Add(textBox);
                }
            }
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            int r = 0, c = 0;
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    string textBoxValue = textBox.Text;
                    if (int.TryParse(textBoxValue, out int floatValue))
                    {
                        matrixx[r, c] = floatValue;
                    }
                    
                    c++;
                    if (c == col)
                    {
                        c = 0;
                        r++;
                    }
                }
            }

        }

        public void Doit(ref int[,] List, int r, int c)
        {
            row = r; col = c;
            List = matrixx;
        }
    }

}
