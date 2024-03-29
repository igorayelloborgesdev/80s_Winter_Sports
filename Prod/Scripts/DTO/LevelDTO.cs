using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.DTO
{
    public class LevelObjDTO
    {
        public List<LevelDTO> levelList = new List<LevelDTO>();
    }
    public class LevelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string prefabName { get; set; }
        public int SubLevelId { get; set; }
    }
}
