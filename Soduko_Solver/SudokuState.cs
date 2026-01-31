using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class SudokuState
    {
        int[,] mat;
        int len;
        SudokuStack s = new SudokuStack();
        List<(int, int)> empties = new List<(int, int)>();
        int[] rowsBitMask;
        int[] colsBitMask;
        int[] boxesBitMask;
        int boxSize;
        int minCluesDensity;
        int[,] weight;
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
        public SudokuState(int[,] mat) {
            this.mat = mat;
            len = mat.GetLength(0);
            rowsBitMask = new int[mat.GetLength(0)];
            colsBitMask = new int[mat.GetLength(0)];
            boxesBitMask = new int[mat.GetLength(0)];
            boxSize = (int)Math.Sqrt(mat.GetLength(0));
            minCluesDensity = (int)(0.94 * mat.GetLength(0) * mat.GetLength(0));

            weight = new int[mat.GetLength(0), mat.GetLength(0)]; // weight[index,value]
        }
        public SudokuState(SudokuState state)
        {
            this.mat = (int[,])state.Mat.Clone();
            this.len = state.Len;
            this.rowsBitMask = (int[])state.RowsBitMask.Clone();
            this.colsBitMask = (int[])state.ColsBitMask.Clone();
            this.boxesBitMask = (int[])state.BoxesBitMask.Clone();
            this.empties = new List<(int, int)>(state.Empties);
            this.weight = (int[,])state.Weight.Clone();
            this.boxSize = state.BoxSize;
            this.minCluesDensity = state.MinClueDensity;
        }
    }
}
