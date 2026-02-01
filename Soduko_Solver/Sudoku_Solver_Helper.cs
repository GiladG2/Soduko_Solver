using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class Sudoku_Solver_Helper
    {
        static SudokuState state;
        public Sudoku_Solver_Helper(SudokuState state2) { 
            state = state2;
        }
        
        //Function that rolls back changes made by a failed recursion branch
        public static void RollBack(int diff, int mode)
        {
            for (int j = 0; j < diff; j++)
            {
                var (number, index, (row, collumn)) = state.Stack.Pop();
                RemoveSeenDigit(number, row, collumn);
                state.Mat[row, collumn] = 0;
                if ((mode == 0 && j != diff - 1) || mode == 1)   //If rolling back from naked singles => roll everything back
                    state.Empties.Insert(index, (row, collumn)); //If not => do not roll the current guess.

            }
        }
        //Function that resets the empties list
        public static void ResetEmpties(int[,] mat)
        {
            //Intialize the empties list
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
        //Function that returns a bit with all the possible values of cell (r,c)
        public static int GetUsedValues(int r, int c)
        {
            int b = (r / state.BoxSize) * state.BoxSize + (c / state.BoxSize);
            return state.BoxesBitMask[b] | state.RowsBitMask[r] | state.ColsBitMask[c];
        }
        //Function that returns the size of the cell's domain (amount of valid options)
        public static int GetAmoutOfOptions(int used)
        {
            return state.Len - Count_Bits(used);
        }
        //Function that adds the number k to the bitmasks of row, col and their box
        public static void AddSeenDigits(int k, int row, int col)
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
        //Func that recives a bitmask and returns the position of the heighest turned on bit in the bitmask
        public static int Get_Bit_Position(int mask)
        {
            int pos = 0;
            while ((mask >> pos) > 1) pos++;
            return pos + 1;
        }
        //Function that returns an array of the domain
        public static int[] GetDomain(int used)
        {
            int[] domain = new int[GetAmoutOfOptions(used)];
            int k = 0;
            for (int j = 0; j < state.Len; j++)
            {
                if ((used & 1) == 0)
                {
                    domain[k] = j + 1;
                    k++;
                }
                used >>= 1;
            }
            return domain;
        }
        //Function that removes the number k from the bitmasks of row, col and their box
        public static void RemoveSeenDigit(int k, int row, int col)
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
        //Function that checks if the nth bit is already turned on in bit
        public static bool Is_Already_Placed(int bit, int n)
        {
            return (bit & (1 << (n - 1))) != 0;
        }
    }
}
