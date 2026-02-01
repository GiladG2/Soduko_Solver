using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class LCV_Value : IComparable<LCV_Value>
    {
        private int options_removed; //Counts the amount of options removed by placing value
        private int value; 
        //Properties to get the fields
        public int Options_Removed { get => options_removed; set => options_removed = value; }
        public int Value { get => value; set => this.value = value; }
        public LCV_Value(int options_removed, int value) {
            this.options_removed = options_removed;
            this.value = value;
        }
        //CompareTo for array sort
        //returns -1 if the current value removes less options than other
        //returns 0 if the current value removes the same amount of options as other
        //returns 1 if the current value removes more options than other
        public int CompareTo(LCV_Value other) {
        
            if(this.Options_Removed <  other.Options_Removed) return -1;
            if(this.Options_Removed ==  other.Options_Removed) return 0;
            return 1;
        }
    }
}
