using System;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
        private static Writer writer = null;
        private static Solver solver = null;
        private static System.Timers.Timer timer = null;
        private static Thread solvethread;

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

        public static Writer Writer
        {
            get { return writer; }
            set { writer = value; }
        }

        public static Solver Solver
        {
            get { return solver; }
            set { solver = value; }
        }

        public static System.Timers.Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public void Update(string name, string path)
        {
            Parser.Update(name, path);
            Parser.Parse();
            UI.PopulateComponents(Parser.Pieces);
            //Log("File at " + path + " parsed.");
        }

        public void Run()
        {
            //Log("Searching for solutions...");
            if (Solver.Clean)
                Solver.UpdateInput(Parser.Target, Parser.Pieces);
            Solver.Run();
            Timer = new System.Timers.Timer(2000);
            Timer.Elapsed += OnCheck;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public void Pause()
        {
            Solver.Pause();
        }

        public void Stop()
        {
            Solver.Stop();
            Solver.Reset();
            Solver.UpdateInput(Parser.Target, Parser.Pieces);
        }

        public void GetResults()
        {
            // replace with method that gets the one currently being checked
            int result = Solver.SolutionState;
            if (result == -1)
                return;
            Timer.Enabled = false;
            if (result == 0)
            {
                //Console.WriteLine("No target possible.");
            }
            else if (result == 1)
            {
                //Console.WriteLine("No target found.");
            }
            else if (result == 2)
            {
                //Console.WriteLine("Target(s) found.");
                UI.PopulateSolutions(Solver.Colorcodes);
            }
        }

        private void OnCheck(Object source, System.Timers.ElapsedEventArgs e)
        {
            GetResults();
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
            Writer = new Writer();
            Solver = new Solver();
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.Run(UI);
        }
    }
}
