using System;
using System.Windows.Forms;

namespace _8
{
    public partial class puzzle8 : Form
    {
        private static int startX, startY, z;
        private static State currnetState, goalState, shuffledState;
        private static Agenda agenda;
        private static System.Threading.Thread thread;
        private static System.Drawing.Color red = System.Drawing.Color.MediumVioletRed,
            green = System.Drawing.Color.Green, White = System.Drawing.Color.Transparent;
        private static System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public puzzle8(){InitializeComponent();}

        private void App_Load(object sender, EventArgs e)
        {
            int[,] Blocks = new int[3,3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Blocks[i, j] = (j + 1) + (i * 3);

            goalState = new State('G', "-End", Blocks, 0, 0);
            currnetState = new State('G', "-End", Blocks, startX, startY);
            shuffledState = new State('G', "-End", Blocks, startX, startY);
            agenda = new Agenda(currnetState);
            _showBlocks(currnetState.getBlocks());
        }
        
        private bool GOAL(State cState)
        {
            if (z >= 5000000){
                labelStatus.Text = "            Out of RAM!!!\n" + z + " state has been checked. in " + watch.ElapsedMilliseconds + "ms";
                return true;
            }
            if (cState.getWay().Length > 30){
                labelStatus.Text = "            No way to Finish!!!\n" + z + " state has been checked. in " + watch.ElapsedMilliseconds + "ms";
                return true;
            }
            int[,] cBlocks = cState.getBlocks(), gBlocks = goalState.getBlocks();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (cBlocks[i, j] != gBlocks[i, j])
                        return false;

            watch.Stop();
            labelStatus.Text = "            Ok\n" + z + " state has been checked. in " + watch.ElapsedMilliseconds + "ms";
            _showBlocks(cState.getBlocks());
            labelWay.Text = currnetState.getWay();
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(__Show));
            thread.Start();
            return true;
        }

