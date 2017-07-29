using System;

namespace tokenizer
{
    class delimiter : Token
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
