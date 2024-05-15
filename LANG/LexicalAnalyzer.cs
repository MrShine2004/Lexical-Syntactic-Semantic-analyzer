using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static LANG.LexicalAnalyzer;

namespace LANG
{
    internal class LexicalAnalyzer
    {
        private StreamReader reader;
        private string currentLexeme;
        private int lineNumber;
        private int NumberInL;
        private int NumberId;
        private char Buffer;
        private bool OnBuf;
        private bool CommentaryR;
        private bool ReadBuf;

        // Таблица зарезервированных слов
        private Dictionary<string, string> reservedWords = new Dictionary<string, string>
        {
            { "module", "MODULE" },
            { "var", "VAR" },
            { "int", "INT" },
            { "bool", "BOOL" },
            { "float", "FLOAT" },
            { "arr", "ARR" },
            { "begin", "BEGIN" },
            { "end", "END" },
            { "repeat", "REPEAT" },
            { "while", "WHILE" },
            { "if", "IF" },
            { "else", "ELSE" },
            // Добавьте остальные зарезервированные слова
        };



        private static readonly string[] Operators = { "+", "-", "*", "/", "=", "<", ">", "<=", ">=", "==", "!=", "||", "&&", "!" };
        private static readonly char[] Separators = { '(', ')', '{', '}', '[', ']', ';', ',', ':', '-', '+', '/', '*' };
        private static readonly string[] Booleans = { "true", "false" };
        private static readonly string[] ReservedWords = { "true", "false", "+", "-", "*", "/", "=", "|", "&", "<", ">", "<=", ">=", "==", "!=", "||", "&&", "!", "(", ")", "{", "}", "[", "]", ";", ":", ",", ".", "module", "var", "int", "bool", "float", "arr", "begin", "end", "while", "repeat", "if", "else" };
        //private static readonly string[][] Tokens = { { } };
        private List<Error> Errors = new List<Error>();
        private Dictionary<string, int> identifierIndexTable = new Dictionary<string, int>();

