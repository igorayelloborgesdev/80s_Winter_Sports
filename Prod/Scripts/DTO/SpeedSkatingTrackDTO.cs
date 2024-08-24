using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.DTO
{
    public class SpeedSkatingTrackDTO
    {
        public Godot.Vector3 position { get; set; }
        public int id { get; set; }        
        public float distance { get; set; }
        public bool isRamp { get; set; }
        public float incRamp { get; set; }
        public bool isLeftWall { get; set; }
    }
}
