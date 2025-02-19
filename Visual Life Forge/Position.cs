using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    internal class Position
    {
        public (int, int) posCoordinate;
        public Position(int x, int y)
        {
            posCoordinate = (x, y);
        }
    }
}
