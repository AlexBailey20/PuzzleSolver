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

        //Column offset for finding positions
        public int cOff
        {
            get { return c_off; }
            set { c_off = value; }
        }

        //Row offset for finding positions
        public int rOff
        {
            get { return r_off; }
            set { r_off = value; }

        }

        //Row offset of the first character in the top row for placement in the first empty tile
        public int Topleft
        {
            get { return topleft; }
            set { topleft = value; }
        }

        //Number of different positions based on the offsets to be used in the for loop
        public int Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        //character matrix
        public char[,] Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        //Constructor for the Orientation object, initializes the character matrix and finds
        //the offset of the first character in the top row to be used in the empty tile recursion
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

        //Takes two character matrices and compares if they have identical dimensions and entries or not
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

        //For the tile placement recursion, this method is used to find all the different
        //positions the rectangle surrounding this piece can be placed in the solution
        public void FindPos(int csol, int rsol)
        {
            cOff = csol - dimensions.GetLength(0) + 1;
            rOff = rsol - dimensions.GetLength(1) + 1;
            Positions = cOff * rOff;
        }

        //Checks if a piece can be placed in the solution by making sure it fits in the dimensions and making sure it will not overlap a character with a character in place
        //If it can go in, the piece is added to the running_solution and its color is added to the color matrix and the method returns true
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

        //Used in the solution building recursion
        //Takes the pieces in this orientation and pulls it from the running_solution
        //Sets the color of the spots it was pulled from back to the default -1
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
