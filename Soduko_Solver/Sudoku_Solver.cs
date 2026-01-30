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
        public static SudokuState state;

        public Sudoku_Solver(int[,] mat)
        {
            state = new SudokuState(mat);
        }
        static void ResetEmpties(int[,] mat)
        {
            state.Empties = new List<(int, int)>();
            for (int rows = 0; rows < mat.GetLength(0); rows++)
                for (int cols = 0; cols < mat.GetLength(0); cols++)
                {
                    if (state.Mat[rows, cols] + '0' < '0' || state.Mat[rows, cols] > state.Mat.GetLength(0))
                        throw new Invalid_Character_Exception(state.Mat[rows, cols], rows + 1, cols + 1);
                    if (state.Mat[rows, cols] == 0)
                        state.Empties.Add((rows, cols));
                }
        }

        //func that recieves a soduko board and returns true if it is solvable and false if not
        static public string Solve_Sudoku()
        {
            state.Empties.Clear();
            for (int rows = 0; rows < state.Mat.GetLength(0); rows++)
                for (int cols = 0; cols < state.Mat.GetLength(0); cols++)
                {
                    if (state.Mat[rows, cols] + '0' < '0' || state.Mat[rows, cols] > state.Mat.GetLength(0))
                        throw new Invalid_Character_Exception(state.Mat[rows, cols], rows + 1, cols + 1);
                    if (state.Mat[rows, cols] != 0)
                        AddSeenDigits(state.Mat[rows, cols], rows, cols);
                    else
                        state.Empties.Add((rows, cols));
                }
            if (state.Empties.Count > state.MinClueDensity)
                SimpleBacktracking(state.Empties.Count - state.MinClueDensity);
            ResetEmpties(state.Mat);
            string matString = "";
            BackTrack();
            for (int i = 0; i < state.Mat.GetLength(0); i++)
                for (int j = 0; j < state.Mat.GetLength(0); j++)
                    matString += (char)('0' + state.Mat[i, j]);
            return matString;
        }
        public static bool SimpleBacktracking(int target, int j = 0)
        {
            if (target == 0)
                return true;
            var (rows, cols) = state.Empties[j];
            int b = (rows / state.BoxSize) * state.BoxSize + (cols / state.BoxSize);
            if (state.Mat[rows, cols] == 0)
                for (int i = 1; i < state.Mat.GetLength(0) + 1; i++)
                {
                    if (!Is_Already_Placed(state.RowsBitMask[rows], i)
                        && !Is_Already_Placed(state.ColsBitMask[cols], i)
                        && !Is_Already_Placed(state.BoxesBitMask[b], i))
                    {
                        AddSeenDigits(i, rows, cols);
                        state.Mat[rows, cols] = i;
                        if (SimpleBacktracking(target - 1, j + 1))
                            return true;
                        RemoveSeenDigit(i, rows, cols);
                        target++;
                        state.Mat[rows, cols] = 0;
                    }
                }
            else
                SimpleBacktracking(target, j + 1);
            return false;
        }
        private static int CalculateOptions(int r, int c)
        {
            if (state.Mat[r, c] != 0)
                return -1;
            int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
            int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
            return state.Len - Count_Bits(used);
        }
        private static bool ForwardChecking(int r, int c)
        {
            for (int i = 0; i < state.Len; i++)
                if (r != i && CalculateOptions(i, c) == 0)
                    return false;
            for (int i = 0; i < state.Len; i++)
                if (i != c && CalculateOptions(r, i) == 0)
                    return false;
            int boxS = (r / state.BoxSize) * state.BoxSize;
            int boxF = (c / state.BoxSize) * state.BoxSize;
            for (int i = boxS; i < boxS + state.BoxSize; i++)
                for (int j = boxF; j < boxF + state.BoxSize; j++)
                {
                    if (i == r && j == c)
                        continue;
                    if (CalculateOptions(i, j) == 0)
                        return false;
                }


            return true;
        }
        private static bool ChainNakedSingles(SudokuStack s)
        {
            var (r, c) = (0, 0);
            bool propagation = true;
            do
            {
                propagation = false;
                for (int i = state.Empties.Count - 1; i >= 0; i--)
                {
                    (r, c) = state.Empties[i];
                    int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
                    int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
                    int options = state.Len - Count_Bits(used);
                    if (options == 0)
                        return false;
                    if (options == 1)
                    {
                        propagation = true;
                        int bitmask = (~used) & ((1 << state.Len) - 1);
                        int pick = bitmask & (-bitmask);
                        int num = Get_Bit_Position(pick);
                        AddSeenDigits(num, r, c);
                        state.Mat[r, c] = num;
                        state.Empties.RemoveAt(i);
                        s.Push(num, i, r, c);

                    }
                }
            }
            while (propagation);
            return true;
        }
        private static bool IsNakedSinglesEfficient()
        {
            return state.Len > 30 // If the mat is large enough
                && (double)state.Empties.Count / (state.Len * state.Len) < 0.3 // and 70% of the mat is filled 
                &&!ChainNakedSingles(state.Stack); //chain naked singles
        }
        private static bool BackTrack()
        {
            int before_Naked_Singles = state.Stack.Len;
            if (IsNakedSinglesEfficient())
            {
                RollBack(state.Stack.Len - before_Naked_Singles, 1);
                return false;
            }
            //base case: no empty cells (filled the entire board)

            if (state.Empties.Count == 0)
                return true;
            int minOptions = state.Len + 1,
                targetIndex = -1,
                bitmask = 0;
            //MRV => iterate over empties and find the cell with the least amount of values
            //if 2 cells have the same amount of options, choose the one 
            //that has been resulting in more failed sudokus (with the higher weight).
            for (int i = 0; i < state.Empties.Count; i++)
            {
                var (r, c) = state.Empties[i];
                int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
                int used = state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
                int options = state.Len - Count_Bits(used);
                var (minR, minC) = (0, 0);
                if (targetIndex != -1)
                    (minR, minC) = state.Empties[targetIndex];
                if (options < minOptions ||
                    (targetIndex != -1 && (options == minOptions && state.Weight[r, c] > state.Weight[minR, minC])))// weighted check
                {
                    minOptions = options;
                    targetIndex = i;
                    bitmask = (~used) & ((1 << state.Len) - 1);
                }
            }
            var (targetedR, targetedC) = state.Empties[targetIndex];
            state.Empties.RemoveAt(targetIndex);
            while (bitmask > 0)
            {
                int currentState = state.Stack.Len;
                int pick = bitmask & (-bitmask);
                int num = Get_Bit_Position(pick);
                AddSeenDigits(num, targetedR, targetedC);
                state.Stack.Push(num, targetIndex, targetedR, targetedC);
                state.Mat[targetedR, targetedC] = num;
                if (BackTrack())
                    return true;
                state.Weight[targetedR, targetedC]++;
                int diff = state.Stack.Len - currentState;
                RollBack(diff,0); // Rollback the changes of a failed recursion (guess) branch
                //RemoveSeenDigit(num, targetedR, targetedC);
                //state.Mat[targetedR, targetedC] = 0;
                bitmask -= pick;
            }
            state.Empties.Insert(targetIndex, (targetedR, targetedC));
            return false;
        }
        static void RollBack(int diff, int mode)
        {
            for (int j = 0; j < diff; j++)
            {
                var (number, index, (row, collumn)) = state.Stack.Pop();
                RemoveSeenDigit(number, row, collumn);
                state.Mat[row, collumn] = 0;
                if ((mode == 0 && j != diff - 1) || mode == 1)
                    state.Empties.Insert(index, (row, collumn));
            }
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