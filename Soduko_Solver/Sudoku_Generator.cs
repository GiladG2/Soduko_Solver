using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Sudoku_Generator
    {
        // Randomly remove target amount of cells in a solved mat.
        static public string GenerateSudokuBoard(int[,] mat, int target)
        {
            Random rnd = new Random();
            int r, c;
            for (int i = 0; i < target; i++)
            {
                r = rnd.Next(0, mat.GetLength(0));
                c = rnd.Next(0, mat.GetLength(0));
                if (mat[r, c] != 0)
                    mat[r, c] = 0;
                else
                    target++;
            }
            Console.WriteLine("Generated board");
            Board_Formatter.Print_Mat(mat);
            string matString = "";
            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(0); j++)
                    matString += (char)('0' + mat[i, j]);
            return matString;
        }
    }
}
