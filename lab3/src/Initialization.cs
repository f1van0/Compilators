using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
	{
	class Initialization : Expression
		{
		public Expression Left { get { return Children[0]; } }
		public Expression Right { get { return Children[1]; } }
		public Initialization (Expression left, Expression right) : base($"{left.Code} = {right.Code}", State.Initialization)
			{
			Children.Add(left);
			Children.Add(right);
			}
		}
	}
