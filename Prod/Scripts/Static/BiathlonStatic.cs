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
        public static List<int> arrowCount = new List<int>();        
        #endregion
        #region Methods
        public static void Reset()
        {
            arrowCount = new List<int>();
        }
        #endregion
    }
}