        private void Shuffle_Click(object sender, EventArgs e){
            int[,] Blocks = new int[3,3];
            Random rand = new Random();
            bool isStandard = false;

            if (thread != null)
                if (thread.ThreadState != System.Threading.ThreadState.Stopped)
                    thread.Suspend();

            while (!isStandard)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        bool p = true;
                        int r = rand.Next(0, 10);
                        Blocks[i, j] = 0;
                        for (int k = 0; k < 3; k++)
                            for (int l = 0; l < 3; l++)
                                if (Blocks[l, k] == r)
                                    p = false;
                        if (p) Blocks[i, j] = r;
                        else j--;
                        if (r == 9 && p)
                        {
                            startX = j;
                            startY = i;
                        }
                    }
                }
                isStandard = true; ///////........ is Standard !?!?!?
            }

            currnetState = new State('.', "S", Blocks, startX, startY);
            agenda = new Agenda(currnetState);
            
            shuffledState = currnetState;
            labelStatus.Text =  "            Ok";
            labelWay.Text = "way...";

            _showColor(currnetState.getBlocks());
            _showBlocks(currnetState.getBlocks());
        }

        private void Sort_Click(object sender, EventArgs e)
        {
            if (thread != null)
                if (thread.ThreadState != System.Threading.ThreadState.Stopped)
                    thread.Suspend();

            z = 0;
            labelStatus.Text = "            Loading...";
            System.Threading.Tasks.Task.Delay(1).Wait();
            watch = System.Diagnostics.Stopwatch.StartNew();

            agenda = new Agenda(currnetState);
            
            while (!GOAL(currnetState))
            {
                int[,] Blocks = currnetState.getBlocks();
                string way = currnetState.getWay();
                char name = currnetState.getName();
                int x = currnetState.getX(), y = currnetState.getY();

                if (x < 2 && name != 'L')
                    agenda.Add('R', way, Blocks, x + 1, y);

                if (y < 2 && name != 'U')
                    agenda.Add('D', way, Blocks, x, y + 1);

                if (y > 0 && name != 'D')
                    agenda.Add('U', way, Blocks, x, y - 1);

                if (x > 0 && name != 'R')
                    agenda.Add('L', way, Blocks, x - 1, y);

                z++;
                if (way.Length <= 1) agenda.Remove(ref currnetState);
                if (!agenda.Remove(ref currnetState)) { labelStatus.Text = "            No way to Finish!!!\n" + z + " state has been checked. in " + watch.ElapsedMilliseconds + "ms"; }// break; }
            }
        }


        delegate void Setter<T>(T value);

        private void __Show()
        {
            char[] a = currnetState.getWay().ToCharArray();
            int x = startX, y = startY;

            int[,] Blocks = new int[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                   Blocks[i, j] = shuffledState.getBlocks()[i, j];
		
            for (int i = 0; i < a.Length; i++){
                _showBlocks(Blocks);
                int temp;
                switch (a[i])
                {
                    case 'R':
                        temp = Blocks[y, x];
                        Blocks[y, x] = Blocks[y, x + 1];
                        Blocks[y, x + 1] = temp;
                        x++;
                        break;
                    case 'L':
                        temp = Blocks[y, x];
                        Blocks[y, x] = Blocks[y, x - 1];
                        Blocks[y, x - 1] = temp;
                        x--;
                        break;
                    case 'U':
                        temp = Blocks[y, x];
                        Blocks[y, x] = Blocks[y - 1, x];
                        Blocks[y - 1, x] = temp;
                        y--;
                        break;
                    case 'D':
                        temp = Blocks[y, x];
                        Blocks[y, x] = Blocks[y + 1, x];
                        Blocks[y + 1, x] = temp;
                        y++;
                        break;
                    default: break;
                }
                System.Threading.Tasks.Task.Delay(1000).Wait();
            }
            _showBlocks(Blocks);
            currnetState = new State('.', "S", Blocks, startX, startY);
            agenda = new Agenda(currnetState);
            shuffledState = currnetState;        
        }


        private delegate void SetDelegate(int[,] number);
        private void _showBlocks(int[,] Blocks)
        {
            if (this.InvokeRequired)
               this.BeginInvoke(new SetDelegate(_showBlocks), new object[] { Blocks });
            
            label1.Text = (Blocks[0, 0] != 9) ? (Blocks[0, 0].ToString()) : " ";
            label2.Text = (Blocks[0, 1] != 9) ? (Blocks[0, 1].ToString()) : " ";
            label3.Text = (Blocks[0, 2] != 9) ? (Blocks[0, 2].ToString()) : " ";
            label4.Text = (Blocks[1, 0] != 9) ? (Blocks[1, 0].ToString()) : " ";
            label5.Text = (Blocks[1, 1] != 9) ? (Blocks[1, 1].ToString()) : " ";
            label6.Text = (Blocks[1, 2] != 9) ? (Blocks[1, 2].ToString()) : " ";
            label7.Text = (Blocks[2, 0] != 9) ? (Blocks[2, 0].ToString()) : " ";
            label8.Text = (Blocks[2, 1] != 9) ? (Blocks[2, 1].ToString()) : " ";
            label9.Text = (Blocks[2, 2] != 9) ? (Blocks[2, 2].ToString()) : " ";
            _showColor(Blocks);
        }

        private void _showColor(int[,] Blocks)
        {
            int[,] gBlocks = goalState.getBlocks();
            if (Blocks[0, 0] != 9) label1.BackColor = (Blocks[0, 0] == gBlocks[0, 0]) ? green : red;
            else label1.BackColor = White;
            if (Blocks[0, 1] != 9) label2.BackColor = (Blocks[0, 1] == gBlocks[0, 1]) ? green : red;
            else label2.BackColor = White;
            if (Blocks[0, 2] != 9) label3.BackColor = (Blocks[0, 2] == gBlocks[0, 2]) ? green : red;
            else label3.BackColor = White;
            if (Blocks[1, 0] != 9) label4.BackColor = (Blocks[1, 0] == gBlocks[1, 0]) ? green : red;
            else label4.BackColor = White;
            if (Blocks[1, 1] != 9) label5.BackColor = (Blocks[1, 1] == gBlocks[1, 1]) ? green : red;
            else label5.BackColor = White;
            if (Blocks[1, 2] != 9) label6.BackColor = (Blocks[1, 2] == gBlocks[1, 2]) ? green : red;
            else label6.BackColor = White;
            if (Blocks[2, 0] != 9) label7.BackColor = (Blocks[2, 0] == gBlocks[2, 0]) ? green : red;
            else label7.BackColor = White;
            if (Blocks[2, 1] != 9) label8.BackColor = (Blocks[2, 1] == gBlocks[2, 1]) ? green : red;
            else label8.BackColor = White;
            if (Blocks[2, 2] != 9) label9.BackColor = (Blocks[2, 2] == gBlocks[2, 2]) ? green : red;
            else label9.BackColor = White;

        }


    }


}
