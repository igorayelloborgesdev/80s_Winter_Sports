using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Static;

namespace WinterSports.Scripts.Prefabs
{
    public partial class LugeCollision : Area3D
    {
        #region Behaviors
        private void OnBodyEntered(Node body)
        {
            try
            {
                if (body.Name.ToString().Trim().ToLower() == "speednormal")
                {
                    LugeStatic.statesLuge = LugeStatic.StatesLuge.trackNormal;
                }
                if (body.Name.ToString().Trim().ToLower() == "speedslow")
                {
                    LugeStatic.statesLuge = LugeStatic.StatesLuge.trackSlow;
                }
                if (body.Name.ToString().Trim().ToLower() == "speedhigh")
                {
                    LugeStatic.statesLuge = LugeStatic.StatesLuge.trackSpeed;
                }
                if (body.Name.ToString().Trim().ToLower() == "speedslowwall")
                {
                    LugeStatic.isColliding = true;
                }
                if (body.Name.ToString().Trim().ToLower() == "area3dfinish")
                {
                    LugeStatic.isEndRun = true;
                }                
            }
            catch (Exception ex) { }
        }
        private void OnBodyExited(Node body)
        {
            try
            {
                if (body.Name.ToString().Trim().ToLower() == "speedslowwall")
                {
                    LugeStatic.isColliding = false;
                }
            }
            catch (Exception ex) { }
        }
        public override void _Ready()
        {
            Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
            Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
        }
        #endregion
    }
}
