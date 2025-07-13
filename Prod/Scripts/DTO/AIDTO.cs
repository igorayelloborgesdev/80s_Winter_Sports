using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.DTO
{
    public class AIDTO
    {
        public List<AIObjDTO> aiObjDTOList = new List<AIObjDTO>();
        public void SaveAI(AIDTO aiDTO)
        {
            SaveLoad.SaveData<AIDTO>(aiDTO, "Data//AIInit.json");
        }
    }
    public class AIObjDTO
    {
        public int id { get; set; }
        public double score { get; set; }
    }
}
