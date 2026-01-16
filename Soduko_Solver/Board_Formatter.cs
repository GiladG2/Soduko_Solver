using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Board_Formatter
    {
        static public int[,] Format(string baseMat)
        {
            int n = baseMat.Length;
            if(Math.Sqrt(n) % 1 !=0 || Math.Sqrt(Math.Sqrt(n)) % 1 != 0)
                throw new Invalid_Length_Exception();
            n = (int)Math.Sqrt(n);
            int[,] mat = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    mat[i, j] = baseMat[i * n + j] - '0';
            return mat;
        }
        static public void Print_Mat(int[,] mat)
        {
            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                    Console.Write(mat[i, j] + " ");
                Console.WriteLine();
            }
        }
    }
}
