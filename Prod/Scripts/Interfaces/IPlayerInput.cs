using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Interfaces
{
    public interface IPlayerInput
    {
        void PlayerInput(AnimationPlayer animationPlayer);
        void PlayAnimation(AnimationPlayer animationPlayer, int animID);        
        void SetCharacterBody3D(CharacterBody3D characterBody3D);
        void SetPauseScreen(Control pauseScreen);
        void Pause();
        void UnPause();
        void Init();
        void Reset();
        float GetSpeed();
        float GetMaxSpeed();
    }
}
