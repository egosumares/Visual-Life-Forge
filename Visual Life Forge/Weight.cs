using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    internal class Weight
    {
        public double strength;

        public Weight()
        {
            Random rnd = new Random();
            // readjusting this value to get nice, varied neural networks.
            strength = (rnd.NextDouble() * 2) - 0.5;

        }
    }
}

