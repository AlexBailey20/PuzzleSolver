using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    public class Tile : IComparable
    {
        private char[,] dimensions;
        private List<Orientation> orientations;
        private int size;
        private int rsize;
        private int csize;
        private int positions;
        private int csol;
        private int rsol;
        private int coff;
        private int roff;
        private int colorcode;
        private bool target;

        //Character matrix as read in by the Parser, before rotations or reflections
        public char[,] Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        //List of all different orientations the piece can take based on reflected and rotated flags and the piece's symmetry
        public List<Orientation> Orientations
        {
            get { return orientations; }
            set { orientations = value; }
        }

        //Number of tiles in the piece
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        //Rows
        public int rSize
        {
            get { return rsize; }
            set { rsize = value; }
        }

        //Columns
        public int cSize
        {
            get { return csize; }
            set { csize = value; }
        }

        //Number of positions the piece can take on an empty board
        public int Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        //Solution columns
        public int cSol
        {
            get { return csol; }
            set { csol = value; }
        }

        //Solution rows
        public int rSol
        {
            get { return rsol; }
            set { rsol = value; }
        }

        //Possible column offsets of the piece
        public int cOff
        {
            get { return coff; }
            set { coff = value; }
        }

        //Possible row offsets of the piece
        public int rOff
        {
            get { return roff; }
            set { roff = value; }
        }

        //Color code specific to the Tile
        public int Colorcode
        {
            get { return colorcode; }
            set { colorcode = value; }
        }

        //Flag to indicate if this is the solution Tile or not
        public bool Target
        {
            get { return target; }
            set { target = value; }
        }

        // copy constructor
        public Tile(Tile tile)
        {
            Dimensions = tile.Dimensions;
            Orientations = tile.Orientations;
            rSize = tile.rSize;
            cSize = tile.cSize;
            Size = tile.Size;
            Positions = tile.Positions;
            cSol = tile.cSol;
            rSol = tile.rSol;
            cOff = tile.cOff;
            rOff = tile.rOff;
            Colorcode = tile.Colorcode;
            Target = tile.Target;
        }

        //Constructor, takes a 2D char array and finds the number of chars which are significant (non-space)
        public Tile(char[,] input, int row, int col, int code)
        {
            Dimensions = input;
            Colorcode = code;
            Orientations = new List<Orientation>();
            rSize = row;
            cSize = col;
            Size = 0;
            cSol = 0;
            rSol = 0;
            Positions = 0;
            cOff = 0;
            rOff = 0;
            Target = false;
            char c = ' ';
            this.Compress();
            for (int i = 0; i < cSize; i++)
            {
                for (int j = 0; j < rSize; j++)
                {
                    if (Dimensions[i, j] != c)
                        Size += 1;
                }
            }
        }

        //Iterate through all orientations of the parameter Tile and indicate if it matches this tile. If so, sets their color codes equal to avoid duplicate solutions
        public void CheckIsomorphic(Tile t)
        {
            for (int i = 0; i < t.Orientations.Count; i++)
            {
                if (t.Orientations[i].CheckSame(Dimensions))
                    Colorcode = t.Colorcode;
            }
        }

        //Comparator method for sorting array to find solution and to optimize algorithm slightly by placing largest tiles first
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Tile t = obj as Tile;
            if (t != null)
                return this.Size.CompareTo(t.Size);
            else
                return 0;
        }

        //Method which finds the number of different ways the Tile can be placed on the blank solution array
        public void FindPositions()
        {
            for (int i = 0; i < Orientations.Count; i++)
                Orientations[i].FindPos(cSol, rSol);
        }

        //Compares each element in the potential_solution to the actual solution. Bounds are safe as both are defined by the same values
        public bool CheckValid(char[,] potentialsolution)
        {
            for (int i = 0; i < cSize; i++)
            {
                for (int j = 0; j < rSize; j++)
                {
                    if (potentialsolution[i, j] != Dimensions[i, j])
                        return false;
                }
            }
            return true;
        }

        //Takes the running solution and colors and compares it to all previous solutions, and all rotations and reflections of those solutions to see if it is a new solution
        public bool CheckNewSolution(int[,] runningcolors, List<int[,]> colorsolutions, bool symmetric)
        {
            bool repeatedsolution = true;
            for (int i = colorsolutions.Count - 1; i >= 0; i--)
            {
                repeatedsolution = true;
                for (int j = 0; j < runningcolors.GetLength(0); j++)
                {
                    for (int k = 0; k < runningcolors.GetLength(1); k++)
                    {
                        if (runningcolors[j, k] != colorsolutions[i][j, k])
                        {
                            repeatedsolution = false;
                            break;
                        }
                    }
                    if (!repeatedsolution)
                        break;
                }
                if (repeatedsolution)
                    return false;
            }
            return CheckAgainstSolutionRotations(runningcolors, colorsolutions);
        }

        //Called by CheckNewSolution to compare rotations of previous solutions to the potential new solution
        public bool CheckAgainstSolutionRotations(int[,] runningcolors, List<int[,]> colorsolutions)
        {
            for (int i = 0; i < colorsolutions.Count; i++)
            {
                if (CheckReflection(runningcolors, colorsolutions[i]) || CheckRotation90(runningcolors, colorsolutions[i]) || CheckRotation180(runningcolors, colorsolutions[i]) || CheckRotation270(runningcolors, colorsolutions[i]))
                    return false;
            }
            return true;
        }

        //Called by CheckNewSolution to compare reflections of previous solutions to the potential new solution
        public bool CheckReflection(int[,] runningcolors, int[,] colorsolution)
        {
            if (runningcolors.GetLength(0) != colorsolution.GetLength(0) || runningcolors.GetLength(1) != colorsolution.GetLength(1))
                return false;
            for (int i = 0; i < runningcolors.GetLength(0); i++)
            {
                for (int j = 0; j < runningcolors.GetLength(1); j++)
                {
                    if (runningcolors[i, j] != colorsolution[colorsolution.GetLength(0) - i - 1, j])
                        return false;
                }
            }
            return true;
        }

        //Returns a bool indicating whether or not the solution has rotational symmetry
        public bool CheckRotationalSymmetry()
        {
            if (Dimensions.GetLength(0) != Dimensions.GetLength(1))
                return false;
            for (int i = 0; i < Dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < Dimensions.GetLength(1); j++)
                {
                    if (Dimensions[i, j] != Dimensions[j, Dimensions.GetLength(1) - i - 1])
                        return false;
                }
            }
            return true;
        }

        //For the empty tile recursion, optimizes the search tree by rotating the solution dimension to minimize row length
        public bool CheckDimensionRotation()
        {
            if (Dimensions.GetLength(0) < Dimensions.GetLength(1))
            {
                char[,] rotated = new char[Dimensions.GetLength(1), Dimensions.GetLength(0)];
                for (int i = 0; i < rotated.GetLength(0); i++)
                {
                    for (int j = 0; j < rotated.GetLength(1); j++)
                    {
                        rotated[i, j] = Dimensions[Dimensions.GetLength(0) - j - 1, i];
                    }
                }
                Dimensions = rotated;
                cSize = rotated.GetLength(0);
                rSize = rotated.GetLength(1);
                return true;
            }
            return false;
        }

        //Returns a bool indicating if reflecting the Tile results in the same Tile or not
        public bool CheckReflectedSymmetry()
        {
            for (int i = 0; i < Dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < Dimensions.GetLength(1); j++)
                {
                    if (Dimensions[i, j] != Dimensions[Dimensions.GetLength(0) - i - 1, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Returns a bool indicating if rotating the Tile 180 degrees results in the same Tile or not
        public bool Check180Symmetry()
        {
            for (int i = 0; i < Dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < Dimensions.GetLength(1); j++)
                {
                    if (Dimensions[i, j] != Dimensions[Dimensions.GetLength(0) - i - 1, dimensions.GetLength(1) - j - 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //The next three methods, called by the solution uniqueness checker, compare the possible solution to all rotations and reflections of the previous solutions
        public bool CheckRotation90(int[,] runningcolors, int[,] colorsolution)
        {
            if (runningcolors.GetLength(1) != colorsolution.GetLength(0) || runningcolors.GetLength(0) != colorsolution.GetLength(1))
                return false;
            bool refl = true;
            int[,] reflected = new int[colorsolution.GetLength(1), colorsolution.GetLength(0)];
            for (int i = 0; i < runningcolors.GetLength(0); i++)
            {
                for (int j = 0; j < runningcolors.GetLength(1); j++)
                {
                    reflected[i, j] = colorsolution[j, runningcolors.GetLength(1) - i - 1];
                    if (runningcolors[i, j] != colorsolution[j, runningcolors.GetLength(1) - i - 1])
                        refl = false;
                }
            }
            if (refl)
                return true;
            return CheckReflection(runningcolors, reflected);
        }

        //
        public bool CheckRotation180(int[,] runningcolors, int[,] colorsolution)
        {
            if (runningcolors.GetLength(0) != colorsolution.GetLength(0) || runningcolors.GetLength(1) != colorsolution.GetLength(1))
                return false;
            bool refl = true;
            int[,] reflected = new int[colorsolution.GetLength(0), colorsolution.GetLength(1)];
            for (int i = 0; i < runningcolors.GetLength(0); i++)
            {
                for (int j = 0; j < runningcolors.GetLength(1); j++)
                {
                    reflected[i, j] = colorsolution[colorsolution.GetLength(0) - i - 1, colorsolution.GetLength(1) - j - 1];
                    if (runningcolors[i, j] != colorsolution[colorsolution.GetLength(0) - i - 1, colorsolution.GetLength(1) - j - 1])
                        refl = false;
                }
            }
            if (refl)
                return true;
            return CheckReflection(runningcolors, reflected);
        }

        //
        public bool CheckRotation270(int[,] runningcolors, int[,] colorsolution)
        {
            if (runningcolors.GetLength(1) != colorsolution.GetLength(0) || runningcolors.GetLength(0) != colorsolution.GetLength(1))
                return false;
            bool refl = true;
            int[,] reflected = new int[colorsolution.GetLength(1), colorsolution.GetLength(0)];
            for (int i = 0; i < runningcolors.GetLength(0); i++)
            {
                for (int j = 0; j < runningcolors.GetLength(1); j++)
                {
                    reflected[i, j] = colorsolution[colorsolution.GetLength(0) - j - 1, i];
                    if (runningcolors[i, j] != colorsolution[colorsolution.GetLength(0) - j - 1, i])
                        refl = false;
                }
            }
            if (refl)
                return true;
            return CheckReflection(runningcolors, reflected);
        }

        //Method to check if adding tile will overlap with any tiles already added
        public bool CheckOverlap(char[,] potential_solution, int col_offset, int row_offset, int g)
        {
            for (int i = 0; i < Orientations[g].Dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < Orientations[g].Dimensions.GetLength(1); j++)
                {
                    if (Orientations[g].Dimensions[i, j] != ' ')
                    {
                        if (potential_solution[i + col_offset, j + row_offset] != ' ')
                            return false;
                    }
                }
            }
            return true;
        }

        //Method which takes a potential solution and a placement of this tile on that solution space, adds the tile to that solution space
        public bool PlaceInSolution(char[,] potentialsolution, int[,] potentialcolors, int coloffset, int rowoffset, int g)
        {
            if (!CheckOverlap(potentialsolution, coloffset, rowoffset, g))
                return false;
            for (int i = 0; i < Orientations[g].Dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < Orientations[g].Dimensions.GetLength(1); j++)
                {
                    if (Orientations[g].Dimensions[i, j] != ' ')
                    {
                        potentialsolution[i + coloffset, j + rowoffset] = Orientations[g].Dimensions[i, j];
                        potentialcolors[i + coloffset, j + rowoffset] = Colorcode;
                    }
                }
            }
            return true;
        }

        //Takes the tile and removes it from the running possible solution array based on the current offsets being used
        public void RemoveFromSolution(char[,] potentialsolution, int[,] potentialcolors, int coloffset, int rowoffset, int g)
        {
            for (int i = 0; i < Orientations[g].Dimensions.GetLength(0); i++)
            {
                for (int j = 0; j < Orientations[g].Dimensions.GetLength(1); j++)
                {
                    if (Orientations[g].Dimensions[i, j] != ' ')
                    {
                        potentialsolution[i + coloffset, j + rowoffset] = ' ';
                        potentialcolors[i + coloffset, j + rowoffset] = -1;
                    }
                }
            }
        }

        //Method to find each unique rotation and reflection for this tile
        public void FindOrientations(char[,] initialorientation, int csize, int rsize)
        {
            Orientation o1 = new Orientation(initialorientation, csize, rsize);
            Orientations.Add(o1);
            Reflect(initialorientation);
            Rotate90(initialorientation);
            Rotate180(initialorientation);
            Rotate270(initialorientation);
        }

        //Method to ensure this possible orientation will fit within the solution space (not to long, etc.)
        public bool CheckDimensions(char[,] dims)
        {
            if (dims.GetLength(0) > cSol || dims.GetLength(1) > rSol)
                return false;
            return true;
        }

        //Takes the running solution and compares it to the solution Tile to see if the solution has placed an incorrect Tile
        public bool RunningCheck(char[,] possolution)
        {
            for (int i = 0; i < possolution.GetLength(0); i++)
            {
                for (int j = 0; j < possolution.GetLength(1); j++)
                {
                    if(possolution[i,j] != ' ' && possolution[i,j] != Dimensions[i, j])
                        return true;
                }
            }
            return false;
        }

        //Method which reflects a 2D char array, checks if its a unique orientation, and potentially adds it to the list of orientations
        public void Reflect(char[,] initialorientation)
        {
            char[,] reflected = new char[initialorientation.GetLength(0), initialorientation.GetLength(1)];
            for (int i = 0; i < initialorientation.GetLength(0); i++)
            {
                for(int j = 0; j < initialorientation.GetLength(1); j++)

                    reflected[i, j] = initialorientation[initialorientation.GetLength(0) - i - 1, j];
            }
            bool uni = true;
            for (int n = 0; n < Orientations.Count; n++)
            {
                if (Orientations[n].CheckSame(reflected))
                {
                    uni = false;
                    break;
                }
            }
            if (uni && CheckDimensions(reflected))
            {
                Orientation m = new Orientation(reflected, reflected.GetLength(0), reflected.GetLength(1));
                Orientations.Add(m);
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
                    rotated[i, j] = o[j, o.GetLength(1) - i - 1];
            }
            for (int n = 0; n < Orientations.Count; n++)
            {
                if (Orientations[n].CheckSame(rotated))
                {
                    uni = false;
                    r = n;
                    break;
                }
            }
            if (uni && CheckDimensions(rotated))
            {
                Orientation m = new Orientation(rotated, rotated.GetLength(0), rotated.GetLength(1));
                Orientations.Add(m);
                Reflect(rotated);
            }
            else
                Reflect(Orientations[r].Dimensions);
        }

        // 
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
            for (int n = 0; n < Orientations.Count; n++)
            {
                if (Orientations[n].CheckSame(rotated))
                {
                    uni = false;
                    r = n;
                    break;
                }
            }
            if (uni && CheckDimensions(rotated))
            {
                Orientation m = new Orientation(rotated, rotated.GetLength(0), rotated.GetLength(1));
                Orientations.Add(m);
                Reflect(rotated);
            }
            else
            {
                Reflect(Orientations[r].Dimensions);
            }
        }

        //
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
            for (int n = 0; n < Orientations.Count; n++)
            {
                if (Orientations[n].CheckSame(rotated))
                {
                    uni = false;
                    r = n;
                    break;
                }
            }
            if (uni && CheckDimensions(rotated))
            {
                Orientation m = new Orientation(rotated, rotated.GetLength(0), rotated.GetLength(1));
                Orientations.Add(m);
                Reflect(rotated);
            }
            else
            {
                Reflect(Orientations[r].Dimensions);
            }
        }
        //Method which takes a piece that has no symmetry and fixes it in one orientation to limit redundant solution paths
        public void Fix(bool rotl, bool refl, bool turn)
        {
            if (rotl && refl)
            {
                Orientation o = Orientations[0];
                for (int i = 0; i < Orientations.Count; i++)
                {
                    if (Orientations[i].Topleft == 0)
                    {
                        o = Orientations[i];
                        break;
                    }
                }
                Orientations.RemoveRange(0, Orientations.Count);
                Orientations.Add(o);
            }
            else if (refl && turn && !rotl)
            {
                Orientations.RemoveAt(7);
                Orientations.RemoveAt(6);
                Orientations.RemoveAt(5);
                Orientations.RemoveAt(4);
                Orientations.RemoveAt(3);
                Orientations.RemoveAt(1);
            }
            else if (refl && !turn)
            {
                Orientations.RemoveAt(7);
                Orientations.RemoveAt(5);
                Orientations.RemoveAt(3);
                Orientations.RemoveAt(1);

            }
            else if (turn && !refl)
            {
                Orientations.RemoveAt(7);
                Orientations.RemoveAt(6);
                Orientations.RemoveAt(5);
                Orientations.RemoveAt(4);
            }
        }

        //Method to take the Tile dimensions and shrink them to the smallest possible rectangle around the Tile
        public void Compress()
        {
            int imin = cSize;
            int jmin = rSize;
            int imax = 0;
            int jmax = 0;
            for (int i = 0; i < cSize; i++)
            {
                for (int j = 0; j < rSize; j++)
                {
                    if (Dimensions[i, j] != ' ')
                    {
                        if (i < imin)
                            imin = i;
                        if (j < jmin)
                            jmin = j;
                        if (i > imax)
                            imax = i;
                        if (j > jmax)
                            jmax = j;
                    }
                }
            }
            char[,] new_dim = new char[imax - imin + 1, jmax - jmin + 1];
            this.rSize = jmax - jmin + 1;
            this.cSize = imax - imin + 1;
            for (int n = 0; n < this.cSize; n++)
            {
                for (int m = 0; m < this.rSize; m++)
                    new_dim[n, m] = Dimensions[n + imin, m + jmin];
            }
            this.Dimensions = new_dim;
        }
    }
}