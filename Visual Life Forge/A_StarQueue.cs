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
            priorities = new List<double>();
        }

        public void Enqueue(Position item, double priority)
        {
            for (int i = 0; i < priorities.Count; i++)
            {
                if (priority < priorities[i])
                {
                    positions.Insert(i, item);
                    priorities.Insert(i, priority);
                    break;
                }else if (priority == priorities[i])
                {
                    positions.Insert(i, item);
                    priorities.Insert(i, priority); break;
                }
                else if (i == priorities.Count-1)
                {
                    positions.Insert(i, item);
                    priorities.Insert(i, priority);
                    break;
                }
            }
            // if nothing in either lists, the above for loop won't run and so nothing will be added
            if (priorities.Count == 0)
            {
                priorities.Add(priority);
                positions.Add(item);
            }

        }

        public void Dequeue()
        {
            priorities.RemoveAt(0);
            positions.RemoveAt(0);
        }
    }
}
