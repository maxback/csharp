using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Codenation.Challenge
{

	public class FieldTestAddition1
	{
		[Add]
		private decimal val1 = 10;
		[Add]
		private decimal val2 = 20;

		private decimal val3 = 30;

		public decimal expected = 30;
	}

	public class FieldTestAddition2
	{
		[Add]
		private decimal val1 = -10;
		[Add]
		private decimal val2 = 20;
		private decimal val3 = -30;

		public decimal expected = 10;
	}
	public class FieldTestSubtraction1
	{
		[Subtract]
		private decimal val1 = 10;
		[Subtract]
		private decimal val2 = 20;
		private decimal val3 = 30;

		public decimal expected = -30;
	}

	public class FieldTestSubtraction2
	{
		[Subtract]
		private decimal val1 = -10;
		[Subtract]
		private decimal val2 = 20;
		private decimal val3 = -30;

		public decimal expected = -10;
	}

	public class FieldTestTotal
	{
		[Add]
		private decimal val1 = -10;
		[Add]
		private decimal val2 = 20;
		private decimal val3 = -30;

		[Subtract]
		private decimal val4 = -10;

		[Subtract]
		private decimal val5 = 20;
		private decimal val6 = 50;

		public decimal expected = 0;
	}

	public class FieldCalculatorTest
    {


		const string CLASS_FULL_NAME = "Codenation.Challenge.FieldCalculator";
        const string INTERFACE_FULL_NAME = "Codenation.Challenge.ICalculateField";
        const string ASSEMBLY_NAME = "Source";
        const string ADDITION_METHOD = "Addition";
        const string SUBTRACTION_METHOD = "Subtraction";
        const string TOTAL_METHOD = "Total";

        /// When a class C implements an interface I, to find the method MC on class that 
        /// correspond the method MI of that interface, you must use the GetInterfaceMap
        private MethodInfo GetImplementationMethod(Type sourceInterface, Type sourceClass, string methodName)
        {
            var map = sourceClass.GetInterfaceMap(sourceInterface);
            var methodIndex = map.InterfaceMethods.ToList().FindIndex(x => x.Name == methodName);            
            return map.TargetMethods[methodIndex];
        }

        [Fact]
        public void Should_Exists_The_Class()
        {
            var assembly = Assembly.Load(ASSEMBLY_NAME);
            var expected = assembly.GetType(CLASS_FULL_NAME);
            Assert.NotNull(expected);
        }

        [Fact]
        public void Should_Implements_ICalculateField_Interface()
        {
            var assembly = Assembly.Load(ASSEMBLY_NAME);            
            var actual = assembly.GetType(CLASS_FULL_NAME);
            Assert.NotNull(actual);
            var interfaces = actual.GetInterfaces().Select(x => x.FullName).ToList();
            Assert.Contains(INTERFACE_FULL_NAME, interfaces);
        }

		[Theory]
		[InlineData(ADDITION_METHOD)]
		[InlineData(SUBTRACTION_METHOD)]
		[InlineData(TOTAL_METHOD)]
		public void Should_Implements_Methods_By_Name(string methodName)
		{
			var assembly = Assembly.Load(ASSEMBLY_NAME);
			var actual = assembly.GetType(CLASS_FULL_NAME);
			Assert.NotNull(actual);

			var methods = actual.GetMethods().Select(x => x.Name).ToList();
			Assert.Contains(methodName, methods);
		}

		[Theory]
		[InlineData(ADDITION_METHOD)]
		[InlineData(SUBTRACTION_METHOD)]
		[InlineData(TOTAL_METHOD)]
		public void Should_Implements_Methods_Of_Interface(string methodName)
		{

			var mi = GetImplementationMethod(typeof(ICalculateField), typeof(FieldCalculator), methodName);

			Assert.NotNull(mi);

		}

		[Fact]
		public void Should_Do_Addition1()
		{
			var obj = new FieldTestAddition1();
			ICalculateField calculator = new FieldCalculator();

			decimal result = calculator.Addition(obj);

			Assert.Equal(obj.expected, result);
		}

		[Fact]
		public void Should_Do_Addition2()
		{
			var obj = new FieldTestAddition2();
			ICalculateField calculator = new FieldCalculator();

			decimal result = calculator.Addition(obj);

			Assert.Equal(obj.expected, result);
		}

		[Fact]
		public void Should_Do_Subtract1()
		{
			var obj = new FieldTestSubtraction1();
			ICalculateField calculator = new FieldCalculator();

			decimal result = calculator.Subtraction(obj);

			Assert.Equal(obj.expected, result);
		}

		[Fact]
		public void Should_Do_Subtract2()
		{
			var obj = new FieldTestSubtraction2();
			ICalculateField calculator = new FieldCalculator();

			decimal result = calculator.Subtraction(obj);

			Assert.Equal(obj.expected, result);
		}

		[Fact]
		public void Should_Do_Total()
		{
			var obj = new FieldTestTotal();
			ICalculateField calculator = new FieldCalculator();

			decimal result = calculator.Total(obj);

			Assert.Equal(obj.expected, result);
		}

	}
}