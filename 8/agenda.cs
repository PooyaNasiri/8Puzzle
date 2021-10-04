using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8
{
    internal class Agenda
    {
        private State front, rear;
        internal Agenda(State currnetState)
        {
            front = currnetState;
            rear = currnetState;
            front.next = rear;
        }
        internal bool Add(char name, string way, int[,] Block, int x, int y)
        {
            State s = new State(name, way, Block, x, y);
            rear.next = s;
            rear = s;
            return true;
        }
        internal bool Remove(ref State item)
        {
            if (front == null) return false;
            item = front;
            if (front == rear) rear = front = null;
            else front = front.next;
            return true;
        }
    }
}