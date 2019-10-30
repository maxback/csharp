using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Codenation.Challenge
{
    public class FIFACupStatsTest
    {

		[Fact]
		public void Shoud_Map_Fieds_From_First_Line()
		{
			int id, name, full_name, age;
			var cup = new FIFACupStats();
			cup.MapFields("ID,name,full_name,age");
			Assert.True(cup.FieldPositions.TryGetValue("ID", out id));					Assert.Equal(0, id);
			Assert.True(cup.FieldPositions.TryGetValue("name", out name));				Assert.Equal(1, name);
			Assert.True(cup.FieldPositions.TryGetValue("full_name", out full_name));	Assert.Equal(2, full_name);
			Assert.True(cup.FieldPositions.TryGetValue("age", out age));				Assert.Equal(3, age);
		}

		[Fact]
		public void Shoud_Read_Objects_With_Fields_Selecteds()
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes("1,Max,Max Back,40\r\n2,Eric,Eric Back,15\r\n3,Thomas,Thomas Back,2\r\n");
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			string fields = "ID,name,full_name,age";
			cup.MapFields(fields);

			var regs = cup.ReadRegisters(sourceReader, fields.Split(','), FIFACupStats.ALL);
			Assert.Equal(3, regs.Count);
			Assert.Equal("1", regs[0].ID);
			Assert.Equal("Max Back", regs[0].full_name);
			Assert.Equal("40", regs[0].age);

			Assert.Equal("2", regs[1].ID);
			Assert.Equal("Eric Back", regs[1].full_name);
			Assert.Equal("15", regs[1].age);


			Assert.Equal("3", regs[2].ID);
			Assert.Equal("Thomas Back", regs[2].full_name);
			Assert.Equal("2", regs[2].age);
		}


		[Fact]
		public void Shoud_Read_Only_2_First_Objects()
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes("1,Max,Max Back,40\r\n2,Eric,Eric Back,15\r\n3,Thomas,Thomas Back,2\r\n");
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			string fields = "ID,name,full_name,age";
			cup.MapFields(fields);

			var regs = cup.ReadRegisters(sourceReader, fields.Split(','), 2);
			Assert.Equal(2, regs.Count);

			Assert.Equal("1", regs[0].ID);
			Assert.Equal("Max Back", regs[0].full_name);
			Assert.Equal("40", regs[0].age);

			Assert.Equal("2", regs[1].ID);
			Assert.Equal("Eric Back", regs[1].full_name);
			Assert.Equal("15", regs[1].age);
		}

		[Theory]
		[InlineData("ID,name,nationality\r\n1,aaa,Brasil\r\n2,bbb,Argentina\r\n3,ccc,Brasil\r\n4,ddd,Uruguai", 3)]
		[InlineData("ID,name,nationality\r\n1,aaa,Brasil\r\n2,bbb,\r\n3,ccc,Brasil\r\n4,ddd,Uruguai", 2)]
		[InlineData("nationality\r\n1\r\n2\r\n3\r\n4\r\n5\r\n5\r\n5\r\n5\r\n1\r\n6\r\n7\r\n8\r\n8\r\n8\r\n9\r\n10\r\n1\r\n1", 10)]
		public void Shoud_read_Distinct_Count_Nationality_From_memory_Stream(string content, int expected)
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes(content);
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			var count = cup.NationalityDistinctCountFromStream(sourceReader);
			Assert.Equal(expected, count);
		}

		[Fact]
		public void Shoud_read_Distinct_Count_Nationality_From_File()
		{
			var cup = new FIFACupStats();
			var count = cup.NationalityDistinctCount();
			Assert.Equal(164, count);
		}


		[Theory]
		[InlineData("ID,name,club,nationality\r\n1,aaa,Vasco,Brasil\r\n2,bbb,Avai,Argentina\r\n3,ccc,,Brasil\r\n4,ddd,Vasco,Uruguai", 2)]
		[InlineData("ID,club\r\n1,aaa\r\n2,bbb\r\n3,ccc\r\n4,ddd", 4)]
		[InlineData("club\r\n1\r\n2\r\n3\r\n4\r\n5\r\n5\r\n5\r\n5\r\n1\r\n6\r\n7\r\n8\r\n8\r\n8\r\n9\r\n10\r\n1\r\n1", 10)]
		public void Shoud_read_Distinct_Count_Club_From_memory_Stream(string content, int expected)
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes(content);
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			var count = cup.ClubDistinctCountFromStream(sourceReader);
			Assert.Equal(expected, count);
		}


		[Fact]
		public void Shoud_read_Distinct_Count_Club_From_File()
		{
			var cup = new FIFACupStats();
			var count = cup.ClubDistinctCount();
			Assert.Equal(647, count);
		}


		[Fact]
		public void Shoud_Return_20_Itens_When_Get_Top_Players_From_String()
		{
			var cup = new FIFACupStats();
			string content = "full_name\r\n1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10\r\n11\r\n12\r\n13\r\n14\r\n15\r\n16\r\n17\r\n18\r\n19\r\n20\r\n21\r\n22";
			byte[] byteArray = Encoding.ASCII.GetBytes(content);
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			var fullNames = cup.First20PlayersFromStream(sourceReader);
			Assert.Equal(20, fullNames.Count);
			for(int i=1; i<=20; i++)
			{
				Assert.Equal(i.ToString(), fullNames[i-1]);
			}
			
		}



		[Fact]
		public void Shoud_Return_20_Itens_When_Get_Top_Players()
		{
			var expected = new List<string>()
			{
				"C. Ronaldo dos Santos Aveiro",
				"Lionel Messi",
				"Neymar da Silva Santos Jr.",
				"Luis Suárez",
				"Manuel Neuer",
				"Robert Lewandowski",
				"David De Gea Quintana",
				"Eden Hazard",
				"Toni Kroos",
				"Gonzalo Higuaín",
				"Sergio Ramos García",
				"Kevin De Bruyne",
				"Thibaut Courtois",
				"Alexis Sánchez",
				"Luka Modrić",
				"Gareth Bale",
				"Sergio Agüero",
				"Giorgio Chiellini",
				"Gianluigi Buffon",
				"Paulo Dybala"
			};

			var cup = new FIFACupStats();
			var topPlayers = cup.First20Players();
			Assert.NotNull(topPlayers);
			Assert.Equal(20, topPlayers.Count);

			for (int i = 0; i < 20; i++)
			{
				Assert.Equal(expected[i], topPlayers[i]);
			}
		}

		[Theory]
		[InlineData("full_name,eur_release_clause\r\nE,6\r\nC,8\r\nA,10\r\nI,2\r\nH,3\r\nB,9\r\nJ,1\r\nG,4\r\nK,0\r\nD,7\r\nL,-1\r\nF,5",
			  10, "A,B,C,D,E,F,G,H,I,J")]
		public void Shoud_Return_10_Itens_When_Get_Top_Players_By_Release_Clause_From_Memopry_Stream(string content, int expected, string expectedNames)
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes(content);
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			var players = cup.Top10PlayersByReleaseClauseFromStream(sourceReader);
			Assert.Equal(expected, players.Count);

			var names = expectedNames.Split(',');

			for (int i = 0; i < expected; i++)
			{
				Assert.Equal(names[i], players[i]);
			}
			
		}

		[Fact]
        public void Shoud_Return_10_Itens_When_Get_Top_Players_By_Release_Clause()
        {
            var cup = new FIFACupStats();
            var topPlayers = cup.Top10PlayersByReleaseClause();
            Assert.NotNull(topPlayers);
            Assert.Equal(10, topPlayers.Count);
        }

		[Theory]
		[InlineData("full_name,birth_date,eur_wage\r\nA,1999-02-20,1000.0\r\nC,1999-02-22,2000.0\r\nB,1999-02-21,1000.0\r\nD,1999-02-22,1000.0\r\nE,1999-08-20,1000.0\r\nF,2000-02-20,1000.0\r\nG,2000-03-20,1000.0\r\nH,2001-01-01,1000.0\r\nJ,2001-02-02,1000.0\r\nI,2001-01-10,1000.0\r\nL,2002-08-10,1000.0\r\nK,2002-01-01,1000.0", 10,
			  "A,B,C,D,E,F,G,H,I,J,K,L")]
		[InlineData("full_name,birth_date,eur_wage\r\na,1900-01-01,\r\nd,2000-12-01,\r\nc,1950-07-01,\r\nb,1900-08-01,", 4, "a,b,c,d")]
		public void Shoud_Return_10_Itens_When_Get_Top_Players_By_Age_From_memory_Stream(string content, int expected, string expectedNames)
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes(content);
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			var players = cup.Top10PlayersByAgeFromStream(sourceReader);
			Assert.Equal(expected, players.Count);

			var names = expectedNames.Split(',');

			for (int i=0; i< expected; i++)
			{
				Assert.Equal(names[i], players[i]);
			}
		}

		[Fact]
        public void Shoud_Return_10_Itens_When_Get_Top_Players_By_Age()
        {
            var cup = new FIFACupStats();
            var topPlayers = cup.Top10PlayersByAge();
            Assert.NotNull(topPlayers);
            Assert.Equal(10, topPlayers.Count);
        }

		
		[Theory]
		[InlineData("age\r\n1\r\n2\r\n3", 3, "1,1|2,1|3,1")]
		[InlineData("age\r\n1\r\n2\r\n3\r\n1", 3, "1,2|2,1|3,1")]
		[InlineData("age\r\n1\r\n2\r\n3\r\n1\r\n2\r\n2\r\n3\r\n3\r\n3", 3, "1,2|2,3|3,4")]
		[InlineData("age\r\n1\r\n2\r\n3\r\n1\r\n2\r\n2\r\n3\r\n3\r\n3\r\n4\r\n5", 5, "1,2|2,3|3,4|4,1|5,1")]
		public void Shoud_Return_Map_Of_Ages_From_memory_Stream(string content, int expected, string expectedMap)
		{
			var cup = new FIFACupStats();

			byte[] byteArray = Encoding.ASCII.GetBytes(content);
			var source = new MemoryStream(byteArray);
			var sourceReader = new StreamReader(source);

			var map = cup.AgeCountMapFromStream(sourceReader);
			Assert.Equal(expected, map.Count);

			var map_items = expectedMap.Split('|');

			for (int i = 0; i < expected; i++)
			{
				string[] map_item = map_items[i].Split(',');
				int value;
				//test Key (age)
				Assert.True(map.TryGetValue(int.Parse(map_item[0]), out value));
				//test Value (count)
				Assert.Equal(map_item[1], value.ToString());
			}
		}

		[Fact]
		public void Shoud_Return_Map_Of_Ages()
		{
			string expectedMap = "16,18|17,270|18,682|19,1088|20,1252|21,1275|22,1324|23,1395|24,1321|25,1515|26,1199|27,1153|28,1053|29,1127|30,807|31,666|32,506|33,610|34,271|35,188|36,137|37,69|38,38|39,18|40,4|41,3|43,2|44,2|47,1|";
			var cup = new FIFACupStats();

			var map = cup.AgeCountMap();

			string str_mapa = "";
			int value;
			foreach (var key in map.Keys)
			{
				if (map.TryGetValue(key, out value))
				{
					str_mapa += key.ToString() + ',' + value.ToString() + '|';
				}
			}

			Assert.Equal(expectedMap, str_mapa);

		}

	}
}
