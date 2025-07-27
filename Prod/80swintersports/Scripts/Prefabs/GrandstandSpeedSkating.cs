using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GrandstandSpeedSkating : Node3D
{
    #region Exports
    [Export] private PackedScene supporter = null;
    [Export] private Node3D centerLookAt = null;
    [Export] private Node3D[] pointsList1 = null;
    [Export] private Node3D[] pointsList2 = null;
    [Export] private Node3D[] pointsList3 = null;
    [Export] private Node3D[] pointsList4 = null;
    [Export] private Node3D[] pointsList5 = null;
    [Export] private Node3D[] pointsList6 = null;
    [Export] private Node3D[] pointsList7 = null;
    [Export] private Node3D[] pointsList8 = null;

    [Export] private Node3D[] pointsCurveList1 = null;
    [Export] private Node3D[] pointsCurveList2 = null;
    [Export] private Node3D[] pointsCurveList3 = null;
    [Export] private Node3D[] pointsCurveList4 = null;
    [Export] private Node3D[] pointsCurveList5 = null;
    [Export] private Node3D[] pointsCurveList6 = null;
    [Export] private Node3D[] pointsCurveList7 = null;
    [Export] private Node3D[] pointsCurveList8 = null;
    [Export] private Node3D[] pointsCurveList9 = null;
    [Export] private Node3D[] pointsCurveList10 = null;
    [Export] private Node3D[] pointsCurveList11 = null;
    [Export] private Node3D[] pointsCurveList12 = null;
    [Export] private Node3D[] pointsCurveList13 = null;
    [Export] private Node3D[] pointsCurveList14 = null;
    [Export] private Node3D[] pointsCurveList15 = null;
    [Export] private Node3D[] pointsCurveList16 = null;
    #endregion
    #region Variables
    public float width = 0.2f;
    public int numberOfPoints = 50;
    public int numberOfPointsCurve = 28;
    private List<Node3D[]> pointsList = new List<Node3D[]>();
    private List<Node3D[]> pointsCurveList = new List<Node3D[]>();
    private List<Supporter> supportersList = new List<Supporter>();
    #endregion
    #region Behavior
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Init();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    #endregion
    #region Methods
    private void Init()
    {
        AddPathReference();
        InstantiateSupporters(pointsList, numberOfPoints, false);
        InstantiateSupporters(pointsCurveList, numberOfPointsCurve, true);
    }
    private void AddPathReference()
    {
        pointsList.Add(pointsList1);
        pointsList.Add(pointsList2);
        pointsList.Add(pointsList3);
        pointsList.Add(pointsList4);
        pointsList.Add(pointsList5);
        pointsList.Add(pointsList6);
        pointsList.Add(pointsList7);
        pointsList.Add(pointsList8);

        pointsCurveList.Add(pointsCurveList1);
        pointsCurveList.Add(pointsCurveList2);
        pointsCurveList.Add(pointsCurveList3);
        pointsCurveList.Add(pointsCurveList4);
        pointsCurveList.Add(pointsCurveList5);
        pointsCurveList.Add(pointsCurveList6);
        pointsCurveList.Add(pointsCurveList7);
        pointsCurveList.Add(pointsCurveList8);
        pointsCurveList.Add(pointsCurveList9);
        pointsCurveList.Add(pointsCurveList10);
        pointsCurveList.Add(pointsCurveList11);
        pointsCurveList.Add(pointsCurveList12);
        pointsCurveList.Add(pointsCurveList13);
        pointsCurveList.Add(pointsCurveList14);
        pointsCurveList.Add(pointsCurveList15);
        pointsCurveList.Add(pointsCurveList16);
    }

    private void InstantiateSupporters(List<Node3D[]> aPointsList, int aNumberOfPoints, bool isCurve)
    {
        foreach (var pointsObj in aPointsList)
        {
            float t;
            Vector3 position;
            for (int i = 0; i < aNumberOfPoints; i++)
            {
                t = i / (aNumberOfPoints - 1.0f);
                position = (1.0f - t) * (1.0f - t) * pointsObj[0].Position + 2.0f * (1.0f - t) * t * pointsObj[1].Position + t * t * pointsObj[2].Position;
                supportersList.Add(supporter.Instantiate<Supporter>());
                this.AddChild(supportersList[supportersList.Count - 1]);
                supportersList[supportersList.Count - 1].Position = new Vector3(position.X, position.Y, position.Z);
                Vector3 positionLookAt = Vector3.Zero;
                if (isCurve)
                    positionLookAt = new Vector3(centerLookAt.Position.X, supportersList[supportersList.Count - 1].Position.Y, centerLookAt.Position.Z);
                else
                    positionLookAt = new Vector3(centerLookAt.Position.X, supportersList[supportersList.Count - 1].Position.Y, supportersList[supportersList.Count - 1].Position.Z);
                supportersList[supportersList.Count - 1].LookAt(positionLookAt, Vector3.Up, true);                
                Random rnd = new Random();
                var colorId = rnd.Next(0, supportersList[supportersList.Count - 1].GetColorCount);
                supportersList[supportersList.Count - 1].GenerateBodyColor(colorId);
                supportersList[supportersList.Count - 1].Name = i.ToString();
            }
        }

    }
    #endregion
}
