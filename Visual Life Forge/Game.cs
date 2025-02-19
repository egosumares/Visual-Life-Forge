using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Visual_Life_Forge
{
    class Game
    {
        public List<Organism> Organisms;
        public List<Predator> Predators;
        public List<Consumer> Consumers;
        public Grid BaseGrid;
        public List<int> fitnesses;
        public int simulationCount;
        public double mutationRate;
        public Game()
        {
            Organisms = new List<Organism>();
            BaseGrid = new Grid(10);
            fitnesses = new List<int>();
            InstantiateOrganisms();
            StartSimulations();
            Random rnd = new Random();
            mutationRate = rnd.NextDouble();

        }

        public Game(string test)
        {
            Organisms = new List<Organism>();
            BaseGrid = new Grid(10);
            fitnesses = new List<int>();
            Random rnd = new Random();
            mutationRate = rnd.NextDouble();
            BaseGrid.obstacles = new List<Obstacle>();
            BeginSimulation();



        }

        public void InstantiateOrganisms()
        {
            for (int i = 0; i < 10; i++)
            {
                Organism jimmyTest = new Organism(BaseGrid, "string");
                Organisms.Add(jimmyTest);
            }
            simulationCount++;
            Console.WriteLine($"Simulation {simulationCount} has BEGUN.");

        }
        public void InstantiateOrganisms2()
        {
            List<Organism> twoBest = new List<Organism>();
            List<int> indexes = new List<int>();
            int highestScore = fitnesses.Max();

            int highestIndex = fitnesses.IndexOf(highestScore);
            fitnesses[highestIndex] = 0;
            indexes.Add(highestIndex);
            highestIndex = fitnesses.IndexOf(fitnesses.Max());
            indexes.Add(highestIndex);
            twoBest.Add(Organisms[indexes[0]]);
            // take the neural network of this organism and it's starting values then 
            // ISSUE: which part is the evolution going to be based on, the actual neural network itself or the genome?
            // should both of them be part of the evolution? for now, YES


            twoBest.Add(Organisms[indexes[1]]);

            double inputJitter = 0;
            double inputHealth = 0;
            double inputSpeed = 0;
            double inputVision = 0;
            // finding the best of each attribute between these two organisms
            if (twoBest[0].jitterTrue < twoBest[1].jitterTrue)
            {
                inputJitter = twoBest[0].jitterTrue;
            }
            else
            {
                inputJitter = twoBest[1].jitterTrue;
            }

            if (twoBest[0].totalHealth > twoBest[1].totalHealth)
            {
                inputHealth = twoBest[0].totalHealth;
            }
            else
            {
                inputHealth = twoBest[1].totalHealth;
            }

            if (twoBest[0].speedTrue > twoBest[1].speedTrue) { inputSpeed = twoBest[0].speedTrue; }
            else { inputSpeed = twoBest[1].speedTrue; }
            if (twoBest[0].visionTrue < twoBest[1].visionTrue) { inputVision = twoBest[1].visionTrue; }
            else
            {
                inputVision = twoBest[0].visionTrue;
            }
            Organisms.Clear();
            fitnesses.Clear();
            for (int i = 0; i < 10; i++)
            {
                // neural network THEN genome
                Organism betterJim = new Organism(BaseGrid, "neuralNetwork", twoBest[0].genome, twoBest[0].Layers, mutationRate);
                Organisms.Add(betterJim);

            }
            simulationCount++;
            Console.WriteLine($"Simulation {simulationCount} has BEGUN.");
            StartSimulations();
        }

        // i want to redefine how I do this simulation stuff. 
        // One simulation runs ALL the organisms for some set settings and I should have csv save state for the settings chosen. 
        // I don't want to be doing the evolution myself, the game should do it itself.

        public void BeginSimulation()
        {
            Console.WriteLine("How long should the grid be in positions?");
            string length = Console.ReadLine();
            Console.WriteLine("How wide should the grid be in positions?");
            string width = Console.ReadLine();
            Console.WriteLine("How many predators should there be?");
            string predatorCount = Console.ReadLine();
            Console.WriteLine("How many consumers should there be?");
            string consumerCount = Console.ReadLine();
            Console.WriteLine("How many pieces of food should be there at any given time?");
            string foodCount = Console.ReadLine();
            // now, using these variables, form file then begin simulation!!
            NewStartSimulations(length, predatorCount, consumerCount, foodCount);
        }
        public void StartSimulations()
        {

            BaseGrid = new Grid(10);

            foreach (Organism testJimmy in Organisms)
            {
                Simulation jimSim = new Simulation(testJimmy, BaseGrid);
                fitnesses.Add(jimSim.tickCount);
            }
            InstantiateOrganisms2();
        }

        public void NewStartSimulations(string size, string predatorCount, string consumerCount, string foodCount)
        {
            BaseGrid = new Grid(Convert.ToInt32(size));
            Predators = new List<Predator
                >();
            Consumers = new List<Consumer>();
            for (int i = 0; i < Convert.ToInt32(predatorCount); i++)
            {
                Predator predator = new Predator();
                predator.baseOrganism = new Organism(BaseGrid, "test");
                Predators.Add(predator);
            }
            for (int i = 0; i < Convert.ToInt32(consumerCount); i++)
            {
                Consumer consumer = new Consumer();
                consumer.consumerOrganism = new Organism(BaseGrid, "test");
                Consumers.Add(consumer);
            }

            Simulation mainSim = new Simulation(Predators, Consumers, BaseGrid);
        }

    }
}

