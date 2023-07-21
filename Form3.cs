using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceAllocation
{
    public partial class Form3 : Form
    {
        public Form3(int pnum,int rnum,int []availlable, int [ , ]allocation, int[,] request, int[,] needed, int[,] maximum)
        {

            InitializeComponent();
            dataGridView2.Columns.Add("Column1", " ");
            dataGridView3.Columns.Add("Column1", " ");
            dataGridView4.Columns.Add("Column1", " ");
            dataGridView5.Columns.Add("Column1", " ");
            for (int i = 0; i < rnum ; i++)
            {
                dataGridView1.Columns.Add("R"+i, "R" + i);
                dataGridView2.Columns.Add("R"+i, "R" + i);
                dataGridView3.Columns.Add("R"+i, "R" + i);
                dataGridView4.Columns.Add("R"+i, "R" + i);
                dataGridView5.Columns.Add("R"+i, "R" + i);
            }

            for (int i = 0; i < pnum; i++)
            {
                dataGridView2.Rows.Add("R" + i, " ");
                dataGridView3.Rows.Add("R" + i, " ");
                dataGridView4.Rows.Add("R" + i, " ");
                dataGridView5.Rows.Add("R" + i, " ");
            }


            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                // Loop through each column
                for (int col = 0; col < availlable.Length; col++)
                {
                    // Retrieve the value of the current cell
                    dataGridView1.Rows[row].Cells[col].Value = availlable[col].ToString();
                }
            }

           
                // Loop through each column
                for (int col = 0; col < pnum; col++)
                {
                    // Retrieve the value of the current cell
                    dataGridView2.Rows[col].Cells[0].Value = "P" + col.ToString();
                    dataGridView3.Rows[col].Cells[0].Value = "P" + col.ToString();
                    dataGridView4.Rows[col].Cells[0].Value = "P" + col.ToString();
                    dataGridView5.Rows[col].Cells[0].Value = "P" + col.ToString();
                }

            for (int row = 0; row < dataGridView2.Rows.Count - 1; row++)
            {
                for (int col = 0; col < allocation.GetLength(1); col++)
                {
                    dataGridView2.Rows[row].Cells[col+1].Value = allocation[row, col];
                }
            }

            for (int row = 0; row < dataGridView3.Rows.Count - 1; row++)
            {
                for (int col = 0; col < request.GetLength(1); col++)
                {
                    dataGridView3.Rows[row].Cells[col + 1].Value = request[row, col];
                }
            }

            for (int row = 0; row < dataGridView4.Rows.Count - 1; row++)
            {
                for (int col = 0; col < request.GetLength(1); col++)
                {
                    dataGridView4.Rows[row].Cells[col + 1].Value = needed[row, col];
                }
            }

            for (int row = 0; row < dataGridView5.Rows.Count - 1; row++)
            {
                for (int col = 0; col < request.GetLength(1); col++)
                {
                    dataGridView5.Rows[row].Cells[col + 1].Value = maximum[row, col];
                }
            }

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
