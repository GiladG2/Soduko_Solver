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
        static public float Test(string matBase)
        {
            float totalTime = 0;
            try
            {
                int[,] mat = Board_Formatter.Format(matBase);
                Sudoku_Solver s = new Sudoku_Solver(mat);
                Console.WriteLine($"Board size : {mat.GetLength(0)} X {mat.GetLength(0)}");
                Console.WriteLine("Original mat");
                Board_Formatter.Print_Mat(mat);
                Stopwatch stopwatch = Stopwatch.StartNew();
                string final_mat = Sudoku_Solver.Solve_Sudoku();
                stopwatch.Stop();
                totalTime = stopwatch.ElapsedMilliseconds;
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine();
                Console.WriteLine("Solved mat");
                Board_Formatter.Print_Mat(mat);
                Console.WriteLine($"Execution Time: {ts.TotalSeconds} s");
                if (!Tester.TestSolvedSudoku(mat))
                    throw new Unsolvable_Mat_Exception();
                Console.WriteLine($"Solved mat: {final_mat}");
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (Invalid_Length_Exception ex) {}
            catch (Invalid_Character_Exception ex) { }
            catch (Duplicate_Val_In_Box ex) { }
            catch (Unsolvable_Mat_Exception ex) { }
            catch (Duplicate_Val_In_Column ex) { }
            catch (Duplicate_Val_In_Row ex) { }
            catch (Empty_Board_Exception ex) { }
            return totalTime;
        }
        static void Main(string[] args)
        {
            string matBase44 = "0003040010040030";
            string matBase = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            string matBase1616 = ";5?000=000000>030000046050100=:0:<80?00000;@20600910503;:0000000800000012@:070;0000@0600>0900:<15:00>0;00=8300201070@0920<0050>000009005060<00?8@0>0=30082?4:00;=;000>0000301600<04060000>01@30203000000=050600000=?79030;428<0:00:405000900=;0>080040<>73@:0200";
            string matBase2525 = "0E003000000000F000<0000000=00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000600000000000000009000000400000000000000000000000000000000000000000000000000000500000000000000000000000700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000B000000000000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000000000000000A000";
            string hardmat1616 = "1030000000020000020000000000100000000000000000000000000000000000000123000000000000000000000000000000000100000000000000000000000000000000012000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000100000000000000000000000";
            string hardMat2525 = "1020000300000000000000000000000000000000000000000003000020000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200000000100000000000000300000000000000000000000004000010000000000000000001000000002000000000000000000000000000100000000000000000000000000000000002000000000010000000000000000000000000000000000000000000100000000000002000000000020000000000000000000000000001020000000000000001000000000000000020000000000000000000000000001200000000000000000000000000000100000000000000000000000000000000000010000000004000000000000000012000003000000010000000000000200000000000000000000000000000";
            string hardmat2525_2 = "0E003200000000F000<0000000=00000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200010000000000000000000600000000000000009000000401000000001000000000000000000200000000000000000000000500001000000000000000000700000000000000000020000001000000000000000000000000000000000000000000000000000000000000000000010000000000000200000000000000000000000000000000000000000000000000000001000000002000000000010000000020000030000000000000000B000000001000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000001000000000A000";
            string matError = "005300000800000020070010500400005300010070006003200080000060500000009004000000030";
            string[] testBoards = { matBase44, matBase,matBase1616,hardmat1616,hardMat2525,hardmat2525_2};
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < testBoards.Length; i++)
              Test(testBoards[i]);
            Test(matBase44);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            sw.Start();
            Stopwatch sw2 = new Stopwatch();
            string hardestMat = "";
            double maxTime = 0;
            string testMat = "";
            float totalTime = 0;
            for (int i = 0; i < 0; i++)
            {
                
                Console.WriteLine($"Sudoku num. {i+1}");
                //testMat = "1E45326789;=>?F@AB<CD:GHI?=DFGEHI14:@ABC7352896;<>96BCH;<>A:13457DEFGI82=?@278:;@CDFGE9<HI64=>?13AB5<>@AI5=?B3268DG1H9:;C4EF7F3=>@42<HICD6E1A58B7?G9:;B:7I6>AGE@4F5;?C=2D9H1<384?A2<C185FHI3@9G6;E:>BD7=E5;H8BD:9=<>7G24?1F3@I6CAG19DC673;?A8:=BI@>H<2E45F:9CG=34A62FBI7;H>D85<@?E1D<H6495C=1@?E83BIA7G:>F;2;@?B5:>ED7GA24HF9<C1=83I6AI237<@FG86:91>=;E?45HBDC8F>1E?BHI;D<=C52:@36A97G4=H37F8952<BE16@:C?ID4;>AGI2G9A=;63>?7C:48FH5EB<@1D5DE4>F?@:A3G;<=9176BIC82H68<?BGE17CI5HAD;24@>3F:=9C;1@:HIB4D92F>8<G3A=E756?H4I<?A39>B=;@2E57G1F6DC8:@A6=218;?E74DF:3<C9HG5I>B3GFE1D:=@H5CB96>8I;A7?24<7C58DIF2<6>HG3A?B:4@;=19E>B:;97G4C581?I<ED6=2FAH@3";
                testMat = "14356789:;<2=>?@;2<@345:9=>?16789:>?12<=678@345;678=;>?@34159:2<<?712345@89:;=>6:=;8>976<241?@354592?@=1>36;:8<7>@63:<;8?57=2149764:<?>;5128@9=351?9@=627<:34;8>28@;51:34>=97<6?3>=<7894;?@6521:=;26951?8@3<>7:48954=6372:;><?@1@3:>4;2<16?7859=?<178:@>=95463;2";
                //testMat = "2143341213244231";
                //testMat = "831529674796814253542637189159783426483296715627145938365471892274958361918362547";
                int[,] testMatFormatted = Board_Formatter.Format(testMat);
                Sudoku_Solver s2 = new Sudoku_Solver(testMatFormatted);
                testMat = Tester.GenerateSudokuBoard(testMatFormatted, (int)(testMatFormatted.GetLength(0) * testMatFormatted.GetLength(0) * 0.9));
                sw2.Start();
                totalTime +=Test(testMat);
                sw2.Stop();
                if(sw2.ElapsedMilliseconds > maxTime)
                {
                    maxTime = sw2.ElapsedMilliseconds;
                    hardestMat = testMat;
                }
            }
            Console.WriteLine(hardestMat);
            sw.Stop();
            Console.WriteLine("Elapsed : " +(float)totalTime/1000);
            Console.WriteLine();
            Console.WriteLine("Write 'End' to exit the solver");
            while (true)
            {
                Console.WriteLine("Enter a mat");
                matBase = Console.ReadLine();
                if (matBase == "End")
                    break;
                if(matBase == "a25")
                {
                    sw.Start();
                    for (int i = 0; i < 100; i++)
                    {
                        Console.WriteLine($"Sudoku num. {i}");
                        Test("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    }
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                }
                if(matBase == "again")
                {
                    sw.Start();
                    for (int i = 0; i < 1000; i++)
                        Test("0E003000000000F000<0000000=00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000600000000000000009000000400000000000000000000000000000000000000000000000000000500000000000000000000000700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000B000000000000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000000000000000A000");
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                }
                Test(matBase);
            }
            Console.WriteLine("Thanks for using my Sudoku Solver!");
        }
    }
}
