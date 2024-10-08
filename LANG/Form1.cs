using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LANG
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxAnalyse.Text = "module MyProgram;\r\nvar x: int;\r\narr [3] a: int; \r\nbegin\r\n    x = 10;\r\n    do {\r\n        x = x * 2.54 + x*x+3;\r\n} repeat (x > 5 || x >= 0 && x != 0 - 5 || x == 6);\r\n  /* This is my Commentary */  \r\n else {\r\n        x = 0 - x + 5;\r\n}\r\nend\r\n";
        }

        private void buttonAnalyse_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
            richTextBoxErrors.Text = "";
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
                Dictionary<string, int> indexesTable = lexicalAnalyzer.GetIdTable();
                foreach (Token token in tokens)
                {
                    // Вывод ошибок (Тип - Other)
                    if (token.TokenType == TokenType.Other)
                    {
                        richTextBoxErrors.AppendText($"Лексическая ошибка! Неопознанный символ ( Лексема: ' {token.Lexeme} ' ) в строке <{token.LineNumber}, {token.NumberInLine}>{Environment.NewLine}");
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

                foreach (KeyValuePair<string, int> indexTable in indexesTable)
                {
                    Console.WriteLine($"<{indexTable.Key} = {indexTable.Value}>{Environment.NewLine}");
                }

                if(richTextBoxErrors.Text == "")
                {
                    SyntaxisAnalyser syntaxisAnalyzer = new SyntaxisAnalyser(tokens);
                }

            }

        }


        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
            textBoxAnalyse.Text = "";
            richTextBoxErrors.Text = "";
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
