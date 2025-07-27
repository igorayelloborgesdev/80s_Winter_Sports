using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class KeyboardInput
{
    public static KeyObj GetKeyPressed()
    {
        if (Input.IsAnythingPressed())
        {
            if (Input.IsKeyPressed(Key.Up)) { return new KeyObj() { keyName = "Up", keyId = (int)Key.Up }; };
            if (Input.IsKeyPressed(Key.Down)) { return new KeyObj() { keyName = "Down", keyId = (int)Key.Down }; };            
            if (Input.IsKeyPressed(Key.Left)) { return new KeyObj() { keyName = "Left", keyId = (int)Key.Left }; };            
            if (Input.IsKeyPressed(Key.Right)) { return new KeyObj() { keyName = "Right", keyId = (int)Key.Right }; };            
            if (Input.IsKeyPressed(Key.Key0)) { return new KeyObj() { keyName = "Key0", keyId = (int)Key.Key0 }; };            
            if (Input.IsKeyPressed(Key.Key1)) { return new KeyObj() { keyName = "Key1", keyId = (int)Key.Key1 }; };            
            if (Input.IsKeyPressed(Key.Key2)) { return new KeyObj() { keyName = "Key2", keyId = (int)Key.Key2 }; };            
            if (Input.IsKeyPressed(Key.Key3)) { return new KeyObj() { keyName = "Key3", keyId = (int)Key.Key3 }; };            
            if (Input.IsKeyPressed(Key.Key4)) { return new KeyObj() { keyName = "Key4", keyId = (int)Key.Key4 }; };            
            if (Input.IsKeyPressed(Key.Key5)) { return new KeyObj() { keyName = "Key5", keyId = (int)Key.Key5 }; };            
            if (Input.IsKeyPressed(Key.Key6)) { return new KeyObj() { keyName = "Key6", keyId = (int)Key.Key6 }; };            
            if (Input.IsKeyPressed(Key.Key7)) { return new KeyObj() { keyName = "Key7", keyId = (int)Key.Key7 }; };            
            if (Input.IsKeyPressed(Key.Key8)) { return new KeyObj() { keyName = "Key8", keyId = (int)Key.Key8 }; };            
            if (Input.IsKeyPressed(Key.Key9)) { return new KeyObj() { keyName = "Key9", keyId = (int)Key.Key9 }; };
            if (Input.IsKeyPressed(Key.Kp0)) { return new KeyObj() { keyName = "Kp0", keyId = (int)Key.Kp0 }; };
            if (Input.IsKeyPressed(Key.Kp1)) { return new KeyObj() { keyName = "Kp1", keyId = (int)Key.Kp1 }; };
            if (Input.IsKeyPressed(Key.Kp2)) { return new KeyObj() { keyName = "Kp2", keyId = (int)Key.Kp2 }; };
            if (Input.IsKeyPressed(Key.Kp3)) { return new KeyObj() { keyName = "Kp3", keyId = (int)Key.Kp3 }; };
            if (Input.IsKeyPressed(Key.Kp4)) { return new KeyObj() { keyName = "Kp4", keyId = (int)Key.Kp4 }; };
            if (Input.IsKeyPressed(Key.Kp5)) { return new KeyObj() { keyName = "Kp5", keyId = (int)Key.Kp5 }; };
            if (Input.IsKeyPressed(Key.Kp6)) { return new KeyObj() { keyName = "Kp6", keyId = (int)Key.Kp6 }; };
            if (Input.IsKeyPressed(Key.Kp7)) { return new KeyObj() { keyName = "Kp7", keyId = (int)Key.Kp7 }; };
            if (Input.IsKeyPressed(Key.Kp8)) { return new KeyObj() { keyName = "Kp8", keyId = (int)Key.Kp8 }; };
            if (Input.IsKeyPressed(Key.Kp9)) { return new KeyObj() { keyName = "Kp9", keyId = (int)Key.Kp9 }; };
            if (Input.IsKeyPressed(Key.Enter)) { return new KeyObj() { keyName = "Enter", keyId = (int)Key.Enter }; };            
            if (Input.IsKeyPressed(Key.Escape)) { return new KeyObj() { keyName = "Escape", keyId = (int)Key.Escape }; };
            if (Input.IsKeyPressed(Key.Space)) { return new KeyObj() { keyName = "Space", keyId = (int)Key.Space }; };
            if (Input.IsKeyPressed(Key.A)) { return new KeyObj() { keyName = "A", keyId = (int)Key.A }; };            
            if (Input.IsKeyPressed(Key.B)) { return new KeyObj() { keyName = "B", keyId = (int)Key.B }; };            
            if (Input.IsKeyPressed(Key.C)) { return new KeyObj() { keyName = "C", keyId = (int)Key.C }; };            
            if (Input.IsKeyPressed(Key.D)) { return new KeyObj() { keyName = "D", keyId = (int)Key.D }; };            
            if (Input.IsKeyPressed(Key.E)) { return new KeyObj() { keyName = "E", keyId = (int)Key.E }; };            
            if (Input.IsKeyPressed(Key.F)) { return new KeyObj() { keyName = "F", keyId = (int)Key.F }; };            
            if (Input.IsKeyPressed(Key.G)) { return new KeyObj() { keyName = "G", keyId = (int)Key.G }; };            
            if (Input.IsKeyPressed(Key.H)) { return new KeyObj() { keyName = "H", keyId = (int)Key.H }; };            
            if (Input.IsKeyPressed(Key.I)) { return new KeyObj() { keyName = "I", keyId = (int)Key.I }; };            
            if (Input.IsKeyPressed(Key.J)) { return new KeyObj() { keyName = "J", keyId = (int)Key.J }; };            
            if (Input.IsKeyPressed(Key.K)) { return new KeyObj() { keyName = "K", keyId = (int)Key.K }; };            
            if (Input.IsKeyPressed(Key.L)) { return new KeyObj() { keyName = "L", keyId = (int)Key.L }; };            
            if (Input.IsKeyPressed(Key.M)) { return new KeyObj() { keyName = "M", keyId = (int)Key.M }; };            
            if (Input.IsKeyPressed(Key.N)) { return new KeyObj() { keyName = "N", keyId = (int)Key.N }; };            
            if (Input.IsKeyPressed(Key.O)) { return new KeyObj() { keyName = "O", keyId = (int)Key.O }; };            
            if (Input.IsKeyPressed(Key.P)) { return new KeyObj() { keyName = "P", keyId = (int)Key.P }; };            
            if (Input.IsKeyPressed(Key.Q)) { return new KeyObj() { keyName = "Q", keyId = (int)Key.Q }; };            
            if (Input.IsKeyPressed(Key.R)) { return new KeyObj() { keyName = "R", keyId = (int)Key.R }; };            
            if (Input.IsKeyPressed(Key.S)) { return new KeyObj() { keyName = "S", keyId = (int)Key.S }; };            
            if (Input.IsKeyPressed(Key.T)) { return new KeyObj() { keyName = "T", keyId = (int)Key.T }; };            
            if (Input.IsKeyPressed(Key.U)) { return new KeyObj() { keyName = "U", keyId = (int)Key.U }; };            
            if (Input.IsKeyPressed(Key.V)) { return new KeyObj() { keyName = "V", keyId = (int)Key.V }; };            
            if (Input.IsKeyPressed(Key.X)) { return new KeyObj() { keyName = "X", keyId = (int)Key.X }; };
            if (Input.IsKeyPressed(Key.W)) { return new KeyObj() { keyName = "W", keyId = (int)Key.W }; };            
            if (Input.IsKeyPressed(Key.Y)) { return new KeyObj() { keyName = "Y", keyId = (int)Key.Y }; };            
            if (Input.IsKeyPressed(Key.Z)) { return new KeyObj() { keyName = "Z", keyId = (int)Key.Z }; };            
        }                
        return null;
    }

}
