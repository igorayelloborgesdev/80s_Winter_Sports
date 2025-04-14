using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Model;
using static WinterSports.Scripts.Events.PlayerInputIceHockey;

namespace WinterSports.Scripts.Interfaces
{
    public interface IPlayerInput
    {
        void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0, 
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeRR = null, CrossCountryOvertake crossCountryOvertakeRL = null);
        void PlayAnimation(AnimationPlayer animationPlayer, int animID);        
        void SetCharacterBody3D(CharacterBody3D characterBody3D);
        void SetPauseScreen(Control pauseScreen);
        void SetFinishSessionScreen(Control finishSessionScreen);
        void Pause();
        void UnPause();
        void ShowHideFinishSessionScreen();
        void Init(List<List<CrossCountryModel>> crossCountryModelAIList = null, int initLine = 0);
        void Reset();
        float GetSpeed();
        float GetMaxSpeed();
        void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList);
        void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList);
        void SetCharacter(Character character);
        bool GetPause();
        void SetSkiJumpPoint(int[] flyPoints);
        void SetWindSkiJump(float angle, float power);
        void SetSkiCollision(SkiCollision skiCollision);
        float GetEnergy();
        bool GetIsFinished();
        bool GetIsAccel();
        bool GetIsBreak();
        void SetIsAI(bool isAI);
        void SetCharacterIdCountry(int characterIdCountry);
        int GetLinePosition();
        bool GetIsAI();
        void OnlyPause();
        void SetIceHockeyGoals(IceHockeyGoal Goal1, IceHockeyGoal Goal2);
        void SetIceHockeyTeams(List<Character> iceHockeyTeam1, List<Character> iceHockeyTeam2);
        void SetPuck(RigidBody3D puck);
        void SetisSelected(ref bool isSelected);
        void SetisPuckControl(ref bool isPuckControl);
        bool GetisSelected();
        bool GetisPuckControl();
        InputShoot GetinputShoot();

        void SetObj<T>(T obj);
    }
}
