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

        public void PopulateComponents(List<Tile> components)
        {
            if (ComponentsList.Columns.Count == 0)
            {
                ComponentsList.View = View.Details;
                ComponentsList.GridLines = true;
                ComponentsList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                foreach (Tile component in components)
                {
                    int rows = component.cSize;
                    int cols = component.rSize;
                    while (ComponentsList.Columns.Count < cols)
                        ComponentsList.Columns.Add("");
                    //    for (int i = 0; i < ComponentsList.Columns.Count; i++)
                    //        ComponentsList.Columns[i].TextAlign = HorizontalAlignment.Center;
                    string[] rowchars = new string[cols];
                    string[] empty = { };
                    for (int j = 0; j < rows; j++)
                    {
                        var row = new ListViewItem();
                        for (int k = 0; k < cols; k++)
                        {
                            rowchars[k] = component.Dimensions[j, k].ToString();
                            //Console.Out.WriteLine(rowchars[k]);
                        }
                        for (int r = 0; r < cols; r++)
                        {
                            row.SubItems.Add("");
                            if (rowchars[r] != " ")
                            {
                                SetSubItemColor(component, null, row, j, r);
                                row.UseItemStyleForSubItems = false;
                            }
                            else
                            {
                                row.SubItems[r].BackColor = Color.White;
                                row.UseItemStyleForSubItems = false;
                            }

                        }
                        ComponentsList.Items.Add(row);
                    }
                    ComponentsList.Items.Add(new ListViewItem(empty));
                }
                ComponentsList.Refresh();
            }
            else
            {

            }
        }

        public void PopulateSolutions(List<int[,]> solutions)
        {
            SolutionsList.View = View.Details;
            SolutionsList.GridLines = true;
            SolutionsList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            foreach(int[,] solution in solutions)
            {
                int rows = solution.GetLength(1);
                int cols = solution.GetLength(0);
                while (SolutionsList.Columns.Count < cols)
                    SolutionsList.Columns.Add("");
           //     for (int i = 0; i < ComponentsList.Columns.Count; i++)
           //         ComponentsList.Columns[i].TextAlign = HorizontalAlignment.Center;
                string[] rowchars = new string[cols];
                string[] empty = { };
                for (int j = 0; j < rows; ++j)
                {
                    var row = new ListViewItem();
                    for (int k = 0; k < cols; k++)
                    {
                        rowchars[k] = solution[k, j].ToString();
                    }
                    for (int r = 0; r < cols; r++)
                    {
                        row.SubItems.Add("");
                        if (rowchars[r] != " ")
                        {
                            SetSubItemColor(null, solution, row, r, j);
                            row.UseItemStyleForSubItems = false;
                        }
                        else
                        {
                            row.SubItems[r].BackColor = Color.White;
                            row.UseItemStyleForSubItems = false;
                        }

                    }
                    SolutionsList.Items.Add(row);
                }
                SolutionsList.Items.Add(new ListViewItem(empty));
            }
            SolutionsList.Refresh();
        }

        public void SetSubItemColor(Tile component, int[,] solution, ListViewItem row, int r, int j)
        {
            if (solution == null)
            {
                if (component.Target)
                    row.SubItems[j].BackColor = Color.Black;
                else
                {
                    if (component.Colorcode == 0)
                        row.SubItems[j].BackColor = Color.Red;
                    if (component.Colorcode == 1)
                        row.SubItems[j].BackColor = Color.Blue;
                    if (component.Colorcode == 2)
                        row.SubItems[j].BackColor = Color.Green;
                    if (component.Colorcode == 3)
                        row.SubItems[j].BackColor = Color.Yellow;
                    if (component.Colorcode == 4)
                        row.SubItems[j].BackColor = Color.Purple;
                    if (component.Colorcode == 5)
                        row.SubItems[j].BackColor = Color.Brown;
                    if (component.Colorcode == 6)
                        row.SubItems[j].BackColor = Color.Orange;
                    if (component.Colorcode == 7)
                        row.SubItems[j].BackColor = Color.Lime;
                    if (component.Colorcode == 8)
                        row.SubItems[j].BackColor = Color.Pink;
                    if (component.Colorcode == 9)
                        row.SubItems[j].BackColor = Color.Violet;
                    if (component.Colorcode == 10)
                        row.SubItems[j].BackColor = Color.Aqua;
                    if (component.Colorcode == 11)
                        row.SubItems[j].BackColor = Color.Teal;
                    if (component.Colorcode == 12)
                        row.SubItems[j].BackColor = Color.Maroon;
                    if (component.Colorcode == 13)
                        row.SubItems[j].BackColor = Color.Magenta;
                    if (component.Colorcode == 14)
                        row.SubItems[j].BackColor = Color.YellowGreen;
                    if (component.Colorcode == 15)
                        row.SubItems[j].BackColor = Color.PowderBlue;
                    if (component.Colorcode == 16)
                        row.SubItems[j].BackColor = Color.MidnightBlue;
                    if (component.Colorcode == 17)
                        row.SubItems[j].BackColor = Color.Plum;
                    if (component.Colorcode == 18)
                        row.SubItems[j].BackColor = Color.PeachPuff;
                    if (component.Colorcode == 19)
                        row.SubItems[j].BackColor = Color.PaleVioletRed;
                    if (component.Colorcode == 20)
                        row.SubItems[j].BackColor = Color.DimGray;
                }
            }
            else if (component == null)
            {
                if (solution[r, j] == 0)
                    row.SubItems[r].BackColor = Color.Red;
                if (solution[r, j] == 1)
                    row.SubItems[r].BackColor = Color.Blue;
                if (solution[r, j] == 2)
                    row.SubItems[r].BackColor = Color.Green;
                if (solution[r, j] == 3)
                    row.SubItems[r].BackColor = Color.Yellow;
                if (solution[r, j] == 4)
                    row.SubItems[r].BackColor = Color.Purple;
                if (solution[r, j] == 5)
                    row.SubItems[r].BackColor = Color.Brown;
                if (solution[r, j] == 6)
                    row.SubItems[r].BackColor = Color.Orange;
                if (solution[r, j] == 7)
                    row.SubItems[r].BackColor = Color.Lime;
                if (solution[r, j] == 8)
                    row.SubItems[r].BackColor = Color.Pink;
                if (solution[r, j] == 9)
                    row.SubItems[r].BackColor = Color.Violet;
                if (solution[r, j] == 10)
                    row.SubItems[r].BackColor = Color.Aqua;
                if (solution[r, j] == 11)
                    row.SubItems[r].BackColor = Color.Teal;
                if (solution[r, j] == 12)
                    row.SubItems[r].BackColor = Color.Maroon;
                if (solution[r, j] == 13)
                    row.SubItems[r].BackColor = Color.Magenta;
                if (solution[r, j] == 14)
                    row.SubItems[r].BackColor = Color.YellowGreen;
                if (solution[r, j] == 15)
                    row.SubItems[r].BackColor = Color.PowderBlue;
                if (solution[r, j] == 16)
                    row.SubItems[r].BackColor = Color.MidnightBlue;
                if (solution[r, j] == 17)
                    row.SubItems[r].BackColor = Color.Plum;
                if (solution[r, j] == 18)
                    row.SubItems[r].BackColor = Color.PeachPuff;
                if (solution[r, j] == 19)
                    row.SubItems[r].BackColor = Color.PaleVioletRed;
                if (solution[r, j] == 20)
                    row.SubItems[r].BackColor = Color.DimGray;
            }
        }
        
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
            Controller.Run();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Controller.Pause();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Controller.Stop();
        }

        private void OnEvent(object sender, GUIEventArgs e)
        {
            //Console.Out.WriteLine(e.message + "\n");
        }
    }
}
