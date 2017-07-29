using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tokenizer
{
    public class delimiter : Token
    {
        public Boolean keepable;

        public delimiter(Boolean keepable)
        {
            this.keepable = keepable;
        }

        public delimiter(string value, string type, int index, int row, Boolean keepable)
        {
            this.value = value;
            this.type = type;
            this.index = index;
            this.row = row;
            this.keepable = keepable;
        }
    }
}
