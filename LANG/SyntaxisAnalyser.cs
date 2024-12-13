using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace LANG
{
    internal class SyntaxisAnalyser
    {
        private List<Token> tokensList;
        private int currentTokenIndex;
        private Form1 form;  // Ссылка на форму
        private Dictionary<string, LANG.LexicalAnalyzer.IdentifierInfo> indexesTable;
        private string curIdent;
        private int currentAddress = 0;  // Хранит текущий адрес для переменной
        private LexicalAnalyzer lexicalAnalyzer;
        public List<int> curDimensions { get; set; } = new List<int>();
        bool check = false;
        private const int sizeInt = 4;
        private const int sizeFloat = 8;
        private const int sizeBool = 1;

        public SyntaxisAnalyser (List<Token> tokens,
            Form1 form,
            Dictionary<string, LANG.LexicalAnalyzer.IdentifierInfo> iT,
            LexicalAnalyzer lA)
        {
            this.tokensList = tokens;
            this.currentTokenIndex = 0;
            this.form = form;  // Инициализация формы
            this.indexesTable = iT;
            this.lexicalAnalyzer = lA;
        }

        private Token CurrentToken => tokensList[currentTokenIndex];

        // Инициализация парсинга
        public void Parse ()
        {
            try
            {
                ParseModule();
            }
            catch (Exception ex)
            {
                form.richTextBoxErrors.Text += ex.Message + Environment.NewLine;  // Вывод ошибки в richTextBox
            }
        }

        // Парсинг модуля, 1
        private void ParseModule ()
        {
            Expect(TokenType.tModule);  // module
            Expect(TokenType.id);       // id (имя модуля)
            Expect(TokenType.tz);       // ;
            ParseBlock();               // var/arr + operations
            Expect(TokenType.tEnd);     // end
        }

        // Описание переменной, 4
        private void ParseDeclaration ()
        {
            if (CurrentToken.TokenType == TokenType.id)
                curIdent = CurrentToken.Lexeme;
            Expect(TokenType.id);    // Имя переменной или массива
            if(indexesTable[curIdent].Type != TokenType.EOF)
            {
                form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine, CurrentToken.Lexeme.Length, Color.Red);
                throw new Exception($"Ошибка семантического анализа ({CurrentToken.LineNumber}, {CurrentToken.NumberInLine}): переменная '{curIdent}' уже существует");
            }
            Expect(TokenType.dt);    // :
            ParseType();             // Тип переменной
        }

        // Простой тип, 8
        private void ParseType ()
        {
            // Могут быть типы int, float, и Boolean
            if (CurrentToken.TokenType == TokenType.tInt ||
                CurrentToken.TokenType == TokenType.tFloat ||
                CurrentToken.TokenType == TokenType.tBool)
            {
                indexesTable[curIdent].Type = CurrentToken.TokenType;

                // Проверка, является ли переменная массивом
                if (curDimensions != null && curDimensions.Count > 0)
                {
                    indexesTable[curIdent].Dimensions = curDimensions.ToArray();

                    // Рассчитываем количество элементов массива (произведение всех размерностей)
                    int totalElements = 1;
                    foreach (int dim in curDimensions)
                    {
                        totalElements *= dim;
                    }

                    // Проверяем кратность текущего адреса размеру типа
                    int size = GetTypeSize(indexesTable[curIdent].Type);

                    if (currentAddress % size != 0)
                    {
                        currentAddress += size - (currentAddress % size);  // Выравнивание по размеру типа
                    }
                    indexesTable[curIdent].Address = currentAddress;

                    // Вычисляем общий объем памяти, который занимает переменная или массив
                    currentAddress += totalElements * size;
                }
                else
                {
                    // Проверяем кратность текущего адреса размеру типа
                    int size = GetTypeSize(indexesTable[curIdent].Type);

                    if (currentAddress % size != 0)
                    {
                        currentAddress += size - (currentAddress % size);  // Выравнивание по размеру типа
                    }
                    indexesTable[curIdent].Address = currentAddress;

                    // Вычисляем общий объем памяти, который занимает переменная или массив
                    currentAddress += size;
                }

                curDimensions.Clear(); // Очищаем список перед началом новой записи
                currentTokenIndex++;  // Пропустить тип
            }
            else
            {
                form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine, CurrentToken.Lexeme.Length, Color.Red);
                throw new Exception($"Ошибка синтаксического анализа ({CurrentToken.LineNumber}, {CurrentToken.NumberInLine}): ожидался тип данных, но получен {CurrentToken.TokenType}");
            }
        }



        // Парсинг блока, 3
        private void ParseBlock ()
        {
            while(CurrentToken.TokenType != TokenType.tBegin)
            {
                Console.WriteLine($"Текущий токен: {CurrentToken.TokenType}");
                // Описание, 4
                if (CurrentToken.TokenType == TokenType.tVar)
                {
                    Expect(TokenType.tVar);    // var
                    ParseDeclaration();        // переменная
                    Expect(TokenType.tz);      // ;
                }
                // Массив, 9
                else if (CurrentToken.TokenType == TokenType.tArray)
                {
                    Expect(TokenType.tArray);  // arr
                    Expect(TokenType.kvsc1);   // [
                    ParseLstInd();             // обработка списка индексов (LstInd)
                    Expect(TokenType.kvsc2);   // ]
                    ParseDeclaration();        // переменная
                    Expect(TokenType.tz);      // ;
                }
                else {
                    form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine, CurrentToken.Lexeme.Length, Color.Red);
                    throw new Exception($"Ошибка синтаксического анализа ({CurrentToken.LineNumber}, {CurrentToken.NumberInLine}): ожидался var или arr, но получен {CurrentToken.TokenType}");
                }
            }

            Expect(TokenType.tBegin);    // begin
            ParseStatements();           // операторы
        }

        // LstInd, список массивов, 10
        private void ParseLstInd ()
        {
            curDimensions.Clear(); // Очищаем список перед началом новой записи
            
            // Первый размер массива
            if (CurrentToken.TokenType == TokenType.ct)
            {
                curDimensions.Add(int.Parse(CurrentToken.Lexeme));  // Добавляем первый размер
            }
            Expect(TokenType.ct);  // Ожидаем целое число

            // Проверка на наличие дополнительных индексов
            while (CurrentToken.TokenType == TokenType.z)
            {
                Expect(TokenType.z);  // Пропускаем запятую

                if (CurrentToken.TokenType == TokenType.ct)
                {
                    curDimensions.Add(int.Parse(CurrentToken.Lexeme));  // Добавляем следующий размер
                }
                Expect(TokenType.ct);  // Ожидаем целое число
            }

            // Можете сохранить `dimensionsArray` в идентификаторе или другом месте, если необходимо
        }

        // Последовательность операторов, 11
        private void ParseStatements ()
        {
            while (CurrentToken.TokenType != TokenType.figsc2 && CurrentToken.TokenType != TokenType.tEnd)
            {
                if (CurrentToken.TokenType == TokenType.EOF)
                {
                    throw new Exception("Ошибка: не найдена закрывающая фигурная скобка.");
                }

                Console.WriteLine($"Текущий токен: {CurrentToken.TokenType}");
                ParseStatement();
            }
        }




        // Оператор, 12
        private void ParseStatement ()
        {
            if (CurrentToken.TokenType == TokenType.id)
            {
                if (indexesTable[CurrentToken.Lexeme].Type == TokenType.EOF)
                {
                    form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine-1, CurrentToken.Lexeme.Length, Color.Red);
                    throw new Exception($"Ошибка семантического анализа ({CurrentToken.LineNumber}, {CurrentToken.NumberInLine}): переменная '{CurrentToken.Lexeme}' не объявлена");
                }
                ParseAssignment();  // Обработка присваивания x = expr;
            }
            else if (CurrentToken.TokenType == TokenType.tRepeat)
            {
                ParseLoop();        // do { ... } repeat (condition);
            }
            else
            {
                form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine, CurrentToken.Lexeme.Length, Color.Red);
                throw new Exception($"Ошибка синтаксического анализа ({CurrentToken.LineNumber}, {CurrentToken.NumberInLine}): ожидался оператор или закрывающая фигурная скобка, но получен {CurrentToken.TokenType}");
            }
        }

        // Присваивание, 13
        private void ParseAssignment ()
        {
            if (CurrentToken.TokenType == TokenType.id && indexesTable[CurrentToken.Lexeme].Dimensions == null)
            {
                curIdent = CurrentToken.Lexeme;
                Expect(TokenType.id);       // переменная
                Expect(TokenType.ravno);    // =
                ParseExpression();          // выражение
                Expect(TokenType.tz);       // ;
            }
            else
            {
                // Присваивание элементу массива
                if (CurrentToken.TokenType == TokenType.id)
                {
                    curIdent = CurrentToken.Lexeme;// Сохраняем имя массива
                }  
                Expect(TokenType.id);            // имя массива

                // Обрабатываем индексы массива
                ParseArrayAccess();

                Expect(TokenType.ravno);         // =
                ParseExpression();               // выражение
                Expect(TokenType.tz);            // ;
            }
            
        }

        private object ParseArrayAccess ()
        {
            // Здесь мы используем object, чтобы результатом мог быть как числовой индекс, так и элемент массива.
            List<object> indexes = new List<object>();

            // Ожидаем открывающую скобку для индексации массива
            Expect(TokenType.kvsc1);

            // Чтение индексов массива (например, a[1, 2] или a[a[1,2], 1])
            while (CurrentToken.TokenType != TokenType.kvsc2)
            {
                if (CurrentToken.TokenType == TokenType.EOF)
                {
                    throw new Exception($"Ошибка синтаксического анализа: нет закрывающей скобки, но получен {CurrentToken.Lexeme}");
                }

                // Рекурсивный вызов для вложенной индексации
                if (CurrentToken.TokenType == TokenType.id)
                {
                    string arrayName = CurrentToken.Lexeme;

                    if (indexesTable[arrayName].Type == TokenType.tInt)
                    {
                        if (indexesTable.ContainsKey(arrayName) && indexesTable[arrayName].Dimensions != null && indexesTable[arrayName].Dimensions.Length > 0)
                        {
                            Expect(TokenType.id);
                            // Если текущий токен — массив, вызываем рекурсивный анализ
                            object result = ParseArrayAccess();
                            indexes.Add(result);  // Добавляем результат рекурсивного вызова
                        }
                        else
                        {
                            // Если это переменная или другая структура, добавляем как индекс
                            indexes.Add(arrayName);
                            Expect(TokenType.id);
                        }
                    }
                    else
                    {
                        throw new Exception($"Ошибка синтаксического анализа: недопустимый тип '{indexesTable[arrayName].Type}' для индекса массива.");
                    }
                }
                else if (CurrentToken.TokenType == TokenType.ct)
                {
                    // Если это числовой индекс
                    int index = int.Parse(CurrentToken.Lexeme);
                    indexes.Add(index);
                    Expect(TokenType.ct);
                }
                else
                {
                    throw new Exception($"Ошибка синтаксического анализа: недопустимый индекс '{CurrentToken.Lexeme}' для массива.");
                }

                if (CurrentToken.TokenType == TokenType.z)
                {
                    Expect(TokenType.z);  // Пропускаем запятую
                }
                else break;
            }

            // Закрывающая скобка
            Expect(TokenType.kvsc2);

            // Проверяем количество индексов
            if (indexesTable[curIdent].Dimensions.Length != indexes.Count)
            {
                throw new Exception($"Ошибка семантического анализа: неправильное количество индексов для массива '{curIdent}'");
            }

            // Проверка индексов (проводится только для числовых индексов)
            for (int i = 0; i < indexes.Count; i++)
            {
                if (indexes[i] is int)
                {
                    int index = (int)indexes[i];
                    if (index >= indexesTable[curIdent].Dimensions[i] || index < 0)
                    {
                        throw new Exception($"Ошибка семантического анализа: индекс {index} выходит за пределы массива '{curIdent}' в измерении {i}");
                    }
                }
                // Если индекс — это массив или переменная, проверка границ будет на этапе выполнения.
            }

            // Возвращаем объект — это может быть либо число, либо элемент массива
            return indexes;
        }




        // Цикл пост, 16
        private void ParseLoop ()
        {
            Expect(TokenType.tRepeat);  // repeat
            Expect(TokenType.figsc1);   // {
            curIdent = "";
            ParseStatements();          // операторы цикла
            Expect(TokenType.figsc2);   // }
            Expect(TokenType.tUntil);  //   until
            Expect(TokenType.sc1);      // (
            check = false;
            curIdent = "loop";
            ParseExpression();          // условие выхода
            if(!check)
            {
                form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine - 1, CurrentToken.Lexeme.Length, Color.Red);
                throw new Exception($"Ошибка семантического анализа: несоответствие типов, должно быть tBool, {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
            }
            Expect(TokenType.sc2);      // )
            Expect(TokenType.tz);       // ;
        }

        // Выражение, 17
        private void ParseExpression ()
        {
            ParseSimpleExpression();  // Простое выражение

            // Опционально проверяем наличие отношения (пункт 18)
            if (CurrentToken.TokenType == TokenType.less ||
                CurrentToken.TokenType == TokenType.lessOrEq ||
                CurrentToken.TokenType == TokenType.more ||
                CurrentToken.TokenType == TokenType.moreOrEq ||
                CurrentToken.TokenType == TokenType.eq ||
                CurrentToken.TokenType == TokenType.noEq)
            {
                check = true;
                Token relationToken = CurrentToken;  // Сохраняем оператор отношения
                currentTokenIndex++;  // Пропускаем оператор
                ParseSimpleExpression();  // Правое простое выражение
            }
            if(CurrentToken.TokenType == TokenType.And || CurrentToken.TokenType == TokenType.Or)
            {
                curIdent = "loop";
                currentTokenIndex++;
                ParseExpression();
            }
        }

        // Простое выражение, 19
        private void ParseSimpleExpression ()
        {
            ParseTerm();  // Терм

            // Проверка на наличие аддитивных операций (пункт 20)
            while (CurrentToken.TokenType == TokenType.plus ||
                   CurrentToken.TokenType == TokenType.minus)  // || в грамматике
            {
                Console.WriteLine($"Текущий токен: {CurrentToken.TokenType}");
                Token addOpToken = CurrentToken;  // Сохраняем оператор
                currentTokenIndex++;  // Пропускаем оператор
                ParseTerm();  // Парсим следующий терм
            }
        }

        // Терм, 21
        private void ParseTerm ()
        {
            ParseFactor();  // Фактор

            // Проверка на наличие мультипликативных операций (пункт 22)
            while (CurrentToken.TokenType == TokenType.mul ||
                   CurrentToken.TokenType == TokenType.div)  // && в грамматике
            {
                Console.WriteLine($"Текущий токен: {CurrentToken.TokenType}");
                Token multOpToken = CurrentToken;  // Сохраняем оператор
                currentTokenIndex++;  // Пропускаем оператор
                ParseFactor();  // Парсим следующий фактор
            }
        }

        // Фактор, 23
        private void ParseFactor ()
        {
            if (CurrentToken.TokenType == TokenType.ct)  // Константа
            {
                if (curIdent == "loop")
                {
                    
                }
                else if (indexesTable[curIdent].Type == TokenType.tInt)
                {
                    if (!lexicalAnalyzer.IsInteger(CurrentToken.Lexeme))
                    {
                        form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine-1, CurrentToken.Lexeme.Length, Color.Red);
                        throw new Exception($"Ошибка семантического анализа: несоответствие типов, должно быть {indexesTable[curIdent].Type}, {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
                    }
                }
                else if (indexesTable[curIdent].Type == TokenType.tFloat)
                {
                    if (!lexicalAnalyzer.IsFloat(CurrentToken.Lexeme))
                    {
                        form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine-1, CurrentToken.Lexeme.Length, Color.Red);
                        throw new Exception($"Ошибка семантического анализа: несоответствие типов, должно быть {indexesTable[curIdent].Type}, {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
                    }
                }
                else if (indexesTable[curIdent].Type == TokenType.tBool)
                {
                    if (!lexicalAnalyzer.IsBoolean(CurrentToken.Lexeme))
                    {
                        form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine-1, CurrentToken.Lexeme.Length, Color.Red);
                        throw new Exception($"Ошибка семантического анализа: несоответствие типов, должно быть {indexesTable[curIdent].Type}, {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
                    }
                }
                currentTokenIndex++;  // Пропускаем константу
            }
            else if(CurrentToken.TokenType == TokenType.tTrue || CurrentToken.TokenType == TokenType.tFalse)
            {
                if (curIdent == "loop")
                {

                }
                else if (indexesTable[curIdent].Type == TokenType.tTrue || indexesTable[curIdent].Type == TokenType.tFalse)
                {
                    if (!lexicalAnalyzer.IsBoolean(CurrentToken.Lexeme))
                    {
                        form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine - 1, CurrentToken.Lexeme.Length, Color.Red);
                        throw new Exception($"Ошибка семантического анализа: несоответствие типов, должно быть {indexesTable[curIdent].Type}, {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
                    }
                }
                currentTokenIndex++;  // Пропускаем константу
            }
            else if (CurrentToken.TokenType == TokenType.id)  // Переменная или индексированная переменная
            {
                if (curIdent == "loop")
                {
                    curIdent = "" + indexesTable[CurrentToken.Lexeme].Type;
                    if (indexesTable[CurrentToken.Lexeme].Type == TokenType.tBool)
                        check = true;
                }
                else if(indexesTable[curIdent].Type == TokenType.tBool)
                {
                    check = false;
                    curIdent = "loop";
                    ParseExpression();          
                    return;
                }
                else if (indexesTable[curIdent].Type != indexesTable[CurrentToken.Lexeme].Type)
                {
                    form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine - 1, CurrentToken.Lexeme.Length, Color.Red);
                    throw new Exception($"Ошибка семантического анализа: несоответствие типов, должно быть {indexesTable[curIdent].Type}, {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
                }
                ParseVariable();  // Парсим переменную
            }
            else if (CurrentToken.TokenType == TokenType.sc1)  // Открывающая скобка "("
            {
                Expect(TokenType.sc1);  // (
                ParseExpression();  // Парсим выражение в скобках
                Expect(TokenType.sc2);  // )
            }
            else if (CurrentToken.TokenType == TokenType.Not)  // Унарный оператор "!"
            {
                currentTokenIndex++;  // Пропускаем "!"
                ParseFactor();  // Парсим фактор после "!"
            }
            else if (CurrentToken.TokenType == TokenType.And || CurrentToken.TokenType == TokenType.Or)
            {
                currentTokenIndex++;
                ParseExpression();
            }
            else
            {
                form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine, CurrentToken.Lexeme.Length, Color.Red);
                throw new Exception($"Ошибка синтаксического анализа: неожиданный токен {CurrentToken.Lexeme} на строке {CurrentToken.LineNumber}");
            }
        }

        // Переменная или индексированная переменная, 14, 15
        private void ParseVariable ()
        {
            if(curIdent != "loop")
            curIdent = CurrentToken.Lexeme;
            Expect(TokenType.id);  // Переменная или индексированная переменная

            if (CurrentToken.TokenType == TokenType.kvsc1)  // Если это индексированная переменная
            {
                ParseArrayAccess();
            }
        }

        private void Expect (TokenType expected)
        {
            if (CurrentToken.TokenType == expected)
            {
                currentTokenIndex++;
            }
            else
            {
                form.HighlightError(CurrentToken.LineNumber, CurrentToken.NumberInLine-1, CurrentToken.Lexeme.Length, Color.Red);
                throw new Exception($"Ошибка синтаксического анализа ({CurrentToken.LineNumber}, {CurrentToken.NumberInLine}): ожидался {expected}, но получен {CurrentToken.TokenType}");
            }
        }

        private int GetTypeSize (TokenType type)
        {
            switch (type)
            {
                case TokenType.tInt:
                    return sizeInt;   // 4 байта
                case TokenType.tFloat:
                    return sizeFloat; // 8 байт
                case TokenType.tBool:
                    return sizeBool;  // 1 байт
                default:
                    throw new Exception("Неизвестный тип");
            }
        }
    }

}
