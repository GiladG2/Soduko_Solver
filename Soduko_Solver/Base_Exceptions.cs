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
}
