using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Prefabs
{
    public partial class GateCollision : Area3D
    {
        #region Const
        private string bodyNameActivateTrigger = "Character";
        #endregion
        #region Variables
        private bool isCollided = false;
        #endregion
        #region Behaviors
        private void OnBodyEntered(Node body)
        {            
            if (body.Name.ToString().Trim().ToLower() == bodyNameActivateTrigger.Trim().ToLower()) 
            {
                isCollided = true;
                SkiStatic.isCollided = true;
            }            
        }
        // This method connects the body_entered signal
        public override void _Ready()
        {            
            Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));            
        }
        #endregion
        #region Method
        public void Reset()
        {
            isCollided = false;
        }
        public bool GetIsCollided()
        {
            return isCollided;
        }
        #endregion
    }
}
