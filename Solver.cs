using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    public class Solver
    {
        private int smallest;
        private int biggest;
        private int solcsize;
        private int solrsize;
        private bool pent;
        private bool rotate;
        private Tile target;
        private List<Tile> pieces;
        private List<int[,]> colorcodes;

        public int Smallest
        {
            get { return smallest; }
            set { smallest = value; }
        }

        public int Biggest
        {
            get { return biggest; }
            set { biggest = value; }
        }

        public int solCSize
        {
            get { return solcsize; }
            set { solcsize = value; }
        }

        public int solRSize
        {
            get { return solrsize; }
            set { solrsize = value; }
        }

        public bool Pent
        {
            get { return pent; }
            set { pent = value; }
        }

        public bool Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }

        public Tile Target
        {
            get { return target; }
            set { target = value; }
        }

        public List<Tile> Pieces
        {
            get { return pieces; }
            set { pieces = value; }
        }

        public List<int[,]> Colorcodes
        {
            get { return colorcodes; }
            set { colorcodes = value; }
        }

        public Solver()
        {
            Smallest = 0;
            Biggest = 0;
            solCSize = 0;
            solRSize = 0;
            Rotate = false;
            Pent = true;
            Pieces = new List<Tile>();
            Colorcodes = new List<int[,]>();
        }

        public void Update(Tile targettile, List<Tile> tilepieces)
        {
            if (Target == null)
                Target = new Tile(targettile);
            else
                Target = targettile;
            Pieces = tilepieces;
            // 
            Pieces[Pieces.Count - 1].Target = true;
            Target = Pieces[Pieces.Count - 1];
        }

        //Check all the sizes of tiles to see if solution tile is the same size or smaller than sum of other tiles
        //If smaller, check to see if equal to some sum of other tile sizes
        public int CheckSizes()
        {
            Rotate = Target.CheckDimensionRotation();
            solCSize = Target.Dimensions.GetLength(0);
            solRSize = Target.Dimensions.GetLength(1);
            Smallest = Pieces[0].Size;
            int solcolumns = Pieces[Pieces.Count - 1].cSize;
            int solrows = Pieces[Pieces.Count - 1].rSize;
            foreach (Tile t in Pieces)
            {
                t.cSol = solcolumns;
                t.rSol = solrows;
                t.FindOrientations(t.Dimensions, t.cSize, t.rSize);
                t.FindPositions();
            }
            int solsize = Pieces[Pieces.Count - 1].Size;
            int sum = 0;
            for (int i = 0; i < Pieces.Count - 1; i++)
                sum += Pieces[i].Size;
            if (sum == solsize)
                return 1;
            else if (sum < solsize)
                return -1;
            else
                return 0;
        }

        public void CheckDuplicateTiles()
        {
            for (int i = 1; i < Pieces.Count; i++)
                if (Pieces[i].Size == Pieces[i - 1].Size)
                {
                    Pieces[i].CheckIsomorphic(Pieces[i - 1]);
                }
        }

        public bool EmptySpaceCheck(bool[,] spaces, int csize, int rsize)
        {
            int small = -1;
            int big = -1;
            for (int nn = 0; nn < csize; nn++)
            {
                for (int mm = 0; mm < rsize; mm++)
                {
                    if (spaces[nn, mm] == true)
                    {
                        int i = SpaceRecursion(spaces, nn, mm, csize, rsize);
                        if (i % 5 != 0 && pent)
                            return true;
                        if (i < small || small == -1)
                            small = i;
                        if (i > big)
                            big = i;
                    }
                }
            }
            return (small < Smallest || big < Biggest);
        }

        public int SpaceRecursion(bool[,] spaces, int i, int j, int csize, int rsize)
        {
            if (i < 0 || j < 0 || i >= csize || j >= rsize)
                return 0;
            else if (spaces[i, j] == false)
                return 0;
            else
            {
                spaces[i, j] = false;
                return (1 + SpaceRecursion(spaces, i - 1, j, csize, rsize) + SpaceRecursion(spaces, i, j - 1, csize, rsize) + SpaceRecursion(spaces, i, j + 1, csize, rsize) + SpaceRecursion(spaces, i + 1, j, csize, rsize));
            }
        }

        //Recursive function to check every combination of tile placements. Starting from the biggest tile, from each position that tile can take it calls the
        //function on the next biggest tile. If at any point there is overlap, the tile is removed and the next possible branch is explored.
        //Once a possible solution is found, it is checked against the solution tile and added (if valid) to the solution list which is returned
        public void SolutionRecursion(List<Tile> pieces, List<int[,]> codes, char[,] runningsolution, int[,] runningcolors, int csize, int rsize)
        {
            List<Tile> smallerpieces = new List<Tile>();
            for (int t = 1; t < pieces.Count; t++)
                smallerpieces.Add(pieces[t]);
            Tile biggest = pieces[0];
            Biggest = biggest.Size;
            if (pieces.Count > 1)
                Biggest = pieces[1].Size;
            bool found = false;
            bool[,] spaces = new bool[csize, rsize];
            for (int g = 0; g < biggest.Orientations.Count; g++)
            {
                for (int i = 0; i < biggest.Orientations[g].cOff; i++)
                {
                    for (int j = 0; j < biggest.Orientations[g].rOff; j++)
                    {
                        if (biggest.PlaceInSolution(runningsolution, runningcolors, i, j, g))
                        {
                            if (smallerpieces.Count > 0)
                            {
                                for (int ii = 0; ii < csize; ii++)
                                {
                                    for (int jj = 0; jj < rsize; jj++)
                                    {
                                        spaces[ii, jj] = (runningsolution[ii, jj] == ' ' && Target.Dimensions[ii, jj] != ' ');
                                    }
                                }
                                if (EmptySpaceCheck(spaces, csize, rsize))
                                {
                                    biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                                    continue;
                                }
                            }
                            if (Target.RunningCheck(runningsolution))
                            {
                                biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                                continue;
                            }
                            if (smallerpieces.Count == 0)
                            {
                                if (Target.CheckValid(runningsolution) && Target.CheckNewSolution(runningcolors, Colorcodes))
                                {
                                    char[,] newsolution = new char[csize, rsize];
                                    int[,] newcolors = new int[csize, rsize];
                                    for (int n = 0; n < csize; n++)
                                    {
                                        for (int m = 0; m < rsize; m++)
                                        {
                                            newsolution[n, m] = runningsolution[n, m];
                                            newcolors[n, m] = runningcolors[n, m];
                                        }
                                    }
                                    Colorcodes.Add(newcolors);
                                    Console.WriteLine(Colorcodes.Count);
                                    found = true;
                                }
                                biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                                if (found)
                                {
                                    break;
                                }
                            }
                            else {
                                SolutionRecursion(smallerpieces, Colorcodes, runningsolution, runningcolors, csize, rsize);
                                biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                            }
                        }
                        if (found)
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        found = false;
                        break;
                    }
                }
            }
        }

        public void SolutionBuildingRecursion(List<Tile> puzzle_pieces, char[,] running_solution, int[,] running_colors, int ii, int jj)
        {
            int j = jj;
            bool[,] spaces = new bool[solCSize, solRSize];
            for (int i = ii; i < solCSize; i++)
            {
                while (j < solRSize)
                {
                    if (running_solution[i, j] == ' ' && Target.Dimensions[i, j] != ' ')
                    {
                        for (int n = 0; n < puzzle_pieces.Count; n++)
                        {
                            for (int m = 0; m < puzzle_pieces[n].Orientations.Count; m++)
                            {
                                if (puzzle_pieces[n].Orientations[m].PlaceInSolution(running_solution, running_colors, i, j, puzzle_pieces[n].Colorcode))
                                {
                                    /*       for(int x = 0; x < sol_csize; x++)
                                           {
                                               for(int y = 0; y < sol_rsize; y++)
                                               {
                                                   Console.Write(running_solution[x, y]);
                                               }
                                               Console.WriteLine();
                                           }
                                        Console.WriteLine();
                                        Console.ReadKey(); */
                                    if (Target.RunningCheck(running_solution))
                                    {
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        continue;
                                    }
                                    if (puzzle_pieces.Count > 1)
                                    {
                                        for (int iii = 0; iii < solCSize; iii++)
                                        {
                                            for (int jjj = 0; jjj < solRSize; jjj++)
                                            {
                                                spaces[iii, jjj] = (running_solution[iii, jjj] == ' ' && Target.Dimensions[iii, jjj] != ' ');
                                            }
                                        }
                                        if (EmptySpaceCheck(spaces, solCSize, solRSize))
                                        {
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            continue;
                                        }
                                    }
                                    if (puzzle_pieces.Count > 1)
                                    {
                                        List<Tile> smaller_pieces = new List<Tile>();
                                        for (int t = 0; t < puzzle_pieces.Count; t++)
                                        {
                                            if (t != n)
                                            {
                                                smaller_pieces.Add(puzzle_pieces[t]);
                                            }
                                        }
                                        SolutionBuildingRecursion(smaller_pieces, running_solution, running_colors, i, j);
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                    }
                                    else if (puzzle_pieces.Count == 1)
                                    {
                                        if (Target.CheckValid(running_solution) && Target.CheckNewSolution(running_colors, Colorcodes))
                                        {
                                            int[,] new_colors = new int[solCSize, solRSize];
                                            for (int x = 0; x < solCSize; x++)
                                            {
                                                for (int y = 0; y < solRSize; y++)
                                                {
                                                    new_colors[x, y] = running_colors[x, y];
                                                }
                                            }
                                            Console.Out.WriteLine(new_colors.Length);
                                            Colorcodes.Add(new_colors);
                                            Console.WriteLine(Colorcodes.Count);
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            return;
                                        }
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                    }
                                }
                            }
                            if (n == puzzle_pieces.Count - 1)
                            {
                                return;
                            }
                        }
                    }
                    j++;
                }
                j = 0;
            }
        }

        public void SolutionBuildingRecursionSubsets(List<Tile> puzzle_pieces, char[,] running_solution, int[,] running_colors, int ii, int jj, int sum)
        {
            int j = jj;
            bool[,] spaces = new bool[solCSize, solRSize];
            for (int i = ii; i < solCSize; i++)
            {
                while (j < solRSize)
                {
                    if (running_solution[i, j] == ' ' && Target.Dimensions[i, j] != ' ')
                    {
                        for (int n = 0; n < puzzle_pieces.Count; n++)
                        {
                            if (puzzle_pieces[n].Size + sum > Target.Size)
                            {
                                continue;
                            }
                            for (int m = 0; m < puzzle_pieces[n].Orientations.Count; m++)
                            {
                                if (puzzle_pieces[n].Orientations[m].PlaceInSolution(running_solution, running_colors, i, j, puzzle_pieces[n].Colorcode))
                                {
                                    sum += puzzle_pieces[n].Size;
                                    if (Target.RunningCheck(running_solution))
                                    {
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        sum -= puzzle_pieces[n].Size;
                                        continue;
                                    }
                                    if (puzzle_pieces.Count > 1 && sum < Target.Size)
                                    {
                                        for (int iii = 0; iii < solCSize; iii++)
                                        {
                                            for (int jjj = 0; jjj < solRSize; jjj++)
                                            {
                                                spaces[iii, jjj] = (running_solution[iii, jjj] == ' ' && Target.Dimensions[iii, jjj] != ' ');
                                            }
                                        }
                                        if (EmptySpaceCheck(spaces, solCSize, solRSize))
                                        {
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            sum -= puzzle_pieces[n].Size;
                                            continue;
                                        }
                                    }
                                    if (puzzle_pieces.Count > 1 && sum < Target.Size)
                                    {
                                        List<Tile> smaller_pieces = new List<Tile>();
                                        for (int t = 0; t < puzzle_pieces.Count; t++)
                                        {
                                            if (t != n)
                                            {
                                                smaller_pieces.Add(puzzle_pieces[t]);
                                            }
                                        }
                                        SolutionBuildingRecursionSubsets(smaller_pieces, running_solution, running_colors, i, j, sum);
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        sum -= puzzle_pieces[n].Size;
                                    }
                                    else if (puzzle_pieces.Count == 1 || sum == Target.Size)
                                    {
                                        if (Target.CheckValid(running_solution) && Target.CheckNewSolution(running_colors, colorcodes))
                                        {
                                            char[,] new_solution = new char[solCSize, solRSize];
                                            int[,] new_colors = new int[solCSize, solRSize];
                                            for (int x = 0; x < solCSize; x++)
                                            {
                                                for (int y = 0; y < solRSize; y++)
                                                {
                                                    new_solution[x, y] = running_solution[x, y];
                                                    new_colors[x, y] = running_colors[x, y];
                                                }
                                            }
                                            Colorcodes.Add(new_colors);
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            return;
                                        }
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        sum -= puzzle_pieces[n].Size;
                                    }
                                }
                            }
                            if (n == puzzle_pieces.Count - 1)
                            {
                                return;
                            }
                        }
                    }
                    j++;
                }
                j = 0;
            }
        }

        public int Solve()
        {
            int i = CheckSizes();
            if (i == -1)
                return 0;               // case 1: no solution possible
            else if (i == 0)
                Console.WriteLine("Some pieces may not be used if a target is found");
            CheckDuplicateTiles();
            List<Tile> options = new List<Tile>();
            char[,] blanksolution = new char[Target.cSize, Target.rSize];
            int[,] blankcolors = new int[Target.cSize, Target.rSize];
            bool rotl = false;
            bool refl = false;
            bool turn = false;
            for (int k = 0; k < Target.cSize; k++)
            {
                for (int j = 0; j < Target.rSize; j++)
                {
                    blanksolution[k, j] = ' ';
                    blankcolors[k, j] = -1;
                }
            }
            foreach (Tile t in Pieces)
            {
                if (t.Target)
                {
                    rotl = t.CheckRotationalSymmetry();
                    refl = t.CheckReflectedSymmetry();
                    turn = t.Check180Symmetry();
                }
                else
                {
                    options.Add(t);
                    Pent = Pent && (t.Size == 5);
                }
            }
            options.Reverse();
            for (int w = 0; w < options.Count; w++)
            {
                if (options[w].Orientations.Count == 8)
                {
                    options[w].Fix(rotl, refl, turn);
                    break;
                }
            }
            if (options[0].Size > Target.Size / 2)
            {
            SolutionRecursion(options, Colorcodes, blanksolution, blankcolors, Target.cSize, Target.rSize);
            }
            else if (i == 0)
            {
                SolutionBuildingRecursionSubsets(options, blanksolution, blankcolors, 0, 0, 0);
            }
            else
            {
                SolutionBuildingRecursion(options, blanksolution, blankcolors, 0, 0);
            }
            
            if (Colorcodes.Count == 0)
                return 1;               // case 1: no solution found              
            return 2;                   // case 2: solution(s) found
        }
    }
}
