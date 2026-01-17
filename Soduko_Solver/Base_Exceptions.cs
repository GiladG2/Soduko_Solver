using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Base_Exceptions : Exception
    {
        public Base_Exceptions(string msg)
        {
            Console.WriteLine($"Error : {msg}");
        }
    }
    internal class Invalid_Length_Exception: Base_Exceptions
    {
        public Invalid_Length_Exception() : base("Invalid soduko board len")
        {
        }
    }
    internal class Invalid_Character_Exception: Base_Exceptions
    {
        public Invalid_Character_Exception(int val, int i,int j) : base($"Invalid character {(char)(val + '0')} at ({i},{j})")
        {
        }
    }
    internal class Unsolvable_Mat_Exception : Base_Exceptions
    {
        public Unsolvable_Mat_Exception() : base("The board is unsolveable")
        {
        }
    }
    internal class Duplicate_Val_In_Column: Base_Exceptions
    {
        public Duplicate_Val_In_Column(int val,int col) : base($"Unsolveable board, {val} appears twice at column {col}")
        {
        }
    }
    internal class Duplicate_Val_In_Row : Base_Exceptions
    {
        public Duplicate_Val_In_Row(int val, int row) : base($"Unsolveable board, {val} appears twice at row {row}")
        {

        }
    }
    internal class Duplicate_Val_In_Box : Base_Exceptions
    {
        public Duplicate_Val_In_Box(int val, int box) : base($"Unsolveable board, {val} appears twice at box {box}")
        {

        }
    }
}
