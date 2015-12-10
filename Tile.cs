using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    class Tile
    {
        public char[,] dimensions;
        public int size;
        public int rsize;
        public int csize;
        public bool solution;
        public Tile(char[,] input, int row, int col)
        {
            dimensions = input;
            rsize = row;
            csize = col;
            size = 0;
            char c = ' ';
            solution = false;
            for(int i=0; i < csize; i++)
            {
                for(int j = 0; j < rsize; j++)
                {
                    if (dimensions[i, j] != c)
                    {
                        size += 1;
                    }
                }
            }
        }
    }
}
