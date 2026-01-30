using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class SudokuStack
    {
        Stack<(int, int, (int, int))> s = new Stack<(int, int, (int, int))>();
        int len = 0;
        public int Len { get=> len; set => len = value; }
        public void Push(int num,int i,int r,int c)
        {
            s.Push((num, i, (r, c)));
            len++;
        }
        public (int,int,(int,int)) Pop()
        {
            len--;
            return s.Pop();
        }
    }
}
