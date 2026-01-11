using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //800000070006010053040600000000080400003000700020005038000000800004050061900002000
            //010203040506000070000809000A0B0C0000000000D0E0F000000000100002030004050600000000070008090000A0B000C0D000E0F01000002000304000005060007080009000A0B0C0D0E0F00

            while (true) {
                Console.WriteLine("Enter a soduko board");
                string matBase = Console.ReadLine();
                Board_Formatter board_Formatter = new Board_Formatter();
                board_Formatter.Print_Mat(board_Formatter.Format(matBase));

            }
        }
    }
}
