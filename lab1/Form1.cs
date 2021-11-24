using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab1.Compiller;

namespace lab1
{
    public partial class Form1 : Form
    {
        string postfix = "";
        Identifier lastResult;
        public Form1()
        {
            InitializeComponent();
            Compilator.Evaluated += PrintExression;    }

        private void PrintExression(Identifier result, Identifier one, ICallable fun, List<Identifier> rest)
        {
            lastResult = result;

            turpleText.Text += $"{one.Literal}{fun}{String.Join(" ", rest.Select(i => i.Literal))}->{result.Literal}   \t{fun}   \t{result.Literal}   \t\t{one.Literal}   \t\t{String.Join(" ", rest.Select(i => i.Literal))}\r\n";
            postfixText.Text += $"    {one.PostfixName} {String.Join(" ", rest.Select(i => i.PostfixName))} {fun}\r\n"
            +
                $" >> результат промежуточной операции: {result.Value}\r\n";
        }

        Dictionary<string, Func<float, float, float>> Functions =
            new Dictionary<string, Func<float, float, float>>
            {
                ["+"] = (op1, op2) => op1 + op2,
                ["-"] = (op1, op2) => op1 - op2,
                ["*"] = (op1, op2) => op1 * op2,
                ["/"] = (op1, op2) => op1 / op2
            };

        List<string> Partionize(string _formula, out string[] Identifiers)
        {
            string formula = new String(_formula.Where(chr => chr != ' ').ToArray());
            HashSet<string> op = new HashSet<string>(Compilator.AllOperators.Concat(Compilator.AllServiceSymbols));
            Identifiers = formula.Split(op.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
            string buffer = String.Join("", formula.Split(Identifiers, StringSplitOptions.RemoveEmptyEntries));
            StringBuilder build = new StringBuilder();
            List<string> Opers = new List<string>();
            foreach (var symb in buffer)
            {
                build.Append(symb);
                if (op.Contains(build.ToString()))
                {
                    Opers.Add(build.ToString());
                    build.Clear();
                }
            }
            if (build.Length > 0)
                throw new Exception("Ошибка разбора");
            int opCount = Opers.Count();
            int idCount = Identifiers.Count();
            List<string> partioned = new List<string>();
            int i = 0;
            int k = 0;
            int o = 0;
            while (idCount + opCount > 0)
            {
                if (Opers[o][0] == formula[i])
                {
                    opCount--;
                    i += Opers[o].Length;
                    partioned.Add(Opers[o++]);
                }
                if (Identifiers[k][0] == formula[i])
                {
                    idCount--;
                    i += Identifiers[k].Length;
                    partioned.Add(Identifiers[k++]);
                }
            }
            return partioned;
        }



        //List<string> Postfix(List<string> expression, HashSet<string> Identifiers)
        //	{
        //	List<string> postfix = new List<string>();
        //	Stack<string> opStack = new Stack<string>();
        //	foreach ( string element in expression )
        //		{
        //		if ( Identifiers.Contains(element) )
        //			postfix.Add(element);
        //		else
        //			switch ( element )
        //				{
        //				case "(":{
        //					opStack.Push(element);
        //					break;
        //					}
        //				case ")":{
        //					var top = opStack.Pop();
        //					while ( top!= "(" ){
        //						postfix.Add(top);
        //						top = opStack.Pop();
        //						}
        //					break;
        //					}
        //				default:{
        //					while ( opStack.Count>0 && Operators[opStack.Peek()] >= Operators[element])
        //						postfix.Add(opStack.Pop());
        //					opStack.Push(element);
        //					break;
        //					}
        //				}
        //		}
        //	while ( opStack.Count > 0 )
        //		postfix.Add(opStack.Pop());
        //	return postfix;
        //	}



        /// <summary>
        /// Выполняет постфиксную запись лексем
        /// </summary>
        /// <param name="Postfix"></param>
        /// <returns></returns>
        //float Evaluate(List<string> Postfix)
        //	{
        //	Stack<string> buffer = new Stack<string>();
        //	foreach ( var item in Postfix )
        //		{
        //		if ( !Operators.ContainsKey(item) )
        //			buffer.Push(item);
        //		else
        //			{
        //			var op2 = int.Parse(buffer.Pop());
        //			var op1 = int.Parse(buffer.Pop());
        //			var result = Functions[item](op1, op2);
        //			buffer.Push(result.ToString());
        //			}
        //		}
        //	return float.Parse(buffer.Pop());
        //	}



        void button1_Click(object sender, EventArgs e)
        {
            IdentifierManager.Clear();
            Memory.Clear();
            lexemsText.Text = "";
            var lines = Compilator.LexicalAnalize(textInput.Text);
            foreach (var line in lines)
                lexemsText.Text += String.Join(" ", line) + "\r\n";
            HashSet<ILexem> Identifiers;
            List<List<ILexem>> expressions = Compilator.SemanticAnalize(lines, out Identifiers);
            lexTable.Text = "";
            foreach (var item in Identifiers)
                lexTable.Text += $"{item.Serialize()}\r\n";

            Identifier res;
            postfixText.Text = "";
            turpleText.Text += "Выражение\tКод\tПриемник\tОперанд1\tОперанд2\r\n";
            try
            {
                foreach (var expression in expressions)
                {
                    postfixText.Text += String.Join(" ", expression) + "\r\n";
                    res = Compilator.Evaluate(expression);
                    postfixText.Text += "> Конечный результат: " + res.Value.ToString() + "\r\n";
                    turpleText.Text += "\r\n";
                }
            }
            catch (TypeMismatchException ex)
            {
                postfixText.Text += ex.Message;
            }
            catch (VariableException ex)
            {
                postfixText.Text += ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
