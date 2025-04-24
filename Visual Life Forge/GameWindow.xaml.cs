using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Visual_Life_Forge
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Simulation simulation;
        private Grid BaseGrid;
        // these lists might be useless. I don't ever use the code below
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
                predator.AttackStrength = predator.baseOrganism.strengthTrue;
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

                // Attach mouse events to the cell.
                cellBorder.MouseEnter += CellBorder_MouseEnter;
                cellBorder.MouseLeave += CellBorder_MouseLeave;

                Canvas cellCanvas = new Canvas();
                cellBorder.Child = cellCanvas;
                SimulationGrid.Children.Add(cellBorder);
            }

            simulation = new Simulation(Predators, Consumers, BaseGrid);

            RunSimulation();
        }

        private string OrganismInformation(int y, int x)
        {
            string outputString = "";
            foreach (Consumer consumer in simulation.testConsumers)
            {
                if (consumer.consumerOrganism.organismPosition.posCoordinate == (y, x))
                {
                    outputString += $"Name: {consumer.name}\n Health: {consumer.consumerOrganism.healthTrue} \nVision: {consumer.consumerOrganism.visionTrue}";
                    return outputString;
                }
            }
            foreach (Predator predator in simulation.testPredators)
            {
                if (predator.baseOrganism.organismPosition.posCoordinate == (y, x))
                {
                    outputString += $"Name: {predator.name}\n Health: {predator.baseOrganism.healthTrue} \nVision: {predator.baseOrganism.visionTrue}";
                    return outputString;
                }
            }
            return "";
            
        }

        private void CellBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            // The sender is the cell that the mouse entered.
            Border border = sender as Border;
            int index = SimulationGrid.Children.IndexOf(border);
            int columns = SimulationGrid.Columns;
            int row = index / columns;
            int col = index % columns;

            
            string organismInfo = OrganismInformation(row, col);

            if (!string.IsNullOrEmpty(organismInfo))
            {
                OrganismInfoText.Text = organismInfo;
                OrganismInfoBorder.Visibility = Visibility.Visible;
            }
            else
            {
                OrganismInfoBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void CellBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            // Hide the organism info when the mouse leaves the cell.
            OrganismInfoBorder.Visibility = Visibility.Collapsed;
        }

        // BEFORE: using System.Timers.Timer and Elapsed event
        // AFTER: using DispatcherTimer and Tick event
        public void RunSimulation()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(400);
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

            SimulationGrid.Rows = simulation.Grid.gridSize;
            SimulationGrid.Columns = simulation.Grid.gridSize;

            // Clear previous visuals
            foreach (var child in SimulationGrid.Children)
            {
                if (child is Border border && border.Child is Canvas canvas)
                {
                    canvas.Children.Clear();
                }
            }

            double cellSize = SimulationGrid.ActualWidth / SimulationGrid.Columns;
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

                    
                    double offsetX = (cellSize - width) / 2;
                    double offsetY = (cellSize - height) / 6 ;
                    Canvas.SetLeft(shape, offsetX);
                    Canvas.SetTop(shape, offsetY);

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