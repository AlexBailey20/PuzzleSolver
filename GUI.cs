using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleSolver
{
    public partial class GUI : Form
    {
        private ApplicationController controller = null;

        public ApplicationController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public GUI()
        {
            InitializeComponent();
        }

        public void PopulateComponents(string[] lines)
        {
            foreach (string line in lines)
                ComponentsBox.AppendText(Environment.NewLine + line);
            Console.Out.WriteLine(lines.Length);
            ComponentsBox.Refresh();
        }

        public void PopulateSolutions()
        {
            StreamReader streamreader = new StreamReader("C:\\Users\\Xeny\\solutions.txt");
            string line;
            while ((line = streamreader.ReadLine()) != null)
                SolutionsBox.AppendText(Environment.NewLine + line);
            SolutionsBox.Refresh();
        }

//        public void PopulateSolutions(List<int[,]> solutions)
//        {
//            
//        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create file browser
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt";             // TODO: are we supposed to read other types?
            openFileDialog.Title = "Select an input text file";

            // show file browser; if user selects .txt file...
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // check that Controller is not null, instantiate if not
                if (Controller == null)
                {
                    Controller = new ApplicationController();
                    Controller.OnEvent += new ApplicationController.EventHandler(OnEvent);
                }
                // update Controller's Filename and Filepath properties
                string filename = openFileDialog.FileName;
                string filepath = Path.GetDirectoryName(filename);
                Controller.Update(filename, filepath);
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Controller.RunSearch();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {

        }

        private void StopButton_Click(object sender, EventArgs e)
        {

        }

        private void OnEvent(object sender, GUIEventArgs e)
        {
            Console.Out.WriteLine(e.message + "\n");
        }
    }
}
