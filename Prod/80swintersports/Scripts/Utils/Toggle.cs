using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Toggle
{
    #region Variables
    public int idSelected = 0;
    public List<Button> buttonsList = null;
    public string colorNormal = "009bf0";
    public string colorSelected = "e7a706";
    #endregion
    #region Builder
    public void SetButtonsListBuilder(List<Button> aButtonsList)
    { 
        buttonsList = aButtonsList; 
    }
    #endregion
    #region Methods
    public void ToggleButton(int id)
    {
        foreach (var buttonObj in buttonsList)
        {
            buttonObj.GetChild<NinePatchRect>(0).Modulate = new Color(colorNormal);
        }
        buttonsList[id].GetChild<NinePatchRect>(0).Modulate = new Color(colorSelected);
    }    
    #endregion
}
