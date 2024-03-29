using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class ModalSimple
{
    #region Variables    
    public Control simpleModalControl { get; set; }
    public Label simpleModalTitleLabel { get; set; }
    public Label simpleModalContextLabel { get; set; }
    public Button simpleModalButton { get; set; }
    #endregion
    #region Get Set
    public Button GetSetSimpleModalButton
    {
        get 
        {
            return simpleModalButton;
        }
        set 
        { 
            simpleModalButton = value; 
        }
    }
    #endregion
    #region Builder    
    public ModalSimple SetModalBuilder(Control aSimpleModalControl, Label aSimpleModalTitleLabel, Label aSimpleModalContextLabel, Button aSimpleModalButton)
    {
        simpleModalControl = aSimpleModalControl;
        simpleModalTitleLabel = aSimpleModalTitleLabel;
        simpleModalContextLabel = aSimpleModalContextLabel;
        simpleModalButton = aSimpleModalButton;
        return this;
    }
    #endregion
    #region Events
    public void OpenModal()
    {
        simpleModalControl.Show();
    }
    public void CloseModal()
    {
        simpleModalControl.Hide();
    }
    #endregion
    #region Methods
    public void SetTitle(string title)
    {
        simpleModalTitleLabel.Text = title;
    }
    public void SetContext(string context)
    {
        simpleModalContextLabel.Text = context;
    }
    #endregion
}
