using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Static
{
    public static class SpeedSkatingStatic
    {
        #region variables
        public static bool isCollided = false;
        public static int direction = 0;
        public static int id = 0;
        public static int currentIndex = 0;
        public static int arrowCount = 0;
        public static bool resetArrowCount = false;
        public static bool isNotScore = false;
        public static bool isLapFinished = false;
        #endregion
        #region Methods
        public static void Reset()
        {
            isCollided = false;
            direction = 0;
            id = 0;
            currentIndex = 0;            
            resetArrowCount = false;
            isNotScore = false;
            isLapFinished = false;
        }
        #endregion
    }
}
