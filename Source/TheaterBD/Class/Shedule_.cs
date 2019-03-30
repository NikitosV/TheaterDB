using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterBD.Class
{
    public class Shedule_
    {
        public Shedule_(int id, string name)
        {
            Id = id;
            SH = name;
        }

        public int Id { get; private set; }
        public string SH { get; private set; }
    }
}