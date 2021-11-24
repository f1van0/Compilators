namespace lab1
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
		protected override void Dispose (bool disposing)
			{
			if ( disposing && ( components != null ) )
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
		private void InitializeComponent ()
			{
            this.button1 = new System.Windows.Forms.Button();
            this.textInput = new System.Windows.Forms.TextBox();
            this.lexemsText = new System.Windows.Forms.TextBox();
            this.lexTable = new System.Windows.Forms.TextBox();
            this.postfixText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.turpleText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(711, 126);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Зпустить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textInput
            // 
            this.textInput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textInput.Location = new System.Drawing.Point(11, 28);
            this.textInput.Margin = new System.Windows.Forms.Padding(2);
            this.textInput.Multiline = true;
            this.textInput.Name = "textInput";
            this.textInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textInput.Size = new System.Drawing.Size(778, 94);
            this.textInput.TabIndex = 2;
            // 
            // lexemsText
            // 
            this.lexemsText.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lexemsText.Location = new System.Drawing.Point(11, 180);
            this.lexemsText.Margin = new System.Windows.Forms.Padding(2);
            this.lexemsText.Multiline = true;
            this.lexemsText.Name = "lexemsText";
            this.lexemsText.ReadOnly = true;
            this.lexemsText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lexemsText.Size = new System.Drawing.Size(778, 92);
            this.lexemsText.TabIndex = 3;
            // 
            // lexTable
            // 
            this.lexTable.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lexTable.Location = new System.Drawing.Point(11, 316);
            this.lexTable.Margin = new System.Windows.Forms.Padding(2);
            this.lexTable.Multiline = true;
            this.lexTable.Name = "lexTable";
            this.lexTable.ReadOnly = true;
            this.lexTable.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lexTable.Size = new System.Drawing.Size(290, 254);
            this.lexTable.TabIndex = 4;
            // 
            // postfixText
            // 
            this.postfixText.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.postfixText.Location = new System.Drawing.Point(322, 316);
            this.postfixText.Margin = new System.Windows.Forms.Padding(2);
            this.postfixText.Multiline = true;
            this.postfixText.Name = "postfixText";
            this.postfixText.ReadOnly = true;
            this.postfixText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.postfixText.Size = new System.Drawing.Size(467, 254);
            this.postfixText.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ввод выражений";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 301);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Таблица лексем";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(319, 301);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Постфиксная запись";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 165);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Вывод получившхся выражений";
            // 
            // turpleText
            // 
            this.turpleText.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.turpleText.Location = new System.Drawing.Point(11, 596);
            this.turpleText.Margin = new System.Windows.Forms.Padding(2);
            this.turpleText.Multiline = true;
            this.turpleText.Name = "turpleText";
            this.turpleText.ReadOnly = true;
            this.turpleText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.turpleText.Size = new System.Drawing.Size(778, 247);
            this.turpleText.TabIndex = 10;
            this.turpleText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 581);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Запись в виде тетрад";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 860);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.turpleText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.postfixText);
            this.Controls.Add(this.lexTable);
            this.Controls.Add(this.lexemsText);
            this.Controls.Add(this.textInput);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

			}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textInput;
		private System.Windows.Forms.TextBox lexemsText;
		private System.Windows.Forms.TextBox lexTable;
		private System.Windows.Forms.TextBox postfixText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox turpleText;
        private System.Windows.Forms.Label label1;
    }
	}

