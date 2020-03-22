using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab2.Producer;

namespace lab2
	{
	public partial class Form1 : Form
		{
		public Form1 ()
			{
			InitializeComponent();
			textBox1.Text = "Подлежащее Сказуемое\r\nПодлежащее Сказуемое Дополнение\r\nСказуемое Подлежащее Дополнение\r\nСказуемое Подлежащее\r\nСказуемое Дополнение";
			}

		private void label2_Click (object sender, EventArgs e)
			{
			}

		private void button1_Click (object sender, EventArgs e)
			{
			textBox2.Text = "";
			string[] patterns = textBox1.Text.Split("\n\r".ToArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach ( var pattern in patterns )
				{
				try
					{
					string Sentence = Generator.Generate(pattern);
					textBox2.Text += Sentence + "\r\n";
					}
				catch ( Exception ex)
					{
					textBox2.Text += ex.Message;
					break;
					}
				}
			}
		}
	}
