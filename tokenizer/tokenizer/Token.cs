using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tokenizer
{
    class Token
    {
        public string value = "";
        public int type;
        public int index;
        public int row;

        public Token();

        public Token(string value)
        {
            this.value = value;
        }

        public Token(string value, int type)
        {
            this.value = value;
            this.type = type;
        }

        public Token(string value, int type, int index, int row)
        {
            this.value = value;
            this.type = classify(value);
            this.index = index;
            this.row = row;
        }

        public int classify(string value)
        {
            int temp = 0;
            System.CodeDom.Compiler.CodeDomProvider provider =
                System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            switch (value)
            {
                case "*":
                    return 1;
                case "+":
                    return 2;
                case "-":
                    return 3;
                case "/":
                    return 4;
                case "%":
                    return 5;
                case "^":
                    return 6;
                case "<=":
                    return 7;
                case "<":
                    return 8;
                case "=":
                    return 9;
                case ">":
                    return 10;
                case ">=":
                    return 11;
                case "!=":
                    return 12;
                case "!":
                    return 13;
                case "&":
                    return 14;
                case "|":
                    return 15;
                case ":=":
                    return 16;
                case ",":
                    return 17;
                case ";":
                    return 18;
                case "(":
                    return 19;
                case ")":
                    return 20;
                case "[":
                    return 21;
                case "]":
                    return 22;
                case "{":
                    return 23;
                case "}":
                    return 24;
                case "bool":
                    return 25;
                case "char":
                    return 26;
                case "int":
                    return 27;
                case "forward":
                    return 28;
                case "while":
                    return 29;
                case "if":
                    return 30;
                case "else":
                    return 31;
                case "return":
                    return 32;
                case "read":
                    return 33;
                case "write":
                    return 34;
                case "->":
                    return 40;
                default:
                    break;

            }
            if (value.ToLower() == "true" || value.ToLower() == "false")
            {
                return 35;
            }
            else if (value[0] == '\'' && value[value.Length - 1] == '\'' && value.Length == 3)
            {
                return 36;
            }
            else if (int.TryParse(value, out temp))
            {
                return 37;
            }
            else if (value[0] == '"' && value[value.Length - 1] == '"')
            {
                return 38;
            }
            else if (provider.IsValidIdentifier(value))
            {
                return 39;
            }
            return -1;
        }
    }
}
