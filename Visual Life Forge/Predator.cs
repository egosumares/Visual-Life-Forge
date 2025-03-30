using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    internal class Predator
    {
        public Organism baseOrganism;
        // the attackStrength will be drawn from one of the base stats, SPEED
        double attackStrength;
        public string name;
        public Generator generator;

        // I want a ChaseConsumer() function where it chases consumers that are close to it. However, if it finds another that is closer, it should go for that one instead. Since predators eat only consumers,
        // that should really be all that it does

        public Predator()
        {
            generator = new Generator();
            name = generator.CreateName();
        }

        public void Debug()
        {
            double speedRounded = Math.Round(baseOrganism.speedTrue, 2);
            double healthRounded = Math.Round(baseOrganism.healthTrue, 2);
            double visionRounded = Math.Round(baseOrganism.visionTrue, 2);
            double jitterRounded = Math.Round(baseOrganism.jitterTrue, 2);
            Console.WriteLine($"The organism's new coordinates are ({baseOrganism.organismPosition.posCoordinate.Item1}, {baseOrganism.organismPosition.posCoordinate.Item2})");
            Console.WriteLine($"Organism Name: {name}");
            Console.WriteLine($"Speed: {speedRounded}");
            Console.WriteLine($"Health: {healthRounded}");
            Console.WriteLine($"Vision: {visionRounded}");
            Console.WriteLine($"Jitter: {jitterRounded}");
        }
        public void FindNearestConsumer(Simulation simulation)
        {
            // use simulation's list of organisms
            //  List<double> distances = new List<double>();
            // List<double> orderedDistanes = new List<double>();
            //   foreach (Organism consumer in simulation.testOrganisms)
            //  {
            //      double distanceSquared = Math.Pow(consumer.consumerOrganism.foodPosition.posCoordinate.Item1 - organismPosition.posCoordinate.Item1, 2) + Math.Pow(food.foodPosition.posCoordinate.Item2 - organismPosition.posCoordinate.Item2, 2);
            //      double distance = Math.Pow(distanceSquared, 2);
            //      distances.Add(distance);
            // }
            //  double minDistance = distances.Min();
            //  int index = distances.IndexOf(minDistance);
            //   if (minDistance < vision)
            //  {
            //      Pathfinding(g, g.foods[index].foodPosition);
            // }

            List<double> distances = new List<double>();
            List<double> orderedDistances = new List<double>();
            foreach (Consumer consumer in simulation.testConsumers)
            {
                double distanceSquared = Math.Pow(consumer.consumerOrganism.organismPosition.posCoordinate.Item1 - baseOrganism.organismPosition.posCoordinate.Item1, 2) + Math.Pow(consumer.consumerOrganism.organismPosition.posCoordinate.Item2 - baseOrganism.organismPosition.posCoordinate.Item2, 2);
                double distance = Math.Pow(distanceSquared, 0.5);
                distances.Add(distance);
            }
            double minDistance = distances.Min();
            int index = distances.IndexOf(minDistance);
            if (minDistance < baseOrganism.vision)
            {
                ChasePrey(simulation.testConsumers[index], simulation.Grid);
                // maybe use bool to call chase prey function in the simulation class?
            }
            else
            {
                List<Position> positions = simulation.Grid.AdjacentCells(baseOrganism.organismPosition);
                for (int i = 0; i < positions.Count - 1; i++)
                {
                    foreach (var obstacle in simulation.Grid.obstacles)
                    {
                        // this is occupied by an obstacle so that cell can't be occupied at all. 
                        if (obstacle.obstaclePosition == positions[i]) positions.Remove(positions[i]);
                    }
                }
                Random rnd = new Random();
                int positionIndex = rnd.Next(positions.Count);

                baseOrganism.organismPosition = positions[positionIndex];
                baseOrganism.healthTrue -= 1;
            }
        }

        public void ChasePrey(Consumer victim, Grid g)
        {
            A_StarQueue mainQueue = new A_StarQueue();


            // costs will be the manhattan distance, and then the euclidean distance
            if (baseOrganism.healthTrue <= 0)
            {
                baseOrganism.healthTrue = 0;
                Console.WriteLine("The organism's health is 0 and is no longer alive.");

            }
            else
            {
                Dictionary<Position, Position> cameFrom = new Dictionary<Position, Position>();
                int goalPositionIndex = 0;
                int indexOfStart = 0;
                double StartCost = 0;
                foreach (Position position in g.gridPositions)
                {



                    // maybe the two lines below shouldn't be in this exact position?
                    double euclideanCostSquared = Math.Pow(Math.Abs(position.posCoordinate.Item1 - victim.consumerOrganism.organismPosition.posCoordinate.Item1), 2) + Math.Pow(Math.Abs(position.posCoordinate.Item2 - victim.consumerOrganism.organismPosition.posCoordinate.Item2), 2);
                    double heuristic = Math.Pow(euclideanCostSquared, 0.5);

                    if (baseOrganism.organismPosition.posCoordinate.Item1 == position.posCoordinate.Item1 && position.posCoordinate.Item2 == baseOrganism.organismPosition.posCoordinate.Item2)
                    {
                        indexOfStart = g.gridPositions.IndexOf(position);
                        StartCost = heuristic;
                    }
                    if (victim.consumerOrganism.organismPosition.posCoordinate.Item1 == position.posCoordinate.Item1 && position.posCoordinate.Item2 == victim.consumerOrganism.organismPosition.posCoordinate.Item2)
                    {
                        goalPositionIndex = g.gridPositions.IndexOf(position);
                    }

                }

                mainQueue.Enqueue(g.gridPositions[indexOfStart], StartCost);

                Position previousNode = g.gridPositions[indexOfStart];
                while (!mainQueue.positions.Contains(g.gridPositions[goalPositionIndex]))
                {
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
                        double euclideanCostSquared = Math.Pow(Math.Abs(cell.posCoordinate.Item1 - victim.consumerOrganism.organismPosition.posCoordinate.Item1), 2) + Math.Pow(Math.Abs(cell.posCoordinate.Item2 - victim.consumerOrganism.organismPosition.posCoordinate.Item2), 2);
                        double heuristic = Math.Pow(euclideanCostSquared, 0.5);

                        mainQueue.Enqueue(cell, cost + heuristic);
                    }
                    cameFrom.Add(mainQueue.positions[0], previousNode);
                }
                Position current = victim.consumerOrganism.organismPosition;
                List<Position> finalPath = new List<Position>();
                while (current != g.gridPositions[indexOfStart])
                {
                    finalPath.Insert(finalPath.Count - 1, current);
                    current = cameFrom[current];

                }

                // this should be a modified version of A* that makes the consumer the target for chasing.
            }
        }
        public void Attack(Consumer victim)
        {
            // this function should decrease the health of the victim, but of the actual victim, not just this consumer here. apparently, passing classes is reference by default. YAY!
            // the predator also loses energy from attacking
            victim.consumerOrganism.healthTrue -= attackStrength;
            baseOrganism.healthTrue -= attackStrength / 4;
        }


    }
}

