using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class Tester
    {
        public static Random rnd = new Random();
        static public string GenerateSudokuBoard(int[,] mat, int target)
        {
            int r, c;
            for(int i =0;i<target;i++)
            {
                r = rnd.Next(0, mat.GetLength(0));
                c = rnd.Next(0, mat.GetLength(0));
                if (mat[r,c] != 0)
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
        static public bool TestSolvedSudoku(int[,] solvedMat,bool solved = true)
        {
            bool[,] rows = new bool[solvedMat.GetLength(0),solvedMat.GetLength(0)];
            bool[,] cols = new bool[solvedMat.GetLength(0), solvedMat.GetLength(0)];
            bool[,] boxes = new bool[solvedMat.GetLength(0), solvedMat.GetLength(0)];
            int boxsize = (int)Math.Sqrt(solvedMat.GetLength(0));
            for (int i = 0;  i < solvedMat.GetLength(0); i++)
            {
                for(int j = 0; j < solvedMat.GetLength(1); j++)
                {
                    if (solved && solvedMat[i, j] == 0)
                        return false;
                    if(solvedMat[i, j] != 0)
                    {
                        if (rows[i, solvedMat[i, j] - 1] || cols[i, solvedMat[i, j]-1] || boxes[(i / boxsize) * boxsize + j / boxsize, solvedMat[i, j] - 1])
                            return false;
                        rows[i, solvedMat[i, j] - 1] = true;
                        cols[i,solvedMat[i, j]-1] = true;
                        boxes[i / boxsize * boxsize + j / boxsize, solvedMat[i, j] - 1] = true;
                    }
                        
                }
            }
            for (int i = 0; i < solvedMat.GetLength(0); i++)
                for (int j = 0; j < solvedMat.GetLength(1); j++)
                    if (solvedMat[i,j] != 0)
                        if (!rows[i, solvedMat[i, j] - 1] || !cols[i, solvedMat[i, j] - 1] || !boxes[(i / boxsize) * boxsize + j / boxsize, solvedMat[i, j] - 1])
                            return false;
            return true;
        }
    }
}
