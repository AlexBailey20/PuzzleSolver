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
        private static Writer writer = null;
        private static Solver solver = null;

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

        public void Update(string name, string path)
        {
            Parser.Update(name, path);
            Parser.Parse();
            Solver.Update(Parser.Target, Parser.Pieces);
            string[] lines = Parser.ReadFile();             // will deprecate once displays are updated
            UI.PopulateComponents(lines);                   // "
            Log("File at " + path + " parsed.");
        }

        public void RunSearch()
        {
            Log("Searching for solutions...");
            int result = Solver.Solve();
            List<int[,]> solutions = new List<int[,]>();
            if (result == 0)
            {
                Console.WriteLine("No target possible.");
            }
            else if (result == 1)
            {
                Console.WriteLine("No target found.");
            }
            else if (result == 2)
            {
                Console.WriteLine("Target(s) found.");
                solutions = Solver.Colorcodes;
                Console.WriteLine(Solver.Colorcodes.Count);
                Writer.Compose(solutions);          // will deprecate when displays updated
                UI.PopulateSolutions();             // "
            }
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
            Application.Run(UI);
        }
    }
}