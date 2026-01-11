using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Sudoku_Solver
    {
        //func that recieves a soduko board and returns true if it is solvable and false is not
        static public bool Solve_Sudoku(int[,] mat)
        {


            return BackTrack(mat, 0, 0, mat.GetLength(0));
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
            for(int sr = boxRow;sr<boxRow+boxSize;sr++)
                for(int sc = boxCol; sc < boxCol+boxSize; sc++)
                    if (mat[sr, sc] == num)
                        return false;
            return true;
        }
         private static bool BackTrack(int[,] mat,int row,int col, int len)
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
                return BackTrack(mat, row, col + 1, len);
            for(int k =1; k<=len;k++)
            {
                if(Is_Safe(mat,row,col,k))
                {
                    mat[row, col] = k;
                    if(BackTrack(mat,row,col+1,len))
                        return true;
                    mat[row, col] = 0;
                }
            }
            return false;
        }
    }
}
