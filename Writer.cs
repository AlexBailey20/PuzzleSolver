using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    public class Writer
    {
        private string filename = null;
        private string filepath = null;
        private List<Tile> pieces;
        private List<int[,]> colorcodes;
        private Tile solution;
        private int smallest;
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

        public Tile Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        public int Smallest
        {
            get { return smallest; }
            set { smallest = value; }
        }

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public Writer()
        {
            Filename = "solutions.txt";
            Filepath = "C:\\Users\\wesbonelli\\Desktop\\PuzzleSolver";
            Code = 0;
            Pieces = new List<Tile>();
            Colorcodes = new List<int[,]>();
        }

        public void Update(string name, string path)
        {
            Filename = name;
            Filepath = path;
        }

        public void WriteFile(string[] lines)
        {
            StreamWriter streamwriter = new StreamWriter(@"C:\\Users\\wesbonelli\\Desktop\\PuzzleSolver\\solutions.txt");
            // iterate through lines
            foreach (string line in lines)
            {
                // write to file
                streamwriter.WriteLine(line);
            }
            streamwriter.Close();
        }

        public void Compose(List<int[,]> colorcodes)
        {
            int count = 0;
            string line = "";
            List<string> alllines = new List<string>();
            List<string> templines = new List<string>();
            // iterate through all solutions
            foreach (int[,] colorcode in colorcodes)
            {
                // iterate through each row
                for (int n = 0; n < colorcode.GetLength(0); n++)
                {
                    // iterate through all characters
                    for (int m = 0; m < colorcode.GetLength(1); m++)
                    {
                        line += colorcode[n, m];
                    }
                    // form subarray for each solution
                    templines.Add(line);
                    line = "";
                }
                // add each subarray to array holding all solutions
                foreach (string templine in templines)
                    alllines.Add(templine);
                // clear templines for next pass
                templines.Clear();
                // advance counter
                count++;
            }
            // change to array and pass to WriteFile
            string[] alllinesarray = alllines.ToArray();
            WriteFile(alllinesarray);
        }
    }
}
