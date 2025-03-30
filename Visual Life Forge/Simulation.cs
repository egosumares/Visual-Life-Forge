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
                    predator.FindNearestConsumer(this);
                }
            }
            foreach (var consumer in testConsumers)
            {
                if (consumer.consumerOrganism.healthTrue <= 0) { consumersCount++; }
                else
                {
                    double value = consumer.consumerOrganism.healthTrue;
                    consumer.consumerOrganism.FindNearestFood(Grid);
                    // if the health value has incremented and it hasn't moved, it has eaten the food
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
            while (Grid.foods.Count < Grid.gridSize)
            {
                Grid.AddFood();
            }


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
