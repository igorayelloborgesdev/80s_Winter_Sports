using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Static;

public partial class SpeedSkating : Node
{
    #region Exports
    [Export] private MeshInstance3D[] initPoint = null;
    [Export] private Node3D[] pointsList = null;
    [Export] private PackedScene speed_collider = null;
    #endregion
    #region Constants
    private Dictionary<int, int> laps = new Dictionary<int, int>() {
        { 0, 2 },
        { 1, 4 }
    };
    #endregion
    #region Variables
    private List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList = new List<SpeedSkatingTrackDTO>();
    private int levelId = 0;
    private int startPointId = 0;
    private float multiplyPoint = 5.0f;
    private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
    #endregion
    #region Methods
    public MeshInstance3D GetSetInitPoint(int id)
    {
        levelId = id;
        return initPoint[levelId];
    }
    public int GetLaps(int id)
    {
        return laps[id];
    }
    public void InstantiateRail()
    {
        int count = 0;
        int countId = 0;
        for (int i = 0; i < pointsList.Length; i += 3)
        {            
            int points = (int)(RailLength(i) * multiplyPoint);
            for (int j = 0; j < points; j++)
            {                
                float t;
                Vector3 position;

                t = j / (points - 1.0f);
                position = (1.0f - t) * (1.0f - t) * pointsList[i + 0].Position + 2.0f * (1.0f - t) * t * pointsList[i + 1].Position + t * t * pointsList[i + 2].Position;

                speedSkatingTrackDTOList.Add(new SpeedSkatingTrackDTO()
                {
                    position = position,
                    id = countId                    
                });
                countId++;
            }
            count++;
        }
        for (int i = 0; i < speedSkatingTrackDTOList.Count; i++)
        {
            speedSkatingTrackDTOList[i].distance = initPoint[levelId].Position.DistanceTo(speedSkatingTrackDTOList[i].position);
        }
        startPointId = speedSkatingTrackDTOList.OrderBy(x => x.distance).First().id;        
        InstantiateSpeedCollider();
    }
    private float RailLength(int index)
    {
        var chord = (pointsList[index + 2].Position - pointsList[index].Position).Length();
        var cont_net = (pointsList[index].Position - pointsList[index + 1].Position).Length() + (pointsList[index + 2].Position - pointsList[index + 1].Position).Length();
        var app_arc_length = (cont_net + chord) / 2;
        return app_arc_length;
    }
    private void InstantiateSpeedCollider()
    {
        int count = 0;
        for (int i = 0; i < speedSkatingTrackDTOList.Count; i++)
        {
            if (i % 30 == 0 && i != 0)
            {
                var directionArrow = speed_collider.Instantiate<DirectionArrow>();
                directionArrow.id = count;
                Random rnd = new Random();
                var direction = rnd.Next(1, 5);
                directionArrow.direction = direction;
                directionArrow.Position = speedSkatingTrackDTOList[i].position;
                this.AddChild(directionArrow);
                count++;
                directionArrow.LookAtFromPosition(speedSkatingTrackDTOList[i].position, speedSkatingTrackDTOList[i - 1].position,Vector3.Up);
                directionArrow.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                directionArrow.GenerateBodyColor();
                directionArrowList.Add(directionArrow);
            }            
        }
        SpeedSkatingStatic.arrowCount = directionArrowList.Count;
    }
    #endregion
    #region Get Set
    public int GetStartPointId
    { 
        get 
        { 
            return startPointId; 
        }
    }
    public List<SpeedSkatingTrackDTO> GetSpeedSkatingTrackDTOList
    {
        get
        {
            return speedSkatingTrackDTOList;
        }
    }
    public List<DirectionArrow> GetDirectionArrowList
    {
        get { 
            return directionArrowList;
        }
    }
    #endregion
}
