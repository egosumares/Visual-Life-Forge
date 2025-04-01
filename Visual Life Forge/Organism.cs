using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Visual_Life_Forge
{
    internal class Organism
    {
        // public double vision;
        public double visionTrue;
        // private double jitter;
        public double totalHealth;
        // public double speed;
        // private (double, double) coordinate;
        public Position organismPosition;
        public double jitterTrue;
        public double healthTrue;
        public double speedTrue;
        // if the organism has a higher strength than another, it can 'bully' it off a spot. 
        public double strengthTrue;
        public List<List<Neuron>> Layers;
        public List<int> layerSizes;
        public int layerCount;
        public int age;
        // get a list of names, choose one at random, add a number, then use this same base for its children in the simulation.

        public List<double> genome;

        public Organism(Grid g, string NeuralNetworkTest)
        {
            genome = new List<double>();
            Random random = new Random();

            Layers = new List<List<Neuron>>();
            int randomNumber3 = random.Next(10);
            int randomNumber2 = random.Next(10);
            organismPosition = new Position(randomNumber3, randomNumber2);
            layerSizes = new List<int>();
            layerCount = 4;
            layerSizes.Add(8);
            layerSizes.Add(8);
            layerSizes.Add(8);
            layerSizes.Add(3);
            for (int i = 0; i < layerCount; i++)
            {
                List<Neuron> Layer = new List<Neuron>();
                for (int j = 0; j < layerSizes[i]; j++)
                {
                    List<Weight> Weights = new List<Weight>();
                    double bias = random.NextDouble();
                    if (i + 1 < layerCount)
                    {
                        Weights = CreateWeights(layerSizes[i + 1]);
                    }
                    else
                    {
                        Weights = CreateWeights(0);
                    }
                    Neuron neuron1 = new Neuron(Weights, bias, j, i);
                    Layer.Add(neuron1);
                }

                Layers.Add(Layer);

            }
            List<double> inputs = new List<double>();
            for (int i = 0; i < layerSizes[0]; i++)
            {
                double randomNumber = random.NextDouble() * 10;
                genome.Add(randomNumber);

            }

            List<double> attributes = StatFinder(4, genome);
           // jitterTrue = attributes[0];
            healthTrue = attributes[0];
            if (healthTrue < 0) { healthTrue = 1; }
            //speedTrue = attributes[2];
           // if (speedTrue < 0) speedTrue = 1;
            visionTrue = attributes[1];
            if (visionTrue < 0) visionTrue = 1;
            strengthTrue = attributes[2];
            if (strengthTrue < 0) strengthTrue = 1;

        }

        public Organism(Grid g, string NeuralNetworkTest, List<double> genome, List<List<Neuron>> neuralNetwork, double mutationRate)
        {
            // THIS COULD BE moved to another function then called?
            Random rnd = new Random();
            // randomly manipulate the weights within the layer of the 
            this.genome = genome;
            foreach (List<Neuron> layer in neuralNetwork)
            {
                foreach (Neuron neuron in layer)
                {
                    foreach (Weight weight in neuron.weights)
                    {
                        double randomAdjuster = rnd.NextDouble() * 2 - 1;
                        randomAdjuster *= mutationRate;
                        weight.strength += randomAdjuster;
                    }
                }
            }
            foreach (double gene in genome.ToList())
            {
                double randomAdjuster = rnd.NextDouble() * 2 - 1;
                randomAdjuster *= mutationRate;
                genome[genome.IndexOf(gene)] += (randomAdjuster * gene / 10); // this makes the mutation greater proportional to the size of the gene value

            }

            Random random = new Random();

            Layers = new List<List<Neuron>>();

            organismPosition = new Position((g.gridSize / 2), (g.gridSize / 2));
            layerSizes = new List<int>();
            layerCount = 3;
            layerSizes.Add(9);
            layerSizes.Add(8);
            layerSizes.Add(4);
            for (int i = 0; i < layerCount; i++)
            {
                List<Neuron> Layer = new List<Neuron>();
                for (int j = 0; j < layerSizes[i]; j++)
                {
                    List<Weight> Weights = new List<Weight>();
                    double bias = random.NextDouble();
                    if (i + 1 < layerCount)
                    {
                        Weights = CreateWeights(layerSizes[i + 1]);
                    }
                    else
                    {
                        Weights = CreateWeights(0);
                    }
                    Neuron neuron1 = new Neuron(Weights, bias, j, i);
                    Layer.Add(neuron1);
                }

                Layers.Add(Layer);

            }
            List<double> inputs = new List<double>();
            for (int i = 0; i < layerSizes[0]; i++)
            {
                double randomNumber = random.NextDouble() * 10;
                genome.Add(randomNumber);

            }

            List<double> attributes = StatFinder(4, genome);
            jitterTrue = attributes[0];
            if (jitterTrue < 0)
            { jitterTrue = 0; }
            healthTrue = attributes[1];
            if (healthTrue < 0) { healthTrue = 0; }
            speedTrue = attributes[2];
            if (speedTrue < 0) speedTrue = 0;
            visionTrue = attributes[3];
            if (visionTrue < 0) visionTrue = 0;
        }


        public void DebugStats(
            List<Food> foods, Grid g)

        {
            // coordinate = (g.gridSize / 2, g.gridSize / 2);
            Console.WriteLine($"ORGANISM: jitter: {Math.Round(jitterTrue, 2)} \n " +
                $"health: {Math.Round(healthTrue, 2)} \n " +
                $"speed: {Math.Round(speedTrue, 2)} \n" +
               // $"Starting Co-ordinates: ({Math.Round(coordinate.Item1, 2)}, {Math.Round(coordinate.Item2, 2)})" +
                $"Vision: {visionTrue}");
            int count = 0;
            foreach (List<Neuron> layer in Layers)
            {

                foreach (Neuron neuron in layer)
                {
                    Console.WriteLine($"In Layer {count}, the neuron with name {neuron.name} is here.");

                }
                count++;
            }
        }

        // the integer nextlayer size will determine how many weights the neurnon will have. 
        public List<Weight> CreateWeights(int nextLayerSize)
        {
            Random rnd = new Random();
            List<Weight> NeuronWeights = new List<Weight>();
            for (int i = 0; i < nextLayerSize; i++)
            {
                Weight weight = new Weight();
                NeuronWeights.Add(weight);

            }
            return NeuronWeights;

            // this will create the number of weights that a single neuron will require, based on how many weights there are in the subsequent layer. 

        }

        public void Movement(List<Food> foods, Grid g)
        {
            //REDUNDANT
            if (healthTrue <= 0)
            {
                healthTrue = 0;
                Console.WriteLine("The organism's health is 0 and is no longer alive.");

            }
            else
            {

                healthTrue = healthTrue - 2;
                Random random = new Random();
                List<double> distances = new List<double>();
                List<(double, double)> values = new List<(double, double)>();
                foreach (Food food in foods)
                {
                    values.Add(food.foodCoordinate);
                }

                foreach (var value in values)
                {
               //     double distance = Math.Pow(Math.Pow(value.Item1 - coordinate.Item1, 2) + Math.Pow(value.Item2 - coordinate.Item2, 2), 0.5);
              //      distances.Add(distance);

                }
                double minimumDistance = distances.Min();
                int minimumIndex = distances.IndexOf(minimumDistance);
                if (visionTrue > minimumDistance)
                {
                    // GET THIS CODE BACK FOR LEGACY CODE? PUT IN DESIGN OR SOMETHING?
               //     double angle = ((random.NextDouble() * 2 - 1) * jitter) + (((Math.Atan2(values[minimumIndex].Item1 - coordinate.Item1, values[minimumIndex].Item2 - coordinate.Item2))) / 3.141593) * 180;

                    // this gives a value between -180 and 180 about which angle the organism should go. 
                  //  if (0 <= angle && angle < 90)
                    {
               //         coordinate.Item1 = coordinate.Item1 + speedTrue * Math.Cos((angle) / 180 * Math.PI);
               //         coordinate.Item2 = coordinate.Item2 + (speedTrue * Math.Sin((angle) / 180 * Math.PI) * -1);
                    }
                //    else if (90 <= angle && angle < 180)
                //    {
               //         coordinate.Item1 = coordinate.Item1 + speedTrue * Math.Cos((angle) / 180 * Math.PI);
             //           coordinate.Item2 = coordinate.Item2 + speedTrue * Math.Sin((angle) / 180 * Math.PI);
                    }
                 //   else if (-90 <= angle && angle < 0)
                  //  {
                 //       coordinate.Item1 = coordinate.Item1 + (speedTrue * Math.Cos((angle) / 180 * Math.PI) * -1);
                  //      coordinate.Item2 = coordinate.Item2 + (speedTrue * Math.Sin((angle) / 180 * Math.PI) * -1);
                //    }
                    else
                    {
                 //       coordinate.Item1 = coordinate.Item1 + (speedTrue * Math.Cos((angle) / 180 * Math.PI) * -1);
                 //       coordinate.Item2 = coordinate.Item2 + speedTrue * Math.Sin((angle) / 180 * Math.PI);
                    }

               // }
               // else
              //  {
              //      coordinate.Item1 = coordinate.Item1 + (speedTrue * Math.Cos((random.NextDouble() * 360) / 180 * Math.PI));
              //      coordinate.Item2 = coordinate.Item2 + speedTrue * Math.Sin((random.NextDouble() * 360) / 180 * Math.PI);
               // }
              ///  double values1 = coordinate.Item1;
              //  double values2 = coordinate.Item2;
               // if (coordinate.Item1 < 0)
              //  {
              //      coordinate.Item1 = 0;
              //  }
               // if (coordinate.Item2 < 0) { coordinate.Item2 = 0; }
              //  else if (coordinate.Item1 > 10)
              //  {
              //      coordinate.Item1 = 10;
              //  }
               // if (coordinate.Item2 > 10) { coordinate.Item2 = 10; }
                //Console.WriteLine($"The new organism's co-ordinate is (({Math.Round(coordinate.Item1, 2)}, {Math.Round(coordinate.Item2, 2)}))");

               // double angle2 = ((random.NextDouble() * 2 - 1) * jitter) + ((Math.Atan2(values[minimumIndex].Item1 - coordinate.Item1, values[minimumIndex].Item2 - coordinate.Item2) / 3.141593) * 180);
               // double speechAngle = angle2;
              //  if (speechAngle < 0)
              //  {
              //      speechAngle += 360;
             //   }
              //  Console.WriteLine($"Found food at an angle of {Math.Round(speechAngle, 2)} degrees to the organism. ");
              //  double testAngle = ((random.NextDouble() * 2 - 1) * jitter) + ((Math.Atan2(values[minimumIndex].Item1 - coordinate.Item1, values[minimumIndex].Item2 - coordinate.Item2) / 3.141593) * 180);
              // if (angle2 < 0)
              //  {
              //      angle2 += 180;
              //  }
             //   if (angle2 <= 0)
             //   {
             //testAngle += 180;
                // }
                // if ((angle2 - testAngle) <= 5 && (angle2 - testAngle) >= -5)
                //{
                    // here i want the energy of the organism to go up, and the food to basically disappear. 
                    // I need to remove its co-ordinate from the grid's list of foods, then remove the food item. 
                    // add the energy of the food
                  //  healthTrue = healthTrue + g.foods[minimumIndex].energy;
                 //   Console.WriteLine($"The food at index {minimumIndex} has been removed and the organism has gained {g.foods[minimumIndex].energy} to have a new health of {Math.Round(healthTrue, 2)}");
                  //  g.foods.RemoveAt(minimumIndex);


               // }


            }

        }

        // here I will code the A* pathfinding using the grid structure.
        // here i need the grid to work with foods and obstacles.

        public void EatFood(Grid g)
        {
            healthTrue++;
        }
        public void FindNearestFood(Grid g)
        {
            List<double> distances = new List<double>();
            List<double> orderedDistanes = new List<double>();
            foreach (Food food in g.foods)
            {
                double distanceSquared = Math.Pow(food.foodPosition.posCoordinate.Item1 - organismPosition.posCoordinate.Item1, 2) + Math.Pow(food.foodPosition.posCoordinate.Item2 - organismPosition.posCoordinate.Item2, 2);
                double distance = Math.Pow(distanceSquared, 0.5);
                distances.Add(distance);
            }
            double minDistance = distances.Min();
            int index = distances.IndexOf(minDistance);
            if (minDistance == 0)
            {
                EatFood(g); return;
            }
            if (minDistance < visionTrue)
            {
                Pathfinding(g, g.foods[index].foodPosition);
            }
            else
            {
                List<Position> positions = g.AdjacentCells(organismPosition);
                bool shouldBreak = false;
                foreach (Position position in positions)
                {
                    foreach (var obstacle in g.obstacles)
                    {
                        // this is occupied by an obstacle so that cell can't be occupied at all. 
                        if (obstacle.obstaclePosition.posCoordinate == position.posCoordinate) { positions.Remove(position); shouldBreak = true; break; }
                    }
                    if (shouldBreak) { break; }

                }
                Random rnd = new Random();
                int positionIndex = rnd.Next(positions.Count);
                organismPosition = positions[positionIndex];
                healthTrue -= 1;
            }
        }
        private void Pathfinding(Grid g, Position goalNode)
        {
            // goalNode refers to the node that the food should try and reach.
            // the queue that will hold all of the positions and their estimated costs. 
            A_StarQueue mainQueue = new A_StarQueue();


            // costs will be the manhattan distance, and then the euclidean distance
            if (healthTrue <= 0)
            {
                healthTrue = 0;
                Console.WriteLine("The organism's health is 0 and is no longer alive.");

            }
            else
            {
                Dictionary<(int, int), (int, int)> cameFrom = new Dictionary<(int, int), (int, int)>();
                int goalPositionIndex = 0;
                int indexOfStart = 0;
                double StartCost = 0;
                foreach (Position position in g.gridPositions)
                {



                    // maybe the two lines below shouldn't be in this exact position?
                    double euclideanCostSquared = Math.Pow(Math.Abs(position.posCoordinate.Item1 - goalNode.posCoordinate.Item1), 2) + Math.Pow(Math.Abs(position.posCoordinate.Item2 - goalNode.posCoordinate.Item2), 2);
                    double heuristic = Math.Pow(euclideanCostSquared, 0.5);

                    if (position.posCoordinate == organismPosition.posCoordinate)
                    {
                        indexOfStart = g.gridPositions.IndexOf(position);
                        StartCost = heuristic;
                    }
                    if (goalNode.posCoordinate == position.posCoordinate)
                    {

                        goalPositionIndex = g.gridPositions.IndexOf(position);
                    }

                }

                mainQueue.Enqueue(g.gridPositions[indexOfStart], StartCost);

                Position previousNode = g.gridPositions[indexOfStart];
                bool MainNoHasEnd = true;
                List<Position> visitedNodes = new List<Position>();
                while (MainNoHasEnd)
                {
                    foreach (Position visitedNode in visitedNodes)
                    {
                        foreach (Position position in mainQueue.positions)
                        {
                            if (visitedNode.posCoordinate == position.posCoordinate) { mainQueue.Dequeue(); }
                        }
                    }
                    
                    List<Position> adjacentCells = g.AdjacentCells(mainQueue.positions[0]);
                    mainQueue.Dequeue();
                    foreach (var cell in adjacentCells)
                    {
                        double cost = 0;
                        foreach (var obstacle in g.obstacles)
                        {
                            if (cell.posCoordinate == obstacle.obstaclePosition.posCoordinate)
                            {
                                cost = int.MaxValue;
                            }
                        }
                        //changes
                        double euclideanCostSquared = Math.Pow(Math.Abs(cell.posCoordinate.Item1 - goalNode.posCoordinate.Item1), 2) + Math.Pow(Math.Abs(cell.posCoordinate.Item2 - goalNode.posCoordinate.Item2), 2);
                        double heuristic = Math.Pow(euclideanCostSquared, 0.5);

                        mainQueue.Enqueue(cell, cost + heuristic);
                    }
                    int check = 66; 

                   cameFrom.Add(previousNode.posCoordinate, mainQueue.positions[0].posCoordinate);

                    foreach (var position in mainQueue.positions)
                    {
                        if (position.posCoordinate == g.gridPositions[goalPositionIndex].posCoordinate)
                        {
                            MainNoHasEnd = false;
                        }

                    }
                    previousNode = mainQueue.positions[0];
                }
                Position orgPos = organismPosition;
                (int, int) current = goalNode.posCoordinate;
                List<(int, int)> finalPath = new List<(int, int)>();

                MoveOrganism(cameFrom[organismPosition.posCoordinate]);
                // finalPath should contin the 
            }
        }


        public void MoveOrganism((int, int) pos)
        {
            organismPosition.posCoordinate = pos;
            healthTrue--;
        }
        public List<Weight> FindWeights(ref Neuron neuron1)
        {
            List<Weight> weights = new List<Weight>();
            if (neuron1.layer != 0)
            {
                foreach (Neuron neuronHere in Layers[neuron1.layer - 1])
                {
                    int count = 0;
                    foreach (Weight weight in neuronHere.weights)
                    {
                        if (count == neuron1.index) // this doesn't work, because the end neuron is null
                        {
                            weights.Add(weight);
                        }
                        count++;
                    }
                }
            }
            return weights;
        }



        // i have the weights covered but I also need to take in the input. 

        // for genome size, this will be equal to the number of input neurons in the input layer
        public List<double> StatFinder(int genomeSize, List<double> startInputs)
        {
            // get the starting value of each neuron. 
            // input neurons get the input then apply their bias, that's 
            Random random = new Random();
            // List<double> startInputs = new List<double>();
            //for (int i = 0; i < genomeSize; i++)
            // {
            // startInputs.Add(random.NextDouble());
            // }
            int startLayer = 1;
            List<double> intermediaryOutput = new List<double>();
            List<double> output = LayerValueTransfer(startInputs, startLayer);
            // this start bit doesn't make much sense. Maybe, make a separate method for just calculating the output for the input neurons before moving on. 
            startLayer++;
            foreach (double outpu in output)
            {
                intermediaryOutput.Add(outpu);
            }
            while (startLayer <= Layers.Count - 1)
            {
                List<double> outputs = LayerValueTransfer(intermediaryOutput, startLayer);
                intermediaryOutput.Clear();
                foreach (double outputt in outputs)
                {
                    intermediaryOutput.Add(outputt);
                }

                startLayer++;
            }
            // this should give the final values that are needed. 
            return intermediaryOutput;


        }
        // this will be a function intended to transfer the inputs between the layers of neurons based on the weights etc. 
        public List<double> LayerValueTransfer(List<double> input, int LAYER)
        {
            // this needs to handle several different conditions. for example, when the input is the input values of the whole network
            // also, for output layers, things have to be different. 
            List<double> output = new List<double>();
            for (int i = 0; i < layerSizes[LAYER]; i++)
            {
                // need inputs of double value and then the neuron that it's coming from 
                Neuron inputNeuron = Layers[LAYER][i];
                List<Weight> inputWeights = FindWeights(ref inputNeuron);
                output.Add(Layers[LAYER][i].CalculateOutput(inputWeights, input));
            }


            return output;
        }
    }
}
