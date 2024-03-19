namespace LANG
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAnalyse = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.textBoxAnalyse = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttonAnalyse
            // 
            this.buttonAnalyse.Location = new System.Drawing.Point(554, 12);
            this.buttonAnalyse.Name = "buttonAnalyse";
            this.buttonAnalyse.Size = new System.Drawing.Size(95, 41);
            this.buttonAnalyse.TabIndex = 1;
            this.buttonAnalyse.Text = "Анализировать";
            this.buttonAnalyse.UseVisualStyleBackColor = true;
            this.buttonAnalyse.Click += new System.EventHandler(this.buttonAnalyse_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Location = new System.Drawing.Point(554, 59);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(95, 41);
            this.buttonClearAll.TabIndex = 4;
            this.buttonClearAll.Text = "Очистить всё";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Location = new System.Drawing.Point(655, 12);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.ReadOnly = true;
            this.richTextBoxOutput.Size = new System.Drawing.Size(535, 674);
            this.richTextBoxOutput.TabIndex = 5;
            this.richTextBoxOutput.Text = "";
            // 
            // textBoxAnalyse
            // 
            this.textBoxAnalyse.Location = new System.Drawing.Point(12, 12);
            this.textBoxAnalyse.Name = "textBoxAnalyse";
            this.textBoxAnalyse.Size = new System.Drawing.Size(536, 674);
            this.textBoxAnalyse.TabIndex = 6;
            this.textBoxAnalyse.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1202, 698);
            this.Controls.Add(this.textBoxAnalyse);
            this.Controls.Add(this.richTextBoxOutput);
            this.Controls.Add(this.buttonClearAll);
            this.Controls.Add(this.buttonAnalyse);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAnalyse;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.RichTextBox textBoxAnalyse;
    }
}

