using System;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace PuzzleSolver
{

    /// <summary>
    /// The controller for the PuzzleSolver application.
    /// </summary>
    public class ApplicationController
    {
        private static GUI _ui = null;
        private static Parser _parser = null;
        private static Solver _solver = null;
        private static System.Timers.Timer _timer = null;
        private string _filename;

        /// <summary>
        /// The application's GUI.
        /// </summary>
        public static GUI UI
        {
            get { return _ui; }
            set { _ui = value; }
        }

        /// <summary>
        /// Parses an input file (.txt) and generates a list of tiles.
        /// </summary>
        /// <remarks>
        /// Reads the input file, transforms it into a 2-dimensional character
        /// array, and searches this array for puzzle pieces to create a
        /// list of <c>Tile</c> objects. Also creates a <c>Tile</c> object representing
        /// the target solution.
        /// </remarks>
        public static Parser Parser
        {
            get { return _parser; }
            set { _parser = value; }
        }

        /// <summary>
        /// Searches the problem space for solutions.
        /// </summary>
        public static Solver Solver
        {
            get { return _solver; }
            set { _solver = value; }
        }

        public static System.Timers.Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public String Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// When passed an input file, updates necessary properties and prepares application for the solution search process.
        /// </summary>
        /// <param name="filename">The current input file.</param>
        public void Update(string filename)
        {
            Solver.Stop();
            Solver.Reset();
            if (Timer != null)
                Timer.Close();
            Parser.Reset();
            Filename = filename;
            Parser.Update(filename);
            Parser.Parse();
            Solver.UpdateInput(Parser.Target, Parser.Pieces);
            UI.PopulateComponents(Parser.Pieces);
            UI.SetOptionsEnabled();
            UI.UpdateNotificationBox("Input loaded from: " + filename + ". Component tiles are multi-colored and target tile is black.");
        }

        /// <summary>
        /// Prepares application for the solution search process and runs the <c>Solver</c> to begin solution search.
        /// TODO: clean up unnecessary overlap with <c>Update</c>.
        /// </summary>
        public void Run()
        {
            if (!Solver.Running)
            {
                UI.SetNavigation(false);
                Solver.Stop();
                Solver.Reset();
                if (Timer != null)
                    Timer.Close();
                Parser.Reset();
                Parser.Update(Filename);
                Parser.Parse();
                Solver.UpdateInput(Parser.Target, Parser.Pieces);
                Solver.RotationOption = UI.GetRotationOption();
                Solver.ReflectionOption = UI.GetReflectionOption();
                UI.UpdateNotificationBox("Searching for solutions...");
                Solver.Running = true;
                Solver.Run();
                Timer = new System.Timers.Timer(500);
                Timer.Elapsed += OnCheck;
                Timer.AutoReset = true;
                Timer.Enabled = true;
                Timer.Start();
            }
        }

        /// <summary>
        /// Polls the <c>Solver</c> for results every 500ms.
        /// </summary>
        /// <remarks>
        /// If the search is complete, updates the UI and creates a solution file (.txt) if solutions have been found.
        /// </remarks>
        public void GetResults()
        {
            int result = Solver.SolutionState;
            if (result == -1)
            {
                UI.UpdateCurrent(Solver.Current);
                return;
            }
            if (result == 0)
            {
                UI.UpdateNotificationBox("No solution possible.");
                UI.ClearCurrent();
                Timer.Close();
                Solver.Running = false;
            }
            else if (result == 1)
            {
                UI.UpdateNotificationBox("No solutions found.");
                UI.ClearCurrent();
                Timer.Close();
                Solver.Running = false;
            }
            else if (result == 2 && Solver.Complete)
            {
                int num = Solver.Colorcodes.Count;
                if (num == 1)
                    UI.UpdateNotificationBox(num + " solution found.");
                else
                    UI.UpdateNotificationBox(num + " solutions found.");
                UI.PopulateSolutions(Solver.Colorcodes);
                UI.ClearCurrent();
                UI.SetNavigation(true);
                Timer.Close();
                Solver.Running = false;
                string temp = Filename.Remove((Filename.Length) - 4, 4);
                string p = temp + "sol.txt";
                StreamWriter sw = new StreamWriter(p, true);
                string[] all = Compose(Solver.Colorcodes);
                foreach (string str in all)
                    sw.WriteLine(str);
                sw.Close();
            }
            UI.SetOptionsEnabled();
        }

        /// <summary>
        /// Composes a file (.txt) containing all solutions to the current puzzle.
        /// </summary>
        /// <param name="colorcodes">The list of solutions, represented as 2-dimensional integer arrays.</param>
        /// <returns>A string array of all solutions, stacked vertically.</returns>
        public string[] Compose(List<int[,]> colorcodes)
        {
            int count = 0;
            string line = "";
            List<string> alllines = new List<string>();
            List<string> templines = new List<string>();
            foreach (int[,] colorcode in colorcodes)
            {
                for (int n = 0; n < colorcode.GetLength(1); n++)
                {
                    for (int m = 0; m < colorcode.GetLength(0); m++)
                        line += colorcode[m, n];
                    templines.Add(line);
                    line = "";
                }
                foreach (string templine in templines)
                    alllines.Add(templine);
                alllines.Add("");
                templines.Clear();
                count++;
            }
            string[] alllinesarray = alllines.ToArray();
            return alllinesarray;
        }

        /// <summary>
        /// Called by delegate ElapsedEventHandler every 500ms; calls <c>GetResults</c> to check the status of <c>Solver</c>.
        /// </summary>
        /// <param name="source">The <c>Timer</c> object.</param>
        /// <param name="e">No <c>ElapsedEventArgs</c> will be passed.</param>
        private void OnCheck(Object source, System.Timers.ElapsedEventArgs e)
        {
            GetResults();
        }

        /// <summary>
        /// Runs the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UI = new GUI();
            Parser = new Parser();
            Solver = new Solver();
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.Run(UI);
        }
    }
}
