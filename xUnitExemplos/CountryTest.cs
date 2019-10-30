using System;
using Xunit;

namespace Codenation.Challenge
{
    public class CountryTest
    {

        [Fact]
        public void Should_Return_10_Itens_When_Get_Top_10_States()
        {            
            var states = new Country();
            var top = states.Top10StatesByArea();
            Assert.NotNull(top);
            Assert.Equal(10, top.Length);
        }

		[Fact]
		public void Should_Return_Top_10_States()
		{
			var states = new Country();
			var top = states.Top10StatesByArea();
			var Esperados  = new State[]
			{
				new State("Amazonas",           "AM", 1559159.148),
				new State("Pará",               "PA", 1247954.666),
				new State("Mato Grosso",        "MT", 903366.192),
				new State("Minas Gerais",       "MG", 586522.122),
				new State("Bahia",              "BA", 564733.177),
				new State("Mato Grosso do Sul", "MS", 357145.532),
				new State("Goiás",              "GO", 340111.783),
				new State("Maranhão",           "MA", 331937.450),
				new State("Rio Grande do Sul",  "RS", 281730.223),
				new State("Tocantins",          "TO", 277720.520)
			};

			for(int i=0; i<10; i++)
			{
				Assert.Equal(Esperados[i].Name, top[i].Name);
				Assert.Equal(Esperados[i].Acronym, top[i].Acronym);
				Assert.Equal(Esperados[i].Area, top[i].Area);
			}

		}


	}
}
