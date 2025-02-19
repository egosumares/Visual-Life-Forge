using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Simulation simulation;
        private Grid BaseGrid;
        private List<Predator> Predators;
        private List<Consumer> Consumers;

        public GameWindow(int predatorCount,int consumerCount,int gridSize)
        {
            InitializeComponent();
            BaseGrid = new Grid(gridSize);
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
            simulation = new Simulation(Predators, Consumers, BaseGrid);
        }
    }
}
