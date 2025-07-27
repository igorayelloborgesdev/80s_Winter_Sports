using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using WinterSports.Scripts.DTO;

public partial class LugeBobsleigh : Node
{
    #region Exports
    [Export] private MeshInstance3D[] initPoint = null;
    [Export] private Node3D[] pointsList1 = null;
    [Export] private Node3D[] pointsList2 = null;
    [Export] private Node3D[] pointsList3 = null;
    [Export] private Node3D[] pointsList4 = null;
    [Export] private Node3D[] pointsList5 = null;
    [Export] private Node3D[] pointsList6 = null;
    [Export] private Node3D[] pointsList7 = null;
    [Export] private Node3D[] pointsList8 = null;
    [Export] private Node3D[] pointsList9 = null;
    [Export] private Node3D[] pointsList10 = null;
    [Export] private Node3D[] pointsList11 = null;
    [Export] private Node3D[] pointsList12 = null;
    [Export] private Node3D[] pointsList13 = null;
    [Export] private Node3D[] pointsList14 = null;
    [Export] private Node3D[] pointsList15 = null;
    [Export] private Node3D[] pointsList16 = null;
    [Export] private Node3D[] pointsList17 = null;
    [Export] private Node3D[] pointsList18 = null;
    [Export] private Node3D[] pointsList19 = null;
    [Export] private Node3D[] pointsList20 = null;
    [Export] private Node3D[] pointsList21 = null;
    [Export] private Node3D[] pointsList22 = null;
    [Export] private Node3D[] pointsList23 = null;
    [Export] private Node3D[] pointsList24 = null;
    [Export] private Node3D[] pointsList25 = null;
    [Export] private Node3D[] pointsList26 = null;
    [Export] private Node3D[] pointsList27 = null;
    [Export] private Node3D[] pointsList28 = null;
    [Export] private Node3D[] pointsList29 = null;
    [Export] private Node3D[] pointsList30 = null;
    [Export] private Node3D[] pointsList31 = null;
    [Export] private Node3D[] pointsList32 = null;
    [Export] private Node3D[] pointsList33 = null;
    [Export] private Node3D[] pointsList34 = null;
    [Export] private Node3D[] pointsList35 = null;
    [Export] private Node3D[] pointsList36 = null;
    [Export] private Node3D[] pointsList37 = null;
    [Export] private Node3D[] pointsList38 = null;
    [Export] private Node3D[] pointsList39 = null;
    [Export] private Node3D[] pointsList40 = null;
    [Export] private Node3D[] pointsList41 = null;
    [Export] private Node3D[] pointsList42 = null;
    [Export] private Node3D[] pointsList43 = null;
    [Export] private Node3D[] pointsList44 = null;
    [Export] private Node3D[] gates = null;
    #endregion
    #region Variables
    private List<List<Node3D>> pointsList = new List<List<Node3D>>();
    private List<SpeedSkatingTrackDTO> lugeTrackDTOList = new List<SpeedSkatingTrackDTO>();
    private int startPointId = 0;    
    #endregion
    #region Constants
    private float multiplyPoint = 12.0f;
    #endregion
    #region Get Set
    public MeshInstance3D GetSetInitPoint(int id)
    {
        return initPoint[id];
    }
    #endregion

    //public override void _Ready()
    //{
    //    InstantiateRail();
    //}

    #region Methods
    public void InstantiateRail(int id)
    {
        pointsList.Add(pointsList1.ToList());
        pointsList.Add(pointsList2.ToList());
        pointsList.Add(pointsList3.ToList());
        pointsList.Add(pointsList4.ToList());
        pointsList.Add(pointsList5.ToList());
        pointsList.Add(pointsList6.ToList());
        pointsList.Add(pointsList7.ToList());
        pointsList.Add(pointsList8.ToList());
        pointsList.Add(pointsList9.ToList());
        pointsList.Add(pointsList10.ToList());
        pointsList.Add(pointsList11.ToList());
        pointsList.Add(pointsList12.ToList());
        pointsList.Add(pointsList13.ToList());
        pointsList.Add(pointsList14.ToList());
        pointsList.Add(pointsList15.ToList());
        pointsList.Add(pointsList16.ToList());
        pointsList.Add(pointsList17.ToList());
        pointsList.Add(pointsList18.ToList());
        pointsList.Add(pointsList19.ToList());
        pointsList.Add(pointsList20.ToList());
        pointsList.Add(pointsList21.ToList());
        pointsList.Add(pointsList22.ToList());
        pointsList.Add(pointsList23.ToList());
        pointsList.Add(pointsList24.ToList());
        pointsList.Add(pointsList25.ToList());
        pointsList.Add(pointsList26.ToList());
        pointsList.Add(pointsList27.ToList());
        pointsList.Add(pointsList28.ToList());
        pointsList.Add(pointsList29.ToList());
        pointsList.Add(pointsList30.ToList());
        pointsList.Add(pointsList31.ToList());
        pointsList.Add(pointsList32.ToList());
        pointsList.Add(pointsList33.ToList());
        pointsList.Add(pointsList34.ToList());
        pointsList.Add(pointsList35.ToList());
        pointsList.Add(pointsList36.ToList());
        pointsList.Add(pointsList37.ToList());
        pointsList.Add(pointsList38.ToList());
        pointsList.Add(pointsList39.ToList());
        pointsList.Add(pointsList40.ToList());
        pointsList.Add(pointsList41.ToList());
        pointsList.Add(pointsList42.ToList());
        pointsList.Add(pointsList43.ToList());
        pointsList.Add(pointsList44.ToList());

        int count = 0;
        int countId = 0;
        for (int a = 0; a < pointsList.Count; a++)
        {            
            for (int i = 0; i < pointsList[a].Count; i += 3)
            {
                int points = (int)(RailLength(a, i) * multiplyPoint);
                for (int j = 0; j < points; j++)
                {                    
                    float t;
                    Vector3 position;
                    t = j / (points - 1.0f);
                    position = (1.0f - t) * (1.0f - t) * pointsList[a][i + 0].Position + 2.0f * (1.0f - t) * t * pointsList[a][i + 1].Position + t * t * pointsList[a][i + 2].Position;
                    lugeTrackDTOList.Add(new SpeedSkatingTrackDTO()
                    {
                        position = position,
                        id = countId
                    });
                    countId++;                 
                }
                count++;
            }
        }
        for (int a = 0; a < lugeTrackDTOList.Count; a++)
        {            
            lugeTrackDTOList[a].distance = initPoint[id].Position.DistanceTo(lugeTrackDTOList[a].position);         
        }
        startPointId = lugeTrackDTOList.OrderBy(x => x.distance).First().id;        
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
    public void EnableDisableGate(int id)
    {        
        gates[id == 0 ? 1 : 0].Hide();
    }
    #endregion
    #region Get Set
    public int GetSetStartPointId
    {
        get
        {
            return startPointId;
        }
        set
        {
            startPointId = value;
        }
    }
    public List<SpeedSkatingTrackDTO> GetLugeTrackDTOList
    {
        get
        {
            return lugeTrackDTOList;
        }
    }    
    #endregion
}
