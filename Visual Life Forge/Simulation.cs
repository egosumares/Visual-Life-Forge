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
            this.testPredators = Predators;
            testConsumers = consumers;
            Grid = g;
            Grid.obstacles = new List<Obstacle>();
            NewSimulation();

        }


        public void NewSimulation()
        {
            AddObstacles();

            // why is nothing happening?
            //_timer = new System.Timers.Timer(150);
            _timer.Elapsed += RunSimulationOnce;

            _timer.Enabled = true;

            Console.ReadLine();
        }

        public void Tick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Tick");
        }
        public void RunSimulationOnce(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("abcd");
            DrawScreen();
            tickCount++;
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
                    consumer.consumerOrganism.FindNearestFood(Grid);
                }
            }
            while (Grid.foods.Count < Grid.gridSize)
            {
                Grid.AddFood();
            }
            if (predCount == testPredators.Count || consumersCount == testConsumers.Count)
            {
                _timer.Stop(); return;
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
