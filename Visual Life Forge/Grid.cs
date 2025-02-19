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
        public Grid(int gridsize)
        {
            List<int> posValues = new List<int>();
            gridPositions = new List<Position>();
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
            gridSize = gridsize;
            foods = new List<Food>();

            for (int i = 0; i < gridsize; i++)
            {
                int index = rnd.Next(gridPositions.Count);
                Food food1 = new Food(gridPositions[index]);
                foods.Add(food1);
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
            int iMin = -1;
            int iMax = 2;
            int jMin = -1;
            int jMax = 2;
            if (position.posCoordinate.Item1 == 0) iMin = 0;

            else if (position.posCoordinate.Item1 == gridSize - 1) iMax = 1;

            else if (position.posCoordinate.Item2 == 0) jMin = 0;

            else if (position.posCoordinate.Item2 == gridSize - 1) jMax = 1;

            for (int i = iMin; i < iMax; i++)
            {
                for (int j = jMin; j < jMax; j++)
                {
                    if (i == 0 || j == 0 && !(i == 0 && j == 0))
                    {
                        int xCoordinate = i + position.posCoordinate.Item1;
                        int yCoordinate = j + position.posCoordinate.Item2;
                        Position testPos = null;
                        foreach (Position pos in gridPositions)
                        {
                            {
                                if (pos.posCoordinate.Item1 == xCoordinate && pos.posCoordinate.Item2 == yCoordinate)
                                {
                                    testPos = pos;
                                }
                            }
                            if (!(i == 0 && j == 0))
                            {
                                cells.Add(testPos);
                            }
                            ;
                        }
                    }

                }
            }

            return cells;
        }
    }
}
