using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Static
{
    public static class LugeStatic
    {
        #region variables    
        public enum StatesLuge
        {
            trackNormal,
            trackSlow,
            trackSpeed            
        };
        public static StatesLuge statesLuge = StatesLuge.trackNormal;
        public static bool isColliding = false;
        public static bool isSpeedNormal = false;
        public static bool isEndRun = false;
        #endregion
        #region Methods
        public static void Reset()
        {
            statesLuge = StatesLuge.trackNormal;
            isColliding = false;
            isSpeedNormal = false;
            isEndRun = false;
        }
        #endregion
    }
}
