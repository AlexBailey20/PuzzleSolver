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
        public List<Orientation> orientations;
        public int size;
        public int rsize;
        public int csize;
        public int positions;
        public int sol_c;
        public int sol_r;
        public int c_off;
        public int r_off;
        public int color_code;
        public bool solution;
//Constructor, takes a 2D char array and finds the number of chars which are significant (non-space)
        public Tile(char[,] input, int row, int col, int code)
        {
            dimensions = input;
            color_code = code;
            orientations = new List<Orientation>();
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
        public void CheckIsomorphic(Tile t)
        {
            for(int i = 0; i < t.orientations.Count; i++)
            {
                if (t.orientations[i].CheckSame(dimensions))
                {
                    color_code = t.color_code;
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
            for(int i = 0; i < orientations.Count; i++)
            {
                orientations[i].FindPos(sol_c, sol_r);
            }
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
        public bool CheckNewSolution(int[,] running_colors, List<int[,]> color_solutions)
        {
            bool repeated_solution = true;
            for(int i = 0; i < color_solutions.Count; i++)
            {
                repeated_solution = true;
                for(int j = 0; j < running_colors.GetLength(0); j++)
                {
                    for(int k = 0; k < running_colors.GetLength(1); k++)
                    {
                        if(running_colors[j,k] != color_solutions[i][j, k])
                        {
                            repeated_solution = false;
                            break;
                        }
                    }
                    if (!repeated_solution)
                    {
                        break;
                    }
                }
                if (repeated_solution)
                {
                    return false;
                }
            }
            return CheckAgainstSolutionRotations(running_colors, color_solutions);
        }
        public bool CheckAgainstSolutionRotations(int[,] running_colors, List<int[,]> color_solutions)
        {
            for(int i = 0; i < color_solutions.Count; i++)
            {
                if(CheckReflection(running_colors, color_solutions[i]) || CheckRotation90(running_colors, color_solutions[i]) || CheckRotation180(running_colors, color_solutions[i]) || CheckRotation270(running_colors, color_solutions[i])){
                    return false;
                }

            }
            return true;
        }
        public bool CheckReflection(int[,] running_colors, int[,] color_solution)
        {
            if(running_colors.GetLength(0) != color_solution.GetLength(0) || running_colors.GetLength(1) != color_solution.GetLength(1))
            {
                return false;
            }
            for (int i = 0; i < running_colors.GetLength(0); i++)
            {
                for (int j = 0; j < running_colors.GetLength(1); j++)
                {
                    if (running_colors[i, j] != color_solution[color_solution.GetLength(0) - i - 1, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool CheckRotation90(int[,] running_colors, int[,] color_solution)
        {
            if (running_colors.GetLength(1) != color_solution.GetLength(0) || running_colors.GetLength(0) != color_solution.GetLength(1))
            {
                return false;
            }
            bool refl = true;
            int[,] reflected = new int[color_solution.GetLength(1), color_solution.GetLength(0)];
            for (int i = 0; i < running_colors.GetLength(0); i++)
            {
                for (int j = 0; j < running_colors.GetLength(1); j++)
                {
                    reflected[i, j] = color_solution[j, running_colors.GetLength(1) - i - 1];
                    if (running_colors[i, j] != color_solution[j, running_colors.GetLength(1) - i - 1])
                    {
                        refl = false;
                    }
                }
            }
            if (refl)
            {
                return true;
            }
            return CheckReflection(running_colors, reflected);
        }
        public bool CheckRotation180(int[,] running_colors, int[,] color_solution)
        {
            if (running_colors.GetLength(0) != color_solution.GetLength(0) || running_colors.GetLength(1) != color_solution.GetLength(1))
            {
                return false;
            }
            bool refl = true;
            int[,] reflected = new int[color_solution.GetLength(0), color_solution.GetLength(1)];
            for (int i = 0; i < running_colors.GetLength(0); i++)
            {
                for (int j = 0; j < running_colors.GetLength(1); j++)
                {
                    reflected[i, j] = color_solution[color_solution.GetLength(0) - i - 1, color_solution.GetLength(1) - j - 1];
                    if (running_colors[i, j] != color_solution[color_solution.GetLength(0) - i - 1, color_solution.GetLength(1) - j - 1])
                    {
                        refl = false;
                    }
                }
            }
            if (refl)
            {
                return true;
            }
            return CheckReflection(running_colors, reflected);
        }
        public bool CheckRotation270(int[,] running_colors, int[,] color_solution)
        {
            if (running_colors.GetLength(1) != color_solution.GetLength(0) || running_colors.GetLength(0) != color_solution.GetLength(1))
            {
                return false;
            }
            bool refl = true;
            int[,] reflected = new int[color_solution.GetLength(1), color_solution.GetLength(0)];
            for (int i = 0; i < running_colors.GetLength(0); i++)
            {
                for (int j = 0; j < running_colors.GetLength(1); j++)
                {
                    reflected[i, j] = color_solution[color_solution.GetLength(0) - j - 1, i];
                    if (running_colors[i, j] != color_solution[color_solution.GetLength(0) - j - 1, i])
                    {
                        refl = false;
                    }
                }
            }
            if (refl)
            {
                return true;
            }
            return CheckReflection(running_colors, reflected);
        }
        //Method to check if adding tile will overlap with any tiles already added
        public bool CheckOverlap(char[,] potential_solution, int col_offset, int row_offset, int g)
        {
            for (int i = 0; i < orientations[g].dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < orientations[g].dimensions.GetLength(1); j++)
                {
                    if (orientations[g].dimensions[i, j] != ' ')
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
        public bool PlaceInSolution(char[,] potential_solution, int[,] potential_colors, int col_offset, int row_offset, int g)
        {
            if(!CheckOverlap(potential_solution, col_offset, row_offset, g))
            {
                return false;
            }
            for(int i = 0; i < orientations[g].dimensions.GetLength(0); i++)
            {
                for(int j = 0; j < orientations[g].dimensions.GetLength(1); j++)
                {
                    if (orientations[g].dimensions[i, j] != ' ')
                    {                        
                        potential_solution[i + col_offset, j + row_offset] = orientations[g].dimensions[i, j];
                        potential_colors[i + col_offset, j + row_offset] = color_code;
                    }
                }
            }
            return true;
        }
        //Takes the tile and removes it from the running possible solution array based on the current offsets being used
        public void RemoveFromSolution(char[,] potential_solution, int[,] potential_colors, int col_offset, int row_offset, int g)
        {
            for (int i = 0; i < orientations[g].dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < orientations[g].dimensions.GetLength(1); j++)
                {
                    if (orientations[g].dimensions[i, j] != ' ')
                    {
                        potential_solution[i + col_offset, j + row_offset] = ' ';
                        potential_colors[i + col_offset, j + row_offset] = -1;
                    }
                }
            }
        }
//Method to find each unique rotation and reflection for this tile
        public void FindOrientations(char[,] initial_orientation, int csize, int rsize)
        {
            Orientation o1 = new Orientation(initial_orientation, csize, rsize);
            orientations.Add(o1);
            Reflect(initial_orientation);
            Rotate90(initial_orientation);
            Rotate180(initial_orientation);
            Rotate270(initial_orientation);
        }
        public bool CheckDimensions(char[,] dims)
        {
            if(dims.GetLength(0) > sol_c || dims.GetLength(1) > sol_r)
            {
                return false;
            }
            return true;
        }
        public bool RunningCheck(char[,] pos_solution)
        {
            for(int i = 0; i < pos_solution.GetLength(0); i++)
            {
                for (int j = 0; j < pos_solution.GetLength(1); j++)
                {
                    if(pos_solution[i,j] != ' ' && pos_solution[i,j] != dimensions[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
//Method which reflects a 2D char array, checks if its a unique orientation, and potentially adds it to the list of orientations
        public void Reflect(char[,] initial_orientation)
        {
            char[,] reflected = new char[initial_orientation.GetLength(0), initial_orientation.GetLength(1)];
            for(int i = 0; i < initial_orientation.GetLength(0); i++)
            {
                for(int j = 0; j < initial_orientation.GetLength(1); j++)
                {
                    reflected[i, j] = initial_orientation[initial_orientation.GetLength(0) - i - 1, j];
                }
            }
            bool uni = true;
            for(int n = 0; n < orientations.Count; n++)
            {
                if (orientations[n].CheckSame(reflected))
                {
                    uni = false;
                    break;
                }
            }
            if (uni && CheckDimensions(reflected))
            {
                Orientation m = new Orientation(reflected, reflected.GetLength(0), reflected.GetLength(1));
                orientations.Add(m);
            }
        }
//Methods which rotate a 2D char array either 90, 180, or 270 degrees and checks its uniqueness, then calls reflect on the rotated version
        public void Rotate90(char[,] o)
        {
            char[,] rotated = new char[o.GetLength(1), o.GetLength(0)];
            bool uni = true;
            int r = 0;
            for (int i = 0; i < o.GetLength(1); i++)
            {
                for (int j = 0; j < o.GetLength(0); j++)
                {
                    rotated[i, j] = o[j, o.GetLength(1) - i - 1];
                }
            }
            for (int n = 0; n < orientations.Count; n++)
            {
                if (orientations[n].CheckSame(rotated))
                {
                    uni = false;
                    r = n;
                    break;
                }
            }
            if (uni && CheckDimensions(rotated))
            {
                Orientation m = new Orientation(rotated, rotated.GetLength(0), rotated.GetLength(1));
                orientations.Add(m);
                Reflect(rotated);
            }
            else
            {
                Reflect(orientations[r].dimensions);
            }
        }
        public void Rotate180(char[,] o)
        {
            char[,] rotated = new char[o.GetLength(0), o.GetLength(1)];
            bool uni = true;
            int r = 0;
            for (int i = 0; i < o.GetLength(0); i++)
            {
                for (int j = 0; j < o.GetLength(1); j++)
                {
                    rotated[i, j] = o[o.GetLength(0) - i - 1, o.GetLength(1) - j - 1];
                }
            }
            for (int n = 0; n < orientations.Count; n++)
            {
                if (orientations[n].CheckSame(rotated))
                {
                    uni = false;
                    r = n;
                    break;
                }
            }
            if (uni && CheckDimensions(rotated))
            {
                Orientation m = new Orientation(rotated, rotated.GetLength(0), rotated.GetLength(1));
                orientations.Add(m);
                Reflect(rotated);
            }
            else
            {
                Reflect(orientations[r].dimensions);
            }
        }
        public void Rotate270(char[,] o)
        {
            char[,] rotated = new char[o.GetLength(1), o.GetLength(0)];
            bool uni = true;
            int r = 0;
            for (int i = 0; i < o.GetLength(1); i++)
            {
                for (int j = 0; j < o.GetLength(0); j++)
                {
                    rotated[i, j] = o[o.GetLength(0) - j - 1, i];
                }
            }
            for (int n = 0; n < orientations.Count; n++)
            {
                if (orientations[n].CheckSame(rotated))
                {
                    uni = false;
                    r = n;
                    break;
                }
            }
            if (uni && CheckDimensions(rotated))
            {
                Orientation m = new Orientation(rotated, rotated.GetLength(0), rotated.GetLength(1));
                orientations.Add(m);
                Reflect(rotated);
            }
            else
            {
                Reflect(orientations[r].dimensions);
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
