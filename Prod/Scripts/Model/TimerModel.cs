using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Model
{
    public class TimerModel
    {
        #region Variables
        public double timer = 0.0;
        public enum States
        {
            Init,
            Running,
            Stop            
        };
        public States states = States.Init;
        #endregion
    }
}
