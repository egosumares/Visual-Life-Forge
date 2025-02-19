using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Visual_Life_Forge
{
    class Neuron
    {
        public List<Weight> weights;
        public double bias;
        public int layer;
        public int index;
        public string name;
        public Neuron(List<Weight> weights1, double bias, double index, int layer)
        {
            weights = [.. weights1];
            this.bias = bias;
            Random random = new Random();
            int randomNumber = random.Next(0, 1000);
            int randomNumber2 = random.Next(0, 10);
            name = $"{randomNumber2}Neuron{randomNumber}";
            // need to specify which layer it is. 
            bias = random.NextDouble() * 3 - 1.5;
            this.index = Convert.ToInt32(index);
            this.layer = layer;
        }


        // here, pass the weights that you need to apply, and then pass a list of tuples. The first value will be the double input, and the second will be the index of the neuron that it came from. 

        public double CalculateOutput(List<Weight> weights, List<double> inputs)
        {

            // here, the neuron can't take every input if it's the input neuron. It just doesn't make any sense. 
            List<Weight> weightsToApply = weights;

            if (layer == 0)
            {
                return inputs[index] + bias;
            }
            else
            {
                double output = 0;
                int count = 0;
                foreach (Weight weight in weights)
                {
                    output += inputs[count] * weight.strength;
                    count++;
                }

                return output;
            }
        }
    }
}
