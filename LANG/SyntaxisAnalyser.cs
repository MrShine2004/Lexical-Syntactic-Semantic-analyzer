using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANG
{
    internal class SyntaxisAnalyser
    {
        private List<Token> tokensList;
        private int currentTokenIndex;

        public SyntaxisAnalyser (List<Token> tokens)
        {
            this.tokensList = tokens;
            this.currentTokenIndex = 0;
        }

        private Token CurrentToken => tokensList[currentTokenIndex];

        public void Parse ()
        {
            // Начни с верхнего нетерминала грамматики
            ParseModule();
        }

        private void ParseModule ()
        {
            Expect(TokenType.tModule);  // module
            Expect(TokenType.id);       // id (имя модуля)
            Expect(TokenType.tz);       // ;
            ParseBlock();               // var/arr + operations
            Expect(TokenType.tEnd);     // end
        }

        private void ParseDeclaration ()
        {
            Expect(TokenType.id);    // Имя переменной или массива
            Expect(TokenType.dt);    // :
            ParseType();             // Тип переменной
        }

        private void ParseType ()
        {
            // В твоем языке могут быть типы int, float, и Boolean
            if (CurrentToken.TokenType == TokenType.tInt ||
                CurrentToken.TokenType == TokenType.tFloat ||
                CurrentToken.TokenType == TokenType.tBool)
            {
                currentTokenIndex++;  // Пропустить тип
            }
            else
            {
                throw new Exception($"Ошибка синтаксического анализа: ожидался тип данных, но получен {CurrentToken.TokenType}");
            }
        }


        private void ParseBlock ()
        {
            if (CurrentToken.TokenType == TokenType.tVar)
            {
                Expect(TokenType.tVar);    // var
                ParseDeclaration();        // переменная
                Expect(TokenType.tz);      // ;
            }
            else if (CurrentToken.TokenType == TokenType.tArray)
            {
                Expect(TokenType.tArray);  // arr
                Expect(TokenType.kvsc1);   // [
                ParseLstInd();             // обработка списка индексов (LstInd)
                Expect(TokenType.kvsc2);   // ]
                Expect(TokenType.id);      // имя массива
                Expect(TokenType.dt);      // :
                ParseType();               // тип массива
                Expect(TokenType.tz);      // ;
            }

            Expect(TokenType.tBegin);    // begin
            ParseStatements();           // операторы
        }

        private void ParseLstInd ()
        {
            // Первый размер массива
            Expect(TokenType.ct);  // целое число

            // Проверка на наличие дополнительных индексов
            while (CurrentToken.TokenType == TokenType.z)
            {
                Expect(TokenType.z);  // запятая
                Expect(TokenType.ct);   // следующий размер массива
            }
        }


        private void ParseStatements ()
        {
            while (CurrentToken.TokenType != TokenType.tEnd &&
                   CurrentToken.TokenType != TokenType.tElse)
            {
                ParseStatement();
            }

            if (CurrentToken.TokenType == TokenType.tElse)
            {
                Expect(TokenType.tElse);
                Expect(TokenType.figsc1);  // {
                ParseStatements();         // операторы
                Expect(TokenType.figsc2);  // }
            }
        }

        private void ParseStatement ()
        {
            if (CurrentToken.TokenType == TokenType.id)
            {
                ParseAssignment();  // Обработка присваивания x = expr;
            }
            else if (CurrentToken.TokenType == TokenType.tDo)
            {
                ParseLoop();        // do { ... } repeat (condition);
            }
        }

        private void ParseAssignment ()
        {
            Expect(TokenType.id);       // переменная
            Expect(TokenType.ravno);    // =
            ParseExpression();          // выражение
            Expect(TokenType.tz);       // ;
        }

        private void ParseLoop ()
        {
            Expect(TokenType.tDo);
            Expect(TokenType.figsc1);   // {
            ParseStatements();          // операторы цикла
            Expect(TokenType.figsc2);   // }
            Expect(TokenType.tRepeat);  // repeat
            Expect(TokenType.sc1);      // (
            ParseExpression();          // условие выхода
            Expect(TokenType.sc2);      // )
            Expect(TokenType.tz);       // ;
        }

        private void ParseExpression ()
        {
            // Реализация синтаксического анализа выражений (x * 2 + ...)
            // Используй деревья разбора или обратную польскую нотацию для выражений
        }

        private void Expect (TokenType expected)
        {
            if (CurrentToken.TokenType == expected)
            {
                currentTokenIndex++;
            }
            else
            {
                throw new Exception($"Ошибка синтаксического анализа: ожидался {expected}, но получен {CurrentToken.TokenType}");
            }
        }
    }

}
