using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    //Class that represents the changes/operations stack
    public class SudokuStack
    {
        private Stack<(int, int, (int, int))> s = new Stack<(int, int, (int, int))>(); // ()
        private int len = 0; //Current length of the stack
        //Property to get len
        public int Len { get=> len; set => len = value; }
        //Push to insert values to the stack
        public void Push(int num,int i,int r,int c)
        {
            s.Push((num, i, (r, c)));
            len++;
        }
        //Pop value from the stack and return it
        public (int,int,(int,int)) Pop()
        {
            len--;
            return s.Pop();
        }
    }
}
