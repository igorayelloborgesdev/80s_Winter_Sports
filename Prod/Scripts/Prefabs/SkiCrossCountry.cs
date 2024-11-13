using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WinterSports.Scripts.Model;

public partial class SkiCrossCountry : Node
{
    #region Behaviors
    public override void _Ready()
    {        
    }    
    #endregion
    #region Methos
    public List<CrossCountryModel> InstantiateArea3D()
    {
        List<CrossCountryModel> crossCountryModelList = new List<CrossCountryModel>();
        var child = this.GetChildren();
        var hillBaseList = child.Where(p => p.Name.ToString().Contains("HillBase")).ToList();
        int count = 0;
        foreach (MeshInstance3D hill in hillBaseList)
        {            
            CrossCountryModel crossCountryModel = new CrossCountryModel();
            crossCountryModel.id = count;
            crossCountryModel.position = hill.GlobalPosition;
            crossCountryModelList.Add(crossCountryModel);
            count++;             
        }        
        return crossCountryModelList;
    }
    #endregion
}
