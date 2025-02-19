using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    internal class A_StarQueue
    {
        public List<Position> positions;
        public List<double> priorities;
        public A_StarQueue()
        {
            positions = new List<Position>();
        }

        public void Enqueue(Position item, double priority)
        {
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                if (priorities[i] > priority)
                {
                    positions.Insert(i, item);
                    priorities.Insert(i, priority);
                    break;
                }
                else if (i == 0)
                {
                    positions.Insert(i, item);
                    priorities.Insert(i, priority);
                }
            }

        }

        public void Dequeue()
        {
            priorities.RemoveAt(0);
            positions.RemoveAt(0);
        }
    }
}
