using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Sudoku_Solver
    {
        static List<(int, int)> empties = new List<(int, int)>();
        static int[] rowsBitMask;
        static int[] colsBitMask;
        static int[] boxesBitMask;
        static int boxSize;
        static int minCluesDensity;
        static Random rnd = new Random();
        static int[,] weight;
        static int[] boxWeight;
        static int[] rowWeight;
        static int[] colWeight;
        public Sudoku_Solver(int[,] mat)
        {
            rowsBitMask = new int[mat.GetLength(0)];
            colsBitMask = new int[mat.GetLength(0)];
            boxesBitMask = new int[mat.GetLength(0)];
            boxSize = (int)Math.Sqrt(mat.GetLength(0));
            minCluesDensity = (int)(0.94 * mat.GetLength(0) * mat.GetLength(0));
            rowWeight = new int[mat.GetLength(0)];
            colWeight = new int[mat.GetLength(0)];
            boxWeight = new int[mat.GetLength(0)];
            weight = new int[mat.GetLength(0),mat.GetLength(0)];
        }
        
        //func that recieves a soduko board and returns true if it is solvable and false if not
        static public string Solve_Sudoku(int[,] mat)
        {
            empties.Clear();
            int firstEmptyRow = -1;
            int firstEmptyCol = -1;
            for (int rows = 0; rows < mat.GetLength(0); rows++)
                for (int cols = 0; cols < mat.GetLength(0); cols++)
                {
                    if (mat[rows, cols] + '0' < '0' || mat[rows, cols] > mat.GetLength(0))
                        throw new Invalid_Character_Exception(mat[rows, cols], rows + 1, cols + 1);
                    if (mat[rows, cols] != 0)
                        AddSeenDigits(mat[rows, cols], rows, cols);
                    else
                    {
                        empties.Add((rows, cols));
                        if (firstEmptyRow == -1)
                        {
                            firstEmptyRow = rows;
                            firstEmptyCol = cols;
                        }
                    }
                }
            if (empties.Count > minCluesDensity)
            {
                //int[] options = GetMaxOptions(mat.GetLength(0));
                Console.WriteLine($"{minCluesDensity}, {empties.Count} => target = {empties.Count - minCluesDensity}");
                SimpleBacktracking(mat, empties.Count - minCluesDensity, 0);
                Console.WriteLine("After simple backtracking");
                Board_Formatter.Print_Mat(mat);
                Console.WriteLine();
            }

            empties = new List<(int, int)>();
            for (int rows = 0; rows < mat.GetLength(0); rows++)
                for (int cols = 0; cols < mat.GetLength(0); cols++)
                {
                    if (mat[rows, cols] + '0' < '0' || mat[rows, cols] > mat.GetLength(0))
                        throw new Invalid_Character_Exception(mat[rows, cols], rows + 1, cols + 1);
                    if (mat[rows, cols] == 0)
                    {
                        empties.Add((rows, cols));
                        if (firstEmptyRow == -1)
                        {
                            firstEmptyRow = rows;
                            firstEmptyCol = cols;
                        }
                    }
                }
            string matString = "";
            BackTrack(mat, mat.GetLength(0));
            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(0); j++)
                    matString += (char)('0' + mat[i, j]);
            return matString;
        }
        public static bool SimpleBacktracking(int[,] mat, int target, int j)
        {
            if (target == 0)
                return true;
            var (rows, cols) = empties[j];
            int b = (rows / boxSize) * boxSize + (cols / boxSize);
            if (mat[rows, cols] == 0)
                for (int i = 1; i < mat.GetLength(0) + 1; i++)
                {
                    if (!Is_Already_Placed(rowsBitMask[rows], i)
                        && !Is_Already_Placed(colsBitMask[cols], i)
                        && !Is_Already_Placed(boxesBitMask[b], i))
                    {
                        AddSeenDigits(i, rows, cols);
                        mat[rows, cols] = i;
                        if (SimpleBacktracking(mat, target - 1, j + 1))
                            return true;
                        RemoveSeenDigit(i, rows, cols);
                        target++;
                        mat[rows, cols] = 0;
                    }
                }
            else
                SimpleBacktracking(mat, target, j + 1);
            return false;
        }
        private static int SumWeight(int r,int c)
        {
            return rowWeight[r] + colWeight[c] + boxWeight[(r/boxSize)*boxSize+(c/boxSize)];
        }
        private static bool BackTrack(int[,] mat, int len)
        {
            //base case: no empty cells (filled the entire board)
            if (empties.Count == 0)
                return true;
            int minOptions = len + 1,
                targetIndex = -1,
                bitmask = 0;
            for (int i = 0; i < empties.Count; i++)
            {
                var (r, c) = empties[i];
                int b = (r / boxSize) * boxSize + (c / boxSize);
                int used = boxesBitMask[b] | rowsBitMask[r] | colsBitMask[c];
                int options = len - Count_Bits(used);
                var (minR, minC) = (0, 0);
                if (targetIndex != -1)
                    (minR, minC) = empties[targetIndex];
                if (options < minOptions ||
                    (targetIndex != -1 && (options == minOptions && weight[r,c] > weight[minR,minC])))// weighted check
                {
                    minOptions = options;
                    targetIndex = i;
                    bitmask = (~used) & ((1 << len) - 1);
                    if (options == 1) //found the min options
                        break;
                }
            }
            var (targetedR, targetedC) = empties[targetIndex];
            empties.RemoveAt(targetIndex);
            while (bitmask > 0)
            {
                int pick = bitmask & (-bitmask);
                int num = Get_Bit_Position(pick);
                AddSeenDigits(num, targetedR, targetedC);
                mat[targetedR, targetedC] = num;
                if (BackTrack(mat, len))
                    return true;
                int maxBit = (1 << len) - 1;
                int targetB = (targetedR / boxSize) * boxSize + (targetedC / boxSize);
                
                weight[targetedR, targetedC]++;
                RemoveSeenDigit(num, targetedR, targetedC);
                mat[targetedR, targetedC] = 0;
                bitmask -= pick;
            }
            empties.Insert(targetIndex, (targetedR, targetedC));
            return false;
        }
        static void AddSeenDigits(int k, int row, int col)
        {
            if (Is_Already_Placed(rowsBitMask[row], k))
                throw new Duplicate_Val_In_Row(k, row + 1);
            if (Is_Already_Placed(colsBitMask[col], k))
                throw new Duplicate_Val_In_Column(k, col + 1);
            int b = (row / boxSize) * boxSize + (col / boxSize);
            if (Is_Already_Placed(boxesBitMask[b], k))
                throw new Duplicate_Val_In_Box(k, b + 1);
            rowsBitMask[row] |= 1 << (k - 1);
            colsBitMask[col] |= 1 << (k - 1);
            boxesBitMask[b] |= 1 << (k - 1);
        }
        static public int Get_Bit_Position(int mask)
        {
            int pos = 0;
            while ((mask >> pos) > 1) pos++;
            return pos + 1;
        }
        static void RemoveSeenDigit(int k, int row, int col)
        {
            rowsBitMask[row] &= ~(1 << ((k - 1)));
            colsBitMask[col] &= ~(1 << ((k - 1)));
            boxesBitMask[(row / boxSize) * boxSize + (col / boxSize)] &= ~(1 << ((k - 1)));
        }
        //recieves integer n and returns the number of 1 bits in it.
        public static int Count_Bits(int n)
        {
            int count = 0;
            while (n > 0)
            {
                n &= (n - 1);
                count++;
            }
            return count;
        }
        public static bool Is_Already_Placed(int bit, int n)
        {
            return (bit & (1 << (n - 1))) != 0;
        }

    }
}
