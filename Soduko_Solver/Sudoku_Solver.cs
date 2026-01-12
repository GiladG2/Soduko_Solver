using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Sudoku_Solver
    {
        //func that recieves a soduko board and returns true if it is solvable and false is not
        static public bool Solve_Sudoku(int[,] mat)
        {
            int[] rowsBitMask = new int[mat.GetLength(0)];
            int[] colsBitMask = new int[mat.GetLength(0)];
            int[] boxesBitMask = new int[mat.GetLength(0)];
            int boxSize = (int)Math.Sqrt(mat.GetLength(0));
            for(int rows = 0;rows<mat.GetLength(0);rows++)
                for(int cols = 0;cols<mat.GetLength(0);cols++)
                    if(mat[rows,cols] != 0)
                    {
                        rowsBitMask[rows] |= 1<< (mat[rows,cols]);
                        colsBitMask[cols] |= 1<< (mat[rows,cols]);
                        boxesBitMask[(rows / boxSize) * boxSize + (cols / boxSize)] |= 1<< (mat[rows,cols]);
                    }
            return BackTrack(mat, 0, 0, mat.GetLength(0),boxesBitMask,rowsBitMask,colsBitMask);
        }
        
        private static bool Is_Safe_Bitmask(int rows,int cols,int[] rowsBitMask, int[] colBitMask, int[] boxesBitMask,int num)
        {
            int boxSize = (int)Math.Sqrt(boxesBitMask.Length);
            int bitmask = 0;
            bitmask |= 1 << (num);
            if ((rowsBitMask[rows] & bitmask) != 0 || (colBitMask[cols] & bitmask) != 0 ||(
                boxesBitMask[(rows / boxSize) * boxSize + (cols / boxSize)] & bitmask) != 0)
                return false;
            return true;

        }
         private static bool BackTrack(int[,] mat,int row,int col, int len, int[] boxesBitMask, int[] rowsBitMask, int[] colsBitMask)
        {
            //base case: reached end of board (filled every cell)
            if (row == len - 1 && col == len)
                return true;
            if(col == len)
            {
                col = 0;
                row++;
            }
            if (mat[row, col] != 0)
                return BackTrack(mat, row, col + 1, len,boxesBitMask,rowsBitMask,colsBitMask);
            int boxSize = (int)Math.Sqrt(len);
            for(int k =1; k<=len;k++)
            {
                if(Is_Safe_Bitmask(row,col,rowsBitMask,colsBitMask,boxesBitMask,k))
                {
                    mat[row, col] = k;
                    AddSeenDigits(k, row, col, boxSize, rowsBitMask, colsBitMask, boxesBitMask);
                    if (BackTrack(mat,row,col+1,len, boxesBitMask, rowsBitMask, colsBitMask))
                        return true;
                    mat[row, col] = 0;
                    RemoveSeenDigit(k, row, col, boxSize, rowsBitMask, colsBitMask, boxesBitMask);
                }
            }
            return false;
        }
        static void AddSeenDigits(int k, int row, int col, int boxSize, int[] rowsBitMask, int[] colsBitMask, int[] boxesBitMask)
        {
            rowsBitMask[row] |= 1 << (k);
            colsBitMask[col] |= 1 << (k);
            boxesBitMask[(row / boxSize) * boxSize + (col / boxSize)] |= 1 << (k);
        }
        static void RemoveSeenDigit(int k,int row,int col,int boxSize, int[] rowsBitMask, int[] colsBitMask, int[] boxesBitMask)
        {
            rowsBitMask[row] &= ~(1 << (k));
            colsBitMask[col] &= ~(1 << (k));
            boxesBitMask[(row / boxSize) * boxSize + (col / boxSize)] &= ~(1 << (k));
        }
        private static bool Is_Safe(int[,] mat, int row, int col, int num)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
                if (mat[i, col] == num)
                    return false;
            for (int j = 0; j < mat.GetLength(1); j++)
                if (mat[row, j] == num)
                    return false;
            int boxSize = (int)Math.Sqrt(mat.GetLength(0));
            int boxRow = (row / boxSize) * boxSize;
            int boxCol = (col / boxSize) * boxSize;
            for (int sr = boxRow; sr < boxRow + boxSize; sr++)
                for (int sc = boxCol; sc < boxCol + boxSize; sc++)
                    if (mat[sr, sc] == num)
                        return false;
            return true;
        }
    }
}
