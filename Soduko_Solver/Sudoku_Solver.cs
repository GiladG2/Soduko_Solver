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
        static SudokuState state;

        public Sudoku_Solver(int[,] mat)
        {
            state = new SudokuState(mat);
        }
        static void ResetEmpties(int[,] mat)
        {
            state.Empties= new List<(int, int)>();
            for (int rows = 0; rows < mat.GetLength(0); rows++)
                for (int cols = 0; cols < mat.GetLength(0); cols++)
                {
                    if (mat[rows, cols] + '0' < '0' || mat[rows, cols] > mat.GetLength(0))
                        throw new Invalid_Character_Exception(mat[rows, cols], rows + 1, cols + 1);
                    if (mat[rows, cols] == 0)
                        state.Empties.Add((rows, cols));
                }
        }

        //func that recieves a soduko board and returns true if it is solvable and false if not
        static public string Solve_Sudoku(int[,] mat)
        {
            state.Empties.Clear();

            for (int rows = 0; rows < mat.GetLength(0); rows++)
                for (int cols = 0; cols < mat.GetLength(0); cols++)
                {
                    if (mat[rows, cols] + '0' < '0' || mat[rows, cols] > mat.GetLength(0))
                        throw new Invalid_Character_Exception(mat[rows, cols], rows + 1, cols + 1);
                    if (mat[rows, cols] != 0)
                        AddSeenDigits(mat[rows, cols], rows, cols);
                    else
                        state.Empties.Add((rows, cols));
                }
            if (state.Empties.Count > state.MinClueDensity)
            {
                SimpleBacktracking(mat, state.Empties.Count - state.MinClueDensity);
                Console.WriteLine("After simple backtracking");
                Board_Formatter.Print_Mat(mat);
                Console.WriteLine();
            }
            ResetEmpties(mat);
            string matString = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            BackTrack(mat, mat.GetLength(0));
            sw.Stop();
            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(0); j++)
                    matString += (char)('0' + mat[i, j]);
            return matString;
        }
        public static bool SimpleBacktracking(int[,] mat, int target, int j = 0)
        {
            if (target == 0)
                return true;
            var (rows, cols) = state.Empties[j];
            int b = (rows / state.BoxSize) * state.BoxSize + (cols / state.BoxSize);
            if (mat[rows, cols] == 0)
                for (int i = 1; i < mat.GetLength(0) + 1; i++)
                {
                    if (!Is_Already_Placed(state.RowsBitMask[rows], i)
                        && !Is_Already_Placed(state.ColsBitMask[cols], i)
                        && !Is_Already_Placed(state.BoxesBitMask[b], i))
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
        private static int CalculateOptions(int[,] mat, int len, int r, int c)
        {
            if (mat[r, c] != 0)
                return -1;
            int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
            int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
            return len - Count_Bits(used);
        }
        private static bool ForwardChecking(int[,] mat, int len, int r, int c)
        {
            for (int i = 0; i < len; i++)
                if (r != i && CalculateOptions(mat, len, i, c) == 0)
                    return false;
            for (int i = 0; i < len; i++)
                if (i != c && CalculateOptions(mat, len, r, i) == 0)
                    return false;
            int boxS = (r / state.BoxSize) * state.BoxSize;
            int boxF = (c / state.BoxSize) * state.BoxSize;
            for (int i = 0; i < boxS + state.BoxSize; i++)
                for (int j = 0; j < boxF + state.BoxSize; j++)
                    if (i != r && j != c && CalculateOptions(mat, len, i, j) == 0)
                        return false;
            return true;
        }
        private static (int, int) NakedSingles(int[,] mat, int len, int r, int c)
        {
            for (int i = 0; i < len; i++)
                if (mat[r, i] == 0 && r != i && CalculateOptions(mat, len, i, c) == 1)
                    return (i, c);
            for (int i = 0; i < len; i++)
                if (mat[i, c] == 0 && i != c && CalculateOptions(mat, len, r, i) == 1)
                    return (r, i);
            int boxS = (r / state.BoxSize) * state.BoxSize;
            int boxF = (c / state.BoxSize) * state.BoxSize;
            for (int i = 0; i < boxS + state.BoxSize; i++)
                for (int j = 0; j < boxF + state.BoxSize; j++)
                    if (mat[i, j] == 0 && i != r && j != c && CalculateOptions(mat, len, i, j) == 1)
                        return (i, j);
            return (-1, -1);
        }
        private static bool IsNakedSingle(int[,] mat, int len, int r, int c)
        {
            int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
            int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
            int options = len - Count_Bits(used);
            return options == 1;
        }
        private static bool ChainNakedSingles(int[,] mat, int len)
        {
            var (r, c) = (0, 0);
            bool propagation = true;
            do
            {
                propagation = false;
                for (int i = state.Empties.Count-1; i >=0; i--)
                {
                    (r, c) = state.Empties[i];
                    int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
                    int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
                    int options = len - Count_Bits(used);
                    if (options == 0)
                        return false;
                    if(options == 1)
                    {
                        propagation = true;
                        int bitmask = (~used) & ((1 << len) - 1);
                        int pick = bitmask & (-bitmask);
                        int num = Get_Bit_Position(pick);
                        AddSeenDigits(num, r, c);
                        mat[r, c] = num;
                        state.Empties.Remove((r, c));
                    }
                }
            }
            while (propagation);
            return true;
        }
        private static bool BackTrack(int[,] mat, int len)
        {
            //if(!ChainNakedSingles(mat, len))
            //   return false;
            //base case: no empty cells (filled the entire board)\
            if (state.Empties.Count == 0)
                return true;
            int minOptions = len + 1,
                targetIndex = -1,
                bitmask = 0;
            for (int i = 0; i < state.Empties.Count; i++)
            {
                var (r, c) = state.Empties[i];
                int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
                int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
                int options = len - Count_Bits(used);
                var (minR, minC) = (0, 0);
                if (targetIndex != -1)
                    (minR, minC) = state.Empties[targetIndex];
                if (options < minOptions ||
                    (targetIndex != -1 && (options == minOptions && state.Weight[r, c] > state.Weight[minR, minC])))// weighted check
                {
                    minOptions = options;
                    targetIndex = i;
                    bitmask = (~used) & ((1 << len) - 1);
                }
            }
            var (targetedR, targetedC) = state.Empties[targetIndex];
            state.Empties.RemoveAt(targetIndex);
            while (bitmask > 0)
            {
                int pick = bitmask & (-bitmask);
                int num = Get_Bit_Position(pick);
                AddSeenDigits(num, targetedR, targetedC);
                mat[targetedR, targetedC] = num;
                //if (ForwardChecking(mat, len,targetedR, targetedC))
                if (BackTrack(mat, len))
                    return true;
                int targetB = (targetedR / state.BoxSize) * state.BoxSize + (targetedC / state.BoxSize);
                state.Weight[targetedR, targetedC]++;
                RemoveSeenDigit(num, targetedR, targetedC);
                mat[targetedR, targetedC] = 0;
                bitmask -= pick;
            }
            state.Empties.Insert(targetIndex, (targetedR, targetedC));
            return false;
        }
        static void AddSeenDigits(int k, int row, int col)
        {
            if (Is_Already_Placed(state.RowsBitMask[row], k))
                throw new Duplicate_Val_In_Row(k, row + 1);
            if (Is_Already_Placed(state.ColsBitMask[col], k))
                throw new Duplicate_Val_In_Column(k, col + 1);
            int b = (row / state.BoxSize) * state.BoxSize + (col / state.BoxSize);
            if (Is_Already_Placed(state.BoxesBitMask[b], k))
                throw new Duplicate_Val_In_Box(k, b + 1);
            state.RowsBitMask[row] |= 1 << (k - 1);
            state.ColsBitMask[col] |= 1 << (k - 1);
            state.BoxesBitMask[b] |= 1 << (k - 1);
        }
        static public int Get_Bit_Position(int mask)
        {
            int pos = 0;
            while ((mask >> pos) > 1) pos++;
            return pos + 1;
        }
        static void RemoveSeenDigit(int k, int row, int col)
        {
            state.RowsBitMask[row] &= ~(1 << ((k - 1)));
            state.ColsBitMask[col] &= ~(1 << ((k - 1)));
            state.BoxesBitMask[(row / state.BoxSize) * state.BoxSize + (col / state.BoxSize)] &= ~(1 << ((k - 1)));
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