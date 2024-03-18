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
            textBoxAnalyse.Text = "module MyProgram;\r\nvar x: int;\r\nbegin\r\n    x = 10;\r\n    if x > 5 {\r\n        x = x - 5;\r\n}\r\n  /* This is my Commentary */  \r\n else {\r\n        x = x + 5;\r\n}\r\nend\r\n";
        }

        private void buttonAnalyse_Click(object sender, EventArgs e)
        {
            textBoxOutput.Text = "";
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
                foreach (Token token in tokens)
                {
                    // Обрабатывайте токены, как вам необходимо
                    textBoxOutput.Text += $"<{token.TokenType}, ( Лексема: '{token.Lexeme}' )>"/*, {token.LineNumber}>"*/ + Environment.NewLine;
                }
            }
        }


        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            textBoxOutput.Text = "";
            textBoxAnalyse.Text = "";
        }
    }
}
