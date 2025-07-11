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
    }
}
