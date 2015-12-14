using System;
using System.IO;
using System.Collections.Generic;
namespace PuzzleSolver
{
    public class Parser
    {
        private string filename = null;
        private string filepath = null;
        private bool rotate_solution;
        private List<Tile> pieces;
        private List<int[,]> colorcodes;
        private List<char[,]> puzzlesolutions;
        private Tile solution;
        private int smallest_size;
        private int sol_csize;
        private int sol_rsize;
        private int biggest_size;
        private bool pent;
        private int code;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public string Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        public bool Pent
        {
            get { return pent; }
            set { pent = value; }
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
        public List<char[,]> Puzzlesolutions
        {
            get { return puzzlesolutions; }
            set { puzzlesolutions = value; }
        }
        public Tile Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        public int Smallest
        {
            get { return smallest_size; }
            set { smallest_size = value; }
        }

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        // constructor
        public Parser()
        {
            Filename = "";
            Filepath = "";
            rotate_solution = false;
            pent = true;
            sol_csize = 0;
            sol_rsize = 0;
            puzzlesolutions = new List<char[,]>();
            smallest_size = 0;
            biggest_size = 0;
            Code = 0;
            Pieces = new List<Tile>();
            Colorcodes = new List<int[,]>();
        }

        // updates Parser's Filename and Filepath properties
        public void Update(string name, string path)
        {
            Filename = name;
            Filepath = path;
        }

        //Read the file (file is currently hardcoded in), return all the lines as an array of strings
        public string[] ReadFile()
        {
            StreamReader reader = File.OpenText(Filename);
            string line;
            List<string> allLines = new List<string>();
            while ((line = reader.ReadLine()) != null)
                allLines.Add(line);
            return allLines.ToArray();
        }

        //Takes the string array of all lines, finds the number of rows and the max row length to create rectangular 2D array
        //Transfers the string array into the 2D array of chars, iterates through each char in the array, if the char is significant, send it to CreateTile()
        //Result of CreateTile() is used in the constructor of a new Tile()
        public void Parse()
        {
            string[] allLines = ReadFile();
            int colLength = allLines.Length;
            int rowLength = allLines[0].Length;
            foreach (string s in allLines)
            {
                if (rowLength < s.Length)
                {
                    rowLength = s.Length;
                }
            }
            char[,] lines = new char[colLength, rowLength];
            int i = 0;
            int j = 0;
            foreach (string s in allLines)
            {
                j = 0;
                foreach (char c in s)
                {
                    lines[i, j] = c;
                    j++;
                }
                for (int k = j; k < rowLength; k++)
                {
                    lines[i, k] = ' ';
                }
                i++;
            }
            for (int n = 0; n < colLength; n++)
            {
                for (int m = 0; m < rowLength; m++)
                {
                    if (lines[n, m] != ' ')
                    {
                        char[,] newTile = new char[colLength, rowLength];
                        for (int q = 0; q < colLength; q++)
                        {
                            for (int w = 0; w < rowLength; w++)
                            {
                                newTile[q, w] = ' ';
                            }
                        }
                        CreateTile(newTile, lines, n, m, colLength, rowLength);
                        Tile t = new Tile(newTile, rowLength, colLength, code);
                        Code += 1;
                        Pieces.Add(t);
                    }
                }
            }
            Pieces.Sort();
        }

        public void PrintSol(char[,] runningsolution)
        {
            for (int i = 0; i < runningsolution.GetLength(0); i++)
            {
                for (int j = 0; j < runningsolution.GetLength(1); j++)
                {
                    Console.Write(runningsolution[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ReadKey();
        }

        //Recursive function, takes an all space 2D array and the parsed 2D array (same size), the two indices, and the array sizes (for convenience)
        //Changes the value at i,j in tile to the value at i,j in all_tiles, sets that value to be insignificant in alltiles, then checks all relevant directions
        //For the significance of the char at each of those spots, calling itself on that char if that char is significant
        public void CreateTile(char[,] tile, char[,] alltiles, int i, int j, int cl, int rl)
        {
            tile[i, j] = alltiles[i, j];
            alltiles[i, j] = ' ';
            if (i > 0)
            {
                if (alltiles[i - 1, j] != ' ')
                {
                    CreateTile(tile, alltiles, i - 1, j, cl, rl);
                }
            }
            if (j > 0)
            {
                if (alltiles[i, j - 1] != ' ')
                {
                    CreateTile(tile, alltiles, i, j - 1, cl, rl);
                }
            }
            if (i < (cl - 1))
            {
                if (alltiles[i + 1, j] != ' ')
                {
                    CreateTile(tile, alltiles, i + 1, j, cl, rl);
                }
            }
            if (j < (rl - 1))
            {
                if (alltiles[i, j + 1] != ' ')
                {
                    CreateTile(tile, alltiles, i, j + 1, cl, rl);
                }
            }
        }

        //Check all the sizes of tiles to see if solution tile is the same size or smaller than sum of other tiles
        //If smaller, check to see if equal to some sum of other tile sizes
        //Also sets largest tile as the solution piece
        public int CheckSizes()
        {
            Pieces[Pieces.Count - 1].Solution = true;
            Solution = Pieces[Pieces.Count - 1];
            rotate_solution = Solution.CheckDimensionRotation();
            sol_csize = Solution.Dimensions.GetLength(0);
            sol_rsize = Solution.Dimensions.GetLength(1);
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
        public bool EmptySpaceCheck(bool[,] spaces, int csize, int rsize)
        {
            int small = -1;
            int big = -1;
            for(int nn = 0; nn<csize; nn++){
                for(int mm = 0; mm<rsize; mm++){
                    if(spaces[nn, mm] == true){
                        int i = SpaceRecursion(spaces, nn, mm, csize, rsize);
                        if(i % 5 != 0 && pent){
                            return true;
                        }
                        if(i<small || small == -1){
                            small = i;
                        }
                        if( i > big){
                            big = i;
                        }
                    }
                }
            }
            return (small<smallest_size || big<biggest_size);
        }
        public int SpaceRecursion(bool[,] spaces, int i, int j, int csize, int rsize)
        {
            if (i < 0 || j < 0 || i >= csize || j >= rsize)
            {
                return 0;
            }
            else if (spaces[i, j] == false)
                {
                    return 0;
                }
                else {
                    spaces[i, j] = false;
                    return (1 + SpaceRecursion(spaces, i - 1, j, csize, rsize) + SpaceRecursion(spaces, i, j - 1, csize, rsize) + SpaceRecursion(spaces, i, j + 1, csize, rsize) + SpaceRecursion(spaces, i + 1, j, csize, rsize));
                }
        }
        public void SolutionBuildingRecursion(List<Tile> puzzle_pieces, char[,] running_solution, int[,] running_colors, int ii, int jj)
        {
            int j = jj;
            bool[,] spaces = new bool[sol_csize, sol_rsize];
            for (int i = ii; i < sol_csize; i++)
            {
                while (j < sol_rsize)
                {
                    if (running_solution[i, j] == ' ' && solution.Dimensions[i, j] != ' ')
                    {
                        for (int n = 0; n < puzzle_pieces.Count; n++)
                        {
                            for (int m = 0; m<puzzle_pieces[n].Orientations.Count; m++)
                            {
								if(puzzle_pieces[n].Orientations[m].PlaceInSolution(running_solution, running_colors, i, j, puzzle_pieces[n].ColorCode))
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
                                    if (solution.RunningCheck(running_solution))
                                    {
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        continue;
                                    }
                                    if (puzzle_pieces.Count > 1)
                                    {
                                        for (int iii = 0; iii<sol_csize; iii++)
                                        {
                                            for (int jjj = 0; jjj<sol_rsize; jjj++)
                                            {
                                                spaces[iii, jjj] = (running_solution[iii, jjj] == ' ' && solution.Dimensions[iii, jjj] != ' ');
                                            }
                                        }
                                        if (EmptySpaceCheck(spaces, sol_csize, sol_rsize))
                                        {
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            continue;
                                        }
                                    }
                                            if (puzzle_pieces.Count > 1)
                                    {
                                        List<Tile> smaller_pieces = new List<Tile>();
                                        for (int t = 0; t<puzzle_pieces.Count; t++)
                                        {
                                            if (t != n)
                                            {
                                                smaller_pieces.Add(puzzle_pieces[t]);
                                            }
                                        }
                                        SolutionBuildingRecursion(smaller_pieces, running_solution, running_colors, i, j);
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        }
									    else if(puzzle_pieces.Count == 1)
                                        {
                                            if (solution.CheckValid(running_solution) && solution.CheckNewSolution(running_colors, colorcodes))
                                                {
                                                char[,] new_solution = new char[sol_csize, sol_rsize];
                                                int[,] new_colors = new int[sol_csize, sol_rsize];
                                                for (int x = 0; x<sol_csize; x++)
                                                {
                                                    for (int y = 0; y<sol_rsize; y++)
                                                    {
                                                        new_solution[x, y] = running_solution[x, y];
                                                        new_colors[x, y] = running_colors[x, y];
                                                    }
                                                 }
                                            puzzlesolutions.Add(new_solution);
                                            colorcodes.Add(new_colors);
                                            Console.WriteLine(puzzlesolutions.Count);
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            return;
                                        }
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                    }
                                }
                            }
                            if (n == puzzle_pieces.Count -1)
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
            bool[,] spaces = new bool[sol_csize, sol_rsize];
            for (int i = ii; i < sol_csize; i++)
            {
                while (j < sol_rsize)
                {
                    if (running_solution[i, j] == ' ' && solution.Dimensions[i, j] != ' ')
                    {
                        for (int n = 0; n < puzzle_pieces.Count; n++)
                        {
                            if(puzzle_pieces[n].Size + sum > Solution.Size)
                            {
                                continue;
                            }
                            for (int m = 0; m < puzzle_pieces[n].Orientations.Count; m++)
                            {
                                if (puzzle_pieces[n].Orientations[m].PlaceInSolution(running_solution, running_colors, i, j, puzzle_pieces[n].ColorCode))
                                {
                                    sum += puzzle_pieces[n].Size;
                                    if (solution.RunningCheck(running_solution))
                                    {
                                        puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                        sum -= puzzle_pieces[n].Size;
                                        continue;
                                    }
                                    if (puzzle_pieces.Count > 1 && sum < Solution.Size)
                                    {
                                        for (int iii = 0; iii < sol_csize; iii++)
                                        {
                                            for (int jjj = 0; jjj < sol_rsize; jjj++)
                                            {
                                                spaces[iii, jjj] = (running_solution[iii, jjj] == ' ' && solution.Dimensions[iii, jjj] != ' ');
                                            }
                                        }
                                        if (EmptySpaceCheck(spaces, sol_csize, sol_rsize))
                                        {
                                            puzzle_pieces[n].Orientations[m].RemoveFromSolution(running_solution, running_colors, i, j);
                                            sum -= puzzle_pieces[n].Size;
                                            continue;
                                        }
                                    }
                                    if (puzzle_pieces.Count > 1 && sum < Solution.Size)
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
                                    else if (puzzle_pieces.Count == 1 || sum == Solution.Size)
                                    {
                                        if (solution.CheckValid(running_solution) && solution.CheckNewSolution(running_colors, colorcodes))
                                        {
                                            char[,] new_solution = new char[sol_csize, sol_rsize];
                                            int[,] new_colors = new int[sol_csize, sol_rsize];
                                            for (int x = 0; x < sol_csize; x++)
                                            {
                                                for (int y = 0; y < sol_rsize; y++)
                                                {
                                                    new_solution[x, y] = running_solution[x, y];
                                                    new_colors[x, y] = running_colors[x, y];
                                                }
                                            }
                                            puzzlesolutions.Add(new_solution);
                                            colorcodes.Add(new_colors);
                                            Console.WriteLine(puzzlesolutions.Count);
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
            biggest_size = biggest.Size;
            if(pieces.Count > 1)
            {
                biggest_size = pieces[1].Size;
            }
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
                                        spaces[ii, jj] = (runningsolution[ii, jj] == ' ' && solution.Dimensions[ii, jj] != ' ');
                                    }
                                }
                                if (EmptySpaceCheck(spaces, csize, rsize))
                                {
                                    biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                                    continue;
                                }
                            }
                            if (Solution.RunningCheck(runningsolution))
                            {
                                biggest.RemoveFromSolution(runningsolution, runningcolors, i, j, g);
                                continue;
                            }
                            if (smallerpieces.Count == 0)
                            {
                                if (Solution.CheckValid(runningsolution) && Solution.CheckNewSolution(runningcolors, Colorcodes))
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
                                    Console.WriteLine(solutions.Count);
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
    }
}