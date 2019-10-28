using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleTestAttributes
{
	public class ParameterData
	{
		[AdditionalInfo("age (int):")]
		public int Age;

		[AdditionalInfo]
		public string Name;

		[AdditionalInfo]
		private string _privateName;

		public string WithouAttribute;

		public ParameterData(string name, int age)
		{
			Name = name;
			_privateName = "(private) " + name;
			Age = age;
			WithouAttribute = "Wtihout attribute";
		}

	}
}
