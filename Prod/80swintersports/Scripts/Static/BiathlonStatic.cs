using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Static
{
    public static class BiathlonStatic
    {
        #region variables                
        public static int direction = 0;
        public static bool isCollided = false;
        public static int id = 0;
        public static int idY = 0;
        public static int currentIndex = 0;
        public static bool isNotScore = false;
        public static bool isShooting = false;
        public static bool isLapFinished = false;
        #endregion
        #region Methods
        public static void Reset()
        {            
            direction = 0;
            isCollided = false;
            id = 0;
            idY = 0;
            currentIndex = 0;
            isNotScore = false;
            isShooting = false;
            isLapFinished = false;
        }
        #endregion
    }
}
