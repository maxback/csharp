using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTestAttributes
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class AdditionalInfoAttribute : Attribute
	{
		public string Text { get; set; }
		public AdditionalInfoAttribute(string text)
		{
			Text = text;
		}

		public AdditionalInfoAttribute()
		{
			Text = string.Empty;
		}

	}
}
