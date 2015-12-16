using System;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PuzzleSolver
{

    public class ApplicationController
    {
        private static GUI ui = null;
        private static Parser parser = null;
        private static Solver solver = null;
        private static System.Timers.Timer timer = null;
        private string filename;

        //Windows Form containing our user interface code
        public static GUI UI
        {
            get { return ui; }
            set { ui = value; }
        }

        //Parses input files
        public static Parser Parser
        {
            get { return parser; }
            set { parser = value; }
        }

        //Solution search mechanism/algorithms
        public static Solver Solver
        {
            get { return solver; }
            set { solver = value; }
        }

        //Timer to keep checking for solutions from Solver
        public static System.Timers.Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        //input puzzle filename
        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        //Updates all necessary propertis and prepares for solving when passed an input file
        public void Update(string name)
        {
            Solver.Stop();
            Solver.Reset();
            if (Timer != null)
                Timer.Close();
            Parser.Reset();
            Filename = name;
            Parser.Update(name);
            Parser.Parse();
            Solver.UpdateInput(Parser.Target, Parser.Pieces);
            UI.PopulateComponents(Parser.Pieces);
            UI.SetOptionsEnabled();
            UI.UpdateNotificationBox("Input loaded from: " + name + ". Component tiles are multi-colored and target tile is black.");
        }

        //conducts the search process
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
                Parser.Update(filename);
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

        //pulls results from Solver based on SolutionState, called every 500ms or so while search is running
        //reports findings, takes necessary closing actions once search is complete
        //writes solution file
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

        public string[] Compose(List<int[,]> colorcodes)
        {
            int count = 0;
            string line = "";
            List<string> alllines = new List<string>();
            List<string> templines = new List<string>();
            // iterate through all solutions
            foreach (int[,] colorcode in colorcodes)
            {
                // iterate through each row
                for (int n = 0; n < colorcode.GetLength(1); n++)
                {
                    // iterate through all characters
                    for (int m = 0; m < colorcode.GetLength(0); m++)
                    {
                        line += colorcode[m, n];
                    }
                    // form subarray for each solution
                    templines.Add(line);
                    line = "";
                }
                // add each subarray to array holding all solutions
                foreach (string templine in templines)
                    alllines.Add(templine);
                alllines.Add("");
                // clear templines for next pass
                templines.Clear();
                // advance counter
                count++;
            }
            // change to array and pass to WriteFile
            string[] alllinesarray = alllines.ToArray();
            return alllinesarray;
        }

        //called eveery 500ms to check for solutions
        private void OnCheck(Object source, System.Timers.ElapsedEventArgs e)
        {
            GetResults();
        }

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
