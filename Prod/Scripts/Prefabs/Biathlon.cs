using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Static;

public partial class Biathlon : Node
{
    #region Exports
    [Export] private MeshInstance3D initPoint = null;    
    [Export] private Node3D[] pointsList1 = null;
    [Export] private Node3D[] pointsList2 = null;
    [Export] private Node3D[] pointsList3 = null;
    [Export] private Node3D[] pointsList4 = null;
    [Export] private Node3D[] pointsList5 = null;
    [Export] private PackedScene speed_collider = null;
    #endregion
    #region Variables    
    private List<List<SpeedSkatingTrackDTO>> biathlonTrackDTOList = new List<List<SpeedSkatingTrackDTO>>();
    private float multiplyPoint = 5.0f;
    private int startPointId = 0;
    private List<List<DirectionArrow>> directionArrowList = new List<List<DirectionArrow>>();
    private List<List<Node3D>> pointsList = new List<List<Node3D>>();
    #endregion
    #region Methods
    public MeshInstance3D GetSetInitPoint()
    {     
        return initPoint;
    }
    public void InstantiateRail()
    {
        pointsList.Add(pointsList1.ToList());
        
        int count = 0;
        int countId = 0;
        for (int a = 0; a < pointsList.Count; a++)
        {
            biathlonTrackDTOList.Add(new List<SpeedSkatingTrackDTO>());
            for (int i = 0; i < pointsList[a].Count; i += 3)
            {
                int points = (int)(RailLength(a, i) * multiplyPoint);
                for (int j = 0; j < points; j++)
                {
                    float t;
                    Vector3 position;

                    t = j / (points - 1.0f);
                    position = (1.0f - t) * (1.0f - t) * pointsList[a][i + 0].Position + 2.0f * (1.0f - t) * t * pointsList[a][i + 1].Position + t * t * pointsList[a][i + 2].Position;

                    biathlonTrackDTOList[a].Add(new SpeedSkatingTrackDTO()
                    {
                        position = position,
                        id = countId
                    });
                    countId++;
                }
                count++;
            }
        }
        for (int a = 0; a < biathlonTrackDTOList.Count; a++)
        {
            for (int i = 0; i < biathlonTrackDTOList[a].Count; i++)
            {
                biathlonTrackDTOList[a][i].distance = initPoint.Position.DistanceTo(biathlonTrackDTOList[a][i].position);
            }            
        }
        startPointId = biathlonTrackDTOList[0].OrderBy(x => x.distance).First().id;
        InstantiateSpeedCollider();
    }
    private float RailLength(int indexX, int indexY)
    {
        var chord = (pointsList[indexX][indexY + 2].Position - pointsList[indexX][indexY].Position).Length();
        var cont_net = (pointsList[indexX][indexY].Position - pointsList[indexX][indexY + 1].Position).Length() 
            + (pointsList[indexX][indexY + 2].Position - pointsList[indexX][indexY + 1].Position).Length();
        var app_arc_length = (cont_net + chord) / 2;
        return app_arc_length;
    }
    private void InstantiateSpeedCollider()
    {
        int count = 0;
        for (int a = 0; a < biathlonTrackDTOList.Count; a++)
        {
            directionArrowList.Add(new List<DirectionArrow>());
            for (int i = 0; i < biathlonTrackDTOList[a].Count; i++)
            {
                if (i % 30 == 0 && i != 0)
                {
                    var directionArrow = speed_collider.Instantiate<DirectionArrow>();
                    directionArrow.id = count;
                    Random rnd = new Random();
                    var direction = rnd.Next(1, 5);
                    directionArrow.direction = direction;
                    directionArrow.Position = biathlonTrackDTOList[a][i].position;
                    this.AddChild(directionArrow);
                    count++;
                    directionArrow.LookAtFromPosition(biathlonTrackDTOList[a][i].position, biathlonTrackDTOList[a][i - 1].position, Vector3.Up);
                    directionArrow.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                    directionArrow.GenerateBodyColor();
                    directionArrowList[a].Add(directionArrow);
                }
            }
            BiathlonStatic.arrowCount.Add(directionArrowList[a].Count);
        }
    }
    #endregion
    #region Get Set    
    public List<List<DirectionArrow>> GetDirectionArrowList
    {
        get
        {
            return directionArrowList;
        }
    }
    public List<List<SpeedSkatingTrackDTO>> GetBiathlonTrackDTOList
    {
        get
        {
            return biathlonTrackDTOList;
        }
    }
    public int GetStartPointId
    {
        get
        {
            return startPointId;
        }
    }
    #endregion
    //[Export] private RayCast3D rayCast3D = null;
    //[Export] private MeshInstance3D gunTarget = null;
    //private bool isTriggerPulled = false;
    //private float increment = 0.01f;

    //[Export] private MeshInstance3D[] gunTargetArray = null;

    //public override void _Process(double delta)
    //{        

    //    if (Input.IsKeyPressed(Key.Left))
    //    {
    //        var inc = gunTarget.Position.X - increment;
    //        gunTarget.Position = new Vector3(inc, gunTarget.Position.Y, gunTarget.Position.Z);
    //    }
    //    if (Input.IsKeyPressed(Key.Right))
    //    {
    //        var inc = gunTarget.Position.X + increment;
    //        gunTarget.Position = new Vector3(inc, gunTarget.Position.Y, gunTarget.Position.Z);
    //    }
    //    if (Input.IsKeyPressed(Key.Up))
    //    {
    //        var inc = gunTarget.Position.Y + increment;
    //        gunTarget.Position = new Vector3(gunTarget.Position.X, inc, gunTarget.Position.Z);
    //    }
    //    if (Input.IsKeyPressed(Key.Down))
    //    {
    //        var inc = gunTarget.Position.Y - increment;
    //        gunTarget.Position = new Vector3(gunTarget.Position.X, inc, gunTarget.Position.Z);
    //    }
    //    if (Input.IsKeyPressed(Key.A))
    //    {
    //        if (!isTriggerPulled)
    //        {
    //            var collider = rayCast3D.GetCollider() as Node;
    //            if (rayCast3D.IsColliding())
    //            {
    //                //GD.Print("Hit");
    //                if (collider is not null)
    //                {
    //                    //GD.Print(collider.Name);//<-
    //                    //GD.Print(rayCast3D.IsColliding());//<-
    //                    //GD.Print(rayCast3D.GetCollisionPoint());//<-
    //                }
    //            }
    //            else
    //            {
    //                //GD.Print("Miss");

    //                //GD.Print(gunTarget.Position);//<-
    //                var angle = Mathf.DegToRad(128.0f);
    //                var hyp = 0.1f;

    //                var cos = Math.Cos(angle);
    //                var ca = cos * hyp;
    //                var sin = Math.Sin(angle);
    //                var co = sin * hyp;

    //                gunTargetArray[1].Position = new Vector3(gunTarget.Position.X + (float)ca, gunTarget.Position.Y, gunTarget.Position.Z);

    //                GD.Print(ca);
    //                GD.Print(co);

    //            }
    //            isTriggerPulled = true;
    //        }
    //    }
    //    if (!Input.IsKeyPressed(Key.A) && isTriggerPulled)
    //    {
    //        isTriggerPulled = false;
    //        //GD.Print("Recharge");
    //    }
    //}
}
