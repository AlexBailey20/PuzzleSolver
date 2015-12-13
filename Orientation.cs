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
        private char[,] dimensions;
        private int positions;

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

        public char[,] Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public int Positions
        {
            get { return positions; }
            set { positions = value; }
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
        public Orientation(char[,] dimension, int csize, int rsize)
        {
            Dimensions = dimension;
        }
    }
}
