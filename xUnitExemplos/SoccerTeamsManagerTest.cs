using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Codenation.Challenge.Exceptions;

namespace Codenation.Challenge
{
    public class SoccerTeamsManagerTest
    {      
        [Fact]
        public void Should_Be_Unique_Ids_For_Teams()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
            Assert.Throws<UniqueIdentifierException>(() =>
                manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2"));
        }

		[Fact]
		public void Should_Be_Valid_Team_When_Add_Player()
		{
			var manager = new SoccerTeamsManager();
			Assert.Throws<TeamNotFoundException>(() =>
				manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1000));
		}

		[Fact]
		public void Should_Be_Unique_Ids_For_Player()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1000);
			Assert.Throws<UniqueIdentifierException>(() =>
				manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1000));
		}


		[Fact]
		public void Should_Be_Valid_Player_When_Set_Team_Captain()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddPlayer(1, 1, "Jogador 1", DateTime.Today, 0, 0);
			manager.SetCaptain(1);
			Assert.Equal(1, manager.GetTeamCaptain(1));
			Assert.Throws<PlayerNotFoundException>(() =>
				manager.SetCaptain(2));
		}

		[Fact]
		public void Should_Be_Unique_Player_As_Team_Captain()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddTeam(2, "Time 2", DateTime.Now, "cor 3", "cor 4");
			manager.AddPlayer(1, 1,"Jogador 1", DateTime.Today, 0, 0);
			manager.AddPlayer(2, 1, "Jogador 2", DateTime.Today, 0, 0);
			manager.AddPlayer(3, 2, "Jogador 3", DateTime.Today, 0, 0);
			manager.AddPlayer(4, 2, "Jogador 4", DateTime.Today, 0, 0);

			manager.SetCaptain(1);
			Assert.Equal(1, manager.GetTeamCaptain(1));

			manager.SetCaptain(2);
			Assert.Equal(2, manager.GetTeamCaptain(1));

			manager.SetCaptain(3);
			Assert.Equal(2, manager.GetTeamCaptain(1));
			Assert.Equal(3, manager.GetTeamCaptain(2));

		}

		[Fact]
		public void Should_Be_Captain_Found()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1000);
			Assert.Throws<CaptainNotFoundException>(() =>
				manager.GetTeamCaptain(1));
		}

		[Fact]
		public void Should_Be_Player_Exists_When_Get_Name()
		{
			var manager = new SoccerTeamsManager();
			Assert.Throws<PlayerNotFoundException>(() =>
				manager.GetPlayerName(1));
		}

		[Fact]
		public void Should_Be_Player_Name()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1000);

			Assert.Equal("Player 1", manager.GetPlayerName(1));
		}

		[Fact]
		public void Should_Be_Team_Exists_When_Get_Name()
		{
			var manager = new SoccerTeamsManager();
			Assert.Throws<TeamNotFoundException>(() =>
				manager.GetTeamName(1));
		}

		[Fact]
		public void Should_Be_Team_Name()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1000);

			Assert.Equal("Time 1", manager.GetTeamName(1));
		}

		[Fact]
        public void Should_Ensure_Sort_Order_When_Get_Team_Players()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");

            var playersIds = new List<long>() {15, 2, 33, 4, 13};
            for(int i = 0; i < playersIds.Count(); i++)
                manager.AddPlayer(playersIds[i], 1, $"Jogador {i}", DateTime.Today, 0, 0);

            playersIds.Sort();
            Assert.Equal(playersIds, manager.GetTeamPlayers(1));
        }

        [Theory]
        [InlineData("10,20,300,40,50", 2)]
        [InlineData("50,240,3,1,50", 1)]
        [InlineData("10,22,24,3,24", 2)]
        public void Should_Choose_Best_Team_Player(string skills, int bestPlayerId)
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");

            var skillsLevelList = skills.Split(',').Select(x => int.Parse(x)).ToList();
            for(int i = 0; i < skillsLevelList.Count(); i++)
                manager.AddPlayer(i, 1, $"Jogador {i}", DateTime.Today, skillsLevelList[i], 0);

            Assert.Equal(bestPlayerId, manager.GetBestTeamPlayer(1));
        }

		[Theory]
		[InlineData("01/01/2004,01/01/2003,01/01/2002,01/01/2001,01/01/2000", 4)]
		[InlineData("01/01/2004,01/01/2000,01/01/2000,01/01/2003,01/01/2002", 1)]
		[InlineData("01/01/2005,01/01/2003,01/01/2000,01/01/2003,01/01/2005", 2)]
		public void Should_Choose_Older_Team_Player(string skills, int olderPlayerId)
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");

			var bdList = skills.Split(',').Select(x => DateTime.Parse(x)).ToList();
			for (int i = 0; i < bdList.Count(); i++)
				manager.AddPlayer(i, 1, $"Jogador {i}", bdList[i],1, 0);

			Assert.Equal(olderPlayerId, manager.GetOlderTeamPlayer(1));
		}


		[Fact]
		public void Should_Ensure_Sort_Order_When_Get_Teams()
		{
			var manager = new SoccerTeamsManager();

			var teamIds = new List<long>() { 15, 2, 33, 4, 13 };
			for (int i = 0; i < teamIds.Count(); i++)
				manager.AddTeam(teamIds[i], $"Time {i}", DateTime.Today, "cor 1", "cor 2");

			teamIds.Sort();
			Assert.Equal(teamIds, manager.GetTeams());
		}


		[Theory]
		[InlineData("1000,2000,3000,400,500", 2)]
		[InlineData("5000,2400,3,1000,5100", 4)]
		[InlineData("10000,22000,22000,3000,2400", 1)]
		public void Should_Choose_Best_HigherSalary_Player(string skills, int bestPlayerId)
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");

			var salaryLevelList = skills.Split(',').Select(x => decimal.Parse(x)).ToList();
			for (int i = 0; i < salaryLevelList.Count(); i++)
				manager.AddPlayer(i, 1, $"Jogador {i}", DateTime.Today, 1, salaryLevelList[i]);

			Assert.Equal(bestPlayerId, manager.GetHigherSalaryPlayer(1));
		}

		[Fact]
		public void Should_Be_Player_Salary()
		{
			var manager = new SoccerTeamsManager();
			manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
			manager.AddPlayer(1, 1, "Player 1", DateTime.Now, 1, 1001);
			manager.AddPlayer(2, 1, "Player 2", DateTime.Now, 1, 1000);

			Assert.Equal(1001, manager.GetPlayerSalary(1));
		}

		
		[Theory]
		[InlineData("1,2,3,4,5|6,7,8,9,10|11,12,13,14,15", 2, "14,13")]
		[InlineData("1,2,3,4,5|6,7,8,9,15|15,12,13,14,10", 3, "9,10,13")]
		[InlineData("1,20,3,4,50|6,70,8,9,10|11,12,13,14,15", 4, "6,4,1,14")]
		public void Should_Choose_Top_Team_Player(string treamskillsstr, int top, string topstr)
		{
			long id = 0;
			var manager = new SoccerTeamsManager();
			var teamSkills = treamskillsstr.Split("|");
			for (int i = 0; i < teamSkills.Count(); i++)
			{
				manager.AddTeam(i, $"Time {i}", DateTime.Now, "cor 1", "cor 2");

				var skillsLevelList = teamSkills[i].Split(',').Select(x => int.Parse(x)).ToList();
				for (int j = 0; j < skillsLevelList.Count(); j++)
					manager.AddPlayer(id++, i, $"Jogador {id}", DateTime.Today, skillsLevelList[j], 0);
			}
			var ltp = manager.GetTopPlayers(top);
			var topstrobtido = string.Join(",", ltp);

			Assert.Equal(topstr, topstrobtido);
		}


		[Theory]
        [InlineData("Azul;Vermelho", "Azul;Amarelo", "Amarelo")]
        [InlineData("Azul;Vermelho", "Amarelo;Laranja", "Amarelo")]
        [InlineData("Azul;Vermelho", "Azul;Vermelho", "Vermelho")]
        public void Should_Choose_Right_Color_When_Get_Visitor_Shirt_Color(string teamColors, string visitorColors, string visitorMatchColor)
        {
            long teamId = 1;
            long visitorTeamId = 2;
            var teamColorList = teamColors.Split(";");
            var visitorColorList = visitorColors.Split(";");

            var manager = new SoccerTeamsManager();
            manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, teamColorList[0], teamColorList[1]);
            manager.AddTeam(visitorTeamId, $"Time {visitorTeamId}", DateTime.Now, visitorColorList[0], visitorColorList[1]);

            Assert.Equal(visitorMatchColor, manager.GetVisitorShirtColor(teamId, visitorTeamId));
        }
    }
}
