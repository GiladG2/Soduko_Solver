using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class SudokuState
    {
        private int[,] mat; // Saves the int[,] representation of the Sudoku board
        private int len; // Saves the length of the Board (1/4/9/16/25)
        private SudokuStack s = new SudokuStack(); // Saves the operation stack of the board
        private List<(int, int)> empties = new List<(int, int)>(); //Saves all the empty cells (row,column) in the board
        private int[] rowsBitMask; // Saves all the bitmasks that represent each row's domain
        private int[] colsBitMask; // Saves all the bitmasks numbers that represent each column's domain
        private int[] boxesBitMask; // Saves all the bitmasks numbers that represent each box's domain
        private int boxSize; // Saves the board's box size
        private int minCluesDensity; // Saves the minimum filled cells density required for skipping simple backtracking
        private int[,] weight; // Saves the weight of each cell ( weight[row,column] )
        
        //Properties to get the fields
        
        public SudokuStack Stack { get => s;set => s = value; }
        public int[,] Mat { get => mat;set=> mat = value; }
        public int Len { get => len;set => len = value; }
        public int[] RowsBitMask { get => rowsBitMask; set => rowsBitMask = value; }
        public int[] ColsBitMask { get => colsBitMask; set => colsBitMask = value; }
        public int[] BoxesBitMask { get=> boxesBitMask; set => boxesBitMask = value; }
        public int BoxSize { get => boxSize; set => boxSize = value; }
        public int MinClueDensity { get => minCluesDensity;set=> minCluesDensity = value; }
        public List<(int,int)> Empties { get => empties; set => empties = value; }
        public int[,] Weight { get => weight; set => weight = value; }
        //Constructor
        public SudokuState(int[,] mat) {
            this.mat = mat;
            len = mat.GetLength(0);
            rowsBitMask = new int[mat.GetLength(0)];
            colsBitMask = new int[mat.GetLength(0)];
            boxesBitMask = new int[mat.GetLength(0)];
            boxSize = (int)Math.Sqrt(mat.GetLength(0));
            minCluesDensity = (int)(0.94 * mat.GetLength(0) * mat.GetLength(0));

            weight = new int[mat.GetLength(0), mat.GetLength(0)];
        }
    }
}
