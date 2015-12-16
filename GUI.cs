using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PuzzleSolver
{
    public partial class GUI : Form
    {
        private ApplicationController controller = null;
        private FlowLayoutPanel layoutpanel;
        private FlowLayoutPanel reflrotacontrolpanel;
        private FlowLayoutPanel controlpanel;
        private FlowLayoutPanel displaypanel;
        private FlowLayoutPanel componentsdisplaypanel;
        private FlowLayoutPanel solutionsdisplaypanel;

        public FlowLayoutPanel Layoutpanel
        {
            get { return layoutpanel; }
            set { layoutpanel = value; }
        }

        public FlowLayoutPanel Reflrotacontrolpanel
        {
            get { return reflrotacontrolpanel; }
            set { reflrotacontrolpanel = value; }
        }

        public FlowLayoutPanel Controlpanel
        {
            get { return controlpanel; }
            set { controlpanel = value; }
        }

        public FlowLayoutPanel Displaypanel
        {
            get { return displaypanel; }
            set { displaypanel = value; }
        }

        public FlowLayoutPanel Componentsdisplaypanel
        {
            get { return componentsdisplaypanel; }
            set { componentsdisplaypanel = value; }
        }

        public FlowLayoutPanel Solutionsdisplaypanel
        {
            get { return solutionsdisplaypanel; }
            set { solutionsdisplaypanel = value; }
        }

        public ApplicationController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public GUI()
        {
            InitializeComponent();
            this.Width = (Screen.FromControl(this).Bounds.Width - 50);
            this.Height = (Screen.FromControl(this).Bounds.Height - 100);
            this.StartPosition = FormStartPosition.CenterScreen;
            Layoutpanel = new FlowLayoutPanel();                // everything
            Reflrotacontrolpanel = new FlowLayoutPanel();       // reflection/rotation options
            Controlpanel = new FlowLayoutPanel();               // solve button and reflection/rotation options
            Displaypanel = new FlowLayoutPanel();               // all displays
            Componentsdisplaypanel = new FlowLayoutPanel();     // top row of displays
            Solutionsdisplaypanel = new FlowLayoutPanel();     // bottom row of displays
            Arrange();
        }

        public void Arrange()
        {
            Layoutpanel.FlowDirection = FlowDirection.TopDown;
            Layoutpanel.Width = this.Width;
            Layoutpanel.Height = this.Height;

            Reflrotacontrolpanel.FlowDirection = FlowDirection.TopDown;
            Reflrotacontrolpanel.Controls.Add(ReflectionCheck);
            Reflrotacontrolpanel.Controls.Add(RotationCheck);
            ReflectionCheck.Anchor = AnchorStyles.Left;
            RotationCheck.Anchor = AnchorStyles.Left;

            Controlpanel.FlowDirection = FlowDirection.LeftToRight;
            Controlpanel.Anchor = AnchorStyles.Left;
            Controlpanel.Width = Layoutpanel.Width;
            Controlpanel.Height = (SolveButton.Height + 5);
            Controlpanel.Controls.Add(SolveButton);
            Controlpanel.Controls.Add(Reflrotacontrolpanel);

            NotificationBox.Anchor = AnchorStyles.Left;
            NotificationBox.Width = Layoutpanel.Width;
            NotificationBox.Height = 20;
            NotificationBox.ReadOnly = true;

            Displaypanel.FlowDirection = FlowDirection.TopDown;
            Displaypanel.Anchor = AnchorStyles.Left;

            Displaypanel.Width = (Layoutpanel.Width);
            Displaypanel.Height = (Layoutpanel.Height - Controlpanel.Height - NotificationBox.Height - 40);

            Componentsdisplaypanel.FlowDirection = FlowDirection.LeftToRight;
            Componentsdisplaypanel.Anchor = AnchorStyles.Left;

            Componentsdisplaypanel.Width = (Displaypanel.Width);
            Componentsdisplaypanel.Height = (Displaypanel.Height / 2) - 10;

            Solutionsdisplaypanel.FlowDirection = FlowDirection.LeftToRight;
            Solutionsdisplaypanel.Anchor = AnchorStyles.Left;

            Solutionsdisplaypanel.Width = (Displaypanel.Width);
            Solutionsdisplaypanel.Height = (Displaypanel.Height / 2) - 10;

            ComponentsList.Width = (Componentsdisplaypanel.Width / 2) - 5;
            ComponentsList.Height = (Componentsdisplaypanel.Height) - 5;
            Target.Width = (Componentsdisplaypanel.Width / 2) - 5;
            Target.Height = (Componentsdisplaypanel.Height) - 5;
            SolutionsList.Width = (Solutionsdisplaypanel.Width / 2) - 5;
            SolutionsList.Height = (Solutionsdisplaypanel.Height) - 5;
            Current.Width = (Solutionsdisplaypanel.Width / 2) - 5;
            Current.Height = (Solutionsdisplaypanel.Height) - 5;

            Componentsdisplaypanel.Controls.Add(Target);
            Componentsdisplaypanel.Controls.Add(ComponentsList);

            Solutionsdisplaypanel.Controls.Add(Current);
            Solutionsdisplaypanel.Controls.Add(SolutionsList);

            Displaypanel.Controls.Add(Componentsdisplaypanel);
            Displaypanel.Controls.Add(Solutionsdisplaypanel);

            Layoutpanel.Controls.Add(MenuStrip);
            Layoutpanel.Controls.Add(Controlpanel);
            Layoutpanel.Controls.Add(NotificationBox);
            Layoutpanel.Controls.Add(Displaypanel);

            this.Controls.Add(Layoutpanel);
        }

        public bool GetRotationOption()
        {
            RotationCheck.Enabled = false;
            return RotationCheck.Checked;
        }

        public bool GetReflectionOption()
        {
            ReflectionCheck.Enabled = false;
            return ReflectionCheck.Checked;
        }

        public void setOptionsEnabled()
        {
            RotationCheck.Enabled = true;
            ReflectionCheck.Enabled = true;
        }

        public void UpdateNotificationBox(string message)
        {
            NotificationBox.Clear();
            NotificationBox.AppendText(message);
        }

        public void PopulateComponents(List<Tile> components)
        {
            if (ComponentsList.Columns.Count == 0)
            {
                ComponentsList.View = View.Details;
                ComponentsList.HeaderStyle = ColumnHeaderStyle.None;
                Target.View = View.Details;
                Target.HeaderStyle = ColumnHeaderStyle.None;
                foreach (Tile component in components)
                {
                    int rows = component.cSize;
                    int cols = component.rSize;
                    if (component == components.Last())
                    {
                        while (Target.Columns.Count < cols)
                            Target.Columns.Add("");
                        string[] rowchars = new string[cols];
                        string[] empty = { };
                        for (int j = 0; j < rows; j++)
                        {
                            var row = new ListViewItem();
                            for (int k = 0; k < cols; k++)
                            {
                                rowchars[k] = component.Dimensions[j, k].ToString();
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
                            Target.Items.Add(row);
                        }
                    }
                    else
                    {
                        while (ComponentsList.Columns.Count < cols)
                            ComponentsList.Columns.Add("");
                        string[] rowchars = new string[cols];
                        string[] empty = { };
                        for (int j = 0; j < rows; j++)
                        {
                            var row = new ListViewItem();
                            for (int k = 0; k < cols; k++)
                            {
                                rowchars[k] = component.Dimensions[j, k].ToString();
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
                }
                ComponentsList.Refresh();
                Target.Refresh();
            }
        }

        public void PopulateSolutions(List<int[,]> solutions)
        {
            SolutionsList.View = View.Details;
            SolutionsList.HeaderStyle = ColumnHeaderStyle.None;
            //SolutionsList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            List<ListViewItem> allrows = new List<ListViewItem>();
            foreach(int[,] solution in solutions)
            {
                int rows = solution.GetLength(1);
                int cols = solution.GetLength(0);
                while (SolutionsList.Columns.Count < cols)
                    SolutionsList.Columns.Add("");
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
                    allrows.Add(row);
                }
                allrows.Add(new ListViewItem());
            }
            foreach (ListViewItem row in allrows)
            {
                if (!SolutionsList.Items.Contains(row))
                    SolutionsList.Items.Add(row);
            }
            SolutionsList.Refresh();
        }

        public void UpdateCurrent(int[,] current)
        {
            Current.Clear();
            Current.View = View.Details;
            Current.HeaderStyle = ColumnHeaderStyle.None;
            List<ListViewItem> allrows = new List<ListViewItem>();
            int rows = current.GetLength(1);
            int cols = current.GetLength(0);
            while (Current.Columns.Count < cols)
                Current.Columns.Add("");
            string[] rowchars = new string[cols];
            string[] empty = { };
            for (int j = 0; j < rows; ++j)
            {
                var row = new ListViewItem();
                for (int k = 0; k < cols; k++)
                {
                    rowchars[k] = current[k, j].ToString();
                }
                for (int r = 0; r < cols; r++)
                {
                    row.SubItems.Add("");
                    if (rowchars[r] != " ")
                    {
                        SetSubItemColor(null, current, row, r, j);
                        row.UseItemStyleForSubItems = false;
                    }
                    else
                    {
                        row.SubItems[r].BackColor = Color.White;
                        row.UseItemStyleForSubItems = false;
                    }

                }
                allrows.Add(row);
            }
            foreach (ListViewItem row in allrows)
                Current.Items.Add(row);
            Current.Refresh();
        }

        public void ClearCurrent()
        {
            Current.Clear();
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
                ComponentsList.Clear();
                Target.Clear();
                SolutionsList.Clear();
                Current.Clear();
                Controller.Update(filename, filepath);
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            SolutionsList.Clear();
            Controller.Run();
        }

        private void OnEvent(object sender, GUIEventArgs e)
        {
            //Console.Out.WriteLine(e.message + "\n");
        }
    }
}
