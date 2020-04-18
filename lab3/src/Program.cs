using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
	{
	class CProgram : Expression
		{
		public List<string> Errors { get; }
		public CProgram (IEnumerable<Expression> lines, IEnumerable<string> Errors) : base(String.Join("\r\n", lines.Select(line => line.Code)), State.Program)
			{
			Children.AddRange(lines);
			this.Errors = new List<string>(Errors);
			}
		}
	}
