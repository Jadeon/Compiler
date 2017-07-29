using System
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace tokenizer
{
    class Program
    {
        static List<List<string>> selectionset = getSS();
        static List<Symbol_Entry> SymbolTable = new List<Symbol_Entry>();
        static void Main(string[] args)
        {     
            string[] programLines = {};
            List<Token> tokens = new List<Token>();
            string file = readFile();
            try
            {
                programLines = File.ReadAllLines(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            List<string> keep_delim = new List<string>();
            List<string> toss_delim = new List<string>();
            keep_delim = File.ReadAllLines(@"..\..\k_delimiter.txt").ToList();
            toss_delim = File.ReadAllLines(@"..\..\ta_delimiter.txt").ToList();
            tokens = tokenize(programLines, keep_delim, toss_delim);
            Boolean baseResult = Base(tokens);
            foreach (Token x in tokens) {
                Console.WriteLine("Token: {0,-10}Type: {1,-10}Row: {2,-10}Index: {3,-10}",x.value,x.type,x.row,x.index);
            }
            Console.WriteLine(baseResult);
            Console.ReadLine();            
        }

        #region Read File
        public static string readFile()
        {
            string file = "";
            string readLine = "";

            Console.WriteLine("Enter the full path to the program you would like to tokenize:");
            Console.WriteLine("(Press Enter to use default file)");

            readLine = Console.ReadLine();
            file = String.IsNullOrWhiteSpace(readLine) ? @"..\..\..\prog.txt" : readLine;

            if (!File.Exists(file))
            {
                Console.WriteLine("File Not Found: " + file);
                Console.WriteLine("Press any key...");
                Console.ReadKey();
                Console.Clear();
                file = readFile();  
                
            }

            return file;
        }
        #endregion

        #region Tokenize
        public static List<Token> tokenize(string[] program, List<string> keep_delim, List<string> toss_delim)
        {
            List<Token> tokenList = new List<Token>();
            List<string> master_delim = new List<string>(keep_delim.Union(toss_delim));
            string stringBuilder = "";            
            int row = 0;
            #region Iterate-through-Array
            foreach (string Line in program)
            {
                row++;
                int index = 0;
                #region Iterate-through-Line
                while (index < Line.Length)
                {
                    string activeToken = Line[index].ToString();
                    string nextToken = "";                
         

                    #region Delimiter-Found
                    if (master_delim.Contains(activeToken))
                    {
                        #region Empty String Builder
                        if (stringBuilder != "")
                        {
                            
                            tokenList.Add(new Token(stringBuilder, 000, index, row));
                            stringBuilder = "";
                        }
                        #endregion

                        #region Special(")-Delimiter
                        if (activeToken == "\"")
                        {                            
                            int searchResult = Line.IndexOf('"', index + 1);
                            stringBuilder = Line.Substring(index, searchResult - index) + "\"";
                            tokenList.Add(new Token(stringBuilder, 000, index, row));
                            stringBuilder = "";
                            index = searchResult + 1;
                        }
                        #endregion   

                        #region ThrowAway-Delimiter
                        else if (toss_delim.Contains(activeToken))
                        {
                            stringBuilder = "";
                            index++;
                        }
                        #endregion

                        #region Double-Delimiter
                        else if (index < Line.Length - 1)
                        {
                            nextToken = Line[index + 1].ToString();
                            if (keep_delim.Contains(nextToken) && (nextToken == ">" || nextToken == "=") && activeToken != "<")
                            {
                                tokenList.Add(new Token(activeToken + nextToken, 000, index, row));
                                index += 2;
                            }

                            #region Standard-Delimiter
                            else
                            {
                                tokenList.Add(new Token(activeToken, 000, index, row));
                                index++;
                            }
                            #endregion
                        }
                        #region Standard-Delimiter
                        else
                        {
                            tokenList.Add(new Token(activeToken, 000, index, row));
                            index++;
                        }
                        #endregion
                        #endregion
                    }
                    #endregion

                    #region StringBuilder
                    else {
                        stringBuilder += activeToken.ToString();
                        index++;
                    }
                    #endregion
                }
                #endregion
            }
            return tokenList;
            #endregion
        }
        #endregion

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
            if (baseID(Tokens))
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
                if (baseID(Tokens))
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
            if (Var_list(Tokens))
            {
                if (assign(Tokens))
                {
                    if (Arg_list(Tokens))
                    {
                        if (semic(Tokens))
                        {
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
            if (baseID(Tokens))
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
                if (Term(Tokens))
                {
                    if (N_expr_1(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (minus(Tokens))
            {
                if (Term(Tokens))
                {
                    if (N_expr_1(Tokens))
                    {
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
                if (Factor(Tokens))
                {
                    if (Term_1(Tokens))
                    {
                        return true;
                    }
                }
            }

            else if (div(Tokens))
            {
                if (Factor(Tokens))
                {
                    if (Term_1(Tokens))
                    {
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
                if (Sub_factor_1(Tokens))
                {
                    if (Factor_1(Tokens))
                    {
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
                if (Sub_factor(Tokens))
                {
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
                if (Base(Tokens))
                {
                    return true;
                }
            }
            else if (leftParen(Tokens))
            {
                if (Expr(Tokens))
                {
                    return true;
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
            if (baseID(Tokens))
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

        public static Boolean baseID(List<Token> Tokens)
        {
            Token current = Tokens[0];
            if (current.type == 39)
            {
                popList(Tokens);
                return true;
            }
            return false;
        }
        #endregion
        #endregion

        #region Pop
        public static void popList(List<Token> Tokens)
        {
            Tokens.RemoveAt(0);
        }
        #endregion
    
        #region SelectionSet Logic
        public static Boolean inSS(List<List<String>> SS, Token T, int rulenum)
        {

            return SS[rulenum - 1].Contains(T.value);
        }
        
        public static List<List<String>> getSS()
        {
            List<List<String>> retval = new List<List<string>>();
            string[] temp = { };
            List<string> temp2 = new List<string>();
            List<String> lines = File.ReadAllLines(@"..\..\..\selectionSetsList.txt").ToList<String>();
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
        #endregion

        #region Symbolize
        public static List<Symbol_Entry> symbolize(List<Token> tokenList)
        {
            Token nextToken = new Token();
            Token currentToken = new Token();

            int arrSize     = -1;
            String scopeFlag = "Error in Scopification";
            String Entry_Type = "";

            List<Symbol_Entry> Symbol_Table = new List<Symbol_Entry>();
            for (int i = 0; i < tokenList.Count(); i++)
            {
                if (i <= tokenList.Count()-1)
                {
                    nextToken = tokenList[i + 1];

                }
                currentToken = tokenList[i];
                Entry_Type = get_EntryType(currentToken.value, nextToken);
                if (new[] { 35, 36, 37, 38 }.Contains(currentToken.type))
                {
                    //Constant                    
                    Symbol_Entry se = new Symbol_Entry(currentToken.row, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                    Symbol_Table.Add(se);
                }
                else if (currentToken.type == 39)
                {
                    //Func_ID
                    Symbol_Entry se = new Symbol_Entry(currentToken.row, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                    Symbol_Table.Add(se);
                    scopeFlag = tokenList[i].value;
                }
                else if (currentToken.type == 41)
                {
                    //Var_ID
                    if(i <= tokenList.Count() - 2)
                    {
                        if (int.TryParse(tokenList[i+2].value, out arrSize))
                        {
                            Symbol_Entry se = new Symbol_Entry(currentToken.row, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, arrSize);
                            Symbol_Table.Add(se);
                        }
                    }else
                    {
                        Symbol_Entry se = new Symbol_Entry(currentToken.row, currentToken.value, currentToken.type, Entry_Type, scopeFlag, offset(Entry_Type, arrSize), false, 1);
                        Symbol_Table.Add(se);
                    }
                   
                                  
                }
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
            }else if (nextToken.value == "[")
            {
                retVal = "IntArray";
            }else
            {
                retVal = "Error in identifying Entry_Type";
            }
            return retVal;
        }

        public static int offset(string Entry_Type, int arraySize)
        {
            int offset =0;

            if(Entry_Type == "String")
            {
               offset = 4*arraySize;
            }else if(Entry_Type == "Char")
            {
                offset=2*arraySize;
            }else if(Entry_Type == "Bool")
            {
                offset=2*arraySize;
            }else if(Entry_Type == "Int")
            {
                offset=6*arraySize;
            }else if(Entry_Type == "IntArray")
            {
                offset=12*arraySize;
            }else
            {
                offset = 0;
            }

            return offset;
        }
        #endregion

    }
}

