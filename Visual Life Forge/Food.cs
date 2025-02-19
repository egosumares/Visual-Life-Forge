using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    class Food
    {
        public int energy;
        private int duration;
        public (double, double) foodCoordinate;
        public Position foodPosition;

        public Food(Position coordinate)
        {
            energy = 3;
            duration = 10;
            Random random = new Random();
            //foodCoordinate.Item1 = (Math.Round(random.NextDouble() * 1000))/100 ;
            //foodCoordinate.Item2 = (Math.Round(random.NextDouble()* 1000))/100;
            foodPosition = coordinate;
        }

    }
}
