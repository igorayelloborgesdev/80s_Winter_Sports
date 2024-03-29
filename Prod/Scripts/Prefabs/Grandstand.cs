using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

public partial class Grandstand : Node3D
{
	#region Exports
	[Export] private PackedScene supporter = null;
    [Export] private Node3D[] pointsList1 = null;
    [Export] private Node3D[] pointsList2 = null;
    [Export] private Node3D[] pointsList3 = null;
    [Export] private Node3D[] pointsList4 = null;
    [Export] private Node3D[] pointsList5 = null;
    [Export] private Node3D[] pointsList6 = null;
    [Export] private Node3D[] pointsList7 = null;
    [Export] private Node3D[] pointsList8 = null;
    #endregion
    #region Variables
    public float width = 0.2f;
    public int numberOfPoints = 30;
    private List<Node3D[]> pointsList = new List<Node3D[]>();
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
        InstantiateSupporters();
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
    }

    private void InstantiateSupporters()
    {
        foreach(var pointsObj in pointsList)
        {
            float t;
            Vector3 position;
            for (int i = 0; i < numberOfPoints; i++)
            {
                t = i / (numberOfPoints - 1.0f);
                position = (1.0f - t) * (1.0f - t) * pointsObj[0].Position + 2.0f * (1.0f - t) * t * pointsObj[1].Position + t * t * pointsObj[2].Position;
                supportersList.Add(supporter.Instantiate<Supporter>());
                this.AddChild(supportersList[supportersList.Count - 1]);
                supportersList[supportersList.Count - 1].Position = new Vector3(position.X, position.Y, position.Z);
                Random rnd = new Random();
                var colorId = rnd.Next(0, supportersList[supportersList.Count - 1].GetColorCount);
                supportersList[supportersList.Count - 1].GenerateBodyColor(colorId);
                supportersList[supportersList.Count - 1].Name = i.ToString();        
            }
        }

    }
    #endregion
}
