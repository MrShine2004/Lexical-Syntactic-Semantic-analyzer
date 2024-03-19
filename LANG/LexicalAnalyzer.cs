using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using static LANG.LexicalAnalyzer;

namespace LANG
{
    internal class LexicalAnalyzer
    {
        private StreamReader reader;
        private string currentLexeme;
        private int lineNumber;

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
        private static readonly string[] Operators = { "+", "-", "*", "/", "=", "<", ">", "<=", ">=", "==", "!=" };
        private static readonly char[] Separators = { '(', ')', '{', '}', ';', ',' , ':'};
        private static readonly string[] Booleans = { "true", "false" };
        private static readonly string[] ReservedWords = { "true", "false", "+", "-", "*", "/", "=", "<", ">", "<=", ">=", "==", "!=", "(", ")", "{", "}", "[", "]", ";", ":", ",", ".", "module", "var", "int", "bool", "float", "arr", "begin", "end", "while", "repeat", "if", "else"};

        private List<Error> Errors = new List<Error>();
        private char Buffer = '\0';

        public LexicalAnalyzer(string code)
        {
            reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(code)));
            currentLexeme = "";
            lineNumber = 1;
        }

        private char ReadNextChar()
        {
            int nextChar = reader.Read();
            Console.Write((char)nextChar);
            return (nextChar == -1) ? '\0' : (char)nextChar;
        }

        private TokenType GetTokenType(string lexeme)
        {
            if (string.IsNullOrEmpty(lexeme))
            {
                return TokenType.Other;
            }

            if (reservedWords.ContainsKey(lexeme))
            {
                return TokenType.Keyword;
            }
            else if (Operators.Any(lexeme.Contains))
            {
                return TokenType.Operator;
            }
            else if (Separators.Contains(lexeme[0]))
            {
                return TokenType.Separator;
            }
            else if (IsNumber(lexeme))
            {
                return TokenType.Number;
            }
            else
            {
                return TokenType.Identifier;
            }
        }


        private bool IsNumber(string lexeme)
        {
            // Метод проверяет, является ли лексема числом
            return int.TryParse(lexeme, out _) || float.TryParse(lexeme, out _);
        }
        private void IsCommentary(ref char currentChar)
        {
            if (currentChar == '/')
            {
                Buffer = currentChar;
                currentChar = ReadNextChar();
                if (currentChar == '*')
                {
                    Buffer = '\0';
                    while (!reader.EndOfStream)
                    {
                        currentChar = ReadNextChar();
                        if (currentChar == '*')
                        {
                            currentChar = ReadNextChar();
                            if (currentChar == '/')
                            {
                                currentChar = ReadNextChar();
                                return;
                            }
                        }
                    }
                }
            }
        }


        private void ProcessNextToken(List<Token> tokens)
        {
            string lexeme;
            TokenType tokenType;
            bool Checker = false;
            bool SaveChecker = false;
            char currentChar;
            if (Buffer != '\0')
            {
                currentChar = Buffer;
                Buffer = '\0';
            }
            else
            {
                currentChar = ReadNextChar();
            }

            while (char.IsWhiteSpace(currentChar))
            {
                if (currentChar == '\n')
                {
                    lineNumber++;
                }
                if (Buffer != '\0')
                {
                    currentChar = Buffer;
                    Buffer = '\0';
                }
                else
                {
                    currentChar = ReadNextChar();
                }
            }

            if (currentChar == '\0')
            {
                return;
            }

            IsCommentary(ref currentChar);

            // Считывание букв, цифр и других символов для формирования лексемы
            while (char.IsLetterOrDigit(currentChar) || ReservedWords.Contains(currentChar.ToString()))
            {
                if (currentChar == '\n')
                {
                    lineNumber++;
                }
                IsCommentary(ref currentChar);
                if (Separators.Contains(currentChar))
                {
                    if(Checker)
                    {
                        Checker = false;
                        if (!string.IsNullOrEmpty(currentLexeme))
                        {
                            lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                            tokenType = GetTokenType(lexeme);
                            tokens.Add(new Token(tokenType, lexeme, lineNumber));
                            currentLexeme = "";
                        }
                        SaveChecker = true;
                    }
                    currentLexeme += currentChar;
                    if (Buffer != '\0')
                    {
                        currentChar = Buffer;
                        Buffer = '\0';
                    }
                    else
                    {
                        currentChar = ReadNextChar();
                    }
                    if (!string.IsNullOrEmpty(currentLexeme))
                    {
                        lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                        tokenType = GetTokenType(lexeme);
                        tokens.Add(new Token(tokenType, lexeme, lineNumber));
                        currentLexeme = "";
                    }
                    if (currentChar == '\n')
                    {
                        currentChar = ReadNextChar();
                    }
                }
                if(char.IsLetterOrDigit(currentChar) || ReservedWords.Contains(currentChar.ToString()))
                {
                    if (currentChar == '\n')
                    {
                        lineNumber++;
                    }
                    currentLexeme += currentChar;
                    if (Buffer != '\0')
                    {
                        currentChar = Buffer;
                        Buffer = '\0';
                    }
                    else
                    {
                        currentChar = ReadNextChar();
                    }
                    Checker = true;
                    SaveChecker = false;
                }
            }

            if(!SaveChecker)
            {
                if(!string.IsNullOrEmpty(currentLexeme))
                {
                    lexeme = currentLexeme.ToLower(); // Преобразуем в нижний регистр
                    tokenType = GetTokenType(lexeme);
                    tokens.Add(new Token(tokenType, lexeme, lineNumber));
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
        Identifier,
        Keyword,
        Operator,
        Number,
        Separator,
        Other
    }
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Lexeme { get; set; }
        public int LineNumber { get; set; }

        public Token(TokenType tokenType, string lexeme, int lineNumber)
        {
            TokenType = tokenType;
            Lexeme = lexeme;
            LineNumber = lineNumber;
        }
    }


}
