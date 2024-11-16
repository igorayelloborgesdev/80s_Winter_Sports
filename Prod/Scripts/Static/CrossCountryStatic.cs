using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Static
{
    public static class CrossCountryStatic
    {
        #region variables 
        public static bool isPause = false;
        #endregion
        #region Methods
        public static void Reset()
        {
            isPause = false;
        }
        #endregion
    }
}
