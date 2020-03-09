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
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.textInput = new System.Windows.Forms.TextBox();
			this.lexemsText = new System.Windows.Forms.TextBox();
			this.lexTable = new System.Windows.Forms.TextBox();
			this.postfixText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "P=(a+b+c)/2 ";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Lime;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(15, 40);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(87, 33);
			this.button1.TabIndex = 1;
			this.button1.Text = "Compile";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textInput
			// 
			this.textInput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textInput.Location = new System.Drawing.Point(15, 79);
			this.textInput.Multiline = true;
			this.textInput.Name = "textInput";
			this.textInput.Size = new System.Drawing.Size(190, 312);
			this.textInput.TabIndex = 2;
			// 
			// lexemsText
			// 
			this.lexemsText.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lexemsText.Location = new System.Drawing.Point(269, 79);
			this.lexemsText.Multiline = true;
			this.lexemsText.Name = "lexemsText";
			this.lexemsText.ReadOnly = true;
			this.lexemsText.Size = new System.Drawing.Size(193, 312);
			this.lexemsText.TabIndex = 3;
			// 
			// lexTable
			// 
			this.lexTable.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lexTable.Location = new System.Drawing.Point(487, 79);
			this.lexTable.Multiline = true;
			this.lexTable.Name = "lexTable";
			this.lexTable.ReadOnly = true;
			this.lexTable.Size = new System.Drawing.Size(283, 312);
			this.lexTable.TabIndex = 4;
			// 
			// postfixText
			// 
			this.postfixText.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.postfixText.Location = new System.Drawing.Point(794, 79);
			this.postfixText.Multiline = true;
			this.postfixText.Name = "postfixText";
			this.postfixText.ReadOnly = true;
			this.postfixText.Size = new System.Drawing.Size(277, 312);
			this.postfixText.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(266, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 17);
			this.label2.TabIndex = 6;
			this.label2.Text = "Выражение";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(484, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 17);
			this.label3.TabIndex = 7;
			this.label3.Text = "Таблица лексем";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(791, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(146, 17);
			this.label4.TabIndex = 8;
			this.label4.Text = "Постфиксная запись";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1175, 677);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.postfixText);
			this.Controls.Add(this.lexTable);
			this.Controls.Add(this.lexemsText);
			this.Controls.Add(this.textInput);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textInput;
		private System.Windows.Forms.TextBox lexemsText;
		private System.Windows.Forms.TextBox lexTable;
		private System.Windows.Forms.TextBox postfixText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		}
	}

