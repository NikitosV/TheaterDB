using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterBD.Class
{
    public class Spectacle_
    {
        public Spectacle_(int id, string name)
        {
            Id = id;
            NNN = name;
        }

        public int Id { get; private set; }
        public string NNN { get; private set; }
    }
}
