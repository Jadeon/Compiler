using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tokenizer
{
    class Symbol_Entry
    {
        public int line_num;
        public string value = "";
        public int type;
        public string entry_type = "";`
        public string scope = "";
        public int offset;
        public bool forward;
        public int arr_size;



        public Symbol_Entry(int line_num, string value, int type, string entry_type, string scope, int offset, bool forward, int arr_size)
        {
            this.line_num = line_num;
            this.value = value;
            this.type = type;
            this.entry_type = entry_type;
            this.scope = scope;
            this.offset = offset;
            this.forward = forward;
            this.arr_size = arr_size;
        }

    }
}