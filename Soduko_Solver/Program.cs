using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace Soduko_Solver
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            int used = 45;
            int[] domain = new int[5];
            int k = 0;
            Console.WriteLine(9 - Sudoku_Solver.Count_Bits(used));
            for (int j = 0; j < 9; j++)
            {
                if ((used & 1) == 0)
                {
                    domain[k] = j + 1;
                    k++;
                }
                used >>= 1;
            }
            for (int i = 0; i < 5; i++)
                Console.Write($"domain[{i}] => {domain[i]} ");
        string matBase = ""; //saves the string of the mat
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Write 'End' to exit the solver");
            while (true)
            {
                Console.WriteLine("Enter a mat");
                matBase = Console.ReadLine();
                if (matBase == "End")
                    break;

                if(matBase == "Fight")
                {
                    sw.Start();
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine($"Sudoku num. {i + 1}");
                        string testMat = "1E45326789;=>?F@AB<CD:GHI?=DFGEHI14:@ABC7352896;<>96BCH;<>A:13457DEFGI82=?@278:;@CDFGE9<HI64=>?13AB5<>@AI5=?B3268DG1H9:;C4EF7F3=>@42<HICD6E1A58B7?G9:;B:7I6>AGE@4F5;?C=2D9H1<384?A2<C185FHI3@9G6;E:>BD7=E5;H8BD:9=<>7G24?1F3@I6CAG19DC673;?A8:=BI@>H<2E45F:9CG=34A62FBI7;H>D85<@?E1D<26495C=1@?E83BIA7G:>F;H;@?B5:>ED7GA24HF9<C1=83I6AIH37<@FG86:91>=;E?45DB2C8F>E1?BHI;D<=C52:@36497AG=H37F8952<BE16@:C?IDA;>G4I2G9A=;63>?7C:48FH5EB<@1D5DE4>F?@:A3G;<=9176BIHC8268<?BGE17CI5HAD;24@>3F:=9C;1@:HIB4D92F>8<G3A=E756?H4I<?A39>B=;@2E57G1F6C8D:@A6=218;?E74DF:3<C9HG5I>B3GF1ED:=@H5CB96>8I;A7?24<7C58DIF2<6>HG3A?B:4@;=19E>B:;97G4C581?I<ED6=2FAH@3";
                        int[,] testMatFormatted = Board_Formatter.Format(testMat);
                        Sudoku_Solver s2 = new Sudoku_Solver(testMatFormatted);
                        testMat = Tester.GenerateSudokuBoard(testMatFormatted, (int)(testMatFormatted.GetLength(0) * testMatFormatted.GetLength(0) * 0.97));
                        Sudoku_Runner.Run(testMat);
                    }
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                    continue;
                }
                if(matBase == "a25")
                {
                    sw.Start();
                    for (int i = 0; i < 100; i++)
                    {
                        Console.WriteLine($"Sudoku num. {i}");
                        Sudoku_Runner.Run("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    }
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                    continue;
                }
                if(matBase == "again")
                {
                    sw.Start();
                    for (int i = 0; i < 1000; i++)
                        Sudoku_Runner.Run("0E003000000000F000<0000000=00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000600000000000000009000000400000000000000000000000000000000000000000000000000000500000000000000000000000700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000B000000000000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000000000000000A000");
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                    continue;
                }
                Sudoku_Runner.Run(matBase);
            }
            Console.WriteLine("Thanks for using my Sudoku Solver!");
        }
    }
}
