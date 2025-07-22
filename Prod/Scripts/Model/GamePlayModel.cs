using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Model
{
    public class GamePlayModel
    {
        public Button goToMainMenu = null;
        public Button returnMenu = null;
        public Button resetMenu = null;
        public Button backMenuFinishButton = null;
        public Button returnFinishButton = null;
        public double bestTimeScore = 0.0;
        public double currentTimeScore = 0.0;
        public double biathlonCurrentTimeScore = 0.0;
        public int shootErrors = 0;
        public Button backMenuFinishButtonStandings = null;
        public Button returnFinishButtonStandings = null;
        public List<Button> iceHockeyFlagSelectButton = new List<Button>();
        public List<CountryDTO> iceHockeyCountry = new List<CountryDTO>();
        public List<CountryDTO> pot1 = new List<CountryDTO>();
        public List<CountryDTO> pot2 = new List<CountryDTO>();
        public List<CountryDTO> pot3 = new List<CountryDTO>();
        public List<List<Bracket>> bracketList = new List<List<Bracket>>();
        public int iceHockeyRound = 0;
    }
    public class Bracket
    {
        public int team1 = 0;
        public int team2 = 0;
        public int teamWinner = 0;
        public int id = -1;
        public int nextBracketId = -1;
    }

}
