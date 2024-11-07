using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LANG;


namespace LANG
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxAnalyse.Text = "module MyProgram;\r\nvar x: int;\r\narr [3, 3] a: int; \r\nvar b: bool;\r\nbegin\r\n    x = 10;\r\n    a[1, 2] = 4;\r\n    b = true;\r\n    b = x > 1;\r\n    repeat {\r\n        x = (x * (2 + x * x) + 3) / 2;\r\n} until (b && (x > 5 || a[1, 2] >= 0) && x != 0 - 5 || x == 6 || b);\r\nrepeat {\r\n        x = x + 1;\r\n} until (b);\r\n  /* This is my Commentary */  \r\nend";
        }

        private void buttonAnalyse_Click(object sender, EventArgs e)
        {
            textBoxAnalyse.SelectAll(); // Выделяем весь текст
            textBoxAnalyse.SelectionColor = Color.Black; // Меняем цвет на черный
            textBoxAnalyse.DeselectAll(); // Сбрасываем выделение
            richTextBoxOutput.Text = "";
            richTextBoxErrors.Text = "";
            richTextBoxIdentTables.Text = "";
            // Получаем текст из текстбокса
            string code = textBoxAnalyse.Text;

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Введите код перед выполнением операции.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Проверяем на пустоту
                LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(code);

                List<Token> tokens = lexicalAnalyzer.GetTokens();
                Dictionary<string, LANG.LexicalAnalyzer.IdentifierInfo> indexesTable = lexicalAnalyzer.GetIdTable();

                foreach (Token token in tokens)
                {
                    // Вывод ошибок (Тип - Other)
                    if (token.TokenType == TokenType.Other)
                    {
                        richTextBoxErrors.AppendText($"Лексическая ошибка! Неопознанный символ ( Лексема: ' {token.Lexeme} ' ) в строке <{token.LineNumber}, {token.NumberInLine}>{Environment.NewLine}");
                        HighlightError(token.LineNumber, token.NumberInLine, token.Lexeme.Length, Color.Red);
                    }

                    if (token.TokenType == TokenType.id)
                    {
                        richTextBoxOutput.AppendText($"<{token.TokenType}, {token.ID}>, ( Лексема: ' {token.Lexeme} ' ){Environment.NewLine}");
                    }
                    else
                    {
                        richTextBoxOutput.AppendText($"<{token.TokenType}>, ( Лексема: ' {token.Lexeme} ' ){Environment.NewLine}");
                    }
                }

                foreach (KeyValuePair<string, LANG.LexicalAnalyzer.IdentifierInfo> indexTable in indexesTable)
                {
                    Console.WriteLine($"<{indexTable.Key} = {indexTable.Value}>{Environment.NewLine}");
                }

                if(richTextBoxErrors.Text == "")
                {
                    SyntaxisAnalyser syntaxisAnalyzer = new SyntaxisAnalyser(tokens, this, indexesTable, lexicalAnalyzer);
                    syntaxisAnalyzer.Parse();
                }

                if (richTextBoxErrors.Text == "")
                {
                    foreach (KeyValuePair<string, LANG.LexicalAnalyzer.IdentifierInfo> indexTable in indexesTable)
                    {
                        richTextBoxIdentTables.AppendText($"< ID: {indexTable.Value.ID}, {indexTable.Key}; Address: {indexTable.Value.Address}; Type: {indexTable.Value.Type}; D: {indexTable.Value.Dimensions};>{Environment.NewLine}");
                    }
                }
            }

        }


        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
            textBoxAnalyse.Text = "";
            richTextBoxErrors.Text = "";
            richTextBoxIdentTables.Text = "";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxAnalyse.Text = "";
        }

        private void RightFeel_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
            richTextBoxErrors.Text = "";
            richTextBoxIdentTables.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void HighlightError (int lineNumber, int charPosition, int length, Color color)
        {
            // Получаем текст из RichTextBox
            string text = textBoxAnalyse.Text;

            // Разбиваем текст на строки
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Проверяем, есть ли указанная строка
            if (lineNumber < 1 || lineNumber > lines.Length)
                return; // Не корректный номер строки

            // Получаем нужную строку
            string line = lines[lineNumber - 1]; // Уменьшаем на 1, т.к. индексация начинается с 0

            // Проверяем, есть ли указанный символ в строке
            if (charPosition < 1 || charPosition+1 > line.Length)
                return; // Не корректный номер символа

            // Определяем начальную позицию для выделения
            int startIndex = textBoxAnalyse.GetFirstCharIndexFromLine(lineNumber - 1) + charPosition;

            // Устанавливаем выделение
            textBoxAnalyse.Select(startIndex, length); // Выделяем 1 символ

            // Меняем цвет выделенного текста
            textBoxAnalyse.SelectionColor = color;

            // Сбрасываем выделение
            textBoxAnalyse.Select(0, 0); // Убираем выделение
        }

    }
}
