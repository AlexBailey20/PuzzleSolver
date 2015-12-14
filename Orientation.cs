using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    public class Orientation
    {
        private int c_off;
        private int r_off;
        private int topleft;
        private int positions;
        private char[,] dimensions;

        public int cOff
        {
            get { return c_off; }
            set { c_off = value; }
        }

        public int rOff
        {
            get { return r_off; }
            set { r_off = value; }

        }

        public int Topleft
        {
            get { return topleft; }
            set { topleft = value; }
        }

        public int Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        public char[,] Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Orientation(char[,] dimension, int csize, int rsize)
        {
            Dimensions = dimension;
            for (int j = 0; j < Dimensions.GetLength(1); j++)
            {
                if (Dimensions[0, j] != ' ')
                {
                    Topleft = j;
                    break;
                }
            }
        }

        public bool CheckSame(char[,] potential)
        {
            if(potential.GetLength(0) != Dimensions.GetLength(0) || potential.GetLength(1) != Dimensions.GetLength(1))
            {
                return false;
            }
            else
            {
                for(int i = 0; i < Dimensions.GetLength(0); i++)
                {
                    for(int j = 0; j < Dimensions.GetLength(1); j++)
                    {
                        if(potential[i,j] != Dimensions[i, j])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void FindPos(int csol, int rsol)
        {
            cOff = csol - dimensions.GetLength(0) + 1;
            rOff = rsol - dimensions.GetLength(1) + 1;
            Positions = cOff * rOff;
        }

        public bool PlaceInSolution(char[,] running_solution, int[,] running_color, int ii, int jj, int color)
        {
            if ((ii + dimensions.GetLength(0) > running_solution.GetLength(0)) || (jj + dimensions.GetLength(1) - Topleft > running_solution.GetLength(1)) || (jj - Topleft < 0))
            {
                return false;
            }
            for (int i = 0; i < dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < dimensions.GetLength(1); j++)
                {
                    if (dimensions[i, j] != ' ' && running_solution[ii + i, jj + j - Topleft] != ' ')
                    {
                        return false;
                    }
                }
            }
            for (int n = 0; n < dimensions.GetLength(0); n++)
            {
                for (int m = 0; m < dimensions.GetLength(1); m++)
                {
                    if (dimensions[n, m] != ' ')
                    {
                        running_solution[ii + n, jj + m - Topleft] = dimensions[n, m];
                        running_color[ii + n, jj + m - Topleft] = color;
                    }
                }
            }
            return true;
        }

        internal void RemoveFromSolution(char[,] running_solution, int[,] running_colors, int ii, int jj)
        {
            for (int i = 0; i < dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < dimensions.GetLength(1); j++)
                {
                    if (dimensions[i, j] != ' ')
                    {
                        running_solution[i + ii, j + jj - Topleft] = ' ';
                        running_colors[i + ii, j + jj - Topleft] = -1;
                    }
                }
            }
        }
    }
}
