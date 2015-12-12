using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    class Orientation
    {
        public int c_off;
        public int r_off;
        public char[,] dimensions;
        public int positions;

        public bool CheckSame(char[,] potential)
        {
            if(potential.GetLength(0) != dimensions.GetLength(0) || potential.GetLength(1) != dimensions.GetLength(1))
            {
                return false;
            }
            else
            {
                for(int i = 0; i < dimensions.GetLength(0); i++)
                {
                    for(int j = 0; j < dimensions.GetLength(1); j++)
                    {
                        if(potential[i,j] != dimensions[i, j])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public void FindPos(int sol_c, int sol_r)
        {
            c_off = sol_c - dimensions.GetLength(0) + 1;
            r_off = sol_r - dimensions.GetLength(1) + 1;
            positions = c_off * r_off;
        }
        public Orientation(char[,] dimension, int c_size, int r_size)
        {
            dimensions = dimension;
        }
    }
}
