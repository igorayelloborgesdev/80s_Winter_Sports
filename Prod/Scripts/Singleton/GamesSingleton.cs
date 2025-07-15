using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterSports.Scripts.Singleton
{
    public static class GamesSingleton
    {
        public static List<SportSingleton> sportSingleton = new List<SportSingleton>();
        public static void Init()
        {
            sportSingleton.Clear();
            sportSingleton = new List<SportSingleton>();
            for (int i = 1; i < 13; i++) 
            {
                List<int> result = new List<int>();
                for (int j = 0; j < 3; j++)
                    result.Add(0);
                sportSingleton.Add(new SportSingleton() { id = i, isFinished = false, results = result});;
            }
        }
    }
    public class SportSingleton
    {
        public int id { get; set; }
        public bool isFinished { get; set; } = false;

        public List<int> results = new List<int>();
    }
}
