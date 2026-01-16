using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Tester
    {

        static public bool TestSolvedSudoku(int[,] solvedMat)
        {
            bool[,] rows = new bool[solvedMat.GetLength(0),solvedMat.GetLength(0)];
            bool[,] cols = new bool[solvedMat.GetLength(0), solvedMat.GetLength(0)];
            bool[,] boxes = new bool[solvedMat.GetLength(0), solvedMat.GetLength(0)];
            int boxsize = (int)Math.Sqrt(solvedMat.GetLength(0));
            for (int i = 0;  i < solvedMat.GetLength(0); i++)
            {
                for(int j = 0; j < solvedMat.GetLength(1); j++)
                {
                    if (solvedMat[i, j] == 0)
                        return false;
                    if (rows[i, solvedMat[i, j] - 1] || cols[i, solvedMat[i, j]-1] || boxes[(i / boxsize) * boxsize + j / boxsize, solvedMat[i, j] - 1])
                        return false;
                    rows[i, solvedMat[i, j] - 1] = true;
                    cols[i,solvedMat[i, j]-1] = true;
                    boxes[i / boxsize * boxsize + j / boxsize, solvedMat[i, j] - 1] = true;
                }
            }
            for (int i = 0; i < solvedMat.GetLength(0); i++)
                for (int j = 0; j < solvedMat.GetLength(1); j++)
                    if (!rows[i, solvedMat[i, j] - 1] || !cols[i, solvedMat[i, j] - 1] || !boxes[(i / boxsize) * boxsize + j / boxsize, solvedMat[i, j] - 1])
                        return false;
            return true;
        }
    }
}
