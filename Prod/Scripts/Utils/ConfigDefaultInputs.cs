using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigDefaultInputs
{
    #region Variables
    public static int keyboardJoystick = 0;
    public static List<KeyObj> keysControlArray = new List<KeyObj>();
    #endregion
    #region Methods
    public static void Init()
    {
        keysControlArray.Add(new KeyObj() { keyName = "Escape", keyId = 4194305 });
        keysControlArray.Add(new KeyObj() { keyName = "Up", keyId = 4194320 });
        keysControlArray.Add(new KeyObj() { keyName = "Down", keyId = 4194322 });
        keysControlArray.Add(new KeyObj() { keyName = "Left", keyId = 4194319 });
        keysControlArray.Add(new KeyObj() { keyName = "Right", keyId = 4194321 });
        keysControlArray.Add(new KeyObj() { keyName = "A", keyId = 65 });
        keysControlArray.Add(new KeyObj() { keyName = "S", keyId = 83 });
    }
    #endregion
}
