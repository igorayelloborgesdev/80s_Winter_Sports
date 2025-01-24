using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputIceHockey : IPlayerInput
    {
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0,
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeRR = null, CrossCountryOvertake crossCountryOvertakeRL = null)
        { }
        public void PlayAnimation(AnimationPlayer animationPlayer, int animID)
        { }
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
        public void Init(List<List<CrossCountryModel>> crossCountryModelAIList = null, int initLine = 0)
        { }
        public void Reset()
        { }
        public float GetSpeed()
        {
            return 0.0f;
        }
        public float GetMaxSpeed()
        {
            return 0.0f;
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList)
        { }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        { }
        public void SetCharacter(Character character)
        { }
        public bool GetPause()
        {
            return false;
        }
        public void SetSkiJumpPoint(int[] flyPoints)
        { }
        public void SetWindSkiJump(float angle, float power)
        { }
        public void SetSkiCollision(SkiCollision skiCollision)
        { }
        public float GetEnergy()
        {
            return 0.0f;
        }
        public bool GetIsFinished()
        {
            return false;
        }
        public bool GetIsAccel()
        {
            return false;
        }
        public bool GetIsBreak()
        {
            return false;
        }
        public void SetIsAI(bool isAI)
        { }
        public void SetCharacterIdCountry(int characterIdCountry)
        { }
        public int GetLinePosition()
        { return 0; }
        public bool GetIsAI()
        { return false; }
        public void OnlyPause()
        { }
        #endregion
    }
}
