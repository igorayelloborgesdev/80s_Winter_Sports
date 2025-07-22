using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Model
{
    public class GamesModel
    {
        public List<Button> buttonsGame = new List<Button>();
        public List<List<TextureRect>> flagGame = new List<List<TextureRect>>();
        public List<List<Label>> flagGameLabel = new List<List<Label>>();
        public List<Button> gamesButton = new List<Button>();
        public List<CountryMedalTableModel> countryMedalTableModel = new List<CountryMedalTableModel>();
        public List<Label> sportPosLabel = new List<Label>();
        public List<TextureRect> countryTextureRect = new List<TextureRect>();
        public List<Label> sportCodeLabel = new List<Label>();
        public List<Label> sportGoldLabel = new List<Label>();
        public List<Label> sportSilverLabel = new List<Label>();
        public List<Label> sportBronzeLabel = new List<Label>();
        public List<Label> sportTotalLabel = new List<Label>();        
    }
    public class CountryMedalTableModel
    {
        public int id = 0;
        public int gold = 0;
        public int silver = 0;
        public int bronze = 0;
        public int total = 0;
    }
}
