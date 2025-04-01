using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Visual_Life_Forge
{
    internal class Simulation
    {
        public Organism testOrganism;
        public List<Predator> testPredators;
        public List<Consumer> testConsumers;
        public Grid Grid;
        private double Score;
        public int tickCount;
        public int gridSize;
        public bool loopInitiated;
        private System.Timers.Timer _timer = new System.Timers.Timer(150);
        // I'm gonna need to make a BIG debugstats function here. 
        public Simulation(Organism org, Grid g)
        {
            testOrganism = org;
            Grid = g;
            NewSimulation();
        }

        public Simulation(List<Predator> Predators, List<Consumer> consumers, Grid g)
        {
            Random rnd = new Random();
            this.testPredators = Predators;
            testConsumers = consumers;
            // ORDERING LISTS BY STRENGTH NOW
            testPredators = testPredators.OrderByDescending(p => p.baseOrganism.strengthTrue).ToList();
            testConsumers = testConsumers.OrderByDescending(p => p.consumerOrganism.strengthTrue).ToList();
            Grid = g;


            
            // update the organism's positions using the AVAILABLE POSITIONS
            foreach (Consumer consumer in testConsumers) 
            {
                int index = rnd.Next(Grid.availablePositions.Count);
                consumer.consumerOrganism.organismPosition = Grid.availablePositions[index];
                Grid.RemovePosition(consumer.consumerOrganism.organismPosition);
            }
            foreach (Predator predator in testPredators)
            {
                int index = rnd.Next(Grid.availablePositions.Count);
                predator.baseOrganism.organismPosition = Grid.availablePositions[index];
                Grid.RemovePosition(predator.baseOrganism.organismPosition);
            }
           // NewSimulation(); I can now control this in the GameWindow function

        }


        public void NewSimulation()
        {
            AddObstacles();

            // why is nothing happening?
            //_timer = new System.Timers.Timer(150);
            // _timer.Elapsed += RunSimulationOnce;

            _timer.Enabled = true;

            Console.ReadLine();
        }

        public void Tick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Tick");
        }
        public void RunSimulationOnce()
        {

    
            int predCount = 0;
            int consumersCount = 0;
            foreach (var predator in testPredators)
            {
                if (predator.baseOrganism.healthTrue <= 0) { predCount++; }
                else
                {
                    Position oldPosition = predator.baseOrganism.organismPosition;
                    predator.FindNearestConsumer(this);
                    Position newPosition = predator.baseOrganism.organismPosition;
                    // get the dead consumer next to the predator and remove it from the simulation. This shouldn't be too hard.
                    UpdateAvailablePositions(oldPosition, newPosition);
                    // for this stuff, how about I just pass in the simulation to the organism/predator for their pathfinding functions? 
                    // lets them know if organisms are in the way or not, it doesn't really make sense in terms of real life though. not rlly bothered for now
                    if (predator.hasEaten) 
                    {
                        Position predatorCell = predator.baseOrganism.organismPosition;
                        List<Position> adjacentCells = Grid.AdjacentCells(predatorCell);
                        List<(int, int)> adjacentCoordinates = new List<(int, int)>();
                        foreach (var pos in adjacentCells)
                        {
                            adjacentCoordinates.Add(pos.posCoordinate);
                        }
                        // finds all the coordinates adjacent to the predator
                        foreach (var consumer in testConsumers)
                        {
                           if (adjacentCoordinates.Contains(consumer.consumerOrganism.organismPosition.posCoordinate)) 
                            {
                                // remove the consumer from the list once it has been eaten
                                testConsumers.Remove(consumer); predator.hasEaten = false; break;
                            }
                        }
                    }
                }
            }
            foreach (var consumer in testConsumers)
            {
                if (consumer.consumerOrganism.healthTrue <= 0) { consumersCount++; }
                else
                {
                    Position oldPosition = consumer.consumerOrganism.organismPosition;
                    double value = consumer.consumerOrganism.healthTrue;
                    consumer.consumerOrganism.FindNearestFood(Grid);
                    Position newPosition = consumer.consumerOrganism.organismPosition;
                    // if the health value has incremented and it hasn't moved, it has eaten the food
                    UpdateAvailablePositions(oldPosition, newPosition);
                    if (consumer.consumerOrganism.healthTrue == value +1)
                    {
                        // find the food with the same coordinate
                        foreach (Food food in Grid.foods)
                        {
                            if (consumer.consumerOrganism.organismPosition.posCoordinate == food.foodPosition.posCoordinate)
                            {
                                // remove it from the grid
                                Grid.foods.Remove(food); break;
                            }
                        }
                    }
                }
            }
            // NO! JUST order testConsumers in order of strength :)

            


        }

        private void UpdateAvailablePositions(Position oldPos, Position newPos)
        {
            // (int, int) oldPosCoordinate = oldPos.posCoordinate;
            (int, int) newPosCoordinate = newPos.posCoordinate;
            foreach(var Position in Grid.availablePositions)
            {
                if (Position.posCoordinate == newPosCoordinate)
                { Grid.availablePositions.Remove(Position); break; }
            }
            Grid.availablePositions.Add(oldPos);
        }

        private void UpdateAvailablePositions(Position pos)
        {

        }
        public double DetermineScore()
        {

            return 0;
        }

        public void AddObstacles()
        {
            Grid.obstacles = new List<Obstacle>();
            List<Position> posAvailable = Grid.gridPositions.ToList();
            foreach (var food in Grid.foods)
            {
                foreach (Position position in Grid.gridPositions)
                {
                    if (food.foodPosition == position)
                    {
                        posAvailable.Remove(position);
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int index = rnd.Next(posAvailable.Count);
                Obstacle obstacle = new Obstacle(Grid.gridPositions[index]);
                // this ensures no collisions
                posAvailable.Remove(Grid.gridPositions[index]);
                Grid.obstacles.Add(obstacle);
            }
        }
        public void DrawScreen()
        {
            int[,] chars = new int[Grid.gridSize, Grid.gridSize];

            for (int i = 0; i < Grid.gridSize; i++)
            {
                for (int j = 0; j < Grid.gridSize; j++)
                {
                    chars[i, j] = '▣';
                }
            }

            // handling obstacles & organisms
            for (int i = 0; i < Grid.gridSize; i++)
            {
                for (int j = 0; j < Grid.gridSize; j++)
                {
                    foreach (var obstacle in Grid.obstacles)
                    {
                        if (obstacle.obstaclePosition.posCoordinate.Item2 == i && obstacle.obstaclePosition.posCoordinate.Item1 == j)
                        {
                            chars[i, j] = '⛝';
                        }
                    }
                    foreach (var predator in testPredators)
                    {
                        if (predator.baseOrganism.organismPosition.posCoordinate.Item1 == i && predator.baseOrganism.organismPosition.posCoordinate.Item2 == j)
                        {
                            chars[i, j] = '⚇';
                        }

                    }
                    foreach (var consumer in testConsumers)
                    {
                        if (consumer.consumerOrganism.organismPosition.posCoordinate.Item1 == i && consumer.consumerOrganism.organismPosition.posCoordinate.Item2 == j)
                        {
                            chars[i, j] = '⚪';
                        }
                    }

                    foreach (var food in Grid.foods)
                    {
                        chars[i, j] = '$';
                    }

                    Console.Write(Convert.ToString(chars[i, j]));
                }
                Console.Write('\n');
            }
        }
    }
}
