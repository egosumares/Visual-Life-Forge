using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    internal class Consumer
    {
        public Organism consumerOrganism;
        // when true, the organism shouldn't even think of moving.
        public bool attacked;
        public string name;
        public Generator generator;

        public Consumer()
        {
            generator = new Generator();
            name = generator.CreateName();
        }

        public void Debug()
        {
            double speedRounded = Math.Round(consumerOrganism.speedTrue, 2);
            double healthRounded = Math.Round(consumerOrganism.healthTrue, 2);
            double visionRounded = Math.Round(consumerOrganism.visionTrue, 2);
            double jitterRounded = Math.Round(consumerOrganism.jitterTrue, 2);
            Console.WriteLine($"The organism's new coordinates are ({consumerOrganism.organismPosition.posCoordinate.Item1}, {consumerOrganism.organismPosition.posCoordinate.Item2})");
            Console.WriteLine($"Organism Name: {name}");
            Console.WriteLine($"Speed: {speedRounded}");
            Console.WriteLine($"Health: {healthRounded}");
            Console.WriteLine($"Vision: {visionRounded}");
            Console.WriteLine($"Jitter: {jitterRounded}");
        }
        // I want a RunFromPredator() function. 
        // Modify A* to make the heuristic cost just the distance of the node from the predator as it moves, just continue to make it run away
        // FUSE run from predator and find food function to make sure that organisms run from attackers while trying to get food too.
        public void RunFromPredator(Predator predator, Grid g)
        {
            A_StarQueue mainQueue = new A_StarQueue();


            // costs will be the manhattan distance, and then the euclidean distance
            if (consumerOrganism.healthTrue <= 0)
            {
                consumerOrganism.healthTrue = 0;
                Console.WriteLine("The organism's health is 0 and is no longer alive.");

            }
            else
            {
                // add a timer so just after a set amount of time, you have a certain route from the queue. Use dictionary to form it and just make consumer take this path. 
                // if food AND predator found, a dynamic eat-and-chase method can be made?
                Dictionary<Position, Position> cameFrom = new Dictionary<Position, Position>();
                int goalPositionIndex = 0;
                int indexOfStart = 0;
                double StartCost = 0;
                foreach (Position position in g.gridPositions)
                {



                    // maybe the two lines below shouldn't be in this exact position?
                    double euclideanCostSquared = Math.Pow(Math.Abs(position.posCoordinate.Item1 - predator.baseOrganism.organismPosition.posCoordinate.Item1), 2) + Math.Pow(Math.Abs(position.posCoordinate.Item2 - predator.baseOrganism.organismPosition.posCoordinate.Item2), 2);
                    double heuristic = (-1) * Math.Pow(euclideanCostSquared, 0.5);

                    if (consumerOrganism.organismPosition.posCoordinate == position.posCoordinate)
                    {
                        indexOfStart = g.gridPositions.IndexOf(position);
                        StartCost = heuristic;
                    }
                    //if (goalNode.posCoordinate.Item1 == position.posCoordinate.Item1 && position.posCoordinate.Item2 == goalNode.posCoordinate.Item2)
                    // {
                    //    goalPositionIndex = g.gridPositions.IndexOf(position);
                    //  }

                }

                mainQueue.Enqueue(g.gridPositions[indexOfStart], StartCost);

                Position previousNode = g.gridPositions[goalPositionIndex];
                int count = 0;
                while (!mainQueue.positions.Contains(g.gridPositions[goalPositionIndex]) && count < 15)
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
                        double euclideanCostSquared = Math.Pow(Math.Abs(cell.posCoordinate.Item1 - predator.baseOrganism.organismPosition.posCoordinate.Item1), 2) + Math.Pow(Math.Abs(cell.posCoordinate.Item2 - predator.baseOrganism.organismPosition.posCoordinate.Item2), 2);
                        double heuristic = Math.Pow(euclideanCostSquared, 0.5);

                        mainQueue.Enqueue(cell, cost + heuristic);
                    }

                    // this might not just trace back to the original node. Might not work that way
                    cameFrom.Add(mainQueue.positions[0], previousNode);

                }
                var lastPair = cameFrom.LastOrDefault();
                Position current = lastPair.Key;
                List<Position> finalPath = new List<Position>();
                while (current != g.gridPositions[indexOfStart])
                {
                    finalPath.Insert(finalPath.Count - 1, current);
                    current = cameFrom[current];

                }
            }
        }
    }
}
