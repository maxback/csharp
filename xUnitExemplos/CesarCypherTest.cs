using System;
using Xunit;

namespace Codenation.Challenge
{
    public class CesarCypherTest
    {
		[Fact]
		public void Should_Not_Accept_Argument_Out_Of_Range_When_Crypt()
		{
			var cypher = new CesarCypher();
			Assert.Throws<ArgumentOutOfRangeException>(() => cypher.Crypt("é palhaçada"));
		}

		//estourou ao submeter - faltava validar ao descriptografar também
	    [Fact]
		public void Should_Ensure_Letter_Or_Number_When_Decrypt()
		{
			var cypher = new CesarCypher();
			Assert.Throws<ArgumentOutOfRangeException>(() => cypher.Decrypt("é palhaçada"));
		}

		[Fact]
        public void Should_Not_Accept_Null_When_Crypt()
        {            
            var cypher = new CesarCypher();
            Assert.Throws<ArgumentNullException>(() => cypher.Crypt(null));
        }

        [Fact]
        public void Should_Keep_Numbers_Out_When_Crypt()
        {
            var cypher = new CesarCypher();
            Assert.Equal("d1e2f3g4h5i6j7k8l9m0", cypher.Crypt("a1b2c3d4e5f6g7h8i9j0"));
        }

		[Fact]
		public void Ignore_UpperCase_When_Crypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("def", cypher.Crypt("ABC"));
		}

		[Fact]
		public void Ignore_Spaces_When_Crypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("d e f", cypher.Crypt("a b c"));
		}

		[Fact]
		public void Ignore_Spaces_When_Decrypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("a b c", cypher.Decrypt("d e f"));
		}

		[Fact]
		public void Empty_Message_When_Crypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal(string.Empty, cypher.Crypt(string.Empty));
		}


		[Fact]
		public void Overflow_When_Crypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("zabc", cypher.Crypt("wxyz"));
		}

		[Fact]
		public void Underflow_When_Decrypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("wxyz", cypher.Decrypt("zabc"));
		}

		[Fact]
		public void Example_Crypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj", cypher.Crypt("the quick brown fox jumps over the lazy dog"));
		}

		[Fact]
		public void Example_Decrypt()
		{
			var cypher = new CesarCypher();
			Assert.Equal("the quick brown fox jumps over the lazy dog", cypher.Decrypt("wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj"));
		}


	}
}
