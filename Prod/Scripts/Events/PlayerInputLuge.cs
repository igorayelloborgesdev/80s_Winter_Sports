using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using static WinterSports.Scripts.Events.PlayerInputIceHockey;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputLuge : IPlayerInput
    {
        #region Constant        
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"Luge_Idle"},
            {2,"Luge_Run"},
            {3,"Bobsleigh_Idle"},
            {4,"Bobsleigh_Run"},
            {5,"Bobsleigh_Jump"}
        };
        #endregion
        #region Variables
        private bool isPause = false;
        private Control finishSessionScreen = null;
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0,
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeFR = null, CrossCountryOvertake crossCountryOvertakeFL = null) 
        { }
        public void PlayerInputAI(AnimationPlayer animationPlayer, double delta = 0.0f)
        {

        }
        public void PlayAnimation(AnimationPlayer animationPlayer, int animID)
        {            
            animationPlayer.Play(animName[animID]);
        }
        public void SetCharacterBody3D(CharacterBody3D characterBody3D)
        { }
        public void SetPauseScreen(Control pauseScreen)
        { }
        public void SetFinishSessionScreen(Control finishSessionScreen)
        {
            this.finishSessionScreen = finishSessionScreen;
        }
        public void Pause()
        { }
        public void UnPause()
        { }
        public void ShowHideFinishSessionScreen()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHideFinishSessionScreenMenu(isPause);
        }
        public void Init(List<List<CrossCountryModel>> crossCountryModelAIList = null, int initLine = 0)
        { }
        public void Reset()
        { }
        public float GetSpeed()
        { return 0.0f; }
        public float GetMaxSpeed()
        { return 0.0f; }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList)
        { }
        public void SetSkiJumpPoint(int[] flyPoints) { }
        public void SetWindSkiJump(float angle, float power) { }
        public void SetSkiCollision(SkiCollision skiCollision) { }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        { }
        public void SetCharacter(Character character)
        { }
        public bool GetPause()
        {
            return isPause;
        }
        public float GetEnergy()
        {
            return 0.0f;
        }
        public bool GetIsFinished() { return false; }
        public void SetIsAI(bool isAI) { }
        public void SetCharacterIdCountry(int characterIdCountry) { }
        public int GetLinePosition()
        {
            return 0;
        }
        public bool GetIsAI()
        {
            return false;
        }
        public void OnlyPause()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;            
        }
        #endregion
        #region Methods
        private void ShowHideFinishSessionScreenMenu(bool isPause)
        {
            if (isPause)
                finishSessionScreen.Show();
            else
                finishSessionScreen.Hide();
        }
        public bool GetIsAccel() { return false; }
        public bool GetIsBreak() { return false; }
        public void SetIceHockeyGoals(IceHockeyGoal Goal1, IceHockeyGoal Goal2)
        {

        }
        public void SetIceHockeyTeams(ref List<Character> iceHockeyTeam1, ref List<Character> iceHockeyTeam2) { }
        public void SetPuck(RigidBody3D puck) { }
        public void SetisSelected(ref bool isSelected) { }
        public void SetisPuckControl(ref bool isPuckControl) { }
        public bool GetisSelected() { return false; }
        public bool GetisPuckControl() { return false; }
        public InputShoot GetinputShoot()
        {
            return InputShoot.None;
        }
        public void SetObj<T>(T obj)
        {
         
        }
        public void SetIsPlayerTeamPlayerNumber(bool isPlayerTeam, int playerNumber)
        {

        }
        public RigidBody3D GetPuck()
        {
            return null;
        }
        public void ResetControlAndSelected()
        { }
        #endregion
    }
}
