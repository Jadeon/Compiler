using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Compilerv1
{
    class Rule
    {
        public Rule(Token head, List<Token> body)
        {
            Head = head;
            Body = body;
        }

        public Rule(String head, List<string> body)
        {
            //fill in with token getter thing
        }

        public Token Head;
        public List<Token> Body;
        
    }

    class Quad
    {
        public Quad(string s1, string s2, string s3, string s4)
        {
            arg1 = s1;
            arg2 = s2;
            arg3 = s3;
            arg4 = s4;
        }

        public Quad(string args)
        {
            string[] temp = args.Split(',');

            arg1 = temp[0];
            arg2 = temp[1];
            arg3 = temp[2];
            arg4 = temp[3];
        }


        string arg1;
        string arg2;
        string arg3;
        string arg4;
    }

    class Symbol_Entry
    {
        public int line_num;
        public string value = "";
        public int type;
        public string entry_type = "";
        public string scope = "";
        public int offset;
        public bool forward;
        public int arr_size;
        public int quadNum = -1;



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

    class Token
    {
        public Token()
        {
        }
        public Token(string value, int type, int line, int col)
        {
            this.type = type;
            this.value = value;
            this.line = line;
            this.col = col;
        }

        public Token(string value, int line, int col)
        {
            this.value = value;
            type = classify(value);
            this.line = line;
            this.col = col;
            #region switch statement
            /*if (value == ",")
            {
                this.value = value;
                type = "comma";
            }
            else if (value == ";")
            {
                this.value = value;
                type = "semicolon";
            }
            else if (value == "")
            {
                this.value = value;
                type = "space";
            }
            else if (value == ":=")
            {
                this.value = value;
                type = "assignment";
            }
            else if (value == "->")
            {
                this.value = value;
                type = "arrow";
            }
            else if (arithmetics.Contains(value))
            {
                this.value = value;
                type = "arithmetic";
            }
            else if (relations.Contains(value))
            {
                this.value = value;
                type = "relation";
            }
            else if (squiggles.Contains(value))
            {
                this.value = value;
                type = "brace";
            }
            else if (sqbrack.Contains(value))
            {
                this.value = value;
                type = "bracket";
            }
            else if (paren.Contains(value))
            {
                this.value = value;
                type = "parenthesis";
            }
            else if (connectives.Contains(value))
            {
                this.value = value;
                type = "boolean connective";
            }
            else
            {
                this.value = value;
                type = "id";
            } */
            #endregion

        }

        private String[] arithmetics = { "+", "-", "/", "*", "^", "%" };
        private String[] relations = { "<", ">", "<=", ">=", "=", "!=" };
        private String[] squiggles = { "{", "}" };
        private String[] sqbrack = { "[", "]" };
        private String[] paren = { "(", ")" };
        private String[] connectives = { "&", "|" };
        public int type;
        public string value;
        public int line;
        public int col;

        /// <summary>
        /// This function will classify the type of token a given value string will be.
        /// The mapping will be as follows
        /// 1 *
        /// 2 +
        /// 3 -
        /// 4 /
        /// 5 %
        /// 6 ^
        /// 7 <=
        /// 8 <
        /// 9 =
        /// 10 >
        /// 11 >=
        /// 12 !=
        /// 13 !
        /// 14 &
        /// 15 |
        /// 16 :=
        /// 17 ,
        /// 18 ;
        /// 19 (
        /// 20 )
        /// 21 [
        /// 22 ]
        /// 23 {
        /// 24 }
        /// 25 bool
        /// 26 char
        /// 27 int
        /// 28 forward
        /// 29 while
        /// 30 if
        /// 31 else
        /// 32 return
        /// 33 read
        /// 34 write
        /// 35 <<bool>>
        /// 36 <<char>>
        /// 37 <<int>>
        /// 38 <<string>>
        /// 39 <<func_id>>
        /// 40 <<var_id>>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
                return 41;
            }
            return -1;
        }
    }


    class Program
    {
        static List<List<String>> selectionset = getSS();

        static List<Symbol_Entry> symbolTable = new List<Symbol_Entry>();
        static List<string> resultStack = new List<string>();
        static List<Quad> quadList = new List<Quad>();

        static int tempGenGlobal = 1;

        static string popResult()
        {
            string retval = resultStack[resultStack.Count - 1];

            resultStack.RemoveAt(resultStack.Count - 1);

            return retval;
        }

        static void pushResult(string S)
        {
            resultStack.Add(S);
        }

        static string tempVariable()
        {
            string retval = "T" + tempGenGlobal.ToString();
            tempGenGlobal++;

            return retval;  
        }


        static void Main(string[] args)
        {
            string P = "ironically uninitialized";
            string[] PL;
            List<String> D;
            string filename = "none";
            Console.WriteLine("Please enter the path of the program to be compiled.");
            filename = Console.ReadLine();
            String[] tDelims = { " ", "\n", "\r", "\t" };

            List<Token> Tokens = new List<Token>();
            
            if (filename == "none")
            {
                P = File.ReadAllText(@"../../prog.txt");
                PL = File.ReadAllLines(@"../../prog.txt");
            }
            else
            {
                P = File.ReadAllText(filename);
                PL = File.ReadAllLines(filename);
            }
            D = File.ReadAllLines(@"../../delims.txt").ToList<String>();
            //D.Add(" ");
            //D.Add("\n");
            //D.Add("\r");
            //D.Add("\t");

            //Tokens = Tokenize(P, D);

            Tokens = TokenizeList(PL, D, tDelims.ToList<String>());
            symbolTable = symbolize(Tokens);
            Boolean correct = Parse(Tokens);
            try
            {
                
                
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                Console.WriteLine("Program has finished");
                Console.WriteLine(P);
                Console.WriteLine("Tokenized list:");
                foreach (Token T in Tokens)
                {
                    Console.Write("\tvalue: " + T.value + "\ttype:" + T.type+"\n");
                }
            }

            Console.ReadLine();
        }

        public static List<Token> TokenizeList(string[] Program, List<String> kDelimiters, List<String> tDelimiters)
        {
            List<Token> retval = new List<Token>();
            List<String> allDelims = new List<String>(kDelimiters.Union(tDelimiters));
            int line = 1;
            int nextchar = 0;
            string builder = "";
            char current;
            char next;
            foreach (string Line in Program)
            {
                int col = 0;
                while(col <= Line.Length - 1)
                {
                    current = Line[col];
                    //keepable delimiter
                    if (allDelims.Contains(current.ToString()))
                    {
                        if (current == '"')
                        {
                            if (builder != "")
                            {
                                retval.Add(new Token(builder, line, col));
                            }
                            nextchar = Line.IndexOf('"', col + 1);
                            builder = Line.Substring(col, nextchar - col) + "\"";
                            retval.Add(new Token(builder, line, col));
                            builder = "";
                            col = nextchar + 1;
                        }
                        else if (current == '(')
                        {
                            if (builder != "")
                            {
                                if (builder.ToLower() == "if" || builder.ToLower() == "while" || builder.ToLower() == "read" || builder.ToLower() == "write" || builder.ToLower() == "return")
                                {
                                    retval.Add(new Token(builder, line, col));
                                    builder = ""; 
                                }
                                else
                                {
                                    retval.Add(new Token(builder, 39, line, col));
                                    builder = ""; 
                                }
                            }
                            retval.Add(new Token("(", line, col));
                            col++;
                        }
                        else if (tDelimiters.Contains(current.ToString()))
                        {
                            if (builder != "")
                            {
                                retval.Add(new Token(builder, line, col));
                                builder = "";
                            }
                            col++;
                        }
                        //size two delimiter
                        else if (col < Line.Length - 1)
                        {
                            next = Line[col + 1];
                            if (kDelimiters.Contains(next.ToString()) && (next == '>' || next == '=' || next == '>') && current != '<')
                            {
                                if (builder != "")
                                {
                                    retval.Add(new Token(builder, line, col));
                                    builder = "";
                                }
                                retval.Add(new Token(current.ToString() + next.ToString(), line, col));
                                col += 2;
                            }
                            else
                            {
                                if (builder != "")
                                {
                                    retval.Add(new Token(builder, line, col));
                                    builder = "";
                                }
                                retval.Add(new Token(current.ToString(), line, col));
                                col++;
                            }
                        }
                        else
                        {
                            if (builder != "")
                            {
                                retval.Add(new Token(builder, line, col));
                                builder = "";
                            }
                            retval.Add(new Token(current.ToString(), line, col));
                            builder = "";
                            col++;
                        }
                    }
                    //string thing
                    else
                    {
                        builder += current.ToString();
                        col++;
                    }
                }
                line++;
            }

            retval.Add(new Token("$", line, 1));
            return retval;
        }

        #region Parsing!

        public static Boolean Parse(List<Token> Tokens)
        {
            if (Func(Tokens))
            {
                if (Func_list(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Func_list(List<Token> Tokens)
        {
            if (Func(Tokens))
            {
                if (Func_list(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 3))
            {
                return true;
            }

            return false;
        }

        public static Boolean Func(List<Token> Tokens)
        {
            if (funcID(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Vars(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            if (arrow(Tokens))
                            {
                                if (lessStrict(Tokens))
                                {
                                    if (Return_list(Tokens))
                                    {
                                        if (greaterStrict(Tokens))
                                        {
                                            if (leftBrace(Tokens))
                                            {
                                                if (Var_decs(Tokens))
                                                {
                                                    while (resultStack.Count > 0)
                                                    {
                                                        popResult();
                                                    }
                                                    if (Block(Tokens))
                                                    {
                                                        if (rightBrace(Tokens))
                                                        {
                                                            return true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static Boolean Vars(List<Token> Tokens)
        {
            if (Var_dec(Tokens))
            {
                if (Vars_tail(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 7))
            {
                return true;
            }

            return false;
        }

        public static Boolean Var_dec(List<Token> Tokens)
        {
            if (Type(Tokens))
            {
                if (varID(Tokens))
                {
                    if (Array_dec(Tokens))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static Boolean Vars_tail(List<Token> Tokens)
        {
            if (comma(Tokens))
            {
                if (Var_dec(Tokens))
                {
                    if (Vars_tail(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 10))
            {
                return true;
            }

            return false;
        }

        public static Boolean Var_decs(List<Token> Tokens)
        {
            if (Vars(Tokens))
            {
                if (semic(Tokens))
                {

                    return true; 
                }
            }

            else if (inSS(selectionset, Tokens[0], 12))
            {
                return true;
            }

            return false;
        }

        public static Boolean Array_dec(List<Token> Tokens)
        {
            if (leftBracket(Tokens))
            {
                if (baseInt(Tokens))
                {
                    if (rightBracket(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 14))
            {
                return true;
            }

            return false;
        }

        public static Boolean Return_list(List<Token> Tokens)
        {
            if (Type(Tokens))
            {
                if (Return_tail(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 16))
            {
                return true;
            }

            return false;
        }

        public static Boolean Return_tail(List<Token> Tokens)
        {
            if (comma(Tokens))
            {
                if (Type(Tokens))
                {
                    if (Return_tail(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 18))
            {
                return true;
            }

            return false;
        }

        public static Boolean Type(List<Token> Tokens)
        {
            return (intKW(Tokens) || charKW(Tokens) || boolKW(Tokens));
        }

        public static Boolean Block(List<Token> Tokens)
        {
            if (Stmt(Tokens))
            {
                if (Block(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 23))
            {
                return true;
            }

            return false;
        }

        public static Boolean Stmt(List<Token> Tokens)
        {
            int getSize = resultStack.Count;
            if (Var_list(Tokens))
            {
                int numArgs = resultStack.Count - getSize;
                List<string> varNames = new List<string>();
                for (int i = 0; i < numArgs; i++)
                {
                    varNames.Add(popResult());
                }
                if (assign(Tokens))
                {

                    if (Arg_list(Tokens))
                    {
                        if (semic(Tokens))
                        {
                            varNames.Reverse();
                            foreach (string var in varNames)
                            {
                                genQuad("assign," + lookup(var) + ",," + lookup(popResult()));
                            }
                            return true;
                        }
                    }
                }
            }

            else if (ifKW(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Expr(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            if (leftBrace(Tokens))
                            {
                                if (Block(Tokens))
                                {
                                    if (rightBrace(Tokens))
                                    {
                                        if (Else_tail(Tokens))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else if (whileKW(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Expr(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            if (leftBrace(Tokens))
                            {
                                if (Block(Tokens))
                                {
                                    if (rightBrace(Tokens))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else if (writeKW(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Write_list(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            if (semic(Tokens))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            else if (readKW(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Var_list(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            if (semic(Tokens))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            else if (returnKW(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Arg_list(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            if (semic(Tokens))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static Boolean Var_list(List<Token> Tokens)
        {
            if (Var_ref(Tokens))
            {
                if (Var_tail(Tokens))
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean Var_tail(List<Token> Tokens)
        {
            if (comma(Tokens))
            {
                if (Var_list(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 33))
            {
                return true;
            }

            return false;
        }

        public static Boolean Var_ref(List<Token> Tokens)
        {
            if (varID(Tokens))
            {
                if (Array_ref(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Array_ref(List<Token> Tokens)
        {
            if (leftBracket(Tokens))
            {
                if (Expr(Tokens))
                {
                    if (rightBracket(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 36))
            {
                return true;
            }

            return false;
        }

        public static Boolean Else_tail(List<Token> Tokens)
        {
            if (elseKW(Tokens))
            {
                if (leftBrace(Tokens))
                {
                    if (Block(Tokens))
                    {
                        if (rightBrace(Tokens))
                        {
                            return true;
                        }
                    }
                }
            }

            if (inSS(selectionset, Tokens[0], 38))
            {
                return true;
            }

            return false;
        }

        public static Boolean Arg_list(List<Token> Tokens)
        {
            if (Arg(Tokens))
            {
                if (Arg_tail(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Arg(List<Token> Tokens)
        {
            if (baseChar(Tokens))
            {
                return true;
            }

            else if (Expr(Tokens))
            {
                return true;
            }

            return false;
        }

        public static Boolean Arg_tail(List<Token> Tokens)
        {
            if (comma(Tokens))
            {
                if (Arg_list(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 43))
            {
                return true;
            }

            return false;
        }

        public static Boolean Write_list(List<Token> Tokens)
        {
            if (Write_item(Tokens))
            {
                if (Write_tail(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Write_item(List<Token> Tokens)
        {
            if (baseChar(Tokens))
            {
                return true;
            }

            else if (baseString(Tokens))
            {
                return true;
            }

            else if (Expr(Tokens))
            {
                return true;
            }

            return false;
        }

        public static Boolean Write_tail(List<Token> Tokens)
        {
            if (comma(Tokens))
            {
                if (Write_list(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 49))
            {
                return true;
            }

            return false;
        }

        public static Boolean Expr(List<Token> Tokens)
        {
            if (B_expr(Tokens))
            {
                if (Expr_1(Tokens))
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean Expr_1(List<Token> Tokens)
        {
            if (logiOr(Tokens))
            {
                if (B_expr(Tokens))
                {
                    if (Expr_1(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (logiAnd(Tokens))
            {
                if (B_expr(Tokens))
                {
                    if (Expr_1(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 53))
            {
                return true;
            }

            return false;
        }

        public static Boolean B_expr(List<Token> Tokens)
        {
            if (N_expr(Tokens))
            {
                if (B_expr_1(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean B_expr_1(List<Token> Tokens)
        {
            if (lessStrict(Tokens))
            {
                if (N_expr(Tokens))
                {
                    return true;
                }
            }

            else if (greaterStrict(Tokens))
            {
                if (N_expr(Tokens))
                {
                    return true;
                }
            }

            else if (greaterEq(Tokens))
            {
                if (N_expr(Tokens))
                {
                    return true;
                }
            }

            else if (lessEq(Tokens))
            {
                if (N_expr(Tokens))
                {
                    return true;
                }
            }

            else if (equal(Tokens))
            {
                if (N_expr(Tokens))
                {
                    return true;
                }
            }

            else if (notEq(Tokens))
            {
                if (N_expr(Tokens))
                {
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 61))
            {
                return true;
            }

            return false;
        }

        public static Boolean N_expr(List<Token> Tokens)
        {
            if (Term(Tokens))
            {
                if (N_expr_1(Tokens))
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean N_expr_1(List<Token> Tokens)
        {

            if (add(Tokens))
            {
                string tempgen = "add," + lookup(popResult()) + ",";
                if (Term(Tokens))
                {
                    if (N_expr_1(Tokens))
                    {
                        string tempVar = tempVariable();
                        tempgen += lookup(popResult()) + "," + tempVar;
                        pushResult(tempVar);
                        genQuad(tempgen);
                        return true;
                    }
                }
            }

            else if (minus(Tokens))
            {
                string tempgen = "minus," + lookup(popResult()) + ",";
                if (Term(Tokens))
                {
                    if (N_expr_1(Tokens))
                    {
                        string tempVar = tempVariable();
                        tempgen += lookup(popResult()) + "," + tempVar;
                        pushResult(tempVar);
                        genQuad(tempgen);
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 65))
            {
                return true;
            }

            return false;
        }

        public static Boolean Term(List<Token> Tokens)
        {
            if (Factor(Tokens))
            {
                if (Term_1(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Term_1(List<Token> Tokens)
        {
            if (mult(Tokens))
            {
                string tempgen = "mult," + lookup(popResult()) + ",";
                if (Factor(Tokens))
                {
                    if (Term_1(Tokens))
                    {
                        string tempVar = tempVariable();
                        tempgen += lookup(popResult()) + "," + tempVar;
                        pushResult(tempVar);
                        genQuad(tempgen);
                        return true;
                    }
                }
            }

            else if (div(Tokens))
            {
                string tempgen = "div," + lookup(popResult()) + ",";
                if (Factor(Tokens))
                {
                    if (Term_1(Tokens))
                    {
                        string tempVar = tempVariable();
                        tempgen += lookup(popResult()) + "," + tempVar;
                        pushResult(tempVar);
                        genQuad(tempgen);
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 69))
            {
                return true;
            }

            return false;
        }

        public static Boolean Factor(List<Token> Tokens)
        {
            if (Sub_factor(Tokens))
            {
                if (Factor_1(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Factor_1(List<Token> Tokens)
        {
            if (mod(Tokens))
            {
                string tempgen = "mod," + lookup(popResult()) + ",";
                if (Sub_factor_1(Tokens))
                {
                    if (Factor_1(Tokens))
                    {
                        string tempVar = tempVariable();
                        tempgen += lookup(popResult()) + "," + tempVar;
                        pushResult(tempVar);
                        genQuad(tempgen);
                        return true;
                    }
                }
            }

            else if (inSS(selectionset, Tokens[0], 72))
            {
                return true;
            }

            return false;
        }

        public static Boolean Sub_factor(List<Token> Tokens)
        {
            if (Base(Tokens))
            {
                if (Sub_factor_1(Tokens))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean Sub_factor_1(List<Token> Tokens)
        {
            if (exp(Tokens))
            {
                string tempgen = "exp," + lookup(popResult()) + ",";
                if (Sub_factor(Tokens))
                {
                    string tempVar = tempVariable();
                    tempgen += lookup(popResult()) + "," + tempVar;
                    pushResult(tempVar);
                    genQuad(tempgen);
                    return true;
                }
            }

            else if (inSS(selectionset, Tokens[0], 75))
            {
                return true;
            }

            return false;
        }

        public static Boolean Base(List<Token> Tokens)
        {
            if (baseInt(Tokens))
            {
                return true;
            }

            else if (Var_ref(Tokens))
            {
                return true;
            }

            else if (baseBool(Tokens))
            {
                return true;
            }
            else if (logiNeg(Tokens))
            {

                string tempgen = "negate, ,";
                if (Base(Tokens))
                {
                    string tempVar = tempVariable();
                    tempgen += popResult() + "," + tempVar;
                    pushResult(tempVar);
                    genQuad(tempgen);
                    return true;
                }
            }
            else if (leftParen(Tokens))
            {
                if (Expr(Tokens))
                {
                    if (rightParen(Tokens))
                    {
                        return true;
                    }
                }
            }
            else if (Function_call(Tokens))
            {
                return true;
            }

            return false;
        }

        public static Boolean Function_call(List<Token> Tokens)
        {
            if (varID(Tokens))
            {
                if (leftParen(Tokens))
                {
                    if (Call_list(Tokens))
                    {
                        if (rightParen(Tokens))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static Boolean Call_list(List<Token> Tokens)
        {
            if (Arg_list(Tokens))
            {
                return true;
            }

            else if (inSS(selectionset, Tokens[0], 84))
            {
                return true;
            }

            return false;
        }



        #region Arithmetic Ops
        public static Boolean mult(List<Token> Tokens)
        {
            if (Tokens[0].type == 1)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean add(List<Token> Tokens)
        {
            if (Tokens[0].type == 2)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean minus(List<Token> Tokens)
        {
            if (Tokens[0].type == 3)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean div(List<Token> Tokens)
        {
            if (Tokens[0].type == 4)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean mod(List<Token> Tokens)
        {
            if (Tokens[0].type == 5)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean exp(List<Token> Tokens)
        {
            if (Tokens[0].type == 6)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion

        #region Relations
        public static Boolean lessEq(List<Token> Tokens)
        {
            if (Tokens[0].type == 7)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean lessStrict(List<Token> Tokens)
        {
            if (Tokens[0].type == 8)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean equal(List<Token> Tokens)
        {
            if (Tokens[0].type == 9)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean greaterStrict(List<Token> Tokens)
        {
            if (Tokens[0].type == 10)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean greaterEq(List<Token> Tokens)
        {
            if (Tokens[0].type == 11)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean notEq(List<Token> Tokens)
        {
            if (Tokens[0].type == 12)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion

        #region Logical Connectives
        public static Boolean logiNeg(List<Token> Tokens)
        {
            if (Tokens[0].type == 13)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        public static Boolean logiAnd(List<Token> Tokens)
        {
            if (Tokens[0].type == 14)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        public static Boolean logiOr(List<Token> Tokens)
        {
            if (Tokens[0].type == 15)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion

        #region :=/,/;/->
        public static Boolean assign(List<Token> Tokens)
        {
            if (Tokens[0].type == 16)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        public static Boolean arrow(List<Token> Tokens)
        {
            if (Tokens[0].type == 40)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        public static Boolean comma(List<Token> Tokens)
        {
            if (Tokens[0].type == 17)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        public static Boolean semic(List<Token> Tokens)
        {
            if (Tokens[0].type == 18)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion

        #region Parenthesis/Brackets/Braces
        public static Boolean leftParen(List<Token> Tokens)
        {
            if (Tokens[0].type == 19)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean rightParen(List<Token> Tokens)
        {
            if (Tokens[0].type == 20)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean leftBracket(List<Token> Tokens)
        {
            if (Tokens[0].type == 21)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean rightBracket(List<Token> Tokens)
        {
            if (Tokens[0].type == 22)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean leftBrace(List<Token> Tokens)
        {
            if (Tokens[0].type == 23)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean rightBrace(List<Token> Tokens)
        {
            if (Tokens[0].type == 24)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion

        #region Keywords
        public static Boolean boolKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 25)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean charKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 26)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean intKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 27)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean forwardKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 28)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean whileKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 29)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean ifKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 30)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean elseKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 31)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean returnKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 32)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean readKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 33)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean writeKW(List<Token> Tokens)
        {
            if (Tokens[0].type == 34)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion

        #region Base Types
        public static Boolean baseInt(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 37)
            {
                popList(Tokens);
                resultStack.Add(current.value);
                return true;
            }
            return false;
        }

        public static Boolean baseBool(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 35)
            {
                popList(Tokens);
                resultStack.Add(current.value);
                return true;
            }
            return false;
        }

        public static Boolean baseChar(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 36)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean baseString(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 38)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean funcID(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 39)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }

        public static Boolean varID(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 41)
            {
                pushResult(current.value);
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion 
        #endregion

        #region Symbolize

        public static string lookup(string ID)
        {
            foreach (Symbol_Entry SE in symbolTable)
            {
                if (SE.value == ID)
                {
                    return symbolTable.IndexOf(SE).ToString();
                }
            }
            return "error in symbol lookup";
        }

        public static bool isEntered(List<Symbol_Entry> current, Symbol_Entry newSE)
        {

            foreach (Symbol_Entry SE in current)
            {
                if (SE.value == newSE.value && SE.scope == newSE.scope)
                {
                    return false;
                }
            }

            return true;
        }

        public static List<Symbol_Entry> symbolize(List<Token> tokenList)
        {
            Token nextToken = new Token();
            Token currentToken = new Token();

            int arrSize = -1;
            String scopeFlag = "Error in Scopification";
            String Entry_Type = "";

            List<Symbol_Entry> Symbol_Table = new List<Symbol_Entry>();
            for (int i = 0; i < tokenList.Count(); i++)
            {
                if (i < tokenList.Count() - 1)
                {
                    nextToken = tokenList[i + 1];

                }
                currentToken = tokenList[i];
                Entry_Type = get_EntryType(currentToken.value, nextToken);
                if (new[] { 35, 36, 37, 38 }.Contains(currentToken.type))
                {
                    //Constant                    
                    Symbol_Entry se = new Symbol_Entry(currentToken.line, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                    if(isEntered(Symbol_Table,se))
                    {
                        Symbol_Table.Add(se);
                    }
                }
                else if (currentToken.type == 39)
                {
                    //Func_ID
                    Symbol_Entry se = new Symbol_Entry(currentToken.line, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                    if(isEntered(Symbol_Table,se))
                    {
                        Symbol_Table.Add(se);
                    }
                    scopeFlag = tokenList[i].value;
                }
                else if (currentToken.type == 41)
                {
                    if (i > 0)
                    {
                        if (tokenList[i - 1].value == "int")
                        {
                            Entry_Type = "int";
                        }

                        else if (tokenList[i - 1].value == "bool")
                        {
                            Entry_Type = "bool";
                        }

                        else if (tokenList[i - 1].value == "char")
                        {
                            Entry_Type = "char";
                        }

                        else if (tokenList[i - 1].value == "string")
                        {
                            Entry_Type = "string";
                        }

                        else
                        {
                            Entry_Type = "var";
                        }
                    }
                    //Var_ID
                    if (i <= tokenList.Count() - 2)
                    {
                        if (int.TryParse(tokenList[i + 2].value, out arrSize))
                        {
                            Symbol_Entry se = new Symbol_Entry(currentToken.line, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, arrSize);
                            if(isEntered(Symbol_Table,se))
                            {
                                Symbol_Table.Add(se);
                            }
                        }else
                        {
                            Symbol_Entry se = new Symbol_Entry(currentToken.line, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                            if(isEntered(Symbol_Table,se))
                            {
                                Symbol_Table.Add(se);
                            }
                        }
                    }else
                    {
                        Symbol_Entry se = new Symbol_Entry(currentToken.line, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                        if(isEntered(Symbol_Table,se))
                        {
                            Symbol_Table.Add(se);
                        }
                    }


                }else if
            }
            return Symbol_Table;
        }

        public static String get_EntryType(String tokenValue, Token nextToken)
        {
            int temp;
            String retVal = "";
            if (new[] { "false", "true" }.Contains(tokenValue.ToLower()))
            {
                retVal = "Bool";
            }
            else if (int.TryParse(tokenValue, out temp))
            {
                retVal = "Int";
            }
            else if (tokenValue[0] == '\'')
            {
                retVal = "Char";
            }
            else if (tokenValue[0] == '"')
            {
                retVal = "String";
            }
            else if (nextToken.value == "[")
            {
                retVal = "IntArray";
            }
            else
            {
                retVal = "Error in identifying Entry_Type";
            }
            return retVal;
        }

        public static int offset(string Entry_Type, int arraySize)
        {
            int offset = 0;

            if (Entry_Type == "String")
            {
                offset = 4 * arraySize;
            }
            else if (Entry_Type == "Char")
            {
                offset = 2 * arraySize;
            }
            else if (Entry_Type == "Bool")
            {
                offset = 2 * arraySize;
            }
            else if (Entry_Type == "Int")
            {
                offset = 6 * arraySize;
            }
            else if (Entry_Type == "IntArray")
            {
                offset = 12 * arraySize;
            }
            else
            {
                offset = 0;
            }

            return offset;
        }
        #endregion

        public static void popList(List<Token> Tokens)
        {
            Tokens.RemoveAt(0);
        }

        public static Boolean inSS(List<List<String>> SS, Token T, int rulenum)
        {

            return SS[rulenum - 1].Contains(T.value);
        }

        public static List<List<String>> getSS()
        {
            List<List<String>> retval = new List<List<string>>();
            string[] temp = { };
            List<string> temp2 = new List<string>();
            List<String> lines = File.ReadAllLines(@"../../selectionSetsList.txt").ToList<String>();
            foreach (string S in lines)
            {
                temp = S.Split(',');
                foreach (string s1 in temp)
                {
                    temp2.Add(s1.Replace("comma", ","));
                }
                retval.Add(temp2);
                temp2 = new List<string>();
            }

            return retval;
        }

        public static void genQuad(string S)
        {
            quadList.Add(new Quad(S));            
        }
    }
}