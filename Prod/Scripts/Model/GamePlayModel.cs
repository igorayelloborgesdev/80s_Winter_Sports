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
    }
}
