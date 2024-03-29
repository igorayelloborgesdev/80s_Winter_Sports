using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CountryObjDTO
{
    public List<CountryDTO> countryList = new List<CountryDTO>();
}

public class CountryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public Color HairColor { get; set; }
    public Color SkinColor { get; set; }
    public Color BootsColor { get; set; }
    public Color kit1BodyColor { get; set; }
    public Color kit1ArmsColor { get; set; }
    public Color kit1LegsColor { get; set; }
    public Color kit2BodyColor { get; set; }
    public Color kit2ArmsColor { get; set; }
    public Color kit2LegsColor { get; set; }
    public Color kit3BodyColor { get; set; }
    public Color kit3ArmsColor { get; set; }
    public Color kit3LegsColor { get; set; }
}
