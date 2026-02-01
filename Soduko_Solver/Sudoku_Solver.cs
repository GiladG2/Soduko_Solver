using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Soduko_Solver.Sudoku_Solver_Helper;
namespace Soduko_Solver
{
    public class Sudoku_Solver
    {
        public static SudokuState state;
        public Sudoku_Solver(int[,] mat)
        {
            state = new SudokuState(mat);
            Sudoku_Solver_Helper s = new Sudoku_Solver_Helper(state);
        }

        //func that recieves a Sudoku board and returns true if it is solvable and false if not
        static public string Solve_Sudoku()
        {
            state.Empties.Clear();
            for (int rows = 0; rows < state.Mat.GetLength(0); rows++)
                for (int cols = 0; cols < state.Mat.GetLength(0); cols++)
                {
                    if (state.Mat[rows, cols] == -16)
                        throw new Invalid_Space_Exception();
                    if (state.Mat[rows, cols] + '0' < '0' || state.Mat[rows, cols] > state.Mat.GetLength(0))
                        throw new Invalid_Character_Exception(state.Mat[rows, cols], rows + 1, cols + 1);
                    if (state.Mat[rows, cols] != 0)
                       AddSeenDigits(state.Mat[rows, cols], rows, cols);
                    else
                        state.Empties.Add((rows, cols));
                }
            //Fill with simple brute force in order to have enough constraints for the MRV
            if (state.Empties.Count > state.MinClueDensity)
               SimpleBacktracking(state.Empties.Count - state.MinClueDensity); 
            ResetEmpties(state.Mat);
            string matString = "";
            //Continue solving after using simpler brute force
            BackTrack();
            for (int i = 0; i < state.Mat.GetLength(0); i++)
                for (int j = 0; j < state.Mat.GetLength(0); j++)
                    matString += (char)('0' + state.Mat[i, j]);
            return matString;
        }
        //Brute force fill if there are not enough constraints to start using MRV
        public static bool SimpleBacktracking(int target, int j = 0)
        {
            //Filled the neccessary amount of cells
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
        //Function that chains naked singles (implementing constraint propagation)
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
                    int used = GetUsedValues(r, c);
                    int options = GetAmoutOfOptions(used);
                    if (options == 0)
                    {
                        state.Weight[r, c]++;
                        return false;
                    }//Failed branch - a cell's domain = 0, meaning no valid placements in an empty cell
                    if (options == 1) //A cell with one option - found a naked single
                    {
                        propagation = true; // Found a naked single => Continue chaining
                        int bitmask = (~used) & ((1 << state.Len) - 1);
                        int pick = bitmask & (-bitmask);
                        int num = Get_Bit_Position(pick);
                        PlaceNumber(num, r, c, i);
                        state.Empties.RemoveAt(i);
                    }
                }
            }
            while (propagation);
            return true;
        }
        //Function that returns if it is effiecient to chain naked singles
        private static bool IsNakedSinglesEfficient()
        {
            return state.Len > 9 // If the mat is large enough
                && (double)state.Empties.Count / (state.Len * state.Len) < 0.3 // and 70% of the mat is filled 
                &&!ChainNakedSingles(state.Stack); //chain naked singles
        }
        //Function that recieves a number, cell's coordinates and its index in the empties list and places it on the board
        private static void PlaceNumber(int num,int r,int c,int i)
        {
            AddSeenDigits(num, r, c);
            state.Stack.Push(num, i, r, c);
            state.Mat[r, c] = num;
        }
        //Return a sorted array where the value that removes the least amount of options is at the start
        private static LCV_Value[] LCV(int r,int c,int used)
        {
            int[] domain = GetDomain(used);//Get cell (r,c)'s domain    
            LCV_Value[] lcvSortedArray = new LCV_Value[domain.Length];
            //If found a naked single do not do redundant for loops
            if (domain.Length == 1) {
                lcvSortedArray[0] = new LCV_Value(0,domain[0]);
                return lcvSortedArray;
            }
            int currentOptionsRemoved = 0;
            for(int i = 0; i < domain.Length; i++)
            {
                currentOptionsRemoved = 0;

                //Check how many options value removes from its column
                for(int j = 0;j<state.Len;j++)
                {
                    if (j == r)
                        continue;
                    if (state.Mat[j,c] == 0 && !Is_Already_Placed(GetUsedValues(j,c), domain[i]))
                        currentOptionsRemoved++;
                }
                //Check how many options value removes from its row
                for (int j = 0; j < state.Len; j++)
                {
                    if (j == c)
                        continue;
                    if (state.Mat[r, j] == 0 && !Is_Already_Placed(GetUsedValues(r,j), domain[i]))
                        currentOptionsRemoved++;
                }
                //Check how many options value removes from its box
                int boxS = (r / state.BoxSize) * state.BoxSize;
                int boxF = (c / state.BoxSize) * state.BoxSize;
                for (int k = boxS; k < boxS + state.BoxSize; k++)
                    for (int j = boxF; j < boxF + state.BoxSize; j++)
                    {
                        if (k == r || j == c ) // Already checked for row and column
                            continue;
                        if (state.Mat[k,j] == 0 && !Is_Already_Placed((GetUsedValues(k, j)), domain[i]))
                            currentOptionsRemoved++;
                    }
                lcvSortedArray[i] = new LCV_Value(currentOptionsRemoved, domain[i]);
            }
            //Sort the array with CompareTo written in LCV_Value
            Array.Sort(lcvSortedArray);
            return lcvSortedArray;
        }

        private static bool BackTrack()
        {
            int before_Naked_Singles = state.Stack.Len;
            if (IsNakedSinglesEfficient())
            {
               RollBack(state.Stack.Len - before_Naked_Singles, 1);// RollBack the changes of a failed naked singles chain
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
                int used = GetUsedValues(r, c);
                int options = GetAmoutOfOptions(used);
                var (minR, minC) = (0, 0);
                if (targetIndex != -1)
                    (minR, minC) = state.Empties[targetIndex];
                if (options < minOptions ||
                        (targetIndex != -1 
                        && (options == minOptions 
                        && state.Weight[r, c] > state.Weight[minR, minC])))// weighted check
                {
                    minOptions = options;
                    targetIndex = i;
                    bitmask = (~used) & ((1 << state.Len) - 1);
                    if (options == 1) //Found a naked single => has to have the least amount of options, thus break (finish the search
                        break;        //for the least amount of options)
                }
            }
            var (targetedR, targetedC) = state.Empties[targetIndex];
            state.Empties.RemoveAt(targetIndex);
            int usedValues = GetUsedValues(targetedR, targetedC);
            LCV_Value[] domain = LCV(targetedR, targetedC, usedValues);
            for(int i =0;i<domain.Length;i++)
            {
                int currentState = state.Stack.Len;
                int num = domain[i].Value; //Get the best value based on LCV
                PlaceNumber(num, targetedR, targetedC, targetIndex);
                if (BackTrack())
                    return true;
                state.Weight[targetedR, targetedC]++;
                int diff = state.Stack.Len - currentState;
                RollBack(diff, 0);
            }
            state.Empties.Insert(targetIndex, (targetedR, targetedC));
            return false;
        }


        
        //Commented due to making my solver slower
        /*
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
        }*/
    }
}