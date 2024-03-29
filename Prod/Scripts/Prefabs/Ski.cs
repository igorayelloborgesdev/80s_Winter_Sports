using Godot;
using System;
using System.Linq;

public partial class Ski : Node
{
    #region Exports
    [Export] private GateStartFinish[] gateStart = null;
    [Export] private GateStartFinish[] gateFinish = null;
    [Export] private MeshInstance3D[] initPoint = null;
    [Export] private Node3D[] gates = null;
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
    #endregion
}