        public LexicalAnalyzer(string code)
        {
            reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(code)));
            // Создание таблицы индексов идентификаторов
            NumberId = 0;
            currentLexeme = "";
            lineNumber = 1;
            NumberInL = 0;
            Buffer = '\0';
            OnBuf = false;
            ReadBuf = false;
            CommentaryR = false;
        }

        private char ReadNextChar()
        {

            if (OnBuf && Buffer != '\0' && !ReadBuf)
            {
                char cChar = Buffer;
                NumberInL--;
                Buffer = '\0';
                OnBuf = false;
                ReadBuf = true;
                return cChar;
            }
            else
            {
                int nextChar = reader.Read();
                if ((char)nextChar == '\n')
                {
                    NumberInL = 0;
                }
                else
                {
                    NumberInL++;
                }
                Console.Write((char)nextChar);
                ReadBuf = false;
                return (nextChar == -1) ? '\0' : (char)nextChar;
            }
        }

        private TokenType GetTokenType(string lexeme)
        {
            if (string.IsNullOrEmpty(lexeme))
            {
                return TokenType.Other;
            }

            switch (lexeme)
            {
                case "const":
                    return TokenType.tConst;
                case "else":
                    return TokenType.tElse;
                case "end":
                    return TokenType.tEnd;
                case "functional":
                    return TokenType.tFunc;
                case "repeat":
                    return TokenType.tRepeat;
                case "do":
                    return TokenType.tDo;
                case "if":
                    return TokenType.tIf;
                case "module":
                    return TokenType.tModule;
                case "begin":
                    return TokenType.tBegin;
                case "arr":
                    return TokenType.tArray;
                case "var":
                    return TokenType.tVar;
                case "int":
                    return TokenType.tInt;
                case "float":
                    return TokenType.tFloat;
                case "bool":
                    return TokenType.tBool;
                case "true":
                    return TokenType.tTrue;
                case "false":
                    return TokenType.tFalse;
                case ";":
                    return TokenType.tz;
                case ",":
                    return TokenType.z;
                case "(":
                    return TokenType.sc1;
                case ")":
                    return TokenType.sc2;
                case ":":
                    return TokenType.dt;
                case "=":
                    return TokenType.ravno;
                case "[":
                    return TokenType.kvsc1;
                case "]":
                    return TokenType.kvsc2;
                case "{":
                    return TokenType.figsc1;
                case "}":
                    return TokenType.figsc2;
                case ">":
                    return TokenType.more;
                case "==":
                    return TokenType.eq;
                case "||":
                    return TokenType.Or;
                case "&&":
                    return TokenType.And;
                case "!":
                    return TokenType.Not;
                case "<":
                    return TokenType.less;
                case ">=":
                    return TokenType.moreOrEq;
                case "<=":
                    return TokenType.lessOrEq;
                case "!=":
                    return TokenType.noEq;
                case "-":
                    return TokenType.minus;
                case "+":
                    return TokenType.plus;
                case "/":
                    return TokenType.div;
                case "*":
                    return TokenType.mul;
                default:
                    // Проверяем, является ли lexeme идентификатором
                    if (IsIdentifier(lexeme))
                    {
                        return TokenType.id;
                    }
                    // Проверяем, является ли lexeme числом
                    else if (IsNumber(lexeme) || IsExponentialNumber(lexeme))
                    {
                        return TokenType.ct;
                    }
                    else
                    {
                        return TokenType.Other;
                    }
            }
        }


        //tConst,
        //tElse,
        //tEnd,
        //tFunc,
        //tRepeat,
        //tIf,
        //tModule,
        //tBegin,
        //tArray,
        //tWhile,
        //tVar,
        //tz,
        //z,
        //sc1,
        //sc2,
        //dt,
        //ravno,
        //kvsc1,
        //kvsk2,
        //id,
        //ct,
        //Other

        private bool IsNumber(string lexeme)
        {
            // Метод проверяет, является ли лексема числом
            return int.TryParse(lexeme, out _) || float.TryParse(lexeme, out _);
        }
        static bool IsExponentialNumber(string input)
        {
            string pattern = @"^[+-]?\d+(\.\d+)?([eE][+-]?\d+)?$";
            return Regex.IsMatch(input, pattern);
        }
        private bool IsBoolean(string lexeme)
        {
            bool result;
            return bool.TryParse(lexeme, out result);
        }
        private bool IsInteger(string lexeme)
        {
            int result;
            return int.TryParse(lexeme, out result);
        }
        private bool IsFloat(string lexeme)
        {
            float result;
            return float.TryParse(lexeme, out result);
        }
        private bool IsIdentifier(string lexeme)
        {
            // Проверяем, начинается ли строка с буквы
            if (!char.IsLetter(lexeme[0]))
            {
                return false;
            }

            // Проверяем оставшиеся символы на буквы или цифры
            for (int i = 1; i < lexeme.Length; i++)
            {
                if (!char.IsLetterOrDigit(lexeme[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private void IsCommentary(ref char currentChar, ref string Buf, ref List<Token> tokens)
        {
            if (currentChar == '/')
            {
                Buffer = currentChar;
                currentChar = ReadNextChar();
                OnBuf = true;

                if (currentChar == '*')
                {
                    Buffer = '\0';
                    OnBuf = false;
                    while (!reader.EndOfStream)
                    {
                        currentChar = ReadNextChar();
                        if (currentChar == '\n')
                        {
                            NumberInL = 0;
                            lineNumber++;
                        }
                        if (currentChar == '*')
                        {
                            currentChar = ReadNextChar();
                            if (currentChar == '\n')
                            {
                                NumberInL = 0;
                                lineNumber++;
                            }
                            if (currentChar == '/')
                            {
                                currentChar = ReadNextChar();
                                Buffer = '\0';
                                CommentaryR = true; 
                                OnBuf = false;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    OnBuf = false;
                }
                CommentaryR = false; 
                if (!CommentaryR)
                {
                    string lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                    TokenType tokenType = GetTokenType(lexeme);
                    int ident = -1;
                    if (tokenType == TokenType.id)
                    {
                        if (tokenType == TokenType.id)
                        {
                            // Проверяем, есть ли идентификатор уже в таблице
                            if (identifierIndexTable.ContainsKey(lexeme))
                            {
                                // Если есть, берем его индекс
                                ident = identifierIndexTable[lexeme];
                            }
                            else
                            {
                                // Если нет, добавляем новый идентификатор в таблицу и присваиваем ему индекс
                                ident = identifierIndexTable.Count + 1; // Предполагаем, что индексы начинаются с 1
                                identifierIndexTable.Add(lexeme, ident);
                            }
                        }
                    }
                    if (lexeme != "")
                    {
                        tokens.Add(new Token(tokenType, lexeme, lineNumber, NumberInL - lexeme.Length, ident));
                    }
                    currentLexeme = "";


                    currentLexeme += '/';
                    lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                    tokenType = GetTokenType(lexeme);
                    ident = -1;
                    if (tokenType == TokenType.id)
                    {
                        if (tokenType == TokenType.id)
                        {
                            // Проверяем, есть ли идентификатор уже в таблице
                            if (identifierIndexTable.ContainsKey(lexeme))
                            {
                                // Если есть, берем его индекс
                                ident = identifierIndexTable[lexeme];
                            }
                            else
                            {
                                // Если нет, добавляем новый идентификатор в таблицу и присваиваем ему индекс
                                ident = identifierIndexTable.Count + 1; // Предполагаем, что индексы начинаются с 1
                                identifierIndexTable.Add(lexeme, ident);
                            }
                        }
                    }
                    tokens.Add(new Token(tokenType, lexeme, lineNumber, NumberInL - lexeme.Length, ident));
                    currentLexeme = "";
                }
                return;
            }
            CommentaryR = true;
        }


        private void ProcessNextToken(List<Token> tokens)
        {
            string lexeme;
            TokenType tokenType;
            bool Checker = false;
            bool SaveChecker = false;
            char currentChar;
            currentChar = ReadNextChar();

            while (char.IsWhiteSpace(currentChar))
            {
                if (currentChar == '\n')
                {
                    NumberInL = 0;
                    lineNumber++;
                }
                currentChar = ReadNextChar();
            }

            if (currentChar == '\0')
            {
                return;
            }

            IsCommentary(ref currentChar, ref currentLexeme, ref tokens);

            // Считывание букв, цифр и других символов для формирования лексемы
            while (char.IsLetterOrDigit(currentChar) || ReservedWords.Contains(currentChar.ToString()))
            {
                if (currentChar == '\n')
                {
                    NumberInL = 0;
                    lineNumber++;
                }
                IsCommentary(ref currentChar, ref currentLexeme, ref tokens);
                
                if (Separators.Contains(currentChar))
                {
                    if (Checker)
                    {
                        Checker = false;
                        if (!string.IsNullOrEmpty(currentLexeme))
                        {
                            lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                            tokenType = GetTokenType(lexeme);
                            int ident = -1;
                            if (tokenType == TokenType.id)
                            {
                                if (tokenType == TokenType.id)
                                {
                                    // Проверяем, есть ли идентификатор уже в таблице
                                    if (identifierIndexTable.ContainsKey(lexeme))
                                    {
                                        // Если есть, берем его индекс
                                        ident = identifierIndexTable[lexeme];
                                    }
                                    else
                                    {
                                        // Если нет, добавляем новый идентификатор в таблицу и присваиваем ему индекс
                                        ident = identifierIndexTable.Count + 1; // Предполагаем, что индексы начинаются с 1
                                        identifierIndexTable.Add(lexeme, ident);
                                    }
                                }
                            }
                            tokens.Add(new Token(tokenType, lexeme, lineNumber, NumberInL - lexeme.Length, ident));
                            currentLexeme = "";
                        }
                        SaveChecker = true;
                    }
                    currentLexeme += currentChar;
                    currentChar = ReadNextChar();
                    if (!string.IsNullOrEmpty(currentLexeme))
                    {
                        lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                        tokenType = GetTokenType(lexeme);
                        int ident = -1;
                        if (tokenType == TokenType.id)
                        {
                            if (tokenType == TokenType.id)
                            {
                                // Проверяем, есть ли идентификатор уже в таблице
                                if (identifierIndexTable.ContainsKey(lexeme))
                                {
                                    // Если есть, берем его индекс
                                    ident = identifierIndexTable[lexeme];
                                }
                                else
                                {
                                    // Если нет, добавляем новый идентификатор в таблицу и присваиваем ему индекс
                                    ident = identifierIndexTable.Count + 1; // Предполагаем, что индексы начинаются с 1
                                    identifierIndexTable.Add(lexeme, ident);
                                }
                            }
                        }
                        tokens.Add(new Token(tokenType, lexeme, lineNumber, NumberInL - lexeme.Length, ident));
                        currentLexeme = "";
                    }
                    if (currentChar == '\n')
                    {
                        if (currentChar == '\n')
                        {
                            NumberInL = 0;
                            lineNumber++;
                        }
                        currentChar = ReadNextChar();
                    }
                }
                else
                if (char.IsLetterOrDigit(currentChar) || ReservedWords.Contains(currentChar.ToString()))
                {
                    if (currentChar == '\n')
                    {
                        NumberInL = 0;
                        lineNumber++;
                    }
                    currentLexeme += currentChar;
                    currentChar = ReadNextChar();
                    Checker = true;
                    SaveChecker = false;
                }
                else
                    if (currentChar != '\0' && currentChar != '\n' && !char.IsWhiteSpace(currentChar))
                {
                    tokens.Add(new Token(TokenType.Other, "" + currentChar, lineNumber));
                }
            }
            if (currentChar != '\0' && currentChar != '\n' && !char.IsWhiteSpace(currentChar))
            {
                tokens.Add(new Token(TokenType.Other, "" + currentChar, lineNumber));
            }
            if (currentChar == '\n')
            {
                NumberInL = 0;
                lineNumber++;
            }
            if (!SaveChecker)
            {
                if (!string.IsNullOrEmpty(currentLexeme))
                {
                    lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                    tokenType = GetTokenType(lexeme);
                    int ident = -1;
                    if (tokenType == TokenType.id)
                    {
                        if (tokenType == TokenType.id)
                        {
                            // Проверяем, есть ли идентификатор уже в таблице
                            if (identifierIndexTable.ContainsKey(lexeme))
                            {
                                // Если есть, берем его индекс
                                ident = identifierIndexTable[lexeme];
                            }
                            else
                            {
                                // Если нет, добавляем новый идентификатор в таблицу и присваиваем ему индекс
                                ident = identifierIndexTable.Count + 1; // Предполагаем, что индексы начинаются с 1
                                identifierIndexTable.Add(lexeme, ident);
                            }
                        }
                    }
                    tokens.Add(new Token(tokenType, lexeme, lineNumber, NumberInL - lexeme.Length, ident));
                    currentLexeme = "";
                }
            }

            lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
            if (string.IsNullOrEmpty(lexeme))
            {
                currentLexeme = "";
                if (currentChar != '\0')
                {
                    ProcessNextToken(tokens);
                }
                return;
            }
        }



        public List<Token> GetTokens()
        {
            List<Token> tokens = new List<Token>();
            while (true)
            {
                ProcessNextToken(tokens);
                if (currentLexeme == "")
                {
                    break;
                }

                // Проверка на конец файла
                if (reader.EndOfStream)
                {
                    break;
                }
            }

            return tokens;
        }

        public class Error
        {
            public string Lexeme { get; set; }
            public int LineNumber { get; set; }
        }
    }
}

namespace LANG
{


    public enum TokenType
    {
        tConst,
        tElse,
        tEnd,
        tFunc,
        tIf,
        tModule,
        tBegin,
        tArray,
        tRepeat,
        tDo,
        tVar,
        tInt,
        tFloat,
        tBool,
        And,
        Or,
        Not,
        NotRavno,
        tz,
        z,
        sc1,
        sc2,
        dt,
        ravno,
        kvsc1,
        kvsc2,
        figsc1,
        figsc2,
        more,
        eq,
        less,
        moreOrEq,
        lessOrEq,
        noEq,
        minus,
        plus,
        div,
        mul,
        id,
        ct,
        tTrue,
        tFalse,
        Other
    }
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Lexeme { get; set; }
        public int LineNumber { get; set; }
        public int NumberInLine { get; set; }
        public int ID { get; set; }


        public Token(TokenType tokenType, string lexeme, int lineNumber)
        {
            TokenType = tokenType;
            Lexeme = lexeme;
            LineNumber = lineNumber;
        }
        public Token(TokenType tokenType, string lexeme, int lineNumber, int Number)
        {
            TokenType = tokenType;
            Lexeme = lexeme;
            LineNumber = lineNumber;
            NumberInLine = Number;
        }
        public Token(TokenType tokenType, string lexeme, int lineNumber, int Number, int id)
        {
            TokenType = tokenType;
            Lexeme = lexeme;
            LineNumber = lineNumber;
            NumberInLine = Number;
            ID = id;
        }
    }

    public class IdentifierInfo
    {
        public string Lex { get; set; }   // Лексема идентификатора
        public string Cat { get; set; }   // Категория идентификатора
        public string Tip { get; set; }   // Тип идентификатора
        public Dictionary<string, object> Other { get; set; } // Дополнительные поля

        public IdentifierInfo(string lex, string cat, string tip, Dictionary<string, object> other)
        {
            Lex = lex;
            Cat = cat;
            Tip = tip;
            Other = other;
        }
    }
}