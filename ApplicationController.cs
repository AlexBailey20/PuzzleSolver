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
        private string filename;
        private string filepath;

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

        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public String Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        public void Update(string name, string path)
        {
            Solver.Stop();
            Solver.Reset();
            if (Timer != null)
                Timer.Close();
            Parser.Reset();
            Filename = name;
            Filepath = path;
            Parser.Update(name, path);
            Parser.Parse();
            Solver.UpdateInput(Parser.Target, Parser.Pieces);
            UI.PopulateComponents(Parser.Pieces);
            UI.SetOptionsEnabled();
            UI.UpdateNotificationBox("Input loaded from: " + name + ". Component tiles are multi-colored and target tile is black.");
        }

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
                Parser.Update(filename, filepath);
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
            }
            UI.SetOptionsEnabled();
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
