using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Visual_Life_Forge
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        public int predatorCount;
        public int consumerCount;
        public int gridSize;

        private void OpenGameWindow(object sender, RoutedEventArgs e)
        {
            // Parse values from TextBoxes
            predatorCount = int.TryParse(PredatorTextBox.Text, out predatorCount) ? predatorCount : 0;
            consumerCount = int.TryParse(ConsumerTextBox.Text, out consumerCount) ? consumerCount : 0;
            gridSize = int.TryParse(GridSizeTextBox.Text, out gridSize) ? gridSize : 10;  // Default grid size is 10

            // Pass these values to the GameWindow
            GameWindow gameWindow = new GameWindow(predatorCount, consumerCount, gridSize);
            gameWindow.Show();
            this.Close(); // Optionally close this window when starting the game
        }
        private void IncreasePredator(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PredatorTextBox.Text, out int value))
            {
                PredatorTextBox.Text = (value + 1).ToString();
            }
        }

        private void DecreasePredator(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PredatorTextBox.Text, out int value) && value > 0)
            {
                PredatorTextBox.Text = (value - 1).ToString();
            }
        }

        private void PredatorCountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Number of predators: {PredatorTextBox.Text}");
        }

        // Consumer Count Handlers
        private void IncreaseConsumer(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ConsumerTextBox.Text, out int value))
            {
                ConsumerTextBox.Text = (value + 1).ToString();
            }
        }

        private void DecreaseConsumer(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ConsumerTextBox.Text, out int value) && value > 0)
            {
                ConsumerTextBox.Text = (value - 1).ToString();
            }
        }

        private void ConsumerCountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Number of consumers: {ConsumerTextBox.Text}");
        }

        // Grid Size Handlers
        private void IncreaseGridSize(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(GridSizeTextBox.Text, out int value))
            {
                GridSizeTextBox.Text = (value + 1).ToString();
            }
        }

        private void DecreaseGridSize(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(GridSizeTextBox.Text, out int value) && value > 1)
            {
                GridSizeTextBox.Text = (value - 1).ToString();
            }
        }

        private void GridSizeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Grid size: {GridSizeTextBox.Text} units");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenGameWindow(sender, e);
        }
    }
}
