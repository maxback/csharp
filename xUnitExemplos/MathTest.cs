using Xunit;

namespace Codenation.Challenge
{
    public class MathTest
    {

        [Fact]
        public void Fibonacci_Test()
        {            
            var math = new Math();
            var result = math.Fibonacci();
            Assert.NotNull(result);     
        }

		[Fact]
		public void Fibonacci_Count_Test()
		{
			var math = new Math();
			var result = math.Fibonacci();
			Assert.Equal(14, result.Count);
		}

		[Fact]
		public void Fibonacci_Primeiros_Test()
		{
			var math = new Math();
			var result = math.Fibonacci();
			int i = 0;

			Assert.Equal(0, result[i++]);
			Assert.Equal(1, result[i++]);
			Assert.Equal(1, result[i++]);
			Assert.Equal(2, result[i++]);
			Assert.Equal(3, result[i++]);
			Assert.Equal(5, result[i++]);
			Assert.Equal(8, result[i++]);
			Assert.Equal(13, result[i++]);
			Assert.Equal(21, result[i++]);
			Assert.Equal(34, result[i++]);
			Assert.Equal(55, result[i++]);
			Assert.Equal(89, result[i++]);
			Assert.Equal(144, result[i++]);
			Assert.Equal(233, result[i++]);

		}

		[Fact]
        public void Is_Fibonacci_Test()
        {
            var math = new Math();
            Assert.True(math.IsFibonacci(5));
        }

        [Fact]
        public void Is_NotFibonacci_Test()
        {
            var math = new Math();
            Assert.False(math.IsFibonacci(4));
        }
    }
}
