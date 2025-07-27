using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WinterSports.Scripts.Model;

public partial class Ski : Node
{
    #region Exports
    [Export] private GateStartFinish[] gateStart = null;
    [Export] private GateStartFinish[] gateFinish = null;
    [Export] private MeshInstance3D[] initPoint = null;
    [Export] private Node3D[] gates = null;
    [Export] private MeshInstance3D[] initPointCrossCountry = null;
    [Export] private SkiCrossCountry skiCrossCountry = null;
    [Export] private SkiCrossCountryAI skiCrossCountryAI = null;
    #endregion
    #region Behaviors
    public override void _Ready()
    {
        
    }
    #endregion
    #region Methods
    public void ShowGate(int id)
    {
        int count = 0;
        foreach (var gate in gates)
        {
            if (gate is not null)
            {
                if (count == id)
                {
                    gates[id].SetProcess(true);
                    gates[id].Show();
                }
                else
                {
                    gate.ProcessMode = ProcessModeEnum.Disabled;
                    gate.SetProcess(false);
                    gate.Hide();
                }                
            }
            count++;
        }        
    }    
    public GateStartFinish GetStart(int id)
    {        
        return gateStart[id];
    }
    public GateStartFinish GetFinish(int id)
    {        
        return gateFinish[id];
    }
    public MeshInstance3D GetInitPoint(int id)
    {
        return initPoint[id];
    }
    public MeshInstance3D GetInitPointCrossCountry(int id)
    {
        return initPointCrossCountry[id];
    }
    public void HideGates()
    {
        foreach (var gate in gates)
        {
            if (gate is not null)
            {
                gate.ProcessMode = ProcessModeEnum.Disabled;
                gate.SetProcess(false);
                gate.Hide();                
            }            
        }
    }
    public void ShowGateStartFinish()
    {        
        for (int i = 0; i < gateStart.Length - 1; i++)
        {
            if (gateStart[i] is not null)
            {
                gateStart[i].ProcessMode = ProcessModeEnum.Disabled;
                gateStart[i].SetProcess(false);
                gateStart[i].Hide();             
            }            
        }
        for (int i = 0; i < gateFinish.Length - 1; i++)            
        {
            if (gateFinish[i] is not null)
            {
                gateFinish[i].ProcessMode = ProcessModeEnum.Disabled;
                gateFinish[i].SetProcess(false);
                gateFinish[i].Hide();         
            }         
        }
        
    }
    public void ShowHideCrossCountry(bool isShow, int id)
    {        
        if (isShow)
        {
            gateStart[id].Show();
            gateFinish[id].Show();
            gateFinish[id].SetProcess(true);
            gateStart[id].SetProcess(true);
        }
        else 
        {
            gateFinish[id].ProcessMode = ProcessModeEnum.Disabled;
            gateFinish[id].SetProcess(false);            
            gateFinish[id].Hide();
            gateStart[id].ProcessMode = ProcessModeEnum.Disabled;
            gateStart[id].SetProcess(false);
            gateStart[id].Hide();
        }
    }
    public List<CrossCountryModel> GetCrossCountryModel()
    {
        return skiCrossCountry.InstantiateArea3D();
    }
    public List<CrossCountryModel> GetCrossCountryAIModel(string parentName)
    {
        return skiCrossCountryAI.InstantiateArea3D(parentName);
    }
    #endregion
}
