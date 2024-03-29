using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class JoystickInput
{
    public static KeyObj GetJoyPressed()
    {
        if (Input.GetConnectedJoypads().Count > 0)
        {
            foreach (int joystickId in Input.GetConnectedJoypads())
            {
                if (Input.IsAnythingPressed())
                {
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.A)) { return new KeyObj() { keyName = "A", keyId = (int)JoyButton.A }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.B)) { return new KeyObj() { keyName = "B", keyId = (int)JoyButton.B }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.X)) { return new KeyObj() { keyName = "X", keyId = (int)JoyButton.X }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.Y)) { return new KeyObj() { keyName = "Y", keyId = (int)JoyButton.Y }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.DpadLeft)) { return new KeyObj() { keyName = "DpadLeft", keyId = (int)JoyButton.DpadLeft }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.DpadRight)) { return new KeyObj() { keyName = "DpadRight", keyId = (int)JoyButton.DpadRight }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.DpadUp)) { return new KeyObj() { keyName = "DpadUp", keyId = (int)JoyButton.DpadUp }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.DpadDown)) { return new KeyObj() { keyName = "DpadDown", keyId = (int)JoyButton.DpadDown }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.Back)) { return new KeyObj() { keyName = "Back", keyId = (int)JoyButton.Back }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.Start)) { return new KeyObj() { keyName = "Start", keyId = (int)JoyButton.Start }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.LeftShoulder)) { return new KeyObj() { keyName = "LeftShoulder", keyId = (int)JoyButton.LeftShoulder }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.RightShoulder)) { return new KeyObj() { keyName = "RightShoulder", keyId = (int)JoyButton.RightShoulder }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.LeftStick)) { return new KeyObj() { keyName = "LeftStick", keyId = (int)JoyButton.LeftStick }; };
                    if (Input.IsJoyButtonPressed(joystickId, JoyButton.RightStick)) { return new KeyObj() { keyName = "RightStick", keyId = (int)JoyButton.RightStick }; };
                    if (Input.GetJoyAxis(joystickId, JoyAxis.LeftX) != 0) { return new KeyObj() { keyName = "LeftX", keyId = (int)JoyAxis.LeftX }; };
                    if (Input.GetJoyAxis(joystickId, JoyAxis.LeftY) != 0) { return new KeyObj() { keyName = "LeftY", keyId = (int)JoyAxis.LeftY }; };
                }
            };
        }
        return null;
    }
}
