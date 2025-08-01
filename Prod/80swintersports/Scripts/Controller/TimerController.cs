using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Model;

namespace WinterSports.Scripts.Controller
{
    public class TimerController
    {
        #region Model
        private TimerModel timerModel = null;
        #endregion
        #region Methods
        public void Init()
        {
            timerModel = new TimerModel();
            timerModel.timer = 0.0;
            timerModel.states = TimerModel.States.Init;
        }
        public void TimerRunning(double delta)
        {
            if (timerModel is null)
                return;

            if (timerModel.states == TimerModel.States.Running)
            {
                timerModel.timer += delta;
            }
        }
        public double GetTimer()
        {
            if(timerModel is null)
                return 0.0;
            return timerModel.timer;
        }
        public void StartTimer()
        {
            if (timerModel.states == TimerModel.States.Init)
            {
                timerModel.states = TimerModel.States.Running;
            }
        }
        public void StopTimer()
        {
            if (timerModel.states == TimerModel.States.Running)
            {
                timerModel.states = TimerModel.States.Stop;
            }
        }
        public void UnstopTimer()
        {
            if (timerModel.states == TimerModel.States.Stop)
            {
                timerModel.states = TimerModel.States.Running;
            }
        }
        public void ResetTimer()
        {
            timerModel.timer = 0.0;
            timerModel.states = TimerModel.States.Init;
        }
        public void ResetTimerRunning()
        {
            timerModel.timer = 0.0;
        }
        #endregion
        #region Get
        public TimerModel GetTimerModel
        {
            get { return timerModel; }
        }
        #endregion
    }
}
