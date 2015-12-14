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
        private Tile target;
        private List<Tile> pieces;
        private List<int[,]> colorcodes;

        public int Smallest
        {
            get { return smallest; }
            set { smallest = value; }
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
        }

        //Check all the sizes of tiles to see if solution tile is the same size or smaller than sum of other tiles
        //If smaller, check to see if equal to some sum of other tile sizes
        public int CheckSizes()
        {
            Pieces[Pieces.Count - 1].Solution = true;
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
            {
                sum += Pieces[i].Size;
            }
            if (sum == solsize)
            {
                return 1;
            }
            else if (sum < solsize)
            {
                return -1;
            }
            else {
                /*             int diff = sum - sol_size;
                               List<Tile> removable_tiles = new List<Tile>;
                               if(pieces[0].size > diff){
                                   return -1;
                               }
                               foreach(Tile t in pieces){
                                   if(t.size == diff){
                                       return 0;
                                   }
                                   else if(t.size < diff){

                                   } */
                return 0;
            }
        }

        public void CheckDuplicateTiles()
        {
            for (int i = 1; i < Pieces.Count; i++)
                if (Pieces[i].Size == Pieces[i - 1].Size)
                {
                    Pieces[i].CheckIsomorphic(Pieces[i - 1]);
                }
        }

        //Recursive function to check every combination of tile placements. Starting from the biggest tile, from each position that tile can take it calls the
        //function on the next biggest tile. If at any point there is overlap, the tile is removed and the next possible branch is explored.
        //Once a possible solution is found, it is checked against the solution tile and added (if valid) to the solution list which is returned
        public List<char[,]> SolutionRecursion(List<Tile> pieces, List<char[,]> solutions, char[,] runningsolution, int[,] runningcolors, int csize, int rsize)
        {
            List<Tile> smallerpieces = new List<Tile>();
            for (int t = 1; t < pieces.Count; t++)
            {
                smallerpieces.Add(pieces[t]);
            }
            Tile biggest = pieces[0];
            bool found = false;
            for (int g = 0; g < biggest.Orientations.Count; g++)
            {
                for (int i = 0; i < biggest.Orientations[g].cOff; i++)
                {
                    for (int j = 0; j < biggest.Orientations[g].rOff; j++)
                    {
                        if (biggest.PlaceInSolution(runningsolution, runningcolors, i, j, g))
                        {
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
                                    solutions.Add(newsolution);
                                    Colorcodes.Add(newcolors);
                                    found = true;
                                }
                                biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                                if (found)
                                {
                                    break;
                                }
                            }
                            else {
                                SolutionRecursion(smallerpieces, solutions, runningsolution, runningcolors, csize, rsize);
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
            return solutions;
        }

        public List<int[,]> Solve()
        {
            int i = CheckSizes();
            if (i == -1)
                Console.WriteLine("No target possible");
            else if (i == 0)
                Console.WriteLine("Some pieces may not be used if a target is found");
            CheckDuplicateTiles();
            List<Tile> options = new List<Tile>();
            char[,] blanksolution = new char[Target.cSize, Target.rSize];
            int[,] blankcolors = new int[Target.cSize, Target.rSize];
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
                if (t.Solution == false)
                {
                    options.Add(t);
                }
            }
            options.Reverse();
            List<char[,]> foundsolutions = new List<char[,]>();
            // solus and printing loops below will deprecate once display is upgraded
            List<char[,]> solus = SolutionRecursion(options, foundsolutions, blanksolution, blankcolors, Target.cSize, Target.rSize);
            Console.Out.WriteLine(solus.ToArray().ToString());
            return Colorcodes;
        }
    }
}
