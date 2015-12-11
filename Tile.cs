using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    class Tile : IComparable
    {
        public char[,] dimensions;
        public int size;
        public int rsize;
        public int csize;
        public int positions;
        public int sol_c;
        public int sol_r;
        public int c_off;
        public int r_off;
        public bool solution;
//Constructor, takes a 2D char array and finds the number of chars which are significant (non-space)
        public Tile(char[,] input, int row, int col)
        {
            dimensions = input;
            rsize = row;
            csize = col;
            size = 0;
            sol_c = 0;
            sol_r = 0;
            positions = 0;
            c_off = 0;
            r_off = 0;
            char c = ' ';
            solution = false;
            this.Compress();
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
//Comparator method for sorting array to find solution and to optimize algorithm slightly by placing largest tiles first
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Tile t = obj as Tile;
            if (t != null)
                return this.size.CompareTo(t.size);
            else
            {
                return 0;
            }
        }
//Method which finds the number of different ways the Tile can be placed on the blank solution array
        public void FindPositions()
        {
            this.c_off = sol_c - csize + 1;
            this.r_off = sol_r - rsize + 1;
            this.positions = c_off * r_off;
        }
//Compares each element in the potential_solution to the actual solution. Bounds are safe as both are defined by the same values
        public bool CheckValid(char[,] potential_solution)
        {
            for(int i = 0; i < csize; i++)
            {
                for(int j = 0; j < rsize; j++)
                {
                    if(potential_solution[i,j] != dimensions[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
//Method to check if adding tile will overlap with any tiles already added
        public bool CheckOverlap(char[,] potential_solution, int col_offset, int row_offset)
        {
            for (int i = 0; i < csize; i++)
            {
                for (int j = 0; j < rsize; j++)
                {
                    if (dimensions[i, j] != ' ')
                    {
                        if (potential_solution[i + col_offset, j + row_offset] != ' ')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
//Method which takes a potential solution and a placement of this tile on that solution space, adds the tile to that solution space
        public bool PlaceInSolution(char[,] potential_solution, int col_offset, int row_offset)
        {
            if(!CheckOverlap(potential_solution, col_offset, row_offset))
            {
                return false;
            }
            for(int i = 0; i < csize; i++)
            {
                for(int j = 0; j < rsize; j++)
                {
                    if (dimensions[i, j] != ' ')
                    {                        
                        potential_solution[i + col_offset, j + row_offset] = dimensions[i, j];
                    }
                }
            }
            return true;
        }
//Takes the tile and removes it from the running possible solution array based on the current offsets being used
        public void RemoveFromSolution(char[,] potential_solution, int col_offset, int row_offset)
        {
            for (int i = 0; i < csize; i++)
            {
                for (int j = 0; j < rsize; j++)
                {
                    if (dimensions[i, j] != ' ')
                    {
                        potential_solution[i + col_offset, j + row_offset] = ' ';
                    }
                }
            }
        }
//Method to take the Tile dimensions and shrink them to the smallest possible rectangle around the Tile
        public void Compress()
        {
            int min_i = csize;
            int min_j = rsize;
            int max_i = 0;
            int max_j = 0;
            for(int i = 0; i< csize; i++)
            {
                for(int j = 0; j < rsize; j++)
                {
                    if(dimensions[i,j] != ' ')
                    {
                        if(i < min_i)
                        {
                            min_i = i;
                        }
                        if(j < min_j)
                        {
                            min_j = j;
                        }
                        if(i > max_i)
                        {
                            max_i = i;
                        }
                        if(j > max_j)
                        {
                            max_j = j;
                        }
                    }
                }
            }
            char[,] new_dim = new char[max_i - min_i + 1, max_j - min_j + 1];
            this.rsize = max_j - min_j + 1;
            this.csize = max_i - min_i + 1;
            for (int n = 0; n < this.csize; n++)
            {
                for (int m = 0; m < this.rsize; m++)
                {
                    new_dim[n, m] = dimensions[n + min_i, m + min_j];
                }
            }
            this.dimensions = new_dim;
        }
    }
}
