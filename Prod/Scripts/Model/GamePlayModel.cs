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
    }
}
