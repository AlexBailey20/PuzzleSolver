using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    public class GUIEventArgs : EventArgs
    {
        public string message { get; set; }
    }

    public class ApplicationController
    {
        private static GUI ui = null;
        private static Parser parser = null;
        private string filename = null;
        private string filepath = null;

        public delegate void EventHandler(object sender, GUIEventArgs e);
        public event EventHandler OnEvent;

        public static GUI UI
        {
            get { return ui; }
            set { ui = value; }
        }

        public static Parser Parser
        {
            get { return parser; }
            set { parser = value; }
        }

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

        public void UpdateFile(string name, string path)
        {
            Filename = name;
            Filepath = path;
            Parser.Update(Filename, Filepath);
            Log("Filename updated to: " + Filename + ". Filepath updated to: " + Filepath + ".");
            Parser.Parse();
        }

        public void UpdateTileDisplays()
        {

        }

        public void RunSearch()
        {

            int i = Parser.CheckSizes();
            if (i == -1)
            {
                Console.WriteLine("No solution possible");
            }
            else if (i == 0)
            {
                Console.WriteLine("Some pieces may not be used if a solution is found");
            }
            Parser.CheckDuplicateTiles();
            List<Tile> options = new List<Tile>();
            char[,] blanksolution = new char[Parser.Solution.cSize, Parser.Solution.rSize];
            int[,] blankcolors = new int[Parser.Solution.cSize, Parser.Solution.rSize];
            bool rotl = false;
            bool refl = false;
            bool turn = false;
            for (int k = 0; k < Parser.Solution.cSize; k++)
            {
                for (int j = 0; j < Parser.Solution.rSize; j++)
                {
                    blanksolution[k, j] = ' ';
                    blankcolors[k, j] = -1;
                }
            }
            foreach (Tile t in Parser.Pieces)
            {
                if (t.Solution == false)
                {
                    options.Add(t);
                    Parser.Pent = Parser.Pent && (t.Size == 5);
                }
                if (t.Solution)
                {
                    rotl = t.CheckRotationalSymmetry();
                    refl = t.CheckReflectedSymmetry();
                    turn = t.Check180Symmetry();
                }
            }
            options.Reverse();
            for(int w = 0; w < options.Count; w++)
            {
                if(options[w].Orientations.Count == 8)
                {
                    options[w].Fix(rotl, refl, turn);
                    break;
                }
            }  
            List<char[,]> foundsolutions = new List<char[,]>();
            if (options[0].Size > Parser.Solution.Size / 2)
            {
                Parser.Puzzlesolutions = Parser.SolutionRecursion(options, foundsolutions, blanksolution, blankcolors, Parser.Solution.cSize, Parser.Solution.rSize);
            }
            else if(i == 0)
            {
                Parser.SolutionBuildingRecursionSubsets(options, blanksolution, blankcolors, 0, 0, 0);
            }
            else
            {
                Parser.SolutionBuildingRecursion(options, blanksolution, blankcolors, 0, 0);
            }
            for (int x = 0; x < Parser.Puzzlesolutions.Count; x++)
            {
                Console.WriteLine("Solution " + (x + 1));
                for (int y = 0; y < Parser.Solution.cSize; y++)
                {
                    for (int z = 0; z < Parser.Solution.rSize; z++)
                    {
                        Console.Write(Parser.Puzzlesolutions[x][y, z]);
                    }
                    Console.WriteLine();
                }
            }
            for (int q = 0; q < Parser.Colorcodes.Count; q++)
            {
                Console.WriteLine("Color_Codes " + (q + 1));
                for (int s = 0; s < Parser.Solution.cSize; s++)
                {
                    for (int f = 0; f < Parser.Solution.rSize; f++)
                    {
                        Console.Write(Parser.Colorcodes[q][s, f]);
                    }
                    Console.WriteLine();
                }
            }
            if(Parser.Colorcodes.Count == 0)
            {
                Console.WriteLine("No solution found");
            }
            Console.ReadKey();
            Console.ReadKey();
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private void Log(string message)
        {
            GUIEventArgs args = new GUIEventArgs();
            args.message = message;
            if (OnEvent != null)
            {
                OnEvent(this, args);
            }
        }

        [STAThread]
        static void Main()
        {
            UI = new GUI();
            Parser = new Parser();
            Application.Run(UI);
        }
    }
}
