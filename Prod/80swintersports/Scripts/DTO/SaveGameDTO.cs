using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Singleton;

namespace WinterSports.Scripts.DTO
{
    public class SaveGameDTO
    {
        public int id {  get; set; }
        public int difficult { get; set; }
        public int country { get; set; }
        public string dateTime { get; set; }

        public List<SportSingleton> sportSingleton = new List<SportSingleton>();        
        
    }
}
