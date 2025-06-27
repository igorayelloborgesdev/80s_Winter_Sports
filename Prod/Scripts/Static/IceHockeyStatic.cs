using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Static
{
    public static class IceHockeyStatic
    {
        #region variables
        public enum StatesIceHockey
        {
            Select,
            Init,
            Goal
        };
        public static StatesIceHockey statesIceHockey = StatesIceHockey.Select;
        public static bool isGoal = false;
        public static int score1 = 0;
        public static int score2 = 0;
        #endregion
        #region Method
        public static void Reset()
        {
            statesIceHockey = StatesIceHockey.Select;
            isGoal = false;
            score1 = 0;
            score2 = 0;
        }
        #endregion
    }
}
