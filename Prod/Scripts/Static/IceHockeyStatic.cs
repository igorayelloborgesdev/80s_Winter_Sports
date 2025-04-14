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
            Init
        };
        public static StatesIceHockey statesIceHockey = StatesIceHockey.Select;
        #endregion
        #region Method
        public static void Reset()
        {
            statesIceHockey = StatesIceHockey.Select;
        }
        #endregion
    }
}
