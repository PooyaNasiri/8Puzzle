using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8
{
    internal class State
    {
        internal State next;
        private char name;
        private string way;
        private int x, y;
        private int[,] Block;
        internal State(char name, string way, int[,] Block, int x, int y)
        {
            this.Block = new int[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    this.Block[i, j] = Block[i, j];
            int temp;
            switch (name)
            {
                case 'R':
                    temp = this.Block[y, x];
                    this.Block[y, x] = this.Block[y, x - 1];
                    this.Block[y, x - 1] = temp;
                    break;
                case 'L':
                    temp = this.Block[y, x];
                    this.Block[y, x] = this.Block[y, x + 1];
                    this.Block[y, x + 1] = temp;
                    break;
                case 'U':
                    temp = this.Block[y, x];
                    this.Block[y, x] = this.Block[y + 1, x];
                    this.Block[y + 1, x] = temp;
                    break;
                case 'D':
                    temp = this.Block[y, x];
                    this.Block[y, x] = this.Block[y - 1, x];
                    this.Block[y - 1, x] = temp;
                    break;
                default: break;
            }
            this.name = name;
            this.way = way + name;
            this.x = x;
            this.y = y;
        }
        internal string getWay()
        {
            return way;
        }
        internal char getName()
        {
            return name;
        }
        internal int getX()
        {
            return x;
        }
        internal int getY()
        {
            return y;
        }
        internal int[,] getBlocks()
        {
            return this.Block;
        }

    }
}
