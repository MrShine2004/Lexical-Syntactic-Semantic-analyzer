﻿namespace LANG
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
            this.LeftFeel = new System.Windows.Forms.Button();
            this.RightFeel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBoxErrors = new System.Windows.Forms.RichTextBox();
            this.richTextBoxIdentTables = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.buttonClearAll.Location = new System.Drawing.Point(554, 75);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(95, 41);
            this.buttonClearAll.TabIndex = 4;
            this.buttonClearAll.Text = "Всё";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxOutput.Location = new System.Drawing.Point(655, 31);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.ReadOnly = true;
            this.richTextBoxOutput.Size = new System.Drawing.Size(424, 655);
            this.richTextBoxOutput.TabIndex = 5;
            this.richTextBoxOutput.Text = "";
            // 
            // textBoxAnalyse
            // 
            this.textBoxAnalyse.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxAnalyse.Location = new System.Drawing.Point(12, 31);
            this.textBoxAnalyse.Name = "textBoxAnalyse";
            this.textBoxAnalyse.Size = new System.Drawing.Size(536, 477);
            this.textBoxAnalyse.TabIndex = 6;
            this.textBoxAnalyse.Text = "";
            // 
            // LeftFeel
            // 
            this.LeftFeel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LeftFeel.Location = new System.Drawing.Point(554, 122);
            this.LeftFeel.Name = "LeftFeel";
            this.LeftFeel.Size = new System.Drawing.Size(45, 45);
            this.LeftFeel.TabIndex = 7;
            this.LeftFeel.Text = "<";
            this.LeftFeel.UseVisualStyleBackColor = true;
            this.LeftFeel.Click += new System.EventHandler(this.button1_Click);
            // 
            // RightFeel
            // 
            this.RightFeel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RightFeel.Location = new System.Drawing.Point(605, 122);
            this.RightFeel.Name = "RightFeel";
            this.RightFeel.Size = new System.Drawing.Size(45, 45);
            this.RightFeel.TabIndex = 8;
            this.RightFeel.Text = ">";
            this.RightFeel.UseVisualStyleBackColor = true;
            this.RightFeel.Click += new System.EventHandler(this.RightFeel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(571, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Очистить";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Текст тестовой программы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(656, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Поле вывода токенов";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 515);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Ошибки:";
            // 
            // richTextBoxErrors
            // 
            this.richTextBoxErrors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxErrors.ForeColor = System.Drawing.Color.Red;
            this.richTextBoxErrors.Location = new System.Drawing.Point(12, 534);
            this.richTextBoxErrors.Name = "richTextBoxErrors";
            this.richTextBoxErrors.ReadOnly = true;
            this.richTextBoxErrors.Size = new System.Drawing.Size(536, 152);
            this.richTextBoxErrors.TabIndex = 14;
            this.richTextBoxErrors.Text = "";
            // 
            // richTextBoxIdentTables
            // 
            this.richTextBoxIdentTables.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxIdentTables.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxIdentTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxIdentTables.Location = new System.Drawing.Point(1085, 31);
            this.richTextBoxIdentTables.Name = "richTextBoxIdentTables";
            this.richTextBoxIdentTables.ReadOnly = true;
            this.richTextBoxIdentTables.Size = new System.Drawing.Size(418, 655);
            this.richTextBoxIdentTables.TabIndex = 15;
            this.richTextBoxIdentTables.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1085, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Поле вывода идентификаторов";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1511, 703);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBoxIdentTables);
            this.Controls.Add(this.richTextBoxErrors);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RightFeel);
            this.Controls.Add(this.LeftFeel);
            this.Controls.Add(this.textBoxAnalyse);
            this.Controls.Add(this.richTextBoxOutput);
            this.Controls.Add(this.buttonClearAll);
            this.Controls.Add(this.buttonAnalyse);
            this.Name = "Form1";
            this.Text = "AYal";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAnalyse;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.RichTextBox textBoxAnalyse;
        private System.Windows.Forms.Button LeftFeel;
        private System.Windows.Forms.Button RightFeel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.RichTextBox richTextBoxErrors;
        private System.Windows.Forms.RichTextBox richTextBoxIdentTables;
        private System.Windows.Forms.Label label5;
    }
}

