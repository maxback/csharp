using System;
using System.Reflection;

namespace ConsoleTestAttributes
{
	class Program
	{
		static string GetTextAttibutes(ParameterData parameterData)
		{
			Type t;
			AdditionalInfoAttribute att;
			string r = string.Empty;

			t = typeof(ParameterData);

			//get Fields from class
			FieldInfo[] fi = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			for (int i = 0; i < fi.Length; i++)
			{
				att = (AdditionalInfoAttribute)Attribute.GetCustomAttribute(fi[i], typeof(AdditionalInfoAttribute));
				if (att != null)
				{
					r += $"- Text of Attribute: '{att.Text}'\r\n";
					r += $"  Name of Field: '{fi[i].Name}'\r\n";
					r += $"  Value of Field: '{fi[i].GetValue(parameterData).ToString()}'\r\n";
				}
			}
			return r;
		}

		static void Main(string[] args)
		{
			var persons = new ParameterData[]
			{
				new ParameterData("John", 30),
				new ParameterData("Peter", 40)
			};

			Console.WriteLine("Text from object:");

			foreach(var person in persons)
			{
				Console.WriteLine("-------------------------------------------");
				Console.WriteLine("Attibutes: ");
				Console.WriteLine(GetTextAttibutes(person));
			}

			Console.ReadKey();
		}
	}
}
