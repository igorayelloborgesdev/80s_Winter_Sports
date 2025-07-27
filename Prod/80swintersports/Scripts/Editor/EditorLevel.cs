using Godot;
using System;

public partial class EditorLevel : Control
{
    [Export] private Color bootsColor;
    [Export] private Color[] hairColor;
    [Export] private Color[] skinColor;

    [Export] private Color[] kit1BodyColor;
    [Export] private Color[] kit1ArmsColor;
    [Export] private Color[] kit1LegsColor;

    [Export] private Color[] kit2BodyColor;    
    [Export] private Color[] kit2LegsColor;

    [Export] private Color[] kit3BodyColor;    
    [Export] private Color[] kit3LegsColor;
    public override void _Ready()
    {
        //DataGenerator.CreateCountryData(hairColor, skinColor, bootsColor
        //    ,kit1BodyColor, kit1ArmsColor, kit1LegsColor, kit2BodyColor, 
        //    kit2LegsColor, kit3BodyColor, kit3LegsColor);
        //DataGenerator.CreateLevelData();
        //DataGenerator.CreateAIData();        
    }
}
