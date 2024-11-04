using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Static
{
    public static class SkiJumpStatic
    {
        #region variables     
        public static float impulseScore = 0.0f;
        public static List<float> fliyngScore = new List<float>();
        public static float landingScore = 0.0f;        
        #endregion
        #region Methods
        public static void Reset()
        {
            impulseScore = 0.0f;
            fliyngScore.Clear();
            landingScore = 0.0f;
        }
        #endregion
    }
}
