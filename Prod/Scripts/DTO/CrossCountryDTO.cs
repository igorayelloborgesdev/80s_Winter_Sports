using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.DTO
{
    public class CrossCountryObjDTO
    {
        public List<CrossCountryDTO> CrossCountryDTOList = new List<CrossCountryDTO>();
    }
    public class CrossCountryDTO
    {
        public int id { get; set; }
        public Godot.Vector3 position { get; set; }
        public float speed { get; set; }
        public bool isAccel { get; set; }
        public bool isBreak { get; set; }
    }
}
