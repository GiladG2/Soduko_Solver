using System;
using System.Collections.Generic;
using System.Linq;
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
        static string msg = "Invalid soduko board len";
        public Invalid_Length_Exception() : base(msg)
        {
        }
    }
}
