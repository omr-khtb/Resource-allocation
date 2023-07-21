using ResourceAllocation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceAllocation
{
    public partial class Form1 : Form
    {


        Bitmap OFF; int flag_who;
        bool[] isSafe;
        int[] safeSequence;
        List<Arrow> Arrows = new List<Arrow>();
        List<Edge> Process = new List<Edge>();
        List<Edge> Resource = new List<Edge>();
        int[,] requestMatrix;
        int[,] maximumMatrix;
        int[,] neededMatrix = new int[,]
 {
    {1, 2, 3},
    {4, 5, 6},
    {7, 8, 9}
 };
        int[,] allocationMatrix;
        int[] availabeMatrx;
        bool drag; int selected; int oldx, oldy, dx, dy, arrowOldX, arrowOldY, currentX, currentY;
        int edgeButton = 0, select, typeStart, typeEnd;
        bool finishedDrag = false;



        //GUI
        bool sidebarExpand = true;


        public Form1()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
            sidebar.Height = this.ClientSize.Height;
            this.Size = new Size(1700, 850);
            this.CenterToScreen();
            Paint += Form1_Paint;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            KeyDown += Form1_KeyDown;
            sidebarTimer.Interval = 1;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:

                    this.Hide();
                    Form2 f2 = new Form2();
                    f2.ShowDialog();
                    this.Close();
                    break;
            }
        }
        void DeadLockAvoid()
        {
            int[,] maximumMatrix = new int[,]
            {
         { 7, 5, 3},
         { 3, 2, 2},
         { 9, 0, 2},
         {2, 2, 2},
         { 4, 3, 3},
};
  
            

            bool allProcessesDone = true;
            // Get the dimensions of the matrices
            int rowCount = allocationMatrix.GetLength(0);
            int columnCount = allocationMatrix.GetLength(1);
            // Initialize the needed matrix
            neededMatrix = new int[rowCount, columnCount];

            // Calculate the needed matrix
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    neededMatrix[i, j] = maximumMatrix[i, j] - allocationMatrix[i, j];
                }
            }


            string matrixString = "";
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    matrixString += allocationMatrix[i, j].ToString() + "\t";
                }
                matrixString += "\n";
            }
            // Display the neededMatrix in a MessageBox
            MessageBox.Show(matrixString, "allocation Matrix");

            matrixString = "";
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    matrixString += neededMatrix[i, j].ToString() + "\t";
                }
                matrixString += "\n";
            }
            // Display the neededMatrix in a MessageBox
            MessageBox.Show(matrixString, "Needed Matrix");


            matrixString = "";
            for (int i = 0; i < columnCount; i++)
            {
                matrixString += availabeMatrx[i].ToString() + "\t";
            }
            // Display the neededMatrix in a MessageBox
            MessageBox.Show(matrixString, "availble matrix");

            bool[] processDone = new bool[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                processDone[i] = false;
            }
            for (int z = 0; z < 6; z++)
            {
                bool deadlockDetected = true;
                allProcessesDone = true;
                // Check each process in the needed matrix
                for (int i = 0; i < rowCount; i++)
                {
                    // Skip the process if it's already done
                    if (processDone[i])
                        continue;

                    bool canExecute = true;

                    // Compare each resource in the process with the available resources
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (neededMatrix[i, j] > availabeMatrx[j])
                        {
                            canExecute = false;
                            break;
                        }
                    }

                    if (canExecute)
                    {
                        // Process can be executed, mark it as done and release allocated resources
                        processDone[i] = true;
                        MessageBox.Show("this p is done: " + i);
                        for (int j = 0; j < columnCount; j++)
                        {
                            availabeMatrx[j] += allocationMatrix[i, j];
                        }
                        deadlockDetected = false;
                        matrixString = "";
                        for (int ads = 0; ads < columnCount; ads++)
                        {
                            matrixString += availabeMatrx[ads].ToString() + "\t";
                        }
                        // Display the neededMatrix in a MessageBox
                        MessageBox.Show(matrixString, "availble matrix");
                    }
                }
                for (int i = 0; i < rowCount; i++)
                {
                    if (processDone[i] == false)
                    {
                        allProcessesDone = false;
                    }
                }






                if (allProcessesDone)
                {
                    MessageBox.Show("all prodcess are done simultaneously");
                    break;
                }

            }
            allProcessesDone = true;
            for (int i = 0; i < rowCount; i++)
            {
                if (processDone[i] == false)
                {
                    allProcessesDone = false;
                }
            }

            if (!allProcessesDone)
            {
                string a = "";
                for (int i = 0; i < rowCount; i++)
                {
                    if (processDone[i] == false)
                    {
                        a += " p:" + i;
                    }

                }
                MessageBox.Show("processes not done are: " + a);
            }
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:

                    this.Hide();
                    Form2 f2 = new Form2();
                    f2.ShowDialog();
                    this.Close();
                    break;

            }
        }
        void detectDeadLock()
        {
            int[] Work = new int[Resource.Count];
            string[] finish = new string[Process.Count];
            int ctProcess = 0, firstTry = 0;
            for (int i = 0; i < allocationMatrix.GetLength(0); i++)
            {
                int flag = 0;
                for (int j = 0; j < allocationMatrix.GetLength(1); j++)
                {
                    if (allocationMatrix[i, j] != 0)
                    {
                        flag = 1;
                    }
                }
                if (flag == 1)
                {
                    finish[i] = "false";
                }
                else
                {
                    finish[i] = "true";
                }
            }
            for (int i = 0; i < Resource.Count; i++)
            {
                Work[i] = 0;
            }
            while (ctProcess < Process.Count && (firstTry == 0 || firstTry == 1))
            {
                for (int i = 0; i < Process.Count; i++)
                {
                    if (finish[i] == "false")
                    {
                        for (int k = 0; k < requestMatrix.GetLength(0); k++)
                        {
                            int flag = 0;

                            for (int j = 0; j < requestMatrix.GetLength(1); j++)
                            {
                                if (requestMatrix[k, j] > Work[j])
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                for (int j = 0; j < requestMatrix.GetLength(1); j++)
                                {
                                    Work[j] += requestMatrix[k, j];
                                }
                                //MessageBox.Show(ctProcess + " " + i);
                                finish[i] = "true";
                            }
                        }
                    }
                }
                ctProcess++;
            }
            for (int i = 0; i < Process.Count; i++)
            {
                if (finish[i] == "false")
                {
                    MessageBox.Show("System is Deadlocked");
                    break;
                }
            }

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (edgeButton == 1)
            {
                int holder = isHit(e.X, e.Y, ref typeEnd);
                int creationFlag = 1;
                if (holder != -1)
                {
                    if (typeEnd != typeStart)// to make sure that the arrow between resource and process 1 is for Process, 2 is for Resource
                    {
                        drag = false;
                        finishedDrag = true;
                        if (typeStart == 1)
                        {
                            //if (Process[select].provideArrows.Count != 0)
                            //{
                            //    creationFlag = 0;
                            //}
                            //if (Resource[holder].arrows.Count > Resource[holder].resourcePoints)
                            //    creationFlag = 0;
                        }
                        else
                        {
                            if (Resource[select].provideArrows.Count >= Resource[select].resourcePoints)
                            {
                                creationFlag = 0;
                            }
                            if (Process[holder].recievedArrows.Count != 0)
                                creationFlag = 1;
                        }
                        if (creationFlag == 1)
                        {
                            Arrow arrow = new Arrow();
                            arrow.srcX = arrowOldX; arrow.srcY = arrowOldY;
                            arrow.dstX = currentX; arrow.dstY = currentY;
                            arrow.type = typeEnd;
                            if (typeStart == 1)
                            {
                                Process[select].arrowState.Add("provide");
                                Process[select].provideArrows.Add(arrow);
                                Process[select].moveArrows.Add(arrow);
                                Resource[holder].arrowState.Add("recieve");
                                Resource[holder].recievedArrows.Add(arrow);
                                Resource[holder].moveArrows.Add(arrow);
                            }
                            else
                            {
                                Process[holder].arrowState.Add("recieve");
                                Process[holder].recievedArrows.Add(arrow);
                                Process[holder].moveArrows.Add(arrow);
                                Resource[select].arrowState.Add("provide");
                                Resource[select].provideArrows.Add(arrow);
                                Resource[select].moveArrows.Add(arrow);

                            }
                            Arrows.Add(arrow);
                            //MessageBox.Show(arrow.srcX + " " + arrow.srcY + " " + arrow.dstX + " " + arrow.dstY + " " + arrow.type + "");
                        }
                    }
                    else
                        drag = false;
                }
                else
                    drag = false;
            }
            else
            {
                drag = false;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag && edgeButton != 1)
            {
                dx = e.X - oldx; dy = e.Y - oldy;
                if (flag_who == 1)
                {
                    Process[selected].X += dx;
                    Process[selected].Y += dy;

                    if (Process[selected].moveArrows.Count != 0)
                        for (int i = 0; i < Process[selected].moveArrows.Count; i++)
                        {
                            if (Process[selected].arrowState[i] == "recieve")
                            {
                                Process[selected].moveArrows[i].dstX += dx;
                                Process[selected].moveArrows[i].dstY += dy;
                            }
                            else
                            {
                                Process[selected].moveArrows[i].srcX += dx;
                                Process[selected].moveArrows[i].srcY += dy;
                            }
                        }
                }
                else if (flag_who == 2)
                {
                    Resource[selected].X += dx;
                    Resource[selected].Y += dy;
                    if (Resource[selected].moveArrows.Count != 0)
                        for (int i = 0; i < Resource[selected].moveArrows.Count; i++)
                        {
                            if (Resource[selected].arrowState[i] == "recieve")
                            {
                                Resource[selected].moveArrows[i].dstX += dx;
                                Resource[selected].moveArrows[i].dstY += dy;
                            }
                            else
                            {
                                Resource[selected].moveArrows[i].srcX += dx;
                                Resource[selected].moveArrows[i].srcY += dy;
                            }
                        }
                }

                oldx = e.X; oldy = e.Y;
            }
            if (drag && edgeButton == 1)
            {
                currentX = e.X; currentY = e.Y;
            }
            DrawDub(this.CreateGraphics());
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            arrowOldX = oldx = e.X;
            arrowOldY = oldy = e.Y;
            if (edgeButton == 0)
            {
                flag_who = 0;
                for (int i = 0; i < Process.Count; i++)
                {
                    if (e.X >= Process[i].X && e.X <= Process[i].X + Process[i].W
                        && e.Y >= Process[i].Y && e.Y <= Process[i].Y + Process[i].H)
                    {
                        drag = true;
                        selected = i; flag_who = 1;
                        break;
                    }
                }
                for (int i = 0; i < Resource.Count; i++)
                {
                    if (e.X >= Resource[i].X && e.X <= Resource[i].X + Resource[i].W
                        && e.Y >= Resource[i].Y && e.Y <= Resource[i].Y + Resource[i].H)
                    {
                        drag = true;
                        selected = i; flag_who = 2;
                        break;
                    }
                }
                if (e.Clicks >= 2 && flag_who == 2 && e.Button == MouseButtons.Left)
                {
                    Resource[selected].resourcePoints++;
                }
                if (e.Clicks >= 2 && flag_who == 2 && e.Button == MouseButtons.Right)
                {
                    Resource[selected].resourcePoints--;
                }
            }
            else
            {
                select = isHit(e.X, e.Y, ref typeStart);
                if (select != -1)
                {
                    drag = true;
                    currentX = e.X;
                    currentY = e.Y;
                }
            }

        }
        int isHit(int XM, int YM, ref int type)
        {
            for (int i = 0; i < Resource.Count; i++)
            {
                if (XM >= Resource[i].X && XM <= Resource[i].X + Resource[i].W
                        && YM >= Resource[i].Y && YM <= Resource[i].Y + Resource[i].H)
                {
                    type = 2;
                    return i;
                }
            }
            for (int i = 0; i < Process.Count; i++)
            {
                if (XM >= Process[i].X && XM <= Process[i].X + Process[i].W
                        && YM >= Process[i].Y && YM <= Process[i].Y + Process[i].H)
                {
                    type = 1;// 1 is for Process, 2 is for Resource
                    return i;
                }
            }
            return -1;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDub(e.Graphics);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            createRequestMatrix();
        }

        private void sidebsr(object sender, EventArgs e)
        {

        }

        private void sidebarTime_Tick(object sender, EventArgs e)
        {

            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void menu_btn_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OFF = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            /*
            Form1 f1 = new Form1();
            f1.Hide();
            Form3 f3 = new Form3(P);
            f3.ShowDialog();*/


        }

        private void button5_Click(object sender, EventArgs e)
        {
            DeadLockAvoid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edgeButton = 0;
            createProcess();
            DrawDub(this.CreateGraphics());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(Process.Count, Resource.Count, availabeMatrx, allocationMatrix, requestMatrix, neededMatrix, maximumMatrix);

            f3.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edgeButton = 0;
            createResource();
            DrawDub(this.CreateGraphics());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            edgeButton = 1;
        }
        void createProcess()
        {
            Edge pnn = new Edge();
            Random R = new Random();
            int X = R.Next(sidebar.Width, this.ClientSize.Width - 50);
            int Y = R.Next(150, this.ClientSize.Height / 2 - 50);
            pnn.X = X;
            pnn.Y = Y;
            pnn.W = 70;
            pnn.H = 70;
            pnn.clr = Color.Turquoise;
            Process.Add(pnn);
        }

        void createResource()
        {
            Edge pnn = new Edge();
            Random R = new Random();
            int X = R.Next(sidebar.Width, this.ClientSize.Width - 60);
            int Y = R.Next(150, this.ClientSize.Height / 2 - 50);
            pnn.X = X;
            pnn.Y = Y;
            pnn.W = 60;
            pnn.H = 80;
            pnn.clr = Color.Coral;
            Resource.Add(pnn);
        }

        void createRequestMatrix()
        {
            requestMatrix = new int[Process.Count, Resource.Count];
            //public int srcX, srcY, dstX, dstY, type;
            for (int i = 0; i < Arrows.Count; i++)
            {
                if (Arrows[i].type == 2)
                {
                    for (int j = 0; j < Process.Count; j++)
                    {
                        if (Arrows[i].srcX >= Process[j].X && Arrows[i].srcX <= Process[j].X + Process[j].W &&
                                Arrows[i].srcY >= Process[j].Y && Arrows[i].srcY <= Process[j].Y + Process[j].Y)
                        {
                            for (int k = 0; k < Resource.Count; k++)
                            {
                                if (Arrows[i].dstX >= Resource[k].X && Arrows[i].dstX <= Resource[k].X + Resource[k].W &&
                                    Arrows[i].dstY >= Resource[k].Y && Arrows[i].dstY <= Resource[k].Y + Resource[k].Y)
                                {
                                    requestMatrix[j, k] = 1;
                                }
                                else if (requestMatrix[j, k] != 1)
                                {
                                    requestMatrix[j, k] = 0;
                                }
                            }
                        }
                    }
                }
            }
            //for (int i = 0; i < requestMatrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < requestMatrix.GetLength(1); j++)
            //    {
            //        MessageBox.Show(i + " " + j + " " + requestMatrix[i, j]);
            //    }
            //}
            createAllocationtMatrix();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            
        }

        void createAllocationtMatrix()
        {
            allocationMatrix = new int[Process.Count, Resource.Count];
            //public int srcX, srcY, dstX, dstY, type;
            for (int i = 0; i < Arrows.Count; i++)
            {
                if (Arrows[i].type == 1)
                {
                    for (int j = 0; j < Process.Count; j++)
                    {
                        if (Arrows[i].dstX >= Process[j].X && Arrows[i].dstX <= Process[j].X + Process[j].W &&
                                Arrows[i].dstY >= Process[j].Y && Arrows[i].dstY <= Process[j].Y + Process[j].Y)
                        {
                            for (int k = 0; k < Resource.Count; k++)
                            {
                                if (Arrows[i].srcX >= Resource[k].X && Arrows[i].srcX <= Resource[k].X + Resource[k].W &&
                                    Arrows[i].srcY >= Resource[k].Y && Arrows[i].srcY <= Resource[k].Y + Resource[k].Y)
                                {
                                    allocationMatrix[j, k] = 1;
                                    Resource[k].usedPoints++;
                                }
                                else if (allocationMatrix[j, k] != 1)
                                {
                                    allocationMatrix[j, k] = 0;
                                }
                            }
                        }
                    }
                }
            }
            createAvailabe();
        }

        void createAvailabe()
        {
            availabeMatrx = new int[Resource.Count];
            for (int i = 0; i < Resource.Count; i++)
            {
                availabeMatrx[i] = Resource[i].resourcePoints - Resource[i].usedPoints;
            }
            detectDeadLock();
        }

        void DrawDub(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(OFF);
            DrawScene(g2);
            g.DrawImage(OFF, 0, 0);
        }
        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.White);
            string text;
            for (int i = 0; i < Process.Count; i++)
            {
                Font font = new Font("Courier New", 16);
                Brush brush = Brushes.Black;
                text = "P" + i;
                Brush brsh = new SolidBrush(Process[i].clr);
                Pen pn = new Pen(Color.Black, 2);

                g2.FillEllipse(brsh, Process[i].X, Process[i].Y, Process[i].W, Process[i].H);
                g2.DrawEllipse(pn, Process[i].X, Process[i].Y, Process[i].W, Process[i].H);
                g2.DrawString(text, font, brush, Process[i].X + (Process[i].W / 2) - 16, Process[i].Y + (Process[i].H / 2) - 8);
            }
            int resourceOffset;
            for (int i = 0; i < Resource.Count; i++)
            {
                resourceOffset = 0;
                Font font = new Font("Courier New", 16, FontStyle.Bold);
                Brush brush = Brushes.Black;
                text = "R" + i;
                Brush brsh = new SolidBrush(Resource[i].clr);
                Pen pn = new Pen(Color.Black, 2);

                g2.FillRectangle(brsh, Resource[i].X, Resource[i].Y, Resource[i].W, Resource[i].H);
                g2.DrawRectangle(pn, Resource[i].X, Resource[i].Y, Resource[i].W, Resource[i].H);
                g2.DrawString(text, font, brush, Resource[i].X + (Resource[i].W / 2) - 16, Resource[i].Y - 20);
                for (int z = 0; z < Resource[i].resourcePoints; z++)
                {
                    g2.DrawString(Resource[i].resourcePoints.ToString(), font, brush, Resource[i].X + (Resource[i].W / 2) - 10, Resource[i].Y + (Resource[i].H / 2) - 10);
                    resourceOffset += 10;
                }
            }
            //drawing the arrows
            Pen drawPen = penCreation(0, -1);
            if (drag && edgeButton == 1)
            {
                g2.DrawLine(drawPen, oldx, oldy, currentX, currentY);
            }

            if (finishedDrag)
            {
                for (int i = 0; i < Arrows.Count; i++)
                {
                    drawPen = penCreation(Arrows[i].type, i);
                    g2.DrawLine(drawPen, Arrows[i].srcX, Arrows[i].srcY, Arrows[i].dstX, Arrows[i].dstY);
                }
            }
        }
        Pen penCreation(int type, int i)
        {
            float[] dashValues = { 2, 2, 2, 2 };
            Pen drawPen = new Pen(Color.Black, 3);
            drawPen.DashPattern = dashValues;
            AdjustableArrowCap cap = new AdjustableArrowCap(4, 4);
            drawPen.CustomEndCap = cap;
            if (i == -1)
                return drawPen;
            if (Arrows[i].type == 1)
            {
                drawPen = new Pen(Color.Green, 3);
                drawPen.DashPattern = dashValues;
                drawPen.CustomEndCap = cap;
            }
            else
            {
                drawPen = new Pen(Color.Purple, 3);
                drawPen.DashPattern = dashValues;
                drawPen.CustomEndCap = cap;
            }
            return drawPen;
        }
        void bankerAlgorithm()
        {
            int numberOfProcesses = Process.Count;
            int numberOfResources = Resource.Count;

            neededMatrix = new int[numberOfProcesses, numberOfResources];
            isSafe = new bool[numberOfProcesses];
            safeSequence = new int[numberOfProcesses];
            maximumMatrix = new int[numberOfProcesses, numberOfResources];
            for (int r = 0; r < numberOfProcesses; r++)
                for (int c = 0; c < numberOfResources; c++)
                    maximumMatrix[r, c] = 10;
            // Initialize need matrix
            for (int i = 0; i < numberOfProcesses; i++)
            {
                for (int j = 0; j < numberOfResources; j++)
                {
                    neededMatrix[i, j] = maximumMatrix[i, j] - allocationMatrix[i, j];
                }
            }
        }
        public bool IsSafe()
        {
            int numberOfProcesses = allocationMatrix.GetLength(0);
            int numberOfResources = allocationMatrix.GetLength(1);
            int[] work = (int[])availabeMatrx.Clone();
            bool[] isAllocated = new bool[numberOfProcesses];

            int count = 0;
            while (count < numberOfProcesses)
            {
                bool found = false;
                for (int i = 0; i < numberOfProcesses; i++)
                {
                    if (!isAllocated[i])
                    {
                        int j;
                        for (j = 0; j < numberOfResources; j++)
                        {
                            if (neededMatrix[i, j] > work[j])
                            {
                                break;
                            }
                        }

                        if (j == numberOfResources)
                        {
                            for (int k = 0; k < numberOfResources; k++)
                            {
                                work[k] += allocationMatrix[i, k];
                            }

                            safeSequence[count] = i;
                            isAllocated[i] = true;
                            found = true;
                            count++;
                        }
                    }
                }

                if (!found)
                {
                    break;
                }
            }

            return count == numberOfProcesses;
        }
        public void PrintSafeSequence()
        {

        }
        class Edge
        {
            public int X, Y, W, H, resourcePoints = 1, usedPoints;
            public List<string> arrowState = new List<string>();// to identify wether the arrow is pointing towards or coming out of this object "recieve" "provide"
            public Color clr;
            public List<Arrow> recievedArrows = new List<Arrow>();
            public List<Arrow> provideArrows = new List<Arrow>();
            public List<Arrow> moveArrows = new List<Arrow>();
        }

        class Arrow
        {
            public int srcX, srcY, dstX, dstY, type;
        }

        
    }
}













