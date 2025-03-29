using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    class Grid
    {
        Random rnd = new Random();
        public int gridSize;
        public List<Food> foods;
        public List<Position> gridPositions;
        public List<Obstacle> obstacles;
        public List<Position> availablePositions;
        public Grid(int gridsize)
        {
            List<int> posValues = new List<int>();
            gridPositions = new List<Position>();
            availablePositions = new List<Position>();
          
            for (int i = 0; i < gridsize; i++)
            {
                posValues.Add(i);
            }
            foreach (int i in posValues)
            {
                foreach (int j in posValues)
                {
                    gridPositions.Add(new Position(i, j));
                }
            }
            foreach (var value in gridPositions)
            {
                availablePositions.Add(value);
            }
            gridSize = gridsize;
            foods = new List<Food>();

            for (int i = 0; i < gridsize; i++)
            {
                int index = rnd.Next(availablePositions.Count);
                Food food1 = new Food(availablePositions[index]);
                foods.Add(food1);
                availablePositions.Remove(availablePositions[index]);
            }
            obstacles = new List<Obstacle>();
            for (int i = 0; i < gridSize; i++)
            {
                int index = rnd.Next(availablePositions.Count);
                Obstacle obstacle = new Obstacle(availablePositions[index]);
                obstacles.Add(obstacle);
                availablePositions.Remove(availablePositions[index]);
            }
        }

        public void RemovePosition(Position pos)
        {
            foreach (var value in availablePositions)
            {
                if (value.posCoordinate == pos.posCoordinate) availablePositions.Remove(pos); return;
            }
        }
        public void AddFood()
        {
            int index = rnd.Next(gridPositions.Count);
            Food food = new Food(gridPositions[index]);
            foods.Add(food);
        }

        public List<Position> AdjacentCells(Position position)
        {
            List<Position> cells = new List<Position>();
            for (int i = -1; i < 2; i++)
            {
                
                for (int j = -1; j < 2; j++)
                {
                    
                    if (position.posCoordinate.Item1 + i >= 0 && position.posCoordinate.Item2 + j < gridSize && position.posCoordinate.Item2 + j >= 0 && position.posCoordinate.Item1 + i < gridSize)
                    {
                        Position pos = new Position(position.posCoordinate.Item1 + i, position.posCoordinate.Item2 + j);
                        if (!(i == 0 && j == 0))
                        {
                            cells.Add(pos);
                        }
                        
                    }

                }
            }
            

            return cells;
        }
    }
}
