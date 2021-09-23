using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheAPI.Models
{
    public class ResultStarWarsPlanetsViewModel
    {
        public string From { get; set; }//daonde vem as informações
        public IList<Planets> Results { get; set; }
    }
    public class Planets
    {
        public string Name { get; set; }
        public string Climate { get; set; }
        public string Gravity { get; set; }
        public string Terrain { get; set; }
    }
}
