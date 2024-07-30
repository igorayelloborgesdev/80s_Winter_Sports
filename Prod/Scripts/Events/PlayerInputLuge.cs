using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputLuge : IPlayerInput
    {
        #region Constant        
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"Luge_Idle"},
            {2,"Luge_Run"}
        };
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f) 
        { }
        public void PlayAnimation(AnimationPlayer animationPlayer, int animID)
        {            
            animationPlayer.Play(animName[animID]);
        }
        public void SetCharacterBody3D(CharacterBody3D characterBody3D)
        { }
        public void SetPauseScreen(Control pauseScreen)
        { }
        public void SetFinishSessionScreen(Control finishSessionScreen)
        { }
        public void Pause()
        { }
        public void UnPause()
        { }
        public void ShowHideFinishSessionScreen()
        { }
        public void Init()
        { }
        public void Reset()
        { }
        public float GetSpeed()
        { return 0.0f; }
        public float GetMaxSpeed()
        { return 0.0f; }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList)
        { }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        { }
        public void SetCharacter(Character character)
        { }
        #endregion
    }
}
