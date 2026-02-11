using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class Board_Formatter
    {
        //Function to format a Sudoku board string into a 2-dimensional array
        static public int[,] Format(string baseMat)
        {
            int n = baseMat.Length;
            if(n == 0)
                throw new Empty_Board_Exception();
            if (n != 81) //For release: limited board size to 9 X 9
                throw new Invalid_Length_Exception();
            n = (int)Math.Sqrt(n);
            int[,] mat = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    mat[i, j] = baseMat[i * n + j] - '0';
            return mat;
        }

        //Function to print a Sudoku board
        static public void Print_Mat(int[,] mat)
        {
            int boxSize = (int)Math.Sqrt(mat.GetLength(0));
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (i % boxSize == 0)
                {
                    for (int k = 0; k < (boxSize + 1) + mat.GetLength(0) * 3; k++)
                        Console.Write("-");
                    Console.WriteLine();
                }
                for (int j = 0; j < mat.GetLength(0); j++)
                {
                    if (j % boxSize == 0)
                        Console.Write("|");
                    Console.Write(mat[i, j]);
                    if (mat[i, j] > 9)
                        Console.Write(" ");
                    else
                        Console.Write("  ");

                }
                Console.WriteLine("|");
            }
            for (int k = 0; k < (boxSize + 1) + mat.GetLength(0) * 3; k++)
                Console.Write("-");
            Console.WriteLine();
        }
    }
}
