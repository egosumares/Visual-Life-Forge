using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Simulation simulation;
        private Grid BaseGrid;
        private List<Predator> Predators;
        private List<Consumer> Consumers;

        // BEFORE: System.Timers.Timer _timer = new System.Timers.Timer(150);
        // AFTER:
        private DispatcherTimer _timer = new DispatcherTimer();

        public GameWindow(int predatorCount, int consumerCount, int gridSize)
        {
            InitializeComponent();

            BaseGrid = new Grid(gridSize);
            Predators = new List<Predator>();
            Consumers = new List<Consumer>();

            for (int i = 0; i < predatorCount; i++)
            {
                Predator predator = new Predator();
                predator.baseOrganism = new Organism(BaseGrid, "test");
                Predators.Add(predator);
            }

            for (int i = 0; i < consumerCount; i++)
            {
                Consumer consumer = new Consumer();
                consumer.consumerOrganism = new Organism(BaseGrid, "test");
                Consumers.Add(consumer);
            }

            SimulationGrid.Children.Clear();

            for (int i = 0; i < gridSize * gridSize; i++)
            {
                Border cellBorder = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(1)
                };

                Canvas cellCanvas = new Canvas();
                cellBorder.Child = cellCanvas;
                SimulationGrid.Children.Add(cellBorder);
            }

            simulation = new Simulation(Predators, Consumers, BaseGrid);

            RunSimulation();
        }

        // BEFORE: using System.Timers.Timer and Elapsed event
        // AFTER: using DispatcherTimer and Tick event
        public void RunSimulation()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(300);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        // AFTER: async tick handler moves simulation logic off the UI thread
        private async void Timer_Tick(object sender, EventArgs e)
        {
            DrawScreen();
            // Run simulation on background thread
            await Task.Run(() =>
            {
                simulation.RunSimulationOnce();
            });

            // Update UI on main thread
            
        }

        // AFTER: UI update code moved here for clarity and reuse
        private void DrawScreen()
        {
            int totalOrganisms = simulation.testConsumers.Count + simulation.testPredators.Count;
            OrganismCount.Text = totalOrganisms.ToString();

            // Clear previous visuals
            foreach (var child in SimulationGrid.Children)
            {
                if (child is Border border && border.Child is Canvas canvas)
                {
                    canvas.Children.Clear();
                }
            }

            double cellSize = 50;
            int columns = SimulationGrid.Columns;

            // Helper to draw a shape in the correct cell
            void DrawEllipse(int row, int col, double width, double height, Brush color)
            {
                int index = row * columns + col;
                if (index < 0 || index >= SimulationGrid.Children.Count) return;

                if (SimulationGrid.Children[index] is Border cellBorder &&
                    cellBorder.Child is Canvas canvas)
                {
                    Ellipse shape = new Ellipse
                    {
                        Width = width,
                        Height = height,
                        Fill = color
                    };

                    double offset = (cellSize - width) / 2;
                    Canvas.SetLeft(shape, offset);
                    Canvas.SetTop(shape, offset / 2);

                    canvas.Children.Add(shape);
                }
            }

            // Draw consumers (green)
            foreach (var consumer in simulation.testConsumers)
            {
                var pos = consumer.consumerOrganism.organismPosition.posCoordinate;
                DrawEllipse(pos.Item1, pos.Item2, 20, 20, Brushes.Green);
            }

            // Draw predators (red)
            foreach (var predator in simulation.testPredators)
            {
                var pos = predator.baseOrganism.organismPosition.posCoordinate;
                DrawEllipse(pos.Item1, pos.Item2, 20, 20, Brushes.Red);
            }

            // Draw food (blue)
            foreach (var food in simulation.Grid.foods)
            {
                var pos = food.foodPosition.posCoordinate;
                DrawEllipse(pos.Item1, pos.Item2, 10, 10, Brushes.Blue);
            }

            // draw obstacles (black)
            foreach (var obstacle in simulation.Grid.obstacles)
            {
                var pos = obstacle.obstaclePosition.posCoordinate;
                DrawEllipse(pos.Item1, pos.Item2, 30, 30, Brushes.Black);
            }
        }
    }
}