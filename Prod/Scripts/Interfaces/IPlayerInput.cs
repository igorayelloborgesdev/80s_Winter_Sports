using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;

namespace WinterSports.Scripts.Interfaces
{
    public interface IPlayerInput
    {
        void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f);
        void PlayAnimation(AnimationPlayer animationPlayer, int animID);        
        void SetCharacterBody3D(CharacterBody3D characterBody3D);
        void SetPauseScreen(Control pauseScreen);
        void SetFinishSessionScreen(Control finishSessionScreen);
        void Pause();
        void UnPause();
        void ShowHideFinishSessionScreen();
        void Init();
        void Reset();
        float GetSpeed();
        float GetMaxSpeed();
        void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList);
        void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList);
        void SetCharacter(Character character);
    }
}
