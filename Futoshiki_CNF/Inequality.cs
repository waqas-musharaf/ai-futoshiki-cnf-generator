using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futoshiki_CNF
{
    public class Inequality
    {
        public int isLessRow;
        public int isLessColumn;
        public int isGreaterRow;
        public int isGreaterColumn;    

        public Inequality() { }

        public Inequality(int isLessRow, int isLessColumn, int isGreaterRow, int isGreaterColumn)
        {                    
            this.isLessRow = isLessRow;
            this.isLessColumn = isLessColumn;
            this.isGreaterRow = isGreaterRow;
            this.isGreaterColumn = isGreaterColumn;
        }
    }
}
