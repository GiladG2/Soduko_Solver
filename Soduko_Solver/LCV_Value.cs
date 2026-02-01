using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class LCV_Value : IComparable<LCV_Value>
    {
        int options_removed;
        int value;
        public int Options_Removed { get => options_removed; set => options_removed = value; }
        public int Value { get => value; set => this.value = value; }
        public LCV_Value(int options_removed, int value) {
            this.options_removed = options_removed;
            this.value = value;
        }
        public int CompareTo(LCV_Value other) {
        
            if(this.Options_Removed <  other.Options_Removed) return -1;
            if(this.Options_Removed ==  other.Options_Removed) return 0;
            return 1;
        }
    }
}
