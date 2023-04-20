using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet_db_videogame
{
    public class Videogame
    {
        public string Name { get; set; }
        public string Overview { get; set; }
        public int IdSoftwareHouse { get; set; }
        public DateTime ReleaseDate { get; set; }


        public Videogame(string name, string overview, int idSoftwareHouse, DateTime releaseDate)
        {
            Name = name;
            Overview = overview;
            IdSoftwareHouse = idSoftwareHouse;
            ReleaseDate = releaseDate;
        }
    }
}
