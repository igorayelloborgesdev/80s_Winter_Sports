using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WinterSports.Scripts.DTO;

public partial class SkiJump : Node
{
    #region Exports
    [Export] private Node3D track = null;
    [Export] private MeshInstance3D initPoint = null;
    #endregion
    #region Variables
    private List<List<Node3D>> pointsList = new List<List<Node3D>>();
    private List<SpeedSkatingTrackDTO> skiJumpTrackDTOList = new List<SpeedSkatingTrackDTO>();
    private int startPointId = 0;
    #endregion
    #region Constants
    private const float multiplyPoint = 12.0f;
    private int[] movePoints = new int[] { 8, 9 };
    private Vector3[] movePointsVector = new Vector3[] { Vector3.Zero, Vector3.Zero };
    private const float maxImpulse = 2.0f;
    private int[] flyPoints = new int[] { 0, 0, 0 };
    #endregion
    #region Behaviors
    public override void _Ready()
    {
        InitRailPosition();
    }
    #endregion
    #region Methods
    public void InstantiateRail(float impulseRail)
    {
        pointsList.Clear();
        skiJumpTrackDTOList.Clear();

        var impulse = (impulseRail * maxImpulse) / 100.0f;

        track.GetChild<Node3D>(movePoints[0]).GlobalPosition = 
            new Vector3(movePointsVector[0].X + impulse, movePointsVector[0].Y, movePointsVector[0].Z
            );
        track.GetChild<Node3D>(movePoints[1]).GlobalPosition =
            new Vector3(movePointsVector[1].X + impulse, movePointsVector[1].Y, movePointsVector[1].Z);
        
        var childCount = track.GetChildCount();
        for (int i = 0; i < childCount/ 3; i++)
        {
            var pointListInside = new List<Node3D>();
            for (int j = 0; j < 3; j++)
            {                
                pointListInside.Add(track.GetChild<Node3D>(((i * 3) + j)));
            }            
            pointsList.Add(pointListInside);            
        }

        int count = 0;
        int countId = 0;
        for (int a = 0; a < pointsList.Count; a++)
        {
            for (int i = 0; i < pointsList[a].Count; i += 3)
            {
                int points = (int)(RailLength(a, i) * multiplyPoint);
                if (a == 2)//Flight
                {
                    flyPoints[0] = skiJumpTrackDTOList.Count + 1;
                    flyPoints[1] = skiJumpTrackDTOList.Count + (points/2);
                    flyPoints[2] = skiJumpTrackDTOList.Count + points;                    
                }
                for (int j = 0; j < points; j++)
                {                 
                    float t;
                    Vector3 position;
                    t = j / (points - 1.0f);
                    position = (1.0f - t) * (1.0f - t) * pointsList[a][i + 0].Position + 2.0f * (1.0f - t) * t * pointsList[a][i + 1].Position + t * t * pointsList[a][i + 2].Position;
                    skiJumpTrackDTOList.Add(new SpeedSkatingTrackDTO()
                    {
                        position = position,
                        id = countId
                    });
                    countId++;                    
                }                
                count++;
            }
        }
        for (int a = 0; a < skiJumpTrackDTOList.Count; a++)
        {
            skiJumpTrackDTOList[a].distance = initPoint.Position.DistanceTo(skiJumpTrackDTOList[a].position);
        }
        startPointId = skiJumpTrackDTOList.OrderBy(x => x.distance).First().id;
    }
    private float RailLength(int indexX, int indexY)
    {
        var chord = (pointsList[indexX][indexY + 2].Position - pointsList[indexX][indexY].Position).Length();
        var cont_net = (pointsList[indexX][indexY].Position - pointsList[indexX][indexY + 1].Position).Length()
            + (pointsList[indexX][indexY + 2].Position - pointsList[indexX][indexY + 1].Position).Length();
        var app_arc_length = (cont_net + chord) / 2;
        return app_arc_length;
    }
    private void InstatiateRefBox(Vector3 position, int id)
    {
        var myMesh = new MeshInstance3D();
        myMesh.Mesh = new BoxMesh();
        myMesh.Name = "Mesh" + id.ToString();
        this.AddChild(myMesh);
        myMesh.Scale = myMesh.Scale * 0.1f;
        myMesh.Position = position;
    }
    private void InitRailPosition()
    {
        movePointsVector[0] = new Vector3(
                track.GetChild<Node3D>(movePoints[0]).GlobalPosition.X,
                track.GetChild<Node3D>(movePoints[0]).GlobalPosition.Y,
                track.GetChild<Node3D>(movePoints[0]).GlobalPosition.Z
                );
        movePointsVector[1] = new Vector3(
            track.GetChild<Node3D>(movePoints[1]).GlobalPosition.X,
            track.GetChild<Node3D>(movePoints[1]).GlobalPosition.Y,
            track.GetChild<Node3D>(movePoints[1]).GlobalPosition.Z
            );
    }
    #endregion
    #region Get Set
    public MeshInstance3D GetSetInitPoint()
    {
        return initPoint;
    }
    public int GetStartPointId
    { 
        get 
        { 
            return startPointId; 
        }
    }
    public List<SpeedSkatingTrackDTO> GetSkiJumpTrackDTOList
    {
        get
        {
            return skiJumpTrackDTOList;
        }
    }
    public int[] GetFlyPoints
    { 
        get 
        { 
            return flyPoints; 
        }
    }
    #endregion
}
